<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="ASPLogin" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
    <title>Welcome to HROne</title>
    <link href="empstyle.css" rel="stylesheet" type="text/css" />
</head>

<body>
<table width="810px" border="0" cellpadding="0" cellspacing="0">
    <col width="381px" valign="top" />
    <col width="429px" />
  <tr >
    <td style="height:288px; max-height:288px; vertical-align:top; background-image: url(images/login1.jpg);">
<!--
        <img src="images/login1.jpg" alt="login1.jpg" width="381px" height="288px" />
-->
    </td>
      <td rowspan="3" style="height:588px; max-height:588px; vertical-align:top; background-image: url(images/login3.jpg);">
<!--
        <img src="images/login3.jpg" alt="login3.jpg" width="429px" height="588px" />
-->
    </td>
  </tr>
  <tr>
    <td style="height:140px; max-height:140px; vertical-align:top; background-image: url(images/login2.jpg);">
<!--
        <img src="images/login2.jpg" alt="login2.jpg" width="381px" height="140px"/> 
-->
    </td>
  </tr>
  <tr >
     <td style="height:160px; max-height:160px; vertical-align:top; background-image: url(images/login4.jpg);">
        <form id="form1" runat="server" >
            <table width="381px" border="0" cellpadding="2" cellspacing="0" style="text-align:left;" >
                <col width="103px" />
                <col width="138px" />

                <tr runat="server" visible="false" id="SaaSCustomerIDRow" >
                    <td class="style11" style="white-space:nowrap; width: 103px;">
                        <asp:Label ID="Label4" Text="Customer ID" runat="server" />:
                    </td>
                    <td class="style11" colspan="2" style="width: 138px">
                        <asp:TextBox ID="txtCustomerID" runat="server" /></td>
                </tr>
                <tr runat="server" visible="false" id="multiDBRow">
                    <td class="style11" style="white-space:nowrap; width: 103px;">
                        <asp:Label ID="Label5" Text="Database" runat="server" />:
                    </td>
                    <td class="style11" colspan="2" style="width: 138px">
                        <asp:DropDownList ID="cboDatabase" runat="server" />
                    </td>
                </tr>
              <tr>
                <td style="width: 103px" >
                    <asp:Label ID="Label1" Text="Emp No " runat="server" />:
                </td>
                <td >
                    <asp:TextBox ID="Username" runat="server" />
                </td>
              </tr>
              <tr  >
                <td style="width: 103px">
                    <asp:Label ID="Label2" Text="Password " runat="server" />:
                </td>
                <td>
                    <asp:TextBox TextMode="Password" ID="Password" runat="server" />
                </td>
              </tr>
              <tr>
                 <td class="style7" colspan="2" align="center">
                    <asp:Label ID="Prompt" runat="Server" />
                </td>
              </tr>
               <tr>
                  <td style="width: 103px" >&nbsp;</td>
                  <td >
                    <asp:Button Text="Login" ID="Login" runat="server" OnClick="Login_Click" />
                </td>
                </tr>
          </table>
        </form>
    </td>
  </tr>
<%--  <tr style="min-height:200px;"><td></td></tr>
--%>
</table>
<br/>

</body>
</html>