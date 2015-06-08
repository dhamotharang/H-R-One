<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Payroll_GenerateBankFile_ANZ.ascx.cs" Inherits="Payroll_GenerateBankFile_ANZ" %>
<table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="15%" />
    <tr>
        <td class="pm_field_title" colspan="3">
            <asp:Label Text="ANZ National Bank File Detail" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
            <asp:Label Text="My Product Code" runat="server" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:DropDownList ID="MyProductCode" runat="server">
                <asp:ListItem Text="ACH" Value="ECPAY" Enabled="false"  />
                <asp:ListItem Text="PAYROLL" Value="PAYROLL" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
            <asp:Label ID="Label1" Text="Payment Product" runat="server" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:DropDownList ID="PaymentProduct" runat="server">
                <asp:ListItem Text="PAYROLL OUTWARD SACR BULK - HKD" Value="HKPAYRBHKD" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
           <asp:Label ID="lblClientCodeHeader" Text="Client Code" runat="server" />
        </td>
        <td class="pm_field" colspan="2">
            <asp:TextBox ID="ClientCode" runat="server" MaxLength="10" CssClass="pm_required" />
        </td>
    </tr>
 </table>
