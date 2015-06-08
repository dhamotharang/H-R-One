<%@ page language="C#" masterpagefile="~/MainMasterPage.master" autoeventwireup="true" inherits="ProductUpgrade, HROneWeb.deploy" viewStateEncryptionMode="Always" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" Runat="Server">
        <table class="pm_section" width="100%"  id="UploadPanel" runat="server">
            <tr>
                <td class="pm_search_header">
                    <asp:Label ID="Label1" runat="server" Text="Upload file path" /></td>
                <td class="pm_search">
                    <asp:FileUpload ID="BinFile" runat="server" Width="400"/>
                    <asp:Button ID="Upload" runat="server" Text="Upload" OnClick="Upload_Click" />
                </td>
            </tr>
            <tr>
                <td class="pm_search_header">
                    <asp:Label ID="Label2" runat="server" Text="Server Username" /></td>
                <td class="pm_search">
                    <asp:TextBox ID="Username" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_search_header">
                    <asp:Label ID="Label3" runat="server" Text="Password" /></td>
                <td class="pm_search">
                    <asp:TextBox ID="Password" runat="server" TextMode="Password" />
                </td>
            </tr>
        </table>
        <table class="pm_section" width="100%" id="UpgradingPanel" runat="server" visible="false">
            <tr>
                <td class="pm_search_header" align="center" >
                    <asp:Label ID="lblUpgrading" runat="server" Text="Upgrading..."  /><br />
                    <asp:Image ID="imgLoading" runat="server" ImageUrl="~/images/wait.gif" />
                </td>
            </tr>
        </table>
        
</asp:Content>

