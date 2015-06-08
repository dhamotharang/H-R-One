<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Emp_LeaveApplicationCancel_List.ascx.cs" Inherits="Emp_LeaveApplicationCancel_List" %>
    
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<input type="hidden" id="EmpID" runat="server" name="ID" />

<table border="0" width="100%" cellspacing="0" cellpadding="0">
    <tr>
        <td class="pm_search_header">
            <asp:Label ID="lblYear" runat="server" Text="Year" />:
            <asp:DropDownList ID="Year" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Year_SelectedIndexChanged"  />
        </td>
    </tr>
</table> 
<table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="0">
    <col width="26px" />
    <col width="75px" />
    <col width="75px" />
    <col width="50px" />
    <col width="50px" />
    <col width="150px" />
    <col width="50px" />
    <col width="50px" />
    <tr>
        <td class="pm_list_header" align="center">
        </td>

        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_LeaveAppDateFrom" OnClick="ChangeOrder_Click" Text="From"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_LeaveAppDateTo" OnClick="ChangeOrder_Click" Text="To"></asp:LinkButton></td>
        <td align="left" class="pm_list_header" colspan="2" >
            <asp:LinkButton runat="server" ID="_LeaveAppTimeFrom" OnClick="ChangeOrder_Click" Text="Time"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_LeaveCodeID" OnClick="ChangeOrder_Click" Text="Leave Code"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_LeaveAppDays" OnClick="ChangeOrder_Click" Text="Day"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_LeaveAppUnit" OnClick="ChangeOrder_Click" Text="Type"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:Label runat="server" ID="_LeaveAppRemark" Text="Remark"></asp:Label></td>
    </tr>
    <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
        <ItemTemplate>
            <tr>
                <td class="pm_list" align="center">
                    <asp:CheckBox ID="ItemSelect" runat="server" Visible="false"  />
                </td>
                <td class="pm_list">
                    <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Emp_LeaveApplication_View.aspx?EmpID=" + EmpID.Value + "&LeaveAppID=" + sbinding.getValue(Container.DataItem,"LeaveAppID"))%>">
                        <%#sbinding.getFValue(Container.DataItem,"LeaveAppDateFrom","yyyy-MM-dd")%>
                    </a>
                </td>
                <td class="pm_list">
                        <%#sbinding.getFValue(Container.DataItem,"LeaveAppDateTo","yyyy-MM-dd")%>
                </td>
                <td class="pm_list">
                        <%#sbinding.getFValue(Container.DataItem,"LeaveAppTimeFrom","HH:mm")%>
                </td>
                <td class="pm_list">
                        <%#sbinding.getFValue(Container.DataItem, "LeaveAppTimeTo", "HH:mm")%>
                </td>
                <td class="pm_list">
                    <%#sbinding.getValue(Container.DataItem, "LeaveCodeID")%>
                </td>
                <td class="pm_list">
                    <%#sbinding.getFValue(Container.DataItem,"LeaveAppDays","0.00")%>
                </td>
                <td class="pm_list">
                    <%#sbinding.getValue(Container.DataItem,"LeaveAppUnit")%>
                </td>
                <td class="pm_list">
                    <%#sbinding.getValue(Container.DataItem,"LeaveAppRemark")%>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
<tb:RecordListFooter id="ListFooter" runat="server"
    OnFirstPageClick="FirstPage_Click"
    OnPrevPageClick="PrevPage_Click"
    OnNextPageClick="NextPage_Click"
    OnLastPageClick="LastPage_Click"
  />