
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="QuickbiteWeb.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>QuickBite - Login</title>
    <style>
        body {
            font-family: Arial;
            background: #fff7ed;
            color: #78350F;
            text-align: center;
        }
        .panel {
            width: 350px;
            margin: 60px auto;
            padding: 25px;
            background: white;
            border-radius: 12px;
        }
        .field {
            width: 90%;
            padding: 10px;
            margin: 8px 0 15px;
            box-sizing: border-box;
        }
        .btn {
            background: #EA580C;
            color: white;
            border: none;
            padding: 12px 18px;
            margin: 6px;
            cursor: pointer;
        }
        h1 { color: #9A3412; }
    </style>
</head>
<body>
<form id="form1" runat="server">
    <div class="panel">
        <h1>QuickBite</h1>
        <h2>Login / Register</h2>

        <asp:Label ID="lblName" runat="server" Text="Full Name" />
        <asp:TextBox ID="txtFullName" runat="server"
            CssClass="field" placeholder="Enter your full name" />

        <br />

        <asp:Label ID="lblEmail" runat="server" Text="Email" />
        <asp:TextBox ID="txtEmail" runat="server"
            CssClass="field" TextMode="Email" placeholder="Enter your email" />

        <br />

        <asp:Label ID="lblPassword" runat="server" Text="Password" />
        <asp:TextBox ID="txtPassword" runat="server"
            CssClass="field" TextMode="Password" placeholder="Enter your password" />

        <br />

        <asp:Button ID="btnRegister" runat="server"
            Text="Register" CssClass="btn"
            OnClick="btnRegister_Click" />

        <asp:Button ID="btnLogin" runat="server"
            Text="Login" CssClass="btn"
            OnClick="btnLogin_Click" />

        <br /><br />

        <asp:Label ID="lblMessage" runat="server" />

        <br /><br />
        <a href="Default.aspx">Back to Home</a>
    </div>
</form>
</body>
</html>
