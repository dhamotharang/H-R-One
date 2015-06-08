<%@ page language="C#" autoeventwireup="true" inherits="Payroll_TrialRun_List, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>

<%@ Register Src="~/controls/Payroll_PeriodInfo.ascx" TagName="Payroll_PeriodInfo"
    TagPrefix="uc1" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
<%@ Register Src="~/controls/EmployeeSearchControl.ascx" TagName="EmployeeSearchControl" TagPrefix="uc2" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">

    <input type="hidden" id="PayGroupID" runat="server" />
    <input type="hidden" id="PayPeriodID" runat="server" />
	
	<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
         <tr>
             <td>
                    <asp:Label ID="lblTitle" runat="server" Text="Payroll Trial Run" />
             </td>
         </tr>
    </table>
    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
        <tr>
            <td>
                <asp:Label runat="server" Text="Payroll Trial Run Employee List" />
            </td>
        </tr>
    </table>
    
        
            
    
    <table width="100%">
        <tr>
            <td>
                                <asp:Button ID="Back" runat="server" Text="- Back -" OnClick="Back_Click" UseSubmitBehavior="false"
                                    cssclass="button" />

            </td>
        </tr>
        <tr>
            <td>
                <uc1:Payroll_PeriodInfo ID="ucPayroll_PeriodInfo" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
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
            </td>
        </tr>
        </table> 
        <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Search" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="Reset" EventName="Click" />
        </Triggers>

        <ContentTemplate >
        <table width="100%">
        <tr>
            <td>
                <asp:Panel ID="panelExistingEmployee" runat="server" Visible="False">
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
                        <td class="pm_field_title" colspan="5">
                            <asp:Label runat="server" Text="Existing Employee" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pm_list_header" align="center">
                            <asp:Panel ID="ExistingEmployeeSelectAllPanel" runat="server">
                            <%-- Start 0000016, Miranda, 2014-05-30 --%>
                            <input type="checkbox" onclick="checkAll('<%=ExistingEmployeeRepeater.ClientID %>','ItemSelect',this.checked);" checked="checked" />
                            <%-- End 0000016, Miranda, 2014-05-30 --%>
                        </asp:Panel>
                        </td>
                        <td align="left" class="pm_list_header" >
                            <asp:LinkButton runat="server" ID="ExistingEmployee_EmpNo" OnClick="ExistingEmployeeChangeOrder_Click" Text="Emp No"/>
                        </td>
                        <td align="left" class="pm_list_header" colspan="2" >
                            <asp:LinkButton runat="server" ID="ExistingEmployee_EmpEngFullName" OnClick="ExistingEmployeeChangeOrder_Click" Text="Name"/>
                        </td>
                        <td align="left" class="pm_list_header" >
                            <asp:LinkButton runat="server" ID="ExistingEmployee_EmpAlias" OnClick="ExistingEmployeeChangeOrder_Click" Text="Alias"/>
                        </td>
                    </tr>
                    <asp:Repeater ID="ExistingEmployeeRepeater" runat="server" OnItemDataBound="Repeater_ItemDataBound" Visible="True">
                        <ItemTemplate>
                            <tr>
                                <td class="pm_list" align="center">
                                    <asp:CheckBox ID="ItemSelect" runat="server" />
                                </td>
                                <td class="pm_list">
                                    <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Emp_View.aspx?EmpID=" + existingEmployeeSearchBinding.getValue(Container.DataItem,"EmpID"))%>">
                                        <%#existingEmployeeSearchBinding.getValue(Container.DataItem, "EmpNo")%>
                                    </a>
                                </td>
                                <td class="pm_list" >
                                    <%#existingEmployeeSearchBinding.getValue(Container.DataItem, "EmpEngSurname")%>
                                </td>
                                <td class="pm_list">
                                    <%#existingEmployeeSearchBinding.getValue(Container.DataItem, "EmpEngOtherName")%>
                                </td>
                                <td class="pm_list">
                                    <%#existingEmployeeSearchBinding.getValue(Container.DataItem, "EmpAlias")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
                <tb:RecordListFooter id="existingEmployeeListFooter" runat="server"
                    ShowAllRecords="true" visible="true" 
                  />
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="panelNewJoin" runat="server" Visible="False">
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
                        <td class="pm_field_title" colspan="5">
                            <asp:Label runat="server" Text="New Join Employee" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pm_list_header" align="center">
                            <asp:Panel ID="NewJoinSelectAllPanel" runat="server">
                            <input type="checkbox" onclick="checkAll('<%=NewJoinRepeater.ClientID %>','ItemSelect',this.checked);" />
                        </asp:Panel>
                        </td>
                        <td align="left" class="pm_list_header" >
                            <asp:LinkButton runat="server" ID="NewJoin_EmpNo" OnClick="NewJoinChangeOrder_Click" Text="Emp No"/>
                        </td>
                        <td align="left" class="pm_list_header" colspan="2" >
                            <asp:LinkButton runat="server" ID="NewJoin_EmpEngFullName" OnClick="NewJoinChangeOrder_Click" Text="Name"/>
                        </td>
                        <td align="left" class="pm_list_header" >
                            <asp:LinkButton runat="server" ID="NewJoin_EmpAlias" OnClick="NewJoinChangeOrder_Click" Text="Alias"/>
                        </td>
                    </tr>
                    <asp:Repeater ID="NewJoinRepeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td class="pm_list" align="center">
                                    <asp:CheckBox ID="ItemSelect" runat="server" />
                                </td>
                                <td class="pm_list">
                                    <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Emp_View.aspx?EmpID=" + newJoinSearchBinding.getValue(Container.DataItem,"EmpID"))%>">
                                        <%#newJoinSearchBinding.getValue(Container.DataItem, "EmpNo")%>
                                    </a>
                                </td>
                                <td class="pm_list" >
                                    <%#newJoinSearchBinding.getValue(Container.DataItem, "EmpEngSurname")%>
                                </td>
                                <td class="pm_list">
                                    <%#newJoinSearchBinding.getValue(Container.DataItem, "EmpEngOtherName")%>
                                </td>
                                <td class="pm_list">
                                    <%#newJoinSearchBinding.getValue(Container.DataItem, "EmpAlias")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
                <tb:RecordListFooter id="newJoinListFooter" runat="server"
                    ShowAllRecords="true" visible="true" 
                  />
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="panelFinalPayment" runat="server" Visible="False">
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
                        <td class="pm_field_title" colspan="5">
                            <asp:Label runat="server" Text="Terminated Employee" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pm_list_header" align="center">
                            <asp:Panel ID="FinalPaymentSelectAllPanel" runat="server">
                            <input type="checkbox" onclick="checkAll('<%=FinalPaymentRepeater.ClientID %>','ItemSelect',this.checked);" />
                        </asp:Panel>
                        </td>
                        <td align="left" class="pm_list_header" >
                            <asp:LinkButton runat="server" ID="FinalPayment_EmpNo" OnClick="FinalPaymentChangeOrder_Click" Text="Emp No"/>
                        </td>
                        <td align="left" class="pm_list_header" colspan="2" >
                            <asp:LinkButton runat="server" ID="FinalPayment_EmpEngFullName" OnClick="FinalPaymentChangeOrder_Click" Text="Name"/>
                        </td>
                        <td align="left" class="pm_list_header" >
                            <asp:LinkButton runat="server" ID="FinalPayment_EmpAlias" OnClick="FinalPaymentChangeOrder_Click" Text="Alias"/>
                        </td>
                    </tr>
                    <asp:Repeater ID="FinalPaymentRepeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td class="pm_list" align="center">
                                    <asp:CheckBox ID="ItemSelect" runat="server" />
                                </td>
                                <td class="pm_list">
                                    <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Emp_View.aspx?EmpID=" + finalPaymentSearchBinding.getValue(Container.DataItem,"EmpID"))%>">
                                        <%#finalPaymentSearchBinding.getValue(Container.DataItem, "EmpNo")%>
                                    </a>
                                </td>
                                <td class="pm_list" >
                                    <%#finalPaymentSearchBinding.getValue(Container.DataItem, "EmpEngSurname")%>
                                </td>
                                <td class="pm_list">
                                    <%#finalPaymentSearchBinding.getValue(Container.DataItem, "EmpEngOtherName")%>
                                </td>
                                <td class="pm_list">
                                    <%#finalPaymentSearchBinding.getValue(Container.DataItem, "EmpAlias")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
                <tb:RecordListFooter id="finalPayment" runat="server"
                    ShowAllRecords="true" visible="true" 
                  />
                </asp:Panel> 
            </td>
        </tr>
        <tr>
            <td align="left" class="pm_field">
                <asp:Button ID="btnProcess" runat="server" Text="Process" CSSClass="button" 
                    onclick="btnProcess_Click"/>
                    <asp:CheckBox ID="chkSkipMPFCal" runat="server" Text="Skip MPF calculation if recurring payment calculation does not exist" Checked="true"  />
            </td>
        </tr>
    </table>
    </ContentTemplate> 
    </asp:UpdatePanel> 
</asp:Content> 