<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Leave_ALProrataRoundingRule_Edit.aspx.cs" Inherits="Leave_ALProrataRoundingRule_Edit" MasterPageFile="~/MainMasterPage.master"  %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>



<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="ALProrataRoundingRuleID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="Label1" Text="Annual Leave Prorata Rounding Rule Setup" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <%=CurID<=0?HROne.Common.WebUtility.GetLocalizedString("Add"):HROne.Common.WebUtility.GetLocalizedString("Edit") %>
                    <asp:Label Text="Annual Leave Prorata Rounding Rule " runat="server" />:
                    <%=ALProrataRoundingRuleCode.Text%>
                </td>
            </tr>
        </table>
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar id="toolBar" runat="server"
                     NewButton_Visible="false" 
                     EditButton_Visible="false" 
                     OnBackButton_Click="Back_Click"
                     OnSaveButton_Click ="Save_Click"
                     OnDeleteButton_Click="Delete_Click"
                      />
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
                <td class="pm_field_header" >
                    <asp:Label Text="Code" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="ALProrataRoundingRuleCode" runat="Server" /></td>
			</tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label Text="Description" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="ALProrataRoundingRuleDesc" runat="Server" /></td>
            </tr>
		</table>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
        </asp:UpdatePanel >
</asp:Content> 