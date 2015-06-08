<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Training_Seminar_Edit.aspx.cs" Inherits="Training_Seminar_Edit" MasterPageFile="~/MainMasterPage.master" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>


<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
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
                    <%=CurID<=0?HROne.Common.WebUtility.GetLocalizedString("Add"):HROne.Common.WebUtility.GetLocalizedString("Edit") %>
                    <asp:Label runat="server" Text="Training Seminar" />
                </td>
            </tr>
        </table>
        
            
                
        
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar id="toolBar" runat="server"
                     NewButton_Visible="false" 
                     EditButton_Visible="false" 
                     OnBackButton_Click="Back_Click"
                     OnSaveButton_Click ="Save_Click"
                     OnDeleteButton_Click="Delete_Click"
                      />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
                </td>
            </tr>
        </table>
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="15%" />
            <col width="45%" />
            <col width="15%" />
            <col width="25%" />
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Name" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="TrainingCourseID" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label5" runat="server" Text="Description" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:TextBox ID="TrainingSeminarDesc" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Date" />:</td>
                <td class="pm_field">
                    <uc1:WebDatePicker id="TrainingSeminarDateFrom" runat="server" />
                    <asp:Label ID="Label3" runat="Server" Text =" - " />
                    <uc1:WebDatePicker id="TrainingSeminarDateTo" runat="server" />
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label1" runat="server" Text="Duration" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="TrainingSeminarDuration" runat="Server" />
                    <asp:DropDownList ID="TrainingSeminarDurationUnit" runat="Server" >
                        <asp:ListItem Value="H" Text="Hour(s)" />
                    </asp:DropDownList> 
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label2" runat="server" Text="Trainer" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:TextBox ID="TrainingSeminarTrainer" runat="Server" />
                </td>
            </tr>
            
        </table>
 
</asp:Content> 