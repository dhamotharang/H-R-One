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

public partial class Emp_Residence_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "PER005";
    public Binding binding;
    public DBManager db = EEmpPlaceOfResidence.db;
    public EEmpPlaceOfResidence obj;
    public DBManager empDB = EEmpPersonalInfo.db;
    public EEmpPersonalInfo empOBJ;
    public int CurID = -1;
    public int CurEmpID = -1;

    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        
        binding = new Binding(dbConn, db);
        binding.add(EmpPoRID);
        binding.add(EmpID);
        binding.add(new TextBoxBinder(db, EmpPoRFrom.TextBox, EmpPoRFrom.ID));
        binding.add(new TextBoxBinder(db, EmpPoRTo.TextBox, EmpPoRTo.ID));
        binding.add(EmpPoRLandLord);
        binding.add(EmpPoRLandLordAddr);
        binding.add(EmpPoRPropertyAddr);
        binding.add(EmpPoRNature);
        binding.add(EmpPoRPayToLandEE);
        binding.add(EmpPoRPayToLandER);
        binding.add(EmpPoRRefundToEE);
        binding.add(EmpPoRPayToERByEE);
        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["EmpPoRID"], out CurID))
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
            {
                empOBJ = new EEmpPersonalInfo();
                DBFilter filter = new DBFilter();
                if (CurEmpID != -1)
                {
                    filter.add(new Match("EmpID", CurEmpID));
                    ArrayList empInfoList = EEmpPersonalInfo.db.select(dbConn, filter);
                    if (empInfoList.Count > 0)
                    {
                        empOBJ = (EEmpPersonalInfo)empInfoList[0];
                        EmpPoRPropertyAddr.Text = empOBJ.EmpResAddr;
                        if (empOBJ.EmpResAddrAreaCode.Equals("H"))
                            EmpPoRPropertyAddr.Text += ", " + "Hong Kong";
                        else if (empOBJ.EmpResAddrAreaCode.Equals("K"))
                            EmpPoRPropertyAddr.Text += ", " + "Kowloon";
                        else if (empOBJ.EmpResAddrAreaCode.Equals("N"))
                            EmpPoRPropertyAddr.Text += ", " + "New Territories";
                    }
                }
                EmpPoRNature.Text = "Flat";
                toolBar.DeleteButton_Visible = false;
            }
        }
    }
    protected bool loadObject() 
    {
	    obj=new EEmpPlaceOfResidence();
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
        EEmpPlaceOfResidence c = new EEmpPlaceOfResidence();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);

        ValidateData(c, errors);

        if (c.EmpPoRTo!=DateTime.MinValue &&  c.EmpPoRFrom > c.EmpPoRTo)
            errors.addError("EmpPoRFrom", HROne.Translation.PageErrorMessage.ERROR_DATE_TO_TOO_EARLY);

        if (!errors.isEmpty())
            return;

        EEmpPlaceOfResidence prev = (EEmpPlaceOfResidence)AppUtils.GetLastObj(dbConn, EEmpPlaceOfResidence.db, "EmpPoRFrom", CurEmpID);
        if (prev != null && prev.EmpPoRID  != CurID)
        {
            if (prev.EmpPoRTo.Ticks.Equals(0))

                if (prev.EmpPoRFrom <= c.EmpPoRFrom.AddDays(-1))
                {
                    prev.EmpPoRTo = c.EmpPoRFrom.AddDays(-1);
                    db.update(dbConn, prev);

                }
                else
                    if (c.EmpPoRTo.Ticks.Equals(0))
                        errors.addError(HROne.Translation.PageErrorMessage.ERROR_DATE_FROM_OVERLAP);
        }

        if (!errors.isEmpty())
            return;

        WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
        if (CurID < 0)
        {
//            Utils.MarkCreate(Session, c);

            db.insert(dbConn, c);
            CurID = c.EmpPoRID;
//            url = Utils.BuildURL(-1, CurID);
        }
        else
        {
//            Utils.Mark(Session, c);
            db.update(dbConn, c);
        }
        WebUtils.EndFunction(dbConn);


        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_Residence_View.aspx?EmpPoRID="+CurID+ "&EmpID=" + c.EmpID);


    }
    protected void Delete_Click(object sender, EventArgs e)
    {

        EEmpPlaceOfResidence c = new EEmpPlaceOfResidence();
        c.EmpPoRID = CurID;
        db.select(dbConn, c);
        WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
        db.delete(dbConn, c);
        WebUtils.EndFunction(dbConn);
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_Residence_View.aspx?EmpID=" + EmpID.Value);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_Residence_View.aspx?EmpPoRID=" + CurID + "&EmpID=" + EmpID.Value);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_Residence_View.aspx?EmpID=" + EmpID.Value);

    }

    private void ValidateData(EEmpPlaceOfResidence obj, PageErrors errors)
    {
        DBFilter overlapCheckingFilter = new DBFilter();
        overlapCheckingFilter.add(new Match("EmpID", obj.EmpID));
        if (obj.EmpPoRID > 0)
            overlapCheckingFilter.add(new Match("EmpPoRID", "<>", obj.EmpPoRID));
        overlapCheckingFilter.add(new Match("EmpPoRTo", ">=", obj.EmpPoRFrom));
        overlapCheckingFilter.add(new Match("EmpPoRFrom", "<=", obj.EmpPoRTo.Ticks.Equals(0) ? DateTime.MaxValue : obj.EmpPoRTo));

        if (EEmpPlaceOfResidence.db.count(dbConn, overlapCheckingFilter) > 0)
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_INCORRECT_DATE_RANGE);
    }
}
