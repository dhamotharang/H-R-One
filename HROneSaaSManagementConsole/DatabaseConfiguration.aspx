<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DatabaseConfiguration.aspx.cs" Inherits="DatabaseConfiguration" %>

<%@ Register Src="controls/DBConfig_SQLServer.ascx" TagName="DBConfig_SQLServer" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Database Connection Setting</title>
    <link href="css.css" rel="stylesheet" type="text/css" />

</head>
<body>
    <form id="form1" runat="server">
    <div >
        <center>
        <table border="0" class="pm_list_section" cellspacing="0" cellpadding="1" >
            <tr>
                <td class="pm_list_header" align="center" colspan="2">
                    <asp:Label ID="DBConfigHeader" runat="server" Text="Database Connection Setting" />
                </td>
            </tr>
            <tr>
                <td align="left" class="pm_field_header" >
                    <asp:Label ID="lblServerType" runat="server" Text="Server Type" />
                </td>
                <td align="left" class="pm_field">
                    <asp:Label ID="ServerType" runat="server" Text="SQL Server"/>
                </td>
            </tr>
            <tr>
                <td align="left" class="pm_field" colspan="2">
                    <asp:Label ID="Message" runat="server" />
                </td>
            </tr>
        <asp:Panel ID="SQLServerSetting" runat="server" >
            <uc1:DBConfig_SQLServer id="DBConfig_SQLServer1" runat="server" />            
        </asp:Panel>
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
