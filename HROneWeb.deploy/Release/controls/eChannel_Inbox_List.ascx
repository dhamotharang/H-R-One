<%@ control language="C#" autoeventwireup="true" inherits="eChannel_Inbox_List, HROneWeb.deploy" %>
    
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<input type="hidden" id="CompanyDBID" runat="server" name="ID" />

<table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="0">
    <col width="26px" />
    <col width="150px" />
    <col width="150px" />
    <col width="150px" />
    <tr>
        <td class="pm_list_header" align="center">
            <asp:Panel ID="AddPanelSelectAll" runat="server" Visible="false">
            <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);"  />
            </asp:Panel> 
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_CompanyInboxSubject" OnClick="ChangeOrder_Click" Text="Subject"></asp:LinkButton>
        </td>
        <td align="left" class="pm_list_header" >
            <asp:LinkButton runat="server" ID="_CompanyInboxCreateDate" OnClick="ChangeOrder_Click" Text="Date Received"></asp:LinkButton>
        </td>
        <td align="left" class="pm_list_header" >
            <asp:LinkButton runat="server" ID="_CompanyInboxExpiryDate" OnClick="ChangeOrder_Click" Text="Expiry Date"></asp:LinkButton>
        </td>
    </tr>
    <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
        <ItemTemplate>
            <tr>
                <td class="pm_list" align="center">
                    <asp:CheckBox ID="ItemSelect" runat="server" Visible="false" />
                </td>
                <td class="pm_list">
                    <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "EChannel_CompanyInbox_Detail.aspx?CompanyInboxID=" + sbinding.getValue(Container.DataItem, "CompanyInboxID") )%>">
                        <asp:Label ID="CompanyInboxSubject" runat="server" />
                    </a> 
                </td>
                <td class="pm_list">
                    <asp:Label ID="CompanyInboxCreateDate" runat="server" />
                </td>
                <td class="pm_list">
                    <asp:Label ID="CompanyInboxExpiryDate" runat="server" />
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