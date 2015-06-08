<%@ control language="C#" autoeventwireup="true" inherits="Payroll_GenerateMPFFile_HSBCOIS, HROneWeb.deploy" %>
<%--<asp:HiddenField ID="hfBankCode" runat="server" />--%>
<%--<table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
    <col width="15%" />
    <tr>
        <td class="pm_field_title" colspan="3">
            <asp:Label ID="Label1" Text="HSBC/Hang Seng Bank MPF File Detail" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
            <asp:Label ID="Label3" Text="File Format" runat="server" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:DropDownList ID="FileType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="FileType_SelectedIndexChanged">
                <asp:ListItem Text="e-channel Format" Value="AMPFF" />
                <asp:ListItem Text="CD-ROM" Value="D" Enabled="true"/>
                <asp:ListItem Text="Agreed Format" Value="AMCND" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr id="PaymentMethodRow" runat="server" >
        <td class="pm_field_header" >
            <asp:Label ID="Label2" Text="Payment Method" runat="server" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:DropDownList ID="PaymentMethod" runat="server">
                <asp:ListItem Text="Direct Debit" Value="D" />
                <asp:ListItem Text="Direct Credit to Bank Draft" Value="B" />
                <asp:ListItem Text="Cheque" Value="C" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr id="RemoteProfileID" runat="server" visible="false"  >
        <td class="pm_field_header" >
           <asp:Label ID="Label5" runat="server" Text="Remote Profile ID" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:TextBox ID="txtRemoteProfileID" runat="server" CssClass="pm_required" />
        </td>
    </tr>
 </table>
--%>