<%@ control language="C#" autoeventwireup="true" inherits="Emp_WorkingExperience_List, HROneWeb.deploy" %>

<%@ Register Src="RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %> 

<input type="hidden" id="EmpID" runat="server" name="ID" />
<asp:Panel ID="AllowEditPanel" runat="server">
<asp:Button ID="New" runat="server" Text="New" CSSclass="button"  OnClick="New_Click" />
<asp:Button ID="Delete" runat="server" CssClass="button" OnClick="Delete_Click" Text="Delete" />
</asp:Panel>
<table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_list_section">
    <col width="26px" />
    <col width="100px" />
    <col width="100px" />
    <col width="300px" />
    <tr>
        <td class="pm_list_header" align="center">
            <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_EmpWorkExpFromYear" OnClick="ChangeOrder_Click" Text="From"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_EmpWorkExpToYear" OnClick="ChangeOrder_Click" Text="To"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_EmpWorkExpCompanyName" OnClick="ChangeOrder_Click" Text="Company"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_EmpWorkExpPosition" OnClick="ChangeOrder_Click"
                Text="Position"></asp:LinkButton></td>
    </tr>
    <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
        <ItemTemplate>
            <tr>
                <td class="pm_list" align="center">
                    <asp:CheckBox ID="ItemSelect" runat="server" />
                </td>
                <td class="pm_list">
                    <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Emp_WorkingExperience_View.aspx?EmpID=" + sbinding.getValue(Container.DataItem,"EmpID") + "&EmpWorkExpID=" + sbinding.getValue(Container.DataItem,"EmpWorkExpID"))%>">
                        <asp:Label ID="EmpWorkExpFromMonth" runat="server" />&nbsp<asp:Label ID="EmpWorkExpFromYear" runat="server" />
                    </a>
                </td>
                <td class="pm_list">
                        <asp:Label ID="EmpWorkExpToMonth" runat="server" />&nbsp<asp:Label ID="EmpWorkExpToYear" runat="server" />
                </td>
                <td class="pm_list">
                        <%#sbinding.getValue(Container.DataItem,"EmpWorkExpCompanyName") %>
                </td>
                <td class="pm_list">
                        <%#sbinding.getValue(Container.DataItem,"EmpWorkExpPosition") %>
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
