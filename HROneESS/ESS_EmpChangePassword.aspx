<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ESS_EmpChangePassword.aspx.cs"   Inherits="ESS_ChangePassword" MasterPageFile="~/MainMasterPage.master"  %>

<asp:Content ID="bannerContent" runat="server" ContentPlaceHolderID="bannerContentPlaceHolder" > 
    <table width="898" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <a href="ESS_Home.aspx" >
                    <img src="images/banner15_01.jpg" alt="" style="border-width: 0px; display : block" />
                </a>
            </td>
            <td valign="bottom" >
                <img src="images/banner15_02.jpg" alt="" style="border-width: 0px; display : block" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <img src="images/banner15_03.jpg" alt="" style="border-width: 0px; display : block"/>
            </td>
        </tr>
    </table>
</asp:Content>
 <asp:Content ID="mainContent" runat="server" ContentPlaceHolderID="mainContentPlaceHolder" > 
     <input type="hidden" id="EmpID" runat="server" name="ID" />
    <table width="587px" border="0" cellpadding="5" cellspacing="0"  class="pm_field_section">
        <col width="200px" />
        <col width="287px" />
        <tr >
            <td colspan="2" class="pm_field_title">
                <table width="575px" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td align="left"> 
                            <asp:Label ID="Label3" Text="Change Password" runat="server" /> 
                        </td>
                        <td align="right">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
                            <ContentTemplate >
                            <asp:Button ID="Save" runat="server" Text="Save" CssClass="button" OnClick="Save_Click" />
                            </ContentTemplate>
                            </asp:UpdatePanel> 
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr  >
            <td class="pm_field_header">
                <asp:Label ID="Label4" Text="Old Password" runat="server" />:
            </td>
            <td class="pm_field">
                <asp:TextBox TextMode="Password" ID="Password" runat="server" />
            </td>
        </tr>
        <tr >
            <td class="pm_field_header">
                <asp:Label ID="Label6" Text="New Password " runat="server" />:
            </td>
            <td class="pm_field">
                <asp:TextBox TextMode="Password" ID="NewPassword" runat="server" />
            </td>
        </tr>
        <tr >
            <td class="pm_field_header">
                <asp:Label ID="Label8" Text="Confirm New Password" runat="server" />:
            </td>
            <td class="pm_field">
                <asp:TextBox TextMode="Password" ID="ReTypePassword" runat="server" />
            </td>
        </tr>
    </table>
    
    <br />
    <table width="587px" border="0" cellpadding="5" cellspacing="0"  class="pm_field_section">
        <col width="200px" />
        <col width="287px" />
        <tr >
            <td colspan="2" class="pm_field_title">
                <table width="575px" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td align="left"> 
                            <asp:Label ID="Label1" Text="Change System Language" runat="server" /> 
                        </td>
                        
                        <td align="right">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" >
                            <ContentTemplate >
                            <asp:Button ID="SaveLanguage" runat="server" Text="Save" CssClass="button" OnClick="SaveLanguage_Click" />
                            </ContentTemplate>
                            </asp:UpdatePanel> 
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr  >
            <td class="pm_field_header">        
                <asp:Label ID="Label9" runat="server" EnableViewState="false" Text="PARAM_CODE_DEFAULT_LANGUAGE" />
            </td>
            <td align="left" class="pm_field" >
                <asp:DropDownList  ID="cbxPARAM_CODE_DEFAULT_LANGUAGE" runat="server" >
                </asp:DropDownList>
            </td>        
        </tr>
    </table>
</asp:Content> 