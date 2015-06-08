<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Emp_Dependant_View.aspx.cs"    Inherits="Emp_Dependant_View" MasterPageFile="~/MainMasterPage.master"  %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/Emp_Common.ascx" TagName="Emp_Common" TagPrefix="uc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="EmpDependantID" runat="server" name="ID" />
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
                    <asp:Label runat="server" Text="View Dependant" />:
                    <%=EmpDependantSurname.Text %>
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
                    <asp:Label runat="server" Text="Surname" />:</td>
                <td class="pm_field" >
                    <asp:Label ID="EmpDependantSurname" runat="Server" /></td>
                <td class="pm_field_header" >
                    <asp:Label runat="server" Text="Other Name" />:</td>
                <td class="pm_field" >
                    <asp:Label ID="EmpDependantOtherName" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Chinese Name" />:</td>
                <td class="pm_field">
                    <asp:Label ID="EmpDependantChineseName" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label ID="Label3" runat="server" Text="Date of Birth" />:</td>
                <td class="pm_field">
                    <asp:label ID="EmpDependantDateOfBirth" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Gender" />:</td>
                <td class="pm_field">
                    <asp:label ID="EmpDependantGender" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label ID="Label1" runat="server" Text="Relationship" />:</td>
                <td class="pm_field">
                    <asp:label ID="EmpDependantRelationship" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label2" runat="server" Text="HKID" />:</td>
                <td class="pm_field">
                    <asp:Label ID="EmpDependantHKID" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Passport No." />:</td>
                <td class="pm_field">
                    <asp:Label ID="EmpDependantPassportNo" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Country of Issue" />:</td>
                <td class="pm_field">
                    <asp:Label ID="EmpDependantPassportIssuedCountry" runat="Server" /></td>
            </tr>
             <!-- Start 0000190, Miranda, 2015-04-30 -->
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label4" runat="server" Text="Medical scheme Insured" />:</td>
                <td class="pm_field">
                    <asp:CheckBox ID="EmpDependantMedicalSchemeInsured" runat="Server" Enabled="false" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label5" runat="server" Text="Medical Effective Date" />:</td>
                <td class="pm_field">
                    <asp:Label id="EmpDependantMedicalEffectiveDate" runat="server" />
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label6" runat="server" Text="Expiry Date" />:</td>
                <td class="pm_field">
                    <asp:Label id="EmpDependantExpiryDate" runat="server" />
                </td>
            </tr>
            <!-- End 0000190, Miranda, 2015-04-30 -->
        </table>
</asp:Content> 