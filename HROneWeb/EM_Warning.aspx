<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EM_Warning.aspx.cs" Inherits="EM_Warning" %>
<%@ Register TagPrefix="pmv" Namespace="HROne.DataAccess" Assembly="HROne.DataAccess" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
    <title>HROne</title>
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
    <form id="form1" runat="server" AUTOCOMPLETE="Off">
        <p>
            &nbsp;</p>
        <center>
            <div style="text-align: center; width: 422px; height: 327px; background-image: url(images/back.jpg);
                border: 1">
                <div style="text-align: center; margin: auto; top: 123px; position: relative; border: 0">
                    <center>
                        <table  style="width: 263px; height: 120px;" border="0" cellpadding="0" cellspacing="5">
                            <tr>
                                <td class="style11" colspan="2">
                                        <asp:Label ID="Label1" Text="Warning!!" runat="server" ForeColor="RED" Font-Bold="true" />
                                </td>
                            </tr>
                            <tr>
                                <td class="style11" colspan="2">
                                        <asp:Label ID="Label4" Text="System does not have an user with Security Permission." runat="server"  ForeColor="RED" Font-Bold="true" />
                                </td>
                            </tr>
                            <tr>
                                <td class="style11" colspan="2" >
                                    <asp:Label ID="Label5" Text="Please contact our customer service for Pass Code to enter Emergency Mode" runat="server"  ForeColor="RED" Font-Bold="true" /><br />
                                    <asp:Label ID="Label2" Text="or press &quot;Skip&quot; to continue to use HROne. " runat="server"  ForeColor="RED" Font-Bold="true" /><br />
                                </td>
                            </tr>
                            <tr>
                                <td class="style11" colspan="2" >
                                    <asp:Label ID="lblPassCode" runat="server" Text="Pass Code " />:
                                    <asp:TextBox ID="txtPassCode" runat="server" Columns="40" MaxLength="30"/> 
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button Text="Submit" ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btnSkip" runat="server" Text="Skip" OnClick="btnSkip_Click" />
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </div>
        </center>
        <span class="error">
            <pmv:PageError ID="pageError" runat="server" Text="Error" ShowFieldErrors="true" IsPopup="true"
            ShowPrompt="false" />
        </span>
    </form>
</body>
</html>
