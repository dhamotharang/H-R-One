<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Emp_LateWaiveHistory_Form.ascx.cs" Inherits="Emp_LateWaiveHistory_Form" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
<input type="hidden" id="EmpID" runat="server" name="ID" />
<%--<table border="0" width="100%" cellspacing="0" cellpadding="5" class="pm_field_section">
    <tr style="background-color: #FFFFFF">
        <td class="pm_search_header">
            <asp:Label ID="lblYear" runat="server" Text="Year" />:
            <asp:DropDownList ID="Year" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Year_SelectedIndexChanged" />
        </td>
    </tr>
</table>--%>
<table width="100%" border="0" cellpadding="5" cellspacing="0" class="pm_list_section">
    <col width="10%" />
    <col width="20%" />
    <col width="10%" />
    <col width="10%" />
    <col width="10%" />
    <col width="20%" />
    <col width="20%" />
    <tr>
        <td colspan="7" class="pm_field_title">
            <asp:Label ID="Label3" Text="Late Waive History" runat="server" />
        </td>
    </tr>
    <tr>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_AttendanceRecordDate" OnClick="ChangeOrder_Click"
                Text="Date"></asp:LinkButton>
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_RosterCodeDesc" OnClick="ChangeOrder_Click"
                Text="Roster"></asp:LinkButton>
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_AttendanceRecordWorkStart" OnClick="ChangeOrder_Click"
                Text="In Time"></asp:LinkButton>
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_AttendanceRecordWorkEnd" OnClick="ChangeOrder_Click"
                Text="Out Time"></asp:LinkButton>
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_AttendanceRecordActualLateMins" OnClick="ChangeOrder_Click"
                Text="Late (Mins)"></asp:LinkButton>
        </td>
        <td align="left" class="pm_list_header">
            <asp:Label runat="server" ID="_RequestLateWaiveCreateDate" OnClick="ChangeOrder_Click" Text="Action Date"></asp:Label>
        </td>
        <td align="left" class="pm_list_header">
            <asp:Label runat="server" ID="_RequestLateWaiveReason" OnClick="ChangeOrder_Click" Text="Reason for waive"></asp:Label>
        </td>
    </tr>
    <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
        <ItemTemplate>
            <tr style="background-color: #FFFFFF" class="tablecontent">
                <td class="pm_list" style="white-space: nowrap;">
                    <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "ESS_EmpLateWaiveHistory.aspx?EmpRequestID=" + sbinding.getValue(Container.DataItem,"EmpRequestID"))%>">
                        <%#sbinding.getFValue(Container.DataItem, "AttendanceRecordDate", "yyyy-MM-dd")%>
                    </a>
                </td>
                <td class="pm_list" style="white-space: nowrap;">
                    <%#sbinding.getValue(Container.DataItem, "RosterCodeDesc")%>
                </td>
                <td class="pm_list" style="white-space: nowrap;">
                    <%#sbinding.getFValue(Container.DataItem, "AttendanceRecordWorkStart", "HH:mm")%>
                </td>
                <td class="pm_list" style="white-space: nowrap;">
                    <%#sbinding.getFValue(Container.DataItem, "AttendanceRecordWorkEnd", "HH:mm")%>
                </td>
                <td class="pm_list" style="white-space: nowrap;">
                    <%#sbinding.getValue(Container.DataItem, "AttendanceRecordActualLateMins")%>
                </td>
                <td class="pm_list" style="white-space: nowrap;">
                    <%#sbinding.getFValue(Container.DataItem, "RequestLateWaiveCreateDate", "yyyy-MM-dd HH:mm:ss")%>
                </td>
                <td class="pm_list" style="white-space: nowrap;">
                    <%#sbinding.getValue(Container.DataItem, "RequestLateWaiveReason")%>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
<table width="100%" cellspacing="0" cellpadding="5" class="pm_list_pagenav">
    <tr>
        <td align="right">
            <tb:RecordListFooter ID="ListFooter" runat="server" ShowAllRecords="true" />
        </td>
    </tr>
</table>
<br />
<table id="LateWaiveDetailsTable" runat="server" border="0" width="100%" cellspacing="0"
    cellpadding="5" class="pm_field_section">
    <col width="25%" />
    <col width="25%" />
    <col width="25%" />
    <col width="25%" />
    <tr>
        <td colspan="4" class="pm_field_title">
            <asp:Label ID="Label8" Text="Approval History" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header">
            <asp:Label ID="Label1" Text="Action Date" runat="server" />
        </td>
        <td class="pm_field">
            <%-- Start 0000112, Miranda, 2015-01-11 --%>
            <asp:Label ID="lblActionDate" runat="server" />
            <%-- End 0000112, Miranda, 2015-01-11 --%>
        </td>
        <td class="pm_field_header">
            <asp:Label ID="Label7" Text="Status" runat="server" />
        </td>
        <td class="pm_field">
            <%-- Start 0000112, Miranda, 2015-01-11 --%>
            <asp:Label ID="lblStatus" runat="server" />
            <%-- End 0000112, Miranda, 2015-01-11 --%>
        </td>
    </tr>
    <tr>
        <td class="pm_field_header">
            <%-- Start 0000112, Miranda, 2015-01-11 --%>
            <asp:Label ID="Label2" Text="Action By" runat="server" />
            <%-- End 0000112, Miranda, 2015-01-11 --%>
        </td>
        <td class="pm_field">
            <asp:Label ID="lblActionBy" runat="server" />
        </td>
        <%-- Start 0000112, Miranda, 2015-01-11 --%>
        <td class="pm_field_header">
            <asp:Label ID="Label9" Text="Reason" runat="server" />
            <%-- End 0000112, Miranda, 2015-01-11 --%>
        </td>
        <td class="pm_field">
            <asp:Label ID="lblReason" runat="server" />
        </td>
    </tr>
</table>
