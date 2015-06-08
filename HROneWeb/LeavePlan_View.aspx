<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LeavePlan_View.aspx.cs" Inherits="LeavePlan_View" MasterPageFile="~/MainMasterPage.master"  EnableEventValidation="false"  %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/LeavePlanEntitle_List.ascx" TagName="LeavePlanEntitle_List"
    TagPrefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="LeavePlanID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label runat="server" EnableViewState="false" Text="Leave Plan Setup" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label runat="server" EnableViewState="false" Text="View Leave Plan" />:
                    <%=LeavePlanDesc.Text %>
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
                     <%-- Start 0000005, Miranda, 2014-03-22 --%>
                     <asp:Button ID="ReCalc" CssClass="button" runat="server" Text="Re-Calculate All Staff's Leave Balance" OnClick="ReCalc_Click" />
                     <%-- End 0000005, Miranda, 2014-03-22 --%>
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
                </td>
            </tr>
        </table>
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="40%" />
            <col width="60%" />
            <tr>
                <td class="pm_field_header" >
                    <asp:Label runat="server" EnableViewState="false" Text="Code" />:</td>
                <td class="pm_field">
                    <asp:Label ID="LeavePlanCode" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" EnableViewState="false" Text="Description" />:</td>
                <td class="pm_field">
                    <asp:Label ID="LeavePlanDesc" runat="Server" /></td>
            </tr>
            <tr>
                <td align="left" class="pm_field_header">
                    <asp:Label ID="Label17" runat="server" EnableViewState="false" Text="Use Common Leave Year" />?
                </td>
                <td align="left" class="pm_field">
                    <asp:Label ID="LeavePlanUseCommonLeaveYear" runat="server" />
                </td>
            </tr>
            <tr id="CommonLeaveYearRow1" runat="server" >
                <td align="left" class="pm_field_header">
                    <asp:Label ID="lblLeavePlanCommonLeaveYearStartDate" runat="server" EnableViewState="false" Text="Common Leave Year Start Date" />
                </td>
                <td align="left" class="pm_field">
                    <asp:Label ID="LeavePlanCommonLeaveYearStartDay" runat="server" />
                    <asp:Label ID="LeavePlanCommonLeaveYearStartMonth" runat="server" />
                </td>
            </tr>
            <tr id="CommonLeaveYearRow2" runat="server" >
                <td class="pm_field_header">
                    <asp:Label ID="Label3" runat="server" EnableViewState="false" Text="Year of Service Calculation Method" />:
                </td>
                <td class="pm_field">
                    <asp:Label ID="LeavePlanNoCountFirstIncompleteYearOfService" runat="Server" />
                </td>
            </tr>
            <tr id="ALProrataRoundingRuleRow" runat="server" >
                <td class="pm_field_header">
                    <asp:Label ID="Label5" runat="server" EnableViewState="false" Text="Annual Leave Prorata Rounding Rule" />:</td>
                <td class="pm_field">
                    <asp:Label ID="ALProrataRoundingRuleID" runat="Server" /><br />
                    <asp:Label ID="LeavePlanALRoundingRuleIsApplyFirstYearBroughtForwardOnly" runat="server" EnableViewState="false" Text="Apply on first year brought forward only" Visible="false"  />
                </td>
            </tr>
            <tr id="ResetYearOfServiceRow" runat="server">
                <td class="pm_field_header">
                    <asp:Label ID="Label1" runat="server" EnableViewState="false" Text="Reset Year of Service When Changed Employee Leave Plan" />:</td>
                <td class="pm_field">
                    <asp:Label ID="LeavePlanResetYearOfService" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label8" runat="server" EnableViewState="false" Text="Do not include Feb 29 when calculating prorata" />:
                </td>
                <td class="pm_field">
                    <asp:Label ID="LeavePlanProrataSkipFeb29" runat="Server" />
                </td>
            </tr>
            <tr id="ComparePreviousLeavePlanRow" runat="server">
                <td class="pm_field_header">
                    <asp:Label ID="Label2" runat="server" EnableViewState="false" Text="Compare previous leave plan enrolled" />:
                </td>
                <td class="pm_field">
                    <asp:Label ID="LeavePlanComparePreviousLeavePlan" runat="Server" />
                </td>
            </tr>
            <tr runat="server" id="LeavePlanComparePreviousLeavePlanOptionPanel">
                <td class="pm_field_header">
                    <asp:Label ID="Label18" runat="server" EnableViewState="false" Text="Leave Plan Rank Level" />:
                </td>
                <td class="pm_field">
                    <asp:Label ID="LeavePlanLeavePlanCompareRank" runat="Server" />
                </td>
            </tr>
            <asp:PlaceHolder ID="RestDayStatutoryHolidayPanel" runat="server" >
            <tr>
                <td class="pm_field_title" colspan="2">
                    <asp:Label ID="Label16" runat="server" EnableViewState="false" Text="Statutory Holiday Options" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label6" runat="server" EnableViewState="false" Text="Enable Statutory Holiday Entitlement in Employee Leave Balance" />:</td>
                <td class="pm_field">
                    <asp:Label ID="LeavePlanUseStatutoryHolidayEntitle" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_title" colspan="2">
                    <asp:Label ID="Label4" runat="server" EnableViewState="false" Text="Public Holiday Options" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label7" runat="server" EnableViewState="false" Text="Enable Public Holiday Entitlement in Employee Leave Balance" />:</td>
                <td class="pm_field">
                    <asp:Label ID="LeavePlanUsePublicHolidayEntitle" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_title" colspan="2">
                    <asp:Label ID="Label12" runat="server" EnableViewState="false" Text="Rest Day Options" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label9" runat="server" EnableViewState="false" Text="Enable Rest Day Entitlement in Employee Leave Balance" />:</td>
                <td class="pm_field">
                    <asp:Label ID="LeavePlanUseRestDayEntitle" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label10" runat="server" EnableViewState="false" Text="Number of Rest Day Entitled" />:</td>
                <td class="pm_field">
                    <asp:Label ID="LeavePlanRestDayEntitleDays" runat="Server" />&nbsp&nbsp<asp:Label runat="server" EnableViewState="false" Text="day(s)" /> <asp:Label ID="LeavePlanRestDayEntitlePeriod" runat="Server" />
                </td>
            </tr>
                <tr id="RestDayWeeklyOption" runat="server">
                <td class="pm_field_header">
                    <asp:Label ID="Label11" runat="server" EnableViewState="false" Text="Week to gain Rest Day for Weekly Entilement" />:</td>
                <td class="pm_field">
                    <asp:Label ID="LeavePlanRestDayWeeklyEntitleStartDay" runat="Server" />
                </td>
            </tr>
            <tr id="RestDayMonthlyOption1" runat="server">
                <td class="pm_field_header">
                    <asp:Label ID="Label14" runat="server" EnableViewState="false" Text="Number of day for prorata calculation" />:</td>
                <td class="pm_field">
                    <asp:Label ID="LeavePlanRestDayMonthlyEntitleProrataBase" runat="Server" />
                    (0 = <asp:Label ID="Label13" runat="server" EnableViewState="false" Text="Use Calendar Days"/>)
                </td>
            </tr>
            <tr id="RestDayMonthlyOption2" runat="server">
                <td class="pm_field_header">
                    <asp:Label ID="Label15" runat="server" EnableViewState="false" Text="Rest Day Prorata Rounding Rule" />:</td>
                <td class="pm_field">
                    <asp:Label ID="LeavePlanRestDayMonthlyEntitleProrataRoundingRuleID" runat="Server" /></td>
            </tr>
            </asp:PlaceHolder>
        </table>
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td align="right">
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
        <ContentTemplate >
            <asp:DataList ID="LeavePlanEntitle" runat="server" RepeatColumns="3" ItemStyle-VerticalAlign="Top" AlternatingItemStyle-VerticalAlign="Top" OnItemDataBound="LeavePlanEntitle_ItemDataBound" RepeatDirection="Horizontal"  >
                <HeaderTemplate>
                </HeaderTemplate>
                <ItemTemplate>
                        <uc1:LeavePlanEntitle_List ID="LeavePlanEntitle_List" runat="server" />
                </ItemTemplate>
                <AlternatingItemTemplate>
                        <uc1:LeavePlanEntitle_List ID="LeavePlanEntitle_List" runat="server" />
                </AlternatingItemTemplate>
                <FooterTemplate>
                </FooterTemplate>
                <AlternatingItemStyle VerticalAlign="Top" />
                <ItemStyle VerticalAlign="Top" />
            </asp:DataList>
        </ContentTemplate> 
        </asp:UpdatePanel> 
</asp:Content> 