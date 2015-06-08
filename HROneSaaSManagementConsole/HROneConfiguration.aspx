<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HROneConfiguration.aspx.cs" Inherits="HROneConfiguration" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>HROne SaaS Configuration</title>
    <link href="css.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <center>
        <table border="0" class="pm_list_section" cellspacing="0" cellpadding="1" >
            <tr>
                <td class="pm_list_header" align="center" colspan="2">
                    <asp:Label ID="DBConfigHeader" runat="server" Text="HROne SaaS Configuration" />
                </td>
            </tr>
            <tr id="DBNameRow" runat="server" >
                <td align="left" class="pm_field_header" >
                    <asp:Label ID="Label1" runat="server" Text="Physical Path for HROne configuration file" />
                </td>
                <td align="left" class="pm_field">
                    <asp:TextBox ID="txtPhysiclPath" runat="server" Columns="100"/><br />
                    (e.g c:\inetpub\wwwroot\HROneweb\HROne.config )
                </td>
            </tr>
            <tr>
                <td align="left" class="pm_field" colspan="2">
                    <asp:Label ID="Message" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header"></td>
                <td align="left" class="pm_field" >
                    <asp:Button ID="OK" runat="server" Text="OK" CssClass="button" OnClick="OK_Click" />
                </td>
            </tr>
        </table>
        </center> 
    </div>
    </form>
</body>
</html>
