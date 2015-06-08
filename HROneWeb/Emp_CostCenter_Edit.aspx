<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Emp_CostCenter_Edit.aspx.cs" Inherits="Emp_CostCenter_Edit" MasterPageFile="~/MainMasterPage.master"  %>

<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/Emp_Common.ascx" TagName="Emp_Common" TagPrefix="uc1" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="EmpCostCenterID" runat="server" name="ID" />
        <input type="hidden" id="EmpID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label Text="Employee Information" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <%=CurID<=0?HROne.Common.WebUtility.GetLocalizedString("Add"):HROne.Common.WebUtility.GetLocalizedString("Edit") %>
                    <asp:Label Text="Cost Center" runat="server" />
                </td>
            </tr>
        </table>
        
             
        
        <uc1:Emp_Common ID="Emp_Common1" runat="server"></uc1:Emp_Common>
        <br />
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar ID="toolBar" runat="server" NewButton_Visible="false" EditButton_Visible="false" OnBackButton_Click="Back_Click" OnSaveButton_Click="Save_Click" OnDeleteButton_Click="Delete_Click" />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openAlert(); return false;" />
                </td>
            </tr>
        </table>
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="15%" />
            <col width="35%" />
            <col width="15%" />
            <col width="35%" />
            <tr>
                <td class="pm_field_header" >
                    <asp:Label Text="From" runat="server" />:</td>
                <td class="pm_field" >
                    <uc1:WebDatePicker id="EmpCostCenterEffFr" runat="server" />
                </td>
                <td class="pm_field_header" >
                    <asp:Label Text="To" runat="server" />:</td>
                <td class="pm_field" >
                    <uc1:WebDatePicker id="EmpCostCenterEffTo" runat="server" />
                </td>
            </tr>
        </table>

        <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
            <col width="26px" />
            <col width="200px" />
            <col width="400px" />
            <tr>
                <td class="pm_list_header">
                </td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_CostCenterCode" Text="Cost Center Code" OnClick="ChangeOrder_Click" /></td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_CostCenterDesc" Text="Description" OnClick="ChangeOrder_Click" /></td>
                <td align="right" class="pm_list_header">
                    <asp:Label runat="server" ID="_EmpCostCenterPercentage" Text="Percent" /></td>
            </tr>
            <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_list" align="center">
                            <asp:CheckBox ID="ItemSelect" runat="server" Visible="false" />
                        </td>
                        <td class="pm_list">
                            <%#sbinding.getValue(Container.DataItem, "CostCenterCode")%>
                        </td>
                        <td class="pm_list">
                            <%#sbinding.getValue(Container.DataItem, "CostCenterDesc")%>
                        </td>
                        <td class="pm_list" align="right">
                            <asp:TextBox ID="EmpCostCenterPercentage" runat="server" style="text-align:right;" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <table width="100%" cellspacing="0" cellpadding="0" class="pm_list_pagenav">
            <tr>
                <td align="right">
                    <tb:RecordListFooter id="ListFooter" runat="server"
                        ShowAllRecords="true" visible="true" 
                      />
                </td>
            </tr>
        </table>
</asp:Content> 