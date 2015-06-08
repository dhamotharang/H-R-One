<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Taxation_Adjustment_View_Update_Backup.aspx.cs"    Inherits="Taxation_Adjustment_View_Update_Backup" MasterPageFile="~/MainMasterPage.master" %>

<%@ Register Src="~/controls/Taxation_Form_Header.ascx" TagName="Taxation_Form_Header"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">

        <input type="hidden" id="TaxEmpID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Taxation Adjustment" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="View Taxation Adjustment" />
                </td>
            </tr>
        </table>
        <table width="100%" class="pm_section">
            <tr>
                <td valign="top" colspan="2">
                    <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
                        <tr>
                            <td>
                                <asp:Panel ID="TaxEmpEditPanel" runat="Server">
                                                               <asp:Button ID="Back" runat="server" Text="- Back -" OnClick="Back_Click" UseSubmitBehavior="false"
                                    cssclass="button" /> 

                                    <input type="button" value="Edit" onclick="document.location='Taxation_Adjustment_Edit.aspx?TaxEmpID=<%=TaxEmpID.Value %>';"
                                        class="button" />
                                </asp:Panel>
                            </td>
                            <td align="right">
                                <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openAlert(); return false;" />
                            </td>
                        </tr>
                    </table>
                    <table border="1" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
                        <col width="15%" />
                        <col width="35%" />
                        <col width="15%" />
                        <col width="35%" />
                        <tr>
                            <td colspan="4">
                                <uc1:Taxation_Form_Header ID="ucTaxation_Form_Header" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="pm_field_title" colspan="1">
                                <asp:Label runat="server" Text="Detail" />
                            </td>
                            <td class="pm_field_title" colspan="3">
                                <input type="button" value="- Payment Detail -" onclick="document.location='Taxation_Adjustment_Payment_View.aspx?TaxEmpID=<%=TaxEmpID.Value %>';"
                                    class="button" />
                                <input type="button" value="- Place of Residence Detail -" onclick="document.location='Taxation_Adjustment_PlaceOfResidence_View.aspx?TaxEmpID=<%=TaxEmpID.Value %>';"
                                    class="button" />
                            </td>
                        </tr>
                        <asp:Panel ID="TaxFormBFGPanel1" runat="server">
                            <tr>
                                <td class="pm_field_header">
                                    <asp:Label runat="server" Text="Tax File No." />.:
                                </td>
                                <td class="pm_field" colspan="3">
                                    <asp:Label ID="TaxEmpTaxFileNo" runat="Server" />
                                </td>
                            </tr>
                        </asp:Panel>
                        <tr>
                            <td class="pm_field_header" >
                                <asp:Label runat="server" Text="Surname" />:
                            </td>
                            <td class="pm_field" >
                                <asp:Label ID="TaxEmpSurname" runat="Server" />
                            </td>
                            <td class="pm_field_header" >
                                <asp:Label runat="server" Text="Other Name in Full" />:
                            </td>
                            <td class="pm_field" >
                                <asp:Label ID="TaxEmpOtherName" runat="Server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="pm_field_header" >
                                <asp:Label runat="server" Text="Chinese Name" />:
                            </td>
                            <td class="pm_field">
                                <asp:Label ID="TaxEmpChineseName" runat="Server" />
                            </td>
                            <td class="pm_field_header" >
                                <asp:Label runat="server" Text="HKID" />:
                            </td>
                            <td class="pm_field">
                                <asp:Label ID="TaxEmpHKID" runat="Server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="pm_field_header" >
                                <asp:Label runat="server" Text="Sex" />:
                            </td>
                            <td class="pm_field">
                                <asp:Label ID="TaxEmpSex" runat="Server" />
                            </td>
                            <td class="pm_field_header" >
                                <asp:Label runat="server" Text="Martial Status" />:
                            </td>
                            <td class="pm_field">
                                <asp:Label ID="TaxEmpMartialStatus" runat="Server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="pm_field_header" >
                                <asp:Label runat="server" Text="Passport No." />.:
                            </td>
                            <td class="pm_field">
                                <asp:Label ID="TaxEmpPassportNo" runat="Server" />
                            </td>
                            <td class="pm_field_header" >
                                <asp:Label runat="server" Text="Country Issued" />:
                            </td>
                            <td class="pm_field">
                                <asp:Label ID="TaxEmpIssuedCountry" runat="Server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="pm_field_header">
                                <asp:Label runat="server" Text="Spouse Name" />:
                            </td>
                            <td class="pm_field">
                                <asp:Label ID="TaxEmpSpouseName" runat="Server" />
                            </td>
                            <td class="pm_field_header">
                                <asp:Label runat="server" Text="Spouse HKID" />:
                            </td>
                            <td class="pm_field">
                                <asp:Label ID="TaxEmpSpouseHKID" runat="Server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="pm_field_header">
                                <asp:Label runat="server" Text="Spouse Passport No" />.:
                            </td>
                            <td class="pm_field">
                                <asp:Label ID="TaxEmpSpousePassportNo" runat="Server" />
                            </td>
                            <td class="pm_field_header">
                                <asp:Label runat="server" Text="Spouse Passport Country Issued" />:
                            </td>
                            <td class="pm_field">
                                <asp:Label ID="TaxEmpSpouseIssuedCountry" runat="Server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="pm_field_header">
                                <asp:Label runat="server" Text="Residential Address" />:
                            </td>
                            <td class="pm_field" colspan="3">
                                <asp:Label ID="TaxEmpResAddr" runat="Server" />
                                <asp:Label ID="TaxEmpResAddrAreaCode" runat="Server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="pm_field_header">
                                <asp:Label runat="server" Text="Correspondence Address" />:
                            </td>
                            <td class="pm_field" colspan="3">
                                <asp:Label ID="TaxEmpCorAddr" runat="Server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="pm_field_header">
                                <asp:Label runat="server" Text="Capacity in which employed" />:
                            </td>
                            <td class="pm_field">
                                <asp:Label ID="TaxEmpCapacity" runat="Server" />
                            </td>
                            <td class="pm_field_header">
                                <asp:Label runat="server" Text="Part time Employer Name" />:
                            </td>
                            <td class="pm_field">
                                <asp:Label ID="TaxEmpPartTimeEmployer" runat="Server" />
                            </td>
                        </tr>
                        <asp:Panel ID="TaxFormBFGPanel2" runat="server">
                            <tr>
                                <td class="pm_field_header">
                                    <asp:Label runat="server" Text="Period of employment" />:
                                </td>
                                <td class="pm_field" colspan="3">
                                    <asp:Label ID="TaxEmpStartDate" runat="Server" />
                                    -
                                    <asp:Label ID="TaxEmpEndDate" runat="Server" />
                                </td>
                            </tr>
                        </asp:Panel>
                        <asp:Panel ID="TaxFormFPanel1" runat="server">
                            <tr>
                                <td class="pm_field_header">
                                    <asp:Label runat="server" Text="Reason for Cessation" />:
                                </td>
                                <td class="pm_field" colspan="3">
                                    <asp:Label ID="TaxEmpCessationReason" runat="Server" />
                                </td>
                            </tr>
                        </asp:Panel>
                            <tr>
                                <td class="pm_field_header" colspan="3">
                                    <asp:Label runat="server" Text="Oversea concern yes no" />:
                                </td>
                                <td class="pm_field">
                                    <asp:Label ID="TaxEmpOvearseasIncomeIndicator" runat="Server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="pm_field_header">
                                    <asp:Label runat="server" Text="Name of overseas company" />:
                                </td>
                                <td class="pm_field" colspan="3">
                                    <asp:Label ID="TaxEmpOverseasCompanyName" runat="Server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="pm_field_header">
                                    <asp:Label runat="server" Text="Address" />:
                                </td>
                                <td class="pm_field" colspan="3">
                                    <asp:Label ID="TaxEmpOverseasCompanyAddress" runat="Server" />
                                </td>
                            </tr>
                        <asp:Panel ID="TaxFormBFGPanel3" runat="server">
                            <tr>
                                <td class="pm_field_header" colspan="3">
                                    <asp:Label runat="server" Text="Amount" />
                                    (<asp:Label runat="server" Text="This amount must also be included in item 11" />):
                                </td>
                                <td class="pm_field">
                                    <asp:Label ID="TaxEmpOverseasCompanyAmount" runat="Server" />
                                </td>
                            </tr>
                        </asp:Panel>
                        <asp:Panel ID="TaxFormBPanel" runat="server">
                            <tr>
                                <td class="pm_field_header">
                                    <asp:Label runat="server" Text="Remark" />:
                                </td>
                                <td class="pm_field" colspan="3">
                                    <asp:Label ID="TaxEmpRemark" runat="Server" />
                                </td>
                            </tr>
                        </asp:Panel>
                        <asp:Panel ID="TaxFormEPanel1" runat="server" Visible="true">
                            <tr>
                                <td class="pm_field_header" colspan="3">
                                    <asp:Label runat="server" Text="Name and address of previous Hong Kong Employer" />:
                                </td>
                                <td class="pm_field">
                                    <asp:Label ID="TaxEmpPreviousEmployerNameddress" runat="Server" />
                                </td>
                            </tr>
                        </asp:Panel>
                        <asp:Panel ID="TaxFormFPanel2" runat="server">
                            <tr>
                                <td class="pm_field_header">
                                    <asp:Label runat="server" Text="New employer name and address" />:
                                </td>
                                <td class="pm_field" colspan="3">
                                    <asp:Label ID="TaxEmpNewEmployerNameddress" runat="Server" />
                                </td>
                            </tr>
                        </asp:Panel>
                        <asp:Panel ID="TaxFormFGPanel" runat="server">
                            <tr>
                                <asp:Panel ID="TaxFormFPanel3" runat="server">
                                    <td class="pm_field_header">
                                        <asp:Label runat="server" Text="Future correspondence address of employee" />:
                                    </td>
                                </asp:Panel>
                                <asp:Panel ID="TaxFormGPanel3" runat="server">
                                    <td class="pm_field_header">
                                        <asp:Label runat="server" Text="Correspondence address after departure" />:
                                    </td>
                                </asp:Panel>
                                <td class="pm_field" colspan="3">
                                    <asp:Label ID="TaxEmpFutureCorAddr" runat="Server" />
                                </td>
                            </tr>
                        </asp:Panel>
                        <asp:Panel ID="TaxFormGPanel1" runat="server" Visible="true">
                            <tr>
                                <td class="pm_field_header">
                                    <asp:Label runat="server" Text="Leave Hong Kong Date" />:
                                </td>
                                <td class="pm_field" colspan="3">
                                    <asp:Label ID="TaxEmpLeaveHKDate" runat="Server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="pm_field_header">
                                    <asp:Label runat="server" Text="The employee's Salaries Tax borne by employer" />:
                                </td>
                                <td class="pm_field" colspan="3">
                                    <asp:Label ID="TaxEmpIsERBearTax" runat="Server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="pm_field_header">
                                    <asp:Label runat="server" Text="Any money held under Section 52(7) of the Inland Revenue Ordinance" />:
                                </td>
                                <td class="pm_field">
                                    <asp:Label ID="TaxEmpIsMoneyHoldByOrdinance" runat="Server" />
                                </td>
                                <asp:Panel ID="TaxEmpHoldAmountPanel" runat="server">
                                    <td class="pm_field_header">
                                        <asp:Label runat="server" Text="estimated amount" />
                                        $:
                                    </td>
                                    <td class="pm_field">
                                        <asp:Label ID="TaxEmpHoldAmount" runat="Server" />
                                    </td>
                                </asp:Panel>
                                <asp:Panel ID="TaxEmpReasonForNotHoldPanel" runat="server">
                                    <td class="pm_field_header">
                                        <asp:Label runat="server" Text="the reason is" />:
                                    </td>
                                    <td class="pm_field">
                                        <asp:Label ID="TaxEmpReasonForNotHold" runat="Server" />
                                    </td>
                                </asp:Panel>
                            </tr>
                            <tr>
                                <td class="pm_field_header">
                                    <asp:Label runat="server" Text="Reason for departure" />:
                                </td>
                                <td class="pm_field">
                                    <asp:Label ID="TaxEmpReasonForDepartureReason" runat="Server" />
                                </td>
                                <asp:Panel ID="TaxEmpReasonForDepartureOtherReasonPanel" runat="server">
                                    <td class="pm_field_header">
                                        <asp:Label runat="server" Text="please specify" />
                                    </td>
                                    <td class="pm_field">
                                        <asp:Label ID="TaxEmpReasonForDepartureOtherReason" runat="Server" />
                                    </td>
                                </asp:Panel>
                            </tr>
                            <tr>
                                <td class="pm_field_header">
                                    <asp:Label runat="server" Text="Whether the employee would return to Hong Kong" />:
                                </td>
                                <td class="pm_field">
                                    <asp:Label ID="TaxEmpIsEEReturnHK" runat="Server" />
                                </td>
                                <asp:Panel ID="TaxEmpEEReturnHKDatePanel" runat="server">
                                    <td class="pm_field_header">
                                        <asp:Label runat="server" Text="probable date of return is" />
                                    </td>
                                    <td class="pm_field">
                                        <asp:Label ID="TaxEmpEEReturnHKDate" runat="Server" />
                                    </td>
                                </asp:Panel>
                            </tr>
                        </asp:Panel>
                        <asp:Panel ID="TaxFormEGPanel1" runat="server" Visible="true">
                            <tr>
                                <asp:Panel ID="TaxFormGPanel4" runat="server" Visible="true">
                                    <td class="pm_field_header" colspan="3">
                                        <asp:Label runat="server" Text="Whether the employee has any share options granted by your company or any other
                                        corporation in respect of his employment with/office in your company that are not
                                        yet exercised, assigned or released" />.
                                    </td>
                                </asp:Panel>
                                <asp:Panel ID="TaxFormEPanel2" runat="server" Visible="true">
                                    <td class="pm_field_header" colspan="3">
                                        <asp:Label runat="server" Text="Whether the employee has been conditionally granted a share options prior to commencing
                                        to be employed in Hong Kong, which can be exercised after rendering services in
                                        Hong Kong" />
                                    </td>
                                </asp:Panel>
                                <td class="pm_field">
                                    <asp:Label ID="TaxEmpIsShareOptionsGrant" runat="Server" />
                                </td>
                            </tr>
                        </asp:Panel>
                        <asp:Panel ID="TaxFormGPanel2" runat="server" Visible="true">
                            <asp:Panel ID="TaxEmpIsShareOptionsGrantPanel" runat="server">
                                <tr>
                                    <td class="pm_field_header">
                                        <asp:Label runat="server" Text="no. of shares not yet exercised" />:
                                    </td>
                                    <td class="pm_field">
                                        <asp:Label ID="TaxEmpShareOptionsGrantCount" runat="Server" />
                                    </td>
                                    <td class="pm_field_header">
                                        <asp:Label runat="server" Text="Date of Grant" />:
                                    </td>
                                    <td class="pm_field">
                                        <asp:Label ID="TaxEmpShareOptionsGrantDate" runat="Server" />
                                    </td>
                                </tr>
                            </asp:Panel>
                        </asp:Panel>
                    </table>
                </td>
            </tr>
        </table>
</asp:Content> 