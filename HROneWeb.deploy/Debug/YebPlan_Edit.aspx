<%@ page language="C#" masterpagefile="~/MainMasterPage.master" autoeventwireup="true" inherits="YEBPlan_Edit, HROneWeb.deploy" viewStateEncryptionMode="Always" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" Runat="Server">

        <input type="hidden" id="YEBPlanID" runat="server" name="ID" />
		<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Year End Bonus Plan Setup" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <%=CurID<=0?HROne.Common.WebUtility.GetLocalizedString("Add"):HROne.Common.WebUtility.GetLocalizedString("Edit") %>
                    <asp:Label ID="Label2" runat="server" Text="Year End Bonus Plan" />:
                    <%=YEBPlanCode.Text %>
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
                    <asp:TextBox ID="YEBPlanCode" runat="Server" />
                </td>
                <td class="pm_field_header" >
                    <asp:Label ID="Label4" runat="server" Text="Description" />:
                </td>
                <td class="pm_field" >
                    <asp:TextBox ID="YEBPlanDesc" runat="Server" />
                </td>
            </tr>
            <tr >
                <td class="pm_field_header">
                    <asp:Label ID="Label5" runat="server" Text="Formula" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="YEBPlanPaymentBaseMethod" runat="Server" OnSelectedIndexChanged="YEBPlanPaymentBaseMethod_SelectedIndexChanged" AutoPostBack="true"  />
                    <asp:DropDownList ID="YEBPlanRPPaymentCodeID" runat="Server" Visible="false"/>
                    x
                    <asp:TextBox ID="YEBPlanMultiplier" runat="Server" />
                    
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label8" runat="server" Text="Prorata Method" />:
                </td>
                <td class="pm_field">
                    <asp:DropDownList ID="YEBPlanProrataMethod" runat="Server" />
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label7" runat="server" Text="Payment Code" />:
                </td>
                <td class="pm_field">
                    <asp:DropDownList ID="YEBPlanPaymentCodeID" runat="Server" Width="100%" />
                </td>
                
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label9" runat="server" Text="Eligible Period"/>
                </td>
                <td class="pm_field">
                    <asp:TextBox ID="YEBPlanEligiblePeriod" runat="Server"  /><asp:DropDownList ID="YEBPlanEligibleUnit" runat="Server"  /><br />
                    <asp:CheckBox ID="YEBPlanIsEligibleAfterProbation" runat="server" Text="Eligible After Probation" /><br />
                    <asp:CheckBox ID="YEBPlanEligiblePeriodIsCheckEveryYEBYear" runat="server" Text="Check Every Year" /><br />
                    <asp:CheckBox ID="YEBPlanEligiblePeriodIsExcludeMax3MonthsProbation" runat="server" Text="Exclude first 3 month probation period" />
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label6" runat="server" Text="Globally applied to all employee" />?</td>
                <td class="pm_field">
                    <asp:CheckBox ID="YEBPlanIsGlobal" runat="Server" Text="" />
                </td>
            
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
        </asp:UpdatePanel >
</asp:Content>

