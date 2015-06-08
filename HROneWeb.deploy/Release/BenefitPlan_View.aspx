<%@ page language="C#" masterpagefile="~/MainMasterPage.master" autoeventwireup="true" inherits="BenefitPlan_View, HROneWeb.deploy" viewStateEncryptionMode="Always" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" Runat="Server">

        <input type="hidden" id="UserID" runat="server" name="ID" />
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
                    <asp:Label ID="Label2" runat="server" Text="View Benefit Plan" />:
                    <%=BenefitPlanCode.Text %>
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
                    <asp:Label ID="BenefitPlanCode" runat="Server" />
                </td>
                <td class="pm_field_header" >
                    <asp:Label ID="Label4" runat="server" Text="Description" />:
                </td>
                <td class="pm_field" >
                    <asp:Label ID="BenefitPlanDesc" runat="Server" />
                </td>
            </tr>
            <tr >
                <td class="pm_field_header">
                    <asp:Label ID="Label5" runat="server" Text="Employer Premium" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="BenefitPlanERPaymentBaseMethod" runat="Server"  />
                    <span id="spanBenefitPlanERRP" runat="server">
                        : <asp:Label ID="BenefitPlanERPaymentCodeID" runat="Server" /> x <asp:Label ID="BenefitPlanERMultiplier" runat="Server" />
                    </span>
                    <span id="spanBenefitPlanERFA" runat="server">
                        : <asp:Label ID="BenefitPlanERAmount" runat="server" />
                    </span>
                </td>
            </tr>
            <tr >
                <td class="pm_field_header">
                    <asp:Label ID="Label6" runat="server" Text="Employee Premium" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="BenefitPlanEEPaymentBaseMethod" runat="Server"  />
                    <span id="spanBenefitPlanEERP" runat="server">
                        : <asp:Label ID="BenefitPlanEEPaymentCodeID" runat="Server" /> x <asp:Label ID="BenefitPlanEEMultiplier" runat="Server" />
                    </span>
                    <span id="spanBenefitPlanEEFA" runat="server">
                        : <asp:Label ID="BenefitPlanEEAmount" runat="server" />
                    </span>
                </td>
            </tr>
            <tr >
                <td class="pm_field_header">
                    <asp:Label ID="Label7" runat="server" Text="Spouse Premium" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="BenefitPlanSpousePaymentBaseMethod" runat="Server"  />
                    <span id="spanBenefitPlanSpouseRP" runat="server">
                        : <asp:Label ID="BenefitPlanSpousePaymentCodeID" runat="Server" /> x <asp:Label ID="BenefitPlanSpouseMultiplier" runat="Server" />
                    </span>
                    <span id="spanBenefitPlanSpouseFA" runat="server">
                        : <asp:Label ID="BenefitPlanSpouseAmount" runat="server" />
                    </span>
                </td>
            </tr>
            <tr >
                <td class="pm_field_header">
                    <asp:Label ID="Label8" runat="server" Text="Child Premium" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="BenefitPlanChildPaymentBaseMethod" runat="Server"  />
                    <span id="spanBenefitPlanChildRP" runat="server">
                        : <asp:Label ID="BenefitPlanChildPaymentCodeID" runat="Server" /> x <asp:Label ID="BenefitPlanChildMultiplier" runat="Server" />
                    </span>
                    <span id="spanBenefitPlanChildFA" runat="server">
                        : <asp:Label ID="BenefitPlanChildAmount" runat="server" />
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

