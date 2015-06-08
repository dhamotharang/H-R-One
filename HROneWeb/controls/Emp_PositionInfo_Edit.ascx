<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Emp_PositionInfo_Edit.ascx.cs" Inherits="Emp_PositionInfo_Edit_Control" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
        <input type="hidden" id="ID" runat="server" name="ID" />
        <input type="hidden" id="HiddenEmpID" runat="server" name="OldID" />
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_field_section">
            <col width="15%" />
            <col width="35%" />
            <col width="15%" />
            <col width="35%" />
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="ILabel1" Text="From" runat="server" />
                    :</td>
                <td class="pm_field" >
                    <uc1:WebDatePicker id="EmpPosEffFr" runat="server" />
                </td>
                <td class="pm_field_header" >
                    <asp:Label ID="ILabel2" Text="To" runat="server" />
                    :</td>
                <td class="pm_field" >
                    <uc1:WebDatePicker id="EmpPosEffTo" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="ILabel4" Text="Company" runat="server" />
                    :</td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="CompanyID" runat="Server" AutoPostBack="true" OnSelectedIndexChanged="CompanyID_SelectedIndexChanged" /></td>
            </tr>
            <tr>
                <td colspan="2" valign="top" style="padding: 0; border-style: none; border-width: 0">
                    <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_field_section">
                        <col width="30%" />
                        <col width="70%" />
                        <asp:Repeater ID="HierarchyLevel" runat="server" OnItemDataBound="HierarchyLevel_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td class="pm_field_header">
                                        <%#((HROne.Lib.Entities.EHierarchyLevel )Container.DataItem).HLevelDesc%>
                                    </td>
                                    <td class="pm_field">
                                        <asp:DropDownList ID="HElementID" runat="server" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr>
                            <td class="pm_field_header" >
                                <asp:Label ID="ILabel3" Text="Rank" runat="server" />
                                :</td>
                            <td class="pm_field" >
                                <asp:DropDownList ID="RankID" runat="Server" /></td>
                        </tr>
                        <tr>
                            <td class="pm_field_header">
                                <asp:Label ID="Label3" Text="Position" runat="server" />:</td>
                            <td class="pm_field">
                                <asp:DropDownList ID="PositionID" runat="Server" /></td>
                        </tr>
                        <tr>
                            <td class="pm_field_header" >
                                <asp:Label ID="Label4" Text="Employment Type" runat="server" />
                                :</td>
                            <td class="pm_field" >
                                <asp:DropDownList ID="EmploymentTypeID" runat="Server" /></td>
                        </tr>
                        <tr>
                            <td class="pm_field_header" >
                                <asp:Label ID="ILabel7" Text="Staff Type" runat="server" />
                                :</td>
                            <td class="pm_field" >
                                <asp:DropDownList ID="StaffTypeID" runat="Server" /></td>
                        </tr>
                        <tr>
                            <td class="pm_field_header">
                                <asp:Label ID="Label5" Text="Remark" runat="server" />:</td>
                            <td class="pm_field">
                                <asp:TextBox ID="EmpPosRemark" runat="Server" TextMode="MultiLine" Columns="30" Rows="3" /></td>
                        </tr>
                    </table>
                </td>
                <td colspan="2" valign="top" style="padding: 0; border-style: none; border-width: 0">
                    <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_field_section">
                        <col width="30%" />
                        <col width="70%" />
                        <tr>
                            <td class="pm_field_header">
                                <asp:Label ID="Label6" Text="Payroll Group" runat="server" />
                                :</td>
                            <td class="pm_field">
                                <asp:DropDownList ID="PayGroupID" runat="Server" /></td>
                        </tr>
                        <tr>
                            <td class="pm_field_header">
                                <asp:Label ID="Label7" Text="Leave Plan" runat="server" />:
                            </td>
                            <td class="pm_field">
                                <asp:DropDownList ID="LeavePlanID" runat="Server" /><br />
                                <asp:CheckBox ID="EmpPosIsLeavePlanResetEffectiveDate" Text="Reset Grant Date" runat="server" />
                            </td>
                        </tr>
                        <tr id="WorkHourPatternRow" runat="server" >
                            <td class="pm_field_header">
                                <asp:Label ID="Label8" Text="Work Hour Pattern" runat="server" />
                                :</td>
                            <td class="pm_field">
                                <asp:DropDownList ID="WorkHourPatternID" runat="Server" /></td>
                        </tr>
                        <tr>
                            <td class="pm_field_header">
                                <asp:Label ID="ILabel8" Text="YEB Plan" runat="server" />
                                :</td>
                            <td class="pm_field">
                                <asp:DropDownList ID="YEBPlanID" runat="Server" /></td>
                        </tr>
                        <asp:Panel ID="ESSAuthorizationPanel" runat="server" >
                        <tr>
                            <td class="pm_field_header">
                                <asp:Label ID="ILabel5" Text="Authorization Workflow for Leave Application" runat="server" />:
                            </td>
                            <td class="pm_field">
                                <asp:DropDownList ID="AuthorizationWorkFlowIDLeaveApp" runat="Server" />
                            </td>
                        </tr>
                        <%-- Start 0000112, Miranda, 2015-01-11 --%>
                        <tr>
                            <td class="pm_field_header">
                                <asp:Label ID="Label2" Text="Authorization Workflow for Late Waive" runat="server" />:
                            </td>
                            <td class="pm_field">
                                <asp:DropDownList ID="AuthorizationWorkFlowIDLateWaive" runat="Server" />
                            </td>
                        </tr>
                        <%-- End 0000112, Miranda, 2015-01-11 --%>
                        <tr>
                            <td class="pm_field_header" >
                                <asp:Label ID="ILabel6" Text="Authorization Workflow for Personal Information Change" runat="server" />:
                            </td>
                            <td class="pm_field">
                                <asp:DropDownList ID="AuthorizationWorkFlowIDEmpInfoModified" runat="Server" />
                            </td>
                        </tr>
                        <%-- Start 0000060, Miranda, 2014-07-13 --%>
                        <tr id="eot_row" runat="server">
                            <td class="pm_field_header">
                                <asp:Label ID="Label1" Text="Authorization Workflow CL Requisitions" runat="server" />:
                            </td>
                            <td class="pm_field">
                                <asp:DropDownList ID="AuthorizationWorkFlowIDOTClaims" runat="Server" />
                            </td>
                        </tr>
                        <%-- End 0000060, Miranda, 2014-07-13 --%>
                        <%-- Start 0000112, Miranda, 2014-12-10 --%>
                        <%-- Start 0000112, Miranda, 2015-01-11 --%>
                        <%--<tr>
                            <td class="pm_field_header">
                                <asp:Label ID="Label2" Text="Authorization Workflow for Late Waive" runat="server" />:
                            </td>
                            <td class="pm_field">
                                <asp:DropDownList ID="AuthorizationWorkFlowIDLateWaive" runat="Server" />
                            </td>
                        </tr>--%>
                        <%-- End 0000112, Miranda, 2015-01-11 --%>
                        <%-- End 0000112, Miranda, 2014-12-10 --%>
                        </asp:Panel>
                        <asp:Panel ID="AttendancePanel" runat="server" >
<%--
                        <tr>
                            <td class="pm_field_header">
                                <asp:Label ID="ILabel10" Text="Default Roster Code" runat="server" />
                                :</td>
                            <td class="pm_field">
                                <asp:DropDownList ID="EmpPosDefaultRosterCodeID" runat="Server" /></td>
                        </tr>
--%>
                        <tr>
                            <td class="pm_field_header">
                                <asp:Label ID="ILabel9" Text="Attendance Plan" runat="server" />
                                :</td>
                            <td class="pm_field">
                                <asp:DropDownList ID="AttendancePlanID" runat="Server" /></td>
                        </tr>
<%--                        <tr>
                            <td class="pm_field_header">
                                <asp:Label ID="Label9" Text="Roster Table Group" runat="server" />
                                :</td>
                            <td class="pm_field">
                                <asp:DropDownList ID="RosterTableGroupID" runat="Server" />
                                <asp:CheckBox ID="EmpPosIsRosterTableGroupSupervisor" runat="server" Text="Supervisor" />
                            </td>
                        </tr>--%>
                        </asp:Panel>
                    </table>
                </td>
            </tr>
        </table>
