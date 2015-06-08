<%@ page language="C#" masterpagefile="~/MainMasterPage.master" autoeventwireup="true" inherits="eChannel_SubmitAutopayFile, HROneWeb.deploy" viewStateEncryptionMode="Always" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
<%@ Register Src="~/controls/eChannel_AutopayFile_List.ascx" TagName="eChannel_AutopayFile_List" TagPrefix="tb" %>
<%@ Register Src="~/controls/HSBCBankPaymentCode_List.ascx" TagName="HSBCBankPaymentCode_List" TagPrefix="tb" %>
<%@ Register Src="~/controls/eChannel_RemoteProfile_List.ascx" TagName="eChannel_RemoteProfile_List" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="ILabel1" Text="Submit Autopay File" runat="server" />
                </td>
            </tr>
        </table>

        <table class="pm_section" width="100%">
            <tr >
                <td colspan="2">
                    <asp:Label ID="Label2" runat="server" Text="BANKFILE_CONSOLIDATION_MESSAGE" ForeColor="red" />
                </td>
            </tr>
            <tr >
                <td colspan="2">
                    <asp:Label ID="Label3" runat="server" Text="BANKFILE_CLEARING_MESSAGE" ForeColor="red" />
                </td>
            </tr>
        </table> 
        <asp:PlaceHolder ID="UploadBankFilePanel" runat="server" >
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="Label4" Text="Remote Profile for HSBC/Hang Seng" runat="server" />
                </td>
            </tr>
        </table>
        <tb:eChannel_RemoteProfile_List ID="eChannel_RemoteProfile_List1" runat="server" />
        <br />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="ILabel2" Text="Registered Bank Payment Code" runat="server" />
                </td>
            </tr>
        </table>
        <tb:HSBCBankPaymentCode_List ID="HSBCBankPaymentCode_List1" runat="server" />
        <br />
        <table class="pm_section" width="100%">
            <tr>
                <td class="pm_search_header">
                    <asp:Label ID="Label1" runat="server" Text="Upload file path" />:</td>
                <td class="pm_search">
                    <asp:FileUpload ID="BankFileUpload" runat="server" Width="400" />
                    <asp:Button ID="Upload" runat="server" Text="Upload" OnClick="Upload_Click"  CssClass="button"/>
                </td>
            </tr>
        </table>
        </asp:PlaceHolder>
        <asp:UpdatePanel ID="ListUpdatePanel" runat="server" >
        <ContentTemplate>
        <tb:eChannel_AutopayFile_List ID="eChannel_AutopayFile_List1" runat="server" />
        </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content> 