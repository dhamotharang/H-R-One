<%@ page language="C#" masterpagefile="~/MainMasterPage.master" autoeventwireup="true" inherits="Payroll_GroupProcessContinuous, HROneWeb.deploy" viewStateEncryptionMode="Always" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" Runat="Server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Continuous Payroll Process" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Payroll Group Information" />
                </td>
            </tr>
        </table>
        <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
            <col width="26px" />
            <col width="200px" />
            <tr>
                <td class="pm_list_header">
                    <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
                </td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_PayGroupCode" OnClick="ChangeOrder_Click" Text="Payroll Group"/>
                </td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_PayGroupDesc" OnClick="ChangeOrder_Click" Text="Description"/>
                </td>
            </tr>
            <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_list" align="center">
                            <asp:CheckBox ID="ItemSelect" runat="server" />
                        </td>
                        <td class="pm_list">
                            <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Payroll_Group_View.aspx?PayGroupID=" + binding.getValue(Container.DataItem,"PayGroupID"))%>">
                                <%#binding.getValue(Container.DataItem, "PayGroupCode")%>
                            </a>
                        </td>
                        <td class="pm_list">
                            <%#binding.getValue(Container.DataItem, "PayGroupDesc")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>        
        <tb:RecordListFooter id="ListFooter" runat="server" ShowAllRecords="true" />

    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
        <tr>
            <td>
                <asp:Label ID="Label3" runat="server" Text="Options" />
            </td>
        </tr>
    </table>
                
    <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional"  >
    <Triggers>
    </Triggers>
    <ContentTemplate>
        <table class="pm_section" width="100%">
            <tr>
                <td align="left">
                    <asp:Label ID="Label10" runat="server" Text="Period" />:
                </td>
                <td align="left">
                    <uc1:WebDatePicker ID="PayPeriodFr" runat="server" />
                    -
                    <uc1:WebDatePicker ID="PayPeriodTo" runat="server" />
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:CheckBox ID="SkipRecurringPaymentProcess" runat="server" Text="Skip Recurring Payment Process" />
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:CheckBox ID="SkipClaimsAndDeductionsProcess" runat="server" Text="Skip Claims and Deductions Process" />
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:CheckBox ID="SkipYearEndBonusProcess" runat="server" Text="Skip Year End Bonus Process" />
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:CheckBox ID="SkipAdditionalRenumerationProcess" runat="server" Text="Skip Additional Renumeration Process" />
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:CheckBox ID="RollbackPayrollProcess" runat="server" Text="Rollback Payroll" />
                    <asp:Label ID="lblPassCode" runat="server" Text="Pass code for rollback process" />
                    <asp:TextBox ID="txtPassCode" runat="server" Columns="60" MaxLength="30"/>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:Button ID="btnProcessEnd" runat="server" Text="Process" CssClass="button" OnClick="btnProcessEnd_Click" />
                </td>
            </tr>
        </table>
      </ContentTemplate> 
      </asp:UpdatePanel> 
</asp:Content>

