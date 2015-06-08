<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ORSOPlan_Edit.aspx.cs" Inherits="ORSOPlan_Edit" MasterPageFile="~/MainMasterPage.master"  %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="ORSOPlanID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="Label4" Text="P-Fund Plan Setup" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <%=CurID<=0?HROne.Common.WebUtility.GetLocalizedString("Add"):HROne.Common.WebUtility.GetLocalizedString("Edit") %>
                    <asp:Label Text="P-Fund Plan " runat="server" />:
                    <%=ORSOPlanCode.Text %>
                </td>
            </tr>
        </table>
        
            
                
        
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
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
        <ContentTemplate> 
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width ="30%" />
            <col width ="70%" />
			<tr>
                <td class="pm_field_header" >
                    <asp:Label Text="Plan Code" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="ORSOPlanCode" runat="Server" /></td>
			</tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label Text="Description" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="ORSOPlanDesc" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label1" Text="Scheme Number" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="ORSOPlanSchemeNo" runat="Server" /></td>
            </tr>
            
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label2" Text="Name of Employer" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="ORSOPlanCompanyName" runat="Server" /></td>
            </tr>    
                    
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label3" Text="Pay Center" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="ORSOPlanPayCenter" runat="Server" /></td>
            </tr>
            
            <!-- Start 0000084, Ricky So, 2014-08-21 -->  
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Use residual for employer" />:
                </td>
                <td class="pm_field">
                   <asp:CheckBox ID="ORSOPlanEmployerResidual" runat="server" /> &nbsp &nbsp &nbsp &nbsp Cap:
                   <asp:TextBox ID="ORSOPlanEmployerResidualCap" runat="server" />
                </td>
			</tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Use residual for employee" />:
                </td>
                <td class="pm_field">
                    <asp:CheckBox ID="ORSOPlanEmployeeResidual" runat="server" /> &nbsp &nbsp &nbsp &nbsp Cap:
                   <asp:TextBox ID="ORSOPlanEmployeeResidualCap" runat="server" />
                </td>
            </tr>
            <!-- End 0000084, Ricky So, 2014-08-21 -->  
			<tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Max. employer's contribution" /></td>
                <td class="pm_field">
					<asp:TextBox ID="ORSOPlanMaxEmployerVC" runat="server" />
                </td>
            </tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Max. employee's contribution" /></td>
                <td class="pm_field">
					<asp:TextBox ID="ORSOPlanMaxEmployeeVC" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="ILabel6" runat="server" Text="Rounding Rule for Employer Contribution" />  :</td>
                <td class="pm_field">
                    <asp:DropDownList ID="ORSOPlanEmployerRoundingRule" runat="server" />
                    <asp:DropDownList ID="ORSOPlanEmployerDecimalPlace" runat="server" />
                    <asp:Label ID="ILabel7" runat="server" Text="decimal place(s)" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="ILabel8" runat="server" Text="Rounding Rule for Employee Contribution" />  :</td>
                <td class="pm_field">
                    <asp:DropDownList ID="ORSOPlanEmployeeRoundingRule" runat="server" />
                    <asp:DropDownList ID="ORSOPlanEmployeeDecimalPlace" runat="server" />
                    <asp:Label ID="ILabel9" runat="server" Text="decimal place(s)" />
                </td>
            </tr>
		</table>
		</ContentTemplate> 
		</asp:UpdatePanel> 
</asp:Content> 