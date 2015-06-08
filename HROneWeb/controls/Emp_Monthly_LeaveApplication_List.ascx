<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Emp_Monthly_LeaveApplication_List.ascx.cs"
    Inherits="Emp_Monthly_LeaveApplication_List" %>
    
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<input type="hidden" id="EmpID" runat="server" name="ID" />


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
            <asp:LinkButton runat="server" ID="_LeaveAppDateFrom" Text="From"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_LeaveAppDateTo" Text="To"></asp:LinkButton></td>
        <td align="left" class="pm_list_header" colspan="2" >
            <asp:LinkButton runat="server" ID="_LeaveAppTimeFrom" Text="Time"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_LeaveCodeID" Text="Leave Code"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_LeaveAppDays" Text="Day"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_LeaveAppUnit" Text="Type"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:Label runat="server" ID="_LeaveAppRemark" Text="Remark"></asp:Label></td>
    </tr>
    <asp:Repeater ID="Repeater" runat="server">
        <ItemTemplate>
            <tr>
                <td class="pm_list" align="center">

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
                    <%#sbinding.getValue(Container.DataItem, "LeaveCodeDesc")%>
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
