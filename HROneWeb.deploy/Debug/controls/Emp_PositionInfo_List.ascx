<%@ control language="C#" autoeventwireup="true" inherits="Emp_PositionInfo_List, HROneWeb.deploy" %>
<%@ Register Src="DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %> 

<input type="hidden" id="EmpID" runat="server" name="ID"/>
<table border="0" width="100%" class="pm_section_title">
    <tr>
        <td >
            <asp:Label ID="Label1" runat="server" Text="Position History" />
        </td>
    </tr>
</table>
<table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar id="toolBar" runat="server"
                     BackButton_Visible="false"
                     EditButton_Visible="false" 
                     SaveButton_Visible="false" 
                     OnNewButton_Click ="New_Click"
                     OnDeleteButton_Click="Delete_Click"
                      />
                </td>
            </tr>
        </table>
<table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
    <colgroup >
        <col width="24px" />
        <col width="75px" />
        <col width="75px" />
    </colgroup>
    <tr>
        <td class="pm_list_header" align="center">
                    <asp:Panel ID="SelectAllPanel" runat="server">
                    <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
                    </asp:Panel>           
        </td>
        <td align="left" class="pm_list_header" >
            <asp:LinkButton runat="server" ID="_EmpPosEffFr" OnClick="ChangeOrder_Click" Text="From"></asp:LinkButton></td>
        <td align="left" class="pm_list_header" >
            <asp:LinkButton runat="server" ID="_EmpPosEffTo" OnClick="ChangeOrder_Click" Text="To"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_CompanyCode" OnClick="ChangeOrder_Click" Text="Company"></asp:LinkButton></td>
<%--        <td align="left" class="pm_list_header" >
            <asp:LinkButton runat="server" ID="_BusinessHierarchy" OnClick="ChangeOrder_Click" Text="Business Hierarchy"></asp:LinkButton></td>
--%>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_PositionCode" OnClick="ChangeOrder_Click" Text="Position"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_RankCode" OnClick="ChangeOrder_Click" Text="Rank"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_StaffTypeCode" OnClick="ChangeOrder_Click" Text="Staff Type"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_LeavePlanCode" OnClick="ChangeOrder_Click" Text="Leave Plan"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_PayGroupCode" OnClick="ChangeOrder_Click" Text="Payroll Group"></asp:LinkButton></td>
    </tr>
    <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
        <ItemTemplate>
            <tr>
                <td class="pm_list" align="center">
                    <asp:CheckBox ID="ItemSelect" runat="server" />
                </td>
                <td class="pm_list">
                    <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Emp_PositionInfo_View.aspx?EmpID=" + sbinding.getValue(Container.DataItem,"EmpID") + "&EmpPosID=" + sbinding.getValue(Container.DataItem,"EmpPosID"))%>">
                    <%#sbinding.getFValue(Container.DataItem,"EmpPosEffFr")%>
                    </a>
                </td>
                <td class="pm_list">
                    <%#sbinding.getFValue(Container.DataItem,"EmpPosEffTo")%>
                </td>
                <td class="pm_list">
                    <asp:Label ID="CompanyID" runat="server" />
                </td>
<%--                <td class="pm_list" visible="false">
                    <%#sbinding.getFValue(Container.DataItem,"BusinessHierarchy")%>
                </td>
--%>
                <td class="pm_list">
                    <asp:Label ID="PositionID" runat="server" />
                </td>
                <td class="pm_list">
                    <asp:Label ID="RankID" runat="server" />
                </td>
                <td class="pm_list">
                    <asp:Label ID="StaffTypeID" runat="server" />
                </td>
                <td class="pm_list">
                    <asp:Label ID="LeavePlanID" runat="server" />
                </td>
                <td class="pm_list">
                    <asp:Label ID="PayGroupID" runat="server" />
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