<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Emp_LeaveApplication_Edit.aspx.cs"    Inherits="Emp_LeaveApplication_Edit" MasterPageFile="~/MainMasterPage.master"  %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/Emp_Common.ascx" TagName="Emp_Common" TagPrefix="uc1" %>
<%@ Register Src="~/controls/Emp_LeaveBalance_List.ascx" TagName="Emp_LeaveBalance_List"
    TagPrefix="uc5" %>
<%-- Start 0000120, KuangWei, 2014-12-14 --%>
<%@ Register Src="~/controls/Emp_Monthly_LeaveApplication_List.ascx" TagName="Emp_Monthly_LeaveApplication_List" TagPrefix="uc3" %>
<%-- End 0000120, KuangWei, 2014-12-14 --%>    
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="LeaveAppID" runat="server" />
        <input type="hidden" id="EmpID" runat="server" />
		<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label EnableViewState="false" Text="Employee Information" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="ActionHeader" runat="server" EnableViewState="false" Text="Edit" />
                    <asp:Label EnableViewState="false" Text="Leave Application" runat="server" />
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
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" EnableViewState="false" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolBar" />
        </Triggers>
        <ContentTemplate >
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <colgroup>
            <col width="15%" />
            <col width="35%" />
            <col width="15%" />
            <col width="35%" />
            </colgroup>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label EnableViewState="false" Text="Leave Code" runat="server" />:
                </td>
                <td class="pm_field" >
                    <asp:DropDownList ID="LeaveCodeID" runat="Server" AutoPostBack="true" OnSelectedIndexChanged="LeaveCodeID_SelectedIndexChanged" />
                </td>
                <td class="pm_field_header" >
                    <asp:Label ID="Label4" EnableViewState="false" Text="Type" runat="server" />:</td>
                <td class="pm_field" >
                    <asp:DropDownList ID="LeaveAppUnit" runat="Server" AutoPostBack="true" OnSelectedIndexChanged="LeaveAppUnit_SelectedIndexChanged" />
                </td>
            </tr>
            <tr>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label EnableViewState="false" Text="Date From" runat="server" />:</td>
                <td class="pm_field">
                    <uc1:WebDatePicker id="LeaveAppDateFrom" runat="server" AutoPostBack="true" OnChanged="LeaveAppDateFrom_Changed" />
				</td>
                <asp:PlaceHolder ID="LeaveAppDateToPlaceHolder" runat="server">
                <td class="pm_field_header">
                    <asp:Label EnableViewState="false" Text="Date To" runat="server" />:</td>
                <td class="pm_field">
                    <uc1:WebDatePicker id="LeaveAppDateTo" runat="server" AutoPostBack="true" OnChanged="LeaveAppDateTo_Changed" />
                </td>
                </asp:PlaceHolder>
            </tr>
            <tr id="TimeRow" runat="server">
                <td class="pm_field_header">
                    <asp:Label EnableViewState="false" Text="Time" runat="server" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:TextBox ID="LeaveAppTimeFrom" runat="Server" AutoPostBack="true" OnTextChanged="LeaveAppTime_TextChanged"/>-
                    <asp:TextBox ID="LeaveAppTimeTo" runat="Server" AutoPostBack="true" OnTextChanged="LeaveAppTime_TextChanged"/>
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label EnableViewState="false" Text="Days Taken" runat="server" />:</td>
                <td class="pm_field" >
                    <asp:TextBox ID="LeaveAppDays" runat="Server" />
                    <asp:Button ID="btnEstimateTotalLeaveDay" runat="server" EnableViewState="false" Text="Total Number of Days" CssClass="button" OnClick="btnEstimateTotalLeaveDay_Click"/>
                </td>
                <td class="pm_field" colspan="2">
                    <asp:Label ID="lblStatutoryHolidayList" runat="Server" />
                </td>
            </tr>
            <tr runat="server" id="HoursClaimPanel">
                <td class="pm_field_header">
                    <asp:Label ID="lblLeaveAppHours" EnableViewState="false" Text="Hours Claim" runat="server" />:</td>
                <td class="pm_field" colspan="3" >
                    <asp:TextBox ID="LeaveAppHours" runat="Server" CssClass="pm_required" />
                </td>
            </tr>
            <tr id="LeaveCodeIsShowMedicalCertOptionPanel" runat="server">
                <td class="pm_field_header">
                   <%-- Start 0000048, Miranda, 2014-06-03 --%>
                   <asp:Label ID="Label1" EnableViewState="false" Text="Relevant Certificate" runat="server" />:</td>
                   <%-- End 0000048, Miranda, 2014-06-03 --%>
                <td class="pm_field" colspan="3">  
                    <asp:CheckBox ID="LeaveAppHasMedicalCertificate" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label EnableViewState="false" Text="Remark" runat="server" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:TextBox ID="LeaveAppRemark" runat="Server" TextMode="MultiLine" Columns="80"
                        Rows="5" /></td>
            </tr>
            <tr id="PayrollProcessPanel" runat="server" >
                <td class="pm_field_header">
                   <asp:Label ID="Label3" EnableViewState="false" Text="Skip Payroll Process" runat="server" />:</td>
                <td class="pm_field">  
                    <asp:CheckBox ID="LeaveAppNoPayProcess" runat="Server" />
                </td>
            </tr>
        </table>
        <br />
        <%-- Start 0000120, KuangWei, 2014-12-14 --%>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="Label5" runat="server" EnableViewState="false" Text="Monthly Leave Application Reference" />
                </td>
            </tr>
        </table>    
        <uc3:Emp_Monthly_LeaveApplication_List ID="Emp_Monthly_LeaveApplication_List1" runat="server" />        
        <br />
        <%-- End 0000120, KuangWei, 2014-12-14 --%>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" EnableViewState="false" Text="Leave Balance" />
                </td>
            </tr>
        </table>
        
        <uc5:Emp_LeaveBalance_List ID="Emp_LeaveBalance_List1" runat="server" />
        </ContentTemplate>
        </asp:UpdatePanel>
        
</asp:Content> 