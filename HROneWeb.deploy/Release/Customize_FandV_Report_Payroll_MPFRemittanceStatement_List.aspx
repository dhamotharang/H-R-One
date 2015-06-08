<%@ page language="C#" autoeventwireup="true" inherits="Customize_FandV_Report_Payroll_MPFRemittanceStatement_List, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>
<%@ Register Src="~/controls/EmployeeSearchControl.ascx" TagName="EmployeeSearchControl" TagPrefix="uc2" %>
<%@ Register Src="~/controls/Payroll_PeriodInfo.ascx" TagName="Payroll_PeriodInfo" TagPrefix="uc1" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
<%-- Start 0000004, Miranda, 2014-06-19 --%>
<input type="hidden" id="hiddenFieldDefaultMonthPeriod" runat="server" value="1" />
<%-- End 0000004, Miranda, 2014-06-19 --%>
	<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
		<tr>
			<td>
				<asp:Label ID="lblReportHeader" runat="server" Text="MPF Remittance Statement" />
			</td>
		</tr>
	</table>
    <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_section_title">
        <tr>
            <td>
                <asp:Label runat="server" Text="Report Parameters" />
            </td>
        </tr>
    </table>
    <table width="100%" class="pm_section">
        <tr>
            <td class="pm_field_header">
                <asp:Label ID="Label2" runat="server" Text="MPF Plan" />:
            </td>
            <td class="pm_field">
                <asp:DropDownList ID="MPFPlanID" runat="server"  cssclass="pm_required">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="pm_field_header">
                <asp:Label ID="Label3" runat="server" Text="P-Fund Plan" />:
            </td>
            <td class="pm_field">
                <asp:DropDownList ID="ORSOPlanID" runat="server"  cssclass="pm_required">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="pm_field_header">
                <asp:Label runat="server" Text="Payroll Period" />:
            </td>
            <td class="pm_field">
                <%-- Start 0000004, Miranda, 2014-06-19 --%>
                <uc1:WebDatePicker id="PayPeriodFr" runat="server" OnChanged="PayPeriod_Changed" AutoPostBack="true" cssClass="pm_required" />
                -
                <uc1:WebDatePicker id="PayPeriodTo" runat="server" OnChanged="PayPeriod_Changed" AutoPostBack="true" cssClass="pm_required" />
                <%-- End 0000004, Miranda, 2014-06-19 --%>
            </td>
        </tr>        
        <tr>            
            <td class="pm_field_header">
                <asp:Label ID="abc" runat="server" Text="Cheque Number" />:
            </td>
            <td class="pm_field">
                <asp:TextBox ID="txtChequeNo" runat="server" MaxLength="10" />
            </td>
        </tr>
    </table>
    <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_section_title">
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Employee Filter" />
            </td>
        </tr>
    </table>
    
    <asp:UpdatePanel ID="SearchUpdatePanel" runat="server" UpdateMode="Conditional" >
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="Reset" EventName="Click" />
    </Triggers>
    <ContentTemplate >
        <uc2:EmployeeSearchControl id="EmployeeSearchControl1" runat="server" />
    </ContentTemplate> 
    </asp:UpdatePanel>     

    <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
        <tr />
            <td>
                <asp:Button ID="Search" runat="server" Text="Search" CssClass="button" OnClick="Search_Click" />
                <asp:Button ID="Reset" runat="server" Text="Reset" CssClass="button" OnClick="Reset_Click" />
            </td>
        </tr>
    </table>
    
    <asp:Panel ID="panelEmployeeList" runat="server">

		<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Employee List" />
                </td>
            </tr>
        </table>
        <table width="100%" class="pm_list_pagenav">
            <tr>
                <td>
                    <asp:Button id="btnGenerate2" runat="server"  Text="Generate" OnClick="btnGenerate_Click" CssClass="button"/>
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
                    <%-- Start 0000016, Miranda, 2014-05-30 --%>
                    <input type="checkbox" onclick="checkAll('<%=empRepeater.ClientID %>','ItemSelect',this.checked);" checked="checked" />
                    <%-- End 0000016, Miranda, 2014-05-30 --%>
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
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_EmpStatus" OnClick="empChangeOrder_Click" Text="Status" /></td>                       
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
                        <td class="pm_list">
                            <%#empSBinding.getValue(Container.DataItem, "EmpStatus")%>
                        </td>                           
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <table width="100%" cellspacing="0" cellpadding="0" class="pm_list_pagenav">
            <tr>
                <td>
                    <asp:Button id="btnGenerate1" runat="server" Text="Generate" OnClick="btnGenerate_Click" CssClass ="button" />
                </td>
                <td align="right">
                    <tb:RecordListFooter id="ListFooter" runat="server"
                        ShowAllRecords="true"
                      />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content> 