<%@ page language="C#" autoeventwireup="true" inherits="Emp_LeaveBalanceAdjustment_View, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/Emp_Common.ascx" TagName="Emp_Common" TagPrefix="uc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="LeaveBalAdjID" runat="server" />
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
                    <asp:Label runat="server" Text="View" />
                    <asp:Label Text="Leave Balance Adjustment" runat="server" />
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
                <td class="pm_field_header" >
                    <asp:Label Text="Adjust Date" runat="server" />:</td>
                <td class="pm_field" >
                    <asp:Label ID="LeaveBalAdjDate" runat="Server" /></td>
                <td class="pm_field_header" >
                    <asp:Label ID="Label2" Text="Adjust Type" runat="server" />:</td>
                <td class="pm_field" >
                    <asp:Label ID="LeaveBalAdjType" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label1" Text="Leave Type" runat="server" />:</td>
                <td class="pm_field" >
                    <asp:Label ID="LeaveTypeID" runat="Server"  /></td>            

                <td class="pm_field_header">
                    <asp:Label Text="Value" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="LeaveBalAdjValue" runat="Server" />                    
                </td>

            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Remark" runat="server" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="LeaveBalAdjRemark" runat="Server" /></td>
            </tr>
        </table>
</asp:Content> 