<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ESS_Home.aspx.cs" Inherits="ESS_home" MasterPageFile="~/MainMasterPage.master" %>

<asp:Content ID="bannerContent" runat="server" ContentPlaceHolderID="bannerContentPlaceHolder">
    <table width="898px" border="0" cellpadding="0" cellspacing="0">
      <tr>
        <td>
            <a href="ESS_Home.aspx">
                <img src="images/banner01_01.jpg" alt="" style="border-width: 0px; display : block" />
            </a>
        </td>
        <td valign="bottom" >
            <img src="images/banner01_02.jpg" alt="" style="border-width: 0px; display : block"/>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <img src="images/banner01_03.jpg" alt="" style="border-width: 0px; display : block"/>
        </td>
      </tr>
    </table>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContentPlaceHolder">
    <input type="hidden" id="EmpID" runat="server" name="ID" />
    <table width="100%" border="0" cellspacing="5" cellpadding="0">
        <tr>
            <td >
                <p class="title01">
                    Welcome to HROne</p>
                <p class="content">
                    HROne is a comprehensive human resources software tailored for small to medium sized companies in all walks of business.<br/>
                    <br/>
                    Employee Self-Service
                        To minimize paper work and to make the HR job more efficient, the Employee Self Service functions allow personal information to be checked and updated by individual employees. They can check out the latest company policies through the company intranet, and submit leave applications for management approvals. It also can serve as a simple time-card system to record in/out time attendance.
                    </p>
            </td>
        </tr>
    </table>
</asp:Content>
