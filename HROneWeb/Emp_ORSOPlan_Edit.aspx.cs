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

public partial class Emp_ORSOPlan_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "PER008";
    public Binding binding;
    public DBManager db = EEmpORSOPlan.db;
    public EEmpORSOPlan obj;
    public int CurID = -1;
    public int CurEmpID = -1;
    public Hashtable CurEmpORSOPlanGroups=new Hashtable();
    public Hashtable CurRanks = new Hashtable();

    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        
        binding = new Binding(dbConn, db);
        binding.add(EmpORSOID);
        binding.add(EmpID);
        binding.add(new TextBoxBinder(db, EmpORSOEffFr.TextBox, EmpORSOEffFr.ID));
        binding.add(new TextBoxBinder(db, EmpORSOEffTo.TextBox, EmpORSOEffTo.ID));
        binding.add(new DropDownVLBinder(db, ORSOPlanID, EORSOPlan.VLORSOPlan).setNotSelected(null));
        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["EmpORSOID"], out CurID))
            CurID = -1;

        if (!int.TryParse(DecryptedRequest["EmpID"], out CurEmpID))
            CurEmpID = -1;

        EmpID.Value = CurEmpID.ToString();

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

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
	    obj=new EEmpORSOPlan();
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


        return true;
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        EEmpORSOPlan c = new EEmpORSOPlan();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);

        if (c.EmpORSOEffTo.Ticks > 0 && c.EmpORSOEffTo < c.EmpORSOEffFr)
        {
            errors.addError("EmpORSOEffTo", HROne.Translation.PageErrorMessage.ERROR_DATE_TO_TOO_EARLY);
            return;
        }

        AND andTerms = new AND();
        andTerms.add(new Match("EmpORSOID", "<>", c.EmpORSOID));
        andTerms.add(new Match("EmpORSOEffFr", "<=", c.EmpORSOEffFr));
        EEmpORSOPlan lastObj = (EEmpORSOPlan)AppUtils.GetLastObj(dbConn, db, "EmpORSOEffFr", c.EmpID, andTerms);
        if (lastObj != null && (c.EmpORSOEffFr <= lastObj.EmpORSOEffTo || c.EmpORSOEffFr == lastObj.EmpORSOEffFr))
        {
            errors.addError("EmpORSOEffFr", HROne.Translation.PageErrorMessage.ERROR_DATE_FROM_OVERLAP);
            return;
        }

        WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
        if (CurID < 0)
        {
//            Utils.MarkCreate(Session, c);

            db.insert(dbConn, c);
            CurID = c.EmpORSOID;
//            url = Utils.BuildURL(-1, CurID);
        }
        else
        {
//            Utils.Mark(Session, c);
            db.update(dbConn, c);
        }


        if (lastObj != null)
        {
            if (lastObj.EmpORSOEffTo < lastObj.EmpORSOEffFr)
            {
                lastObj.EmpORSOEffTo = c.EmpORSOEffFr.AddDays(-1);
                db.update(dbConn, lastObj);
            }
        }
        WebUtils.EndFunction(dbConn);

        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_ORSOPlan_View.aspx?EmpORSOID="+CurID+ "&EmpID=" + c.EmpID);


    }
    protected void Delete_Click(object sender, EventArgs e)
    {

        EEmpORSOPlan c = new EEmpORSOPlan();
        c.EmpORSOID = CurID;
        db.select(dbConn, c);
        WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
        db.delete(dbConn, c);
        WebUtils.EndFunction(dbConn);
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_Pension_View.aspx?EmpID=" + EmpID.Value);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_ORSOPlan_View.aspx?EmpORSOID=" + CurID + "&EmpID=" + EmpID.Value);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_Pension_View.aspx?EmpID=" + EmpID.Value);

    }
}
