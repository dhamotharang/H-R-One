<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GenerateKeyPair.aspx.cs" Inherits="GenerateKeyPair" MasterPageFile="~/MasterPage.master" %>



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
        <asp:PostBackTrigger ControlID="Save" />
    </Triggers>
    <ContentTemplate >
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Generate Key Pair" />
                </td>
            </tr>
        </table>

            <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1" >
                <col width="25%" />
                <col width="25%" />
                <col width="25%" />
                <col width="25%" />
                <tr >
                    <td align="left" class="pm_field_header">
                        <asp:Label ID="Label5" runat="server" Text="Database Encryption Passpharse" />
                    </td>
                    <td align="left" class="pm_field" colspan="3">
                        <asp:TextBox ID="txtPasspharse" runat="server" Columns="80" />
                    </td>
                </tr>
                <tr >
                    <td align="left" class="pm_field_header">
                        <asp:Label ID="Label17" runat="server" Text="Generate Encrypted Data by" />
                    </td>
                    <td align="left" class="pm_field" colspan="3">
                        <asp:RadioButtonList ID="cbxEncryptedDataBy" runat="server" >
                            <asp:ListItem Text="Build in Key Pair" value="BuildIn" Selected="True"  />
                        </asp:RadioButtonList> 
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