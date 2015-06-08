<%@ control language="C#" autoeventwireup="true" inherits="Emp_AuthorizeHistory_List, HROneESS.deploy" %>

<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
<%-- Start 0000064, Miranda, 2014-09-19 --%>
<%@ Register Src="~/controls/EmployeeSearchControl.ascx" TagName="EmployeeSearchControl" TagPrefix="uc1" %>
<%-- End 0000064, Miranda, 2014-09-19 --%>

<input type="hidden" id="EmpID" runat="server" name="ID" />
    <%-- Start 0000064, Miranda, 2014-09-19 --%>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="Reset" EventName="Click" />
    </Triggers>
    <ContentTemplate >
       <uc1:EmployeeSearchControl ID="EmployeeSearchControl1" runat="server" />
    </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
        <tr>
            <td>
                <asp:Button ID="Search" runat="server" Text="Search" CssClass="button" OnClick="Search_Click" />
                <asp:Button ID="Reset" runat="server" Text="Reset" CssClass="button" OnClick="Reset_Click" />
            </td>
        </tr>
    </table>
    <br />
    <%-- End 0000064, Miranda, 2014-09-19 --%>
        <table  width="100%" border="0" cellpadding="5" cellspacing="0" class="pm_list_section" >
            <tr >
                <td colspan="7" class="pm_list_title">
                    <asp:Label ID="Label1" runat="server" EnableViewState="false" Text="Approval History"/>
                </td>
            </tr>
            <tr >
                <td colspan="7" class="pm_list_title">
                    <table width="100%" border="0" cellspacing="1" cellpadding="1">
                        <tr>
                            <td >
                                <asp:Label ID="lblHeader" runat="server" Text="Request From" /> :
                            </td>
                            <td >
                                <uc1:WebDatePicker ID="EmpRequestFromDate" runat="server" ShowDateFormatLabel="false" AutoPostBack="true"/><asp:Label ID="Label2" runat="server" Text="To" />
                                <uc1:WebDatePicker ID="EmpRequestToDate" runat="server" ShowDateFormatLabel="false" AutoPostBack="true"/>
                            </td>
                        </tr>
                        <tr>
                            <td >
                                <asp:Label ID="Label4" runat="server" Text="Request Type" /> :
                            </td>
                            <%-- Start 0000180, KuangWei, 2015-03-25 --%>
                            <td>
                                <asp:DropDownList ID="EmpRequestType" runat="server" AutoPostBack="true" />
                            </td>
                            <td >
                                <asp:Label runat="server" Text="Leave Code" /> :
                            </td>
                            <td>
                                <asp:DropDownList ID="LeaveCode" runat="Server" AutoPostBack="true" />
                            </td>                           
                            <%-- End 0000180, KuangWei, 2015-03-25 --%>
                        </tr>                    
                        </table>
                </td>
            </tr>
            <tr >
                <td class="pm_list_header">
                </td>                
                <td class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_EmpID" OnClick="ChangeOrder_Click"  Text="Emp No" />
                </td>
                <td class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_EmpEngSurname" OnClick="ChangeOrder_Click" Text="Name" />
                </td>                        
                <td class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_EmpRequestType" OnClick="ChangeOrder_Click"  Text="Request Type" />
                </td>
                <%-- Start 0000180, KuangWei, 2015-03-25 --%>
                <td class="pm_list_header" align="center">
                    <asp:LinkButton runat="server" ID="_LeaveCodeDesc" OnClick="ChangeOrder_Click"  Text="Leave Code" />
                </td>                
                <%-- End 0000180, KuangWei, 2015-03-25 --%>
                <td class="pm_list_header">
                    <asp:Label runat="server" Text="Status" />
                </td>
                <td class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_EmpRequestApprovalHistoryCreateDateTime" OnClick="ChangeOrder_Click" Text="Action Date" />
                </td>
            </tr>
            <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
                <ItemTemplate> 
                    <tr >
                        <td class="pm_list">
                        </td>
                        <td class="pm_list">
                            <a id="requestLink" runat="server"  >
                                <%#binding.getValue(Container.DataItem, "EmpNo")%>
                            </a>
                        </td>
                        <td class="pm_list">
                            <%#binding.getValue(Container.DataItem, "EmpEngSurname")%> <%#binding.getValue(Container.DataItem, "EmpEngOtherName")%> <%#binding.getValue(Container.DataItem, "EmpAlias")%>
                        </td>                            
                        <td class="pm_list">
                            <%#binding.getValue(Container.DataItem, "EmpRequestType")%>
                        </td>
                        <%-- Start 0000180, KuangWei, 2015-03-25 --%>
                        <td class="pm_list">
                            <%#binding.getValue(Container.DataItem, "LeaveCodeDesc")%>
                        </td>
                        <%-- End 0000180, KuangWei, 2015-03-25 --%>
                        <td class="pm_list">
                            <%#binding.getValue(Container.DataItem, "EmpRequestApprovalHistoryStatusBefore")%>
                            (<%#binding.getValue(Container.DataItem, "EmpRequestApprovalHistoryAuthorizationWorkFlowIndexBefore")%>) 
                            =>
                            <%#binding.getValue(Container.DataItem, "EmpRequestApprovalHistoryStatusAfter")%>
                            (<%#binding.getValue(Container.DataItem, "EmpRequestApprovalHistoryAuthorizationWorkFlowIndexAfter")%>) 
                        </td>
                        <td class="pm_list">
                            <%#binding.getFValue(Container.DataItem, "EmpRequestApprovalHistoryCreateDateTime", "yyyy-MM-dd HH:mm:ss")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <table width="100%" cellspacing="0" cellpadding="0" class="pm_list_pagenav">
            <tr>
                <td align="right" >
                    <tb:RecordListFooter id="ListFooter" runat="server"
                        OnFirstPageClick="Page_Click"
                        OnPrevPageClick="Page_Click"
                        OnNextPageClick="Page_Click"
                        OnLastPageClick="Page_Click"
                        ListOrderBy="EmpRequestApprovalHistoryCreateDateTime"
                        ListOrder="false" 
                      />
                </td>
            </tr>
        </table>
