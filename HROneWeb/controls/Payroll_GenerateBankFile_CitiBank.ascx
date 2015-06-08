<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Payroll_GenerateBankFile_CitiBank.ascx.cs" Inherits="Payroll_GenerateBankFile_CitiBank" %>
<table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
    <col width="15%" />
    <tr>
        <td class="pm_field_title" colspan="3">
            <asp:Label Text="CitiBank File Detail" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
            <asp:Label Text="Product Code" runat="server" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:DropDownList ID="ProductCode" runat="server">
                <asp:ListItem Text="PayLink AUTOPAY Payroll" Value="HKP" />
                <asp:ListItem Text="PayLink AUTOPAY" Value="ATP" />
            </asp:DropDownList>

        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
            <asp:Label ID="lblFileFormat" Text="File Format" runat="server" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:DropDownList ID="FileFormat" runat="server">
                <asp:ListItem Text="Excel File" Value="Excel" />
                <asp:ListItem Text="Plain Text File" Value="Text" />
            </asp:DropDownList>

        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
            <asp:Label ID="lblPaymentDetails" Text="Payment Details" runat="server" />:
        </td>
        <td class="pm_field" colspan="2">
        <asp:TextBox ID="txtPaymentDetails" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
        </td>
        <td class="pm_field" colspan="2">
        <asp:CheckBox ID="PaylinkCheque" Text="Use Paylink Cheque for cheque payment" runat="server" />
        </td>
    </tr>
 </table>
