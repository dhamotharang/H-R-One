<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OTClaimCancelRecord.ascx.cs" Inherits="OTClaimCancelRecord" %>

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
                    <asp:Label ID="lblHeader" runat="server" EnableViewState="false" Text="Cancellation of OT" /> :
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
            <asp:Label ID="Label3" EnableViewState="false" Text="Date From" runat="server" />:
        </td>
        <td class="pm_field" style="background-color:White;" colspan="3">
            <asp:Label ID="OTClaimDateFrom" runat="Server"  />
        </td>                        
    </tr>
    <tr id="OTClaimDateToPlaceHolder" runat="server" >
        <td class="pm_field_header">
            <asp:Label ID="Label4" EnableViewState="false" Text="Date To" runat="server" />:
        </td>
        <td class="pm_field" style="background-color:White;" colspan="3" >
            <asp:Label ID="OTClaimDateTo" runat="Server" />
        </td>
    </tr>
    <tr id="TimeRow" runat="server" >
        <td class="pm_field_header">
            <asp:Label ID="Label8" EnableViewState="false" Text="Time" runat="server" />:
        </td>
        <td class="pm_field" style="background-color:White;" colspan="3">
            <asp:Label ID="OTClaimTimeFrom" runat="Server" />-
            <asp:Label ID="OTClaimTimeTo" runat="Server" />
        </td>
    </tr>
    <tr runat="server" id="HoursClaimPanel">
        <td class="pm_field_header">
            <asp:Label ID="Label10" EnableViewState="false" Text="Hours Claim" runat="server" />:
        </td>
        <td class="pm_field" style="background-color:White;" >
            <asp:Label ID="OTClaimHours" runat="Server" />
        </td>
    </tr>
    <tr  >
        <td class="pm_field_header">
            <asp:Label ID="Label7" EnableViewState="false" Text="Remark" runat="server" />:
        </td>
        <td class="pm_field" style="background-color:White;" colspan="3">
            <asp:Label ID="OTClaimRemark" runat="Server" />
        </td>
    </tr>
    <tr >
        <td class="pm_field_header">
            <asp:Label ID="Label5" runat="server" EnableViewState="false" Text="Reason for Cancel" />:
        </td>
        <td class="pm_field" style="background-color:White;" colspan="3">
            <asp:Label ID="RequestOTClaimCancelReason" runat="server"  />
        </td>
    </tr>
    <tr class="pm_field_title" runat="server" id="AuthorizerOptionSectionRow" >
        <td colspan="4">
            <asp:Button EnableViewState="false" Text="Authorize" CssClass="button" ID="Authorize" runat="server" OnClick="OTAuthorize_Click"  />
            <asp:Button EnableViewState="false" Text="Reject" CssClass="button" ID="Reject" runat="server" OnClick="OTReject_Click" />
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
