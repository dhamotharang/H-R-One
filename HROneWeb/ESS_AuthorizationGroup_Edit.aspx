<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ESS_AuthorizationGroup_Edit.aspx.cs" Inherits="ESS_AuthorizationGroup_Edit" MasterPageFile="~/MainMasterPage.master" %>


<%@ Register Src="controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <input type="hidden" id="AuthorizationGroupID" runat="server" name="ID" />
	<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Authorization Group Setup" />
                </td>
            </tr>
        </table>
     <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
        <tr>
            <td>
                <%=CurID<=0?HROne.Common.WebUtility.GetLocalizedString("Add"):HROne.Common.WebUtility.GetLocalizedString("Edit") %>
                <asp:Label runat="server" Text="Authorization Group" />
            </td>
        </tr>
    </table>
    
        
            
    
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar ID="toolBar" runat="server" 
                        NewButton_Visible="false" 
                        EditButton_Visible="false"
                        OnBackButton_Click="Back_Click" 
                        OnSaveButton_Click="Save_Click" 
                        OnDeleteButton_Click="Delete_Click" 
                    />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CssClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;" />
                </td>
            </tr>
        </table>
        <table width="100%" class="pm_section">
                <col width="30%" />
                <col width="70%" />
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label1" runat="server" Text="Code" />:
                </td>
                <td class="pm_field">
                    <asp:TextBox ID="AuthorizationCode" runat="server"/>

                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label6" runat="server" Text="Description" />:
                </td>
                <td class="pm_field">
                    <asp:TextBox ID="AuthorizationDesc" runat="server"/>
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label3" runat="server" Text="Receive Other Group Alert" />:
                </td>
                <td class="pm_field">
                    <asp:CheckBox ID="AuthorizationGroupIsReceiveOtherGrpAlert" runat="server"/>
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label2" runat="server" Text="E-mail Address" />:<br />
                    (<asp:Label ID="Label12" runat="server" Text="Separate address with ENTER" />)
                </td>
                <td class="pm_field">
                    <asp:TextBox ID="AuthorizationGroupEmailAddress" runat="Server" TextMode="multiLine" Wrap="true" Columns="90" Rows="5"/>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
        </asp:UpdatePanel> 
</asp:Content> 