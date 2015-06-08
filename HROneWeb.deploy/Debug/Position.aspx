<%@ page language="C#" autoeventwireup="true" inherits="Position, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>

<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/Misccode_sel_LeftMenu.ascx" TagName="Misccode_sel_LeftMenu" TagPrefix="uc1" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_page_title">
        <tr>
            <td>
                <asp:Label Text="Miscellaneous Code Setup" runat="Server" /></td>
        </tr>
    </table>
    <table width="100%" cellspacing="0" cellpadding="0">
        <col width="10%" />
        <tr>
            <td valign="top" >
                <uc1:Misccode_sel_LeftMenu ID="Misccode_sel_LeftMenu1" runat="server" />
            </td>
            <td valign="top">
                <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_section_title">
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="Position" />
                        </td>
                    </tr>
                </table>
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
                <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="toolBar" />
                    </Triggers>
                    <ContentTemplate>
                        <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
                            <col width="26px" /> 
                            <col width="120px" /> 
                            <col width="150px" /> 
                            <tr>
                                <%-- Start 0000099, KuangWei, 2014-09-29 --%>
                                <td class="pm_list_header">
                                    <asp:Panel ID="SelectAllPanel" runat="server">
                                    <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','DeleteItem',this.checked);" />
                                    </asp:Panel>                                
                                </td>
                                <%-- End 0000099, KuangWei, 2014-09-29 --%>
                                <td class="pm_list_header">
                                </td>
                                <td align="left" class="pm_list_header">
                                    <asp:LinkButton runat="server" ID="_PositionCode" OnClick="ChangeOrder_Click" Text="Code" /></td>
                                <td align="left" class="pm_list_header">
                                    <asp:LinkButton runat="server" ID="_PositionDesc" OnClick="ChangeOrder_Click" Text="Description" /></td>
                                <td align="left" class="pm_list_header">
                                    <asp:LinkButton runat="server" ID="_PositionCapacity" OnClick="ChangeOrder_Click" Text="Position Capacity" /></td>
                            </tr>
                            <tr id="AddPanel" runat="server">
                                <td class="pm_list">
                                </td>
                                <td class="pm_list" align="center">
                                    <asp:Button ID="Add" runat="server" Text="Add" CssClass="button" OnClick="Add_Click" /></td>
                                <td class="pm_list">
                                    <asp:TextBox ID="PositionCode" runat="server" /></td>
                                <td class="pm_list">
                                    <asp:TextBox ID="PositionDesc" runat="server" /></td>
                                <td class="pm_list">
                                    <asp:TextBox ID="PositionCapacity" runat="server" /></td>
                            </tr>
                            <asp:DataList ID="Repeater" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" ShowFooter="false" ShowHeader="false" OnItemCommand="Repeater_ItemCommand" OnItemDataBound="Repeater_ItemDataBound">
                                <EditItemTemplate>
                                    <tr>
                                        <td class="pm_list" align="center">
                                            <asp:CheckBox ID="DeleteItem" runat="server" />
                                            <input type="hidden" runat="server" id="PositionID" />
                                        </td>
                                        <td class="pm_list" align="center">
                                            <asp:Button ID="Save" runat="server" Text="Save" CssClass="button" />
                                            <asp:Button ID="Cancel" runat="server" Text="Cancel" CssClass="button" />
                                        </td>
                                        <td class="pm_list">
                                            <asp:TextBox ID="PositionCode" runat="server" />
                                        </td>
                                        <td class="pm_list">
                                            <asp:TextBox ID="PositionDesc" runat="server" />
                                        </td>
                                        <td class="pm_list">
                                            <asp:TextBox ID="PositionCapacity" runat="server" />
                                        </td>
                                    </tr>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td class="pm_list" align="center">
                                            <asp:CheckBox ID="DeleteItem" runat="server" />
                                            <input type="hidden" runat="server" id="PositionID" />
                                        </td>
                                        <td class="pm_list" align="center">
                                            <asp:Button ID="Edit" CssClass="button" runat="server" Text="Edit" />
                                        </td>
                                        <td class="pm_list">
                                            <%#sbinding.getValue(Container.DataItem,"PositionCode")%>
                                        </td>
                                        <td class="pm_list">
                                            <%#sbinding.getValue(Container.DataItem,"PositionDesc")%>
                                        </td>
                                        <td class="pm_list">
                                            <%#sbinding.getValue(Container.DataItem,"PositionCapacity")%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:DataList>
                        </table>
                        <tb:RecordListFooter ID="ListFooter" runat="server" ShowAllRecords="true" Visible="true" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
