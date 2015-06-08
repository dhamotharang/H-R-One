<%@ page language="C#" autoeventwireup="true" inherits="Payroll_Group_View, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
<%@ Register Src="~/controls/Payroll_PeriodInfo.ascx" TagName="Payroll_PeriodInfo" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="PayGroupID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" EnableViewState="false" Text="Payroll Group Setup" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" EnableViewState="false" Text="View Payroll Group" />:
                    <%=PayGroupCode.Text%>
                </td>
            </tr>
        </table>
        
            
                
        
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar id="toolBar" runat="server"
                     NewButton_Visible="false" 
                     CopyButton_Visible="true" 
                     SaveButton_Visible="false" 
                     OnCopyButton_Click="Copy_Click"
                     OnBackButton_Click="Back_Click"
                     OnEditButton_Click ="Edit_Click"
                     OnDeleteButton_Click ="Delete_Click"
                      />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
                </td>
            </tr>
        </table>
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="15%"/>
            <col width="35%"/>
            <col width="15%"/>
            <col width="35%"/>
            <tr>
                <td class="pm_field_title" colspan="4">
                    <asp:Label ID="Label3" runat="server" EnableViewState="false" Text="Payroll Group Information" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label4" runat="server" EnableViewState="false" Text="Code" />:
                </td>
                <td class="pm_field" >
                    <asp:Label ID="PayGroupCode" runat="Server" />
                </td>
                <td class="pm_field_header" >
                    <asp:Label ID="Label5" runat="server" EnableViewState="false" Text="Description" />:
                </td>
                <td class="pm_field" >
                    <asp:Label ID="PayGroupDesc" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label6" runat="server" EnableViewState="false" Text="Frequency" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayGroupFreq" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label42" runat="server" EnableViewState="false" Text="First Period Start Day" />:
                </td>
                <td class="pm_field" >
                    <asp:Label ID="PayGroupDefaultStartDay" runat="Server" />
                </td>
                <td class="pm_field_header" >
                    <asp:Label ID="Label43" runat="server" EnableViewState="false" Text="First Period Leave Cut Off Day" />:
                </td>
                <td class="pm_field" >
                    <asp:Label ID="PayGroupLeaveDefaultCutOffDay" runat="Server" />
                </td>
            </tr>
            <tr id="SemiMonthlyDayOptionsRow" runat="server" >
                <td class="pm_field_header" >
                    <asp:Label ID="Label44" runat="server" EnableViewState="false" Text="Second Period Start Day" />:
                </td>
                <td class="pm_field" >
                    <asp:Label ID="PayGroupDefaultNextStartDay" runat="Server" />
                </td>
                <td class="pm_field_header" >
                    <asp:Label ID="Label45" runat="server" EnableViewState="false" Text="Second Period Leave Cut Off Day" />:
                </td>
                <td class="pm_field" >
                    <asp:Label ID="PayGroupLeaveDefaultNextCutOffDay" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label22" runat="server" EnableViewState="false" Text="Rest day with pay" />:
                </td>
                <td class="pm_field" >
                    <asp:Label ID="PayGroupRestDayHasWage" runat="Server" />
                </td>
                <td class="pm_field_header" >
                    <asp:Label ID="Label30" runat="server" EnableViewState="false" Text="Meal break with pay" />:
                </td>
                <td class="pm_field" >
                    <asp:Label ID="PayGroupLunchTimeHasWage" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label27" runat="server" EnableViewState="false" Text="Payment Code for Additional Remuneration (for Statutory Minimum Wage)" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayGroupAdditionalRemunerationPayCodeID" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_title" colspan="4">
                    <asp:Label ID="Label39" Text="Prorata Formula Options" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label24" runat="server" EnableViewState="false" Text="Default Prorata Formula" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayGroupDefaultProrataFormula" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label32" runat="server" EnableViewState="false" Text="Rest Day" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayGroupRestDayProrataFormula" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label38" runat="server" EnableViewState="false" Text="New Join" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayGroupNewJoinFormula" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label40" runat="server" EnableViewState="false" Text="Change of Payroll Group or Recurring Payment" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayGroupExistingFormula" runat="Server" />
                </td>
            </tr>
            <tr id="PayGroupIsCNDProrataRow" runat="server" >
                <td class="pm_field_header" >
                    <asp:Label ID="Label41" runat="server" EnableViewState="false" Text="Prorata on Claims and Deductions" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayGroupIsCNDProrata" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_title" colspan="4">
                    <asp:Label ID="Label15" Text="Statutory Holiday Options" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label37" runat="server" EnableViewState="false" Text="Skip Statutory Holiday Calculation" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayGroupIsSkipStatHol" runat="Server" />
                </td>
            </tr>
            <asp:PlaceHolder ID="StatutoryHolidayOptionSection" runat="server" >
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label14" runat="server" EnableViewState="false" Text="Statutory Holiday Deduction Formula" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayGroupStatHolDeductFormula" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label16" runat="server" EnableViewState="false" Text="Payment Code for Statutory Holiday Deduction" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayGroupStatHolDeductPaymentCodeID" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label17" runat="server" EnableViewState="false" Text="Statutory Holiday Allowance Formula" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayGroupStatHolAllowFormula" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label18" runat="server" EnableViewState="false" Text="Payment Code for Statutory Holiday Allowance" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayGroupStatHolAllowPaymentCodeID" runat="Server" /></td>
            </tr>
            <tr id="PayGroupStatHolNextMonthRow" runat="server" >
                <td class="pm_field_header">
                    <asp:Label ID="Label19" runat="server" EnableViewState="false" Text="Calculate Statutory Holiday on next month" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayGroupStatHolNextMonth" runat="Server" />
                </td>
            </tr>
            <tr >
                <td class="pm_field_header">
                    <asp:Label ID="Label46" runat="server" EnableViewState="false" Text="Use Public Holiday Table instead of Statutory Holiday Table" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayGroupIsStatHolUsePublicHoliday" runat="Server" />
                </td>
            </tr>            
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label20" runat="server" EnableViewState="false" Text="Statutory Holiday Payment Eligible After" /> <br />(<asp:Label ID="Label21" runat="server" EnableViewState="false" Text="Start Date of Service" />):
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayGroupStatHolEligiblePeriod" runat="server" />
                    <asp:Label ID="PayGroupStatHolEligibleUnit" runat="server" /><br />
                    <asp:CheckBox ID="PayGroupStatHolEligibleAfterProbation" runat="server" EnableViewState="false" Text="Eligible After Probation" Enabled="false" /><br />
                    <asp:CheckBox ID="PayGroupStatHolEligibleSkipDeduction" runat="server" EnableViewState="false" Text="Skip Deduction" Enabled="false" />
                </td>
            </tr>
            </asp:PlaceHolder>
            <tr>
                <td class="pm_field_title" colspan="4">
                    <asp:Label ID="Label12" Text="Final Payment Options" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label7" runat="server" EnableViewState="false" Text="Final Payment Prorata Formula" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayGroupTerminatedFormula" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label8" runat="server" EnableViewState="false" Text="Daily Formula for AL Compensation" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayGroupTerminatedALCompensationDailyFormula" runat="Server" /><br />
                    <asp:Label ID="Label47" runat="server" EnableViewState="false" Text="second formula for comparsion" />:&nbsp<asp:Label ID="PayGroupTerminatedALCompensationDailyFormulaAlternative" runat="Server" /><br />
                    <asp:CheckBox ID="PayGroupTerminatedALCompensationIsSkipRoundingRule" runat="server" EnableViewState="false" Text="Do NOT apply rounding rule while calculating balance of Annual Leave" Enabled="false" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label9" runat="server" EnableViewState="false" Text="Payment Code for AL Compensation by Employer" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayGroupTerminatedALCompensationPaymentCodeID" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label31" runat="server" EnableViewState="false" Text="Payment Code for AL Compensation by Employee" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayGroupTerminatedALCompensationByEEPaymentCodeID" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                                <asp:Label ID="Label23" runat="server" EnableViewState="false" Text="Minimum period of employment for AL Compensation" /> :
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayGroupTerminatedALCompensationProrataEligiblePeriod" runat="server" />
                    <asp:Label ID="PayGroupTerminatedALCompensationProrataEligibleUnit" runat="server" />
                    <asp:CheckBox ID="PayGroupTerminatedALCompensationEligibleAfterProbation" runat="server" EnableViewState="false" Text="Eligible After Probation" Enabled="false" /><br />
                    <asp:CheckBox ID="PayGroupTerminatedALCompensationProrataEligibleCheckEveryLeaveYear" runat="server" EnableViewState="false" Text="Check every leave year for prorata of Annual Leave" Enabled="false" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label10" runat="server" EnableViewState="false" Text="Monthly payment formula for Payment In Lieu" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayGroupTerminatedPaymentInLieuMonthlyBaseMethod" runat="Server" /><br />
                    <asp:Label ID="Label48" runat="server" EnableViewState="false" Text="second formula for comparsion" />:&nbsp<asp:Label ID="PayGroupTerminatedPaymentInLieuMonthlyBaseMethodAlternative" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label11" runat="server" EnableViewState="false" Text="Daily payment formula for Payment In Lieu" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayGroupTerminatedPaymentInLieuDailyFormula" runat="Server" /><br />
                    <asp:Label ID="Label49" runat="server" EnableViewState="false" Text="second formula for comparsion" />:&nbsp<asp:Label ID="PayGroupTerminatedPaymentInLieuDailyFormulaAlternative" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label13" runat="server" EnableViewState="false" Text="Payment Code for Payment In Lieu (Employer)" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayGroupTerminatedPaymentInLieuERPaymentCodeID" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label25" runat="server" EnableViewState="false" Text="Payment Code for Payment In Lieu (Employee)" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayGroupTerminatedPaymentInLieuEEPaymentCodeID" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label26" runat="server" EnableViewState="false" Text="Monthly payment formula for Long Service Payment / Severance Payment" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayGroupTerminatedLSPSPMonthlyBaseMethod" runat="Server" /><br />
                    <asp:Label ID="Label50" runat="server" EnableViewState="false" Text="second formula for comparsion" />:&nbsp<asp:Label ID="PayGroupTerminatedLSPSPMonthlyBaseMethodAlternative" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label28" runat="server" EnableViewState="false" Text="Payment code for Long Service Payment" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayGroupTerminatedLSPPaymentCodeID" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label29" runat="server" EnableViewState="false" Text="Payment code for Severance Payment" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayGroupTerminatedSPPaymentCodeID" runat="Server" />
                </td>
            </tr>
            <asp:PlaceHolder ID="FinalPaymentHolidayPanel" runat="server" >
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label33" runat="server" EnableViewState="false" Text="Daily Formula for Rest Day Compensation" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayGroupTerminatedRestDayCompensationDailyFormula" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label34" runat="server" EnableViewState="false" Text="Payment Code for Rest Day Compensation" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayGroupTerminatedRestDayCompensationPaymentCodeID" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label35" runat="server" EnableViewState="false" Text="Daily Formula for Statutory Holiday Compensation" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayGroupTerminatedStatutoryHolidayCompensationDailyFormula" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label36" runat="server" EnableViewState="false" Text="Payment Code for Statutory Holiday Compensation" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayGroupTerminatedStatutoryHolidayCompensationPaymentCodeID" runat="Server" />
                </td>
            </tr>
            </asp:PlaceHolder> 
            <tr>
                <td class="pm_field_title" colspan="4">
                    <asp:Label ID="Label51" Text="Miscellaneous" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label52" runat="server" EnableViewState="false" Text="Default Next Period From" />:
                </td>
                <td class="pm_field" >
                    <asp:Label ID="PayGroupNextStartDate" runat="Server" />
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label53" runat="server" EnableViewState="false" Text="Default Next Period To" />:
                </td>
                <td class="pm_field" >
                    <asp:Label ID="PayGroupNextEndDate" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label54" runat="server" EnableViewState="false" Text="Payment Comparison" />:<br />
                    (<asp:Label ID="Label55" runat="server" EnableViewState="false" Text="Applicable only if deduction formula and allowance formula is different" />)
                </td>
                <td class="pm_field">
                    <asp:CheckBox ID="PayGroupPayAdvance" runat="server" EnableViewState="false" Text="Comparison between formula" Enabled="false" /><br  />
                    <asp:CheckBox ID="PayGroupPayAdvanceCompareTotalPaymentOnly" runat="server" EnableViewState="false" Text="Comparison between result" Enabled="false"   />
                </td>
                <asp:PlaceHolder ID="PayGroupUseCNDForDailyHourlyPaymentSection" runat="server" >
                <td class="pm_field_header">
                    <asp:Label ID="Label56" runat="server" EnableViewState="false" Text="Use Claims and Deduction for Daily/Hourly Recurring Payment" />:
                </td>
                <td class="pm_field" >
                    <asp:Label ID="PayGroupUseCNDForDailyHourlyPayment" runat="server" />
                </td>
                </asp:PlaceHolder>
            </tr>
            <tr>
                <td class="pm_field_title" colspan="4">
                    <asp:Label ID="lblYEBPlanOption" Text="Year End Bonus Options" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label57" runat="server" EnableViewState="false" Text="Process Month" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayGroupYEBStartPayrollMonth" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label58" runat="server" EnableViewState="false" Text="Eligible Period" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayGroupYEBMonthFrom" runat="server" />
                    <asp:Label ID="lblTo" runat="server" EnableViewState="false" Text="To" />
                    <asp:Label ID="PayGroupYEBMonthTo" runat="server" />
                </td>
            </tr>
            <tr id="LeaveCodeOverrideHeaderRow" runat="server" >
                <td class="pm_field_title" colspan="4">
                    <asp:Label ID="Label59" Text="Leave Calculation Override" runat="server" />
                    <tb:RecordListFooter ID="LeaveCodeOverrideListFooter" runat="server" 
                        ShowAllRecords="true"
                        ListOrderBy="LeaveCode"
                        ListOrder="true" Visible="false"  />

                </td>
            </tr>
            <asp:Repeater ID="LeaveCodeOverrideRepeater" runat="server" OnItemDataBound="LeaveCodeOverrideRepeater_ItemDataBound">
            <ItemTemplate>
                <tr>
                    <td class="pm_field_title" colspan="4">
                        <asp:Label ID="LeaveCodeID" runat="server" />
                    </td>
                </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="lblPayrollGroupLeaveCodeSetupLeaveDeductFormula" runat="server" EnableViewState="false" Text="Deduction Formula" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayrollGroupLeaveCodeSetupLeaveDeductFormula" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="lblPayrollGroupLeaveCodeSetupLeaveDeductPaymentCodeID" runat="server" EnableViewState="false" Text="Payment Code for Deduction" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayrollGroupLeaveCodeSetupLeaveDeductPaymentCodeID" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="lblPayrollGroupLeaveCodeSetupLeaveAllowFormula" runat="server" EnableViewState="false" Text="Allowance Formula" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayrollGroupLeaveCodeSetupLeaveAllowFormula" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="lblPayrollGroupLeaveCodeSetupLeaveAllowPaymentCodeID" runat="server" EnableViewState="false" Text="Payment Code for Allowance" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayrollGroupLeaveCodeSetupLeaveAllowPaymentCodeID" runat="Server" />
                </td>
            </tr>
            </ItemTemplate>
            </asp:Repeater>
        </table>
        <br/>
        <br/>
        <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolBar"  />
        </Triggers>
        <ContentTemplate >        
            <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
                <tr>
                    <td class="pm_field_header">
                        <asp:Label runat="server" EnableViewState="false" Text="Payroll Cycle" />:
                    </td>
                    <td class="pm_field">
                        <table border="0">
                            <tr>
                                <td>
                                    <asp:DropDownList ID="PayPeriodID" runat="server" OnSelectedIndexChanged="PayPeriodID_SelectedIndexChanged"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Panel ID="PayPeriodEditPanel" runat="server" Wrap="False" Width="100px">
                                        <asp:Button ID="PayPeriodEdit" runat="server"  Text="Edit" CssClass="button" OnClick="PayPeriodEdit_Click"/>
                                        <asp:Button ID="PayPeriodNew" runat="server" EnableViewState="false" Text="New" CssClass="button" OnClick="PayPeriodNew_Click" />
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <uc1:Payroll_PeriodInfo ID="ucPayroll_PeriodInfo" runat="server" />
                    </td>
                </tr>
            </table>
          <%--  </ContentTemplate >        
            <ContentTemplate >      --%>  
            <br/>
            <br/>           

            <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
                <col width="26px" />
                <col width="170px" />

                <tr>
                    <td class="pm_field_header" colspan="2">
                        <asp:Label runat="server" EnableViewState="false" Text="Payroll Group Visibility" />:
                    </td>
                    <td class="pm_field">
                        <table border="0">
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="UsersEdit" runat="server"  Text="Edit" CssClass="button" OnClick="UsersEdit_Click"/> Setting no authorized users means it is public to all users
<%--                                    <asp:Panel runat="server" Wrap="False" Width="100px">
                                        <asp:Button ID="UsersEdit" runat="server"  Text="Edit" CssClass="button" OnClick="UsersEdit_Click"/>
                                    </asp:Panel>                                
--%>                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                
                <tr>
                    <td class="pm_field_title" colspan="3">
                        <asp:Label Text="Payroll Group Visibility" runat="server" />
                    </td>
                </tr> 
                
                <asp:Repeater ID="SecurityRepeater" runat="server" OnItemDataBound="SecurityRepeater_ItemDataBound">
                    <ItemTemplate>
                        <tr >
                            <td class="pm_list" align="center">
                                <asp:CheckBox ID="UserSelect" runat="server"  Enabled="false" Checked="true"/>
                            </td>
                            <td class="pm_list" colspan="3">
                                <asp:Label ID="UserName" runat="server" />
<%--                                <%#sbinding.getValue(Container.DataItem,"UserName")%>
--%>                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </ContentTemplate >        
         
        </asp:UpdatePanel> 
</asp:Content> 