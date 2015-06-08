<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LeaveApplicationRecord.ascx.cs" Inherits="LeaveApplicationRecord" %>
<%@ Register Src="~/controls/Emp_LeaveBalance_List.ascx" TagName="Emp_LeaveBalance_List" TagPrefix="uc2" %>

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
                    <asp:Label ID="lblHeader" runat="server" EnableViewState="false" Text="Leave Application" /> :
                </td>
                <td align="right" >
                    <asp:Button ID="btnCancel" runat="server" EnableViewState="false" Text="Cancel" OnClick="btnCancel_Click" />
                </td>
            </tr>
        </table>
        </td>
    </tr>
<%--    <tr >
        <td class="pm_field_header">
            <asp:Label ID="Label5" EnableViewState="false" Text="Name of Applicant" runat="server" />:
        </td>
        <td class="pm_field" style="background-color:White;" colspan="3">
            <asp:Label ID="EmpName" runat="Server" />
        </td>
    </tr>--%>
    <tr  >
        <td class="pm_field_header">
            <asp:Label ID="Label1" EnableViewState="false" Text="Leave Code" runat="server" />:
        </td>
        <td class="pm_field" style="background-color:White;">
            <asp:Label ID="RequestLeaveCodeID" runat="Server" />
        </td>
        <td class="pm_field_header">
            <asp:Label ID="Label2" EnableViewState="false" Text="Type" runat="server" />:
        </td>
        <td class="pm_field" style="background-color:White;">
            <asp:Label ID="RequestLeaveAppUnit" runat="Server"/>
        </td>
    </tr>
    <tr  >
        <td class="pm_field_header">
            <asp:Label ID="Label3" EnableViewState="false" Text="Date From" runat="server" />:
        </td>
        <td class="pm_field" style="background-color:White;" colspan="3">
            <asp:Label ID="RequestLeaveAppDateFrom" runat="Server"  /> &nbsp
            <asp:Label ID="RequestLeaveAppDateFromAM" runat="Server"  />
        </td>                        
    </tr>
    <tr id="LeaveAppDateToPlaceHolder" runat="server" >
        <td class="pm_field_header">
            <asp:Label ID="Label4" EnableViewState="false" Text="Date To" runat="server" />:
        </td>
        <td class="pm_field" style="background-color:White;" colspan="3" >
            <asp:Label ID="RequestLeaveAppDateTo" runat="Server" /> &nbsp
            <asp:Label ID="RequestLeaveAppDateToAM" runat="Server" />
        </td>
    </tr>
    <tr id="TimeRow" runat="server" >
        <td class="pm_field_header">
            <asp:Label ID="Label8" EnableViewState="false" Text="Time" runat="server" />:
        </td>
        <td class="pm_field" style="background-color:White;" colspan="3">
            <asp:Label ID="RequestLeaveAppTimeFrom" runat="Server" />-
            <asp:Label ID="RequestLeaveAppTimeTo" runat="Server" />
        </td>
    </tr>
    <tr >
        <td class="pm_field_header">
            <asp:Label ID="Label6" EnableViewState="false" Text="Days Taken" runat="server" />:
        </td>
        <td class="pm_field" style="background-color:White;" colspan="3">
            <asp:Label ID="RequestLeaveDays" runat="Server" />
        </td>
    </tr>
    <tr runat="server" id="HoursClaimPanel">
        <td class="pm_field_header">
            <asp:Label ID="Label10" EnableViewState="false" Text="Hours Claim" runat="server" />:
        </td>
        <td class="pm_field" style="background-color:White;" >
            <asp:Label ID="RequestLeaveAppHours" runat="Server" />
        </td>
    </tr>
    <tr  >
        <td class="pm_field_header">
            <asp:Label ID="Label7" EnableViewState="false" Text="Remark" runat="server" />:
        </td>
        <td class="pm_field" style="background-color:White;" colspan="3">
            <asp:Label ID="RequestLeaveAppRemark" runat="Server" />
        </td>
    </tr>
    <tr id="LeaveCodeIsShowMedicalCertOptionPanel" runat="server">
        <td class="pm_field_header">
           <!-- Start 0000048, Miranda, 2014-06-03 -->
           <asp:Label ID="Label9" EnableViewState="false" Text="Relevant Certificate" runat="server" />:
           <!-- End 0000048, Miranda, 2014-06-03 -->
        </td>
        <td class="pm_field" style="background-color:White;" colspan="3">
            <asp:Label ID="RequestLeaveAppHasMedicalCertificate" runat="Server" />
        </td>
    </tr>
    <tr class="pm_field_title" runat="server" id="AuthorizerOptionSectionRow" >
        <td colspan="4">
            <asp:Button EnableViewState="false" Text="Authorize" CssClass="button" ID="Authorize" runat="server" OnClick="LeaveAuthorize_Click"  />
            <asp:Button EnableViewState="false" Text="Reject" CssClass="button" ID="Reject" runat="server" OnClick="LeaveReject_Click" />
            <br />
            <asp:Label runat="server" EnableViewState="false" Text="Reason" />:<asp:TextBox ID="RejectReason" runat="server" Columns="50" MaxLength="100" />
        </td>
    </tr>
    <tr  runat="server" id="RejectReasonRow" >
        <td class="pm_field_header">
            <!-- Start 0000063, KuangWei, 2014-08-25 -->
            <asp:Label ID="lblReject" runat="server" EnableViewState="false" Text="Reason:" />
            <asp:Label ID="lblAuthorize" runat="server" EnableViewState="false" Text="Reason:" />
            <!-- End 0000063, KuangWei, 2014-08-25 -->
        </td>
        <td class="pm_field" style="background-color:White;" colspan="3">
            <asp:Label ID="lblRejectReason" runat="server" />
        </td>
    </tr>
</table>
<br />
<uc2:Emp_LeaveBalance_List ID="Emp_LeaveBalance_List1" runat="server" />
