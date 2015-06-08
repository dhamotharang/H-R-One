<%@ page language="C#" autoeventwireup="true" inherits="Customize_AttendancePreparationProcess_View, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="ProcessID" runat="server" name="ID" />
       <input type="hidden" id="AttendancePreparationProcessID" runat="server" name="ID" /> 
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="Label1" Text="Attendance Preparation Process Setup" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label Text="View Attendance Preparation Process" runat="server" />:
                    <%=AttendancePreparationProcessDesc.Text%>
                </td>
            </tr>
        </table>
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar id="toolBar" runat="server"
                     NewButton_Visible="false" 
                     OnEditButton_Click="Edit_Click" 
                     OnBackButton_Click="Back_Click"
                     SaveButton_Visible="false"
                     OnDeleteButton_Click="Delete_Click"
                     />
                </td>

                <td align="right">
                    <asp:Button ID="btnConfirmAndSeal" runat="server" CssClass="red_button" Text="Confirm Process" OnClick="btnConfirmProcess_Click" />
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
                </td>
            </tr>
        </table>
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
                    <asp:Label id="AttendancePreparationProcessMonth" runat="server" />
                </td>
			</tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label Text="Description" runat="server"/>:
                </td>
                <td class="pm_field">
                    <asp:Label ID="AttendancePreparationProcessDesc" runat="Server" />
                </td>
            </tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label2" Text="Cover Period" runat="server"/>:
                </td>
                <td class="pm_field">
                    <asp:Label ID="AttendancePreparationProcessPeriodFr" runat="Server" />&nbsp&nbsp to &nbsp&nbsp
                    <asp:Label ID="AttendancePreparationProcessPeriodTo" runat="Server" />
                </td>
            </tr>			
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Status" runat="server"/>:
                </td>
                <td class="pm_field">
                    <asp:Label ID="AttendancePreparationProcessStatus" runat="server" />-
                    <asp:Label ID="AttendancePreparationProcessStatusDesc" runat="server" />
                </td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Payment Date" runat="server"/>:
                </td>
                <td class="pm_field">
                    <asp:Label id="AttendancePreparationProcessPayDate" runat="server" />
                </td>
            </tr>
            <tr id="BasicInfoCommands" runat="server">
                <td colspan="2">
                    
                </td>
            </tr>
		</table>

        <br />

        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="30%" />
            <col width="70%" />
    		<tr>
                <td class="pm_field_title"  colspan="2">
                   <asp:Label ID="Label3" runat="server" Text="Manuipluate Attendance Data" />
                </td>
			</tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label Text="Uploaded Count" runat="server"/>:
                </td>
                <td class="pm_field">
			        <asp:Label id="UploadCount" runat="server" text="0"/> &nbsp &nbsp &nbsp &nbsp
                    <asp:Button ID="btnClearUploaded" Text="Clear Uploaded" runat="server" CssClass="button" OnClick="btnClearData_Click"  /> 
                </td>
			</tr>
			<tr id="ButtonCommands" runat="server">
			    <td colspan="2">
			        <br />
                    <asp:Button ID="btnExportDataEntrySheet" Text="Export Data Entry Sheet" runat="server" CssClass="button" OnClick="btnExport_Click" /> &nbsp &nbsp
                    <asp:Button ID="btnImportDataEntrySheet" Text="Import Data Entry Sheet" runat="server" CssClass="button" OnClick="Import_Click"/> &nbsp &nbsp 
                    <asp:Button ID="btnGenerateCalculatedReport" Text="Generate Calculated Attendance Record Report" runat="server" CssClass="button" OnClick="GenerateCalculatedReport_Click" Width="400"/>
			    </td>
			</tr>
		</table>         
        <br />
        <br />        
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="30%" />
            <col width="70%" />
    		<tr>
                <td class="pm_field_title"  colspan="2">
                   <asp:Label runat="server" Text="Claims and Deduction" />
                </td>
			</tr>
			<tr>
			    <td colspan="2">
        		<br/>
                    <asp:Button id="btnGenerateCND" runat="server" OnClick="btnGenerateCND_Click" text="Generate Claims and Deduction" CssClass="button" />
			    </td>
			</tr>
		</table> 
</asp:Content> 