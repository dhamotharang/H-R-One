<%@ page language="C#" autoeventwireup="true" inherits="Training_Seminar_View, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>

<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <input type="hidden" id="TrainingSeminarID" runat="server" name="ID" />
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
                <asp:Label runat="server" Text="View Training Seminar" />
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
    <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
        <col width="15%" />
        <col width="35%" />
        <col width="15%" />
        <col width="35%" />
        <tr>
            <td class="pm_field_header">
                <asp:Label runat="server" Text="Name" />:</td>
            <td class="pm_field" colspan="3">
                <asp:Label ID="TrainingCourseID" runat="Server" />
            </td>
        </tr>
        <tr>
            <td class="pm_field_header">
                <asp:Label ID="Label5" runat="server" Text="Description" />:</td>
            <td class="pm_field" colspan="3">
                <asp:Label ID="TrainingSeminarDesc" runat="Server" />
            </td>
        </tr>
        <tr>
            <td class="pm_field_header">
                <asp:Label runat="server" Text="Date" />:</td>
            <td class="pm_field">
                <asp:Label ID="TrainingSeminarDateFrom" runat="Server" />
                <asp:Label ID="Label3" runat="Server" Text=" - " />
                <asp:Label ID="TrainingSeminarDateTo" runat="Server" />
            </td>
            <td class="pm_field_header">
                <asp:Label ID="Label1" runat="server" Text="Duration" />:</td>
            <td class="pm_field">
                <asp:Label ID="TrainingSeminarDuration" runat="Server" />
                <asp:Label ID="TrainingSeminarDurationUnit" runat="Server" />
            </td>
        </tr>
        <tr>
            <td class="pm_field_header">
                <asp:Label ID="Label2" runat="server" Text="Trainer" />:</td>
            <td class="pm_field" colspan="3">
                <asp:Label ID="TrainingSeminarTrainer" runat="Server" />
            </td>
        </tr>
    </table>
    <asp:Panel ID="TrainingSeminarEnrollPanel" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="Label4" runat="server" Text="Enroll Training Seminar" />
                </td>
            </tr>
        </table>
        <asp:Button ID="btnEnroll" runat="server" Text="Enroll" CssClass="button" OnClick="btnEnroll_Click" />
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
                    <asp:LinkButton runat="server" ID="_EmpNo" OnClick="ChangeOrder_Click" Text="EmpNo" /></td>
                <td align="left" class="pm_list_header" colspan="2">
                    <asp:LinkButton runat="server" ID="_EmpEngFullName" OnClick="ChangeOrder_Click" Text="Name" /></td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_EmpAlias" OnClick="ChangeOrder_Click" Text="Alias" /></td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_EmpStatus" OnClick="ChangeOrder_Click" Text="Status" /></td>
            </tr>
            <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_list" align="center">
                            <asp:CheckBox ID="ItemSelect" runat="server" />
                        </td>
                        <td class="pm_list">
                            <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Emp_View.aspx?EmpID=" + trainingEnrollBinding.getValue(Container.DataItem,"EmpID"))%>">
                                <%#trainingEnrollBinding.getValue(Container.DataItem, "EmpNo")%>
                            </a>
                        </td>
                        <td class="pm_list">
                            <%#trainingEnrollBinding.getValue(Container.DataItem, "EmpEngSurname")%>
                        </td>
                        <td class="pm_list">
                            <%#trainingEnrollBinding.getValue(Container.DataItem, "EmpEngOtherName")%>
                        </td>
                        <td class="pm_list">
                            <%#trainingEnrollBinding.getValue(Container.DataItem, "EmpAlias")%>
                        </td>
                        <td class="pm_list">
                            <%#trainingEnrollBinding.getValue(Container.DataItem, "EmpStatus")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <tb:RecordListFooter ID="ListFooter" runat="server" 
            OnFirstPageClick="FirstPage_Click"
            OnPrevPageClick="PrevPage_Click" 
            OnNextPageClick="NextPage_Click" 
            OnLastPageClick="LastPage_Click" 
        />
    </asp:Panel>
</asp:Content>
