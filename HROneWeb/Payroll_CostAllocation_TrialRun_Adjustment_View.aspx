<%@ Page Language="C#" MasterPageFile="~/MainMasterPage.master" AutoEventWireup="true" CodeFile="Payroll_CostAllocation_TrialRun_Adjustment_View.aspx.cs" Inherits="Payroll_CostAllocation_TrialRun_View" %>


<%@ Register Src="~/controls/Emp_Header.ascx" TagName="Emp_Header" TagPrefix="uc1" %>
<%@ Register Src="~/controls/Payroll_PeriodInfo.ascx" TagName="Payroll_PeriodInfo" TagPrefix="uc2" %>
<%@ Register Src="~/controls/Payroll_CostAllocationList.ascx" TagName="Payroll_CostAllocationList" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" Runat="Server">
        <input type="hidden" id="EmpPayrollID" runat="server" name="ID" />
        <input type="hidden" id="EmpPayStatus" runat="server" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="Title" Text="Cost Allocation Adjustment" runat="server" />
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
                    <asp:Button ID="Back" Text="- Back -" runat="server" CssClass="button" OnClick="Back_Click" UseSubmitBehavior="false" />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openAlert(); return false;"/>
                </td>
            </tr>
        </table>
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <tr>
                <td>
                    <uc1:Emp_Header ID="ucEmp_Header" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <uc2:Payroll_PeriodInfo ID="ucPayroll_PeriodInfo" runat="server" />
                </td>
            </tr>
        </table>
        
             
        
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate >
        <uc1:Payroll_CostAllocationList ID="Payroll_CostAllocationList" runat="server"  />
    </ContentTemplate> 
    </asp:UpdatePanel> 
        

</asp:Content>

