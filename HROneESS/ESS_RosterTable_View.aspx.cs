using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using HROne.Lib.Entities;
using HROne.DataAccess;
using Telerik.Web.UI;

public partial class ESS_RosterTable_View : HROneWebPage
{
    //protected object EditedAppointmentID
    //{
    //    get { return ViewState["EditedAppointmentID"]; }
    //    set { ViewState["EditedAppointmentID"] = value; }
    //}


    //protected SchedulerDataSource dataSource;

    protected SearchBinding sbinding;
    int m_ESSUserID = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session))
            return;


        EESSUser user = WebUtils.GetCurUser(Session);
        if (user != null)
            m_ESSUserID = user.EmpID;

        DateTime currentDate= AppUtils.ServerDateTime().Date;
        DBFilter empRosterTableGroupFilter = new DBFilter();
        empRosterTableGroupFilter.add(new Match("ertg.EmpID", m_ESSUserID));
        OR orEmpRosterTableGroupFromTerm = new OR();
        orEmpRosterTableGroupFromTerm.add(new Match("ertg.EmpRosterTableGroupEffFr", "<=", currentDate));
        orEmpRosterTableGroupFromTerm.add(new NullTerm("ertg.EmpRosterTableGroupEffFr"));
        empRosterTableGroupFilter.add(orEmpRosterTableGroupFromTerm);
        OR orEmpRosterTableGroupToTerm = new OR();
        orEmpRosterTableGroupToTerm.add(new Match("ertg.EmpRosterTableGroupEffTo", ">=", currentDate));
        orEmpRosterTableGroupToTerm.add(new NullTerm("ertg.EmpRosterTableGroupEffTo"));
        empRosterTableGroupFilter.add(orEmpRosterTableGroupToTerm);

        DBFilter RosterTableGroupIDFilter = new DBFilter();
        RosterTableGroupIDFilter.add(new IN("RosterTableGroupID", "SELECT ertg.RosterTableGroupID FROM " + EEmpRosterTableGroup.db.dbclass.tableName + " ertg", empRosterTableGroupFilter));
        sbinding = new SearchBinding(dbConn, ERosterClientSite.db);
        sbinding.add(new DropDownVLSearchBinder(RosterTableGroupID, "RosterTableGroupID", ERosterTableGroup.VLRosterTableGroup).setFilter(RosterTableGroupIDFilter));
        sbinding.init(DecryptedRequest, null);

        RosterTableGroupID.Items[0].Text = "All";


        HtmlLink cssLink = new HtmlLink();
        cssLink.Href = "~/CSS/RadScheduler.css";
        cssLink.Attributes.Add("rel", "stylesheet");
        cssLink.Attributes.Add("type", "text/css");
        Header.Controls.Add(cssLink);

    }


    //protected void MonthViewRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    //{
    //    Label DayNum = (Label)e.Item.FindControl("DayNum");
    //    DateTime date = (DateTime)e.Item.DataItem;
    //    Repeater RosterTableRepeater = (Repeater)e.Item.FindControl("RosterTableRepeater");

    //    DayNum.Text = date.Day.ToString();
    //    if (date.Month != month)
    //    {
    //        DayNum.ForeColor = System.Drawing.Color.LightGray;
    //    }

    //    DBFilter filter = new DBFilter();
    //    filter.add(new Match("RosterTableDate", date));
    //    ArrayList rosterTableList = ERosterTable.db.select(filter);
    //    RosterTableRepeater.DataSource = rosterTableList;
    //    RosterTableRepeater.DataBind();


    //}
    //protected void RosterTableRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    //{
    //    Label RosterTableDetail = (Label)e.Item.FindControl("RosterTableDetail");
    //    ERosterTable rosterTable = (ERosterTable)e.Item.DataItem;

    //    EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
    //    empInfo.EmpID = rosterTable.EmpID;

    //    ERosterCode rosterCode = new ERosterCode();
    //    rosterCode.RosterCodeID = rosterTable.RosterCodeID;

    //    if (EEmpPersonalInfo.db.select(empInfo) && ERosterCode.db.select(rosterCode))
    //    {
    //        RosterTableDetail.Text = empInfo.EmpNo + "-" + rosterCode.RosterCode;
    //    }

    //}

    protected void appointmentDataSource_ObjectCreated(object sender, ObjectDataSourceEventArgs e)
    {
        if (e.ObjectInstance != null)
        {
            ((HROne.Lib.Attendance.DataSource.RosterTableEventDataSource)e.ObjectInstance).dbConn = dbConn;
            //SqlConnection conn = new SqlConnection();
            //conn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnectionStringNameInWeb.Config"].ConnectionString;
            //e.ObjectInstance.GetType().GetProperty("Connection").SetValue(e.ObjectInstance, conn, null);
        }
    }
    protected void appointmentDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["DateFrom"] = RadScheduler1.VisibleRangeStart;
        e.InputParameters["DateTo"] = RadScheduler1.VisibleRangeEnd;


        e.InputParameters["ESSUserID"] = m_ESSUserID;
        e.InputParameters["DisplayOvernightToNextDay"] = !RadScheduler1.SelectedView.Equals(Telerik.Web.UI.SchedulerViewType.MonthView);
        if (string.IsNullOrEmpty(RosterTableGroupID.SelectedValue))
            e.InputParameters["RosterTableGroupID"] = 0;
        else
            e.InputParameters["RosterTableGroupID"] = int.Parse(RosterTableGroupID.SelectedValue);

        if (RadScheduler1.SelectedView == SchedulerViewType.TimelineView)
            RadScheduler1.Height = Unit.Parse("600px");
        else
            RadScheduler1.Height = Unit.Parse("");

        // Start 0000179, KuangWei, 2015-03-20
        e.InputParameters["LeaveChecking"] = LeaveChecking.Checked;
        e.InputParameters["RosterChecking"] = RosterChecking.Checked;
        // End 0000179, KuangWei, 2015-03-20
    }

    protected void RadScheduler1_AppointmentCommand(object sender, Telerik.Web.UI.AppointmentCommandEventArgs e)
    {
        RadScheduler1.Rebind();
    }
    protected void RadScheduler1_AppointmentClick(object sender, Telerik.Web.UI.SchedulerEventArgs e)
    {
        Console.Write(e.ToString());
    }
    protected void RadScheduler1_AppointmentContextMenuItemClicked(object sender, Telerik.Web.UI.AppointmentContextMenuItemClickedEventArgs e)
    {
        //Console.Write(e.ToString());

        //if (e.MenuItem.Value.Equals("CommandNew", StringComparison.CurrentCultureIgnoreCase))
        //{
        //        EditedAppointmentID = e.Appointment.ID;

        //        ScriptManager.RegisterStartupScript(Page, GetType(), "formScript", "Sys.Application.add_load(openForm);", true);
        //        PopulateEditForm(e.Appointment);

        //}

    }

    protected void RadScheduler1_FormCreating(object sender, SchedulerFormCreatingEventArgs e)
    {
        //if (e.Mode == SchedulerFormMode.Insert || e.Mode == SchedulerFormMode.Edit)
        //{
        //    EditedAppointmentID = e.Appointment.ID;
        //    e.Cancel = true;

        //    ScriptManager.RegisterStartupScript(Page, GetType(), "formScript", "Sys.Application.add_load(openForm);", true);
        //    PopulateEditForm(e.Appointment);
        //}
    }

    //protected void PopulateEditForm(Appointment editedAppointment)
    //{
    //    WebFormUtils.loadValues(RosterClientID, ERosterClient.VLRosterClient, new DBFilter());

    //    if (editedAppointment.ID != null)
    //    {
    //        string[] id_Array = editedAppointment.ID.ToString().Split(new char[] { '_' });

    //        if (id_Array.GetLength(0) == 2)
    //        {
    //            if (id_Array[0].Equals("RosterTable"))
    //            {
    //                ERosterTable rosterTable = new ERosterTable();
    //                rosterTable.RosterTableID = int.Parse(id_Array[1]);
    //                if (ERosterTable.db.select(rosterTable))
    //                {
    //                    Binding ebinding = new Binding(ERosterTable.db);
    //                    ebinding.add(RosterTableID);
    //                    ebinding.add(EmpID);
    //                    ebinding.add(RosterTableDate);
    //                    ebinding.add(new DropDownVLBinder(ERosterTable.db, RosterCodeID, ERosterCode.VLRosterCode));
    //                    ebinding.init(Request, Session);
    //                    Hashtable values = new Hashtable();
    //                    ERosterTable.db.populate(rosterTable, values);
    //                    ebinding.toControl(values);

    //                    EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
    //                    empInfo.EmpID = rosterTable.EmpID;
    //                    if (EEmpPersonalInfo.db.select(empInfo))
    //                        EmpName.Text = empInfo.EmpEngFullNameWithAlias;
    //                    else
    //                        EmpName.Text = string.Empty;

    //                }

    //                ERosterCode rosterCode = new ERosterCode();
    //                rosterCode.RosterCodeID = rosterTable.RosterCodeID;
    //                ERosterCode.db.select(rosterCode);
    //                if (rosterCode.RosterClientID > 0)
    //                    RosterClientID.SelectedValue = rosterCode.RosterClientID.ToString();

    //                RefreshClientSite(rosterCode.RosterClientID);

    //                if (rosterCode.RosterClientSiteID > 0)
    //                    RosterClientSiteID.SelectedValue = rosterCode.RosterClientSiteID.ToString();

    //                RefreshRosterCode(rosterCode.RosterClientSiteID);
    //            }
    //        }
    //    }
    //}

    //protected void RefreshClientSite(int RosterClientID)
    //{
    //    string selected = RosterClientSiteID.SelectedValue;

    //    DBFilter rosterClientSiteFilter = new DBFilter();
    //    rosterClientSiteFilter.add(new Match("RosterClientID", RosterClientID));
    //    WebFormUtils.loadValues(RosterClientSiteID, ERosterClientSite.VLRosterClientSite, rosterClientSiteFilter, null, (string)selected, (string)"combobox.notselected");

    //}

    //protected void RefreshRosterCode(int RosterClientSiteID)
    //{
    //    string selected = RosterCodeID.SelectedValue;

    //    DBFilter rosterCodeFilter = new DBFilter();
    //    rosterCodeFilter.add(new Match("RosterClientSiteID", RosterClientSiteID));
    //    WebFormUtils.loadValues(RosterCodeID, ERosterCode.VLRosterCode, rosterCodeFilter, null, (string)selected, (string)"combobox.notselected");


    //}

    //protected void SubmitButton_Click(object sender, EventArgs e)
    //{
    //    ERosterTable rosterTable = new ERosterTable();

    //    Binding ebinding = new Binding(ERosterTable.db);
    //    ebinding.add(RosterTableID);
    //    ebinding.add(RosterTableDate);
    //    ebinding.add(EmpID);
    //    ebinding.add(new DropDownVLBinder(ERosterTable.db, RosterCodeID, ERosterCode.VLRosterCode));
    //    Hashtable values = new Hashtable();
    //    ebinding.toValues(values);
    //    ERosterTable.db.parse(values, rosterTable);

    //    ERosterTable.db.update(rosterTable);

    //    //// Create resource based on the selected user
    //    ////Resource user = new Resource("User", int.Parse(UserDropDown.SelectedValue), UserDropDown.SelectedItem.Text);
    //    //DateTime start = RadScheduler1.DisplayToUtc(StartTime.SelectedDate.Value);
    //    //DateTime end = RadScheduler1.DisplayToUtc(EndTime.SelectedDate.Value);

    //    //if (EditedAppointmentID == null)
    //    //{
    //    //    // Insert
    //    //    Appointment appointment = new Appointment(null, start, end, DescriptionText.Text);
    //    //    //appointment.Resources.Add(user);

    //    //    RadScheduler1.InsertAppointment(appointment);
    //    //}
    //    //else
    //    //{
    //    //    Appointment appointment = RadScheduler1.Appointments.FindByID(EditedAppointmentID);
    //    //    Appointment appointmentToUpdate = RadScheduler1.PrepareToEdit(appointment, RadScheduler1.EditingRecurringSeries);

    //    //    appointmentToUpdate.Subject = DescriptionText.Text;
    //    //    appointmentToUpdate.Start = start;
    //    //    appointmentToUpdate.End = end;

    //    //    // Remove the existing user resource, if any
    //    //    Resource existingUser = appointmentToUpdate.Resources.GetResourceByType("User");
    //    //    if (existingUser != null)
    //    //    {
    //    //        appointmentToUpdate.Resources.Remove(existingUser);
    //    //    }
    //    //    //appointmentToUpdate.Resources.Add(user);

    //    //    RadScheduler1.UpdateAppointment(appointmentToUpdate);
    //    //}
    //    RadScheduler1.Rebind();
    //    RadDock1.Closed = true;
    //}



    //protected void RosterClientID_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    string selected = RosterClientID.SelectedValue;
    //    int intRosterClientID;

    //    if (int.TryParse(selected, out intRosterClientID))
    //        RefreshClientSite(intRosterClientID);
    //    else
    //        RefreshClientSite(0);

    //    selected = RosterClientSiteID.SelectedValue;
    //    int intRosterClientSiteID;

    //    if (int.TryParse(selected, out intRosterClientSiteID))
    //        RefreshRosterCode(intRosterClientSiteID);
    //    else
    //        RefreshRosterCode(0);

    //}
    //protected void RosterClientSiteID_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    string selected = RosterClientSiteID.SelectedValue;
    //    int intRosterClientSiteID;

    //    if (int.TryParse(selected, out intRosterClientSiteID))
    //        RefreshRosterCode(intRosterClientSiteID);
    //    else
    //        RefreshRosterCode(0);

    //}
    //protected void RadDock1_DockPositionChanged(object sender, DockPositionChangedEventArgs e)
    //{
    //    Console.Write(e.ToString());
    //}


    //protected void RadCalendar1_SelectionChanged(object sender, Telerik.Web.UI.Calendar.SelectedDatesEventArgs e)
    //{
    //    if (!RadCalendar1.SelectedDate.Equals(DateTime.MinValue))
    //        RadScheduler1.SelectedDate = RadCalendar1.SelectedDate;
    //}
    //protected void RadScheduler1_NavigationComplete(object sender, SchedulerNavigationCompleteEventArgs e)
    //{
    //    RadCalendar1.SelectedDate = RadScheduler1.SelectedDate;
    //    RadCalendar1.FocusedDate = RadCalendar1.SelectedDate;
    //}
    protected void RadScheduler1_AppointmentDataBound(object sender, SchedulerEventArgs e)
    {

        //e.Appointment.ContextMenuID=
        e.Appointment.AllowDelete = false;


        if (e.Appointment != null)
        {
            HROne.Lib.Attendance.DataSource.RosterTableEvent rosterTableEvent = (HROne.Lib.Attendance.DataSource.RosterTableEvent)e.Appointment.DataItem;
            e.Appointment.ToolTip = rosterTableEvent.ToolTip;
            e.Appointment.AllowEdit = false;

            if (rosterTableEvent.Id.StartsWith(HROne.Lib.Attendance.DataSource.RosterTableEvent.PUBLICHOLIDAY_PREFIX)
                || rosterTableEvent.Id.StartsWith(HROne.Lib.Attendance.DataSource.RosterTableEvent.STATUTORYHOLIDAY_PREFIX))
            {
                e.Appointment.ForeColor = System.Drawing.Color.Red;
            }

            if (rosterTableEvent.IsCancel)
            {
                e.Appointment.ForeColor = System.Drawing.Color.Green;
            }

            //EEmpPositionInfo userEmpPos = AppUtils.GetLastPositionInfo(dbConn, e.Appointment.Start.Date, m_ESSUserID);
            object dbObjectItem = HROne.Lib.Attendance.DataSource.RosterTableEvent.IDToDBObject(dbConn, e.Appointment.ID);

            if (dbObjectItem is ERosterTable)
            {
                ERosterTable rosterTable = (ERosterTable)dbObjectItem;


                ERosterCode rosterCode = new ERosterCode();
                rosterCode.RosterCodeID = rosterTable.RosterCodeID;

                if (ERosterCode.db.select(dbConn, rosterCode))
                {
                    if (!string.IsNullOrEmpty(rosterCode.RosterCodeColorCode))
                    {
                        try
                        {
                            e.Appointment.BackColor = System.Drawing.ColorTranslator.FromHtml(rosterCode.RosterCodeColorCode);
                            e.Appointment.ForeColor = AppUtils.ComputeTextColor(e.Appointment.BackColor);
                        }
                        catch
                        {
                        }
                    }
                }
                
                    DBFilter userRosterTableGroupListFilter = new DBFilter();
                    userRosterTableGroupListFilter.add(new Match("EmpID", m_ESSUserID));
                    userRosterTableGroupListFilter.add(new Match("EmpRosterTableGroupIsSupervisor", true));
                    userRosterTableGroupListFilter.add(new Match("empRosterTableGroupEffFr", "<=", e.Appointment.Start.Date));
                    OR orEmpPosEffToTerms = new OR();
                    orEmpPosEffToTerms.add(new Match("empRosterTableGroupEffTo", ">=", e.Appointment.Start.Date));
                    orEmpPosEffToTerms.add(new NullTerm("empRosterTableGroupEffTo"));
                    userRosterTableGroupListFilter.add(orEmpPosEffToTerms);
                    ArrayList empRosterTableGroupList = EEmpRosterTableGroup.db.select(dbConn, userRosterTableGroupListFilter);
                
                foreach (EEmpRosterTableGroup empRosterTableGroup in empRosterTableGroupList)
                {
                    DBFilter subordinateRosterTableGroupListFilter = new DBFilter();
                    subordinateRosterTableGroupListFilter.add(new Match("EmpID", rosterTable.EmpID));
                    subordinateRosterTableGroupListFilter.add(new Match("RosterTableGroupID", empRosterTableGroup.RosterTableGroupID));
                    subordinateRosterTableGroupListFilter.add(new Match("empRosterTableGroupEffFr", "<=", e.Appointment.Start.Date));
                    OR orSubOrdinateEmpPosEffToTerms = new OR();
                    orSubOrdinateEmpPosEffToTerms.add(new Match("empRosterTableGroupEffTo", ">=", e.Appointment.Start.Date));
                    orSubOrdinateEmpPosEffToTerms.add(new NullTerm("empRosterTableGroupEffTo"));
                    subordinateRosterTableGroupListFilter.add(orSubOrdinateEmpPosEffToTerms);

                    if (EEmpRosterTableGroup.db.count(dbConn, subordinateRosterTableGroupListFilter) > 0)
                        e.Appointment.AllowEdit = true;

                    //EEmpPositionInfo empPos = AppUtils.GetLastPositionInfo(dbConn, e.Appointment.Start.Date, rosterTable.EmpID);
                    //if (empPos != null)
                    //    if (empPos.RosterTableGroupID.Equals(userEmpPos.RosterTableGroupID))
                    //        e.Appointment.AllowEdit = true;
                    //    else
                    //        e.Appointment.AllowEdit = false;
                }

            }
        }
        if (RadScheduler1.SelectedView == SchedulerViewType.TimelineView)
            if (RadScheduler1.Appointments.Count > 10)
                RadScheduler1.Height = Unit.Parse("");
    }
    protected void RadScheduler1_AppointmentContextMenuItemClicking(object sender, AppointmentContextMenuItemClickingEventArgs e)
    {

    }
    protected void RadScheduler1_TimeSlotCreated(object sender, TimeSlotCreatedEventArgs e)
    {
        string holidayDescription = string.Empty;
        if (DateTime.Compare(e.TimeSlot.Start.Date, DateTime.Now.Date) == 0)
        {
            e.TimeSlot.CssClass = "RadSchedulerToday";
        }
        else if (e.TimeSlot.Start.Date.DayOfWeek == DayOfWeek.Sunday)
        {
            e.TimeSlot.CssClass = "RadSchedulerHoliday";
        }
        else if (EPublicHoliday.IsHoliday(dbConn, e.TimeSlot.Start.Date))
        {
            e.TimeSlot.CssClass = "RadSchedulerHoliday";
            //Label holidayLabel = new Label();
            //holidayLabel.Text = holidayDescription;
            //holidayLabel.ForeColor = System.Drawing.Color.Red;
            ////holidayLabel.CssClass = "FloatRight";
            ////e.TimeSlot.Control.Controls.AddAt(1, holidayLabel);
            //e.TimeSlot.Control.Controls[0].Controls.AddAt(1, holidayLabel);
        }
        else if (EStatutoryHoliday.IsHoliday(dbConn, e.TimeSlot.Start.Date))
        {
            e.TimeSlot.CssClass = "RadSchedulerHoliday";
            //Label holidayLabel = new Label();
            //holidayLabel.Text = holidayDescription;
            //holidayLabel.ForeColor = System.Drawing.Color.Red;
            ////holidayLabel.CssClass = "FloatRight";
            ////e.TimeSlot.Control.Controls.AddAt(1, holidayLabel);
            //e.TimeSlot.Control.Controls[0].Controls.AddAt(1, holidayLabel);
        }
    }
    protected void RosterTableGroupID_SelectedIndexChanged(object sender, EventArgs e)
    {
        RadScheduler1.Rebind();
    }

    // Start 0000179, KuangWei, 2015-03-20
    protected void RosterTableCheckedChanged(object sender, EventArgs e)
    {
        RadScheduler1.Rebind();
    }
    // End 0000179, KuangWei, 2015-03-20
}
