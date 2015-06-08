<%@ page language="C#" masterpagefile="~/MainMasterPage.master" autoeventwireup="true" inherits="ESS_RosterTable_View, HROneESS.deploy" %>
<%@ Register Src="~/controls/Attendance_RosterTable_Edit_AdvancedTemplate.ascx" TagName="Attendance_RosterTable_Edit_AdvancedTemplate" TagPrefix="uc1" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="bannerContentPlaceHolder" Runat="Server">
    <table width="898px" border="0" cellpadding="0" cellspacing="0">
      <tr>
        <td>
            <a href="ESS_Home.aspx">
                <img src="images/banner09_01.jpg" alt="" style="border-width: 0px; display : block" />
            </a>
        </td>
        <td valign="bottom" >
            <img src="images/banner09_02.jpg" alt="" style="border-width: 0px; display : block" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <img src="images/banner09_03.jpg" alt="" style="border-width: 0px; display : block" />
        </td>
      </tr>
    </table>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContentPlaceHolder" Runat="Server">
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
        
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="pm_field_section">
        <col width="30%" />
        <col width="30%" />
        <col width="40%" />
        <tr>
            <td class="pm_field_header">
                <asp:Label ID="Label1" runat="server" Text="Roster Table Group" />:
            </td>
            <td class="pm_field">
                <asp:DropDownList ID="RosterTableGroupID" runat="server" AutoPostBack="true" OnSelectedIndexChanged="RosterTableGroupID_SelectedIndexChanged" /> 
            </td>
			<!--
            <td class="pm_field">
                <asp:CheckBox Text="Leave" Checked="true" runat="server" /> &nbsp; &nbsp; &nbsp; &nbsp
                <asp:CheckBox Text="Roster" Checked="true" runat="server" />
            </td>
			-->
        <%-- Start 0000179, KuangWei, 2015-03-20 --%>
            <td class="pm_field" colspan="3" >
                <asp:CheckBox ID="LeaveChecking" runat="Server" Checked="true" AutoPostBack="true" OnCheckedChanged="RosterTableCheckedChanged" />
                <asp:Label runat="server" Text="Leave" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="RosterChecking" runat="Server" Checked="true" AutoPostBack="true" OnCheckedChanged="RosterTableCheckedChanged" />
                <asp:Label runat="server" Text="Roster" />                
            </td>                         
        <%-- End 0000179, KuangWei, 2015-03-20 --%>
        </tr>
    </table>
    <asp:UpdatePanel ID="SchedulerUpdatePanel" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="RosterTableGroupID" EventName="SelectedIndexChanged" />
        </Triggers>
        <ContentTemplate>

            <telerik:RadScheduler ID="RadScheduler1" runat="server" DataSourceID="appointmentDataSource"
             DayStartTime="00:00:00" Skin="Sitefinity" Height="" SelectedView="MonthView" 
                DayEndTime="1.00:00:00"  AppointmentStyleMode="Default" 
                DataKeyField="ID" DataSubjectField="Subject" DataStartField="StartTime" DataEndField="EndTime"
                DataRecurrenceField="RecurrenceInfo" DataRecurrenceParentKeyField="RecurrenceParentId"
                OnAppointmentClick="RadScheduler1_AppointmentClick" OnAppointmentCommand="RadScheduler1_AppointmentCommand" OnAppointmentContextMenuItemClicked="RadScheduler1_AppointmentContextMenuItemClicked"
                OnAppointmentDataBound="RadScheduler1_AppointmentDataBound" OnAppointmentContextMenuItemClicking="RadScheduler1_AppointmentContextMenuItemClicking" 
                AllowInsert="False" ShowFullTime="True" ShowFooter="False" MinutesPerRow="15" OnTimeSlotCreated="RadScheduler1_TimeSlotCreated">
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
                     <div class="rsAptSubject" >
                         <%# Eval("Subject")%>
                     </div>
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
    <asp:ObjectDataSource ID="appointmentDataSource" runat="server" 
        DataObjectTypeName="HROne.Lib.Attendance.DataSource.RosterTableEvent" 
        TypeName="HROne.Lib.Attendance.DataSource.RosterTableEventDataSource" 
        DeleteMethod="Delete" SelectMethod="Select" InsertMethod="Insert" UpdateMethod="Update" 
        OnObjectCreated ="appointmentDataSource_ObjectCreated"
        OnSelecting="appointmentDataSource_Selecting" >
        <SelectParameters >
        </SelectParameters>
    </asp:ObjectDataSource>

</asp:Content>

