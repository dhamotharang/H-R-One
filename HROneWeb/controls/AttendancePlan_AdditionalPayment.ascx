<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AttendancePlan_AdditionalPayment.ascx.cs" Inherits="AttendancePlan_AdditionalPayment" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
        <input type="hidden" id="AttendancePlanID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="Label21" EnableViewState="false" Text="Additional Payment" runat="server" />
                </td>
            </tr>
        </table>
        
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <asp:Button ID="Delete" CssClass="button" runat="server" EnableViewState="false" Text="Delete" OnClick="Delete_Click" />
                </td>
            </tr>
        </table>


        <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
            <col width="26px" /> 
            <col width="120px" /> 
            <col width="150px" /> 
            <tr>
                <td class="pm_list_header">
                </td>
                <td class="pm_list_header">
                </td>
                <td align="left" class="pm_list_header">
                    <asp:Label runat="server" EnableViewState="false" Text="Payment Code"  />
                </td>
                <td align="left" class="pm_list_header">
                    <asp:Label runat="server" EnableViewState="false" Text="Amount" />
                </td>
                <td align="left" class="pm_list_header">
                    <asp:Label ID="Label8" runat="server" EnableViewState="false" Text="Condition" />
                </td>
            </tr>
            <asp:PlaceHolder ID="AddPanel" runat="server" >
            <tr>
                <td class="pm_list" rowspan="4">
                </td>
                <td class="pm_list" align="center" rowspan="4">
                    <asp:Button ID="Add" runat="server" Text="Add" CssClass="button" OnClick="Add_Click" />
                </td>
                <td class="pm_list" align="left" rowspan="4">
                    <asp:DropDownList ID="PaymentCodeID" runat="server" />
                </td>
                <td class="pm_list" align="left" rowspan="4">
                    <asp:TextBox ID="AttendancePlanAdditionalPaymentAmount" runat="server" />
                </td>
                <td class="pm_list">
                    <asp:Label ID="Label7" runat="server" EnableViewState="false" Text="Late" />&le;
                    <asp:TextBox ID="AttendancePlanAdditionalPaymentMaxLateMins" runat="server" />
                    <asp:Label ID="Label1" runat="server" EnableViewState="false" Text="mins" />
                </td>
            </tr>
            <tr>
                <td class="pm_list">
                    <asp:Label ID="Label2" runat="server" EnableViewState="false" Text="Early Leave" />&le;
                    <asp:TextBox ID="AttendancePlanAdditionalPaymentMaxEarlyLeaveMins" runat="server" />
                    <asp:Label ID="Label3" runat="server" EnableViewState="false" Text="mins" />
                </td>
            </tr>
            <tr>
                <td class="pm_list">
                    <asp:Label ID="Label4" runat="server" EnableViewState="false" Text="Overtime" />&ge;
                    <asp:TextBox ID="AttendancePlanAdditionalPaymentMinOvertimeMins" runat="server" />
                    <asp:Label ID="Label5" runat="server" EnableViewState="false" Text="mins" />
                </td>
            </tr>
            <tr>
                <td class="pm_list">
                    <asp:Label ID="Label6" runat="server" EnableViewState="false" Text="Roster across" />
                    <asp:TextBox ID="AttendancePlanAdditionalPaymentRosterAcrossTime" runat="server" />
                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderAttendancePlanAdditionalPaymentRosterAcrossTime" runat="server" 
                        AcceptAMPM="false" UserTimeFormat="TwentyFourHour" Mask="99:99" MaskType="Time" TargetControlID="AttendancePlanAdditionalPaymentRosterAcrossTime" PromptCharacter="_" > 
                    </ajaxToolkit:MaskedEditExtender>
                </td>
            </tr>
            </asp:PlaceHolder>
            <asp:DataList ID="Repeater" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal"
                ShowFooter="false" ShowHeader="false" OnItemCommand="Repeater_ItemCommand" OnItemDataBound="Repeater_ItemDataBound">
                <EditItemTemplate>
                    <tr>
                        <td class="pm_list" align="center" rowspan="4">
                            <asp:CheckBox ID="DeleteItem" runat="server" />
                            <input type="hidden" runat="server" id="AttendancePlanAdditionalPaymentID" />
                        </td>
                        <td class="pm_list" align="center" rowspan="4">
                            <asp:Button ID="Save" runat="server" EnableViewState="false" Text="Save" CssClass="button" />
                            <asp:Button ID="Cancel" runat="server" EnableViewState="false" Text="Cancel" CssClass="button" />
                        </td>
                        <td class="pm_list" align="left" rowspan="4">
                            <asp:DropDownList ID="PaymentCodeID" runat="server" />
                        </td>
                        <td class="pm_list" align="left" rowspan="4">
                            <asp:TextBox ID="AttendancePlanAdditionalPaymentAmount" runat="server" />
                        </td>
                        <td class="pm_list">
                            <asp:Label runat="server" EnableViewState="false" Text="Late" />&le;
                            <asp:TextBox ID="AttendancePlanAdditionalPaymentMaxLateMins" runat="server" />
                            <asp:Label ID="Label1" runat="server" EnableViewState="false" Text="mins" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pm_list">
                            <asp:Label ID="Label2" runat="server" EnableViewState="false" Text="Early Leave" />&le;
                            <asp:TextBox ID="AttendancePlanAdditionalPaymentMaxEarlyLeaveMins" runat="server" />
                            <asp:Label ID="Label3" runat="server" EnableViewState="false" Text="mins" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pm_list">
                            <asp:Label ID="Label4" runat="server" EnableViewState="false" Text="Overtime" />&ge;
                            <asp:TextBox ID="AttendancePlanAdditionalPaymentMinOvertimeMins" runat="server" />
                            <asp:Label ID="Label5" runat="server" EnableViewState="false" Text="mins" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pm_list">
                            <asp:Label ID="Label6" runat="server" EnableViewState="false" Text="Roster across" />
                            <asp:TextBox ID="AttendancePlanAdditionalPaymentRosterAcrossTime" runat="server" />
                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderAttendancePlanAdditionalPaymentRosterAcrossTime" runat="server" 
                                AcceptAMPM="false" UserTimeFormat="TwentyFourHour" Mask="99:99" MaskType="Time" TargetControlID="AttendancePlanAdditionalPaymentRosterAcrossTime" PromptCharacter="_" > 
                            </ajaxToolkit:MaskedEditExtender>
                        </td>
                    </tr>
                </EditItemTemplate>
                <ItemTemplate>
                  <tr>
                        <td class="pm_list" align="center" rowspan="4">
                            <asp:CheckBox ID="DeleteItem" runat="server" />
                            <input type="hidden" runat="server" id="AttendancePlanAdditionalPaymentID" />
                        </td>
                        <td class="pm_list" align="center" rowspan="4">
                            <asp:Button ID="Edit" runat="server" EnableViewState="false" Text="Edit" CssClass="button" />
                        </td>
                        <td class="pm_list" align="left" rowspan="4">
                            <%#sbinding.getValue(Container.DataItem, "PaymentCodeID")%>
                        </td>
                        <td class="pm_list" align="left" rowspan="4">
                            <%#sbinding.getFValue(Container.DataItem, "AttendancePlanAdditionalPaymentAmount", "0.00")%>
                        </td>
                        <td class="pm_list">
                            <asp:Label ID="Label9" runat="server" EnableViewState="false" Text="Late" />&le;
                            <%#sbinding.getValue(Container.DataItem, "AttendancePlanAdditionalPaymentMaxLateMins")%>
                            <asp:Label ID="Label1" runat="server" EnableViewState="false" Text="mins" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pm_list">
                            <asp:Label ID="Label2" runat="server" EnableViewState="false" Text="Early Leave" />&le;
                            <%#sbinding.getValue(Container.DataItem, "AttendancePlanAdditionalPaymentMaxEarlyLeaveMins")%>
                            <asp:Label ID="Label3" runat="server" EnableViewState="false" Text="mins" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pm_list">
                            <asp:Label ID="Label4" runat="server" EnableViewState="false" Text="Overtime" />&ge;
                            <%#sbinding.getValue(Container.DataItem, "AttendancePlanAdditionalPaymentMinOvertimeMins")%>
                            <asp:Label ID="Label5" runat="server" EnableViewState="false" Text="mins" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pm_list">
                            <asp:PlaceHolder ID="AttendancePlanAdditionalPaymentRosterAcrossTimePanel" runat="server" >
                            <asp:Label ID="Label6" runat="server" EnableViewState="false" Text="Roster across" />
                            <asp:Label ID="AttendancePlanAdditionalPaymentRosterAcrossTime" runat="server" />
                            </asp:PlaceHolder>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:DataList>
            </table>
        <tb:RecordListFooter id="ListFooter" runat="server"
             ShowAllRecords="true" 
          />
