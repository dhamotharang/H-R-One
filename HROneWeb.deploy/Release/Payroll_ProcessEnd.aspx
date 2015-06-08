<%@ page language="C#" autoeventwireup="true" inherits="Payroll_ProcessEnd, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>

<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>


<%@ Register Src="~/controls/Payroll_PeriodInfo.ascx" TagName="Payroll_PeriodInfo"
    TagPrefix="uc1" %>
<%-- Start 0000185, KuangWei, 2015-04-21 --%>
<%@ Register Src="~/controls/EmployeeSearchControl.ascx" TagName="EmployeeSearchControl" TagPrefix="uc2" %>
<%-- End 0000185, KuangWei, 2015-04-21 --%>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Payroll Process End" />
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
                    <asp:Label runat="server" Text="Payroll Group" />:
                </td>
                <td class="pm_field">
                    <asp:DropDownList ID="PayGroupID" runat="server" AutoPostBack="True" OnSelectedIndexChanged="PayGroupID_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Payroll Cycle" />:
                </td>
                <td class="pm_field">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional"  >
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="PayGroupID" />
                    </Triggers>
                    <ContentTemplate>
                    <asp:DropDownList ID="PayPeriodID" runat="server" OnSelectedIndexChanged="PayPeriodID_SelectedIndexChanged"
                        AutoPostBack="True" />
                    </ContentTemplate> 
                    </asp:UpdatePanel> 
                </td>
            </tr>
        </table>
        <%-- Start 0000185, KuangWei, 2015-04-21 --%>
        <asp:UpdatePanel ID="SearchUpdatePanel" runat="server" UpdateMode="Conditional" >
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
        <%-- End 0000185, KuangWei, 2015-04-21 --%>       
    <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional"  >
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="PayGroupID" />
        <asp:AsyncPostBackTrigger ControlID="PayPeriodID" />
    </Triggers>
    <ContentTemplate>
        <asp:Panel ID="panelPayPeriod" runat="server">
            <input type="hidden" id="CurrentPayPeriodID" runat="server" name="ID" />
            <uc1:Payroll_PeriodInfo ID="ucPayroll_PeriodInfo" runat="server" />
            <br />
            <table class="pm_section" width="100%">
                <tr>
                    <td align="right">
                        <asp:Button ID="Button1" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openAlert(); return false;"/>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="panelNotConfirmEmployeeList" runat="server">
                <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="The following employee has not been confirmed" />:
                        </td>
                    </tr>
                </table>
                <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
                    <col width="26px" />
                    <col width="150px" />
                    <col width="150px" />
                    <col width="350px"/>
                    <col width="150px" />
                    <tr>
                        <td class="pm_list_header" align="center" >
                        </td>
                        <td align="left" class="pm_list_header" >
                            <asp:LinkButton runat="server" ID="NotConfirm_EmpNo" OnClick="NotConfirm_ChangeOrder_Click" Text="EmpNo" />
                        </td>
                        <td align="left" class="pm_list_header" colspan="2" >
                            <asp:LinkButton runat="server" ID="NotConfirm_EmpEngFullName" OnClick="NotConfirm_ChangeOrder_Click" Text="Name" />
                        </td>
                        <td align="left" class="pm_list_header" >
                            <asp:LinkButton runat="server" ID="NotConfirm_EmpAlias" OnClick="NotConfirm_ChangeOrder_Click" Text="Alias" />
                        </td>
                    </tr>
                    <asp:Repeater ID="NotConfirm_Repeater" runat="server" OnItemDataBound="NotConfirm_Repeater_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td class="pm_list" align="center">
                                    <asp:CheckBox ID="ItemSelect" runat="server" Visible="False" />
                                </td>
                                <td class="pm_list">
                                    <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Payroll_TrialRunAdjust_View.aspx?EmpPayrollID=" + sNotConfirmEmpBinding.getValue(Container.DataItem,"EmpPayrollID"))%>">
                                        <%#sNotConfirmEmpBinding.getValue(Container.DataItem, "EmpNo")%>
                                    </a>
                                </td>
                                <td class="pm_list">
                                    <%#sNotConfirmEmpBinding.getValue(Container.DataItem, "EmpEngSurname")%>
                                </td>
                                <td class="pm_list">
                                    <%#sNotConfirmEmpBinding.getValue(Container.DataItem, "EmpEngOtherName")%>
                                </td>
                                <td class="pm_list">
                                    <%#sNotConfirmEmpBinding.getValue(Container.DataItem, "EmpAlias")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
                <tb:RecordListFooter id="NotConfirm_ListFooter" runat="server"
                    OnFirstPageClick="NotConfirm_FirstPage_Click"
                    OnPrevPageClick="NotConfirm_PrevPage_Click"
                    OnNextPageClick="NotConfirm_NextPage_Click"
                    OnLastPageClick="NotConfirm_LastPage_Click"
                  />
            </asp:Panel>
            <asp:Panel ID="panelNotTrialRunEmployeeList" runat="server">
                <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="The following employee has not been run" />:
                        </td>
                    </tr>
                </table>
                <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
                    <col width="26px" />
                    <col width="150px" />
                    <col width="150px" />
                    <col width="350px"/>
                    <col width="150px" />
                    <tr>
                        <td class="pm_list_header" align="center" >
                        </td>
                        <td align="left" class="pm_list_header" >
                            <asp:LinkButton runat="server" ID="NotTrialRun_EmpNo" OnClick="NotTrialRun_ChangeOrder_Click" Text="EmpNo" /></td>
                        <td align="left" class="pm_list_header" colspan="2" >
                            <asp:LinkButton runat="server" ID="NotTrialRun_EmpEngFullName" OnClick="NotTrialRun_ChangeOrder_Click" Text="Name" /></td>
                        <td align="left" class="pm_list_header" >
                            <asp:LinkButton runat="server" ID="NotTrialRun_EmpAlias" OnClick="NotTrialRun_ChangeOrder_Click" Text="Alias" /></td>
                    </tr>
                    <asp:Repeater ID="NotTrialRun_Repeater" runat="server" OnItemDataBound="NotTrialRun_Repeater_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td class="pm_list" align="center">
                                    <asp:CheckBox ID="ItemSelect" runat="server" Visible="False" />
                                </td>
                                <td class="pm_list">
                                    <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Emp_View.aspx?EmpID=" + sNotTrialRunEmpBinding.getValue(Container.DataItem,"EmpID"))%>">
                                        <%#sNotTrialRunEmpBinding.getValue(Container.DataItem, "EmpNo")%>
                                    </a>
                                </td>
                                <td class="pm_list">
                                    <%#sNotTrialRunEmpBinding.getValue(Container.DataItem, "EmpEngSurname")%>
                                </td>
                                <td class="pm_list">
                                    <%#sNotTrialRunEmpBinding.getValue(Container.DataItem, "EmpEngOtherName")%>
                                </td>
                                <td class="pm_list">
                                    <%#sNotTrialRunEmpBinding.getValue(Container.DataItem, "EmpAlias")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
                <tb:RecordListFooter id="NotTrialRun_ListFooter" runat="server"
                    OnFirstPageClick="NotTrialRun_FirstPage_Click"
                    OnPrevPageClick="NotTrialRun_PrevPage_Click"
                    OnNextPageClick="NotTrialRun_NextPage_Click"
                    OnLastPageClick="NotTrialRun_LastPage_Click"
                  />
            </asp:Panel>
            <table class="pm_section" width="100%">
                <tr>
                    <td align="left">
                        <asp:Panel ID="panelProcessEndOption" runat="server">
                            <asp:Button ID="btnProcessEnd" runat="server" Text="Process End" CssClass="button"
                                OnClick="btnProcessEnd_Click" />
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
      </ContentTemplate> 
      </asp:UpdatePanel> 
</asp:Content> 