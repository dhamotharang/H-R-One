<%@ Control Language="C#" AutoEventWireup="true" CodeFile="eChannel_RemoteProfile_List.ascx.cs" Inherits="controls_eChannel_RemoteProfile_List" %>
    
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<input type="hidden" id="CompanyDBID" runat="server" name="ID" />


<table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="0">
    <col width="26px" />
    <col width="50%" />
    <col width="50%" />
    <tr>
        <td class="pm_list_header" align="center">
            <asp:Panel ID="AddPanelSelectAll" runat="server" Visible="false" >
            <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','DeleteItem',this.checked);" />
            </asp:Panel> 
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_HSBCExchangeProfileRemoteProfileID" OnClick="ChangeOrder_Click" Text="Remote Profile ID"></asp:LinkButton></td>
        <td align="left" class="pm_list_header" >
            <asp:LinkButton runat="server" ID="_HSBCExchangeProfileBankCode" OnClick="ChangeOrder_Click" Text="Bank"></asp:LinkButton></td>
    </tr>
    <asp:DataList ID="Repeater" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" ShowFooter="false"  
        ShowHeader="false" OnItemCommand="Repeater_ItemCommand" OnItemDataBound="Repeater_ItemDataBound">
        <ItemTemplate>
            <tr>
                <td class="pm_list" align="center">
                    <asp:CheckBox ID="DeleteItem" runat="server" Visible="false"  />
                    <input type="hidden" runat="server" id="HSBCExchangeProfileID" />
                </td>
                <td class="pm_list">
                        <%#sbinding.getValue(Container.DataItem, "HSBCExchangeProfileRemoteProfileID")%>
                </td>
                <td class="pm_list">
                        <%#sbinding.getValue(Container.DataItem, "HSBCExchangeProfileBankCode")%>
                </td>
            </tr>
        </ItemTemplate>
    </asp:DataList>
</table>
<tb:RecordListFooter id="ListFooter" runat="server"
    OnFirstPageClick="FirstPage_Click"
    OnPrevPageClick="PrevPage_Click"
    OnNextPageClick="NextPage_Click"
    OnLastPageClick="LastPage_Click"
    ListOrderBy="HSBCExchangeProfileRemoteProfileID"
    ListOrder="true" 
  />