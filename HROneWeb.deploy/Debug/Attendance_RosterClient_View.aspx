<%@ page language="C#" autoeventwireup="true" inherits="Attendance_RosterClient_View, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>

<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="RosterClientID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label Text="Roster Client Setup" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label Text="Roster Client" runat="server" />:
                    <%=RosterClientCode.Text %>
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
                     OnDeleteButton_Click="Delete_ClickTop"
                      />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
                </td>
            </tr>
        </table>
        <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
            <tr>
                <td class="pm_field_title" colspan="7">
                    <asp:Label ID="Label1" Text="Roster Client" runat="server" />
                </td>
                <td class="pm_field_title" align="right">
                </td>
            </tr>
        </table>
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="30%" />
            <col width="70%" />
            <tr>
                <td class="pm_field_header" >
                    <asp:Label Text="Code" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="RosterClientCode" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Name" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="RosterClientName" runat="Server" /></td>
            </tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label2" Text="Hierarchy Level Mapping to Site Code" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="RosterClientMappingSiteCodeToHLevelID" runat="Server" /></td>
            </tr>
			<tr id="CostCenterRow" runat="server" >
                <td class="pm_field_header">
                    <asp:Label ID="Label47" Text="Cost Center" runat="server" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="CostCenterID" runat="Server"/>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
        </asp:UpdatePanel >
        <br />


        
            
                
        
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar id="siteToolBar" runat="server"
                     BackButton_Visible="false"
                     EditButton_Visible="false" 
                     SaveButton_Visible="false" 
                     OnNewButton_Click ="New_Click"
                     OnDeleteButton_Click="Delete_Click"
                      />
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="siteToolBar"/>
        </Triggers>

        <ContentTemplate >
        <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
            <tr>
                <col width="26px" /> 
                <col width="200px" /> 
                <td class="pm_list_header">
                </td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_RosterClientSiteCode" OnClick="ChangeOrder_Click"
                        Text="Code" />
                </td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_RosterClientSitePropertyName" OnClick="ChangeOrder_Click"
                        Text="Property Name" />
                </td>
            </tr>
            <asp:Repeater ID="Repeater" runat="server" OnItemCommand="Repeater_ItemCommand" OnItemDataBound="Repeater_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_list" align="center" >
                            <asp:CheckBox ID="ItemSelect" runat="server" />
                            <input type="hidden" runat="server" id="RosterClientSiteID" />
                        </td>
                        <td class="pm_list"  align="left">
                            <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Attendance_RosterClientSite_View.aspx?RosterClientSiteID=" + sbinding.getValue(Container.DataItem,"RosterClientSiteID"))%>">
                            <%#sbinding.getValue(Container.DataItem,"RosterClientSiteCode")%>
                            </a>
                        </td>
                        <td class="pm_list"  align="left">
                            <%#sbinding.getValue(Container.DataItem,"RosterClientSitePropertyName")%>
                        </td>

                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            </table>
            <tb:RecordListFooter id="ListFooter" runat="server"
                ShowAllRecords="true"
              />
        </ContentTemplate> 
        </asp:UpdatePanel> 
</asp:Content> 