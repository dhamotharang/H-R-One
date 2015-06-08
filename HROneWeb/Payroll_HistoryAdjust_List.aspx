<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Payroll_HistoryAdjust_List.aspx.cs" Inherits="Payroll_HistoryAdjust_List" MasterPageFile="~/MainMasterPage.master"  %>
<%@ Register Src="~/controls/EmployeeSearchControl.ascx" TagName="EmployeeSearchControl" TagPrefix="uc2" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label Text="Payroll History Enquiry/Adjustment" runat="server" />
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="SearchUpdatePanel" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Reset" EventName="Click" />
        </Triggers>

        <ContentTemplate >
            <uc2:EmployeeSearchControl id="EmployeeSearchControl1" runat="server" >
            <AdditionElements >
            <tr>
                <td class="pm_search_header">
                    <asp:Label ID="Label6" Text="Payroll Cycle Reference Date" runat="server" />:
                </td>
                <td class="pm_search">
                    <uc1:WebDatePicker id="PayPeriodAsOFDate" runat="server" />
                </td>
                <td></td>
                <td></td>
            </tr>
            </AdditionElements>
            </uc2:EmployeeSearchControl> 
        </ContentTemplate> 
        </asp:UpdatePanel> 
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <asp:Button ID="Search" runat="server" Text="Search" CssClass="button" OnClick="Search_Click" />
                    <asp:Button ID="Reset" runat="server" Text="Reset" CssClass="button" OnClick="Reset_Click" />
                </td>
            </tr>
        </table>
		<br /><br />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label Text="Employee List" runat="server" />
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Search" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="Reset" EventName="Click" />
        </Triggers>

        <ContentTemplate >
        <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
            <col width="26px" />
            <col width="75px" />
            <col width="75px" />
            <col width="100px"/>
            <col width="75px" />
            <col width="75px" />
            <col width="75px" />
            <col width="250px" />
            <col width="120px" />
            <tr>
                <td class="pm_list_header" align="center" >
                </td>
                <td align="left" class="pm_list_header" >
                    <asp:LinkButton runat="server" ID="_EmpNo" OnClick="ChangeOrder_Click" Text="EmpNo" /></td>
                <td align="left" class="pm_list_header" colspan="2" >
                    <asp:LinkButton runat="server" ID="_EmpEngFullName" OnClick="ChangeOrder_Click" Text="Name" /></td>
                <td align="left" class="pm_list_header" >
                    <asp:LinkButton runat="server" ID="_EmpAlias" OnClick="ChangeOrder_Click" Text="Alias" /></td>
                <td align="left" class="pm_list_header" colspan="2" >
                    <asp:LinkButton runat="server" ID="_PayPeriodFr" OnClick="ChangeOrder_Click" Text="Period" /></td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_EmpPayIsRP" OnClick="ChangeOrder_Click" Text="Type" />
                </td>
                <td align="left" class="pm_list_header" >
                    <asp:LinkButton runat="server" ID="_PayPeriodConfirmDate" OnClick="ChangeOrder_Click" Text="Confirm Date" /></td>
            </tr>
            <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_list" align="center">
                            <asp:CheckBox ID="ItemSelect" runat="server" Visible="False" />
                        </td>
                        <td class="pm_list">
                            <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Payroll_TrialRunAdjust_View.aspx?EmpPayrollID=" + binding.getValue(Container.DataItem,"EmpPayrollID"))%>">
                                <%#binding.getValue(Container.DataItem,"EmpNo")%>
                            </a>
                        </td>
                        <td class="pm_list">
                            <%#binding.getValue(Container.DataItem,"EmpEngSurname")%>
                        </td>
                        <td class="pm_list">
                            <%#binding.getValue(Container.DataItem,"EmpEngOtherName")%>
                        </td>
                        <td class="pm_list">
                            <%#binding.getValue(Container.DataItem,"EmpAlias")%>
                        </td>
                        <td class="pm_list">
                            <%#binding.getFValue(Container.DataItem, "PayPeriodFr", "yyyy-MM-dd")%>
                        </td>
                        <td class="pm_list">
                            <%#binding.getFValue(Container.DataItem,"PayperiodTo","yyyy-MM-dd")%>
                        </td>
                        <td class="pm_list">
                            <asp:Label ID="Type" runat="server" />
                        </td>
                        <td class="pm_list">
                            <%#binding.getFValue(Container.DataItem, "PayPeriodConfirmDate", "yyyy-MM-dd HH:mm:ss")%>
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
        </ContentTemplate> 
        </asp:UpdatePanel> 
</asp:Content> 