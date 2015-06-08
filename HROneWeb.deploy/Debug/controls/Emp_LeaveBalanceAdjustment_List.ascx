<%@ control language="C#" autoeventwireup="true" inherits="Emp_LeaveBalanceAdjustment_List, HROneWeb.deploy" %>
<%@ Register Src="DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %> 

<input type="hidden" id="EmpID" runat="server" name="ID" />
<table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
    <tr>
        <td>
            <tb:DetailToolBar ID="toolBar" runat="server" BackButton_Visible="false" EditButton_Visible="false" SaveButton_Visible="false" OnNewButton_Click="New_Click" OnDeleteButton_Click="Delete_Click" />
        </td>
    </tr>
</table>
<table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="0">
    <col width="26px" />
    <col width="100px" />
    <col width="150px" />
    <col width="100px" />
    <col width="75px" />
    <tr>
        <td class="pm_list_header" align="center">
            <asp:Panel ID="SelectAllPanel" runat="server">
                <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
            </asp:Panel>
        </td>
        <td align="left" class="pm_list_header" >
            <asp:LinkButton runat="server" ID="_LeaveBalAdjDate" OnClick="ChangeOrder_Click" Text="Adjust Date"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_LeaveTypeID" OnClick="ChangeOrder_Click" Text="Leave Type"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_LeaveBalAdjType" OnClick="ChangeOrder_Click" Text="Adjust Type"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_LeaveBalAdjValue" OnClick="ChangeOrder_Click" Text="Adjust Value"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:Label runat="server" ID="_LeaveBalAdjRemark" Text="Remark"></asp:Label></td>
    </tr>
    <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
        <ItemTemplate>
            <tr>
                <td class="pm_list" align="center">
                    <asp:CheckBox ID="ItemSelect" runat="server" />
                </td>
                <td class="pm_list">
                    <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Emp_LeaveBalanceAdjustment_View.aspx?EmpID=" + EmpID.Value + "&LeaveBalAdjID=" + sbinding.getValue(Container.DataItem,"LeaveBalAdjID"))%>">
                        <%#sbinding.getFValue(Container.DataItem, "LeaveBalAdjDate", "yyyy-MM-dd")%>
                    </a>
                </td>
                <td class="pm_list">
                    <%#sbinding.getValue(Container.DataItem, "LeaveTypeID")%>
                </td>
                <td class="pm_list">
                    <%#sbinding.getValue(Container.DataItem, "LeaveBalAdjType")%>
                </td>
                <td class="pm_list">
                    <%#sbinding.getFValue(Container.DataItem, "LeaveBalAdjValue", "0.00")%>
                </td>
                <td class="pm_list">
                    <%#sbinding.getValue(Container.DataItem,"LeaveBalAdjRemark")%>
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