<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MPFParameter_List.aspx.cs" Inherits="MPFParameter_List" MasterPageFile="~/MainMasterPage.master" %>

<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="MPF Parameter Setup" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="MPF Parameter List" />
                </td>
            </tr>
        </table>
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar id="toolBar" runat="server"
                     BackButton_Visible="false"
                     EditButton_Visible="false" 
                     SaveButton_Visible="false" 
                     OnNewButton_Click ="New_Click"
                     OnDeleteButton_Click="Delete_Click"
                      />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openAlert(); return false;"/>
                </td>
            </tr>
        </table>
        <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
            <col width="26px" />
            <col width="100px" />
            <col width="150px" />
            <col width="150px" />
            <col width="150px" />
            <col width="150px" />
            <col width="150px" />
            <col width="150px" />
            <tr>
                <td class="pm_list_header" align="center">
                   <asp:Panel ID="SelectAllPanel" runat="server">
                    <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
                    </asp:Panel> 
                </td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_MPFParamEffFr" OnClick="ChangeOrder_Click" Text="Effective From"/></td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_MPFParamMinMonthly" OnClick="ChangeOrder_Click" Text="Min Monthly"/></td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_MPFParamMaxMonthly" OnClick="ChangeOrder_Click" Text="Max Monthly"/></td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_MPFParamMinDaily" OnClick="ChangeOrder_Click" Text="Min Daily"/></td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_MPFParamMaxDaily" OnClick="ChangeOrder_Click" Text="Max Daily"/></td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_MPFParamEEPercent" OnClick="ChangeOrder_Click" Text="Employee Cont."/></td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_MPFParamERPercent" OnClick="ChangeOrder_Click" Text="Employer Cont."/></td>
            </tr>
            <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_list" align="center">
                            <asp:CheckBox ID="ItemSelect" runat="server" />
                        </td>
                        <td class="pm_list">
                            <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "MPFParameter_Edit.aspx?MPFParamID=" + binding.getValue(Container.DataItem,"MPFParamID"))%>">
                                <%#binding.getFValue(Container.DataItem,"MPFParamEffFr")%>
                            </a>
                        </td>
                        <td class="pm_list" align="right">
                            <%#binding.getFValue(Container.DataItem,"MPFParamMinMonthly","$0")%>
                        </td>
                        <td class="pm_list" align="right">
                            <%#binding.getFValue(Container.DataItem, "MPFParamMaxMonthly", "$0")%>
                        </td>
                        <td class="pm_list" align="right">
                            <%#binding.getFValue(Container.DataItem, "MPFParamMinDaily", "$0")%>
                        </td>
                        <td class="pm_list" align="right">
                            <%#binding.getFValue(Container.DataItem, "MPFParamMaxDaily", "$0")%>
                        </td>
                        <td class="pm_list" align="right">
                            <%#binding.getFValue(Container.DataItem, "MPFParamEEPercent","0.00")%>
                        </td>
                        <td class="pm_list" align="right">
                            <%#binding.getFValue(Container.DataItem, "MPFParamERPercent", "0.00")%>
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
            ListOrderBy="MPFParamEffFr"
            ListOrder="false" 
          />
</asp:Content> 