<%@ control language="C#" autoeventwireup="true" inherits="Emp_Beneficiaries_List, HROneWeb.deploy" %>

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
    <col width="150px" />
    <col width="100px" />
    <col width="100px" />
    <col width="100px" />
    <col width="100px" />
    <tr>
        <td class="pm_list_header" align="center">
            <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_EmpBeneficiariesName" OnClick="ChangeOrder_Click" Text="Name"></asp:LinkButton>
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_EmpBeneficiariesShare" OnClick="ChangeOrder_Click" Text="Share(%)"></asp:LinkButton>
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_EmpBeneficiariesHKID" OnClick="ChangeOrder_Click" Text="HKID"></asp:LinkButton>
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_EmpBeneficiariesRelation" OnClick="ChangeOrder_Click" Text="Relation"></asp:LinkButton>
        </td>
    </tr>
    <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
        <ItemTemplate>
            <tr>
                <td class="pm_list" align="center">
                    <asp:CheckBox ID="ItemSelect" runat="server" />
                </td>
                <td class="pm_list">
                    <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Emp_Beneficiaries_View.aspx?EmpID=" + sbinding.getValue(Container.DataItem,"EmpID") + "&EmpBeneficiariesID=" + sbinding.getValue(Container.DataItem,"EmpBeneficiariesID"))%>">
                        <%#sbinding.getValue(Container.DataItem, "EmpBeneficiariesName")%>
                    </a>
                </td>
                <td class="pm_list">
                        <%#sbinding.getFValue(Container.DataItem, "EmpBeneficiariesShare", "0.00")%>
                </td>
                <td class="pm_list">
                        <%#sbinding.getValue(Container.DataItem, "EmpBeneficiariesHKID")%>
                </td>
                <td class="pm_list">
                        <%#sbinding.getValue(Container.DataItem, "EmpBeneficiariesRelation")%>
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
