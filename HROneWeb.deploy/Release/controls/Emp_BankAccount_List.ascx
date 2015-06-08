<%@ control language="C#" autoeventwireup="true" inherits="Emp_BankAccount_List, HROneWeb.deploy" %>
    
<%@ Register Src="RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<input type="hidden" id="EmpID" runat="server" name="ID" />
<asp:Panel ID="AllowEditPanel" runat="server">
<asp:Button ID="New" runat="server" Text="New" CSSclass="button"  OnClick="New_Click" />
<asp:Button ID="Delete" runat="server" CssClass="button" OnClick="Delete_Click" Text="Delete" />
</asp:Panel>
<table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_list_section">
    <col width="26px" />
    <col width="150px" />
    <col width="250px" />
    <tr>
        <td class="pm_list_header" align="center">
            <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_EmpBankCode" OnClick="ChangeOrder_Click" Text="Bank Account Number"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_EmpBankAccountHolderName" OnClick="ChangeOrder_Click"
                Text="Holder Name"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:Label Text="Is Default Account" runat="server" /></td>
        <td align="left" class="pm_list_header">
            <asp:Label ID="Label1" Text="Remark" runat="server" /></td>
            
    </tr>
    <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
        <ItemTemplate>
            <tr>
                <td class="pm_list" align="center">
                    <asp:CheckBox ID="ItemSelect" runat="server" />
                </td>
                <td class="pm_list">
                    <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Emp_BankAccount_View.aspx?EmpID=" + sbinding.getValue(Container.DataItem,"EmpID") + "&EmpBankAccountID=" + sbinding.getValue(Container.DataItem,"EmpBankAccountID"))%>">
                        <%#sbinding.getValue(Container.DataItem,"EmpBankCode")%>- 
                        <%#sbinding.getValue(Container.DataItem,"EmpBranchCode")%>-
                        <%#sbinding.getValue(Container.DataItem,"EmpAccountNo")%>
                    </a>
                </td>
                <td class="pm_list">
                    <%#sbinding.getValue(Container.DataItem,"EmpBankAccountHolderName")%>
                </td>
                <td class="pm_list">
                    <%#sbinding.getValue(Container.DataItem,"EmpAccDefault")%>
                </td>
                <td class="pm_list">
                    <%#sbinding.getValue(Container.DataItem,"EmpBankAccountRemark")%>
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
  />