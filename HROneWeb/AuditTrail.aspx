<%@ Page Language="C#" MasterPageFile="~/MainMasterPage.master" AutoEventWireup="true" CodeFile="AuditTrail.aspx.cs" Inherits="AuditTrail" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" Runat="Server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="lblReportHeader" runat="server" Text="Audit Trail" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="Label4" runat="server" Text="Criteria" />
                </td>
            </tr>
        </table>
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openAlert(); return false;"/>
                </td>
            </tr>
        </table>
    <table width="100%" class="pm_section">
        <col width="15%" />
        <col width="45%" />
        <col width="10%" />
        <col width="30%" />
        <tr>
            <td class="pm_search_header" valign="top" >
                <asp:Label ID="Label2" runat="server" Text="Function" />:
            </td>
            <td class="pm_search">
                (<asp:Label ID="Label5" runat="server" Text="Press &quotCtrl&quot for multiple selection" />)<br />
                <asp:ListBox  runat="server" ID="FunctionID" SelectionMode="Multiple" Rows="10" />
            </td>
            <td class="pm_search_header" valign="top" >
                <asp:Label ID="Label3" runat="server" Text="Users" />:
            </td>
            <td class="pm_search">
                (<asp:Label ID="Label6" runat="server" Text="Press &quotCtrl&quot for multiple selection" />)<br />
                <asp:ListBox  runat="server" ID="UserID" SelectionMode="Multiple" Rows="10"/>
            </td>
        </tr>
        <tr>
            <td class="pm_search_header">
                <asp:Label ID="Label20" runat="server" Text="Emp No. (if any)" />:
            </td>
            <td class="pm_search" colspan="3">
                <asp:TextBox  runat="server" ID="EmpNo" />
            </td>
        </tr>
        <tr>
            <td class="pm_search_header">
                <asp:Label ID="Label9" runat="server" Text="Date" />:
            </td>
            <td class="pm_search" colspan="3">
                <uc1:WebDatePicker id="CreateDateFrom" runat="server" />
                -
                <uc1:WebDatePicker id="CreateDateTo" runat="server" />
            </td> 
        </tr>
        <tr>
            <td class="pm_search_header">
            </td>
            <td class="pm_search">
                <asp:CheckBox ID="chkShowHeaderOnly" runat="server" Text="Show Header Only" />
            </td>
        </tr>
        <tr>
            <td class="pm_search_header">
            </td>
            <td class="pm_search">
                <asp:CheckBox ID="chkShowWithoutDataUpdate" runat="server" Text="Show header without data update" />
            </td>
        </tr>
        <tr>
            <td class="pm_search_header">
            </td>
            <td class="pm_search">
                <asp:CheckBox ID="chkShowKeyIDOnly" runat="server" Text="Show Key ID Only" />
            </td>
        </tr>
        <tr>
            <td class="pm_search_header">
            </td>
            <td class="pm_search">
                <asp:CheckBox ID="chkDoNotConvertID" runat="server" Text="Show Internal ID Only (Without Item Description)" />
            </td>
        </tr>
    </table>
    <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
        <tr>
            <td>
                <asp:Button ID="btnGenerate" runat="server" Text="Generate" CssClass="button" OnClick="btnGenerate_Click" />
            </td>
        </tr>
    </table>
</asp:Content>

