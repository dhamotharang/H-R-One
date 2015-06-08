<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Attendance_GeneratePayment_List.aspx.cs" Inherits="Attendance_GeneratePayment_List" MasterPageFile="~/MainMasterPage.master"  %>

<%@ Register Src="~/controls/ReportExportControl.ascx" TagName="ReportExportControl" TagPrefix="uc3" %>
<%@ Register Src="~/controls/EmployeeSearchControl.ascx" TagName="EmployeeSearchControl" TagPrefix="uc2" %>
<%@ Register Src="~/controls/Payroll_PeriodInfo.ascx" TagName="Payroll_PeriodInfo" TagPrefix="uc1" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label Text="Generate Attendance Payment" runat="server" />
                </td>
            </tr>
        </table>
        <table width="100%" class="pm_section">
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="ILabel1" Text="Payroll Group" runat="server" />
                    :
                </td>
                <td class="pm_field">
                    <asp:DropDownList ID="PayGroupID" runat="server" AutoPostBack="True" OnSelectedIndexChanged="PayGroupID_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="ILabel2" Text="Payroll Cycle" runat="server" />
                    :
                </td>
                <td class="pm_field">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="PayGroupID" EventName="SelectedIndexChanged" />
                    </Triggers>

                    <ContentTemplate >
                    <asp:DropDownList ID="PayPeriodID" runat="server" OnSelectedIndexChanged="PayPeriodID_SelectedIndexChanged" AutoPostBack="True" />
                    </ContentTemplate>
                    </asp:UpdatePanel> 
                </td>
            </tr>
        </table>
        
             
        
    <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional" >
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="PayGroupID" EventName="SelectedIndexChanged" />
        <asp:AsyncPostBackTrigger ControlID="PayPeriodID" EventName="SelectedIndexChanged" />
        <asp:AsyncPostBackTrigger ControlID="Search" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="Reset" EventName="Click" />
        <asp:PostBackTrigger ControlID="btnGenerate" />
        <asp:PostBackTrigger ControlID="btnGenerate2" />
    </Triggers>

    <ContentTemplate >
        <asp:Panel ID="panelPayPeriod" runat="server">
            <input type="hidden" id="CurrentPayPeriodID" runat="server" name="ID" />
            <uc1:Payroll_PeriodInfo ID="ucPayroll_PeriodInfo" runat="server" />
            <asp:Panel ID="panelEmployeeList" runat="server">
                <uc2:EmployeeSearchControl id="EmployeeSearchControl1" runat="server" />
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
                            <asp:Label ID="ILabel3" Text="Employee List" runat="server" />
                        </td>
                    </tr>
                </table>
                <table class="pm_section" width="100%">
                    <tr>
                        <td>
                            <asp:Button id="btnGenerate" runat="server"  Text="Generate" CssClass="button" OnCommand="btnGenerate_Command" CommandName="Export" />
                        </td>
                    </tr>
                </table>
                <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
                    <col width="26px" />
                    <col width="150px" />
                    <col width="150px" />
                    <col width="350px" />
                    <col width="150px" />
                    <col width="100px" />
                    <tr>
                        <td class="pm_list_header" align="center">
                            <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
                        </td>
                        <td align="left" class="pm_list_header">
                            <asp:LinkButton runat="server" ID="_EmpNo" OnClick="ChangeOrder_Click" Text="EmpNo" />
                        </td>
                        <td align="left" class="pm_list_header" colspan="2">
                            <asp:LinkButton runat="server" ID="_EmpEngFullName" OnClick="ChangeOrder_Click" Text="Name" />
                        </td>
                        <td align="left" class="pm_list_header">
                            <asp:LinkButton runat="server" ID="_EmpAlias" OnClick="ChangeOrder_Click" Text="Alias" />
                        </td>
                        <td align="left" class="pm_list_header">
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
                                    <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Emp_VIew.aspx?EmpID=" + sbinding.getValue(Container.DataItem,"EmpID"))%>">
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
                            <asp:Button id="btnGenerate2" runat="server" Text="Generate" CssClass="button" OnCommand="btnGenerate_Command" CommandName="Export" />
                            <asp:Button id="btnImportToCND" runat="server" Text="Import to Claims and Deductions" OnCommand="btnGenerate_Command" CssClass="button" CommandName="ImportCND"/>
                        </td>
                        <td align="right">
                            <tb:RecordListFooter id="ListFooter" runat="server" ShowAllRecords="true" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </asp:Panel>
        </ContentTemplate> 
        </asp:UpdatePanel> 
</asp:Content> 