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

public partial class Emp_CompensationLeaveEntitle : HROneWebPage
{
    private const string FUNCTION_CODE = "PER018";

    public Binding binding;
    public DBManager db = ECompensationLeaveEntitle .db;
    public int CurID = -1;
    public int CurEmpID = -1;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        
        binding = new Binding(dbConn, db);
        binding.add(CompensationLeaveEntitleID);
        binding.add(EmpID);
        binding.add(new TextBoxBinder(db, CompensationLeaveEntitleEffectiveDate.TextBox, CompensationLeaveEntitleEffectiveDate.ID));
        binding.add(new TextBoxBinder(db, CompensationLeaveEntitleDateExpiry.TextBox, CompensationLeaveEntitleDateExpiry.ID));
        binding.add(new TextBoxBinder(db, CompensationLeaveEntitleClaimPeriodFrom.TextBox, CompensationLeaveEntitleClaimPeriodFrom.ID));
        binding.add(new TextBoxBinder(db, CompensationLeaveEntitleClaimPeriodTo.TextBox, CompensationLeaveEntitleClaimPeriodTo.ID));
        binding.add(CompensationLeaveEntitleClaimHourFrom);
        binding.add(CompensationLeaveEntitleClaimHourTo);
        binding.add(CompensationLeaveEntitleHoursClaim);
        binding.add(CompensationLeaveEntitleApprovedBy);
        binding.add(CompensationLeaveEntitleRemark);
        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["CompensationLeaveEntitleID"], out CurID))
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

        }
    }


    protected bool loadObject() 
    {
        ECompensationLeaveEntitle obj = new ECompensationLeaveEntitle();
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
        ECompensationLeaveEntitle c = new ECompensationLeaveEntitle();

        if (string.IsNullOrEmpty(CompensationLeaveEntitleClaimPeriodTo.Value))
            CompensationLeaveEntitleClaimPeriodTo.Value = CompensationLeaveEntitleClaimPeriodFrom.Value;

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

        ELeaveType compensationLeaveType = ELeaveType.COMPENSATION_LEAVE_TYPE(dbConn);

        WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
        LeaveBalanceCalc leaaveBalCal = new LeaveBalanceCalc(dbConn, c.EmpID);

        if (CurID < 0)
        {
//            Utils.MarkCreate(Session, c);

            db.insert(dbConn, c);
            CurID = c.CompensationLeaveEntitleID;
            //if (compensationLeaveType != null)
            //    leaaveBalCal.RecalculateAfter(c.CompensationLeaveEntitleEffectiveDate, compensationLeaveType.LeaveTypeID);
            //else
            //    leaaveBalCal.RecalculateAfter(c.CompensationLeaveEntitleEffectiveDate);

//            url = Utils.BuildURL(-1, CurID);
        }
        else
        {
//            Utils.Mark(Session, c);
            //ECompensationLeaveEntitle leaveBalApp = new ECompensationLeaveEntitle();
            //leaveBalApp.CompensationLeaveEntitleID = CurID;
            //db.select(dbConn, leaveBalApp);
            db.update(dbConn, c);
            //if (compensationLeaveType != null)
            //    leaaveBalCal.RecalculateAfter(leaveBalApp.CompensationLeaveEntitleEffectiveDate < c.CompensationLeaveEntitleEffectiveDate ? leaveBalApp.CompensationLeaveEntitleEffectiveDate : c.CompensationLeaveEntitleEffectiveDate, compensationLeaveType.LeaveTypeID);
            //else
            //    leaaveBalCal.RecalculateAfter(leaveBalApp.CompensationLeaveEntitleEffectiveDate < c.CompensationLeaveEntitleEffectiveDate ? leaveBalApp.CompensationLeaveEntitleEffectiveDate : c.CompensationLeaveEntitleEffectiveDate);

        }
        WebUtils.EndFunction(dbConn);


        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_CompensationLeaveEntitle_View.aspx?EmpID=" + c.EmpID + "&CompensationLeaveEntitleID=" + CurID);


    }
    protected void Delete_Click(object sender, EventArgs e)
    {

        ECompensationLeaveEntitle c = new ECompensationLeaveEntitle();
        c.CompensationLeaveEntitleID = CurID;
        db.select(dbConn, c);
        WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
        db.delete(dbConn, c);
        WebUtils.EndFunction(dbConn);
        //LeaveBalanceCalc leaaveBalCal = new LeaveBalanceCalc(dbConn, c.EmpID);
        //leaaveBalCal.RecalculateAfter(c.CompensationLeaveEntitleEffectiveDate, ELeaveType.COMPENSATION_LEAVE_TYPE(dbConn).LeaveTypeID);

        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_CompensationLeaveEntitle_View.aspx?EmpID=" + EmpID.Value);
    }


    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_CompensationLeaveEntitle_View.aspx?EmpID=" + EmpID.Value + "&CompensationLeaveEntitleID=" + CurID);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_CompensationLeaveEntitle_View.aspx?EmpID=" + EmpID.Value);

    }
}
