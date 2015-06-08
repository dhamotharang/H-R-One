<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RosterCode_OTRatioList.ascx.cs" Inherits="controls_RosterCode_OTRatioList" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
        <input type="hidden" id="RosterCodeID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="Label21" Text="Overtime Ratio" runat="server" />
                </td>
            </tr>
        </table>
        
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <asp:Button ID="Delete" CssClass="button" runat="server" Text="Delete" OnClick="Delete_Click" />
                </td>
            </tr>
        </table>


        <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
            <col width="26px" /> 
            <col width="120px" /> 
            <col width="150px" /> 
            <tr>
                <td class="pm_list_header">
                </td>
                <td class="pm_list_header">
                </td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_RosterCodeDetailNoOfHour" OnClick="ChangeOrder_Click"
                        Text="No Of Hour" />
                    <asp:Label ID="Label41" runat="server" Text="&le;" />
                </td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_RosterCodeDetailRate" OnClick="ChangeOrder_Click"
                        Text="Rate" />
                </td>

            </tr>
            <tr id="AddPanel" runat="server" >
                <td class="pm_list" >
                </td>
                <td class="pm_list" align="center">
                    <asp:Button ID="Add" runat="server" Text="Add" CssClass="button" OnClick="Add_Click" /></td>
                <td class="pm_list" align="left">
                    <asp:TextBox ID="RosterCodeDetailNoOfHour" runat="server" /></td>
                <td class="pm_list" align="left">
                    <asp:TextBox ID="RosterCodeDetailRate" runat="server" /></td>
            </tr>
            <asp:DataList ID="Repeater" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal"
                ShowFooter="false" ShowHeader="false" OnItemCommand="Repeater_ItemCommand" OnItemDataBound="Repeater_ItemDataBound">
                <EditItemTemplate>
                    <tr>
                        <td class="pm_list" align="center" >
                            <asp:CheckBox ID="DeleteItem" runat="server" />
                            <input type="hidden" runat="server" id="RosterCodeDetailID" />
                        </td>
                        <td class="pm_list" align="center" >
                            <asp:Button ID="Save" runat="server" Text="Save" CssClass="button" />
                            <asp:Button ID="Cancel" runat="server" Text="Cancel" CssClass="button" />
                        </td>
                        <td class="pm_list" align="left">
                            <asp:TextBox ID="RosterCodeDetailNoOfHour" runat="server" />
                        </td>
                        <td class="pm_list" align="left">
                            <asp:TextBox ID="RosterCodeDetailRate" runat="server" />
                        </td>
                    </tr>
                </EditItemTemplate>
                <ItemTemplate>
                  <tr>
                        <td class="pm_list" align="center" >
                            <asp:CheckBox ID="DeleteItem" runat="server" />
                            <input type="hidden" runat="server" id="RosterCodeDetailID" />
                        </td>
                        <td class="pm_list" align="center">
                            <asp:Button ID="Edit" runat="server" Text="Edit" CssClass="button" />
                        </td>
                        <td class="pm_list" align="left">
                            <%#sbinding.getFValue(Container.DataItem,"RosterCodeDetailNoOfHour","0.00")%>
                        </td>
                        <td class="pm_list" align="left">
                            <%#sbinding.getFValue(Container.DataItem,"RosterCodeDetailRate","0.00")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:DataList>
            </table>
        <tb:RecordListFooter id="ListFooter" runat="server"
             ShowAllRecords="true" ListOrderBy="RosterCodeDetailNoOfHour" ListOrder="true" 
          />
