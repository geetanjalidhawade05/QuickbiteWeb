
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace QuickbiteWeb
{
    public partial class Cart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCart();
            }
        }

        private string GetConnectionString()
        {
            ConnectionStringSettings settings =
                ConfigurationManager.ConnectionStrings["QuickBiteConnection"];

            if (settings == null ||
                string.IsNullOrWhiteSpace(settings.ConnectionString))
            {
                throw new Exception(
                    "QuickBiteConnection is missing from Web.config.");
            }

            return settings.ConnectionString;
        }

        private void LoadCart()
        {
            DataTable cart = Session["Cart"] as DataTable;

            GridView2.DataSource = cart;
            GridView2.DataBind();

            decimal total = 0;

            if (cart != null)
            {
                foreach (DataRow row in cart.Rows)
                {
                    total += Convert.ToDecimal(row["Price"]) *
                             Convert.ToInt32(row["Quantity"]);
                }
            }

            Label3.Text = "Total Amount: ₹" + total.ToString("N2");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("Menu.aspx");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            DataTable cart = Session["Cart"] as DataTable;

            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (cart == null || cart.Rows.Count == 0)
            {
                lblOrderResult.Text =
                    "Your cart is empty. Please add food items first.";
                return;
            }

            int userId = Convert.ToInt32(Session["UserID"]);
            decimal total = 0;

            foreach (DataRow row in cart.Rows)
            {
                int quantity = Convert.ToInt32(row["Quantity"]);
                decimal price = Convert.ToDecimal(row["Price"]);

                if (quantity <= 0 || price < 0)
                {
                    lblOrderResult.Text =
                        "Invalid quantity or price in your cart.";
                    return;
                }

                total += price * quantity;
            }

            DateTime orderDate = DateTime.Now;
            DateTime pickupTime = orderDate.AddMinutes(20);

            using (SqlConnection con =
                   new SqlConnection(GetConnectionString()))
            {
                con.Open();

                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    try
                    {
                        int orderId;

                        // SQL Server automatically generates OrderID.
                        using (SqlCommand cmd = new SqlCommand(
                            @"INSERT INTO dbo.Orders
                              (UserID, OrderDate, PickupTime,
                               TotalAmount, OrderStatus)
                              VALUES
                              (@UserID, @OrderDate, @PickupTime,
                               @TotalAmount, @OrderStatus);

                              SELECT CAST(SCOPE_IDENTITY() AS INT);",
                            con, transaction))
                        {
                            cmd.Parameters.Add("@UserID", SqlDbType.Int)
                                .Value = userId;

                            cmd.Parameters.Add("@OrderDate", SqlDbType.DateTime)
                                .Value = orderDate;

                            cmd.Parameters.Add("@PickupTime", SqlDbType.DateTime)
                                .Value = pickupTime;

                            SqlParameter totalParameter =
                                cmd.Parameters.Add(
                                    "@TotalAmount", SqlDbType.Decimal);

                            totalParameter.Precision = 10;
                            totalParameter.Scale = 2;
                            totalParameter.Value = total;

                            cmd.Parameters.Add(
                                "@OrderStatus", SqlDbType.VarChar, 50)
                                .Value = "Placed";

                            orderId = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        // Save each item using the generated OrderID.
                        foreach (DataRow row in cart.Rows)
                        {
                            using (SqlCommand cmd = new SqlCommand(
                                @"INSERT INTO dbo.OrderDetails
                                  (OrderID, FoodID, Quantity, UnitPrice)
                                  VALUES
                                  (@OrderID, @FoodID, @Quantity, @UnitPrice)",
                                con, transaction))
                            {
                                cmd.Parameters.Add("@OrderID", SqlDbType.Int)
                                    .Value = orderId;

                                cmd.Parameters.Add("@FoodID", SqlDbType.Int)
                                    .Value = Convert.ToInt32(row["FoodID"]);

                                cmd.Parameters.Add("@Quantity", SqlDbType.Int)
                                    .Value = Convert.ToInt32(row["Quantity"]);

                                SqlParameter priceParameter =
                                    cmd.Parameters.Add(
                                        "@UnitPrice", SqlDbType.Decimal);

                                priceParameter.Precision = 10;
                                priceParameter.Scale = 2;
                                priceParameter.Value =
                                    Convert.ToDecimal(row["Price"]);

                                cmd.ExecuteNonQuery();
                            }
                        }

                        // Save everything together.
                        transaction.Commit();

                        Session["Cart"] = null;
                        LoadCart();

                        lblOrderResult.Text =
                            "Order placed successfully! Order ID: " +
                            orderId +
                            ". Total: ₹" + total.ToString("N2") +
                            ". Estimated pickup: " +
                            pickupTime.ToString("hh:mm tt") + ".";

                        Button1.Enabled = false;
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            transaction.Rollback();
                        }
                        catch (Exception rollbackException)
                        {
                            System.Diagnostics.Debug.WriteLine(
                                rollbackException.ToString());
                        }

                        System.Diagnostics.Debug.WriteLine(ex.ToString());

                        lblOrderResult.Text =
                            "Order failed: " +
                            Server.HtmlEncode(ex.Message);
                    }
                }
            }
        }
    }
}
