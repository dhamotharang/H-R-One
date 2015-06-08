<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Payroll_ORSORecordList.ascx.cs" Inherits="Payroll_ORSORecordList" %>

<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<input type="hidden" id="EmpPayrollID" runat="server" name="ID" />
<input type="hidden" id="EmpPayStatus" runat="server" />


<table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_section_title">
    <tr>
        <td>
            <asp:Label ID="ILabel1" runat="server" Text="P-Fund Detail" />
            :
        </td>
    </tr>
</table>
<table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
    <tr>
        <td>
            <asp:Button ID="Delete" CssClass="button" runat="server" Text="Delete" OnClick="Delete_Click" />
            <asp:Button ID="btnRecalculate" CssClass="button" runat="server" Text="Recalculate" OnClick="Recalculate_Click" />
        </td>
    </tr>
</table>
<table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
    <col width="26px" /> 
    <col width="50px" /> 
    <tr>
        <td class="pm_list_header">
        </td>
        <td class="pm_list_header">
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_ORSORecPeriodFr" OnClick="ChangeOrder_Click" Text="From" />
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_ORSORecPeriodTo" OnClick="ChangeOrder_Click" Text="To" />
        </td>
        <td align="right" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_ORSORecActRI" OnClick="ChangeOrder_Click" Text="R.I." />
        </td>
        <td align="right" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_ORSORecActER" OnClick="ChangeOrder_Click" Text="Employer" />
        </td>
        <td align="right" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_ORSORecActEE" OnClick="ChangeOrder_Click" Text="Employee" />
        </td>
        <td align="center" class="pm_list_header">
            <asp:Label Text="P-Fund Plan" runat="server"/>
        </td>        

    </tr>
    <asp:Panel ID="AddPanel" runat="server">
        <tr>
            <td class="pm_list">
            </td>
            <td class="pm_list" align="center">
                <asp:Button ID="Add" runat="server" Text="Add" CssClass="button" OnClick="Add_Click" />
            </td>
            <td class="pm_list">
                <uc1:WebDatePicker id="ORSORecPeriodFr" runat="server" ShowDateFormatLabel="false" />
            </td>
            <td class="pm_list">
                <uc1:WebDatePicker id="ORSORecPeriodTo" runat="server" ShowDateFormatLabel="false" />
            </td>
            <td class="pm_list" align="right">
                <asp:TextBox ID="ORSORecActRI" runat="server" Columns="10" style="text-align:right" />
            </td>
            <td class="pm_list" align="right">
                <asp:TextBox ID="ORSORecActER" runat="server" Columns="5" style="text-align:right" />
            </td>
            <td class="pm_list" align="right">
                <asp:TextBox ID="ORSORecActEE" runat="server" Columns="5" style="text-align:right" />
            </td>
            <td class="pm_list" align="center">
                <asp:DropDownList ID="ORSOPlanID" runat="server">
                </asp:DropDownList>
            </td>
            
        </tr>
    </asp:Panel>
    <asp:DataList ID="Repeater" runat="server" RepeatLayout="Flow" ShowFooter="false" ShowHeader="false" OnItemCommand="Repeater_ItemCommand" OnItemDataBound="Repeater_ItemDataBound" RepeatDirection="Horizontal">
        <EditItemTemplate>
            <tr>
                <td class="pm_list" align="center">
                    <asp:CheckBox ID="DeleteItem" runat="server" />
                    <%--  Start 0000162, Ricky So, 2015-01-25 --%>
                    <%-- 
                    <asp:Button ID="btnRecalculate" CssClass="button" runat="server" Text="Recalculate" OnClick="Recalculate_Click" />
                     --%>
                    <%--  End 0000162, Ricky So, 2015-01-25 --%>
                    <input type="hidden" runat="server" id="ORSORecordID" />
                </td>
                <td class="pm_list" align="center">
                    <asp:Button ID="Save" CssClass="button" runat="server" Text="Save" />
                    <asp:Button ID="Cancel" CssClass="button" runat="server" Text="Cancel" />
                </td>
                <td class="pm_list">
                    <uc1:WebDatePicker id="ORSORecPeriodFr" runat="server" ShowDateFormatLabel="false" />
                </td>
                <td class="pm_list">
                    <uc1:WebDatePicker id="ORSORecPeriodTo" runat="server" ShowDateFormatLabel="false" />
                </td>
                <td class="pm_list" align="right">
                    <asp:TextBox ID="ORSORecActRI" runat="server" Columns="10" style="text-align:right" />
                </td>
                <td class="pm_list" align="right">
                    <asp:TextBox ID="ORSORecActER" runat="server" Columns="5" style="text-align:right" />
                </td>
                <td class="pm_list" align="right">
                    <asp:TextBox ID="ORSORecActEE" runat="server" Columns="5" style="text-align:right" />
                </td>
                <td class="pm_list" align="center">
                    <asp:DropDownList ID="ORSOPlanID" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
        </EditItemTemplate>
        <ItemTemplate>
            <tr>
                <td class="pm_list" align="center">
                    <asp:CheckBox ID="DeleteItem" runat="server" />
                    <input type="hidden" runat="server" id="ORSORecordID" />
                </td>
                <td class="pm_list" align="center">
                    <asp:Button ID="Edit" CssClass="button" runat="server" Text="Edit" />
                </td>
                <td class="pm_list">
                    <%#sBinding.getFValue(Container.DataItem, "ORSORecPeriodFr", "yyyy-MM-dd")%>
                </td>
                <td class="pm_list">
                    <%#sBinding.getFValue(Container.DataItem, "ORSORecPeriodTo", "yyyy-MM-dd")%>
                </td>
                <td class="pm_list" align="right">
                    <%#sBinding.getFValue(Container.DataItem, "ORSORecActRI", "$#,##0.00")%>
                </td>
                <td class="pm_list" align="right">
                    <%#sBinding.getFValue(Container.DataItem, "ORSORecActER", "$#,##0.00")%>
                </td>
                <td class="pm_list" align="right">
                    <%#sBinding.getFValue(Container.DataItem, "ORSORecActEE", "$#,##0.00")%>
                </td>
                <td class="pm_list" align="center">
                    <%#sBinding.getValue(Container.DataItem, "ORSOPlanID")%>
                </td>
            </tr>
        </ItemTemplate>
    </asp:DataList>
    <tr>
        <td class="pm_list" align="right" colspan="4">
            <tb:RecordListFooter id="ListFooter" runat="server"
                ShowAllRecords="true" Visible="false" 
              />
            <asp:Label ID="ILabel2" runat="server" Text="Total" />
        </td>
        <td class="pm_list" align="right" colspan="1">
            <asp:Label ID="lblORSORITotal" runat="server" Text="Label"></asp:Label>
        </td>
        <td class="pm_list" align="right" colspan="1">
            <asp:Label ID="lblORSOERTotal" runat="server" Text="Label"></asp:Label>
        </td>
        <td class="pm_list" align="right" colspan="1">
            <asp:Label ID="lblORSOEETotal" runat="server" Text="Label"></asp:Label>
        </td>
   </tr>
</table>
