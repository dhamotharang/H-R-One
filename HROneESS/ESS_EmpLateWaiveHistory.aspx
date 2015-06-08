<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ESS_EmpLateWaiveHistory.aspx.cs"
    Inherits="ESS_EmpLateWaiveHistory" MasterPageFile="~/MainMasterPage.master" %>

<%@ Register Src="controls/Emp_LateWaiveHistory_Form.ascx" TagName="Emp_LateWaiveHistory_Form"
    TagPrefix="uc2" %>
<%@ Register Src="controls/Emp_info.ascx" TagName="Emp_info" TagPrefix="uc1" %>
<asp:Content ID="bannerContent" runat="server" ContentPlaceHolderID="bannerContentPlaceHolder">
    <table width="898px" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <a href="ESS_Home.aspx">
                    <img src="images/banner22_01.jpg" alt="" style="border-width: 0px; display: block" />
                </a>
            </td>
            <td valign="bottom">
                <img src="images/banner22_02.jpg" alt="" style="border-width: 0px; display: block" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <img src="images/banner22_03.jpg" alt="" style="border-width: 0px; display: block" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="mainContent" runat="server" ContentPlaceHolderID="mainContentPlaceHolder">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <!-- Web Control-->
            <uc1:Emp_info ID="Emp_info1" runat="server" />
            <!-- End of Web Control-->
            <br />
            <uc2:Emp_LateWaiveHistory_Form ID="Emp_LateWaiveHistory_Form_1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
