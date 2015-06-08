<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Attendance_Record_Import.aspx.cs"    Inherits="Attendance_Record_Import" MasterPageFile="~/MainMasterPage.master" EnableViewState="false"  %>

<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="ILabel1" Text="Import Attendance Record" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="ILabel2" Text="Import Details" runat="server" />
                </td>
            </tr>
        </table>
        
            
                
        
        <table class="pm_section" width="100%">
            <tr>
                <td class="pm_search_header">
                    <asp:Label runat="server" Text="Upload file path" />:</td>
                <td class="pm_search">
                    <asp:FileUpload ID="CNDImportFile" runat="server" Width="400" />
                    <asp:Button ID="Upload" runat="server" Text="Upload" OnClick="Upload_Click"  CssClass="button"/>
                </td>
                <td class="pm_search">
                </td>
                <td>
                    <asp:Label ID="connString" runat="server" />
                </td>
            </tr>
        </table>
        <asp:Panel ID="ImportSection" runat="server" Visible="false" >
            <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
                <col width="26px" />
                <col width="50px" />
                <col width="60px" />
                <col width="50px" />
                <col width="75px" />
                <col width="75px" />
                <col width="75px" />
                <col width="50px" />
                <col width="50px" />
                <col width="50px" />
                <col width="50px" />
                <col width="50px" />
                <tr>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="_EmpNo" OnClick="ChangeOrder_Click" Text="EmpNo" />
                    </td>
                    <td align="left" class="pm_list_header"  colspan="2">
                        <asp:LinkButton runat="server" ID="_EmpEngFullName" OnClick="ChangeOrder_Click" Text="Name" />
                    </td>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="_EmpAlias" OnClick="ChangeOrder_Click" Text="Alias" />
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
                <asp:DataList ID="Repeater" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal"
                    ShowFooter="false" ShowHeader="false" OnItemDataBound="Repeater_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td class="pm_list" rowspan="4">
                                    <%#sbinding.getValue(Container.DataItem,"EmpNo")%>
                            </td>
                            <td class="pm_list" rowspan="4">
                                <%#sbinding.getValue(Container.DataItem, "EmpEngSurname")%>
                            </td>
                            <td class="pm_list" rowspan="4">
                                <%#sbinding.getValue(Container.DataItem, "EmpEngOtherName")%>
                            </td>
                            <td class="pm_list" rowspan="4">
                                <%#sbinding.getValue(Container.DataItem, "EmpAlias")%>
                            </td>
                            <td class="pm_list" rowspan="4" style="white-space:nowrap">
                                <%#sbinding.getFValue(Container.DataItem, "AttendanceRecordDate", "yyyy-MM-dd")%>
                            </td>
                            <td class="pm_list" align="left" valign="top" rowspan="4">
                                <asp:Label ID="RosterCodeID" runat="server" />
                            </td>
                            <td class="pm_list" align="right">
                                <asp:Label ID="Label4" runat="server" Text="Roster Time Override" />:
                            </td>
                            <td class="pm_list" align="center">
                                <%#sbinding.getFValue(Container.DataItem, "AttendanceRecordRosterCodeInTimeOverride", "HH:mm")%>
                            </td>
                            <td class="pm_list" align="center">
                                <%#sbinding.getFValue(Container.DataItem, "AttendanceRecordRosterCodeLunchStartTimeOverride", "HH:mm")%>
                            </td>
                            <td class="pm_list" align="center">
                                <%#sbinding.getFValue(Container.DataItem, "AttendanceRecordRosterCodeLunchEndTimeOverride", "HH:mm")%>
                            </td>
                            <td class="pm_list" align="center">
                                <%#sbinding.getFValue(Container.DataItem, "AttendanceRecordRosterCodeOutTimeOverride", "HH:mm")%>
                            </td>
                            <td class="pm_list" align="center" colspan="9">
                            </td>
                        </tr>
                        <tr>
                            <td class="pm_list" align="right">
                                <asp:Label ID="Label2" runat="server" Text="Actual" />:
                            </td>
                            <td class="pm_list" align="center">
                                <%#sbinding.getFValue(Container.DataItem, "AttendanceRecordWorkStart", "HH:mm")%>
                            </td>
                            <td class="pm_list" align="center">
                                <%#sbinding.getFValue(Container.DataItem, "AttendanceRecordLunchOut", "HH:mm")%>
                            </td>
                            <td class="pm_list" align="center">
                                <%#sbinding.getFValue(Container.DataItem, "AttendanceRecordLunchIn", "HH:mm")%>
                            </td>
                            <td class="pm_list" align="center">
                                <%#sbinding.getFValue(Container.DataItem, "AttendanceRecordWorkEnd", "HH:mm")%>
                            </td>
                            <td class="pm_list" align="right">
                                <%#sbinding.getValue(Container.DataItem, "AttendanceRecordActualLateMins")%>
                            </td>
                            <td class="pm_list" align="right">
                                <%#sbinding.getValue(Container.DataItem, "AttendanceRecordActualEarlyLeaveMins")%>
                            </td>
                            <td class="pm_list" align="right">
                                <%#sbinding.getValue(Container.DataItem, "AttendanceRecordActualLunchLateMins")%>
                            </td>
                            <td class="pm_list" align="right">
                                <%#sbinding.getValue(Container.DataItem, "AttendanceRecordActualLunchEarlyLeaveMins")%>
                            </td>
                            <td class="pm_list" align="right">
                                <%#sbinding.getValue(Container.DataItem, "AttendanceRecordActualOvertimeMins")%>
                            </td>
                            <td class="pm_list" align="right">
                                <%#sbinding.getValue(Container.DataItem, "AttendanceRecordActualLunchOvertimeMins")%>
                            </td>
                            <td class="pm_list" align="right">
                                <%#sbinding.getFValue(Container.DataItem, "AttendanceRecordActualLunchTimeMins")%>
                            </td>
                            <td class="pm_list" align="right">
                                <%#sbinding.getFValue(Container.DataItem, "AttendanceRecordActualWorkingDay", "#,##0.000")%>
                            </td>
                            <td class="pm_list" align="right">
                                <%#sbinding.getFValue(Container.DataItem, "AttendanceRecordActualWorkingHour", "#,##0.000")%>
                            </td>
                        </tr>
                        <tr>
                            <td class="pm_list" align="right">
                                <asp:Label ID="Label1" runat="server" Text="Location" />:
                            </td>
                            <td class="pm_list" align="center">
                                <%#sbinding.getValue(Container.DataItem, "AttendanceRecordWorkStartLocation")%>
                            </td>
                            <td class="pm_list" align="center">
                                <%#sbinding.getValue(Container.DataItem, "AttendanceRecordLunchOutLocation")%>
                            </td>
                            <td class="pm_list" align="center">
                                <%#sbinding.getValue(Container.DataItem, "AttendanceRecordLunchInLocation")%>
                            </td>
                            <td class="pm_list" align="center">
                                <%#sbinding.getValue(Container.DataItem, "AttendanceRecordWorkEndLocation")%>
                            </td>
                            <td class="pm_list" align="right">
                                <asp:Label ID="ILabel3" runat="server" Text="Absent" />?
                            </td>
                            <td class="pm_list" align="left">
                                <%#sbinding.getValue(Container.DataItem, "AttendanceRecordIsAbsent")%>
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
                                <%#sbinding.getValue(Container.DataItem, "AttendanceRecordRemark")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr>
                            <td class="pm_list_alt_row" rowspan="4">
                                    <%#sbinding.getValue(Container.DataItem,"EmpNo")%>
                            </td>
                            <td class="pm_list_alt_row" rowspan="4">
                                <%#sbinding.getValue(Container.DataItem, "EmpEngSurname")%>
                            </td>
                            <td class="pm_list_alt_row" rowspan="4">
                                <%#sbinding.getValue(Container.DataItem, "EmpEngOtherName")%>
                            </td>
                            <td class="pm_list_alt_row" rowspan="4">
                                <%#sbinding.getValue(Container.DataItem, "EmpAlias")%>
                            </td>
                            <td class="pm_list_alt_row" rowspan="4" style="white-space:nowrap">
                                <%#sbinding.getFValue(Container.DataItem, "AttendanceRecordDate", "yyyy-MM-dd")%>
                            </td>
                            <td class="pm_list_alt_row" align="left" valign="top" rowspan="4">
                                <asp:Label ID="RosterCodeID" runat="server" />
                            </td>
                            <td class="pm_list_alt_row" align="right">
                                <asp:Label ID="Label4" runat="server" Text="Roster Time Override" />:
                            </td>
                            <td class="pm_list_alt_row" align="center">
                                <%#sbinding.getFValue(Container.DataItem, "AttendanceRecordRosterCodeInTimeOverride", "HH:mm")%>
                            </td>
                            <td class="pm_list_alt_row" align="center">
                                <%#sbinding.getFValue(Container.DataItem, "AttendanceRecordRosterCodeLunchStartTimeOverride", "HH:mm")%>
                            </td>
                            <td class="pm_list_alt_row" align="center">
                                <%#sbinding.getFValue(Container.DataItem, "AttendanceRecordRosterCodeLunchEndTimeOverride", "HH:mm")%>
                            </td>
                            <td class="pm_list_alt_row" align="center">
                                <%#sbinding.getFValue(Container.DataItem, "AttendanceRecordRosterCodeOutTimeOverride", "HH:mm")%>
                            </td>
                            <td class="pm_list_alt_row" align="center" colspan="9">
                            </td>
                        </tr>
                        <tr>
                            <td class="pm_list_alt_row" align="right">
                                <asp:Label ID="Label2" runat="server" Text="Actual" />:
                            </td>
                            <td class="pm_list_alt_row" align="center">
                                <%#sbinding.getFValue(Container.DataItem, "AttendanceRecordWorkStart", "HH:mm")%>
                            </td>
                            <td class="pm_list_alt_row" align="center">
                                <%#sbinding.getFValue(Container.DataItem, "AttendanceRecordLunchOut", "HH:mm")%>
                            </td>
                            <td class="pm_list_alt_row" align="center">
                                <%#sbinding.getFValue(Container.DataItem, "AttendanceRecordLunchIn", "HH:mm")%>
                            </td>
                            <td class="pm_list_alt_row" align="center">
                                <%#sbinding.getFValue(Container.DataItem, "AttendanceRecordWorkEnd", "HH:mm")%>
                            </td>
                            <td class="pm_list_alt_row" align="right">
                                <%#sbinding.getValue(Container.DataItem, "AttendanceRecordActualLateMins")%>
                            </td>
                            <td class="pm_list_alt_row" align="right">
                                <%#sbinding.getValue(Container.DataItem, "AttendanceRecordActualEarlyLeaveMins")%>
                            </td>
                            <td class="pm_list_alt_row" align="right">
                                <%#sbinding.getValue(Container.DataItem, "AttendanceRecordActualLunchLateMins")%>
                            </td>
                            <td class="pm_list_alt_row" align="right">
                                <%#sbinding.getValue(Container.DataItem, "AttendanceRecordActualLunchEarlyLeaveMins")%>
                            </td>
                            <td class="pm_list_alt_row" align="right">
                                <%#sbinding.getValue(Container.DataItem, "AttendanceRecordActualOvertimeMins")%>
                            </td>
                            <td class="pm_list_alt_row" align="right">
                                <%#sbinding.getValue(Container.DataItem, "AttendanceRecordActualLunchOvertimeMins")%>
                            </td>
                            <td class="pm_list_alt_row" align="right">
                                <%#sbinding.getFValue(Container.DataItem, "AttendanceRecordActualLunchTimeMins")%>
                            </td>
                            <td class="pm_list_alt_row" align="right">
                                <%#sbinding.getFValue(Container.DataItem, "AttendanceRecordActualWorkingDay", "#,##0.000")%>
                            </td>
                            <td class="pm_list_alt_row" align="right">
                                <%#sbinding.getFValue(Container.DataItem, "AttendanceRecordActualWorkingHour", "#,##0.000")%>
                            </td>
                        </tr>
                        <tr>
                            <td class="pm_list_alt_row" align="right">
                                <asp:Label ID="Label1" runat="server" Text="Location" />:
                            </td>
                            <td class="pm_list_alt_row" align="center">
                                <%#sbinding.getValue(Container.DataItem, "AttendanceRecordWorkStartLocation")%>
                            </td>
                            <td class="pm_list_alt_row" align="center">
                                <%#sbinding.getValue(Container.DataItem, "AttendanceRecordLunchOutLocation")%>
                            </td>
                            <td class="pm_list_alt_row" align="center">
                                <%#sbinding.getValue(Container.DataItem, "AttendanceRecordLunchInLocation")%>
                            </td>
                            <td class="pm_list_alt_row" align="center">
                                <%#sbinding.getValue(Container.DataItem, "AttendanceRecordWorkEndLocation")%>
                            </td>
                            <td class="pm_list_alt_row" align="right">
                                <asp:Label ID="ILabel3" runat="server" Text="Absent" />?
                            </td>
                            <td class="pm_list_alt_row" align="left">
                                <%#sbinding.getValue(Container.DataItem, "AttendanceRecordIsAbsent")%>
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
                                <%#sbinding.getValue(Container.DataItem, "AttendanceRecordRemark")%>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:DataList>
            </table>
            <table width="100%" cellspacing="0" cellpadding="0" class="pm_list_pagenav">
                <tr>
                    <td>
                        <asp:Button ID="Import" Text="Import" runat="server" OnClick="Import_Click" CssClass="button"/>                    
                        <asp:CheckBox ID="Recalculate" runat="server" Text="Recalculate result from IN/OUT record" CssClass="pm_link_pagenav" />
                    </td>
                    <td align="right">
                    <tb:RecordListFooter id="ListFooter" runat="server"
                        OnFirstPageClick="ChangePage"
                        OnPrevPageClick="ChangePage"
                        OnNextPageClick="ChangePage"
                        OnLastPageClick="ChangePage"
                      />
                    </td>
                </tr>
            </table>            
        </asp:Panel>
</asp:Content> 