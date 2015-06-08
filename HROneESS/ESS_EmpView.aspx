<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ESS_EmpView.aspx.cs" Inherits="ESS_EmpView" MasterPageFile="~/MainMasterPage.master"  %>
<%-- Start 0000070, Miranda, 2014-08-27 --%>
<%@ Register Src="~/controls/Emp_Beneficiaries_List.ascx" TagName="Emp_Beneficiaries_List" TagPrefix="uc2" %>
<%-- End 0000070, Miranda, 2014-08-27 --%>

<asp:Content ID="bannerContent" runat="server" ContentPlaceHolderID="bannerContentPlaceHolder" >  
    <table width="898px" border="0" cellpadding="0" cellspacing="0">
      <tr>
        <td>
            <a href="ESS_Home.aspx">
                <img src="images/banner02_01.jpg" alt="" style="border-width: 0px; display : block" />
            </a>
        </td>
        <td  valign="bottom" >
            <img src="images/banner02_02.jpg" alt="" style="border-width: 0px; display : block" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <img src="images/banner02_03.jpg" alt="" style="border-width: 0px; display : block" />
        </td>
      </tr>
    </table>
</asp:Content>
    <asp:Content ID="mainContent" runat="server" ContentPlaceHolderID="mainContentPlaceHolder" > 
    <input type="hidden" id="EmpID" runat="server" name="ID" /> 
    <table width="100%" border="0" cellpadding="5" cellspacing="0" class="pm_field_section">
        <col width="15%" />
        <col width="35%" />
        <col width="15%" />
        <col width="35%" />
      <tr >
        <td colspan="4" class="pm_field_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td>
                        <asp:Label ID="lblHeader" runat="server" Text="Employee Info" />
                    </td>
                    <td align="right" >
                        <asp:Button ID="Edit" runat="server" Text="Edit" OnClick="Edit_Click" />
                    </td>
                </tr>
            </table>
        </td>
      </tr>
      <tr >
        <td class="pm_field_header">
            <asp:Label Text="Emp No " runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpNo" runat="server" />
        </td>
        <td class="pm_field_header">
            <asp:Label Text="Alias" runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpAlias" runat="server" />
        </td>
      </tr>
      <tr >
        <td class="pm_field_header">
            <asp:Label Text="Surname " runat="server"/>:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpEngSurname" runat="server" />
        </td>
        <td class="pm_field_header">
            <asp:Label Text="Other Name " runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpEngOtherName" runat="server" />
        </td>
      </tr>
      <tr >
        <td class="pm_field_header">
            <asp:Label ID="Label5" Text="Gender " runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpGender" runat="server" />
        </td>
        <td class="pm_field_header">
            <asp:Label Text="Chinese Name " runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpChiFullName" runat="server" />
        </td>
      </tr>
      <tr >
        <td class="pm_field_header">
            <asp:Label ID="Label6" Text="HKID " runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpHKID" runat="server" />
        </td>
        <td class="pm_field_header">
            <asp:Label Text="Date of Birth " runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpDateOfBirth" runat="server" />
        </td>
      </tr>
      <tr >
        <td class="pm_field_header">
            <asp:Label Text="Date of Joining " runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpDateOfJoin" runat="server" />
        </td>
        <td class="pm_field_header">
            <asp:Label Text="Start Date of Service " runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpServiceDate" runat="server" />
        </td>
      </tr>
      <tr >
        <td class="pm_field_header">
            <asp:Label Text="Marital Status " runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpMaritalStatus" runat="server" />
        </td>
        <td class="pm_field_header">
            <asp:Label Text="Place Of Birth " runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpPlaceOfBirth" runat="server" />
        </td>
      </tr>
      <tr >
        <td class="pm_field_header">
            <asp:Label Text="Passport No." runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpPassportNo" runat="server" />
        </td>
        <td class="pm_field_header">
            <asp:Label Text="Passport Country " runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpPassportIssuedCountry" runat="server" />
        </td>
      </tr>
	  <tr >
        <td class="pm_field_header">
            <asp:Label Text="Passport Expiry Date" runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpPassportExpiryDate" runat="server" />
        </td>
        <td class="pm_field_header">
            <asp:Label ID="Label7" Text="Nationality " runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpNationality" runat="server" />
        </td>
      </tr>
	  <tr >
        <td class="pm_field_header">
            <asp:Label ID="Label1" Text="Residential Address " runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpResAddr" runat="server" />
            <asp:Label ID="EmpResAddrAreaCode" runat="server" />
        </td>
        <td class="pm_field_header">
            <asp:Label ID="Label2" Text="Correspondence " runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpCorAddr" runat="server" />
        </td>
      </tr>              
	  <tr >
        <td class="pm_field_header">
            <asp:Label ID="Label8" Text="Probation Period " runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpProbaPeriod" runat="server" />
            <asp:Label ID="EmpProbaUnit" runat="server" />
        </td>
		<td class="pm_field_header">
		    <asp:Label ID="Label9" Text="Probation Last Date " runat="server" />:
		</td>
        <td class="pm_field" >
            <asp:Label ID="EmpProbaLastDate" runat="server" />
        </td>
      </tr>
	  <tr >
        <td class="pm_field_header">
            <asp:Label ID="Label10" Text="Notice Period " runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpNoticePeriod" runat="server" />
            <asp:Label ID="EmpNoticeUnit" runat="server" />
        </td>
        <td class="pm_field_header">
            <asp:Label ID="Label3" Text="Home " runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpHomePhoneNo" runat="server" />
        </td>
      </tr>
	  <tr  >
        <td class="pm_field_header">
            <asp:Label Text="Mobile No." runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="EmpMobileNo" runat="server" />
        </td>
        <td class="pm_field_header">
            <asp:Label Text="Office " runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="EmpOfficePhoneNo" runat="server" />
        </td>
      </tr>
	  <tr >
        <td class="pm_field_header">
            <asp:Label Text="Personal E-mail Address" runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="EmpEmail" runat="server" />
        </td>
        <td class="pm_field_header">
            <asp:Label Text="Company E-mail Address" runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="EmpInternalEmail" runat="server" />
        </td>                
      </tr>
    </table>
    <%-- Start 0000070, Miranda, 2014-08-27 --%>
    <br />
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="pm_field_section">
        <tr>
            <td class="pm_field_title">
                <asp:Label ID="Label4" runat="server" Text="Beneficiaries" />
            </td>
        </tr>
    </table>
    <uc2:Emp_Beneficiaries_List ID="Emp_Beneficiaries_List1" runat="server" />
    <%-- End 0000070, Miranda, 2014-08-27 --%>
</asp:Content> 
