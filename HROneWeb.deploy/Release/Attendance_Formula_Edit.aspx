<%@ page language="C#" autoeventwireup="true" inherits="Attendance_Formula_Edit, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>

<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="AttendanceFormulaID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label Text="Attendance Formula Setup" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <%=CurID<=0?HROne.Common.WebUtility.GetLocalizedString("Add"):HROne.Common.WebUtility.GetLocalizedString("Edit") %>
                  <asp:Label Text="Attendance Formula" runat="server" />:
                    <%=AttendanceFormulaCode.Text %>
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
        <ContentTemplate >
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="15%" />
            <col width="35%" />
            <col width="15%" />
            <col width="35%" />
            <tr>
                <td class="pm_field_header" >
                    <asp:Label Text="Code " runat="server" />:</td>
                <td class="pm_field" >
                    <asp:TextBox ID="AttendanceFormulaCode" runat="Server" /></td>
                <td class="pm_field_header" >
                    <asp:Label ID="Label1" Text="Description " runat="server" />:</td>
                <td class="pm_field"  >
                    <asp:TextBox ID="AttendanceFormulaDesc" runat="Server" /></td>                    
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label4" Text="Type " runat="server" />:</td>                        
                <td class="pm_field" colspan="3">
                    <asp:DropDownList  ID="AttendanceFormulaType" runat="Server" AutoPostBack="true" /></td>
            </tr>
            <asp:Panel ID="FormulaPanel" runat="server" >                                           
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label2" Text="Formula for Daily Prorata" runat="server" />:</td>                        
                <td class="pm_field" colspan="3">
                    <asp:DropDownList  ID="AttendanceFormulaPayFormID" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Formula for Hourly prorata" runat="server" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:Label Text="Daily Payment" runat="server" />/ 
                    <asp:TextBox ID="AttendanceFormulaWorkHourPerDay" runat="Server" style=" text-align:right;" />
                    <asp:Label ID="Label21" Text="hour(s)" runat="server" />   
                    <asp:CheckBox ID="AttendanceFormulaIsUseRosterCodeForDefaultWorkHourPerDay" runat="server" Text="Use settings under Roster Code if available" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="lblRoundingRule" runat="server" Text="Rounding Rule" />  :</td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="AttendanceFormulaRoundingRule" runat="server" />
                    <asp:DropDownList ID="AttendanceFormulaDecimalPlace" runat="server" />
                    <asp:Label ID="Label7" runat="server" Text="decimal place(s)" />
                </td>
            </tr>
            </asp:Panel> 
            <asp:Panel ID="FixOTPanel" runat="server" >  
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Rate" runat="server" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:TextBox ID="AttendanceFormulaFixedRate" runat="Server" />
                    <asp:Label ID="Unit" runat="server" Text="per hour" />
                </td>
            </tr>
            </asp:Panel> 
        </table>
        </ContentTemplate>
        </asp:UpdatePanel >
</asp:Content> 