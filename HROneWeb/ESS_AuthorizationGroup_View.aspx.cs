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

public partial class ESS_AuthorizationGroup_View : HROneWebPage
{
    private const string FUNCTION_CODE = "SEC003";
    private const string AUTHORIZER_FUNCTION_CODE = "SEC004";
    public EAuthorizationGroup obj;
    private DBManager db = EAuthorizationGroup.db;
    private int CurID;
    protected Binding binding;
    protected SearchBinding SBinding;
    protected ListInfo info;
    private DataView view;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        AuthorizerPanel.Visible = WebUtils.CheckPermission(Session, AUTHORIZER_FUNCTION_CODE, WebUtils.AccessLevel.Read);
        btnAdd.Visible = WebUtils.CheckPermission(Session, AUTHORIZER_FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite);
        btnRemove.Visible = WebUtils.CheckPermission(Session, AUTHORIZER_FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite);
        SelectAllPanel.Visible = btnRemove.Visible;


        binding = new Binding(dbConn, db);
        binding.add(AuthorizationGroupID);
        binding.add(AuthorizationCode);
        binding.add(AuthorizationDesc);
        //binding.add(new LabelVLBinder(db, AuthorizationGroupIsApproveEEInfo, Values.VLTrueFalseYesNo));
        //binding.add(new LabelVLBinder(db, AuthorizationGroupIsApproveLeave, Values.VLTrueFalseYesNo));
        binding.add(new LabelVLBinder(db, AuthorizationGroupIsReceiveOtherGrpAlert, Values.VLTrueFalseYesNo));
        
        binding.add(AuthorizationGroupEmailAddress);
        binding.init(Request, Session);

        SBinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);
        SBinding.add(new HiddenMatchSearchBinder(AuthorizationGroupID));
        SBinding.initValues("EmpStatus", null, EEmpPersonalInfo.VLEmpStatus, null);
        SBinding.initValues("AuthorizerIsReadOnly", null, Values.VLYesNo, null);
        SBinding.initValues("AuthorizerSkipEmailAlert", null, Values.VLYesNo, null);
        SBinding.init(DecryptedRequest, null);

        if (!int.TryParse(DecryptedRequest["AuthorizationGroupID"], out CurID))
            CurID = -1;

        info = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (CurID > 0)
            {
                loadObject();
                view = loadData(info, db, Repeater);
            }
            else
            {
                loadObject();
                AuthorizerPanel.Visible = false;
                toolBar.EditButton_Visible = false;
            }

        }

    }

    protected bool loadObject()
    {
        obj = new EAuthorizationGroup();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
        if (!db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);
        return true;
    }

    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = new DBFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = " e.* , ar.*";
        string from = "from [" + EAuthorizer.db.dbclass.tableName + "] ar, EmpPersonalInfo e ";

        filter.add(new MatchField("ar.EmpID", "e.EmpID"));
        filter.add(new Match("AuthorizationGroupID", CurID));
        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);

        view = new DataView(table);

        if (repeater != null)
        {
            repeater.DataSource = table;
            repeater.DataBind();
        }

        return view;
    }
    protected void FirstPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);

    }
    protected void PrevPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);
    }
    protected void NextPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);
    }
    protected void LastPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);
    }
    protected void ChangeOrder_Click(object sender, EventArgs e)
    {
        LinkButton l = (LinkButton)sender;
        String id = l.ID.Substring(1);
        if (info.orderby == null)
            info.order = true;
        else if (info.orderby.Equals(id))
            info.order = !info.order;
        else
            info.order = true;
        info.orderby = id;

        view = loadData(info, db, Repeater);

    }
    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(EAuthorizer.db, row, cb);
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
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "ESS_AuthorizationGroup_List.aspx");
    }
    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/ESS_AuthorizationGroup_Edit.aspx?AuthorizationGroupID=" + CurID);
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/ESS_AuthorizationGroup_AddAuthorizer.aspx?AuthorizationGroupID=" + CurID);
    }
    protected void btnRemove_Click(object sender, EventArgs e)
    {
        ArrayList list = WebUtils.SelectedRepeaterItemToBaseObjectList(EAuthorizer.db, Repeater, "ItemSelect");
        foreach (EAuthorizer o in list)
        {
            if (EAuthorizer.db.select(dbConn, o))
            {
                WebUtils.StartFunction(Session, FUNCTION_CODE, o.EmpID);
                EAuthorizer.db.delete(dbConn, o);
                WebUtils.EndFunction(dbConn);
            }
        }
        view = loadData(info, db, Repeater);
    }
}
