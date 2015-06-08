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

public partial class Emp_WorkingExperience_View : HROneWebPage
{
    private const string FUNCTION_CODE = "PER016";

    public Binding binding;
    public DBManager db = EEmpWorkExp.db;
    public EEmpWorkExp obj;
    public int CurID = -1;
    public int CurEmpID = -1;
    public Hashtable CurEmpWorkExpGroups=new Hashtable();
    public Hashtable CurRanks = new Hashtable();
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;
        

        binding = new Binding(dbConn, db);
        binding.add(EmpWorkExpID);
        binding.add(EmpID);
        binding.add(new BlankZeroLabelVLBinder(db, EmpWorkExpFromMonth, Values.VLMonth));
        binding.add(EmpWorkExpFromYear);
        binding.add(new BlankZeroLabelVLBinder(db, EmpWorkExpToMonth, Values.VLMonth));
        binding.add(EmpWorkExpToYear);
        binding.add(EmpWorkExpCompanyName);
        binding.add(EmpWorkExpPosition);
        binding.add(new BlankZeroLabelVLBinder(db, EmpWorkExpEmploymentTypeID, EEmploymentType.VLEmploymentType));
        binding.add(new LabelVLBinder(db, EmpWorkExpIsRelevantExperience, Values.VLTrueFalseYesNo));
        binding.add(EmpWorkExpRemark);
        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["EmpWorkExpID"], out CurID))
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
	    obj=new EEmpWorkExp();
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

        if (EmpWorkExpToYear.Text.Equals("0"))
            EmpWorkExpToYear.Text = string.Empty;

        return true;
    }


    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        EEmpWorkExp obj = new EEmpWorkExp();
        obj.EmpWorkExpID = CurID;

        if (EEmpWorkExp.db.select(dbConn, obj))
        {
            WebUtils.StartFunction(Session, FUNCTION_CODE, obj.EmpID);
            db.delete(dbConn, obj);
            WebUtils.EndFunction(dbConn);
        }        
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_WorkingExperience_View.aspx?EmpID=" + EmpID.Value);
    }
    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_WorkingExperience_Edit.aspx?EmpWorkExpID=" + EmpWorkExpID.Value + "&EmpID=" + EmpID.Value);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_WorkingExperience_View.aspx?EmpID=" + EmpID.Value);
        
    }
}
