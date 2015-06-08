<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Emp_WorkingExperience_View.aspx.cs" Inherits="Emp_WorkingExperience_View"  MasterPageFile="~/MainMasterPage.master" %>

<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/Emp_Common.ascx" TagName="Emp_Common" TagPrefix="uc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="EmpWorkExpID" runat="server" name="ID" />
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
                    <asp:Label Text="View Working Experiencet" runat="server" />
                </td>
            </tr>
        </table>
        
            
                
        
		<uc1:Emp_Common id="Emp_Common1" runat="server"></uc1:Emp_Common><br />
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
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="15%" />
            <col width="35%" />
            <col width="15%" />
            <col width="35%" />
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label1" Text="From" runat="server" />
                    (<asp:Label ID="Label6" Text="Month" runat="server" />/<asp:Label ID="Label8" Text="Year" runat="server" />):
                </td>
                <td class="pm_field" >
                    <asp:Label ID="EmpWorkExpFromMonth" runat="Server" />
                    <asp:Label ID="EmpWorkExpFromYear" runat="Server" />
                </td>
                <td class="pm_field_header" >
                    <asp:Label ID="Label2" Text="To" runat="server" />
                    (<asp:Label ID="Label7" Text="Month" runat="server" />/<asp:Label ID="Label9" Text="Year" runat="server" />):
                </td>
                <td class="pm_field" >
                    <asp:Label ID="EmpWorkExpToMonth" runat="Server" />
                    <asp:Label ID="EmpWorkExpToYear" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label3" Text="Company Name" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="EmpWorkExpCompanyName" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label ID="Label4" Text="Position" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="EmpWorkExpPosition" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label10" Text="Employment Type" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="EmpWorkExpEmploymentTypeID" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label ID="Label11" Text="Relevance Experience" runat="server" />?</td>
                <td class="pm_field">
                    <asp:Label ID="EmpWorkExpIsRelevantExperience" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label5" Text="Remark" runat="server" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="EmpWorkExpRemark" runat="Server"  /></td>
            </tr>
        </table>
</asp:Content> 