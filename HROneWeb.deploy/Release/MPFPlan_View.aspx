<%@ page language="C#" autoeventwireup="true" inherits="MPFPlan_View, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>



<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="MPFPlanID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label Text="MPF Plan Setup" runat="Server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label Text="View MPF Plan" runat="Server" />
                    :
                    <%=MPFPlanCode.Text %>
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
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_field_section">
            <col width="20%" />
            <col width="30%" />
            <col width="20%" />
            <col width="30%" />
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Plan Code" runat="Server" />
                    :</td>
                <td class="pm_field">
                    <asp:Label ID="MPFPlanCode" runat="Server"  /></td>
                <td class="pm_field_header">
                    <asp:Label Text="Description" runat="Server" />
                    :</td>
                <td class="pm_field">
                    <asp:Label ID="MPFPlanDesc" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Scheme No" runat="Server" />
                    :
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="MPFSchemeID" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Name of Employer" runat="Server" />
                    :</td>
                <td class="pm_field">
                    <asp:Label ID="MPFPlanCompanyName" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label ID="lblMPFPlanParticipationNo" Text="Employer Participation No." runat="Server" />
                    :</td>
                <td class="pm_field">
                    <asp:Label ID="MPFPlanParticipationNo" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Contact Name" runat="Server" />
                    :</td>
                <td class="pm_field">
                    <asp:Label ID="MPFPlanContactName" runat="Server"  /></td>
                <td class="pm_field_header">
                    <asp:Label Text="Contact No." runat="Server" />
                    :</td>
                <td class="pm_field">
                    <asp:Label ID="MPFPlanContactNo" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="ILabel1" Text="Address" runat="Server" />
                    :</td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="MPFPlanCompanyAddress" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="ILabel5" runat="server" Text="Rounding Rule for Employer Contribution" />  :</td>
                <td class="pm_field">
                    <asp:Label ID="MPFPlanEmployerRoundingRule" runat="server" />
                    <asp:Label ID="MPFPlanEmployerDecimalPlace" runat="server" />
                    <asp:Label ID="ILabel6" runat="server" Text="decimal place(s)" />
                </td>                <td class="pm_field_header">
                    <asp:Label ID="ILabel7" runat="server" Text="Rounding Rule for Employee Contribution" />  :</td>
                <td class="pm_field">
                    <asp:Label ID="MPFPlanEmployeeRoundingRule" runat="server" />
                    <asp:Label ID="MPFPlanEmployeeDecimalPlace" runat="server" />
                    <asp:Label ID="ILabel8" runat="server" Text="decimal place(s)" />
                </td>            
            </tr>
            <asp:Panel ID="HSBCMPFPanel" runat="server" >
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="ILabel4" Text="Pay Center" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="MPFPlanPayCenter" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label ID="ILabel2" Text="Default Class Name" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="MPFPlanDefaultClassName" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="ILabel3" Text="Employer ID" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="MPFPlanEmployerID" runat="Server" /></td>
            </tr>

            </asp:Panel>
            <asp:Panel ID="BOCIMPFPanel" runat="server" >
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="ILabel10" Text="Scheme No" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="MPFPlanSchemeNo" runat="Server"  /></td>
                <td class="pm_field_header">
                    <asp:Label ID="lblMPFPlanBOCISequenceNo" Text="Next Sequence No." runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="MPFPlanBOCISequenceNo" runat="Server" /></td>
            </tr>
            <tr id="Tr1" runat="server" visible="false" >
                <td class="pm_field_header">
                    <asp:Label ID="ILabel11" Text="Plan No" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="MPFPlanPlanNo" runat="Server" /></td>
            </tr>
            </asp:Panel>
            <asp:Panel ID="AIAMPFPanel" runat="server" >
            <tr class="pm_field_header">
                <td class="pm_field_header">
                    <asp:Label ID="Label3" Text="Employer Plan No" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="MPFPlanAIAERPlanNo" runat="Server"  /></td>
                <td class="pm_field_header">
                    <asp:Label ID="Label5" Text="Payroll Frequency" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="MPFPlanAIAPayFrequency" runat="Server" /></td>
            </tr>
            </asp:Panel>
            <asp:Panel ID="ManulifePanel" runat="server" >
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label1" Text="Sub-Scheme No." runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="MPFPlanManulifeSubSchemeNo" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label ID="Label2" Text="Group No." runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="MPFPlanManulifeGroupNo" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label4" Text="Sub-group No." runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="MPFPlanManulifeSubGroupNo" runat="Server" /></td>
            </tr>
            </asp:Panel>
        </table>
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td align="right">
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
        </asp:UpdatePanel >
</asp:Content> 