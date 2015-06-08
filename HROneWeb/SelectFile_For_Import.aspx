<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectFile_For_Import.aspx.cs" Inherits="SelectFile_For_Import" MasterPageFile="~/MainMasterPage.master" EnableViewState="false" %>

<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="ILabel1" Text="Select File For Import" runat="server" />
                </td>
            </tr>
        </table>
 
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
<%--            <tr>
                <td>
                    <asp:Label ID="ILabel2" Text="Import Details" runat="server" />
                </td>
            </tr>--%>
             <tr>
                <td>
                    <asp:Button ID="btnBack" runat="Server" Text="- Back -" CssClass="button" OnClick="btnBack_Click" />
                </td>
            </tr>
       </table>
        
            
                
        
        <table class="pm_section" width="100%">
            <tr>
                <td class="pm_search_header">
                    <asp:Label runat="server" Text="Upload file path" />:</td>
                <td class="pm_search">
                    <asp:FileUpload ID="FileUploadControl" runat="server" Width="400" />
                    <asp:Button ID="Upload" runat="server" Text="Upload" OnClick="Upload_Click"  CssClass="button"/>
                </td>
                <td class="pm_search">
                    
                </td>
                <td>
                    <asp:Label ID="connString" runat="server" />
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:PostBackTrigger ControlID="Upload" />
            </Triggers>
            <ContentTemplate>
        </ContentTemplate> 
        </asp:UpdatePanel> 
</asp:Content> 