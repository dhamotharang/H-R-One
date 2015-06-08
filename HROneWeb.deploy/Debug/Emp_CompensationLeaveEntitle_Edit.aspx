<%@ page language="C#" autoeventwireup="true" inherits="Emp_CompensationLeaveEntitle, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/Emp_Common.ascx" TagName="Emp_Common" TagPrefix="uc1" %>
<%@ Register Src="~/controls/Emp_LeaveBalance_List.ascx" TagName="Emp_LeaveBalance_List"
    TagPrefix="uc5" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="CompensationLeaveEntitleID" runat="server" />
        <input type="hidden" id="EmpID" runat="server" />
		<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label Text="Employee Information" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="ActionHeader" runat="server" Text="Edit" />
                    <asp:Label Text="Compensation Leave Entitlement" runat="server" />
                </td>
            </tr>
        </table>
        
            
                
        
        <uc1:Emp_Common id="Emp_Common1" runat="server"/><br /> 
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
            <asp:AsyncPostBackTrigger ControlID="toolBar" />
        </Triggers>
        <ContentTemplate >
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="15%" />
            <col width="35%" />
            <col width="15%" />
            <col width="35%" />
            <tr>
                <td class="pm_field_header" >
                    <asp:Label Text="Effective Date" runat="server" />:</td>
                <td class="pm_field" >
                    <uc1:WebDatePicker id="CompensationLeaveEntitleEffectiveDate" runat="server" />
                </td>
                <td class="pm_field_header" >
                    <asp:Label ID="Label2" Text="Date Expiry" runat="server" />:</td>
                <td class="pm_field" >
                    <uc1:WebDatePicker id="CompensationLeaveEntitleDateExpiry" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Claim Period" runat="server" />:
                </td>
                <td class="pm_field" colspan="3">
                    <uc1:WebDatePicker id="CompensationLeaveEntitleClaimPeriodFrom" runat="server" /> -
                    <uc1:WebDatePicker id="CompensationLeaveEntitleClaimPeriodTo" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Claim Time" runat="server" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:TextBox ID="CompensationLeaveEntitleClaimHourFrom" runat="Server" />-
                    <asp:TextBox ID="CompensationLeaveEntitleClaimHourTo" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label Text="No. of Hours Claim" runat="server" />:
                </td>
                <td class="pm_field" >
                    <asp:TextBox ID="CompensationLeaveEntitleHoursClaim" runat="Server" />
                </td>
                <td class="pm_field_header" >
                    <asp:Label ID="Label1" Text="Approved By" runat="server" />:
                </td>
                <td class="pm_field" >
                    <asp:TextBox ID="CompensationLeaveEntitleApprovedBy" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Remark" runat="server" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:TextBox ID="CompensationLeaveEntitleRemark" runat="Server" TextMode="MultiLine" Columns="80" Rows="5" />
                </td>
            </tr>
        </table>
        </ContentTemplate>
        </asp:UpdatePanel>
        
</asp:Content> 