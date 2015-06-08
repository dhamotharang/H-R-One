<%@ page language="C#" masterpagefile="~/MainMasterPage.master" autoeventwireup="true" inherits="Attendance_RosterTable_View, HROneWeb.deploy" viewStateEncryptionMode="Always" %>

<%@ Register Src="~/controls/Attendance_RosterTable_Edit_AdvancedTemplate.ascx" TagName="Attendance_RosterTable_Edit_AdvancedTemplate" TagPrefix="uc1" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" Runat="Server">
	<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
		<tr>
			<td>
				<asp:Label ID="Label5" runat="server" Text="Roster Table Overview" />
			</td>
		</tr>
	</table>

       <script type="text/javascript">    
//       $telerik.$.popupDialog.prototype._positionForm = function() {
//            // This will be called each time the form needs to be repositioned
//            var $ = $telerik.$;
//            $(this._targetElement).css({ top: "200px", left: "0px" });
//        }
        //<![CDATA[

            // Dictionary containing the advanced template client object
         // for a given RadScheduler instance (the control ID is used as key).
         var schedulerTemplates = {};
        
         function schedulerFormCreated(scheduler, eventArgs) {
         // Create a client-side object only for the advanced templates
         var mode = eventArgs.get_mode();
         if (mode == Telerik.Web.UI.SchedulerFormMode.AdvancedInsert ||
                    mode == Telerik.Web.UI.SchedulerFormMode.AdvancedEdit) {
         // Initialize the client-side object for the advanced form
         var formElement = eventArgs.get_formElement();    
         var templateKey = scheduler.get_id() + "_" + mode;
         var advancedTemplate = schedulerTemplates[templateKey];
         if (!advancedTemplate)
         {
         // Initialize the template for this RadScheduler instance
         // and cache it in the schedulerTemplates dictionary
     var schedulerElement = scheduler.get_element();
     var isModal = scheduler.get_advancedFormSettings().modal;
     advancedTemplate = new window.SchedulerAdvancedTemplate(schedulerElement, formElement, isModal);
     advancedTemplate.initialize();

     schedulerTemplates[templateKey] = advancedTemplate;

                        // Remove the template object from the dictionary on dispose.
     scheduler.add_disposing(function() {
                            schedulerTemplates[templateKey] = null;
     });
         }

         // Are we using Web Service data binding?
         if (!scheduler.get_webServiceSettings().get_isEmpty()) {
         // Populate the form with the appointment data
         var apt = eventArgs.get_appointment();
         var isInsert = mode == Telerik.Web.UI.SchedulerFormMode.AdvancedInsert;
         advancedTemplate.populate(apt, isInsert);
         }
         }
         }
            
        //]]>
        </script>
<%--    <telerik:RadScriptBlock runat="server" ID="RadScriptBlock1">
        <script type="text/javascript">
            function openForm() {
                var dock = $find("<%= RadDock1.ClientID %>");
                // Center the RadDock on the screen
                var viewPort = $telerik.getViewPortSize();
                var xPos = Math.round((viewPort.width - parseInt(dock.get_width())) / 2);
                var yPos = Math.round((viewPort.height - parseInt(dock.get_height())) / 2);
                $telerik.setLocation(dock.get_element(), { x: xPos, y: yPos });

                dock.set_closed(false);

                var descriptionTextBox = $get('<%= RosterClientID.ClientID %>');
                descriptionTextBox.focus();

                Sys.Application.remove_load(openForm);
            }

            function hideForm() {
                var dock = $find("<%= RadDock1.ClientID %>");
                dock.set_closed(true);

                return true;
            }

            function dockMoved(sender, args) {
                //Return RadDock to his original HTML parent so it gets updated via ajax
                $get("<%= DockPanel.ClientID %>").appendChild(sender.get_element());
            }
                
        </script>
        </telerik:RadScriptBlock>
