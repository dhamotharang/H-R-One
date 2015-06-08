<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CompanyInbox_List.ascx.cs" Inherits="CompanyInbox_List" %>
    
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<input type="hidden" id="CompanyDBID" runat="server" name="ID" />


<table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="0">
    <col width="26px" />
    <col width="75px" />
    <col width="75px" />
    <tr>
        <td class="pm_list_header" align="center">
            <asp:Panel ID="AddPanelSelectAll" runat="server" Visible="false" >
            <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);"  />
            </asp:Panel> 
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_CompanyInboxSubject" OnClick="ChangeOrder_Click" Text="Subject"></asp:LinkButton>
        </td>
        <td align="left" class="pm_list_header" >
            <asp:LinkButton runat="server" ID="_CompanyInboxCreateDate" OnClick="ChangeOrder_Click" Text="Date"></asp:LinkButton>
        </td>
    </tr>
    <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
        <ItemTemplate>
            <tr>
                <td class="pm_list" align="center">
                    <asp:CheckBox ID="ItemSelect" runat="server" Visible="false" />
                </td>
                <td class="pm_list">
                    <asp:Label ID="CompanyInboxSubject" runat="server" />
                </td>
                <td class="pm_list">
                    <asp:Label ID="CompanyInboxCreateDate" runat="server" />
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
    ListOrderBy="CompanyInboxCreateDate"
    ListOrder="false" 
  />