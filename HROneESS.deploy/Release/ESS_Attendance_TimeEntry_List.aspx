<%@ page language="C#" masterpagefile="~/MainMasterPage.master" autoeventwireup="true" inherits="ESS_Attendance_TimeEntry_List, HROneESS.deploy" %>
<%@ Register Src="~/controls/EmployeeSearchControl.ascx" TagName="EmployeeSearchControl" TagPrefix="uc1" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="bannerContentPlaceHolder" Runat="Server">
    <table width="898px" border="0" cellpadding="0" cellspacing="0">
      <tr>
        <td>
            <a href="ESS_Home.aspx">
                <img src="images/banner18_01.jpg" alt="" style="border-width: 0px; display : block" />
            </a>
        </td>
        <td valign="bottom" >
            <img src="images/banner18_02.jpg" alt="" style="border-width: 0px; display : block" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <img src="images/banner18_03.jpg" alt="" style="border-width: 0px; display : block" />
        </td>
      </tr>
    </table>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContentPlaceHolder" Runat="Server">
    <div style="width:600px">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate >
    <table width="100%" class="pm_section" border="0" cellpadding="0" cellspacing="0" >
    <tr>
        <td class="pm_field_header">
            <asp:Label ID="Label3" runat="server" Text="Year/Month" />:
        </td>
        <td class="pm_field">
            <asp:TextBox ID="Year" runat="server" AutoPostBack="True" OnTextChanged="YearMonth_SelectedIndexChanged" MaxLength="4" Columns="4" />
            <asp:DropDownList ID="Month" runat="server" AutoPostBack="True" OnSelectedIndexChanged="YearMonth_SelectedIndexChanged" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header">
            <asp:Label ID="Label1" runat="server" Text="Week" />:
        </td>
        <td class="pm_field">
            <input id="hiddenRosterTableGroupID" type="hidden" runat="server" />        
            <input id="hiddenRosterClientID" type="hidden" runat="server" />
            <input id="hiddenRosterClientSiteID" type="hidden" runat="server" />
            <asp:DropDownList ID="Week" runat="server" AutoPostBack="True" OnTextChanged="Week_SelectedIndexChanged"/>
        </td>
    </tr>
    <%--
    <tr>
        <td class="pm_field_header">
            <asp:Label runat="server" Text="Roster Table Group" />:
        </td>
        <td class="pm_field">
            <asp:DropDownList ID="cboRosterTableGroup" runat="server" AutoPostBack="True" OnTextChanged="cboRosterTableGroup_SelectedIndexChanged" />
        </td>
    </tr>
     --%>
    </table> 
    </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="Reset" EventName="Click" />
    </Triggers>
    <ContentTemplate >
    <uc1:EmployeeSearchControl ID="EmployeeSearchControl1" runat="server" />
    </ContentTemplate>
    </asp:UpdatePanel>
    <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
        <tr>
            <td>
                <asp:Button ID="Search" runat="server" Text="Search" CssClass="button" OnClick="Search_Click" />
                <asp:Button ID="Reset" runat="server" Text="Reset" CssClass="button" OnClick="Reset_Click" />
            </td>
        </tr>
    </table>
    <br />
    </div>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="Search" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="Reset" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="Year" EventName="TextChanged" />
        <asp:AsyncPostBackTrigger ControlID="Month" EventName="SelectedIndexChanged" />
        <asp:AsyncPostBackTrigger ControlID="Week" EventName="SelectedIndexChanged" />
        <asp:PostBackTrigger ControlID="btnExport" />
    </Triggers>
    <ContentTemplate >
    <table width="100%"border="0" cellpadding="5" cellspacing="0" class="pm_list_section">
    <tr >
        <td class="pm_list_header">
            <asp:LinkButton runat="server" ID="_EmpNo" OnClick="ChangeOrder_Click" Text="EmpNo" />
        </td>
        <td class="pm_list_header">
            <asp:LinkButton runat="server" ID="_EmpEngFullName" OnClick="ChangeOrder_Click" Text="Name" />
        </td>
        <td class="pm_list_header" align="left" >
            <asp:LinkButton runat="server" ID="_Position" OnClick="ChangeOrder_Click" Text="Position"></asp:LinkButton>
        </td>
        <td class="pm_list_header" align="center" style="white-space:nowrap" >
            <asp:Label ID="Label4" runat="server" Text="Mon" /><br />
            <asp:Label ID="AttendanceInOutDate0" runat="server" />
        </td>
        <td class="pm_list_header" align="center" style="white-space:nowrap" >
            <asp:Label ID="Label5" runat="server" Text="Tue" /><br />
            <asp:Label ID="AttendanceInOutDate1" runat="server" />
        </td>
        <td class="pm_list_header" align="center" style="white-space:nowrap" >
            <asp:Label ID="Label6" runat="server" Text="Wed" /><br />
            <asp:Label ID="AttendanceInOutDate2" runat="server" />
        </td>
        <td class="pm_list_header" align="center" style="white-space:nowrap" >
            <asp:Label ID="Label7" runat="server" Text="Thu" /><br />
            <asp:Label ID="AttendanceInOutDate3" runat="server" />
        </td>
        <td class="pm_list_header" align="center" style="white-space:nowrap" >
            <asp:Label ID="Label8" runat="server" Text="Fri" /><br />
            <asp:Label ID="AttendanceInOutDate4" runat="server" />
        </td>
        <td class="pm_list_header" align="center" style="white-space:nowrap" >
            <asp:Label ID="Label9" runat="server" Text="Sat" /><br />
            <asp:Label ID="AttendanceInOutDate5" runat="server" />
        </td>
        <td class="pm_list_header" align="center" style="white-space:nowrap" >
            <asp:Label ID="Label2" runat="server" Text="Sun" /><br />
            <asp:Label ID="AttendanceInOutDate6" runat="server" />
        </td>            
    </tr>
   
    <asp:Repeater ID="timeCardRecordRepeater" runat="server" OnItemDataBound="Repeater_ItemDataBound"  >
        <ItemTemplate>
            <tr style="background-color:#FFFFFF" class="tablecontent">
                <td class="pm_list" style="white-space:nowrap;" >
                    <input id="EmpID" type="hidden" runat="server" />
                    <%#empSBinding.getValue(Container.DataItem, "EmpNo")%>
                </td>
                <td class="pm_list" style="white-space:nowrap;" >
                    <%#empSBinding.getValue(Container.DataItem, "EmpEngFullName")%>
                </td>
                <td class="pm_list" style="white-space:nowrap;" >
                    <%#empSBinding.getValue(Container.DataItem, "Position")%>
                </td>
                <td class="pm_list" style="white-space:nowrap;" >
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                    <ContentTemplate >                    
                    <asp:Panel id="HidePanel0" runat="server">
                    <table border="0" cellpadding="0" cellspacing="0">
                     <tr>                      
                        <td align="right">RC :</td>
                        <td style="white-space:nowrap;">
                            <asp:DropDownList ID="RosterCodeID0" runat="server" width="100px" AutoPostBack="true" OnSelectedIndexChanged="AttendanceRecordRC_SelectIndexChange" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">In :</td>
                        <td style="white-space:nowrap;">
                            <asp:DropDownList ID="AttendanceRecordWorkStartHour0" runat="server" AutoPostBack="true" OnSelectedIndexChanged="AttendanceRecordWorkStart_SelectIndexChange" />:
                            <asp:DropDownList ID="AttendanceRecordWorkStartMinute0" runat="server" AutoPostBack="true" OnSelectedIndexChanged="AttendanceRecordWorkStart_SelectIndexChange"/>
                        </td>
                    </tr>                   
                    <tr>
                        <td align="right">Out :</td>
                        <td style="white-space:nowrap;">
                            <asp:DropDownList ID="AttendanceRecordWorkEndHour0" runat="server" />:
                            <asp:DropDownList ID="AttendanceRecordWorkEndMinute0" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Leave Info :</td>
                        <td style="white-space:nowrap;">
                            <asp:Label ID="LeaveInfo0" runat="server" />
                        </td>
                    </tr>
                    </table>
                    </asp:Panel>
                    </ContentTemplate>
                    </asp:UpdatePanel> 
                </td>
                <td class="pm_list" style="white-space:nowrap;">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate >
                    <asp:Panel id="HidePanel1" runat="server">
                    <table border="0" cellpadding="0" cellspacing="0">
                      <tr>
                        <td align="right">RC :</td>
                        <td style="white-space:nowrap;">
                            <asp:DropDownList ID="RosterCodeID1" runat="server"  width="100px"  AutoPostBack="true" OnSelectedIndexChanged="AttendanceRecordRC_SelectIndexChange"/>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">In :</td>
                        <td style="white-space:nowrap;">
                            <asp:DropDownList ID="AttendanceRecordWorkStartHour1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="AttendanceRecordWorkStart_SelectIndexChange"/>:
                            <asp:DropDownList ID="AttendanceRecordWorkStartMinute1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="AttendanceRecordWorkStart_SelectIndexChange"/>
                        </td>
                    </tr>                   
                    <tr>
                        <td align="right">Out :</td>
                        <td style="white-space:nowrap;">
                            <asp:DropDownList ID="AttendanceRecordWorkEndHour1" runat="server" />:
                            <asp:DropDownList ID="AttendanceRecordWorkEndMinute1" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Leave Info :</td>
                        <td style="white-space:nowrap;">
                            <asp:Label ID="LeaveInfo1" runat="server" />
                        </td>
                    </tr>
                    </table>
                    </asp:Panel>
                </ContentTemplate >
                </asp:UpdatePanel> 
                </td>
                <td class="pm_list" style="white-space:nowrap;">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                    <ContentTemplate >
                    <asp:Panel id="HidePanel2" runat="server">
                    <table border="0" cellpadding="0" cellspacing="0">
                     <tr>
                        <td align="right">RC :</td>
                        <td style="white-space:nowrap;">
                            <asp:DropDownList ID="RosterCodeID2" runat="server"  width="100px"  AutoPostBack="true" OnSelectedIndexChanged="AttendanceRecordRC_SelectIndexChange"/>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">In :</td>
                        <td style="white-space:nowrap;">
                            <asp:DropDownList ID="AttendanceRecordWorkStartHour2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="AttendanceRecordWorkStart_SelectIndexChange"/>:
                            <asp:DropDownList ID="AttendanceRecordWorkStartMinute2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="AttendanceRecordWorkStart_SelectIndexChange"/>
                        </td>
                    </tr>                  
                    <tr>
                        <td align="right">Out :</td>
                        <td style="white-space:nowrap;">
                            <asp:DropDownList ID="AttendanceRecordWorkEndHour2" runat="server" />:
                            <asp:DropDownList ID="AttendanceRecordWorkEndMinute2" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Leave Info :</td>
                        <td style="white-space:nowrap;">
                            <asp:Label ID="LeaveInfo2" runat="server" />
                        </td>
                    </tr>
                    </table>
                    </asp:Panel>
                    </ContentTemplate>
                    </asp:UpdatePanel>                    
                </td>
                <td class="pm_list" style="white-space:nowrap;">
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                    <ContentTemplate >
                    <asp:Panel id="HidePanel3" runat="server">
                    <table border="0" cellpadding="0" cellspacing="0">
                      <tr>
                        <td align="right">RC :</td>
                        <td style="white-space:nowrap;">
                            <asp:DropDownList ID="RosterCodeID3" runat="server"  width="100px"  AutoPostBack="true" OnSelectedIndexChanged="AttendanceRecordRC_SelectIndexChange"/>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">In :</td>
                        <td style="white-space:nowrap;">
                            <asp:DropDownList ID="AttendanceRecordWorkStartHour3" runat="server" AutoPostBack="true" OnSelectedIndexChanged="AttendanceRecordWorkStart_SelectIndexChange"/>:
                            <asp:DropDownList ID="AttendanceRecordWorkStartMinute3" runat="server" AutoPostBack="true" OnSelectedIndexChanged="AttendanceRecordWorkStart_SelectIndexChange"/>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Out :</td>
                        <td style="white-space:nowrap;">
                            <asp:DropDownList ID="AttendanceRecordWorkEndHour3" runat="server" />:
                            <asp:DropDownList ID="AttendanceRecordWorkEndMinute3" runat="server" />
                        </td>
                    </tr> 
                    <tr>
                        <td align="right">Leave Info :</td>
                        <td style="white-space:nowrap;">
                            <asp:Label ID="LeaveInfo3" runat="server" />
                        </td>
                    </tr>
                    </table>
                    </asp:Panel>               
                    </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td class="pm_list" style="white-space:nowrap;">
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                    <ContentTemplate >
                    <asp:Panel id="HidePanel4" runat="server">
                    <table border="0" cellpadding="0" cellspacing="0">
                       <tr>
                        <td align="right">RC :</td>
                        <td style="white-space:nowrap;">
                            <asp:DropDownList ID="RosterCodeID4" runat="server"  width="100px"  AutoPostBack="true" OnSelectedIndexChanged="AttendanceRecordRC_SelectIndexChange"/>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">In :</td>
                        <td style="white-space:nowrap;">
                            <asp:DropDownList ID="AttendanceRecordWorkStartHour4" runat="server" AutoPostBack="true" OnSelectedIndexChanged="AttendanceRecordWorkStart_SelectIndexChange"/>:
                            <asp:DropDownList ID="AttendanceRecordWorkStartMinute4" runat="server" AutoPostBack="true" OnSelectedIndexChanged="AttendanceRecordWorkStart_SelectIndexChange"/>
                        </td>
                    </tr>                  
                    <tr>
                        <td align="right">Out :</td>
                        <td style="white-space:nowrap;">
                            <asp:DropDownList ID="AttendanceRecordWorkEndHour4" runat="server" />:
                            <asp:DropDownList ID="AttendanceRecordWorkEndMinute4" runat="server" />
                        </td>
                    </tr> 
                    <tr>
                        <td align="right">Leave Info :</td>
                        <td style="white-space:nowrap;">
                            <asp:Label ID="LeaveInfo4" runat="server" />
                        </td>
                    </tr>
                    </table>
                    </asp:Panel>             
                    </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td class="pm_list" style="white-space:nowrap;">
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                    <ContentTemplate >
                    <asp:Panel id="HidePanel5" runat="server">
                    <table border="0" cellpadding="0" cellspacing="0">
                     <tr>
                        <td align="right">RC :</td>
                        <td style="white-space:nowrap;">
                            <asp:DropDownList ID="RosterCodeID5" runat="server"  width="100px"  AutoPostBack="true" OnSelectedIndexChanged="AttendanceRecordRC_SelectIndexChange"/>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">In :</td>
                        <td style="white-space:nowrap;">
                            <asp:DropDownList ID="AttendanceRecordWorkStartHour5" runat="server" AutoPostBack="true" OnSelectedIndexChanged="AttendanceRecordWorkStart_SelectIndexChange"/>:
                            <asp:DropDownList ID="AttendanceRecordWorkStartMinute5" runat="server" AutoPostBack="true" OnSelectedIndexChanged="AttendanceRecordWorkStart_SelectIndexChange"/>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Out :</td>
                        <td style="white-space:nowrap;">
                            <asp:DropDownList ID="AttendanceRecordWorkEndHour5" runat="server" />:
                            <asp:DropDownList ID="AttendanceRecordWorkEndMinute5" runat="server" />
                        </td>
                    </tr> 
                    <tr>
                        <td align="right">Leave Info :</td>
                        <td style="white-space:nowrap;">
                            <asp:Label ID="LeaveInfo5" runat="server" />
                        </td>
                    </tr>
                    </table>
                    </asp:Panel>               
                    </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td class="pm_list" style="white-space:nowrap;">
                    <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional">
                    <ContentTemplate >
                    <asp:Panel id="HidePanel6" runat="server">
                    <table border="0" cellpadding="0" cellspacing="0"> 
                    <tr>
                        <td align="right">RC :</td>
                        <td style="white-space:nowrap;">
                            <asp:DropDownList ID="RosterCodeID6" runat="server"  width="100px"  AutoPostBack="true" OnSelectedIndexChanged="AttendanceRecordRC_SelectIndexChange"/>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">In :</td>
                        <td style="white-space:nowrap;">
                            <asp:DropDownList ID="AttendanceRecordWorkStartHour6" runat="server" AutoPostBack="true" OnSelectedIndexChanged="AttendanceRecordWorkStart_SelectIndexChange"/>:
                            <asp:DropDownList ID="AttendanceRecordWorkStartMinute6" runat="server" AutoPostBack="true" OnSelectedIndexChanged="AttendanceRecordWorkStart_SelectIndexChange"/>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Out :</td>
                        <td style="white-space:nowrap;">
                            <asp:DropDownList ID="AttendanceRecordWorkEndHour6" runat="server" />:
                            <asp:DropDownList ID="AttendanceRecordWorkEndMinute6" runat="server" />
                        </td>
                    </tr>  
                    <tr>
                        <td align="right">Leave Info :</td>
                        <td style="white-space:nowrap;">
                            <asp:Label ID="LeaveInfo6" runat="server" />
                        </td>
                    </tr>
                    </table>
                    </asp:Panel>              
                    </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>   
    </table> 
    <table width="100%" cellspacing="0" cellpadding="0" class="pm_list_pagenav">
        <tr>
            <td align="right" class="pm_list_pagenav" >
                <asp:Panel ID="SubmitPanel" runat="server" style="float:left" >
                <asp:Button ID="btnCopyFromLastWeek" runat="server" Text="Copy" OnClick="btnCopyFromLastWeek_Click" />
                <asp:Button ID="btnSave" runat="server" Text="Save Only" OnClick="btnSave_Click" />
                <asp:Button ID="btnExport" runat="server" Text="Export" OnClick="btnExport_Click" />
                <asp:Button ID="btnSubmit" runat="server" Text="Save and E-mail to Main Office" OnClick="btnSubmit_Click" />
                </asp:Panel>
                <div style="float:right">
                <tb:RecordListFooter id="ListFooter" runat="server"
                     ShowAllRecords="true" 
                  />
                </div>
            </td>
        </tr>
    </table>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

