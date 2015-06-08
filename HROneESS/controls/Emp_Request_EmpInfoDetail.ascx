<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Emp_Request_EmpInfoDetail.ascx.cs" Inherits="Emp_Request_EmpInfoDetail" %>
<input type="hidden" id="RequestEmpID" runat="server" />
<table width="100%" border="0" cellpadding="5" cellspacing="0" class="pm_field_section">
    <col width="15%" />
    <col width="35%" />
    <col width="15%" />
    <col width="35%" />
    <tr >
        <td colspan="4" class="pm_field_title">
            <asp:Label ID="Label1" Text="Request Employee Info Changes" runat="server" />
        </td>
    </tr>
    <tr  >
        <td class="pm_field_header">
            <asp:Label ID="Label16" Text="Emp No " runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="EmpNo" runat="server" />
        </td>
        <td class="pm_field_header">
            <asp:Label ID="Label3" Text="Alias" runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="RequestEmpAlias" runat="server" />
        </td>
    </tr>
    <tr >
        <td class="pm_field_header">
            <asp:Label ID="Label18" Text="Surname " runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpEngSurname" runat="server" />
        </td>
        <td class="pm_field_header">
            <asp:Label ID="Label19" Text="Other Name " runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpEngOtherName" runat="server" />
        </td>
    </tr>
    <tr >
        <td class="pm_field_header">
            <asp:Label ID="Label22" Text="Gender " runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpGender" runat="server" />
        </td>
        <td class="pm_field_header">
            <asp:Label ID="Label21" Text="Chinese Name " runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpChiFullName" runat="server" />
        </td>
    </tr>
    <tr >
        <td class="pm_field_header">
            <asp:Label ID="Label20" Text="HKID " runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpHKID" runat="server" /></td>
        <td class="pm_field_header">
            <asp:Label ID="Label23" Text="Date of Birth" runat="server" />
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpDateOfBirth" runat="server" />
        </td>
    </tr>
    <tr  >
        <td class="pm_field_header">
            <asp:Label ID="Label24" Text="Date of Joining " runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="EmpDateOfJoin" runat="server" />
        </td>
        <td class="pm_field_header">
            <asp:Label ID="Label25" Text="Start Date of Service " runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="EmpServiceDate" runat="server" />
        </td>
    </tr>                
    <tr >
        <td class="pm_field_header">
            <asp:Label ID="Label4" Text="Marital Status " runat="server" />:
        </td>
        <%-- Start 0000092, KuangWei, 2014-10-13 --%>        
        <td class="pm_field">
            <asp:Label ID="RequestEmpMaritalStatus" runat="server" />
        </td>
        <td class="pm_field_header">
            <asp:Label ID="Label26" Text="Place Of Birth " runat="server" />:
        </td>        
        <td class="pm_field" >
            <asp:Label ID="RequestEmpPlaceOfBirth" runat="server" />
        </td>        
        <%-- End 0000092, KuangWei, 2014-10-13 --%>        
    </tr>
    <tr >
        <td class="pm_field_header">
            <asp:Label ID="Label5" Text="Passport No." runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="RequestEmpPassportNo" runat="server" />
        </td>                
        <td class="pm_field_header">
            <asp:Label ID="Label6" Text="Passport Country " runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="RequestEmpPassportIssuedCountry" runat="server" />
        </td>
    </tr>
    <tr  >
        <td class="pm_field_header">
            <asp:Label ID="Label2" Text="Passport Expiry Date" runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="RequestEmpPassportExpiryDate" runat="server" />
        </td>                
        <td class="pm_field_header">
            <asp:Label ID="Label7" Text="Nationality " runat="server" />:
        </td>                        
        <td class="pm_field">
            <asp:Label ID="RequestEmpNationality" runat="server" />
        </td>
    </tr>
    <tr >
        <td class="pm_field_header">
            <asp:Label ID="Label12" Text="Residential Address " runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="RequestEmpResAddr" runat="server" /><br />
            <asp:Label ID="RequestEmpResAddrAreaCode" runat="Server" />
        </td>
        <td class="pm_field_header">
            <asp:Label ID="Label14" Text="Correspondence " runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="RequestEmpCorAdd" runat="server" />
        </td>
    </tr> 
    <tr >
        <td class="pm_field_header">
            <asp:Label ID="Label8" Text="Home " runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="RequestEmpHomePhoneNo" runat="server" />
        </td>
        <td class="pm_field_header">
            <asp:Label ID="Label9" Text="Mobile No." runat="server" />
        </td>
        <td class="pm_field">
            <asp:Label ID="RequestEmpMobileNo" runat="server" />
        </td>
    </tr>
    <tr >
        <td class="pm_field_header">
            <asp:Label ID="Label10" Text="Office " runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="RequestEmpOfficePhoneNo" runat="server" />
        </td>
        <td class="pm_field_header">
            <asp:Label ID="Label11" Text="Personal E-mail Address" runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="RequestEmpEmail" runat="server" />
        </td>
    </tr>
    <tr >
        <td class="pm_field_header">
            <asp:Label ID="Label13" Text="Submission Date" runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="RequestEmpCreateDate" runat="server" />
        </td>
        <td class="pm_field_header">
            <asp:Label ID="Label15" Text="Status" runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="RequestStatus" runat="server" />
        </td>                        
    </tr>                              
    <tr class="pm_field_title" runat="server" id="AuthorizerOptionSectionRow" >
        <td class="pm_field" colspan="4">
            <asp:Button Text="Authorize" CssClass="button" ID="Authorize" runat="server" OnClick="EmpInfoAuthorize_Click"  OnClientClick="return confirm('Are you sure?');" />
            <asp:Button Text="Reject" CssClass="button" ID="Reject" runat="server" OnClick="EmpInfoReject_Click"  OnClientClick="return confirm('Are you sure?');" />
            <br />
            <asp:Label ID="Label17" runat="server" Text="Reason" />:<asp:TextBox ID="RejectReason" runat="server" Columns="50" MaxLength="100" />
        </td>
    </tr>
</table>
