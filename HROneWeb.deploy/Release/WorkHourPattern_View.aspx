<%@ page language="C#" masterpagefile="~/MainMasterPage.master" autoeventwireup="true" inherits="WorkHourPattern_View, HROneWeb.deploy" viewStateEncryptionMode="Always" %>
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
                    <asp:Label ID="Label2" runat="server" Text="View Work Hour Pattern" />:
                    <%=WorkHourPatternCode.Text%>
                </td>
            </tr>
        </table>
        
            
                
        
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar id="toolBar" runat="server"
                     NewButton_Visible="false" 
                     SaveButton_Visible="false" 
                     OnBackButton_Click="Back_Click"
                     OnEditButton_Click ="Edit_Click"
                     OnDeleteButton_Click="Delete_Click"
                    />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openAlert(); return false;"/>
                </td>
            </tr>
        </table>
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="30%"/>
            <col width="70%"/>
            <col width="30%"/>
            <col width="70%"/>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label3" runat="server" Text="Code" />:</td>
                <td class="pm_field" >
                    <asp:Label ID="WorkHourPatternCode" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label4" runat="server" Text="Description" />:</td>
                <td class="pm_field">
                    <asp:Label ID="WorkHourPatternDesc" runat="Server" /></td>
                    
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label15" runat="server" Text="Working hour determine method" />:</td>
                <td class="pm_field">
                    <asp:Label ID="WorkHourPatternWorkDayDetermineMethod" runat="Server" /></td>
            </tr>
            <tr runat="server" id="WorkHourPerDayRow">
                <td class="pm_field_header">
                    <asp:Label ID="Label16" runat="server" Text="Default Working hour per day" />:</td>
                <td class="pm_field">
                    <asp:Label ID="WorkHourPatternContractWorkHoursPerDay" runat="Server" /></td>
            </tr>
            <tr runat="server" id="LunchTimePerDayRow">
                <td class="pm_field_header">
                    <asp:Label ID="Label17" runat="server" Text="Default meal break hour per day" />:</td>
                <td class="pm_field">
                    <asp:Label ID="WorkHourPatternContractLunchTimeHoursPerDay" runat="Server" /></td>
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
                <td class="pm_list_header" runat="server" id="WorkHoursPerDayHeaderCell">
                    <asp:Label ID="Label18" runat="server" Text="Working hour per day" />
                </td>
                <td class="pm_list_header" runat="server" id="LunchTimeHoursPerDayHeaderCell">
                    <asp:Label ID="Label19" runat="server" Text="Meal break hour per day" />
                </td>
            </tr>
            <tr >
                <td class="pm_field_header">
                    <asp:Label ID="Label5" runat="server" Text="Sunday" />:</td>
                <td class="pm_field">
                    <asp:Label ID="WorkHourPatternSunDefaultRosterCodeID" runat="Server" />
                    <asp:Label ID="WorkHourPatternSunDefaultDayUnit" runat="Server" />
                </td>
                <td class="pm_field" runat="server" id="WorkHoursPerDaySunCell">
                    <asp:Label ID="WorkHourPatternSunWorkHoursPerDay" runat="Server" />
                </td>
                <td class="pm_field" runat="server" id="LunchTimeHoursPerDaySunCell">
                    <asp:Label ID="WorkHourPatternSunLunchTimeHoursPerDay" runat="Server" />
                </td>
            </tr>
            <tr >
                <td class="pm_field_header">
                    <asp:Label ID="Label6" runat="server" Text="Monday" />:</td>
                <td class="pm_field">
                    <asp:Label ID="WorkHourPatternMonDefaultRosterCodeID" runat="Server" />
                    <asp:Label ID="WorkHourPatternMonDefaultDayUnit" runat="Server" />
                </td>
                <td class="pm_field" runat="server" id="WorkHoursPerDayMonCell">
                    <asp:Label ID="WorkHourPatternMonWorkHoursPerDay" runat="Server" />
                </td>
                <td class="pm_field" runat="server" id="LunchTimeHoursPerDayMonCell">
                    <asp:Label ID="WorkHourPatternMonLunchTimeHoursPerDay" runat="Server" />
                </td>
            </tr>
            <tr >
                <td class="pm_field_header">
                    <asp:Label ID="Label7" runat="server" Text="Tuesday" />:</td>
                <td class="pm_field">
                    <asp:Label ID="WorkHourPatternTueDefaultRosterCodeID" runat="Server" />
                    <asp:Label ID="WorkHourPatternTueDefaultDayUnit" runat="Server" />
                </td>
                <td class="pm_field" runat="server" id="WorkHoursPerDayTueCell">
                    <asp:Label ID="WorkHourPatternTueWorkHoursPerDay" runat="Server" />
                </td>
                <td class="pm_field" runat="server" id="LunchTimeHoursPerDayTueCell">
                    <asp:Label ID="WorkHourPatternTueLunchTimeHoursPerDay" runat="Server" />
                </td>
            </tr>
            <tr >
                <td class="pm_field_header">
                    <asp:Label ID="Label8" runat="server" Text="Wednesday" />:</td>
                <td class="pm_field">
                    <asp:Label ID="WorkHourPatternWedDefaultRosterCodeID" runat="Server" />
                    <asp:Label ID="WorkHourPatternWedDefaultDayUnit" runat="Server" />
                </td>
                <td class="pm_field" runat="server" id="WorkHoursPerDayWedCell">
                    <asp:Label ID="WorkHourPatternWedWorkHoursPerDay" runat="Server" />
                </td>
                <td class="pm_field" runat="server" id="LunchTimeHoursPerDayWedCell">
                    <asp:Label ID="WorkHourPatternWedLunchTimeHoursPerDay" runat="Server" />
                </td>
            </tr>
            <tr >
                <td class="pm_field_header">
                    <asp:Label ID="Label9" runat="server" Text="Thursday" />:</td>
                <td class="pm_field">
                    <asp:Label ID="WorkHourPatternThuDefaultRosterCodeID" runat="Server" />
                    <asp:Label ID="WorkHourPatternThuDefaultDayUnit" runat="Server" />
                </td>
                <td class="pm_field" runat="server" id="WorkHoursPerDayThuCell">
                    <asp:Label ID="WorkHourPatternThuWorkHoursPerDay" runat="Server" />
                </td>
                <td class="pm_field" runat="server" id="LunchTimeHoursPerDayThuCell">
                    <asp:Label ID="WorkHourPatternThuLunchTimeHoursPerDay" runat="Server" />
                </td>
            </tr>
            <tr >
                <td class="pm_field_header">
                    <asp:Label ID="Label10" runat="server" Text="Friday" />:</td>
                <td class="pm_field">
                    <asp:Label ID="WorkHourPatternFriDefaultRosterCodeID" runat="Server" />
                    <asp:Label ID="WorkHourPatternFriDefaultDayUnit" runat="Server" />
                </td>
                <td class="pm_field" runat="server" id="WorkHoursPerDayFriCell">
                    <asp:Label ID="WorkHourPatternFriWorkHoursPerDay" runat="Server" />
                </td>
                <td class="pm_field" runat="server" id="LunchTimeHoursPerDayFriCell">
                    <asp:Label ID="WorkHourPatternFriLunchTimeHoursPerDay" runat="Server" />
                </td>
            </tr>
            <tr >
                <td class="pm_field_header">
                    <asp:Label ID="Label11" runat="server" Text="Saturday" />:</td>
                <td class="pm_field">
                    <asp:Label ID="WorkHourPatternSatDefaultRosterCodeID" runat="Server" />
                    <asp:Label ID="WorkHourPatternSatDefaultDayUnit" runat="Server" />
                </td>
                <td class="pm_field" runat="server" id="WorkHoursPerDaySatCell">
                    <asp:Label ID="WorkHourPatternSatWorkHoursPerDay" runat="Server" />
                </td>
                <td class="pm_field" runat="server" id="LunchTimeHoursPerDaySatCell">
                    <asp:Label ID="WorkHourPatternSatLunchTimeHoursPerDay" runat="Server" />
                </td>
            </tr>
            <tr >
                <td class="pm_field_header">
                    <asp:Label ID="Label13" runat="server" Text="Use Public Holiday Table" />:</td>
                <td class="pm_field">
                    <asp:CheckBox ID="WorkHourPatternUsePublicHolidayTable" runat="server" Enabled="false" />
                    <asp:Label ID="WorkHourPatternPublicHolidayDefaultRosterCodeID" runat="Server" />
                </td>
            </tr>
            <tr >
                <td class="pm_field_header">
                    <asp:Label ID="Label14" runat="server" Text="Use Statutory Holiday Table" />:</td>
                <td class="pm_field">
                    <asp:CheckBox ID="WorkHourPatternUseStatutoryHolidayTable" runat="server" Enabled="false"  />
                    <asp:Label ID="WorkHourPatternStatutoryHolidayDefaultRosterCodeID" runat="Server" />
                </td>
            </tr>
        </table>
        </asp:PlaceHolder>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
        </asp:UpdatePanel >
</asp:Content>

