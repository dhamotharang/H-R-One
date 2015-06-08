<%@ page language="C#" autoeventwireup="true" inherits="Payroll_GenerateBankFile_View, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" maintainscrollpositiononpostback="true" viewStateEncryptionMode="Always" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
<%@ Register Src="~/controls/Payroll_PeriodSelectionList.ascx" TagName="Payroll_PeriodSelectionList" TagPrefix="uc1" %>
<%@ Register Src="~/controls/Payroll_GenerateBankFile_HSBC.ascx" TagName="Payroll_GenerateBankFile_HSBC" TagPrefix="bankControl" %>
<%@ Register Src="~/controls/Payroll_GenerateBankFile_SCB.ascx" TagName="Payroll_GenerateBankFile_SCB"TagPrefix="bankControl" %>
<%@ Register Src="~/controls/Payroll_GenerateBankFile_CitiBank.ascx" TagName="Payroll_GenerateBankFile_CitiBank" TagPrefix="bankControl" %>
<%@ Register Src="~/controls/Payroll_GenerateBankFile_BOC.ascx" TagName="Payroll_GenerateBankFile_BOC" TagPrefix="bankControl" %>
<%@ Register Src="~/controls/Payroll_GenerateBankFile_ICBC.ascx" TagName="Payroll_GenerateBankFile_ICBC" TagPrefix="bankControl" %>
<%@ Register Src="~/controls/Payroll_GenerateBankFile_BOCNY.ascx" TagName="Payroll_GenerateBankFile_BOCNY" TagPrefix="bankControl" %>
<%@ Register Src="~/controls/Payroll_GenerateBankFile_ANZ.ascx" TagName="Payroll_GenerateBankFile_ANZ" TagPrefix="bankControl" %>
<%@ Register Src="~/controls/Payroll_GenerateBankFile_BOAmerica.ascx" TagName="Payroll_GenerateBankFile_BOAmerica" TagPrefix="bankControl" %>
<%@ Register Src="~/controls/Payroll_GenerateBankFile_DBS.ascx" TagName="Payroll_GenerateBankFile_DBS" TagPrefix="bankControl" %>
<%@ Register Src="~/controls/Payroll_GenerateBankFile_UOB.ascx" TagName="Payroll_GenerateBankFile_UOB" TagPrefix="bankControl" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
	<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
         <tr>
             <td>
                    <asp:Label ID="lblReportHeader" runat="server" Text="Generate Bank File" />
             </td>
         </tr>
    </table>

    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
        <tr>
            <td>
                <asp:Label ID="Label7" Text="Payroll Batch List" runat="server" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="PayrollSelectionPanel" runat="server" UpdateMode="Conditional" >
    <Triggers>
    </Triggers>

    <ContentTemplate >
        <uc1:Payroll_PeriodSelectionList id="Payroll_PeriodSelectionList1" runat="server" PayrollBatchStatusSelectionOption="ConfirmOnly" ShowPayrollGroupDropDownList="true" OnPayrollBatchChecked="Payroll_PeriodSelectionList1_PayrollBatchChecked" PayrollBatchCheckBoxDefaultCheckedOption="ExcludeBankFileGenerated" />
    </ContentTemplate> 
    </asp:UpdatePanel> 
    
    <br />    
            
    <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional"  >
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="Payroll_PeriodSelectionList1" EventName="PayrollBatchChecked"/>
        <asp:PostBackTrigger ControlID="btnGenerate" />
        <asp:PostBackTrigger ControlID="btnAutoPayList" />
        <asp:PostBackTrigger ControlID="btnAutoPayListExcel" />
    </Triggers>
    <ContentTemplate>
        
        <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
            <col width="15%" />
            <tr>
                <td class="pm_field_title" colspan="3">
                    <asp:Label ID="Label2" Text="Bank File Information" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label3" runat="server" Text="Bank Account" />
                </td>
                <td class="pm_field">
                    <asp:DropDownList ID="BankAccount" runat="server" CssClass="pm_required" OnSelectedIndexChanged="BankAccount_SelectedIndexChanged" AutoPostBack="true" />
                    <asp:CheckBox ID="chkShowAllCompany" runat="server" Text="Show all bank account" AutoPostBack="true"  />
                </td>
            </tr>
            <tr id="ValueDateRow" runat="server" >
                <td class="pm_field_header">
                    <asp:Label ID="Label4" runat="server" Text="Value Date" />
                </td>
                <td class="pm_field">
                    <uc1:WebDatePicker id="ValueDate" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label5" runat="server" Text="Bank File Type" />
                </td>
                <td class="pm_field">
                    <asp:DropDownList ID="BankFileType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="BankFileType_SelectedIndexChanged">
                        <asp:ListItem Text="HSBC/Hang Seng Bank" Value="HSBC" />
                        <asp:ListItem Text="Standard Chartered Bank" Value="SCB" />
						<asp:ListItem Text="Bank Of China" Value="BOC" />
						<asp:ListItem Text="Bank Of China (Nan Yang)" Value="BOCNY" />
						<asp:ListItem Text="ICBC (Asia)" Value="ICBC" />
						<asp:ListItem Text="Citibank" Value="CitiBank" />
						<asp:ListItem Text="ANZ National Bank" Value="ANZ" />
						<asp:ListItem Text="Bank of America" Value="BOAmerica" />
						<asp:ListItem Text="DBS" Value="DBS" />
						<asp:ListItem Text="Wing Lung Bank" Value="WLB" Enabled="false"/>
				        <asp:ListItem Text="UOB" Value="UOB" />
				        <asp:ListItem Text="UnSpecify (CSV format)" Value="" />
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <asp:Panel ID="BankControlListPanel" runat="server" >
            <bankControl:Payroll_GenerateBankFile_HSBC ID="Payroll_GenerateBankFile_HSBCControl" runat="server" Visible="False" />
            <bankControl:Payroll_GenerateBankFile_SCB ID="Payroll_GenerateBankFile_SCBControl" runat="server" Visible="False" />
            <bankControl:Payroll_GenerateBankFile_CitiBank ID="Payroll_GenerateBankFile_CitiBankControl" runat="server" Visible="False" />
            <bankControl:Payroll_GenerateBankFile_BOC ID="Payroll_GenerateBankFile_BOCControl" runat="server" Visible="False" />
            <bankControl:Payroll_GenerateBankFile_BOCNY ID="Payroll_GenerateBankFile_BOCNYControl" runat="server" Visible="False" />
            <bankControl:Payroll_GenerateBankFile_ICBC ID="Payroll_GenerateBankFile_ICBCControl" runat="server" Visible="False" />
            <bankControl:Payroll_GenerateBankFile_ANZ ID="Payroll_GenerateBankFile_ANZControl" runat="server" Visible="False" />
            <bankControl:Payroll_GenerateBankFile_BOAmerica ID="Payroll_GenerateBankFile_BOAmericaControl" runat="server" Visible="False" />
            <bankControl:Payroll_GenerateBankFile_DBS ID="Payroll_GenerateBankFile_DBSControl" runat="server" Visible="False" />
            <bankControl:Payroll_GenerateBankFile_UOB ID="Payroll_GenerateBankFile_UOBControl" runat="server" Visible="False" />
        </asp:Panel>
		<br/>
		<asp:Panel ID="panelEmployeeList" runat="server" >
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="Label6" runat="server" Text="Employee List" />
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
                <td class="pm_list_header" align="center">
                    <asp:Panel ID="EmployeeSelectAllPanel" runat="server">
                    <%-- Start 0000016, Miranda, 2014-05-30 --%>
                    <input type="checkbox" onclick="checkAll('<%=empRepeater.ClientID %>','ItemSelect',this.checked);" checked="checked" />
                    <%-- End 0000016, Miranda, 2014-05-30 --%>
                </asp:Panel>
                </td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_EmpNo" OnClick="empChangeOrder_Click" Text="Emp No"/>
                </td>
                <td align="left" class="pm_list_header" colspan="2">
                    <asp:LinkButton runat="server" ID="_EmpEngFullName" OnClick="empChangeOrder_Click" Text="Name"/>
                </td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_EmpAlias" OnClick="empChangeOrder_Click" Text="Alias"/>
                </td>
            </tr>
            <asp:Repeater ID="empRepeater" runat="server" OnItemDataBound="empRepeater_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_list" align="center">
                            <asp:CheckBox ID="ItemSelect" runat="server" Checked="True" />
                        </td>
                        <td class="pm_list">
                            <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Emp_View.aspx?EmpID=" + empSBinding.getValue(Container.DataItem,"EmpID"))%>">
                                <%#empSBinding.getValue(Container.DataItem, "EmpNo")%>
                            </a>
                        </td>
                        <td class="pm_list">
                            <%#empSBinding.getValue(Container.DataItem, "EmpEngSurname")%>
                        </td>
                        <td class="pm_list">
                            <%#empSBinding.getValue(Container.DataItem, "EmpEngOtherName")%>
                        </td>
                        <td class="pm_list">
                            <%#empSBinding.getValue(Container.DataItem, "EmpAlias")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <table width="100%" cellspacing="0" cellpadding="0" class="pm_list_pagenav">
            <tr>
                <td>
                    <asp:Panel ID="ReportToolbarPanel" runat="server" >
                        <asp:Button ID="btnGenerate" runat="server" Text="Generate Bank File" CssClass="button" OnClick="btnGenerate_Click" />
                        <asp:Button ID="btnAutoPayList" runat="server" Text="Generate Autopay List" CssClass="button" OnClick="btnAutoPayList_Click" CommandArgument="PDF"  />
                        <asp:Button ID="btnAutoPayListExcel" runat="server" Text="Generate Autopay List (Excel)" CssClass="button" OnClick="btnAutoPayList_Click" CommandArgument="Excel"  />
                    </asp:Panel>
                </td>
                <td align="right">
                    <tb:RecordListFooter id="EmpListFooter" runat="server" ShowAllRecords="true" />
                </td>
            </tr>
        </table>
        </asp:Panel> 
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content> 