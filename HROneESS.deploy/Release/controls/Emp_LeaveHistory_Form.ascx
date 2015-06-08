<%@ control language="C#" autoeventwireup="true" inherits="Emp_LeaveHistory_Form, HROneESS.deploy" %>

<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<input type="hidden" id="EmpID" runat="server" name="ID" />

<table width="100%"border="0" cellpadding="5" cellspacing="0" class="pm_list_section">
    <col width="80px" />
    <col width="80px" />
    <col width="75px" />
    <col width="75px" />
    <col width="150px" />
    <col width="75px" />
    <col width="75px" />
    <col width="190px" />
    <tr style="background-color:#FFFFFF">
        <td colspan="9" class="pm_list_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" >
                <tr>
                    <td >
                        <asp:Label ID="lblHeader" runat="server" Text="From" /> :
                        <uc1:WebDatePicker id="LeaveAppDateFrom" runat="server" ShowDateFormatLabel="false" AutoPostBack="true" OnChanged="Search_Click" /><asp:Label ID="Label1" runat="server" Text="To" />
                        <uc1:WebDatePicker id="LeaveAppDateTo" runat="server" ShowDateFormatLabel="false" AutoPostBack="true" OnChanged="Search_Click" />
                    </td>
                    <td >
                        <asp:Label ID="lblLeaveType" runat="server" Text="Leave Type" />:
                        <asp:DropDownList ID="LeaveType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Search_Click" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr >
        <td class="pm_list_header">
            <asp:LinkButton runat="server" ID="_LeaveAppDateFrom" OnClick="ChangeOrder_Click" Text="From" />
        </td>
        <td class="pm_list_header">
            <asp:LinkButton runat="server" ID="_LeaveAppDateTo" OnClick="ChangeOrder_Click" Text="To" />
        </td>
        <td class="pm_list_header" align="left" colspan="2">
            <asp:LinkButton runat="server" ID="_LeaveAppTimeFrom" OnClick="ChangeOrder_Click" Text="Time"></asp:LinkButton>
        </td>
        <td class="pm_list_header">
            <asp:LinkButton runat="server" ID="_LeaveCode" OnClick="ChangeOrder_Click"  Text="Leave Code" />
        </td>
        <%-- Start 0000094, Ricky So, 2014-09-15 --%>
        <td class="pm_list_header" align="center" colspan="2">
            <asp:LinkButton runat="server" ID="_LeaveDuration" OnClick="ChangeOrder_Click" Text="Duration" />
        </td>
<%--
        <td class="pm_list_header">
            <asp:LinkButton runat="server" ID="_LeaveAppDays" OnClick="ChangeOrder_Click" Text="Day" />
        </td>
        <td class="pm_list_header">
            <asp:LinkButton runat="server" ID="_LeaveAppUnit" OnClick="ChangeOrder_Click" Text="Type" />
        </td>
        
--%>
        <%-- End 0000094, Ricky So, 2014-09-15 --%>
        <td class="pm_list_header">
            <asp:Label runat="server" ID="_LeaveAppRemark" Text="Remark"></asp:Label>
        </td>
        <td class="pm_list_header">&nbsp</td>
    </tr>
    <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound" OnItemCommand="Repeater_ItemCommand" >
        <ItemTemplate>
            <tr style="background-color:#FFFFFF" class="tablecontent">
                <td class="pm_list" style="white-space:nowrap;" >
                    <%#binding.getFValue(Container.DataItem, "LeaveAppDateFrom", "yyyy-MM-dd")%>
                    <%#binding.getFValue(Container.DataItem, "LeaveAppDateFromAM")%>
                </td>
                <td class="pm_list" style="white-space:nowrap;" >
                    <%#binding.getFValue(Container.DataItem, "LeaveAppDateTo" , "yyyy-MM-dd")%>
                    <%#binding.getFValue(Container.DataItem, "LeaveAppDateToAM")%>
                </td>
                <td class="pm_list" style="white-space:nowrap;" >
                        <%#binding.getFValue(Container.DataItem, "LeaveAppTimeFrom", "HH:mm")%>
                </td>
                <td class="pm_list" style="white-space:nowrap;" >
                        <%#binding.getFValue(Container.DataItem, "LeaveAppTimeTo", "HH:mm")%>
                </td>
                <td class="pm_list">
                    <%#binding.getValue(Container.DataItem, "LeaveCode")%>
                    -
                    <%#binding.getValue(Container.DataItem, "LeaveCodeDesc")%>
                </td>
                <td class="pm_list" align="center">
                    <%-- Start 0000094, Ricky So, 2014-09-09 --%>
                    <asp:Label ID="LeaveAppDays" runat="server" />
                    <%--#binding.getFValue(Container.DataItem,"LeaveAppDays" , "0.00")--%>
                    <%-- End 0000094, Ricky So, 2014-09-09 --%>
                </td>
                <td class="pm_list" align="center">
                    <%#binding.getValue(Container.DataItem,"LeaveAppUnit")%>
                </td>
                <td class="pm_list">
                    <%#binding.getValue(Container.DataItem,"LeaveAppRemark")%>
                </td>
                <td class="pm_list" align="Center">
                    <asp:Button ID="Cancel" runat="server" Text="Cancel" CssClass="button" />
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
<table width="100%" cellspacing="0" cellpadding="0" class="pm_list_pagenav">
    <tr>
        <td align="right" >
            <tb:RecordListFooter id="ListFooter" runat="server"
                 ShowAllRecords="true" 
              />
        </td>
    </tr>
</table>
