<%@ page language="C#" autoeventwireup="true" inherits="LeavePlan_Edit, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="LeavePlanID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" EnableViewState="false" Text="Leave Plan Setup" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <%=CurID<=0?HROne.Common.WebUtility.GetLocalizedString("Add"):HROne.Common.WebUtility.GetLocalizedString("Edit") %>
                    <asp:Label ID="Label2" runat="server" EnableViewState="false" Text="Leave Plan " />:
                    <%=LeavePlanDesc.Text %>
                </td>
            </tr>
        </table>
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
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
        <ContentTemplate>
            <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_field_section">
                <col width="40%" />
                <col width="60%" />
                <tr>
                    <td class="pm_field_header" >
                        <asp:Label runat="server" EnableViewState="false" Text="Code" />:</td>
                    <td class="pm_field">
                        <asp:TextBox ID="LeavePlanCode" runat="Server" /></td>
                </tr>
                <tr>
                    <td class="pm_field_header">
                        <asp:Label runat="server" EnableViewState="false" Text="Description" />:</td>
                    <td class="pm_field">
                        <asp:TextBox ID="LeavePlanDesc" runat="Server" /></td>
                </tr>
                <tr>
                    <td align="left" class="pm_field_header">
                        <asp:Label runat="server" EnableViewState="false" Text="Use Common Leave Year" />?
                    </td>
                    <td align="left" class="pm_field">
                        <asp:CheckBox ID="LeavePlanUseCommonLeaveYear" runat="server" AutoPostBack="true" Checked="true" />
                    </td>
                </tr>
                <tr id="CommonLeaveYearRow1" runat="server" >
                    <td align="left" class="pm_field_header">
                        <asp:Label ID="lblLeavePlanCommonLeaveYearStartDate" runat="server" EnableViewState="false" Text="Common Leave Year Start Date" />
                    </td>
                    <td align="left" class="pm_field">
                        <asp:TextBox ID="LeavePlanCommonLeaveYearStartDay" runat="server" CssClass="pm_required"/>
                        <asp:DropDownList ID="LeavePlanCommonLeaveYearStartMonth" runat="server" CssClass="pm_required"/>
                    </td>
                </tr>
                <tr id="CommonLeaveYearRow2" runat="server" >
                    <td class="pm_field_header">
                        <asp:Label ID="Label3" runat="server" EnableViewState="false" Text="Year of Service Calculation Method" />:
                    </td>
                    <td class="pm_field">
                        <asp:DropDownList ID="LeavePlanNoCountFirstIncompleteYearOfService" runat="server" CssClass="pm_required" />
                    </td>
                </tr>
                <tr id="ALProrataRoundingRuleRow" runat="server" >
                    <td class="pm_field_header">
                        <asp:Label ID="Label5" runat="server" EnableViewState="false" Text="Annual Leave Prorata Rounding Rule" />:</td>
                    <td class="pm_field">
                        <asp:DropDownList ID="ALProrataRoundingRuleID" runat="Server" />
                        <asp:CheckBox ID="LeavePlanALRoundingRuleIsApplyFirstYearBroughtForwardOnly" runat="server" EnableViewState="false" Text="Apply on first year brought forward only" />
                    </td>
                </tr>
                <%-- Start 0000014, Miranda, 2014-04-02 --%>
                <tr id="ResetYearOfServiceRow" runat="server" >
                    <td class="pm_field_header">
                        <asp:Label runat="server" EnableViewState="false" Text="Reset Year of Service When Changed Employee Leave Plan" />:</td>
                    <td class="pm_field">
                        <asp:CheckBox ID="LeavePlanResetYearOfService" runat="Server" /></td>
                </tr>
                <tr>
                    <td class="pm_field_header">
                        <asp:Label ID="Label8" runat="server" EnableViewState="false" Text="Do not include Feb 29 when calculating prorata" />:</td>
                    <td class="pm_field">
                        <asp:CheckBox ID="LeavePlanProrataSkipFeb29" runat="Server" /></td>
                </tr>
                <%-- End 0000014, Miranda, 2014-04-02 --%>
                <tr id="ComparePreviousLeavePlanRow" runat="server">
                    <td class="pm_field_header">
                        <asp:Label ID="Label4" runat="server" EnableViewState="false" Text="Compare previous leave plan enrolled" />:
                    </td>
                    <td class="pm_field">
                        <asp:CheckBox ID="LeavePlanComparePreviousLeavePlan" runat="Server" AutoPostBack="true" />
                    </td>
                </tr>
                <tr id="LeavePlanComparePreviousLeavePlanOptionPanel" runat="server" >
                    <td class="pm_field_header">
                        <asp:Label ID="Label6" runat="server" EnableViewState="false" Text="Leave Plan Rank Level" />:
                    </td>
                    <td class="pm_field">
                        <asp:TextBox ID="LeavePlanLeavePlanCompareRank" runat="Server" />
                    </td>
                </tr>
                <asp:PlaceHolder ID="RestDayStatutoryHolidayPanel" runat="server" >
                <tr>
                    <td class="pm_field_title" colspan="2">
                        <asp:Label runat="server" EnableViewState="false" Text="Statutory Holiday Options" />
                    </td>
                </tr>
                <tr>
                    <td class="pm_field_header">
                        <asp:Label ID="Label9" runat="server" EnableViewState="false" Text="Enable Statutory Holiday Entitlement in Employee Leave Balance" />:</td>
                    <td class="pm_field">
                        <asp:CheckBox ID="LeavePlanUseStatutoryHolidayEntitle" runat="Server" />
                    </td>
                </tr>
                <tr>
                    <td class="pm_field_title" colspan="2">
                        <asp:Label ID="Label7" runat="server" EnableViewState="false" Text="Public Holiday Options" />
                    </td>
                </tr>
                <tr>
                    <td class="pm_field_header">
                        <asp:Label ID="Label18" runat="server" EnableViewState="false" Text="Enable Public Holiday Entitlement in Employee Leave Balance" />:</td>
                    <td class="pm_field">
                        <asp:CheckBox ID="LeavePlanUsePublicHolidayEntitle" runat="Server" />
                    </td>
                </tr>
                <tr>
                    <td class="pm_field_title" colspan="2">
                        <asp:Label ID="Label13" runat="server" EnableViewState="false" Text="Rest Day Options" />
                    </td>
                </tr>
                <tr>
                    <td class="pm_field_header">
                        <asp:Label ID="Label10" runat="server" EnableViewState="false" Text="Enable Rest Day Entitlement in Employee Leave Balance" />:</td>
                    <td class="pm_field">
                        <asp:CheckBox ID="LeavePlanUseRestDayEntitle" runat="Server" />
                    </td>
                </tr>
                <tr>
                    <td class="pm_field_header">
                        <asp:Label ID="Label11" runat="server" EnableViewState="false" Text="Number of Rest Day Entitled" />:
                    </td>
                    <td class="pm_field">
                        <asp:TextBox ID="LeavePlanRestDayEntitleDays" runat="Server" />&nbsp&nbsp<asp:Label ID="Label17" runat="server" EnableViewState="false" Text="day(s)" />&nbsp&nbsp<asp:DropDownList ID="LeavePlanRestDayEntitlePeriod" runat="Server" AutoPostBack="true" />
                    </td>
                </tr>
                <tr id="RestDayWeeklyOption" runat="server">
                    <td class="pm_field_header">
                        <asp:Label ID="Label12" runat="server" EnableViewState="false" Text="Week to gain Rest Day for Weekly Entilement" />:</td>
                    <td class="pm_field">
                        <asp:DropDownList ID="LeavePlanRestDayWeeklyEntitleStartDay" runat="Server" />
                    </td>
                </tr>
                <tr id="RestDayMonthlyOption1" runat="server">
                    <td class="pm_field_header">
                        <asp:Label ID="Label14" runat="server" EnableViewState="false" Text="Number of day for prorata calculation" />:</td>
                    <td class="pm_field">
                        <asp:TextBox ID="LeavePlanRestDayMonthlyEntitleProrataBase" runat="Server" />
                        (0 = <asp:Label ID="Label15" runat="server" EnableViewState="false" Text="Use Calendar Days"/>)
                    </td>
                </tr>
                <tr id="RestDayMonthlyOption2" runat="server">
                    <td class="pm_field_header">
                        <asp:Label ID="Label16" runat="server" EnableViewState="false" Text="Rounding Rule" />:</td>
                    <td class="pm_field">
                        <asp:DropDownList ID="LeavePlanRestDayMonthlyEntitleProrataRoundingRuleID" runat="Server" /></td>
                </tr>
                </asp:PlaceHolder> 
            </table>
        </ContentTemplate>
        </asp:UpdatePanel >
</asp:Content> 