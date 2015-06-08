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
using System.ComponentModel;
using HROne.Lib.Entities;
using HROne.DataAccess;
using Telerik.Web.UI;

namespace HROne.UI.Scheduler
{
    //public enum AdvancedFormMode
    //{
    //    Insert, Edit
    //}

    public partial class Attendance_RosterTable_Edit_AdvancedTemplate : HROneWebControl
    {
        #region Private members

        private bool FormInitialized
        {
            get
            {
                return (bool)(ViewState["FormInitialized"] ?? false);
            }

            set
            {
                ViewState["FormInitialized"] = value;
            }
        }

        //private AdvancedFormMode mode = AdvancedFormMode.Insert;

        #endregion

        #region Protected properties

        protected RadScheduler Owner
        {
            get
            {
                return Appointment.Owner;
            }
        }

        protected Appointment Appointment
        {
            get
            {
                SchedulerFormContainer container = (SchedulerFormContainer)BindingContainer;
                return container.Appointment;
            }
        }

        #endregion

        //protected object item_ID;
        //[Bindable(BindableSupport.Yes, BindingDirection.TwoWay)]
        //public object ObjectID
        //{
        //    get
        //    {
        //        return item_ID;
        //    }

        //    set
        //    {
        //        item_ID = value;
        //    }
        //}
        //#region Public properties

        //public AdvancedFormMode Mode
        //{
        //    get
        //    {
        //        return mode;
        //    }
        //    set
        //    {
        //        mode = value;
        //    }
        //}

        //string m_subject;
        //[Bindable(BindableSupport.Yes, BindingDirection.TwoWay)]
        //public string Subject
        //{
        //    get
        //    {
        //        return m_subject;
        //    }

        //    set
        //    {
        //        m_subject = value;
        //    }
        //}

        //string m_description;
        //[Bindable(BindableSupport.Yes, BindingDirection.TwoWay)]
        //public string Description
        //{
        //    get
        //    {
        //        return m_description;
        //    }

        //    set
        //    {
        //        m_description = value;
        //    }
        //}

        //DateTime m_start;
        //[Bindable(BindableSupport.Yes, BindingDirection.TwoWay)]
        //public DateTime Start
        //{
        //    get
        //    {
        //        return m_start;
        //    }

        //    set
        //    {
        //        m_start = value;
        //    }
        //}

        //DateTime m_end;
        //[Bindable(BindableSupport.Yes, BindingDirection.TwoWay)]
        //public DateTime End
        //{
        //    get
        //    {

        //        return m_end;
        //    }

        //    set
        //    {
        //        m_end = value;
        //    }
        //}
        //#endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            this.DataBinding += new EventHandler(Page_DataBinding);
        }

        protected void Page_DataBinding(object sender, EventArgs e)
        {
            PopulateEditForm();
        }

