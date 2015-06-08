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
public partial class Emp_LeaveBalanceEntitle_View : HROneWebPage
{
    private const string FUNCTION_CODE = "PER009";

    public Binding binding;
    public DBManager db = ELeaveBalanceEntitle.db;
    public ELeaveBalanceEntitle obj;
    public int CurID = -1;
    public int CurEmpID = -1;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        binding = new Binding(dbConn, db);
        binding.add(LeaveBalanceEntitleID);
        binding.add(EmpID);
        binding.add(LeaveTypeID);
        binding.add(new LabelVLBinder(db, LeaveTypeDescription,"LeaveTypeID", ELeaveType.VLLeaveType));
        binding.add(new LabelVLBinder(db, LeaveBalanceEntitleEffectiveDate, new WFHourList()));
        binding.add(new LabelVLBinder(db, LeaveBalanceEntitleDateExpiry, new WFHourList()));
        binding.add(LeaveBalanceEntitleDays);

        if (!int.TryParse(DecryptedRequest["LeaveBalanceEntitleID"], out CurID))
            CurID = -1;

        if (!int.TryParse(DecryptedRequest["EmpID"], out CurEmpID))
            CurEmpID = -1;

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
        obj = new ELeaveBalanceEntitle();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);

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

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);

        CurEmpID = obj.EmpID;
        return true;
    }

    //protected void Delete_Click(object sender, EventArgs e)
    //{

    //    ELeaveApplication c = new ELeaveApplication();
    //    c.LeaveAppID = CurID;
    //    db.select(dbConn, c);
    //    WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
    //    db.delete(dbConn, c);
    //    WebUtils.EndFunction(dbConn);
    //    //HROne.LeaveCalc.LeaveBalanceCalc leaaveBalCal = new HROne.LeaveCalc.LeaveBalanceCalc(dbConn, c.EmpID);
    //    //leaaveBalCal.RecalculateAfter(c.LeaveAppDateFrom, ELeaveType.COMPENSATION_LEAVE_TYPE(dbConn).LeaveTypeID);
    //    HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_LeaveApplication_View.aspx?EmpID=" + EmpID.Value);
    //}

    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_LeaveBalanceEntitle_Edit.aspx?LeaveBalanceEntitleID=" + LeaveBalanceEntitleID.Value + "&EmpID=" + EmpID.Value);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_LeaveBalanceEntitleHistory_View.aspx?EmpID=" + EmpID.Value + "&LeaveTypeID=" + LeaveTypeID.Value);
    }
}
