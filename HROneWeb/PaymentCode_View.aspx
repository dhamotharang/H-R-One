<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PaymentCode_View.aspx.cs"    Inherits="PaymentCode_View" MasterPageFile="~/MainMasterPage.master" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="PaymentCodeID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Payment Code Setup" />
                </td>
            </tr>
        </table>
		<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="View Payment Code" />: 
                    <%=PaymentCode.Text %>
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
        <table border="0" width="100%" cellspacing="0" class="pm_field_section">
            <col width="15%" />
            <col width="35%" />
            <col width="15%" />
            <col width="35%" />
            <tr>
                <td class="pm_field_header" >
                    <asp:Label runat="server" Text="Payment Code" /> :
                </td>
                <td class="pm_field" >
                    <asp:Label ID="PaymentCode" runat="Server" />
                </td>
                <td class="pm_field_header" >
                    <asp:Label runat="server" Text="Description" /> :
                </td>
                <td class="pm_field" >
                    <asp:Label ID="PaymentCodeDesc" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Payment Type" /> :</td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PaymentTypeID" runat="server"/>
                    
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label5" runat="server" Text="Pro-rata Options" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:CheckBox ID="PaymentCodeIsProrata" runat="server" Text="Pro-rata when new join/terminated" Enabled="false" /><br />
                    <asp:CheckBox ID="PaymentCodeIsProrataLeave" runat="server" Text="Pro-rata for Leave" Enabled="false" /><br />
                    <asp:CheckBox ID="PaymentCodeIsProrataStatutoryHoliday" runat="server" Text="Pro-rata for Statutory Holiday" Enabled="false" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="ILabel5" runat="server" Text="Is MPF" /> :</td>
                <td class="pm_field">
                    <asp:Label ID="PaymentCodeIsMPF" runat="server" />
                </td>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Is Voluntary" /> :</td>
                <td class="pm_field">
                    <asp:Label ID="PaymentCodeIsTopUp" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="lblIsPfund" runat="server" Text="Is P-Fund" /> :</td>
                <td class="pm_field">
                    <asp:Label ID="PaymentCodeIsORSO" runat="server" />
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="ILabel6" runat="server" Text="Is Wages" /> :</td>
                <td class="pm_field">
                    <asp:Label ID="PaymentCodeIsWages" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="lblPaymentCodeHideInPaySlip" runat="server" Text="Hide in Pay Slip" /> :</td>
                <td class="pm_field">
                    <asp:Label ID="PaymentCodeHideInPaySlip" runat="server" />
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="ILabel3" runat="server" Text="Display Sequence No." />:</td>
                <td class="pm_field">
                    <asp:Label ID="PaymentCodeDisplaySeqNo" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="ILabel1" runat="server" Text="Rounding Rule" />  :</td>
                <td class="pm_field"  colspan="3">
                    <asp:Label ID="PaymentCodeRoundingRule" runat="server" />
                    <asp:Label ID="PaymentCodeDecimalPlace" runat="server" />
                    <asp:Label ID="Label3" runat="server" Text="decimal place(s)" />
                    <asp:Label ID="PaymentCodeRoundingRuleIsAbsoluteValue" runat="server" Text="Ignore -ve sign when rounding" />
                </td>
            </tr>
            <tr id="PaymentCodeNotRemoveContributionFromTopUpRow" runat="server" >
                <td class="pm_field_header">
                    <asp:Label ID="Label1" runat="server" Text="Do not deduct Voluntary contribution being contributed to mandatory contribution" /> :<br/>
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PaymentCodeNotRemoveContributionFromTopUp" runat="server" />
                    <asp:Label ID="Label2" runat="server" Text="(The setting &quotDo not deduct MPF Only payment being contribute&quot under AVC Plan Setting is required)" /> 
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label6" runat="server" Text="Rounding Rule" />  :</td>
                <td class="pm_field"  colspan="3">
                    <asp:Label ID="Label7" runat="server" />
                    <asp:Label ID="Label8" runat="server" />
                    <asp:Label ID="Label9" runat="server" Text="decimal place(s)" />
                    <asp:Label ID="Label10" runat="server" Text="Ignore -ve sign when rounding" />
                </td>
            </tr>
            <!-- Start 000159, Ricky So, 2015-01-23 -->            
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label11" runat="server" Text="Is Hit-Rate Based" />  :</td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PaymentCodeIsHitRateBased" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label12" runat="server" Text="Template Default Hit-Rate for " />  :</td>
                <td class="pm_field" colspan="3">
                    1st Month : <asp:Label ID="PaymentCodeDefaultRateAtMonth1" runat="Server" />% &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp 
                    2nd Month : <asp:Label ID="PaymentCodeDefaultRateAtMonth2" runat="Server" />% &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp 
                    3rd Month : <asp:Label ID="PaymentCodeDefaultRateAtMonth3" runat="Server" />%
                </td>
            </tr>
            <!-- End 000159, Ricky So, 2015-01-23 -->            
            <tr>
                <td class="pm_field_title" colspan="4">
                    <asp:Label ID="Label4" runat="server" Text="Taxation Payment Type" />
                </td>
            </tr>
            <asp:Repeater ID="TaxPaymentRepeater" runat="server" OnItemDataBound="TaxPaymentRepeater_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_field_header">
                            <asp:Label ID="TaxFormType" runat="server" />
                        </td>
                        <td class="pm_field" colspan="3">
                            <asp:Label ID="TaxPayID" runat="server" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
        </asp:UpdatePanel >
</asp:Content> 