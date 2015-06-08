<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Attendance_RosterClientSite_Edit.aspx.cs" Inherits="Attendance_RosterClientSite_Edit" MasterPageFile="~/MainMasterPage.master"  %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>



<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="RosterClientID" runat="server" />
        <input type="hidden" id="RosterClientSiteID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="Label1" Text="Roster Client Site Setup" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <%=CurID<=0?HROne.Common.WebUtility.GetLocalizedString("Add"):HROne.Common.WebUtility.GetLocalizedString("Edit") %>
                    <asp:Label Text="Roster Client Site" runat="server" />:
                    <%=RosterClientSiteCode.Text %>
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
            <col width="15%" />
            <col width="35%" />
            <col width="15%" />
            <col width="35%" />
			<tr>
                <td class="pm_field_header" >
                    <asp:Label Text="Code" runat="server" />:</td>
                <td class="pm_field" >
                    <asp:TextBox ID="RosterClientSiteCode" runat="Server"  /></td>
                <td class="pm_field_header" ">
                    <asp:Label Text="Property Name" runat="server" />:</td>
                <td class="pm_field" >
                    <asp:TextBox ID="RosterClientSitePropertyName" runat="Server" /></td>
            </tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label5" Text="Nature of Premises" runat="server" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:TextBox ID="RosterClientSitePremisesNature" runat="Server" /></td>
            </tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label2" Text="Location" runat="server" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:TextBox ID="RosterClientSiteLocation" runat="Server" /></td>
            </tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label3" Text="In Charge" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="RosterClientSiteInCharge" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label ID="Label4" Text="Contact No" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="RosterClientSiteInChargeContactNo" runat="Server" /></td>
            </tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label7" Text="Service Hours" runat="server" />:</td>
                <td class="pm_field" >
                    <asp:TextBox ID="RosterClientSiteServiceHours" runat="Server"/></td>
                <td class="pm_field_header" >
                    <asp:Label ID="Label9" Text="Shift" runat="server" />:</td>
                <td class="pm_field" >
                    <asp:TextBox ID="RosterClientSiteShift" runat="Server" /></td>
            </tr>
			<tr id="CostCenterRow" runat="server" >
                <td class="pm_field_header">
                    <asp:Label ID="Label47" Text="Cost Center" runat="server" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="CostCenterID" runat="Server"/>
                </td>
            </tr>
		</table>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
        </asp:UpdatePanel >
</asp:Content> 