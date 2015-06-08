<%@ page language="C#" autoeventwireup="true" inherits="ESS_EmpSupervisorApproval, HROneESS.deploy" masterpagefile="~/MainMasterPage.master" async="true" %>

<%@ Register Src="controls/Emp_Authorize_List.ascx" TagName="Emp_Authorize_List_Form" TagPrefix="uc4" %>
<%@ Register Src="controls/Emp_info.ascx" TagName="Emp_info" TagPrefix="uc1" %>

<asp:Content ID="bannerContent" runat="server" ContentPlaceHolderID="bannerContentPlaceHolder" > 
    <table width="898px" border="0" cellpadding="0" cellspacing="0">
      <tr>
        <td>
            <a href="ESS_Home.aspx">
                <img src="images/banner11_01.jpg" alt="" style="border-width: 0px; display : block" />
            </a>
        </td>
        <td valign="bottom" >
            <img src="images/banner11_02.jpg" alt="" style="border-width: 0px; display : block" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <img src="images/banner11_03.jpg" alt="" style="border-width: 0px; display : block" />
        </td>
      </tr>
    </table>
</asp:Content>
<asp:Content ID="mainContent" runat="server" ContentPlaceHolderID="mainContentPlaceHolder" > 
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <ContentTemplate >
            <!--WebControl-->
            <uc1:Emp_info ID="uc1Emp_info" runat="server" />
            <!--End WenControl-->
            <br />
            <uc4:Emp_Authorize_List_Form ID="Emp_Authorize_List_Form1" runat="server" /> 
        </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content> 