<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SetupBankKey.aspx.cs" Inherits="SetupBankKey" MasterPageFile="~/MasterPage.master" %>



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
                    <asp:Label ID="Label1" runat="server" Text="Setup Bank Key" />
                </td>
            </tr>
        </table>
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                <tr>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text="HSBC" />
                    </td>
                </tr>
            </table>
            <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1" >
                <col width="25%" />
                <col width="75%" />
                <tr>
                    <td align="left" class="pm_field_header" >
                        <asp:Label ID="lblPARAM_CODE_BANKKEY_HSBC_PATH" runat="server" Text="Path" />
                    </td>
                    <td align="left" class="pm_field" >
                        <asp:TextBox ID="txtPARAM_CODE_BANKKEY_HSBC_PATH" runat="server" Columns="70" />
                    </td>
                </tr>
                <tr>
                    <td align="left" class="pm_field_header" >
                        <asp:Label ID="lblPARAM_CODE_BANKKEY_HSBC_PASSWORD" runat="server" Text="Password" />
                    </td>
                    <td align="left" class="pm_field" >
                        <asp:TextBox ID="txtPARAM_CODE_BANKKEY_HSBC_PASSWORD" runat="server" TextMode="password"  />
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Hang Seng" />
                    </td>
                </tr>
            </table>
            <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1" >
                <col width="25%" />
                <col width="75%" />
                <tr>
                    <td align="left" class="pm_field_header" >
                        <asp:Label ID="lblPARAM_CODE_BANKKEY_HASE_PATH" runat="server" Text="Path" />
                    </td>
                    <td align="left" class="pm_field" >
                        <asp:TextBox ID="txtPARAM_CODE_BANKKEY_HASE_PATH" runat="server"  Columns="70"/>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="pm_field_header" >
                        <asp:Label ID="lblPARAM_CODE_BANKKEY_HASE_PASSWORD" runat="server" Text="Password" />
                    </td>
                    <td align="left" class="pm_field" >
                        <asp:TextBox ID="txtPARAM_CODE_BANKKEY_HASE_PASSWORD" runat="server" TextMode="password" />
                    </td>
                </tr>
            </table>

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