<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Attendance_RosterTableGroup_View.aspx.cs" Inherits="Attendance_RosterTableGroup_View" MasterPageFile="~/MainMasterPage.master"  %>

<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="RosterTableGroupID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label Text="Roster Table Group Setup" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label Text="View Roster Table Group" runat="server" />:
                    <%=RosterTableGroupCode.Text %>
                </td>
            </tr>
        </table>
        
            
                
        
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar id="toolBar" runat="server"
                     NewButton_Visible="false" 
                     SaveButton_Visible="false" 
                     OnBackButton_Click="Back_Click"
                     OnEditButton_Click ="Edit_Click"
                     OnDeleteButton_Click="Delete_ClickTop"
                      />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
                </td>
            </tr>
        </table>
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="15%" />
            <col width="35%" />
            <col width="15%" />
            <col width="35%" />
    		<tr>
                <td class="pm_field_title"  colspan="4">
                   <asp:Label ID="Label1" runat="server" Text="Roster Table Group" /></td>
			</tr>
			<tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label2" Text="Code" runat="server" />:</td>
                <td class="pm_field" >
                    <asp:Label ID="RosterTableGroupCode" runat="Server" /></td>
                <td class="pm_field_header" >
                    <asp:Label ID="Label3" Text="Description" runat="server" />:</td>
                <td class="pm_field" >
                    <asp:Label ID="RosterTableGroupDesc" runat="Server" /></td>
            </tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label33" Text="Client Name" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="RosterClientID" runat="Server"  /></td>
                <td class="pm_field_header">
                    <asp:Label ID="Label34" Text="Site" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="RosterClientSiteID" runat="Server"  /></td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolBar" />
        </Triggers>
        </asp:UpdatePanel> 
</asp:Content> 