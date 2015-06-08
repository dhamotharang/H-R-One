<%@ page language="C#" autoeventwireup="true" inherits="SetupRequirements, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>



<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="UserID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    System Requirement</td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    Workstation</td>
            </tr>
        </table>

        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <tr>
                <td class="pm_field">
												<br/> 
												- It is recommended that you have a PC with these features: 
												<br/> <br/>
												1. Network ready<br />
												2. Pentium IV 2.0G Hz, 512 MB RAM, 20GB Hard disk free space or above <br/>
												3. Microsoft Windows 2000, XP, Vista, Mac OS X 10.4 or above <br/>
												4. Microsoft Internet Explorer 6.0+ or Firefox 1.0+ <br/>
												5. VGA Monitor, preferably with 1024 x 768 resolution 
												<br/>
												6. Acrobat 6.0 with Asian font pack of Traditional Chinese 
												for report viewing <br/><br/><br/>

						</td>
            </tr>

        </table>
<br/><br/>

        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    Database Server </td>
            </tr>
        </table>
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <tr>
                <td class="pm_field">
		<br/>
												- It is recommended that you have server with these features: 
												<br/> <br/>
												1. Pentium IV 2 GB RAM, 80 GB hard disk or above <br/>
												2. Microsoft Windows 2000 Professional Server, XP Professional or 2003 Server or above<br/> 
												3. MS SQL Server 2000, MSDE or above <br/>
												4. Web server IIS 5.0 or above 
												<br/><br/>
												</td>
            </tr>

        </table>
</asp:Content> 