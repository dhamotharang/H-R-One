<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Emp_PositionInfo_View.aspx.cs" Inherits="Emp_PositionInfo_View" MasterPageFile="~/MainMasterPage.master"  %>

<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/Emp_Common.ascx" TagName="Emp_Common" TagPrefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="EmpPosID" runat="server" name="ID" />
        <input type="hidden" id="EmpID" runat="server" name="ID" />
        <%-- Start 0000125, Miranda, 2014-11-19 --%>
        <input type="hidden" id="CompanyID" runat="server" name="CompanyID" />
        <input type="hidden" id="PayGroupID" runat="server" name="PayGroupID" />
        <input type="hidden" id="LeavePlanID" runat="server" name="LeavePlanID" />
        <input type="hidden" id="WorkHourPatternID" runat="server" name="WorkHourPatternID" />
        <input type="hidden" id="YEBPlanID" runat="server" name="YEBPlanID" />
        <input type="hidden" id="AttendancePlanID" runat="server" name="AttendancePlanID" />
        <%-- End 0000125, Miranda, 2014-11-19 --%>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label Text="Employee Information" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label Text="View Position Information" runat="server" />
                </td>
            </tr>
        </table>
        
             
        
        <uc1:Emp_Common ID="Emp_Common1" runat="server"></uc1:Emp_Common>
        <br />
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar ID="toolBar" runat="server" NewButton_Visible="false" SaveButton_Visible="false" OnBackButton_Click="Back_Click" OnEditButton_Click="Edit_Click" OnDeleteButton_Click="Delete_Click" />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
                </td>
            </tr>
        </table>
        <table border="0" width="100%" cellpadding="" cellspacing="0" class="pm_field_section">
            <col width="15%" />
            <col width="35%" />
            <col width="15%" />
            <col width="35%" />
            <tr >
                <td class="pm_field_header" >
                    <asp:Label Text="From" runat="server" />
                    :</td>
                <td class="pm_field" >
                    <asp:Label ID="EmpPosEffFr" runat="Server"  />
                </td>
                <td class="pm_field_header" >
                    <asp:Label Text="To" runat="server" />
                    :</td>
                <td class="pm_field" >
                    <asp:Label ID="EmpPosEffTo" runat="Server"  />
                </td>
            </tr>
            <tr>
                <td colspan="2" valign="top" style="padding:0;border-style:none; border-width:0">
                    <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_field_section">
                        <col width="30%" />
                        <col width="70%" />
                        <tr>
                            <td class="pm_field_header" >
                                <%-- Start 0000125, Miranda, 2014-11-19 --%>                                
                                <asp:HyperLink ID="hlCompany" runat="server" NavigateUrl="~/Company_List.aspx" ForeColor="RoyalBlue" >
                                    <asp:Image runat="server" ImageUrl="~/images/magnifier.png" ImageAlign="Right" />
                                </asp:HyperLink>
                                <asp:Label ID="ILabel1" Text="Company" runat="server"  /> :
                                <%-- End 0000125, Miranda, 2014-11-19 --%>
                            </td>
                            <td class="pm_field" >
                                <%-- Start 0000125, Miranda, 2014-11-19 --%>
                                <asp:HyperLink ID="hlCompanyID" runat="server" NavigateUrl="~/Company_View.aspx?CompanyID=" >
                                    <asp:Label ID="CompanyName" runat="Server" />
                                </asp:HyperLink>
                                <%-- End 0000125, Miranda, 2014-11-19 --%>
                            </td>
                        </tr>
                        <asp:Repeater ID="HierarchyLevel" runat="server" OnItemDataBound="HierarchyLevel_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td class="pm_field_header">
                                        <%-- Start 0000125, Miranda, 2014-11-19 --%>

                                        <asp:HyperLink ID="hlHEList" runat="server" NavigateUrl="~/HierarchyElement_List.aspx" ForeColor="RoyalBlue" >
                                            <asp:Image runat="server" ImageUrl="~/images/magnifier.png" ImageAlign="Right" />
                                        </asp:HyperLink>
                                        <%#((HROne.Lib.Entities.EHierarchyLevel )Container.DataItem).HLevelDesc%>
                                        <%-- End 0000125, Miranda, 2014-11-19 --%>
                                    </td>
                                    <td class="pm_field">
                                        <%-- Start 0000125, Miranda, 2014-11-19 --%>
                                        <asp:HyperLink ID="hlHElementID" runat="server" NavigateUrl="~/HierarchyElement_View.aspx?HElementID=" >
                                            <asp:Label ID="HElementID" runat="server" />
                                        </asp:HyperLink>
                                        <%-- End 0000125, Miranda, 2014-11-19 --%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr>
                            <td class="pm_field_header">
                                <%-- Start 0000125, Miranda, 2014-11-19 --%>
                                <asp:HyperLink ID="hlRank" runat="server" NavigateUrl="~/Rank.aspx" ForeColor="RoyalBlue" >
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/images/magnifier.png" ImageAlign="Right" />
                                </asp:HyperLink>
                                <asp:Label ID="ILabel3" Text="Rank" runat="server" /> :
                                <%-- End 0000125, Miranda, 2014-11-19 --%>
                            </td>
                            <td class="pm_field">
                                <asp:Label ID="RankID" runat="Server" /></td>
                        </tr>
                        <tr>
                            <td class="pm_field_header">
                                <%-- Start 0000125, Miranda, 2014-11-19 --%>
                                <asp:HyperLink ID="hlPosition" runat="server" NavigateUrl="~/Position.aspx" ForeColor="RoyalBlue" >
                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/images/magnifier.png" ImageAlign="Right" />
                                </asp:HyperLink>
                                <asp:Label ID="ILabel2" Text="Position" runat="server" /> :
                                <%-- End 0000125, Miranda, 2014-11-19 --%>
                            </td>
                            <td class="pm_field">
                                <asp:Label ID="PositionID" runat="Server" /></td>
                        </tr>
                        <tr>
                            <td class="pm_field_header" >
                                <%-- Start 0000125, Miranda, 2014-11-19 --%>
                                <asp:HyperLink ID="hlEmpType" runat="server" NavigateUrl="~/EmploymentType.aspx" ForeColor="RoyalBlue" >
                                   <asp:Image ID="Image3" runat="server" ImageUrl="~/images/magnifier.png" ImageAlign="Right" />
                                </asp:HyperLink>
                                <asp:Label ID="ILabel7" Text="Employment Type" runat="server" /> :
                                 <%-- End 0000125, Miranda, 2014-11-19 --%>
                            </td>
                            <td class="pm_field"  >
                                <asp:Label ID="EmploymentTypeID" runat="Server" /></td>
                        </tr>
                        <tr>
                            <td class="pm_field_header" >
                                <%-- Start 0000125, Miranda, 2014-11-19 --%>
                                <asp:HyperLink ID="hlStaffType" runat="server" NavigateUrl="~/StaffType.aspx" ForeColor="RoyalBlue" >
                                   <asp:Image ID="Image4" runat="server" ImageUrl="~/images/magnifier.png" ImageAlign="Right" />
                                </asp:HyperLink>
                                <asp:Label ID="ILabel4" Text="Staff Type" runat="server" /> :
                                <%-- End 0000125, Miranda, 2014-11-19 --%>
                            </td>
                            <td class="pm_field"  >
                                <asp:Label ID="StaffTypeID" runat="Server" /></td>
                        </tr>
                        <tr>
                            <td class="pm_field_header">
                                <asp:Label ID="Label3" Text="Remark" runat="server" />:</td>
                            <td class="pm_field">
                                <asp:Label ID="EmpPosRemark" runat="Server" /></td>
                        </tr>
                    </table>
                </td>
                <td colspan="2" valign="top" style="padding:0; border-style:none; border-width:0">
                    <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_field_section">
                        <col width="30%" />
                        <col width="70%" />
                        <tr>
                            <td class="pm_field_header" >
                                <%-- Start 0000125, Miranda, 2014-11-19 --%>
                                <asp:HyperLink ID="hlPayGroup" runat="server" NavigateUrl="~/Payroll_Group_List.aspx" ForeColor="RoyalBlue" >
                                   <asp:Image ID="Image5" runat="server" ImageUrl="~/images/magnifier.png" ImageAlign="Right" />
                                </asp:HyperLink>
                                <asp:Label ID="Label5" Text="Payroll Group" runat="server" /> :
                                <%-- End 0000125, Miranda, 2014-11-19 --%>
                            </td>
                            <td class="pm_field" >
                                <%-- Start 0000125, Miranda, 2014-11-19 --%>
                                <asp:HyperLink ID="hlPayGroupID" runat="server" NavigateUrl="~/Payroll_Group_View.aspx?PayGroupID=" >
                                    <asp:Label ID="PayGroupName" runat="Server" />
                                </asp:HyperLink>
                                <%-- End 0000125, Miranda, 2014-11-19 --%>
                            </td>
                        </tr>
                        <tr>
                            <td class="pm_field_header">
                                <%-- Start 0000125, Miranda, 2014-11-19 --%>
                                <asp:HyperLink ID="hlLeavePlan" runat="server" NavigateUrl="~/LeavePlan_List.aspx" ForeColor="RoyalBlue" >
                                   <asp:Image ID="Image6" runat="server" ImageUrl="~/images/magnifier.png" ImageAlign="Right" />
                                </asp:HyperLink>
                                <asp:Label ID="Label6" Text="Leave Plan" runat="server" /> :
                                <%-- End 0000125, Miranda, 2014-11-19 --%>
                            </td>
                            <td class="pm_field">
                                <%-- Start 0000125, Miranda, 2014-11-19 --%>
                                <asp:HyperLink ID="hlLeavePlanID" runat="server" NavigateUrl="~/LeavePlan_View.aspx?LeavePlanID=" >
                                    <asp:Label ID="LeavePlanDesc" runat="Server" /><br />
                                </asp:HyperLink>
                                <%-- End 0000125, Miranda, 2014-11-19 --%>
                                <asp:CheckBox ID="EmpPosIsLeavePlanResetEffectiveDate"  runat="server" Enabled="false"  /><asp:Label runat="server" Text="Reset Grant Date" />
                            </td>
                        </tr>
                        <tr id="WorkHourPatternRow" runat="server" >
                            <td class="pm_field_header">
                                <%-- Start 0000125, Miranda, 2014-11-19 --%>
                                <asp:HyperLink ID="hlWorkHourPattern" runat="server" NavigateUrl="~/WorkHourPattern_List.aspx" ForeColor="RoyalBlue" >
                                   <asp:Image ID="Image7" runat="server" ImageUrl="~/images/magnifier.png" ImageAlign="Right" />
                                </asp:HyperLink>
                                <asp:Label ID="Label1" Text="Work Hour Pattern" runat="server" /> :
                                <%-- End 0000125, Miranda, 2014-11-19 --%>
                            </td>
                            <td class="pm_field">
                                <%-- Start 0000125, Miranda, 2014-11-19 --%>
                                <asp:HyperLink ID="hlWorkHourPatternID" runat="server" NavigateUrl="~/WorkHourPattern_View.aspx?WorkHourPatternID=" >
                                    <asp:Label ID="WorkHourPatternDesc" runat="Server" />
                                </asp:HyperLink>
                                <%-- End 0000125, Miranda, 2014-11-19 --%>
                            </td>
                        </tr>
                        <tr>
                            <td class="pm_field_header">
                                <%-- Start 0000125, Miranda, 2014-11-19 --%>
                                <asp:HyperLink ID="hlYEBPlan" runat="server" NavigateUrl="~/YEBPlan_List.aspx" ForeColor="RoyalBlue" >
                                   <asp:Image ID="Image8" runat="server" ImageUrl="~/images/magnifier.png" ImageAlign="Right" />
                                </asp:HyperLink>
                                <asp:Label ID="ILabel8" Text="YEB Plan" runat="server" /> :
                                <%-- End 0000125, Miranda, 2014-11-19 --%>
                            </td>
                            <td class="pm_field">
                                <%-- Start 0000125, Miranda, 2014-11-19 --%>
                                <asp:HyperLink ID="hlYEBPlanID" runat="server" NavigateUrl="~/YEBPlan_View.aspx?YEBPlanID=" >
                                    <asp:Label ID="YEBPlanDesc" runat="Server" />
                                </asp:HyperLink>
                                <%-- End 0000125, Miranda, 2014-11-19 --%>
                            </td>
                        </tr>
                        <asp:Panel ID="ESSAuthorizationPanel" runat="server" >
                        <tr>
                            <td class="pm_field_header">
                                <%-- Start 0000125, Miranda, 2014-11-19 --%>
                                <asp:HyperLink ID="hlAuthWorkflowForLeaveApp" runat="server" NavigateUrl="~/ESS_AuthorizationWorkFlow_List.aspx" ForeColor="RoyalBlue" >
                                    <asp:Image ID="Image9" runat="server" ImageUrl="~/images/magnifier.png" ImageAlign="Right" />
                                </asp:HyperLink>
                                <asp:Label ID="ILabel5" Text="Authorization Workflow for Leave Application" runat="server" /> :
                                <%-- End 0000125, Miranda, 2014-11-19 --%>
                            </td>
                            <td class="pm_field">
                                <%-- Start 0000125, Miranda, 2014-11-19 --%>
                                <asp:HyperLink ID="hlAuthWorkFlowIDLeaveApp" runat="server" NavigateUrl="~/ESS_AuthorizationWorkflow_View.aspx?AuthorizationWorkflowID=" >
                                    <asp:Label ID="AuthorizationWorkFlowIDLeaveApp" runat="Server" />
                                </asp:HyperLink>
                                <%-- End 0000125, Miranda, 2014-11-19 --%>
                            </td>
                        </tr>
                        <%-- Start 0000112, Miranda, 2015-01-11 --%>
                        <tr id="lateWaiveRow" runat="server">
                            <td class="pm_field_header" >
                                <asp:HyperLink ID="hlAuthWorkflowForLateWaive" runat="server" NavigateUrl="~/ESS_AuthorizationWorkFlow_List.aspx" ForeColor="RoyalBlue" >
                                    <asp:Image ID="Image13" runat="server" ImageUrl="~/images/magnifier.png" ImageAlign="Right" />
                                </asp:HyperLink>
                                <asp:Label ID="Label4" Text="Authorization Workflow for Late Waive" runat="server" />:
                            </td>
                            <td class="pm_field">
                                <asp:HyperLink ID="hlAuthWorkflowIDLateWaive" runat="server" NavigateUrl="~/ESS_AuthorizationWorkflow_View.aspx?AuthorizationWorkflowID=" >
                                    <asp:Label ID="AuthorizationWorkFlowIDLateWaive" runat="Server" />
                                </asp:HyperLink>
                            </td>
                        </tr>
                        <%-- End 0000112, Miranda, 2015-01-11 --%>
                        <tr>
                            <td class="pm_field_header" >
                                <%-- Start 0000125, Miranda, 2014-11-19 --%>
                                <asp:HyperLink ID="hlAuthWorkflowForEmpInfo" runat="server" NavigateUrl="~/ESS_AuthorizationWorkFlow_List.aspx" ForeColor="RoyalBlue" >
                                    <asp:Image ID="Image10" runat="server" ImageUrl="~/images/magnifier.png" ImageAlign="Right" />
                                </asp:HyperLink>
                                <asp:Label ID="ILabel6" Text="Authorization Workflow for Personal Information Change" runat="server" /> :
                                <%-- End 0000125, Miranda, 2014-11-19 --%>
                            </td>
                            <td class="pm_field">
                                <%-- Start 0000125, Miranda, 2014-11-19 --%>
                                <asp:HyperLink ID="hlAuthWorkflowIDEmpInfo" runat="server" NavigateUrl="~/ESS_AuthorizationWorkflow_View.aspx?AuthorizationWorkflowID=" >
                                    <asp:Label ID="AuthorizationWorkFlowIDEmpInfoModified" runat="Server" />
                                </asp:HyperLink>
                                <%-- End 0000125, Miranda, 2014-11-19 --%>
                            </td>
                        </tr>
                        <%-- Start 0000060, Miranda, 2014-07-13 --%>
                        <tr id="eot_row" runat="server">
                            <td class="pm_field_header" >
                                <%-- Start 0000125, Miranda, 2014-11-19 --%>
                                <asp:HyperLink ID="hlAuthWorkflowForOTClaims" runat="server" NavigateUrl="~/ESS_AuthorizationWorkFlow_List.aspx" ForeColor="RoyalBlue" >
                                    <asp:Image ID="Image11" runat="server" ImageUrl="~/images/magnifier.png" ImageAlign="Right" />
                                </asp:HyperLink>
                                <asp:Label ID="Label2" Text="Authorization Workflow CL Requisitions" runat="server" /> :
                                <%-- End 0000125, Miranda, 2014-11-19 --%>
                            </td>
                            <td class="pm_field">
                                <%-- Start 0000125, Miranda, 2014-11-19 --%>
                                <asp:HyperLink ID="hlAuthWorkflowIDOTClaims" runat="server" NavigateUrl="~/ESS_AuthorizationWorkflow_View.aspx?AuthorizationWorkflowID=" >
                                    <asp:Label ID="AuthorizationWorkFlowIDOTClaims" runat="Server" />
                                </asp:HyperLink>
                                <%-- End 0000125, Miranda, 2014-11-19 --%>
                            </td>
                        </tr>
                        <%-- End 0000060, Miranda, 2014-07-13 --%>
                        <%-- Start 0000112, Miranda, 2014-12-10 --%>
                        <%-- Start 0000112, Miranda, 2015-01-11 --%>
                        <%--<tr id="lateWaiveRow" runat="server">
                            <td class="pm_field_header" >
                                <asp:HyperLink ID="hlAuthWorkflowForLateWaive" runat="server" NavigateUrl="~/ESS_AuthorizationWorkFlow_List.aspx" ForeColor="RoyalBlue" >
                                    <asp:Image ID="Image13" runat="server" ImageUrl="~/images/magnifier.png" ImageAlign="Right" />
                                </asp:HyperLink>
                                <asp:Label ID="Label4" Text="Authorization Workflow for Late Waive" runat="server" />:
                            </td>
                            <td class="pm_field">
                                <asp:HyperLink ID="hlAuthWorkflowIDLateWaive" runat="server" NavigateUrl="~/ESS_AuthorizationWorkflow_View.aspx?AuthorizationWorkflowID=" >
                                    <asp:Label ID="AuthorizationWorkFlowIDLateWaive" runat="Server" />
                                </asp:HyperLink>
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
                                <asp:Label ID="EmpPosDefaultRosterCodeID" runat="Server" /></td>
                        </tr>
