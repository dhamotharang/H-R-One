<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Payroll_StopPayment.aspx.cs"    Inherits="Payroll_StopPayment" MasterPageFile="~/MainMasterPage.master"   %>

<%@ Register Src="~/controls/Payroll_PeriodSelectionList.ascx" TagName="Payroll_PeriodSelectionList" TagPrefix="uc1" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Stop Payment" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="Label7" Text="Payroll Batch List" runat="server" />
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="PayrollSelectionPanel" runat="server" UpdateMode="Conditional" >
        <Triggers>
        </Triggers>

        <ContentTemplate >
            <uc1:Payroll_PeriodSelectionList id="Payroll_PeriodSelectionList1" runat="server" PayrollBatchStatusSelectionOption="ConfirmOnly" ShowPayrollGroupDropDownList="true" SkipProcessEndPeriod="true" OnPayrollBatchChecked="Payroll_PeriodSelectionList1_PayrollBatchChecked" PayrollBatchCheckBoxDefaultCheckedOption="ExcludeBankFileGenerated" />
        </ContentTemplate> 
        </asp:UpdatePanel> 
        <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional"  >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Payroll_PeriodSelectionList1" EventName="PayrollBatchChecked"/>
        </Triggers>
        <ContentTemplate>

        <asp:Panel ID="panelUndoTrialRunDetail" runat="server">
                <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="Employee List" />
                        </td>
                    </tr>
                </table>
                <table class="pm_section" width="100%">
                    <tr>
                        <td align="right">
                            <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openAlert(); return false;"/>
                        </td>
                    </tr>
                </table>
                <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
                    <col width="26px" />
                    <col width="75px" />
                    <colgroup width="200px" >
                        <col width="75px" />
                        <col />
                    </colgroup> 
                    <col width="75px" />
                    <col width="250px" />
                    <col width="100px" />
                    <tr>
                        <td class="pm_list_header" align="center">
                            <asp:Panel ID="UndoPayrollPanel" runat="server">
                                <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
                            </asp:Panel>
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
                            <asp:LinkButton runat="server" ID="_EmpPayIsRP" OnClick="ChangeOrder_Click" Text="Type" />
                        </td>
                        <td align="left" class="pm_list_header">
                            <asp:LinkButton runat="server" ID="_EmpPayConfirmDate" OnClick="ChangeOrder_Click" Text="Confirmed Date" />
                        </td>
                        <td align="left" class="pm_list_header">
                            <asp:LinkButton runat="server" ID="_EmpPayConfirmBy" OnClick="ChangeOrder_Click" Text="Confirmed By" />
                        </td>
                    </tr>
                    <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td class="pm_list" align="center">
                                    <asp:CheckBox ID="ItemSelect" runat="server" />
                                </td>
                                <td class="pm_list">
                                    <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Payroll_TrialRunAdjust_View.aspx?EmpPayrollID=" + sbinding.getValue(Container.DataItem,"EmpPayrollID"))%>">
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
                                    <asp:Label ID="Type" runat="server" />
                                </td>
                                <td class="pm_list">
                                    <%#sbinding.getFValue(Container.DataItem, "EmpPayConfirmDate","yyyy-MM-dd HH:mm:ss")%>
                                </td>
                                <td class="pm_list">
                                    <asp:Label ID="EmpPayConfirmBy" runat="server" />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
                <table width="100%" cellspacing="0" cellpadding="0" class="pm_list_pagenav">
                    <tr>
                        <td align="left">
                            <asp:Button ID="btnUndo" runat="server" Text="Submit" CssClass="button" OnClick="btnUndo_Click" />
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