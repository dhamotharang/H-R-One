<%@ Page Language="C#" MasterPageFile="~/MainMasterPage.master" AutoEventWireup="true" CodeFile="Payroll_GroupProcessContinuous_Process.aspx.cs" Inherits="Payroll_GroupProcessContinuous_Process" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <input type="hidden" id="CurPage" runat="server" />
    <input type="hidden" id="NumPage" runat="server" />
    <input type="hidden" id="PayGroupID" runat="server" />
    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Payroll Continuous Process" />
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td class="pm_field" align="center">
                <asp:Label ID="lblProgressMessage" runat="server" Text="Trial Run is in progress ..."></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="pm_field" align="center">
                <asp:Label ID="lblProgress" runat="server" Text=""/>
            </td>
        </tr>
    </table>
</asp:Content> 