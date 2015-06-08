<%@ page language="C#" autoeventwireup="true" inherits="Emp_LeaveBalanceAdjustment_Edit, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/Emp_Common.ascx" TagName="Emp_Common" TagPrefix="uc1" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>

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
                    <asp:Label ID="ActionHeader" runat="server" Text="Edit" />
                    <asp:Label Text="Leave Balance Adjustment" runat="server" />
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
                     OnBackButton_Click="Back_Click"
                     OnSaveButton_Click ="Save_Click"
                     OnDeleteButton_Click="Delete_Click"
                      />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
                </td>
            </tr>
        </table>

        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="15%" />
            <col width="35%" />
            <col width="15%" />
            <col width="35%" />
            <tr>
                <td class="pm_field_header" >
                    <asp:Label Text="Adjust Date" runat="server" />:</td>
                <td class="pm_field" >
                    <uc1:WebDatePicker id="LeaveBalAdjDate" runat="server" />
                </td>
                <td class="pm_field_header" >
                    <asp:Label ID="Label2" Text="Adjust Type" runat="server" />:</td>          
                <td class="pm_field" >
                    <asp:DropDownList ID="LeaveBalAdjType" runat="Server" /></td>
            </tr>
            <tr>                    
                <td class="pm_field_header" >
                    <asp:Label ID="Label1" Text="Leave Type" runat="server" />:</td>
                <td class="pm_field" >
                    <asp:DropDownList ID="LeaveTypeID" runat="Server"  /></td>
                <td class="pm_field_header">
                    <asp:Label Text="Value" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="LeaveBalAdjValue" runat="Server" />                    
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label  Text="Remark" runat="server" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:TextBox ID="LeaveBalAdjRemark" runat="Server" TextMode="MultiLine" Columns="90" Rows="5" /></td>
            </tr>

        </table>
</asp:Content> 