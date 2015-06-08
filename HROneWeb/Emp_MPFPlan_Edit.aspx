<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Emp_MPFPlan_Edit.aspx.cs"    Inherits="Emp_MPFPlan_Edit" MasterPageFile="~/MainMasterPage.master"  %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/Emp_Common.ascx" TagName="Emp_Common" TagPrefix="uc1" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="EmpMPFID" runat="server" name="ID" />
        <input type="hidden" id="EmpID" runat="server" name="ID" />
		<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label runat="server" EnableViewState="false" Text="Employee Information"/>
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <%=CurID<=0?HROne.Common.WebUtility.GetLocalizedString("Add"):HROne.Common.WebUtility.GetLocalizedString("Edit") %>
                    <asp:Label runat="server" EnableViewState="false" Text="MPF Plan" />
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
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_field_section">
            <col width="15%" />
            <col width="35%" />
            <col width="15%" />
            <col width="35%" />
            <tr>
                <td class="pm_field_header" >
                    <asp:Label runat="server" EnableViewState="false" Text="From" />:</td>
                <td class="pm_field" >
                    <uc1:WebDatePicker id="EmpMPFEffFr" runat="server" />
                </td>
                <td class="pm_field_header" >
                    <asp:Label runat="server" EnableViewState="false" Text="To" />:</td>
                <td class="pm_field" >
                    <uc1:WebDatePicker id="EmpMPFEffTo" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" EnableViewState="false" Text="MPF Plan" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="MPFPlanID" runat="Server" AutoPostBack="true" />
                </td>
            </tr>
            <asp:Panel ID="HSBCMPFPanel" runat="server" >
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="ILabel2" runat="server" EnableViewState="false" Text="Class Name"/>:
                </td>
                <td class="pm_field"  colspan="3">
                    <asp:TextBox ID="EmpMPFPlanClassName" runat="Server"  Columns="8" MaxLength="8" />
                </td>
            </tr>
            </asp:Panel>
            <asp:Panel ID="AIAMPFPanel" runat="server" >
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label1" runat="server" EnableViewState="false" Text="Benefit Plan No."/>:
                </td>
                <td class="pm_field"  colspan="3">
                    <asp:TextBox ID="EmpMPFPlanAIABenefitPlanNo" runat="Server"  Columns="6" MaxLength="6" />
                </td>
            </tr>
            </asp:Panel>
            <asp:Panel ID="SCBMPFPanel" runat="server" >
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label2" runat="server" EnableViewState="false" Text="Provident Fund Type" />:
                </td>
                <td class="pm_field"  colspan="3">
                    <asp:DropDownList ID="EmpMPFPlanSCBPFundType" runat="Server" />
                </td>
            </tr>
            </asp:Panel>
        </table>
</asp:Content> 