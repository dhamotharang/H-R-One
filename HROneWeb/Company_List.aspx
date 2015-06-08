<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Company_List.aspx.cs" Inherits="Company_List"  MasterPageFile="~/MainMasterPage.master"  %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label Text="Company Setup" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label Text="Company Search" runat="server" />
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
                    <asp:Label Text="Company Code" runat="server" />:
                </td>
                <td class="pm_search">
                    <asp:TextBox runat="server" ID="CompanyCode" /></td>
				<td class="pm_search_header">
                    <asp:Label Text="Company Name" runat="server" />:
                </td>
                <td class="pm_search">
                    <asp:TextBox runat="server" ID="CompanyName" /></td>

            </tr>
            <tr>
                <td class="pm_search_header">
                    <asp:Label Text="Company Address" runat="server" />:
                </td>
                <td class="pm_search">
                    <asp:TextBox runat="server" ID="CompanyAddress" /></td>
                <td class="pm_search_header">
                    <asp:Label Text="Company Contact Person" runat="server" />:
                </td>
                <td class="pm_search">
                    <asp:TextBox runat="server" ID="CompanyContactPerson" /></td>
            </tr>
            <tr>
                <td class="pm_search_header">
                    <asp:Label Text="Company BR No" runat="server" />:
                </td>
                <td class="pm_search">
                    <asp:TextBox runat="server" ID="CompanyBRNo" /></td>
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
                    <asp:Label Text="Company List" runat="server" />
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
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Search" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="Reset" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="toolBar"  />
        </Triggers>

        <ContentTemplate >
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_list_section">
            <col width="26px" />
            <col width="150px" />
            <col width="350px" />
            <col width="150px" />
            <col width="100px" />
            <tr>
                <td class="pm_list_header" align="center">
                    <asp:Panel ID="SelectAllPanel" runat="server">
                    <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
                    </asp:Panel>
                </td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_CompanyCode" OnClick="ChangeOrder_Click" Text="Code"></asp:LinkButton></td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_CompanyName" OnClick="ChangeOrder_Click" Text="Name"></asp:LinkButton></td>
                <td class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_CompanyContactNo" OnClick="ChangeOrder_Click" Text="Phone"></asp:LinkButton></td>
                <td class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_CompanyFaxNo" OnClick="ChangeOrder_Click" Text="Fax"></asp:LinkButton></td>
                <td class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_CompanyBRNo" OnClick="ChangeOrder_Click" Text="BR"></asp:LinkButton></td>
            </tr>
            <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_list" align="center">
                            <asp:CheckBox ID="ItemSelect" runat="server" />
                        </td>
                        <td class="pm_list">
                            <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Company_View.aspx?CompanyID=" + binding.getValue(Container.DataItem,"CompanyID"))%>">
                                <%#binding.getValue(Container.DataItem,"CompanyCode")%>
                            </a>
                        </td>
                        <td class="pm_list">
                            <%#binding.getValue(Container.DataItem,"CompanyName")%>
                        </td>
                        <td class="pm_list">
                            <%#binding.getValue(Container.DataItem,"CompanyContactNo")%>
                        </td>
                        <td class="pm_list">
                            <%#binding.getValue(Container.DataItem,"CompanyFaxNo")%>
                        </td>
                        <td class="pm_list">
                            <%#binding.getValue(Container.DataItem,"CompanyBRNo")%>
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
          />
        </ContentTemplate> 
        </asp:UpdatePanel> 
</asp:Content> 