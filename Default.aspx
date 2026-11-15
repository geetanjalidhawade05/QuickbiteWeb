<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs"
    Inherits="QuickbiteWeb._Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>QuickBite - Home</title>

    <style>
        body {
            margin: 0;
            font-family: Arial, sans-serif;
            background-image: url('C:\Users\Admin\Desktop\QuickbiteWeb\Images\Warm Culinary Waves with Fresh Ingredients.png');
            background-size: cover;
            background-position: center;
            background-repeat: no-repeat;
            background-attachment: fixed;
            min-height: 100vh;
            color: #7c2d12;
            text-align: center;
        }

        .header {
            background: rgba(234, 88, 12, 0.94);
            color: white;
            padding: 25px;
        }

        .content {
            max-width: 650px;
            margin: 70px auto;
            padding: 35px 25px;
            background: rgba(255, 247, 237, 0.94);
            border-radius: 15px;
            box-shadow: 0 5px 20px rgba(0, 0, 0, 0.25);
        }

        .btn {
            display: inline-block;
            background-color: #ea580c;
            color: white;
            padding: 14px 25px;
            margin: 10px;
            border: none;
            border-radius: 6px;
            font-size: 16px;
            text-decoration: none;
        }

        .btn:hover {
            background-color: #c2410c;
        }

        .login-link {
            color: #9a3412;
            font-weight: bold;
            text-decoration: none;
        }

        .login-link:hover {
            text-decoration: underline;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">

        <div class="header">
            <h1>QuickBite</h1>
            <p>Delicious food, just a click away!</p>
        </div>

        <div class="content">
            <h2>Welcome to QuickBite</h2>
            <p>Explore our menu and order your favourite food.</p>

            <a href="Menu.aspx" class="btn">Food Menu</a>
            <a href="Cart.aspx" class="btn">View Cart</a>

            <br /><br />

            <a href="Login.aspx" class="login-link">
                Login / Register
            </a>
        </div>

    </form>
</body>
</html>

