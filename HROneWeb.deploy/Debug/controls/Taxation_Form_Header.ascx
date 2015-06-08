<%@ control language="C#" autoeventwireup="true" inherits="Taxation_Form_Header, HROneWeb.deploy" %>


<input type="hidden" id="TaxFormID" runat="server" name="ID" />
<table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
    <col width="15%" />
    <col width="35%" />
    <col width="15%" />
    <col width="35%" />    <tr>
        <td class="pm_field_title" colspan="4">
            <asp:Label Text="Taxation Header" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
            <asp:Label Text="Name of Employer" runat="server" />:
        </td>
        <td class="pm_field" colspan="3">
            <asp:Label ID="TaxFormEmployerName" runat="Server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header">
            <asp:Label Text="Year Return" runat="server"/>:
        </td>
        <td class="pm_field">
            <asp:Label ID="TaxFormYear" runat="Server" />
        </td>
        <td class="pm_field_header" >
            <asp:Label Text="Form" runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="TaxFormType" runat="Server" />
        </td>
    </tr>
</table>
