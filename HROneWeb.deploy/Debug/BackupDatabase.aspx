<%@ page language="C#" masterpagefile="~/MainMasterPage.master" autoeventwireup="true" inherits="BackupDatabase, HROneWeb.deploy" viewStateEncryptionMode="Always" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" Runat="Server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="Label1" Text="Backup Database" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="Label4" runat="server" Text="Criteria" />
                </td>
            </tr>
        </table>
    <table width="100%" class="pm_section">
        <tr>
            <td class="pm_search_header" valign="top" >
                <asp:Label ID="Label2" runat="server" Text="ZIP with password" />:
            </td>
            <td class="pm_search">
                <asp:TextBox ID="ZipPassword" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="pm_search_header" valign="top" >
            </td>
            <td class="pm_search">
                <asp:Button ID="btnBackup" runat="server" Text="Backup" CssClass="button" OnClick="btnBackup_Click" />
            </td>
        </tr>
    </table> 
</asp:Content>

