<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Payroll_GenerateMPFFile_BOCI.ascx.cs" Inherits="Payroll_GenerateMPFFile_BOCI" %>
<asp:HiddenField ID="CurMPFPlanID" runat="server" />
<table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
    <col width="15%" />
    <tr>
        <td class="pm_field_title" colspan="3">
            <asp:Label ID="Label1" Text="BOCI MPF File Detail" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
            <asp:Label ID="Label2" Text="Payment Method" runat="server" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:DropDownList ID="PaymentMethod" runat="server" AutoPostBack="true"  OnSelectedIndexChanged="PaymentMethod_SelectedIndexChanged">
                <asp:ListItem Text="Autopay" Value="A" />
                <asp:ListItem Text="Cheque" Value="Q" />
                <asp:ListItem Text="Cash" Value="C" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr runat="server" visible="false" >
        <td class="pm_field_header" >
            <asp:Label ID="Label4" Text="Sequence No." runat="server" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:Textbox ID="SequenceNum" runat="server" />
        </td>
    </tr>
    <tr runat="server" id="ChequePanel" visible="false" >
        <td class="pm_field_header" >
            <asp:Label ID="Label3" Text="Cheque No." runat="server" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:Textbox ID="ChequeNum" runat="server" />
        </td>
    </tr>
 </table>
