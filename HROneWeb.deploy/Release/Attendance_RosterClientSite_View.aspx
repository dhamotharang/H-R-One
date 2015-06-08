<%@ page language="C#" autoeventwireup="true" inherits="Attendance_RosterClientSite_View, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>



<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="RosterClientID" runat="server" />
        <input type="hidden" id="RosterClientSiteID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label Text="Roster Client Site Setup" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
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
        <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
            <col width="15%" />
            <col width="35%" />
            <col width="15%" />
            <col width="35%" />
			<tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label1" Text="Code" runat="server" />:</td>
                <td class="pm_field" >
                    <asp:Label ID="RosterClientSiteCode" runat="Server"/></td>
                <td class="pm_field_header" >
                    <asp:Label ID="Label2" Text="Property Name" runat="server" />:</td>
                <td class="pm_field" >
                    <asp:Label ID="RosterClientSitePropertyName" runat="Server" /></td>
            </tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label6" Text="Nature of Premises" runat="server" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="RosterClientSitePremisesNature" runat="Server"  /></td>
            </tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label3" Text="Location" runat="server" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="RosterClientSiteLocation" runat="Server" /></td>
            </tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label4" Text="In Charge" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="RosterClientSiteInCharge" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label ID="Label5" Text="Contact No" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="RosterClientSiteInChargeContactNo" runat="Server" /></td>
            </tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label7" Text="Service Hours" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="RosterClientSiteServiceHours" runat="Server"/></td>
                <td class="pm_field_header">
                    <asp:Label ID="Label9" Text="Shift" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="RosterClientSiteShift" runat="Server" /></td>
            </tr>
			<tr id="CostCenterRow" runat="server" >
                <td class="pm_field_header">
                    <asp:Label ID="Label47" Text="Cost Center" runat="server" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="CostCenterID" runat="Server"/>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
        </asp:UpdatePanel >

</asp:Content> 