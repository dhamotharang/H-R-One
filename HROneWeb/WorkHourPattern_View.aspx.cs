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
using HROne.DataAccess;
//using perspectivemind.validation;
using HROne.Lib.Entities;
using HROne.Translation;
public partial class WorkHourPattern_View : HROneWebPage
{
    private const string FUNCTION_CODE = "SYS009";

    public Binding binding;
    public DBManager db = EWorkHourPattern.db;
    public int CurID = -1;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        

        binding = new Binding(dbConn, db);
        binding.add(WorkHourPatternID);

        binding.add(WorkHourPatternCode);
        binding.add(WorkHourPatternDesc);
        binding.add(new BlankZeroLabelVLBinder(db, WorkHourPatternWorkDayDetermineMethod, EWorkHourPattern.VLWorkDayDetermineMethod));

        binding.add(new BlankZeroLabelVLBinder(db, WorkHourPatternSunDefaultRosterCodeID, ERosterCode.VLRosterCode));
        binding.add(new BlankZeroLabelVLBinder(db, WorkHourPatternMonDefaultRosterCodeID, ERosterCode.VLRosterCode));
        binding.add(new BlankZeroLabelVLBinder(db, WorkHourPatternTueDefaultRosterCodeID, ERosterCode.VLRosterCode));
        binding.add(new BlankZeroLabelVLBinder(db, WorkHourPatternWedDefaultRosterCodeID, ERosterCode.VLRosterCode));
        binding.add(new BlankZeroLabelVLBinder(db, WorkHourPatternThuDefaultRosterCodeID, ERosterCode.VLRosterCode));
        binding.add(new BlankZeroLabelVLBinder(db, WorkHourPatternFriDefaultRosterCodeID, ERosterCode.VLRosterCode));
        binding.add(new BlankZeroLabelVLBinder(db, WorkHourPatternSatDefaultRosterCodeID, ERosterCode.VLRosterCode));
        binding.add(WorkHourPatternContractWorkHoursPerDay);
        binding.add(WorkHourPatternContractLunchTimeHoursPerDay);
        binding.add(WorkHourPatternSunDefaultDayUnit);
        binding.add(WorkHourPatternMonDefaultDayUnit);
        binding.add(WorkHourPatternTueDefaultDayUnit);
        binding.add(WorkHourPatternWedDefaultDayUnit);
        binding.add(WorkHourPatternThuDefaultDayUnit);
        binding.add(WorkHourPatternFriDefaultDayUnit);
        binding.add(WorkHourPatternSatDefaultDayUnit);

        binding.add(new CheckBoxBinder(db, WorkHourPatternUsePublicHolidayTable));
        binding.add(new CheckBoxBinder(db, WorkHourPatternUseStatutoryHolidayTable));
        binding.add(new BlankZeroLabelVLBinder(db, WorkHourPatternPublicHolidayDefaultRosterCodeID, ERosterCode.VLRosterCode));
        binding.add(new BlankZeroLabelVLBinder(db, WorkHourPatternStatutoryHolidayDefaultRosterCodeID, ERosterCode.VLRosterCode));

        binding.add(WorkHourPatternSunWorkHoursPerDay);
        binding.add(WorkHourPatternMonWorkHoursPerDay);
        binding.add(WorkHourPatternTueWorkHoursPerDay);
        binding.add(WorkHourPatternWedWorkHoursPerDay);
        binding.add(WorkHourPatternThuWorkHoursPerDay);
        binding.add(WorkHourPatternFriWorkHoursPerDay);
        binding.add(WorkHourPatternSatWorkHoursPerDay);

