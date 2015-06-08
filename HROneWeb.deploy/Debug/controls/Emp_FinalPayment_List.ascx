<%@ control language="C#" autoeventwireup="true" inherits="Emp_FinalPayment_List, HROneWeb.deploy" %>


<%@ Register Src="RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %> 

<input type="hidden" id="EmpID" runat="server" name="ID" />

<asp:Panel ID="AllowEditPanel" runat="server">
<table width="100%" class="pm_button_section" >
<tr>
    <td>
        <asp:Button ID="New" runat="server" Text="New" CSSclass="button"  OnClick="New_Click" />
        <asp:Button ID="Delete" runat="server" CssClass="button" OnClick="Delete_Click" Text="Delete" />
    </td>
    <td runat="server" id="PaymentMethodPanel" align="right">
        <asp:Label ID="lblPaymentMethod" runat="server" Text="Payment Method" />
        <asp:DropDownList ID="PaymentMethod" runat="server" >
            <asp:ListItem Text="Follow pre-defined payment method" Value="" Selected="true" />
            <asp:ListItem Text="Default autopay bank account" Value="A" />
            <asp:ListItem Text="Cheque" Value="Q" />
            <asp:ListItem Text="Cash" Value="C" />
            <asp:ListItem Text="Other" Value="O" />
        </asp:DropDownList> 
        <asp:Button ID="Generate" runat="server" CssClass="button" OnClick="Generate_Click" Text="Generate Final Payment" UseSubmitBehavior="false" />
    </td>
</tr>
</table>
</asp:Panel>
<table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
    <col width="26px" />
    <col width="250px" />
    <col width="75px" />
    <col width="75px" />
    <col width="100px" />
    <col width="75px" />
    <col width="150px" />
    <tr>
        <td class="pm_list_header" align="center">
            <asp:Panel ID="AllowEditPanel2" runat="server">
                <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
            </asp:Panel>
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_PaymentCode" OnClick="ChangeOrder_Click" Text="Payment Code" />
        </td>
        <td align="right" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_EmpFinalPayAmount" OnClick="ChangeOrder_Click" Text="Amount" />
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_EmpFinalPayMethod" OnClick="ChangeOrder_Click" Text="Pay Method" />
        </td>
        <td align="center" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_EmpFinalPayIsRestDayPayment" OnClick="ChangeOrder_Click" Text="Rest Payment" />
        </td>
        <td align="right" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_EmpFinalPayNumOfDayAdj" OnClick="ChangeOrder_Click" Text="Days Adjust" />
        </td>
        <td id="CostCenterHeaderCell" runat="server" align="left" class="pm_list_header" >
            <asp:LinkButton runat="server" ID="_CostCenterID" OnClick="ChangeOrder_Click" Text="Cost Center"></asp:LinkButton>
        </td>
    </tr>
    <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
        <ItemTemplate>
            <tr id="detailRow" runat="server" >
                <td class="pm_list" align="center" rowspan="2">
                    <asp:CheckBox ID="ItemSelect" runat="server" />
                </td>
                <td class="pm_list">
                    <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Emp_FinalPayment_View.aspx?EmpID=" + sbinding.getValue(Container.DataItem,"EmpID") + "&EmpFinalPayID=" + sbinding.getValue(Container.DataItem,"EmpFinalPayID"))%>">
                        <%#sbinding.getValue(Container.DataItem, "PayCodeID")%>
                    </a>
                </td>
                <td class="pm_list" align="right" style="white-space:nowrap;">
                    <%#sbinding.getFValue(Container.DataItem,"EmpFinalPayAmount","$0.00")%>
                </td>
                <td class="pm_list">
                    <%#sbinding.getValue(Container.DataItem,"EmpFinalPayMethod")%>
                </td>
                <td class="pm_list" align="center">
                    <%#sbinding.getValue(Container.DataItem, "EmpFinalPayIsRestDayPayment")%>
                </td>
                <td class="pm_list" align="right">
                    <%#sbinding.getFValue(Container.DataItem, "EmpFinalPayNumOfDayAdj", "0.00")%>
                </td>
                <td id="CostCenterDetailCell" runat="server" class="pm_list">
                    <asp:Label ID="CostCenterID" runat="server" />
                </td>
            </tr>
            <tr >
                <td id="RemarkCell" runat="server" class="pm_list" colspan="3">
                    <asp:Label runat="server" Text="Remark"/>:&nbsp
                    <%#sbinding.getValue(Container.DataItem, "EmpFinalPayRemark")%>
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr id="detailRow" runat="server" >
                <td class="pm_list_alt_row" align="center" rowspan="2">
                    <asp:CheckBox ID="ItemSelect" runat="server" />
                </td>
                <td class="pm_list_alt_row">
                    <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Emp_FinalPayment_View.aspx?EmpID=" + sbinding.getValue(Container.DataItem,"EmpID") + "&EmpFinalPayID=" + sbinding.getValue(Container.DataItem,"EmpFinalPayID"))%>">
                        <%#sbinding.getValue(Container.DataItem, "PayCodeID")%>
                    </a>
                </td>
                <td class="pm_list_alt_row" align="right" style="white-space:nowrap;">
                    <%#sbinding.getFValue(Container.DataItem,"EmpFinalPayAmount","$0.00")%>
                </td>
                <td class="pm_list_alt_row">
                    <%#sbinding.getValue(Container.DataItem,"EmpFinalPayMethod")%>
                </td>
                <td class="pm_list_alt_row" align="center">
                    <%#sbinding.getValue(Container.DataItem, "EmpFinalPayIsRestDayPayment")%>
                </td>
                <td class="pm_list_alt_row" align="right">
                    <%#sbinding.getFValue(Container.DataItem, "EmpFinalPayNumOfDayAdj", "0.00")%>
                </td>
                <td id="CostCenterDetailCell" runat="server" class="pm_list_alt_row">
                    <asp:Label ID="CostCenterID" runat="server" />
                </td>
            </tr>
            <tr >
                <td id="RemarkCell" runat="server" class="pm_list_alt_row" colspan="3">
                    <asp:Label ID="Label1" runat="server" Text="Remark"/>:&nbsp
                    <%#sbinding.getValue(Container.DataItem, "EmpFinalPayRemark")%>
                </td>
            </tr>
        </AlternatingItemTemplate>
    </asp:Repeater>
</table>
<tb:RecordListFooter id="ListFooter" runat="server"
    OnFirstPageClick="FirstPage_Click"
    OnPrevPageClick="PrevPage_Click"
    OnNextPageClick="NextPage_Click"
    OnLastPageClick="LastPage_Click"
  />