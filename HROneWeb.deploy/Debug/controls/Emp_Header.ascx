<%@ control language="C#" autoeventwireup="true" inherits="Emp_Header, HROneWeb.deploy" %>
<input type="hidden" id="EmpID" runat="server" name="ID" />
<table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
    <col width="12%" />
    <col width="22%" />
    <col width="12%" />
    <col width="22%" />
    <col width="12%" />
    <col width="20%" />
    <tr>
        <td class="pm_field_title" colspan="6">
            <asp:Label Text="Employee Info" runat="server" /></td>
    </tr>
    <tr>
        <td class="pm_field_header" >
            <asp:Label Text="EmpNo" runat="server" />:</td>
        <td class="pm_field" >
            <asp:Label ID="EmpNo" runat="Server" /></td>
        <td class="pm_field_header" >
            <asp:Label Text="Alias" runat="server" />:</td>
        <td class="pm_field" >
            <asp:Label ID="EmpAlias" runat="Server" /></td>
    </tr>
    <tr>
        <td class="pm_field_header">
            <asp:Label Text="Surname" runat="server" />:</td>
        <td class="pm_field">
            <asp:Label ID="EmpEngSurname" runat="Server" /></td>
        <td class="pm_field_header">
            <asp:Label Text="Other Name" runat="server" />:</td>
        <td class="pm_field">
            <asp:Label ID="EmpEngOtherName" runat="Server" /></td>
        <td class="pm_field_header">
            <asp:Label Text="Chinese Name" runat="server" />:</td>
        <td class="pm_field">
            <asp:Label ID="EmpChiFullName" runat="Server" /></td>
    </tr>
</table>
