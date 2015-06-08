<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Payroll_TrialRunAdjust_View.aspx.cs" Inherits="Payroll_TrialRunAdjust_View" MasterPageFile="~/MainMasterPage.master"  %>


<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/Emp_Header.ascx" TagName="Emp_Header" TagPrefix="uc1" %>
<%@ Register Src="~/controls/Payroll_PeriodInfo.ascx" TagName="Payroll_PeriodInfo" TagPrefix="uc2" %>
<%@ Register Src="~/controls/Payroll_PaymentRecordList.ascx" TagName="Payroll_PaymentRecordList" TagPrefix="uc1" %>
<%@ Register Src="~/controls/Payroll_MPFRecordList.ascx" TagName="Payroll_MPFRecordList" TagPrefix="uc1" %>
<%@ Register Src="~/controls/Payroll_ORSORecordList.ascx" TagName="Payroll_ORSORecordList" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="EmpPayrollID" runat="server" name="ID" />
        <input type="hidden" id="EmpID" runat="server" name="EmpID" />
        <input type="hidden" id="EmpPayStatus" runat="server" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="Title" Text="Payroll Trial Run Adjustment" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="SubTitle" Text="Payroll Trial Run Information" runat="server" />:
                </td>
            </tr>
        </table>
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <asp:HiddenField ID="FUNCTION_CODE" runat="server" />
                    <asp:Button ID="Back" Text="- Back -" runat="server" CssClass="button" OnClick="Back_Click" UseSubmitBehavior="false" />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openAlert(); return false;"/>
                </td>
            </tr>
        </table>
        <br />
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <tr>
                <td colspan="2">
                    <uc1:Emp_Header ID="ucEmp_Header" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <uc2:Payroll_PeriodInfo ID="ucPayroll_PeriodInfo" runat="server" />
                </td>
            </tr>
        </table> 
        <asp:UpdatePanel ID="UpdatePanel2" runat="server"  UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Payroll_PaymentRecordList" />
        </Triggers>
        <ContentTemplate >
        
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="30%" />
            <col width="70%" />
            <tr>
                <td colspan="2">
                    <tb:DetailToolBar id="toolBar" runat="server"
                     NewButton_Visible="false" 
                     SaveButton_Visible="false" 
                     BackButton_Visible="false" 
                     OnEditButton_Click ="btnEdit_Click"
                     DeleteButton_Visible="false"
                     CustomButton1_Visible="false" 
                     CustomButton1_Name="Cancel"
                     OnSaveButton_Click="btnUpdate_Click"
                     OnCustomButton1_Click="btnCancel_Click"
                      />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label6" runat="server" Text="Trial Run Date" />:
                </td>
                <td class="pm_field" >
                    <asp:Label ID="EmpPayTrialRunDate" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label8" runat="server" Text="Trial Run By" />:
                </td>
                <td class="pm_field" >
                    <asp:Label ID="EmpPayTrialRunBy" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label7" runat="server" Text="Confirm Date" />:
                </td>
                <td class="pm_field" >
                    <asp:Label ID="EmpPayConfirmDate" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label9" runat="server" Text="Confirm By" />:
                </td>
                <td class="pm_field" >
                    <asp:Label ID="EmpPayConfirmBy" runat="server" />
                </td>
            </tr>
            <tr runat="server" id="OldNumOfDayContPanel">
                <td class="pm_field_header">
                    <asp:Label ID="lblNumDays" runat="server" Text="No. of days count" />:
                </td>
                <td class="pm_field">
                    <asp:Label ID="lblEmpPayNumOfDayCount" runat="server" />
                    <asp:TextBox ID="EmpPayNumOfDayCount" runat="server" Visible="false"  />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label1" runat="server" Text="Total Working Hours" />:
                </td>
                <td class="pm_field" >
                    <asp:Label ID="lblTotalWorkingHours" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label4" runat="server" Text="Working Hours Count for this payroll Section" />:
                </td>
                <td class="pm_field">
                    <asp:Label ID="lblEmpPayTotalWorkingHours" runat="server" />
                    <asp:TextBox ID="EmpPayTotalWorkingHours" runat="server" Visible="false"  />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label2" runat="server" Text="Total Wages for Minimum Wages" />:
                </td>
                <td class="pm_field">
                    <asp:Label ID="TotalWagesForMinimumWages" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label3" runat="server" Text="Minimum Wages Required" />:
                </td>
                <td class="pm_field">
                    <asp:Label ID="MinimumWagesRequired" runat="server" />
                    <asp:Button ID="btnAddAdditionalRemuneration" runat="server"  Text="Add Additional Remuneration" CssClass="button" OnClick="btnAddAdditionalRemuneration_Click" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label5" runat="server" Text="Remark" />:
                </td>
                <td class="pm_field">
                    <asp:Label ID="lblEmpPayRemark" runat="server" />
                    <asp:TextBox ID="EmpPayRemark" runat="server" Columns="50" Rows="5" TextMode="multiLine" Visible="false" />
                </td>
            </tr>
        </table>
    </ContentTemplate> 
    </asp:UpdatePanel>         
        
             
        
        
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnAddAdditionalRemuneration" EventName="Click" />
    </Triggers>
    <ContentTemplate >
        <uc1:Payroll_PaymentRecordList ID="Payroll_PaymentRecordList" runat="server" OnChanged="Payroll_PaymentRecordList_Changed" />
        <uc1:Payroll_MPFRecordList ID="Payroll_MPFRecordList" runat="server" OnRecalculate="Payroll_MPFRecordList_Recalculate" />
        <uc1:Payroll_ORSORecordList ID="Payroll_ORSORecordList" runat="server"  OnRecalculate="Payroll_ORSORecordList_Recalculate" />
    </ContentTemplate> 
    </asp:UpdatePanel>         

</asp:Content> 