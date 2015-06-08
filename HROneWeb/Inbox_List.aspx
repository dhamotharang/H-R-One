<%@ Page Language="C#" MasterPageFile="~/MainMasterPage.master" AutoEventWireup="true" CodeFile="Inbox_List.aspx.cs" Inherits="Inbox_List" %>

<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="Server">
    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Inbox" />
            </td>
        </tr>
    </table>
    <table width="100%" class="pm_section">
    <col width="15%" />
    <col width="85%" />
    <tr>
        <td class="pm_search_header" >
            <asp:Label ID="Label3" runat="server" Text="Total Size" />:
        </td>
        <td class="pm_search" >
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always" >
            <ContentTemplate >
            <asp:Label ID="lblTotalSize" runat="server"  />
            </ContentTemplate> 
            </asp:UpdatePanel>
        </td>
    </tr>
    </table> 
    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" Text="Inbox Message List" />
            </td>
        </tr>
    </table>
    <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
        <tr>
            <td>
                <tb:detailtoolbar id="toolBar" runat="server" 
                    newbutton_visible="false" 
                    backbutton_visible="false" 
                    editbutton_visible="false" 
                    savebutton_visible="false" 
                    onnewbutton_click="New_Click" 
                    ondeletebutton_click="Delete_Click" 
                    CustomButton1_Name="Refresh"
                    CustomButton1_Visible="true"
                    OnCustomButton1_Click="Search_Click"
                />
            </td>
            <td align="right">
                <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openAlert(); return false;" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional" >
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="toolbar"/>
    </Triggers>
    <ContentTemplate >
    <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
        <col width="26px" />
        <col width="150px" />
        <col width="150px" />
        <tr>
            <td class="pm_list_header" align="center">
                <asp:Panel ID="SelectAllPanel" runat="server">
                    <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
                </asp:Panel>
            </td>
            <td align="left" class="pm_list_header">
                <asp:LinkButton runat="server" ID="_InboxFromUsername" OnClick="ChangeOrder_Click" Text="From" /></td>
            <td align="left" class="pm_list_header">
                <asp:LinkButton runat="server" ID="_InboxCreateDate" OnClick="ChangeOrder_Click" Text="Date" /></td>
            <td align="left" class="pm_list_header">
                <asp:LinkButton runat="server" ID="_InboxSubject" OnClick="ChangeOrder_Click" Text="Subject" /></td>
        </tr>
        <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
            <ItemTemplate>
                <tr>
                    <td class="pm_list" align="center">
                        <asp:CheckBox ID="ItemSelect" runat="server" />
                    </td>
                    <td class="pm_list">
                        <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Inbox_Detail.aspx?InboxID=" + binding.getValue(Container.DataItem,"InboxID"))%>">
                            <asp:Label ID="InboxFromUserName" runat="server" />
                        </a>
                    </td>
                    <td class="pm_list">
                        <asp:Label ID="InboxCreateDate" runat="server" />
                    </td>
                    <td class="pm_list">
                        <asp:Label ID="InboxSubject" runat="server" />
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
    <tb:RecordListFooter id="ListFooter" runat="server"
        OnFirstPageClick="FirstPage_Click"
        OnPrevPageClick="PrevPage_Click"
        OnNextPageClick="NextPage_Click"
        OnLastPageClick="LastPage_Click"
        ListOrderBy="InboxCreateDate"
        ListOrder="false" 
    />
    </ContentTemplate> 
    </asp:UpdatePanel>
</asp:Content>
