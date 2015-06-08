<%@ control language="C#" autoeventwireup="true" inherits="Attendance_TimeCardLocationMappingControl, HROneWeb.deploy" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>

<table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
    <tr>
        <td>
            <tb:DetailToolBar ID="toolBar" runat="server" NewButton_Visible="false" EditButton_Visible="false" BackButton_Visible="false" SaveButton_Visible="false" OnDeleteButton_Click="Delete_Click" />
        </td>
        <td align="right">
            <asp:Button ID="btnHelp" runat="server" CssClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;" />
        </td>
    </tr>
</table>
<table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
<col width="26px" /> 
<col width="120px" /> 
<col width="150px" /> 
<tr>
    <td class="pm_list_header">
        <asp:Panel ID="SelectAllPanel" runat="server">
        <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','DeleteItem',this.checked);" />
        </asp:Panel> 
    </td>
    <td class="pm_list_header">
    </td>
    <td align="left" class="pm_list_header">
        <asp:LinkButton runat="server" ID="_TimeCardLocationMapOriginalCode" OnClick="ChangeOrder_Click" Text="Location Name" /></td>
    <td align="left" class="pm_list_header">
        <asp:LinkButton runat="server" ID="_TimeCardLocationMapNewCode" OnClick="ChangeOrder_Click" Text="Mapped to" /></td>
</tr>
<tr id="AddPanel" runat="server">
    <td class="pm_list">
    </td>
    <td class="pm_list" align="center">
        <asp:Button ID="Add" runat="server" Text="Add" CssClass="button" OnClick="Add_Click" /></td>
    <td class="pm_list">
        <asp:TextBox ID="TimeCardLocationMapOriginalCode" runat="server" /></td>
    <td class="pm_list">
        <asp:TextBox ID="TimeCardLocationMapNewCode" runat="server" /></td>
</tr>
<asp:DataList ID="Repeater" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" ShowFooter="false" ShowHeader="false" OnItemCommand="Repeater_ItemCommand" OnItemDataBound="Repeater_ItemDataBound">
    <EditItemTemplate>
        <tr>
            <td class="pm_list" align="center">
                <asp:CheckBox ID="DeleteItem" runat="server" />
                <input type="hidden" runat="server" id="TimeCardLocationMapID" />
            </td>
            <td class="pm_list" align="center">
                <asp:Button ID="Save" runat="server" Text="Save" CssClass="button" />
                <asp:Button ID="Cancel" runat="server" Text="Cancel" CssClass="button" />
            </td>
            <td class="pm_list">
                <asp:TextBox ID="TimeCardLocationMapOriginalCode" runat="server" />
            </td>
            <td class="pm_list">
                <asp:TextBox ID="TimeCardLocationMapNewCode" runat="server" />
            </td>
        </tr>
    </EditItemTemplate>
    <ItemTemplate>
        <tr>
            <td class="pm_list" align="center">
                <asp:CheckBox ID="DeleteItem" runat="server" />
                <input type="hidden" runat="server" id="TimeCardLocationMapID" />
            </td>
            <td class="pm_list" align="center">
                <asp:Button ID="Edit" CssClass="button" runat="server" Text="Edit" />
            </td>
            <td class="pm_list">
                <%#sbinding.getValue(Container.DataItem, "TimeCardLocationMapOriginalCode")%>
            </td>
            <td class="pm_list">
                <%#sbinding.getValue(Container.DataItem, "TimeCardLocationMapNewCode")%>
            </td>
        </tr>
    </ItemTemplate>
</asp:DataList>
</table>
<tb:RecordListFooter ID="ListFooter" runat="server" ShowAllRecords="true" />
