<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Attendance_AttendanceRecordDetail_Edit.ascx.cs" Inherits="Attendance_AttendanceRecordDetail_Edit" %>
<asp:Panel CSSClass="popup_Container" ID="DivAttendanceRecordDetailSection" style="display: none" runat="server">
    <div class="popup_Titlebar" id="PopupHeader">
        <div class="TitlebarLeft"><asp:Label ID="lblHeader" runat="server" EnableViewState="false" Text="Attendance Record Detail" /></div>
        <div class="TitlebarRight" onclick="$get('<%=AttendanceReecordEditCancelButton.ClientID %>').click();">
        X
        </div>
    </div>
    <div class="popup_Body" >
    <input type="hidden" runat="server" id="hiddenAttendanceRecordID" />
    <input type="hidden" runat="server" id="EmpID" />
    <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_field_section">
    <tr>
        <td class="pm_field_header">
            <asp:Label ID="Label6" EnableViewState="false" Text="Date" runat="server" />:
        </td>
        <td class="pm_field" colspan="4">
            <input type="hidden" id="AttendanceRecordDate" runat="server" />
            <%= AttendanceRecordDate.Value %>
        </td>
    </tr>
    <tr>
        <td class="pm_field_header">
            <asp:Label ID="Label7" EnableViewState="false" Text="Roster Code" runat="server" />:
        </td>
        <td class="pm_field" colspan="4">
            <asp:DropDownList ID="RosterClientID" runat="server" OnSelectedIndexChanged="RosterClientID_SelectedIndexChanged" AutoPostBack="true"  />
            <asp:DropDownList ID="RosterClientSiteID" runat="server"  OnSelectedIndexChanged="RosterClientSiteID_SelectedIndexChanged"  AutoPostBack="true" />
            <asp:DropDownList ID="RosterCodeID" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header">
        </td>
        <td align="left" class="pm_list_header">
            <asp:Label ID="Label1" runat="server" EnableViewState="false" Text="Work Start" />
        </td>
        <td align="left" class="pm_list_header">
            <asp:Label ID="Label2" runat="server" EnableViewState="false" Text="Meal Break Out" />
        </td>
        <td align="left" class="pm_list_header">
            <asp:Label ID="Label3" runat="server" EnableViewState="false" Text="Meal Break In" />
        </td>
        <td align="left" class="pm_list_header">
            <asp:Label ID="Label4" runat="server" EnableViewState="false" Text="Work End" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header">
            <asp:Label ID="Label19" EnableViewState="false" Text="Roster Time Override" runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:TextBox ID="AttendanceRecordRosterCodeInTimeOverride" runat="server" ></asp:TextBox>
            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderAttendanceRecordRosterCodeInTimeOverride" runat="server" 
                AcceptAMPM="false" UserTimeFormat="TwentyFourHour" Mask="99:99" MaskType="Time" TargetControlID="AttendanceRecordRosterCodeInTimeOverride" PromptCharacter="_" > 
            </ajaxToolkit:MaskedEditExtender>
        </td>
        <td class="pm_field" >
            <asp:TextBox ID="AttendanceRecordRosterCodeLunchStartTimeOverride" runat="server" />

            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderAttendanceRecordRosterCodeLunchStartTimeOverride" runat="server" 
                AcceptAMPM="false" UserTimeFormat="TwentyFourHour" Mask="99:99" MaskType="Time" TargetControlID="AttendanceRecordRosterCodeLunchStartTimeOverride" PromptCharacter="_" > 
            </ajaxToolkit:MaskedEditExtender>

        </td>
        <td class="pm_field" >
            <asp:TextBox ID="AttendanceRecordRosterCodeLunchEndTimeOverride" runat="server" />
            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderAttendanceRecordRosterCodeLunchEndTimeOverride" runat="server" 
                AcceptAMPM="false" UserTimeFormat="TwentyFourHour" Mask="99:99" MaskType="Time" TargetControlID="AttendanceRecordRosterCodeLunchEndTimeOverride" PromptCharacter="_" > 
            </ajaxToolkit:MaskedEditExtender>

        </td>
        <td class="pm_field" >
            <asp:TextBox ID="AttendanceRecordRosterCodeOutTimeOverride" runat="server" />
            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderAttendanceRecordRosterCodeOutTimeOverride" runat="server" 
                AcceptAMPM="false" UserTimeFormat="TwentyFourHour" Mask="99:99" MaskType="Time" TargetControlID="AttendanceRecordRosterCodeOutTimeOverride" PromptCharacter="_" > 
            </ajaxToolkit:MaskedEditExtender>

        </td>
   </tr>
    <tr>
        <td class="pm_field_header">
            <asp:Label ID="Label5" runat="server" EnableViewState="false" Text="Actual" />:
        </td>
        <td class="pm_field" >
            <asp:TextBox ID="AttendanceRecordWorkStart" runat="server" />
            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderAttendanceRecordWorkStart" runat="server" 
                AcceptAMPM="false" UserTimeFormat="TwentyFourHour" Mask="99:99" MaskType="Time" TargetControlID="AttendanceRecordWorkStart" PromptCharacter="_" > 
            </ajaxToolkit:MaskedEditExtender>

        </td>
        <td class="pm_field" >
            <asp:TextBox ID="AttendanceRecordLunchOut" runat="server" />
            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderAttendanceRecordLunchOut" runat="server" 
                AcceptAMPM="false" UserTimeFormat="TwentyFourHour" Mask="99:99" MaskType="Time" TargetControlID="AttendanceRecordLunchOut" PromptCharacter="_" > 
            </ajaxToolkit:MaskedEditExtender>

        </td>
        <td class="pm_field" >
            <asp:TextBox ID="AttendanceRecordLunchIn" runat="server" />
            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderAttendanceRecordLunchIn" runat="server" 
                AcceptAMPM="false" UserTimeFormat="TwentyFourHour" Mask="99:99" MaskType="Time" TargetControlID="AttendanceRecordLunchIn" PromptCharacter="_" > 
            </ajaxToolkit:MaskedEditExtender>

        </td>
        <td class="pm_field" >
            <asp:TextBox ID="AttendanceRecordWorkEnd" runat="server" />
            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderAttendanceRecordWorkEnd" runat="server" 
                AcceptAMPM="false" UserTimeFormat="TwentyFourHour" Mask="99:99" MaskType="Time" TargetControlID="AttendanceRecordWorkEnd" PromptCharacter="_" > 
            </ajaxToolkit:MaskedEditExtender>

        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
            <asp:Label ID="Label8" runat="server" EnableViewState="false" Text="Location" />:
        </td>
        <td class="pm_field" >
            <asp:TextBox ID="AttendanceRecordWorkStartLocation" runat="server" />
        </td>
        <td class="pm_field" >
            <asp:TextBox ID="AttendanceRecordLunchOutLocation" runat="server" />
        </td>
        <td class="pm_field" >
            <asp:TextBox ID="AttendanceRecordLunchInLocation" runat="server" />
        </td>
        <td class="pm_field" >
            <asp:TextBox ID="AttendanceRecordWorkEndLocation" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header">
            <asp:Label ID="Label13" runat="server" EnableViewState="false" Text="Late (mins)" />:
        </td>
        <td class="pm_field" >
            <asp:TextBox ID="AttendanceRecordActualLateMins" runat="server" style="text-align:right;" /> &nbsp &nbsp &nbsp &nbsp
            
            <!-- Start 000112, Ricky So, 2015-02-10 --> 
            <asp:Label Text="Waived" runat="server"/>: &nbsp &nbsp
            <asp:Label ID="AttendanceRecordWaivedLateMins" runat="server" />                        
            <!-- End 000112, Ricky So, 2015-02-10 --> 
        </td>
        <td class="pm_field_header" colspan="2">
            <asp:Label ID="Label14" runat="server" EnableViewState="false" Text="Meal Break Late (mins)" />:
        </td>
        <td class="pm_field" >
            <asp:TextBox ID="AttendanceRecordActualLunchLateMins" runat="server" style="text-align:right;" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header">
            <asp:Label ID="Label15" runat="server" EnableViewState="false" Text="Early Leave (mins)" />:
        </td>
        <td class="pm_field" >
            <asp:TextBox ID="AttendanceRecordActualEarlyLeaveMins" runat="server"  style="text-align:right;"/>
        </td>
        <td class="pm_field_header" colspan="2">
            <asp:Label ID="Label16" runat="server" EnableViewState="false" Text="Meal Break Early Leave (mins)" />:
        </td>
        <td class="pm_field" >
            <asp:TextBox ID="AttendanceRecordActualLunchEarlyLeaveMins" runat="server"  style="text-align:right;"/>
        </td>
    </tr>
    <tr>
        <td class="pm_field_header">
            <asp:Label ID="Label17" runat="server" EnableViewState="false" Text="Overtime (mins)" />:
        </td>
        <td class="pm_field" >
            <asp:TextBox ID="AttendanceRecordActualOvertimeMins" runat="server"  style="text-align:right;"/>
        </td>
        <td class="pm_field_header" colspan="2">
            <asp:Label ID="Label18" runat="server" EnableViewState="false" Text="Meal Break Overtime (mins)" />:
        </td>
        <td class="pm_field" >
            <asp:TextBox ID="AttendanceRecordActualLunchOvertimeMins" runat="server"  style="text-align:right;"/>
        </td>
    </tr>
    <tr>
        <td class="pm_field_header">
            <asp:Label ID="Label9" runat="server" EnableViewState="false" Text="Working Hour(s)" />:
        </td>
        <td class="pm_field" >
            <asp:TextBox ID="AttendanceRecordActualWorkingHour" runat="server"  style="text-align:right;"/>
        </td>
        <td class="pm_field_header" colspan="2">
            <asp:Label ID="Label10" runat="server" EnableViewState="false" Text="Working Day(s)" />:
        </td>
        <td class="pm_field" >
            <asp:TextBox ID="AttendanceRecordActualWorkingDay" runat="server"  style="text-align:right;"/>
        </td>
    </tr>
    <tr>
        <td class="pm_field_header">
            <asp:Label ID="Label11" runat="server" EnableViewState="false" Text="Meal Break Duration" />:
        </td>
        <td class="pm_field" >
            <asp:TextBox ID="AttendanceRecordActualLunchTimeMins" runat="server"  style="text-align:right;"/>
        </td>
    </tr> 
    <tr>
        <td class="pm_field_header">
            <asp:Label ID="ILabel2" runat="server" EnableViewState="false" Text="Absent" />?
        </td>
        <td class="pm_field" >
            <asp:CheckBox ID="AttendanceRecordIsAbsent" runat="server" />
        </td>
        <td class="pm_field_header" colspan="2">
            <asp:Label ID="ILabel4" runat="server" EnableViewState="false" Text="Has Bonus" />?
        </td>
        <td class="pm_field" >
            <asp:DropDownList ID="AttendanceRecordHasBonus" runat="server" >
                <asp:ListItem Value="" Text="By Attendance Plan" />
                <asp:ListItem Value="Y" Text="Yes" />
                <asp:ListItem Value="N" Text="No" />
            </asp:DropDownList> 
        </td>
    </tr> 
    <tr>
        <td class="pm_field_header">
            <asp:Label ID="Label20" runat="server"  />
        </td>
        <td class="pm_field" >
            <asp:CheckBox ID="WorkAsOvertime" runat="server" EnableViewState="false" Text="Work as OT"/>
            <asp:CheckBox ID="AttendanceRecordWorkOnRestDay" runat="server" EnableViewState="false" Text="Work on rest day"/>
        </td>
        <td class="pm_field_header" colspan="2">
            <asp:Label ID="Label21" runat="server" EnableViewState="false" Text="Override Daily Payment Amount" />
        </td>
        <td class="pm_field" >
            $<asp:TextBox ID="OverrideDailyPayment" runat="server" style="text-align:right" Columns="10" MaxLength="10" />
        </td>
    </tr> 
    <tr>
        <td class="pm_field_header" align="right">
            <asp:Label ID="Label12" runat="server" EnableViewState="false" Text="Remark" />:
        </td>
        <td class="pm_field" align="left" colspan="4">
            <asp:TextBox ID="AttendanceRecordRemark" runat="server" Columns="60"  TextMode="MultiLine" Rows="2"  Wrap="false" />
        </td>
    </tr>
    </table>
    </div> 
    <div class="popup_Buttons" >
        <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" UseSubmitBehavior="false" style="display:none"/>
        <asp:Button ID="AttendanceReecordEditOkayButton" runat="server" EnableViewState="false" Text="OK" CssClass="button" UseSubmitBehavior="false"  OnClick="ButtonMessageOkay_Click"/>
        <asp:Button ID="AttendanceReecordEditCancelButton" runat="server" EnableViewState="false" Text="Cancel" CssClass="button" UseSubmitBehavior="false" OnClick="ButtonMessageCancel_Click"/>
    </div>
        </asp:Panel>

        <asp:Button runat="server" ID="hiddenTargetControlForAttendanceRecordAdjustment" style="display:none"/>
        <ajaxToolkit:ModalPopupExtender id="AdjustmentModalPopupExtender" runat="server" 
            OkControlID="hiddenTargetControlForModalPopup"
		    cancelcontrolid="AttendanceReecordEditCancelButton" 
		    targetcontrolid="hiddenTargetControlForAttendanceRecordAdjustment" 
		    BehaviorID="AdjustmentModalPopupBehaviour"
		    popupcontrolid="DivAttendanceRecordDetailSection"
		    DropShadow="true"  
		    backgroundcssclass="modalBackground" >
        </ajaxToolkit:ModalPopupExtender>
