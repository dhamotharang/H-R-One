<%@ control language="C#" autoeventwireup="true" inherits="Payroll_PeriodInfo, HROneWeb.deploy" %>
<input type="hidden" id="PayPeriodID" runat="server" name="ID" />
<table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
    <col width="20%" />
    <col width="40%" />
    <col width="40%" />
    <tr>
        <td class="pm_field_title" colspan="3">
            <asp:Label Text="Payroll Cycle Information" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
            <asp:Label Text="Payroll Group" runat="server" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:Label ID="PayGroupID" runat="Server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
            <asp:Label Text="Period" runat="server" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:Label ID="PayPeriodFr" runat="Server" />
            -
            <asp:Label ID="PayPeriodTo" runat="Server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
            <asp:Label Text="Leave Application cut off Date" runat="server" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:Label ID="PayPeriodLeaveCutOffDate" runat="Server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
            <asp:Label Text="Attendance Period" runat="server" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:Label ID="PayPeriodAttnFr" runat="Server" />
            -
            <asp:Label ID="PayPeriodAttnTo" runat="Server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
            <asp:Label Text="Status" runat="server" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:Label ID="PayPeriodStatus" runat="Server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >&nbsp;
        </td>
        <td class="pm_field_header" ><asp:Label Text="Date" runat="server" />
        </td>
        <td class="pm_field_header" ><asp:Label Text="User" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
            <asp:Label Text="Trial Run" runat="server" />
        </td>
        <td class="pm_field" >
            <asp:Label ID="PayPeriodTrialRunDate" runat="Server" />
        </td>
        <td class="pm_field" >
            <asp:Label ID="PayPeriodTrialRunBy" runat="Server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
            <asp:Label Text="Rollback" runat="server" />
        </td>
        <td class="pm_field" >
            <asp:Label ID="PayPeriodRollbackDate" runat="Server" />
        </td>
        <td class="pm_field" >
            <asp:Label ID="PayPeriodRollbackBy" runat="Server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
            <asp:Label Text="Confirm" runat="server" />
        </td>
        <td class="pm_field" >
            <asp:Label ID="PayPeriodConfirmDate" runat="Server" />
        </td>
        <td class="pm_field" >
            <asp:Label ID="PayPeriodConfirmBy" runat="Server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
            <asp:Label Text="Process End" runat="server" />
        </td>
        <td class="pm_field" >
            <asp:Label ID="PayPeriodProcessEndDate" runat="Server" />
        </td>
        <td class="pm_field" >
            <asp:Label ID="PayPeriodProcessEndBy" runat="Server" />
        </td>
    </tr>
 </table>
