<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StatutoryMinimumWage.aspx.cs" Inherits="StatutoryMinimumWage" MasterPageFile="~/MainMasterPage.master" %>

<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
		<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Statutory Minimum Wage Setup" />
                </td>
            </tr>
        </table>
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar id="toolBar" runat="server"
                     NewButton_Visible="false" 
                     EditButton_Visible="false" 
                     BackButton_Visible="false"
                     SaveButton_Visible ="false"
                     OnDeleteButton_Click="Delete_Click"
                      />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
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
                    <col width="200px" /> 
                    <tr>
                        <td class="pm_list_header">
                        </td>
                        <td class="pm_list_header">
                        </td>
                        <td align="left" class="pm_list_header">
                            <asp:LinkButton runat="server" ID="_MinimumWageEffectiveDate" OnClick="ChangeOrder_Click" Text="Effective Date"/></td>
                        <td align="left" class="pm_list_header">
                            <asp:LinkButton runat="server" ID="_MinimumWageHourlyRate" OnClick="ChangeOrder_Click" Text="Hourly Rate"/></td>
                    </tr>
                    <tr id="AddPanel" runat="server">
                        <td class="pm_list">
                        </td>
                        <td class="pm_list" align="center">
                            <asp:Button ID="Add" runat="server" Text="Add" CssClass="button" OnClick="Add_Click" /></td>
                        <td class="pm_list">
                            <uc1:WebDatePicker id="MinimumWageEffectiveDate" runat="server" />
                        </td>
                            
                        <td class="pm_list">
                            <asp:TextBox ID="MinimumWageHourlyRate" runat="server" /></td>
                    </tr>
                    <asp:DataList ID="Repeater" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" ShowFooter="false"  
                        ShowHeader="false" OnItemCommand="Repeater_ItemCommand" OnItemDataBound="Repeater_ItemDataBound">
                        <EditItemTemplate>
                            <tr>
                                <td class="pm_list" align="center">
                                    <asp:CheckBox ID="DeleteItem" runat="server" />
                                    <input type="hidden" runat="server" id="MinimumWageID" />
                                </td>
                                <td class="pm_list" align="center">
                                    <asp:Button ID="Save" runat="server" Text="Save" CssClass="button" />
                                    <asp:Button ID="Cancel" runat="server" Text="Cancel" CssClass="button"/>
                                </td>
                                <td class="pm_list">
                                    <uc1:WebDatePicker id="MinimumWageEffectiveDate" runat="server" />
                                </td>
                                <td class="pm_list">
                                    <asp:TextBox ID="MinimumWageHourlyRate" runat="server" />
                                </td>
                            </tr>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <tr>
                                <td class="pm_list" align="center">
                                    <asp:CheckBox ID="DeleteItem" runat="server" />
                                    <input type="hidden" runat="server" id="MinimumWageID" />
                                </td>
                                <td class="pm_list" align="center">
                                    <asp:Button ID="Edit" runat="server" CssClass="button" Text="Edit" />
                                </td>
                                <td class="pm_list">
                                    <%#sbinding.getFValue(Container.DataItem, "MinimumWageEffectiveDate", "yyyy-MM-dd")%>
                                </td>
                                <td class="pm_list">
                                    <%#sbinding.getFValue(Container.DataItem, "MinimumWageHourlyRate","#,##0.00")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:DataList>
                </table>
                <tb:RecordListFooter id="ListFooter" runat="server"
                    ShowAllRecords="true" visible="true" 
                    ListOrderBy="MinimumWageEffectiveDate"
                    ListOrder="false" 
                  />
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content> 