using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using HROne.DataAccess;
//using perspectivemind.validation;
using HROne.Lib.Entities;

public partial class Payroll_TrialRunAdjust_View : HROneWebPage
{
    private const string TRIALRUN_FUNCTION_CODE = "PAY005";
    private const string HISTORY_FUNCTION_CODE = "PAY009";
    //private string FUNCTION_CODE = string.Empty;

    public Binding binding;
    public DBManager db = EEmpPayroll.db;
    public int CurID = -1;



    //

    //private bool IsAllowEdit = true;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session))
            return;

        if (!ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_USE_ORSO).Equals("Y"))
            Payroll_ORSORecordList.Visible = false;



        binding = new Binding(dbConn, db);
        binding.add(EmpPayrollID);
        binding.add(EmpID);
        binding.add(EmpPayStatus);
        binding.add(EmpPayNumOfDayCount);
        binding.add(EmpPayTotalWorkingHours);
        binding.add(EmpPayRemark);
        binding.add(new LabelBinder(db, lblEmpPayNumOfDayCount, "EmpPayNumOfDayCount"));
        binding.add(new LabelBinder(db, lblEmpPayTotalWorkingHours, "EmpPayTotalWorkingHours"));
        binding.add(new LabelBinder(db, lblEmpPayRemark, "EmpPayRemark"));
        binding.add(EmpPayTrialRunDate);
        binding.add(new BlankZeroLabelVLBinder(db, EmpPayTrialRunBy, EUser.VLUserName));
        binding.add(EmpPayConfirmDate);
        binding.add(new BlankZeroLabelVLBinder(db, EmpPayConfirmBy, EUser.VLUserName));
        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["EmpPayrollID"], out CurID))
            CurID = -1;

        if (CurID <= 0)
        {
            //IsAllowEdit = false;
            Payroll_PaymentRecordList.Visible = false;
            Payroll_MPFRecordList.Visible = false;
            Payroll_ORSORecordList.Visible = false;

        }
        loadState();

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (!Page.IsPostBack)
        {
            if (CurID > 0)
                if (loadObject())
                {
                }
        }
        Payroll_PaymentRecordList.FunctionCode = FUNCTION_CODE.Value;
        Payroll_MPFRecordList.FunctionCode = FUNCTION_CODE.Value;
        Payroll_ORSORecordList.FunctionCode = FUNCTION_CODE.Value;
    }

    protected void RefreshMinimumWagesInfo()
    {
        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpPayrollID", CurID));
        filter.add(WebUtils.AddRankFilter(Session, "EmpID", true));
        ArrayList empPayrollList = EEmpPayroll.db.select(dbConn, filter);
        if (empPayrollList.Count == 0)
        {
            TotalWagesForMinimumWages.Text = string.Empty;
            MinimumWagesRequired.Text = string.Empty;
            return;
        }
        EEmpPayroll empPayroll = (EEmpPayroll)empPayrollList[0];

        double dblTotalWagesForMinimumWages = 0;
        double dblMinimumWagesRequired = 0;

        EPayrollPeriod payPeriod = new EPayrollPeriod();
        payPeriod.PayPeriodID = empPayroll.PayPeriodID;
        if (EPayrollPeriod.db.select(dbConn, payPeriod))
            dblTotalWagesForMinimumWages = HROne.Payroll.PayrollProcess.GetTotalWagesWithoutRestDayPayment(dbConn, empPayroll.EmpID, payPeriod.PayPeriodFr, payPeriod.PayPeriodTo, null);

        double totalWorkingHour = HROne.Payroll.PayrollProcess.GetTotalEmpPayrollWorkingHours(dbConn, empPayroll.EmpID, empPayroll.PayPeriodID);
        lblTotalWorkingHours.Text = totalWorkingHour.ToString();


        dblMinimumWagesRequired = ((double)(HROne.Payroll.PayrollProcess.GetMinimumWages(dbConn, empPayroll.EmpID, payPeriod.PayPeriodTo) * totalWorkingHour));
        TotalWagesForMinimumWages.Text = dblTotalWagesForMinimumWages.ToString("#,##0.00");
        MinimumWagesRequired.Text = dblMinimumWagesRequired.ToString("#,##0.00");
        if (dblMinimumWagesRequired - dblTotalWagesForMinimumWages <= 0.01)
            btnAddAdditionalRemuneration.Visible = false;
        else
        {
            EPayrollGroup payGroup = new EPayrollGroup();
            payGroup.PayGroupID = payPeriod.PayGroupID;
            if (EPayrollGroup.db.select(dbConn, payGroup))
                if (payGroup.PayGroupAdditionalRemunerationPayCodeID > 0)
                    btnAddAdditionalRemuneration.Visible = true & toolBar.EditButton_Visible;
                else
                    btnAddAdditionalRemuneration.Visible = false;
            else
                btnAddAdditionalRemuneration.Visible = false;


        }
    }
    public void loadState()
    {
    }
    protected bool loadObject()
    {
        EEmpPayroll obj = new EEmpPayroll();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
        
        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpPayrollID", obj.EmpPayrollID));
        filter.add(WebUtils.AddRankFilter(Session, "EmpID", true));
        ArrayList empPayrollList = EEmpPayroll.db.select(dbConn, filter);
        if (empPayrollList.Count == 0)
        {
            //IsAllowEdit = false;

            Payroll_PaymentRecordList.Visible = false;
            Payroll_MPFRecordList.Visible = false;
            Payroll_ORSORecordList.Visible = false;

            return false;
        }
        obj = (EEmpPayroll)empPayrollList[0];

        if (obj.EmpPayNumOfDayCount > 0)
            OldNumOfDayContPanel.Visible = true;
        else
            OldNumOfDayContPanel.Visible = false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);
        ucEmp_Header.CurrentEmpID = obj.EmpID;
        ucPayroll_PeriodInfo.CurrentPayPeriodID = obj.PayPeriodID;
        Payroll_PaymentRecordList.CurrentEmpPayrollID = obj.EmpPayrollID;
        Payroll_PaymentRecordList.CurrentEmpPayStatus = obj.EmpPayStatus;

        Payroll_MPFRecordList.CurrentEmpPayrollID = obj.EmpPayrollID;
        Payroll_MPFRecordList.CurrentEmpPayStatus = obj.EmpPayStatus;

        Payroll_ORSORecordList.CurrentEmpPayrollID = obj.EmpPayrollID;
        Payroll_ORSORecordList.CurrentEmpPayStatus = obj.EmpPayStatus;
        if (obj.EmpPayStatus == "C")
        {
            Payroll_PaymentRecordList.IsTrialMode = false;
            Payroll_MPFRecordList.IsTrialMode = false;
            Payroll_ORSORecordList.IsTrialMode = false;

            FUNCTION_CODE.Value = HISTORY_FUNCTION_CODE;
            //Payroll_PaymentRecordList.FunctionCode = HISTORY_FUNCTION_CODE;
            //Payroll_MPFRecordList.FunctionCode = HISTORY_FUNCTION_CODE;
            //Payroll_ORSORecordList.FunctionCode = HISTORY_FUNCTION_CODE;

            Title.Text = HROne.Common.WebUtility.GetLocalizedStringByCode(HISTORY_FUNCTION_CODE, "Payroll History Enquiry/Adjustment");
            SubTitle.Text = HROne.Common.WebUtility.GetLocalizedString("Payroll History Information");
            if (!WebUtils.CheckAccess(Response, Session, HISTORY_FUNCTION_CODE, WebUtils.AccessLevel.Read))
                return false;
            if (!WebUtils.CheckPermission(Session, HISTORY_FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            {
                //IsAllowEdit = false;
                Payroll_PaymentRecordList.IsAllowEdit = false;
                Payroll_MPFRecordList.IsAllowEdit = false;
                Payroll_ORSORecordList.IsAllowEdit = false;
                EmpPayNumOfDayCount.Enabled = false;
                toolBar.EditButton_Visible = false;
            }
        }
        else
        {
            FUNCTION_CODE.Value = TRIALRUN_FUNCTION_CODE;

            Payroll_PaymentRecordList.IsTrialMode = true;
            Payroll_MPFRecordList.IsTrialMode = true;
            Payroll_ORSORecordList.IsTrialMode = true;

            

            Title.Text = HROne.Common.WebUtility.GetLocalizedStringByCode(TRIALRUN_FUNCTION_CODE, "Payroll Trial Run Adjustment");
            SubTitle.Text = HROne.Common.WebUtility.GetLocalizedString("Payroll Trial Run Information");
            if (!WebUtils.CheckAccess(Response, Session, TRIALRUN_FUNCTION_CODE, WebUtils.AccessLevel.Read))
                return false;
            if (!WebUtils.CheckPermission(Session, TRIALRUN_FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            {
                Payroll_PaymentRecordList.IsAllowEdit = false;
                Payroll_MPFRecordList.IsAllowEdit = false;
                Payroll_ORSORecordList.IsAllowEdit = false;
                EmpPayNumOfDayCount.Enabled = false;
                toolBar.EditButton_Visible = false;
            }
        }
        RefreshMinimumWagesInfo();

        return true;
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        if (FUNCTION_CODE.Value.Equals(TRIALRUN_FUNCTION_CODE))
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_TrialRunAdjust_List.aspx");
        else if (FUNCTION_CODE.Value.Equals(HISTORY_FUNCTION_CODE))
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_HistoryAdjust_List.aspx");
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Default.aspx");
    }

    protected void ChangeEditAction()
    {
        lblEmpPayNumOfDayCount.Visible = false;
        lblEmpPayTotalWorkingHours.Visible = false;
        lblEmpPayRemark.Visible = false;

        EmpPayNumOfDayCount.Visible = true;
        EmpPayTotalWorkingHours.Visible = true;
        EmpPayRemark.Visible = true;

        toolBar.EditButton_Visible = false;
        toolBar.SaveButton_Visible = true;
        toolBar.CustomButton1_Visible = true;
    }

    protected void ChangeSavedCancelAction()
    {
        lblEmpPayNumOfDayCount.Visible = true;
        lblEmpPayTotalWorkingHours.Visible = true;
        lblEmpPayRemark.Visible = true;

        EmpPayNumOfDayCount.Visible = false;
        EmpPayTotalWorkingHours.Visible = false;
        EmpPayRemark.Visible = false;

        toolBar.EditButton_Visible = true;
        toolBar.SaveButton_Visible = false;
        toolBar.CustomButton1_Visible = false;
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        ChangeEditAction();
        loadObject();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ChangeSavedCancelAction();
        loadObject();
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        EEmpPayroll c = new EEmpPayroll();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);

        if (!errors.isEmpty())
            return;

        WebUtils.StartFunction(Session, FUNCTION_CODE.Value, c.EmpID);
        if (CurID < 0)
        {
            //            Utils.MarkCreate(Session, c);
            db.insert(dbConn, c);
            //            url = Utils.BuildURL(-1, CurID);
        }
        else
        {
            //            Utils.Mark(Session, c);
            db.update(dbConn, c);
        }
        WebUtils.EndFunction(dbConn);
        db.select(dbConn, c);

        //errors.addError(HROne.Common.WebUtility.GetLocalizedStringByCode("Updated", "Updated"));
        ChangeSavedCancelAction();
        loadObject();
    }
    protected void Payroll_MPFRecordList_Recalculate(object sender, EventArgs e)
    {
        Payroll_PaymentRecordList.Refresh();
    }

    protected void Payroll_ORSORecordList_Recalculate(object sender, EventArgs e)
    {
        Payroll_PaymentRecordList.Refresh();
    }

    protected void Payroll_PaymentRecordList_Changed(object sender, EventArgs e)
    {
        RefreshMinimumWagesInfo();

    }
    protected void btnAddAdditionalRemuneration_Click(object sender, EventArgs e)
    {
        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpPayrollID", CurID));
        filter.add(WebUtils.AddRankFilter(Session, "EmpID", true));
        ArrayList empPayrollList = EEmpPayroll.db.select(dbConn, filter);
        if (empPayrollList.Count == 0)
        {
            TotalWagesForMinimumWages.Text = string.Empty;
            MinimumWagesRequired.Text = string.Empty;
            return;
        }
        EEmpPayroll empPayroll = (EEmpPayroll)empPayrollList[0];

        WebUtils.StartFunction(Session, FUNCTION_CODE.Value, empPayroll.EmpID);
        HROne.Payroll.PayrollProcess.InsertAdditionalRemuneration(dbConn, empPayroll);
        WebUtils.EndFunction(dbConn);

        Payroll_PaymentRecordList.Refresh();
        RefreshMinimumWagesInfo();
    }
}
