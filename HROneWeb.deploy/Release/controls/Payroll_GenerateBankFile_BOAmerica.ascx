<%@ control language="C#" autoeventwireup="true" inherits="Payroll_GenerateBankFile_BOAmerica, HROneWeb.deploy" %>
<table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
    <col width="15%" />
    <tr>
        <td class="pm_field_title" colspan="3">
            <asp:Label Text="Bank of America File Detail" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
           <asp:Label ID="lblCompanyID" Text="Company ID" runat="server" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:TextBox ID="CompanyID" runat="server" Columns="10" MaxLength="10" CssClass="pm_required" />
        </td>
    </tr> 
    <tr>
        <td class="pm_field_header" >
           <asp:Label ID="lblEFTKey" Text="Company EFT Key" runat="server" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:TextBox ID="EFTKey" runat="server" Columns="4" MaxLength="4" CssClass="pm_required" />
        </td>
    </tr> 
    <tr>
        <td class="pm_field_header" style="vertical-align:top;" >
           <asp:Label ID="lblOrderingPartyAddress" Text="Ordering Party Address" runat="server" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:TextBox ID="OrderingPartyAddress1" runat="server" Columns="35" MaxLength="35" CssClass="pm_required" /><br />
            <asp:TextBox ID="OrderingPartyAddress2" runat="server" Columns="35" MaxLength="35" /><br />
            <asp:TextBox ID="OrderingPartyCityName" runat="server" Columns="30" MaxLength="30" /><br />
        </td>
    </tr> 
    <tr>
        <td class="pm_field_header" >
            <asp:Label Text="Indicator" runat="server" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:DropDownList ID="Indicator" runat="server">
                <asp:ListItem Text="Production" Value="PROD" Selected="True" />
                <asp:ListItem Text="Testing" Value="TEST" />
            </asp:DropDownList>
        </td>
    </tr>  
 </table>
