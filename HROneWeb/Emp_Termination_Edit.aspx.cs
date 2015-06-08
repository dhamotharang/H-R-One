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

public partial class Emp_Termination_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "PER011";

    public Binding binding;
    public DBManager db = EEmpTermination.db;
    public EEmpTermination obj;
    public int CurID = -1;
    public int CurEmpID = -1;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        binding = new Binding(dbConn, db);
        binding.add(EmpTermID);
        binding.add(EmpID);
        binding.add(new DropDownVLBinder(db, CessationReasonID, ECessationReason.VLCessationReason));
        binding.add(new TextBoxBinder(db, EmpTermResignDate.TextBox, EmpTermResignDate.ID));
        binding.add(EmpTermNoticePeriod);
        binding.add(new DropDownVLBinder(db, EmpTermNoticeUnit, Values.VLEmpUnit));
        binding.add(new TextBoxBinder(db, EmpTermLastDate.TextBox, EmpTermLastDate.ID));
        binding.add(EmpTermRemark);
        binding.add(new CheckBoxBinder(db,EmpTermIsTransferCompany));
        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["EmpID"], out CurEmpID))
            CurEmpID = -1;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

		if(!Page.IsPostBack) 
		{
            if (CurEmpID > 0)
            {
                loadObject();
            }
            else
                toolBar.DeleteButton_Visible = false;
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //EmpTermResignDate.TextBox.Attributes["onchange"] += " changeDate();";
    }
    protected bool loadObject() 
    {

        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpID", CurEmpID));
        filter.add(WebUtils.AddRankFilter(Session, "EmpID", true));
        if (EEmpPersonalInfo.db.count(dbConn, filter) == 0)
            if (CurEmpID <= 0)
                return false;
            else
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");


        DBFilter empTermFilter = new DBFilter();
        empTermFilter.add(new Match("EmpID", CurEmpID));
        ArrayList list = db.select(dbConn, empTermFilter);
        if (list.Count == 0)
        {
            obj = new EEmpTermination();
            obj.EmpID = CurEmpID;
            obj.EmpTermID = -1;


            EEmpPersonalInfo p=new EEmpPersonalInfo();
            p.EmpID = CurEmpID;
            EEmpPersonalInfo.db.select(dbConn, p);
            if (p.EmpNoticePeriod == 0 && p.EmpNoticeUnit == "")
            {
                obj.EmpTermNoticePeriod = 1;
                obj.EmpTermNoticeUnit = "D";
            }
            else
            {
                obj.EmpTermNoticePeriod = p.EmpNoticePeriod;
                obj.EmpTermNoticeUnit = p.EmpNoticeUnit;
            }
        }
        else
        {
            obj = (EEmpTermination)list[0];
            CurID = obj.EmpTermID;
        }

        if (!obj.EmpTermResignDate.Ticks.Equals(0) && obj.EmpTermNoticePeriod >= 0 && obj.EmpTermNoticeUnit != string.Empty)
            ExpectedLastDate.Text = obj.GetExpectedLastEmploymentDate().ToString("yyyy-MM-dd");

	    Hashtable values=new Hashtable();
	    db.populate(obj,values);
	    binding.toControl(values);
        return true;
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        EEmpTermination c = new EEmpTermination();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);


        //EEmpTermination i = new EEmpTermination();
        //i.EmpTermID = c.EmpTermID;
        //if (c.EmpTermID >= 0)
        //    db.select(dbConn, i);
        //if (c.EmpTermID < 0 || i.EmpTermNoticePeriod != c.EmpTermNoticePeriod || !i.EmpTermNoticeUnit.Equals(c.EmpTermNoticeUnit) || !i.EmpTermResignDate.Equals(c.EmpTermResignDate))
        //{
        //    if ("D".Equals(c.EmpTermNoticeUnit))
        //        c.EmpTermLastDate = c.EmpTermResignDate.AddDays(c.EmpTermNoticePeriod - 1);
        //    else
        //        c.EmpTermLastDate = c.EmpTermResignDate.AddMonths(c.EmpTermNoticePeriod ).AddDays(-1);
        //}

        WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
        if (c.EmpTermID < 0)
        {
            DBFilter filter = new DBFilter();
            filter.add(new Match("EmpID", c.EmpID));
            ArrayList list = db.select(dbConn, filter);
            if (list.Count > 0)
            {
                c.EmpTermID = ((EEmpTermination)list[0]).EmpTermID;
                db.update(dbConn, c);
            }
            else
            {
                db.insert(dbConn, c);
            }
            CurID = c.EmpTermID;
        }
        else
        {
//            Utils.Mark(Session, c);
            db.update(dbConn, c);
        }
        WebUtils.EndFunction(dbConn);

        EEmpPersonalInfo pi = new EEmpPersonalInfo();
        pi.EmpID = CurEmpID;
        EEmpPersonalInfo.db.select(dbConn, pi);
        pi.EmpStatus = "T";
        EEmpPersonalInfo.db.update(dbConn, pi);

        db.select(dbConn, c);
        if (c.EmpTermIsTransferCompany && c.NewEmpID <= 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_TransferCompany.aspx?EmpID=" + c.EmpID);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_Termination_View.aspx?EmpID=" + c.EmpID);


    }

    protected void Back_Click(object sender, EventArgs e)
    {

            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_Termination_View.aspx?EmpID=" + EmpID.Value);

    }
    protected void EmpTermNoticePeriod_TextChanged(object sender, EventArgs e)
    {
        UpdateLastDate();
    }
    protected void EmpTermNoticeUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateLastDate();
    }
    protected void EmpTermResignDate_Changed(object sender, EventArgs e)
    {
        UpdateLastDate();
    }

    protected void UpdateLastDate()
    {
        EEmpTermination c = new EEmpTermination();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        db.parse(values, c);

        if (!c.EmpTermResignDate.Ticks.Equals(0) && c.EmpTermNoticePeriod >= 0 && c.EmpTermNoticeUnit != string.Empty)
        {
            EmpTermLastDate.Value = c.GetExpectedLastEmploymentDate().ToString("yyyy-MM-dd");
            ExpectedLastDate.Text = c.GetExpectedLastEmploymentDate().ToString("yyyy-MM-dd");
        }
        else
            ExpectedLastDate.Text = string.Empty;
    }
}
