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
using HROne.LeaveCalc;

public partial class Emp_LeaveApplication_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "PER009";

    public Binding binding;
    public DBManager db = ELeaveApplication .db;
    public ELeaveApplication obj;
    public int CurID = -1;
    public int CurEmpID = -1;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        DBFilter LeaveCodeFilter = new DBFilter();
        {
            DBFilter LeaveTypeFilter = new DBFilter();
            LeaveTypeFilter.add(new Match("lt.LeaveTypeIsDisabled", false));
            LeaveCodeFilter.add(new IN("LeaveTypeID", "SELECT lt.LeaveTypeID FROM " + ELeaveType.db.dbclass.tableName + " lt ", LeaveTypeFilter));
        }
        binding = new Binding(dbConn, db);
        binding.add(LeaveAppID);
        binding.add(EmpID);
        binding.add(new DropDownVLBinder(db, LeaveCodeID, ELeaveCode.VLLeaveCode, LeaveCodeFilter));
        binding.add(new DropDownVLBinder(db, LeaveAppUnit, Values.VLLeaveUnit).setNotSelected(null));
        binding.add(new TextBoxBinder(db, LeaveAppDateFrom.TextBox, LeaveAppDateFrom.ID));
        binding.add(new TextBoxBinder(db, LeaveAppDateTo.TextBox, LeaveAppDateTo.ID));
        binding.add(LeaveAppTimeFrom);
        binding.add(LeaveAppTimeTo);
        binding.add(LeaveAppDays);
        binding.add(LeaveAppHours);
        binding.add(LeaveAppRemark);
        binding.add(new CheckBoxBinder(db, LeaveAppHasMedicalCertificate));
        binding.add(new CheckBoxBinder(db, LeaveAppNoPayProcess));
        binding.init(Request, Session);


        if (!int.TryParse(DecryptedRequest["LeaveAppID"], out CurID))
            CurID = -1;

        if (!int.TryParse(DecryptedRequest["EmpID"], out CurEmpID))
            CurEmpID = -1;

        if (CurID > 0)
            ActionHeader.Text = "Edit";
        else
            ActionHeader.Text = "Add";

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        EmpID.Value = CurEmpID.ToString();

        if (!Page.IsPostBack)
        {
            if (CurID > 0)
            {
                loadObject();
            }
            else
                toolBar.DeleteButton_Visible = false;

            RefreshLeaveAppUnit();
        }
        RefreshLeaveCodeID();

    }


    protected bool loadObject() 
    {
	    obj=new ELeaveApplication ();
	    bool isNew=WebFormWorkers.loadKeys(db, obj, DecryptedRequest);

        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpID", CurEmpID));
        filter.add(WebUtils.AddRankFilter(Session, "EmpID", true));
        if (EEmpPersonalInfo.db.count(dbConn, filter) == 0)
            if (CurEmpID <= 0)
                return false;
            else
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");

        if (!db.select(dbConn, obj))
            if (CurID <= 0)
                return false;
            else
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");

        if (obj.EmpID != CurEmpID)
            if (CurID <= 0)
                return false;
            else
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");

	    Hashtable values=new Hashtable();
	    db.populate(obj,values);
	    binding.toControl(values);

        //  Check if the record is payroll process
        if (obj.EmpPaymentID != 0 || obj.EmpPayrollID != 0)
        {
            toolBar.DeleteButton_Visible = false;
            //toolBar.SaveButton_Visible = false;
//            toolBar.SaveButton_ClientClick = HROne.Translation.PromptMessage.LEAVEAPP_FORCE_EDIT_JAVASCRIPT;
            toolBar.SaveButton_ClientClick = HROne.Translation.PromptMessage.CreateConfirmDialogJavascript(HROne.Common.WebUtility.GetLocalizedStringByCode("LEAVEAPP_FORCE_EDIT_JAVASCRIPT", "The leave application is payroll processed.\r\nYou need to adjust the payroll record manually.\r\nAre you sure to save?"), toolBar.FindControl("SaveButton"));
        }
        if (obj.LeaveAppCancelID > 0)
        {
            toolBar.DeleteButton_Visible = false;
            toolBar.SaveButton_Visible = false;
        }
        CurEmpID = obj.EmpID;
        return true;
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        ELeaveApplication  c = new ELeaveApplication ();

        if (!LeaveAppUnit.SelectedValue.Equals("D"))
            LeaveAppDateTo.Value = LeaveAppDateFrom.Value;
        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);

        if (!c.LeaveAppUnit.Equals("D"))
        {
            //c.LeaveAppDateTo = c.LeaveAppDateFrom;
            if (c.LeaveAppTimeTo < c.LeaveAppTimeFrom)
                errors.addError(HROne.Translation.PageErrorMessage.ERROR_INVALID_HOUR);
        }
        else
        {
            EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
            empInfo.EmpID = c.EmpID;
            if (EEmpPersonalInfo.db.select(dbConn, empInfo))
            {
                if (c.LeaveAppDateTo < c.LeaveAppDateFrom)
                    errors.addError(HROne.Translation.PageErrorMessage.ERROR_DATE_TO_TOO_EARLY);
                else
                {
                    if (c.LeaveAppDateFrom < empInfo.EmpDateOfJoin)
                        errors.addError(HROne.Translation.PageErrorMessage.ERROR_DATE_TOO_EARLY);
                    EEmpTermination empTerm = EEmpTermination.GetObjectByEmpID(dbConn, c.EmpID);
                    if (empTerm!=null)
                        if (c.LeaveAppDateTo > empTerm.EmpTermLastDate)
                            errors.addError(HROne.Translation.PageErrorMessage.ERROR_DATE_TOO_LATE);
                    TimeSpan ts = c.LeaveAppDateTo.Subtract(c.LeaveAppDateFrom);
                    if (c.LeaveAppDays > ts.Days + 1)
                        errors.addError(HROne.Translation.PageErrorMessage.ERROR_DAYS_TOO_LARGE);
                }

            }
        }

        if(c.LeaveAppDateFrom.Month!=c.LeaveAppDateTo.Month || c.LeaveAppDateFrom.Year!=c.LeaveAppDateTo.Year)
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_LEAVE_APP_NOT_SAME_MTH);

        if (HoursClaimPanel.Visible)
        {
            if (c.LeaveAppHours<=0)
                errors.addError(string.Format( HROne.Translation.PageErrorMessage.ERROR_FIELD_REQUIRED, new string[]{lblLeaveAppHours.Text}));
        }
        //DBFilter overlapCheckingFilter = new DBFilter();
        //overlapCheckingFilter.add(new Match("EmpID", c.EmpID));
        //overlapCheckingFilter.add(new Match("LeaveAppID", "<>", c.LeaveAppID));
        //overlapCheckingFilter.add(new Match("LeaveAppDateFrom", "<=", c.LeaveAppDateTo));
        //overlapCheckingFilter.add(new Match("LeaveAppDateTo", ">=", c.LeaveAppDateFrom));
        //if (c.LeaveAppUnit.Equals("H"))
        //    overlapCheckingFilter.add(new Match("LeaveAppUnit", "D"));

        //if (overlapDailyLeaveAppList.Count > 0)
        //{
        //    errors.addError(HROne.Translation.PageErrorMessage.ERROR_LEAVE_APP_OVERLAP);
        //    foreach (ELeaveApplication overlapDailyLeaveApp in overlapDailyLeaveAppList)
        //    {
        //        ELeaveCode leaveCode = new ELeaveCode();
        //        leaveCode.LeaveCodeID = overlapDailyLeaveApp.LeaveCodeID;
        //        if (ELeaveCode.db.select(dbConn, leaveCode))
        //        {
        //            errors.addError("- " + overlapDailyLeaveApp.LeaveAppDateFrom.ToString("yyyy-MM-dd") + (overlapDailyLeaveApp.LeaveAppDateFrom.Equals(overlapDailyLeaveApp.LeaveAppDateTo) ? "" : " To " + overlapDailyLeaveApp.LeaveAppDateTo.ToString("yyyy-MM-dd")) + " " + leaveCode.LeaveCodeDesc);
        //        }
        //    }
        //}
        //if (c.LeaveAppUnit.Equals("H"))
        //{
        //    DateTime newLeaveAppTimeFrom = c.LeaveAppDateFrom.Date.Add(new TimeSpan(c.LeaveAppTimeFrom.Hour, c.LeaveAppTimeFrom.Minute, c.LeaveAppTimeFrom.Second));
        //    DateTime newLeaveAppTimeTo = c.LeaveAppDateTo.Date.Add(new TimeSpan(c.LeaveAppTimeTo.Hour, c.LeaveAppTimeTo.Minute, c.LeaveAppTimeTo.Second));
        //    while (newLeaveAppTimeFrom > newLeaveAppTimeTo)
        //        newLeaveAppTimeTo.AddDays(1);

        //    overlapCheckingFilter = new DBFilter();
        //    overlapCheckingFilter.add(new Match("EmpID", c.EmpID));
        //    overlapCheckingFilter.add(new Match("LeaveAppID", "<>", c.LeaveAppID));
        //    overlapCheckingFilter.add(new Match("LeaveAppDateFrom", "<=", c.LeaveAppDateTo));
        //    overlapCheckingFilter.add(new Match("LeaveAppDateTo", ">=", c.LeaveAppDateFrom));
        //    overlapCheckingFilter.add(new Match("LeaveAppUnit", "H"));
        //    ArrayList leaveAppList = ELeaveApplication.db.select(dbConn, overlapCheckingFilter);
        //    foreach (ELeaveApplication oldLeaveApp in leaveAppList)
        //    {
        //        DateTime oldLeaveAppTimeFrom = oldLeaveApp.LeaveAppDateFrom.Date.Add(new TimeSpan(oldLeaveApp.LeaveAppTimeFrom.Hour, oldLeaveApp.LeaveAppTimeFrom.Minute, oldLeaveApp.LeaveAppTimeFrom.Second));
        //        DateTime oldLeaveAppTimeTo = oldLeaveApp.LeaveAppDateTo.Date.Add(new TimeSpan(oldLeaveApp.LeaveAppTimeTo.Hour, oldLeaveApp.LeaveAppTimeTo.Minute, oldLeaveApp.LeaveAppTimeTo.Second));
        //        while (oldLeaveAppTimeFrom > oldLeaveAppTimeTo)
        //            oldLeaveAppTimeTo.AddDays(1);

        //        if (!newLeaveAppTimeFrom.Equals(newLeaveAppTimeTo) && !oldLeaveAppTimeFrom.Equals(oldLeaveAppTimeTo))
        //            //  Only check if the following case exists
        //            // Time A From------- Time B From ---------- Time B To ------------Time A To
        //            if (newLeaveAppTimeFrom <= oldLeaveAppTimeFrom && oldLeaveAppTimeTo <= newLeaveAppTimeTo || oldLeaveAppTimeFrom <= newLeaveAppTimeFrom && newLeaveAppTimeTo <= oldLeaveAppTimeTo)
        //                errors.addError(HROne.Translation.PageErrorMessage.ERROR_LEAVE_APP_TIME_OVERLAP);
        //    }
        //}
        ArrayList overlapLeaveAppList = new ArrayList();
        if (c.IsOverlapLeaveApplication(dbConn, out overlapLeaveAppList))
        {
            string strDailyOverlapMessage = string.Empty;
            string strHourlyOverlapMessage = string.Empty;

            foreach (ELeaveApplication overlapLeaveApp in overlapLeaveAppList)
            {
                if (overlapLeaveApp.LeaveAppUnit.Equals("D") || c.LeaveAppUnit.Equals("D"))
                {
                    if (string.IsNullOrEmpty(strDailyOverlapMessage))
                        strDailyOverlapMessage = HROne.Translation.PageErrorMessage.ERROR_LEAVE_APP_OVERLAP;
                    ELeaveCode errorLeaveCode = new ELeaveCode();
                    errorLeaveCode.LeaveCodeID = overlapLeaveApp.LeaveCodeID;
                    if (ELeaveCode.db.select(dbConn, errorLeaveCode))
                    {
                        // Start 0000201, Ricky So, 2015-06-02
                        //strDailyOverlapMessage += "\n- " + overlapLeaveApp.LeaveAppDateFrom.ToString("yyyy-MM-dd") +
                        //                                   (overlapLeaveApp.LeaveAppDateFrom.Equals(overlapLeaveApp.LeaveAppDateTo) ? "" : " To " +
                        //                                   overlapLeaveApp.LeaveAppDateTo.ToString("yyyy-MM-dd")) + " " +
                        //                                   errorLeaveCode.LeaveCodeDesc;
                        strDailyOverlapMessage += "\n- " + overlapLeaveApp.LeaveAppDateFrom.ToString("yyyy-MM-dd");

                        if (overlapLeaveApp.LeaveAppUnit == "A")
                            strDailyOverlapMessage += "AM";
                        else if (overlapLeaveApp.LeaveAppUnit == "P")
                            strDailyOverlapMessage += "PM"; 
                        else if (overlapLeaveApp.LeaveAppUnit == "D" && !string.IsNullOrEmpty(overlapLeaveApp.LeaveAppDateFromAM))
                            strDailyOverlapMessage += overlapLeaveApp.LeaveAppDateFromAM;
                        else

                        if (overlapLeaveApp.LeaveAppDateFrom.Equals(overlapLeaveApp.LeaveAppDateTo))
                            strDailyOverlapMessage += " To " + overlapLeaveApp.LeaveAppDateTo.ToString("yyyy-MM-dd");

                        if (overlapLeaveApp.LeaveAppUnit == "D" && !string.IsNullOrEmpty(overlapLeaveApp.LeaveAppDateToAM))
                            strDailyOverlapMessage += overlapLeaveApp.LeaveAppDateToAM;
                        else


                        strDailyOverlapMessage += "   " + errorLeaveCode.LeaveCodeDesc;
                        // End 0000201, Ricky So, 2015-06-02
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(strHourlyOverlapMessage))
                        strHourlyOverlapMessage = HROne.Translation.PageErrorMessage.ERROR_LEAVE_APP_TIME_OVERLAP;
                }
            }

            if (!string.IsNullOrEmpty(strDailyOverlapMessage))
                errors.addError(strDailyOverlapMessage);
            if (!string.IsNullOrEmpty(strHourlyOverlapMessage))
                errors.addError(strHourlyOverlapMessage);
        }

        ELeaveCode leaveCode = new ELeaveCode();
        leaveCode.LeaveCodeID = c.LeaveCodeID;
        if (ELeaveCode.db.select(dbConn, leaveCode))
        {
            if (leaveCode.LeaveCodeIsSkipPayrollProcess)
                c.LeaveAppNoPayProcess = true;
        }

        if (!errors.isEmpty())
            return;

        WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
        //LeaveBalanceCalc leaaveBalCal = new LeaveBalanceCalc(dbConn, c.EmpID);

        if (CurID < 0)
        {
//            Utils.MarkCreate(Session, c);

            db.insert(dbConn, c);
            CurID = c.LeaveAppID;
            //leaaveBalCal.RecalculateAfter(c.LeaveAppDateFrom, leaveCode.LeaveTypeID);
//            url = Utils.BuildURL(-1, CurID);
        }
        else
        {
//            Utils.Mark(Session, c);
            //ELeaveApplication leaveBalApp = new ELeaveApplication();
            //leaveBalApp.LeaveAppID = CurID;
            //db.select(dbConn, leaveBalApp);
            db.update(dbConn, c);
            //leaaveBalCal.RecalculateAfter(leaveBalApp.LeaveAppDateFrom < c.LeaveAppDateFrom ? leaveBalApp.LeaveAppDateFrom : c.LeaveAppDateFrom);

        }
        WebUtils.EndFunction(dbConn);


        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_LeaveApplication_View.aspx?EmpID="+c.EmpID+"&LeaveAppID="+CurID);


    }
    protected void Delete_Click(object sender, EventArgs e)
    {

        ELeaveApplication c = new ELeaveApplication();
        c.LeaveAppID = CurID;
        db.select(dbConn, c);
        WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
        db.delete(dbConn, c);
        WebUtils.EndFunction(dbConn);
        //LeaveBalanceCalc leaaveBalCal = new LeaveBalanceCalc(dbConn, c.EmpID);
        //leaaveBalCal.RecalculateAfter(c.LeaveAppDateFrom, ELeaveType.COMPENSATION_LEAVE_TYPE(dbConn).LeaveTypeID);

        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_LeaveApplication_View.aspx?EmpID=" + EmpID.Value);
    }
    protected void LeaveAppUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        RefreshLeaveAppUnit();
        btnEstimateTotalLeaveDay_Click(sender, e);
    }

    protected void RefreshLeaveAppUnit()
    {
        if (LeaveAppUnit.SelectedValue.Equals("D"))
        {
            TimeRow.Visible = false;
            LeaveAppDateToPlaceHolder.Visible = true;
            btnEstimateTotalLeaveDay.Visible = true;
        }
        else
        {
            TimeRow.Visible = true;
            LeaveAppDateToPlaceHolder.Visible = false;
            if (LeaveAppUnit.SelectedValue.Equals("A") || LeaveAppUnit.SelectedValue.Equals("P"))
                btnEstimateTotalLeaveDay.Visible = false;
            else
                btnEstimateTotalLeaveDay.Visible = true;
        }
    }

    protected void RefreshLeaveCodeID()
    {
        HoursClaimPanel.Visible = false;
        if (!string.IsNullOrEmpty(LeaveCodeID.SelectedValue))
        {
            int tmpLeaveCodeID = 0;
            if (int.TryParse(LeaveCodeID.SelectedValue, out tmpLeaveCodeID))
            {
                ELeaveCode leaveCode = new ELeaveCode();
                leaveCode.LeaveCodeID = tmpLeaveCodeID;
                if (ELeaveCode.db.select(dbConn, leaveCode))
                {
                    PayrollProcessPanel.Visible = !leaveCode.LeaveCodeIsSkipPayrollProcess;
                    LeaveCodeIsShowMedicalCertOptionPanel.Visible = leaveCode.LeaveCodeIsShowMedicalCertOption;
                    if (!leaveCode.LeaveCodeIsSkipPayrollProcess)
                    {
                        ELeaveApplication leaveApp = new ELeaveApplication();
                        leaveApp.LeaveAppID = CurID;
                        if (ELeaveApplication.db.select(dbConn, leaveApp))
                        {
                            ELeaveCode prevLeaveCode = new ELeaveCode();
                            prevLeaveCode.LeaveCodeID = leaveApp.LeaveCodeID;
                            if (ELeaveCode.db.select(dbConn, prevLeaveCode))
                                if (prevLeaveCode.LeaveCodeIsSkipPayrollProcess)
                                    LeaveAppNoPayProcess.Checked = false;
                        }
                    }

                    if (leaveCode.LeaveTypeID.Equals(ELeaveType.COMPENSATION_LEAVE_TYPE(dbConn).LeaveTypeID))
                        HoursClaimPanel.Visible = true;
                    else
                        HoursClaimPanel.Visible = false;
                }

            }
        }
        if (HoursClaimPanel.Visible)
        {
            LeaveAppDays.AutoPostBack = true;
            LeaveAppDays.TextChanged += new EventHandler(LeaveAppDays_TextChanged);
        }
        else
        {
            LeaveAppDays.AutoPostBack = false;
            LeaveAppDays.TextChanged -= new EventHandler(LeaveAppDays_TextChanged);
        }
    }


    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_LeaveApplication_View.aspx?EmpID=" + EmpID.Value + "&LeaveAppID=" + CurID);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_LeaveApplication_View.aspx?EmpID=" + EmpID.Value);

    }
    protected void btnEstimateTotalLeaveDay_Click(object sender, EventArgs e)
    {
        lblStatutoryHolidayList.Text = string.Empty;
        if (LeaveAppUnit.SelectedValue.Equals("D"))
        {
            DateTime dtFrom, dtTo;
            if (DateTime.TryParse(LeaveAppDateFrom.Value, out dtFrom) && DateTime.TryParse(LeaveAppDateTo.Value, out dtTo))
            {
                int intLeaveCodeID = 0;
                try
                {
                    intLeaveCodeID=Convert.ToInt32(LeaveCodeID.SelectedValue);
                }
                catch
                {
                }

                DateTime[] dateSkipArray = null;
                double totalDays = ELeaveApplication.GetEstimatedNumOfLeaveDays(dbConn, CurEmpID, dtFrom, dtTo, intLeaveCodeID, out dateSkipArray);
                LeaveAppDays.Text = totalDays.ToString() ;

                if (dateSkipArray.GetLength(0) > 0)
                {
                    lblStatutoryHolidayList.Text = HROne.Common.WebUtility.GetLocalizedString("Date excluded");
                    foreach (DateTime dateSkip in dateSkipArray)
                    {
                        lblStatutoryHolidayList.Text += "<br/>" + dateSkip.ToString("yyyy-MM-dd");
                    }
                }
                LeaveAppDays_TextChanged(sender, e);
                //double totalDays=Math.Abs(((TimeSpan)dtTo.Subtract(dtFrom)).TotalDays) + 1;

                //DBFilter statutoryHolidayFilter = new DBFilter();
                //statutoryHolidayFilter.add(new Match("StatutoryHolidayDate", "<=", dtFrom < dtTo ? dtTo : dtFrom));
                //statutoryHolidayFilter.add(new Match("StatutoryHolidayDate", ">=", dtFrom < dtTo ? dtFrom : dtTo));
                //statutoryHolidayFilter.add("StatutoryHolidayDate", true);
                //ArrayList statutoryList = EStatutoryHoliday.db.select(dbConn, statutoryHolidayFilter);
                //if (statutoryList.Count > 0)
                //{
                //    lblStatutoryHolidayList.Text = HROne.Common.WebUtility.GetLocalizedString("Statutory Holiday");
                //    foreach (EStatutoryHoliday statHol in statutoryList)
                //    {
                //        lblStatutoryHolidayList.Text += "<br/>" + statHol.StatutoryHolidayDate.ToString("yyyy-MM-dd") + "&nbsp" + statHol.StatutoryHolidayDesc;
                //        totalDays--;
                //    }
                //}
                //LeaveAppDays.Text = totalDays.ToString() ;
            
            }
        }         
        else if (LeaveAppUnit.SelectedValue.Equals("H"))
        {
            LeaveAppTime_TextChanged(sender, e);
        }
        else if (LeaveAppUnit.SelectedValue.Equals("A") || LeaveAppUnit.SelectedValue.Equals("P"))
        {
            LeaveAppDays.Text = "0.5";
            LeaveAppDays_TextChanged(sender, e);
        }


    }
    protected void LeaveCodeID_SelectedIndexChanged(object sender, EventArgs e)
    {
        RefreshLeaveCodeID();
    }
    protected void LeaveAppDays_TextChanged(object sender, EventArgs e)
    {
        DateTime tmpLeaveAppDateFrom;
        double tmpLeaveAppDay = 0;
        double workhour = 0;
        if (!DateTime.TryParse(LeaveAppDateFrom.Value, out tmpLeaveAppDateFrom))
            tmpLeaveAppDateFrom = AppUtils.ServerDateTime().Date;
        if (double.TryParse(LeaveAppDays.Text, out tmpLeaveAppDay) && tmpLeaveAppDay>0)
        {
            EEmpPositionInfo currentEmpPos = AppUtils.GetLastPositionInfo(dbConn, tmpLeaveAppDateFrom, CurEmpID);
            if (currentEmpPos != null)
            {
                EWorkHourPattern workPattern = new EWorkHourPattern();
                workPattern.WorkHourPatternID = currentEmpPos.WorkHourPatternID;
                if (EWorkHourPattern.db.select(dbConn, workPattern))
                    workhour = workPattern.GetDefaultWorkHour(dbConn, tmpLeaveAppDateFrom);
            }
            if (workhour>0)
                LeaveAppHours.Text = ((double)(workhour * tmpLeaveAppDay)).ToString("0.####");
        }
    }

    protected void LeaveAppDateFrom_Changed(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(LeaveAppDateFrom.Value))
        {
            if (string.IsNullOrEmpty(LeaveAppDateTo.Value))
            {
                LeaveAppDateTo.Value = LeaveAppDateFrom.Value;
                LeaveAppDateTo_Changed(sender, e);
            }
            else
            {
                DateTime dtFrom, dtTo;
                if (DateTime.TryParse(LeaveAppDateFrom.Value, out dtFrom) && DateTime.TryParse(LeaveAppDateTo.Value, out dtTo))
                {
                    if (dtFrom > dtTo)
                        LeaveAppDateTo.Value = LeaveAppDateFrom.Value;
                    LeaveAppDateTo_Changed(sender, e);
                }
            }

            // Start 0000120, KuangWei, 2014-12-17        
            DateTime dtLeaveFr;
            if (!DateTime.TryParse(LeaveAppDateFrom.Value, out dtLeaveFr))
                dtLeaveFr = new DateTime();

            Emp_Monthly_LeaveApplication_List1.dtDateFr = dtLeaveFr;
            // End 0000120, KuangWei, 2014-12-17    
        }
    }

    protected void LeaveAppDateTo_Changed(object sender, EventArgs e)
    {
        DateTime dtFrom, dtTo;
        if (DateTime.TryParse(LeaveAppDateFrom.Value, out dtFrom) && DateTime.TryParse(LeaveAppDateTo.Value, out dtTo))
        {
            if (dtFrom <= dtTo)
            {
                btnEstimateTotalLeaveDay_Click(sender, e);
            }
        }
    }
    protected void LeaveAppTime_TextChanged(object sender, EventArgs e)
    {
        if (LeaveAppUnit.SelectedValue.Equals("H"))
        {
            DateTime dtTimeFrom = new DateTime();
            if (!DateTime.TryParseExact(LeaveAppTimeFrom.Text, "HH:mm", null, System.Globalization.DateTimeStyles.None, out dtTimeFrom))
                LeaveAppTimeFrom.Text = string.Empty;

            DateTime dtTimeTo = new DateTime();
            if (!DateTime.TryParseExact(LeaveAppTimeTo.Text, "HH:mm", null, System.Globalization.DateTimeStyles.None, out dtTimeTo))
                LeaveAppTimeTo.Text = string.Empty;

            if (dtTimeFrom.Ticks.Equals(0) || dtTimeTo.Ticks.Equals(0))
                return;

            DateTime tmpLeaveAppDateFrom;
            if (DateTime.TryParse(LeaveAppDateFrom.Value, out tmpLeaveAppDateFrom))
            {
                double workhour = 0;
                EEmpPositionInfo currentEmpPos = AppUtils.GetLastPositionInfo(dbConn, tmpLeaveAppDateFrom, CurEmpID);
                if (currentEmpPos != null)
                {
                    EWorkHourPattern workPattern = new EWorkHourPattern();
                    workPattern.WorkHourPatternID = currentEmpPos.WorkHourPatternID;
                    if (EWorkHourPattern.db.select(dbConn, workPattern))
                        workhour = workPattern.GetDefaultWorkHour(dbConn, tmpLeaveAppDateFrom);
                }
                if (workhour > 0)
                {
                    double timeDiff = ((TimeSpan)dtTimeTo.Subtract(dtTimeFrom)).TotalHours;
                    if (timeDiff < 0) timeDiff += 1;
                    LeaveAppDays.Text = ((double)(timeDiff / workhour)).ToString("0.####");
                    LeaveAppHours.Text = timeDiff.ToString("0.####");
                }
            }
        }
    }
}
