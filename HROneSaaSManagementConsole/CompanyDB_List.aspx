<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CompanyDB_List.aspx.cs" Inherits="CompanyDB_List" %>

<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" Runat="Server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="Label2" Text="Company Database" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="Label3" Text="Company Search" runat="server" />
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="SearchUpdatePanel" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Reset" EventName="Click" />
        </Triggers>

        <ContentTemplate >
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="pm_section">
            <tr>
                <td class="pm_search_header">
                    <asp:Label ID="Label4" Text="Client Code" runat="server" />:
                </td>
                <td class="pm_search">
                    <asp:TextBox runat="server" ID="CompanyDBClientCode" />
                </td>
            </tr>
            <tr>
                <td class="pm_search_header">
                    <asp:Label ID="Label5" Text="Remote Profile ID" runat="server" />:
                </td>
                <td class="pm_search">
                    <asp:TextBox runat="server" ID="txtRemoteProfileID" />
                </td>
            </tr>
        </table>
        </ContentTemplate >
        </asp:UpdatePanel>
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <asp:Button ID="Search" runat="server" Text="Search" CssClass="button" OnClick="Search_Click" />
                    <asp:Button ID="Reset" runat="server" Text="Reset" CssClass="button" OnClick="Reset_Click" />
                </td>
            </tr>
        </table>
        <br />
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Database List" />
                </td>
            </tr>
        </table>
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar id="toolBar" runat="server"
                     BackButton_Visible="false"
                     EditButton_Visible="false" 
                     SaveButton_Visible="false" 
                     OnNewButton_Click ="New_Click"
                     OnDeleteButton_Click="Delete_Click"
                      />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
                </td>
            </tr>
        </table>
        
        <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
            <asp:AsyncPostBackTrigger ControlID="Search" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="Reset" EventName="Click" />
        </Triggers>

        <ContentTemplate >
        <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
            <col width="26px" />
            <col width="150px" />
            <col width="150px" />
            <col width="350px"/>
            <col width="150px" />
            <col width="100px" />
            <tr>
                <td class="pm_list_header" align="center">
                    <asp:Panel ID="SelectAllPanel" runat="server">
                    <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
                    </asp:Panel> 
                </td>
                <td align="left" class="pm_list_header" >
                    <asp:LinkButton runat="server" ID="_CompanyDBClientCode" OnClick="ChangeOrder_Click" Text="Code" />
                </td>
                <td align="left" class="pm_list_header"  >
                    <asp:LinkButton runat="server" ID="_CompanyDBClientName" OnClick="ChangeOrder_Click" Text="Company Name" />
                </td>
                <td align="left" class="pm_list_header"  >
                    <asp:LinkButton runat="server" ID="_DBServerID" OnClick="ChangeOrder_Click" Text="Server Code" />
                </td>  
                <td align="left" class="pm_list_header"  >
                    <asp:LinkButton runat="server" ID="_CompanyDBIsActive" OnClick="ChangeOrder_Click" Text="Active" />
                </td>  
            </tr>
            <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_list" align="center">
                            <asp:CheckBox ID="ItemSelect" runat="server" />
                        </td>
                        <td class="pm_list">
                            <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "CompanyDB_Edit.aspx?CompanyDBID=" + binding.getValue(Container.DataItem,"CompanyDBID"))%>">
                            <%#binding.getValue(Container.DataItem, "CompanyDBClientCode")%>
                            </a>
                        </td>
                        <td class="pm_list" >
                            <%#binding.getValue(Container.DataItem, "CompanyDBClientName")%>
                        </td>
                        <td class="pm_list" >
                            <%#binding.getValue(Container.DataItem, "DBServerID")%>
                        </td>
                        <td class="pm_list" >
                            <%#binding.getValue(Container.DataItem, "CompanyDBIsActive")%>
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
            ListOrderBy="CompanyDBClientCode"
            ListOrder="true" 
          />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