        protected void PopulateEditForm()
        {
            WebFormUtils.loadValues(dbConn, RosterClientID, ERosterClient.VLRosterClient, new DBFilter());

            object dbObjectItem = HROne.Lib.Attendance.DataSource.RosterTableEvent.IDToDBObject(dbConn, Appointment.ID);
            if (dbObjectItem is ERosterTable)
            {
                ERosterTable rosterTable = (ERosterTable)dbObjectItem;
                Hashtable values = new Hashtable();
                ERosterTable.db.populate(rosterTable, values);

                Binding ebinding = new Binding(dbConn, ERosterTable.db);
                ebinding.add(RosterTableID);
                ebinding.add(EmpID);
                ebinding.add(RosterTableDate);
                ebinding.add(RosterTableOverrideInTime);
                ebinding.add(RosterTableOverrideOutTime);

                if (values["RosterCodeID"].Equals("0"))
                    values.Remove("RosterCodeID");
                else
                    ebinding.add(new DropDownVLBinder(ERosterTable.db, RosterCodeID, ERosterCode.VLRosterCode));
                ebinding.init(Request, Session);
                ebinding.toControl(values);
                EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                empInfo.EmpID = rosterTable.EmpID;
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                {
                    EmpNo.Text = empInfo.EmpNo;
                    EmpName.Text = empInfo.EmpEngFullNameWithAlias;
                }
                else
                {
                    EmpNo.Text = string.Empty;
                    EmpName.Text = string.Empty;
                }
                ERosterCode rosterCode = new ERosterCode();
                rosterCode.RosterCodeID = rosterTable.RosterCodeID;
                ERosterCode.db.select(dbConn, rosterCode);
                if (rosterCode.RosterClientID > 0)
                    RosterClientID.SelectedValue = rosterCode.RosterClientID.ToString();

                RefreshClientSite(rosterCode.RosterClientID);

                if (rosterCode.RosterClientSiteID > 0)
                    RosterClientSiteID.SelectedValue = rosterCode.RosterClientSiteID.ToString();

                RefreshRosterCode(rosterCode.RosterClientSiteID);

                if (rosterTable.RosterTableOverrideInTime.Ticks.Equals(0))
                    RosterTableOverrideInTime.Text = string.Empty;
                if (rosterTable.RosterTableOverrideOutTime.Ticks.Equals(0))
                    RosterTableOverrideOutTime.Text = string.Empty;

                RefreshRosterTableInOutTime(rosterTable.RosterCodeID, false);
            }

            //if (item_ID != null)
            //{
            //    string[] id_Array = item_ID.ToString().Split(new char[] { '_' });

            //    if (id_Array.GetLength(0) == 2)
            //    {
            //        if (id_Array[0].Equals("RosterTable"))
            //        {
            //            ERosterTable rosterTable = new ERosterTable();
            //            rosterTable.RosterTableID = int.Parse(id_Array[1]);
            //            if (ERosterTable.db.select(dbConn, rosterTable))
            //            {
            //                Binding ebinding = new Binding(dbConn, ERosterTable.db);
            //                ebinding.add(RosterTableID);
            //                ebinding.add(EmpID);
            //                ebinding.add(RosterTableDate);
            //                ebinding.add(new DropDownVLBinder(ERosterTable.db, RosterCodeID, ERosterCode.VLRosterCode));
            //                ebinding.init(Request, Session);
            //                Hashtable values = new Hashtable();
            //                ERosterTable.db.populate(rosterTable, values);
            //                ebinding.toControl(values);

            //                EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
            //                empInfo.EmpID = rosterTable.EmpID;
            //                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
            //                    EmpName.Text = empInfo.EmpEngFullNameWithAlias;
            //                else
            //                    EmpName.Text = string.Empty;

            //            }

            //            ERosterCode rosterCode = new ERosterCode();
            //            rosterCode.RosterCodeID = rosterTable.RosterCodeID;
            //            ERosterCode.db.select(dbConn, rosterCode);
            //            if (rosterCode.RosterClientID > 0)
            //                RosterClientID.SelectedValue = rosterCode.RosterClientID.ToString();



            //            RefreshClientSite(rosterCode.RosterClientID);

            //            if (rosterCode.RosterClientSiteID > 0)
            //                RosterClientSiteID.SelectedValue = rosterCode.RosterClientSiteID.ToString();

            //            RefreshRosterCode(rosterCode.RosterClientSiteID);
            //        }
            //    }
            //}
        }
        protected void RefreshClientSite(int RosterClientID)
        {
            string selected = RosterClientSiteID.SelectedValue;

            if (string.IsNullOrEmpty(selected))
                selected = null;
            DBFilter rosterClientSiteFilter = new DBFilter();
            rosterClientSiteFilter.add(new Match("RosterClientID", RosterClientID));
            WebFormUtils.loadValues(dbConn, RosterClientSiteID, ERosterClientSite.VLRosterClientSite, rosterClientSiteFilter, null, (string)selected, (string)"combobox.notselected");

        }
        protected void RosterClientID_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = RosterClientID.SelectedValue;
            int intRosterClientID;

