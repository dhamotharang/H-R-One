<%@ page language="C#" autoeventwireup="true" inherits="Payroll_BonusProcess_View, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>
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
                    <asp:Label Text="View Bonus Process " runat="server" />:
                    <%=BonusProcessDesc.Text %>
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
                    <asp:Button ID="btnConfirmAndSeal" runat="server" CssClass="red_button" Text="Confirm Bonus Process" OnClick="btnConfirmProcess_Click" />
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
                </td>
            </tr>
        </table>
       <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
        <ContentTemplate> --%>
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
                    <asp:Label id="BonusProcessMonth" runat="server" />
                </td>
			</tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label Text="Description" runat="server"/>:
                </td>
                <td class="pm_field">
                    <asp:Label ID="BonusProcessDesc" runat="Server" />
                </td>
            </tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label2" Text="Cover Period" runat="server"/>:
                </td>
                <td class="pm_field">
                    <asp:Label ID="BonusProcessPeriodFr" runat="Server" />&nbsp&nbsp to &nbsp&nbsp
                    <asp:Label ID="BonusProcessPeriodTo" runat="Server" />
                </td>
            </tr>			
            <tr>
                <td class="pm_field_header" >
                    <asp:Label Text="Salary Month For Calculations" runat="server"/>:
                </td>
                <td class="pm_field">
                    <asp:Label id="BonusProcessSalaryMonth" runat="server" />
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
                    <asp:Label ID="BonusProcessPayCode" runat="server" />
                </td>
            </tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label Text="Payment Date" runat="server"/>:
                </td>
                <td class="pm_field">
                    <asp:Label id="BonusProcessPayDate" runat="server" />
                </td>
            </tr>
            <tr id="BasicInfoCommands" runat="server">
                <td colspan="2">
                    <%--<asp:Button id="btnConfirmProcess" runat="server"  OnClick="btnConfirmProcess_Click" Text="Confirm Bonus Process" CssClass="button"/>--%>
                </td>
            </tr>
		</table>

        <br />        
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="30%" />
            <col width="20%" />
            <col width="50%" />
            <tr>
                <td class="pm_field_title"  colspan="3">
                   <asp:Label runat="server" Text="Part 1 - Standard Bonus" />
                </td>
			</tr>
			<tr>
                <td class="pm_field_header" >
                    <asp:Label Text="Uploaded Count" runat="server"/>:
                </td>
                <td class="pm_field" colspan="2">
			        <asp:Label id="Part1DataCount" runat="server" text="0"/>
                </td>
			</tr>
			<tr>
                <td class="pm_field_header" >
                    <asp:Label Text="Standard Bonus Rate" runat="server"/>:
                </td>
                <td class="pm_field" colspan="2">
                    <asp:Label id="BonusProcessStdRate" runat="server" />
                    <asp:Label runat="server" text="month(s)" />
                </td>
			</tr>
			<tr id="Part1Commands" runat="server">
			    <td colspan="2">
                    <asp:Button id="btnGeneratePart1Data" runat="server"  OnClick="btnGeneratePart1Data_Click" Text="Generate Data" CssClass="button"/>
                    <asp:Button id="btnExportPart1Template" runat="server"  OnClick="btnExportPart1Template_Click" Text="Export Template" CssClass="button"/>

                    <asp:Button id="btnImportPart1Template" runat="server" OnClick="btnImportPart1Template_Click" text="Import Data" CssClass="button" Visible="false" />
			    </td>
			    <td align="right">
                    <asp:Button id="btnClearPart1Data" runat="server" OnClick="btnClearPart1Data_Click" text="Clear Uploaded" CssClass="button" />
			    </td>

			</tr>
		</table>
        <br />        
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="20%" />
            <col width="10%" />
            <col width="10%" />
            <col width="20%" />
            <col width="20%" />
            <col width="20%" />
    		<tr>
                <td class="pm_field_title"  colspan="6">
                   <asp:Label runat="server" Text="Part 2 - Discretionary Bonus" />
                </td>
			</tr>
			<tr>
                <td class="pm_field_header"  colspan="2">
                    <asp:Label Text="Uploaded Count" runat="server"/>:
                </td>
                <td class="pm_field" colspan="4">
			        <asp:Label id="Part2DataCount" runat="server" text="0"/>
                </td>
			</tr>
    		<tr>
                <td class="pm_field_header">
                   <asp:Label runat="server" Text="Rank 1" />
                </td>
                <td class="pm_field_header" colspan="2">
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
                    <asp:Label id="BonusProcessRank1" runat="server" />
                    <asp:Label runat="server" text="%" />
                </td>
                <td class="pm_field" colspan="2">
                    <asp:Label id="BonusProcessRank2" runat="server" />
                    <asp:Label runat="server" text="%" />
                </td>
                <td class="pm_field">
                    <asp:Label id="BonusProcessRank3" runat="server" />
                    <asp:Label runat="server" text="%" />
                </td>
                <td class="pm_field">
                    <asp:Label id="BonusProcessRank4" runat="server" />
                    <asp:Label runat="server" text="%" />
                </td>
                <td class="pm_field">
                    <asp:Label id="BonusProcessRank5" runat="server" />
                    <asp:Label runat="server" text="%" />
                </td>
			</tr>
			<tr id="Part2Commands" runat="server">
			    <td colspan="4">
                    <asp:Button id="btnExportPart2Template" runat="server"  OnClick="btnExportPart2Template_Click" Text="Generate Template" CssClass="button"/>
                    <asp:Button id="btnImportPart2Template" runat="server" OnClick="btnImportPart2Template_Click" text="Import Data" CssClass="button" />
			    </td>
			    <td colspan="2" align="right">
                    <asp:Button id="btnClearPart2Data" runat="server" OnClick="btnClearPart2Data_Click" text="Clear Uploaded" CssClass="button" />			    </td>			    			        			    
			</tr>
		</table>

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
                    <asp:Button id="btnGenerateCND" runat="server" OnClick="btnGenerateCND_Click" text="Generate Claims and Deduction" CssClass="button" />
			    </td>
			</tr>
		</table> 
<%--    </asp:PlaceHolder>
    </ContentTemplate> 
    </asp:UpdatePanel> --%>
</asp:Content> 