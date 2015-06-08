<%@ control language="C#" autoeventwireup="true" inherits="LeaveApplicationForm, HROneESS.deploy" %>

<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>

<%-- applicableDays = "" means no value range specified, "0.25, 0.50, 0.75, 1.00" means the appling days of leave must fall on this range--%>
<input type="hidden" id="applicableDays" runat="server" value="" />  
<input type="hidden" id="EmpID" runat="server" />
    <table width="100%" border="0" cellpadding="5" cellspacing="0" class="pm_field_section">
        <col width="15%" />
        <col width="35%" />
        <col width="15%" />
        <col width="35%" />
        <tr >
            <td colspan="4" class="pm_field_title">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td >
                            <asp:Label ID="lblHeader" runat="server" EnableViewState="false" Text="Leave Application" /> :
                        </td>
                        <td align="right">
                            <asp:Button ID="Save" runat="server" EnableViewState="false" Text="Submit" CssClass="button" OnClick="Save_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr >
            <td class="pm_field_header">
                <asp:Label ID="Label1" EnableViewState="false" Text="Leave Code" runat="server" />:
            </td>
            <td class="pm_field" style="background-color:White;" >
                <asp:DropDownList ID="RequestLeaveCodeID" runat="Server" EnableViewState="true" AutoPostBack="true" OnSelectedIndexChanged="RequestLeaveCodeID_SelectedIndexChanged" />
            </td>
            <td class="pm_field_header">
                <asp:Label ID="Label2" EnableViewState="false" Text="Unit" runat="server" />:</td>
            <td class="pm_field" style="background-color:White;">
                <asp:DropDownList ID="RequestLeaveAppUnit" runat="Server" AutoPostBack="true" EnableViewState="true" />
            </td>
        </tr>
        <tr >
            <td class="pm_field_header">
                <asp:Label ID="Label3" EnableViewState="false" Text="Date From" runat="server" />:</td>
            <td class="pm_field" style="background-color:White;" colspan="3">
                <uc1:WebDatePicker id="RequestLeaveAppDateFrom" runat="server" AutoPostBack="true" OnChanged="LeaveAppDateFrom_Changed" />
                <asp:DropDownList ID="RequestLeaveAppDateFromAM" runat="server" AutoPostBack="true" OnSelectedIndexChanged="btnEstimateTotalLeaveDay_Click" />
            </td>                        
        </tr>
        <tr id="LeaveAppDateToPlaceHolder" runat="server"  >
            <td class="pm_field_header">
                <asp:Label ID="Label4" EnableViewState="false" Text="Date To" runat="server" />
                :</td>
            <td class="pm_field" style="background-color:White;" colspan="3" >
                <uc1:WebDatePicker id="RequestLeaveAppDateTo" runat="server" AutoPostBack="true" OnChanged="LeaveAppDateTo_Changed" />
                <asp:DropDownList ID="RequestLeaveAppDateToAM" runat="server" AutoPostBack="true" OnSelectedIndexChanged="btnEstimateTotalLeaveDay_Click" />
            </td>
        </tr>
        <tr id="TimeRow" runat="server" visible="false">
            <td class="pm_field_header">
                <asp:Label ID="Label5" EnableViewState="false" Text="Time" runat="server" />:</td>
            <td class="pm_field" style="background-color:White;" colspan="3">
                <asp:TextBox ID="RequestLeaveAppTimeFrom" runat="Server"  AutoPostBack="true"  />-
                <asp:TextBox ID="RequestLeaveAppTimeTo" runat="Server"  AutoPostBack="true"  />
            </td>
        </tr>
        <tr >
            <td class="pm_field_header">
                <asp:Label ID="Label6" EnableViewState="false" Text="Days Taken" runat="server" />:
            </td>
            <td class="pm_field" style="background-color:White;" colspan="2">
                <asp:TextBox ID="RequestLeaveDays" runat="Server" />
                <asp:Button ID="btnEstimateTotalLeaveDay" runat="server" EnableViewState="false" Text="Total Number of Days" CssClass="button" OnClick="btnEstimateTotalLeaveDay_Click"/>
            </td>
            <td class="pm_field" style="background-color:White;" >
                <asp:Label ID="lblStatutoryHolidayList" runat="Server" />
            </td>
        </tr>
        <tr runat="server" id="HoursClaimPanel" visible="false">
            <td class="pm_field_header">
                <asp:Label ID="lblLeaveAppHours" EnableViewState="false" Text="Hours Claim" runat="server" />:
            </td>
            <td class="pm_field" style="background-color:White;" colspan="3" >
                <asp:TextBox ID="RequestLeaveAppHours" runat="Server" />
            </td>
        </tr>
        <tr runat="server" id="LeaveAMPMPanel" visible="false">
            <td class="pm_field_header">
                <asp:Label EnableViewState="false" Text="Info" runat="server" />:
            </td>
            <td class="pm_field" style="background-color:White;" colspan="3" >
                <asp:Label ID="lblInfo" runat="Server" />
            </td>
        </tr>
        
        <tr >
            <td class="pm_field_header">
                <asp:Label ID="Label7" EnableViewState="false" Text="Remark" runat="server" />:
            </td>
            <td class="pm_field" style="background-color:White;" colspan="3">
                <asp:TextBox ID="RequestLeaveAppRemark" runat="Server" TextMode="MultiLine" Columns="35" Rows="5" />
            </td>
        </tr>
        <tr id="LeaveCodeIsShowMedicalCertOptionPanel" runat="server" >
            <td class="pm_field_header">
               <!-- Start 0000048, Miranda, 2014-06-03 -->
               <asp:Label ID="Label8" EnableViewState="true" Text="Relevant Certificate" runat="server" />:
               <!-- End 0000048, Miranda, 2014-06-03 -->
            </td>
            <td class="pm_field" style="background-color:White;" colspan="3">
                <asp:CheckBox ID="RequestLeaveAppHasMedicalCertificate" runat="Server" />
            </td>
        </tr>
    </table>
