<%@ page language="C#" masterpagefile="~/MainMasterPage.master" autoeventwireup="true" inherits="Emp_Export_List, HROneWeb.deploy" viewStateEncryptionMode="Always" %>

<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
<%@ Register Src="~/controls/EmployeeSearchControl.ascx" TagName="EmployeeSearchControl" TagPrefix="uc2" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
	<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
		<tr>
			<td>
				<asp:Label ID="lblReportHeader" runat="server" Text="Export Employee Information" />
			</td>
		</tr>
	</table>

    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" Text="Function List" />
            </td>
        </tr>
    </table>

    <asp:Panel ID="FunctionList" runat="server" >
    <table width="100%" class="pm_section">
        <tr>
            <td class="pm_search">
                    <input type="checkbox" onclick="checkAll('','FUNCTION_PER',this.checked);" />
                                    <asp:Label ID="Label3" runat="server" Text="(Select All)"  />
            </td>
        </tr>
        <tr>
            <td class="pm_search">
                <asp:CheckBox ID="FUNCTION_PER001" runat="server" Text="FUNCTION_PER001" />
            </td>
        </tr>
        <tr>
            <td class="pm_search">
                <asp:CheckBox ID="FUNCTION_PER002" runat="server" Text="FUNCTION_PER002" />
            </td>
        </tr>
        <tr>
            <td class="pm_search">
                <asp:CheckBox ID="FUNCTION_PER003" runat="server" Text="FUNCTION_PER003" />
            </td>
        </tr>
        <tr>
            <td class="pm_search">
                <asp:CheckBox ID="FUNCTION_PER004" runat="server" Text="FUNCTION_PER004" />
            </td>
        </tr>
        <tr>
            <td class="pm_search">
                <asp:CheckBox ID="FUNCTION_PER016" runat="server" Text="FUNCTION_PER016" />
            </td>                          
        </tr>
        <tr>
            <td class="pm_search">
                <asp:CheckBox ID="FUNCTION_PER015" runat="server" Text="FUNCTION_PER015" />
            </td>                           
        </tr>
        <tr>
            <td class="pm_search">
                <asp:CheckBox ID="FUNCTION_PER005" runat="server" Text="FUNCTION_PER005" />
            </td>
        </tr>
        <tr>
            <td class="pm_search">
                <asp:CheckBox ID="FUNCTION_PER006" runat="server" Text="FUNCTION_PER006" />
            </td>
        </tr>
        <tr>
            <td class="pm_search">
                <asp:CheckBox ID="FUNCTION_PER007" runat="server" Text="FUNCTION_PER007" />
            </td>
        </tr>
        <tr>
            <td class="pm_search">
                <asp:CheckBox ID="FUNCTION_PER007_1" runat="server" Text="FUNCTION_PER007-1" />
            </td>                           
        </tr>
        <tr>
            <td class="pm_search">
                <asp:CheckBox ID="FUNCTION_PER008" runat="server" Text="FUNCTION_PER008" />
            </td>                           
        </tr>
        <tr>
            <td class="pm_search">
                <asp:CheckBox ID="FUNCTION_PER019" runat="server" Text="FUNCTION_PER019" />
            </td>                           
        </tr>
        <tr>
            <td class="pm_search">
                <asp:CheckBox ID="FUNCTION_PER009" runat="server" Text="FUNCTION_PER009" />
            </td>                           
        </tr>
        <tr>
            <td class="pm_search">
                <asp:CheckBox ID="FUNCTION_PER011" runat="server" Text="FUNCTION_PER011" />
            </td>                           
        </tr>
        <%-- Start 0000196, KuangWei, 2015-05-22 --%>
        <tr>
            <td class="pm_search">
                <asp:CheckBox ID="FUNCTION_PER022" runat="server" Text="Employee Final Payment" />
            </td>                           
        </tr>        
        <%-- End 0000196, KuangWei, 2015-05-22 --%>
        <tr>
            <td class="pm_search">
                <asp:CheckBox ID="FUNCTION_PER012" runat="server" Text="FUNCTION_PER012" />
            </td>                           
        </tr>
        <tr>
            <td class="pm_search">
                <asp:CheckBox ID="FUNCTION_PER013" runat="server" Text="FUNCTION_PER013" />
            </td>                           
        </tr>
        <tr>
            <td class="pm_search">
                <asp:CheckBox ID="FUNCTION_PER017" runat="server" Text="FUNCTION_PER017" />
            </td>                           
        </tr>
        <tr>
            <td class="pm_search">
                <asp:CheckBox ID="FUNCTION_PER010" runat="server" Text="Leave Balance Adjustment" />
            </td>                           
        </tr>
        <tr>
            <td class="pm_search">
                <asp:CheckBox ID="FUNCTION_PER018" runat="server" Text="FUNCTION_PER018" />
            </td>                           
        </tr>
        <%-- Start 0000070, Miranda, 2014-09-08 --%>
        <tr>
            <td class="pm_search">
                <asp:CheckBox ID="FUNCTION_PER020" runat="server" Text="Employee Benefit" />
            </td>                           
        </tr>
        <tr>
            <td class="pm_search">
                <asp:CheckBox ID="FUNCTION_PER021" runat="server" Text="Employee Beneficiaries" />
            </td>                           
        </tr>
         <%-- End 0000070, Miranda, 2014-09-08 --%>
        <tr>
            <td class="pm_search">
                <br />
            </td>                           
        </tr>
        <tr>
            <td class="pm_search">
                <asp:CheckBox ID="IncludedEmployeeNameHierarchy" runat="server" Text="Include Employee Name, Company, Hierarchy and Position on every sheet (except Employee Personal Information)" />
            </td>                           
        </tr>
        <tr>
            <td class="pm_search">
                <asp:CheckBox ID="DisplayCodeOnly" runat="server" Text="Display Code Only" />
            </td>                           
        </tr>
        <tr>
            <td class="pm_search">
                <asp:CheckBox ID="ShowInternalID" runat="server" Text="Show SynID for update records" Visible="true" />
            </td>                           
        </tr>
    </table>
    <br />
	</asp:Panel>

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
    
        
            
    
    <asp:Panel ID="panelEmployeeList" runat="server">
        <br /><br />
		<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="Label5" runat="server" Text="Employee List" />
                </td>
            </tr>
        </table>
        <table class="pm_section" width="100%">
            <tr>
                <td>
                    <asp:Button id="btnGenerate" runat="server"  OnClick="btnGenerate_Click" Text="Generate" CssClass="button"/>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Search" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="Reset" EventName="Click" />
            <asp:PostBackTrigger ControlID="btnGenerate"/>
            <asp:PostBackTrigger ControlID="btnGenerate1"/>
        </Triggers>

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
                            <asp:Button id="btnGenerate1" runat="server"  OnClick="btnGenerate_Click" Text="Generate" CssClass="button"/>
                </td>
                <td align="right">
                    <tb:RecordListFooter id="ListFooter" runat="server"
                        ShowAllRecords="true" visible="true" 
                      />
                </td>
            </tr>
        </table>
        </ContentTemplate> 
        </asp:UpdatePanel> 
    </asp:Panel>
</asp:Content> 