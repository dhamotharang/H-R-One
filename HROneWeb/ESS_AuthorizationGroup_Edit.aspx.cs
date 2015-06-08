using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using HROne.Lib.Entities;
using HROne.DataAccess;
//using perspectivemind.validation;

public partial class ESS_AuthorizationGroup_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "SEC003";
    private DBManager db = EAuthorizationGroup.db;
    public EAuthorizationGroup obj;

    protected int CurID = -1;
    private Binding binding;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;


        binding = new Binding(dbConn, db);
        binding.add(AuthorizationGroupID);
        binding.add(AuthorizationCode);
        binding.add(AuthorizationDesc);
        //binding.add(new CheckBoxBinder(db, AuthorizationGroupIsApproveEEInfo));
        //binding.add(new CheckBoxBinder(db, AuthorizationGroupIsApproveLeave));
        binding.add(new CheckBoxBinder(db, AuthorizationGroupIsReceiveOtherGrpAlert));
        binding.add(AuthorizationGroupEmailAddress);

        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["AuthorizationGroupID"], out CurID))
            CurID = -1;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (!Page.IsPostBack)
        {
            if (CurID > 0)
            {
                loadObject();
            }
            else
            {
                toolBar.DeleteButton_Visible = false;
            }

        }

    }

    protected bool loadObject()
    {
        EAuthorizationGroup obj = new EAuthorizationGroup();
        bool isNew = WebFormWorkers.loadKeys(EAuthorizationGroup.db, obj, DecryptedRequest);
        if (!EAuthorizationGroup.db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        EAuthorizationGroup.db.populate(obj, values);
        binding.toControl(values);
        return true;
    }

 
    protected void Save_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        EAuthorizationGroup obj = new EAuthorizationGroup();
        Hashtable values = new Hashtable();


        binding.toValues(values);
        db.validate(errors, values);

        if (!errors.isEmpty())
            return;

        db.parse(values, obj);
        if (!AppUtils.checkDuplicate(dbConn, db, obj, errors, "AuthorizationCode"))
            return;

        WebUtils.StartFunction(Session, FUNCTION_CODE);
        if (CurID < 0)
        {
            db.insert(dbConn, obj);
            CurID = obj.AuthorizationGroupID;
        }
        else
        {
            db.update(dbConn, obj);
        }
        WebUtils.EndFunction(dbConn);

         HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "ESS_AuthorizationGroup_View.aspx?AuthorizationGroupID=" + CurID);
    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        EAuthorizationGroup obj = new EAuthorizationGroup();
        obj.AuthorizationGroupID = CurID;
        if (db.select(dbConn, obj))
        {
            DBFilter workflowDetailFilter = new DBFilter();
            workflowDetailFilter.add(new Match("AuthorizationGroupID", obj.AuthorizationGroupID));

            DBFilter workflowFilter = new DBFilter();
            workflowFilter.add(new IN("AuthorizationWorkFlowID", "SELECT AuthorizationWorkFlowID FROM " + EAuthorizationWorkFlowDetail.db.dbclass.tableName, workflowDetailFilter));
            ArrayList workflowList = EAuthorizationWorkFlow.db.select(dbConn, workflowFilter);
            if (workflowList.Count > 0)
            {
                errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_IS_IN_USE, new string[] { HROne.Common.WebUtility.GetLocalizedString("Authorization Group") + ":" + obj.AuthorizationCode }));
                errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
                return;

            }
            else
            {
                DBFilter authorizerFilter = new DBFilter();
                authorizerFilter.add(new Match("AuthorizationGroupID", CurID));
                EAuthorizer.db.delete(dbConn, authorizerFilter);

                WebUtils.StartFunction(Session, FUNCTION_CODE);
                db.delete(dbConn, obj);
                WebUtils.EndFunction(dbConn);
            }
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "ESS_AuthorizationGroup_List.aspx");
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "ESS_AuthorizationGroup_View.aspx?AuthorizationGroupID=" + CurID);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "ESS_AuthorizationGroup_List.aspx");


    }
}
