<%@ Control Language="C#" AutoEventWireup="true" CodeFile="eChannel_MPFContributionFile_List.ascx.cs" Inherits="eChannel_MPFContributionFile_List" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<input type="hidden" id="CompanyDBID" runat="server" name="ID" />


<table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="0">
    <col width="26px" />
    <col width="150px" />
    <col width="150px" />
    <col width="150px" />
    <col width="150px" />
    <col width="150px" />
    <col width="150px" />
    <col width="150px" />
    <tr>
        <td class="pm_list_header" align="center" colspan="2">
            <asp:Panel ID="AddPanelSelectAll" runat="server" Visible="false" >
            <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
            </asp:Panel> 
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_HSBCExchangeProfileID" OnClick="ChangeOrder_Click" Text="Remote Profile ID"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_CompanyMPFFileSubmitDateTime" OnClick="ChangeOrder_Click" Text="Submit Date" />
        </td>
        <td align="left" class="pm_list_header" >
            <asp:LinkButton runat="server" ID="_CompanyMPFFileConfirmDateTime" OnClick="ChangeOrder_Click" Text="Confirm Date" />
        </td>
        <td align="left" class="pm_list_header" >
            <asp:Label runat="server" ID="_SignedBy" Text="Signed By" />
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_CompanyMPFFileConsolidateDateTime" OnClick="ChangeOrder_Click" Text="Consolidate Date" />
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_CompanyMPFFileTransactionReference" OnClick="ChangeOrder_Click" Text="Transaction Reference" />
        </td>
    </tr>
    <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound" OnItemCommand="Repeater_ItemCommand">
        <ItemTemplate>
            <tr>
                <td class="pm_list" align="center">
                    <asp:CheckBox ID="ItemSelect" runat="server" />
                </td>
                <td class="pm_list" align="center">
                    <asp:Button ID="Cancel" Text="Cancel" runat="server" CssClass="button" />
                    <asp:Button ID="Sign" Text="Sign" runat="server" CssClass="button" />
                </td>
                <td class="pm_list">
                        <%#sbinding.getValue(Container.DataItem, "HSBCExchangeProfileID")%>
                </td>
                <td class="pm_list">
                    <%#sbinding.getFValue(Container.DataItem, "CompanyMPFFileSubmitDateTime", "yyyy-MM-dd HH:mm:ss")%>
                </td>
                <td class="pm_list">
                    <%#sbinding.getFValue(Container.DataItem, "CompanyMPFFileConfirmDateTime", "yyyy-MM-dd HH:mm:ss")%>
                </td>
                <td class="pm_list">
                    <asp:Label ID="SignedBy" runat="server" />
                </td>
                <td class="pm_list">
                    <%#sbinding.getFValue(Container.DataItem, "CompanyMPFFileConsolidateDateTime", "yyyy-MM-dd HH:mm:ss")%>
                </td>
                <td class="pm_list">
                    <%#sbinding.getValue(Container.DataItem, "CompanyMPFFileTransactionReference")%>
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
    ListOrderBy="CompanyMPFFileSubmitDateTime"
    ListOrder="false" 
  />