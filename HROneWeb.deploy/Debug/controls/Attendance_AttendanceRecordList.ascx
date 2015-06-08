<%@ control language="C#" autoeventwireup="true" inherits="Attendance_AttendanceRecordList, HROneWeb.deploy" %>

<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
<%@ Register Src="~/controls/Attendance_AttendanceRecordDetail_Edit.ascx" TagName="Attendance_AttendanceRecordDetail_Edit" TagPrefix="uc1" %>

<input type="hidden" id="EmpID" runat="server" name="ID" />

<table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_section_title">
    <tr>
        <td colspan="2">
            <asp:Label ID="lblHeader" runat="server" Text="Attendance Record Detail" />:
        </td>
    </tr>
</table>
<table width="100%" cellpadding="0" cellspacing="0">
    <tr>
        <td class="pm_field_header">
            <asp:Label ID="Label3" runat="server" Text="Year/Month" />:
        </td>
        <td class="pm_field">
            <asp:DropDownList ID="Year" runat="server" AutoPostBack="True" OnSelectedIndexChanged="YearMonth_SelectedIndexChanged"/>
            <asp:DropDownList ID="Month" runat="server" AutoPostBack="True" OnSelectedIndexChanged="YearMonth_SelectedIndexChanged" />
        </td>
    </tr>
</table>
<table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
    <tr>
        <td>
                <tb:DetailToolBar id="toolBar" runat="server"
                 BackButton_Visible="false"
                 NewButton_Visible="false"
                 EditButton_Visible="false" 
                 SaveButton_Visible="false" 
                 OnDeleteButton_Click="Delete_Click"
                  />                
                  <asp:Button ID="Recalculate" CssClass="button" runat="server" Text="Recalculate" OnClick="Recalculate_Click" />
        </td>
    </tr>
</table>
<asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional" >
<Triggers>
    <asp:AsyncPostBackTrigger ControlID="Year" EventName="SelectedIndexChanged" />
    <asp:AsyncPostBackTrigger ControlID="Month" EventName="SelectedIndexChanged" />
    <asp:AsyncPostBackTrigger ControlID="toolBar"  />
    <asp:AsyncPostBackTrigger ControlID="Recalculate" EventName="Click" />
