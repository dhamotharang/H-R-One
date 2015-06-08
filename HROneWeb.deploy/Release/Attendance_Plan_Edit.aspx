<%@ page language="C#" autoeventwireup="true" inherits="Attendance_Plan_Edit, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>



<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="AttendancePlanID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="Label26" Text="Attendance Plan Setup" runat="server" />
                </td>
            </tr>
        </table>        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="ActionHeader" runat="server" Text="Edit" />
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
                     EditButton_Visible="false" 
                     OnBackButton_Click="Back_Click"
                     OnSaveButton_Click ="Save_Click"
                     OnDeleteButton_Click="Delete_Click"
                      />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openAlert(); return false;"/>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
        <ContentTemplate >
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
                    <asp:TextBox ID="AttendancePlanCode" runat="Server" Columns="20" MaxLength="20" /></td>
                <td class="pm_field_header">
                    <asp:Label Text="Description" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="AttendancePlanDesc" runat="Server" Columns="32" MaxLength="70" /></td>
            </tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label29" Text="Payroll Prorata Formula for Absent" runat="server" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="AttendancePlanAbsentProrataPayFormID" runat="server" /></td>
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
                   <asp:CheckBox ID="AttendancePlanOTGainAsCompensationLeaveEntitle" runat="server" AutoPostBack="true" OnCheckedChanged="AttendancePlanOTGainAsCompensationLeaveEntitle_CheckedChanged" />
                </td>
            </tr>
			<tr runat="server" id="OTFormulaRow">
                <td class="pm_field_header">
                    <asp:Label ID="Label5" runat="server" Text="Overtime Formula" />:
                </td>
                <td class="pm_field" colspan="3">
                   <asp:DropDownList ID="AttendancePlanOTFormula" runat="server" />
                </td>
            </tr>
			<tr runat="server" id="OTRatioRow">
                <td class="pm_field_header">
                    <asp:Label ID="Label36" runat="server" Text="Overtime Ratio" />:
                </td>
                <td class="pm_field" >
                   <asp:TextBox ID="AttendancePlanOTRateMultiplier" runat="server" Text="1" />   
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label24" runat="server" Text="Include Meal Break Overtime" />?</td>
                <td class="pm_field">
                     <asp:CheckBox ID="AttendancePlanOTIncludeLunchOvertime" runat="server" />   
                </td>
            </tr>
            <tr runat="server" id="OTPaymentCodeRow">
                <td class="pm_field_header">
                    <asp:Label ID="Label2" runat="server" Text="Payment Code" />:</td>
                <td class="pm_field" colspan="3">
                     <asp:DropDownList ID="AttendancePlanOTPayCodeID" runat="server" />   
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label1" runat="server" Text="Unit" />:
                </td>
                <td class="pm_field" colspan="3">
                     <asp:DropDownList ID="AttendancePlanOTMinsRoundingRule" runat="server" />   
                     <asp:TextBox ID="AttendancePlanOTMinsUnit" runat="server" />   
                     <asp:Label ID="Label4" Text="mins" runat="server" />   
                </td>
            </tr>
			<tr>
                <td class="pm_field_title" colspan="4">
                    <asp:Label ID="Label3" runat="server" Text="Late" /></td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label6" runat="server" Text="Late Formula" />:</td>
                <td class="pm_field" colspan="3">
                     <asp:DropDownList ID="AttendancePlanLateFormula" runat="server" />   
                </td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label10" runat="server" Text="Payment Code" />:</td>
                <td class="pm_field" colspan="3">
                     <asp:DropDownList ID="AttendancePlanLatePayCodeID" runat="server" />   
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label7" runat="server" Text="Unit" />:
                </td>
                <td class="pm_field" colspan="3">
                     <asp:DropDownList ID="AttendancePlanLateMinsRoundingRule" runat="server" />   
                     <asp:TextBox ID="AttendancePlanLateMinsUnit" runat="server" />   
                     <asp:Label ID="Label8" Text="mins" runat="server" />   
                </td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label28" runat="server" Text="Include Early Leave" />?</td>
                <td class="pm_field">
                     <asp:CheckBox ID="AttendancePlanLateIncludeEarlyLeave" runat="server" />   
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label42" runat="server" Text="Include Meal Break Late" />?</td>
                <td class="pm_field">
                     <asp:CheckBox ID="AttendancePlanLateIncludeLunchLate" runat="server" />   
                </td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label44" runat="server" Text="Include Meal Break Early Leave" />?</td>
                <td class="pm_field">
                     <asp:CheckBox ID="AttendancePlanLateIncludeLunchEarlyLeave" runat="server" />   
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label27" runat="server" Text="Compensated late/early leave by Overtime" />?</td>
                <td class="pm_field">
                     <asp:CheckBox ID="AttendancePlanCompensateLateByOT" runat="server" />   
                    <asp:Label ID="Label32" runat="server" Text="(OT Rate and Override Hourly Rate will not take effect)" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label25" runat="server" Text="Max Total Late Tolerance" />:
                </td>
                <td class="pm_field" colspan="3">
                     <asp:TextBox ID="AttendancePlanLateMaxTotalToleranceMins" runat="server" />   
                     <asp:Label ID="Label46" Text="mins" runat="server" />   
                </td>
            </tr>
			<tr>
                <td class="pm_field_title" colspan="4">
                    <asp:Label ID="Label11" runat="server" Text="Bonus Condition" />
                </td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label12" runat="server" Text="Max Late Count" />:
                </td>
                <td class="pm_field">
                     <asp:TextBox ID="AttendancePlanBonusMaxTotalLateCount" runat="server" />   
                     <asp:CheckBox ID="AttendancePlanBonusMaxTotalLateCountIncludeLunch" runat="server" Text="Included Meal Break Late"/>   
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label13" runat="server" Text="Max Late (mins)" />:
                </td>
                <td class="pm_field">
                     <asp:TextBox ID="AttendancePlanBonusMaxTotalLateMins" runat="server" />   
                     <asp:CheckBox ID="AttendancePlanBonusMaxTotalLateMinsIncludeLunch" runat="server" Text="Included Meal Break Late"/>   
                </td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label14" runat="server" Text="Max Early Leave Count" />:
                </td>
                <td class="pm_field">
                     <asp:TextBox ID="AttendancePlanBonusMaxTotalEarlyLeaveCount" runat="server" />   
                     <asp:CheckBox ID="AttendancePlanBonusMaxTotalEarlyLeaveCountIncludeLunch" runat="server" Text="Included Meal Break Early Leave"/>   
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label15" runat="server" Text="Max Early Leave (mins)" />:</td>
                <td class="pm_field">
                     <asp:TextBox ID="AttendancePlanBonusMaxTotalEarlyLeaveMins" runat="server" />   
                     <asp:CheckBox ID="AttendancePlanBonusMaxTotalEarlyLeaveMinsIncludeLunch" runat="server" Text="Included Meal Break Early Leave"/>   
                </td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label16" runat="server" Text="Max Sick Leave taken with Relevant Certificate" />:</td>
                <td class="pm_field">
                     <asp:TextBox ID="AttendancePlanBonusMaxTotalSLWithMedicalCertificate" runat="server" />   
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label19" runat="server" Text="Max Sick Leave taken without Relevant Certificate" />:</td>
                <td class="pm_field">
                     <asp:TextBox ID="AttendancePlanBonusMaxTotalSLWithoutMedicalCertificate" runat="server" />   
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label17" runat="server" Text="Max No Pay Leave" />:</td>
                <td class="pm_field">
                     <asp:TextBox ID="AttendancePlanBonusMaxTotalNonFullPayCasualLeave" runat="server" />   
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label18" runat="server" Text="Max Injury Leave Taken" />:</td>
                <td class="pm_field">
                     <asp:TextBox ID="AttendancePlanBonusMaxTotalInjuryLeave" runat="server" />   
                </td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label20" runat="server" Text="Max Absent" />:</td>
                <td class="pm_field">
                     <asp:TextBox ID="AttendancePlanBonusMaxTotalAbsentCount" runat="server" />   
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label31" runat="server" Text="Entitle bonus for terminated employee worked with incompleted month" />:</td>
                <td class="pm_field" colspan="3">
                     <asp:CheckBox ID="AttendancePlanTerminatedHasBonus" runat="server" />   
                </td>
             </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label21" runat="server" Text="Payment Code" />:</td>
                <td class="pm_field" colspan="3">
                     <asp:DropDownList ID="AttendancePlanBonusPayCodeID" runat="server" />   
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label22" runat="server" Text="Bonus Amount" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="AttendancePlanBonusAmount" runat="server" /><br />
                    <asp:CheckBox ID="AttendancePlanUseBonusAmountByRecurringPayment" runat="server" Text="Override Amount from Recurring Payment" />
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label23" runat="server" Text="Unit" />:</td>
                <td class="pm_field">
                     <asp:DropDownList ID="AttendancePlanBonusAmountUnit" runat="server" OnSelectedIndexChanged="AttendancePlanBonusAmountUnit_SelectedIndexChanged" AutoPostBack="true"  /><br />   
                     <asp:CheckBox ID="AttendancePlanProrataBonusforNewJoin" runat="server" Text="Prorata for New Join Employees" /><br />
                     <asp:CheckBox ID="AttendancePlanProrataBonusforTerminated" runat="server" Text="Prorata for Terminated Employees" />
                </td>
            </tr>
            <tr id="OTBonusPanel" runat="server" >
                <td class="pm_field_header">
                    <asp:Label ID="Label30" runat="server" Text="Bonus Amount for Overtime (per hour)" />:</td>
                <td class="pm_field">
                     <asp:TextBox ID="AttendancePlanBonusOTAmount" runat="server" />   
                </td>
            </tr>
			<tr>
                <td class="pm_field_title" colspan="4">
                    <asp:Label ID="Label40" runat="server" Text="Partial Bonus Condition" /></td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label33" runat="server" Text="Max Late Count" />:
                </td>
                <td class="pm_field">
                     <asp:TextBox ID="AttendancePlanBonusPartialPaidMaxTotalLateCount" runat="server" />   
                     <asp:CheckBox ID="AttendancePlanBonusPartialPaidMaxTotalLateCountIncludeLunch" runat="server" Text="Included Meal Break Late"/>   
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label35" runat="server" Text="Max Late (mins)" />:
                </td>
                <td class="pm_field">
                     <asp:TextBox ID="AttendancePlanBonusPartialPaidMaxTotalLateMins" runat="server" />   
                     <asp:CheckBox ID="AttendancePlanBonusPartialPaidMaxTotalLateMinsIncludeLunch" runat="server" Text="Included Meal Break Late"/>   
                </td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label37" runat="server" Text="Max Early Leave Count" />:</td>
                <td class="pm_field">
                     <asp:TextBox ID="AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveCount" runat="server" />   
                     <asp:CheckBox ID="AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveCountIncludeLunch" runat="server" Text="Included Meal Break Early Leave"/>
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label39" runat="server" Text="Max Early Leave (mins)" />:
                </td>
                <td class="pm_field">
                     <asp:TextBox ID="AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveMins" runat="server" />   
                     <asp:CheckBox ID="AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveMinsIncludeLunch" runat="server" Text="Included Meal Break Early Leave"/>
                </td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label41" runat="server" Text="Max Sick Leave taken with Relevant Certificate" />:</td>
                <td class="pm_field">
                     <asp:TextBox ID="AttendancePlanBonusPartialPaidMaxTotalSLWithMedicalCertificate" runat="server" />   
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label43" runat="server" Text="Max Sick Leave taken without Relevant Certificate" />:</td>
                <td class="pm_field">
                     <asp:TextBox ID="AttendancePlanBonusPartialPaidMaxTotalSLWithoutMedicalCertificate" runat="server" />   
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label45" runat="server" Text="Max No Pay Leave" />:</td>
                <td class="pm_field">
                     <asp:TextBox ID="AttendancePlanBonusPartialPaidMaxTotalNonFullPayCasualLeave" runat="server" />   
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label47" runat="server" Text="Max Injury Leave Taken" />:</td>
                <td class="pm_field">
                     <asp:TextBox ID="AttendancePlanBonusPartialPaidMaxTotalInjuryLeave" runat="server" />   
                </td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label49" runat="server" Text="Max Absent" />:</td>
                <td class="pm_field">
                     <asp:TextBox ID="AttendancePlanBonusPartialPaidMaxTotalAbsentCount" runat="server" />   
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label34" runat="server" Text="Percentage of Bonus" />:</td>
                <td class="pm_field">
                     <asp:TextBox ID="AttendancePlanBonusPartialPaidPercent" runat="server" />%   
                </td>
            </tr>
		</table>
		</ContentTemplate>
		</asp:UpdatePanel>
</asp:Content> 