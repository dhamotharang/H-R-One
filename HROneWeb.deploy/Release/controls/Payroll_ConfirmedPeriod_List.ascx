<%@ control language="C#" autoeventwireup="true" inherits="controls_Payroll_ConfirmedPeriod_List, HROneWeb.deploy" %>

<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
<%@ Register Src="RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
<%-- Start 0000004, Miranda, 2014-06-19 --%>
<input type="hidden" id="hiddenFieldDefaultMonthPeriod" runat="server" value="1" />
<%-- End 0000004, Miranda, 2014-06-19 --%>

        <table width="100%" class="pm_section">
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label1" runat="server" Text="Payroll Period" />:
                </td>
                <td class="pm_field">
                    <%-- Start 0000004, Miranda, 2014-06-19 --%>
                    <uc1:WebDatePicker id="PayPeriodFr" runat="server" OnChanged="PayPeriod_Changed" AutoPostBack="true" />
                    -
                    <uc1:WebDatePicker id="PayPeriodTo" runat="server" OnChanged="PayPeriod_Changed" AutoPostBack="true" />
                    <%-- End 0000004, Miranda, 2014-06-19 --%>
                    <asp:Button ID="Search" runat="server" Text="Search" CssClass="button" OnClick="Search_Click" />
                </td>
            </tr>
        </table>
            <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
                <col width="26px" />
                <col width="100px" />
                <col width="100px" />
                <col width="150px" />
                <col width="150px" />
                <tr>
                    <td class="pm_list_header" align="center">
                        <input type="checkbox" onclick="checkAll('<%=payPeriodRepeater.ClientID %>','ItemSelect',this.checked);" />
                    </td>
                    <td align="left" class="pm_list_header" colspan="2">
                        <asp:LinkButton runat="server" ID="_PayPeriodFr" OnClick="ChangeOrder_Click" Text="Period" />
                    </td>
                    <td align="left" class="pm_list_header">
                        <asp:LinkButton runat="server" ID="_PayPeriodConfirmDate" OnClick="ChangeOrder_Click"  Text="Confirmed Date" />
                    </td>
                    <td align="left" class="pm_list_header">
                        <asp:LinkButton runat="server" ID="_PayPeriodConfirmBy" OnClick="ChangeOrder_Click"  Text="Confirmed By" />
                    </td>
                </tr>
                <asp:Repeater ID="payPeriodRepeater" runat="server" OnItemDataBound="payPeriodRepeater_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td class="pm_list" align="center">
                                <asp:CheckBox ID="ItemSelect" runat="server" Checked="True" />
                            </td>
                            <td class="pm_list">
                                <%#payPeriodSBinding.getFValue(Container.DataItem, "PayPeriodFr","yyyy-MM-dd")%>
                            </td>
                            <td class="pm_list">
                                <%#payPeriodSBinding.getFValue(Container.DataItem, "PayPeriodTo", "yyyy-MM-dd")%>
                            </td>
                            <td class="pm_list">
                                <%#payPeriodSBinding.getFValue(Container.DataItem, "PayPeriodConfirmDate", "yyyy-MM-dd HH:mm:ss")%>
                            </td>
                            <td class="pm_list">
                                <%#payPeriodSBinding.getValue(Container.DataItem, "PayPeriodConfirmBy")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <table width="100%" cellspacing="0" cellpadding="0" class="pm_list_pagenav">
                <tr>
                    <td>
                    </td>
                    <td align="right">
                        <tb:RecordListFooter id="ListFooter" runat="server"
                            ListOrderBy="PayPeriodFr"
                            ListOrder="false" 
                            ShowAllRecords="true" 
                          />
                    </td>
                </tr>
            </table>
