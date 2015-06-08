<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OTClaimsForm.ascx.cs" Inherits="OTClaimsForm" %>

<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>

<input type="hidden" id="EmpID" runat="server" />
    <table width="100%" border="0" cellpadding="5" cellspacing="0" class="pm_field_section">
        <col width="15%" />
        <col width="85%" />
        <tr >
            <td colspan="2" class="pm_field_title">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td >
                            <asp:Label ID="lblHeader" runat="server" EnableViewState="false" Text="CL Requisition" /> :
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
                    <asp:Label ID="Label1" Text="Claim Period" runat="server" />:
                </td>
                <td class="pm_field">
                    <uc1:WebDatePicker id="RequestOTClaimPeriodFrom" runat="server" AutoPostBack="true" OnChanged="OTClaimDateFrom_Changed" /> -
                    <uc1:WebDatePicker id="RequestOTClaimPeriodTo" runat="server" AutoPostBack="true" OnChanged="OTClaimDateTo_Changed" />
                </td>
        </tr>
        <tr id="OTClaimDateToPlaceHolder" runat="server"  >
                <td class="pm_field_header">
                    <asp:Label ID="Label2" Text="Claim Time" runat="server" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="RequestOTClaimHourFrom" runat="Server"  AutoPostBack="true" OnTextChanged="OTClaimTime_TextChanged" />-
                    <asp:TextBox ID="RequestOTClaimHourTo" runat="Server"  AutoPostBack="true" OnTextChanged="OTClaimTime_TextChanged" />
                </td>
        </tr>
        <tr id="TimeRow" runat="server" >
                <td class="pm_field_header" >
                    <asp:Label ID="lblOTHours" Text="No. of Hours Claim" runat="server" />:
                </td>
                <td class="pm_field" >
                    <asp:TextBox ID="RequestOTHours" runat="Server" />
                </td>
        </tr>
        <tr >
                <td class="pm_field_header">
                    <asp:Label ID="Label4" Text="Remark" runat="server" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:TextBox ID="RequestOTClaimRemark" runat="Server" TextMode="MultiLine" Columns="35" Rows="5" />
                </td>
        </tr>
    </table>
