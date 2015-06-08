<%@ page language="C#" autoeventwireup="true" inherits="Report_Taxation_Report_List, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>
<%@ Register Src="~/controls/ReportExportControl.ascx" TagName="ReportExportControl" TagPrefix="uc3" %>
<%@ Register Src="~/controls/EmployeeSearchControl.ascx" TagName="EmployeeSearchControl" TagPrefix="uc2" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="lblReportHeader" runat="server" Text="Taxation Report" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Taxation Information" />
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="TaxFormUpdatePanel" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="TaxCompID" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="TaxFormType" EventName="SelectedIndexChanged" />
        </Triggers>

        <ContentTemplate >
        <table width="100%" class="pm_section">
            <col width="30%" />
            <col width="70%" />
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label4" runat="server" Text="Taxation Company" />:
                </td>
                <td class="pm_field">
                    <asp:DropDownList ID="TaxCompID" runat="server" AutoPostBack="True" OnSelectedIndexChanged="TaxCompID_SelectedIndexChanged" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label5" runat="server" Text="Taxation Form Type" />:
                </td>
                <td class="pm_field">IR56
                    <asp:DropDownList ID="TaxFormType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="TaxFormType_SelectedIndexChanged" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label6" runat="server" Text="Year" />:
                </td>
                <td class="pm_field">
                        <asp:DropDownList runat="server" ID="TaxFormID" AutoPostBack="true" OnSelectedIndexChanged="TaxFormID_SelectedIndexChanged" />
                </td>
            </tr>
            <tr id="SignatureRow" runat="server" visible="false" >
                <td class="pm_field_header">
                    <asp:Label ID="Label1" runat="server" Text="Name of Signature" />:
                </td>
                <td class="pm_field">
                    <asp:TextBox ID="txtNameOfSignature" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label3" runat="server" Text="Language" />:
                </td>
                <td class="pm_field">
                    <asp:DropDownList runat="server" ID="ReportLanguage" />
                </td>
            </tr>
        </table>
                </ContentTemplate> 
        </asp:UpdatePanel> 

            
                
        
        <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Search" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="Reset" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="TaxCompID" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="TaxFormType" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="TaxFormID" EventName="SelectedIndexChanged" />
            
            <asp:PostBackTrigger ControlID="btnPrint"/>
        </Triggers>
        <ContentTemplate >
            <asp:Panel ID="panelEmpList" runat="server">
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                <tr>
                    <td>
                        <asp:Label runat="server" Text="Employee Filter" />
                    </td>
                </tr>
            </table>
            <uc2:EmployeeSearchControl id="EmployeeSearchControl1" runat="server" >
            <AdditionElements>
            <tr>
                <td class="pm_search_header">
                    <asp:Label ID="Label2" runat="server" Text="Last Employment Date" />:
                </td>
                <td class="pm_search" colspan="3">
                    <uc1:WebDatePicker id="LastEmploymentDateFrom" runat="server" />
                    -
                    <uc1:WebDatePicker id="LastEmploymentDateTo" runat="server" />
                 </td>
            </tr>
            </AdditionElements>
            </uc2:EmployeeSearchControl> 
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
                        <asp:Label runat="server" Text="Employee List" />
                    </td>
                </tr>
            </table>
            <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
                <col width="24px" />
                <col width="100px" />
                <colgroup width="350px">
                    <col width="150px" />
                    <col />
                </colgroup>
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
                        <asp:LinkButton runat="server" ID="_EmpTermLastDate" OnClick="ChangeOrder_Click" Text="Last Date" />
                    </td>
                </tr>
                <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td class="pm_list" align="center">
                                <asp:CheckBox ID="ItemSelect" runat="server" />
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
                                <%#sbinding.getFValue(Container.DataItem, "EmpTermLastDate", "yyyy-MM-dd")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <table width="100%" cellspacing="0" cellpadding="0" class="pm_list_pagenav">
                <tr>
                    <td>
                        <asp:Button ID="btnPrint" runat="server" Text="Print" CSSClass="button" OnClick="btnPrint_Click" />
                    </td>
                    <td align="right">
                        <tb:RecordListFooter id="ListFooter" runat="server"
                            ShowAllRecords="true"
                          />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        </ContentTemplate> 
        </asp:UpdatePanel> 
</asp:Content> 