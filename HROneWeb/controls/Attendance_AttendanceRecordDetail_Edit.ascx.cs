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

public partial class Attendance_AttendanceRecordDetail_Edit : HROneWebControl
{
    protected DBManager db = EAttendanceRecord.db;
    public event EventHandler Closed;
    Binding eBinding;
    public const string FUNCTION_CODE = "ATT008";
    protected const string ROSTER_CLIENT_ALL_ROSTER_CODE = "%ALLROSTERCODE%";

    protected void Page_Load(object sender, EventArgs e)
    {
        eBinding = new Binding(dbConn, EAttendanceRecord.db);
        eBinding.add(EmpID);
        eBinding.add(new HiddenBinder(EAttendanceRecord.db, hiddenAttendanceRecordID, "AttendanceRecordID"));
        eBinding.add(AttendanceRecordDate);
        eBinding.add(new DropDownVLBinder(EAttendanceRecord.db, RosterCodeID, ERosterCode.VLRosterCode));
        eBinding.add(AttendanceRecordRosterCodeInTimeOverride);
        eBinding.add(AttendanceRecordRosterCodeLunchStartTimeOverride);
        eBinding.add(AttendanceRecordRosterCodeLunchEndTimeOverride);
        eBinding.add(AttendanceRecordRosterCodeOutTimeOverride);
        eBinding.add(AttendanceRecordWorkStart);
        eBinding.add(AttendanceRecordLunchOut);
        eBinding.add(AttendanceRecordLunchIn);
        eBinding.add(AttendanceRecordWorkEnd);
        eBinding.add(AttendanceRecordWorkStartLocation);
        eBinding.add(AttendanceRecordLunchOutLocation);
        eBinding.add(AttendanceRecordLunchInLocation);
        eBinding.add(AttendanceRecordWorkEndLocation);
        eBinding.add(AttendanceRecordActualLateMins);
        eBinding.add(AttendanceRecordWaivedLateMins);
        
        eBinding.add(AttendanceRecordActualEarlyLeaveMins);
        eBinding.add(AttendanceRecordActualLunchLateMins);
        eBinding.add(AttendanceRecordActualLunchEarlyLeaveMins);
        eBinding.add(AttendanceRecordActualOvertimeMins);
        eBinding.add(AttendanceRecordActualLunchOvertimeMins);
        eBinding.add(AttendanceRecordActualWorkingHour);
        eBinding.add(AttendanceRecordActualWorkingDay);
        eBinding.add(AttendanceRecordActualLunchTimeMins);
        eBinding.add(new CheckBoxBinder(EAttendanceRecord.db, AttendanceRecordIsAbsent));
        eBinding.add(new TextBoxXMLNodeBinder(db, OverrideDailyPayment, "AttendanceRecordExtendData", OverrideDailyPayment.ID));
        eBinding.add(new CheckBoxXMLNodeBinder(db, WorkAsOvertime, "AttendanceRecordExtendData", WorkAsOvertime.ID));
        eBinding.add(new CheckBoxBinder(db, AttendanceRecordWorkOnRestDay));
        eBinding.add(AttendanceRecordRemark);


        eBinding.init(Request, Session);
        WebFormUtils.loadValues(dbConn, RosterClientID, ERosterClient.VLRosterClient, new DBFilter(),null,RosterClientID.SelectedValue,"combobox.notselected");
        RosterClientID.Items.Insert(0, new ListItem("---All---", ROSTER_CLIENT_ALL_ROSTER_CODE));
    }

    public void Show()
    {
        AdjustmentModalPopupExtender.Show();
    }

