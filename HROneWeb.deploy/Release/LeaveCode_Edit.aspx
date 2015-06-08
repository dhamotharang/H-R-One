<%@ page language="C#" autoeventwireup="true" inherits="LeaveCode_Edit, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="LeaveCodeID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" EnableViewState="false" Text="Leave Code Setup" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <%=CurID<=0?HROne.Common.WebUtility.GetLocalizedString("Add"):HROne.Common.WebUtility.GetLocalizedString("Edit") %>
                    <asp:Label runat="server" EnableViewState="false" Text="Leave Code " />:
                    <%=LeaveCodeDesc.Text %>
                </td>
            </tr>
        </table>
        
            
                
        
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar id="toolBar" runat="server"
                     NewButton_Visible="false" 
                     EditButton_Visible="false" 
                     OnBackButton_Click="Back_Click"
                     OnSaveButton_Click ="Save_Click"
                     OnDeleteButton_Click="Delete_Click"
                      />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" EnableViewState="false" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
        <ContentTemplate >
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_field_section">
            <col width="15%" />
            <col width="35%" />
            <col width="15%" />
            <col width="35%" />
            <tr>
                <td class="pm_field_header" >
                    <asp:Label runat="server" EnableViewState="false" Text="Leave Type" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="LeaveTypeID" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label runat="server" EnableViewState="false" Text="Code" />:</td>
                <td class="pm_field" >
                    <asp:TextBox ID="LeaveCode" runat="Server" /></td>
                <td class="pm_field_header" >
                    <asp:Label runat="server" EnableViewState="false" Text="Description" />:</td>
                <td class="pm_field" >
                    <asp:TextBox ID="LeaveCodeDesc" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label3" runat="server" EnableViewState="false" Text="Skip Payroll Process" />:
                </td>
                <td class="pm_field" >
                    <asp:CheckBox ID="LeaveCodeIsSkipPayrollProcess" runat="Server" AutoPostBack="true" />
                </td>
                <td class="pm_field_header">
                    <%-- Start 0000048, Miranda, 2014-06-03 --%>
                    <asp:Label ID="Label5" runat="server" EnableViewState="false" Text="Show Relevant Cert. Option" />:
                    <%-- End 0000048, Miranda, 2014-06-03 --%>
                </td>
                <td class="pm_field" >
                    <asp:CheckBox ID="LeaveCodeIsShowMedicalCertOption" runat="Server" />
                </td>
            </tr>
            <asp:Panel ID="PayrollProcessPanel" runat="server" >
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" EnableViewState="false" Text="Pay Ratio" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="LeaveCodePayRatio" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label runat="server" EnableViewState="false" Text="Payment Comparison" />:
                </td>
                <td class="pm_field">
                    <asp:CheckBox ID="LeaveCodePayAdvance" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label4" runat="server" EnableViewState="false" Text="Payroll Process on Next Month" />:
                </td>
                <td class="pm_field" colspan="3" >
                    <asp:CheckBox ID="LeaveCodeIsPayrollProcessNextMonth" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" EnableViewState="false" Text="Leave Deduction Formula" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="LeaveCodeLeaveDeductFormula" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" ID="lblLeaveDeductPaymentCode" EnableViewState="false" Text="Payment Code for Leave Deduction" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="LeaveCodeLeaveDeductPaymentCodeID" runat="Server" CssClass="pm_required"/><br />
                    <asp:CheckBox ID="LeaveCodeUseAllowancePaymentCodeIfSameAmount" EnableViewState="false" Text="Use Payment Code for Leave Allowance as Deduction Code if net payment is $0" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" EnableViewState="false" Text="Leave Allowance Formula" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="LeaveCodeLeaveAllowFormula" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" ID="lblLeaveAllowPaymentCode" EnableViewState="false" Text="Payment Code for Leave Allowance" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="LeaveCodeLeaveAllowPaymentCodeID" runat="Server" CssClass="pm_required"/>
                </td>
            </tr>
            <tr id="LeaveCodeIsCNDProrataRow" runat="server" >
                <td class="pm_field_header" >
                    <asp:Label ID="Label32" runat="server" EnableViewState="false" Text="Prorata on Claims and Deductions" />:
                </td>
                <td class="pm_field" >
                    <asp:CheckBox ID="LeaveCodeIsCNDProrata" runat="Server" />
                </td>
            </tr>
            </asp:Panel>
            <tr runat="server" id="ESSRow">
                <td class="pm_field_header">
                    <asp:Label ID="Label2" runat="server" EnableViewState="false" Text="Hide in ESS" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:CheckBox ID="LeaveCodeHideInESS" runat="server" />
                </td>
            </tr>
            <tr runat="server">
                <td class="pm_field_header">
                    <asp:Label ID="Label22" runat="server" EnableViewState="false" Text="Leave Application Unit" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:TextBox ID="LeaveAppUnit" runat="server" />
                </td>
            </tr>
        </table>
        </ContentTemplate>
        </asp:UpdatePanel >
</asp:Content> 