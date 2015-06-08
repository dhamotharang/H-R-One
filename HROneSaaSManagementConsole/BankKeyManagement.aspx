<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BankKeyManagement.aspx.cs" Inherits="BankKeyManagement" MasterPageFile="~/MasterPage.master" %>



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
        <asp:AsyncPostBackTrigger ControlID="Renew" EventName="Click" />
    </Triggers>
    <ContentTemplate >
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Bank Key Managment" />
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
                        <asp:Label runat="server" Text="Key ID" />
                    </td>
                    <td align="left" class="pm_field" >
                        <asp:Label ID="lblHSBCKeyID" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="left" class="pm_field_header" >
                        <asp:Label runat="server" Text="Days Expired" />
                    </td>
                    <td align="left" class="pm_field" >
                        <asp:Label ID="lblHSBCKeyDaysExpired" runat="server" />
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                <tr>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="Hang Seng" />
                    </td>
                </tr>
            </table>
            <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1" >
                <col width="25%" />
                <col width="75%" />
                <tr>
                    <td align="left" class="pm_field_header" >
                        <asp:Label runat="server" Text="Key ID" />
                    </td>
                    <td align="left" class="pm_field" >
                        <asp:Label ID="lblHASEKeyID" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="left" class="pm_field_header" >
                        <asp:Label runat="server" Text="Days Expired" />
                    </td>
                    <td align="left" class="pm_field" >
                        <asp:Label ID="lblHASEKeyDaysExpired" runat="server" />
                    </td>
                </tr>
            </table>

        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td align="left" >
                    <asp:Button ID="Renew" Text="Renew" runat="server" OnClick="Renew_Click" cssclass="button"/>
                </td>
                <td align="right">
                </td>
            </tr>
        </table>
    </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content> 