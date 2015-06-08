<%@ page language="C#" autoeventwireup="true" inherits="Emp_AVCPlan_Edit, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>

<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/Emp_Common.ascx" TagName="Emp_Common" TagPrefix="uc1" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="EmpAVCID" runat="server" name="ID" />
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
                    <asp:Label Text="AVC Plan" runat="server" />
                </td>
            </tr>
        </table>
        
             
        
        <uc1:Emp_Common ID="Emp_Common1" runat="server"></uc1:Emp_Common>
        <br />
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar ID="toolBar" runat="server" NewButton_Visible="false" EditButton_Visible="false" OnBackButton_Click="Back_Click" OnSaveButton_Click="Save_Click" OnDeleteButton_Click="Delete_Click" />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
                </td>
            </tr>
        </table>
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="20%" />
            <col width="30%" />
            <col width="20%" />
            <col width="30%" />
            <tr>
                <td class="pm_field_header" >
                    <asp:Label Text="From" runat="server" />:</td>
                <td class="pm_field" >
                    <uc1:WebDatePicker id="EmpAVCEffFr" runat="server" />
                </td>
                <td class="pm_field_header" >
                    <asp:Label Text="To" runat="server" />:</td>
                <td class="pm_field" >
                    <uc1:WebDatePicker id="EmpAVCEffTo" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="AVC Plan" runat="server" />:</td>
                <td class="pm_field">
                    <asp:DropDownList ID="AVCPlanID" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label ID="Label4" Text="Default MPF Plan" runat="server" />:</td>
                <td class="pm_field">
                    <asp:DropDownList ID="DefaultMPFPlanID" runat="Server" AutoPostBack="true"  /></td>
            </tr>
            <asp:Panel ID="BOCIAVCPanel" runat="server" >
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label6" Text="Plan No. for Voluntary Contribution (if any):" runat="server" />:</td>
                <td class="pm_field"  colspan="3">
                    <asp:TextBox ID="EmpAVCPlanBOCIVCPlanNo" runat="Server"  Columns="5" MaxLength="5" /></td>
            </tr>
            </asp:Panel>
            <tr>
                <td class="pm_list_header">
                    <asp:Label ID="Label5" Text="" runat="server" /></td>
                <td class="pm_list_header" align="right" >
                    <asp:Label ID="Label1" Text="AVC below MAX" runat="server" /></td>
                <td class="pm_list_header" align="right" >
                    <asp:Label ID="Label2" Text="AVC above MAX" runat="server" /></td>
                <td class="pm_list_header" align="right" >
                    <asp:Label ID="Label3" Text="Fix Contribution Amount" runat="server" /></td>
            </tr>
            <tr>
                <td class="pm_list">
                    <asp:CheckBox ID="EmpAVCEROverrideSetting" runat="server" Text="Override Employer" />
                </td>
                <td class="pm_list" align="right">
                    <asp:TextBox ID="EmpAVCERBelowRI" runat="server" Columns="3" style="text-align:right;" />
                </td>
                <td class="pm_list" align="right">
                    <asp:TextBox ID="EmpAVCERAboveRI" runat="server" Columns="3" style="text-align:right;" />
                </td>
                <td class="pm_list" align="right">
                    <asp:TextBox ID="EmpAVCERFix" runat="server" Columns="10" style="text-align:right;" />
                </td>
            </tr>
            <tr>
                <td class="pm_list">
                    <asp:CheckBox ID="EmpAVCEEOverrideSetting" runat="server" Text="Override Employee" />
                </td>
                <td class="pm_list" align="right">
                    <asp:TextBox ID="EmpAVCEEBelowRI" runat="server" Columns="3" style="text-align:right;" />
                </td>
                <td class="pm_list" align="right">
                    <asp:TextBox ID="EmpAVCEEAboveRI" runat="server" Columns="3" style="text-align:right;" />
                </td>
                <td class="pm_list" align="right">
                    <asp:TextBox ID="EmpAVCEEFix" runat="server" Columns="10" style="text-align:right;" />
                </td>
            </tr>
        </table>
</asp:Content> 