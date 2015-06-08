<%@ page language="C#" autoeventwireup="true" inherits="Reports, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>



<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="UserID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td class="pm_section_title">
                    <asp:Label runat="server" Text="Reports" /> :</td>
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
                    <a href="Report_EmployeeList.aspx" target="_blank"><asp:Label runat="server" Text="Employee List" /></a></td>
            </tr>
            <tr>
                <td class="pm_field">
                    <a href="Report_EmployeeContact.aspx" target="_blank"><asp:Label runat="server" Text="Employee Contact" /></a></td>
            </tr>
            <tr>
                <td class="pm_field">
                    <a href="Report_EmployeeDetail.aspx" target="_blank"><asp:Label runat="server" Text="Employee Detail" /></a></td>
            </tr>
            <tr>
                <td class="pm_field">
                    <a href="Report_LeaveSummary.aspx" target="_blank"><asp:Label runat="server" Text="Leave Summary" /></a></td>
            </tr>
        </table>
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <tr>
                <td class="pm_field">
                    <a href="Report_Payroll_EEDAWList_List.aspx" target="_blank"><asp:Label runat="server" Text="Employee Daily Average Wages Report" /></a></td>
            </tr>
            <tr>
                <td class="pm_field">
                    <a href="Report_Payroll_EEOverallPaymentSummary_List.aspx" target="_blank"><asp:Label runat="server" Text="Employee Overall Payment Summary" /></a></td>
            </tr>
            <tr>
                <td class="pm_field">
                    <a href="Report_Payroll_MPFDetailList_List.aspx" target="_blank"><asp:Label runat="server" Text="MPF Detail List" /></a></td>
            </tr>
            <tr>
                <td class="pm_field">
                    <a href="Report_Payroll_MPFRemittanceStatement_List.aspx" target="_blank"><asp:Label runat="server" Text="MPF Remittance Statement" /></a></td>
            </tr>
            <tr>
                <td class="pm_field">
                    <a href="Report_Payroll_PaySlip_List.aspx" target="_blank"><asp:Label runat="server" Text="Pay Slip Report" /></a></td>
            </tr>
        </table>    
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <tr>
                <td class="pm_field">
                    <a href="Report_Taxation_Report_List.aspx" target="_blank"><asp:Label runat="server" Text="Taxation Report" /></a></td>
            </tr>
        </table>    
</asp:Content> 