<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Emp_Termination_Edit.aspx.cs"    Inherits="Emp_Termination_Edit" MasterPageFile="~/MainMasterPage.master"  %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/Emp_Common.ascx" TagName="Emp_Common" TagPrefix="uc1" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="EmpTermID" runat="server" name="ID" />
        <input type="hidden" id="EmpID" runat="server" name="ID" />
		<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label Text="Employee Information" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label Text="Termination" runat="server" />
                </td>
            </tr>
        </table>
        
            
                
        
        <uc1:Emp_Common id="Emp_Common1" runat="server"></uc1:Emp_Common><br /> 
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar id="toolBar" runat="server"
                     NewButton_Visible="false" 
                     EditButton_Visible="false" 
                     OnBackButton_Click="Back_Click"
                     OnSaveButton_Click ="Save_Click"
                     DeleteButton_Visible="false"
                      />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
                </td>
            </tr>
        </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false" >
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="toolBar" />
    </Triggers>
    <ContentTemplate >
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Cessation Reason" runat="server" />:</td>
                <td class="pm_field">
                    <asp:DropDownList ID="CessationReasonID" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label Text="Notice Date of Termination" runat="server" />:</td>
                <td class="pm_field">
                    <uc1:WebDatePicker id="EmpTermResignDate" runat="server" OnChanged="EmpTermResignDate_Changed" AutoPostBack="true"   />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Notice Period" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="EmpTermNoticePeriod" runat="Server" OnTextChanged="EmpTermNoticePeriod_TextChanged" AutoPostBack="true"/>
                    <asp:DropDownList ID="EmpTermNoticeUnit" runat="Server" OnSelectedIndexChanged="EmpTermNoticeUnit_SelectedIndexChanged" AutoPostBack="true"/></td>
                <td class="pm_field_header">
                    <asp:Label ID="Label1" Text="Expected Last Employment Date" runat="server" />:
                </td>
                <td class="pm_field">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" >
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="EmpTermResignDate" EventName="Changed" />
                        <asp:AsyncPostBackTrigger ControlID="EmpTermNoticePeriod" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="EmpTermNoticeUnit" EventName="SelectedIndexChanged" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:Label ID="ExpectedLastDate" runat="server" />
                    </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Actual Last Employment Date" runat="server" />:
                </td>
                <td class="pm_field">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" >
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="EmpTermResignDate" EventName="Changed" />
                        <asp:AsyncPostBackTrigger ControlID="EmpTermNoticePeriod" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="EmpTermNoticeUnit" EventName="SelectedIndexChanged" />
                    </Triggers>
                    <ContentTemplate>
                    <uc1:WebDatePicker id="EmpTermLastDate" runat="server" />
                    </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label2" Text="Transfer Company?" runat="server" />:</td>
                <td class="pm_field">
                    <asp:CheckBox runat="server" ID="EmpTermIsTransferCompany"/></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Remark" runat="server" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:TextBox runat="server" ID="EmpTermRemark" TextMode="multiLine" Columns="35"
                        Rows="5" /></td>
            </tr>
        </table>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content> 