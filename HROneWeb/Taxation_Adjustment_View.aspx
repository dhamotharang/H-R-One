<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Taxation_Adjustment_View.aspx.cs"    Inherits="Taxation_Adjustment_View" MasterPageFile="~/MainMasterPage.master" %>

<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="toolbar"/>
    </Triggers>
    <ContentTemplate>
                    <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
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
                                <asp:Button ID="btnPaymentDetail" runat="server" Text="- Payment Detail -" CSSClass="button" OnClick="btnPaymentDetail_Click" /> 
                                   
                                <asp:Button ID="btnPlaceOfResidenceDetail" runat="server" Text="- Place of Residence Detail -" CSSClass="button" OnClick="btnPlaceOfResidenceDetail_Click" />
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
                        <asp:PlaceHolder ID="SpousePanel" runat="server" >
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
                        </asp:PlaceHolder>
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
                                <asp:Label runat="server" Text="Postal Address" />:
                            </td>
                            <td class="pm_field" colspan="3">
                                <asp:Label ID="TaxEmpCorAddr" runat="Server" />
                            </td>
                        </tr>
                        <tr>
                            <!-- Start 0000020, KuangWei, 2014-08-22 -->                        
                            <td class="pm_field_header">
                                <asp:Label ID="lblBCapacity" runat="server" Text="Capacity in which employed:" />
                                <asp:Label ID="lblMCapacity" runat="server" Text="Capacity engaged:" />
                            </td>
                            <td class="pm_field">
                                <asp:Label ID="TaxEmpCapacity" runat="Server" />
                            </td>
                            <td class="pm_field_header">
                                <asp:Label ID="lblPartTime" runat="server" Text="Part time Employer Name:" />
                            </td>
                            <td class="pm_field">
                                <asp:Label ID="TaxEmpPartTimeEmployer" runat="Server" />
                            </td>
                            <!-- End 0000020, KuangWei, 2014-08-22 -->                            
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
                        <!-- Start 0000020, KuangWei, 2014-08-22 -->
                        <asp:Panel ID="TaxFormBPanel1" runat="server">                        
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
                        </asp:Panel>
                        <!-- End 0000020, KuangWei, 2014-08-22 -->                            
                        <asp:Panel ID="TaxFormBFGPanel3" runat="server">
                            <tr>
                                <td class="pm_field_header" colspan="3">
                                    <asp:Label runat="server" Text="Amount" />
                                    (<asp:Label runat="server" Text="This amount must also be included in details of income paid or payable" />):
                                </td>
                                <td class="pm_field">
                                    <asp:Label ID="TaxEmpOverseasCompanyAmount" runat="Server" />
                                </td>
                            </tr>
                        </asp:Panel>
                        <!-- Start 0000020, KuangWei, 2014-08-22 -->
                        <asp:Panel ID="TaxFormMPanel1" runat="server">
                            <tr>
                                <td class="pm_field_header" colspan="3">
                                    <asp:Label ID="Label1" runat="server" Text="Whether a sum has been withheld from the above payment to settle the tax due by the recipient" />:
                                </td>
                                <td class="pm_field">
                                    <asp:Label ID="TaxEmpSumWithheldIndicator" runat="Server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="pm_field_header" colspan="3">
                                    <asp:Label ID="Label5" runat="server" Text="Amount" />
                                    (<asp:Label ID="Label6" runat="server" Text="If yes, please state the amount withheld $" />):
                                </td>
                                <td class="pm_field">
                                    <asp:Label ID="TaxEmpSumWithheldAmount" runat="Server" />
                                </td>
                            </tr>
                        </asp:Panel>
                       <!-- End 0000020, KuangWei, 2014-08-22 -->
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
                                <td class="pm_field_header" colspan="3">
                                    <asp:Label runat="server" Text="Any money held under Section 52(7) of the Inland Revenue Ordinance" />:
                                </td>
                                <td class="pm_field">
                                    <asp:Label ID="TaxEmpIsMoneyHoldByOrdinance" runat="Server" />
                                    <asp:Panel ID="TaxEmpHoldAmountPanel" runat="server">
                                        <asp:Label runat="server" Text="estimated amount" />
                                        $<asp:Label ID="TaxEmpHoldAmount" runat="Server" />
                                    </asp:Panel>
                                    <asp:Panel ID="TaxEmpReasonForNotHoldPanel" runat="server">
                                        <asp:Label runat="server" Text="the reason is" />
                                        <asp:Label ID="TaxEmpReasonForNotHold" runat="Server" />
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td class="pm_field_header">
                                    <asp:Label runat="server" Text="Reason for departure" />:
                                </td>
                                <td class="pm_field" colspan="3">
                                    <asp:Label ID="TaxEmpReasonForDepartureReason" runat="Server" />
                                    <asp:PlaceHolder ID="TaxEmpReasonForDepartureOtherReasonPanel" runat="server">
                                        ,<asp:Label runat="server" Text="please specify" />:
                                        <asp:Label ID="TaxEmpReasonForDepartureOtherReason" runat="Server" />
                                    </asp:PlaceHolder>
                                </td>
                            </tr>
                            <tr>
                                <td class="pm_field_header" colspan="3">
                                    <asp:Label runat="server" Text="Whether the employee would return to Hong Kong" />:
                                </td>
                                <td class="pm_field">
                                    <asp:Label ID="TaxEmpIsEEReturnHK" runat="Server" />
                                    <asp:Panel ID="TaxEmpEEReturnHKDatePanel" runat="server">
                                        <asp:Label ID="Label2" runat="server" Text="probable date of return is" />
                                        <asp:Label ID="TaxEmpEEReturnHKDate" runat="Server" />
                                    </asp:Panel>
                                </td>
                            </tr>
                        </asp:Panel>
                        <asp:Panel ID="TaxFormFGPanel" runat="server">
                            <tr>
                                <td class="pm_field_header">
                                    <asp:Panel ID="TaxFormFPanel3" runat="server">
                                        <asp:Label ID="Label4" runat="server" Text="Future Postal Address of Employee" />:
                                    </asp:Panel>
                                    <asp:Panel ID="TaxFormGPanel3" runat="server">
                                        <asp:Label ID="Label3" runat="server" Text="Postal address after departure" />:
                                    </asp:Panel>
                                </td>
                                <td class="pm_field" colspan="3">
                                    <asp:Label ID="TaxEmpFutureCorAddr" runat="Server" />
                                </td>
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
    </ContentTemplate>
    </asp:UpdatePanel> 
</asp:Content> 