<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CompanyMPFFile_List.ascx.cs" Inherits="CompanyMPFFile_List" %>
    
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<input type="hidden" id="CompanyDBID" runat="server" name="ID" />


<table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="0">
    <col width="26px" />
    <col width="75px" />
    <col width="75px" />
    <col width="50px" />
    <col width="50px" />
    <col width="150px" />
    <col width="50px" />
    <col width="50px" />
    <tr>
        <td class="pm_list_header" align="center">
            <asp:Panel ID="AddPanelSelectAll" runat="server" Visible="false" >
            <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
            </asp:Panel> 
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_HSBCExchangeProfileID" OnClick="ChangeOrder_Click" Text="Remote Profile ID"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_CompanyMPFFileSubmitDateTime" OnClick="ChangeOrder_Click" Text="Submit Date"></asp:LinkButton></td>
        <td align="left" class="pm_list_header" >
            <asp:LinkButton runat="server" ID="_CompanyMPFFileConfirmDateTime" OnClick="ChangeOrder_Click" Text="Confirm Date"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_CompanyMPFFileConsolidateDateTime" OnClick="ChangeOrder_Click" Text="Consolidate Date"></asp:LinkButton></td>
    </tr>
    <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
        <ItemTemplate>
            <tr>
                <td class="pm_list" align="center">
                    <asp:CheckBox ID="ItemSelect" runat="server" />
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
                        <%#sbinding.getFValue(Container.DataItem, "CompanyMPFFileConsolidateDateTime", "yyyy-MM-dd HH:mm:ss")%>
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