</Triggers>
<ContentTemplate>
<table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
    <col width="26px" />
    <col width="75px" />
    <col width="75px" />
    <col width="75px" />
    <col width="50px" />
    <col width="50px" />
    <col width="50px" />
    <col width="50px" />
    <tr>
        <td class="pm_list_header">
            <asp:Panel ID="SelectAllPanel" runat="server">
            <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','DeleteItem',this.checked);" />
            </asp:Panel>
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_AttendanceRecordDate" OnClick="ChangeOrder_Click" Text="Date" />
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_RosterCodeID" OnClick="ChangeOrder_Click" Text="Roster" />
        </td>
        <td align="left" class="pm_list_header">
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_AttendanceRecordWorkStart" OnClick="ChangeOrder_Click" Text="Work Start" />
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_AttendanceRecordLunchOut" OnClick="ChangeOrder_Click" Text="Meal Break Out" />
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_AttendanceRecordLunchIn" OnClick="ChangeOrder_Click" Text="Meal Break In" />
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_AttendanceRecordWorkEnd" OnClick="ChangeOrder_Click" Text="Work End" />
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_AttendanceRecordActualLateMins" OnClick="ChangeOrder_Click" Text="Late (mins)" />
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_AttendanceRecordActualEarlyLeaveMins" OnClick="ChangeOrder_Click" Text="Early Leave (mins)" />
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_AttendanceRecordActualLunchLateMins" OnClick="ChangeOrder_Click" Text="Meal Break Late (mins)" />
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_AttendanceRecordActualLunchEarlyLeaveMins" OnClick="ChangeOrder_Click" Text="Meal Break Early Leave (mins)" />
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_AttendanceRecordActualOvertimeMins" OnClick="ChangeOrder_Click" Text="Overtime (mins)" />
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_AttendanceRecordActualLunchOvertimeMins" OnClick="ChangeOrder_Click" Text="Meal Break Overtime (mins)" />
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_AttendanceRecordActualLunchTimeMins" OnClick="ChangeOrder_Click" Text="Meal Break Time (mins)" />
        </td>    
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_AttendanceRecordActualWorkingDay" OnClick="ChangeOrder_Click" Text="Working Day(s)" />
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_AttendanceRecordActualWorkingHour" OnClick="ChangeOrder_Click" Text="Working Hour(s)" />
        </td>    
    </tr>
    <asp:DataList ID="Repeater" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" ShowFooter="false" ShowHeader="false" OnItemCommand="Repeater_ItemCommand" OnItemDataBound="Repeater_ItemDataBound">
        <SeparatorTemplate>
    <% if (++ImetritisReapeter % 5 == 0) {%> 
        <tr>
            <td class="pm_list_header">
            </td>
            <td align="left" class="pm_list_header">
                <asp:Label runat="server" ID="_AttendanceRecordDate" EnableViewState="false"  Text="Date" />
            </td>
            <td align="left" class="pm_list_header">
                <asp:Label runat="server" ID="_RosterCodeID" EnableViewState="false"  Text="Roster" />
            </td>
            <td align="left" class="pm_list_header">
            </td>
            <td align="left" class="pm_list_header">
                <asp:Label runat="server" ID="_AttendanceRecordWorkStart" EnableViewState="false"  Text="Work Start" />
            </td>
            <td align="left" class="pm_list_header">
                <asp:Label runat="server" ID="_AttendanceRecordLunchOut" EnableViewState="false"  Text="Meal Break Out" />
            </td>
            <td align="left" class="pm_list_header">
                <asp:Label runat="server" ID="_AttendanceRecordLunchIn" EnableViewState="false"  Text="Meal Break In" />
            </td>
            <td align="left" class="pm_list_header">
                <asp:Label runat="server" ID="_AttendanceRecordWorkEnd" EnableViewState="false"  Text="Work End" />
            </td>
            <td align="left" class="pm_list_header">
                <asp:Label runat="server" ID="_AttendanceRecordActualLateMins" EnableViewState="false"  Text="Late (mins)" />
            </td>
            <td align="left" class="pm_list_header">
                <asp:Label runat="server" ID="_AttendanceRecordActualEarlyLeaveMins" EnableViewState="false"  Text="Early Leave (mins)" />
            </td>
            <td align="left" class="pm_list_header">
                <asp:Label runat="server" ID="_AttendanceRecordActualLunchLateMins" EnableViewState="false"  Text="Meal Break Late (mins)" />
            </td>
            <td align="left" class="pm_list_header">
                <asp:Label runat="server" ID="_AttendanceRecordActualLunchEarlyLeaveMins" EnableViewState="false"  Text="Meal Break Early Leave (mins)" />
            </td>
            <td align="left" class="pm_list_header">
                <asp:Label runat="server" ID="_AttendanceRecordActualOvertimeMins" EnableViewState="false"  Text="Overtime (mins)" />
            </td>
            <td align="left" class="pm_list_header">
                <asp:Label runat="server" ID="_AttendanceRecordActualLunchOvertimeMins" EnableViewState="false"  Text="Meal Break Overtime (mins)" />
            </td>
            <td align="left" class="pm_list_header">
                <asp:Label runat="server" ID="_AttendanceRecordActualLunchTimeMins" EnableViewState="false"  Text="Meal Break Time (mins)" />
            </td>    
            <td align="left" class="pm_list_header">
                <asp:Label runat="server" ID="_AttendanceRecordActualWorkingDay" EnableViewState="false"  Text="Working Day(s)" />
            </td>
            <td align="left" class="pm_list_header">
                <asp:Label runat="server" ID="_AttendanceRecordActualWorkingHour" EnableViewState="false"  Text="Working Hour(s)" />
            </td>    
        </tr>
<%    } %>        
        </SeparatorTemplate>
        <EditItemTemplate>
            <tr>
                <td class="pm_list_edit" align="center" rowspan="4">
                    <asp:CheckBox ID="DeleteItem" runat="server" />
                    <input type="hidden" runat="server" id="AttendanceRecordID" />
                </td>
                <td class="pm_list_edit" align="center" valign="top" rowspan="4">
                    <asp:Label ID="AttendanceRecordDate" runat="server"/>
                    <asp:Button ID="Save" CssClass="button" runat="server" Text="Save" />
                    <asp:Button ID="Cancel" CssClass="button" runat="server" Text="Cancel" />
                </td>
                <td class="pm_list_edit" valign="top" rowspan="4">
                    <asp:DropDownList ID="RosterCodeID" runat="server" width="75px"/>
                </td>
                <td class="pm_list_edit" align="right">
                    <asp:Label ID="Label4" runat="server" Text="Roster Time Override" />:
                </td>
                <td class="pm_list_edit" align="right">
                    <asp:TextBox ID="AttendanceRecordRosterCodeInTimeOverride" runat="server" ></asp:TextBox>

                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderAttendanceRecordRosterCodeInTimeOverride" runat="server" 
                        AcceptAMPM="false" UserTimeFormat="TwentyFourHour" Mask="99:99" MaskType="Time" TargetControlID="AttendanceRecordRosterCodeInTimeOverride" PromptCharacter="_" > 
                    </ajaxToolkit:MaskedEditExtender>
                </td>
                <td class="pm_list_edit" align="right">
                    <asp:TextBox ID="AttendanceRecordRosterCodeLunchStartTimeOverride" runat="server" />

                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderAttendanceRecordRosterCodeLunchStartTimeOverride" runat="server" 
                        AcceptAMPM="false" UserTimeFormat="TwentyFourHour" Mask="99:99" MaskType="Time" TargetControlID="AttendanceRecordRosterCodeLunchStartTimeOverride" PromptCharacter="_" > 
                    </ajaxToolkit:MaskedEditExtender>

                </td>
                <td class="pm_list_edit" align="right">
                    <asp:TextBox ID="AttendanceRecordRosterCodeLunchEndTimeOverride" runat="server" />
                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderAttendanceRecordRosterCodeLunchEndTimeOverride" runat="server" 
                        AcceptAMPM="false" UserTimeFormat="TwentyFourHour" Mask="99:99" MaskType="Time" TargetControlID="AttendanceRecordRosterCodeLunchEndTimeOverride" PromptCharacter="_" > 
                    </ajaxToolkit:MaskedEditExtender>

                </td>
                <td class="pm_list_edit" align="right">
                    <asp:TextBox ID="AttendanceRecordRosterCodeOutTimeOverride" runat="server" />
                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderAttendanceRecordRosterCodeOutTimeOverride" runat="server" 
                        AcceptAMPM="false" UserTimeFormat="TwentyFourHour" Mask="99:99" MaskType="Time" TargetControlID="AttendanceRecordRosterCodeOutTimeOverride" PromptCharacter="_" > 
                    </ajaxToolkit:MaskedEditExtender>

                </td>
                <td class="pm_list_edit" align="center" colspan="9">
                </td>
            </tr>
            <tr>
                <td class="pm_list_edit" align="right">
                    <asp:Label ID="Label2" runat="server" Text="Actual" />:
                </td>
                <td class="pm_list_edit" align="right">
                    <asp:TextBox ID="AttendanceRecordWorkStart" runat="server" />
                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderAttendanceRecordWorkStart" runat="server" 
                        AcceptAMPM="false" UserTimeFormat="TwentyFourHour" Mask="99:99" MaskType="Time" TargetControlID="AttendanceRecordWorkStart" PromptCharacter="_" > 
                    </ajaxToolkit:MaskedEditExtender>

                </td>
                <td class="pm_list_edit" align="right">
                    <asp:TextBox ID="AttendanceRecordLunchOut" runat="server" />
                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderAttendanceRecordLunchOut" runat="server" 
                        AcceptAMPM="false" UserTimeFormat="TwentyFourHour" Mask="99:99" MaskType="Time" TargetControlID="AttendanceRecordLunchOut" PromptCharacter="_" > 
                    </ajaxToolkit:MaskedEditExtender>

                </td>
                <td class="pm_list_edit" align="right">
                    <asp:TextBox ID="AttendanceRecordLunchIn" runat="server" />
                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderAttendanceRecordLunchIn" runat="server" 
                        AcceptAMPM="false" UserTimeFormat="TwentyFourHour" Mask="99:99" MaskType="Time" TargetControlID="AttendanceRecordLunchIn" PromptCharacter="_" > 
                    </ajaxToolkit:MaskedEditExtender>

                </td>
                <td class="pm_list_edit" align="right">
                    <asp:TextBox ID="AttendanceRecordWorkEnd" runat="server" />
                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderAttendanceRecordWorkEnd" runat="server" 
                        AcceptAMPM="false" UserTimeFormat="TwentyFourHour" Mask="99:99" MaskType="Time" TargetControlID="AttendanceRecordWorkEnd" PromptCharacter="_" > 
                    </ajaxToolkit:MaskedEditExtender>

                </td>
                <td class="pm_list_edit" align="right">
                    <asp:TextBox ID="AttendanceRecordActualLateMins" runat="server" style="text-align:right;" />
                </td>
                <td class="pm_list_edit" align="right">
                    <asp:TextBox ID="AttendanceRecordActualEarlyLeaveMins" runat="server"  style="text-align:right;"/>
                </td>
                <td class="pm_list_edit" align="right">
                    <asp:TextBox ID="AttendanceRecordActualLunchLateMins" runat="server" style="text-align:right;" />
                </td>
                <td class="pm_list_edit" align="right">
                    <asp:TextBox ID="AttendanceRecordActualLunchEarlyLeaveMins" runat="server"  style="text-align:right;"/>
                </td>
                <td class="pm_list_edit" align="right">
                    <asp:TextBox ID="AttendanceRecordActualOvertimeMins" runat="server"  style="text-align:right;"/>
                </td>
                <td class="pm_list_edit" align="right">
                    <asp:TextBox ID="AttendanceRecordActualLunchOvertimeMins" runat="server"  style="text-align:right;"/>
                </td>
                <td class="pm_list_edit" align="right">
                    <asp:TextBox ID="AttendanceRecordActualLunchTimeMins" runat="server"  style="text-align:right;"/>
                </td>
                <td class="pm_list_edit" align="right">
                    <asp:TextBox ID="AttendanceRecordActualWorkingDay" runat="server"  style="text-align:right;"/>
                </td>
                <td class="pm_list_edit" align="right">
                    <asp:TextBox ID="AttendanceRecordActualWorkingHour" runat="server"  style="text-align:right;"/>
                </td>
            </tr>
            <tr>
                <td class="pm_list_edit" align="right">
                    <asp:Label ID="Label1" runat="server" Text="Location" />:
                </td>
                <td class="pm_list_edit" >
                    <asp:TextBox ID="AttendanceRecordWorkStartLocation" runat="server" />
                </td>
                <td class="pm_list_edit" >
                    <asp:TextBox ID="AttendanceRecordLunchOutLocation" runat="server" />
                </td>
                <td class="pm_list_edit" >
                    <asp:TextBox ID="AttendanceRecordLunchInLocation" runat="server" />
                </td>
                <td class="pm_list_edit" >
                    <asp:TextBox ID="AttendanceRecordWorkEndLocation" runat="server" />
                </td>
                <td class="pm_list_edit" align="left" colspan="2">
                    <asp:Label ID="ILabel2" runat="server" Text="Absent" />?
                    <asp:CheckBox ID="AttendanceRecordIsAbsent" runat="server" />
                </td>
                <td class="pm_list_edit" align="left"  colspan="7">
                    <asp:Label ID="ILabel4" runat="server" Text="Has Bonus" />?
                    <asp:DropDownList ID="AttendanceRecordHasBonus" runat="server" >
                        <asp:ListItem Value="" Text="By Attendance Plan" />
                        <asp:ListItem Value="Y" Text="Yes" />
                        <asp:ListItem Value="N" Text="No" />
                    </asp:DropDownList> 
                </td>
            </tr>
            <tr>
                <td class="pm_list_edit" align="right">
                    <asp:Label runat="server" Text="Remark" />:
                </td>
                <td class="pm_list_edit" align="left" colspan="13">
                    <asp:TextBox ID="AttendanceRecordRemark" runat="server" Columns="60"  TextMode="MultiLine" Rows="2"  Wrap="false" />
                </td>
            </tr>
        </EditItemTemplate>
        <ItemTemplate>
            <tr>
                <td class="pm_list" align="center" rowspan="4">
                    <asp:CheckBox ID="DeleteItem" runat="server" />
                    <input type="hidden" runat="server" id="AttendanceRecordID" />
                </td>
                <td class="pm_list" align="center" valign="top" rowspan="4">
                    <%#sBinding.getFValue(Container.DataItem, "AttendanceRecordDate","yyyy-MM-dd")%>
                    <asp:Button ID="Edit" CssClass="button" runat="server" Text="Edit" />
                </td>
                <td class="pm_list" align="left" valign="top" rowspan="4">
                    <asp:Label ID="RosterCodeID" runat="server" />
                </td>
                <td class="pm_list" align="right">
                    <asp:Label ID="Label4" runat="server" Text="Roster Time Override" />:
                </td>
                <td class="pm_list" align="center">
                    <%#sBinding.getFValue(Container.DataItem, "AttendanceRecordRosterCodeInTimeOverride", "HH:mm")%>
                </td>
                <td class="pm_list" align="center">
                    <%#sBinding.getFValue(Container.DataItem, "AttendanceRecordRosterCodeLunchStartTimeOverride", "HH:mm")%>
                </td>
                <td class="pm_list" align="center">
                    <%#sBinding.getFValue(Container.DataItem, "AttendanceRecordRosterCodeLunchEndTimeOverride", "HH:mm")%>
                </td>
                <td class="pm_list" align="center">
                    <%#sBinding.getFValue(Container.DataItem, "AttendanceRecordRosterCodeOutTimeOverride", "HH:mm")%>
                </td>
                <td class="pm_list" align="center" colspan="9">
                </td>
            </tr>
            <tr>
                <td class="pm_list" align="right">
                    <asp:Label ID="Label2" runat="server" Text="Actual" />:
                </td>
                <td class="pm_list" align="center">
                    <%#sBinding.getFValue(Container.DataItem, "AttendanceRecordWorkStart","HH:mm")%>
                </td>
                <td class="pm_list" align="center">
                    <%#sBinding.getFValue(Container.DataItem, "AttendanceRecordLunchOut", "HH:mm")%>
                </td>
                <td class="pm_list" align="center">
                    <%#sBinding.getFValue(Container.DataItem, "AttendanceRecordLunchIn", "HH:mm")%>
                </td>
                <td class="pm_list" align="center">
                    <%#sBinding.getFValue(Container.DataItem, "AttendanceRecordWorkEnd", "HH:mm")%>
                </td>
                <td class="pm_list" align="right">
                    <%#sBinding.getValue(Container.DataItem, "AttendanceRecordActualLateMins")%>
                </td>
                <td class="pm_list" align="right">
                    <%#sBinding.getValue(Container.DataItem, "AttendanceRecordActualEarlyLeaveMins")%>
                </td>
                <td class="pm_list" align="right">
                    <%#sBinding.getValue(Container.DataItem, "AttendanceRecordActualLunchLateMins")%>
                </td>
                <td class="pm_list" align="right">
                    <%#sBinding.getValue(Container.DataItem, "AttendanceRecordActualLunchEarlyLeaveMins")%>
                </td>
                <td class="pm_list" align="right">
                    <%#sBinding.getValue(Container.DataItem, "AttendanceRecordActualOvertimeMins")%>
                </td>
                <td class="pm_list" align="right">
                    <%#sBinding.getValue(Container.DataItem, "AttendanceRecordActualLunchOvertimeMins")%>
                </td>
                <td class="pm_list" align="right">
                    <%#sBinding.getFValue(Container.DataItem, "AttendanceRecordActualLunchTimeMins")%>
                </td>
                <td class="pm_list" align="right">
                    <%#sBinding.getFValue(Container.DataItem, "AttendanceRecordActualWorkingDay","#,##0.000")%>
                </td>
                <td class="pm_list" align="right">
                    <%#sBinding.getFValue(Container.DataItem, "AttendanceRecordActualWorkingHour", "#,##0.000")%>
                </td>
            </tr>
            <tr>
                <td class="pm_list" align="right">
                    <asp:Label ID="Label1" runat="server" Text="Location" />:
                </td>
                <td class="pm_list" align="center">
                    <%#sBinding.getValue(Container.DataItem, "AttendanceRecordWorkStartLocation")%>
                </td>
                <td class="pm_list" align="center">
                    <%#sBinding.getValue(Container.DataItem, "AttendanceRecordLunchOutLocation")%>
                </td>
                <td class="pm_list" align="center">
                    <%#sBinding.getValue(Container.DataItem, "AttendanceRecordLunchInLocation")%>
                </td>
                <td class="pm_list" align="center">
                    <%#sBinding.getValue(Container.DataItem, "AttendanceRecordWorkEndLocation")%>
                </td>
                <td class="pm_list" align="right">
                    <asp:Label ID="ILabel3" runat="server" Text="Absent" />?
                </td>
                <td class="pm_list" align="left">
                    <%#sBinding.getValue(Container.DataItem, "AttendanceRecordIsAbsent")%>
                </td>
                <td class="pm_list" align="right" colspan="2">
                    <asp:Label ID="ILabel4" runat="server" Text="Has Bonus" />?
                </td>
                <td class="pm_list" align="left" colspan="5">
                    <asp:Label ID="AttendanceRecordHasBonus" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_list" align="right">
                    <asp:Label ID="ILabel1" runat="server" Text="Remark" />:
                </td>
                <td class="pm_list" colspan="13">
                    <%#sBinding.getValue(Container.DataItem, "AttendanceRecordRemark")%>
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr>
                <td class="pm_list_alt_row" align="center" rowspan="4">
                    <asp:CheckBox ID="DeleteItem" runat="server" />
                    <input type="hidden" runat="server" id="AttendanceRecordID" />
                </td>
                <td class="pm_list_alt_row" align="center" valign="top" rowspan="4">
                    <%#sBinding.getFValue(Container.DataItem, "AttendanceRecordDate","yyyy-MM-dd")%>
                    <asp:Button ID="Edit" CssClass="button" runat="server" Text="Edit" />
                </td>
                <td class="pm_list_alt_row" align="left" valign="top" rowspan="4">
                    <asp:Label ID="RosterCodeID" runat="server" />
                </td>
                <td class="pm_list_alt_row" align="right">
                    <asp:Label ID="Label4" runat="server" Text="Roster Time Override" />:
                </td>
                <td class="pm_list_alt_row" align="center">
                    <%#sBinding.getFValue(Container.DataItem, "AttendanceRecordRosterCodeInTimeOverride", "HH:mm")%>
                </td>
                <td class="pm_list_alt_row" align="center">
                    <%#sBinding.getFValue(Container.DataItem, "AttendanceRecordRosterCodeLunchStartTimeOverride", "HH:mm")%>
                </td>
                <td class="pm_list_alt_row" align="center">
                    <%#sBinding.getFValue(Container.DataItem, "AttendanceRecordRosterCodeLunchEndTimeOverride", "HH:mm")%>
                </td>
                <td class="pm_list_alt_row" align="center">
                    <%#sBinding.getFValue(Container.DataItem, "AttendanceRecordRosterCodeOutTimeOverride", "HH:mm")%>
                </td>
                <td class="pm_list_alt_row" align="center" colspan="9">
                </td>
            </tr>
            <tr>
                <td class="pm_list_alt_row" align="right">
                    <asp:Label ID="Label2" runat="server" Text="Actual" />:
                </td>
                <td class="pm_list_alt_row" align="center">
                    <%#sBinding.getFValue(Container.DataItem, "AttendanceRecordWorkStart","HH:mm")%>
                </td>
                <td class="pm_list_alt_row" align="center">
                    <%#sBinding.getFValue(Container.DataItem, "AttendanceRecordLunchOut", "HH:mm")%>
                </td>
                <td class="pm_list_alt_row" align="center">
                    <%#sBinding.getFValue(Container.DataItem, "AttendanceRecordLunchIn", "HH:mm")%>
                </td>
                <td class="pm_list_alt_row" align="center">
                    <%#sBinding.getFValue(Container.DataItem, "AttendanceRecordWorkEnd", "HH:mm")%>
                </td>
                <td class="pm_list_alt_row" align="right">
                    <%#sBinding.getValue(Container.DataItem, "AttendanceRecordActualLateMins")%>
                </td>
                <td class="pm_list_alt_row" align="right">
                    <%#sBinding.getValue(Container.DataItem, "AttendanceRecordActualEarlyLeaveMins")%>
                </td>
                <td class="pm_list_alt_row" align="right">
                    <%#sBinding.getValue(Container.DataItem, "AttendanceRecordActualLunchLateMins")%>
                </td>
                <td class="pm_list_alt_row" align="right">
                    <%#sBinding.getValue(Container.DataItem, "AttendanceRecordActualLunchEarlyLeaveMins")%>
                </td>
                <td class="pm_list_alt_row" align="right">
                    <%#sBinding.getValue(Container.DataItem, "AttendanceRecordActualOvertimeMins")%>
                </td>
                <td class="pm_list_alt_row" align="right">
                    <%#sBinding.getValue(Container.DataItem, "AttendanceRecordActualLunchOvertimeMins")%>
                </td>
                <td class="pm_list_alt_row" align="right">
                    <%#sBinding.getFValue(Container.DataItem, "AttendanceRecordActualLunchTimeMins")%>
                </td>
                <td class="pm_list_alt_row" align="right">
                    <%#sBinding.getFValue(Container.DataItem, "AttendanceRecordActualWorkingDay","#,##0.000")%>
                </td>
                <td class="pm_list_alt_row" align="right">
                    <%#sBinding.getFValue(Container.DataItem, "AttendanceRecordActualWorkingHour", "#,##0.000")%>
                </td>
            </tr>
            <tr>
                <td class="pm_list_alt_row" align="right">
                    <asp:Label ID="Label1" runat="server" Text="Location" />:
                </td>
                <td class="pm_list_alt_row" align="center">
                    <%#sBinding.getValue(Container.DataItem, "AttendanceRecordWorkStartLocation")%>
                </td>
                <td class="pm_list_alt_row" align="center">
                    <%#sBinding.getValue(Container.DataItem, "AttendanceRecordLunchOutLocation")%>
                </td>
                <td class="pm_list_alt_row" align="center">
                    <%#sBinding.getValue(Container.DataItem, "AttendanceRecordLunchInLocation")%>
                </td>
                <td class="pm_list_alt_row" align="center">
                    <%#sBinding.getValue(Container.DataItem, "AttendanceRecordWorkEndLocation")%>
                </td>
                <td class="pm_list_alt_row" align="right">
                    <asp:Label ID="ILabel3" runat="server" Text="Absent" />?
                </td>
                <td class="pm_list_alt_row" align="left">
                    <%#sBinding.getValue(Container.DataItem, "AttendanceRecordIsAbsent")%>
                </td>
                <td class="pm_list_alt_row" align="right" colspan="2">
                    <asp:Label ID="ILabel4" runat="server" Text="Has Bonus" />?
                </td>
                <td class="pm_list_alt_row" align="left" colspan="5">
                    <asp:Label ID="AttendanceRecordHasBonus" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_list_alt_row" align="right">
                    <asp:Label ID="ILabel1" runat="server" Text="Remark" />:
                </td>
                <td class="pm_list_alt_row" colspan="13">
                    <%#sBinding.getValue(Container.DataItem, "AttendanceRecordRemark")%>
                </td>
            </tr>
        </AlternatingItemTemplate>
    </asp:DataList>
</table>
<tb:RecordListFooter id="ListFooter" runat="server"
    MaximumRecordsPerPage="50"
    ListOrderBy="AttendanceRecordDate" ListOrder="true"
    OnFirstPageClick="ChangePage_Click"
    OnPrevPageClick="ChangePage_Click"
    OnNextPageClick="ChangePage_Click"
    OnLastPageClick="ChangePage_Click"
         
  />
<uc1:Attendance_AttendanceRecordDetail_Edit ID="Attendance_AttendanceRecordDetail_Edit1" runat="server"  OnClosed="Attendance_AttendanceRecordDetail_Closed"/>
</ContentTemplate>
</asp:UpdatePanel>
