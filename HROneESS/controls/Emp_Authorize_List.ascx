<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Emp_Authorize_List.ascx.cs" Inherits="Emp_Authorize_List" %>

<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<input type="hidden" id="EmpID" runat="server" name="ID" />
        <table  width="100%" border="0" cellpadding="5" cellspacing="0" class="pm_list_section" >
            <col width="20" /> <!-- item select -->
            <col width="60" /> <!-- emp no -->
            <col width="80" /> <!-- name --> 
            <col width="50" /> <!-- application type -->
            <col width="20" /> <!-- leave code -->
            <col width="70" /> <!-- from -->
            <col width="70" /> <!-- to -->
            <col width="40" /> <!-- duration -->
            <col width="40" /> <!-- status -->
            <col width="70" /> <!-- submit date -->
            <col width="70" /> <!-- modify date -->
            
            <tr >
                <!-- Start 0000065, KuangWei, 2014-08-20 -->
                <td colspan="11" class="pm_list_title">
                <!-- End 0000065, KuangWei, 2014-08-20 -->
                    <table width="100%" border="0" cellspacing="1" cellpadding="1">
                        <tr>
                            <td >
                                <asp:Label ID="lblHeader" runat="server" Text="Request From" /> :
                            </td>
                            <td >
                                <uc1:WebDatePicker ID="EmpRequestFromDate" runat="server" ShowDateFormatLabel="false" AutoPostBack="true"/><asp:Label ID="Label2" runat="server" Text="To" />
                                <uc1:WebDatePicker ID="EmpRequestToDate" runat="server" ShowDateFormatLabel="false" AutoPostBack="true"/>
                            </td>
                            <td >
                                <asp:Label runat="server" Text="Status" />:
                            </td>
                            <td >
                                <asp:DropDownList ID="EmpRequestStatus2" runat="Server" AutoPostBack="true" >
                                    <asp:ListItem Value="A" Text="All" />
                                    <asp:ListItem Value="Y" Text="Awaiting your approval" Selected="true" />
                                    <asp:ListItem Value="O" Text="Awaiting other groups approval" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td >
                                <asp:Label ID="Label4" runat="server" Text="Request Type" /> :
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="EmpRequestType" runat="server" AutoPostBack="true" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr >
                <td class="pm_list_header">
                    <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
                </td>                
                <td class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_EmpID" OnClick="ChangeOrder_Click"  Text="Emp No" />
                </td>
                <td class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_EmpEngSurname" OnClick="ChangeOrder_Click" Text="Name" />
                </td>                        
                <td class="pm_list_header" align="center">
                    <asp:LinkButton runat="server" ID="_EmpRequestType" OnClick="ChangeOrder_Click"  Text="Request Type" />
                </td>
                <%-- Start 0000105, KuangWei, 2014-10-20 --%>
                <td class="pm_list_header" align="center">
                    <asp:LinkButton runat="server" ID="_LeaveCode" OnClick="ChangeOrder_Click"  Text="Leave Code" />
                </td>                
                <%-- End 0000105, KuangWei, 2014-10-20 --%>
                <!-- Start 0000065, KuangWei, 2014-08-20 -->
                <td class="pm_list_header" align="center">
                    <asp:LinkButton runat="server" ID="_EmpRequestFromDate" OnClick="ChangeOrder_Click"  Text="From" />
                </td>
                <td class="pm_list_header" align="center">
                    <asp:LinkButton runat="server" ID="_EmpRequestToDate" OnClick="ChangeOrder_Click" Text="To" />
                </td>                        
                <td class="pm_list_header" align="center">
                    <asp:LinkButton runat="server" ID="_EmpRequestDuration" OnClick="ChangeOrder_Click"  Text="Duration" />
                </td>
                <!-- End 0000065, KuangWei, 2014-08-20 -->
                <td class="pm_list_header" align="center">
                    <asp:Label runat="server" Text="Status" />
                </td>
                <td class="pm_list_header" align="center">
                    <asp:LinkButton runat="server" ID="_EmpRequestCreateDate" OnClick="ChangeOrder_Click" Text="Submit Date" />
                </td>
                <td class="pm_list_header" align="center">
                    <asp:LinkButton runat="server" ID="_EmpRequestModifyDate" OnClick="ChangeOrder_Click" Text="Modify Date" />
                </td>                        

            </tr>
            <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
                <ItemTemplate> 
                    <tr >
                        <td class="pm_list">
                            <asp:CheckBox ID="ItemSelect" runat="server" />
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
                            <asp:Label ID="EmpRequestType" runat="server" />
                            <%-- #binding.getValue(Container.DataItem, "EmpRequestType") --%>
                        </td>
                        <%-- Start 0000105, KuangWei, 2014-10-20 --%>
                        <td class="pm_list">
                            <%#binding.getValue(Container.DataItem, "LeaveCode")%>
                        </td>                        
                        <%-- End 0000105, KuangWei, 2014-10-20 --%>
                        <!-- Start 0000065, KuangWei, 2014-08-20 -->
                        <td class="pm_list" align="center">
                            <%#binding.getFValue(Container.DataItem, "EmpRequestFromDate", "yyyy-MM-dd")%>
                            <%#binding.getFValue(Container.DataItem, "EmpRequestFromDateAM")%>
                        </td>
                        <td class="pm_list" align="center">
                            <%#binding.getFValue(Container.DataItem, "EmpRequestToDate", "yyyy-MM-dd")%>
                            <%#binding.getFValue(Container.DataItem, "EmpRequestToDateAM")%>
                        </td>
                        <td class="pm_list">
                            <%#binding.getFValue(Container.DataItem, "EmpRequestDuration", "yyyy-MM-dd")%>
                        </td>
                        <!-- End 0000065, KuangWei, 2014-08-20 -->
                        <td class="pm_list" align="center">
                            <%#binding.getValue(Container.DataItem, "EmpRequestStatus")%>
                            (<%#binding.getValue(Container.DataItem, "EmpRequestLastAuthorizationWorkFlowIndex")%>)
                        </td>
                        <td class="pm_list" align="center">
                            <%#binding.getFValue(Container.DataItem, "EmpRequestCreateDate", "yyyy-MM-dd HH:mm:ss")%>
                        </td>
                        <!-- Start 0000065, KuangWei, 2014-08-20 -->
                        <td class="pm_list" align="center">
                            <%-- Start 0000103, KuangWei, 2014-10-20 --%>
                            <%#binding.getFValue(Container.DataItem, "EmpRequestModifyDate", "yyyy-MM-dd HH:mm:ss")%>
                            <%-- End 0000103, KuangWei, 2014-10-20 --%>
                        </td>
                        <!-- End 0000065, KuangWei, 2014-08-20 -->
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <table width="100%" cellspacing="0" cellpadding="0" class="pm_list_pagenav">
            <tr>
                <td align="right" >
                    <tb:RecordListFooter id="ListFooter" runat="server" ShowAllRecords="true" 
                      />
                </td>
            </tr>
            <tr>
                <td align="left" class="pm_list_title">
                    <asp:Button Text="Authorize" CssClass="button" ID="Authorize" runat="server" OnClick="Authorize_Click"  OnClientClick="return confirm('Are you sure?');" />
                    <asp:Button Text="Reject" CssClass="button" ID="Reject" runat="server" OnClick="Reject_Click"  OnClientClick="return confirm('Are you sure?');" />
                    <br />
                    <!-- Start 0000063, KuangWei, 2014-08-25 -->
                    <asp:Label ID="Label1" runat="server" Text="Reason" />:<asp:TextBox ID="RejectReason" runat="server" Columns="50" MaxLength="100" />
                    <!-- End 0000063, KuangWei, 2014-08-25 -->
                </td>
            </tr>
            <tr  id="DelegateRow" runat="server" >
                <td class="pm_list_title">
                    <asp:Label ID="Label3" runat="server" Text="Delegate Employee No." /> :
                    <asp:TextBox ID="txtDelegateEmpNoList" runat="server" />
                    <asp:Button ID="btnSaveDelegate" runat="server" Text="Save" CssClass="button" OnClick="btnSaveDelegate_Click" />
                </td>
            </tr>
        </table>
