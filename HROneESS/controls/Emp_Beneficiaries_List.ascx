<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Emp_Beneficiaries_List.ascx.cs" Inherits="Emp_Beneficiaries_List" %>

<input type="hidden" id="EmpID" runat="server" name="ID" />
<table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_list_section">
    <col width="200px" />
    <col width="100px" />
    <col width="150px" />
    <col width="300px" />
    <tr>
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
                <td class="pm_list">
                        <%#sbinding.getValue(Container.DataItem, "EmpBeneficiariesName")%>
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
