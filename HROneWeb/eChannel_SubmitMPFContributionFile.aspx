<%@ Page Language="C#" MasterPageFile="~/MainMasterPage.master" AutoEventWireup="true" CodeFile="eChannel_SubmitMPFContributionFile.aspx.cs" Inherits="eChannel_SubmitMPFContributionFile"  %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
<%@ Register Src="~/controls/eChannel_MPFContributionFile_List.ascx" TagName="eChannel_MPFContributionFile_List" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="ILabel1" Text="Submit MPF Contribution File" runat="server" />
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
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="ILabel2" Text="Detail" runat="server" />
                </td>
            </tr>
        </table>
        
            
                
        <asp:PlaceHolder ID="UploadBankFilePanel" runat="server" >
        <br />
        <table class="pm_section" width="100%">
            <tr>
                <td class="pm_search_header">
                    <asp:Label ID="Label1" runat="server" Text="Upload file path" />:</td>
                <td class="pm_search">
                    <asp:FileUpload ID="BankFileUpload" runat="server" Width="400" />
                    <asp:Button ID="Upload" runat="server" Text="Upload" OnClick="Upload_Click"  CssClass="button"/>
                </td>
                <td class="pm_search">
                    
                </td>
            </tr>
        </table>
        </asp:PlaceHolder>
        <asp:UpdatePanel ID="ListUpdatePanel" runat="server" >
        <ContentTemplate>
        <tb:eChannel_MPFContributionFile_List ID="eChannel_MPFContributionFile_List1" runat="server" />
        </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content> 