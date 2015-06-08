<%--<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Payroll_GenerateBatchBankFile_View.aspx.cs"    Inherits="Payroll_GenerateBatchBankFile_View" MasterPageFile="~/MainMasterPage.master" MaintainScrollPositionOnPostback="true" %>



<%@ Register Src="~/controls/Payroll_PeriodInfo.ascx" TagName="Payroll_PeriodInfo" TagPrefix="uc1" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
<%@ Register Src="~/controls/Payroll_GenerateBankFile_HSBC.ascx" TagName="Payroll_GenerateBankFile_HSBC" TagPrefix="bankControl" %>
<%@ Register Src="~/controls/Payroll_GenerateBankFile_SCB.ascx" TagName="Payroll_GenerateBankFile_SCB"TagPrefix="bankControl" %>
<%@ Register Src="~/controls/Payroll_GenerateBankFile_CitiBank.ascx" TagName="Payroll_GenerateBankFile_CitiBank" TagPrefix="bankControl" %>
<%@ Register Src="~/controls/Payroll_GenerateBankFile_BOC.ascx" TagName="Payroll_GenerateBankFile_BOC" TagPrefix="bankControl" %>
<%@ Register Src="~/controls/Payroll_GenerateBankFile_ICBC.ascx" TagName="Payroll_GenerateBankFile_ICBC" TagPrefix="bankControl" %>
<%@ Register Src="~/controls/Payroll_GenerateBankFile_BOCNY.ascx" TagName="Payroll_GenerateBankFile_BOCNY" TagPrefix="bankControl" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
	<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
         <tr>
             <td>
                    <asp:Label runat="server" Text="Generate Bank File" />
             </td>
         </tr>
    </table>
	<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
        <tr>
            <td>
                <asp:Label runat="server" Text="Payroll Group Information" />
            </td>
        </tr>
    </table>
    <table width="100%" class="pm_section">
        <tr>
            <td class="pm_field_header">
                <asp:Label runat="server" Text="Payroll Cycle" />:
            </td>
            <td class="pm_search">
                <uc1:WebDatePicker id="PayPeriodFr" runat="server" OnChanged="PayPeriod_Changed" AutoPostBack="true" />
                 -
                <uc1:WebDatePicker id="PayPeriodTo" runat="server" OnChanged="PayPeriod_Changed" AutoPostBack="true" />
            </td>
        </tr>
    </table>
    
        
            
    <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional"  >
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="PayPeriodFr" EventName="Changed" />
        <asp:AsyncPostBackTrigger ControlID="PayPeriodTo" EventName="Changed" />
        <asp:AsyncPostBackTrigger ControlID="btnGenerate" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnAutoPayList" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnAutoPayListExcel" EventName="Click" />
    </Triggers>
    <ContentTemplate>
    <asp:Panel ID="panelPayPeriod" runat="server">
        <asp:Panel ID="panelBankFileInfo" runat="server">
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Payroll Batch List" />
                    </td>
                </tr>
            </table>
            <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
                <tr>
                    <td class="pm_list_header" align="center">
                        <asp:CheckBox ID="chkPayBatchCheckAll" runat="server" AutoPostBack="True" />
                    </td>
                    <td align="left" class="pm_list_header">
                        <asp:LinkButton runat="server" ID="_PayGroupDesc" OnClick="payBatchChangeOrder_Click" Text="Payroll Group"/>
                    </td>
                    <td align="left" class="pm_list_header">
                        <asp:LinkButton runat="server" ID="_PayPeriodFr" OnClick="payBatchChangeOrder_Click" Text="Period"/>
                    </td>
                    <td align="left" class="pm_list_header">
                        <asp:LinkButton runat="server" ID="_PayBatchConfirmDate" OnClick="payBatchChangeOrder_Click" Text="Confirm Date"/>
                    </td>
                    <td align="left" class="pm_list_header">
                        <asp:LinkButton runat="server" ID="_PayBatchFileGenDate" OnClick="payBatchChangeOrder_Click" Text="Last Generation Date"/>
                    </td>
                    <td align="left" class="pm_list_header">
                        <asp:LinkButton runat="server" ID="_PayBatchValueDate" OnClick="payBatchChangeOrder_Click" Text="Previous Value Date"/>
                    </td>
                    <td align="left" class="pm_list_header">
                        <asp:LinkButton runat="server" ID="_PayBatchFileGenBy" OnClick="payBatchChangeOrder_Click" Text="Generated By"/>
                    </td>
                    <td align="left" class="pm_list_header">
                        <asp:LinkButton runat="server" ID="_PayBatchRemark" OnClick="payBatchChangeOrder_Click" Text="Remark"/>
                    </td>
                </tr>
                <asp:Repeater ID="payBatchRepeater" runat="server" OnItemDataBound="payBatchRepeater_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td class="pm_list" align="center">
                                <asp:CheckBox ID="ItemSelect" runat="server" Checked="True"  OnCheckedChanged="PayBatchItem_OnCheckedChanged" AutoPostBack="true" />
                            </td>
                            <td class="pm_list">
                                <%#payBatchSBinding.getValue(Container.DataItem, "PayGroupDesc")%>
                            </td>
                            <td class="pm_list">
                                <%#payBatchSBinding.getFValue(Container.DataItem, "PayPeriodFr","yyyy-MM-dd" )%>
                            </td>
                            <td class="pm_list">
                                <%#payBatchSBinding.getFValue(Container.DataItem, "PayBatchConfirmDate","yyyy-MM-dd HH:mm:ss")%>
                            </td>
                            <td class="pm_list">
                                <%#payBatchSBinding.getFValue(Container.DataItem, "PayBatchFileGenDate", "yyyy-MM-dd HH:mm:ss")%>
                            </td>
                            <td class="pm_list">
                                <%#payBatchSBinding.getFValue(Container.DataItem, "PayBatchValueDate", "yyyy-MM-dd")%>
                            </td>
                            <td class="pm_list">
                                <%#payBatchSBinding.getValue(Container.DataItem, "PayBatchFileGenBy")%>
                            </td>
                            <td class="pm_list">
                                <%#payBatchSBinding.getValue(Container.DataItem, "PayBatchRemark")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <table width="100%" cellspacing="0" cellpadding="0" class="pm_list_pagenav">
                <tr>
                    <td align="right">
                        <tb:RecordListFooter id="PayBatchListFooter" runat="server"
                            ShowAllRecords="true" 
                          />
                    </td>
                </tr>
            </table>
        <br />
        
            <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
                <tr>
                    <td class="pm_field_title" colspan="3">
                        <asp:Label ID="Label1" Text="Bank File Information" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="pm_field_header">
                        <asp:Label runat="server" Text="Bank Account" />
                    </td>
                    <td class="pm_field">
                        <asp:DropDownList ID="BankAccount" runat="server" CssClass="pm_required" OnSelectedIndexChanged="BankAccount_SelectedIndexChanged" AutoPostBack="true" />
                        <asp:CheckBox ID="chkShowAllCompany" runat="server" Text="Show all bank account" AutoPostBack="true"  />
                    </td>
                </tr>
                <tr>
                    <td class="pm_field_header">
                        <asp:Label runat="server" Text="Value Date" />
                    </td>
                    <td class="pm_field">
                        <uc1:WebDatePicker id="ValueDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="pm_field_header">
                        <asp:Label runat="server" Text="Bank File Type" />
                    </td>
                    <td class="pm_field">
                        <asp:DropDownList ID="BankFileType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="BankFileType_SelectedIndexChanged">
                            <asp:ListItem Text="HSBC/Hang Seng Bank" Value="HSBC" />
                            <asp:ListItem Text="Standard Chartered Bank" Value="SCB" />
							<asp:ListItem Text="Bank Of China" Value="BOC" />
							<asp:ListItem Text="Bank Of China (Nan Yang)" Value="BOCNY" />
							<asp:ListItem Text="ICBC (Asia)" Value="ICBC" />
							<asp:ListItem Text="Bank of East Asia" Value="BEA" Enabled="false" />
							<asp:ListItem Text="Citibank" Value="CitiBank" />
							<asp:ListItem Text="Wing Lung Bank" Value="WLB" Enabled="false"/>
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
            </asp:Panel>
			<br/>
			<asp:Panel ID="panelEmployeeList" runat="server" >
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                <tr>
                    <td>
                        <asp:Label runat="server" Text="Employee List" />
                    </td>
                </tr>
            </table>
            <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
                <tr>
                    <td class="pm_list_header"align="center">
                        <asp:Panel ID="EmployeeSelectAllPanel" runat="server">
                        <input type="checkbox" onclick="checkAll('<%=empRepeater.ClientID %>','ItemSelect',this.checked);" />
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
                        <tb:RecordListFooter id="EmpListFooter" runat="server"
                            ShowAllRecords="true" 
                          />
                    </td>
                </tr>
            </table>
            </asp:Panel> 
        </asp:Panel>
    </asp:Panel>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content> --%>