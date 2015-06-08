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

public partial class ESS_AuthorizationWorkFlow_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "SEC005";
    private DBManager db = EAuthorizationWorkFlow.db;
    protected SearchBinding AuthorizationWorkFlowDetailSearchBinding;
    protected Binding newWorkFlowDetailBinding;
    protected int CurID = -1;
    private Binding binding;
    private DataTable AuthorizationWorkFlowDetailDataTable;
    private const string AuthorizationWorkFlowDetailListViewStateName = "AuthorizationWorkFlowDetailList";

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

        newWorkFlowDetailBinding = new Binding(dbConn, EAuthorizationWorkFlowDetail.db);
        newWorkFlowDetailBinding.add(new DropDownVLBinder(EAuthorizationWorkFlowDetail.db, AuthorizationGroupID, EAuthorizationGroup.VLAuthorizationGroupID));
        newWorkFlowDetailBinding.init(Request, Session);

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
            DBFilter AuthorizationWorkFlowDetailFilter = new DBFilter();
            AuthorizationWorkFlowDetailFilter.add(new Match("AuthorizationWorkFlowID", CurID));
            AuthorizationWorkFlowDetailFilter.add("AuthorizationWorkFlowIndex", true);
            AuthorizationWorkFlowDetailDataTable = CreateDummyAuthorizationWorkFlowDataTable();

            ArrayList detailList = EAuthorizationWorkFlowDetail.db.select(dbConn, AuthorizationWorkFlowDetailFilter);
            foreach (EAuthorizationWorkFlowDetail workFlowDetail in detailList)
            {
                DataRow row = AuthorizationWorkFlowDetailDataTable.NewRow();
                row["AuthorizationGroupID"] = workFlowDetail.AuthorizationGroupID;
                row["AuthorizationWorkFlowIndex"] = workFlowDetail.AuthorizationWorkFlowIndex;
                AuthorizationWorkFlowDetailDataTable.Rows.Add(row);
            }

            ViewState.Add(AuthorizationWorkFlowDetailListViewStateName, AuthorizationWorkFlowDetailDataTable);
        }
        else
            AuthorizationWorkFlowDetailDataTable = (DataTable)ViewState[AuthorizationWorkFlowDetailListViewStateName];
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        AuthorizationWorkFlowRepeater.DataSource = new DataView(AuthorizationWorkFlowDetailDataTable, string.Empty, ListFooter.ListOrderBy, DataViewRowState.CurrentRows);
        AuthorizationWorkFlowRepeater.DataBind();
        if (AuthorizationWorkFlowRepeater.Items.Count > 0)
        {
            AuthorizationWorkFlowRepeater.Items[0].FindControl("Up").Visible = false;
            AuthorizationWorkFlowRepeater.Items[AuthorizationWorkFlowRepeater.Items.Count - 1].FindControl("Down").Visible = false;
        }
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

    protected DataTable CreateDummyAuthorizationWorkFlowDataTable()
    {
        DataTable table = new DataTable();
        DataColumn IDColumn = new DataColumn("ID", typeof(long));
        IDColumn.AutoIncrement = true;
        IDColumn.AutoIncrementSeed = 1;
        table.Columns.Add(IDColumn);
        table.Columns.Add("AuthorizationGroupID", typeof(int));
        table.Columns.Add("AuthorizationWorkFlowIndex", typeof(int));

        return table;
    }
    protected void Add_Click(object sender, EventArgs e)
    {
        int authorizationGroupID = 0;
        if (int.TryParse(AuthorizationGroupID.SelectedValue, out authorizationGroupID))
        {
            DataRow row = AuthorizationWorkFlowDetailDataTable.NewRow();
            row["AuthorizationGroupID"] = authorizationGroupID;
            row["AuthorizationWorkFlowIndex"] = AuthorizationWorkFlowDetailDataTable.Rows.Count + 1;
            AuthorizationWorkFlowDetailDataTable.Rows.Add(row);
        }
    }

    protected void AuthorizationWorkFlowRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView rowView = (DataRowView)e.Item.DataItem;
            DataTable dummyTable = rowView.DataView.ToTable();
            ((Button)e.Item.FindControl("Delete")).CommandArgument = rowView["ID"].ToString();
            ((Button)e.Item.FindControl("Up")).CommandArgument = rowView["ID"].ToString();
            ((Button)e.Item.FindControl("Down")).CommandArgument = rowView["ID"].ToString();
            //e.Item.FindControl("Edit").Visible = IsAllowEdit;
            //HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("PositionID");
            //h.Value = ((DataRowView)e.Item.DataItem)["PositionID"].ToString();
        }
        HROne.Common.WebUtility.WebControlsLocalization(Session, e.Item.Controls);
    }

    protected void AuthorizationWorkFlowRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        Button b = (Button)e.CommandSource;

        int sourceID = int.Parse(b.CommandArgument);
        DataRow[] rowList = AuthorizationWorkFlowDetailDataTable.Select("ID = " + sourceID);
        if (rowList.Length > 0)
        {
            DataRow sourceRow = rowList[0];
            int sourceflowIndex = (int)sourceRow["AuthorizationWorkFlowIndex"];
            if (b.CommandName.Equals("UP", StringComparison.CurrentCultureIgnoreCase))
            {
                rowList = AuthorizationWorkFlowDetailDataTable.Select("AuthorizationWorkFlowIndex < " + sourceflowIndex, "AuthorizationWorkFlowIndex DESC");
                if (rowList.Length > 0)
                {
                    DataRow destinationRow = rowList[0];
                    int destinationflowIndex = (int)destinationRow["AuthorizationWorkFlowIndex"];
                    sourceRow["AuthorizationWorkFlowIndex"] = destinationflowIndex;
                    destinationRow["AuthorizationWorkFlowIndex"] = sourceflowIndex;
                }
            }
            else if (b.CommandName.Equals("DOWN", StringComparison.CurrentCultureIgnoreCase))
            {
                rowList = AuthorizationWorkFlowDetailDataTable.Select("AuthorizationWorkFlowIndex > " + sourceflowIndex, "AuthorizationWorkFlowIndex");
                if (rowList.Length > 0)
                {
                    DataRow destinationRow = rowList[0];
                    int destinationflowIndex = (int)destinationRow["AuthorizationWorkFlowIndex"];
                    sourceRow["AuthorizationWorkFlowIndex"] = destinationflowIndex;
                    destinationRow["AuthorizationWorkFlowIndex"] = sourceflowIndex;
                }
            }
            else if (b.CommandName.Equals("DELETE", StringComparison.CurrentCultureIgnoreCase))
            {
                AuthorizationWorkFlowDetailDataTable.Rows.Remove(sourceRow);
                rowList = AuthorizationWorkFlowDetailDataTable.Select("AuthorizationWorkFlowIndex > " + sourceflowIndex, "AuthorizationWorkFlowIndex");
                foreach (DataRow destinationRow in rowList)
                {
                    int destinationflowIndex = (int)destinationRow["AuthorizationWorkFlowIndex"];
                    destinationRow["AuthorizationWorkFlowIndex"] = destinationflowIndex - 1;
                }
            }
        }


        //if (b.ID.Equals("Edit"))
        //{
        //    Repeater.EditItemIndex = e.Item.ItemIndex;
        //    view = loadData(info, db, Repeater);
        //    WebUtils.SetEnabledControlSection(AddPanel, false);
        //}
        //else if (b.ID.Equals("Cancel"))
        //{
        //    Repeater.EditItemIndex = -1;
        //    view = loadData(info, db, Repeater);
        //    WebUtils.SetEnabledControlSection(AddPanel, true);
        //}
        //else if (b.ID.Equals("Save"))
        //{
        //    ebinding = new Binding(dbConn, db);
        //    ebinding.add((HtmlInputHidden)e.Item.FindControl("PositionID"));
        //    ebinding.add((TextBox)e.Item.FindControl("PositionCode"));
        //    ebinding.add((TextBox)e.Item.FindControl("PositionDesc"));
        //    ebinding.add((TextBox)e.Item.FindControl("PositionCapacity"));

        //    ebinding.init(Request, Session);


        //    EPosition obj = new EPosition();
        //    Hashtable values = new Hashtable();

        //    PageErrors errors = PageErrors.getErrors(db, Page.Master);
        //    errors.clear();


        //    ebinding.toValues(values);
        //    db.validate(errors, values);

        //    if (!errors.isEmpty())
        //        return;

        //    db.parse(values, obj);

        //    if (string.IsNullOrEmpty(obj.PositionCapacity))
        //    {
        //        obj.PositionCapacity = obj.PositionDesc.Trim();
        //        if (obj.PositionCapacity.Length > 40)
        //            obj.PositionCapacity = obj.PositionCapacity.Substring(0, 40).Trim();
        //    }
        //    if (!AppUtils.checkDuplicate(dbConn, db, obj, errors, "PositionCode"))
        //        return;

        //    WebUtils.StartFunction(Session, FUNCTION_CODE);
        //    db.update(dbConn, obj);
        //    WebUtils.EndFunction(dbConn);

        //    Repeater.EditItemIndex = -1;
        //    view = loadData(info, db, Repeater);
        //    WebUtils.SetEnabledControlSection(AddPanel, true);
        //}


    }

    protected void Save_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        EAuthorizationWorkFlow obj = new EAuthorizationWorkFlow();
        Hashtable values = new Hashtable();


        binding.toValues(values);
        db.validate(errors, values);

        if (!errors.isEmpty())
            return;

        db.parse(values, obj);
        if (!AppUtils.checkDuplicate(dbConn, db, obj, errors, "AuthorizationWorkFlowCode"))
            return;

        WebUtils.StartFunction(Session, FUNCTION_CODE);
        if (CurID < 0)
        {
            db.insert(dbConn, obj);
            CurID = obj.AuthorizationWorkFlowID;
        }
        else
        {
            db.update(dbConn, obj);
        }
        DBFilter previousDetailFilter = new DBFilter();
        previousDetailFilter.add(new Match("AuthorizationWorkFlowID", obj.AuthorizationWorkFlowID));
        ArrayList previousDetailList = EAuthorizationWorkFlowDetail.db.select(dbConn, previousDetailFilter);
        foreach (EAuthorizationWorkFlowDetail previousDetail in previousDetailList)
                EAuthorizationWorkFlowDetail.db.delete(dbConn, previousDetail);

        foreach (DataRow row in AuthorizationWorkFlowDetailDataTable.Rows)
        {
            EAuthorizationWorkFlowDetail detail = new EAuthorizationWorkFlowDetail();
            detail.AuthorizationGroupID = (int)row["AuthorizationGroupID"];
            detail.AuthorizationWorkFlowIndex = (int)row["AuthorizationWorkFlowIndex"];
            detail.AuthorizationWorkFlowID = obj.AuthorizationWorkFlowID;
            EAuthorizationWorkFlowDetail.db.insert(dbConn, detail);
        }
        WebUtils.EndFunction(dbConn);

        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/ESS_AuthorizationWorkFlow_View.aspx?AuthorizationWorkFlowID=" + CurID);
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
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/ESS_AuthorizationWorkFlow_View.aspx?AuthorizationWorkFlowID=" + CurID);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/ESS_AuthorizationWorkFlow_List.aspx");
    }
}
