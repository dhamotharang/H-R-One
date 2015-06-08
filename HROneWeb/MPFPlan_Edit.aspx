<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MPFPlan_Edit.aspx.cs" Inherits="MPFPlan_Edit" MasterPageFile="~/MainMasterPage.master"  %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>



<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="MPFPlanID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="ILabel5" Text="MPF Plan Setup" runat="Server" />
                </td>
            </tr>
        </table>        
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <%=CurID<=0?HROne.Common.WebUtility.GetLocalizedString("Add"):HROne.Common.WebUtility.GetLocalizedString("Edit") %>
                    <asp:Label Text="MPF Plan" runat="server" /> :
                    <%=MPFPlanCode.Text %>
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
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_field_section">
            <col width="20%" />
            <col width="30%" />
            <col width="20%" />
            <col width="30%" />
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Plan Code" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="MPFPlanCode" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label Text="Description" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="MPFPlanDesc" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Scheme No" runat="server" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="MPFSchemeTrusteeCode" runat="Server" AutoPostBack="true" />
                    <asp:DropDownList ID="MPFSchemeID" runat="Server" AutoPostBack="true" />
                </td>

            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Name of Employer" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="MPFPlanCompanyName" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label ID="lblMPFPlanParticipationNo" Text="Employer Participation No." runat="Server" />
                    :</td>
                <td class="pm_field">
                    <asp:TextBox ID="MPFPlanParticipationNo" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Contact Name" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="MPFPlanContactName" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label Text="Contact No." runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="MPFPlanContactNo" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="ILabel1" Text="Address" runat="server" />:</td>
                <td class="pm_field"  colspan="3">
                    <asp:TextBox ID="MPFPlanCompanyAddress" runat="Server" TextMode="MultiLine" Columns="35"
                        Rows="5" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="ILabel6" runat="server" Text="Rounding Rule for Employer Contribution" />  :</td>
                <td class="pm_field">
                    <asp:DropDownList ID="MPFPlanEmployerRoundingRule" runat="server" />
                    <asp:DropDownList ID="MPFPlanEmployerDecimalPlace" runat="server" />
                    <asp:Label ID="ILabel7" runat="server" Text="decimal place(s)" />
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="ILabel8" runat="server" Text="Rounding Rule for Employee Contribution" />  :</td>
                <td class="pm_field">
                    <asp:DropDownList ID="MPFPlanEmployeeRoundingRule" runat="server" />
                    <asp:DropDownList ID="MPFPlanEmployeeDecimalPlace" runat="server" />
                    <asp:Label ID="ILabel9" runat="server" Text="decimal place(s)" />
                </td>
            </tr>
            <asp:Panel ID="HSBCMPFPanel" runat="server" >
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="lblPayCenterHeader" Text="Pay Center" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="MPFPlanPayCenter" runat="Server" Columns="4" MaxLength="4" CssClass="pm_required" /></td>
                <td class="pm_field_header">
                    <asp:Label ID="lblDefaultClassNameHeader" Text="Default Class Name" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="MPFPlanDefaultClassName" runat="Server" Columns="8" MaxLength="8" CssClass="pm_required" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="lblEmployerIDHeader" Text="Employer ID" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="MPFPlanEmployerID" runat="Server" Columns="8" MaxLength="8" CssClass="pm_required" /></td>
            </tr>
            </asp:Panel>
            <asp:Panel ID="BOCIMPFPanel" runat="server" >
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="ILabel10" Text="Scheme No" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="MPFPlanSchemeNo" runat="Server" Columns="11" MaxLength="11" /></td>
                <td class="pm_field_header">
                    <asp:Label ID="lblMPFPlanBOCISequenceNo" Text="Next Sequence No." runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="MPFPlanBOCISequenceNo" runat="Server" Columns="5" MaxLength="5" /></td>
            </tr>
            <tr runat="server" visible="false" class="pm_field_header">
                <td class="pm_field_header">
                    <asp:Label ID="ILabel11" Text="Plan No" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="MPFPlanPlanNo" runat="Server" Columns="5" MaxLength="5" /></td>
            </tr>
            </asp:Panel>
            <asp:Panel ID="AIAMPFPanel" runat="server" >
            <tr class="pm_field_header">
                <td class="pm_field_header">
                    <asp:Label ID="Label3" Text="Employer Plan No" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="MPFPlanAIAERPlanNo" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label ID="Label5" Text="Payroll Frequency" runat="server" />:</td>
                <td class="pm_field">
                    <asp:DropDownList ID="MPFPlanAIAPayFrequency" runat="Server" /></td>
            </tr>
            </asp:Panel>
            <asp:Panel ID="ManulifePanel" runat="server" >
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label1" Text="Sub-Scheme No." runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="MPFPlanManulifeSubSchemeNo" runat="Server" Columns="8" MaxLength="8" /></td>
                <td class="pm_field_header">
                    <asp:Label ID="Label2" Text="Group No." runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="MPFPlanManulifeGroupNo" runat="Server" Columns="8" MaxLength="8" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label4" Text="Sub-group No." runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="MPFPlanManulifeSubGroupNo" runat="Server" Columns="2" MaxLength="2" /></td>
            </tr>
            </asp:Panel>
        </table>
        </ContentTemplate>        
        </asp:UpdatePanel >

</asp:Content> 