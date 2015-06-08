<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Payroll_CostAllocationList.ascx.cs" Inherits="Payroll_CostAllocationList" %>

<%@ Register Src="RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<input type="hidden" id="CostAllocationID" runat="server" name="ID" />
<%--<input type="hidden" id="EmpPayStatus" runat="server" />
--%>
<table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_section_title">
    <tr>
        <td>
            <asp:Label runat="server" Text="Cost Allocation Detail" />:
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
<table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="0">
    <col width="26px" /> 
    <col width="120px" /> 
    <col width="250px" /> 
    <col width="350px" /> 
    <col width="100px" /> 
    <tr>
        <td class="pm_list_header">
        </td>
        <td class="pm_list_header">
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_PaymentCodeID" OnClick="ChangeOrder_Click" Text="Payment" />
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_CostCenterID" OnClick="ChangeOrder_Click" Text="Cost Center" />
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_CostAllocationDetailAmount" OnClick="ChangeOrder_Click" Text="Amount" />
        </td>
    </tr>
    <asp:Panel ID="AddPanel" runat="server">
        <tr>
            <td class="pm_list" rowspan="2">
            </td>
            <td class="pm_list" align="center" rowspan="2">
                <asp:Button ID="Add" runat="server" Text="Add" CssClass="button" OnClick="Add_Click" />
            </td>
            <td class="pm_list">
                <asp:DropDownList ID="PaymentCodeID" runat="server">
                </asp:DropDownList>
            </td>
            <td class="pm_list">
                <asp:DropDownList ID="CostCenterID" runat="server">
                </asp:DropDownList>
            </td>
            <td class="pm_list" align="right">
                <asp:TextBox ID="CostAllocationDetailAmount" runat="server" Columns="10" />
            </td>
        </tr>
    </asp:Panel>
    <asp:DataList ID="Repeater" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" ShowFooter="false" ShowHeader="false" OnItemCommand="Repeater_ItemCommand" OnItemDataBound="Repeater_ItemDataBound">
        <EditItemTemplate>
            <tr>
                <td class="pm_list" align="center">
                    <asp:CheckBox ID="DeleteItem" runat="server" />
                    <input type="hidden" runat="server" id="CostAllocationDetailID" />
                </td>
                <td class="pm_list" align="center">
                    <asp:Button ID="Save" CssClass="button" runat="server" Text="Save" />
                    <asp:Button ID="Cancel" CssClass="button" runat="server" Text="Cancel" />
                </td>
                <td class="pm_list">
                    <asp:DropDownList ID="PaymentCodeID" runat="server">
                    </asp:DropDownList>
                </td>
                <td class="pm_list">
                    <asp:DropDownList ID="CostCenterID" runat="server">
                    </asp:DropDownList>
                </td>
                <td class="pm_list" align="right">
                    <asp:TextBox ID="CostAllocationDetailAmount" runat="server" Columns="10" />
                </td>
            </tr>
        </EditItemTemplate>
        <ItemTemplate>
            <tr>
                <td class="pm_list" align="center" >
                    <asp:CheckBox ID="DeleteItem" runat="server" />
                    <input type="hidden" runat="server" id="CostAllocationDetailID" />
                </td>
                <td class="pm_list" align="center">
                    <asp:Button ID="Edit" CssClass="button" runat="server" Text="Edit" />
                </td>
                <td class="pm_list">
                    <%#sBinding.getValue(Container.DataItem,"PaymentCodeID")%>
                </td>
                <td class="pm_list">
                    <%#sBinding.getValue(Container.DataItem,"CostCenterID")%>
                </td>
                <td class="pm_list" align="right">
                    <%#sBinding.getFValue(Container.DataItem, "CostAllocationDetailAmount", "$#,##0.00")%>
                </td>
            </tr>
        </ItemTemplate>
    </asp:DataList>
    <tr>
        <td class="pm_list" align="right" colspan="4">
            <asp:Label runat="server" Text="Total" />
        </td>
        <td class="pm_list" align="right" colspan="1">
            <asp:Label ID="lblCostAllocationTotal" runat="server" Text="Label"></asp:Label>
            <tb:RecordListFooter id="ListFooter" runat="server"
                ShowAllRecords="true" visible="false" 
              />
        </td>
    </tr>
</table>
