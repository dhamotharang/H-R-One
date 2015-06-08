<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Emp_RosterTableGroup_Edit.aspx.cs" Inherits="Emp_RosterTableGroup_Edit"  MasterPageFile="~/MainMasterPage.master"  %>

<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/Emp_Common.ascx" TagName="Emp_Common" TagPrefix="uc1" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="EmpRosterTableGroupID" runat="server" name="ID" />
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
                    <%=CurID<=0?HROne.Common.WebUtility.GetLocalizedString("Add"):HROne.Common.WebUtility.GetLocalizedString("Edit") %>
                    <asp:Label Text="Roster Table Group" runat="server" />
                </td>
            </tr>
        </table>
        
             
        
        <uc1:Emp_Common ID="Emp_Common1" runat="server"></uc1:Emp_Common>
        <br />
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar ID="toolBar" runat="server" NewButton_Visible="false" EditButton_Visible="false" OnBackButton_Click="Back_Click" OnSaveButton_Click="Save_Click" OnDeleteButton_Click="Delete_Click" />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
                </td>
            </tr>
        </table>
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="20%" />
            <col width="30%" />
            <col width="20%" />
            <col width="30%" />
            <tr>
                <td class="pm_field_header" >
                    <asp:Label Text="From" runat="server" />:</td>
                <td class="pm_field" >
                    <uc1:WebDatePicker id="EmpRosterTableGroupEffFr" runat="server" />
                </td>
                <td class="pm_field_header" >
                    <asp:Label Text="To" runat="server" />:</td>
                <td class="pm_field" >
                    <uc1:WebDatePicker id="EmpRosterTableGroupEffTo" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Roster Table Group" runat="server" />:
                </td>
                <td class="pm_field">
                    <asp:DropDownList ID="RosterTableGroupID" runat="Server" />
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label1" Text="Supervisor" runat="server" />?
                </td>
                <td class="pm_field">
                    <asp:CheckBox ID="EmpRosterTableGroupIsSupervisor" runat="Server" />
                </td>
            </tr>

        </table>
</asp:Content> 