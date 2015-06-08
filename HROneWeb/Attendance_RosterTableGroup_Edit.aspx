<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Attendance_RosterTableGroup_Edit.aspx.cs" Inherits="Attendance_RosterTableGroup_Edit" MasterPageFile="~/MainMasterPage.master"  %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>



<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="RosterTableGroupID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="Label19" Text="Roster Table Group Setup" runat="server" />
                </td>
            </tr>
        </table>

        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <%=CurID<=0?HROne.Common.WebUtility.GetLocalizedString("Add"):HROne.Common.WebUtility.GetLocalizedString("Edit") %>
                    <asp:Label Text="Roster Table Group" runat="server" />:
                    <%=RosterTableGroupCode.Text %>
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
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
        <ContentTemplate >
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="15%" />
            <col width="35%" />
            <col width="15%" />
            <col width="35%" />
    		<tr>
                <td class="pm_field_title"  colspan="4">
                   <asp:Label runat="server" Text="Roster Table Group" /></td>
			</tr>
			<tr>
                <td class="pm_field_header" >
                    <asp:Label Text="Code" runat="server" />:</td>
                <td class="pm_field" >
                    <asp:TextBox ID="RosterTableGroupCode" runat="Server" /></td>
                <td class="pm_field_header" >
                    <asp:Label Text="Description" runat="server" />:</td>
                <td class="pm_field" >
                    <asp:TextBox ID="RosterTableGroupDesc" runat="Server" /></td>
            </tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label16" Text="Client Name" runat="server" />:</td>
                <td class="pm_field" >
                    <asp:DropDownList ID="RosterClientID" runat="Server" AutoPostBack="true" /></td>
                <td class="pm_field_header">
                    <asp:Label ID="Label13" Text="Site" runat="server" />:</td>
                <td class="pm_field" >
                    <asp:DropDownList ID="RosterClientSiteID" runat="Server"  /></td>
            </tr>
        </ContentTemplate>
        </asp:UpdatePanel >
</asp:Content> 