using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuickbiteWeb
{
    public partial class Menu : System.Web.UI.Page
    {
        private string ConnectionString
        {
            get
            {
                return ConfigurationManager
                    .ConnectionStrings["QuickBiteConnection"]
                    .ConnectionString;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["FullName"] != null)
                {
                    lblWelcome.Text = "Welcome, " +
                        Session["FullName"].ToString();
                }

                LoadCategories();
            }
        }

        private void LoadCategories()
        {
            try
            {
                string query = @"
                    SELECT CategoryID, CategoryName
                    FROM dbo.Categories
                    ORDER BY CategoryName";

                DataTable categories = new DataTable();

                using (SqlConnection con =
                    new SqlConnection(ConnectionString))
                using (SqlDataAdapter adapter =
                    new SqlDataAdapter(query, con))
                {
                    adapter.Fill(categories);
                }

                rptCategories.DataSource = categories;
                rptCategories.DataBind();

                lblMessage.ForeColor =
                    System.Drawing.Color.Red;

                lblMessage.Text = categories.Rows.Count == 0
                    ? "No categories found in the database."
                    : "";
            }
            catch (Exception ex)
            {
                lblMessage.Text =
                    "Error loading categories: " + ex.Message;
            }
        }

        protected void rptCategories_ItemDataBound(
            object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item &&
                e.Item.ItemType != ListItemType.AlternatingItem)
            {
                return;
            }

            int categoryID = Convert.ToInt32(
                DataBinder.Eval(e.Item.DataItem, "CategoryID"));

            Repeater foodRepeater =
                (Repeater)e.Item.FindControl("rptFood");

            string query = @"
                SELECT FoodID, FoodName, Price, Description
                FROM dbo.FoodItems
                WHERE CategoryID = @CategoryID
                  AND IsAvailable = 1
                ORDER BY FoodName";

            using (SqlConnection con =
                new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            using (SqlDataAdapter adapter =
                new SqlDataAdapter(cmd))
            {
                cmd.Parameters.Add("@CategoryID", SqlDbType.Int)
                    .Value = categoryID;

                DataTable foods = new DataTable();
                adapter.Fill(foods);

                foodRepeater.DataSource = foods;
                foodRepeater.DataBind();
            }
        }

        protected void rptFood_ItemCommand(
            object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "Add")
                return;

            try
            {
                int foodID = Convert.ToInt32(e.CommandArgument);

                DataTable cart = Session["Cart"] as DataTable;

                if (cart == null)
                {
                    cart = new DataTable();
                    cart.Columns.Add("FoodID", typeof(int));
                    cart.Columns.Add("FoodName", typeof(string));
                    cart.Columns.Add("Price", typeof(decimal));
                    cart.Columns.Add("Quantity", typeof(int));
                }

                string query = @"
                    SELECT FoodID, FoodName, Price
                    FROM dbo.FoodItems
                    WHERE FoodID = @FoodID
                      AND IsAvailable = 1";

                using (SqlConnection con =
                    new SqlConnection(ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@FoodID", SqlDbType.Int)
                        .Value = foodID;

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            lblMessage.ForeColor =
                                System.Drawing.Color.Red;
                            lblMessage.Text =
                                "Food item not found or unavailable.";
                            return;
                        }

                        DataRow[] existing =
                            cart.Select("FoodID = " + foodID);

                        if (existing.Length > 0)
                        {
                            existing[0]["Quantity"] =
                                Convert.ToInt32(
                                    existing[0]["Quantity"]) + 1;
                        }
                        else
                        {
                            DataRow row = cart.NewRow();
                            row["FoodID"] = foodID;
                            row["FoodName"] =
                                reader["FoodName"].ToString();
                            row["Price"] =
                                Convert.ToDecimal(reader["Price"]);
                            row["Quantity"] = 1;
                            cart.Rows.Add(row);
                        }
                    }
                }

                Session["Cart"] = cart;

                lblMessage.ForeColor =
                    System.Drawing.Color.Green;
                lblMessage.Text = "Item added to your cart!";
            }
            catch (Exception ex)
            {
                lblMessage.ForeColor =
                    System.Drawing.Color.Red;
                lblMessage.Text = "Cart error: " + ex.Message;
            }
        }

        protected void rptCategories_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }
    }
}
