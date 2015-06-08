<%@ page language="C#" autoeventwireup="true" inherits="Customize_Report_Payroll_Summary_List, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>
<%@ Register Src="~/controls/EmployeeSearchControl.ascx" TagName="EmployeeSearchControl" TagPrefix="uc2" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/controls/ReportExportControl.ascx" TagName="ReportExportControl" TagPrefix="uc3" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">

    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
        <tr>
            <td>
                <asp:Label ID="lblReportHeader" Text="Customize Payroll Summary Report" runat="server" />
            </td>
        </tr>
    </table>
    <br />
    
    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
        <tr>
            <td>
                <asp:Label runat="server" Text="Payroll Group Information" />
            </td>
        </tr>
    </table>

    <table id="Table1" width="100%" class="pm_section" runat="server" >
        <%--<tr>
            <td class="pm_field_header">
                <asp:Label Text="Company" runat="server" />:
            </td>
            <td class="pm_field">
                <asp:DropDownList ID="cboCompany" runat="server" CssClass="pm_required"/>
            </td>
        </tr>--%>
        <tr>
            <td class="pm_field_header">
                <asp:Label Text="Payroll Batch Status" runat="server" />:
            </td>
            <td class="pm_field">
                <asp:DropDownList ID="cboPayrollStatus" runat="server" CssClass="pm_required" AutoPostBack="True" OnTextChanged ="cboPayrollStatus_OnSelectedIndexChanged" >
                <asp:ListItem Text="Trial Run" Value="T" />
                <asp:ListItem Text="Confirmed" Value="C" />
                </asp:DropDownList>                
            </td>
        </tr>
        <tr id="PayrollCycleRow" visible="false" runat="server">
            <td class="pm_field_header">
                <asp:Label ID="Label1" runat="server" Text="Payroll Cycle" />:
            </td>
            <td class="pm_field">
                <asp:DropDownList ID="PayrollMonth" runat="server" CssClass="pm_required">
                    <asp:ListItem Text="January" Value="1" />
                    <asp:ListItem Text="February" Value="2" />
                    <asp:ListItem Text="March" Value="3" />
                    <asp:ListItem Text="April" Value="4" />
                    <asp:ListItem Text="May" Value="5" />
                    <asp:ListItem Text="June" Value="6" />
                    <asp:ListItem Text="July" Value="7" />
                    <asp:ListItem Text="August" Value="8" />
                    <asp:ListItem Text="September" Value="9" />
                    <asp:ListItem Text="October" Value="10" />
                    <asp:ListItem Text="November" Value="11" />
                    <asp:ListItem Text="December" Value="12" />                        
                </asp:DropDownList>
                <asp:TextBox ID ="PayrollYear" runat="server" Columns="4" MaxLength="4" CssClass="pm_required"/>
            </td>
        </tr>
        <tr>
            <td class="pm_field_header">
                <asp:Label ID="Label4" runat="server" Text="Payment Method" />:
            </td>
            <td class="pm_field">
                <asp:DropDownList ID="cboPaymentMethod" runat="server" CssClass="pm_required"/>
            </td>
        </tr>
        <tr>
            <td class="pm_field_header">
                <asp:Label ID="Label2" runat="server" Text="Report Grouping On" />:
            </td>
            <td class="pm_field">
                <asp:DropDownList ID="cboSummaryOnHierarchyLevel" runat="server" CssClass="pm_required"/>
            </td>
        </tr>
    </table> 
    
    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
        <tr>
            <td>
                <asp:Label ID="Label3" runat="server" Text="Employee Filter" />
            </td>
        </tr>
    </table>
    
    <%-- Start 0000185, KuangWei, 2015-04-21 --%>
    <asp:UpdatePanel ID="SearchUpdatePanel" runat="server" UpdateMode="Conditional" >
        <ContentTemplate >
            <uc2:EmployeeSearchControl id="EmployeeSearchControl1" runat="server" EmpStatusValue="A" />
        </ContentTemplate >
    </asp:UpdatePanel>
    <%-- End 0000185, KuangWei, 2015-04-21 --%>
    <br />
    <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
        <tr>
            <td>
                <asp:Button ID="Search" runat="server" Text="Search" CssClass="button" OnClick="Search_Click" />
                <asp:Button ID="Reset" runat="server" Text="Reset" CssClass="button" OnClick="Reset_Click" />
            </td>
        </tr>
    </table>
    <br />
    
    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
        <tr>
            <td>
                <asp:Label runat="server" Text="Employee List" />
            </td>
        </tr>
    </table>
    
    <table width="100%" cellspacing="0" cellpadding="0" class="pm_list_pagenav">
        <tr>
            <td>
                <asp:Button ID="btnGenerate" runat="server" Text="Generate" CSSClass="button" OnClick="btnGenerate_Click" />
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

        <table width="100%" cellspacing="0" cellpadding="0" class="pm_list_pagenav">
            <tr>
                <td>
                    <asp:Button ID="btnGenerate2" runat="server" Text="Generate" CSSClass="button" OnClick="btnGenerate_Click" />
                </td>
                <td align="right">
                    <tb:RecordListFooter id="ListFooter" runat="server"
                        ShowAllRecords="true"
                      />
                </td>
            </tr>
        </table>    
    
</asp:Content> 