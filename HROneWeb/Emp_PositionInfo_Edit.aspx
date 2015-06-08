<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Emp_PositionInfo_Edit.aspx.cs" Inherits="Emp_Position_Edit" MasterPageFile="~/MainMasterPage.master"  %>

<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/Emp_Common.ascx" TagName="Emp_Common" TagPrefix="uc1" %>
<%@ Register Src="~/controls/Emp_PositionInfo_Edit.ascx" TagName="Emp_PositionInfo_Edit_Control" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="Flow" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="Label1" Text="Employee Information" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="ActionHeader" runat="server" Text="Edit" />
                    <asp:Label ID="Label2" Text="Position Information" runat="server" />
                </td>
            </tr>
        </table>
        
             
        
        <uc1:Emp_Common ID="Emp_Common1" runat="server"></uc1:Emp_Common>
        <br />
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar ID="toolBar" runat="server" NewButton_Visible="false" EditButton_Visible="false" OnBackButton_Click="Back_Click" OnSaveButton_Click="Save_Click" OnDeleteButton_Click="Delete_Click" />
                </td>
                <td align="right" >
                    <asp:Button ID="btnLastPositionTerms" runat="server" CssClass="button" Text="Retrive data from last information" OnClick="btnLastPositionTerms_Click"  />
                </td>
                <td align="right" >
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolBar" />
            <asp:AsyncPostBackTrigger ControlID="btnLastPositionTerms" EventName="Click" />
        </Triggers>
        <ContentTemplate >
            <uc1:Emp_PositionInfo_Edit_Control ID="Emp_PositionInfo_Edit1"  runat="server"  />
        </ContentTemplate>
        </asp:UpdatePanel> 
</asp:Content> 