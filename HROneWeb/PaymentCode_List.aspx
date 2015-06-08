<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PaymentCode_List.aspx.cs"    Inherits="PaymentCode_List" MasterPageFile="~/MainMasterPage.master" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Payment Code Setup" />
                </td>
            </tr>
        </table>
        
            
                
        
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Payment Code Search" />
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="SearchUpdatePanel" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Reset" EventName="Click" />
        </Triggers>

        <ContentTemplate >
        <table width="100%" class="pm_section">
            <tr>
                <td class="pm_search_header">
                    <asp:Label runat="server" Text="Payment Code" />:
                </td>
                <td class="pm_search">
                    <asp:TextBox runat="server" ID="PaymentCode" /></td>
                <td class="pm_search_header">
                    <asp:Label runat="server" Text="Description" />:
                </td>
                <td class="pm_search">
                    <asp:TextBox runat="server" ID="PaymentCodeDesc" /></td>
            </tr>
            <tr>
                <td class="pm_search_header">
                    <asp:Label runat="server" Text="Payment Type" />:
                </td>
                <td class="pm_search">
                    <asp:DropDownList runat="server" ID="PaymentTypeID" /></td>
            </tr>
            <tr>
                <td class="pm_search_header">
                    <asp:Label runat="server" Text="Is MPF" />:
                </td>
                <td class="pm_search">
                    <asp:DropDownList runat="server" ID="PaymentCodeIsMPF" />
                </td>
                <td class="pm_search_header">
                    <asp:Label runat="server" Text="Is Wages" />:
                </td>
                <td class="pm_search">
                    <asp:DropDownList runat="server" ID="PaymentCodeIsWages" /></td>
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
        <br /><br />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Payment Code List" />
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
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_list_section">
            <col width="26px" />
            <col width="150px" />
            <col width="250px" />
            <col width="350px" />
            <col width="100px" />
            <col width="100px" />
            <col width="100px" />
            <col width="100px" />
            <col width="100px" />
            <tr>
                <td class="pm_list_header" align="center">
                    <asp:Panel ID="SelectAllPanel" runat="server">
                    <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
                    </asp:Panel>                    
                </td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_PaymentCode" OnClick="ChangeOrder_Click" Text="Payment Code" /></td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_PaymentCodeDesc" OnClick="ChangeOrder_Click" Text="Description" /></td>
                <td  class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_PaymentTypeDesc" OnClick="ChangeOrder_Click" Text="Payment Type" /></td>
                <td class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_PaymentCodeIsProrata" OnClick="ChangeOrder_Click" Text="Is Pro-rata" /></td>
                <td class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_PaymentCodeIsWages" OnClick="ChangeOrder_Click" Text="Is Wages" /></td>
                <td class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_PaymentCodeIsMPF" OnClick="ChangeOrder_Click" Text="Is MPF" /></td>
                <td class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_PaymentCodeIsTopUp" OnClick="ChangeOrder_Click" Text="Is Voluntary" /></td>
                <td class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_PaymentCodeIsORSO" OnClick="ChangeOrder_Click" Text="Is P-Fund" /></td>
            </tr>
            <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_list" align="center">
                            <asp:CheckBox ID="ItemSelect" runat="server" />
                        </td>
                        <td class="pm_list">
                            <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "PaymentCode_View.aspx?PaymentCodeID=" + binding.getValue(Container.DataItem,"PaymentCodeID"))%>">
                                <%#binding.getValue(Container.DataItem,"PaymentCode")%>
                            </a>
                        </td>
                        <td class="pm_list">
                            <%#binding.getValue(Container.DataItem,"PaymentCodeDesc")%>
                        </td>
                        <td class="pm_list">
                            <%#binding.getValue(Container.DataItem, "PaymentTypeDesc")%>
                        </td>
                        <td class="pm_list">
                            <%#binding.getValue(Container.DataItem,"PaymentCodeIsProrata")%>
                        </td>
                        <td class="pm_list">
                            <%#binding.getValue(Container.DataItem,"PaymentCodeIsWages")%>
                        </td>
                        <td class="pm_list">
                            <%#binding.getValue(Container.DataItem,"PaymentCodeIsMPF")%>
                        </td>
                        <td class="pm_list">
                            <%#binding.getValue(Container.DataItem,"PaymentCodeIsTopUp")%>
                        </td>
                        <td class="pm_list">
                            <%#binding.getValue(Container.DataItem,"PaymentCodeIsORSO")%>
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