<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Emp_Beneficiaries_View.aspx.cs" Inherits="Emp_Beneficiaries_View"  MasterPageFile="~/MainMasterPage.master" %>

<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/Emp_Common.ascx" TagName="Emp_Common" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <input type="hidden" id="EmpBeneficiariesID" runat="server" name="ID" />
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
                <asp:Label Text="View Beneficiaries" runat="server" />
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
                <asp:Label ID="Label1" Text="Name" runat="server" />:
            </td>
            <td class="pm_field" >
                <asp:Label ID="EmpBeneficiariesName" runat="Server" />
            </td>
            <td class="pm_field_header" >
                <asp:Label ID="Label2" Text="Share(%)" runat="server" />:
            </td>
            <td class="pm_field" >
                <asp:Label ID="EmpBeneficiariesShare" runat="Server" />
            </td>
        </tr>
        <tr>
            <td class="pm_field_header">
                <asp:Label ID="Label3" Text="HKID" runat="server" />:
            </td>
            <td class="pm_field">
                <asp:Label ID="EmpBeneficiariesHKID" runat="Server" />
            </td>
            <td class="pm_field_header">
                <asp:Label ID="Label4" Text="Relation" runat="server" />:
            </td>
            <td class="pm_field">
                <asp:Label ID="EmpBeneficiariesRelation" runat="Server" />
            </td>
        </tr>
        <tr>
            <td class="pm_field_header">
                <asp:Label ID="Label10" Text="Address" runat="server" />:
            </td>
            <td class="pm_field" colspan="3">
                <asp:Label ID="EmpBeneficiariesAddress" runat="Server" />
            </td>
        </tr>
        <tr>
            <td class="pm_field_header">
                <asp:Label ID="Label6" Text="District" runat="server" />:
            </td>
            <td class="pm_field">
                <asp:Label ID="EmpBeneficiariesDistrict" runat="Server" />
            </td>
            <td class="pm_field_header">
                <asp:Label ID="Label8" Text="Area" runat="server" />:
            </td>
            <td class="pm_field">
                <asp:Label ID="EmpBeneficiariesArea" runat="Server" />
            </td>
        </tr>
        <tr>
            <td class="pm_field_header">
                <asp:Label ID="Label5" Text="Country" runat="server" />:
            </td>
            <td class="pm_field">
                <asp:Label ID="EmpBeneficiariesCountry" runat="Server" />
            </td>
        </tr>
    </table>
</asp:Content> 