<%@ page language="C#" autoeventwireup="true" inherits="Emp_Contract_Edit, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/Emp_Common.ascx" TagName="Emp_Common" TagPrefix="uc1" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="EmpContractID" runat="server" name="ID" />
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
                    <%=CurID<=0?HROne.Common.WebUtility.GetLocalizedString("Add"):HROne.Common.WebUtility.GetLocalizedString("Edit") %>
                    <asp:Label runat="server" Text="Contract Terms " />
                </td>
            </tr>
        </table>
        
            
                
        
        <uc1:Emp_Common id="Emp_Common1" runat="server"></uc1:Emp_Common><br /> 
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
                    <asp:Label runat="server" Text="Company Name " />:</td>
                <td class="pm_field" >
                    <asp:TextBox ID="EmpContractCompanyName" runat="Server" /></td>
                <td class="pm_field_header" >
                    <asp:Label runat="server" Text="Contract No. " />:</td>
                <td class="pm_field" >
                    <asp:TextBox ID="EmpContractCompanyContactNo" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="From" />:</td>
                <td class="pm_field">
                    <uc1:WebDatePicker id="EmpContractEmployedFrom" runat="server" />
                </td>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="To" />:</td>
                <td class="pm_field">
                    <uc1:WebDatePicker id="EmpContractEmployedTo" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label4" runat="server" Text="Address " />:</td>
                <td class="pm_field" colspan="3">
                    <asp:TextBox ID="EmpContractCompanyAddr" runat="Server" Rows="5" Columns="80" TextMode="MultiLine" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Gratuity" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="CurrencyID" runat="Server" />
                    <asp:TextBox ID="EmpContractGratuity" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_title" colspan="4"><asp:Label ID="lblPayrollProcessHeader" runat="server" Text="For Payroll Calculation Only" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label1" runat="server" Text="Payment Code" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="PayCodeID" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label2" runat="server" Text="Method" />:</td>
                <td class="pm_field">
                    <asp:DropDownList ID="EmpContractGratuityMethod" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label ID="Label3" runat="server" Text="Bank Account Number" />:</td>
                <td class="pm_field">
                    <asp:DropDownList ID="EmpAccID" runat="Server" /></td>
            </tr>
            <tr>
            </tr>
        </table>
</asp:Content> 