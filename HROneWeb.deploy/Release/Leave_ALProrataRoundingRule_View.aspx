<%@ page language="C#" autoeventwireup="true" inherits="Leave_ALProrataRoundingRule_View, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>

<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="ALProrataRoundingRuleID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label Text="Annual Leave Prorata Rounding Rule Setup" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label Text="View Annual Leave Prorata Rounding Rule" runat="server" />:
                    <%=ALProrataRoundingRuleCode.Text%>
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
                     OnDeleteButton_Click="Delete_ClickTop"
                      />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
                </td>
            </tr>
        </table>

        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="30%" />
            <col width="70%" />
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Code" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="ALProrataRoundingRuleCode" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Description" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="ALProrataRoundingRuleDesc" runat="Server" /></td>
            </tr>
        </table>
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td align="right">
                </td>
            </tr>
        </table>
        <br />
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <asp:Button ID="Delete" CssClass="button" runat="server" Text="Delete" OnClick="Delete_Click" />
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Delete" EventName="Click"/>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
        <ContentTemplate >
        <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
            <col width="26px" /> 
            <col width="120px" /> 
            <col width="150px" /> 
            <tr>
                <td class="pm_list_header" >
                </td>
                <td class="pm_list_header" >
                </td>
                <td align="center" class="pm_list_header" >
                    <asp:Label runat="server" ID="_ALProrataRoundingRuleDetailRangeTo" Text="&le;" />
                </td>
                <td align="center" class="pm_list_header" >
                    <asp:Label runat="server" ID="_ALProrataRoundingRuleDetailRoundTo" Text="Round to"/>
                </td>
            </tr>
            <tr id="AddPanel" runat="server" >
                <td class="pm_list" >
                </td>
                <td class="pm_list" align="center" >
                    <asp:Button ID="Add" runat="server" Text="Add" CssClass="button" OnClick="Add_Click" /></td>
                <td class="pm_list" align="right">
                    <asp:TextBox ID="ALProrataRoundingRuleDetailRangeTo" runat="server" /></td>
                <td class="pm_list" align="right">
                    <asp:TextBox ID="ALProrataRoundingRuleDetailRoundTo" runat="server"/></td>
            </tr>
            <asp:DataList ID="Repeater" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal"
                ShowFooter="false" ShowHeader="false" OnItemCommand="Repeater_ItemCommand" OnItemDataBound="Repeater_ItemDataBound">
                <EditItemTemplate>
                    <tr>
                        <td class="pm_list" align="center" >
                            <asp:CheckBox ID="DeleteItem" runat="server" />
                            <input type="hidden" runat="server" id="ALProrataRoundingRuleDetailID" />
                        </td>
                        <td class="pm_list" align="center" >
                            <asp:Button ID="Save" runat="server" Text="Save" CssClass="button" />
                            <asp:Button ID="Cancel" runat="server" Text="Cancel" CssClass="button" />
                        </td>
                        <td class="pm_list" align="right">
                            <asp:TextBox ID="ALProrataRoundingRuleDetailRangeTo" runat="server" style="text-align:right;" /></td>
                        <td class="pm_list" align="right">
                            <asp:TextBox ID="ALProrataRoundingRuleDetailRoundTo" runat="server" style="text-align:right;"/></td>
                    </tr>
                </EditItemTemplate>
                <ItemTemplate>
                    <tr>
                        <td class="pm_list" align="center" >
                            <asp:CheckBox ID="DeleteItem" runat="server" />
                            <input type="hidden" runat="server" id="ALProrataRoundingRuleDetailID" />
                        </td>
                        <td class="pm_list" align="center" >
                            <asp:Button ID="Edit" runat="server" Text="Edit" CssClass="button" />
                        </td>
                        <td class="pm_list" align="right">
                            <%#sbinding.getFValue(Container.DataItem, "ALProrataRoundingRuleDetailRangeTo", "0.####")%>
                        </td>
                        <td class="pm_list" align="right">
                            <%#sbinding.getFValue(Container.DataItem, "ALProrataRoundingRuleDetailRoundTo", "0.####")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:DataList>
            </table>
            <tb:RecordListFooter id="ListFooter" runat="server"
                ShowAllRecords="true" ListOrderBy="ALProrataRoundingRuleDetailRangeTo" ListOrder="true" 
              />
            </ContentTemplate>
        </asp:UpdatePanel >
</asp:Content> 