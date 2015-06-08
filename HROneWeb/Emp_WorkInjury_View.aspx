<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Emp_WorkInjury_View.aspx.cs"    Inherits="Emp_WorkInjury_View" MasterPageFile="~/MainMasterPage.master"  %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/Emp_Common.ascx" TagName="Emp_Common" TagPrefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="EmpWorkInjuryRecordID" runat="server" name="ID" />
        <input type="hidden" id="EmpID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label Text="Employee Information" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="View Work Injury Record" />
                </td>
            </tr>
        </table>
        <uc1:Emp_Common ID="Emp_Common1" runat="server"></uc1:Emp_Common>
        <br />
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar id="toolBar" runat="server"
                     NewButton_Visible="false" 
                     SaveButton_Visible="false" 
                     OnBackButton_Click="Back_Click"
                     OnEditButton_Click ="Edit_Click"
                     OnDeleteButton_Click="Delete_Click"
                      />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openAlert(); return false;"/>
                </td>
            </tr>
        </table>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_field_section">
            <col width="15%" />
            <col width="35%" />
            <col width="15%" />
            <col width="35%" />
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label1" runat="server" Text="Date of Accident" />:</td>
                <td class="pm_field" >
                    <asp:Label ID="EmpWorkInjuryRecordAccidentDate" runat="server" />
                </td>
                <td class="pm_field_header" >
                    <asp:Label ID="Label2" runat="server" Text="Injury Nature" />:</td>
                <td class="pm_field" >
                    <asp:Label ID="EmpWorkInjuryRecordInjuryNature" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label3" runat="server" Text="Location of Accident" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="EmpWorkInjuryRecordAccidentLocation" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label4" runat="server" Text="Reason for Accident" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="EmpWorkInjuryRecordAccidentReason" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label5" runat="server" Text="Date of Report" />:</td>
                <td class="pm_field">
                    <asp:Label id="EmpWorkInjuryRecordReportedDate" runat="server" />
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label6" runat="server" Text="Cheque Received Date" />:</td>
                <td class="pm_field">
                    <asp:Label id="EmpWorkInjuryRecordChequeReceivedDate" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label7" runat="server" Text="Settle Date" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:Label id="EmpWorkInjuryRecordSettleDate" runat="server" />
                </td>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label8" runat="server" Text="Remark" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="EmpWorkInjuryRecordRemark" runat="Server"/></td>
            </tr>
        </table>
</asp:Content> 