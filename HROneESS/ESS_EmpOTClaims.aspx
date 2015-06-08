<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ESS_EmpOTClaims.aspx.cs" Inherits="ESS_EmpOTClaims" MasterPageFile="~/MainMasterPage.master" Async="true" %>
<%@ Register Src="~/controls/Emp_info.ascx" TagName="Emp_info" TagPrefix="uc1" %>
<%@ Register Src="~/controls/OTClaimsForm.ascx" TagName="OTClaimsForm" TagPrefix="uc2" %>

<asp:Content ID="bannerContent" runat="server" ContentPlaceHolderID="bannerContentPlaceHolder" > 
    <table width="898px" border="0" cellpadding="0" cellspacing="0">
      <tr>
        <td>
            <a href="ESS_Home.aspx">
                <img src="images/banner23_01.jpg" alt="" style="border-width: 0px; display : block" />
            </a>
        </td>
        <td valign="bottom" >
            <img src="images/banner23_02.jpg" alt="" style="border-width: 0px; display : block" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <img src="images/banner23_03.jpg" alt="" style="border-width: 0px; display : block" />
        </td>
      </tr>
    </table>
</asp:Content>
<asp:Content ID="mainContent" runat="server" ContentPlaceHolderID="mainContentPlaceHolder" > 
    <input type="hidden" id="EmpID" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
    <ContentTemplate >
        <!-- Web Control-->
        <uc1:emp_info id="Emp_info1" runat="server" />
        <!-- End of Web Control-->
        <br/>
        <uc2:OTClaimsForm id="OTClaimsForm1" runat="server" />
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content> 