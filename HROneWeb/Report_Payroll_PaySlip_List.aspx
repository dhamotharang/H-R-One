<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Report_Payroll_PaySlip_List.aspx.cs" Inherits="Report_Payroll_PaySlip_List" MasterPageFile="~/MainMasterPage.master" %>

<%@ Register Src="~/controls/ReportExportControl.ascx" TagName="ReportExportControl" TagPrefix="uc3" %>
<%@ Register Src="~/controls/EmployeeSearchControl.ascx" TagName="EmployeeSearchControl" TagPrefix="uc2" %>
<%@ Register Src="~/controls/Payroll_PeriodSelectionList.ascx" TagName="Payroll_PeriodSelectionList" TagPrefix="uc1" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
        <tr>
            <td>
                <asp:Label ID="lblReportHeader" Text="Pay Slip Report" runat="server" />
            </td>
        </tr>
    </table>
    <br />
    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
        <tr>
            <td>
                <asp:Label ID="Label3" Text="Payroll Batch List" runat="server" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="PayrollSelectionPanel" runat="server" UpdateMode="Conditional" >
    <Triggers>
    </Triggers>

    <ContentTemplate >
        <uc1:Payroll_PeriodSelectionList id="Payroll_PeriodSelectionList1" runat="server" PayrollBatchStatusSelectionOption="ConfirmOnly"  ShowPayrollGroupDropDownList="true" OnPayrollBatchChecked="Payroll_PeriodSelectionList1_PayrollBatchChecked" />
    </ContentTemplate> 
    </asp:UpdatePanel> 
    <br />
    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
        <tr>
            <td>
                <asp:Label ID="Label5" Text="Parameter" runat="server" />
            </td>
        </tr>
    </table>
    <table width="100%" class="pm_section" >
        <colgroup>
            <col width="25%" />
            <col width="75%" />
        </colgroup>
        <tr>
            <td class="pm_field_header">
                <asp:Label ID="Label1" Text="First Hierarchy Level Display" runat="server" />:
            </td>
            <td class="pm_field">
                <asp:DropDownList ID="HLevel1" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="pm_field_header">
                <asp:Label ID="Label2" Text="Second Hierarchy Level Display" runat="server" />:
            </td>
            <td class="pm_field">
                <asp:DropDownList ID="HLevel2" runat="server" />
            </td>
        </tr>
    </table> 
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
			<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                <tr>
                    <td>
                        <asp:Label ID="Label4" Text="Employee List" runat="server" />
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
                    <td>
                        <uc3:ReportExportControl id="ReportExportControl2" runat="server"  OnClick="btnGenerate_Click" ButtonCssClass="button"/>
                    </td>
                </tr>
            </table>
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
                            <uc3:ReportExportControl id="ReportExportControl1" runat="server"  OnClick="btnGenerate_Click" ButtonCssClass="button"/>
                    </td>
                <td align="right">
                    <tb:RecordListFooter id="ListFooter" runat="server" ShowAllRecords="true" />
                </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content> 