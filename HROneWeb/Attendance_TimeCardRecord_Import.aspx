<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Attendance_TimeCardRecord_Import.aspx.cs"    Inherits="Attendance_TimeCardRecord_Import" MasterPageFile="~/MainMasterPage.master" %>

<%@ Register Src="~/controls/Attendance_ImportFormatParameterControl.ascx" TagName="Attendance_ImportFormatParameterControl" TagPrefix="uc1" %>
<%@ Register Src="~/controls/Attendance_TimeCardLocationMappingControl.ascx" TagName="Attendance_TimeCardLocationMappingControl" TagPrefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="ILabel1" Text="Import Time Card Records" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="ILabel2" Text="Import Details" runat="server" />
                </td>
            </tr>
        </table>
        <table class="pm_section" width="100%">
            <tr>
                <td class="pm_search_header">
                    <asp:Label runat="server" Text="Upload file path" />:</td>
                <td class="pm_search">
                    <asp:FileUpload ID="ImportFile" runat="server" Width="400" />
                    <asp:Button ID="Upload" runat="server" Text="Upload" OnClick="Upload_Click"  CssClass="button"/>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:UpdatePanel runat="server" UpdateMode="conditional" >
                    <Triggers>
                    </Triggers>
                    <ContentTemplate >
                    <uc1:Attendance_ImportFormatParameterControl ID="Attendance_ImportFormatParameterControl1" runat="server" />
                    </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <br />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="Label1" Text="Location Mapping" runat="server" />
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="conditional" >
        <Triggers>
        </Triggers>
        <ContentTemplate >
        <uc1:Attendance_TimeCardLocationMappingControl ID="Attendance_TimeCardLocationMappingControl1" runat="server" />
        </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content> 