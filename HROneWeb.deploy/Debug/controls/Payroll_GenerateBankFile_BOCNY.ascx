<%@ control language="C#" autoeventwireup="true" inherits="Payroll_GenerateBankFile_BOCNY, HROneWeb.deploy" %>
<table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
    <col width="15%" />
    <tr>
        <td class="pm_field_title" colspan="3">
            <asp:Label Text="Bank of China (Nan Yang) Payroll/Autopay File Detail" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
            <asp:Label Text="File Format" runat="server" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:DropDownList ID="cboFileFormat" runat="server">
                <asp:ListItem Text="Payroll File" Value="Payroll" />
                <asp:ListItem Text="Autopay" Value="Autopay" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr runat="server" visible="false" >
        <td class="pm_field_header" >
           <asp:Label ID="lblAutopayMethod" Text="Method" runat="server" />
        </td>
        <td class="pm_field" colspan="2">
            <asp:DropDownList ID="cboAutopayMethod" runat="server">
                <asp:ListItem Text="Pay Out" Value="CR" Selected="True" />
                <asp:ListItem Text="Pay In" Value="DR" />
            </asp:DropDownList>
        </td>
    </tr>
 </table>
