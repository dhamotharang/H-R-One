<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Payroll_BonusProcess_List.aspx.cs" Inherits="Payroll_BonusProcess_List"  MasterPageFile="~/MainMasterPage.master"  %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Bonus Process List" />
                </td>
            </tr>
        </table>
<%--        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="MPF Plan Search" />
                </td>
            </tr>
        </table>
--%>
<%--        
        <asp:UpdatePanel ID="SearchUpdatePanel" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Reset" EventName="Click" />
        </Triggers>
--%>
<%--        <ContentTemplate >
        <table width="100%" class="pm_section">
            <tr>
                <td class="pm_search_header">
                    <asp:Label runat="server" Text="Code" />:
                </td>
                <td class="pm_search">
                    <asp:TextBox runat="server" ID="MPFPlanCode" /></td>
                <td class="pm_search_header">
                    <asp:Label runat="server" Text="Description" />:
                </td>
                <td class="pm_search">
                    <asp:TextBox runat="server" ID="MPFPlanDesc" /></td>
            </tr>
            <tr>
                <td class="pm_search_header">
                    <asp:Label ID="Label1" runat="server" Text="Scheme No" />:
                </td>
                <td class="pm_search">
                    <asp:TextBox runat="server" ID="MPFSchemeCode" /></td>
                <td class="pm_search_header">
                    <asp:Label runat="server" Text="Scheme Name" />:
                </td>
                <td class="pm_search">
                    <asp:TextBox runat="server" ID="MPFPlanSchemeName" /></td>
            </tr>
            <tr>
                <td class="pm_search_header">
                    <asp:Label runat="server" Text="Name of Employer" />:
                </td>
                <td class="pm_search">
                    <asp:TextBox runat="server" ID="MPFPlanCompanyName" /></td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Button ID="Search" runat="server" Text="Search" CssClass="button" OnClick="Search_Click" />
                    <asp:Button ID="Reset" runat="server" Text="Reset" CssClass="button" OnClick="Reset_Click" />
                </td>
            </tr>
        </table>
        </ContentTemplate> 
        </asp:UpdatePanel> 
--%>
        <br /><br />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Bonus Process List" />
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
                     OnDeleteButton_Click="Delete_Click"
                     OnNewButton_Click ="New_Click"
                      />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional" >
<%--        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Search" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="Reset" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
--%>
        <ContentTemplate >
        <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
            <col width="26px" />
            <col width="150px" />
            <col width="350px" />
            <col width="150px" />
            <col width="350px" />
            <tr>
                <td class="pm_list_header" align="center">
                    <asp:Panel ID="SelectAllPanel" runat="server">
                        <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
                    </asp:Panel>
                </td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_BonusProcessMonth" OnClick="ChangeOrder_Click" Text="Month"/></td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_BonusProcessDesc" OnClick="ChangeOrder_Click" Text="Description"/></td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_BonusProcessPayDate" OnClick="ChangeOrder_Click" Text="Payment Date"/></td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_BonusProcessPayCode" OnClick="ChangeOrder_Click" Text="Payment Code"/></td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_BonusProcessStatus" OnClick="ChangeOrder_Click" Text="Status"/></td>
            </tr>
            <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_list" align="center">                                
                            <input type="hidden" runat="server" id="BonusProcessID" />
                            <asp:CheckBox ID="ItemSelect" runat="server" />
                        </td>                        
                        <td class="pm_list">
                            <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Payroll_BonusProcess_View.aspx?BonusProcessID=" + binding.getValue(Container.DataItem,"BonusProcessID"))%>" >
                                <%#binding.getValue(Container.DataItem, "BonusProcessMonth2")%>
                            </a>
                        </td>
                        <td class="pm_list">
                            <%#binding.getValue(Container.DataItem, "BonusProcessDesc")%>
                        </td>
                        <td class="pm_list">
                            <asp:Label ID="BonusProcessPayDate" runat="server"/>
                        </td>
                        <td class="pm_list">
                            <asp:Label ID="BonusProcessPayCode" runat="server"/>
                        </td>
                        <td class="pm_list">
                            <asp:Label ID="BonusProcessStatus" runat="server"/>
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