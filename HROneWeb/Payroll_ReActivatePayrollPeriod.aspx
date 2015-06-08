<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Payroll_ReActivatePayrollPeriod.aspx.cs"    Inherits="Payroll_ReActivatePayrollPeriod" MasterPageFile="~/MainMasterPage.master"  %>

<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
<%-- Start 0000004, Miranda, 2014-06-19 --%>
<input type="hidden" id="hiddenFieldDefaultMonthPeriod" runat="server" value="1" />
<%-- End 0000004, Miranda, 2014-06-19 --%>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Re-activate Payroll Cycle" />
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
        </table>
        
            
                
        
    <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional"  >
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="PayGroupID" />
    </Triggers>
    <ContentTemplate>
        <asp:Panel ID="panelPayPeriod" runat="server">
            <input type="hidden" id="CurrentPayPeriodID" runat="server" name="ID" />
        <table width="100%" class="pm_section">
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label1" runat="server" Text="Payroll Period" />:
                </td>
                <td class="pm_field">
                    <%-- Start 0000004, Miranda, 2014-06-19 --%>
                    <uc1:WebDatePicker id="PayPeriodFr" runat="server" OnChanged="PayPeriod_Changed" AutoPostBack="true" />
                    -
                    <uc1:WebDatePicker id="PayPeriodTo" runat="server" OnChanged="PayPeriod_Changed" AutoPostBack="true" />
                    <%-- End 0000004, Miranda, 2014-06-19 --%>
                    <asp:Button ID="Search" runat="server" Text="Search" CssClass="button" OnClick="Search_Click" />
                </td>
            </tr>
        </table>
            <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
                <col width="26px" />
                <col width="150px" />
                <col width="150px" />
                <tr>
                    <td class="pm_list_header" align="center">
                        <input type="checkbox" onclick="checkAll('<%=payPeriodRepeater.ClientID %>','ItemSelect',this.checked);" />
                    </td>
                    <td align="left" class="pm_list_header" colspan="2">
                        <asp:LinkButton runat="server" ID="_PayPeriodFr" OnClick="ChangeOrder_Click" Text="Period" />
                    </td>
                    <td align="left" class="pm_list_header">
                        <asp:LinkButton runat="server" ID="_PayPeriodConfirmDate" OnClick="ChangeOrder_Click"  Text="Confirmed Date" />
                    </td>
                    <td align="left" class="pm_list_header">
                        <asp:LinkButton runat="server" ID="_PayPeriodConfirmBy" OnClick="ChangeOrder_Click"  Text="Confirmed By" />
                    </td>
                </tr>
                <asp:Repeater ID="payPeriodRepeater" runat="server" OnItemDataBound="payPeriodRepeater_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td class="pm_list" align="center">
                                <asp:CheckBox ID="ItemSelect" runat="server" Checked="false" />
                            </td>
                            <td class="pm_list">
                                <%#payPeriodSBinding.getFValue(Container.DataItem, "PayPeriodFr","yyyy-MM-dd")%>
                            </td>
                            <td class="pm_list">
                                <%#payPeriodSBinding.getFValue(Container.DataItem, "PayPeriodTo", "yyyy-MM-dd")%>
                            </td>
                            <td class="pm_list">
                                <%#payPeriodSBinding.getFValue(Container.DataItem, "PayPeriodConfirmDate", "yyyy-MM-dd HH:mm:ss")%>
                            </td>
                            <td class="pm_list">
                                <%#payPeriodSBinding.getValue(Container.DataItem, "PayPeriodConfirmBy")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <table width="100%" cellspacing="0" cellpadding="0" class="pm_list_pagenav">
                <tr>
                    <td>
                    </td>
                    <td align="right">
                        <tb:RecordListFooter id="ListFooter" runat="server"
                            ListOrderBy="PayPeriodFr"
                            ListOrder="false" 
                            ShowAllRecords="true" 
                          />
                    </td>
                </tr>
            </table>
            <table class="pm_section" width="100%">
                <tr runat="server" ID="panelRollbackOption">
                    <td class="pm_field">
                        <asp:Label ID="lblPassCode" runat="server" Text="Pass code for Re-activate" />
                        <asp:TextBox ID="txtPassCode" runat="server" Columns="60" MaxLength="30"/> 
                    </td>
                    <td align="left">
                        <asp:Button ID="btnRollback" runat="server" Text="Re-activate" CssClass="button" OnClick="btnRollback_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </ContentTemplate> 
    </asp:UpdatePanel> 
</asp:Content> 