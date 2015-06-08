<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MiscCode.aspx.cs" Inherits="MiscCode" MasterPageFile="~/MainMasterPage.master"  %>
<%@ Register Src="controls/Misccode_sel_LeftMenu.ascx" TagName="Misccode_sel_LeftMenu" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_page_title">
        <tr>
            <td>
                <asp:Label ID="Label1" Text="Miscellaneous Code Setup" runat="Server" /></td>
        </tr>
    </table>
    <table width="100%" cellspacing="0" cellpadding="0">
        <col width="10%" />
        <tr>
            <td valign="top" >
                <uc1:Misccode_sel_LeftMenu ID="Misccode_sel_LeftMenu1" runat="server" />
            </td>
            <td valign="top">
            </td>
        </tr>
    </table>
</asp:Content>