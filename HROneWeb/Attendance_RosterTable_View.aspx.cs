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

public partial class Attendance_RosterTable_View : HROneWebPage
{
    //protected object EditedAppointmentID
    //{
    //    get { return ViewState["EditedAppointmentID"]; }
    //    set { ViewState["EditedAppointmentID"] = value; }
    //}


    //protected SchedulerDataSource dataSource;

    protected SearchBinding RosterClientSiteSBinding;
    bool m_IsAllowWrite = true;
    private const string FUNCTION_CODE = "ATT013";
    

    protected void Page_Load(object sender, EventArgs e)
    {
       

        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;

        m_IsAllowWrite = WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite);

        RosterClientSiteSBinding = new SearchBinding(dbConn, ERosterClientSite.db);

        if (!IsPostBack)
        {
            rosterClientSiteloadData();
        }

        HtmlLink cssLink = new HtmlLink();
        cssLink.Href = "~/CSS/RadScheduler.css";
        cssLink.Attributes.Add("rel", "stylesheet");
        cssLink.Attributes.Add("type", "text/css");
        Header.Controls.Add(cssLink);

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            chkRosterClientSiteCheckAll.Attributes.Add("onclick", "checkAll('" + rosterClientSiteRepeater.ClientID + "','ItemSelect',this.checked);");
        }
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
    //    ArrayList rosterTableList = ERosterTable.db.select(dbConn, filter);
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

    //    if (EEmpPersonalInfo.db.select(dbConn, empInfo) && ERosterCode.db.select(dbConn, rosterCode))
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

        ArrayList arrRosterClientSiteIDList = new ArrayList();

        arrRosterClientSiteIDList = WebUtils.SelectedRepeaterItemToBaseObjectList(ERosterClientSite.db, rosterClientSiteRepeater, "ItemSelect");

        e.InputParameters["RosterClientSiteIDList"] = arrRosterClientSiteIDList;

        e.InputParameters["UserID"] = WebUtils.GetCurUser(Session).UserID;
        e.InputParameters["DisplayOvernightToNextDay"] = !RadScheduler1.SelectedView.Equals(Telerik.Web.UI.SchedulerViewType.MonthView);
        e.InputParameters["ShowEmployeeRosterWithSameSite"] = chkEmployeeSameDefaultSite.Checked;
        e.InputParameters["ShowRosterCodeWithoutSiteMapping"] = chkRosterCodeWithoutSiteMapping.Checked;
        if (RadScheduler1.SelectedView == SchedulerViewType.TimelineView)
            RadScheduler1.Height = Unit.Parse("600px");
        else
            RadScheduler1.Height = Unit.Parse("");
    }

    protected void RadScheduler1_AppointmentCommand(object sender, Telerik.Web.UI.AppointmentCommandEventArgs e)
    {
        RadScheduler1.Rebind();
    }
    protected void RadScheduler1_AppointmentClick(object sender, Telerik.Web.UI.SchedulerEventArgs e)
    {
        System.Diagnostics.Debug.Write(e.ToString());
    }
    protected void RadScheduler1_AppointmentContextMenuItemClicked(object sender, Telerik.Web.UI.AppointmentContextMenuItemClickedEventArgs e)
    {
        //System.Diagnostics.Debug.Write(e.ToString());

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
    //                if (ERosterTable.db.select(dbConn, rosterTable))
    //                {
    //                    Binding ebinding = new Binding(dbConn, ERosterTable.db);
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
    //                    if (EEmpPersonalInfo.db.select(dbConn, empInfo))
    //                        EmpName.Text = empInfo.EmpEngFullNameWithAlias;
    //                    else
    //                        EmpName.Text = string.Empty;

    //                }

    //                ERosterCode rosterCode = new ERosterCode();
    //                rosterCode.RosterCodeID = rosterTable.RosterCodeID;
    //                ERosterCode.db.select(dbConn, rosterCode);
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

    //    Binding ebinding = new Binding(dbConn, ERosterTable.db);
    //    ebinding.add(RosterTableID);
    //    ebinding.add(RosterTableDate);
    //    ebinding.add(EmpID);
    //    ebinding.add(new DropDownVLBinder(ERosterTable.db, RosterCodeID, ERosterCode.VLRosterCode));
    //    Hashtable values = new Hashtable();
    //    ebinding.toValues(values);
    //    ERosterTable.db.parse(values, rosterTable);

    //    ERosterTable.db.update(dbConn, rosterTable);

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
    //    System.Diagnostics.Debug.Write(e.ToString());
    //}

    public void rosterClientSiteloadData()
    {


            DBFilter filter = new DBFilter();

            //if (info != null && info.orderby != null && !info.orderby.Equals(""))
            //    filter.add(info.orderby, info.order);

            string select = "* ";
            string from = "from " + ERosterClientSite.db.dbclass.tableName + " ";

            DataTable table = filter.loadData(dbConn, null, select, from);

            rosterClientSiteRepeater.DataSource = table;
            rosterClientSiteRepeater.DataBind();
    }
    protected void rosterClientSiteRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(ERosterClientSite.db, row, cb);
        //cb.Checked = true;
    }
    protected void RosterClientSite_OnCheckedChanged(object sender, EventArgs e)
    {
        RadScheduler1.Rebind();

    }

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

        if (e.Appointment.ID.ToString().StartsWith(HROne.Lib.Attendance.DataSource.RosterTableEvent.LEAVEAPP_ID_PREFIX))
            e.Appointment.AllowEdit = false;
        else
            e.Appointment.AllowEdit = m_IsAllowWrite;

        HROne.Lib.Attendance.DataSource.RosterTableEvent rosterTableEvent = (HROne.Lib.Attendance.DataSource.RosterTableEvent)e.Appointment.DataItem;
        e.Appointment.ToolTip = rosterTableEvent.ToolTip;
        if (rosterTableEvent.Id.StartsWith(HROne.Lib.Attendance.DataSource.RosterTableEvent.PUBLICHOLIDAY_PREFIX)
        || rosterTableEvent.Id.StartsWith(HROne.Lib.Attendance.DataSource.RosterTableEvent.STATUTORYHOLIDAY_PREFIX))
        {
            e.Appointment.AllowEdit = false;
            e.Appointment.ForeColor = System.Drawing.Color.Red;
        }

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
        }
        else if (EStatutoryHoliday.IsHoliday(dbConn, e.TimeSlot.Start.Date))
        {
            e.TimeSlot.CssClass = "RadSchedulerHoliday";
        }
    }
}
