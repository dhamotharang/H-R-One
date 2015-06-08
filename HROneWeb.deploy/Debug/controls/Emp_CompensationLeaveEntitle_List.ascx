<%@ control language="C#" autoeventwireup="true" inherits="Emp_CompensationLeaveEntitle_List, HROneWeb.deploy" %>
    
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<input type="hidden" id="EmpID" runat="server" name="ID" />

<table border="0" width="100%" cellspacing="0" cellpadding="0">
    <tr>
        <td class="pm_search_header">
            <asp:Label ID="lblYear" runat="server" Text="Year" />:
            <asp:DropDownList ID="Year" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Year_SelectedIndexChanged"  />
            <asp:PlaceHolder ID="AddPanel" runat="server" >
                <asp:Button ID="New" runat="server" Text="New" CSSclass="button"  OnClick="New_Click" />
                <asp:Button ID="Delete" runat="server" CssClass="button" OnClick="Delete_Click" Text="Delete" />
            </asp:PlaceHolder>
        </td>
    </tr>
</table> 
<table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="0">
        <col width="26px" />
        <col width="100px" />
        <col width="100px" />
        <colgroup >
            <col width="50px" />
            <col width="50px" />
        </colgroup> 
        <colgroup >
            <col width="50px" />
            <col width="50px" />
        </colgroup> 
        <col width="50px" />
        <col width="150px" />
        <tr>
        <td class="pm_list_header"  align="center">
            <asp:Panel ID="AddPanelSelectAll" runat="server">
            <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
            </asp:Panel> 
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_CompensationLeaveEntitleEffectiveDate" OnClick="ChangeOrder_Click" Text="Effective Date"></asp:LinkButton>
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_CompensationLeaveEntitleDateExpiry" OnClick="ChangeOrder_Click" Text="Date Expiry"></asp:LinkButton>
        </td>
        <td align="left" class="pm_list_header" colspan="2">
            <asp:LinkButton runat="server" ID="_CompensationLeaveEntitleClaimPeriodFrom" OnClick="ChangeOrder_Click" Text="Period"></asp:LinkButton>
        </td>
         <td align="left" class="pm_list_header" colspan="2" >
            <asp:LinkButton runat="server" ID="_CompensationLeaveEntitleClaimHourFrom" OnClick="ChangeOrder_Click" Text="Time"></asp:LinkButton>
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_CompensationLeaveEntitleHoursClaim" OnClick="ChangeOrder_Click" Text="Hours Claim"></asp:LinkButton>
        </td>
        <td align="left" class="pm_list_header">
            <asp:Label runat="server" ID="_CompensationLeaveEntitleApprovedBy" Text="Approved By"></asp:Label>
        </td>
    </tr>
    <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
        <ItemTemplate>
            <tr>
                <td class="pm_list" align="center">
                    <asp:CheckBox ID="ItemSelect" runat="server" />
                </td>
                <td class="pm_list">
                    <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Emp_CompensationLeaveEntitle_View.aspx?EmpID=" + EmpID.Value + "&CompensationLeaveEntitleID=" + sbinding.getValue(Container.DataItem,"CompensationLeaveEntitleID"))%>">
                        <%#sbinding.getFValue(Container.DataItem, "CompensationLeaveEntitleEffectiveDate", "yyyy-MM-dd")%>
                    </a>
                </td>
                <td class="pm_list">
                    <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Emp_CompensationLeaveEntitle_View.aspx?EmpID=" + EmpID.Value + "&CompensationLeaveEntitleID=" + sbinding.getValue(Container.DataItem,"CompensationLeaveEntitleID"))%>">
                        <%#sbinding.getFValue(Container.DataItem, "CompensationLeaveEntitleDateExpiry", "yyyy-MM-dd")%>
                    </a>
                </td>
                <td class="pm_list">
                        <%#sbinding.getFValue(Container.DataItem, "CompensationLeaveEntitleClaimPeriodFrom", "yyyy-MM-dd")%>
                </td>
                <td class="pm_list">
                        <%#sbinding.getFValue(Container.DataItem, "CompensationLeaveEntitleClaimPeriodTo", "yyyy-MM-dd")%>
                </td>
                <td class="pm_list">
                        <%#sbinding.getFValue(Container.DataItem, "CompensationLeaveEntitleClaimHourFrom", "HH:mm")%>
                </td>
                <td class="pm_list">
                        <%#sbinding.getFValue(Container.DataItem, "CompensationLeaveEntitleClaimHourTo", "HH:mm")%>
                </td>
                <td class="pm_list">
                    <%#sbinding.getFValue(Container.DataItem,"CompensationLeaveEntitleHoursClaim","0.00")%>
                </td>
                <td class="pm_list">
                    <%#sbinding.getValue(Container.DataItem, "CompensationLeaveEntitleApprovedBy")%>
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
    ListOrderBy = "CompensationLeaveEntitleEffectiveDate"
    ListOrder = "false" 
  />