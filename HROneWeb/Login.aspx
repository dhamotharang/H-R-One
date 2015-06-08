<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="ASPLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
    <title>HROne</title>
    <link href="css.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
<!--
body {
   /* background-image: url(images/login_top.jpg);
	background-repeat:no-repeat;
	background-color:#ededed;*/
	background:none;
}
.style11 {font-family: Arial, Helvetica, sans-serif; font-size: 12px; }
-->
</style>
</head>
<body>
    <form id="form1" runat="server" >
  <asp:ScriptManager ID="ScriptManager1" runat="server" /> 
<div style="height:100px;clear:both;">					
					</div>
        <center>
		<div style="text-align: center; width: 422px;">
		<img src="images/login_top.jpg"/>
		</div>
            <div style="text-align: center; width: 422px; height: 327px; background-image: url(images/login_back.jpg);background-repeat:no-repeat;">
                
      <div style="text-align: center; margin: auto; top: 80px; position: relative; border: 0"> 
        <center>
          <table style="width: 263px; height: 120px;" border="0" cellpadding="0" cellspacing="5">
            <tr> 
              <td class="style11" style="height: 21px"> <asp:Label ID="Label1" Text="LOGIN_MAIN_MESSAGE" runat="server" /> </td>
            </tr>
            <tr> 
              <td style="height: 94px;"> <table width="300px" border="0" cellspacing="5" cellpadding="0">
                  <col width="100px" />
                  <col width="100px" />
                  <col width="100px" />
                  <tr runat="server" visible="false" id="SaaSCustomerIDRow"> 
                    <td class="style11" style="white-space:nowrap;"> <asp:Label ID="Label4" Text="Customer ID" runat="server" />
                      : </td>
                    <td class="style11" colspan="2"> <asp:TextBox ID="txtCustomerID" runat="server" /></td>
                  </tr>
                  <tr runat="server" visible="false" id="multiDBRow"> 
                    <td class="style11" style="white-space:nowrap;"> <asp:Label ID="Label5" Text="Database" runat="server" />
                      : </td>
                    <td class="style11" colspan="2"> <asp:DropDownList ID="cboDatabase" runat="server" /> </td>
                  </tr>
                  <tr> 
                    <td class="style11" style="white-space:nowrap;"> <asp:Label ID="Label2" Text="Username" runat="server" />
                      : </td>
                    <td class="style11" colspan="2"> <asp:TextBox ID="Username" runat="server" /></td>
                  </tr>
                  <tr> 
                    <td class="style11" style="white-space:nowrap;"> <asp:Label ID="Label3" Text="Password " runat="server" />
                      : </td>
                    <td class="style11" colspan="2"> <asp:TextBox TextMode="Password" ID="Password" runat="server" /></td>
                  </tr>
                  <tr> 
                    <td class="style11" colspan="3"> <asp:UpdatePanel ID="ErrorUpdatePanel" runat="server">
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="btnLogin" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <asp:Label ID="Prompt" runat="Server" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel> </td>
                  </tr>
                  <tr> 
                    <td class="style11"> </td>
                    <td class="style11" align="left"> 
                        <%--
                        <a id="A1" href="1setuprequirements.htm" runat="server" target="_blank"> 
                        <asp:Label ID="SystemRequirement" runat="server" Text="System Requirements" />
                        </a>
                      --%>
                    </td>
                    <td class="style11" align="left"> <asp:Button Text="Login" ID="btnLogin" runat="server" OnClick="Login_Click" /> 
                    </td>
                  </tr>
                </table></td>
            </tr>
          </table>
        </center>
      </div>
            </div>
            <div class="style11" style="text-align:center;">
            <asp:Label ID="Version" runat="server" Text="Ver. " />
            <asp:Label ID="lblVersionNo" runat="server" />
            </div>
        </center>
    </form>
</body>
</html>
