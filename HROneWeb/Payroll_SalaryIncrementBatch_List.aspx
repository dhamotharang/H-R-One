<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Payroll_SalaryIncrementBatch_List.aspx.cs" Inherits="Payroll_SalaryIncrementBatch_List" MasterPageFile="~/MainMasterPage.master"  %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                   <asp:Label ID="Label1" Text="PayScale - Salary Increment Batch List" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="Label2" Text="Search Batch" runat="server" />
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="SearchUpdatePanel" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Reset" EventName="Click" />
        </Triggers>

        <ContentTemplate >
        <table width="100%" class="pm_section">
            <tr>
                <td align="left" class="pm_search_header">
                    <asp:Label Text="As At Date" runat="server" />:
                </td>
                <td align="left" class="pm_search">
                    <uc1:WebDatePicker id="AsAtDate" runat="server" /> <%-- AutoPostBack="true" /> --%>
                </td>
            </tr>
        </table> 
        </ContentTemplate >
        </asp:UpdatePanel>

        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td colspan="4">
                    <asp:Button ID="Search" runat="server" Text="Search" CssClass="button" OnClick="Search_Click" />
                    <asp:Button ID="Reset" runat="server" Text="Reset" CssClass="button" OnClick="Reset_Click" />
                </td>
            </tr>
        </table>
        <br /><br />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="Label3" Text="Salary Increment Batch List" runat="server" />
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
        <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Search" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="Reset" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>

        <ContentTemplate >
        <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
            <col width="26px" />
            <col width="200px" style="white-space:nowrap;"/>
            <col width="200px" style="white-space:nowrap;"/>
            <tr>
                <td class="pm_list_header" align="center">
                    <asp:Panel ID="SelectAllPanel" runat="server">
                        <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
                    </asp:Panel>
                </td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_AsAtDate" OnClick="ChangeOrder_Click" Text="As At Date"></asp:LinkButton></td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_Deferred" OnClick="ChangeOrder_Click" Text="Deferred ?"></asp:LinkButton></td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_Status" OnClick="ChangeOrder_Click" Text="Status"></asp:LinkButton></td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_Count" OnClick="ChangeOrder_Click" Text="Employee Count"></asp:LinkButton></td>
            </tr>
            <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_list" align="center">
                            <asp:CheckBox ID="ItemSelect" runat="server" />
                        </td>
                        <td class="pm_list">
                            <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Payroll_SalaryIncrementBatch_View.aspx?BatchID=" + binding.getValue(Container.DataItem,"BatchID"))%>">
                                <%#binding.getFValue(Container.DataItem, "AsAtDate", "yyyy-MM-dd")%>
                            </a>
                        </td>
                        <td class="pm_list">
                            <%#binding.getValue(Container.DataItem, "DeferredBatchDesc")%>
                        </td>
                        <td class="pm_list">
                            <%#binding.getValue(Container.DataItem, "StatusDesc")%>
                        </td>
                        <td class="pm_list">
                            <%#binding.getValue(Container.DataItem,"EmpCount") %>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <table width="100%" cellspacing="0" cellpadding="0" class="pm_list_pagenav">
            <tr>
                <td align="right">
                    <tb:RecordListFooter id="ListFooter" runat="server"
                        OnFirstPageClick="FirstPage_Click" 
                        OnPrevPageClick="PrevPage_Click"
                        OnNextPageClick="NextPage_Click"
                        OnLastPageClick="LastPage_Click"
                      />
                </td>
            </tr>
        </table>
        </ContentTemplate> 
        </asp:UpdatePanel> 
</asp:Content> 