            if (int.TryParse(selected, out intRosterClientID))
                RefreshClientSite(intRosterClientID);
            else
                RefreshClientSite(0);

            selected = RosterClientSiteID.SelectedValue;
            int intRosterClientSiteID;

            if (int.TryParse(selected, out intRosterClientSiteID))
                RefreshRosterCode(intRosterClientSiteID);
            else
                RefreshRosterCode(0);

        }
        protected void RosterClientSiteID_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = RosterClientSiteID.SelectedValue;
            int intRosterClientSiteID;

            if (int.TryParse(selected, out intRosterClientSiteID))
                RefreshRosterCode(intRosterClientSiteID);
            else
                RefreshRosterCode(0);

        }

        protected void RefreshRosterCode(int RosterClientSiteID)
        {
            string selected = RosterCodeID.SelectedValue;
            if (string.IsNullOrEmpty(selected))
                selected = null;

            DBFilter rosterCodeFilter = new DBFilter();
            rosterCodeFilter.add(new Match("RosterClientSiteID", RosterClientSiteID));
            WebFormUtils.loadValues(dbConn, RosterCodeID, ERosterCode.VLRosterCode, rosterCodeFilter, null, (string)selected, (string)"combobox.notselected");


        }

        protected void RefreshRosterTableInOutTime(int RosterCodeID, bool overrideSetting)
        {
            ERosterCode rosterCode = new ERosterCode();
            rosterCode.RosterCodeID = RosterCodeID;
            if (ERosterCode.db.select(dbConn, rosterCode))
            {
                if (string.IsNullOrEmpty(RosterTableOverrideInTime.Text) || overrideSetting)
                    RosterTableOverrideInTime.Text = ERosterCode.db.getField("RosterCodeInTime").populateValue(rosterCode.RosterCodeInTime);
                if (string.IsNullOrEmpty(RosterTableOverrideOutTime.Text) || overrideSetting)
                    RosterTableOverrideOutTime.Text = ERosterCode.db.getField("RosterCodeOutTime").populateValue(rosterCode.RosterCodeOutTime);
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            ERosterTable rosterTable = new ERosterTable();

            Binding ebinding = new Binding(dbConn, ERosterTable.db);
            ebinding.add(RosterTableID);
            ebinding.add(RosterTableDate);
            ebinding.add(EmpID);
            ebinding.add(new DropDownVLBinder(ERosterTable.db, RosterCodeID, ERosterCode.VLRosterCode));
            ebinding.add(RosterTableOverrideInTime);
            ebinding.add(RosterTableOverrideOutTime);

            Hashtable values = new Hashtable();
            ebinding.toValues(values);
            ERosterTable.db.parse(values, rosterTable);
            ERosterCode rosterCode = new ERosterCode();
            rosterCode.RosterCodeID = rosterTable.RosterCodeID;
            if (ERosterCode.db.select(dbConn, rosterCode))
            {
                if (rosterCode.RosterCodeInTime.TimeOfDay.Equals(rosterTable.RosterTableOverrideInTime.TimeOfDay))
                    rosterTable.RosterTableOverrideInTime = new DateTime(0);
                if (rosterCode.RosterCodeOutTime.TimeOfDay.Equals(rosterTable.RosterTableOverrideOutTime.TimeOfDay))
                    rosterTable.RosterTableOverrideOutTime = new DateTime(0);

                //if (rosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_LEAVE) && rosterCode.LeaveCodeID > 0)
                //{
                //    ELeaveCode leaveCode = new ELeaveCode();
                //    leaveCode.LeaveCodeID = rosterCode.LeaveCodeID;
                //    if (ELeaveCode.db.select(dbConn, leaveCode))
                //    {
                //        ELeaveApplication leaveApp = new ELeaveApplication();
                //        leaveApp.LeaveAppDateFrom = rosterTable.RosterTableDate;
                //        leaveApp.LeaveAppDateTo = rosterTable.RosterTableDate;
                //        leaveApp.LeaveAppDays = 1;
                //        leaveApp.LeaveAppUnit = "D";
                //        leaveApp.LeaveCodeID = leaveCode.LeaveCodeID;
                //        leaveApp.EmpID = rosterTable.EmpID;

                //        ELeaveApplication.db.insert(dbConn, leaveApp);

                //        rosterTable.LeaveAppID = leaveApp.LeaveAppID;
                //        HROne.LeaveCalc.LeaveBalanceCalc leaaveBalCal = new HROne.LeaveCalc.LeaveBalanceCalc(dbConn, rosterTable.EmpID);
                //        leaaveBalCal.RecalculateAfter(rosterTable.RosterTableDate);
                //    }
                //    else
                //        rosterTable.LeaveAppID = 0;

                //}

            }
            //ERosterTable oldRosterTable = new ERosterTable();
            //oldRosterTable.RosterTableID = rosterTable.RosterTableID;
            //if (ERosterTable.db.select(dbConn, oldRosterTable))
            //{
            //    if (oldRosterTable.LeaveAppID > 0)
            //    {
            //        ELeaveApplication leaveApp = new ELeaveApplication();
            //        leaveApp.LeaveAppID = oldRosterTable.LeaveAppID;
            //        if (ELeaveApplication.db.select(dbConn, leaveApp))
            //            if (leaveApp.EmpPayrollID <= 0)
            //            {
            //                ELeaveApplication.db.delete(dbConn, leaveApp);
            //                HROne.LeaveCalc.LeaveBalanceCalc leaaveBalCal = new HROne.LeaveCalc.LeaveBalanceCalc(dbConn, oldRosterTable.EmpID);
            //                leaaveBalCal.RecalculateAfter(oldRosterTable.RosterTableDate);
            //            }
            //    }

            //}

            ERosterTable.db.update(dbConn, rosterTable);

            //// Create resource based on the selected user
            ////Resource user = new Resource("User", int.Parse(UserDropDown.SelectedValue), UserDropDown.SelectedItem.Text);
            //DateTime start = RadScheduler1.DisplayToUtc(StartTime.SelectedDate.Value);
            //DateTime end = RadScheduler1.DisplayToUtc(EndTime.SelectedDate.Value);

            //if (EditedAppointmentID == null)
            //{
            //    // Insert
            //    Appointment appointment = new Appointment(null, start, end, DescriptionText.Text);
            //    //appointment.Resources.Add(user);

            //    RadScheduler1.InsertAppointment(appointment);
            //}
            //else
            //{
            //    Appointment appointment = RadScheduler1.Appointments.FindByID(EditedAppointmentID);
            //    Appointment appointmentToUpdate = RadScheduler1.PrepareToEdit(appointment, RadScheduler1.EditingRecurringSeries);

            //    appointmentToUpdate.Subject = DescriptionText.Text;
            //    appointmentToUpdate.Start = start;
            //    appointmentToUpdate.End = end;

            //    // Remove the existing user resource, if any
            //    Resource existingUser = appointmentToUpdate.Resources.GetResourceByType("User");
            //    if (existingUser != null)
            //    {
            //        appointmentToUpdate.Resources.Remove(existingUser);
            //    }
            //    //appointmentToUpdate.Resources.Add(user);

            //    RadScheduler1.UpdateAppointment(appointmentToUpdate);
            //}
        }

        protected void RosterCodeID_SelectedIndexChanged(object sender, EventArgs e)
        {
            int rosterCodeID = 0;
            if (int.TryParse(RosterCodeID.SelectedValue,out rosterCodeID))
            {
                RefreshRosterTableInOutTime(rosterCodeID, true);
            }
        }
}
}