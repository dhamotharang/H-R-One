<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompanyBankAccount_Edit.aspx.cs" Inherits="CompanyBankAccount_Edit"  MasterPageFile="~/MainMasterPage.master"  %>

<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="CompanyBankAccountID" runat="server" name="ID" />
		<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="Label6" Text="Company Bank Account Setup" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <%=CurID<=0?HROne.Common.WebUtility.GetLocalizedString("Add"):HROne.Common.WebUtility.GetLocalizedString("Edit") %>
                  <asp:Label Text="Company bank Account" runat="server" />:
                    <%=CompanyBankAccountHolderName.Text%>
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
            <col width="15%" />
            <col width="85%" />
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label2" Text="Holder Name" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="CompanyBankAccountHolderName" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label1" Text="Bank Account Number" runat="server" />:
                </td>
                <td class="pm_field">
                <table border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td align="center">
                            <asp:TextBox ID="CompanyBankAccountBankCode" runat="Server" />
                        </td>
                        <td align="center">-</td>
                        <td align="center">
                            <asp:TextBox ID="CompanyBankAccountBranchCode" runat="Server" />
                        </td>
                        <td align="center">-</td>
                        <td align="center">
                            <asp:TextBox ID="CompanyBankAccountAccountNo" runat="Server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pm_field" align="center" style="border:0;">
                            <asp:Label ID="Label4" Text="Bank Code" runat="Server" />
                        </td>
                        <td></td>
                        <td class="pm_field" align="center" style="border:0;">
                            <asp:Label ID="Label5" Text="Branch Code" runat="Server" />
                        </td>
                        <td></td>
                        <td class="pm_field" align="center" style="border:0;">
                            <asp:Label ID="Label7" Text="Account No." runat="Server" />
                        </td>
                    </tr>
                </table>
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" style="vertical-align:top;">
                    <asp:Label ID="Label3" Text="Applied to" runat="server" />:
                </td>
                <td class="pm_field" >
                    <asp:Repeater ID="companyRepeater" runat="server" OnItemDataBound="companyRepeater_ItemDataBound">
                        <ItemTemplate>
                            <asp:CheckBox ID="ItemSelect" runat="server" />
                        </ItemTemplate>
                        <SeparatorTemplate>
                            <br />
                        </SeparatorTemplate>
                    </asp:Repeater>
                    <tb:RecordListFooter ID="ListFooter" runat="server" 
                        ShowAllRecords="true"
                        ListOrderBy="CompanyCode"
                        ListOrder="true" Visible="false"  />
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
        </asp:UpdatePanel >
</asp:Content> 