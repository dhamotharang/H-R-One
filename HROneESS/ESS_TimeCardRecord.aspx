<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ESS_TimeCardRecord.aspx.cs" Inherits="Customize_ESS_TimeCardRecord" MasterPageFile="~/MainMasterPage.master" %>

<%@ Register Src="~/controls/Emp_LeaveHistory_Form.ascx" TagName="Emp_LeaveHistory_Form" TagPrefix="uc2"%>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/controls/Emp_info.ascx" TagName="Emp_info" TagPrefix="uc1" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="bannerContent" runat="server" ContentPlaceHolderID="bannerContentPlaceHolder" > 
    <table width="898px" border="0" cellpadding="0" cellspacing="0">
      <tr>
        <td>
            <a href="ESS_Home.aspx">
                <img src="images/banner16_01.jpg" alt="" style="border-width: 0px; display : block" />
            </a>
        </td>
        <td valign="bottom" >
            <img src="images/banner16_02.jpg" alt="" style="border-width: 0px; display : block" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <img src="images/banner16_03.jpg" alt="" style="border-width: 0px; display : block" />
        </td>
      </tr>
    </table>
</asp:Content>
<asp:Content ID="mainContent" runat="server" ContentPlaceHolderID="mainContentPlaceHolder" > 

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <%-- Start 0000183, KuangWei, 2015-04-10 --%>
        <Triggers>    
            <asp:PostBackTrigger ControlID="TeamRecordExport" />
        </Triggers>
        <%-- End 0000183, KuangWei, 2015-04-10 --%>
    <ContentTemplate >
        <!-- Web Control-->
       <uc1:Emp_info ID="Emp_info1" runat="server" />
       <!-- End of Web Control-->
        <br/>
<table width="100%"border="0" cellpadding="5" cellspacing="0" class="pm_list_section">
    <col width="75px" />
    <col width="75px" />
    <col width="75px" />
    <tr >
        <td colspan="3" class="pm_field_title">
            <asp:Label ID="Label2" Text="Time Card Records" runat="server" />
        </td>
    </tr>
    <tr style="background-color:#FFFFFF">
        <%-- Start 0000183, KuangWei, 2015-04-10 --%>    
        <td colspan="3" class="pm_list_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td>
                        <asp:Label ID="lblHeader" runat="server" Text="Date" /> :
                        <uc1:WebDatePicker id="TimeCardRecordDate" runat="server" ShowDateFormatLabel="true" AutoPostBack="true" OnChanged="Search_Click" />
                    </td>
                    <td align="right" >
                        <asp:Button ID="TeamRecordExport" runat="server" Visible="false" Text="Team Record Export" CssClass="button" OnClick="Export_Click" />
                    </td>
                </tr>
            </table>            
        </td>
        <%-- End 0000183, KuangWei, 2015-04-10 --%>
    </tr>
    <tr >
        <td class="pm_list_header" colspan="2">
            <asp:LinkButton runat="server" ID="_TimeCardRecordDateTime" OnClick="ChangeOrder_Click" Text="Date/Time" />
        </td>
        <td class="pm_list_header">
            <asp:LinkButton runat="server" ID="_TimeCardRecordLocation" OnClick="ChangeOrder_Click" Text="Location" />
        </td>
    </tr>
    <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound" OnItemCommand="Repeater_ItemCommand" >
        <ItemTemplate>
            <tr style="background-color:#FFFFFF" class="tablecontent">
                <td class="pm_list" style="white-space:nowrap;" >
                    <%#sbinding.getFValue(Container.DataItem, "TimeCardRecordDateTime", "yyyy-MM-dd")%>
                </td>
                <td class="pm_list" style="white-space:nowrap;" >
                    <%#sbinding.getFValue(Container.DataItem, "TimeCardRecordDateTime", "HH:mm:ss")%>
                </td>
                <td class="pm_list" style="white-space:nowrap;" >
                        <%#sbinding.getValue(Container.DataItem, "TimeCardRecordLocation")%>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
<table width="100%" cellspacing="0" cellpadding="0" class="pm_list_pagenav">
    <tr>
        <td align="right" >
            <tb:RecordListFooter id="ListFooter" runat="server"
                 ShowAllRecords="true" 
                 ListOrderBy="TimeCardRecordDateTime"
                 ListOrder="true" 
              />
        </td>
    </tr>
</table>
    </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content> 