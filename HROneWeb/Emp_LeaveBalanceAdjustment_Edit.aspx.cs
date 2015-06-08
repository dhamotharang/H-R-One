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

public partial class Emp_LeaveBalanceAdjustment_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "PER010";

    public Binding binding;
    public DBManager db = ELeaveBalanceAdjustment.db;
    public ELeaveBalanceAdjustment obj;
    public int CurID = -1;
    public int CurEmpID = -1;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        
        binding = new Binding(dbConn, db);
        binding.add(LeaveBalAdjID);
        binding.add(EmpID);
        binding.add(new TextBoxBinder(db, LeaveBalAdjDate.TextBox, LeaveBalAdjDate.ID));
        binding.add(new DropDownVLBinder(db, LeaveTypeID, ELeaveType.VLLeaveType).setNotSelected(null));
        binding.add(new DropDownVLBinder(db, LeaveBalAdjType, ELeaveBalanceAdjustment.VLLeaveBalAdjType).setNotSelected(null));
        binding.add(LeaveBalAdjValue);
        binding.add(LeaveBalAdjRemark);
        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["LeaveBalAdjID"], out CurID))
            CurID = -1;

        if (!int.TryParse(DecryptedRequest["EmpID"], out CurEmpID))
            CurEmpID = -1;

        //  MUST applied Label Change before translation
        if (CurID > 0)
            ActionHeader.Text = "Edit";
        else
            ActionHeader.Text = "Add";

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);


		if(!Page.IsPostBack) 
		{
            if (CurID > 0)
                loadObject();
            else
                toolBar.DeleteButton_Visible = false;
        }

        EmpID.Value = CurEmpID.ToString();
    }


    protected bool loadObject() 
    {
        obj = new ELeaveBalanceAdjustment();
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

        CurEmpID = obj.EmpID;
        return true;
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        ELeaveBalanceAdjustment c = new ELeaveBalanceAdjustment();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;
        db.parse(values, c);


        // validate if Compensation Leave selected, no Balance Reset is allowed
        if (c.LeaveTypeID > 0)
        {
            ELeaveType m_leaveType = ELeaveType.GetObject(dbConn, c.LeaveTypeID);
            if (m_leaveType != null)
            {
                if (m_leaveType.LeaveType == ELeaveType.LEAVETYPECODE_COMPENSATION && 
                    c.LeaveBalAdjType == ELeaveBalanceAdjustment.ADJUST_TYPE_RESET_BALANCE)
                {
                    errors.addError("Compensation Leave Adjustment does not support Balance Reset");
                }
            }

            if (!errors.isEmpty())
                return;
        }

        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpID", c.EmpID));
        filter.add(new Match("LeaveBalAdjID", "<>", c.LeaveBalAdjID));
        OR or = new OR();
        AND and;

        and = new AND();
        and.add(new Match("LeaveBalAdjDate", "=", c.LeaveBalAdjDate));
        and.add(new Match("LeaveTypeID", "=", c.LeaveTypeID));
        or.add(and);

        filter.add(or);
        if (db.count(dbConn, filter) > 0)
        {
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_LEAVE_ADJ_OVERLAP);
        }

        if (!errors.isEmpty())
            return;

        WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
        //LeaveBalanceCalc leaaveBalCal = new LeaveBalanceCalc(dbConn, c.EmpID);

        if (CurID < 0)
        {
            db.insert(dbConn, c);
            CurID = c.LeaveBalAdjID;
            //leaaveBalCal.RecalculateAfter(c.LeaveBalAdjDate, c.LeaveTypeID);
        }
        else
        {
            //ELeaveBalanceAdjustment leaveBalAdjust = new ELeaveBalanceAdjustment();
            //leaveBalAdjust.LeaveBalAdjID = CurID;
            //db.select(dbConn, leaveBalAdjust);
            db.update(dbConn, c);
            //leaaveBalCal.RecalculateAfter(leaveBalAdjust.LeaveBalAdjDate < c.LeaveBalAdjDate ? leaveBalAdjust.LeaveBalAdjDate : c.LeaveBalAdjDate);
        }
        WebUtils.EndFunction(dbConn);


        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_LeaveBalanceAdjustment_View.aspx?EmpID=" + c.EmpID + "&LeaveBalAdjID=" + CurID);


    }
    protected void Delete_Click(object sender, EventArgs e)
    {
        ELeaveBalanceAdjustment c = new ELeaveBalanceAdjustment();
        c.LeaveBalAdjID = CurID;
        db.select(dbConn, c);
        WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
        db.delete(dbConn, c);
        WebUtils.EndFunction(dbConn);
        //LeaveBalanceCalc leaaveBalCal = new LeaveBalanceCalc(dbConn, c.EmpID);
        //leaaveBalCal.RecalculateAfter(c.LeaveBalAdjDate, c.LeaveTypeID);

        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_LeaveBalance_View.aspx?EmpID=" + EmpID.Value);
    }
    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_LeaveBalanceAdjustment_View.aspx?EmpID=" + EmpID.Value + "&LeaveBalAdjID=" + CurID);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_LeaveBalance_View.aspx?EmpID=" + EmpID.Value);

    }

}
