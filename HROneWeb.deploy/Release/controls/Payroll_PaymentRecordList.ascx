<%@ control language="C#" autoeventwireup="true" inherits="Payroll_PaymentRecordList, HROneWeb.deploy" %>

<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<input type="hidden" id="EmpPayrollID" runat="server" name="ID" />
<input type="hidden" id="EmpPayStatus" runat="server" />

<table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_section_title">
    <tr>
        <td>
            <asp:Label runat="server" Text="Payment Detail" />:
        </td>
    </tr>
</table>
<table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
    <tr>
        <td>
            <asp:Button ID="Delete" CssClass="button" runat="server" Text="Delete" OnClick="Delete_Click" />
        </td>
    </tr>
</table>
<table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
    <col width="26px" /> 
    <col width="60px" /> 
    <col width="300px" />
    <col width="75px" />
    <col width="75px" />
    <col width="160px" />
    <tr>
        <td class="pm_list_header" >
        </td>
        <td class="pm_list_header" >
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_PaymentCodeID" OnClick="ChangeOrder_Click" Text="Payment" />
        </td>
        <!--            <td align="left" class="pm_list_header" >
            </td>-->
        <td align="right" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_PayRecActAmount" OnClick="ChangeOrder_Click" Text="Amount" />
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_PayRecMethod" OnClick="ChangeOrder_Click" Text="Method" />
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_EmpAccID" OnClick="ChangeOrder_Click" Text="Bank Account Number" />
        </td>
        <td id="CostCenterHeaderCell" runat="server" align="left" class="pm_list_header" style="width:10%;">
            <asp:LinkButton runat="server" ID="_CostCenterID" OnClick="ChangeOrder_Click" Text="Cost Center"></asp:LinkButton>
        </td>
        <td align="right" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_PayRecNumOfDayAdj" OnClick="ChangeOrder_Click" Text="Days Adjust" />
        </td>
        <!--            <td align="left" class="pm_list_header">
                <asp:LinkButton runat="server" ID="_CNDRemark" OnClick="ChangeOrder_Click">Remark</asp:LinkButton>
            </td>-->
    </tr>
    <asp:Panel ID="AddPanel" runat="server">
        <tr id="detailRow" runat="server">
            <td class="pm_list_alt_row"  rowspan="2">
            </td>
            <td class="pm_list_alt_row"  align="center" rowspan="2">
                <asp:Button ID="Add" runat="server" Text="Add" CssClass="button" OnClick="Add_Click" />
            </td>
            <td class="pm_list_alt_row" >
                <asp:DropDownList ID="PaymentCodeID" runat="server" Width="300px" />
            </td>
            <td class="pm_list_alt_row"  align="right" visible="false" >
                <asp:DropDownList ID="CurrencyID" runat="server" />
            </td>
            <td class="pm_list_alt_row"  align="right">
                <asp:TextBox ID="PayRecActAmount" runat="server" Columns="10" style="text-align:right" />
            </td>
            <td class="pm_list_alt_row"  align="center">
                <asp:DropDownList ID="PayRecMethod" runat="server">
                </asp:DropDownList>
            </td>
            <td class="pm_list_alt_row"  align="center">
                <asp:DropDownList ID="EmpAccID" runat="server" Width="160px" />
            </td>
            <td  id="CostCenterDetailCell" runat="server" class="pm_list_alt_row" >
                <asp:DropDownList ID="CostCenterID" runat="server" Width="150px"  />
            </td>
            <td class="pm_list_alt_row"  align="right">
                <asp:TextBox ID="PayRecNumOfDayAdj" runat="server" Columns="3" style="text-align:right"/>
            </td>
        </tr>
        <tr>
            <td id="RemarkCell" runat="server" class="pm_list_alt_row"  align="left" colspan="4">
                <asp:Label ID="ILabel2" runat="server" Text="Remark" />:&nbsp
                <asp:TextBox ID="PayRecRemark" runat="server" Columns="75" />
            </td>
            <td class="pm_list_alt_row"  align="left" colspan="2">
                <asp:CheckBox ID="PayRecIsRestDayPayment" runat="server" Text="Rest Payment" />
            </td>
        </tr>
    </asp:Panel>
    <asp:DataList ID="Repeater" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" ShowFooter="false" ShowHeader="false" OnItemCommand="Repeater_ItemCommand" OnItemDataBound="Repeater_ItemDataBound">
        <EditItemTemplate>
            <tr id="detailRow" runat="server">
                <td class="pm_list_edit"  align="center" rowspan="2">
                    <asp:CheckBox ID="DeleteItem" runat="server" />
                    <input type="hidden" runat="server" id="PayRecID" />
                </td>
                <td class="pm_list_edit"  align="center" rowspan="2">
                    <asp:Button ID="Save" CssClass="button" runat="server" Text="Save" />
                    <asp:Button ID="Cancel" CssClass="button" runat="server" Text="Cancel" />
                </td>
                <td class="pm_list_edit" >
                    <asp:DropDownList ID="PaymentCodeID" runat="server" Width="300px" />
                </td>
                <td class="pm_list_edit"  align="right" visible="false" >
                    <asp:DropDownList ID="CurrencyID" runat="server" />
                </td>
                <td class="pm_list_edit"  align="right">
                    <asp:TextBox ID="PayRecActAmount" runat="server" Columns="10" style="text-align:right"/>
                </td>
                <td class="pm_list_edit" >
                    <asp:DropDownList ID="PayRecMethod" runat="server" />
                </td>
                <td class="pm_list_edit" >
                    <asp:DropDownList ID="EmpAccID" runat="server" Width="160px"/>
                </td>
                <td  id="CostCenterDetailCell" runat="server" class="pm_list_edit" >
                    <asp:DropDownList ID="CostCenterID" runat="server" Width="150px" />
                </td>
                <td class="pm_list_edit"  align="right">
                    <asp:TextBox ID="PayRecNumOfDayAdj" runat="server" Columns="3" style="text-align:right"/>
                </td>
            </tr>
            <tr>
                <td id="RemarkCell" runat="server" class="pm_list_edit"  align="left" colspan="4">
                    <asp:Label ID="ILabel2" runat="server" Text="Remark" />:&nbsp
                    <asp:TextBox ID="PayRecRemark" runat="server" Columns="75" />
                </td>
                <td class="pm_list_edit"  align="left" colspan="2">
                    <asp:CheckBox ID="PayRecIsRestDayPayment" runat="server" Text="Rest Payment" />
                </td>
            </tr>
        </EditItemTemplate>
        <ItemTemplate>
            <tr id="detailRow" runat="server">
                <td class="pm_list"  align="center" rowspan="2">
                    <asp:CheckBox ID="DeleteItem" runat="server" />
                    <input type="hidden" runat="server" id="PayRecID" />
                </td>
                <td class="pm_list"  align="center" rowspan="2">
                    <asp:Button ID="Edit" CssClass="button" runat="server" Text="Edit" />
                </td>
                <td class="pm_list" >
                    <%#sBinding.getValue(Container.DataItem,"PaymentCodeID")%>
                </td>
                <td class="pm_list"  align="right" visible="false" >
                    <%#sBinding.getValue(Container.DataItem, "CurrencyID")%>
                </td>
                <td class="pm_list"  align="right">
                    <%#sBinding.getFValue(Container.DataItem, "PayRecActAmount", "$#,##0.00")%>
                </td>
                <td class="pm_list"  align="center">
                    <%#sBinding.getValue(Container.DataItem, "PayRecMethod")%>
                </td>
                <td class="pm_list" >
                    <asp:Label ID= "EmpAccID" runat="server" />
                </td>
                <td id="CostCenterDetailCell" runat="server" class="pm_list" >
                    <asp:Label ID="CostCenterID" runat="server" />
                </td>
                <td class="pm_list"  align="right">
                    <%#sBinding.getFValue(Container.DataItem, "PayRecNumOfDayAdj", "0.0")%>
                </td>
            </tr>
            <tr>
                <td id="RemarkCell" runat="server" class="pm_list"  align="left" colspan="4">
                    <asp:Label ID="ILabel1" runat="server" Text="Remark" />:&nbsp
                    <%#sBinding.getValue(Container.DataItem, "PayRecRemark")%>
                </td>
                <td class="pm_list"  align="left" colspan="2">
                    <asp:Label ID="Label1" runat="server" Text="Rest Payment"/>?&nbsp
                    <%#sBinding.getValue(Container.DataItem, "PayRecIsRestDayPayment")%>
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr id="detailRow" runat="server">
                <td class="pm_list_alt_row"  align="center" rowspan="2">
                    <asp:CheckBox ID="DeleteItem" runat="server" />
                    <input type="hidden" runat="server" id="PayRecID" />
                </td>
                <td class="pm_list_alt_row"  align="center" rowspan="2">
                    <asp:Button ID="Edit" CssClass="button" runat="server" Text="Edit" />
                </td>
                <td class="pm_list_alt_row" >
                    <%#sBinding.getValue(Container.DataItem,"PaymentCodeID")%>
                </td>
                <td class="pm_list_alt_row"  align="right" visible="false" >
                    <%#sBinding.getValue(Container.DataItem, "CurrencyID")%>
                </td>
                <td class="pm_list_alt_row"  align="right">
                    <%#sBinding.getFValue(Container.DataItem, "PayRecActAmount", "$#,##0.00")%>
                </td>
                <td class="pm_list_alt_row"  align="center">
                    <%#sBinding.getValue(Container.DataItem, "PayRecMethod")%>
                </td>
                <td class="pm_list_alt_row" >
                    <asp:Label ID= "EmpAccID" runat="server" />
                </td>
                <td id="CostCenterDetailCell" runat="server" class="pm_list_alt_row" >
                    <asp:Label ID="CostCenterID" runat="server" />
                </td>
                <td class="pm_list_alt_row"  align="right">
                    <%#sBinding.getFValue(Container.DataItem, "PayRecNumOfDayAdj", "0.0")%>
                </td>
            </tr>
            <tr>
                <td id="RemarkCell" runat="server" class="pm_list_alt_row"  align="left" colspan="4">
                    <asp:Label ID="ILabel1" runat="server" Text="Remark" />:&nbsp
                    <%#sBinding.getValue(Container.DataItem, "PayRecRemark")%>
                </td>
                <td class="pm_list_alt_row"  align="left" colspan="2">
                    <asp:Label ID="Label1" runat="server" Text="Rest Payment"/>?&nbsp
                    <%#sBinding.getValue(Container.DataItem, "PayRecIsRestDayPayment")%>
                </td>
            </tr>
        </AlternatingItemTemplate>
    </asp:DataList>
    <tr>
        <td class="pm_list_footer" align="right" colspan="3">
            <asp:Label runat="server" Text="Total" />
        </td>
        <td class="pm_list_footer" align="right" colspan="1">
            <asp:Label ID="lblPaymentTotal" runat="server" Text="Label"></asp:Label>
            <tb:RecordListFooter id="ListFooter" runat="server"
                ShowAllRecords="true" Visible="false" 
              />
        </td>
    </tr>
</table>
