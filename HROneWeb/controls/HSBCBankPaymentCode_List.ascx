<%@ Control Language="C#" AutoEventWireup="true" CodeFile="HSBCBankPaymentCode_List.ascx.cs" Inherits="HSBCBankPaymentCode_List" %>
    
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<input type="hidden" id="CompanyDBID" runat="server" name="ID" />

<table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="0">
    <col width="5%" />
    <col width="50%" />
    <col width="25%" />
    <col width="20%" />
    <tr>
        <td class="pm_list_header" align="center">
            <asp:Panel ID="AddPanelSelectAll" runat="server" Visible="false">
            <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);"  />
            </asp:Panel> 
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_HSBCBankPaymentCodeBankAccountNo" OnClick="ChangeOrder_Click" Text="Bank Account No"></asp:LinkButton>
        </td>
        <td align="left" class="pm_list_header" >
            <asp:LinkButton runat="server" ID="_HSBCBankPaymentCode" OnClick="ChangeOrder_Click" Text="Bank Payment Code"></asp:LinkButton>
        </td>
        <td align="left" class="pm_list_header" >
            <asp:LinkButton runat="server" ID="_HSBCBankPaymentCodeAutoPayInOutFlag" OnClick="ChangeOrder_Click" Text="Autopay In/Out"></asp:LinkButton>
        </td>
    </tr>
    <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
        <ItemTemplate>
            <tr>
                <td class="pm_list" align="center">
                    <asp:CheckBox ID="ItemSelect" runat="server" Visible="false" />
                </td>
                <td class="pm_list">
                    <asp:Label ID="HSBCBankPaymentCodeBankAccountNo" runat="server" />
                </td>
                <td class="pm_list">
                    <asp:Label ID="HSBCBankPaymentCode" runat="server" />
                </td>
                <td class="pm_list">
                    <asp:Label ID="HSBCBankPaymentCodeAutoPayInOutFlag" runat="server" />
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
    ListOrderBy="HSBCBankPaymentCodeBankAccountNo"
    ListOrder="false" 
  />