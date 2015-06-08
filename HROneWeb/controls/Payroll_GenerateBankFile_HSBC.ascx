<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Payroll_GenerateBankFile_HSBC.ascx.cs" Inherits="Payroll_GenerateBankFile_HSBC" %>
<table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
    <col width="15%" />
    <tr>
        <td class="pm_field_title" colspan="3">
            <asp:Label Text="HSBC/Hang Seng Bank File Detail" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
            <asp:Label Text="Plan Code" runat="server" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:DropDownList ID="PlanCode" runat="server">
                <asp:ListItem Text="Autopay-Out (autoplan 1)" Value="F" />
                <asp:ListItem Text="Autopay-Out (autoplan 2)" Value="E" Enabled="false"  />
                <asp:ListItem Text="Autopay-In (autoplan 1)" Value="G" Enabled="false" />
                <asp:ListItem Text="Autopay-In (autoplan 2)" Value="H" Enabled="false" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr id="FileTypeRow" runat="server" visible="true" >
        <td class="pm_field_header" >
           <asp:Label ID="Label1" Text="File Format" runat="server" />
        </td>
        <td class="pm_field" colspan="2">
            <asp:DropDownList ID="FileType" runat="server" OnSelectedIndexChanged="FileType_SelectedIndexChanged" AutoPostBack="true" >
                <asp:ListItem Text="e-channel" Value="E" Selected="true" />
                <asp:ListItem Text="CD-ROM" Value="D" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
           <asp:Label ID="lblPaymentCodeHeader" Text="Payment Code" runat="server" />
        </td>
        <td class="pm_field" colspan="2">
            <asp:TextBox ID="BankPaymentCode" runat="server" Columns="3" MaxLength="3" CssClass="pm_required" />
            (<asp:Label Text="Max. 3 character" runat="Server" />)
        </td>
    </tr>
    <asp:PlaceHolder ID="eChannelPanel" runat="server" >
    <tr>
        <td class="pm_field_header" >
        </td>
        <td class="pm_field" colspan="2">
            <asp:Checkbox ID="UseBIBFormat" runat="server" Text="Use Business Internet Banking Service (max. 200 transaction per file)" TextAlign="Right" AutoPostBack="true" OnCheckedChanged="UseBIBFormat_CheckedChanged"  />
        </td>
    </tr>
    <tr runat="server" id="MultiplePaymentCodeRow" visible="false" >
        <td class="pm_field_header" >
           <asp:Label ID="lblMultiplePaymentCodeHeader" runat="server" Text="Payment Code for multiple bank file" />
        </td>
        <td class="pm_field" colspan="2">
            <asp:TextBox ID="multipleBankCode" runat="server" TextMode="MultiLine" Columns="5" Rows="3"/>
            <asp:Label ID="Label2" runat="server" Text="(press ENTER to separate each payment code" />
        </td>
    </tr>
    </asp:PlaceHolder>
    <tr>
        <td class="pm_field_header" >
            <asp:Label Text="First Party Reference" runat="server" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:TextBox ID="FirstPartyReference" runat="server" Columns="12" MaxLength="12" />
            (<asp:Label Text="Max. 12 character" runat="server" />)
        </td>
    </tr>
    <asp:PlaceHolder ID="DiskettePanel" runat="server" Visible="false" >
    <tr id="RemoteProfileID" runat="server" >
        <td class="pm_field_header" >
           <asp:Label ID="Label5" runat="server" Text="Remote Profile ID" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:TextBox ID="txtRemoteProfileID" runat="server" CssClass="pm_required" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
           <asp:Label ID="lblContactPerson" runat="server" Text="Contact Person(s)" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:TextBox ID="txtContactPerson" runat="server" CssClass="pm_required" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
           <asp:Label ID="lblContactPersonPhoneNumber" runat="server" Text="Telephone Number(s)" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:TextBox ID="txtContactPersonPhoneNumber" runat="server" CssClass="pm_required" />
        </td>
    </tr>
    </asp:PlaceHolder>
 </table>
