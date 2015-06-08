<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DBServer_Edit.aspx.cs" Inherits="DBServer_Edit"  %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" Runat="Server">
        <input type="hidden" id="DBServerID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="Label1" Text="Database Server" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <%=CurID<=0?"Add":"Edit" %>
                    <asp:Label ID="Label2" Text=" Database Server" runat="server" />
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
            </tr>
        </table>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="toolBar" />
    </Triggers>
    <ContentTemplate >
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_field_section">
            <col width="30%" />
            <col width="70%" />
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label3" runat="server" Text="Code" />:</td>
                <td class="pm_field" >
                    <asp:TextBox ID="DBServerCode" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label4" runat="server" Text="Type" />:</td>
                <td class="pm_field" >
                    <asp:Label ID="DBServerDBType" runat="Server" Text="MSSQL" /></td>
            </tr>
            <tr>
                <td align="left" class="pm_field_header" >
                    <asp:Label ID="lblServerLocaton" runat="server" Text="Server Location" />
                </td>
                <td align="left" class="pm_field">
                    <asp:TextBox ID="DBServerLocation" runat="server"/>
                </td>
            </tr>
            <tr>
                <td align="left" class="pm_field_header" >
                    <asp:Label ID="Label5" runat="server" Text="SA User ID" />
                </td>
                <td align="left" class="pm_field">
                    <asp:TextBox ID="DBServerSAUserID" runat="server"/>
                </td>
            </tr>
            <tr>
                <td align="left" class="pm_field_header" >
                    <asp:Label ID="Label6" runat="server" Text="SA Password" />
                </td>
                <td align="left" class="pm_field">
                    <asp:TextBox ID="DBServerSAPassword" runat="server" TextMode="Password" />
                </td>
            </tr>
            <tr>
                <td align="left" class="pm_field_header" >
                    <asp:Label ID="Label7" runat="server" Text="User ID for Company Database" />
                </td>
                <td align="left" class="pm_field">
                    <asp:TextBox ID="DBServerUserID" runat="server"/>
                </td>
            </tr>
            <tr>
                <td align="left" class="pm_field_header" >
                    <asp:Label ID="Label8" runat="server" Text="Password for Company Database" />
                </td>
                <td align="left" class="pm_field">
                    <asp:TextBox ID="DBServerPassword" runat="server" TextMode="Password" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

