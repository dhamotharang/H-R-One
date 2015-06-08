<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Reports_Emp.aspx.cs" Inherits="Reports_Emp" MasterPageFile="~/MainMasterPage.master" %>



<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="UserID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Report" /></td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Employee Report" /></td>
            </tr>
        </table>
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openAlert(); return false;"/>
                </td>
            </tr>
        </table>
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <tr>
                <td class="pm_field">
                    <a href="Report_EmployeeList.aspx" ><asp:Label runat="server" Text="Employee List" /></a></td>
            </tr>
            <tr>
                <td class="pm_field">
                    <a href="Report_EmployeeContact.aspx" ><asp:Label runat="server" Text="Employee Contact Report" /></a></td>
            </tr>
            <tr>
                <td class="pm_field">
                    <a href="Report_Employee_SalaryList.aspx" ><asp:Label ID="Label1" runat="server" Text="Employee Salary List" /></a></td>
            </tr>
            <tr>
                <td class="pm_field">
                    <a href="Report_Employee_YearOfServiceList.aspx" ><asp:Label ID="Label3" runat="server" Text="Employee Year of Service List" /></a></td>
            </tr>
            <tr>
                <td class="pm_field">
                    <a href="Report_Employee_BankAccountList.aspx" ><asp:Label ID="Label2" runat="server" Text="Employee Bank Account List" /></a></td>
            </tr>
            <tr>
                <td class="pm_field">
                    <a href="Report_EmployeeDetail.aspx" ><asp:Label runat="server" Text="Employee Details Report" /></a></td>
            </tr>
			<tr>
                <td class="pm_field">
                    <a href="Report_Employee_HeadCount.aspx" ><asp:Label runat="server" Text="Employee Head Count Report" /></a></td>
            </tr>
            <tr>
                <td class="pm_field">
                    <a href="Report_Employee_Gender.aspx" ><asp:Label ID="Label7" runat="server" Text="Employee Gender Report" /></a></td>
            </tr>
            <tr>
                <td class="pm_field">
                    <a href="Report_Employee_Residency.aspx" ><asp:Label ID="Label8" runat="server" Text="Employee Residency Report" /></a></td>
            </tr>
            <tr>
                <td class="pm_field">
                    <a href="Report_Employee_BirthdayList.aspx" ><asp:Label ID="Label9" runat="server" Text="Employee Birthday List" /></a></td>
            </tr>
			<tr>
                <td class="pm_field">
                    <a href="Report_Employee_NewJoinList_List.aspx" ><asp:Label ID="Label10" runat="server" Text="New Join Employee List" /></a></td>
            </tr>
			<tr>
                <td class="pm_field">
                    <a href="Report_Employee_ProbationList_List.aspx" ><asp:Label ID="Label11" runat="server" Text="Employee Probation List" /></a></td>
            </tr>
			<tr>
                <td class="pm_field">
                    <a href="Report_Employee_TerminationList_List.aspx" ><asp:Label runat="server" Text="Employee Termination List" /></a></td>
            </tr>
            <tr>
                <td class="pm_field">
                    <a href="Report_Employee_LeaveApplicationList.aspx" ><asp:Label ID="Label6" runat="server" Text="Employee Leave Application List" /></a></td>
            </tr>
            <tr>
                <td class="pm_field">
                    <a href="Report_LeaveSummary.aspx" ><asp:Label runat="server" Text="Leave Summary Report" /></a></td>
            </tr>
            <tr>
                <td class="pm_field">
                    <a href="Report_Leave_BalanceList.aspx" ><asp:Label ID="Label5" runat="server" Text="Leave Balance List" /></a></td>
            </tr>
            <tr id="ESSRequestPanel" runat="server">
                <td class="pm_field">
                    <a href="Report_Employee_ESSRequest.aspx" ><asp:Label ID="Label4" Text="ESS Pending Request Report" runat="server" /></a></td>
            </tr>            
        </table>
</asp:Content> 