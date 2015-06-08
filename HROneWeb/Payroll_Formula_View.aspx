<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Payroll_Formula_View.aspx.cs"    Inherits="Payroll_Formula_View" MasterPageFile="~/MainMasterPage.master" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>



<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="PayFormID" runat="server" name="ID" />
		<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Payroll Formula Setup" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="View Payroll Formula" />:
                    <%=PayFormCode.Text %>
                </td>
            </tr>
        </table>
        
            
                
        
        <table width="100%" class="pm_section">
            <tr>
                <td valign="top" colspan="2">
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
                    <input type="hidden" id="Hidden1" runat="server" name="ID" />
                    <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
                        <col width="15%" />
                        <col width="35%" />
                        <col width="15%" />
                        <col width="35%" />
                        <tr>
                            <td class="pm_field_header" >
                                <asp:Label runat="server" Text="Code" />:
                            </td>
                            <td class="pm_field" >
                                <asp:Label ID="PayFormCode" runat="Server" />
                            </td>
                            <td class="pm_field_header" >
                                <asp:Label runat="server" Text="Description" />:
                            </td>
                            <td class="pm_field" >
                                <asp:Label ID="PayFormDesc" runat="Server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="pm_field_header" >
                                <asp:Label runat="server" Text="Formula" />:
                            </td>
                            <td class="pm_field" colspan="3">
                                <asp:Panel ID="PaymentIsNotSystemDefault" runat="server">
                                    <asp:Label ID="UseMonthlyPaymentLabel" runat="server" Text="Monthly Payment x" />
                                    <asp:Label ID="UseExistingFormulaLabel" runat="server" Text="Use Existing Formula" Visible="false" />
                                    
                                    <asp:Label runat="server" ID="ReferencePayFormID" Visible="false" />
                                    <asp:PlaceHolder  runat="server" ID="FormulaParameter" Visible="true"  >
                                        <asp:Label ID="PayFormMultiplier" runat="Server" />
                                        /
                                        <asp:Label ID="PayFormDivider" runat="Server" />
                                    </asp:PlaceHolder> 
                                </asp:Panel>
                                <asp:Panel ID="PaymentIsSystemDefault" runat="server">
                                    (<asp:Label runat="server" Text="System Defined" />)
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td class="pm_field_header" >
                                <asp:Label runat="server" Text="System Default" />?
                            </td>
                            <td class="pm_field" colspan="3">
                                <asp:Label ID="PayFormIsSys" runat="Server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="pm_field_header">
                                <asp:Label ID="Label1" runat="server" Text="Rounding Rule" />  :</td>
                            <td class="pm_field" colspan="3">
                                <asp:Label ID="PayFormRoundingRule" runat="server" />
                                <asp:Label ID="PayFormDecimalPlace" runat="server" />
                                <asp:Label ID="PayFormDecimalPlaceDesc" runat="server" Text="decimal place(s)" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
        </asp:UpdatePanel >
</asp:Content> 