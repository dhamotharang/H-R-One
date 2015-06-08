<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Emp_Permit_View.aspx.cs"    Inherits="Emp_Permit_View" MasterPageFile="~/MainMasterPage.master"  %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/Emp_Common.ascx" TagName="Emp_Common" TagPrefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="EmpPermitID" runat="server" name="ID" />
        <input type="hidden" id="EmpID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label Text="Employee Information" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="View Work Permit/License" />
                </td>
            </tr>
        </table>
        <uc1:Emp_Common ID="Emp_Common1" runat="server"></uc1:Emp_Common>
        <br />
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar id="toolBar" runat="server"
                     NewButton_Visible="false" 
                     SaveButton_Visible="false" 
                     OnBackButton_Click="Back_Click"
                     OnEditButton_Click ="Edit_Click"
                     OnDeleteButton_Click="Delete_Click"
                      />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openAlert(); return false;"/>
                </td>
            </tr>
        </table>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_field_section">
            <col width="15%" />
            <col width="35%" />
            <col width="15%" />
            <col width="35%" />
            <tr>
                <td class="pm_field_header" >
                    <asp:Label runat="server" Text="Type" />:</td>
                <td class="pm_field" >
                    <asp:Label ID="PermitTypeID" runat="server" />
                </td>
                <td class="pm_field_header" >
                    <asp:Label runat="server" Text="Permit No." />:</td>
                <td class="pm_field" >
                    <asp:Label ID="EmpPermitNo" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Issue Date" />:</td>
                <td class="pm_field">
                    <asp:Label ID="EmpPermitIssueDate" runat="Server" />
                </td>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Expiry" />:</td>
                <td class="pm_field">
                    <asp:Label ID="EmpPermitExpiryDate" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Remark" />:</td>
                <td class="pm_field">
                    <asp:Label ID="EmpPermitRemark" runat="Server" /></td>
            </tr>
        </table>
</asp:Content> 