        binding.add(WorkHourPatternSunLunchTimeHoursPerDay);
        binding.add(WorkHourPatternMonLunchTimeHoursPerDay);
        binding.add(WorkHourPatternTueLunchTimeHoursPerDay);
        binding.add(WorkHourPatternWedLunchTimeHoursPerDay);
        binding.add(WorkHourPatternThuLunchTimeHoursPerDay);
        binding.add(WorkHourPatternFriLunchTimeHoursPerDay);
        binding.add(WorkHourPatternSatLunchTimeHoursPerDay);


        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["WorkHourPatternID"], out CurID))
            CurID = -1;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (CurID > 0)
                loadObject();
            else
                toolBar.DeleteButton_Visible = false;
        }
    }

    protected bool loadObject()
    {
        EWorkHourPattern obj = new EWorkHourPattern();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
        if (!db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);

        CheckWorkHourDetermineControl(obj);


        return true;
    }

    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "WorkHourPattern_Edit.aspx?WorkHourPatternID=" + CurID);
    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        EWorkHourPattern o = new EWorkHourPattern();
        o.WorkHourPatternID = CurID;
        db.select(dbConn, o);
        DBFilter empWorkHourPatternFilter = new DBFilter();
        empWorkHourPatternFilter.add(new Match("WorkHourPatternID", o.WorkHourPatternID));
        empWorkHourPatternFilter.add("empid", true);
        ArrayList empPosList = EEmpPositionInfo.db.select(dbConn, empWorkHourPatternFilter);
        if (empPosList.Count > 0)
        {
            int curEmpID = 0;
            errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_USED_BY_EMPLOYEE, new string[] { HROne.Common.WebUtility.GetLocalizedString("Work Hour Pattern"), o.WorkHourPatternCode }));
            foreach (EEmpPositionInfo empPos in empPosList)
            {
                EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                empInfo.EmpID = empPos.EmpID;
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                    if (curEmpID != empPos.EmpID)
                    {
                        errors.addError("- " + empInfo.EmpNo + ", " + empInfo.EmpEngFullName);
                        curEmpID = empPos.EmpID;
                    }
                    // Start 0000148, Ricky So, 2014/12/21
                    //else
                    //    EEmpPositionInfo.db.delete(dbConn, empPos);
                    // End 0000148, Ricky So, 2014/12/21
            }
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
            return;
        }
        else
        {
            WebUtils.StartFunction(Session, FUNCTION_CODE);
            db.delete(dbConn, o);
            WebUtils.EndFunction(dbConn);

        }

        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "WorkHourPattern_List.aspx");
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "WorkHourPattern_List.aspx");

    }

    private void CheckWorkHourDetermineControl(EWorkHourPattern obj)
    {
        WorkHourPatternDaySettingsPanel.Visible = true;
        lblRosterCodeHeader.Visible = false;
        WorkHourPatternSunDefaultRosterCodeID.Visible = false;
        WorkHourPatternMonDefaultRosterCodeID.Visible = false;
        WorkHourPatternTueDefaultRosterCodeID.Visible = false;
        WorkHourPatternWedDefaultRosterCodeID.Visible = false;
        WorkHourPatternThuDefaultRosterCodeID.Visible = false;
        WorkHourPatternFriDefaultRosterCodeID.Visible = false;
        WorkHourPatternSatDefaultRosterCodeID.Visible = false;

        lblDayUnitHeader.Visible = false;
        WorkHourPatternSunDefaultDayUnit.Visible = false;
        WorkHourPatternMonDefaultDayUnit.Visible = false;
        WorkHourPatternTueDefaultDayUnit.Visible = false;
        WorkHourPatternWedDefaultDayUnit.Visible = false;
        WorkHourPatternThuDefaultDayUnit.Visible = false;
        WorkHourPatternFriDefaultDayUnit.Visible = false;
        WorkHourPatternSatDefaultDayUnit.Visible = false;
        WorkHourPatternStatutoryHolidayDefaultRosterCodeID.Visible = false;
        WorkHourPatternPublicHolidayDefaultRosterCodeID.Visible = false;

        WorkHoursPerDayHeaderCell.Visible = false;
        LunchTimeHoursPerDayHeaderCell.Visible = false;
        WorkHoursPerDaySunCell.Visible = false;
        LunchTimeHoursPerDaySunCell.Visible = false;
        WorkHoursPerDayMonCell.Visible = false;
        LunchTimeHoursPerDayMonCell.Visible = false;
        WorkHoursPerDayTueCell.Visible = false;
        LunchTimeHoursPerDayTueCell.Visible = false;
        WorkHoursPerDayWedCell.Visible = false;
        LunchTimeHoursPerDayWedCell.Visible = false;
        WorkHoursPerDayThuCell.Visible = false;
        LunchTimeHoursPerDayThuCell.Visible = false;
        WorkHoursPerDayFriCell.Visible = false;
        LunchTimeHoursPerDayFriCell.Visible = false;
        WorkHoursPerDaySatCell.Visible = false;
        LunchTimeHoursPerDaySatCell.Visible = false;

        if (obj.WorkHourPatternWorkDayDetermineMethod.Equals(EWorkHourPattern.WORKDAYDETERMINDMETHOD_ROSTERTABLE))
        {
            WorkHourPerDayRow.Visible = true;
            LunchTimePerDayRow.Visible = true;

            lblRosterCodeHeader.Visible = true;
            WorkHourPatternSunDefaultRosterCodeID.Visible = true;
            WorkHourPatternMonDefaultRosterCodeID.Visible = true;
            WorkHourPatternTueDefaultRosterCodeID.Visible = true;
            WorkHourPatternWedDefaultRosterCodeID.Visible = true;
            WorkHourPatternThuDefaultRosterCodeID.Visible = true;
            WorkHourPatternFriDefaultRosterCodeID.Visible = true;
            WorkHourPatternSatDefaultRosterCodeID.Visible = true;
            if (obj.WorkHourPatternUseStatutoryHolidayTable)
                WorkHourPatternStatutoryHolidayDefaultRosterCodeID.Visible = true;
            if (obj.WorkHourPatternUsePublicHolidayTable)
                WorkHourPatternPublicHolidayDefaultRosterCodeID.Visible = true;

        }
        else if (obj.WorkHourPatternWorkDayDetermineMethod.Equals(EWorkHourPattern.WORKDAYDETERMINDMETHOD_MANUALINPUT))
        {
            WorkHourPerDayRow.Visible = true;
            LunchTimePerDayRow.Visible = true;

            lblDayUnitHeader.Visible = true;
            WorkHourPatternSunDefaultDayUnit.Visible = true;
            WorkHourPatternMonDefaultDayUnit.Visible = true;
            WorkHourPatternTueDefaultDayUnit.Visible = true;
            WorkHourPatternWedDefaultDayUnit.Visible = true;
            WorkHourPatternThuDefaultDayUnit.Visible = true;
            WorkHourPatternFriDefaultDayUnit.Visible = true;
            WorkHourPatternSatDefaultDayUnit.Visible = true;

            WorkHoursPerDayHeaderCell.Visible = true;
            LunchTimeHoursPerDayHeaderCell.Visible = true;
            WorkHoursPerDaySunCell.Visible = true;
            LunchTimeHoursPerDaySunCell.Visible = true;
            WorkHoursPerDayMonCell.Visible = true;
            LunchTimeHoursPerDayMonCell.Visible = true;
            WorkHoursPerDayTueCell.Visible = true;
            LunchTimeHoursPerDayTueCell.Visible = true;
            WorkHoursPerDayWedCell.Visible = true;
            LunchTimeHoursPerDayWedCell.Visible = true;
            WorkHoursPerDayThuCell.Visible = true;
            LunchTimeHoursPerDayThuCell.Visible = true;
            WorkHoursPerDayFriCell.Visible = true;
            LunchTimeHoursPerDayFriCell.Visible = true;
            WorkHoursPerDaySatCell.Visible = true;
            LunchTimeHoursPerDaySatCell.Visible = true;
        }
        else
        {
            WorkHourPerDayRow.Visible = true;
            LunchTimePerDayRow.Visible = true;
            WorkHourPatternDaySettingsPanel.Visible = false;
        }
    }
}
