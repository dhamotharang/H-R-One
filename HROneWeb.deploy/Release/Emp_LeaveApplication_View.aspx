<%@ page language="C#" autoeventwireup="true" inherits="Emp_LeaveApplication_View, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>

<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/Emp_Common.ascx" TagName="Emp_Common" TagPrefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="LeaveAppID" runat="server" />
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
                    <asp:Label Text="View Leave Application" runat="server" />
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
                     OnDeleteButton_Click="Delete_Click"
                     CustomButton1_Name="Force Edit" OnCustomButton1_Click="btnForceEdit_Click"
                     CustomButton2_Name="Force Delete" OnCustomButton2_Click="btnForceDelete_Click" 
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
        <uc1:Emp_Common ID="Emp_Common1" runat="server" />
        <br />
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="15%" />
            <col width="35%" />
            <col width="15%" />
            <col width="35%" />
            <tr>
                <td class="pm_field_header" >
                    <asp:Label Text="Leave Code" runat="server" />:</td>
                <td class="pm_field" >
                    <asp:Label ID="LeaveCodeID" runat="Server" /></td>
                <td class="pm_field_header" >
                    <asp:Label Text="Type" runat="server" />:</td>
                <td class="pm_field" >
                    <asp:Label ID="LeaveAppUnit" runat="Server"  /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Date" runat="server" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="LeaveAppDateFrom" runat="Server" />
                    
                    <asp:PlaceHolder ID="LeaveAppDateToPlaceHolder" runat="server">-
                        <asp:Label ID="LeaveAppDateTo" runat="Server" />
                        </asp:PlaceHolder>
                </td>

            </tr>
            <tr id="TimeRow" runat="server">
                <td class="pm_field_header">
                    <asp:Label Text="Time" runat="server" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="LeaveAppTimeFrom" runat="Server" />-
                    <asp:Label ID="LeaveAppTimeTo" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Days Taken" runat="server" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="LeaveAppDays" runat="Server" />
                </td>
            </tr>
            <tr runat="server" id="HoursClaimPanel">
                <td class="pm_field_header">
                    <asp:Label ID="lblLeaveAppHours" Text="Hours Claim" runat="server" />:</td>
                <td class="pm_field" colspan="3" >
                    <asp:Label ID="LeaveAppHours" runat="Server" />
                </td>
            </tr>
            <tr id="LeaveCodeIsShowMedicalCertOptionPanel" runat="server">
                <td class="pm_field_header">
                   <%-- Start 0000048, Miranda, 2014-06-03 --%>
                   <asp:Label ID="Label2" Text="Relevant Certificate" runat="server" />:</td>
                   <%-- End 0000048, Miranda, 2014-06-03 --%>
                <td class="pm_field">  
                    <asp:Label ID="LeaveAppHasMedicalCertificate" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label3" Text="Remark" runat="server" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="LeaveAppRemark" runat="Server" /></td>
            </tr>
            <tr id="PayrollProcessPanel" runat="server" >
                <td class="pm_field_header">
                   <asp:Label Text="Skip Payroll Process" runat="server" />:</td>
                <td class="pm_field">  
                    <asp:Label ID="LeaveAppNoPayProcess" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                   <asp:Label ID="Label1" Text="Payroll Process Cycle" runat="server" />:</td>
                <td class="pm_field" colspan="3">  
                    <asp:Label ID="PayPeriodID" runat="Server" />
                </td>
             </tr>
        </table>
        </ContentTemplate> 
        </asp:UpdatePanel> 
</asp:Content> 