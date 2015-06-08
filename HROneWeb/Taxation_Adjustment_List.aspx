<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Taxation_Adjustment_List.aspx.cs"    Inherits="Taxation_Adjustment_List"  MasterPageFile="~/MainMasterPage.master" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
<%@ Register Src="~/controls/EmployeeSearchControl.ascx" TagName="EmployeeSearchControl" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">

        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Taxation Adjustment" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Taxation Adjustment" />
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="TaxFormUpdatePanel" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="TaxCompID" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="TaxFormType" EventName="SelectedIndexChanged" />
        </Triggers>

        <ContentTemplate >
        <table width="100%" class="pm_section">
            <col width="30%" />
            <col width="70%" />
            <tr>
                <td class="pm_field_header" >
                    <asp:Label runat="server" Text="Taxation Company" />:
                </td>
                <td class="pm_field">
                    <asp:DropDownList ID="TaxCompID" runat="server" AutoPostBack="True" OnSelectedIndexChanged="TaxCompID_SelectedIndexChanged" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Taxation Form Type" />:
                </td>
                <td class="pm_field">IR56
                    <asp:DropDownList ID="TaxFormType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="TaxFormType_SelectedIndexChanged" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Year" />:
                </td>
                <td class="pm_field">
                        <asp:DropDownList runat="server" ID="TaxFormID" AutoPostBack="true" OnSelectedIndexChanged="TaxFormID_SelectedIndexChanged" />
                </td>
            </tr>
        </table>
        </ContentTemplate> 
        </asp:UpdatePanel> 
        <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Search" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="Reset" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="TaxCompID" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="TaxFormType" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="TaxFormID" EventName="SelectedIndexChanged" />
            
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
        <ContentTemplate >
            <asp:Panel ID="panelEmpList" runat="server">
            <uc2:EmployeeSearchControl id="EmployeeSearchControl1" runat="server" />
            <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
                <tr>
                    <td>
                        <asp:Button ID="Search" runat="server" Text="Search" CssClass="button" OnClick="Search_Click" />
                        <asp:Button ID="Reset" runat="server" Text="Reset" CssClass="button" OnClick="Reset_Click" />
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                <tr>
                    <td>
                        <asp:Label runat="server" Text="Employee List" />
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
                     NewButton_Visible ="false"
                     OnDeleteButton_Click="Delete_Click"
                      />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
                </td>
            </tr>
        </table>
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
                    <td class="pm_list_header" align="center" >
                        <asp:Panel ID="SelectAllPanel" runat="server">
                        <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
                        </asp:Panel>
                    </td>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="_EmpNo" OnClick="ChangeOrder_Click" Text="EmpNo" />
                    </td>
                    <td align="left" class="pm_list_header" colspan="2" >
                        <asp:LinkButton runat="server" ID="_EmpEngFullName" OnClick="ChangeOrder_Click" Text="Name" />
                    </td>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="_EmpAlias" OnClick="ChangeOrder_Click" Text="Alias" />
                    </td>
                </tr>
                <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td class="pm_list" align="center">
                                <asp:CheckBox ID="ItemSelect" runat="server" />
                                <input type="hidden" runat="server" id="TaxEmpID" />
                            </td>
                            <td class="pm_list">
                                <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Taxation_Adjustment_View.aspx?TaxEmpID=" + sbinding.getValue(Container.DataItem,"TaxEmpID"))%>">
                                    <%# string.IsNullOrEmpty( sbinding.getValue(Container.DataItem, "EmpNo") )? "#DELETED" : sbinding.getValue(Container.DataItem, "EmpNo")%>
                                </a>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem, "EmpEngSurname")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem, "EmpEngOtherName")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem, "EmpAlias")%>
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
        </asp:Panel>
        </ContentTemplate> 
        </asp:UpdatePanel> 
</asp:Content> 