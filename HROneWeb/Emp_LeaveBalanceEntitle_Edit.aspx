<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Emp_LeaveBalanceEntitle_Edit.aspx.cs"    Inherits="Emp_LeaveBalanceEntitle_Edit" MasterPageFile="~/MainMasterPage.master"  %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/Emp_Common.ascx" TagName="Emp_Common" TagPrefix="uc1" %>
<%@ Register Src="~/controls/Emp_LeaveBalance_List.ascx" TagName="Emp_LeaveBalance_List"
    TagPrefix="uc5" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="LeaveBalanceEntitleID" runat="server" />
        <input type="hidden" id="LeaveTypeID" runat="server" />
        <input type="hidden" id="EmpID" runat="server" />
		<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label EnableViewState="false" Text="Employee Information" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="ActionHeader" runat="server" EnableViewState="false" Text="Edit" />
                    <asp:Label EnableViewState="false" Text="Leave Application" runat="server" />
                </td>
            </tr>
        </table>
        
            
                
        
        <uc1:Emp_Common id="Emp_Common1" runat="server"/><br /> 
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar id="toolBar" runat="server"
                     NewButton_Visible="false" 
                     EditButton_Visible="false" 
                     DeleteButton_Visible="false" 
                     OnBackButton_Click="Back_Click"
                     OnSaveButton_Click ="Save_Click"
                      />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" EnableViewState="false" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolBar" />
        </Triggers>
        <ContentTemplate >
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="30%" />
            <col width="70%" />
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label5" Text="Leave Type" runat="server" />:
                </td>
                <td class="pm_field" >
                    <asp:Label ID="LeaveTypeDescription" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label6" Text="Grant Date" runat="server" />:
                </td>
                <td class="pm_field" >
                    <asp:Label ID="LeaveBalanceEntitleEffectiveDate" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label7" Text="No. of unit grant" runat="server" />:
                </td>
                <td class="pm_field" >
                    <asp:Label ID="LeaveBalanceEntitleDays" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label8" Text="Date Expiry" runat="server" />:
                </td>
                <td class="pm_field" >
                    <uc1:WebDatePicker id="LeaveBalanceEntitleDateExpiry" runat="server" />
                </td>
            </tr>
        </table>

        </ContentTemplate>
        </asp:UpdatePanel>
        
</asp:Content> 