<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SystemParameter.aspx.cs" Inherits="SystemParameter" MasterPageFile="~/MasterPage.master" %>



<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        
            
                
        
<%--        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" UseSubmitBehavior="false" OnClientClick="openAlert(); return false;" />
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
                    <asp:Label ID="Label1" runat="server" Text="System Parameters Setup" />
                </td>
            </tr>
        </table>
        <asp:Panel ID="PanelSystemInformation" runat="server">
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                <tr>
                    <td>
                        <asp:Label ID="Label8" runat="server" Text="System Information" />
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
                        <asp:Label ID="lblSYSTEM_DB_VERSION" runat="server" Text="SYSTEM_DB_VERSION" />
                    </td>
                    <td align="left" class="pm_field" >
                        <asp:Label ID="SYSTEM_DB_VERSION" runat="server" />
                    </td>
                    <td align="left" class="pm_field_header" >
                        <asp:Label ID="lblRUNNING_DB_VERSION" runat="server" Text="RUNNING_DB_VERSION" />
                    </td>
                    <td align="left" class="pm_field" >
                        <asp:Label ID="RUNNING_DB_VERSION" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="left" class="pm_field_header">
                        <asp:Label ID="Label15" runat="server" Text="PARAM_CODE_DEFAULT_RECORDS_PER_PAGE" />
                    </td>
                    <td align="left" class="pm_field" colspan="3">
                        <asp:DropDownList  ID="cbxPARAM_CODE_DEFAULT_RECORDS_PER_PAGE" runat="server" >
                            <asp:ListItem Value="20" Text="20" selected="True" />
                            <asp:ListItem Value="50" Text="50" />
                            <asp:ListItem Value="100" Text="100" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr >
                    <td align="left" class="pm_field_header">
                        <asp:Label ID="Label5" runat="server" Text="Default Document Path (for inbox attachment)" />
                    </td>
                    <td align="left" class="pm_field" colspan="3">
                        <asp:TextBox ID="txtPARAM_CODE_DEFAULT_DOCUMENT_FOLDER" runat="server" Columns="80" />
                    </td>
                </tr>
                <tr >
                    <td align="left" class="pm_field_header">
                        <asp:Label ID="Label17" runat="server" Text="Folder for upload bank file" />
                    </td>
                    <td align="left" class="pm_field" colspan="3">
                        <asp:TextBox ID="txtPARAM_CODE_BANKFILE_UPLOAD_FOLDER" runat="server" Columns="80" />
                    </td>
                </tr>
                <tr >
                    <td align="left" class="pm_field_header">
                        <asp:Label ID="Label9" runat="server" Text="Folder for HSBC MRI Encryption Program" />
                    </td>
                    <td align="left" class="pm_field" colspan="3">
                        <asp:TextBox ID="txtPARAM_CODE_HSBC_MRI_DIRECTORY" runat="server" Columns="80" />
                    </td>
                </tr>
                <tr >
                    <td align="left" class="pm_field_header">
                        <asp:Label ID="Label2" runat="server" Text="Autopay file cut off time for Consolidation " />
                    </td>
                    <td align="left" class="pm_field" colspan="3">
                        <asp:TextBox ID="txtPARAM_CODE_BANKFILE_CUTOFF_TIME" runat="server" Columns="80" />
                    </td>
                </tr>
                <tr >
                    <td align="left" class="pm_field_header">
                        <asp:Label ID="Label4" runat="server" Text="Autopay file last cancel time" />
                    </td>
                    <td align="left" class="pm_field" colspan="3">
                        <asp:TextBox ID="txtPARAM_CODE_BANKFILE_LAST_CANCEL_TIME" runat="server" Columns="80" />
                    </td>
                </tr>
                <tr >
                    <td align="left" class="pm_field_header">
                        <asp:Label ID="Label10" runat="server" Text="Default Max. Inbox Size (MB)" />
                    </td>
                    <td align="left" class="pm_field" colspan="3">
                        <asp:TextBox ID="txtPARAM_CODE_DEFAULT_MAX_INBOX_SIZE_MB" runat="server" Columns="3" MaxLength="3" style="text-align:right;" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="PanelSecuritySection" runat="server">
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                <tr>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text="Security" />
                    </td>
                </tr>
            </table>
            <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1" >
                <col width="25%" />
                <col width="75%" />
                <tr>
                    <td align="left" class="pm_field_header" >
                        <asp:Label ID="lblPARAM_CODE_LOGIN_MAX_FAIL_COUNT" runat="server" Text="PARAM_CODE_LOGIN_MAX_FAIL_COUNT" />
                    </td>
                    <td align="left" class="pm_field" >
                        <asp:TextBox ID="txtPARAM_CODE_LOGIN_MAX_FAIL_COUNT" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="left" class="pm_field_header" >
                        <asp:Label ID="Label20" runat="server" Text="PARAM_CODE_SESSION_TIMEOUT" />
                    </td>
                    <td align="left" class="pm_field">
                        <asp:TextBox ID="txtPARAM_CODE_SESSION_TIMEOUT" runat="server" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="PanelSMTPSection" runat="server">
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                <tr>
                    <td>
                        <asp:Label ID="Label6" runat="server" Text="SMTP Server" />
                    </td>
                </tr>
            </table>
            <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
                <col width="25%" />
                <col width="75%" />
                <tr>
                    <td align="left" class="pm_field_header" >
                        <asp:Label ID="lblPARAM_CODE_SMTP_SERVER_NAME" runat="server" Text="PARAM_CODE_SMTP_SERVER_NAME" />
                    </td>
                    <td align="left" class="pm_field">
                        <asp:TextBox ID="txtPARAM_CODE_SMTP_SERVER_NAME" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="left" class="pm_field_header">
                        <asp:Label ID="lblPARAM_CODE_SMTP_PORT" runat="server" Text="PARAM_CODE_SMTP_PORT" />
                    </td>
                    <td align="left" class="pm_field">
                        <asp:TextBox ID="txtPARAM_CODE_SMTP_PORT" runat="server" />
                        <asp:CheckBox ID="chkPARAM_CODE_SMTP_ENABLE_SSL" runat="server" Text="Enable SSL" />
                    </td>
                </tr>
                <tr>
                    <td align="left" class="pm_field_header">
                        <asp:Label ID="lblPARAM_CODE_SMTP_USERNAME" runat="server" Text="Username" />
                    </td>
                    <td align="left" class="pm_field">
                        <asp:TextBox ID="txtPARAM_CODE_SMTP_USERNAME" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="left" class="pm_field_header">
                        <asp:Label ID="Label7" runat="server" Text="Password" />
                    </td>
                    <td align="left" class="pm_field">
                        <asp:TextBox ID="txtPARAM_CODE_SMTP_PASSWORD" runat="server" TextMode="Password" />
                    </td>
                </tr>            
                <tr>
                    <td align="left" class="pm_field_header">
                        <asp:Label ID="Label11" runat="server" Text="Outgoing Email Address" />
                    </td>
                    <td align="left" class="pm_field">
                        <asp:TextBox ID="txtPARAM_CODE_SMTP_SMTP_OUTGOING_EMAIL_ADDRESS" runat="server" Columns="50" />
                    </td>
                </tr>
                <tr>
                    <td align="left" class="pm_field_header">
                        <asp:Label ID="lblTestSMTPServer" runat='server' Text="Test Email Function" />
                    </td>
                    <td align="left" class="pm_field">
                        <asp:Label ID="lblTestEmailAddress" runat='server' Text="Email Address" />
                        <asp:TextBox ID="txtTestEmailAddress" runat="server" Columns="50" />
                        <asp:Button ID="btnTestEmail" runat="server" CssClass="button" Text="Send" OnClick="btnTestEmail_Click" UseSubmitBehavior="false" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td align="left" >
                    <asp:Button ID="Save" Text="Save" runat="server" OnClick="Save_Click" cssclass="button"/>
                </td>
                <td align="right">
                </td>
            </tr>
        </table>
    </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content> 