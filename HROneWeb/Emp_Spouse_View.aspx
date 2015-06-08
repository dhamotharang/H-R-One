<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Emp_Spouse_View.aspx.cs"    Inherits="Emp_Spouse_View" MasterPageFile="~/MainMasterPage.master"  %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/Emp_Common.ascx" TagName="Emp_Common" TagPrefix="uc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="EmpSpouseID" runat="server" name="ID" />
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
                    <asp:Label runat="server" Text="View Spouse" />:
                    <%=EmpSpouseSurname.Text %>
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
                    <asp:Label ID="EmpSpouseSurname" runat="Server" /></td>
                <td class="pm_field_header" >
                    <asp:Label runat="server" Text="Other Name" />:</td>
                <td class="pm_field" >
                    <asp:Label ID="EmpSpouseOtherName" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Chinese Name" />:</td>
                <td class="pm_field">
                    <asp:Label ID="EmpSpouseChineseName" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label ID="Label3" runat="server" Text="Date of Birth" />:</td>
                <td class="pm_field">
                    <asp:Label ID="EmpSpouseDateOfBirth" runat="Server" />
                    <%-- Start 0000142, KuangWei, 2014-12-20 --%>
                    (<asp:Label ID="Label2" runat="Server" EnableViewState="false" Text="Age"/>:<asp:Label ID="lblAge" runat="Server" />)
                    <%-- End 0000142, KuangWei, 2014-12-20 --%>
                    </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label1" runat="server" Text="HKID" />:</td>
                <td class="pm_field">
                    <asp:Label ID="EmpSpouseHKID" runat="Server"  /></td>
            <%-- Start 0000142, KuangWei, 2014-12-18 --%>
                <td class="pm_field_header">
                    <asp:Label EnableViewState="false" Text="Sex" runat="server" />:
                </td>
                <td class="pm_field">
                    <asp:Label ID="EmpGender" runat="Server" />
                </td> 
            <%-- End 0000142, KuangWei, 2014-12-18 --%> 
            </tr> 
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Passport No." />:</td>
                <td class="pm_field">
                    <asp:Label ID="EmpSpousePassportNo" runat="Server"  /></td>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Country of Issue" />:</td>
                <td class="pm_field">
                    <asp:Label ID="EmpSpousePassportIssuedCountry" runat="Server" /></td>
            </tr>
        <%-- Start 0000142, KuangWei, 2014-12-20 --%>
        <tr>
            <td class="pm_field_header">
                <asp:Label ID="Label5" Text="Medical Scheme Insured" runat="server"  />?
            </td>
            <td class="pm_field" >
                <asp:Label ID="EmpIsMedicalSchemaInsured" runat="Server" />
            </td>
            <td class="pm_field_header">
                <asp:Label ID="Label18" Text="Medical Effective Date" runat="server"  />:
            </td>
            <td class="pm_field" >
                <asp:Label ID="EmpMedicalEffectiveDate" runat="Server" />
            </td>
        </tr>
        <tr>
            <td class="pm_field_header">
                <asp:Label ID="Label4" Text="Expiry Date" runat="server"  />:
            </td>
            <td class="pm_field" >
                <asp:Label ID="EmpMedicalExpiryDate" runat="Server" />   
            </td>     
        </tr>
        <%-- End 0000142, KuangWei, 2014-12-20 --%>            
        </table>
</asp:Content> 