<%@ page language="C#" autoeventwireup="true" inherits="DatabaseUpgrade, HROneWeb.deploy" viewStateEncryptionMode="Always" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Database Upgrade</title>
        <link href="css.css" rel="stylesheet" type="text/css" />

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <center>
        <table border="0" class="pm_list_section" cellspacing="0" cellpadding="1" >
            <tr>
                <td class="pm_list_header" align="center" colspan="2">
                    <asp:Label ID="DBConfigHeader" runat="server" Text="Database Upgrade" />
                </td>
            </tr>
            <tr>
                <td align="left" class="pm_field" colspan="2">
                    <asp:Label ID="Message" runat="server" />
                </td>
            </tr>
            <tr>
                <td align="left" class="pm_field_header" >
                    <asp:Label ID="Label2" runat="server" Text="Database Version" />
                </td>
                <td align="left" class="pm_field">
                    <asp:Label ID="lblDBVersion" runat="server" Text=""/>
                </td>
            </tr>
            <tr>
                <td align="left" class="pm_field_header" >
                    <asp:Label ID="Label1" runat="server" Text="Required Database Version" />
                </td>
                <td align="left" class="pm_field">
                    <asp:Label ID="lblRequiredDBVersion" runat="server" Text=""/>
                </td>
            </tr>
            <tr>
                <td class="pm_field_header"></td>
                <td align="left" class="pm_field" >
                    <asp:Button ID="btnPatchWithoutBackup" runat="server" Text="Patch without backup database" CssClass="button" OnClick="btnPatchWithoutBackup_Click" />
                </td>
            </tr>
        </table> 
        </center>
    </div>
    </form>
</body>
</html>
