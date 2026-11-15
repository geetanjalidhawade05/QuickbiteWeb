using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace QuickbiteWeb
{
    public partial class Login : System.Web.UI.Page
    {
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

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string name = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                lblMessage.Text = "Please fill in all fields.";
                return;
            }

            try
            {
                string cs = GetConnectionString();

                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    string check =
                        "SELECT COUNT(*) FROM Users WHERE Email = @Email";

                    using (SqlCommand cmd = new SqlCommand(check, con))
                    {
                        cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 255)
                            .Value = email;

                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (count > 0)
                        {
                            lblMessage.Text =
                                "This email is already registered. Please log in.";
                            return;
                        }
                    }

                    string query = @"
                        INSERT INTO Users
                            (FullName, Email, Password, Role)
                        VALUES
                            (@Name, @Email, @Password, 'Student')";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 100)
                            .Value = name;

                        cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 255)
                            .Value = email;

                        cmd.Parameters.Add("@Password", SqlDbType.VarChar, 255)
                            .Value = password;

                        cmd.ExecuteNonQuery();
                    }
                }

                lblMessage.Text =
                    "Registration successful! You can now log in.";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                lblMessage.Text = "Registration failed: " + ex.Message;
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                lblMessage.Text = "Enter your email and password.";
                return;
            }

            try
            {
                string cs = GetConnectionString();

                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = @"
                        SELECT UserID, FullName, Role
                        FROM Users
                        WHERE Email = @Email AND Password = @Password";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 255)
                            .Value = email;

                        cmd.Parameters.Add("@Password", SqlDbType.VarChar, 255)
                            .Value = password;

                        con.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Session["UserID"] = reader["UserID"];
                                Session["FullName"] =
                                    reader["FullName"].ToString();
                                Session["Role"] =
                                    reader["Role"].ToString();

                                string role = reader["Role"].ToString();

                                if (role.Equals(
                                    "Admin",
                                    StringComparison.OrdinalIgnoreCase))
                                {
                                    Response.Redirect("Admin.aspx");
                                }
                                else
                                {
                                    Response.Redirect("Menu.aspx");
                                }

                                return;
                            }

                            lblMessage.Text =
                                "Invalid email or password.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                lblMessage.Text = "Login failed: " + ex.Message;
            }
        }
    }
}