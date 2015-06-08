<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="ASPLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
    <title>HROne SaaS Management Console</title>
    <link href="css.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
<!--
body {
    background-image: url(images/back01.jpg);
}
.style11 {font-family: Arial, Helvetica, sans-serif; font-size: 12px; }
-->
</style>
</head>
<body>
    <form id="form1" runat="server" >
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <p>
            &nbsp;</p>
        <center>
            <div style="text-align: center; width: 422px; height: 327px; background-image: url(images/back.jpg); border: 1">
                <div style="text-align: center; margin: auto; top: 123px; position: relative; border: 0">
                    <center>
                        <table style="width: 263px; height: 120px;" border="0" cellpadding="0" cellspacing="5">
                            <tr>
                                <td class="style11" style="height: 21px; text-align:center">
                                                <asp:Label ID="Label4" Text="HROne SaaS Management Console" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 94px;">
                                    <table width="242px" border="0" cellspacing="5" cellpadding="0">
                                        <col width="73px" />
                                        <col width="154px" />
                                        <tr>
                                            <td class="style11" style="white-space:nowrap;" colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style11" style="white-space:nowrap;">
                                                <asp:Label ID="Label2" Text="Username" runat="server" />:
                                            </td>
                                            <td class="style11" >
                                                <asp:TextBox ID="Username" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td class="style11" style="white-space:nowrap;">
                                                <asp:Label ID="Label3" Text="Password " runat="server" />:
                                            </td>
                                            <td class="style11" >
                                                <asp:TextBox TextMode="Password" ID="Password" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td class="style11" colspan="2">
                                                <asp:UpdatePanel ID="ErrorUpdatePanel" runat="server">
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnLogin" EventName="Click" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <asp:Label ID="Prompt" runat="Server" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style11">
                                            </td>
                                            <td class="style11" align="right">
                                                <asp:Button Text="Login" ID="btnLogin" runat="server" OnClick="Login_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </div>
        </center>
    </form>
</body>
</html>
