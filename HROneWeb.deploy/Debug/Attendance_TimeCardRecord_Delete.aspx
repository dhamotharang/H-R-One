<%@ page language="C#" autoeventwireup="true" inherits="Attendance_TimeCardRecord_Delete, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>

<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="lblReportHeader" runat="server" Text="Delete Time Card Records" />
                </td>
            </tr>
        </table>
                <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="Label2" Text="Delete Range" runat="server" />
                </td>
            </tr>
        </table>
        <table width="100%" class="pm_section">
            <tr>
                <td class="pm_search_header">                
                    <asp:Label ID="Label1" runat="server" Text="Delete Date Range" />:
                </td>
                <td class="pm_search" >
                    <uc1:WebDatePicker id="DeleteFrom" runat="server" />
                    -
                    <uc1:WebDatePicker id="DeleteTo" runat="server" />
                 </td>
            </tr>
            <tr>            
                <td class="pm_search_header">
                    <asp:Button ID="Delete" runat="server" Text="Delete" CssClass="button" OnClick="Delete_Click" />
                </td>                
            </tr>
        </table>      
 </asp:Content> 