<%@ control language="C#" autoeventwireup="true" inherits="Emp_OTHistory_Form, HROneESS.deploy" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
<input type="hidden" id="EmpID" runat="server" name="ID" />
<input type="hidden" id="CompensationLeaveEntitleID" runat="server" />
<table border="0" width="100%" cellspacing="0" cellpadding="5" class="pm_field_section">
    <tr style="background-color: #FFFFFF">
        <td class="pm_search_header">
            <asp:Label ID="lblYear" runat="server" Text="Year" />:
            <asp:DropDownList ID="Year" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Year_SelectedIndexChanged" />
        </td>
    </tr>
</table>
<table width="100%" border="0" cellpadding="5" cellspacing="0" class="pm_list_section">
    <col width="80px" />
    <col width="80px" />
    <colgroup>
        <col width="80px" />
        <col width="80px" />
    </colgroup>
    <colgroup>
        <col width="50px" />
        <col width="50px" />
    </colgroup>
    <col width="50px" />
    <col width="150px" />
    <tr>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_CompensationLeaveEntitleEffectiveDate" OnClick="ChangeOrder_Click"
                Text="Effective Date"></asp:LinkButton>
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_CompensationLeaveEntitleDateExpiry" OnClick="ChangeOrder_Click"
                Text="Date Expiry"></asp:LinkButton>
        </td>
        <td align="left" class="pm_list_header" colspan="2">
            <asp:LinkButton runat="server" ID="_CompensationLeaveEntitleClaimPeriodFrom" OnClick="ChangeOrder_Click"
                Text="Period"></asp:LinkButton>
        </td>
        <td align="left" class="pm_list_header" colspan="2">
            <asp:LinkButton runat="server" ID="_CompensationLeaveEntitleClaimHourFrom" OnClick="ChangeOrder_Click"
                Text="Time"></asp:LinkButton>
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_CompensationLeaveEntitleHoursClaim" OnClick="ChangeOrder_Click"
                Text="Hours Claim"></asp:LinkButton>
        </td>
        <td align="left" class="pm_list_header">
            <asp:Label runat="server" ID="_CompensationLeaveEntitleApprovedBy" Text="Approved By"></asp:Label>
        </td>
    </tr>
    <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
        <ItemTemplate>
            <tr style="background-color: #FFFFFF" class="tablecontent">
                <td class="pm_list" style="white-space: nowrap;">
                    <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "ESS_EmpOTClaimsHistory.aspx?CompensationLeaveEntitleID=" + sbinding.getValue(Container.DataItem,"CompensationLeaveEntitleID"))%>">
                        <%#sbinding.getFValue(Container.DataItem, "CompensationLeaveEntitleEffectiveDate", "yyyy-MM-dd")%>
                    </a>
                </td>
                <td class="pm_list" style="white-space: nowrap;">
                    <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "ESS_EmpOTClaimsHistory.aspx?CompensationLeaveEntitleID=" + sbinding.getValue(Container.DataItem,"CompensationLeaveEntitleID"))%>">
                        <%#sbinding.getFValue(Container.DataItem, "CompensationLeaveEntitleDateExpiry", "yyyy-MM-dd")%>
                    </a>
                </td>
                <td class="pm_list" style="white-space: nowrap;">
                    <%#sbinding.getFValue(Container.DataItem, "CompensationLeaveEntitleClaimPeriodFrom", "yyyy-MM-dd")%>
                </td>
                <td class="pm_list" style="white-space: nowrap;">
                    <%#sbinding.getFValue(Container.DataItem, "CompensationLeaveEntitleClaimPeriodTo", "yyyy-MM-dd")%>
                </td>
                <td class="pm_list" style="white-space: nowrap;">
                    <%#sbinding.getFValue(Container.DataItem, "CompensationLeaveEntitleClaimHourFrom", "HH:mm")%>
                </td>
                <td class="pm_list" style="white-space: nowrap;">
                    <%#sbinding.getFValue(Container.DataItem, "CompensationLeaveEntitleClaimHourTo", "HH:mm")%>
                </td>
                <td class="pm_list" style="white-space: nowrap;">
                    <%#sbinding.getFValue(Container.DataItem,"CompensationLeaveEntitleHoursClaim","0.00")%>
                </td>
                <td class="pm_list" style="white-space: nowrap;">
                    <%#sbinding.getValue(Container.DataItem, "CompensationLeaveEntitleApprovedBy")%>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
<table width="100%" cellspacing="0" cellpadding="5" class="pm_list_pagenav">
    <tr>
        <td align="right">
            <tb:RecordListFooter ID="ListFooter" runat="server" ShowAllRecords="true" />
        </td>
    </tr>
</table>
<br />
<table id="OTDetailsTable" runat="server" border="0" width="100%" cellspacing="0"
    cellpadding="5" class="pm_field_section">
    <col width="15%" />
    <col width="35%" />
    <col width="15%" />
    <col width="35%" />
    <tr>
        <td colspan="4" class="pm_field_title">
            <asp:Label ID="Label8" Text="View Compensation Leave Entitlement" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header">
            <asp:Label ID="Label1" Text="Effective Date" runat="server" />:</td>
        <td class="pm_field">
            <asp:Label ID="CompensationLeaveEntitleEffectiveDate" runat="server" />
        </td>
        <td class="pm_field_header">
            <asp:Label ID="Label7" Text="Date Expiry" runat="server" />:</td>
        <td class="pm_field">
            <asp:Label ID="CompensationLeaveEntitleDateExpiry" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header">
            <asp:Label ID="Label2" Text="Claim Period" runat="server" />:
        </td>
        <td class="pm_field" colspan="3">
            <asp:Label ID="CompensationLeaveEntitleClaimPeriodFrom" runat="server" />
            -
            <asp:Label ID="CompensationLeaveEntitleClaimPeriodTo" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header">
            <asp:Label ID="Label3" Text="Claim Time" runat="server" />:</td>
        <td class="pm_field" colspan="3">
            <asp:Label ID="CompensationLeaveEntitleClaimHourFrom" runat="Server" />-
            <asp:Label ID="CompensationLeaveEntitleClaimHourTo" runat="Server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header">
            <asp:Label ID="Label4" Text="No. of Hours Claim" runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="CompensationLeaveEntitleHoursClaim" runat="Server" />
        </td>
        <td class="pm_field_header">
            <asp:Label ID="Label5" Text="Approved By" runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="CompensationLeaveEntitleApprovedBy" runat="Server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header">
            <asp:Label ID="Label6" Text="Remark" runat="server" />:</td>
        <td class="pm_field" colspan="3">
            <asp:Label ID="CompensationLeaveEntitleRemark" runat="Server" />
        </td>
    </tr>
</table>
