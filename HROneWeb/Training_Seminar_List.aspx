<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Training_Seminar_List.aspx.cs"    Inherits="Training_Seminar_List" MasterPageFile="~/MainMasterPage.master"  %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>


<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Training Seminar Setup" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Training Seminar Search" />
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
                    <asp:Label runat="server" Text="Training Course" />:
                </td>
                <td class="pm_search">
                    <asp:DropDownList runat="server" ID="TrainingCourseID" />
                </td>
            </tr>
            <tr>
                <td class="pm_search_header">
                    <asp:Label runat="server" Text="Trainer" />:
                </td>
                <td class="pm_search">
                    <asp:TextBox runat="server" ID="TrainingSeminarTrainer" />
                </td>
            </tr>
            <tr>
                <td class="pm_search_header">
                    <asp:Label ID="Label8" runat="server" Text="Training Date" />:
                </td>
                <td class="pm_search">
                    <uc1:WebDatePicker id="TrainingSeminarDateFrom" runat="server" />
                    <asp:Label ID="Label3" runat="Server" Text =" - " />
                    <uc1:WebDatePicker id="TrainingSeminarDateTo" runat="server" />
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
        <br /><br />
		<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Training Seminar List" />
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
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
        <ContentTemplate >
        <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
            <col width="26px" />
            <col width="200px" />
            <col />
            <col width="100px" />
            <col width="100px" />
            <tr>
                <td class="pm_list_header">
                </td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_TrainingCourseID" OnClick="ChangeOrder_Click" Text="Training Course"/>
                </td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_TrainingSeminarDesc" OnClick="ChangeOrder_Click" Text="Description"/>
                </td>
                <td align="left" class="pm_list_header" colspan="2">
                    <asp:LinkButton runat="server" ID="_TrainingSeminarDateFrom" OnClick="ChangeOrder_Click" Text="Training Date"/>
                </td>
            </tr>
            <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_list" align="center">
                            <asp:CheckBox ID="ItemSelect" runat="server" />
                        </td>
                        <td class="pm_list">
                            <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Training_Seminar_View.aspx?TrainingSeminarID=" + binding.getValue(Container.DataItem,"TrainingSeminarID"))%>">
                                <%#binding.getValue(Container.DataItem, "TrainingCourseID")%>
                            </a>
                        </td>
                        <td class="pm_list">
                            <%#binding.getValue(Container.DataItem, "TrainingSeminarDesc")%>
                        </td>
                        <td class="pm_list">
                            <%#binding.getFValue(Container.DataItem, "TrainingSeminarDateFrom","yyyy-MM-dd")%>
                        </td>
                        <td class="pm_list">
                            <%#binding.getFValue(Container.DataItem, "TrainingSeminarDateTo","yyyy-MM-dd")%>
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