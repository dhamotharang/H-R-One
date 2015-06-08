<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Emp_LeaveBalanceEntitle_View.aspx.cs"    Inherits="Emp_LeaveBalanceEntitle_View" MasterPageFile="~/MainMasterPage.master"  %>

<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/Emp_Common.ascx" TagName="Emp_Common" TagPrefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="LeaveBalanceEntitleID" runat="server" />
        <input type="hidden" id="EmpID" runat="server" />
        <input type="hidden" id="LeaveTypeID" runat="server" />
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
                    <asp:Label Text="View Leave Application" runat="server" />
                </td>
            </tr>
        </table>
        
            
                
        
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar id="toolBar" runat="server"
                     NewButton_Visible="false" 
                     DeleteButton_Visible="false"
                     SaveButton_Visible="false" 
                     OnBackButton_Click="Back_Click"
                     OnEditButton_Click ="Edit_Click"
                      />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolBar" />
        </Triggers>
        <ContentTemplate >
        <uc1:Emp_Common ID="Emp_Common1" runat="server" />
        <br />
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="30%" />
            <col width="70%" />
            <tr>
                <td class="pm_field_header" >
                    <asp:Label Text="Leave Type" runat="server" />:
                </td>
                <td class="pm_field" >
                    <asp:Label ID="LeaveTypeDescription" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Grant Date" runat="server" />:
                </td>
                <td class="pm_field" >
                    <asp:Label ID="LeaveBalanceEntitleEffectiveDate" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="No. of unit grant" runat="server" />:
                </td>
                <td class="pm_field" >
                    <asp:Label ID="LeaveBalanceEntitleDays" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label1" Text="Date Expiry" runat="server" />:
                </td>
                <td class="pm_field" >
                    <asp:Label ID="LeaveBalanceEntitleDateExpiry" runat="Server" />
                </td>
            </tr>
        </table>
        </ContentTemplate> 
        </asp:UpdatePanel> 
</asp:Content> 