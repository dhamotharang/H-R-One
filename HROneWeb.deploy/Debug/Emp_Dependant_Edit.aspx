<%@ page language="C#" autoeventwireup="true" inherits="Emp_Dependant_Edit, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/Emp_Common.ascx" TagName="Emp_Common" TagPrefix="uc1" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>

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
                    <%=CurID<=0?HROne.Common.WebUtility.GetLocalizedString("Add"):HROne.Common.WebUtility.GetLocalizedString("Edit") %>
                    <asp:Label runat="server" Text="Dependant" />:
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
                    <asp:Label runat="server" Text="Surname" />:</td>
                <td class="pm_field" >
                    <asp:TextBox ID="EmpDependantSurname" runat="Server" /></td>
                <td class="pm_field_header" >
                    <asp:Label runat="server" Text="Other Name" />:</td>
                <td class="pm_field" >
                    <asp:TextBox ID="EmpDependantOtherName" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Chinese Name" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="EmpDependantChineseName" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label ID="Label3" runat="server" Text="Date of Birth" />:</td>
                <td class="pm_field">
                    <uc1:WebDatePicker id="EmpDependantDateOfBirth" runat="server" />
                </td>

            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Gender" />:</td>
                <td class="pm_field">
                    <asp:DropDownList ID="EmpDependantGender" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Relationship" />:</td>
                <td class="pm_field">
                    <asp:DropDownList ID="EmpDependantRelationship" runat="Server"  /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label1" runat="server" Text="HKID" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="EmpDependantHKID" runat="Server" />
                    (<asp:TextBox ID="EmpDependantHKID_Digit" runat="Server" />)</td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Passport No." />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="EmpDependantPassportNo" runat="Server"  /></td>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Country of Issue" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="EmpDependantPassportIssuedCountry" runat="Server" /></td>
            </tr>
            <!-- Start 0000190, Miranda, 2015-04-30 -->
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Medical scheme Insured" />:</td>
                <td class="pm_field">
                    <asp:CheckBox ID="EmpDependantMedicalSchemeInsured" runat="Server" Checked="true" AutoPostBack="true" OnCheckedChanged="EmpDependantMedicalSchemeInsured_Changed" on /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Medical Effective Date" />:</td>
                <td class="pm_field">
                    <uc1:WebDatePicker id="EmpDependantMedicalEffectiveDate" runat="server" Enabled="true" />
                </td>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Expiry Date" />:</td>
                <td class="pm_field">
                    <uc1:WebDatePicker id="EmpDependantExpiryDate" runat="server" Enabled="true" />
                </td>
            </tr>
            <!-- End 0000190, Miranda, 2015-04-30 -->
        </table>
</asp:Content> 