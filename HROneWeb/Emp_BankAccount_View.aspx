<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Emp_BankAccount_View.aspx.cs" Inherits="Emp_BankAccount_View"  MasterPageFile="~/MainMasterPage.master" %>

<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/Emp_Common.ascx" TagName="Emp_Common" TagPrefix="uc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="EmpBankAccountID" runat="server" name="ID" />
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
                    <asp:Label Text="View Bank Account" runat="server" />:
                    <asp:Label ID="TitleCode" runat="server" />
                </td>
            </tr>
        </table>
        
            
                
        
		<uc1:Emp_Common id="Emp_Common1" runat="server"></uc1:Emp_Common><br />
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
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="15%" />
            <col width="35%" />
            <col width="15%" />
            <col width="35%" />
            <tr>
                <td class="pm_field_header" >
                    <asp:Label Text="Bank Account Number" runat="server" />:
                </td>
                <td class="pm_field" >
                    <asp:Label ID="EmpBankCode" runat="Server" />-
                    <asp:Label ID="EmpBranchCode" runat="Server" />-
                    <asp:Label ID="EmpAccountNo" runat="Server" />
                </td>
                <td class="pm_field_header" >
                    <asp:Label Text="Holder Name" runat="server" />:
                </td>
                <td class="pm_field" >
                    <asp:Label ID="EmpBankAccountHolderName" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Is Default Account" runat="server" />:
                </td>
                <td class="pm_field" >
                    <asp:CheckBox ID="EmpAccDefault" runat="Server" Enabled="false" />
                </td>
                <td class="pm_field_header" >
                    <asp:Label ID="Label1" Text="Remark" runat="server" />:
                </td>
                <td class="pm_field">
                    <asp:Label ID="EmpBankAccountRemark" runat="Server" />
                </td>
            </tr>
        </table>
</asp:Content> 