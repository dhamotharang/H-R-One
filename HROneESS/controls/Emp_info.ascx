<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Emp_info.ascx.cs" Inherits="Emp_info" %>
<input type="hidden" id="EmpID" runat="server" />
    <table width="100%" border="0" cellpadding="5" cellspacing="0" class="pm_field_section" >
        <col width="15%" />
        <col width="35%" />
        <col width="15%" />
        <col width="35%" />
        <tr >
            <td colspan="4" class="pm_field_title">
                <asp:Label ID="Label1" Text="Employee Info" runat="server" />
            </td>
        </tr>
        <tr >
            <td class="pm_field_header">
                <asp:Label ID="Label2" Text="Emp No" runat="server" />:
            </td>
            <td class="pm_field">
                <asp:Label ID="EmpNo" runat="server" />
            </td>
            <td class="pm_field_header">
                <asp:Label ID="Label3" Text="Alias" runat="server" />:
            </td>
            <td class="pm_field">
                <asp:Label ID="EmpAlias" runat="server" />
            </td>
        </tr>
        <tr >
            <td class="pm_field_header">
                <asp:Label ID="Label4" Text="Surname" runat="server" />:
            </td>
            <td class="pm_field" >
                <asp:Label ID="EmpEngSurname" runat="server" />
            </td>
            <td class="pm_field_header">
                <asp:Label ID="Label5" Text="Other Name" runat="server" />:
            </td>
            <td class="pm_field" >
                <asp:Label ID="EmpEngOtherName" runat="server" />
            </td>
        </tr>
        <tr >
            <td class="pm_field_header">
                <asp:Label ID="Label8" Text="Gender" runat="server" />:
            </td>
            <td class="pm_field" >
                <asp:Label ID="EmpGender" runat="server" />
            </td>
            <td class="pm_field_header">
                <asp:Label ID="Label7" Text="Chinese Name" runat="server" />:
            </td>
            <td class="pm_field" >
                <asp:Label ID="EmpChiFullName" runat="server" />
            </td>
        </tr>
        <tr >
            <td class="pm_field_header">
                <asp:Label ID="Label10" Text="Date of Joining" runat="server" />:
            </td>
            <td class="pm_field">
                <asp:Label ID="EmpDateOfJoin" runat="server" />
            </td>
            <td class="pm_field_header">
                <asp:Label ID="Label11" Text="Start Date of Service" runat="server" />:
            </td>
            <td class="pm_field">
                <asp:Label ID="EmpServiceDate" runat="server" />
            </td>
        </tr>
        <tr >
            <td class="pm_field_header">
                <asp:Label ID="Label6" Text="Probation Last Date" runat="server" />:
            </td>
            <td class="pm_field">
                <asp:Label ID="EmpProbaLastDate" runat="server" />
            </td>
            <td class="pm_field_header">
                <asp:Label ID="Label12" Text="Last Employment Date" runat="server" />:
            </td>
            <td class="pm_field">
                <asp:Label ID="EmpTermLastDate" runat="server" />
            </td>
        </tr>
    </table>
