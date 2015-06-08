<%@ Page Language="C#" MasterPageFile="~/MainMasterPage.master" AutoEventWireup="true" CodeFile="WorkHourPattern_Edit.aspx.cs" Inherits="WorkHourPattern_Edit" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" Runat="Server">

        <input type="hidden" id="WorkHourPatternID" runat="server" name="ID" />
		<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Work Hour Pattern Setup" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <%=CurID<=0?HROne.Common.WebUtility.GetLocalizedString("Add"):HROne.Common.WebUtility.GetLocalizedString("Edit") %>
                    <asp:Label ID="Label2" runat="server" Text="Work Hour Pattern" />:
                    <%=WorkHourPatternCode.Text%>
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
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openAlert(); return false;"/>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
        <ContentTemplate >
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="30%"/>
            <col width="70%"/>
            <col width="30%"/>
            <col width="70%"/>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label3" runat="server" Text="Code" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="WorkHourPatternCode" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label4" runat="server" Text="Description" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="WorkHourPatternDesc" runat="Server" /></td>
                    
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label15" runat="server" Text="Working hour determine method" />:</td>
                <td class="pm_field">
                    <asp:DropDownList ID="WorkHourPatternWorkDayDetermineMethod" runat="Server" AutoPostBack="true" OnSelectedIndexChanged="WorkHourPatternWorkDayDeterminMethod_SelectedIndexChanged" /></td>
            </tr>
            <tr runat="server" ID="WorkHourPerDayRow">
                <td class="pm_field_header">
                    <asp:Label ID="Label16" runat="server" Text="Default Working hour per day" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="WorkHourPatternContractWorkHoursPerDay" runat="Server" /></td>
            </tr>
            <tr runat="server" ID="LunchTimePerDayRow">
                <td class="pm_field_header">
                    <asp:Label ID="Label17" runat="server" Text="Default meal break hour per day" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="WorkHourPatternContractLunchTimeHoursPerDay" runat="Server" /></td>
            </tr>
            
        </table> 
        <br />
        <asp:PlaceHolder ID="WorkHourPatternDaySettingsPanel" runat="server" >
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="20%"/>
            <col width="30%"/>
            <col width="25%"/>
            <col width="25%"/>
            <tr >
                <td class="pm_list_header" >
                    <asp:Label ID="Label12" runat="server" Text="" />&nbsp</td>
                <td class="pm_list_header" >
                    <asp:Label ID="lblRosterCodeHeader" runat="server" Text="Roster" />
                    <asp:Label ID="lblDayUnitHeader" runat="server" Text="Day Unit" />
                </td>
                <td class="pm_list_header" runat="server" id="WorkHoursPerDayHeaderCell"  >
                    <asp:Label ID="Label18" runat="server" Text="Working hour per day" />
                    <asp:Button ID="btnAutoFillWorkHourPerDay" runat="server" Text="Auto Fill" CssClass="button" OnClick="btnAutoFillWorkHourPerDay_Click" />
                </td>
                <td class="pm_list_header"  runat="server" id="LunchTimeHoursPerDayHeaderCell"  >
                    <asp:Label ID="Label19" runat="server" Text="Meal break hour per day" />
                    <asp:Button ID="btnAutoFillLunchTimeHoursPerDay" runat="server" Text="Auto Fill" CssClass="button" OnClick="btnAutoFillLunchTimeHoursPerDay_Click"/>
                </td>
            </tr>
            <tr >
                <td class="pm_field_header">
                    <asp:Label ID="Label5" runat="server" Text="Sunday" />:</td>
                <td class="pm_field">
                    <asp:DropDownList ID="WorkHourPatternSunDefaultRosterCodeID" runat="Server" />
                    <asp:TextBox ID="WorkHourPatternSunDefaultDayUnit" runat="Server" />
                </td>
                <td class="pm_field" runat="server" id="WorkHoursPerDaySunCell">
                    <asp:TextBox ID="WorkHourPatternSunWorkHoursPerDay" runat="Server" />
                </td>
                <td class="pm_field" runat="server" id="LunchTimeHoursPerDaySunCell">
                    <asp:TextBox ID="WorkHourPatternSunLunchTimeHoursPerDay" runat="Server" />
                </td>
            </tr>
            <tr >
                <td class="pm_field_header">
                    <asp:Label ID="Label6" runat="server" Text="Monday" />:</td>
                <td class="pm_field">
                    <asp:DropDownList ID="WorkHourPatternMonDefaultRosterCodeID" runat="Server" />
                    <asp:TextBox ID="WorkHourPatternMonDefaultDayUnit" runat="Server" />
                </td>
                <td class="pm_field" runat="server" id="WorkHoursPerDayMonCell">
                    <asp:TextBox ID="WorkHourPatternMonWorkHoursPerDay" runat="Server" />
                </td>
                <td class="pm_field" runat="server" id="LunchTimeHoursPerDayMonCell">
                    <asp:TextBox ID="WorkHourPatternMonLunchTimeHoursPerDay" runat="Server" />
                </td>
            </tr>
            <tr >
                <td class="pm_field_header">
                    <asp:Label ID="Label7" runat="server" Text="Tuesday" />:</td>
                <td class="pm_field">
                    <asp:DropDownList ID="WorkHourPatternTueDefaultRosterCodeID" runat="Server" />
                    <asp:TextBox ID="WorkHourPatternTueDefaultDayUnit" runat="Server" />
                </td>
                <td class="pm_field" runat="server" id="WorkHoursPerDayTueCell">
                    <asp:TextBox ID="WorkHourPatternTueWorkHoursPerDay" runat="Server" />
                </td>
                <td class="pm_field" runat="server" id="LunchTimeHoursPerDayTueCell">
                    <asp:TextBox ID="WorkHourPatternTueLunchTimeHoursPerDay" runat="Server" />
                </td>
            </tr>
            <tr >
                <td class="pm_field_header">
                    <asp:Label ID="Label8" runat="server" Text="Wednesday" />:</td>
                <td class="pm_field">
                    <asp:DropDownList ID="WorkHourPatternWedDefaultRosterCodeID" runat="Server" />
                    <asp:TextBox ID="WorkHourPatternWedDefaultDayUnit" runat="Server" />
                </td>
                <td class="pm_field" runat="server" id="WorkHoursPerDayWedCell">
                    <asp:TextBox ID="WorkHourPatternWedWorkHoursPerDay" runat="Server" />
                </td>
                <td class="pm_field" runat="server" id="LunchTimeHoursPerDayWedCell">
                    <asp:TextBox ID="WorkHourPatternWedLunchTimeHoursPerDay" runat="Server" />
                </td>
            </tr>
            <tr >
                <td class="pm_field_header">
                    <asp:Label ID="Label9" runat="server" Text="Thursday" />:</td>
                <td class="pm_field">
                    <asp:DropDownList ID="WorkHourPatternThuDefaultRosterCodeID" runat="Server" />
                    <asp:TextBox ID="WorkHourPatternThuDefaultDayUnit" runat="Server" />
                </td>
                <td class="pm_field" runat="server" id="WorkHoursPerDayThuCell">
                    <asp:TextBox ID="WorkHourPatternThuWorkHoursPerDay" runat="Server" />
                </td>
                <td class="pm_field" runat="server" id="LunchTimeHoursPerDayThuCell">
                    <asp:TextBox ID="WorkHourPatternThuLunchTimeHoursPerDay" runat="Server" />
                </td>
            </tr>
            <tr >
                <td class="pm_field_header">
                    <asp:Label ID="Label10" runat="server" Text="Friday" />:</td>
                <td class="pm_field">
                    <asp:DropDownList ID="WorkHourPatternFriDefaultRosterCodeID" runat="Server" />
                    <asp:TextBox ID="WorkHourPatternFriDefaultDayUnit" runat="Server" />
                </td>
                <td class="pm_field" runat="server" id="WorkHoursPerDayFriCell">
                    <asp:TextBox ID="WorkHourPatternFriWorkHoursPerDay" runat="Server" />
                </td>
                <td class="pm_field" runat="server" id="LunchTimeHoursPerDayFriCell">
                    <asp:TextBox ID="WorkHourPatternFriLunchTimeHoursPerDay" runat="Server" />
                </td>
            </tr>
            <tr >
                <td class="pm_field_header">
                    <asp:Label ID="Label11" runat="server" Text="Saturday" />:</td>
                <td class="pm_field">
                    <asp:DropDownList ID="WorkHourPatternSatDefaultRosterCodeID" runat="Server" />
                    <asp:TextBox ID="WorkHourPatternSatDefaultDayUnit" runat="Server" />
                </td>
                <td class="pm_field" runat="server" id="WorkHoursPerDaySatCell">
                    <asp:TextBox ID="WorkHourPatternSatWorkHoursPerDay" runat="Server" />
                </td>
                <td class="pm_field" runat="server" id="LunchTimeHoursPerDaySatCell">
                    <asp:TextBox ID="WorkHourPatternSatLunchTimeHoursPerDay" runat="Server" />
                </td>
            </tr>
            <tr >
                <td class="pm_field_header">
                    <asp:Label ID="Label13" runat="server" Text="Use Public Holiday Table" />:</td>
                <td class="pm_field">
                    <asp:CheckBox ID="WorkHourPatternUsePublicHolidayTable" runat="server" />
                    <asp:DropDownList ID="WorkHourPatternPublicHolidayDefaultRosterCodeID" runat="Server" />
                </td>
            </tr>
            <tr >
                <td class="pm_field_header">
                    <asp:Label ID="Label14" runat="server" Text="Use Statutory Holiday Table" />:</td>
                <td class="pm_field">
                    <asp:CheckBox ID="WorkHourPatternUseStatutoryHolidayTable" runat="server" />
                    <asp:DropDownList ID="WorkHourPatternStatutoryHolidayDefaultRosterCodeID" runat="Server" />
                </td>
            </tr>
        </table>
        </asp:PlaceHolder>
        </ContentTemplate>
        </asp:UpdatePanel >
</asp:Content>

