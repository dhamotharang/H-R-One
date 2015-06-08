<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LateWaiveCancelRecord.ascx.cs" Inherits="LateWaiveCancelRecord" %>

<input type="hidden" id="EmpID" runat="server" />
<table width="100%" border="0" cellpadding="5" cellspacing="0" class="pm_field_section">
    <col width="15%" />
    <col width="35%" />
    <col width="15%" />
    <col width="35%" />
    <tr >
        <td colspan="4" class="pm_field_title">
        <table width="100%" cellpadding="0" cellspacing="0">
            <tr>
                <td colspan="3" >
                    <asp:Label ID="lblHeader" runat="server" EnableViewState="false" Text="Cancellation of Late Waive" /> :
                </td>
                <td align="right" >
                    <asp:Button ID="btnCancel" runat="server" EnableViewState="false" Text="Cancel" OnClick="btnCancel_Click" />
                </td>
            </tr>
        </table>
        </td>
    </tr>
    <tr  >
        <td class="pm_field_header">
            <asp:Label ID="Label3" EnableViewState="false" Text="Date" runat="server" />:
        </td>
        <td class="pm_field" style="background-color:White;" colspan="3">
            <asp:Label ID="AttendanceRecordDate" runat="Server"  />
        </td>                        
    </tr>
    <%-- Start 0000112, Miranda, 2015-01-17 --%>
    <tr>
        <td class="pm_field_header">
            <asp:Label ID="Label1" EnableViewState="false" Text="Roster" runat="server" />:
        </td>
        <td class="pm_field" style="background-color:White;" colspan="3">
            <asp:Label ID="RosterCode" runat="Server"  /> - <asp:Label ID="RosterCodeDesc" runat="server" />
        </td>                        
    </tr>
    <tr>
        <td class="pm_field_header">
            <asp:Label ID="Label2" EnableViewState="false" Text="Roster In Time" runat="server" />:
        </td>
        <td class="pm_field" style="background-color:White;">
            <asp:Label ID="RosterCodeInTime" runat="Server"  />
        </td>                        
        <td class="pm_field_header">
            <asp:Label ID="Label6" EnableViewState="false" Text="Roster Out Time" runat="server" />:
        </td>
        <td class="pm_field" style="background-color:White;">
            <asp:Label ID="RosterCodeOutTime" runat="Server"  />
        </td>                        
    </tr>
    <tr>
        <td class="pm_field_header">
            <asp:Label ID="Label8" EnableViewState="false" Text="In Time" runat="server" />:
        </td>
        <td class="pm_field" style="background-color:White;">
            <asp:Label ID="AttendanceRecordWorkStart" runat="Server"  />
        </td>                        
        <td class="pm_field_header">
            <asp:Label ID="Label10" EnableViewState="false" Text="Out Time" runat="server" />:
        </td>
        <td class="pm_field" style="background-color:White;">
            <asp:Label ID="AttendanceRecordWorkEnd" runat="Server"  />
        </td>                        
    </tr>
    <%-- End 0000112, Miranda, 2015-01-17 --%>
    <tr id="LateWaiveDateToPlaceHolder" runat="server" >
        <td class="pm_field_header">
            <asp:Label ID="Label4" EnableViewState="false" Text="Late (Mins)" runat="server" />:
        </td>
        <td class="pm_field" style="background-color:White;" colspan="3" >
            <asp:Label ID="AttendanceRecordActualLateMins" runat="Server" />
        </td>
    </tr>
    <tr  >
        <td class="pm_field_header">
            <asp:Label ID="Label7" EnableViewState="false" Text="Reason" runat="server" />:
        </td>
        <td class="pm_field" style="background-color:White;" colspan="3">
            <asp:Label ID="LateWaiveReason" runat="Server" />
        </td>
    </tr>
    <tr >
        <td class="pm_field_header">
            <asp:Label ID="Label5" runat="server" EnableViewState="false" Text="Reason for Cancel" />:
        </td>
        <td class="pm_field" style="background-color:White;" colspan="3">
            <asp:Label ID="RequestLateWaiveCancelReason" runat="server"  />
        </td>
    </tr>
    <tr class="pm_field_title" runat="server" id="AuthorizerOptionSectionRow" >
        <td colspan="4">
            <asp:Button EnableViewState="false" Text="Authorize" CssClass="button" ID="Authorize" runat="server" OnClick="LateWaiveAuthorize_Click"  />
            <asp:Button EnableViewState="false" Text="Reject" CssClass="button" ID="Reject" runat="server" OnClick="LateWaiveReject_Click" />
            <br />
            <asp:Label runat="server" EnableViewState="false" Text="Reason" />:<asp:TextBox ID="RejectReason" runat="server" Columns="50" MaxLength="100" />
        </td>
    </tr>
    <tr  runat="server" id="RejectReasonRow" >
        <td class="pm_field_header">
            <asp:Label ID="Label11" runat="server" EnableViewState="false" Text="Reason" />:
        </td>
        <td class="pm_field" style="background-color:White;" colspan="3">
            <asp:Label ID="lblRejectReason" runat="server" />
        </td>
    </tr>
</table>
