<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Emp_CompensationLeaveEntitle_View.aspx.cs"    Inherits="Emp_CompensationLeaveEntitle_View" MasterPageFile="~/MainMasterPage.master"  %>

<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/Emp_Common.ascx" TagName="Emp_Common" TagPrefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="CompensationLeaveEntitleID" runat="server" />
        <input type="hidden" id="EmpID" runat="server" />
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
                    <asp:Label Text="View Compensation Leave Entitlement" runat="server" />
                </td>
            </tr>
        </table>
        
            
                
        
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
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
                </td>
            </tr>
        </table>
        <uc1:Emp_Common ID="Emp_Common1" runat="server" />
        <br />
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="15%" />
            <col width="35%" />
            <col width="15%" />
            <col width="35%" />
            <tr>
                <td class="pm_field_header"  >
                    <asp:Label ID="Label1" Text="Effective Date" runat="server" />:</td>
                <td class="pm_field" >
                    <asp:Label id="CompensationLeaveEntitleEffectiveDate" runat="server" />
                </td>
                <td class="pm_field_header" >
                    <asp:Label ID="Label7" Text="Date Expiry" runat="server" />:</td>
                <td class="pm_field" >
                    <asp:Label id="CompensationLeaveEntitleDateExpiry" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label2" Text="Claim Period" runat="server" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label id="CompensationLeaveEntitleClaimPeriodFrom" runat="server" /> -
                    <asp:Label id="CompensationLeaveEntitleClaimPeriodTo" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label3" Text="Claim Time" runat="server" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="CompensationLeaveEntitleClaimHourFrom" runat="Server" />-
                    <asp:Label ID="CompensationLeaveEntitleClaimHourTo" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header"  >
                    <asp:Label ID="Label4" Text="No. of Hours Claim" runat="server" />:
                </td>
                <td class="pm_field"  >
                    <asp:Label ID="CompensationLeaveEntitleHoursClaim" runat="Server" />
                </td>
                <td class="pm_field_header"  >
                    <asp:Label ID="Label5" Text="Approved By" runat="server" />:
                </td>
                <td class="pm_field" >
                    <asp:Label ID="CompensationLeaveEntitleApprovedBy" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label6" Text="Remark" runat="server" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="CompensationLeaveEntitleRemark" runat="Server" />
                </td>
            </tr>
        </table>
</asp:Content> 