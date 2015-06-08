<%@ page language="C#" autoeventwireup="true" inherits="UserChangePassword, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>



<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
		 <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label Text="Change Password" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label Text="Change Password" runat="server" />
                </td>
            </tr>
        </table>
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
                </td>
            </tr>
        </table>
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="30%" />
            <col width="70%" />
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Old Password" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="OldPassword" runat="Server" Columns="20" TextMode="password" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="New Password" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="Password" runat="Server" Columns="20" TextMode="password" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Confirm New Password" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="Password2" runat="Server" Columns="20" TextMode="password" />
                </td>
            </tr>
        </table>
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td align="right">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
                    <ContentTemplate >
                    <asp:Button ID="Save" runat="server" Text="Save" CssClass="button" OnClick="Save_Click" />
                    </ContentTemplate>
                    </asp:UpdatePanel> 
                </td>
            </tr>
        </table>
</asp:Content> 