    public void LoadAttendanceRecord(int intAttendanceRecordID)
    {
        EAttendanceRecord obj = new EAttendanceRecord();
        obj.AttendanceRecordID = intAttendanceRecordID;
        if (EAttendanceRecord.db.select(dbConn, obj))
        {
            Hashtable values = new Hashtable();
            EAttendanceRecord.db.populate(obj, values);


            eBinding.toControl(values);

            if (obj.AttendanceRecordOverrideBonusEntitled)
                if (obj.AttendanceRecordHasBonus)
                    AttendanceRecordHasBonus.SelectedValue = "Y";
                else
                    AttendanceRecordHasBonus.SelectedValue = "N";
            else
                AttendanceRecordHasBonus.SelectedValue = "";

            AttendanceRecordRosterCodeInTimeOverride.MaxLength = 0;
            AttendanceRecordRosterCodeLunchStartTimeOverride.MaxLength = 0;
            AttendanceRecordRosterCodeLunchEndTimeOverride.MaxLength = 0;
            AttendanceRecordRosterCodeOutTimeOverride.MaxLength = 0;
            AttendanceRecordWorkStart.MaxLength = 0;
            AttendanceRecordLunchOut.MaxLength = 0;
            AttendanceRecordLunchIn.MaxLength = 0;
            AttendanceRecordWorkEnd.MaxLength = 0;

            ERosterCode rosterCode = new ERosterCode();
            rosterCode.RosterCodeID = obj.RosterCodeID;
            ERosterCode.db.select(dbConn, rosterCode);
            if (rosterCode.RosterClientID > 0)
                RosterClientID.SelectedValue = rosterCode.RosterClientID.ToString();

            RefreshClientSite(rosterCode.RosterClientID);

            if (rosterCode.RosterClientSiteID > 0)
                RosterClientSiteID.SelectedValue = rosterCode.RosterClientSiteID.ToString();

            RefreshRosterCode(rosterCode.RosterClientSiteID);

        }
    }
    protected void ButtonMessageOkay_Click(object sender, EventArgs e)
    {


        EAttendanceRecord obj = new EAttendanceRecord();
        Hashtable values = new Hashtable();

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        eBinding.toValues(values);
        db.validate(errors, values);

        if (!errors.isEmpty())
        {
            AdjustmentModalPopupExtender.Show(); 
            return;
        }

        db.parse(values, obj);

        if (AttendanceRecordHasBonus.SelectedValue.Equals("Y", StringComparison.CurrentCultureIgnoreCase))
        {
            obj.AttendanceRecordOverrideBonusEntitled = true;
            obj.AttendanceRecordHasBonus = true;
        }
        else if (AttendanceRecordHasBonus.SelectedValue.Equals("N", StringComparison.CurrentCultureIgnoreCase))
        {
            obj.AttendanceRecordOverrideBonusEntitled = true;
            obj.AttendanceRecordHasBonus = false;
        }
        else
        {
            obj.AttendanceRecordOverrideBonusEntitled = false;
            obj.AttendanceRecordHasBonus = false;
        }

        if (!errors.isEmpty())
        {
            AdjustmentModalPopupExtender.Show(); 
            return;
        }

        WebUtils.StartFunction(Session, FUNCTION_CODE, obj.EmpID);
        db.update(dbConn, obj);
        WebUtils.EndFunction(dbConn);
        if (Closed != null)
            Closed(sender, e);

    }
    protected void ButtonMessageCancel_Click(object sender, EventArgs e)
    {
        if (Closed != null)
            Closed(sender, e);

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
        else if (RosterClientID.SelectedValue.Equals(ROSTER_CLIENT_ALL_ROSTER_CODE, StringComparison.CurrentCultureIgnoreCase))
            RefreshRosterCode(-1);
        else
            RefreshRosterCode(0);
        AdjustmentModalPopupExtender.Show();

    }
    protected void RosterClientSiteID_SelectedIndexChanged(object sender, EventArgs e)
    {
        string selected = RosterClientSiteID.SelectedValue;
        int intRosterClientSiteID;

        if (int.TryParse(selected, out intRosterClientSiteID))
            RefreshRosterCode(intRosterClientSiteID);
        else
            RefreshRosterCode(0);
        AdjustmentModalPopupExtender.Show();

    }

    protected void RefreshRosterCode(int RosterClientSiteID)
    {
        string selected = RosterCodeID.SelectedValue;
        if (string.IsNullOrEmpty(selected))
            selected = null;

        DBFilter rosterCodeFilter = new DBFilter();
        if (RosterClientSiteID >= 0)
            rosterCodeFilter.add(new Match("RosterClientSiteID", RosterClientSiteID));
        WebFormUtils.loadValues(dbConn, RosterCodeID, ERosterCode.VLRosterCode, rosterCodeFilter, null, (string)selected, (string)"combobox.notselected");


    }

}
