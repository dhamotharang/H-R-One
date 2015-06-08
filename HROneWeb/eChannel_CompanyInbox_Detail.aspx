<%@ Page Language="C#" MasterPageFile="~/MainMasterPage.master" AutoEventWireup="true" CodeFile="eChannel_CompanyInbox_Detail.aspx.cs" Inherits="eChannel_CompanyInbox_Detail"  %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" Runat="Server">
    <input type="hidden" id="CompanyInboxID" runat="server" name="ID" />
    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Inbox" />
            </td>
        </tr>
    </table>
    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
        <tr>
            <td>
                <asp:Label runat="server" Text="Message Detail"/>
            </td>
        </tr>
    </table>
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar id="toolBar" runat="server"
                     NewButton_Visible="false" 
                     SaveButton_Visible="false" 
                     EditButton_Visible="false"
                     DeleteButton_Visible="false" 
                     OnBackButton_Click="Back_Click"
                      />
                </td>
            </tr>
        </table>
    <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_field_section">
        <colgroup>
        <col width="15%" />
        <col width="35%" />
        <col width="15%" />
        <col width="35%" />
        </colgroup>
        <tr>
            <td class="pm_field_header" >
                <asp:Label ID="Label6" runat="server" Text="Date" />:
            </td>
            <td class="pm_field" colspan="3">
                <asp:Label ID="CompanyInboxCreateDate" runat="Server" />
            </td>
        </tr>
        <tr>
            <td class="pm_field_header" >
                <asp:Label ID="Label3" runat="server" Text="Subject" />:
            </td>
            <td class="pm_field" colspan="3">
                <asp:Label ID="CompanyInboxSubject" runat="Server" />
            </td>
        </tr>
        <tr id="AttachmentRow" runat="server" >
            <td class="pm_field_header" >
                <asp:Label ID="Label5" runat="server" Text="Attachment" />:
            </td>
            <td class="pm_field" colspan="3">
                <asp:Repeater ID="CompanyInboxAttachmentRepeater" runat="server" OnItemDataBound="CompanyInboxAttachmentRepeater_ItemDataBound" >
                <ItemTemplate>
                    <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "EChannel_CompanyInbox_Attachment_Download.aspx?CompanyInboxID=" + sbinding.getValue(Container.DataItem, "CompanyInboxID") + "&CompanyInboxAttachmentID=" + sbinding.getValue(Container.DataItem, "CompanyInboxAttachmentID"))%>">
                        <%#sbinding.getValue(Container.DataItem, "CompanyInboxAttachmentOriginalFileName")%>
                    </a>
                </ItemTemplate>
                <SeparatorTemplate>
                <br />
                </SeparatorTemplate>
                </asp:Repeater>
            </td>
        </tr>
        <tr>
            <td class="pm_field" colspan="4">
                <asp:Label ID="CompanyInboxMessage" runat="Server" />
            </td>
        </tr>
    </table>
</asp:Content>

