<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PaymentCalculationFormula_Edit.aspx.cs" Inherits="PaymentCalculationFormula_Edit" MasterPageFile="~/MainMasterPage.master"  %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="PayCalFormulaID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label Text="Payment Calculation Formula" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <%=CurID<=0?HROne.Common.WebUtility.GetLocalizedString("Add"):HROne.Common.WebUtility.GetLocalizedString("Edit") %>
                    <asp:Label Text="Payment Calculation Formula" runat="server" />:
                    <%=PayCalFormulaCode.Text%>
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
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="30%" />
            <col width="70%" />
    		<tr>
                <td class="pm_field_title"  colspan="2">
                   <asp:Label runat="server" Text="Payment Calculation Formula" /></td>
			</tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label Text="Code" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="PayCalFormulaCode" runat="Server" /></td>
			</tr>
			<tr>
                <td class="pm_field_header"  rowspan="4">
                    <asp:Label Text="Description" runat="server" />:
                </td>
                <td class="pm_field"  rowspan="4" valign="top">
                    <asp:TextBox ID="PayCalFormulaCodeDesc" runat="Server" Rows="5" Columns="35" TextMode="MultiLine" />  
                </td>
            </tr>
		</table>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
        </asp:UpdatePanel >
</asp:Content> 