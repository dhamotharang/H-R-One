<%@ page language="C#" autoeventwireup="true" inherits="RosterCode_Edit, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>



<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <script type="text/javascript" src="colorpicker/prototype.compressed.js"></script>
    <script type="text/javascript" src="colorpicker/procolor.compressed.js"></script>
        <input type="hidden" id="RosterCodeID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="Label19" Text="Roster Code Setup" runat="server" />
                </td>
            </tr>
        </table>

        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <%=CurID<=0?HROne.Common.WebUtility.GetLocalizedString("Add"):HROne.Common.WebUtility.GetLocalizedString("Edit") %>
                    <asp:Label Text="Roster Code " runat="server" />:
                    <%=RosterCode.Text %>
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
        <ContentTemplate >
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="15%" />
            <col width="35%" />
            <col width="15%" />
            <col width="35%" />
    		<tr>
                <td class="pm_field_title"  colspan="4">
                   <asp:Label runat="server" Text="Roster Code" /></td>
			</tr>
			<tr>
                <td class="pm_field_header" >
                    <asp:Label Text="Roster Code" runat="server" />:</td>
                <td class="pm_field" >
                    <asp:TextBox ID="RosterCode" runat="Server" /></td>
                <td class="pm_field_header" >
                    <asp:Label Text="Description" runat="server" />:</td>
                <td class="pm_field" >
                    <asp:TextBox ID="RosterCodeDesc" runat="Server" /></td>
            </tr>
			<tr>
                <td class="pm_field_header" >
                    <asp:Label Text="Roster Type" runat="server" />:</td>
                <td class="pm_field" >
                    <asp:DropDownList ID="RosterCodeType" runat="Server" OnSelectedIndexChanged="RosterCodeType_SelectedIndexChanged" AutoPostBack="true"  /></td>
                <asp:Panel ID="LeaveCodeSettingPanel" runat="server" > 
                    <td class="pm_field_header">
                        <asp:Label ID="Label20" Text="Related Leave Code" runat="server" />:</td>
                    <td class="pm_field">
                        <asp:DropDownList ID="LeaveCodeID" runat="Server" /></td>
                </asp:Panel>
            </tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label16" Text="Client Name" runat="server" />:</td>
                <td class="pm_field" >
                    <asp:DropDownList ID="RosterClientID" runat="Server" AutoPostBack="true" /></td>
                <td class="pm_field_header">
                    <asp:Label ID="Label13" Text="Site" runat="server" />:</td>
                <td class="pm_field" >
                    <asp:DropDownList ID="RosterClientSiteID" runat="Server"  /></td>
            </tr>
			<tr id="CostCenterRow" runat="server" >
                <td class="pm_field_header">
                    <asp:Label ID="Label23" Text="Cost Center" runat="server" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="CostCenterID" runat="Server" AutoPostBack="true" />
                </td>
            </tr>
    		<tr>
                <td class="pm_field_title"  colspan="4">
                   <asp:Label ID="Label7" runat="server" Text="Work Time Period Setting" /></td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label3" Text="Count Total Work Hour Only" runat="server" />:</td>
                <td class="pm_field" colspan="3">
                   <asp:CheckBox ID="RosterCodeCountWorkHourOnly" runat="server"  AutoPostBack="true" />
                </td>
            </tr>
            <asp:Panel ID="RosterCodeNormalWorkInOut" runat="server" > 
			<tr>
                <td class="pm_field_header">
                    <asp:Label Text="In Time" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="RosterCodeInTime" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label Text="Out Time" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="RosterCodeOutTime" runat="Server" /></td>
            </tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label Text="Grace In Time" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="RosterCodeGraceInTime" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label Text="Grace Out Time" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="RosterCodeGraceOutTime" runat="Server" /></td>
            </tr>
            </asp:Panel>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Enable Meal Break" />:</td>
                <td class="pm_field" colspan="3">
                   <asp:CheckBox ID="RosterCodeHasLunch" runat="server"  AutoPostBack="true" />
                </td>
			</tr>
			<asp:Panel ID="RosterCodeLunchPanel" runat="server" > 
			<tr>
                <td class="pm_field_header">
                    <asp:Label Text="Meal Break Start Time" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="RosterCodeLunchStartTime" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label Text="Meal Break End Time" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="RosterCodeLunchEndTime" runat="Server" /></td>
            </tr>  
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label43" Text="Minimum Working Hour for Meal Break" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="RosterCodeLunchDeductMinimumWorkHour" runat="Server" />
                    <asp:Label ID="Label21" Text="hour(s)" runat="server" />   
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label45" Text="Meal Break Duration" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="RosterCodeLunchDurationHour" runat="Server" />
                    <asp:Label ID="Label44" Text="hour(s)" runat="server" />   
                </td>
            </tr>  
            <asp:Panel ID="RosterCodeCountWorkHourOnlyLunchPanel" runat="server" >
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label11" Text="Deduct Working Hour for Meal Break" runat="server" />:</td>
                <td class="pm_field">
                   <asp:CheckBox ID="RosterCodeLunchIsDeductWorkingHour" runat="server"   />
                </td> 
                <td class="pm_field_header">
                    <asp:Label ID="Label12" runat="server" Text="Meal Break Unit" />:</td>
                <td class="pm_field">
                     <asp:DropDownList ID="RosterCodeLunchDeductWorkingHourMinsRoundingRule" runat="server" />   
                     <asp:TextBox ID="RosterCodeLunchDeductWorkingHourMinsUnit" runat="server" />   
                     <asp:Label ID="Label15" Text="mins" runat="server" />   
                </td>
            </tr>
            </asp:Panel>
            </asp:Panel>
    		<tr>
                <td class="pm_field_title"  colspan="4">
                   <asp:Label ID="Label8" runat="server" Text="Overtime Setting" /></td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Enable Overtime" />:</td>
                <td class="pm_field" colspan="3">
                   <asp:CheckBox ID="RosterCodeHasOT" runat="server"  AutoPostBack="true" />

                </td>
			</tr>
			<asp:Panel ID="RosterCodeOTPanel" runat="server" >
            <asp:Panel ID="RosterCodeNormalWorkInOutOT" runat="server" > 
			<tr>
                <td class="pm_field_header">
                    <asp:Label Text="OT Start Time" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="RosterCodeOTStartTime" runat="Server"  />
                    <asp:Label runat="server" Text="(keep empty if Overtime start immediately)" />
                </td>
                <td class="pm_field_header">
                    <asp:Label Text="OT End Time" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="RosterCodeOTEndTime" runat="Server" />
                    <asp:Label ID="Label22" runat="server" Text="(keep empty if Overtime do not have limit)" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label14" runat="server" Text="Shift overtime start time if late" />?</td>
                <td class="pm_field" colspan="3" >
                     <asp:CheckBox ID="RosterCodeOTShiftStartTimeForLate" runat="server" />   
                </td>
            </tr>
            </asp:Panel> 
            <asp:Panel ID="RosterCodeCountWorkHourOnlyOT" runat="server" >
            <tr>
                <td class="pm_field_header"></td>
                <td class="pm_field" colspan="3" >
                    <asp:Label ID="Label4" Text="Overtime start at" runat="server" />
                    <asp:TextBox ID="RosterCodeCountOTAfterWorkHourMin" runat="Server" /><asp:Label ID="Label5" Text="min(s) after the end of normal working hours" runat="server" />
                </td>
            </tr>
            </asp:Panel>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label2" runat="server" Text="Count OT from Out Time" />?</td>
                <td class="pm_field" >
                   <asp:CheckBox ID="RosterCodeIsOTStartFromOutTime" runat="server" />
                </td>
                <td class="pm_field_header" >
                    <asp:Label ID="Label25" runat="server" Text="Include OT at Meal Break" />?</td>
                <td class="pm_field" >
                   <asp:CheckBox ID="RosterCodeOTIncludeLunch" runat="server" />
                </td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label6" runat="server" Text="OT Unit" />:</td>
                <td class="pm_field" colspan="3">
                     <asp:DropDownList ID="RosterCodeOTMinsRoundingRule" runat="server" />   
                     <asp:TextBox ID="RosterCodeOTMinsUnit" runat="server" />   
                     <asp:Label ID="Label9" Text="mins" runat="server" />   
                </td>
            </tr>

            </asp:Panel> 
    		<tr>
                <td class="pm_field_title"  colspan="4">
                   <asp:Label ID="Label49" runat="server" Text="Late/Early Leave Setting" />
                </td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label50" runat="server" Text="Late Unit" />:
                </td>
                <td class="pm_field" >
                     <asp:DropDownList ID="RosterCodeLateMinsRoundingRule" runat="server" />   
                     <asp:TextBox ID="RosterCodeLateMinsUnit" runat="server" />   
                     <asp:Label ID="Label52" Text="mins" runat="server" />   
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label51" runat="server" Text="Early Leave Unit" />:
                </td>
                <td class="pm_field" >
                     <asp:DropDownList ID="RosterCodeEarlyLeaveMinsRoundingRule" runat="server" />   
                     <asp:TextBox ID="RosterCodeEarlyLeaveMinsUnit" runat="server" />   
                     <asp:Label ID="Label55" Text="mins" runat="server" />   
                </td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label24" runat="server" Text="Meal Break Late Unit" />:
                </td>
                <td class="pm_field" >
                     <asp:DropDownList ID="RosterCodeLunchLateMinsRoundingRule" runat="server" />   
                     <asp:TextBox ID="RosterCodeLunchLateMinsUnit" runat="server" />   
                     <asp:Label ID="Label26" Text="mins" runat="server" />   
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label27" runat="server" Text="Meal Break Early Leave Unit" />:
                </td>
                <td class="pm_field" >
                     <asp:DropDownList ID="RosterCodeLunchEarlyLeaveMinsRoundingRule" runat="server" />   
                     <asp:TextBox ID="RosterCodeLunchEarlyLeaveMinsUnit" runat="server" />   
                     <asp:Label ID="Label28" Text="mins" runat="server" />   
                </td>
			</tr>
    		<tr>
                <td class="pm_field_title"  colspan="4">
                   <asp:Label ID="Label10" runat="server" Text="Miscellaneous" /></td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Cut Off time" />:</td>
                <td class="pm_field">
                     <asp:TextBox ID="RosterCodeCutOffTime" runat="server" />   
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label1" runat="server" Text="Working Day Unit" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="RosterCodeWorkingDayUnit" runat="server" />  
                </td>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Daily Working Hour" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="RosterCodeDailyWorkingHour" runat="server" />  
                </td>
			</tr>
            <asp:Panel ID="RosterCodeCountWorkHourOnlyHalfUnitPanel" runat="server" >
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label38" runat="server" Text="" /></td>
                <td class="pm_field" colspan="3">
                    <asp:CheckBox ID="RosterCodeUseHalfWorkingDaysHours" Text="Calculate Working Hours/Days by half if total working hours is smaller than" runat="server"  />  
                    <asp:TextBox ID="RosterCodeUseHalfWorkingDaysHoursMaxWorkingHours" runat="server" />  
                    <asp:Label runat="server" Text="hour(s)" />
                </td>
			</tr>
			</asp:Panel>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label17" runat="server" Text="Override Hourly Rate" />?:</td>
                <td class="pm_field">
                    <asp:CheckBox ID="RosterCodeIsOverrideHourlyPayment" runat="server" AutoPostBack="true"  />  
                </td>
                <asp:Panel ID="OverrideHourlyPaymentPanel" runat="server" >
                <td class="pm_field_header">
                    <asp:Label ID="Label18" runat="server" Text="New Hourly Rate" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="RosterCodeOverrideHoulyAmount" runat="server" />  
                </td>
                </asp:Panel>
			</tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Color Code" />
			    </td>
                <td class="pm_field">
                    <asp:TextBox ID="RosterCodeColorCode" runat="server" />  
                </td>
			</tr>
		</table>
        </ContentTemplate>
        </asp:UpdatePanel >
</asp:Content> 