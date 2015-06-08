<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Emp_Benefit_List.ascx.cs" Inherits="Emp_Benefit_List" %>

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
            <asp:LinkButton runat="server" ID="_EmpBenefitEffectiveDate" OnClick="ChangeOrder_Click" Text="Effective"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_EmpBenefitExpiryDate" OnClick="ChangeOrder_Click" Text="Expiry"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_EmpBenefitPlanID" OnClick="ChangeOrder_Click" Text="Benefit Plan"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_EmpBenefitEEPremium" OnClick="ChangeOrder_Click"
                Text="EE Premium"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_EmpBenefitERPremium" OnClick="ChangeOrder_Click"
                Text="ER Premium"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_EmpBenefitSpousePremium" OnClick="ChangeOrder_Click"
                Text="Spouse Premium"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_EmpBenefitChildPremium" OnClick="ChangeOrder_Click"
                Text="Child Premium"></asp:LinkButton></td>
    </tr>
    <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
        <ItemTemplate>
            <tr>
                <td class="pm_list" align="center">
                    <asp:CheckBox ID="ItemSelect" runat="server" />
                </td>
                <td class="pm_list">
                    <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Emp_Benefit_View.aspx?EmpID=" + sbinding.getValue(Container.DataItem,"EmpID") + "&EmpBenefitID=" + sbinding.getValue(Container.DataItem,"EmpBenefitID"))%>">
                        <%#sbinding.getFValue(Container.DataItem, "EmpBenefitEffectiveDate", "yyyy-MM-dd")%>
                    </a>
                </td>
                <td class="pm_list">
                        <%#sbinding.getFValue(Container.DataItem, "EmpBenefitExpiryDate", "yyyy-MM-dd")%>
                </td>
                <td class="pm_list">
                        <%#sbinding.getValue(Container.DataItem, "EmpBenefitPlanID")%>
                </td>
                <td class="pm_list">
                        <%#sbinding.getFValue(Container.DataItem, "EmpBenefitEEPremium", "$0.00")%>
                </td>
                <td class="pm_list">
                        <%#sbinding.getFValue(Container.DataItem, "EmpBenefitERPremium", "$0.00")%>
                </td>
                <td class="pm_list">
                        <%#sbinding.getFValue(Container.DataItem, "EmpBenefitSpousePremium", "$0.00")%>
                </td>
                <td class="pm_list">
                        <%#sbinding.getFValue(Container.DataItem, "EmpBenefitChildPremium", "$0.00")%>
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
