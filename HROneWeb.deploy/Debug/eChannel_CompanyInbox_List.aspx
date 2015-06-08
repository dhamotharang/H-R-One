<%@ page language="C#" autoeventwireup="true" inherits="eChannel_CompanyInbox_List, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>

<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
<%@ Register Src="~/controls/eChannel_Inbox_List.ascx" TagName="eChannel_Inbox_List" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="ILabel1" Text="Notification" runat="server" />
                </td>
            </tr>
        </table>
        <table class="pm_section" width="100%">
            <tr >
                <td colspan="2">
                    <asp:Label ID="Label2" runat="server" Text="BANKFILE_REPORT_STORE_PERIOD_MESSAGE" ForeColor="red" />
                </td>
            </tr>
        </table> 
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="ILabel2" Text="Detail" runat="server" />
                </td>
            </tr>
        </table>
        
            
                
        <tb:eChannel_Inbox_List ID="eChannel_Inbox_List1" runat="server" />
</asp:Content> 