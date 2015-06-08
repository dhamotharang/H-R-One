<%@ page language="C#" autoeventwireup="true" inherits="ProductKey, HROneWeb.deploy" viewStateEncryptionMode="Always" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
    <title>HROne</title>
    <link href="css.css" rel="stylesheet" type="text/css" />
<style type="text/css">
<!--
body {
	/*background-image: url(images/back01.jpg);*/
}
.style11 {font-family: Arial, Helvetica, sans-serif; font-size: 12px; white-space:nowrap; }
-->
</style></head>

<body>
<form id="form1" runat="server" >
<asp:HiddenField ID="PreviousURL" runat="server" />
        <p>&nbsp;</p>
        <center>
		<div style="text-align: center; width: 422px;">
		<img src="images/login_top.jpg"/>
		</div>
            <div style="text-align: center; width: 422px; height: 327px; background-image: url(images/login_back.jpg)">
                <div style="text-align: center; margin: auto; top: 80px; position: relative; border: 1">
                    <center>
                        <table width="340px" border="0" cellpadding="0" cellspacing="5">
                            <tr>
                                <td class="style11" style="height: 21px">
                                    <asp:Label ID="Label2" Text="Please enter the product key" runat="server" />.
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 94px;">
                                  <table border="0" cellspacing="2" cellpadding="0" width="100%">
                                    <col width="120px" />
                                    <col width="10%" />
                                    <tr >
                                      <td class="style11"><asp:Label ID="lblProductKey" Text="Product Key" runat="server" /> :</td>
                                      <td class="style11"><asp:TextBox ID="txtProductKey" runat="server" Columns="35" /></td>
                                    </tr>
                                    <tr >
                                      <td class="style11"><asp:Label ID="lblRequestCode" Text="Request Code" runat="server" /> : </td>
                                      <td class="style11"><asp:TextBox ID="txtRequestCode" runat="server" Columns="35" Enabled="false" /></td>
                                    </tr>
                                    <tr >
                                      <td class="style11"><asp:Label ID="lblAuthorizationCode" Text="Authorization Code" runat="server" /> : </td>
                                      <td class="style11"><asp:TextBox ID="txtAuthorizationCode" runat="server" Columns="35" /></td>
                                    </tr>
                                    <tr >
                                      <td class="style11"><asp:Label ID="Label1" Text="Trial Key" runat="server" /> :</td>
                                      <td class="style11"><asp:TextBox ID="txtTrialKey" runat="server" Columns="35" /></td>
                                    </tr>
                                   <tr>
                                      <td class="style11" colspan="2"><asp:Label ID="Prompt" runat="Server" /></td>
                                    </tr>
                                    <tr>
                                      <td class="style11" align="left">
                                      </td>
                                      <td class="style11" align="right" >
                                        <asp:Button Text="OK" ID="OK" runat="server" OnClick="OK_Click" />
                                        <asp:Button Text="Keep Trial" ID="Cancel" runat="server" OnClick="Cancel_Click" />
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