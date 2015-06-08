<%@ Control Language="C#" AutoEventWireup="true" CodeFile="HSBCExchangeProfile_List.ascx.cs" Inherits="HSBCExchangeProfile_List" %>
    
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>


<table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="0">
    <col width="26px" />
    <col width="50%" />
    <col width="30%" />
    <col width="20%" />
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
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_HSBCExchangeProfileIsLocked" OnClick="ChangeOrder_Click" Text="Locked"></asp:LinkButton></td>
    </tr>
    <tr id="AddPanel" runat="server" >
        <td class="pm_list" align="center">
            <asp:Button ID="Add" runat="server" Text="Add" CssClass="button" OnClick="Add_Click" />
        </td>
        <td class="pm_list">
        </td>
        <td class="pm_list">
            <asp:DropDownList ID="HSBCExchangeProfileBankCode" runat="server" >
                <asp:ListItem Text="HSBC" Value="HSBC" />
                <asp:ListItem Text="Hang Seng Bank" Value="HangSeng" />
            </asp:DropDownList> 
        </td>
        <td class="pm_list">
        </td>
    </tr>
    <asp:DataList ID="Repeater" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" ShowFooter="false"  
        ShowHeader="false" OnItemCommand="Repeater_ItemCommand" OnItemDataBound="Repeater_ItemDataBound">
        <ItemTemplate>
            <tr>
                <td class="pm_list" align="center">
                    <asp:CheckBox ID="DeleteItem" runat="server" Visible="false"  />
                    <input type="hidden" runat="server" id="HSBCExchangeProfileID" />
                    <asp:Button ID="Edit" runat="server" CssClass="button" Text="Edit" />
                </td>
                <td class="pm_list">
                        <%#sbinding.getValue(Container.DataItem, "HSBCExchangeProfileRemoteProfileID")%>
                </td>
                <td class="pm_list">
                        <%#sbinding.getValue(Container.DataItem, "HSBCExchangeProfileBankCode")%>
                </td>
                <td class="pm_list">
                        <%#sbinding.getValue(Container.DataItem, "HSBCExchangeProfileIsLocked")%>
                </td>
            </tr>
        </ItemTemplate>
        <EditItemTemplate>
            <tr>
                <td class="pm_list" align="center">
                    <asp:CheckBox ID="DeleteItem" runat="server" Visible="false"  />
                    <input type="hidden" runat="server" id="HSBCExchangeProfileID" />
                    <asp:Button ID="Save" runat="server" Text="Save" CssClass="button" />
                    <asp:Button ID="Cancel" runat="server" Text="Cancel" CssClass="button"/>
                </td>
                <td class="pm_list">
                        <%#sbinding.getValue(Container.DataItem, "HSBCExchangeProfileRemoteProfileID")%>
                </td>
                <td class="pm_list">
                        <%#sbinding.getValue(Container.DataItem, "HSBCExchangeProfileBankCode")%>
                </td>
                <td class="pm_list">
                    <asp:CheckBox ID="HSBCExchangeProfileIsLocked" runat="server" />
                </td>
            </tr>
        </EditItemTemplate>
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