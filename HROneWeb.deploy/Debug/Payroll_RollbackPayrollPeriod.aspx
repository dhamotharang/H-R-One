<%@ page language="C#" autoeventwireup="true" inherits="Payroll_RollbackPayrollPeriod, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>



<%@ Register Src="~/controls/Payroll_PeriodInfo.ascx" TagName="Payroll_PeriodInfo"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Rollback Payroll Process" />
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
                        <asp:AsyncPostBackTrigger ControlID="btnRollback" />
                    </Triggers>
                    <ContentTemplate>
                    <asp:DropDownList ID="PayPeriodID" runat="server" OnSelectedIndexChanged="PayPeriodID_SelectedIndexChanged"
                        AutoPostBack="True" />
                    </ContentTemplate> 
                    </asp:UpdatePanel> 
                </td>
            </tr>
        </table>
        
            
                
        
    <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional"  >
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="PayGroupID" />
        <asp:AsyncPostBackTrigger ControlID="PayPeriodID" />
    </Triggers>
    <ContentTemplate>
        <asp:Panel ID="panelPayPeriod" runat="server">
            <input type="hidden" id="CurrentPayPeriodID" runat="server" name="ID" />
            <uc1:Payroll_PeriodInfo ID="ucPayroll_PeriodInfo" runat="server" />
            <table class="pm_section" width="100%">
                <tr runat="server" id="panelRollbackOption">
                    <td class="pm_field">
                        <asp:Label ID="lblPassCode" runat="server" Text="Pass code for rollback process" />
                        <asp:TextBox ID="txtPassCode" runat="server" Columns="60" MaxLength="30"/> 
                    </td>
                    <td align="left">
                        <asp:Button ID="btnRollback" runat="server" Text="Rollback to previous payroll cycle" CssClass="button" OnClick="btnRollback_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </ContentTemplate> 
    </asp:UpdatePanel> 
</asp:Content> 