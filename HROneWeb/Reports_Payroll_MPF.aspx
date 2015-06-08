<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Reports_Payroll_MPF.aspx.cs" Inherits="Reports_Payroll_MPF" MasterPageFile="~/MainMasterPage.master"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="UserID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label Text="Report" runat="server" /></td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label Text="Payroll & MPF Report" runat="server" /></td>
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
                    <a href="Report_Payroll_Detail_List.aspx" ><asp:Label Text="Payroll Detail Report" runat="server" /></a></td>
            </tr>
            <tr>
                <td class="pm_field">
                    <a href="Report_Payroll_Summary_List.aspx" ><asp:Label Text="Payroll Summary Report" runat="server" /></a></td>
            </tr>
			<tr>
                <td class="pm_field">
                    <a href="Report_Payroll_EEDAWList_List.aspx" ><asp:Label Text="Employee Daily Average Wages Report" runat="server" /></a></td>
            </tr>
            <tr>
                <td class="pm_field">
                    <a href="Report_Payroll_EEOverallPaymentSummary_List.aspx" ><asp:Label Text="Employee Overall Payment Summary" runat="server" /></a></td>
            </tr>
            <tr>
                <td class="pm_field">
                    <a href="Report_Payroll_MPFDetailList_List.aspx" ><asp:Label Text="MPF Details List" runat="server" /></a></td>
            </tr>
            <tr>
                <td class="pm_field">
                    <a href="Report_Payroll_MPFRemittanceStatement_List.aspx" ><asp:Label ID="Label5" Text="MPF Remittance Statement" runat="server" /></a></td>
            </tr>

            <tr id="CUSTOM005" runat="server">
                <td class="pm_field" runat="server">
                    <a href="Customize_FandV_Report_Payroll_MPFRemittanceStatement_List.aspx" ><asp:Label ID="Label6" Text="Customized MPF Remittance Statement" runat="server" /></a></td>
            </tr>

            <!-- Start 0000085, Ricky So, 2014-08-26 -->
            <tr>
                <td class="pm_field">
                    <a href="Report_Payroll_MPFFirstContributionStatement_List.aspx" ><asp:Label Text="MPF First Contribution Statement" runat="server" /></a></td>
            </tr>
            <!-- End 0000085, Ricky So, 2014-08-26 -->

            <tr>
                <td class="pm_field">
                    <a href="Report_Payroll_PaySlip_List.aspx" ><asp:Label Text="Pay Slip Report" runat="server" /></a></td>
            </tr>
            <tr>
                <td class="pm_field">
                    <a href="Report_Payroll_PaySlip_DotMatrix_List.aspx" ><asp:Label Text="Pay Slip Report (Dot Matrix)" runat="server" /></a>
                </td>
            </tr>
            <tr>
                <td class="pm_field">
                    <a href="Report_Payroll_FinalPaymentStatement_List.aspx" ><asp:Label Text="Final Payment Statement" runat="server" /></a></td>
            </tr>
            <%-- Start 0000087, Miranda, 2014-09-16 --%>
            <tr>
                <td class="pm_field">
                    <a href="Report_Payroll_NewJoinPaymentSummary.aspx" ><asp:Label Text="New Join Payment Summary" runat="server" /></a></td>
            </tr>
            <%-- End 0000087, Miranda, 2014-09-16 --%>
            <tr>
                <td class="pm_field">
                    <a href="Report_Payroll_DiscrepancyList_List.aspx" ><asp:Label Text="Payroll Discrepancy List" runat="server" /></a></td>
            </tr>
            <tr>
                <td class="pm_field">
                    <a href="Report_Payroll_NetPaymentListByPaymentMethod_List.aspx" ><asp:Label ID="Label2" Text="Net Payment Report by Payment Method" runat="server" /></a></td>
            </tr>            
            <tr>
                <td class="pm_field">
                    <a href="Report_Payroll_PaymentList.aspx" ><asp:Label Text="Payment Summary List" runat="server" /></a></td>
            </tr>            
            <%-- Start 0000181, KuangWei, 2015-04-16 --%>
            <tr id="CUSTOM004" runat="server">
                <td class="pm_field" runat="server">
                    <a href="Report_Payroll_PaymentList_WaiJi.aspx" ><asp:Label ID="Label4" Text="Payment Summary List (WaiJi)" runat="server" /></a>
                </td>
            </tr>            
            <%-- End 0000181, KuangWei, 2015-04-16 --%>            
            <tr>
                <td class="pm_field">
                    <a href="Report_Payroll_StatutoryMinWageSummary.aspx" ><asp:Label Text="Statutory Minimum Wage Summary Report" runat="server" /></a></td>
            </tr>
            <asp:PlaceHolder ID="HROneOnlyReportSection" runat="server" >     
            <tr>
                <td class="pm_field">
                    <a href="Report_Payroll_PaymentAllocationList.aspx" ><asp:Label Text="Payment Allocation Report" runat="server" /></a></td>
            </tr>            
            <tr>
                <td class="pm_field">
                    <a href="Report_Payroll_PayrollAllocationReport_Detail.aspx" ><asp:Label Text="Payroll Allocation Report - Details" runat="server" /></a></td>
            </tr>            
            <tr>
                <td class="pm_field">
                    <a href="Report_Payroll_LongServicePaymentSeverancePayment_List.aspx" ><asp:Label Text="Long Service Payment / Severance Payment Estimation List" runat="server" /></a></td>
            </tr>
            <tr id="ReportPFundPanel" runat="server">
                <td class="pm_field">
                    <a href="Report_Payroll_PFundStatement_List.aspx" ><asp:Label ID="Label1" Text="P-Fund Report" runat="server" /></a></td>
            </tr>            
            <tr id="CUSTOM001" runat="server">
                <td class="pm_field" runat="server">
                    <a href="Customize_Report_Payroll_Kerry_PFund_List.aspx" ><asp:Label ID="Label3" Text="KTP-Fund Report" runat="server" /></a></td>
            </tr>            
            </asp:PlaceHolder>
        </table>   
</asp:Content> 