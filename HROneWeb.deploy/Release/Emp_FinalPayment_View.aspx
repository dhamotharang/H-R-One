<%@ page language="C#" autoeventwireup="true" inherits="Emp_FinalPayment_View, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>

<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/Emp_Common.ascx" TagName="Emp_Common" TagPrefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="EmpFinalPayID" runat="server" name="ID" />
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
                    <asp:Label Text="View Final Payments" runat="server" />
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
        <table border="0" width="100%" cellspacing="0" class="pm_field_section">
            <col width="15%" />
            <col width="35%" />
            <col width="15%" />
            <col width="35%" />
            <tr>
                <td class="pm_field_header" >
                    <asp:Label Text="Payment Code" runat="server" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="PayCodeID" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label4" Text="Final Pay Amount" runat="server" />
                    :</td>
                <td class="pm_field" >
                    HK$<asp:Label ID="EmpFinalPayAmount" runat="Server"  />
                </td>
                <td class="pm_field_header" >
                    <asp:Label Text="Pay Method" runat="server" />:
                </td>
                <td class="pm_field" >
                    <asp:Label ID="EmpFinalPayMethod" runat="Server" />
                </td>
            </tr>
            <tr id="BankAccountRow" runat="server" >
                <td class="pm_field_header">
                    <asp:Label Text="Bank Account Number" runat="server" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="EmpAccID" runat="Server" />
                    <asp:Label ID="lblDefaultBankAccount" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label2" Text="Days Adjust" runat="server" />:
                </td>
                <td class="pm_field">
                    <asp:Label ID="EmpFinalPayNumOfDayAdj" runat="Server" />
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label1" Text="Rest Payment" runat="server" />?
                </td>
                <td class="pm_field">
                    <asp:Label ID="EmpFinalPayIsRestDayPayment" runat="Server" />
                </td>
            </tr>
            <tr id="CostCenterRow" runat="server" >
                <td class="pm_field_header">
                    <asp:Label ID="Label3" runat="server" Text="Cost Center" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="CostCenterID" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Remark" runat="server" />:
                </td>
                <td class="pm_field">
                    <asp:Label ID="EmpFinalPayRemark" runat="Server"/>
                </td>
                <td class="pm_field_header">
                    <asp:Label Text="Is AutoGen" runat="server" />:
                </td>
                <td class="pm_field">
                    <asp:Label ID="EmpFinalPayIsAutoGen" runat="Server" />
                </td>
            </tr>
        </table>
</asp:Content> 