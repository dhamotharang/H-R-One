<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Report_Employee_Residency.aspx.cs" Inherits="Report_Employee_Residency" MasterPageFile="~/MainMasterPage.master"  %>

<%@ Register Src="~/controls/ReportExportControl.ascx" TagName="ReportExportControl" TagPrefix="uc3" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
<%-- Start 0000185, KuangWei, 2015-05-05 --%>
<%@ Register Src="~/controls/EmployeeSearchControl.ascx" TagName="EmployeeSearchControl" TagPrefix="uc2" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
<%-- End 0000185, KuangWei, 2015-05-05 --%>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    
        
            
    
	<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
		<tr>
			<td>
				<asp:Label ID="lblReportHeader" runat="server" Text="Employee Residency Report" />
			</td>
		</tr>
	</table>
    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" Text="Report Parameter" />
            </td>
        </tr>
    </table>
    <table width="100%" class="pm_section">
        <tr>
            <td class="pm_search_header">
                <asp:Label runat="server" Text="As Of" /> :
            </td>
            <td class="pm_search">
                <uc1:WebDatePicker id="AsOfDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="pm_search_header">
                <asp:Label runat="server" Text="Group By" />:
            </td>
            <td class="pm_search">
                <asp:DropDownList ID="GroupBy" runat="server" AutoPostBack="true" OnSelectedIndexChanged="GroupBy_SelectedIndexChanged"  >
                    <asp:ListItem Text="Company" Value="Company" />
                    <asp:ListItem Text="Hierarchy Level" Value="HLevel" />
                    <asp:ListItem Text="Payroll Group" Value="PayGroup" />
                </asp:DropDownList> 
                <asp:DropDownList ID="HLevelID" runat="server" />
            </td>
        </tr>
     </table>
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td align="right">
                </td>
            </tr>
        </table>
        <%-- Start 0000185, KuangWei, 2015-05-05 --%>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="Label7" runat="server" Text="Employee Filter" />
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="SearchUpdatePanel" runat="server" UpdateMode="Conditional" >
            <ContentTemplate >
                <uc2:EmployeeSearchControl id="EmployeeSearchControl1" runat="server" EmpStatusValue="A" />
            </ContentTemplate >
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
                    <asp:Label ID="Label1" runat="server" Text="Employee List" />
                </td>
            </tr>
        </table>
        <table width="100%" cellspacing="0" cellpadding="0" class="pm_list_pagenav">
            <tr>
                <td >
                    <uc3:ReportExportControl id="ReportExportControl1" runat="server"  OnClick="btnGenerate_Click" ButtonCssClass="button"/>
                </td>
            </tr>
        </table>
        
        <ContentTemplate >
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
                <td align="left" class="pm_list_header" colspan="2">
                    <asp:LinkButton runat="server" ID="_EmpEngFullName" OnClick="ChangeOrder_Click" Text="Name" /></td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_EmpAlias" OnClick="ChangeOrder_Click" Text="Alias" /></td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_EmpStatus" OnClick="ChangeOrder_Click" Text="Status" /></td>
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
                        <td class="pm_list">
                            <%#binding.getValue(Container.DataItem,"EmpStatus")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <table width="100%" cellspacing="0" cellpadding="0" class="pm_list_pagenav">
            <tr>
                <td>
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
    <%-- End 0000185, KuangWei, 2015-05-05 --%>          
</asp:Content> 