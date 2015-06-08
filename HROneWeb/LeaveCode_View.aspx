<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LeaveCode_View.aspx.cs" Inherits="LeaveCode_View" MasterPageFile="~/MainMasterPage.master"  %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="LeaveCodeID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Leave Code Setup" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="View Leave Code" />:
                    <%=LeaveCodeDesc.Text %>
                </td>
            </tr>
        </table>
        
            
                
        
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar id="toolBar" runat="server"
                     NewButton_Visible="false" 
                     SaveButton_Visible="false" 
                     OnBackButton_Click="Back_Click"
                     OnEditButton_Click ="Edit_Click"
                     OnDeleteButton_Click="Delete_Click"
                      />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
                </td>
            </tr>
        </table>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_field_section">
            <col width="15%" />
            <col width="35%" />
            <col width="15%" />
            <col width="35%" />
            <tr>
                <td class="pm_field_header" >
                    <asp:Label runat="server" Text="Leave Type" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="LeaveTypeID" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label runat="server" Text="Code" />:</td>
                <td class="pm_field" >
                    <asp:Label ID="LeaveCode" runat="Server" /></td>
                <td class="pm_field_header" >
                    <asp:Label runat="server" Text="Description" />:</td>
                <td class="pm_field" >
                    <asp:Label ID="LeaveCodeDesc" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label3" runat="server" Text="Skip Payroll Process" />:
                </td>
                <td class="pm_field" >
                    <asp:Label ID="LeaveCodeIsSkipPayrollProcess" runat="Server" />
                </td>
                <td class="pm_field_header">
                    <%-- Start 0000048, Miranda, 2014-06-03 --%>
                    <asp:Label ID="Label5" runat="server" EnableViewState="false" Text="Show Relevant Cert. Option" />:
                    <%-- End 0000048, Miranda, 2014-06-03 --%>
                </td>
                <td class="pm_field" >
                    <asp:Label ID="LeaveCodeIsShowMedicalCertOption" runat="Server" />
                </td>
            </tr>
            <asp:Panel ID="PayrollProcessPanel" runat="server" >
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Pay Ratio" />:</td>
                <td class="pm_field">
                    <asp:Label ID="LeaveCodePayRatio" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Payment Comparison" />:
                </td>
                <td class="pm_field">
                    <asp:Label ID="LeaveCodePayAdvance" runat="server" />
                </td>            
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label4" runat="server" Text="Payroll Process on Next Month" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="LeaveCodeIsPayrollProcessNextMonth" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Leave Deduction Formula" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="LeaveCodeLeaveDeductFormula" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Payment Code for Leave Deduction" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="LeaveCodeLeaveDeductPaymentCodeID" runat="Server" /><br />
                    <asp:CheckBox ID="LeaveCodeUseAllowancePaymentCodeIfSameAmount" runat="server"  Enabled="false"  /><asp:Label runat="server" Text="Use Payment Code for Leave Allowance as Deduction Code if net payment is $0" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Leave Allowance Formula" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="LeaveCodeLeaveAllowFormula" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Payment Code for Leave Allowance" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="LeaveCodeLeaveAllowPaymentCodeID" runat="Server" /></td>
            </tr>
            <tr id="LeaveCodeIsCNDProrataRow" runat="server" >
                <td class="pm_field_header" >
                    <asp:Label ID="Label32" runat="server" Text="Prorata on Claims and Deductions" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="LeaveCodeIsCNDProrata" runat="Server" />
                </td>
            </tr>
            </asp:Panel>
            <tr runat="server" id="ESSRow">
                <td class="pm_field_header">
                    <asp:Label ID="Label2" runat="server" Text="Hide in ESS" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="LeaveCodeHideInESS" runat="server" />
                </td>
            </tr>
            <tr runat="server" >
                <td class="pm_field_header">
                    <asp:Label ID="Label1" runat="server" Text="Leave Application Unit" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="LeaveAppUnit" runat="server" />
                </td>
            </tr>
            
        </table>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
        </asp:UpdatePanel >
</asp:Content> 