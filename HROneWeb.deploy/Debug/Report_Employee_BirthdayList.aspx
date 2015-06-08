<%@ page language="C#" autoeventwireup="true" inherits="Report_Employee_BirthdayList, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>

<%@ Register Src="~/controls/ReportExportControl.ascx" TagName="ReportExportControl" TagPrefix="uc3" %>
<%@ Register Src="~/controls/EmployeeSearchControl.ascx" TagName="EmployeeSearchControl" TagPrefix="uc2" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="lblReportHeader" runat="server" Text="Employee Birthday List" />
                </td>
            </tr>
        </table>
        <table width="100%" class="pm_section">
            <tr>
                <td class="pm_search_header">
                    <asp:Label ID="Label1" runat="server" Text="Birthday Range" />:
                </td>
                <td class="pm_search" >
                    <uc1:WebDatePicker id="BirthdayFrom" runat="server" />
                    -
                    <uc1:WebDatePicker id="BirthdayTo" runat="server" />
                 </td>
            </tr>
            <tr>
                <td class="pm_search_header">
                </td>
                <td class="pm_search" colspan="3">
                    <asp:CheckBox ID="chkDisplayYear" runat="server" Text="Display Year of Birth" />
                 </td>
            </tr>
        </table> 
        <uc2:EmployeeSearchControl id="EmployeeSearchControl1" runat="server" />
        <uc3:ReportExportControl id="ReportExportControl3" runat="server"  OnClick="btnGenerate_Click" ButtonCssClass="button"/>

</asp:Content> 