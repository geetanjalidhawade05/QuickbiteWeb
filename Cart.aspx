<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Cart.aspx.cs"
    Inherits="QuickbiteWeb.Cart" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>QuickBite - My Cart</title>

    <style>
        body {
            font-family: Arial, sans-serif;
            background: #fff7ed;
            color: #78350F;
            margin: 0;
            padding: 30px;
        }

        .container {
            max-width: 950px;
            margin: 0 auto;
            background: white;
            padding: 30px;
            border-radius: 12px;
        }

        h1, h2 {
            text-align: center;
        }

        h1 {
            color: #78350F;
        }

        .subtitle {
            text-align: center;
            color: #EA580C;
            font-size: 20px;
            font-weight: bold;
        }

        .message {
            display: block;
            margin: 15px 0;
            font-weight: bold;
        }

        .total {
            font-size: 22px;
            font-weight: bold;
            margin-top: 20px;
        }

        .pickup {
            margin: 20px 0;
            line-height: 1.6;
        }

        .buttons {
            display: flex;
            justify-content: center;
            gap: 15px;
            flex-wrap: wrap;
            margin-top: 25px;
        }

        .action-button {
            background: #EA580C;
            color: white;
            font-weight: bold;
            font-size: 16px;
            border: none;
            border-radius: 6px;
            padding: 12px 22px;
            cursor: pointer;
        }

        .action-button:hover {
            background: #C2410C;
        }

        .cart-grid {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }

        .cart-grid th {
            background: #78350F;
            color: white;
            padding: 12px;
        }

        .cart-grid td {
            padding: 12px;
            border-bottom: 1px solid #fed7aa;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">

        <div class="container">

            <h1>QuickBite - My Cart</h1>

            <div class="subtitle">My Shopping Cart</div>

            <p>Your selected food items will appear here:</p>

            <asp:Label ID="lblMessage" runat="server"
                CssClass="message" />

            <asp:GridView ID="GridView2" runat="server"
                AutoGenerateColumns="False"
                CssClass="cart-grid"
                EmptyDataText="Your cart is empty. Please add food items from the menu.">

                <Columns>
                    <asp:BoundField DataField="FoodName"
                        HeaderText="Food Item" />

                    <asp:BoundField DataField="Price"
                        HeaderText="Price (₹)"
                        DataFormatString="{0:N2}" />

                    <asp:BoundField DataField="Quantity"
                        HeaderText="Quantity" />

                    <asp:TemplateField HeaderText="Subtotal (₹)">
                        <ItemTemplate>
                            <%# (Convert.ToDecimal(Eval("Price")) *
                                 Convert.ToInt32(Eval("Quantity"))).ToString("N2") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

            </asp:GridView>

            <div class="total">
                <asp:Label ID="Label3" runat="server"
                    Text="Total Amount: ₹0.00" />
            </div>

            <div class="pickup">
                <strong>Estimated Pickup Time:</strong>
                Your order will be ready approximately 20 minutes
                after placing your order.
            </div>

            <asp:Label ID="lblOrderResult" runat="server"
                CssClass="message" />

            <div class="buttons">

                <asp:Button ID="Button1" runat="server"
                    Text="Place Order"
                    CssClass="action-button"
                    OnClick="Button1_Click" />

                <asp:Button ID="Button2" runat="server"
                    Text="Back to Menu"
                    CssClass="action-button"
                    CausesValidation="false"
                    OnClick="Button2_Click" />

            </div>

        </div>

    </form>
</body>
</html>
