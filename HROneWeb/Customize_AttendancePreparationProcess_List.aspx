<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Customize_AttendancePreparationProcess_List.aspx.cs" Inherits="Customize_AttendancePreparationProcess_List"  MasterPageFile="~/MainMasterPage.master"  %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Customized Attendance Preparation Process" />
                </td>
            </tr>
        </table>

        <br /><br />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Attendance Preparation Process List" />
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
        <ContentTemplate >
        <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
            <col width="26px" />
            <col width="150px" />
            <col width="350px" />
            <col width="150px" />
            <col width="200px" />
            <tr>
                <td class="pm_list_header" align="center">
                    <asp:Panel ID="SelectAllPanel" runat="server">
                        <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
                    </asp:Panel>
                </td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_AttendancePreparationProcessMonth" OnClick="ChangeOrder_Click" Text="Month"/></td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_AttendancePreparationProcessDesc" OnClick="ChangeOrder_Click" Text="Description"/></td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_AttendancePreparationProcessPayDate" OnClick="ChangeOrder_Click" Text="Payment Date"/></td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_AttendancePreparationProcessEmpCount" OnClick="ChangeOrder_Click" Text="Employee Count"/></td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_AttendancePreparationProcessStatus" OnClick="ChangeOrder_Click" Text="Batch Status"/></td>
            </tr>
            <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_list" align="center">                                
                            <input type="hidden" runat="server" id="BonusProcessID" />
                            <asp:CheckBox ID="ItemSelect" runat="server" />
                        </td>                        
                        <td class="pm_list">
                            <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Customize_AttendancePreparationProcess_View.aspx?AttendancePreparationProcessID=" + binding.getValue(Container.DataItem,"AttendancePreparationProcessID"))%>" >
                                <%#binding.getValue(Container.DataItem, "AttendancePreparationProcessMonth2")%>
                            </a>
                        </td>
                        <td class="pm_list">
                            <%#binding.getValue(Container.DataItem, "AttendancePreparationProcessDesc")%>
                        </td>
                        <td class="pm_list">
                            <asp:Label ID="AttendancePreparationProcessPayDate" runat="server"/>
                        </td>
                        <td class="pm_list">
                           <%#binding.getValue(Container.DataItem, "AttendancePreparationProcessEmpCount")%> 
                        </td>
                        <td class="pm_list">
                            <asp:Label ID="AttendancePreparationProcessStatus" runat="server"/>
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