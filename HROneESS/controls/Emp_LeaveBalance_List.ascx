<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Emp_LeaveBalance_List.ascx.cs" Inherits="Emp_LeaveBalance_List" %>

<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>

<input type="hidden" id="CurrentEmpID" runat="server" name="ID" />
<input type="hidden" id="m_isSimpleView" runat="server" value="false" />
<input type="hidden" id="m_isViewChangeable" runat="server" value="false" />

<table id="outTable" width="100%" border="0" cellpadding="1" cellspacing="0" class="pm_list_section" >
    <tr>
        <table id="inputTable" width="100%" >
            <tr >
                <td colspan="13" class="pm_list_title">
                    <asp:Label ID="lblHeader" runat="server" Text="As of" /> 
                    <uc1:WebDatePicker id="txtAsOfDate" runat="server" AutoPostBack="true" OnChanged="Go_Click"/>
        <%--                            <asp:Button ID="Go" CssClass="button" runat="server" Text="Submit" OnClick="Go_Click" />
        --%>
                </td>
                <td  align="right">
                    <asp:Button ID="btnExpandView" runat="server" CssClass="button" Text="+"  OnClick="btnExpandView_Click"/>
                    <asp:Button ID="btnCollapseView" runat="server" CssClass="button" Text="-" OnClick="btnCollapseView_Click"/>
                </td>
            </tr>
        </table>
        
        <table id="resultTable" width="100%" border="0" cellpadding="1" cellspacing="0" class="pm_list_section" >
            <col id="col01" width="120px"/>
            <col id="col02" width="50px" />
            <col id="col03" width="60px" />
        <%--     <col span="8" width="50px" />--%>
            <col id="col04" width="50px"  />
            <col id="col05" width="50px" />
            <col id="col06" width="50px" />
            <col id="col07" width="50px" />
            <col id="col08" width="50px" />
            <col id="col09" width="50px" />
            <col id="col10" width="50px" />
            <col id="col11" width="50px" />
            
            <col id="col12" width="60px" />
            <col id="col13" span="1" width="50px" />

            <tr>
                <th align="Left" class="pm_list_header" style="font-size:10px">
                    <asp:Label ID="Label1" Text="Leave Type" runat="server" />
                </th>
                <th align="Left" class="pm_list_header" style="font-size:10px">
                    <asp:Label ID="Label2" Text="Unit" runat="server" />
                </th>
                <th align="Left" class="pm_list_header" style="<%= ShowHideStyle %>">
                    <asp:Label ID="Label3" Text="Effective Date" runat="server" />
                </th>
                <th align="right" class="pm_list_header" style="<%= ShowHideStyle %>">
                    <asp:Label ID="Label4" Text="BF" runat="server" />
                </th>
                <th align="right" class="pm_list_header" style="<%= ShowHideStyle %>">
                    <asp:Label ID="lblForfeiture" Text="Forfeiture" runat="server" />
                </th>
                <th align="right" class="pm_list_header" style="<%= ShowHideStyle %>">
                    <asp:Label ID="Label5" Text="Expired" runat="server" />
                </th>
                <th align="right" class="pm_list_header" style="<%= ShowHideStyle %>">
                    <asp:Label ID="Label6" Text="Entitled" runat="server" />
                </th>
                <th align="right" class="pm_list_header" style="<%= ShowHideStyle %>">
                    <asp:Label ID="Label7" Text="Taken" runat="server" />
                </th>
                <th align="right" class="pm_list_header" style="<%= ShowHideStyle %>">
                    <asp:Label ID="Label8" Text="Adjust" runat="server" />
                </th>
                <th align="right" class="pm_list_header" style="font-size:10px">
                    <asp:Label ID="Label9" Text="Balance" runat="server" />
                </th>
                <th align="right" class="pm_list_header" style="<%= ShowHideStyle %>">
                    <asp:Label ID="Label10" Text="Will expire" runat="server" />
                </th>
                <th align="right" class="pm_list_header" style="<%= ShowHideStyle %>">
                    <asp:Label ID="Label11" Text="Expiry Date" runat="server" />
                </th>
                <th align="right" class="pm_list_header" style="<%= ShowHideStyle %>">
                    <asp:Label ID="lblHeaderReserved" Text="Reserved" runat="server" />
                </th>
                <!--
                <td align="left" class="pm_list_header">
                    Debug</td>
                    -->
            </tr>
            
            <asp:Repeater ID="Repeater" runat="server">
                <ItemTemplate>
                    <tr>
                        <td class="pm_list" align="Left" style="font-size:10px">
                            <%#sbinding.getValue(Container.DataItem, "Description")%>
                        </td>
                        <td class="pm_list" align="Left" style="font-size:10px">
                            <%#sbinding.getValue(Container.DataItem, "BalanceUnit")%>
                        </td>
                        <td class="pm_list" style="<%= ShowHideStyle %>">
                            <%# sbinding.getFValue(Container.DataItem, "LeaveBalanceEffectiveDate") %>
                        </td>
                        <td class="pm_list" align="right" style="<%= ShowHideStyle %>">
                            <%# sbinding.getFValue(Container.DataItem, "LeaveBalanceBF", "0.0000")%>
                        </td>
                        <td class="pm_list" align="right" style="<%= ShowHideStyle %>">
                            <%#sbinding.getFValue(Container.DataItem, "LeaveBalanceForfeiture", "0.0000")%>
                        </td>
                        <td class="pm_list" align="right" style="<%= ShowHideStyle %>">
                            <%#sbinding.getFValue(Container.DataItem, "ExpiryForfeit", "0.0000")%>
                        </td>
                        <td class="pm_list" align="right" style="<%= ShowHideStyle %>">
                            <%#sbinding.getFValue(Container.DataItem, "LeaveBalanceEntitled", "0.0000")%>
                        </td>
                        <td class="pm_list" align="right" style="<%= ShowHideStyle %>">
                            <%#sbinding.getFValue(Container.DataItem, "Taken", "0.0000")%>
                        </td>
                        <td class="pm_list" align="right" style="<%= ShowHideStyle %>">
                            <%#sbinding.getFValue(Container.DataItem, "Adjust", "0.0000")%>
                        </td>
                        <td class="pm_list" align="right" style="font-size:10px">
                            <%#((HROne.Lib.Entities.ELeaveBalance)Container.DataItem).getBalance().ToString(((HROne.Lib.Entities.ELeaveBalance)Container.DataItem).StringFormat)%>
                        </td>
                        <td class="pm_list" align="right" style="<%= ShowHideStyle %>">
                            <%#sbinding.getFValue(Container.DataItem, "NextExpiryForfeit", "0.0000")%>
                        </td>
                        <td class="pm_list" align="right" style="<%= ShowHideStyle %>">
                            <%# sbinding.getFValue(Container.DataItem, "NextExpiryDate", "yyyy-MM-dd")%>
                        </td>
                        <td class="pm_list" align="right" style="<%= ShowHideStyle %>">
                            <%#sbinding.getFValue(Container.DataItem, "Reserved", "0.0000")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </tr>
</table>
