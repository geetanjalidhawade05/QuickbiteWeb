
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace QuickbiteWeb
{
    public partial class TestConnection : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings["QuickBiteConnection"].ConnectionString;

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    Response.Write("Success! QuickBite database connected.");
                }
            }
            catch (Exception ex)
            {
                Response.Write("Connection failed: " + ex.Message);
            }
        }
    }
}