--%>
                        <tr>
                             <td class="pm_field_header">
                                <%-- Start 0000125, Miranda, 2014-11-19 --%>
                                <asp:HyperLink ID="hlAttendancePlan" runat="server" NavigateUrl="~/Attendance_Plan_List.aspx" ForeColor="RoyalBlue" >
                                    <asp:Image ID="Image12" runat="server" ImageUrl="~/images/magnifier.png" ImageAlign="Right" />
                                </asp:HyperLink>
                                <asp:Label ID="ILabel9" Text="Attendance Plan" runat="server" /> :
                                <%-- End 0000125, Miranda, 2014-11-19 --%>
                            </td>
                            <td class="pm_field">
                                <%-- Start 0000125, Miranda, 2014-11-19 --%>
                                <asp:HyperLink ID="hlAttendancePlanID" runat="server" NavigateUrl="~/Attendance_Plan_View.aspx?AttendancePlanID=" >
                                    <asp:Label ID="AttendancePlanDesc" runat="Server" />
                                </asp:HyperLink>
                                <%-- End 0000125, Miranda, 2014-11-19 --%>
                            </td>
                        </tr>
<%--                        <tr>
                            <td class="pm_field_header">
                                <asp:Label ID="Label2" Text="Roster Table Group" runat="server" />
                                :</td>
                            <td class="pm_field">
                                <asp:Label ID="RosterTableGroupID" runat="Server" />
                                <a style="font-style:italic ;"><asp:Label ID="EmpPosIsRosterTableGroupSupervisor" runat="Server" Text="(Supervisor)" /></a>
                            </td>
                        </tr>
--%>                        </asp:Panel>
                    </table>
                </td>
            </tr>
        </table>
</asp:Content> 