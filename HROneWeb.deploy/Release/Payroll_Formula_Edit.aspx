<%@ page language="C#" autoeventwireup="true" inherits="Payroll_Formula_Edit, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>
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
                <%=CurID<=0?HROne.Common.WebUtility.GetLocalizedString("Add"):HROne.Common.WebUtility.GetLocalizedString("Edit") %>
                <asp:Label runat="server" Text="Payroll Formula" />:
            </td>
        </tr>
    </table>
    <table width="100%" class="pm_section">
        <tr>
            <td valign="top">
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
        <ContentTemplate >
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
                            <asp:TextBox ID="PayFormCode" runat="Server" />
                        </td>
                        <td class="pm_field_header" >
                            <asp:Label runat="server" Text="Description" />:
                        </td>
                        <td class="pm_field" >
                            <asp:TextBox ID="PayFormDesc" runat="Server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pm_field_header" >
                            <asp:Label runat="server" Text="Formula" />:
                        </td>
                        <td class="pm_field" colspan="3">
                            <asp:DropDownList runat="server" ID="FormulaType" OnSelectedIndexChanged="FormulaType_SelectedIndexChanged" AutoPostBack="True"  >
                                <asp:ListItem Value="MONTHLYPAYMENT" Text="Monthly Payment x" Selected="True"/> 
                                <asp:ListItem Value="PAYFORMID" Text="Use Existing Formula" />
                            </asp:DropDownList> 
                            <asp:DropDownList runat="server" ID="ReferencePayFormID" Visible="false" />
                            <asp:PlaceHolder  runat="server" ID="FormulaParameter" Visible="true"  >
                                <asp:TextBox ID="PayFormMultiplier" runat="Server" Columns="3" MaxLength="5" /> / 
                                <asp:TextBox ID="PayFormDivider" runat="Server" Columns="3" MaxLength="5" />
                            </asp:PlaceHolder>
                        </td>
                    </tr>
                    <tr>
                        <td class="pm_field_header">
                            <asp:Label ID="Label1" runat="server" Text="Rounding Rule" />  :</td>
                        <td class="pm_field" colspan="3">
                            <asp:DropDownList ID="PayFormRoundingRule" runat="server" />
                            <asp:DropDownList ID="PayFormDecimalPlace" runat="server" />
                            <asp:Label ID="Label2" runat="server" Text="decimal place(s)" />
                        </td>
                    </tr>
                </table>
        </ContentTemplate>
        </asp:UpdatePanel >
            </td>
        </tr>
    </table>
</asp:Content> 