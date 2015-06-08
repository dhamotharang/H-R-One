<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CompanyDB_Edit.aspx.cs" Inherits="CompanyDB_Edit"  %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/HSBCExchangeProfile_List.ascx" TagName="HSBCExchangeProfile_List" TagPrefix="tb" %>
<%@ Register Src="~/controls/CompanyAutopayFile_List.ascx" TagName="CompanyAutopayFile_List" TagPrefix="tb" %>
<%@ Register Src="~/controls/CompanyMPFFile_List.ascx" TagName="CompanyMPFFile_List" TagPrefix="tb" %>
<%@ Register Src="~/controls/CompanyInbox_List.ascx" TagName="CompanyInbox_List" TagPrefix="tb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" Runat="Server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="Label1" Text="Company Database" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <%=CurID<=0?"Add":"Edit" %>
                    <asp:Label ID="Label2" Text=" Company Database" runat="server" />
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
                     CustomButton1_Name="Reset administrator LoginID/Password"
                     CustomButton1_Visible="true"
                     OnCustomButton1_Click="Reset_Click"
                     CustomButton2_Name="Reset administrator Password Only"
                     CustomButton2_Visible="true"
                     OnCustomButton2_Click="ResetPassword_Click"
                      />
                </td>
            </tr>
        </table>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="toolBar" />
    </Triggers>
    <ContentTemplate >
        <input type="hidden" id="CompanyDBID" runat="server" name="ID" />
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_field_section">
            <col width="30%" />
            <col width="70%" />
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label3" runat="server" Text="Client ID" />:</td>
                <td class="pm_field" >
                    <asp:TextBox ID="CompanyDBClientCode" runat="Server" />
                    <asp:CheckBox ID="chkAutoCreateID" runat="server" Text="Auto Assign" Checked="true" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label9" runat="server" Text="Contact Person" />:</td>
                <td class="pm_field" >
                    <asp:TextBox ID="CompanyDBClientContactPerson" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label4" runat="server" Text="Client Name" />:</td>
                <td class="pm_field" >
                    <asp:TextBox ID="CompanyDBClientName" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label10" runat="server" Text="Address" />:</td>
                <td class="pm_field" >
                    <asp:TextBox ID="CompanyDBClientAddress" runat="Server" Columns="75" Rows="5" TextMode="MultiLine" /></td>
            </tr>
            <tr>
                <td align="left" class="pm_field_header" >
                    <asp:Label ID="lblServerLocaton" runat="server" Text="Database Server" />
                </td>
                <td align="left" class="pm_field">
                    <asp:DropDownList ID="DBServerID" runat="server"/>
                </td>
            </tr>
            <tr>
                <td align="left" class="pm_field_header" >
                    <asp:Label ID="Label5" runat="server" Text="Schema Name" />
                </td>
                <td align="left" class="pm_field">
                    <asp:TextBox ID="CompanyDBSchemaName" runat="server"/>
                    <asp:CheckBox ID="chkCreateDB" runat="server" Text="Create database if not exists" />
                </td>
            </tr>
            <tr>
                <td align="left" class="pm_field_header" >
                    <asp:Label ID="Label6" runat="server" Text="Active" />
                </td>
                <td align="left" class="pm_field">
                    <asp:CheckBox ID="CompanyDBIsActive" runat="server" Checked="true"  />
                </td>
            </tr>
            <tr>
                <td align="left" class="pm_field_header" >
                    <asp:Label ID="Label7" runat="server" Text="Max Companies" />
                </td>
                <td align="left" class="pm_field">
                    <asp:TextBox ID="CompanyDBMaxCompany" runat="server" Text="2" />
                </td>
            </tr>
            <tr>
                <td align="left" class="pm_field_header" >
                    <asp:Label ID="Label8" runat="server" Text="Max Users" />
                </td>
                <td align="left" class="pm_field">
                    <asp:TextBox ID="CompanyDBMaxUser" runat="server" Text="3" />
                </td>
            </tr>
            <tr>
                <td align="left" class="pm_field_header" >
                    <asp:Label ID="Label14" runat="server" Text="Max Employees" />
                </td>
                <td align="left" class="pm_field">
                    <asp:TextBox ID="CompanyDBMaxEmployee" runat="server" Text="4" />
                </td>
            </tr>
            <tr>
                <td align="left" class="pm_field_header" >
                    <asp:Label ID="Label16" runat="server" Text="Max Inbox Quota (MB)" />
                </td>
                <td align="left" class="pm_field">
                    <asp:TextBox ID="CompanyDBInboxMaxQuotaMB" runat="server" Text="4" />
                    (<asp:Label ID="Label18" runat="server" Text="0 = use default value under System Parameter" />)
                </td>
            </tr>
            <tr>
                <td align="left" class="pm_field_header" >
                    <asp:Label ID="Label19" runat="server" Text="Autopay/MPF File Options" />
                </td>
                <td align="left" class="pm_field">
                    <asp:CheckBox ID="CompanyDBAutopayMPFFileHasHSBCHASE" runat="server" Text="HSBC/Hang Seng" />
                    <asp:CheckBox ID="CompanyDBAutopayMPFFileHasOthers" runat="server" Text="Others" />
                </td>
            </tr>
            <tr>
                <td align="left" class="pm_field_header" >
                    <asp:Label ID="Label17" runat="server" Text="Module Options" />
                </td>
                <td align="left" class="pm_field">
                    <asp:CheckBox ID="CompanyDBHasEChannel" runat="server" Text="e-Channel" />
                    <asp:CheckBox ID="CompanyDBHasIMGR" runat="server" Text="iMGR" />
                    <asp:CheckBox ID="CompanyDBHasIStaff" runat="server" Text="iStaff" />
                </td>
            </tr>
        </table>
        <br />
        <asp:PlaceHolder ID="AdditionalInformation" runat="server" >
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="Label15" Text="Available Remote Profile" runat="server" />
                </td>
            </tr>
        </table>
        <tb:HSBCExchangeProfile_List ID="HSBCExchangeProfile_List1" runat="server" />
        <br />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="Label11" Text="Autopay File History" runat="server" />
                </td>
            </tr>
        </table>
        <tb:CompanyAutopayFile_List ID="CompanyBankFile_List1" runat="server" />
        <br />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="Label12" Text="MPF File History" runat="server" />
                </td>
            </tr>
        </table>
        <tb:CompanyMPFFile_List ID="CompanyMPFFile_List1" runat="server" />
        <br />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="Label13" Text="Notification" runat="server" />
                </td>
            </tr>
        </table>
        <tb:CompanyInbox_List ID="CompanyInbox_List1" runat="server" />
        </asp:PlaceHolder>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

