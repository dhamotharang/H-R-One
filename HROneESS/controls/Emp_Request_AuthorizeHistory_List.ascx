<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Emp_Request_AuthorizeHistory_List.ascx.cs" Inherits="Emp_Request_AuthorizeHistory_List" %>

<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<input type="hidden" id="EmpID" runat="server" name="ID" />
        <table  width="100%" border="0" cellpadding="5" cellspacing="0" class="pm_list_section" >
            <tr >
                <td colspan="4" class="pm_list_title">
                    <asp:Label runat="server" EnableViewState="false" Text="Approval History"/>
                </td>
            </tr>
            <tr >
                <td class="pm_list_header">
                </td>                
                <td class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_EmpRequestApprovalHistoryCreateDateTime" OnClick="ChangeOrder_Click" Text="Action Date" />
                </td>
                <td class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_EmpRequestApprovalHistoryActionBy" OnClick="ChangeOrder_Click" Text="Action By" />
                </td>
                <td class="pm_list_header">
                    <asp:Label runat="server" EnableViewState="false" Text="Status" />
                </td>
            </tr>
            <asp:Repeater ID="Repeater" runat="server" >
                <ItemTemplate> 
                    <tr >
                        <td class="pm_list">
                        </td>
                        <td class="pm_list">
                            <%#binding.getFValue(Container.DataItem, "EmpRequestApprovalHistoryCreateDateTime", "yyyy-MM-dd HH:mm:ss")%>
                        </td>
                        <td class="pm_list">
                            <%#binding.getValue(Container.DataItem, "EmpRequestApprovalHistoryActionBy")%>
                        </td>
                        <td class="pm_list">
                            <%#binding.getValue(Container.DataItem, "EmpRequestApprovalHistoryStatusBefore")%>
                            (<%#binding.getValue(Container.DataItem, "EmpRequestApprovalHistoryAuthorizationWorkFlowIndexBefore")%>) 
                            =>
                            <%#binding.getValue(Container.DataItem, "EmpRequestApprovalHistoryStatusAfter")%>
                            (<%#binding.getValue(Container.DataItem, "EmpRequestApprovalHistoryAuthorizationWorkFlowIndexAfter")%>) 
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <table width="100%" cellspacing="0" cellpadding="0" class="pm_list_pagenav">
            <tr>
                <td align="right" >
                    <tb:RecordListFooter id="ListFooter" runat="server" ShowAllRecords="true" 
                        ListOrderBy="EmpRequestApprovalHistoryCreateDateTime"
                        ListOrder="false" 
                      />
                </td>
            </tr>
        </table>
