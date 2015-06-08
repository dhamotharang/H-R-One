<%@ page language="C#" autoeventwireup="true" inherits="Customize_AttendancePreparationProcess_Edit, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="AttendancePreparationProcessID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label Text="Attendance Preparation Process Setup" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <%=CurID<=0?HROne.Common.WebUtility.GetLocalizedString("Add"):HROne.Common.WebUtility.GetLocalizedString("Edit") %>
                    <asp:Label Text="Attendance Preparation Process " runat="server" />:
                    <%=AttendancePreparationProcessDesc.Text%>
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
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="30%" />
            <col width="70%" />
    		<tr>
                <td class="pm_field_title"  colspan="2">
                   <asp:Label runat="server" Text="Basic Information" />
                </td>
			</tr>
			<tr>
                <td class="pm_field_header" >
                    <asp:Label Text="Month" runat="server"/>:
                </td>
                <td class="pm_field">
                    <uc1:WebDatePicker id="AttendancePreparationProcessMonth" runat="server"  OnChanged="AttendancePreparationProcessMonth_Changed" AutoPostBack="true" />
                </td>
			</tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label Text="Description" runat="server"/>:
                </td>
                <td class="pm_field">
                    <asp:TextBox ID="AttendancePreparationProcessDesc" runat="Server" />
                </td>
            </tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label Text="Cover Period" runat="server"/>:
                </td>
                <td class="pm_field">
                    <uc1:WebDatePicker id="AttendancePreparationProcessPeriodFr" runat="server" />&nbsp&nbsp to &nbsp&nbsp
                    <uc1:WebDatePicker id="AttendancePreparationProcessPeriodTo" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Status" runat="server"/>:
                </td>
                <td class="pm_field">
                    <asp:Label ID="AttendancePreparationProcessStatus" runat="server" />-
                    <asp:Label ID="AttendancePreparationProcessStatusDesc" runat="server" />
                </td>
			</tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label Text="Payment Date" runat="server"/>:
                </td>
                <td class="pm_field">
                    <uc1:WebDatePicker id="AttendancePreparationProcessPayDate" runat="server" />
                </td>
            </tr>
		</table>
    </ContentTemplate> 
    </asp:UpdatePanel> 
</asp:Content> 