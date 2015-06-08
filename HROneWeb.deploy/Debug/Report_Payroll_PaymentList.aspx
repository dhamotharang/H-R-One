<%@ page language="C#" autoeventwireup="true" inherits="Report_Payroll_PaymentList, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>

<%@ Register Src="~/controls/Payroll_ConfirmedPeriod_List.ascx" TagName="Payroll_ConfirmedPeriod_List" TagPrefix="uc3" %>
<%@ Register Src="~/controls/EmployeeSearchControl.ascx" TagName="EmployeeSearchControl" TagPrefix="uc2" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
<%@ Register Src="~/controls/Payroll_PeriodSelectionList.ascx" TagName="Payroll_PeriodSelectionList" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="lblReportHeader" Text="Payment Summary List" runat="server" />
                </td>
            </tr>
        </table>
    <asp:UpdatePanel ID="PayrollSelectionPanel" runat="server" UpdateMode="Conditional" >
    <Triggers>
    </Triggers>

    <ContentTemplate >
        <uc1:Payroll_PeriodSelectionList id="Payroll_PeriodSelectionList1" runat="server" PayrollBatchStatusSelectionOption="All" ShowPayrollGroupDropDownList="true" OnPayrollBatchChecked="Payroll_PeriodSelectionList1_PayrollBatchChecked" />
    </ContentTemplate> 
    </asp:UpdatePanel> 
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
			<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                <tr>
                    <td>
                        <asp:Label ID="Label1" Text="Employee List" runat="server" />
                    </td>
                </tr>
            </table>
        <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Payroll_PeriodSelectionList1" EventName="PayrollBatchChecked"/>
            <asp:AsyncPostBackTrigger ControlID="Search" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="Reset" EventName="Click" />
            <asp:PostBackTrigger ControlID="btnGenerate" />
        </Triggers>

        <ContentTemplate >
            <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
            <col width="26px" />
            <col width="150px" />
            <col width="150px" />
            <col width="350px"/>
            <col width="150px" />
            <col width="100px" />
                <tr>
                    <td class="pm_list_header" align="center" >
                        <%-- Start 0000016, Miranda, 2014-05-30 --%>
                        <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" checked="checked" />
                        <%-- End 0000016, Miranda, 2014-05-30 --%>
                    </td>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="_EmpNo" OnClick="ChangeOrder_Click" Text="EmpNo" />
                    </td>
                    <td align="left" class="pm_list_header" colspan="2" >
                        <asp:LinkButton runat="server" ID="_EmpEngFullName" OnClick="ChangeOrder_Click" Text="Name" />
                    </td>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="_EmpAlias" OnClick="ChangeOrder_Click" Text="Alias" />
                    </td>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="_EmpStatus" OnClick="ChangeOrder_Click" Text="Status" />
                    </td>                                                      
                </tr>
                <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td class="pm_list" align="center">
                                <asp:CheckBox ID="ItemSelect" runat="server" Checked="True" />
                            </td>
                            <td class="pm_list">
                                <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Emp_View.aspx?EmpID=" + sbinding.getValue(Container.DataItem,"EmpID"))%>">
                                    <%#sbinding.getValue(Container.DataItem, "EmpNo")%>
                                </a>
                                
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem, "EmpEngSurname")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem, "EmpEngOtherName")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem, "EmpAlias")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"EmpStatus")%>
                            </td>                               
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <table width="100%" cellspacing="0" cellpadding="0" class="pm_list_pagenav">
                <tr>
                    <td>
                            <asp:Button ID="btnGenerate" runat="server" Text="Generate" CSSClass="button" OnClick="btnGenerate_Click" />
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