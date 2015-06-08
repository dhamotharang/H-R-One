<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Emp_LeaveBalanceEntitle_List.ascx.cs" Inherits="Emp_LeaveBalanceEntitle_List" %>
    
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<input type="hidden" id="EmpID" runat="server" name="ID" />


<table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="0">
    <col width="26px" />
    <col width="20%" />
    <col width="20%" />
    <col width="20%" />
    <col  />
    <col width="20%" />
    <tr>
        <td class="pm_list_header" align="center">
            <asp:Panel ID="AddPanelSelectAll" runat="server" Visible="false">
            <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
            </asp:Panel> 
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_LeaveBalanceEntitleEffectiveDate" OnClick="ChangeOrder_Click" Text="Date Grant"></asp:LinkButton>
        </td>
        <td align="center" class="pm_list_header" colspan="2">
            <asp:LinkButton runat="server" ID="_LeaveBalanceEntitleGrantPeriodFrom" OnClick="ChangeOrder_Click" Text="Grant Period"></asp:LinkButton>
        </td>
        <td align="right" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_LeaveBalanceEntitleDays" OnClick="ChangeOrder_Click" Text="Day"></asp:LinkButton>
        </td>
        <td align="right" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_LeaveBalanceEntitleDateExpiry" OnClick="ChangeOrder_Click" Text="Date Expiry"></asp:LinkButton>
        </td>
    </tr>
    <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
        <ItemTemplate>
            <tr>
                <td class="pm_list" align="center">
                    <asp:CheckBox ID="ItemSelect" runat="server" Visible="false" />
                </td>
                <td class="pm_list">
                    <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Emp_LeaveBalanceEntitle_View.aspx?EmpID=" + EmpID.Value + "&LeaveBalanceEntitleID=" + sbinding.getValue(Container.DataItem,"LeaveBalanceEntitleID"))%>">
                        <%#sbinding.getFValue(Container.DataItem, "LeaveBalanceEntitleEffectiveDate")%>
                    </a>
                </td>
                <td class="pm_list" align="right" >
                    <%#sbinding.getFValue(Container.DataItem, "LeaveBalanceEntitleGrantPeriodFrom")%>
                </td>
                <td class="pm_list" align="right" >
                    <%#sbinding.getFValue(Container.DataItem, "LeaveBalanceEntitleGrantPeriodTo")%>
                </td>
                <td class="pm_list" align="right" >
                    <%#sbinding.getFValue(Container.DataItem, "LeaveBalanceEntitleDays", "0.00")%>
                </td>
                <td class="pm_list" align="right" >
                    <%#sbinding.getFValue(Container.DataItem, "LeaveBalanceEntitleDateExpiry")%>
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
    ListOrderBy="LeaveBalanceEntitleEffectiveDate"
    ListOrder="false" 
  />