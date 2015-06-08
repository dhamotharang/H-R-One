<%@ page language="C#" autoeventwireup="true" inherits="ESS_EmpRequestDetail, HROneESS.deploy" masterpagefile="~/MainMasterPage.master" async="true" %>

<%@ Register Src="~/controls/Emp_Request_EmpInfoDetail.ascx" TagName="Emp_Request_EmpinfoDetail" TagPrefix="uc3" %>
<%@ Register Src="~/controls/LeaveApplicationRecord.ascx" TagName="LeaveApplicationRecord" TagPrefix="uc2" %>
<%@ Register Src="~/controls/LeaveApplicationCancelRecord.ascx" TagName="LeaveApplicationCancelRecord" TagPrefix="uc2" %>
<%-- Start 0000060, Miranda, 2014-07-13 --%>
<%@ Register Src="~/controls/OTClaimRecord.ascx" TagName="OTClaimRecord" TagPrefix="uc2" %>
<%@ Register Src="~/controls/OTClaimCancelRecord.ascx" TagName="OTClaimCancelRecord" TagPrefix="uc2" %>
<%-- End 0000060, Miranda, 2014-07-13 --%>
<%-- Start 0000112, Miranda, 2014-12-10 --%>
<%@ Register Src="~/controls/LateWaiveRecord.ascx" TagName="LateWaiveRecord" TagPrefix="uc2" %>
<%@ Register Src="~/controls/LateWaiveCancelRecord.ascx" TagName="LateWaiveCancelRecord" TagPrefix="uc2" %>
<%-- End 0000112, Miranda, 2014-12-10 --%>
<%@ Register Src="~/controls/Emp_info.ascx" TagName="Emp_info" TagPrefix="uc1" %>
<%@ Register Src="~/controls/Emp_Request_AuthorizeHistory_List.ascx" TagName="Emp_Request_AuthorizeHistory_List" TagPrefix="uc2" %>

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
            <uc3:Emp_Request_EmpinfoDetail ID="uc3Emp_Request_Empinfo" runat="server" />
            <uc2:LeaveApplicationRecord ID="LeaveApplicationRecord1" runat="server" /> 
            <uc2:LeaveApplicationCancelRecord ID="LeaveApplicationCancelRecord1" runat="server" /> 
            <%-- Start 0000060, Miranda, 2014-07-13 --%>
            <uc2:OTClaimRecord ID="OTClaimRecord1" runat="server" /> 
            <uc2:OTClaimCancelRecord ID="OTClaimCancelRecord1" runat="server" /> 
            <%-- End 0000060, Miranda, 2014-07-13 --%>
            <%-- Start 0000112, Miranda, 2014-12-10 --%>
            <uc2:LateWaiveRecord ID="LateWaiveRecord1" runat="server" /> 
            <uc2:LateWaiveCancelRecord ID="LateWaiveCancelRecord1" runat="server" /> 
            <%-- End 0000112, Miranda, 2014-12-10 --%>
            <br />
            <uc2:Emp_Request_AuthorizeHistory_List ID="Emp_Request_AuthorizeHistory_List1" runat="server" /> 
        </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content> 