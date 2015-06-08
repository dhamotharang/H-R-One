<%@ page language="C#" autoeventwireup="true" inherits="Payroll_Group_User_Edit, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>

<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
	<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
		<tr>
			<td>
				<asp:Label ID="Label1" runat="server" Text="Payroll Group" />
			</td>
		</tr>
	</table>
	<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
        <tr>
            <td>
                <%=CurID<=0?HROne.Common.WebUtility.GetLocalizedString("Add"):HROne.Common.WebUtility.GetLocalizedString("Edit") %>
                <asp:Label ID="Label2" runat="server" Text="Payroll Group User" />:
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
                      />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openAlert(); return false;"/>
                </td>
            </tr>
        </table>
        <table border="1" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="15%"/>
            <col width="35%"/>
            <col width="15%"/>
            <col width="35%"/>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label3" runat="server" Text="Payroll Group" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label  ID="PayGroupID" runat="server" />
                </td>
            </tr>
<%--            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label4" runat="server" Text="Is Public?" />
                </td>
                <td  colspan="3">
                    <asp:CheckBox id="PayGroupIsPublic" runat="server"/>
                </td>
            </tr>--%>
            
        </table>
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <tr>
                <td class="pm_field_header" colspan="3">
                    <asp:Label ID="Label5" runat="server" Text="Authorized Users" />
                </td>
            </tr>
            <asp:Repeater ID="Users" runat="server" OnItemDataBound="Users_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_list" colspan="1">
                            &nbsp;&nbsp;<asp:CheckBox id="UserSelect" runat="server"/>
                            <input type="hidden" runat="server" id="UserID" />
                            <%# userBinding.getValue(Container.DataItem, "LoginID")%>
                        </td>
                        <td class="pm_list" colspan="2">                            
                            <%# userBinding.getValue(Container.DataItem, "UserName")%> 
                        </td>
                    </tr>      
                </ItemTemplate>
            </asp:Repeater>
        </table>        
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="toolbar"/>
    </Triggers>
    </asp:UpdatePanel >
</asp:Content> 