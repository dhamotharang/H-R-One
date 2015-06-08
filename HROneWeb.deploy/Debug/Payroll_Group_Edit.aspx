<%@ page language="C#" autoeventwireup="true" inherits="Payroll_Group_Edit, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="PayGroupID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label runat="server" EnableViewState="false" Text="Payroll Group Setup" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <%=CurID<=0?HROne.Common.WebUtility.GetLocalizedString("Add"):HROne.Common.WebUtility.GetLocalizedString("Edit") %>
                    <asp:Label runat="server" EnableViewState="false" Text="Payroll Group" />:
                    <%=PayGroupCode.Text%>
                </td>
            </tr>
        </table>
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
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
        <ContentTemplate>
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="15%"/>
            <col width="35%"/>
            <col width="15%"/>
            <col width="35%"/>
            <tr>
                <td class="pm_field_title" colspan="4">
                    <asp:Label ID="Label15" runat="server" EnableViewState="false" Text="Payroll Group Information" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label runat="server" EnableViewState="false" Text="Code" />:
                </td>
                <td class="pm_field" >
                    <asp:TextBox ID="PayGroupCode" runat="Server" />
                </td>
                <td class="pm_field_header" >
                    <asp:Label runat="server" EnableViewState="false" Text="Description" />:
                </td>
                <td class="pm_field" >
                    <asp:TextBox ID="PayGroupDesc" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label runat="server" EnableViewState="false" Text="Frequency" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="PayGroupFreq" runat="Server" AutoPostBack="true" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="lblPayGroupDefaultStartDay" runat="server" EnableViewState="false" Text="First Period Start Day" />:
                </td>
                <td class="pm_field" >
                    <asp:TextBox ID="PayGroupDefaultStartDay" runat="Server" />
                </td>
                <td class="pm_field_header" >
                    <asp:Label ID="lblPayGroupLeaveDefaultCutOffDay" runat="server" EnableViewState="false" Text="First Period Leave Cut Off Day" />:
                </td>
                <td class="pm_field" >
                    <asp:TextBox ID="PayGroupLeaveDefaultCutOffDay" runat="Server" />
                </td>
            </tr>
            <tr id="SemiMonthlyDayOptionsRow" runat="server" >
                <td class="pm_field_header" >
                    <asp:Label ID="lblPayGroupDefaultNextStartDay" runat="server" EnableViewState="false" Text="Second Period Start Day" />:
                </td>
                <td class="pm_field" >
                    <asp:TextBox ID="PayGroupDefaultNextStartDay" runat="Server" CssClass="pm_required" />
                </td>
                <td class="pm_field_header" >
                    <asp:Label ID="lblPayGroupLeaveDefaultNextCutOffDay" runat="server" EnableViewState="false" Text="Second Period Leave Cut Off Day" />:
                </td>
                <td class="pm_field" >
                    <asp:TextBox ID="PayGroupLeaveDefaultNextCutOffDay" runat="Server" CssClass="pm_required"/>
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label25" runat="server" EnableViewState="false" Text="Rest day with pay" />:
                </td>
                <td class="pm_field" >
                    <asp:CheckBox ID="PayGroupRestDayHasWage" runat="Server" />
                </td>
                <td class="pm_field_header" >
                    <asp:Label ID="Label30" runat="server" EnableViewState="false" Text="Meal break with pay" />:
                </td>
                <td class="pm_field" >
                    <asp:CheckBox ID="PayGroupLunchTimeHasWage" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label27" runat="server" EnableViewState="false" Text="Payment Code for Additional Remuneration (for Statutory Minimum Wage)" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="PayGroupAdditionalRemunerationPayCodeID" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_title" colspan="4">
                    <asp:Label ID="Label39" Text="Prorata Formula Options" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label24" runat="server" EnableViewState="false" Text="Default Formula" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="PayGroupDefaultProrataFormula" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label26" runat="server" EnableViewState="false" Text="Rest Day" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="PayGroupRestDayProrataFormula" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label runat="server" EnableViewState="false" Text="New Join" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="PayGroupNewJoinFormula" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label38" runat="server" EnableViewState="false" Text="Change of Payroll Group or Recurring Payment" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="PayGroupExistingFormula" runat="Server" />
                </td>
            </tr>
            <tr id="PayGroupIsCNDProrataRow" runat="server" >
                <td class="pm_field_header" >
                    <asp:Label ID="Label32" runat="server" EnableViewState="false" Text="Prorata on Claims and Deductions" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:CheckBox ID="PayGroupIsCNDProrata" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_title" colspan="4">
                    <asp:Label ID="Label16" Text="Statutory Holiday Options" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label37" runat="server" EnableViewState="false" Text="Skip Statutory Holiday Calculation" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:CheckBox ID="PayGroupIsSkipStatHol" runat="Server" AutoPostBack="true"  />
                </td>
            </tr>
            <asp:PlaceHolder ID="StatutoryHolidayOptionSection" runat="server" >
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="lblPayGroupStatHolDeductFormula" runat="server" EnableViewState="false" Text="Statutory Holiday Deduction Formula" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="PayGroupStatHolDeductFormula" runat="Server" CssClass="pm_required" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="lblPayGroupStatHolDeductPaymentCodeID" runat="server" EnableViewState="false" Text="Payment Code for Statutory Holiday Deduction" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="PayGroupStatHolDeductPaymentCodeID" runat="Server" CssClass="pm_required"/></td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="lblPayGroupStatHolAllowFormula" runat="server" EnableViewState="false" Text="Statutory Holiday Allowance Formula" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="PayGroupStatHolAllowFormula" runat="Server" CssClass="pm_required"/>
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="lblPayGroupStatHolAllowPaymentCodeID" runat="server" EnableViewState="false" Text="Payment Code for Statutory Holiday Allowance" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="PayGroupStatHolAllowPaymentCodeID" runat="Server"  CssClass="pm_required"/>
                </td>
            </tr>
            <tr id="PayGroupStatHolNextMonthRow" runat="server" >
                <td class="pm_field_header">
                    <asp:Label ID="Label20" runat="server" EnableViewState="false" Text="Calculate Statutory Holiday on next month" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:CheckBox ID="PayGroupStatHolNextMonth" runat="Server" />
                </td>
            </tr>
            <tr >
                <td class="pm_field_header">
                    <asp:Label ID="Label17" runat="server" EnableViewState="false" Text="Use Public Holiday Table instead of Statutory Holiday Table" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:CheckBox ID="PayGroupIsStatHolUsePublicHoliday" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label21" runat="server" EnableViewState="false" Text="Statutory Holiday Payment Eligible After" /> <br /> (<asp:Label ID="Label22" runat="server" EnableViewState="false" Text="Start Date of Service" />):
                </td>
                <td class="pm_field" colspan="3">
                    <asp:TextBox ID="PayGroupStatHolEligiblePeriod" runat="server" Columns="1" MaxLength="2" />
                    <asp:DropDownList ID="PayGroupStatHolEligibleUnit" runat="server" /><br />
                    <asp:CheckBox ID="PayGroupStatHolEligibleAfterProbation" runat="server" EnableViewState="false" Text="Eligible After Probation"/><br />
                    <asp:CheckBox ID="PayGroupStatHolEligibleSkipDeduction" runat="server" EnableViewState="false" Text="Skip Deduction"/>
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
                    <asp:Label runat="server" EnableViewState="false" Text="Final Payment Prorata Formula" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="PayGroupTerminatedFormula" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label3" runat="server" EnableViewState="false" Text="Daily Formula for AL Compensation" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="PayGroupTerminatedALCompensationDailyFormula" runat="Server" /><br />
                    <asp:Label runat="server" EnableViewState="false" Text="second formula for comparsion" />:&nbsp<asp:DropDownList ID="PayGroupTerminatedALCompensationDailyFormulaAlternative" runat="Server" /><br />
                    <asp:CheckBox ID="PayGroupTerminatedALCompensationIsSkipRoundingRule" runat="server" EnableViewState="false" Text="Do NOT apply rounding rule while calculating balance of Annual Leave" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label4" runat="server" EnableViewState="false" Text="Payment Code for AL Compensation by Employer" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="PayGroupTerminatedALCompensationPaymentCodeID" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label31" runat="server" EnableViewState="false" Text="Payment Code for AL Compensation by Employee" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="PayGroupTerminatedALCompensationByEEPaymentCodeID" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label23" runat="server" EnableViewState="false" Text="Minimum period of employment for AL Compensation" /> :
                </td>
                <td class="pm_field" colspan="3">
                    <asp:TextBox ID="PayGroupTerminatedALCompensationProrataEligiblePeriod" runat="server" MaxLength="2" />
                    <asp:DropDownList ID="PayGroupTerminatedALCompensationProrataEligibleUnit" runat="server" />
                    <asp:CheckBox ID="PayGroupTerminatedALCompensationEligibleAfterProbation" runat="server" EnableViewState="false" Text="Eligible After Probation"/><br />
                    <asp:CheckBox ID="PayGroupTerminatedALCompensationProrataEligibleCheckEveryLeaveYear" runat="server" EnableViewState="false" Text="Check every leave year for prorata of Annual Leave"/>
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label5" runat="server" EnableViewState="false" Text="Monthly payment formula for Payment In Lieu" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="PayGroupTerminatedPaymentInLieuMonthlyBaseMethod" runat="Server" /><br />
                    <asp:Label ID="Label18" runat="server" EnableViewState="false" Text="second formula for comparsion" />:&nbsp<asp:DropDownList ID="PayGroupTerminatedPaymentInLieuMonthlyBaseMethodAlternative" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label6" runat="server" EnableViewState="false" Text="Daily payment formula for Payment In Lieu" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="PayGroupTerminatedPaymentInLieuDailyFormula" runat="Server" /><br />
                    <asp:Label ID="Label19" runat="server" EnableViewState="false" Text="second formula for comparsion" />:&nbsp<asp:DropDownList ID="PayGroupTerminatedPaymentInLieuDailyFormulaAlternative" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label7" runat="server" EnableViewState="false" Text="Payment Code for Payment In Lieu (Employer)" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="PayGroupTerminatedPaymentInLieuERPaymentCodeID" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label8" runat="server" EnableViewState="false" Text="Payment Code for Payment In Lieu (Employee)" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="PayGroupTerminatedPaymentInLieuEEPaymentCodeID" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label9" runat="server" EnableViewState="false" Text="Monthly payment formula for Long Service Payment / Severance Payment" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="PayGroupTerminatedLSPSPMonthlyBaseMethod" runat="Server" /><br />
                    <asp:Label ID="Label40" runat="server" EnableViewState="false" Text="second formula for comparsion" />:&nbsp<asp:DropDownList ID="PayGroupTerminatedLSPSPMonthlyBaseMethodAlternative" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label10" runat="server" EnableViewState="false" Text="Payment code for Long Service Payment" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="PayGroupTerminatedLSPPaymentCodeID" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label11" runat="server" EnableViewState="false" Text="Payment code for Severance Payment" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="PayGroupTerminatedSPPaymentCodeID" runat="Server" />
                </td>
            </tr>
            <asp:PlaceHolder ID="FinalPaymentHolidayPanel" runat="server" >
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label33" runat="server" EnableViewState="false" Text="Daily Formula for Rest Day Compensation" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="PayGroupTerminatedRestDayCompensationDailyFormula" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label34" runat="server" EnableViewState="false" Text="Payment Code for Rest Day Compensation" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="PayGroupTerminatedRestDayCompensationPaymentCodeID" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label35" runat="server" EnableViewState="false" Text="Daily Formula for Statutory Holiday Compensation" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="PayGroupTerminatedStatutoryHolidayCompensationDailyFormula" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label36" runat="server" EnableViewState="false" Text="Payment Code for Statutory Holiday Compensation" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="PayGroupTerminatedStatutoryHolidayCompensationPaymentCodeID" runat="Server" />
                </td>
            </tr>
            </asp:PlaceHolder>
            <tr>
                <td class="pm_field_title" colspan="4">
                    <asp:Label ID="Label13" Text="Miscellaneous" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" EnableViewState="false" Text="Default Next Period From" />:
                </td>
                <td class="pm_field" >
                    <uc1:WebDatePicker ID="PayGroupNextStartDate" runat="server" />
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="lblPayGroupNextEndDate" runat="server" EnableViewState="false" Text="Default Next Period To" />:
                </td>
                <td class="pm_field" >
                    <uc1:WebDatePicker ID="PayGroupNextEndDate" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" EnableViewState="false" Text="Payment Comparison" />:<br />
                    (<asp:Label ID="Label29" runat="server" EnableViewState="false" Text="Applicable only if deduction formula and allowance formula is different" />)
                </td>
                <td class="pm_field">
                    <asp:CheckBox ID="PayGroupPayAdvance" runat="server" EnableViewState="false" Text="Comparison between formula"/><br  />
                    <asp:CheckBox ID="PayGroupPayAdvanceCompareTotalPaymentOnly" runat="server" EnableViewState="false" Text="Comparison between result"/>
                </td>
                <asp:PlaceHolder ID="PayGroupUseCNDForDailyHourlyPaymentSection" runat="server" >
                <td class="pm_field_header">
                    <asp:Label ID="Label28" runat="server" EnableViewState="false" Text="Use Claims and Deduction for Daily/Hourly Recurring Payment" />:
                </td>
                <td class="pm_field" >
                    <asp:CheckBox ID="PayGroupUseCNDForDailyHourlyPayment" runat="server" />
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
                    <asp:Label ID="Label1" runat="server" EnableViewState="false" Text="Process Month" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="PayGroupYEBStartPayrollMonth" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label2" runat="server" EnableViewState="false" Text="Eligible Period" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="PayGroupYEBMonthFrom" runat="server" />
                    <asp:Label ID="lblTo" runat="server" EnableViewState="false" Text="To" />
                    <asp:DropDownList ID="PayGroupYEBMonthTo" runat="server" />
                </td>
            </tr>
            <tr >
                <td class="pm_field_title" colspan="4">
                    <asp:Label ID="Label14" Text="Leave Calculation Override" runat="server" />
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
                    <asp:DropDownList ID="PayrollGroupLeaveCodeSetupLeaveDeductFormula" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="lblPayrollGroupLeaveCodeSetupLeaveDeductPaymentCodeID" runat="server" EnableViewState="false" Text="Payment Code for Deduction" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="PayrollGroupLeaveCodeSetupLeaveDeductPaymentCodeID" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="lblPayrollGroupLeaveCodeSetupLeaveAllowFormula" runat="server" EnableViewState="false" Text="Allowance Formula" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="PayrollGroupLeaveCodeSetupLeaveAllowFormula" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="lblPayrollGroupLeaveCodeSetupLeaveAllowPaymentCodeID" runat="server" EnableViewState="false" Text="Payment Code for Allowance" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="PayrollGroupLeaveCodeSetupLeaveAllowPaymentCodeID" runat="Server" />
                </td>
            </tr>
            </ItemTemplate>
            </asp:Repeater>
        </table>
        </ContentTemplate>
        </asp:UpdatePanel >
</asp:Content> 