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

public partial class LeaveType_Edit : HROneWebPage
{

    private const string FUNCTION_CODE = "LEV001";
    public Binding binding;
    public DBManager db = ELeaveType.db;
    public ELeaveType  obj;
    public int CurID = -1;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        

        binding = new Binding(dbConn, db);
        binding.add(LeaveTypeID);
        binding.add(LeaveType);
        binding.add(LeaveTypeDesc);
        binding.add(LeaveDecimalPlace);
        binding.add(new CheckBoxBinder(db, LeaveTypeIsUseWorkHourPattern));
        binding.add(new CheckBoxBinder(db, LeaveTypeIsSkipStatutoryHolidayChecking));
        binding.add(new CheckBoxBinder(db, LeaveTypeIsSkipPublicHolidayChecking));
        binding.add(new CheckBoxBinder(db, LeaveTypeIsDisabled));
        binding.add(new CheckBoxBinder(db, LeaveTypeIsESSHideLeaveBalance));
        binding.add(new CheckBoxBinder(db, LeaveTypeIsESSIgnoreEntitlement));
        binding.add(new CheckBoxBinder(db, LeaveTypeIsESSRestrictNegativeBalanceAsOfToday));
        binding.add(new CheckBoxBinder(db, LeaveTypeIsESSRestrictNegativeBalanceAsOfApplicationDateFrom));
        binding.add(new CheckBoxBinder(db, LeaveTypeIsESSRestrictNegativeBalanceAsOfApplicationDateTo));
        binding.add(new CheckBoxBinder(db, LeaveTypeIsESSRestrictNegativeBalanceAsOfEndOfLeaveYear));
        //binding.add(new TextBoxBinder(db, LeaveTypeIsESSAllowableBalance, LeaveTypeIsESSAllowableBalance.ID));
        binding.add(LeaveTypeIsESSAllowableAdvanceBalance);

        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["LeaveTypeID"], out CurID))
            CurID = -1;

        HROne.ProductLicense productLicense = WebUtils.productLicense(Session);
        if (productLicense.IsESS)
            ESSRow.Visible = true;
        else
            ESSRow.Visible = false;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

		if(!Page.IsPostBack) 
		{
            if (CurID > 0)
            {
                loadObject();
            }
            else
                toolBar.DeleteButton_Visible = false;
        }

        if (obj != null)
        {
            // disable 4th checkbox for Compensation Leave
            LeaveTypeIsESSRestrictNegativeBalanceAsOfEndOfLeaveYear.Enabled = (!obj.LeaveType.Equals("COMPENSATION"));
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {

        if (LeaveTypeIsESSIgnoreEntitlement.Checked == true)
        {
            LeaveTypeIsESSRestrictNegativeBalanceAsOfToday.Checked = false;
            LeaveTypeIsESSRestrictNegativeBalanceAsOfApplicationDateFrom.Checked = false;
            LeaveTypeIsESSRestrictNegativeBalanceAsOfApplicationDateTo.Checked = false;
            LeaveTypeIsESSRestrictNegativeBalanceAsOfEndOfLeaveYear.Checked = false;
        }

        LeaveTypeIsESSRestrictNegativeBalanceAsOfToday.Enabled = !LeaveTypeIsESSIgnoreEntitlement.Checked;
        LeaveTypeIsESSRestrictNegativeBalanceAsOfApplicationDateFrom.Enabled = !LeaveTypeIsESSIgnoreEntitlement.Checked;
        LeaveTypeIsESSRestrictNegativeBalanceAsOfApplicationDateTo.Enabled = !LeaveTypeIsESSIgnoreEntitlement.Checked;
        LeaveTypeIsESSRestrictNegativeBalanceAsOfEndOfLeaveYear.Enabled = !LeaveTypeIsESSIgnoreEntitlement.Checked;

        LeaveTypeIsESSAllowableAdvanceBalance.Enabled = (LeaveTypeIsESSRestrictNegativeBalanceAsOfToday.Checked) ||
                                                        (LeaveTypeIsESSRestrictNegativeBalanceAsOfApplicationDateFrom.Checked) ||
                                                        (LeaveTypeIsESSRestrictNegativeBalanceAsOfApplicationDateTo.Checked) ||
                                                        (LeaveTypeIsESSRestrictNegativeBalanceAsOfEndOfLeaveYear.Checked);

    }


    protected bool loadObject() 
    {
	    obj=new ELeaveType ();
	    bool isNew=WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
	    if(!db.select(dbConn, obj))
		    return false;

	    Hashtable values=new Hashtable();
	    db.populate(obj,values);
	    binding.toControl(values);

        if (obj.IsSystemUse())
            LeaveType.Enabled = false;
        else
            LeaveType.Enabled = true;

        if (obj.LeaveType == ELeaveType.LEAVETYPECODE_COMPENSATION)
            LeaveUnit.Text = " Hour(s)";
        else
            LeaveUnit.Text = " Day(s)";

        return true;
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        ELeaveType  c = new ELeaveType ();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);
        if (!AppUtils.checkDuplicate(dbConn, db, c, errors, "LeaveType"))
            return;

        WebUtils.StartFunction(Session, FUNCTION_CODE);
        if (CurID < 0)
        {
//            Utils.MarkCreate(Session, c);

            db.insert(dbConn, c);
            CurID = c.LeaveTypeID;
//            url = Utils.BuildURL(-1, CurID);
        }
        else
        {
//            Utils.Mark(Session, c);
            db.update(dbConn, c);
        }
        WebUtils.EndFunction(dbConn);


        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "LeaveType_View.aspx?LeaveTypeID="+CurID);


    }

    protected void LeaveTypeIsESSRestrictNegativeBalanceAsOfEndOfLeaveYear_Changed(object sender, EventArgs e)
    {
       // LeaveUnit.Enabled = (LeaveTypeIsESSRestrictNegativeBalanceAsOfEndOfLeaveYear.Checked);

    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ELeaveType obj = new ELeaveType();
        obj.LeaveTypeID = CurID;
        db.select(dbConn, obj);

        DBFilter leaveCodeFilter = new DBFilter();
        leaveCodeFilter.add(new Match("LeaveTypeID", obj.LeaveTypeID));

        DBFilter leaveApplicationFilter = new DBFilter();
        leaveApplicationFilter.add(new IN("LeaveCodeID", "Select LeaveCodeID from " + ELeaveCode.db.dbclass.tableName, leaveCodeFilter));
        leaveApplicationFilter.add("empid", true);
        ArrayList leaveApplicationList = ELeaveApplication.db.select(dbConn, leaveApplicationFilter);
        if (leaveApplicationList.Count > 0)
        {
            int curEmpID = 0;
            errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_USED_BY_EMPLOYEE, new string[] { HROne.Common.WebUtility.GetLocalizedString("Leave Type"), obj.LeaveType }));
            foreach (ELeaveApplication leaveApplication in leaveApplicationList)
            {
                EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                empInfo.EmpID = leaveApplication.EmpID;
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                {
                    if (curEmpID != leaveApplication.EmpID)
                    {
                        errors.addError("- " + empInfo.EmpNo + ", " + empInfo.EmpEngFullName);
                        curEmpID = leaveApplication.EmpID;
                    }
                }
                else
                    ELeaveApplication.db.delete(dbConn, leaveApplication);

            }
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
            return;
        }
        else
        {

            WebUtils.StartFunction(Session, FUNCTION_CODE);
            ELeaveCode.db.delete(dbConn, leaveCodeFilter);
            ELeavePlanEntitle.db.delete(dbConn, leaveCodeFilter);
            db.delete(dbConn, obj);
            WebUtils.EndFunction(dbConn);
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "LeaveType_List.aspx");
    }
    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "LeaveType_View.aspx?LeaveTypeID=" + CurID);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "LeaveType_List.aspx");

    }
}
