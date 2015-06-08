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
using HROne.Translation;
using HROne.Lib.Entities;

public partial class ESS_AuthorizationWorkFlow_List : HROneWebPage
{
    private const string FUNCTION_CODE = "SEC005";

    protected DBManager db = EAuthorizationWorkFlow.db;
    protected SearchBinding sbinding;
    protected ListInfo info;
    protected DataView view;
    //public Binding binding;
    public Binding ebinding;
    public EAuthorizationWorkFlow obj;
    public int CurID = -1;
    private bool IsAllowEdit = true;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;
        if (!WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
        {
            //AddPanel.Visible = false;
            IsAllowEdit = false;
        }



        //binding = new Binding(dbConn, db);
        //binding.add(AuthorizationCode);
        //binding.add(AuthorizationDesc);
        //binding.add(new CheckBoxBinder(db, AuthorizationGroupIsApproveEEInfo));
        //binding.add(new CheckBoxBinder(db, AuthorizationGroupIsApproveLeave));
        //binding.add(new CheckBoxBinder(db, AuthorizationGroupIsReceiveOtherGrpAlert));
        //binding.init(Request, Session);

        sbinding = new SearchBinding(dbConn, db);
        sbinding.add(new LikeSearchBinder(AuthorizationWorkflowCode, "AuthorizationWorkflowCode"));
        // Start 0000028, Miranda, 2014-04-02
        //sbinding.add(new LikeSearchBinder(AuthorizationWorkflowDesc, "AuthorizationWorkflowDesc"));
        sbinding.add(new LikeSearchBinder(AuthorizationWorkflowDesc, "AuthorizationWorkflowDescription"));
        // End 0000028, Miranda, 2014-04-02
        
        sbinding.init(DecryptedRequest, null);

        info = ListFooter.ListInfo;
        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            view = loadData(info, db, Repeater);
        }

    }

    public DataView loadData(ListInfo info, DBManager db, DataList repeater)
    {
        DBFilter filter = sbinding.createFilter();

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);

        string select = "c.*";
        string from = "from " + db.dbclass.tableName + " c ";

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);

        view = new DataView(table);
        if (repeater != null)
        {
            repeater.DataSource = view;
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
    protected void Search_Click(object sender, EventArgs e)
    {
        info.page = 0;
        view = loadData(info, db, Repeater);

    }
    protected void Reset_Click(object sender, EventArgs e)
    {
        sbinding.clear();
        info.page = 0;
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

        Repeater.EditItemIndex = -1;
        view = loadData(info, db, Repeater);

    }
    protected void New_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "ESS_AuthorizationWorkflow_Edit.aspx");
    }


    protected void Repeater_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        e.Item.FindControl("DeleteItem").Visible = IsAllowEdit;
        if (e.Item.ItemIndex == Repeater.EditItemIndex)
        {
            ebinding = new Binding(dbConn, db);
            ebinding.add((HtmlInputHidden)e.Item.FindControl("AuthorizationWorkflowID"));
            ebinding.add((TextBox)e.Item.FindControl("AuthorizationWorkflowCode"));
            ebinding.add((TextBox)e.Item.FindControl("AuthorizationWorkflowDesc"));
            ebinding.init(Request, Session);


            EAuthorizationWorkFlow obj = new EAuthorizationWorkFlow();
            db.toObject(((DataRowView)e.Item.DataItem).Row, obj);
            Hashtable values = new Hashtable();
            db.populate(obj, values);
            ebinding.toControl(values);
        }
        else
        {
            //e.Item.FindControl("Edit").Visible = IsAllowEdit;
            HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("AuthorizationWorkFlowID");
            h.Value = ((DataRowView)e.Item.DataItem)["AuthorizationWorkFlowID"].ToString();
        }
        HROne.Common.WebUtility.WebControlsLocalization(Session, e.Item.Controls);
    }


    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ArrayList list = new ArrayList();
        foreach(DataListItem item in Repeater.Items) 
        {
            CheckBox c=(CheckBox)item.FindControl("DeleteItem");
            HtmlInputHidden h = (HtmlInputHidden)item.FindControl("AuthorizationWorkFlowID");
            if (c.Checked)
            {
                EAuthorizationWorkFlow obj = new EAuthorizationWorkFlow();
                obj.AuthorizationWorkFlowID = Int32.Parse(h.Value);
                list.Add(obj);
            }
        }
        foreach (EAuthorizationWorkFlow obj in list)
        {
            db.select(dbConn, obj);
            DBFilter empPosFilter = new DBFilter();
            OR orAuthorizationWorkFlow = new OR();
            orAuthorizationWorkFlow.add(new Match("AuthorizationWorkFlowIDLeaveApp", obj.AuthorizationWorkFlowID));
            orAuthorizationWorkFlow.add(new Match("AuthorizationWorkFlowIDEmpInfoModified", obj.AuthorizationWorkFlowID));
            empPosFilter.add(orAuthorizationWorkFlow);
            empPosFilter.add("empid", true);
            ArrayList empPosList = EEmpPositionInfo.db.select(dbConn, empPosFilter);
            if (empPosList.Count > 0)
            {
                errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_USED_BY_EMPLOYEE, new string[] { HROne.Common.WebUtility.GetLocalizedString("Authorization Workflow"), obj.AuthorizationWorkFlowCode }));
                foreach (EEmpPositionInfo empPos in empPosList)
                {
                    EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                    empInfo.EmpID = empPos.EmpID;
                    if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                        errors.addError("- " + empInfo.EmpNo + ", " + empInfo.EmpEngFullName);
                    else
                        EEmpPositionInfo.db.delete(dbConn, empPos);

                }
                errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
                return;

            }
            else
            {
                DBFilter authorizationWorkFlowDetailFilter = new DBFilter();
                authorizationWorkFlowDetailFilter.add(new Match("AuthorizationWorkFlowID", obj.AuthorizationWorkFlowID));
                EAuthorizationWorkFlowDetail.db.delete(dbConn, authorizationWorkFlowDetailFilter);

                WebUtils.StartFunction(Session, FUNCTION_CODE);
                db.delete(dbConn, obj);
                WebUtils.EndFunction(dbConn);
            }
        }
        view = loadData(info, db, Repeater);
    }
}
