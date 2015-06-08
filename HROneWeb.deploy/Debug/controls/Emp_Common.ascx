<%@ control language="C#" autoeventwireup="true" inherits="Emp_Common, HROneWeb.deploy" %>
<input type="hidden" id="EmpID" runat="server" name="ID" />
<table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
    <col width="20%" />
    <col width="30%" />
    <col width="20%" />
    <col width="30%" />
    <tr>
        <td class="pm_field_title" colspan="4">
            <asp:Label EnableViewState="false" Text="Employee Info" runat="server" />
        </td>
        <td class="pm_field" runat="server" id="ProfilePhotoCell" rowspan="7">
                <asp:Image ID="ProfilePhoto" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
            <asp:Label EnableViewState="false" Text="EmpNo" runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpNo" runat="Server" />
        </td>
        <td class="pm_field_header" >
            <asp:Label EnableViewState="false" Text="Alias" runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpAlias" runat="Server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header">
            <asp:Label EnableViewState="false" Text="Surname" runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="EmpEngSurname" runat="Server" />
        </td>
        <td class="pm_field_header">
            <asp:Label EnableViewState="false" Text="Other Name" runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="EmpEngOtherName" runat="Server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header">
            <asp:Label EnableViewState="false" Text="Gender" runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="EmpGender" runat="Server" />
        </td>
        <td class="pm_field_header">
            <asp:Label EnableViewState="false" Text="Chinese Name" runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="EmpChiFullName" runat="Server" />
        </td>
    </tr>
    <tr id="PrivateSection" runat="server">
        <td class="pm_field_header">
            <asp:Label EnableViewState="false" Text="HKID" runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpHKID" runat="Server" />
        </td>
        <td class="pm_field_header">
            <asp:Label EnableViewState="false" Text="Date of Birth" runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="EmpDateOfBirth" runat="Server" />
            (<asp:Label ID="Label2" runat="Server" EnableViewState="false" Text="Age"/>:<asp:Label ID="lblAge" runat="Server" />)
        </td>
    </tr>
    <tr>
        <td class="pm_field_header">
            <asp:Label EnableViewState="false" Text="Date of Joining" runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="EmpDateOfJoin" runat="Server" />
        </td>
        <td class="pm_field_header">
            <asp:Label EnableViewState="false" Text="Start Date of Service" runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="EmpServiceDate" runat="Server" />
            (<asp:Label ID="Label3" runat="Server" EnableViewState="false" Text="Year of Service"/>:<asp:Label ID="lblYearOfService" runat="Server" />)
        </td>
    </tr>
    <tr>
        <td class="pm_field_header">
            <asp:Label EnableViewState="false" Text="Status" runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="EmpStatus" runat="Server" />
            <asp:Panel ID="NewEmpNoPanel" runat="server" Visible="false ">
            (<asp:Label ID="lblNewEmpNoLabel" runat="server" EnableViewState="false" Text="New Emp. No" />:<asp:HyperLink ID="hlNewEmpNo" runat="server" />)
            </asp:Panel>
        </td>
        <td class="pm_field_header">
            <asp:Label ID="lblLastEmploymentDateLabel" EnableViewState="false" Text="Last Employment Date" runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="lblLastEmploymentDateValue" runat="Server" />
        </td>
    </tr>
    <tr id="PrevEmpNoRow" runat="server" >
        <td class="pm_field_header" >
            <asp:Label ID="Label1" EnableViewState="false" Text="Previous Emp. No." runat="server" />:
        </td>
        <td class="pm_field" colspan="3">
            <asp:HyperLink ID="hlPreviousEmpNo" runat="server" />
        </td>
    </tr>
    <tr id="MasterEmpNoRow" runat="server" >
        <td class="pm_field_header" >
            <asp:Label ID="Label4" EnableViewState="false" Text="Master Emp. No." runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:HyperLink ID="hlMasterEmpNo" runat="server" />
        </td>
        <td class="pm_field_header" >
            <asp:Label ID="Label32" runat="server" EnableViewState="false" Text="Options" />
        </td>
        <td class="pm_field">
            <asp:CheckBox ID="EmpIsCombinePaySlip" runat="server" EnableViewState="false" Text="Combine Payslip to Master" Enabled="false" /><br />
            <asp:CheckBox ID="EmpIsCombineMPF" runat="server" EnableViewState="false" Text="Combine MPF to Master" Enabled="false" /><br />
            <asp:CheckBox ID="EmpIsCombineTax" runat="server" EnableViewState="false" Text="Combine Tax to Master" Enabled="false" />
        </td>
    </tr>
    <tr id="ChildRoleEmpNoRow" runat="server" >
        <td class="pm_field_header" >
            <asp:Label ID="Label5" EnableViewState="false" Text="Other roles" runat="server" />:
        </td>
        <td class="pm_field" colspan="3">
            <asp:Repeater ID="ChildRoleEmpNoRepeater" runat="server" OnItemDataBound="ChildRoleEmpNoRepeater_ItemDataBound">
            <ItemTemplate>
                <asp:HyperLink ID="hlChildRoleEmpNo" runat="server" />
            </ItemTemplate>
            <SeparatorTemplate>
            <br />
            </SeparatorTemplate>
            </asp:Repeater>
        </td>
    </tr>
</table>
