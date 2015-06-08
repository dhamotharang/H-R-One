<%@ control language="C#" autoeventwireup="true" inherits="LeavePlanEntitle_List, HROneWeb.deploy" %>
<%@ Register Src="DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %> 

<input type="hidden" id="LeavePlanID" runat="server" name="ID" />
<input type="hidden" id="LeaveTypeID" runat="server" name="ID" />
<table border="0" width="323px" class="pm_list_section" cellspacing="0" cellpadding="0">
    <col width="150px" />
    <col width="150px" />
    <col  />
    <tr>
        <td class="pm_field_title" colspan="3" >
            <asp:Label ID="LeaveTypeDesc"  runat="server" />
        </td>
<%--        <td class="pm_field_title" />
        <td class="pm_field_title" width="120px"/>
--%>
    </tr> 
    <tr id="LeavePlanBroughtForwardViewCell" runat="server" visible="true" >
        <td valign="top">
            <tr class="pm_field_header_no_border" style="min-height:32px">
                <td >
                    &nbsp
                    <asp:Label ID="LeavePlanBroughtForwardName" runat="server" Text="Max. B/F"/>:
                </td>
                <td>
                    <asp:Label ID="lblLeavePlanBroughtForward" Font-Size="Larger" runat="server"  />&nbsp&nbsp&nbsp<asp:Label ID="Label2" runat="server" Text="Day(s)" />
                </td>
                <td class="pm_field_header_no_border" style="text-align:right">
                    <asp:Button ID="btnEditLeavePlanBroughtForward" runat="server" CSSClass="button" OnClick="btnEditLeavePlanBroughtForward_Click" Width="70" Text="Edit"/>
                </td>
            </tr>
            <tr class="pm_field_header_no_border" style="min-height:32px">
                <td>
                    &nbsp
                    <asp:Label ID="Label4" runat="server" Text="Expiry after"/>:
                </td>
                <td >
                    <asp:Label ID="lblLeavePlanBroughtForwardNumOfMonthExpired" Font-Size="Larger" runat="server" />&nbsp&nbsp&nbsp<asp:Label ID="Label5" runat="server" Text="Month(s)" />
                </td>
                <td>
                </td>
            </tr>
        </td>
        
    </tr>
    
    
    <tr id="LeavePlanBroughtForwardEditCell" runat="server" visible="false">        
        <td valign="top" >
            <tr class="pm_field_header_no_border"  style="min-height:32px">
                <td>
                    &nbsp
                    <asp:Label ID="Label1" runat="server" Text="Max. B/F"/>:
                </td>
                <td >
                    <asp:TextBox ID="txtLeavePlanBroughtForward" runat="server" MaxLength="4" Columns="2" style="text-align:right" />&nbsp
                    <asp:Label ID="Label3" runat="server" Text="Day(s)" />
                </td>
                <td class="pm_field_header_no_border" style="text-align:right;">
                    <asp:Button ID="btnSaveLeavePlanBroughtForward" runat="server" CSSClass="button" OnClick="btnSaveLeavePlanBroughtForward_Click" Width="70" Text="Save"/>
                </td>
            </tr>
            <tr class="pm_field_header_no_border"  style="min-height:32">
                <td >
                    &nbsp
                    <asp:Label ID="LeavePlanBroughtForwardNumOfMonthExpiredName" runat="server" Text="Expiry after"/>:
                </td>
                <td >
                    <asp:TextBox ID="txtLeavePlanBroughtForwardNumOfMonthExpired" runat="server" MaxLength="4" Columns="2" style="text-align:right"/>&nbsp
                    <asp:Label ID="lblMonths1" runat="server" Text="Month(s)" />
                </td>
                <td class="pm_field_header_no_border" style="text-align:right;">
                    <asp:Button ID="btnCancelLeavePlanBroughtForward" runat="server" CSSClass="button" OnClick="btnCancelLeavePlanBroughtForward_Click" Width="70" Text="Cancel"/>
                </td>
            </tr>
        </td>

    </tr>
</table>    


<%--leave entitle info section--%>
<table border="0" width="323px" class="pm_list_section" cellspacing="0" cellpadding="0">
    <col width="70px"/>
    <col width="120px"/>
    <col />
    <col />
    <tr>
        <td class="pm_list_header" ></td>
        <td class="pm_list_header" ></td>
        <td align="center" class="pm_list_header" >
            <asp:Label id="lblDesc1" Text="Year of Service less than" runat="server" /></td>
        <td align="center" class="pm_list_header" colspan="2">
            <asp:Label id="lblDesc2" Text="Days Entitled" runat="server" /></td>
    </tr>
    <tr id="AddPanel" runat="server">
        <td class="pm_list" align="center" >
            <asp:Button Text="Delete" width="50" CssClass="button" ID="Delete" runat="server" OnClick="Delete_Click" />
        </td>
        <td class="pm_list" align="center" >
            <asp:Button ID="Add" width="50" runat="server" Text="Add" CssClass="button" OnClick="Add_Click" /></td>
        <td class="pm_list" align="center" >
            <asp:TextBox ID="LeavePlanEntitleYearOfService" runat="server" style="text-align:center;"/></td>
        <td class="pm_list" align="center" colspan="2" >
            <asp:TextBox ID="LeavePlanEntitleDays" runat="server" style="text-align:center;"/></td>
    </tr>
    
    

    <asp:DataList ID="Repeater" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" ShowFooter="false"
        ShowHeader="false" OnItemCommand="Repeater_ItemCommand" OnItemDataBound="Repeater_ItemDataBound">
        <ItemTemplate>
            <tr>
                <td class="pm_list" align="center">
                    <asp:CheckBox ID="DeleteItem" runat="server" />
                    <input type="hidden" runat="server" id="LeavePlanEntitleID" />
                </td>
                <td class="pm_list" align="center">
                    <asp:Button ID="Edit" width="50" CssClass="button" runat="server" Text="Edit" />
                </td>
                <td class="pm_list" align="center" >
                    <%#sbinding.getValue(Container.DataItem,"LeavePlanEntitleYearOfService")%>
                </td>
                <td class="pm_list" align="center">
                    <%#sbinding.getValue(Container.DataItem,"LeavePlanEntitleDays")%>
                </td>
            </tr>
        </ItemTemplate>
        
        <EditItemTemplate>
            <tr>
                <td class="pm_list" align="center">
                    <asp:CheckBox ID="DeleteItem" runat="server" />
                    <input type="hidden" runat="server" id="LeavePlanEntitleID" />
                </td>
                <td class="pm_list" align="center">
                    <asp:Button ID="Save" Width="50" runat="server" Text="Save" CSSClass="button" />
                    <asp:Button ID="Cancel" Width="50" runat="server" Text="Cancel" CSSClass="button" />
                </td>
                <td class="pm_list" align="center" >
                    <asp:TextBox ID="LeavePlanEntitleYearOfService" runat="server" style="text-align:center;" />
                </td>
                <td class="pm_list" align="center">
                    <asp:TextBox ID="LeavePlanEntitleDays" runat="server" style="text-align:center;"/>
                </td>
            </tr>
        </EditItemTemplate>
    </asp:DataList>
    
    </table>
    
    
    <tb:RecordListFooter id="ListFooter" runat="server"
        ShowAllRecords="true" 
      />
<br />
