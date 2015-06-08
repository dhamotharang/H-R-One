<%@ Page Language="C#" MasterPageFile="~/MainMasterPage.master" AutoEventWireup="true" CodeFile="BenefitPlan_Edit.aspx.cs" Inherits="BenefitPlan_Edit" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" Runat="Server">

        <input type="hidden" id="BenefitPlanID" runat="server" name="ID" />
		<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Benefit Plan Setup" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <%=CurID<=0?HROne.Common.WebUtility.GetLocalizedString("Add"):HROne.Common.WebUtility.GetLocalizedString("Edit") %>
                    <asp:Label ID="Label2" runat="server" Text="Benefit Plan" />:
                    <%=BenefitPlanCode.Text %>
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
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openAlert(); return false;"/>
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
                    <asp:Label ID="Label3" runat="server" Text="Code" />:
                </td>
                <td class="pm_field" >
                    <asp:TextBox ID="BenefitPlanCode" runat="Server" />
                </td>
                <td class="pm_field_header" >
                    <asp:Label ID="Label4" runat="server" Text="Description" />:
                </td>
                <td class="pm_field" >
                    <asp:TextBox ID="BenefitPlanDesc" runat="Server" />
                </td>
            </tr>
            <tr >
                <td class="pm_field_header">
                    <asp:Label ID="Label5" runat="server" Text="Employer Premium" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="BenefitPlanERPaymentBaseMethod" runat="Server" OnSelectedIndexChanged="BenefitPlanPaymentBaseMethod_SelectedIndexChanged" AutoPostBack="true"  />
                    <span id="spanBenefitPlanERRP" runat="server" visible="false">
                        <asp:DropDownList ID="BenefitPlanERPaymentCodeID" runat="Server" />
                        x
                        <asp:TextBox ID="BenefitPlanERMultiplier" runat="Server" />
                    </span>
                    <span id="spanBenefitPlanERFA" runat="server" visible="false">
                        <asp:TextBox ID="BenefitPlanERAmount" runat="server" />
                    </span>
                </td>
            </tr>
            <tr >
                <td class="pm_field_header">
                    <asp:Label ID="Label6" runat="server" Text="Employee Premium" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="BenefitPlanEEPaymentBaseMethod" runat="Server" OnSelectedIndexChanged="BenefitPlanPaymentBaseMethod_SelectedIndexChanged" AutoPostBack="true"  />
                    <span id="spanBenefitPlanEERP" runat="server" visible="false">
                        <asp:DropDownList ID="BenefitPlanEEPaymentCodeID" runat="Server" />
                        x
                        <asp:TextBox ID="BenefitPlanEEMultiplier" runat="Server" />
                    </span>
                    <span id="spanBenefitPlanEEFA" runat="server" visible="false">
                        <asp:TextBox ID="BenefitPlanEEAmount" runat="server" />
                    </span>
                </td>
            </tr>
            <tr >
                <td class="pm_field_header">
                    <asp:Label ID="Label7" runat="server" Text="Spouse Premium" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="BenefitPlanSpousePaymentBaseMethod" runat="Server" OnSelectedIndexChanged="BenefitPlanPaymentBaseMethod_SelectedIndexChanged" AutoPostBack="true"  />
                    <span id="spanBenefitPlanSpouseRP" runat="server" visible="false">
                        <asp:DropDownList ID="BenefitPlanSpousePaymentCodeID" runat="Server" />
                        x
                        <asp:TextBox ID="BenefitPlanSpouseMultiplier" runat="Server" />
                    </span>
                    <span id="spanBenefitPlanSpouseFA" runat="server" visible="false">
                        <asp:TextBox ID="BenefitPlanSpouseAmount" runat="server" />
                    </span>
                </td>
            </tr>
            <tr >
                <td class="pm_field_header">
                    <asp:Label ID="Label8" runat="server" Text="Child Premium" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="BenefitPlanChildPaymentBaseMethod" runat="Server" OnSelectedIndexChanged="BenefitPlanPaymentBaseMethod_SelectedIndexChanged" AutoPostBack="true"  />
                    <span id="spanBenefitPlanChildRP" runat="server" visible="false">
                        <asp:DropDownList ID="BenefitPlanChildPaymentCodeID" runat="Server" />
                        x
                        <asp:TextBox ID="BenefitPlanChildMultiplier" runat="Server" />
                    </span>
                    <span id="spanBenefitPlanChildFA" runat="server" visible="false">
                        <asp:TextBox ID="BenefitPlanChildAmount" runat="server" />
                    </span>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
        </asp:UpdatePanel >
</asp:Content>

