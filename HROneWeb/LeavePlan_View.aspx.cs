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
using HROne.LeaveCalc;

public partial class LeavePlan_View : HROneWebPage
{
    private const string FUNCTION_CODE = "LEV003";

    public Binding binding;
    public DBManager db = ELeavePlan .db;
    public ELeavePlan obj;
    public int CurID = -1;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;



        binding = new Binding(dbConn, db);
        binding.add(LeavePlanID);
        binding.add(LeavePlanCode);
        binding.add(LeavePlanDesc);
        binding.add(new LabelVLBinder(db, LeavePlanUseCommonLeaveYear, Values.VLTrueFalseYesNo));
        binding.add(LeavePlanCommonLeaveYearStartDay);
        binding.add(new BlankZeroLabelVLBinder(db, LeavePlanCommonLeaveYearStartMonth, Values.VLMonth));
        binding.add(new BlankZeroLabelVLBinder(db, ALProrataRoundingRuleID, EALProrataRoundingRule.VLALProrataRoundingRule));
        binding.add(new LabelVLBinder(db, LeavePlanProrataSkipFeb29, Values.VLTrueFalseYesNo));
        binding.add(new LabelVLBinder(db, LeavePlanResetYearOfService, Values.VLTrueFalseYesNo));
        binding.add(new LabelVLBinder(db, LeavePlanNoCountFirstIncompleteYearOfService, ELeavePlan.VLYearOfServiceReferenceMethod));
        binding.add(new LabelVLBinder(db, LeavePlanComparePreviousLeavePlan, Values.VLTrueFalseYesNo));
        binding.add(LeavePlanLeavePlanCompareRank);

