<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DBConfig_SQLServer.ascx.cs" Inherits="DBConfig_SQLServer" %>
            <tr>
                <td align="left" class="pm_field_header" >
                    <asp:Label ID="lblServerLocaton" runat="server" Text="Server Location" />
                </td>
                <td align="left" class="pm_field">
                    <asp:TextBox ID="MSSQLServerLocation" runat="server"/>
                </td>
            </tr>
            <tr>
                <td align="left" class="pm_field_header" >
                    <asp:Label ID="lblDatabase" runat="server" Text="Database" />
                </td>
                <td align="left" class="pm_field">
                    <asp:TextBox ID="MSSQLDatabase" runat="server"/>
                </td>
            </tr>
            <tr>
                <td align="left" class="pm_field_header" >
                    <asp:Label ID="Label2" runat="server" Text="User ID" />
                </td>
                <td align="left" class="pm_field">
                    <asp:TextBox ID="MSSQLUserID" runat="server"/>
                </td>
            </tr>
            <tr>
                <td align="left" class="pm_field_header" >
                    <asp:Label ID="Label3" runat="server" Text="Password" />
                </td>
                <td align="left" class="pm_field">
                    <asp:TextBox ID="MSSQLPassword" runat="server" TextMode="Password" />
                </td>
            </tr>
            <tr>
                <td align="left" class="pm_field_header" >
                    <asp:Label ID="Label1" runat="server" Text="Create Database if not exists." />
                </td>
                <td align="left" class="pm_field">
                    <asp:CheckBox ID="chkCreateDatabase" runat="server" AutoPostBack="true" />
                </td>
            </tr>
            <asp:PlaceHolder ID="CreateDatabaseSection" runat="server" Visible="false" >
            <tr>
                <td align="left" class="pm_field_header" >
                    <asp:Label ID="Label4" runat="server" Text="System Administrator UserID" />
                </td>
                <td align="left" class="pm_field">
                    <asp:TextBox ID="SAUserID" runat="server"/>
                </td>
            </tr>
            <tr>
                <td align="left" class="pm_field_header" >
                    <asp:Label ID="Label5" runat="server" Text="System Administrator Password" />
                </td>
                <td align="left" class="pm_field">
                    <asp:TextBox ID="SAPassword" runat="server" TextMode="Password" />
                </td>
            </tr>
            </asp:PlaceHolder>
