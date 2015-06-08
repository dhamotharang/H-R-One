<%@ page language="C#" autoeventwireup="true" inherits="MPFParameter_Edit, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="MPFParamID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="MPF Parameter : - Edit" />
                </td>
            </tr>
        </table>
        
            
                
        
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar id="toolBar" runat="server"
                     NewButton_Visible="false" 
                     EditButton_Visible="false" 
                     OnBackButton_Click="Back_Click"
                     OnSaveButton_Click ="Save_Click"
                     OnDeleteButton_Click="Delete_Click"
                      />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openAlert(); return false;"/>
                </td>
            </tr>
        </table>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_field_section">
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="From" />:</td>
                <td class="pm_field">
                    <uc1:WebDatePicker id="MPFParamEffFr" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Min Monthly" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="MPFParamMinMonthly" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Max Monthly" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="MPFParamMaxMonthly" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Min Daily" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="MPFParamMinDaily" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Max Daily" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="MPFParamMaxDaily" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Employee Contribution" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="MPFParamEEPercent" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Employer Contribution" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="MPFParamERPercent" runat="Server" /></td>
            </tr>
        </table>
</asp:Content> 