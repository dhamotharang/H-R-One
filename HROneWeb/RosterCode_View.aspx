<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RosterCode_View.aspx.cs" Inherits="RosterCode_View" MasterPageFile="~/MainMasterPage.master"  %>

<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/RosterCode_OTRatioList.ascx" TagName="RosterCode_OTRatioList" TagPrefix="uc" %>
<%@ Register Src="~/controls/RosterCode_AdditionalPayment.ascx" TagName="RosterCode_AdditionalPayment" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <script type="text/javascript" src="colorpicker/prototype.compressed.js"></script>
    <script type="text/javascript" src="colorpicker/procolor.compressed.js"></script>
        <input type="hidden" id="RosterCodeID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label Text="Roster Code Setup" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label Text="View Roster Code" runat="server" />:
                    <%=RosterCode.Text %>
                </td>
            </tr>
        </table>
        
            
                
        
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar id="toolBar" runat="server"
                     NewButton_Visible="false" 
                     SaveButton_Visible="false" 
                     OnBackButton_Click="Back_Click"
                     OnEditButton_Click ="Edit_Click"
                     OnDeleteButton_Click="Delete_ClickTop"
                      />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
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
                   <asp:Label ID="Label1" runat="server" Text="Roster Code" /></td>
			</tr>
			<tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label2" Text="Roster Code" runat="server" />:</td>
                <td class="pm_field" >
                    <asp:Label ID="RosterCode" runat="Server" /></td>
                <td class="pm_field_header" >
                    <asp:Label ID="Label3" Text="Description" runat="server" />:</td>
                <td class="pm_field" >
                    <asp:Label ID="RosterCodeDesc" runat="Server" /></td>
            </tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label4" Text="Roster Type" runat="server" />:</td>
                <td class="pm_field" >
                    <asp:Label ID="RosterCodeType" runat="Server" /></td>
                <asp:Panel ID="LeaveCodeSettingPanel" runat="server" > 
                    <td class="pm_field_header">
                        <asp:Label ID="Label37" Text="Related Leave Code" runat="server" />:</td>
                    <td class="pm_field">
                        <asp:Label ID="LeaveCodeID" runat="Server" /></td>
                </asp:Panel>            
            </tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label33" Text="Client Name" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="RosterClientID" runat="Server"  /></td>
                <td class="pm_field_header">
                    <asp:Label ID="Label34" Text="Site" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="RosterClientSiteID" runat="Server"  /></td>
            </tr>
			<tr id="CostCenterRow" runat="server" >
                <td class="pm_field_header">
                    <asp:Label ID="Label47" Text="Cost Center" runat="server" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="CostCenterID" runat="Server"/>
                </td>
            </tr>
    		<tr>
                <td class="pm_field_title"  colspan="4">
                   <asp:Label ID="Label28" runat="server" Text="Work Time Period Setting" /></td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label20" Text="Count Total Work Hour Only" runat="server" />:</td>
                <td class="pm_field" colspan="3">
                   <asp:Label ID="RosterCodeCountWorkHourOnly" runat="server" /></td>
            </tr>
            <asp:Panel ID="RosterCodeNormalWorkInOut" runat="server" > 
			<tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label5" Text="In Time" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="RosterCodeInTime" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label ID="Label6" Text="Out Time" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="RosterCodeOutTime" runat="Server" /></td>
            </tr>          
			<tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label7" Text="Grace In Time" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="RosterCodeGraceInTime" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label ID="Label8" Text="Grace Out Time" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="RosterCodeGraceOutTime" runat="Server" /></td>
            </tr>
            </asp:Panel>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label9" runat="server" Text="Enable Meal Break" />:</td>
                <td class="pm_field" colspan="3">
                   <asp:Label ID="RosterCodeHasLunch" runat="server"/>
                </td>
			</tr>
			<asp:Panel ID="RosterCodeLunchPanel" runat="server" > 
			<tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label10" Text="Meal Break Start Time" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="RosterCodeLunchStartTime" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label ID="Label11" Text="Meal Break End Time" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="RosterCodeLunchEndTime" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label43" Text="Minimum Working Hour for Meal Break" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="RosterCodeLunchDeductMinimumWorkHour" runat="Server" />
                    <asp:Label ID="Label46" Text="hour(s)" runat="server" />   
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label45" Text="Meal Break Duration" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="RosterCodeLunchDurationHour" runat="Server" />
                    <asp:Label ID="Label44" Text="hour(s)" runat="server" />   
                </td>
            </tr>  
            <asp:Panel ID="RosterCodeCountWorkHourOnlyLunchPanel" runat="server" >
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label29" Text="Deduct Working Hour for Meal Break" runat="server" />:</td>
                <td class="pm_field">
                   <asp:Label ID="RosterCodeLunchIsDeductWorkingHour" runat="server"   />
                </td> 
                <td class="pm_field_header">
                    <asp:Label ID="Label30" runat="server" Text="Meal Break Unit" />:</td>
                <td class="pm_field">
                     <asp:Label ID="RosterCodeLunchDeductWorkingHourMinsRoundingRule" runat="server" />   
                     <asp:Label ID="RosterCodeLunchDeductWorkingHourMinsUnit" runat="server" />   
                     <asp:Label ID="Label31" Text="mins" runat="server" />   
                </td>
            </tr>
            </asp:Panel>
            </asp:Panel>
    		<tr>
                <td class="pm_field_title"  colspan="4">
                   <asp:Label ID="Label27" runat="server" Text="Overtime Setting" /></td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label12" runat="server" Text="Enable Overtime" />:</td>
                <td class="pm_field" colspan="3">
                   <asp:Label ID="RosterCodeHasOT" runat="server" />

                </td>
			</tr>
			<asp:Panel ID="RosterCodeOTPanel" runat="server" >
            <asp:Panel ID="RosterCodeNormalWorkInOutOT" runat="server" > 
			<tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label13" Text="OT Start Time" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="RosterCodeOTStartTime" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label ID="Label14" Text="OT End Time" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="RosterCodeOTEndTime" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label32" runat="server" Text="Shift overtime start time if late" />?</td>
                <td class="pm_field" colspan="3" >
                     <asp:Label ID="RosterCodeOTShiftStartTimeForLate" runat="server" />   
                </td>
            </tr>
            </asp:Panel>            
            <asp:Panel ID="RosterCodeCountWorkHourOnlyOT" runat="server" >
            <tr>
                <td class="pm_field_header"></td>
                <td class="pm_field" colspan="3" >
                    <asp:Label ID="Label25" Text="Overtime start at" runat="server" />
                    <asp:Label ID="RosterCodeCountOTAfterWorkHourMin" runat="Server" /><asp:Label ID="Label26" Text="min(s) after the end of normal working hours" runat="server" />
                </td>
            </tr>
            </asp:Panel>            
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label18" runat="server" Text="Count OT from Out Time"  />?</td>
                <td class="pm_field" >
                   <asp:Label ID="RosterCodeIsOTStartFromOutTime" runat="server" />
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label48" runat="server" Text="Include OT at Meal Break"  />?</td>
                <td class="pm_field" >
                   <asp:Label ID="RosterCodeOTIncludeLunch" runat="server" />
                </td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label19" runat="server" Text="OT Unit" />:</td>
                <td class="pm_field" colspan="3">
                     <asp:Label ID="RosterCodeOTMinsRoundingRule" runat="server" />   
                     <asp:Label ID="RosterCodeOTMinsUnit" runat="server" />   
                     <asp:Label ID="Label23" Text="mins" runat="server" />   
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
                     <asp:Label ID="RosterCodeLateMinsRoundingRule" runat="server" />   
                     <asp:Label ID="RosterCodeLateMinsUnit" runat="server" />   
                     <asp:Label ID="Label52" Text="mins" runat="server" />   
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label51" runat="server" Text="Early Leave Unit" />:
                </td>
                <td class="pm_field" >
                     <asp:Label ID="RosterCodeEarlyLeaveMinsRoundingRule" runat="server" />   
                     <asp:Label ID="RosterCodeEarlyLeaveMinsUnit" runat="server" />   
                     <asp:Label ID="Label55" Text="mins" runat="server" />   
                </td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label24" runat="server" Text="Meal Break Late Unit" />:
                </td>
                <td class="pm_field" >
                     <asp:Label ID="RosterCodeLunchLateMinsRoundingRule" runat="server" />   
                     <asp:Label ID="RosterCodeLunchLateMinsUnit" runat="server" />   
                     <asp:Label ID="Label56" Text="mins" runat="server" />   
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label57" runat="server" Text="Meal Break Early Leave Unit" />:
                </td>
                <td class="pm_field" >
                     <asp:Label ID="RosterCodeLunchEarlyLeaveMinsRoundingRule" runat="server" />   
                     <asp:Label ID="RosterCodeLunchEarlyLeaveMinsUnit" runat="server" />   
                     <asp:Label ID="Label60" Text="mins" runat="server" />   
                </td>
			</tr>
    		<tr>
                <td class="pm_field_title"  colspan="4">
                   <asp:Label ID="Label22" runat="server" Text="Miscellaneous" /></td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label15" runat="server" Text="Cut Off time" />:</td>
                <td class="pm_field">
                     <asp:Label ID="RosterCodeCutOffTime" runat="server" />   
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label17" runat="server" Text="Working Day Unit" />:</td>
                <td class="pm_field">
                    <asp:Label ID="RosterCodeWorkingDayUnit" runat="server" />  
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label16" runat="server" Text="Daily Working Hour" />:</td>
                <td class="pm_field">
                    <asp:Label ID="RosterCodeDailyWorkingHour" runat="server" />  
                </td>
			</tr>
            <asp:Panel ID="RosterCodeCountWorkHourOnlyHalfUnitPanel" runat="server" >
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label38" runat="server" Text="" /></td>
                <td class="pm_field" colspan="3">
                    <asp:CheckBox ID="RosterCodeUseHalfWorkingDaysHours" runat="server"  Enabled="false" />  
                    <asp:Label ID="Label40" runat="server" Text="Calculate Working Hours/Days by half if total working hours is smaller than"  />
                    <asp:Label ID="RosterCodeUseHalfWorkingDaysHoursMaxWorkingHours" runat="server" />  
                    <asp:Label ID="Label39" runat="server" Text="hour(s)"/>  
                </td>
			</tr>
			</asp:Panel> 
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label35" runat="server" Text="Override Hourly Rate" />?:</td>
                <td class="pm_field">
                    <asp:Label ID="RosterCodeIsOverrideHourlyPayment" runat="server" />  
                </td>
                <asp:Panel ID="OverrideHourlyPaymentPanel" runat="server" >
                <td class="pm_field_header">
                    <asp:Label ID="Label36" runat="server" Text="New Hourly Rate" />:</td>
                <td class="pm_field">
                    <asp:Label ID="RosterCodeOverrideHoulyAmount" runat="server" />  
                </td>
                </asp:Panel>
			</tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label42" runat="server" Text="Color Code" />
			    </td>
                <td class="pm_field">
                    <asp:Label ID="RosterCodeColorCode" runat="server" />  
                </td>
			</tr>
		</table>
        <br />
        <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
        <ContentTemplate >
            <uc:RosterCode_OTRatioList ID="RosterCode_OTRatioList1" runat="server" />
            <uc:RosterCode_AdditionalPayment ID="RosterCode_AdditionalPayment1" runat="server" />
        </ContentTemplate>
        </asp:UpdatePanel >
</asp:Content> 