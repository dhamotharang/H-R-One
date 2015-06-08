<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Payroll_GenerateMPFFile_List.aspx.cs"    Inherits="Payroll_GenerateMPFFile_List" MasterPageFile="~/MainMasterPage.master" %>

<%@ Register Src="~/controls/ReportExportControl.ascx" TagName="ReportExportControl" TagPrefix="uc3" %>
<%@ Register Src="~/controls/Payroll_PeriodInfo.ascx" TagName="Payroll_PeriodInfo" TagPrefix="uc1" %>
<%@ Register Src="~/controls/Payroll_GenerateMPFFile_HSBC.ascx" TagName="Payroll_GenerateMPFFile_HSBC" TagPrefix="mpfControl" %>
<%@ Register Src="~/controls/Payroll_GenerateMPFFile_BOCI.ascx" TagName="Payroll_GenerateMPFFile_BOCI" TagPrefix="mpfControl" %>
<%@ Register Src="~/controls/Payroll_GenerateMPFFile_Manulife.ascx" TagName="Payroll_GenerateMPFFile_Manulife" TagPrefix="mpfControl" %>
<%@ Register Src="~/controls/Payroll_GenerateMPFFile_AIA.ascx" TagName="Payroll_GenerateMPFFile_AIA" TagPrefix="mpfControl" %>
<%@ Register Src="~/controls/Payroll_GenerateMPFFile_HSBCOIS.ascx" TagName="Payroll_GenerateMPFFile_HSBCOIS" TagPrefix="mpfControl" %>

<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
<%-- Start 0000004, Miranda, 2014-06-19 --%>
<input type="hidden" id="hiddenFieldDefaultMonthPeriod" runat="server" value="1" />
<%-- End 0000004, Miranda, 2014-06-19 --%>
	<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
		<tr>
			<td>
				<asp:Label ID="lblReportHeader" runat="server" Text="Generate MPF File" />
			</td>
		</tr>
	</table>
    <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_section_title">
        <tr>
            <td>
                <asp:Label runat="server" Text="MPF Detail" />:
            </td>
        </tr>
    </table>
    <%-- Start 0000004, Miranda, 2014-06-19 --%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <%-- End 0000004, Miranda, 2014-06-19 --%>
    <table width="100%" class="pm_section">
        <tr>
            <td class="pm_field_header">
                <asp:Label runat="server" Text="MPF Plan" />:
            </td>
            <td class="pm_field">
                <asp:DropDownList ID="MPFPlanID" runat="server"  cssclass="pm_required" AutoPostBack="true" OnSelectedIndexChanged="MPFPlanID_SelectedIndexChanged"  >
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
        <td colspan="2">
        </td>
        </tr>
        <tr>
            <td class="pm_field_header">
                <asp:Label runat="server" Text="Payroll Period" />:
            </td>
            <td class="pm_field">
                <uc1:WebDatePicker id="PayPeriodFr" runat="server" OnChanged="PayPeriod_Changed" AutoPostBack="true" />
                 -
                <uc1:WebDatePicker id="PayPeriodTo" runat="server" OnChanged="PayPeriod_Changed" AutoPostBack="true" />
            </td>
        </tr>
    </table>
    <%-- Start 0000004, Miranda, 2014-06-19 --%>
    </ContentTemplate>
    </asp:UpdatePanel>
    <%-- End 0000004, Miranda, 2014-06-19 --%>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional"  >
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="MPFPlanID"  EventName="SelectedIndexChanged" />
        <%-- Start 0000004, Miranda, 2014-06-19 --%>
        <asp:AsyncPostBackTrigger ControlID="PayPeriodFr" EventName="Changed" />
        <asp:AsyncPostBackTrigger ControlID="PayPeriodTo" EventName="Changed" />
        <%-- End 0000004, Miranda, 2014-06-19 --%>
    </Triggers>
    <ContentTemplate>
    <mpfControl:Payroll_GenerateMPFFile_HSBC ID="Payroll_GenerateMPFFile_HSBCControl" runat="server" Visible="False" />

    <mpfControl:Payroll_GenerateMPFFile_BOCI ID="Payroll_GenerateMPFFile_BOCIControl" runat="server" Visible="False" OnParameterChange="MPFPlanID_SelectedIndexChanged" />

    <mpfControl:Payroll_GenerateMPFFile_Manulife ID="Payroll_GenerateMPFFile_ManulifeControl" runat="server" Visible="False" />
    <mpfControl:Payroll_GenerateMPFFile_AIA ID="Payroll_GenerateMPFFile_AIAControl" runat="server" Visible="False" />
    
    <mpfControl:Payroll_GenerateMPFFile_HSBCOIS ID="Payroll_GenerateMPFFile_HSBCOISControl" runat="server" Visible="False" />

    </ContentTemplate> 
    </asp:UpdatePanel>     
        
            
    
    <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional"  >
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="MPFPlanID"  EventName="SelectedIndexChanged" />
        <asp:AsyncPostBackTrigger ControlID="PayPeriodFr" EventName="Changed" />
        <asp:AsyncPostBackTrigger ControlID="PayPeriodTo" EventName="Changed" />
        <asp:AsyncPostBackTrigger ControlID="Payroll_GenerateMPFFile_HSBCControl" />
        <asp:PostBackTrigger ControlID="btnGenerate" />
    </Triggers>
    <ContentTemplate>
    <asp:Panel ID="panelEmployeeList" runat="server">
        <br /><br />
		<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Employee List" />
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
                    <asp:Button ID="btnGenerate" runat="server" Text="Generate MPF File" cssclass="button" OnClick="btnGenerate_Click" />
                </td>
                <td align="right">
                    <tb:RecordListFooter id="ListFooter" runat="server"
                        ShowAllRecords="true" visible="true" 
                      />
                </td>
            </tr>
        </table>
    </asp:Panel>
    </ContentTemplate> 
    </asp:UpdatePanel> 
</asp:Content> 