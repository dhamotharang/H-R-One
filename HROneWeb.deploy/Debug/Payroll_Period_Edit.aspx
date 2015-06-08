<%@ page language="C#" autoeventwireup="true" inherits="Payroll_Period_Edit, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>

<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <input type="hidden" id="PayPeriodID" runat="server" name="ID" />
    <input type="hidden" id="HiddenPayGroupID" runat="server" name="ID" />
	<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
		<tr>
			<td>
				<asp:Label runat="server" Text="Payroll Group" />
			</td>
		</tr>
	</table>
	<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
        <tr>
            <td>
                <%=CurID<=0?HROne.Common.WebUtility.GetLocalizedString("Add"):HROne.Common.WebUtility.GetLocalizedString("Edit") %>
                <asp:Label runat="server" Text="Payroll Cycle" />:
            </td>
        </tr>
    </table>
    <table width="100%" class="pm_section">
        <tr>
            <td valign="top">
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
                <input type="hidden" id="Hidden1" runat="server" name="ID" />
                <asp:CheckBox id="PayPeriodIsAutoCreate" runat="server"  Visible="false" />
                <table border="1" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
                    <col width="15%"/>
                    <col width="35%"/>
                    <col width="15%"/>
                    <col width="35%"/>
                    <tr>
                        <td class="pm_field_header" >
                            <asp:Label runat="server" Text="Payroll Group" />:
                        </td>
                        <td class="pm_field" colspan="3">
                            <asp:Label  ID="PayGroupID" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pm_field_header" >
                            <asp:Label runat="server" Text="From" />:
                        </td>
                        <td class="pm_field" >
                            <uc1:WebDatePicker id="PayPeriodFr" runat="server" />
                        </td>
                        <td class="pm_field_header" >
                            <asp:Label runat="server" Text="To" />:
                        </td>
                        <td class="pm_field" >
                            <uc1:WebDatePicker id="PayPeriodTo" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pm_field_header" >
                            <asp:Label runat="server" Text="Leave Cut off Date" />:
                        </td>
                        <td class="pm_field" colspan="3">
                            <uc1:WebDatePicker id="PayPeriodLeaveCutOffDate" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pm_field_header" >
                            <asp:Label runat="server" Text="Attendance From" />:
                        </td>
                        <td class="pm_field" >
                            <uc1:WebDatePicker id="PayPeriodAttnFr" runat="server" />
                        </td>
                        <td class="pm_field_header" >
                            <asp:Label runat="server" Text="Attendance To" />:
                        </td>
                        <td class="pm_field" >
                            <uc1:WebDatePicker id="PayPeriodAttnTo" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="toolbar"/>
    </Triggers>
    </asp:UpdatePanel >
</asp:Content> 