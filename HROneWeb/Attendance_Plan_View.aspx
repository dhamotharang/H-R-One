<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Attendance_Plan_View.aspx.cs" Inherits="Attendance_Plan_View" MasterPageFile="~/MainMasterPage.master"  %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/AttendancePlan_AdditionalPayment.ascx" TagName="AttendancePlan_AdditionalPayment" TagPrefix="uc" %>



<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="AttendancePlanID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="Label26" Text="Attendance Plan Setup" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="Label42" Text="View" runat="server" />
                    <asp:Label Text="Attendance Plan Setup" runat="server" />:
                    <%=AttendancePlanCode.Text %>
                </td>
            </tr>
        </table>
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar id="toolBar" runat="server"
                     NewButton_Visible="false" 
                     SaveButton_Visible="false"
                     EditButton_Visible="true" 
                     OnBackButton_Click="Back_Click"
                     OnEditButton_Click="Edit_Click"
                     OnDeleteButton_Click="Delete_Click"
                      />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openAlert(); return false;"/>
                </td>
            </tr>
        </table>
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="15%" />
            <col width="35%" />
            <col width="15%" />
            <col width="35%" />
    		<tr>
                <td class="pm_field_title"  colspan="4">
                   <asp:Label runat="server" Text="Attendance Plan" /></td>
			</tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label Text="Plan Code" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="AttendancePlanCode" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label Text="Description" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="AttendancePlanDesc" runat="Server" /></td>
            </tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label29" Text="Payroll Prorata Formula for Absent" runat="server" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="AttendancePlanAbsentProrataPayFormID" runat="server" /></td>
            </tr>
			<tr>
                <td class="pm_field_title" colspan="4">
                    <asp:Label ID="Label9" runat="server" Text="Overtime" /></td>
			</tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label38" runat="server" Text="Gain as Compensation Leave Entitlement" />:
                </td>
                <td class="pm_field" colspan="3">
                   <asp:Label ID="AttendancePlanOTGainAsCompensationLeaveEntitle" runat="server" />
                </td>
            </tr>
			<tr runat="server" id="OTFormulaRow">
                <td class="pm_field_header">
                    <asp:Label ID="Label5" runat="server" Text="Overtime Formula" />:
                </td>
                <td class="pm_field" colspan="3">
                   <asp:Label ID="AttendancePlanOTFormula" runat="server" /> 
                </td>
            </tr>
			<tr runat="server" id="OTRatioRow">
                <td class="pm_field_header">
                    <asp:Label ID="Label36" runat="server" Text="Overtime Ratio" />:
                </td>
                <td class="pm_field" >
                   <asp:Label ID="AttendancePlanOTRateMultiplier" runat="server" Text="1" />   
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label24" runat="server" Text="Include Meal Break Overtime" />?</td>
                <td class="pm_field">
                     <asp:Label ID="AttendancePlanOTIncludeLunchOvertime" runat="server" />   
                </td>
            </tr>
            <tr runat="server" id="OTPaymentCodeRow">
                <td class="pm_field_header">
                    <asp:Label ID="Label2" runat="server" Text="Payment Code" />:</td>
                <td class="pm_field" colspan="3">
                     <asp:Label ID="AttendancePlanOTPayCodeID" runat="server" />   
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label1" runat="server" Text="Unit" />:
                </td>
                <td class="pm_field" colspan="3">
                     <asp:Label ID="AttendancePlanOTMinsRoundingRule" runat="server" />   
                     <asp:Label ID="AttendancePlanOTMinsUnit" runat="server" />   
                     <asp:Label ID="Label4" Text="mins" runat="server" />   
                </td>
			</tr>
			<tr>
                <td class="pm_field_title" colspan="4">
                    <asp:Label ID="Label3" runat="server" Text="Late" />
                </td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label6" runat="server" Text="Late Formula" />:</td>
                <td class="pm_field" colspan="3">
                     <asp:Label ID="AttendancePlanLateFormula" runat="server" />   
                </td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label10" runat="server" Text="Payment Code" />:</td>
                <td class="pm_field" colspan="3">
                     <asp:Label ID="AttendancePlanLatePayCodeID" runat="server" />   
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label7" runat="server" Text="Unit" />:</td>
                <td class="pm_field" colspan="3">
                     <asp:Label ID="AttendancePlanLateMinsRoundingRule" runat="server" />   
                     <asp:Label ID="AttendancePlanLateMinsUnit" runat="server" />   
                     <asp:Label ID="Label8" Text="mins" runat="server" />   
                </td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label28" runat="server" Text="Include Early Leave" />?</td>
                <td class="pm_field">
                     <asp:Label ID="AttendancePlanLateIncludeEarlyLeave" runat="server" />   
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label44" runat="server" Text="Include Meal Break Late" />?</td>
                <td class="pm_field">
                     <asp:Label ID="AttendancePlanLateIncludeLunchLate" runat="server" />   
                </td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label46" runat="server" Text="Include Meal Break Early Leave" />?</td>
                <td class="pm_field">
                     <asp:Label ID="AttendancePlanLateIncludeLunchEarlyLeave" runat="server" />   
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label27" runat="server" Text="Compensated late/early leave by Overtime" />?</td>
                <td class="pm_field">
                     <asp:Label ID="AttendancePlanCompensateLateByOT" runat="server" />   
                    <asp:Label ID="Label32" runat="server" Text="(OT Rate and Override Hourly Rate will not take effect)" />
               </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label25" runat="server" Text="Max Total Late Tolerance" />:
                </td>
                <td class="pm_field" colspan="3">
                     <asp:Label ID="AttendancePlanLateMaxTotalToleranceMins" runat="server" />   
                     <asp:Label ID="Label48" Text="mins" runat="server" />   
                </td>
            </tr>			<tr>
                <td class="pm_field_title" colspan="4">
                    <asp:Label ID="Label11" runat="server" Text="Bonus Condition" />
                </td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label12" runat="server" Text="Max Late Count" />:
                </td>
                <td class="pm_field">
                     <asp:Label ID="AttendancePlanBonusMaxTotalLateCount" runat="server" />   
                     <asp:Label ID="AttendancePlanBonusMaxTotalLateCountIncludeLunch" runat="server" Text="Included Meal Break Late"/>   
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label13" runat="server" Text="Max Late (mins)" />:
                </td>
                <td class="pm_field">
                     <asp:Label ID="AttendancePlanBonusMaxTotalLateMins" runat="server" />   
                     <asp:Label ID="AttendancePlanBonusMaxTotalLateMinsIncludeLunch" runat="server" Text="Included Meal Break Late"/>   
                </td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label14" runat="server" Text="Max Early Leave Count" />:
                </td>
                <td class="pm_field">
                     <asp:Label ID="AttendancePlanBonusMaxTotalEarlyLeaveCount" runat="server" />   
                     <asp:Label ID="AttendancePlanBonusMaxTotalEarlyLeaveCountIncludeLunch" runat="server" Text="Included Meal Break Early Leave"/>   
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label15" runat="server" Text="Max Early Leave (mins)" />:
                </td>
                <td class="pm_field">
                     <asp:Label ID="AttendancePlanBonusMaxTotalEarlyLeaveMins" runat="server" />   
                     <asp:Label ID="AttendancePlanBonusMaxTotalEarlyLeaveMinsIncludeLunch" runat="server" Text="Included Meal Break Early Leave"/>   
                </td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label16" runat="server" Text="Max Sick Leave taken with Relevant Certificate" />:</td>
                <td class="pm_field">
                     <asp:Label ID="AttendancePlanBonusMaxTotalSLWithMedicalCertificate" runat="server" />   
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label19" runat="server" Text="Max Sick Leave taken without Relevant Certificate" />:</td>
                <td class="pm_field">
                     <asp:Label ID="AttendancePlanBonusMaxTotalSLWithoutMedicalCertificate" runat="server" />   
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label17" runat="server" Text="Max No Pay Leave" />:</td>
                <td class="pm_field">
                     <asp:Label ID="AttendancePlanBonusMaxTotalNonFullPayCasualLeave" runat="server" />   
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label18" runat="server" Text="Max Injury Leave Taken" />:</td>
                <td class="pm_field">
                     <asp:Label ID="AttendancePlanBonusMaxTotalInjuryLeave" runat="server" />   
                </td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label20" runat="server" Text="Max Absent" />:</td>
                <td class="pm_field">
                     <asp:Label ID="AttendancePlanBonusMaxTotalAbsentCount" runat="server" />   
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label31" runat="server" Text="Entitle bonus for terminated employee worked with incompleted month" />:</td>
                <td class="pm_field" colspan="3">
                     <asp:Label ID="AttendancePlanTerminatedHasBonus" runat="server" />   
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label21" runat="server" Text="Payment Code for Bonus Payment" />:</td>
                <td class="pm_field" colspan="3">
                     <asp:Label ID="AttendancePlanBonusPayCodeID" runat="server" />   
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label22" runat="server" Text="Bonus Amount" />:</td>
                <td class="pm_field">
                     <asp:Label ID="AttendancePlanBonusAmount" runat="server" /><br />
                     <asp:Label ID="AttendancePlanUseBonusAmountByRecurringPayment" runat="server" Text="Override Amount from Recurring Payment" />
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label23" runat="server" Text="Unit" />:</td>
                <td class="pm_field">
                     <asp:Label ID="AttendancePlanBonusAmountUnit" runat="server" /><br />
                     <asp:Label ID="AttendancePlanProrataBonusforNewJoin" runat="server" Text="Prorata for New Join Employees" /><br />
                     <asp:Label ID="AttendancePlanProrataBonusforTerminated" runat="server" Text="Prorata for Terminated Employees" />
                </td>
            </tr>
            <tr id="OTBonusPanel" runat="server" >
                <td class="pm_field_header">
                    <asp:Label ID="Label30" runat="server" Text="Bonus Amount for Overtime (per hour)" />:</td>
                <td class="pm_field">
                     <asp:Label ID="AttendancePlanBonusOTAmount" runat="server" />   
                </td>
            </tr>
			<tr>
                <td class="pm_field_title" colspan="4">
                    <asp:Label ID="Label40" runat="server" Text="Partial Bonus Condition" />
                </td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label33" runat="server" Text="Max Late Count" />:
                </td>
                <td class="pm_field">
                     <asp:Label ID="AttendancePlanBonusPartialPaidMaxTotalLateCount" runat="server" />
                     <asp:Label ID="AttendancePlanBonusPartialPaidMaxTotalLateCountIncludeLunch" runat="server" Text="Included Meal Break Late"/>   
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label35" runat="server" Text="Max Late (mins)" />:</td>
                <td class="pm_field">
                     <asp:Label ID="AttendancePlanBonusPartialPaidMaxTotalLateMins" runat="server" />   
                     <asp:Label ID="AttendancePlanBonusPartialPaidMaxTotalLateMinsIncludeLunch" runat="server" Text="Included Meal Break Late"/>   
                </td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label37" runat="server" Text="Max Early Leave Count" />:
                </td>
                <td class="pm_field">
                     <asp:Label ID="AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveCount" runat="server" />   
                     <asp:Label ID="AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveCountIncludeLunch" runat="server" Text="Included Meal Break Early Leave"/>
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label39" runat="server" Text="Max Early Leave (mins)" />:
                </td>
                <td class="pm_field">
                     <asp:Label ID="AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveMins" runat="server" />   
                     <asp:Label ID="AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveMinsIncludeLunch" runat="server" Text="Included Meal Break Early Leave"/>
                </td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label41" runat="server" Text="Max Sick Leave taken with Relevant Certificate" />:</td>
                <td class="pm_field">
                     <asp:Label ID="AttendancePlanBonusPartialPaidMaxTotalSLWithMedicalCertificate" runat="server" />   
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label43" runat="server" Text="Max Sick Leave taken without Relevant Certificate" />:</td>
                <td class="pm_field">
                     <asp:Label ID="AttendancePlanBonusPartialPaidMaxTotalSLWithoutMedicalCertificate" runat="server" />   
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label45" runat="server" Text="Max No Pay Leave" />:</td>
                <td class="pm_field">
                     <asp:Label ID="AttendancePlanBonusPartialPaidMaxTotalNonFullPayCasualLeave" runat="server" />   
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label47" runat="server" Text="Max Injury Leave Taken" />:</td>
                <td class="pm_field">
                     <asp:Label ID="AttendancePlanBonusPartialPaidMaxTotalInjuryLeave" runat="server" />   
                </td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label49" runat="server" Text="Max Absent" />:</td>
                <td class="pm_field">
                     <asp:Label ID="AttendancePlanBonusPartialPaidMaxTotalAbsentCount" runat="server" />   
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label34" runat="server" Text="Percentage of Bonus" />:</td>
                <td class="pm_field">
                     <asp:Label ID="AttendancePlanBonusPartialPaidPercent" runat="server" />%   
                </td>
            </tr>
		</table>
        <br />
        <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
        <ContentTemplate >
            <uc:AttendancePlan_AdditionalPayment ID="AttendancePlan_AdditionalPayment1" runat="server" />
        </ContentTemplate>
        </asp:UpdatePanel >
</asp:Content> 