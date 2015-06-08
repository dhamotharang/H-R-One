<%@ page language="C#" autoeventwireup="true" inherits="Emp_Beneficiaries_Edit, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>
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
                <%=CurID<=0?HROne.Common.WebUtility.GetLocalizedString("Add"):HROne.Common.WebUtility.GetLocalizedString("Edit") %>
                <asp:Label Text="Beneficiaries" runat="server" />
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
                <asp:Label Text="Name" runat="server" />:
            </td>
            <td class="pm_field" >
                <asp:TextBox ID="EmpBeneficiariesName" runat="Server" />
            </td>
            <td class="pm_field_header" >
                <asp:Label ID="Label1" Text="Share(%)" runat="server" />:
            </td>
            <td class="pm_field" >
                <asp:TextBox ID="EmpBeneficiariesShare" runat="Server" />
            </td>
        </tr>
        <tr>
            <td class="pm_field_header">
                <asp:Label Text="HKID" runat="server" />:
            </td>
            <td class="pm_field">
                <asp:TextBox ID="EmpBeneficiariesHKID" runat="Server" />(<asp:TextBox ID="EmpBeneficiariesHKID_Digit"
                    runat="Server" Columns="1" MaxLength="1" style="text-align:center" />)
            </td>
            <td class="pm_field_header">
                <asp:Label Text="Relation" runat="server" />:
            </td>
            <td class="pm_field">
                <asp:TextBox ID="EmpBeneficiariesRelation" runat="Server" />
            </td>
        </tr>
        <tr>
            <td class="pm_field_header">
                <asp:Label ID="Label4" Text="Address" runat="server" />:
            </td>
            <td class="pm_field" colspan="3">
                <asp:TextBox ID="EmpBeneficiariesAddress" runat="Server" TextMode="MultiLine" Columns="90" Rows="3" />
            </td>
        </tr>
        <tr>
            <td class="pm_field_header">
                <asp:Label ID="Label2" Text="District" runat="server" />:
            </td>
            <td class="pm_field">
                <asp:TextBox ID="EmpBeneficiariesDistrict" runat="Server" />
            </td>
            <td class="pm_field_header">
                <asp:Label ID="Label5" Text="Area" runat="server" />:
            </td>
            <td class="pm_field">
                <%-- Start 0000139, Miranda, 2014-12-20 --%>
                <asp:DropDownList ID="EmpBeneficiariesArea" runat="Server" />
                <%-- End 0000139, Miranda, 2014-12-20 --%>
            </td>
        </tr>
        <tr>
            <td class="pm_field_header">
                <asp:Label Text="Country" runat="server" />:
            </td>
            <td class="pm_field">
                <asp:TextBox ID="EmpBeneficiariesCountry" runat="Server" />
            </td>
        </tr>
    </table>
</asp:Content> 