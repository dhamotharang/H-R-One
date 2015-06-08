<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Payroll_ESS_ReleasePaySlip_List.aspx.cs"    Inherits="Payroll_ESS_ReleasePaySlip_List" MasterPageFile="~/MainMasterPage.master"   %>

<%@ Register Src="~/controls/Payroll_PeriodSelectionList.ascx" TagName="Payroll_PeriodSelectionList" TagPrefix="uc1" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Release Pay Slip to ESS" />
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
            <uc1:Payroll_PeriodSelectionList id="Payroll_PeriodSelectionList1" runat="server" PayrollBatchStatusSelectionOption="ConfirmOnly" ShowPayrollGroupDropDownList="true" PayrollBatchCheckBoxDefaultCheckedOption="ReleasePaySlipToESS" />
        </ContentTemplate> 
        </asp:UpdatePanel> 
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="button" OnClick="btnSubmit_Click" />

</asp:Content> 