<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Emp_Qualification_Edit.aspx.cs"    Inherits="Emp_Qualification_Edit" MasterPageFile="~/MainMasterPage.master"  %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/Emp_Common.ascx" TagName="Emp_Common" TagPrefix="uc1" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="EmpQualificationID" runat="server" name="ID" />
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
                    <asp:Label ID="ActionHeader" runat="server" Text="Edit" />
                    <asp:Label runat="server" Text="Qualification" />
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
                     EditButton_Visible="false" 
                     OnBackButton_Click="Back_Click"
                     OnSaveButton_Click ="Save_Click"
                     OnDeleteButton_Click="Delete_Click"
                      />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
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
                    <asp:Label runat="server" Text="Qualification" />:</td>
                <td class="pm_field" >
                    <asp:DropDownList ID="QualificationID" runat="server" />
                </td>
                <td class="pm_field_header" >
                    <asp:Label runat="server" Text="Institute" />:</td>
                <td class="pm_field" >
                    <asp:TextBox ID="EmpQualificationInstitution" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label1" runat="server" Text="Learning Method" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="EmpQualificationLearningMethod" runat="server"/>
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="From" />:</td>
                <td class="pm_field">
                    <uc1:WebDatePicker id="EmpQualificationFrom" runat="server" />
                </td>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="To" />:</td>
                <td class="pm_field">
                    <uc1:WebDatePicker id="EmpQualificationTo" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Remark" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:TextBox ID="EmpQualificationRemark" runat="Server" TextMode="MultiLine" Columns="35" Rows="5" />
                </td>
            </tr>
        </table>
</asp:Content> 