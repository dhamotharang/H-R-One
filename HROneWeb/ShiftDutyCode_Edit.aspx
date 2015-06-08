<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShiftDutyCode_Edit.aspx.cs" Inherits="ShiftDutyCode_Edit" MasterPageFile="~/MainMasterPage.master"  %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="ShiftDutyCodeID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label Text="Shift Duty Code" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <%=CurID<=0?HROne.Common.WebUtility.GetLocalizedString("Add"):HROne.Common.WebUtility.GetLocalizedString("Edit") %>
                    <asp:Label Text="Shift Duty Code" runat="server" />:
                    <%=ShiftDutyCode.Text%>
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
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="50%" />
            <col width="50%" />
    		<tr>
                <td class="pm_field_title"  colspan="2">
                   <asp:Label runat="server" Text="Shift Duty Code" /></td>
			</tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label Text="Code" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="ShiftDutyCode" runat="Server" /></td>
			</tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label Text="Description" runat="server" />:
                </td>
                <td class="pm_field">
                    <asp:TextBox ID="ShiftDutyCodeDesc" runat="Server" />
                </td>
            </tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label Text="Shift Duty From Time" runat="server" />:
                </td>
                <td class="pm_field">
                    <asp:TextBox ID="ShiftDutyFromTime" runat="Server" />
                </td>  
            </tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label Text="Shift Duty To Time" runat="server" />:
                </td>
                <td class="pm_field">
                    <asp:TextBox ID="ShiftDutyToTime" runat="Server" />
                </td>
            </tr>
		</table>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
        </asp:UpdatePanel >
</asp:Content> 