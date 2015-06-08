<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Payroll_TrialRun.aspx.cs"    Inherits="Payroll_TrialRun" MasterPageFile="~/MainMasterPage.master"  %>



<%@ Register Src="~/controls/Payroll_PeriodInfo.ascx" TagName="Payroll_PeriodInfo"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        
            
                
        
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Payroll Trial Run" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Payroll Trial Run Information" />
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
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional"  >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="PayGroupID" />
            <asp:AsyncPostBackTrigger ControlID="PayPeriodID" />
        </Triggers>
        <ContentTemplate>
        <asp:Panel ID="panelPayPeriod" runat="server">
            <input type="hidden" id="CurrentPayPeriodID" runat="server" name="ID" />
            <uc1:Payroll_PeriodInfo ID="ucPayroll_PeriodInfo" runat="server" />
            <asp:Panel ID="panelTrialRunDetail" runat="server">
                <table width="100%" class="pm_section">
                    <tr>
                        <td class="pm_field_title">
                            <asp:Label runat="server" Text="Trial Run Detail" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pm_field">
                            <table width="100%">
                                <tr>
                                    <td class="pm_field" style="border-style: none">
                                        <asp:CheckBox ID="cbxRecurringPayment" runat="server" Text="Recurring Payment" Checked="true" />
                                    </td>
                                    <td class="pm_field" style="border-style: none">
                                        <asp:CheckBox ID="cbxClaimsAndDeduction" runat="server" Text="Claims and Deduction" Checked="true" />
                                    <td class="pm_field" style="border-style: none">
                                        <asp:CheckBox ID="cbxAdditionalRemuneration" runat="server" Text="Additional Remuneration" Checked="false" />
                                    <td class="pm_field" style="border-style: none">
                                        <asp:CheckBox ID="cbxYearEndBonus" runat="server" Text="Year End Bonus" Checked="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="pm_field" style="border-style: none">
                                        <asp:CheckBox ID="cbxExistingEE" runat="server" Text="Existing Employee" Checked="true" />
                                    </td>
                                    <td class="pm_field" style="border-style: none">
                                        <asp:CheckBox ID="cbxNewJoinEE" runat="server" Text="New Join Employee" Checked="true" />
                                    </td>
                                    <td class="pm_field" style="border-style: none">
                                        <asp:CheckBox ID="cbxFinalPayment" runat="server" Text="Terminated Employee" Checked="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Button ID="btnDisplay" runat="server" Text="Display" class="button" OnClick="btnDisplay_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </asp:Panel>
        </ContentTemplate>
        </asp:UpdatePanel>

</asp:Content> 