        binding.add(new LabelVLBinder(db, LeavePlanUseStatutoryHolidayEntitle, Values.VLTrueFalseYesNo));
        binding.add(new LabelVLBinder(db, LeavePlanUsePublicHolidayEntitle, Values.VLTrueFalseYesNo));
        binding.add(new LabelVLBinder(db, LeavePlanUseRestDayEntitle, Values.VLTrueFalseYesNo));
        binding.add(new LabelVLBinder(db, LeavePlanRestDayEntitlePeriod, ELeavePlan.VLRestDayEntitlementPeriod));
        binding.add(LeavePlanRestDayEntitleDays);
        binding.add(new LabelVLBinder(db, LeavePlanRestDayWeeklyEntitleStartDay, ELeavePlan.VLRestDayGainWeekDay));
        binding.add(LeavePlanRestDayMonthlyEntitleProrataBase);
        binding.add(new LabelVLBinder(db, LeavePlanRestDayMonthlyEntitleProrataRoundingRuleID, EALProrataRoundingRule.VLALProrataRoundingRule));

        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["LeavePlanID"], out CurID))
            CurID = -1;

        HROne.ProductLicense productLicense = WebUtils.productLicense(Session);

        if (productLicense.ProductType != HROne.ProductLicense.ProductLicenseType.HROne)
        {
            ALProrataRoundingRuleRow.Visible = false;
            ResetYearOfServiceRow.Visible = false;
            ComparePreviousLeavePlanRow.Visible = false;
            RestDayStatutoryHolidayPanel.Visible = false;
        }
        else
        {
            ALProrataRoundingRuleRow.Visible = true;
            ResetYearOfServiceRow.Visible = true;
            ComparePreviousLeavePlanRow.Visible = true;
            RestDayStatutoryHolidayPanel.Visible = true;
        }

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (!Page.IsPostBack)
        {
            if (CurID > 0)
            {
                loadObject();
                loadLeaveTypes();
            }
            else
                toolBar.DeleteButton_Visible = false;
        }

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
    }

    protected bool loadObject() 
    {
	    obj=new ELeavePlan ();
	    bool isNew=WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
	    if(!db.select(dbConn, obj))
		    return false;

	    Hashtable values=new Hashtable();
	    db.populate(obj,values);
	    binding.toControl(values);

        if (obj.LeavePlanALRoundingRuleIsApplyFirstYearBroughtForwardOnly)
            LeavePlanALRoundingRuleIsApplyFirstYearBroughtForwardOnly.Visible = true;
        else
            LeavePlanALRoundingRuleIsApplyFirstYearBroughtForwardOnly.Visible = false;

        CommonLeaveYearRow1.Visible = obj.LeavePlanUseCommonLeaveYear;
        CommonLeaveYearRow2.Visible = obj.LeavePlanUseCommonLeaveYear;

        RestDayMonthlyOption1.Visible = obj.LeavePlanRestDayEntitlePeriod.Equals("M");
        RestDayMonthlyOption2.Visible = obj.LeavePlanRestDayEntitlePeriod.Equals("M");
        RestDayWeeklyOption.Visible = obj.LeavePlanRestDayEntitlePeriod.Equals("W");

        LeavePlanComparePreviousLeavePlanOptionPanel.Visible = obj.LeavePlanComparePreviousLeavePlan;

        return true;
    }
    protected void loadLeaveTypes()
    {
        DBFilter filter = new DBFilter();
        filter.add(new Match("LeaveType", "<>", ELeaveType.LEAVETYPECODE_COMPENSATION));
        filter.add(new Match("LeaveType", "<>", ELeaveType.LEAVETYPECODE_RESTDAY));
        filter.add(new Match("LeaveType", "<>", ELeaveType.LEAVETYPECODE_STATUTORYHOLIDAY));
        filter.add(new Match("LeaveType", "<>", ELeaveType.LEAVETYPECODE_PUBLICHOLIDAY));
        DataTable table = ELeaveType.db.loadDataSet(dbConn, null, filter);
        DataView view = new DataView(table);
        LeavePlanEntitle.DataSource = view;
        LeavePlanEntitle.DataBind();
    }



    protected void LeavePlanEntitle_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Footer || e.Item.ItemType == ListItemType.Header)
            return;
        DataRowView o=(DataRowView)e.Item.DataItem;
        int LeaveTypeID=(int)o["LeaveTypeID"];
        string LeaveTypeCode=(string)o["LeaveTypeDesc"];
        LeavePlanEntitle_List list=(LeavePlanEntitle_List)e.Item.FindControl("LeavePlanEntitle_List");
        list.CurrentLeaveTypeID = LeaveTypeID;
        list.Title = LeaveTypeCode;



    }

    protected void Delete_Click(object sender, EventArgs e)
    {

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ELeavePlan c = new ELeavePlan();
        c.LeavePlanID = CurID;
        if (ELeavePlan.db.select(dbConn, c))
        {
            DBFilter empPosFilter = new DBFilter();
            empPosFilter.add(new Match("LeavePlanID", c.LeavePlanID));
            empPosFilter.add("empid", true);
            ArrayList empPosList = EEmpPositionInfo.db.select(dbConn, empPosFilter);
            if (empPosList.Count > 0)
            {
                errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_USED_BY_EMPLOYEE, new string[] { HROne.Common.WebUtility.GetLocalizedString("Leave Plan Code"), c.LeavePlanCode }));
                foreach (EEmpPositionInfo empPos in empPosList)
                {
                    EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                    empInfo.EmpID = empPos.EmpID;
                    if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                        errors.addError("- " + empInfo.EmpNo + ", " + empInfo.EmpEngFullName);
                    else
                        EEmpPositionInfo.db.delete(dbConn, empPos);

                }
                errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
                return;

            }
            else
            {
                WebUtils.StartFunction(Session, FUNCTION_CODE);
                db.delete(dbConn, c);
                DBFilter dbFilter = new DBFilter();
                dbFilter.add(new Match("LeavePlanID", c.LeavePlanID));
                ArrayList leaveEntitleDetailList = ELeavePlanEntitle.db.select(dbConn, dbFilter);
                foreach (ELeavePlanEntitle leaveEntitlement in leaveEntitleDetailList)
                    ELeavePlanEntitle.db.delete(dbConn, leaveEntitlement);
                WebUtils.EndFunction(dbConn);
            }
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "LeavePlan_List.aspx");
    }
    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "LeavePlan_Edit.aspx?LeavePlanID=" + CurID);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "LeavePlan_List.aspx");
    }

    // Start 0000005, Miranda, 2014-03-26
    protected void ReCalc_Click(object sender, EventArgs e)
    {
        int empID;

        DBFilter empPosFilter = new DBFilter();
        empPosFilter.add(new Match("LeavePlanID", CurID));
        empPosFilter.add(new NullTerm("EmpPosEffTo"));

        // Start 000158, Ricky So, 2015-01-19
        //ArrayList empPosList = EEmpPositionInfo.db.select(dbConn, empPosFilter);
        //if (empPosList.Count > 0)
        //{
        //    foreach (EEmpPositionInfo empPos in empPosList)
        //    {
        //        empID = empPos.EmpID;
        //        LeaveBalanceCalc calc = new LeaveBalanceCalc(dbConn, empID);
        //        calc.Recalculate();
        //    }
        //}

        empPosFilter.add(new MatchField("pos.EmpID", "personal.EmpID"));
        empPosFilter.add(new Match("personal.EmpStatus", "A"));

        string select = "pos.EmpID";
        string from = "from [EmpPositionInfo] pos, [EmpPersonalInfo] personal ";//LEFT JOIN " + EEmpPositionInfo.db.dbclass.tableName + " p ON c.EmpID=p.EmpID AND p.EmpPosEffTo IS NULL";

        DataTable table = empPosFilter.loadData(dbConn, null, select, from);
        foreach(DataRow m_row in table.Rows)
        {
            LeaveBalanceCalc calc = new LeaveBalanceCalc(dbConn, (int) m_row[0]);
            calc.Recalculate();     
        }

        // End 000158, Ricky So, 2015-01-19

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();
        errors.addError(table.Rows.Count.ToString() + " employee(s) leave balances recalculated!");
    }
    // End 0000005, Miranda, 2014-03-26
}
