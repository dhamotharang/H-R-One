<%@ control language="C#" autoeventwireup="true" inherits="Emp_Termination, HROneWeb.deploy" %>
<input type="hidden" id="EmpID" runat="server" name="ID" />
<table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
    <col width="20%" />
    <col width="30%" />
    <col width="20%" />
    <col width="30%" />
    <tr>
        <td class="pm_field_header" >
            <asp:Label Text="Cessation Reason" runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="CessationReasonID" runat="Server" />
        </td>
        <td class="pm_field_header" >
            <asp:Label Text="Notice Date of Termination" runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpTermResignDate" runat="Server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
            <asp:Label Text="Notice Period" runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpTermNoticePeriod" runat="Server" />
            <asp:Label ID="EmpTermNoticeUnit" runat="Server" />
        </td>
        <td class="pm_field_header">
            <asp:Label ID="Label1" Text="Expected Last Employment Date" runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="ExpectedLastDate" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header">
            <asp:Label Text="Actual Last Employment Date" runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="EmpTermLastDate" runat="Server" />
        </td>
        <td class="pm_field_header">
            <asp:Label ID="Label2" Text="Transfer Company?" runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="EmpTermIsTransferCompany" runat="Server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header">
            <asp:Label Text="Remark" runat="server" />:
        </td>
        <td class="pm_field" colspan="3">
            <asp:Label ID="EmpTermRemark" runat="Server" />
        </td>
    </tr>
</table>
