<%@ Page Language="C#" MasterPageFile="~/MainMasterPage.master" AutoEventWireup="true" CodeFile="UserPreference.aspx.cs" Inherits="UserPreference"  %>

<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional"  >
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="Save" EventName="Click" />
    </Triggers>
    <ContentTemplate >
		<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="User Preference" />
                </td>
            </tr>
        </table>
        <table width="80%" cellspacing="0" class="pm_section">
            <col width="20%" />
            <tr>
                <td class="pm_field_header" align="center" >
                    <asp:Label ID="lblLanguage" runat="server" Text="Language" />
                </td>
                <td class="pm_field">
                    <asp:DropDownList ID="cboLanguage" runat="server" />
                </td>
            </tr>
            <tr id="UserIsKeepConnectedRow" runat="server" >
                <td class="pm_field_header" align="center">
                    <asp:Label ID="Label5" runat="server" Text="Keep Connected"  />
                </td>
                <td class="pm_field">
                    <asp:CheckBox ID="UserIsKeepConnected" runat="server" />
                </td>
            </tr>
        </table>  
        <table width="80%" cellspacing="0" class="pm_section">
            <col width="26px" />
            <col width="300px" />
            <col width="120px" />
            <col width="120px" />
            <tr>
                <td class="pm_field_title" colspan="4">
                    <asp:Label ID="Label1" runat="server" Text="Reminder" /></td>
            </tr>
            <tr>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=ReminderRepeater.ClientID %>','ItemSelect',this.checked);" />
                </td>
                <td class="pm_field_header" align="center">
                    <asp:Label ID="ReminderTypeHeader" runat="server" Text="Reminder Type" />
                </td>
                <td class="pm_field_header" align="center">
                    <asp:Label ID="Label3" runat="server" Text="Remind Day(s) Before" />
                </td>
                <td class="pm_field_header" align="center">
                    <asp:Label ID="Label4" runat="server" Text="Remind Day(s) After" />
                </td>
            </tr>
            <asp:Repeater ID="ReminderRepeater" runat="server" OnItemDataBound="ReminderRepeater_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_field">
                            <asp:CheckBox ID="ItemSelect" runat="server" />
                        </td>
                        <td class="pm_field">
                            <asp:Label ID="ReminderTypeDesc" runat="server" Text="" />
                        </td>
                        <td class="pm_field" align="right">
                            <asp:TextBox ID="UserReminderOptionRemindDaysBefore" runat="server" />
                        </td>
                        <td class="pm_field" align="right">
                            <asp:TextBox ID="UserReminderOptionRemindDaysAfter" runat="server" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <tb:RecordListFooter id="ListFooter" runat="server"
            ShowAllRecords="true" Visible="false" 
          />
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td align="left" >
                <asp:Button ID="Save" Text="Save" runat="server" OnClick="Save_Click" cssclass="button"/>
                </td>
                <td align="right">
                </td>
            </tr>
        </table>
    </ContentTemplate> 
    </asp:UpdatePanel> 
</asp:Content>

