<%@ control language="C#" autoeventwireup="true" inherits="LateWaiveForm, HROneESS.deploy" %>

<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<input type="hidden" id="EmpID" runat="server" />
    <table width="100%" border="0" cellpadding="5" cellspacing="0" class="pm_field_section">
        <col width="12%" />
        <col width="15%" />
        <col width="28%" />
        <col width="15%" />
        <col width="15%" />
        <col width="15%" />
        <tr >
            <td colspan="6" class="pm_field_title">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td >
                            <asp:Label ID="lblHeader" runat="server" EnableViewState="false" Text="Late Waive" />
                        </td>
                        <%--<td align="right">
                            <asp:Button ID="Save" runat="server" EnableViewState="false" Text="Submit" CssClass="button" OnClick="Save_Click" />
                        </td>--%>
                    </tr>
                </table>
            </td>
        </tr>
        <tr >
            <td class="pm_field_header">
                <asp:Label ID="Label1" Text="Period" runat="server" />:
            </td>
            <td class="pm_field" colspan="5">
                <uc1:WebDatePicker id="RequestLateWaivePeriodFrom" runat="server" AutoPostBack="true" OnChanged="Search_Click" /> -
                <uc1:WebDatePicker id="RequestLateWaivePeriodTo" runat="server" AutoPostBack="true" OnChanged="Search_Click" />
            </td>
        </tr>
        <%--<tr id="OTClaimDateToPlaceHolder" runat="server"  >
                <td class="pm_field_header">
                    <asp:Label ID="Label2" Text="Claim Time" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="RequestOTClaimHourFrom" runat="Server"  AutoPostBack="true" OnTextChanged="OTClaimTime_TextChanged" />-
                    <asp:TextBox ID="RequestOTClaimHourTo" runat="Server"  AutoPostBack="true" OnTextChanged="OTClaimTime_TextChanged" />
                </td>
        </tr>
        <tr id="TimeRow" runat="server" >
                <td class="pm_field_header" >
                    <asp:Label ID="lblOTHours" Text="No. of Hours Claim" runat="server" />:
                </td>
                <td class="pm_field" >
                    <asp:TextBox ID="RequestOTHours" runat="Server" />
                </td>
        </tr>
        <tr >
                <td class="pm_field_header">
                    <asp:Label ID="Label4" Text="Remark" runat="server" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:TextBox ID="RequestOTClaimRemark" runat="Server" TextMode="MultiLine" Columns="35" Rows="5" />
                </td>
        </tr>--%>
        <tr>
            <td class="pm_list_header">
                <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
            </td>
            <td class="pm_list_header">
                <asp:LinkButton runat="server" ID="_AttendanceRecordDate" OnClick="ChangeOrder_Click"  Text="Date" />
            </td>
            <td class="pm_list_header" align="center">
                <asp:LinkButton runat="server" ID="_RosterCodeID" OnClick="ChangeOrder_Click"  Text="Roster" />
            </td>
            <td class="pm_list_header" align="center">
                <asp:LinkButton runat="server" ID="_AttendanceRecordWorkStart" OnClick="ChangeOrder_Click"  Text="In Time" />
            </td>
            <td class="pm_list_header" align="center">
                <asp:LinkButton runat="server" ID="_AttendanceRecordWorkEnd" OnClick="ChangeOrder_Click"  Text="Out Time" />
            </td>
            <td class="pm_list_header" align="center">
                <asp:LinkButton runat="server" ID="_AttendanceRecordAcutalLateMins" OnClick="ChangeOrder_Click"  Text="Late (Mins)" />
            </td>
        </tr>
        <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
            <ItemTemplate>
                <tr>
                    <td class="pm_list">
                        <asp:CheckBox ID="ItemSelect" runat="server" />
                    </td>
                    <td class="pm_list" align="center">
                        <%#sbinding.getFValue(Container.DataItem, "AttendanceRecordDate", "yyyy-MM-dd")%>
                    </td>
                    <td class="pm_list">
                        <%#sbinding.getValue(Container.DataItem, "RosterCodeDesc")%>
                    </td>
                    <td class="pm_list" align="center">
                        <%#sbinding.getFValue(Container.DataItem, "AttendanceRecordWorkStart", "HH:mm")%>
                    </td>
                    <td class="pm_list" align="center">
                        <%#sbinding.getFValue(Container.DataItem, "AttendanceRecordWorkEnd", "HH:mm")%>
                    </td>
                    <td class="pm_list">
                        <%#sbinding.getValue(Container.DataItem, "AttendanceRecordActualLateMins")%>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
    <table width="100%" cellspacing="0" cellpadding="0" class="pm_list_pagenav">
        <tr>
            <td align="right" >
                <tb:RecordListFooter id="ListFooter" runat="server" ShowAllRecords="true" 
                  />
            </td>
        </tr>
        <tr>
            <td align="left" class="pm_list_title">
                <%--<asp:Button Text="Authorize" CssClass="button" ID="Authorize" runat="server" OnClick="Authorize_Click"  OnClientClick="return confirm('Are you sure?');" />
                <asp:Button Text="Reject" CssClass="button" ID="Reject" runat="server" OnClick="Reject_Click"  OnClientClick="return confirm('Are you sure?');" />
                <br />--%>
                <!-- Start 0000063, KuangWei, 2014-08-25 -->
                <asp:Label ID="Label2" runat="server" Text="Reason" />:<asp:TextBox ID="RequestLateWaiveReason" runat="server" Columns="50" MaxLength="100" />
                <br />
                <asp:Button Text="Submit" CssClass="button" ID="Submit" runat="server" OnClick="Save_Click"  OnClientClick="return confirm('Are you sure?');" />
                <!-- End 0000063, KuangWei, 2014-08-25 -->
            </td>
        </tr>
        <%--<tr  id="DelegateRow" runat="server" >
            <td class="pm_list_title">
                <asp:Label ID="Label3" runat="server" Text="Delegate Employee No." /> :
                <asp:TextBox ID="txtDelegateEmpNoList" runat="server" />
                <asp:Button ID="btnSaveDelegate" runat="server" Text="Save" CssClass="button" OnClick="btnSaveDelegate_Click" />
            </td>
        </tr>--%>
    </table>