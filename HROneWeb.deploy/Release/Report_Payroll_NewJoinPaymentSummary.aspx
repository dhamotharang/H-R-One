<%@ page language="C#" autoeventwireup="true" inherits="Report_Payroll_NewJoinPaymentSummary, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>

<%@ Register Src="~/controls/ReportExportControl.ascx" TagName="ReportExportControl" TagPrefix="uc3" %>
<%@ Register Src="~/controls/EmployeeSearchControl.ascx" TagName="EmployeeSearchControl" TagPrefix="uc2" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
<%@ Register Src="~/controls/Payroll_PeriodSelectionList.ascx" TagName="Payroll_PeriodSelectionList" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="lblReportHeader" runat="server" Text="New Join Payment Summary" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="Label4" Text="Payroll Batch List" runat="server" />
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="PayrollSelectionPanel" runat="server" UpdateMode="Conditional" >
        <Triggers>
        </Triggers>
        <ContentTemplate >
            <uc1:Payroll_PeriodSelectionList id="Payroll_PeriodSelectionList1" runat="server" ShowPayrollGroupDropDownList="true" OnPayrollBatchChecked="Payroll_PeriodSelectionList1_PayrollBatchChecked" />
        </ContentTemplate> 
        </asp:UpdatePanel> 
        <br />
        <asp:UpdatePanel ID="SearchUpdatePanel" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Reset" EventName="Click" />
        </Triggers>

        <ContentTemplate >
            <uc2:EmployeeSearchControl id="EmployeeSearchControl1" runat="server" />
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
        <table width="100%" class="pm_section">
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label2" runat="server" Text="Prepared by" />:
                </td>
                <td class="pm_field">
                    <asp:TextBox runat="server" ID="txtPreparedBy" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label3" runat="server" Text="Reviewed by" />:
                </td>
                <td class="pm_field">
                    <asp:TextBox runat="server" ID="txtReviewedBy" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Employee List" />
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Payroll_PeriodSelectionList1" EventName="PayrollBatchChecked"/>
            <asp:AsyncPostBackTrigger ControlID="Search" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="Reset" EventName="Click" />
            <asp:PostBackTrigger ControlID="ReportExportControl1" />
            <asp:PostBackTrigger ControlID="ReportExportControl2" />
        </Triggers>

        <ContentTemplate >
        <table class="pm_section" width="100%">
            <tr>
                <td >
                    <uc3:ReportExportControl id="ReportExportControl1" runat="server"  OnClick="btnGenerate_Click" ButtonCssClass="button"/>
                </td>
            </tr>
        </table>
        <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
            <col width="26px" />
            <col width="150px" />
            <colgroup width="350px" >
                <col width="150px" />
                <col />
            </colgroup> 
            <col width="150px" />
            <col width="100px" />
            <tr>
                <td class="pm_list_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
                </td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_EmpNo" OnClick="ChangeOrder_Click" Text="EmpNo" /></td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_EmpEngSurname" OnClick="ChangeOrder_Click" Text="Surname" /></td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_EmpEngOtherName" OnClick="ChangeOrder_Click" Text="Other Name" /></td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_EmpAlias" OnClick="ChangeOrder_Click" Text="Alias" /></td>
            </tr>
            <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_list" align="center">
                            <asp:CheckBox ID="ItemSelect" runat="server" />
                        </td>
                        <td class="pm_list">
                            <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Emp_View.aspx?EmpID=" + binding.getValue(Container.DataItem,"EmpID"))%>">
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
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
       <table width="100%" cellspacing="0" cellpadding="0" class="pm_list_pagenav">
            <tr>
                <td >
                    <uc3:ReportExportControl id="ReportExportControl2" runat="server"  OnClick="btnGenerate_Click" ButtonCssClass="button"/>
                </td>
                <td align="right">
                    <tb:RecordListFooter id="ListFooter" runat="server"
                        ShowAllRecords="true"
                      />
                </td>
            </tr>
        </table>
        </ContentTemplate> 
        </asp:UpdatePanel> 
</asp:Content> 