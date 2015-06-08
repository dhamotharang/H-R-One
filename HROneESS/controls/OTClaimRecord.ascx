<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OTClaimRecord.ascx.cs" Inherits="OTClaimRecord" %>

<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>

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
                    <asp:Label ID="lblHeader" runat="server" EnableViewState="false" Text="CL Requisition" /> :
                </td>
                <td align="right" >
                    <asp:Button ID="btnCancel" runat="server" EnableViewState="false" Text="Cancel" OnClick="btnCancel_Click" />
                </td>
            </tr>
        </table>
        </td>
    </tr>
    <tr>
        <td class="pm_field_header">
            <asp:Label ID="Label3" EnableViewState="false" Text="Date From" runat="server" />:
        </td>
        <td class="pm_field" style="background-color:White;" colspan="3">
            <asp:Label ID="RequestOTClaimPeriodFrom" runat="Server"  />
        </td>                        
    </tr>
    <tr id="OTClaimDateToPlaceHolder" runat="server" >
        <td class="pm_field_header">
            <asp:Label ID="Label4" EnableViewState="false" Text="Date To" runat="server" />:
        </td>
        <td class="pm_field" style="background-color:White;" colspan="3" >
            <asp:Label ID="RequestOTClaimPeriodTo" runat="Server" />
        </td>
    </tr>
    <tr id="TimeRow" runat="server" >
        <td class="pm_field_header">
            <asp:Label ID="Label8" EnableViewState="false" Text="Time" runat="server" />:
        </td>
        <td class="pm_field" style="background-color:White;" colspan="3">
            <asp:Label ID="RequestOTClaimHourFrom" runat="Server" />-
            <asp:Label ID="RequestOTClaimHourTo" runat="Server" />
        </td>
    </tr>
    <tr runat="server" id="HoursClaimPanel">
        <td class="pm_field_header">
            <asp:Label ID="Label10" EnableViewState="false" Text="Hours Claim" runat="server" />:
        </td>
        <td class="pm_field" style="background-color:White;" colspan="3" >
            <asp:Label ID="RequestOTHours" runat="Server" />
        </td>
    </tr>
    <tr  >
        <td class="pm_field_header">
            <asp:Label ID="Label7" EnableViewState="false" Text="Remark" runat="server" />:
        </td>
        <td class="pm_field" style="background-color:White;" colspan="3">
            <asp:Label ID="RequestOTClaimRemark" runat="Server" />
        </td>
    </tr>
    <tr class="pm_field_title" runat="server" id="AuthorizerOptionSectionRow" >
        <td colspan="4">
            <asp:Button EnableViewState="false" Text="Authorize" CssClass="button" ID="Authorize" runat="server" OnClick="OTAuthorize_Click"  />
            <asp:Button EnableViewState="false" Text="Reject" CssClass="button" ID="Reject" runat="server" OnClick="OTReject_Click" />
            <br />
            <asp:Label runat="server" EnableViewState="false" Text="Reason" />:<asp:TextBox ID="RejectReason" runat="server" Columns="50" MaxLength="100" />
            <%--Start 0000060, Miranda, 2014-07-22--%>
            <br />
            <table runat="server" id="FinalApprovalTable" width="100%" border="0" cellpadding="0" cellspacing="0" class="pm_field_section">
                <col width="15%" />
                <col width="35%" />
                <col width="15%" />
                <col width="35%" />
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" EnableViewState="false" Text="Effective Date" />:
                    </td>
                    <td>
                        <uc1:WebDatePicker id="RequestOTClaimEffectiveDate" runat="server" AutoPostBack="true" />
                    </td>
                    <td>
                        <asp:Label ID="Label2" runat="server" EnableViewState="false" Text="Date Expiry" />:
                    </td>
                    <td>
                        <uc1:WebDatePicker id="RequestOTClaimDateExpiry" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label5" runat="server" EnableViewState="false" Text="Approved By" />:
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="ApprovedBy" runat="server" Columns="50" MaxLength="100" ReadOnly="true" />
                    </td>
                </tr>
            </table>
            <%--End 0000060, Miranda, 2014-07-22--%>
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
