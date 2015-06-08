<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LeaveType_Edit.aspx.cs" Inherits="LeaveType_Edit" MasterPageFile="~/MainMasterPage.master"  %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>



<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">

        <input type="hidden" id="LeaveTypeID" runat="server" name="ID" />
		<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Leave Type Setup" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <%=CurID<=0?HROne.Common.WebUtility.GetLocalizedString("Add"):HROne.Common.WebUtility.GetLocalizedString("Edit") %>
                    <asp:Label runat="server" Text="Leave Type" />:
                    <%=LeaveType.Text %>
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
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_field_section">
            <col width="20%" />
            <col width="30%" />
            <col width="20%" />
            <col width="30%" />
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Code" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="LeaveType" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Description" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="LeaveTypeDesc" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Decimal Place(s) for Leave Balance Display" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="LeaveDecimalPlace" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label ID="Label1" runat="server" Text="Use Work Hour Pattern" />?</td>
                <td class="pm_field">
                    <asp:CheckBox ID="LeaveTypeIsUseWorkHourPattern" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label2" runat="server" Text="Skip Statutory Holiday Checking" />?</td>
                <td class="pm_field">
                    <asp:CheckBox ID="LeaveTypeIsSkipStatutoryHolidayChecking" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label ID="Label3" runat="server" Text="Skip Public Holiday Checking" />?</td>
                <td class="pm_field">
                    <asp:CheckBox ID="LeaveTypeIsSkipPublicHolidayChecking" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label6" runat="server" Text="Disabled" />?
                </td>
                <td class="pm_field" colspan="3" >
                    <asp:CheckBox ID="LeaveTypeIsDisabled" runat="Server" />
                </td>
            </tr>
        </table>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_field_section" runat="server" id="ESSRow" >
            <col width="20%" />
            <col width="30%" />
            <col width="20%" />
            <col width="30%" />
            <tr>
                <td class="pm_field_title" colspan="4">
                    <asp:Label runat="server" EnableViewState="false" Text="ESS Options" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label4" runat="server" EnableViewState="false" Text="Hide Leave Balance in ESS" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:CheckBox ID="LeaveTypeIsESSHideLeaveBalance" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                </td>
                <td class="pm_field" colspan="3" >
                    <asp:Label ID="Label7" runat="server" EnableViewState="false" Text="Current entitlement is not available for leave applications" />:
                    <asp:CheckBox ID="LeaveTypeIsESSIgnoreEntitlement" runat="server" AutoPostBack="true" EnableViewState="false" />
                    <br />
                    <br />
                    <asp:Label ID="Label5" runat="server" EnableViewState="false" Text="Do not allow to apply leave application if the employee does not have enough leave balance as at" />:
                    <br /><asp:CheckBox ID="LeaveTypeIsESSRestrictNegativeBalanceAsOfToday" runat="server" EnableViewState="false" AutoPostBack="true" Text="date of submit" />
                    <br /><asp:CheckBox ID="LeaveTypeIsESSRestrictNegativeBalanceAsOfApplicationDateFrom" runat="server" EnableViewState="false" AutoPostBack="true" Text="first date of leave application" />
                    <br /><asp:CheckBox ID="LeaveTypeIsESSRestrictNegativeBalanceAsOfApplicationDateTo" runat="server" EnableViewState="false" AutoPostBack="true" Text="last date of leave application" />
                    <br /><asp:CheckBox ID="LeaveTypeIsESSRestrictNegativeBalanceAsOfEndOfLeaveYear" runat="server" EnableViewState="false" AutoPostBack="true" Text="last date of leave year" OnCheckedChanged="LeaveTypeIsESSRestrictNegativeBalanceAsOfEndOfLeaveYear_Changed" />
                    <br />
                    <br />
                    <br /><asp:Label >Allowable Advance Balance</asp:Label>:
                          <asp:TextBox ID="LeaveTypeIsESSAllowableAdvanceBalance" runat="server" EnableViewState="true" />
                          <asp:Label ID="LeaveUnit" runat="server" />
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
        </asp:UpdatePanel >
</asp:Content> 