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

public partial class ESS_AuthorizationWorkFlow_View : HROneWebPage
{
    private const string FUNCTION_CODE = "SEC005";
    private DBManager db = EAuthorizationWorkFlow.db;
    protected SearchBinding AuthorizationWorkFlowDetailSearchBinding;
    protected int CurID = -1;
    private Binding binding;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;


        binding = new Binding(dbConn, db);
        binding.add(AuthorizationWorkFlowID);
        binding.add(AuthorizationWorkFlowCode);
        binding.add(AuthorizationWorkFlowDescription);
        binding.init(Request, Session);


        AuthorizationWorkFlowDetailSearchBinding = new SearchBinding(dbConn, EAuthorizationWorkFlowDetail.db);
        AuthorizationWorkFlowDetailSearchBinding.initValues("AuthorizationGroupID", null, EAuthorizationGroup.VLAuthorizationGroupID, ci);
        AuthorizationWorkFlowDetailSearchBinding.init(DecryptedRequest, null);

        if (!int.TryParse(DecryptedRequest["AuthorizationWorkFlowID"], out CurID))
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

    protected void Page_PreRender(object sender, EventArgs e)
    {
        DBFilter AuthorizationWorkFlowDetailFilter = new DBFilter();
        AuthorizationWorkFlowDetailFilter.add(new Match("AuthorizationWorkFlowID", CurID));

        DataTable AuthorizationWorkFlowDetailDataTable = AuthorizationWorkFlowDetailFilter.loadData(dbConn, ListFooter.ListInfo, "*", " FROM "+ EAuthorizationWorkFlowDetail.db.dbclass.tableName);

        AuthorizationWorkFlowRepeater.DataSource = new DataView(AuthorizationWorkFlowDetailDataTable, string.Empty, ListFooter.ListOrderBy, DataViewRowState.CurrentRows);
        AuthorizationWorkFlowRepeater.DataBind();
        //if (AuthorizationWorkFlowRepeater.Items.Count > 0)
        //{
        //    AuthorizationWorkFlowRepeater.Items[0].FindControl("Up").Visible = false;
        //    AuthorizationWorkFlowRepeater.Items[AuthorizationWorkFlowRepeater.Items.Count - 1].FindControl("Down").Visible = false;
        //}
    }

    protected bool loadObject()
    {
        EAuthorizationWorkFlow obj = new EAuthorizationWorkFlow();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
        if (!db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);
        return true;
    }

    protected void AuthorizationWorkFlowRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView rowView = (DataRowView)e.Item.DataItem;
            DataTable dummyTable = rowView.DataView.ToTable();
            //e.Item.FindControl("Edit").Visible = IsAllowEdit;
            //HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("PositionID");
            //h.Value = ((DataRowView)e.Item.DataItem)["PositionID"].ToString();
        }
        HROne.Common.WebUtility.WebControlsLocalization(Session, e.Item.Controls);
    }



    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        EAuthorizationWorkFlow obj = new EAuthorizationWorkFlow();
        obj.AuthorizationWorkFlowID = CurID;
        if (db.select(dbConn, obj))
        {
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
                authorizationWorkFlowDetailFilter.add(new Match("AuthorizationWorkFlowID", CurID));
                EAuthorizationWorkFlowDetail.db.delete(dbConn, authorizationWorkFlowDetailFilter);

                WebUtils.StartFunction(Session, FUNCTION_CODE);
                db.delete(dbConn, obj);
                WebUtils.EndFunction(dbConn);
            }
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/ESS_AuthorizationWorkFlow_List.aspx");
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/ESS_AuthorizationWorkFlow_List.aspx");
    }

    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/ESS_AuthorizationWorkFlow_Edit.aspx?AuthorizationWorkFlowID=" + CurID);
    }
}
