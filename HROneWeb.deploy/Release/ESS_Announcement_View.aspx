<%@ page language="C#" autoeventwireup="true" inherits="ESS_Announcement_View, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="ESSAnnouncementID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="ESS Announcement Setup" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="View ESS Announcement" />:
                    <%=ESSAnnouncementCode.Text%>
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
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
                </td>
            </tr>
        </table>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_field_section">
            <col width="15%" />
            <col width="35%" />
            <col width="15%" />
            <col width="35%" />
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label6" runat="server" EnableViewState="false" Text="Announcement Code" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="ESSAnnouncementCode" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label7" runat="server" EnableViewState="false" Text="Effective Date" />:</td>
                <td class="pm_field" >
                     <asp:Label id="ESSAnnouncementEffectiveDate" runat="server" /></td>
                <td class="pm_field_header" >
                    <asp:Label ID="Label8" runat="server" EnableViewState="false" Text="Expiry Date" />:</td>
                <td class="pm_field" >
                     <asp:Label id="ESSAnnouncementExpiryDate" runat="server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label3" runat="server" EnableViewState="false" Text="Announcement Content" />:</td>
                <td class="pm_field" colspan="3">
                    <CKEditor:CKEditorControl ID="ESSAnnouncementContent" runat="server" Enabled="false" ReadOnly="true" ToolbarStartupExpanded="false"></CKEditor:CKEditorControl></td>
            </tr>
             <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label4" runat="server" EnableViewState="false" Text="Target Companies" />:
                </td>
                <td class="pm_field" valign="top">
                    <asp:Repeater ID="ESSAnnouncementTargetCompanies" runat="server" OnItemDataBound="Companies_ItemDataBound">
                        <ItemTemplate>
                            <input type="checkbox" id="CompanySelect" runat="server" disabled="disabled"/>
                            <input type="hidden" runat="server" id="CompanyID" />
                            <%#  companyBinding.getValue(Container.DataItem,"CompanyName")%>
                            <br />
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label1" runat="server" EnableViewState="false" Text="Target Ranks" />:
                </td>
                <td class="pm_field" valign="top">
                    <asp:Repeater ID="ESSAnnouncementTargetRanks" runat="server" OnItemDataBound="Ranks_ItemDataBound">
                        <ItemTemplate>
                            <input type="checkbox" id="RankSelect" runat="server" disabled="disabled"/>
                            <input type="hidden" runat="server" id="RankID" />
                            <%#  rankBinding.getValue(Container.DataItem,"RankDesc")%>
                            <br />
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
        </asp:UpdatePanel >
</asp:Content> 