<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Emp_RecurringPayment_List.ascx.cs"
    Inherits="Emp_RecurringPayment_List" %>
<%@ Register Src="DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %> 

<input type="hidden" id="EmpID" runat="server" name="ID" value="0" />
<input type="hidden" id="PayCodeID" runat="server" />
<input type="hidden" id="ShowHistoryFlag" runat="server" />
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
    <col width="26px" />            <%-- checkbox --%>
    <col span="2" width="75px" />   <%-- from & to--%>
    <col />                         <%-- payment code --%> 
    <col width="75px" />            <%-- amount --%> 
    <col width="10px" />
    <col width="120px" />           <%-- Unit --%>
 
    <tr id="HeaderRow" runat="server" >
        <td class="pm_list_header" align="center">
            <asp:Panel ID="SelectAllPanel" runat="server">
                <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
            </asp:Panel>
        </td>
        <td align="left" class="pm_list_header" >
            <asp:LinkButton runat="server" ID="_EmpRPEffFr" OnClick="ChangeOrder_Click" Text="From"></asp:LinkButton>
        </td>
        <td align="left" class="pm_list_header" >
            <asp:LinkButton runat="server" ID="_EmpRPEffTo" OnClick="ChangeOrder_Click" Text="To"></asp:LinkButton>
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_PaymentCode" OnClick="ChangeOrder_Click" Text="Payment Code"></asp:LinkButton>
        </td>
        <td align="right" class="pm_list_header" >
            <asp:LinkButton runat="server" ID="_EmpRPAmount" OnClick="ChangeOrder_Click" Text="Amount"></asp:LinkButton>
        </td>
        <td align="left" class="pm_list_header" />
        <td align="left" class="pm_list_header" >
            <asp:LinkButton runat="server" ID="_EmpRPUnit" OnClick="ChangeOrder_Click" Text="Unit"></asp:LinkButton>
        </td>
        <td align="left" class="pm_list_header" >
            <asp:LinkButton runat="server" ID="_EmpRPMethod" OnClick="ChangeOrder_Click" Text="Method"></asp:LinkButton>
        </td>
        <td id="CostCenterHeaderCell" runat="server" align="left" class="pm_list_header" >
            <asp:LinkButton runat="server" ID="_CostCenterCode" OnClick="ChangeOrder_Click" Text="Cost Center"></asp:LinkButton>
        </td>
    </tr>
    <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
        <ItemTemplate>
            <tr>
                <td class="pm_list" align="center">
                    <asp:CheckBox ID="ItemSelect" runat="server" />
                </td>
                <td class="pm_list" >
                    <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Emp_RecurringPayment_View.aspx?EmpID=" + sbinding.getValue(Container.DataItem,"EmpID") + "&EmpRPID=" + sbinding.getValue(Container.DataItem,"EmpRPID"))%>">
                        <%#sbinding.getFValue(Container.DataItem,"EmpRPEffFr", "yyyy-MM-dd")%>
                    </a>
                </td>
                <td class="pm_list" style="white-space:nowrap;">
                    <%#sbinding.getFValue(Container.DataItem,"EmpRPEffTo", "yyyy-MM-dd")%>
                </td>
                <td class="pm_list" >
                    <%#sbinding.getValue(Container.DataItem,"PayCodeID")%>
                </td>
                <td class="pm_list" align="right" style="white-space:nowrap;">
                    <%#sbinding.getFValue(Container.DataItem,"EmpRPAmount","$0.00")%>
                </td>
                <td class="pm_list" align="right" style="white-space:nowrap;"/>                
                <td class="pm_list">
                    <%#sbinding.getValue(Container.DataItem,"EmpRPUnit")%>
                </td>
                <td class="pm_list" style="white-space:nowrap;">
                    <%#sbinding.getValue(Container.DataItem,"EmpRPMethod")%>
                </td>
                <td id="CostCenterDetailCell" runat="server" class="pm_list" >
                    <asp:Label ID="CostCenterID" runat="server" />
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    <tr class="pm_list_pagenav">
        <td align="center" colspan="1">
            <asp:ImageButton ID="btnShowAll" runat="server" ImageUrl="~/images/Display_all_records_new1.png" ToolTip="Display All Records" CssClass="button" OnClick="btnShowAll_Click" />
            <asp:ImageButton ID="btnShowLatestOnly" runat="server" ImageUrl="~/images/Display_latest_record_new1.png" ToolTip="Display Latest Record(s) Only" CssClass="button" OnClick="btnShowLatestOnly_Click"/>
        </td> 
        <td id="FooterCell" runat="server" align="right" colspan="7">
            <tb:RecordListFooter id="ListFooter" runat="server"
                OnFirstPageClick="FirstPage_Click"
                OnPrevPageClick="PrevPage_Click"
                OnNextPageClick="NextPage_Click"
                OnLastPageClick="LastPage_Click"
                ListOrderBy="EmpRPEffFr" ListOrder="false"   
              />
          </td>
    </tr>
</table>
