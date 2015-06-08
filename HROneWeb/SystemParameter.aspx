<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SystemParameter.aspx.cs" Inherits="SystemParameter" MasterPageFile="~/MainMasterPage.master" %>

<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/controls/SystemParameter_Customization.ascx" TagName="SystemParameter_Customization" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        
            
                
        
<%--        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" EnableViewState="false" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openAlert(); return false;" />
                </td>
            </tr>
        </table>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="Save" EventName="Click" />
    </Triggers>
    <ContentTemplate >
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" EnableViewState="false" Text="System Parameters Setup" />
                </td>
            </tr>
        </table>
        <asp:Panel ID="PanelSystemInformation" runat="server">
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                <tr>
                    <td>
                        <asp:Label ID="Label8" runat="server" EnableViewState="false" Text="System Information" />
                    </td>
                </tr>
            </table>
            <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1" >
                <col width="25%" />
                <col width="25%" />
                <col width="25%" />
                <col width="25%" />
                <tr>
                    <td align="left" class="pm_field_header" >
                        <asp:Label ID="lblSYSTEM_APP_VERSION" runat="server" EnableViewState="false" Text="SYSTEM_APP_VERSION" />
                    </td>
                    <td align="left" class="pm_field" colspan="3">
                        <asp:Label ID="SYSTEM_APP_VERSION" runat="server"/>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="pm_field_header" >
                        <asp:Label ID="lblSYSTEM_DB_VERSION" runat="server" EnableViewState="false" Text="SYSTEM_DB_VERSION" />
                    </td>
                    <td align="left" class="pm_field" >
                        <asp:Label ID="SYSTEM_DB_VERSION" runat="server" />
                    </td>
                    <td align="left" class="pm_field_header" >
                        <asp:Label ID="lblRUNNING_DB_VERSION" runat="server" EnableViewState="false" Text="RUNNING_DB_VERSION" />
                    </td>
                    <td align="left" class="pm_field" >
                        <asp:Label ID="RUNNING_DB_VERSION" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="left" class="pm_field_header" >
                        <asp:Label ID="lblPRODUCT_TYPE" runat="server" EnableViewState="false" Text="PRODUCT_TYPE" />
                    </td>
                    <td align="left" class="pm_field" colspan="3">
                        <asp:Label ID="PRODUCT_TYPE" runat="server"/>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="pm_field_header">
                        <asp:Label ID="Label21" runat="server" EnableViewState="false" Text="Active Company" />
                    </td>
                    <td align="left" class="pm_field">
                        <asp:Label ID="lblActiveCompany" runat="server" /> 
                        /
                        <asp:Label ID="MAX_COMPANY" runat="server" />
                    </td>
                    <td align="left" class="pm_field_header">
                        <asp:Label ID="Label22" runat="server" EnableViewState="false" Text="Active Users" />
                    </td>
                    <td align="left" class="pm_field">
                        <asp:Label ID="lblActiveUser" runat="server" />
                        /
                        <asp:Label ID="MAX_USERS" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="left" class="pm_field_header">
                        <asp:Label ID="Label27" runat="server" EnableViewState="false" Text="Active Employees" />
                    </td>
                    <td align="left" class="pm_field">
                        <asp:Label ID="lblActiveEmployee" runat="server" />
                        /
                        <asp:Label ID="MAX_EMPLOYEES" runat="server" />
                    </td>
                    <td align="left" class="pm_field_header">
                        <asp:Label ID="Label26" runat="server" EnableViewState="false" Text="Inbox size used" />&nbsp(MB)
                    </td>
                    <td align="left" class="pm_field">
                        <asp:Label ID="lblTotalInboxSize" runat="server" />
                        /
                        <asp:Label ID="MAX_INBOX_SIZE_MB" runat="server" />
                    </td>
                </tr>
                <tr runat="server" id="TrialPeriodPanel">
                    <td align="left" class="pm_field_header">
                        <asp:Label ID="lblTRIAL_PERIOD_TO" runat="server" EnableViewState="false" Text="TRIAL_PERIOD_TO" />
                    </td>
                    <td align="left" class="pm_field" colspan="3">
                        <asp:Label ID="TRIAL_PERIOD_TO" runat="server" />
                        <asp:Button ID="ProductKey" runat="server" EnableViewState="false" Text="ENTER_AUTHORIZATION_CODE" CssClass="button" OnClick="ProductKey_Click" UseSubmitBehavior="false" />
                    </td>
                </tr>
                <tr>
                    <td align="left" class="pm_field_header">
                        <asp:Label ID="Label9" runat="server" EnableViewState="false" Text="PARAM_CODE_DEFAULT_LANGUAGE" />
                    </td>
                    <td align="left" class="pm_field" colspan="3">
                        <asp:DropDownList  ID="cbxPARAM_CODE_DEFAULT_LANGUAGE" runat="server" >
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="pm_field_header">
                        <asp:Label ID="Label15" runat="server" EnableViewState="false" Text="PARAM_CODE_DEFAULT_RECORDS_PER_PAGE" />
                    </td>
                    <td align="left" class="pm_field" colspan="3">
                        <asp:DropDownList  ID="cbxPARAM_CODE_DEFAULT_RECORDS_PER_PAGE" runat="server" >
                            <asp:ListItem Value="20" Text="20" selected="True" />
                            <asp:ListItem Value="50" Text="50" />
                            <asp:ListItem Value="100" Text="100" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr runat="server" id="DBTitleRow" >
                    <td align="left" class="pm_field_header">
                        <asp:Label ID="Label18" runat="server" EnableViewState="false" Text="PARAM_CODE_DB_TITLE" />
                    </td>
                    <td align="left" class="pm_field" colspan="3">
                        <asp:TextBox ID="txtPARAM_CODE_DB_TITLE" runat="server" Columns="80" />
                    </td>
                </tr>
                <tr id="DocumentUploadPathSettingRow" runat="server" >
                    <td align="left" class="pm_field_header">
                        <asp:Label ID="Label17" runat="server" EnableViewState="false" Text="PARAM_CODE_DOCUMENT_UPLOAD_FOLDER" />
                    </td>
                    <td align="left" class="pm_field" colspan="3">
                        <asp:TextBox ID="txtPARAM_CODE_DOCUMENT_UPLOAD_FOLDER" runat="server" Columns="80" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="PanelSecuritySection" runat="server">
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                <tr>
                    <td>
                        <asp:Label ID="Label3" runat="server" EnableViewState="false" Text="Security" />
                    </td>
                </tr>
            </table>
            <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1" >
                <col width="25%" />
                <col width="75%" />
                <tr>
                    <td align="left" class="pm_field_header" >
                        <asp:Label ID="lblPARAM_CODE_LOGIN_MAX_FAIL_COUNT" runat="server" EnableViewState="false" Text="PARAM_CODE_LOGIN_MAX_FAIL_COUNT" />
                    </td>
                    <td align="left" class="pm_field" >
                        <asp:TextBox ID="txtPARAM_CODE_LOGIN_MAX_FAIL_COUNT" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="left" class="pm_field_header" >
                        <asp:Label ID="lblPARAM_CODE_SESSION_TIMEOUT" runat="server" EnableViewState="false" Text="PARAM_CODE_SESSION_TIMEOUT" />
                    </td>
                    <td align="left" class="pm_field">
                        <asp:TextBox ID="txtPARAM_CODE_SESSION_TIMEOUT" runat="server" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="PersonalInfoSection" runat="server" >
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                <tr>
                    <td>
                        <asp:Label ID="Label20" runat="server" EnableViewState="false" Text="Personal Information" />
                    </td>
                </tr>
            </table>
            <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
                <col width="25%" />
                <col width="75%" />
                <tr>
                    <td align="left" class="pm_field_header" >
                        <asp:Label ID="Label25" runat="server" EnableViewState="false" Text="PARAM_CODE_HKID_CHECKDIGIT_AUTO_GENERATE" />
                    </td>
                    <td align="left" class="pm_field">
                        <asp:CheckBox ID="chkPARAM_CODE_HKID_CHECKDIGIT_AUTO_GENERATE" runat="server" />
                    </td>
                </tr>
				<!-- Start 0000044, Miranda, 2014-05-09 -->
                <tr>
                    <td align="left" class="pm_field_header" >
                        <asp:Label ID="Label30" runat="server" EnableViewState="false" Text="Auto generate Staff Number" />
                    </td>
                    <td align="left" class="pm_field">
                        <asp:CheckBox ID="chkPARAM_CODE_EMP_NO_AUTO_GENERATE" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="left" class="pm_field_header" >
                        <asp:Label ID="lblPARAM_CODE_EMP_NO_FORMAT" runat="server" EnableViewState="false" Text="New Staff Number format" />
                    </td>
                    <td align="left" class="pm_field">
                        <asp:TextBox ID="txtPARAM_CODE_EMP_NO_FORMAT" runat="server" />
                        (<asp:Label ID="Label31" runat="server" EnableViewState="false" Text="{9} - any digit, {Other Characters} - any fixed character. e.g. Format = A-99999 means the staff number starts with 'A-' and with 5 digit, where 'A-00301' is a valid staff number." />)
                    </td>
                </tr>
				<!-- End 0000044, Miranda, 2014-05-09 -->
            </table>
        </asp:Panel>
        <asp:Panel ID="PanelPayrollSection" runat="server">
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                <tr>
                    <td>
                        <asp:Label ID="Label4" runat="server" EnableViewState="false" Text="Payroll" />
                    </td>
                </tr>
            </table>
            <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
                <col width="25%" />
                <col width="75%" />
                <tr id="ORSOSettingRow" runat="server">
                    <td align="left" class="pm_field_header" >
                        <asp:Label ID="lblPARAM_CODE_USE_ORSO" runat="server" EnableViewState="false" Text="PARAM_CODE_USE_ORSO" />
                    </td>
                    <td align="left" class="pm_field">
                        <asp:CheckBox ID="cbxPARAM_CODE_USE_ORSO" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="left" class="pm_field_header" >
                        <asp:Label ID="lblPARAM_CODE_PAYROLL_MAX_MONTHLY_LSPSP_AMOUNT" runat="server" EnableViewState="false" Text="PARAM_CODE_PAYROLL_MAX_MONTHLY_LSPSP_AMOUNT" />
                    </td>
                    <td align="left" class="pm_field">
                        <asp:TextBox ID="txtPARAM_CODE_PAYROLL_MAX_MONTHLY_LSPSP_AMOUNT" runat="server" />
                        (<asp:Label ID="Label14" runat="server" EnableViewState="false" Text="Default Value" /> = 
                        <asp:Label ID="lblDefault_MAX_MONTHLY_LSPSP_AMOUNT" runat="server"  />)
                    </td>
                </tr>
                <tr>
                    <td align="left" class="pm_field_header" >
                        <asp:Label ID="lblPARAM_CODE_PAYROLL_MAX_TOTAL_LSPSP_AMOUNT" runat="server" EnableViewState="false" Text="PARAM_CODE_PAYROLL_MAX_TOTAL_LSPSP_AMOUNT" />
                    
                    </td>
                    <td align="left" class="pm_field">
                        <asp:TextBox ID="txtPARAM_CODE_PAYROLL_MAX_TOTAL_LSPSP_AMOUNT" runat="server" />
                        (<asp:Label ID="Label13" runat="server" EnableViewState="false" Text="Default Value" /> = 
                        <asp:Label ID="lblDefault_MAX_TOTAL_LSPSP_AMOUNT" runat="server"  />)
                    </td>
                </tr>
                <tr>
                    <td align="left" class="pm_field_header" >
                        <asp:Label ID="Label10" runat="server" EnableViewState="false" Text="PARAM_CODE_PAY_SLIP_HIDE_LEAVE_BALANCE" />
                    </td>
                    <td align="left" class="pm_field">
                        <asp:CheckBox ID="cbxPARAM_CODE_PAY_SLIP_HIDE_LEAVE_BALANCE" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="left" class="pm_field_header" >
                        <asp:Label ID="lblPARAM_CODE_PAY_SLIP_HIDE_MINIMUM_WAGE_INFO" runat="server" EnableViewState="false" Text="PARAM_CODE_PAY_SLIP_HIDE_MINIMUM_WAGE_INFO" />
                    </td>
                    <td align="left" class="pm_field">
                        <asp:CheckBox ID="cbxPARAM_CODE_PAY_SLIP_HIDE_MINIMUM_WAGE_INFO" runat="server" />
                    </td>
                </tr>
                <tr id="BOCIEncryptPathSettingRow" runat="server" >
                    <td align="left" class="pm_field_header" >
                        <asp:Label ID="Label16" runat="server" EnableViewState="false" Text="PARAM_CODE_MPF_FILE_BOCI_ENCRYPT_PATH" />
                    </td>
                    <td align="left" class="pm_field">
                        <asp:TextBox ID="txtPARAM_CODE_MPF_FILE_BOCI_ENCRYPT_PATH" runat="server" Columns="100" />
                    </td>
                </tr>

            </table>
        </asp:Panel>
        
        <asp:Panel ID="PanelReportSection" runat="server" Visible="false" >
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                <tr>
                    <td>
                        <asp:Label ID="Label28" runat="server" EnableViewState="false" Text="Report" />
                    </td>
                </tr>
            </table>
            <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
                <col width="25%" />
                <col width="75%" />
                <tr runat="server">
                    <td align="left" class="pm_field_header" >
                        <asp:Label ID="Label29" runat="server" EnableViewState="false" Text="Default Chinese Font" />
                    </td>
                    <td align="left" class="pm_field">
                        <asp:DropDownList ID="ddbPARAM_CODE_REPORT_CHINESE_FONT" runat="server" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="PanelTaxSection" runat="server">
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                <tr>
                    <td>
                        <asp:Label ID="Label7" runat="server" EnableViewState="false" Text="Taxation" />
                    </td>
                </tr>
            </table>
            <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
                <col width="25%" />
                <col width="75%" />
                <tr>
                    <td align="left" class="pm_field_header" >
                        <asp:Label ID="lblPARAM_CODE_TAXATION_USE_CHINESE_NAME" runat="server" EnableViewState="false" Text="PARAM_CODE_TAXATION_USE_CHINESE_NAME" />
                    </td>
                    <td align="left" class="pm_field">
                        <asp:CheckBox ID="chkPARAM_CODE_TAXATION_USE_CHINESE_NAME" runat="server" />
                    </td>
                </tr>

            </table>
        </asp:Panel>

        <asp:Panel ID="PanelSMTPSection" runat="server">
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                <tr>
                    <td>
                        <asp:Label ID="Label5" runat="server" EnableViewState="false" Text="SMTP Server" />
                    </td>
                </tr>
            </table>
            <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
                <col width="25%" />
                <col width="75%" />
                <tr>
                    <td align="left" class="pm_field_header" >
                        <asp:Label ID="lblPARAM_CODE_SMTP_SERVER_NAME" runat="server" EnableViewState="false" Text="PARAM_CODE_SMTP_SERVER_NAME" />
                    </td>
                    <td align="left" class="pm_field">
                        <asp:TextBox ID="txtPARAM_CODE_SMTP_SERVER_NAME" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="left" class="pm_field_header">
                        <asp:Label ID="lblPARAM_CODE_SMTP_PORT" runat="server" EnableViewState="false" Text="PARAM_CODE_SMTP_PORT" />
                    </td>
                    <td align="left" class="pm_field">
                        <asp:TextBox ID="txtPARAM_CODE_SMTP_PORT" runat="server" />
                        <asp:CheckBox ID="chkPARAM_CODE_SMTP_ENABLE_SSL" runat="server" EnableViewState="false" Text="Enable SSL" />
                    </td>
                </tr>
                <tr>
                    <td align="left" class="pm_field_header">
                        <asp:Label ID="lblPARAM_CODE_SMTP_USERNAME" runat="server" EnableViewState="false" Text="Username" />
                    </td>
                    <td align="left" class="pm_field">
                        <asp:TextBox ID="txtPARAM_CODE_SMTP_USERNAME" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="left" class="pm_field_header">
                        <asp:Label ID="Label6" runat="server" EnableViewState="false" Text="Password" />
                    </td>
                    <td align="left" class="pm_field">
                        <asp:TextBox ID="txtPARAM_CODE_SMTP_PASSWORD" runat="server" TextMode="Password" />
                    </td>
                </tr>            
                <tr>
                    <td align="left" class="pm_field_header">
                        <asp:Label ID="Label11" runat="server" EnableViewState="false" Text="Outgoing Email Address" />
                    </td>
                    <td align="left" class="pm_field">
                        <asp:TextBox ID="txtPARAM_CODE_SMTP_SMTP_OUTGOING_EMAIL_ADDRESS" runat="server" Columns="50" />
                    </td>
                </tr>
                <tr>
                    <td align="left" class="pm_field_header">
                        <asp:Label ID="lblTestSMTPServer" runat='server' EnableViewState="false" Text="Test Email Function" />
                    </td>
                    <td align="left" class="pm_field">
                        <asp:Label ID="lblTestEmailAddress" runat='server' EnableViewState="false" Text="Email Address" />
                        <asp:TextBox ID="txtTestEmailAddress" runat="server" Columns="50" />
                        <asp:Button ID="btnTestEmail" runat="server" CssClass="button" EnableViewState="false" Text="Send" OnClick="btnTestEmail_Click" UseSubmitBehavior="false" />
                    </td>
                </tr>
                <tr>
                    <td align="left" class="pm_field_header"></td>
                    <td align="left" class="pm_field">
                        <a href="EmailLog_List.aspx" ><asp:Label ID="Label19" runat="server" EnableViewState="false" Text="Check E-mail Log" /></a>
                    </td>
                </tr>            
            </table>
        </asp:Panel>

        <asp:Panel ID="EmployeeHierarchyListSection" runat="server">
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                <tr>
                    <td>
                        <asp:Label runat="server" EnableViewState="false" Text="Show/Hide Employee Hierarchy Info" />
                    </td>
                </tr>
            </table>
            <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
                <col width="25%" />
                <col width="75%" />
                <tr>
                    <td class="pm_field_header">
                        <asp:Label runat="server" EnableViewState="false" Text="Options" />
                    </td>
                    <td align="left" class="pm_field">
                        <asp:CheckBox ID="chkPARAM_CODE_EMP_LIST_SHOW_COMPANY" runat="server" EnableViewState="false" Text="Show Company code in Employee Search Result List" /><br />
                        <asp:CheckBox ID="chkPARAM_CODE_EMP_LIST_SHOW_H1" runat="server" EnableViewState="false" Text="Show Hierarchy1 code in Employee Search Result List" /><br />
                        <asp:CheckBox ID="chkPARAM_CODE_EMP_LIST_SHOW_H2" runat="server" EnableViewState="false" Text="Show Hierarchy2 code in Employee Search Result List" /><br />
                        <asp:CheckBox ID="chkPARAM_CODE_EMP_LIST_SHOW_H3" runat="server" EnableViewState="false" Text="Show Hierarchy3 code in Employee Search Result List" /><br />
                    </td>
                </tr>
            </table>
        </asp:Panel>


        <asp:Panel ID="PanelESSSection" runat="server">
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" EnableViewState="false" Text="Employee Self Service" />
                    </td>
                </tr>
            </table>
            <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
                <col width="25%" />
                <col width="75%" />
                <tr>
                    <td align="left" class="pm_field_header" >
                        <asp:Label ID="Label12" runat="server" EnableViewState="false" Text="Default Language" />
                    </td>
                    <td align="left" class="pm_field">
                        <asp:DropDownList  ID="cbxPARAM_CODE_ESS_DEFAULT_LANGUAGE" runat="server" >
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="pm_field_header" >
                        <asp:Label ID="Label23" runat="server" EnableViewState="false" Text="Function" />
                    </td>
                    <td align="left" class="pm_field">
                        <asp:CheckBox ID="chkPARAM_CODE_ESS_FUNCTION_CHANGE_EE_INFO" runat="server" EnableViewState="false" Text="Update Employee Information" /><br />
                        <asp:CheckBox ID="chkPARAM_CODE_ESS_FUNCTION_LEAVE_APPLICATION" runat="server" EnableViewState="false" Text="Leave Application" /><br />
                        <asp:CheckBox ID="chkPARAM_CODE_ESS_FUNCTION_CANCEL_LEAVE_APPLICATION" runat="server" EnableViewState="false" Text="Cancel Leave Application" /><br />
                        <asp:CheckBox ID="chkPARAM_CODE_ESS_FUNCTION_LEAVE_HISTORY" runat="server" EnableViewState="false" Text="Leave History Enquiry" /><br />
                        <asp:CheckBox ID="chkPARAM_CODE_ESS_FUNCTION_LEAVE_BALANCE_ENQUIRY" runat="server" EnableViewState="false" Text="Leave Balance Enquiry" /><br />
                        <asp:CheckBox ID="chkPARAM_CODE_ESS_FUNCTION_LEAVE_BALANCE_REPORT" runat="server" EnableViewState="false" Text="Leave Balance Report" /><br />
                        
                        <asp:CheckBox ID="chkPARAM_CODE_ESS_FUNCTION_LEAVE_APPLICATION_LIST" runat="server" EnableViewState="false" Text="Leave Application List" /><br />
                        
                        <asp:CheckBox ID="chkPARAM_CODE_ESS_FUNCTION_ROSTER_TABLE" runat="server" EnableViewState="false" Text="Roster Table" /><br />
                        <asp:CheckBox ID="chkPARAM_CODE_ESS_FUNCTION_PRINT_PAYSLIP" runat="server" EnableViewState="false" Text="Payslip Printing" /><br />
                        <asp:CheckBox ID="chkPARAM_CODE_ESS_FUNCTION_PRINT_TAXREPORT" runat="server" EnableViewState="false" Text="Taxation Report" /><br />
                        
                        <asp:CheckBox ID="chkPARAM_CODE_ESS_FUNCTION_OVERALL_PAYMENT_SUMMARY" runat="server" EnableViewState="false" Text="Overall Payment Summary" /><br />
                        <asp:CheckBox ID="chkPARAM_CODE_ESS_FUNCTION_OT_CLAIMS" runat="server" EnableViewState="false" Text="CL Requisition" /> &nbsp &nbsp <asp:Label id="lblEnabledOTClaims" Text="(Disabled)" enabled="false" runat="server"/><br />
                        <asp:CheckBox ID="chkPARAM_CODE_ESS_FUNCTION_OT_CLAIMS_HISTORY" runat="server" EnableViewState="false" Text="CL Requisition History" />  &nbsp &nbsp <asp:Label id="lblEnabledOTClaimsHistory" Text="(Disabled)" enabled="false" runat="server"/><br />
                        <!-- Start 0000112, Miranda, 2014-12-10 -->
                        <asp:CheckBox ID="chkPARAM_CODE_ESS_FUNCTION_LATE_WAIVE" runat="server" EnableViewState="false" Text="Late Waive" />  &nbsp &nbsp <asp:Label id="lblEnabledLateWaive" Text="(Disabled)" enabled="false" runat="server" /><br />
                        <asp:CheckBox ID="chkPARAM_CODE_ESS_FUNCTION_LATE_WAIVE_HISTORY" runat="server" EnableViewState="false" Text="Late Waive History" />  &nbsp &nbsp <asp:Label id="lblEnabledLateWaiveHistory" Text="(Disabled)" enabled="false" runat="server"/><br />
                        <!-- End 0000112, Miranda, 2014-12-10 -->
                        <asp:CheckBox ID="chkPARAM_CODE_ESS_FUNCTION_MONTHLY_ATTENDANCE_REPORT" runat="server" EnableViewState="false" Text="Monthly Attendance Report" />   &nbsp &nbsp <asp:Label id="lblEnabledMonthlyAttendanceReport" Text="(Disabled)" enabled="false" runat="server"/><br />
                        <asp:CheckBox ID="chkPARAM_CODE_ESS_FUNCTION_ATTENDANCE_TIMEENTRY_LIST" runat="server" EnableViewState="false" Text="Input in/out Time" />  &nbsp &nbsp <asp:Label id="lblEnabledAttendanceTimeEntryList" Text="(Disabled)" enabled="false" runat="server"/><br />
                        <asp:CheckBox ID="chkPARAM_CODE_ESS_FUNCTION_TIMECARD_RECORD" runat="server" EnableViewState="false" Text="Timecard Record" />  &nbsp &nbsp <asp:Label id="lblEnabledTimeCardRecord" Text="(Disabled)" enabled="false" runat="server"/>
                        <!-- End 0000057, KuangWei, 2014-07-07 -->
                        <!-- Start 0000076, Miranda, 2014-08-21 -->
                        <br />
                        <asp:CheckBox ID="chkPARAM_CODE_ESS_FUNCTION_ATTENDANCE_TIMEENTRY_REPORT" runat="server" EnableViewState="false" Text="Attendance Time Entry Report" />
                        <!-- End 0000076, Miranda, 2014-08-21 -->
                    </td>
                </tr>
                <tr>
                    <td align="left" class="pm_field_header" >
                        <asp:Label ID="Label24" runat="server" EnableViewState="false" Text="Message for Relevant Certificate Alert" />:
                    </td>
                    <td align="left" class="pm_field">
                        <asp:TextBox ID="txtPARAM_CODE_ESS_LEAVE_MEDICIAL_CERT_ALERT" runat="server" TextMode="MultiLine" Columns="80" Rows="5" />
                    </td>
                </tr>
                <tr>
                    <td align="left" class="pm_field_header" >
                        <asp:Label ID="lblPARAM_CODE_ESS_LEAVE_HISTORY_START_DATE" runat="server" EnableViewState="false" Text="Start Date for Leave Application History" />:
                    </td>
                    <td align="left" class="pm_field">
                        <uc1:WebDatePicker ID="wdpPARAM_CODE_ESS_LEAVE_HISTORY_START_DATE" runat="server"  />
                    </td>
                </tr>
                <tr>
                    <td align="left" class="pm_field_header" >
                        <asp:Label ID="lblPARAM_CODE_ESS_PAYSLIP_START_DATE" runat="server" EnableViewState="false" Text="Start Date for Pay Slip Printing" />:
                    </td>
                    <td align="left" class="pm_field">
                        <uc1:WebDatePicker ID="wdpPARAM_CODE_ESS_PAYSLIP_START_DATE" runat="server"  />
                    </td>
                </tr>
                <tr >
                    <td align="left" class="pm_field_header" >
                    </td>
                    <td align="left" class="pm_field">
                        <asp:CheckBox ID="chkPARAM_CODE_ESS_PAYSLIP_AUTO_RELEASE" runat="server" EnableViewState="false" Text="Automatically release the Pay Slip to ESS" /><br />
                    </td>
                </tr>
                <!-- Start 0000060, Miranda, 2014-07-13 -->
                <tr id="eot_row" runat="server">
                    <td align="left" class="pm_field_header">
                        <asp:Label ID="lblPARAM_CODE_ESS_DEF_EOT_EXPIRY" runat="server" EnableViewState="false" Text="Default Expiry for CL Requisition" />:
                    </td>
                    <td align="left" class="pm_field">
                        <!--<uc1:WebDatePicker ID="wdpPARAM_CODE_ESS_DEF_EOT_EXPIRY_DATE" runat="server"  />-->
                        <asp:TextBox ID="txtPARAM_CODE_ESS_DEF_EOT_EXPIRY" runat="server" TextMode="SingleLine" MaxLength="3" />
                        <asp:DropDownList  ID="cbxPARAM_CODE_ESS_DEF_EOT_EXPIRY_TYPE" runat="server" />
                    </td>
                </tr>
                <!-- End 0000060, Miranda, 2014-07-13 -->
            </table>
        </asp:Panel>
        <uc1:SystemParameter_Customization ID="SystemParameterCustomizationControl" runat="server"  />
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td align="left" >
                    <asp:Button ID="Save" EnableViewState="false" Text="Save" runat="server" OnClick="Save_Click" cssclass="button"/>
                </td>
                <td align="right">
                </td>
            </tr>
        </table>
    </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content> 