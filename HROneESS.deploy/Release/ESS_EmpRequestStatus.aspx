<%@ page language="C#" autoeventwireup="true" inherits="ESS_EmpRequestStatus, HROneESS.deploy" masterpagefile="~/MainMasterPage.master" async="true" %>

<%@ Register Src="controls/Emp_Request_EmpInfo.ascx" TagName="Emp_Request_Empinfo" TagPrefix="uc3" %>
<%@ Register Src="controls/Emp_Request_Leave_List.ascx" TagName="Emp_Request_Leave_List" TagPrefix="uc2" %>
<%@ Register Src="controls/Emp_Request_LeaveCancel_List.ascx" TagName="Emp_Request_LeaveCancel_List" TagPrefix="uc2" %>
<%-- Start 0000060, Miranda, 2014-07-13 --%>
<%@ Register Src="controls/Emp_Request_OT_List.ascx" TagName="Emp_Request_OT_List" TagPrefix="uc7" %>
<%@ Register Src="controls/Emp_Request_OTCancel_List.ascx" TagName="Emp_Request_OTCancel_List" TagPrefix="uc6" %>
<%-- End 0000060, Miranda, 2014-07-13 --%>
<%-- Start 0000112, Miranda, 2014-12-10 --%>
<%@ Register Src="controls/Emp_Request_LateWaive_List.ascx" TagName="Emp_Request_LateWaive_List" TagPrefix="uc4" %>
<%@ Register Src="controls/Emp_Request_LateWaiveCancel_List.ascx" TagName="Emp_Request_LateWaiveCancel_List" TagPrefix="uc5" %>
<%-- End 0000112, Miranda, 2014-12-10 --%>
<%@ Register Src="controls/Emp_info.ascx" TagName="Emp_info" TagPrefix="uc1" %>

<asp:Content ID="bannerContent" runat="server" ContentPlaceHolderID="bannerContentPlaceHolder" > 
    <table width="898px" border="0" cellpadding="0" cellspacing="0">
      <tr>
        <td>
            <a href="ESS_Home.aspx">
                <img src="images/banner05_01.jpg" alt="" style="border-width: 0px; display : block" />
            </a>
        </td>
        <td valign="bottom" >
            <img src="images/banner05_02.jpg" alt="" style="border-width: 0px; display : block" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <img src="images/banner05_03.jpg" alt="" style="border-width: 0px; display : block" />
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
            <uc3:Emp_Request_Empinfo ID="uc3Emp_Request_Empinfo" runat="server" />
            <br />
            <uc2:Emp_Request_Leave_List ID="Emp_Request_Leave_List" runat="server" /> 
            <br />
            <uc2:Emp_Request_LeaveCancel_List ID="Emp_Request_LeaveCancel_List" runat="server" /> 
            <%-- Start 0000060, Miranda, 2014-07-13 --%>
            <br />
            <uc7:Emp_Request_OT_List ID="Emp_Request_OT_List" runat="server" />
            <br />
            <uc6:Emp_Request_OTCancel_List ID="Emp_Request_OTCancel_List" runat="server" />
            <%-- End 0000060, Miranda, 2014-07-13 --%>
            <%-- Start 0000112, Miranda, 2014-12-10 --%>
            <br />
            <uc4:Emp_Request_LateWaive_List ID="Emp_Request_LateWaive_List" runat="server" />
            <br />
            <uc5:Emp_Request_LateWaiveCancel_List ID="Emp_Request_LateWaiveCancel_List" runat="server" />
            <%-- End 0000112, Miranda, 2014-12-10 --%>

        </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content> 