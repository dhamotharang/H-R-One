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
using System.Net.Mail;

public partial class LeaveApplicationForm : HROneWebControl 
{
    public Binding binding;
    public SearchBinding sbinding;
    public DBManager db = ERequestLeaveApplication.db;

    public ERequestLeaveApplication obj;
    public int CurID = -1;
    public int CurEmpID = -1;

    public static AppUtils.NewWFTextList VLLeaveUnit = new AppUtils.NewWFTextList(new string[] { ELeaveApplication.LEAVEUNIT_DAYS, ELeaveApplication.LEAVEUNIT_AM, ELeaveApplication.LEAVEUNIT_PM }, new string[] { "Day", "A.M.", "P.M."});

    //public SmtpClient emailClient = new SmtpClient("mail.hr-plus.com.hk", 1025);
    //public MailMessage message = new MailMessage();
    protected const string ERROR_MESSAGE_BALANCE_NOT_ENOUGH = "Leave balance is not enough for your leave application.\r\nAction Abort!";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session))
            return;
        binding = new Binding(dbConn, db);
        binding.add(EmpID);

        DBFilter LeaveTypeFilter = new DBFilter();
        LeaveTypeFilter.add(new Match("lt.LeaveTypeIsDisabled", false));
        DBFilter leaveCodeFilter = new DBFilter();
        leaveCodeFilter.add(new Match("LeaveCodeHideInESS", false));
        leaveCodeFilter.add(new IN("LeaveTypeID", "SELECT lt.LeaveTypeID FROM " + ELeaveType.db.dbclass.tableName + " lt ", LeaveTypeFilter));

        binding.add(new DropDownVLBinder(db, RequestLeaveCodeID, ELeaveCode.VLLeaveCode, leaveCodeFilter));

        // Start 000053, Ricky So, 2014-06-26
        //binding.add(new DropDownVLBinder(db, RequestLeaveAppUnit, Values.VLLeaveUnit).setNotSelected(null));
        int m_leaveCodeID = -1;
        if (int.TryParse(RequestLeaveCodeID.SelectedValue, out m_leaveCodeID))
        {
            ELeaveCode m_leaveCode = ELeaveCode.GetObject(dbConn, m_leaveCodeID);

            if (m_leaveCode != null && m_leaveCode.LeaveAppUnit.Length > 0)
            {
                string m_keyList = "";
                string m_valueList = "";
                foreach (string m_unit in m_leaveCode.LeaveAppUnit.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    m_keyList += ((m_keyList != "") ? "," : "") + m_unit.Substring(0, 1);
                    m_valueList += ((m_valueList != "") ? "," : "") + m_unit;
                }
                VLLeaveUnit = new AppUtils.NewWFTextList(m_keyList.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries),
                                                         m_valueList.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));

                binding.add(new DropDownVLBinder(db, RequestLeaveAppUnit, VLLeaveUnit).setNotSelected(null));
            }
        }
        else
        {
            binding.add(new DropDownVLBinder(db, RequestLeaveAppUnit, VLLeaveUnit).setNotSelected(null));
        }
        // End 0000053, Ricky So, 2014-06-26



        binding.add(new TextBoxBinder(db, RequestLeaveAppDateFrom.TextBox, RequestLeaveAppDateFrom.ID));
        binding.add(new TextBoxBinder(db, RequestLeaveAppDateTo.TextBox, RequestLeaveAppDateTo.ID));
        binding.add(RequestLeaveAppTimeFrom);
        binding.add(RequestLeaveAppTimeTo);
        binding.add(RequestLeaveDays);
        binding.add(RequestLeaveAppHours);
        binding.add(RequestLeaveAppRemark);
        binding.add(new CheckBoxBinder(db, RequestLeaveAppHasMedicalCertificate));
        binding.init(Request, Session);

        HROne.Common.WebUtility.WebControlsLocalization(Session, this.Controls);

        RequestLeaveCodeID.Items.Remove(RequestLeaveCodeID.Items[0]);

        EESSUser user = WebUtils.GetCurUser(Session);
        if (user != null)
        {
            CurEmpID = user.EmpID;
            EmpID.Value = CurEmpID.ToString();
        }

        if (RequestLeaveAppDateFromAM.Items.Count <= 0)
        {
            RequestLeaveAppDateFromAM.Items.Add(new ListItem("AM", "AM"));
            RequestLeaveAppDateFromAM.Items.Add(new ListItem("PM", "PM"));

            RequestLeaveAppDateFromAM.SelectedIndex = 0;
        }
        if (RequestLeaveAppDateToAM.Items.Count <= 0)
        {
            RequestLeaveAppDateToAM.Items.Add(new ListItem("AM", "AM"));
            RequestLeaveAppDateToAM.Items.Add(new ListItem("PM", "PM"));

            RequestLeaveAppDateToAM.SelectedIndex = 0;
        }
    }

    protected ELeaveType GetLeaveTypeFromLeaveCode(int LeaveCodeID)
    {
        ELeaveCode m_leaveCode = ELeaveCode.GetObject(dbConn, LeaveCodeID);

        ELeaveType m_leaveType = ELeaveType.GetObject(dbConn, m_leaveCode.LeaveTypeID);

        return m_leaveType;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (CurID > 0)
            {
                loadObject();
            }
            //if (RequestLeaveAppUnit.SelectedValue.Equals("A") || RequestLeaveAppUnit.SelectedValue.Equals("P"))
            //    btnEstimateTotalLeaveDay.Visible = false;
            //else
            //    btnEstimateTotalLeaveDay.Visible = true;

            //RefreshLeaveAppUnit();
        }

        TimeRow.Visible = (RequestLeaveAppUnit.SelectedValue != "D");
        LeaveAppDateToPlaceHolder.Visible = (RequestLeaveAppUnit.SelectedValue == "D");

        // Start 0000201, Ricky So, 2015-05-28
        RequestLeaveAppDateFromAM.Visible = (RequestLeaveAppUnit.SelectedValue == "D");
        RequestLeaveAppDateToAM.Visible = (RequestLeaveAppUnit.SelectedValue == "D");
        // End 0000201, Ricky So, 2015-05-28

        int m_leaveCodeID;
        if (int.TryParse(RequestLeaveCodeID.SelectedValue, out m_leaveCodeID))
        {
            ELeaveType m_leaveType = GetLeaveTypeFromLeaveCode(m_leaveCodeID);

            if (m_leaveType != null)
            {
                btnEstimateTotalLeaveDay.Visible = (RequestLeaveAppUnit.SelectedValue == "D" || 
                                                    RequestLeaveAppUnit.SelectedValue == "H" || 
                                                    m_leaveType.LeaveType == ELeaveType.LEAVETYPECODE_COMPENSATION);
                HoursClaimPanel.Visible = (m_leaveType.LeaveType == ELeaveType.LEAVETYPECODE_COMPENSATION);
            }
        }


        if (HoursClaimPanel.Visible)
        {
            RequestLeaveDays.AutoPostBack = true;
            RequestLeaveDays.TextChanged += new EventHandler(LeaveAppDays_TextChanged);
        }
        else
        {
            RequestLeaveDays.AutoPostBack = false;
            RequestLeaveDays.TextChanged -= new EventHandler(LeaveAppDays_TextChanged);
        }
        btnEstimateTotalLeaveDay_Click(sender, e);
    }
    protected bool loadObject()
    {
        obj = new ERequestLeaveApplication();
        bool isNew = WebFormWorkers.loadKeys(db, obj, Request);
        if (!db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);

        RequestLeaveAppUnit.SelectedValue = obj.RequestLeaveAppUnit;

        //CurEmpID = obj.EmpID;
        return true;
    }

    protected void LeaveAppUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        //RefreshLeaveAppUnit();
        btnEstimateTotalLeaveDay_Click(sender, e);
    }

    protected void RefreshLeaveAppUnit()
    {
        if (RequestLeaveAppUnit.SelectedValue.Equals("D"))
        {
            TimeRow.Visible = false;
            LeaveAppDateToPlaceHolder.Visible = true;
            btnEstimateTotalLeaveDay.Visible = true;
        }
        else
        {
            TimeRow.Visible = true;
            LeaveAppDateToPlaceHolder.Visible = false;
            if (RequestLeaveAppUnit.SelectedValue.Equals("A") || RequestLeaveAppUnit.SelectedValue.Equals("P"))
                btnEstimateTotalLeaveDay.Visible = false;
            else
                btnEstimateTotalLeaveDay.Visible = true;
        }
    }

    // Start 0000008, Ricky So, 2014-11-21
    protected void GetPendingLeaveBalance(int pEmpID, int pLeaveTypeID, out double pHoursOut, out double pDaysOut)
    {
        DBFilter m_EmpRequestFilter = new DBFilter();
        m_EmpRequestFilter.add(new Match("EmpRequestType", "EELEAVEAPP"));
        m_EmpRequestFilter.add(new Match("EmpID", pEmpID));
        m_EmpRequestFilter.add(new Match("EmpRequestStatus", "Submitted"));

        DBFilter m_RequestLeaveApplicationFilter = new DBFilter();
        m_RequestLeaveApplicationFilter.add(new IN("RequestLeaveAppID", "SELECT EmpRequestRecordID FROM EmpRequest", m_EmpRequestFilter));

        DBFilter m_leaveCodeFilter = new DBFilter();
        m_leaveCodeFilter.add(new Match("LeaveTypeID", pLeaveTypeID));

        m_RequestLeaveApplicationFilter.add(new IN("RequestLeaveCodeID", "SELECT LeaveCodeID FROM LeaveCode", m_leaveCodeFilter));

        pHoursOut = 0;
        pDaysOut = 0;
        foreach (ERequestLeaveApplication m_requestLeaveApp in ERequestLeaveApplication.db.select(dbConn, m_RequestLeaveApplicationFilter))
        {
            if (m_requestLeaveApp.RequestLeaveAppUnit == "H")
            {
                pHoursOut += m_requestLeaveApp.RequestLeaveAppHours;
            } else
            {
                pDaysOut += m_requestLeaveApp.RequestLeaveDays;
            }
        }
    }
    // End 0000008, Ricky So, 2014-11-21

    protected void Save_Click(object sender, EventArgs e)
    {
        // speciailzed for TakYue only
        double m;
        try
        {
            m = Convert.ToDouble(RequestLeaveDays.Text);
        }
        catch 
        {
            PageErrors invalideInputError = PageErrors.getErrors(db, Page);
            invalideInputError.clear();
            invalideInputError.addError("Invalid Days Taken value.");
            return;
        }

        if (!applicableDays.Value.Equals("") && !applicableDays.Value.Contains(m.ToString("0.00")))
        {
            PageErrors invalideInputError = PageErrors.getErrors(db, Page);
            invalideInputError.clear();
            invalideInputError.addError("Days Taken must be in this range : " + applicableDays.Value);
            return;
        }
        // end specializations

        ERequestLeaveApplication c = new ERequestLeaveApplication();

        if (!RequestLeaveAppUnit.SelectedValue.Equals("D"))
            RequestLeaveAppDateTo.Value = RequestLeaveAppDateFrom.Value;

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);

        if (c.RequestLeaveAppUnit.Equals("D"))
        {
            // the following 2 fields are not bind to controls
            c.RequestLeaveAppDateFromAM = this.RequestLeaveAppDateFromAM.SelectedValue;
            c.RequestLeaveAppDateToAM = this.RequestLeaveAppDateToAM.SelectedValue;
        }

        if (!c.RequestLeaveAppUnit.Equals("D"))
        {
            //c.RequestLeaveAppDateTo = c.RequestLeaveAppDateFrom;
            if (c.RequestLeaveAppTimeTo < c.RequestLeaveAppTimeFrom)
                errors.addError("RequestLeaveAppTimeFrom", "Invald hours");
        }
        else
        {
            if (c.RequestLeaveAppDateTo < c.RequestLeaveAppDateFrom)
                errors.addError("RequestLeaveAppDateFrom", "Date To cannot be earlier than Date From");
            // Start 0000201, Ricky So, 2015-05-29
            else if (c.RequestLeaveAppDateTo == c.RequestLeaveAppDateFrom)
            {
                if (c.RequestLeaveAppDateFromAM.CompareTo(c.RequestLeaveAppDateToAM) > 0)
                    errors.addError("RequestLeaveAppDateFrom", "Date To cannot be earlier/equal than Date From");
                else if (c.RequestLeaveAppDateFromAM.CompareTo(c.RequestLeaveAppDateToAM) == 0)
                {
                    // Convert to Half Day application if only AM/PM is applied
                    c.RequestLeaveAppUnit = c.RequestLeaveAppDateFromAM.Substring(0, 1);
                    c.RequestLeaveAppDateFromAM = null;
                    c.RequestLeaveAppDateToAM = null;
                    c.RequestLeaveDays = 0.5;
                }
            }
            // End 0000201, Ricky So, 2015-05-29
            else
            {
                TimeSpan ts = c.RequestLeaveAppDateTo.Subtract(c.RequestLeaveAppDateFrom);
                if (c.RequestLeaveDays > ts.Days + 1)
                    errors.addError("RequestLeaveDays", "Days taken is too large");
            }
        }
        if (c.RequestLeaveAppDateFrom.Month != c.RequestLeaveAppDateTo.Month || c.RequestLeaveAppDateFrom.Year != c.RequestLeaveAppDateTo.Year)
            errors.addError("Leave application must be within the same month");

        if (HoursClaimPanel.Visible)
        {
            if (c.RequestLeaveAppHours <= 0)
                errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_FIELD_REQUIRED, new string[] { lblLeaveAppHours.Text }));
        }

        ELeaveCode curreintLeaveCode = new ELeaveCode();
        curreintLeaveCode.LeaveCodeID = c.RequestLeaveCodeID;
        if (ELeaveCode.db.select(dbConn, curreintLeaveCode))
        {
            ELeaveType leaveType = new ELeaveType();
            leaveType.LeaveTypeID = curreintLeaveCode.LeaveTypeID;
            if (ELeaveType.db.select(dbConn, leaveType)) 
            {
                // Start 0000008, Ricky So, 2014-11-21
                double m_hours = 0;
                double m_days = 0;
                GetPendingLeaveBalance(c.EmpID, leaveType.LeaveTypeID, out m_hours, out m_days);
                // End 0000008, Ricky So, 2014-11-21

                if (leaveType.LeaveTypeIsESSIgnoreEntitlement)
                {
                    HROne.LeaveCalc.LeaveBalanceCalc calc = new HROne.LeaveCalc.LeaveBalanceCalc(dbConn, c.EmpID);
                    HROne.LeaveCalc.LeaveBalanceProcess balanceProcess = calc.getLeaveBalanceProcess(leaveType.LeaveTypeID);
                    // assume as at today (since entitlement is ignored, as-at-date doesn't make any difference
                    balanceProcess.LoadData(AppUtils.ServerDateTime().Date);
                    ELeaveBalance balance = balanceProcess.getLatestLeaveBalance();

                    // Start 0000008, Ricky So, 2014-11-21
                    //if (balance == null
                    //    || (balance.getBalance() - balance.LeaveBalanceEntitled < c.RequestLeaveDays && balanceProcess.BalanceUnit == ELeaveBalance.LeaveBalanceUnit.Day)
                    //    || (balance.getBalance() - balance.LeaveBalanceEntitled < c.RequestLeaveAppHours && balanceProcess.BalanceUnit == ELeaveBalance.LeaveBalanceUnit.Hour)
                    //    )
                    if (balance == null
                        || (balance.getBalance() - m_days - balance.LeaveBalanceEntitled < c.RequestLeaveDays && balanceProcess.BalanceUnit == ELeaveBalance.LeaveBalanceUnit.Day)
                        || (balance.getBalance() - m_hours - balance.LeaveBalanceEntitled < c.RequestLeaveAppHours && balanceProcess.BalanceUnit == ELeaveBalance.LeaveBalanceUnit.Hour)
                        )
                        // Start 0000008, Ricky So, 2014-11-21
                    {
                        errors.addError(ERROR_MESSAGE_BALANCE_NOT_ENOUGH);
                    }
                }
                else if (leaveType.LeaveTypeIsESSRestrictNegativeBalanceAsOfApplicationDateFrom
                    || leaveType.LeaveTypeIsESSRestrictNegativeBalanceAsOfApplicationDateTo
                    || leaveType.LeaveTypeIsESSRestrictNegativeBalanceAsOfEndOfLeaveYear
                    || leaveType.LeaveTypeIsESSRestrictNegativeBalanceAsOfToday)
                {
                    HROne.LeaveCalc.LeaveBalanceCalc calc = new HROne.LeaveCalc.LeaveBalanceCalc(dbConn, c.EmpID);
                    HROne.LeaveCalc.LeaveBalanceProcess balanceProcess = calc.getLeaveBalanceProcess(leaveType.LeaveTypeID);

                    if (leaveType.LeaveTypeIsESSRestrictNegativeBalanceAsOfToday)
                    {
                        balanceProcess.LoadData(AppUtils.ServerDateTime().Date);
                        ELeaveBalance balance = balanceProcess.getLatestLeaveBalance();
                        // Start 0000093, Ricky So, 2014-09-06
                        if (balance == null)
                        {
                            errors.addError(ERROR_MESSAGE_BALANCE_NOT_ENOUGH);
                        }
                        // Start 0000008, Ricky So, 2014-11-21
                        //else if ((balance.getBalance() - c.RequestLeaveDays < -1 * leaveType.LeaveTypeIsESSAllowableAdvanceBalance && balanceProcess.BalanceUnit == ELeaveBalance.LeaveBalanceUnit.Day) ||
                        //          (balance.getBalance() - c.RequestLeaveAppHours < -1 * leaveType.LeaveTypeIsESSAllowableAdvanceBalance && balanceProcess.BalanceUnit == ELeaveBalance.LeaveBalanceUnit.Hour)
                        //         )
                        else if ((balance.getBalance() - m_days - c.RequestLeaveDays < -1 * leaveType.LeaveTypeIsESSAllowableAdvanceBalance && balanceProcess.BalanceUnit == ELeaveBalance.LeaveBalanceUnit.Day) ||
                                  (balance.getBalance() - m_hours - c.RequestLeaveAppHours < -1 * leaveType.LeaveTypeIsESSAllowableAdvanceBalance && balanceProcess.BalanceUnit == ELeaveBalance.LeaveBalanceUnit.Hour)
                                 )
                        // End 0000008, Ricky So, 2014-11-21
                        {
                            errors.addError(ERROR_MESSAGE_BALANCE_NOT_ENOUGH);
                        }

                        //if (balance == null
                        //    || (balance.getBalance() < c.RequestLeaveDays && balanceProcess.BalanceUnit == ELeaveBalance.LeaveBalanceUnit.Day)
                        //    || (balance.getBalance() < c.RequestLeaveAppHours && balanceProcess.BalanceUnit == ELeaveBalance.LeaveBalanceUnit.Hour)
                        //    )
                        //{
                        //    errors.addError(ERROR_MESSAGE_BALANCE_NOT_ENOUGH);
                        //}
                        // End 0000093, Ricky So, 2014-09-06
                    }
                    if (errors.isEmpty() && leaveType.LeaveTypeIsESSRestrictNegativeBalanceAsOfApplicationDateFrom)
                    {
                        balanceProcess.LoadData(c.RequestLeaveAppDateFrom);
                        ELeaveBalance balance = balanceProcess.getLatestLeaveBalance();
                        // Start 0000093, Ricky So, 2014-09-06
                        if (balance == null)
                        {
                            errors.addError(ERROR_MESSAGE_BALANCE_NOT_ENOUGH);
                        }
                        // Start 0000008, Ricky So, 2014-11-21
                        //else if ((balance.getBalance() - c.RequestLeaveDays < -1 * leaveType.LeaveTypeIsESSAllowableAdvanceBalance && balanceProcess.BalanceUnit == ELeaveBalance.LeaveBalanceUnit.Day) ||
                        //          (balance.getBalance() - c.RequestLeaveAppHours < -1 * leaveType.LeaveTypeIsESSAllowableAdvanceBalance && balanceProcess.BalanceUnit == ELeaveBalance.LeaveBalanceUnit.Hour)
                        //         )
                        else if ((balance.getBalance() - m_days - c.RequestLeaveDays < -1 * leaveType.LeaveTypeIsESSAllowableAdvanceBalance && balanceProcess.BalanceUnit == ELeaveBalance.LeaveBalanceUnit.Day) ||
                                  (balance.getBalance() - m_hours - c.RequestLeaveAppHours < -1 * leaveType.LeaveTypeIsESSAllowableAdvanceBalance && balanceProcess.BalanceUnit == ELeaveBalance.LeaveBalanceUnit.Hour)
                                 )
                        // End 0000008, Ricky So, 2014-11-21
                        {
                            errors.addError(ERROR_MESSAGE_BALANCE_NOT_ENOUGH);
                        }
                        
                        //if (balance == null
                        //    || (balance.getBalance() < c.RequestLeaveDays && balanceProcess.BalanceUnit == ELeaveBalance.LeaveBalanceUnit.Day)
                        //    || (balance.getBalance() < c.RequestLeaveAppHours && balanceProcess.BalanceUnit == ELeaveBalance.LeaveBalanceUnit.Hour)
                        //    )
                        //{
                        //    errors.addError(ERROR_MESSAGE_BALANCE_NOT_ENOUGH);
                        //}
                        // End 0000093, Ricky So, 2014-09-06

                    }
                    if (errors.isEmpty() && leaveType.LeaveTypeIsESSRestrictNegativeBalanceAsOfApplicationDateTo)
                    {
                        balanceProcess.LoadData(c.RequestLeaveAppDateTo);
                        ELeaveBalance balance = balanceProcess.getLatestLeaveBalance();
                        // Start 0000093, Ricky So, 2014-09-06
                        if (balance == null)
                        {
                            errors.addError(ERROR_MESSAGE_BALANCE_NOT_ENOUGH);
                        }
                        // Start 0000008, Ricky So, 2014-11-21
                        //else if ((balance.getBalance() - c.RequestLeaveDays < -1 * leaveType.LeaveTypeIsESSAllowableAdvanceBalance && balanceProcess.BalanceUnit == ELeaveBalance.LeaveBalanceUnit.Day) ||
                        //          (balance.getBalance() - c.RequestLeaveAppHours < -1 * leaveType.LeaveTypeIsESSAllowableAdvanceBalance && balanceProcess.BalanceUnit == ELeaveBalance.LeaveBalanceUnit.Hour)
                        //         )
                        else if ((balance.getBalance() - m_days - c.RequestLeaveDays < -1 * leaveType.LeaveTypeIsESSAllowableAdvanceBalance && balanceProcess.BalanceUnit == ELeaveBalance.LeaveBalanceUnit.Day) ||
                                  (balance.getBalance() - m_hours - c.RequestLeaveAppHours < -1 * leaveType.LeaveTypeIsESSAllowableAdvanceBalance && balanceProcess.BalanceUnit == ELeaveBalance.LeaveBalanceUnit.Hour)
                                 )
                        // End 0000008, Ricky So, 2014-11-21
                        {
                            errors.addError(ERROR_MESSAGE_BALANCE_NOT_ENOUGH);
                        }
                        //if (balance == null
                        //    || (balance.getBalance() < c.RequestLeaveDays && balanceProcess.BalanceUnit == ELeaveBalance.LeaveBalanceUnit.Day)
                        //    || (balance.getBalance() < c.RequestLeaveAppHours && balanceProcess.BalanceUnit == ELeaveBalance.LeaveBalanceUnit.Hour)
                        //    )
                        //{
                        //    errors.addError(ERROR_MESSAGE_BALANCE_NOT_ENOUGH);
                        //}
                        // End 0000093, Ricky So, 2014-09-06
                    }
                    if (errors.isEmpty() && leaveType.LeaveTypeIsESSRestrictNegativeBalanceAsOfEndOfLeaveYear)
                    {
                        balanceProcess.LoadData(AppUtils.ServerDateTime().Date);
                        ELeaveBalance balance = balanceProcess.getLatestLeaveBalance();

                        if (!balance.LeaveBalanceEffectiveEndDate.Ticks.Equals(0))
                        {
                            balanceProcess.LoadData(balance.LeaveBalanceEffectiveEndDate);
                            balance = balanceProcess.getLatestLeaveBalance();
                        }
                        // Start 0000093, Ricky So, 2014-09-06
                        if (balance == null)
                        {
                            errors.addError(ERROR_MESSAGE_BALANCE_NOT_ENOUGH);
                        }
                        // Start 0000008, Ricky So, 2014-11-21
                        //else if ((balance.getBalance() - c.RequestLeaveDays < -1 * leaveType.LeaveTypeIsESSAllowableAdvanceBalance && balanceProcess.BalanceUnit == ELeaveBalance.LeaveBalanceUnit.Day) ||
                        //          (balance.getBalance() - c.RequestLeaveAppHours < -1 * leaveType.LeaveTypeIsESSAllowableAdvanceBalance && balanceProcess.BalanceUnit == ELeaveBalance.LeaveBalanceUnit.Hour)
                        //         )
                        else if ((balance.getBalance() - m_days - c.RequestLeaveDays < -1 * leaveType.LeaveTypeIsESSAllowableAdvanceBalance && balanceProcess.BalanceUnit == ELeaveBalance.LeaveBalanceUnit.Day) ||
                                  (balance.getBalance() - m_hours - c.RequestLeaveAppHours < -1 * leaveType.LeaveTypeIsESSAllowableAdvanceBalance && balanceProcess.BalanceUnit == ELeaveBalance.LeaveBalanceUnit.Hour)
                                 )
                        // End 0000008, Ricky So, 2014-11-21
                        {
                            errors.addError(ERROR_MESSAGE_BALANCE_NOT_ENOUGH);
                        }
                        //if (balance == null
                        //    || (balance.getBalance() < c.RequestLeaveDays  && balanceProcess.BalanceUnit == ELeaveBalance.LeaveBalanceUnit.Day)
                        //    || (balance.getBalance() < c.RequestLeaveAppHours  && balanceProcess.BalanceUnit == ELeaveBalance.LeaveBalanceUnit.Hour)
                        //    )
                        //{
                        //    errors.addError(ERROR_MESSAGE_BALANCE_NOT_ENOUGH);
                        //}
                        // End 0000093, Ricky So, 2014-09-06
                    }
                }
            }
        }

        if (!errors.isEmpty())
            return;

        ArrayList overlapLeaveAppList = new ArrayList();
        if (c.IsOverlapLeaveApplication(dbConn, out overlapLeaveAppList))
        {
            string strDailyOverlapMessage = string.Empty;
            string strHourlyOverlapMessage = string.Empty;

            foreach (BaseObject overlapLeaveApp in overlapLeaveAppList)
            {
                if (overlapLeaveApp is ELeaveApplication)
                {
                    ELeaveApplication previousLeaveApp = (ELeaveApplication)overlapLeaveApp;
                    if (previousLeaveApp.LeaveAppUnit.Equals("D") || c.RequestLeaveAppUnit.Equals("D"))
                    {
                        if (string.IsNullOrEmpty(strDailyOverlapMessage))
                            strDailyOverlapMessage = "Leave dates cannot overlap with previous leave applications";
                        ELeaveCode leaveCode = new ELeaveCode();
                        leaveCode.LeaveCodeID = previousLeaveApp.LeaveCodeID;
                        if (ELeaveCode.db.select(dbConn, leaveCode))
                        {
                            // Start 0000201, Ricky So, 2015-06-02
                            //strDailyOverlapMessage += "\r\n- " + previousLeaveApp.LeaveAppDateFrom.ToString("yyyy-MM-dd") + (previousLeaveApp.LeaveAppDateFrom.Equals(previousLeaveApp.LeaveAppDateTo) ? "" : " To " + previousLeaveApp.LeaveAppDateTo.ToString("yyyy-MM-dd")) + " " + leaveCode.LeaveCodeDesc;
                            strDailyOverlapMessage += "\r\n- " + previousLeaveApp.LeaveAppDateFrom.ToString("yyyy-MM-dd");

                            if (previousLeaveApp.LeaveAppUnit == "A")
                                strDailyOverlapMessage += "AM";
                            else if (previousLeaveApp.LeaveAppUnit == "P")
                                strDailyOverlapMessage += "PM";
                            else if (previousLeaveApp.LeaveAppUnit == "D" && !string.IsNullOrEmpty(previousLeaveApp.LeaveAppDateFromAM))
                                strDailyOverlapMessage += previousLeaveApp.LeaveAppDateFromAM;
                            else
                                if (previousLeaveApp.LeaveAppDateFrom.Equals(previousLeaveApp.LeaveAppDateTo))
                                    strDailyOverlapMessage += " To " + previousLeaveApp.LeaveAppDateTo.ToString("yyyy-MM-dd");

                            if (previousLeaveApp.LeaveAppUnit == "D" && !string.IsNullOrEmpty(previousLeaveApp.LeaveAppDateToAM))
                                strDailyOverlapMessage += previousLeaveApp.LeaveAppDateToAM;
                            else
                                strDailyOverlapMessage += "   " + leaveCode.LeaveCodeDesc;
                            // End 0000201, Ricky So, 2015-06-02
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(strHourlyOverlapMessage))
                            strHourlyOverlapMessage = "Leave time cannot overlap with previous leave applications";
                    }
                }
                else if (overlapLeaveApp is ERequestLeaveApplication)
                {
                    ERequestLeaveApplication previousRequestLeaveApp = (ERequestLeaveApplication)overlapLeaveApp;

                    if (previousRequestLeaveApp.RequestLeaveAppUnit.Equals("D") || c.RequestLeaveAppUnit.Equals("D"))
                    {
                        if (string.IsNullOrEmpty(strDailyOverlapMessage))
                            strDailyOverlapMessage = "Leave dates cannot overlap with previous leave applications";
                        ELeaveCode leaveCode = new ELeaveCode();
                        leaveCode.LeaveCodeID = previousRequestLeaveApp.RequestLeaveCodeID;
                        if (ELeaveCode.db.select(dbConn, leaveCode))
                        {
                            // Start 0000201, Ricky So, 2015-06-02
                            //strDailyOverlapMessage += "\r\n- " + previousRequestLeaveApp.RequestLeaveAppDateFrom.ToString("yyyy-MM-dd") + (previousRequestLeaveApp.RequestLeaveAppDateFrom.Equals(previousRequestLeaveApp.RequestLeaveAppDateTo) ? "" : " To " + previousRequestLeaveApp.RequestLeaveAppDateTo.ToString("yyyy-MM-dd")) + " " + leaveCode.LeaveCodeDesc;
                            strDailyOverlapMessage += "\r\n- " + previousRequestLeaveApp.RequestLeaveAppDateFrom.ToString("yyyy-MM-dd");

                            if (previousRequestLeaveApp.RequestLeaveAppUnit == "A")
                                strDailyOverlapMessage += "AM";
                            else if (previousRequestLeaveApp.RequestLeaveAppUnit == "P")
                                strDailyOverlapMessage += "PM";
                            else if (previousRequestLeaveApp.RequestLeaveAppUnit == "D" && !string.IsNullOrEmpty(previousRequestLeaveApp.RequestLeaveAppDateFromAM))
                                strDailyOverlapMessage += previousRequestLeaveApp.RequestLeaveAppDateFromAM;
                            else
                                if (previousRequestLeaveApp.RequestLeaveAppDateFrom.Equals(previousRequestLeaveApp.RequestLeaveAppDateTo))
                                    strDailyOverlapMessage += " To " + previousRequestLeaveApp.RequestLeaveAppDateTo.ToString("yyyy-MM-dd");

                            if (previousRequestLeaveApp.RequestLeaveAppUnit == "D" && !string.IsNullOrEmpty(previousRequestLeaveApp.RequestLeaveAppDateToAM))
                                strDailyOverlapMessage += previousRequestLeaveApp.RequestLeaveAppDateToAM;
                            else


                                strDailyOverlapMessage += "   " + leaveCode.LeaveCodeDesc;
                            // End 0000201, Ricky So, 2015-06-02
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(strHourlyOverlapMessage))
                            strHourlyOverlapMessage = "Leave time cannot overlap with previous leave applications";
                    }
                }
            }

            if (!string.IsNullOrEmpty(strDailyOverlapMessage))
                errors.addError(strDailyOverlapMessage);
            if (!string.IsNullOrEmpty(strHourlyOverlapMessage))
                errors.addError(strHourlyOverlapMessage);
        }

        if (!errors.isEmpty())
            return;

        try
        {
            ESSAuthorizationProcess authorization = new ESSAuthorizationProcess(dbConn);
            authorization.SubmitLeaveApplication(c);
        }
        catch (Exception ex)
        {
            errors.addError(ex.Message);

        }

        if (!errors.isEmpty())
            return;
        if (c.RequestLeaveAppHasMedicalCertificate)
        {
            string message = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_LEAVE_MEDICIAL_CERT_ALERT);
            if (!string.IsNullOrEmpty(message))
            {
                message = message.Replace("\r", "\\r").Replace("\n", "\\n");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "leaveAppAlert", "alert(\"" + message + "\"); window.location=\"./ESS_EmpRequestStatus.aspx\";", true);
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorMessage", "popupDialog(\"testing\");", true);

                return;
            }
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/ESS_EmpRequestStatus.aspx");
    }

    protected void btnEstimateTotalLeaveDay_Click(object sender, EventArgs e)
    {
        lblStatutoryHolidayList.Text = string.Empty;
        if (RequestLeaveAppUnit.SelectedValue.Equals("D"))
        {
            DateTime dtFrom, dtTo;

            if (DateTime.TryParse(RequestLeaveAppDateFrom.Value, out dtFrom) && DateTime.TryParse(RequestLeaveAppDateTo.Value, out dtTo))
            {
                int intLeaveCodeID = 0;
                try
                {
                    intLeaveCodeID = Convert.ToInt32(RequestLeaveCodeID.SelectedValue);
                }
                catch
                {
                }

                DateTime[] dateSkipArray = null;
                double totalDays = ELeaveApplication.GetEstimatedNumOfLeaveDays(dbConn, CurEmpID, dtFrom, dtTo, intLeaveCodeID, out dateSkipArray);
                // Start 0000201, Ricky So, 2015-05-28
                if (RequestLeaveAppDateFromAM.Visible == true && RequestLeaveAppDateFromAM.SelectedValue == "PM")
                    totalDays -= 0.5;

                if (RequestLeaveAppDateToAM.Visible == true && RequestLeaveAppDateToAM.SelectedValue == "AM")
                    totalDays -= 0.5;

                if (totalDays < 0)
                    totalDays = 0;
                // End 0000201, Ricky So, 2015-05-28

                RequestLeaveDays.Text = totalDays.ToString();
                
                if (dateSkipArray.GetLength(0) > 0)
                {
                    lblStatutoryHolidayList.Text = HROne.Common.WebUtility.GetLocalizedString("Date excluded");
                    foreach (DateTime dateSkip in dateSkipArray)
                    {
                        lblStatutoryHolidayList.Text += "<br/>" + dateSkip.ToString("yyyy-MM-dd");
                    }
                }
                LeaveAppDays_TextChanged(sender, e);

            }
        }
        else if (RequestLeaveAppUnit.SelectedValue.Equals("H"))
        {
            LeaveAppTime_TextChanged(sender, e);
        }
        else if (RequestLeaveAppUnit.SelectedValue.Equals("A") || RequestLeaveAppUnit.SelectedValue.Equals("P"))
        {
            RequestLeaveDays.Text = "0.5";
            LeaveAppDays_TextChanged(sender, e);
        }
    }

    protected void RequestLeaveCodeID_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (!string.IsNullOrEmpty(RequestLeaveCodeID.SelectedValue))
        {
            int tmpLeaveCodeID = 0;
            if (int.TryParse(RequestLeaveCodeID.SelectedValue, out tmpLeaveCodeID))
            {
                //RequestLeaveAppUnit.Items.Clear();

                ELeaveCode leaveCode = new ELeaveCode();
                leaveCode.LeaveCodeID = tmpLeaveCodeID;
                if (ELeaveCode.db.select(dbConn, leaveCode))
                {
                    LeaveCodeIsShowMedicalCertOptionPanel.Visible = leaveCode.LeaveCodeIsShowMedicalCertOption;
                    // Start 0000151, Ricky So, 2015-01-06
                    RequestLeaveAppHasMedicalCertificate.Checked = leaveCode.LeaveCodeIsShowMedicalCertOption;
                    // RequestLeaveAppHasMedicalCertificate.Checked = true;
                    // End 0000151, Ricky So, 2015-01-06
                }
            }
        }
    }

    protected void LeaveAppDays_TextChanged(object sender, EventArgs e)
    {
        DateTime tmpLeaveAppDateFrom;
        double tmpLeaveAppDay = 0;
        double workhour = 0;
        if (!DateTime.TryParse(RequestLeaveAppDateFrom.Value, out tmpLeaveAppDateFrom))
            tmpLeaveAppDateFrom = AppUtils.ServerDateTime().Date;
        if (double.TryParse(RequestLeaveDays.Text, out tmpLeaveAppDay) && tmpLeaveAppDay > 0)
        {
            EEmpPositionInfo currentEmpPos = AppUtils.GetLastPositionInfo(dbConn, tmpLeaveAppDateFrom, CurEmpID);
            if (currentEmpPos != null)
            {
                EWorkHourPattern workPattern = new EWorkHourPattern();
                workPattern.WorkHourPatternID = currentEmpPos.WorkHourPatternID;
                if (EWorkHourPattern.db.select(dbConn, workPattern))
                    workhour = workPattern.GetDefaultWorkHour(dbConn, tmpLeaveAppDateFrom);
            }
            if (workhour > 0)
                RequestLeaveAppHours.Text = ((double)(workhour * tmpLeaveAppDay)).ToString("0.####");
        }
    }

    protected void LeaveAppDateFrom_Changed(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(RequestLeaveAppDateFrom.Value))
        {
            if (string.IsNullOrEmpty(RequestLeaveAppDateTo.Value))
            {
                // Start 0000201, Ricky So, 2015-05-28
                //RequestLeaveAppDateTo.Value = RequestLeaveAppDateFrom.Value;

                DateTime m_fromDate;

                if (DateTime.TryParse(RequestLeaveAppDateFrom.Value, out m_fromDate))
                {
                    if (RequestLeaveAppDateFromAM.SelectedValue == "AM")
                    {
                        RequestLeaveAppDateToAM.SelectedValue = "PM";
                        RequestLeaveAppDateTo.Value = RequestLeaveAppDateFrom.Value;
                    }
                    else
                    {
                        RequestLeaveAppDateToAM.SelectedValue = "AM"; 
                        RequestLeaveAppDateTo.Value = m_fromDate.AddDays(1).ToString("yyyy-MM-dd");
                    }
                }
                // End 0000201, Ricky So, 2015-05-28
                //LeaveAppDateTo_Changed(sender, e);
            }
            else
            {
                DateTime dtFrom, dtTo;
                if (DateTime.TryParse(RequestLeaveAppDateFrom.Value, out dtFrom) && DateTime.TryParse(RequestLeaveAppDateTo.Value, out dtTo))
                {
                    if (dtFrom > dtTo)
                        RequestLeaveAppDateTo.Value = RequestLeaveAppDateFrom.Value;
                    //LeaveAppDateTo_Changed(sender, e);
                }
            }
        }
    }

    protected void LeaveAppDateTo_Changed(object sender, EventArgs e)
    {
        DateTime dtFrom, dtTo;
        if (DateTime.TryParse(RequestLeaveAppDateFrom.Value, out dtFrom) && DateTime.TryParse(RequestLeaveAppDateTo.Value, out dtTo))
        {
            if (dtFrom <= dtTo)
            {
                btnEstimateTotalLeaveDay_Click(sender, e);
            }
        }
    }

    protected void LeaveAppTime_TextChanged(object sender, EventArgs e)
    {
        if (RequestLeaveAppUnit.SelectedValue.Equals("H"))
        {
            DateTime dtTimeFrom = new DateTime();
            if (!DateTime.TryParseExact(RequestLeaveAppTimeFrom.Text, "HH:mm", null, System.Globalization.DateTimeStyles.None, out dtTimeFrom))
                RequestLeaveAppTimeFrom.Text = string.Empty;

            DateTime dtTimeTo = new DateTime();
            if (!DateTime.TryParseExact(RequestLeaveAppTimeTo.Text, "HH:mm", null, System.Globalization.DateTimeStyles.None, out dtTimeTo))
                RequestLeaveAppTimeTo.Text = string.Empty;

            if (dtTimeFrom.Ticks.Equals(0) || dtTimeTo.Ticks.Equals(0))
                return;

            DateTime tmpLeaveAppDateFrom;
            if (DateTime.TryParse(RequestLeaveAppDateFrom.Value, out tmpLeaveAppDateFrom))
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
                    RequestLeaveDays.Text = ((double)(timeDiff / workhour)).ToString("0.####");
                    RequestLeaveAppHours.Text = timeDiff.ToString("0.####");
                }
            }
        }
    }
}
