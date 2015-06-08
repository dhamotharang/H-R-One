<%@ page language="C#" autoeventwireup="true" inherits="Payroll_BonusProcess_Edit, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="BonusProcessID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="Label1" Text="Bonus Process Setup" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <%=CurID<=0?HROne.Common.WebUtility.GetLocalizedString("Add"):HROne.Common.WebUtility.GetLocalizedString("Edit") %>
                    <asp:Label Text="Bonus Process " runat="server" />:
                    <%=BonusProcessDesc.Text %>
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
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
        <ContentTemplate> 
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="30%" />
            <col width="70%" />
    		<tr>
                <td class="pm_field_title"  colspan="2">
                   <asp:Label runat="server" Text="Basic Information" />
                </td>
			</tr>
			<tr>
                <td class="pm_field_header" >
                    <asp:Label Text="Month" runat="server"/>:
                </td>
                <td class="pm_field">
                    <uc1:WebDatePicker id="BonusProcessMonth" runat="server"  OnChanged="BonusProcessMonth_Changed" AutoPostBack="true" />
                </td>
			</tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label Text="Description" runat="server"/>:
                </td>
                <td class="pm_field">
                    <asp:TextBox ID="BonusProcessDesc" runat="Server" />
                </td>
            </tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label Text="Cover Period" runat="server"/>:
                </td>
                <td class="pm_field">
                    <uc1:WebDatePicker id="BonusProcessPeriodFr" runat="server" />&nbsp&nbsp to &nbsp&nbsp
                    <uc1:WebDatePicker id="BonusProcessPeriodTo" runat="server" />
                </td>
            </tr>
			<tr>
                <td class="pm_field_header" >
                    <asp:Label Text="Salary Month For Calculations" runat="server"/>:
                </td>
                <td class="pm_field">
                    <uc1:WebDatePicker id="BonusProcessSalaryMonth" runat="server" />
                </td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Status" runat="server"/>:
                </td>
                <td class="pm_field">
                    <asp:Label ID="BonusProcessStatus" runat="server" />-
                    <asp:Label ID="BonusProcessStatusDesc" runat="server" />
                </td>
			</tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label Text="Payment Code" runat="server"/>:
                </td>
                <td class="pm_field">
                    <asp:DropDownList ID="BonusProcessPayCodeID" runat="server" />
                </td>
            </tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label Text="Payment Date" runat="server"/>:
                </td>
                <td class="pm_field">
                    <uc1:WebDatePicker id="BonusProcessPayDate" runat="server" />
                </td>
            </tr>
		</table>


        <br />        
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="30%" />
            <col width="70%" />
    		<tr>
                <td class="pm_field_title"  colspan="2">
                   <asp:Label runat="server" Text="Part 1 - Standard Bonus" />
                </td>
			</tr>
			<tr>
                <td class="pm_field_header" >
                    <asp:Label Text="Standard Bonus Rate" runat="server"/>:
                </td>
                <td class="pm_field">
                    <asp:TextBox id="BonusProcessStdRate" runat="server" />
                    <asp:Label runat="server" Text="month(s)"/>
                </td>
			</tr>
		</table>
        <br />        
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="20%" />
            <col width="20%" />
            <col width="20%" />
            <col width="20%" />
            <col width="20%" />
    		<tr>
                <td class="pm_field_title"  colspan="5">
                   <asp:Label runat="server" Text="Part 2 - Discretionary Bonus" />
                </td>
			</tr>
    		<tr>
                <td class="pm_field_header">
                   <asp:Label runat="server" Text="Rank 1" />
                </td>
                <td class="pm_field_header">
                   <asp:Label runat="server" Text="Rank 2" />
                </td>
                <td class="pm_field_header">
                   <asp:Label runat="server" Text="Rank 3" />
                </td>
                <td class="pm_field_header">
                   <asp:Label runat="server" Text="Rank 4" />
                </td>
                <td class="pm_field_header">
                   <asp:Label runat="server" Text="Rank 5" />
                </td>
			</tr>
			<tr>
                <td class="pm_field">
                    <asp:TextBox id="BonusProcessRank1" runat="server" />
                    <asp:Label runat="server" text="%" />
                </td>
                <td class="pm_field">
                    <asp:TextBox id="BonusProcessRank2" runat="server" />
                    <asp:Label runat="server" text="%" />
                </td>
                <td class="pm_field">
                    <asp:TextBox id="BonusProcessRank3" runat="server" />
                    <asp:Label runat="server" text="%" />
                </td>
                <td class="pm_field">
                    <asp:TextBox id="BonusProcessRank4" runat="server" />
                    <asp:Label runat="server" text="%" />
                </td>
                <td class="pm_field">
                    <asp:TextBox id="BonusProcessRank5" runat="server" />
                    <asp:Label runat="server" text="%" />
                </td>
			</tr>
  
    </asp:PlaceHolder>
    </ContentTemplate> 
    </asp:UpdatePanel> 
</asp:Content> 