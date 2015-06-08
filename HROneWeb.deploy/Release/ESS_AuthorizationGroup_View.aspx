<%@ page language="C#" autoeventwireup="true" inherits="ESS_AuthorizationGroup_View, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>

<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

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
                    <asp:Label runat="server" Text="View" />
                    <asp:Label runat="server" Text="Authorization Group" />
                </td>
            </tr>
        </table>
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar ID="toolBar" runat="server" 
                        NewButton_Visible="false" 
                        SaveButton_Visible="false"
                        OnBackButton_Click="Back_Click" 
                        OnEditButton_Click="Edit_Click" 
                        OnDeleteButton_Click="Delete_Click" 
                    />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CssClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;" />
                </td>
            </tr>
        </table>
        <table width="100%" class="pm_section">
            <col width ="30%" />
            <col width ="70%" />
            <tr>
                <td class="pm_field_header" >
                    <asp:Label runat="server" Text="Code" />:
                </td>
                <td class="pm_field">
                    <asp:Label ID="AuthorizationCode" runat="server"/>

                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label6" runat="server" Text="Description" />:
                </td>
                <td class="pm_field">
                    <asp:Label ID="AuthorizationDesc" runat="server"/>
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label10" runat="server" Text="Receive Other Group Alert" />:
                </td>
                <td class="pm_field">
                    <asp:Label ID="AuthorizationGroupIsReceiveOtherGrpAlert" runat="server"/>
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label2" runat="server" Text="E-mail Address" />:<br />
                    (<asp:Label ID="Label12" runat="server" Text="Separate address with ENTER" />)
                </td>
                <td class="pm_field">
                    <asp:Label ID="AuthorizationGroupEmailAddress" runat="Server" />
                </td>
            </tr>
        </table>
        <br />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
        <ContentTemplate >
        <asp:Panel ID="AuthorizerPanel" runat="server">
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                <tr>
                    <td>
                        <asp:Label runat="server" Text="Authorizer List" />
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="button" OnClick="btnAdd_Click" />
            <asp:Button ID="btnRemove" runat="server" Text="Remove" CssClass="button" OnClick="btnRemove_Click" />
            
            <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
            <col width="26px" />
            <col width="150px" />
            <colgroup width="350px" >
                <col width="150px" />
                <col />
            </colgroup> 
            <col width="150px" />
            <col width="100px" />
                <tr>
                    <td class="pm_list_header" align="center">
                        <asp:Panel ID="SelectAllPanel" runat="server">
                            <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
                        </asp:Panel>
                    </td>
                    <td align="left" class="pm_list_header">
                        <asp:LinkButton runat="server" ID="_EmpNo" OnClick="ChangeOrder_Click" Text="Emp No"/>
                    </td>
                    <td align="left" class="pm_list_header" colspan="2">
                        <asp:LinkButton runat="server" ID="_EmpEngFullName" OnClick="ChangeOrder_Click" Text="Name"/>
                    </td>
                    <td align="left" class="pm_list_header">
                        <asp:LinkButton runat="server" ID="_EmpAlias" OnClick="ChangeOrder_Click" Text="Alias" />
                    </td>
                    <td align="left" class="pm_list_header">
                        <asp:LinkButton runat="server" ID="_EmpStatus" OnClick="ChangeOrder_Click" Text="Status" />
                    </td>   
                    <td align="left" class="pm_list_header">
                        <asp:Label ID="Label1" runat="server" Text="Read Only" />?
                    </td>                    
                    <td align="left" class="pm_list_header">
                        <asp:Label ID="Label3" runat="server" Text="Skip e-mail notification" />?
                    </td>                    
                </tr>
                <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td class="pm_list" align="center">
                                <asp:CheckBox ID="ItemSelect" runat="server" />
                            </td>
                            <td class="pm_list">
                                <%#SBinding.getValue(Container.DataItem, "EmpNo")%>
                            </td>
                            <td class="pm_list">
                                <%#SBinding.getValue(Container.DataItem, "EmpEngSurname")%>
                            </td>
                            <td class="pm_list">
                                <%#SBinding.getValue(Container.DataItem,"EmpEngOtherName")%>
                            </td>
                            <td class="pm_list">
                                <%#SBinding.getValue(Container.DataItem,"EmpAlias")%>
                            </td>
                            <td class="pm_list">
                                <%#SBinding.getValue(Container.DataItem,"EmpStatus")%>
                            </td>                        
                            <td class="pm_list" align="center">
                                 <%#SBinding.getValue(Container.DataItem,"AuthorizerIsReadOnly")%>
                            </td>
                            <td class="pm_list" align="center">
                                 <%#SBinding.getValue(Container.DataItem,"AuthorizerSkipEmailAlert")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <tb:RecordListFooter id="ListFooter" runat="server"
                OnFirstPageClick="FirstPage_Click"
                OnPrevPageClick="PrevPage_Click"
                OnNextPageClick="NextPage_Click"
                OnLastPageClick="LastPage_Click"
                ListOrderBy="EmpNo"
                ListOrder="true" 
            />
    </asp:Panel> 
    </ContentTemplate>
    </asp:UpdatePanel> 
</asp:Content> 