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

public partial class Leave_ALProrataRoundingRule_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "LEV004";
    public Binding binding;
    public DBManager db = EALProrataRoundingRule.db;
    public EALProrataRoundingRule obj;
    public int CurID = -1;
    

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        

        binding = new Binding(dbConn, db);
        binding.add(ALProrataRoundingRuleID);
        binding.add(ALProrataRoundingRuleCode);
        binding.add(ALProrataRoundingRuleDesc);

        binding.init(Request, Session);

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (!int.TryParse(DecryptedRequest["ALProrataRoundingRuleID"], out CurID))
            CurID = -1;

		if(!Page.IsPostBack) 
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
        obj = new EALProrataRoundingRule();
	    bool isNew=WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
	    if(!db.select(dbConn, obj))
		    return false;


	    Hashtable values=new Hashtable();
	    db.populate(obj,values);
	    binding.toControl(values);
        return true;
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        EALProrataRoundingRule c = new EALProrataRoundingRule();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);
        if (!AppUtils.checkDuplicate(dbConn, db, c, errors, "ALProrataRoundingRuleCode"))
            return;

        WebUtils.StartFunction(Session, FUNCTION_CODE);
        if (CurID < 0)
        {
//            Utils.MarkCreate(Session, c);

            db.insert(dbConn, c);
            CurID = c.ALProrataRoundingRuleID;
//            url = Utils.BuildURL(-1, CurID);
        }
        else
        {
//            Utils.Mark(Session, c);
            db.update(dbConn, c);
        }
        WebUtils.EndFunction(dbConn);


        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Leave_ALProrataRoundingRule_View.aspx?ALProrataRoundingRuleID=" + CurID);


    }
    protected void Delete_Click(object sender, EventArgs e)
    {

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        EALProrataRoundingRule o = new EALProrataRoundingRule();
        o.ALProrataRoundingRuleID = CurID;
        if (db.select(dbConn, o))
        {
            DBFilter leavePlanFilter = new DBFilter();
            leavePlanFilter.add(new Match("ALProrataRoundingRuleID", o.ALProrataRoundingRuleID));
            ArrayList leavePlanList = ELeavePlan.db.select(dbConn, leavePlanFilter);
            if (leavePlanList.Count > 0)
            {
                errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_IS_IN_USE, new string[] { o.ALProrataRoundingRuleCode }));
                errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
                return;
            }
            else
            {
                WebUtils.StartFunction(Session, FUNCTION_CODE);
                db.delete(dbConn, o);
                DBFilter obj = new DBFilter();
                obj.add(new Match("ALProrataRoundingRuleID", o.ALProrataRoundingRuleID));
                ArrayList objList = EALProrataRoundingRuleDetail.db.select(dbConn, obj);
                foreach (EALProrataRoundingRuleDetail match in objList)
                    EALProrataRoundingRuleDetail.db.delete(dbConn, match);
                WebUtils.EndFunction(dbConn);
            }
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Leave_ALProrataRoundingRule_List.aspx");
    }
    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Leave_ALProrataRoundingRule_View.aspx?ALProrataRoundingRuleID=" + CurID);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Leave_ALProrataRoundingRule_List.aspx");

    }
}