--%>
                 <table width="100%" >
			        <tr>
			            <td valign="top" >
			                <table>
			                    <tr>
			                        <td>
			                            <table  class="pm_list_section" cellspacing="0" cellpadding="1" width="200">
                                            <colgroup>
			                                <col width="10" />
			                                <col width="190" />
                                            </colgroup>
			                                <tr>
                                                <td class="pm_list_header" align="center" >
                                                    <asp:CheckBox ID="chkRosterClientSiteCheckAll" runat="server" AutoPostBack="True" />
                                                </td>
			                                    <td class="pm_list_header"  >
			                                        <asp:Label ID="Label1" runat="server" Text="Roster Client Site List" />
			                                    </td>
			                                </tr>
                                            <asp:Repeater ID="rosterClientSiteRepeater" runat="server" OnItemDataBound="rosterClientSiteRepeater_ItemDataBound">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td class="pm_list" align="center" >
                                                            <asp:CheckBox ID="ItemSelect" runat="server" OnCheckedChanged="RosterClientSite_OnCheckedChanged" AutoPostBack="true" />
                                                        </td>
                                                        <td class="pm_list" >
                                                            <%#RosterClientSiteSBinding.getValue(Container.DataItem, "RosterClientSiteCode")%> -
                                                            <%#RosterClientSiteSBinding.getValue(Container.DataItem, "RosterClientSitePropertyName")%>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater> 
			                            </table>
			                        </td>
			                    </tr>
			                    <tr>
			                        <td>
			                            <table  class="pm_list_section" cellspacing="0" cellpadding="1" width="200" >
                                            <colgroup>
			                                <col width="10" />
			                                <col width="190" />
                                            </colgroup>
			                                <tr>
			                                    <td class="pm_list_header" colspan="2" >
			                                        <asp:Label ID="Label2" runat="server" Text="Employee Filter" />
			                                    </td>
			                                </tr>
                                            <tr>
                                                <td class="pm_list" align="center" >
                                                    <asp:CheckBox ID="chkEmployeeSameDefaultSite" runat="server" Checked="True" OnCheckedChanged="RosterClientSite_OnCheckedChanged" AutoPostBack="true"/>
                                                </td>
                                                <td class="pm_list" >
                                                    <asp:Label ID="Label4" runat="server" Text="Show employees with same default site under Roster Table Group" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="pm_list" align="center" >
                                                    <asp:CheckBox ID="chkRosterCodeWithoutSiteMapping" runat="server" Checked="True"  OnCheckedChanged="RosterClientSite_OnCheckedChanged" AutoPostBack="true" />
                                                </td>
                                                <td class="pm_list">
                                                    <asp:Label ID="Label3" runat="server" Text="Show roster code without site mapping" />
                                                </td>
                                            </tr>
			                            </table>
			                        </td>
			                    </tr>
			                </table>
			            </td> 
			            <td>
<%--				            <asp:Panel runat="server" ID="DockPanel">
					            <telerik:RadDock runat="server" ID="RadDock1" Width="500px" Height="330px" 
						            Closed="true" Style="z-index: 2000;" Title="Edit Roster" OnClientDockPositionChanged="dockMoved" >
						            <Commands>
							            <telerik:DockCloseCommand />
						            </Commands>
						            <ContentTemplate>
							            <div class="editForm">
								            <div class="header">
									            <asp:Label runat="server" ID="StatusLabel"></asp:Label>
								            </div>
								            <div class="content">
                                                <input type="hidden" id="RosterTableID" runat="server" name="ID" />
                                                <input type="hidden" id="EmpID" runat="server" name="EmpID" />
								                <table>
								                    <tr>
								                        <td><asp:Label ID="Label2" runat="server" Text="Employee Name" />:</td>
								                        <td><asp:Label ID="EmpName" runat="server" /></td>
								                    </tr>
								                    <tr>
								                        <td><asp:Label ID="Label3" runat="server" Text="Date" />:</td>
								                        <td><asp:Label ID="RosterTableDate" runat="server" /></td>
								                    </tr>
								                    <tr>
								                        <td><asp:Label ID="Label4" runat="server" Text="Client" />:</td>
								                        <td><asp:DropDownList ID="RosterClientID" runat="server" OnSelectedIndexChanged="RosterClientID_SelectedIndexChanged" AutoPostBack="true"  /></td>
								                    </tr>
								                    <tr>
								                        <td><asp:Label ID="Label5" runat="server" Text="Site" />:</td>
								                        <td><asp:DropDownList ID="RosterClientSiteID" runat="server"  OnSelectedIndexChanged="RosterClientSiteID_SelectedIndexChanged"  AutoPostBack="true" /></td>
								                    </tr>
								                    <tr>
								                        <td><asp:Label ID="Label6" runat="server" Text="Roster Code" />:</td>
								                        <td><asp:DropDownList ID="RosterCodeID" runat="server" /></td>
								                    </tr>
								                    <tr>
								                        <td><asp:Label ID="Label7" runat="server" Text="Time" />:</td>
								                        <td><asp:TextBox ID="RosterTimeFrom" runat="server" /> - <asp:TextBox ID="RosterTimeTo" runat="server" /> </td>
								                    </tr>
								                </table>

								            </div>
								            <div class="footer">
									            <asp:Button runat="server" ID="SubmitButton" Text="Update" OnClick="SubmitButton_Click"
										            />
									            <button onclick="hideForm();" type="button" style="margin-right: 20px;">
										            Cancel</button>
								            </div>
							            </div>
						            </ContentTemplate>
					            </telerik:RadDock>
				            </asp:Panel>
