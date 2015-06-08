<%@ page language="C#" autoeventwireup="true" inherits="Emp_MPFPlan_View, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/Emp_Common.ascx" TagName="Emp_Common" TagPrefix="uc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="EmpMPFID" runat="server" name="ID" />
        <input type="hidden" id="EmpID" runat="server" name="ID" />
		<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label runat="server" EnableViewState="false" Text="Employee Information" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label runat="server" EnableViewState="false" Text="View MPF Plan" />
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
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_field_section">
            <col width="15%" />
            <col width="35%" />
            <col width="15%" />
            <col width="35%" />
            <tr>
                <td class="pm_field_header" >
                    <asp:Label runat="server" EnableViewState="false" Text="From" />:</td>
                <td class="pm_field" >
                    <asp:Label ID="EmpMPFEffFr" runat="Server" />
                </td>
                <td class="pm_field_header" >
                    <asp:Label runat="server" EnableViewState="false" Text="To" />:</td>
                <td class="pm_field" >
                    <asp:Label ID="EmpMPFEffTo" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" EnableViewState="false" Text="MPF Plan" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="MPFPlanID" runat="Server" /></td>
            </tr>
            <asp:Panel ID="HSBCMPFPanel" runat="server" >
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="ILabel4" runat="server" EnableViewState="false" Text="Class Name" />:</td>
                <td class="pm_field"  colspan="3">
                    <asp:Label ID="EmpMPFPlanClassName" runat="Server" /></td>
            </tr>
            </asp:Panel>
            <asp:Panel ID="AIAMPFPanel" runat="server" >
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label1" runat="server" EnableViewState="false" Text="Benefit Plan No." />:</td>
                <td class="pm_field"  colspan="3">
                    <asp:Label ID="EmpMPFPlanAIABenefitPlanNo" runat="Server" /></td>
            </tr>
            </asp:Panel>
            <asp:Panel ID="SCBMPFPanel" runat="server" >
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label2" runat="server" EnableViewState="false" Text="Provident Fund Type" />:
                </td>
                <td class="pm_field"  colspan="3">
                    <asp:Label ID="EmpMPFPlanSCBPFundType" runat="Server" />
                </td>
            </tr>
            </asp:Panel>
        </table>
</asp:Content> 