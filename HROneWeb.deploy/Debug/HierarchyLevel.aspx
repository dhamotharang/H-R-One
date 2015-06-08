<%@ page language="C#" autoeventwireup="true" inherits="HierarchyLevel, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>

<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
		<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Hierarchy Level Setup" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Hierarchy Level Setup" />
                </td>
            </tr>
        </table>
        
            
                
        
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar id="toolBar" runat="server"
                     NewButton_Visible="false" 
                     EditButton_Visible="false" 
                     BackButton_Visible="false"
                     SaveButton_Visible ="false"
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
            <asp:AsyncPostBackTrigger ControlID="toolBar"  />
        </Triggers>
        <ContentTemplate >
        <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
            <col width="26px" /> 
            <col width="120px" /> 
            <col width="150px" /> 
            <tr>
                <%-- Start 0000099, KuangWei, 2014-09-28 --%>
                <td class="pm_list_header" align="center">
                    <asp:Panel ID="SelectAllPanel" runat="server">
                    <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','DeleteItem',this.checked);" />
                    </asp:Panel>
                </td>
                <%-- End 0000099, KuangWei, 2014-09-28 --%>
                <td class="pm_list_header">
                </td>                       
<%--                <td class="pm_list_header">
                </td>
--%>                <td align="left" class="pm_list_header" align="left">
                    <asp:LinkButton runat="server" ID="_HLevelCode" OnClick="ChangeOrder_Click" Text="Code" /></td>
                <td align="left" class="pm_list_header" align="left">
                    <asp:LinkButton runat="server" ID="_HLevelDesc" OnClick="ChangeOrder_Click" Text="Description" /></td>
                <td align="left" class="pm_list_header" align="left">
                    <asp:LinkButton runat="server" ID="_HLevelSeqNo" OnClick="ChangeOrder_Click" Text="Hierarchy Level" /></td>
            </tr>
            <tr id="AddPanel" runat="server">
                <td class="pm_list">
                </td>
                <td class="pm_list" align="center">
                    <asp:Button ID="Add" runat="server" Text="Add" CssClass="button" OnClick="Add_Click" /></td>
                <td class="pm_list" align="left">
                    <asp:TextBox ID="HLevelCode" runat="server" /></td>
                <td class="pm_list" align="left">
                    <asp:TextBox ID="HLevelDesc" runat="server" /></td>
                <td class="pm_list" align="left">
                    <asp:TextBox ID="HLevelSeqNo" runat="server" /></td>
            </tr>
            <asp:DataList ID="Repeater" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" ShowFooter="false"
                ShowHeader="false" OnItemCommand="Repeater_ItemCommand" OnItemDataBound="Repeater_ItemDataBound">
                <EditItemTemplate>
                    <tr>
                        <td class="pm_list" align="center">
                            <asp:CheckBox ID="DeleteItem" runat="server" />
                            <input type="hidden" runat="server" id="HLevelID" />
                        </td>
                        <td class="pm_list" align="center">
                            <asp:Button ID="Save" runat="server" Text="Save" CssClass="button" />
                            <asp:Button ID="Cancel" runat="server" Text="Cancel" CssClass="button" />
                        </td>
                        <td class="pm_list">
                            <asp:TextBox ID="HLevelCode" runat="server" />
                        </td>
                        <td class="pm_list">
                            <asp:TextBox ID="HLevelDesc" runat="server" />
                        </td>
                        <td class="pm_list">
                            <asp:TextBox ID="HLevelSeqNo" runat="server" />
                        </td>
                    </tr>
                </EditItemTemplate>
                <ItemTemplate>
                    <tr>
                        <td class="pm_list" align="center">
                            <asp:CheckBox ID="DeleteItem" runat="server" />
                            <input type="hidden" runat="server" id="HLevelID" />
                        </td>
                        <td class="pm_list" align="center">
                            <asp:Button ID="Edit" runat="server" Text="Edit" CssClass="button" />
                        </td>
                        <td class="pm_list">
                            <%#sbinding.getValue(Container.DataItem,"HLevelCode")%>
                        </td>
                        <td class="pm_list">
                            <%#sbinding.getValue(Container.DataItem,"HLevelDesc")%>
                        </td>
                        <td class="pm_list">
                            <%#sbinding.getValue(Container.DataItem,"HLevelSeqNo")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:DataList>
        </table>
        <tb:RecordListFooter id="ListFooter" runat="server"
            ShowAllRecords="true" 
        />
        </ContentTemplate> 
        </asp:UpdatePanel> 
</asp:Content> 