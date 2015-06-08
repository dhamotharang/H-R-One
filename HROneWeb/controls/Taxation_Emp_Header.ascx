<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Taxation_Emp_Header.ascx.cs" Inherits="Taxation_Emp_Header" %>


<input type="hidden" id="EmpID" runat="server" name="ID" />
<table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
    <col width="15%" />
    <col width="25%" />
    <col width="15%" />
    <col width="25%" />
    <col width="10%" />
    <col width="10%" />
    <tr>
        <td class="pm_field_title" colspan="6">
            <asp:Label Text="Taxation Info" runat="server" />
         </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
            <asp:Label Text="Name of Employer" runat="server" />:</td>
        <td class="pm_field" >
            <asp:Label ID="TaxFormEmployerName" runat="Server" /></td>
        <td class="pm_field_header" >
            <asp:Label Text="Year Return" runat="server" />:</td>
        <td class="pm_field" >
            <asp:Label ID="TaxFormYear" runat="Server" /></td>
        <td class="pm_field_header" >
            <asp:Label Text="Form" runat="server" />:</td>
        <td class="pm_field" >
            <asp:Label ID="TaxFormType" runat="Server" /></td>
    </tr>
    <tr>
        <td class="pm_field_header" >
            <asp:Label ID="ILabel1" runat="server" Text="EmpNo"/>:</td>
        <td class="pm_field">
            <asp:Label ID="EmpNo" runat="Server" /></td>
        <td class="pm_field_header">
            <asp:Label Text="Alias" runat="server" />:</td>
        <td class="pm_field" >
            <asp:Label ID="EmpAlias" runat="Server" /></td>
    </tr>
    <tr>
        <td class="pm_field_header">
            <asp:Label Text="Surname" runat="server" />:</td>
        <td class="pm_field">
            <asp:Label ID="TaxEmpSurname" runat="Server" /></td>
        <td class="pm_field_header">
            <asp:Label Text="Other Name" runat="server" />:</td>
        <td class="pm_field">
            <asp:Label ID="TaxEmpOtherName" runat="Server" /></td>
        <td class="pm_field_header">
            <asp:Label Text="Chinese Name" runat="server" />:</td>
        <td class="pm_field">
            <asp:Label ID="TaxEmpChineseName" runat="Server" /></td>
    </tr>
</table>
