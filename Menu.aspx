<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Menu.aspx.cs"
    Inherits="QuickbiteWeb.Menu" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>QuickBite - Food Menu</title>

    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 0;
            background: #fff8ef;
            color: #333;
        }

        .header {
            background: #8b2f20;
            color: white;
            text-align: center;
            padding: 22px;
        }

        .header h1 {
            margin: 0;
            font-size: 36px;
        }

        .welcome {
            text-align: center;
            padding: 18px;
        }

        .message {
            font-weight: bold;
        }

        .category {
            margin: 25px;
        }

        .category h2 {
            color: #8b2f20;
            border-bottom: 2px solid #e7b26d;
            padding-bottom: 8px;
        }

        .food-list {
            display: flex;
            flex-wrap: wrap;
            gap: 18px;
        }

        .food-card {
            background: white;
            border: 1px solid #ead8c4;
            border-radius: 10px;
            padding: 18px;
            width: 220px;
            box-shadow: 0 2px 7px #00000012;
        }

        .food-card h3 {
            color: #8b2f20;
            margin-top: 0;
        }

        .price {
            font-size: 18px;
            font-weight: bold;
        }

        .add-button {
            background: #8b2f20;
            color: white;
            padding: 10px 15px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
        }

        .add-button:hover {
            background: #642015;
        }

        .links {
            text-align: center;
            padding: 25px;
        }

        .links a {
            margin: 10px;
            color: #8b2f20;
            font-weight: bold;
            text-decoration: none;
        }

        @media (max-width: 600px) {
            .category {
                margin: 15px;
            }

            .food-card {
                width: auto;
                flex: 1 1 200px;
            }
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">

        <div class="header">
            <h1>QuickBite</h1>
            <h2>Delicious Food, Delivered Fresh!</h2>
        </div>

        <div class="welcome">
            <h3>
                <asp:Label ID="lblWelcome" runat="server"
                    Text="Welcome to QuickBite!" />
            </h3>

            <asp:Label ID="lblMessage" runat="server"
                CssClass="message" />
        </div>

        <asp:Repeater ID="rptCategories" runat="server"
            OnItemDataBound="rptCategories_ItemDataBound">

            <ItemTemplate>

                <div class="category">

                    <h2>
                        <%# Eval("CategoryName") %>
                    </h2>

                    <div class="food-list">

                        <asp:Repeater ID="rptFood" runat="server"
                            OnItemCommand="rptFood_ItemCommand">

                            <ItemTemplate>

                                <div class="food-card">

                                    <h3>
                                        <%# Eval("FoodName") %>
                                    </h3>

                                    <p>
                                        <%# Eval("Description") %>
                                    </p>

                                    <p class="price">
                                        Price: ₹<%# Eval("Price", "{0:0.00}") %>
                                    </p>

                                    <asp:Button
                                        ID="btnAddToCart"
                                        runat="server"
                                        Text="Add to Cart"
                                        CssClass="add-button"
                                        CommandName="Add"
                                        CommandArgument='<%# Eval("FoodID") %>' />

                                </div>

                            </ItemTemplate>

                        </asp:Repeater>

                    </div>
                </div>

            </ItemTemplate>

        </asp:Repeater>

        <div class="links">

            <asp:HyperLink ID="lnkCart" runat="server"
                NavigateUrl="Cart.aspx"
                Text="View Cart" />

            |

            <asp:HyperLink ID="lnkHome" runat="server"
                NavigateUrl="Default.aspx"
                Text="Back to Home" />

        </div>

    </form>
</body>
</html>
