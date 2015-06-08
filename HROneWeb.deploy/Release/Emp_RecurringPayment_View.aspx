<%@ page language="C#" autoeventwireup="true" inherits="Emp_RecurringPayment_View, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/Emp_Common.ascx" TagName="Emp_Common" TagPrefix="uc1" %>
<%@ Register Src="~/controls/Emp_RecurringPayment_List.ascx" TagName="Emp_RecurringPayment_List" TagPrefix="uc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="EmpRPID" runat="server" name="ID" />
        <input type="hidden" id="EmpID" runat="server" name="ID" />
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
                    <asp:Label runat="server" Text="View Recurring Payment" />
                </td>
            </tr>
        </table>
        
            
                
        
        <uc1:Emp_Common id="Emp_Common1" runat="server"></uc1:Emp_Common><br />
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar id="toolBar" runat="server"
                     SaveButton_Visible="false" 
                     OnNewButton_Click="New_Click"
                     OnBackButton_Click="Back_Click"
                     OnEditButton_Click ="Edit_Click"
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
                    <asp:Label runat="server" Text="From" />:
                </td>
                <td class="pm_field" >
                    <asp:Label ID="EmpRPEffFr" runat="Server" />
                </td>
                <td class="pm_field_header" >
                    <asp:Label runat="server" Text="To" />:
                </td>
                <td class="pm_field" >
                    <asp:Label ID="EmpRPEffTo" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Payment Code" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayCodeID" runat="Server" />
                </td>
            </tr>
            <tr id="MonthlyCommissionRow1" runat="server" visible="false">
                <td class="pm_field_header">
                    <asp:Label ID="Label14" runat="server" Text="Target Salary" />:
                </td>
                
                <td class="pm_field">
                    <asp:Label ID="EmpRPBasicSalary" runat="Server" />
                </td>
                
                <td class="pm_field_header">
                    <asp:Label ID="Label15" runat="server" Text="FPS." />:
                </td>
                <td class="pm_field">
                    <asp:Label ID="EmpRPFPS" runat="Server" />%
                </td>
            </tr>
            
            <!-- Start 000159, Ricky So, 2015-01-23
            
            <tr id="NonSalaryBasedComissionBaseRow1" runat="server" visible="false">
                <td class="pm_field_header">
                    <asp:Label ID="Label17" runat="server" Text="On Target Commission" />:
                </td>
                
                <td class="pm_field">
                    <asp:Label ID="EmpRPOTCAmount" runat="Server" />
                </td>                
            </tr>
            
            End 000159, Ricky So, 2015-01-23 -->
            
            
            <tr id="PayscaleRow1" runat="server" visible="false">
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="PayScale Scheme" />:
                </td>
                <td class="pm_field">
                    <asp:Label ID="SchemeCode" runat="Server" />
                </td>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="PayScale Capacity" />:
                </td>
                <td class="pm_field">
                    <asp:Label ID="Capacity" runat="server" />
                </td>
            </tr>
            <tr id="PayscaleRow2" runat="server" visible="false">
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="PayScale Point" />:
                </td>
                <td class="pm_field">
                    <asp:Label ID="Point" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Amount" />:
                </td>
                <td class="pm_field">
                    <asp:Label ID="EmpRPAmount" runat="Server"  />
                    <asp:Label ID="CurrencyID" runat="Server" visible="false" />
                </td>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Unit" />:
                </td>
                <td class="pm_field">
                    <asp:Label ID="EmpRPUnit" runat="Server" />
                    <asp:Label ID="EmpRPUnitPeriodAsDaily" runat="server" Text="Calculate as daily" />
                </td>
            </tr>
            <tr id="EmpRPUnitPeriodAsDailyPayFormIDRow" runat="server" visible="false" >
                <td class="pm_field_header">
                    <asp:Label ID="Label11" runat="server" Text="Prorata Formula" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="EmpRPUnitPeriodAsDailyPayFormID" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Method" />:</td>
                <td class="pm_field">
                    <asp:Label ID="EmpRPMethod" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label ID="Label2" runat="server" Text="Non-payroll Item" />?
                </td>
                <td class="pm_field">
                    <asp:Label ID="EmpRPIsNonPayrollItem" runat="Server" />
                </td>
            </tr>
            <tr id="BankAccountRow" runat="server" >
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Bank Account Number" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="EmpAccID" runat="Server" />
                    <asp:Label ID="lblDefaultBankAccount" runat="server" />
                </td>
            </tr>
            <tr id="CostCenterRow" runat="server" >
                <td class="pm_field_header">
                    <asp:Label ID="Label3" runat="server" Text="Cost Center" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="CostCenterID" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Remark" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="EmpRPRemark" runat="Server" />
                </td>
            </tr>
        </table>
        <br />
       <%-- Start 0000166, KuangWei, 2015-02-03 --%>
       <table id="winson_header" border="0" width="100%" class="pm_section_title" runat="server" visible="false">
            <tr >
                <td>
                    <asp:Label runat="server" Text="Winson Customized Roster Information" />
                </td>
            </tr>
        </table>
        <table id="winson_content" border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section" runat="server" visible="false">
            <col width="15%" />
            <col width="35%" />
            <col width="15%" />
            <col width="35%" />
            <tr >
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Shift Duty Code" />:</td>
                <td class="pm_field">
                    <asp:Label ID="ShiftDutyCode" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Payment Calculation Formula Code" />:
                </td>
                <td class="pm_field">
                    <asp:Label ID="PayCalFormulaCode" runat="Server" />
                </td>
            </tr> 
        </table>
       <%-- End 0000166, KuangWei, 2015-02-03 --%>
        <table border="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Recurring Payment History" />
                </td>
            </tr>
        </table>
        <uc1:Emp_RecurringPayment_List ID="Emp_RecurringPayment_List1" runat="server" ShowHistory="true" AllowModify="false"   ShowNonPayrollItem="true" ShowPayrollItem="true"/>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content> 