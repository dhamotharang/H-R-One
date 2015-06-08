<%@ Page Language="C#" MasterPageFile="~/MainMasterPage.master" AutoEventWireup="true" CodeFile="EmailLog_List.aspx.cs" Inherits="EmailLog_List" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" Runat="Server">
    <asp:Button ID="Back" runat="server" Text="- Back -" OnClick="Back_Click" CSSClass="button" />
    <table border="0" width="100%" class="pm_section_title" cellspacing="0" cellpadding="1">
        <tr>
            <td>
                <asp:Label ID="Label2" Text="E-mail History Records" runat="server" />:
            </td>
        </tr>
    </table>
    <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
        <col width="26px" />
        <col width="100px" />
        <col width="100px" />
        <col  />
        <col width="100px" />
        <tr>
            <td class="pm_list_header">
            </td>
            <td align="left" class="pm_list_header">
                <asp:LinkButton runat="server" ID="_EmailLogStartTime" OnClick="ChangeOrder_Click" Text="Start Time" />
            </td>
            <td align="left" class="pm_list_header">
                <asp:LinkButton runat="server" ID="_EmailLogEndTime" OnClick="ChangeOrder_Click" Text="End Time" />
            </td>
            <td align="left" class="pm_list_header">
                <asp:LinkButton runat="server" ID="_EmailLogToAddress" OnClick="ChangeOrder_Click" Text="Email Recipient" />
            </td>
            <td align="left" class="pm_list_header">
                <asp:LinkButton runat="server" ID="_EmailLogTrialCount" OnClick="ChangeOrder_Click" Text="Trial Count" />
            </td>
            <td align="left" class="pm_list_header">
                <asp:Label runat="server" ID="_EmailLogErrorMessage" Text="Message" />
            </td>
        </tr>
        <asp:DataList ID="Repeater" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" ShowFooter="false"
            ShowHeader="false" >
            <ItemTemplate>
                <tr>
                    <td class="pm_list" align="center" >
                    </td>
                    <td class="pm_list">
                        <%#sbinding.getFValue(Container.DataItem, "EmailLogStartTime", "yyyy-MM-dd HH:mm:ss")%>
                    </td>
                    <td class="pm_list">
                        <%#sbinding.getFValue(Container.DataItem, "EmailLogEndTime", "yyyy-MM-dd HH:mm:ss")%>
                    </td>
                    <td class="pm_list">
                        <%#sbinding.getValue(Container.DataItem, "EmailLogToAddress")%>
                    </td>
                    <td class="pm_list" align="right" >
                        <%#sbinding.getValue(Container.DataItem, "EmailLogTrialCount")%>
                    </td>
                    <td class="pm_list" align="right" >
                        <%#sbinding.getValue(Container.DataItem, "EmailLogErrorMessage")%>
                    </td>
            </ItemTemplate>
        </asp:DataList>
    </table>
    <table width="100%" cellspacing="0" cellpadding="0" class="pm_list_pagenav">
        <tr>
            <td align="right">
                <tb:RecordListFooter id="ListFooter" runat="server"
                    OnFirstPageClick="ChangePage"
                    OnPrevPageClick="ChangePage"
                    OnNextPageClick="ChangePage"
                    OnLastPageClick="ChangePage"
                  />
            </td>
        </tr>
    </table>
</asp:Content>