--%>

                        <asp:UpdatePanel ID="SchedulerUpdatePanel" runat="server" UpdateMode="Conditional"  >
                        <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="rosterClientSiteRepeater" />
                        <asp:AsyncPostBackTrigger ControlID="chkRosterClientSiteCheckAll" EventName="CheckedChanged" />
                        <asp:AsyncPostBackTrigger ControlID="chkEmployeeSameDefaultSite" EventName="CheckedChanged" />
                        <asp:AsyncPostBackTrigger ControlID="chkRosterCodeWithoutSiteMapping" EventName="CheckedChanged" />
                        </Triggers>
                        <ContentTemplate>

                            <telerik:RadScheduler ID="RadScheduler1" runat="server" DataSourceID="appointmentDataSource"
                             DayStartTime="00:00:00" Skin="Sitefinity" Height="" SelectedView="MonthView" 
	                            DayEndTime="1.00:00:00"  AppointmentStyleMode="Default" 
                                DataKeyField="ID" DataSubjectField="Subject" DataStartField="StartTime" DataEndField="EndTime"
	                            DataRecurrenceField="RecurrenceInfo" DataRecurrenceParentKeyField="RecurrenceParentId"
	                            OnAppointmentClick="RadScheduler1_AppointmentClick" OnAppointmentCommand="RadScheduler1_AppointmentCommand" OnAppointmentContextMenuItemClicked="RadScheduler1_AppointmentContextMenuItemClicked"
                                OnAppointmentDataBound="RadScheduler1_AppointmentDataBound" OnAppointmentContextMenuItemClicking="RadScheduler1_AppointmentContextMenuItemClicking" 
                                AllowInsert="False" ShowFullTime="True" ShowFooter="False" MinutesPerRow="15" OnTimeSlotCreated="RadScheduler1_TimeSlotCreated" >
	                            <AdvancedForm Modal="True"  />
	                            <MonthView VisibleAppointmentsPerDay="5" />
	                            <WeekView EnableExactTimeRendering="True" />
	                            <DayView EnableExactTimeRendering="True" />
 	                            <TimelineView NumberOfSlots="24" SlotDuration="01:00:00" ColumnHeaderDateFormat="hh:mm" />
	                            <AppointmentContextMenuSettings EnableDefault="True" />
                                <AppointmentContextMenus>
                                    <telerik:RadSchedulerContextMenu runat="server" ID="SchedulerAppointmentContextMenu" >
                                        
                                        <Items>
                                            <telerik:RadMenuItem Text="Open" Value="CommandEdit" runat="server" />
                                        </Items>
                                    </telerik:RadSchedulerContextMenu>
                                </AppointmentContextMenus>
                                <AppointmentTemplate>
                                     <div class="rsAptSubject">
                                         <%# Eval("Subject") %>
                                     </div>
                                     <%# Eval("Description") %>
                                </AppointmentTemplate>
                                <AdvancedEditTemplate>
                                        <uc1:Attendance_RosterTable_Edit_AdvancedTemplate ID="Attendance_RosterTable_Edit_AdvancedTemplate1" runat="server" 
                                            />

                                </AdvancedEditTemplate>
                                <AdvancedInsertTemplate >
                                        <uc1:Attendance_RosterTable_Edit_AdvancedTemplate ID="Attendance_RosterTable_Edit_AdvancedTemplate1" runat="server"  />
                                </AdvancedInsertTemplate>
                            </telerik:RadScheduler>
                        </ContentTemplate>
                        </asp:UpdatePanel>
			            </td> 
                    </tr> 
			    </table>
    <asp:ObjectDataSource ID="appointmentDataSource" runat="server" 
        DataObjectTypeName="HROne.Lib.Attendance.DataSource.RosterTableEvent" 
        TypeName="HROne.Lib.Attendance.DataSource.RosterTableEventDataSource" 
        DeleteMethod="Delete" SelectMethod="Select" InsertMethod="Insert" UpdateMethod="Update" 
        OnObjectCreated ="appointmentDataSource_ObjectCreated"
        OnSelecting="appointmentDataSource_Selecting"
        >
        <SelectParameters >
        </SelectParameters>
    </asp:ObjectDataSource>
<%--    <asp:ObjectDataSource ID="resourceDataSource" runat="server" DataObjectTypeName="SchedulerResource" TypeName="SchedulerResourceDataSource" SelectMethod="SelectSchedulerResource" InsertMethod="InsertSchedulerEvent" >
    </asp:ObjectDataSource>
--%>
</asp:Content>

