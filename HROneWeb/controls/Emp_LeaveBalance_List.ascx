<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Emp_LeaveBalance_List.ascx.cs"
    Inherits="Emp_LeaveBalance_List" %>
<%@ Register Src="WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
<input type="hidden" id="EmpID" runat="server" name="ID" />
<table border="0" width="100%" cellspacing="0" cellpadding="0"  >
    <tr>
        <td class="pm_search_header" >
            <asp:Label Text="As of" runat="server" />:
            <uc1:WebDatePicker id="AsOfDate" runat="server" AutoPostBack="true" />
            
<%--            <asp:Button ID="Go" CssClass="button" runat="server" Text="Go" />
--%>        </td>
        <td class="pm_search_header" align="right" >            
            <asp:Button ID="ReCalc" CssClass="button" runat="server" Text="Re-Calculate" OnClick="ReCalc_Click" />
        </td>
    </tr>
</table>
<table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1" >
    <col width="120px" />
    <col width="50px" />
    <col width="60px" />
    <col span="8" width="50px" />
    <col width="60px" />
    <col span="1" width="50px" />
    <tr>
        <td align="Left" class="pm_list_header" style="font-size:10px">
            <asp:Label Text="Leave Type" runat="server" />
        </td>
        <td align="Left" class="pm_list_header" style="font-size:10px">
            <asp:Label ID="Label2" Text="Unit" runat="server" />
        </td>
        <td align="Left" class="pm_list_header" style="font-size:10px">
            <asp:Label Text="Effective Date" runat="server" />
        </td>
        <td align="right" class="pm_list_header" style="font-size:10px">
            <asp:Label Text="BF" runat="server" />
        </td>
        <td align="right" class="pm_list_header" style="font-size:10px">
            <asp:Label ID="lblForfeiture" Text="Forfeiture" runat="server" />
        </td>
        <td align="right" class="pm_list_header" style="font-size:10px">
            <asp:Label ID="Label5" Text="Expired" runat="server" />
        </td>
        <td align="right" class="pm_list_header" style="font-size:10px">
            <asp:Label ID="Label1" Text="Entitled" runat="server" />
        </td>
        <td align="right" class="pm_list_header" style="font-size:10px">
            <asp:Label Text="Taken" runat="server" />
        </td>
        <td align="right" class="pm_list_header" style="font-size:10px">
            <asp:Label Text="Adjust" runat="server" />
        </td>
        <td align="right" class="pm_list_header" style="font-size:10px">
            <asp:Label Text="Balance" runat="server" />
        </td>
        <td align="right" class="pm_list_header" style="font-size:10px">
            <asp:Label ID="Label3" Text="Will expire" runat="server" />
        </td>
        <td align="right" class="pm_list_header" style="font-size:10px">
            <asp:Label ID="Label4" Text="Expiry Date" runat="server" />
        </td>
        <td align="right" class="pm_list_header" style="font-size:10px">
            <asp:Label ID="lblHeaderReserved" Text="Reserved" runat="server" />
        </td>
        <!--
        <td align="left" class="pm_list_header">
            Debug</td>
            -->
    </tr>
    <asp:Repeater ID="Repeater" runat="server">
        <ItemTemplate>
            <tr>
                <td class="pm_list" align="Left" style="font-size:10px">
                    <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "EmpTab_LeaveBalanceEntitleHistory_View.aspx?EmpID=" + EmpID.Value + "&LeaveTypeID=" + sbinding.getValue(Container.DataItem,"LeaveTypeID"))%>">
                        <%#sbinding.getValue(Container.DataItem, "Description")%>
                    </a>
                </td>
                <td class="pm_list" align="Left" style="font-size:10px">
                    <%#sbinding.getValue(Container.DataItem, "BalanceUnit")%>
                </td>
                <td class="pm_list" style="font-size:10px">
                    <%# sbinding.getFValue(Container.DataItem, "LeaveBalanceEffectiveDate") %>
                </td>
                <td class="pm_list" align="right" style="font-size:10px">
                    <%# sbinding.getFValue(Container.DataItem, "LeaveBalanceBF", "0.0000;;-")%>
                </td>
                <td class="pm_list" align="right" style="font-size:10px">
                    <%#sbinding.getFValue(Container.DataItem, "LeaveBalanceForfeiture", "0.0000;;-")%>
                </td>
                <td class="pm_list" align="right" style="font-size:10px">
                    <%#sbinding.getFValue(Container.DataItem, "ExpiryForfeit", "0.0000;;-")%>
                </td>
                <td class="pm_list" align="right" style="font-size:10px">
                    <%#sbinding.getFValue(Container.DataItem, "LeaveBalanceEntitled", "0.0000;;-")%>
                </td>
                <td class="pm_list" align="right" style="font-size:10px">
                    <%#sbinding.getFValue(Container.DataItem, "Taken", "0.0000;;-")%>
                </td>
                <td class="pm_list" align="right" style="font-size:10px">
                    <%#sbinding.getFValue(Container.DataItem, "Adjust", "0.0000;;-")%>
                </td>
                <td class="pm_list" align="right" style="font-size:10px">
                    <%#((HROne.Lib.Entities.ELeaveBalance)Container.DataItem).getBalance().ToString(((HROne.Lib.Entities.ELeaveBalance)Container.DataItem).StringFormat)%>
                </td>
                <td class="pm_list" align="right" style="font-size:10px">
                    <%#sbinding.getFValue(Container.DataItem, "NextExpiryForfeit", "0.0000;;-")%>
                </td>
                <td class="pm_list" align="right" style="font-size:10px">
                    <%# sbinding.getFValue(Container.DataItem, "NextExpiryDate", "yyyy-MM-dd")%>
                </td>
                <td class="pm_list" align="right" style="font-size:10px">
                    <%#sbinding.getFValue(Container.DataItem, "Reserved", "0.0000;;-")%>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
