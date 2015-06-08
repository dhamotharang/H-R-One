<%@ page language="C#" masterpagefile="~/MainMasterPage.master" autoeventwireup="true" inherits="Attendance_GenerateCompensationLeaveEntitle_List, HROneWeb.deploy" viewStateEncryptionMode="Always" %>


<%@ Register Src="~/controls/Payroll_PeriodInfo.ascx" TagName="Payroll_PeriodInfo"
    TagPrefix="uc1" %>
<%@ Register Src="~/controls/EmployeeSearchControl.ascx" TagName="EmployeeSearchControl" TagPrefix="uc2" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
	<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
		<tr>
			<td>
				<asp:Label ID="Label1" runat="server" Text="Generate Compensation Leave Entitlement" />
			</td>
		</tr>
	</table>
    <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_section_title">
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" Text="Detail" />:
            </td>
        </tr>
    </table>
    <table width="100%" class="pm_section">
        <tr>
            <td class="pm_search_header">
                <asp:Label ID="Label4" runat="server" Text="Period" />:
            </td>
            <td class="pm_search">
                <uc1:WebDatePicker id="PeriodFr" runat="server" />
                -
                <uc1:WebDatePicker id="PeriodTo" runat="server" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="SearchUpdatePanel" runat="server" UpdateMode="Conditional" >
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="Reset" EventName="Click" />
    </Triggers>

    <ContentTemplate >
        <uc2:EmployeeSearchControl id="EmployeeSearchControl1" runat="server" />
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
    
        
            
    
    <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional" >
    <Triggers>
        <asp:PostBackTrigger ControlID="btnGenerate" />
        <asp:AsyncPostBackTrigger ControlID="Search" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="Reset" EventName="Click" />
    </Triggers>

    <ContentTemplate >
    <asp:Panel ID="panelEmployeeList" runat="server">
        <br /><br />
		<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="Label5" runat="server" Text="Employee List" />
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
                    <input type="checkbox" onclick="checkAll('<%=empRepeater.ClientID %>','ItemSelect',this.checked);" />
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
                    <asp:Button id="btnGenerate" runat="server"  OnClick="btnGenerate_Click" Text="Generate" CssClass="button"/>
                </td>
                <td>
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