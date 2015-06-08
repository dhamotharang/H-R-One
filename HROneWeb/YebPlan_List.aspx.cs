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

public partial class YEBPlan_List : HROneWebPage
{
    private const string FUNCTION_CODE = "SYS007";

    protected DBManager db = EYEBPlan.db;
    protected SearchBinding binding;
    protected ListInfo info;
    protected DataView view;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;
        SelectAllPanel.Visible = toolBar.DeleteButton_Visible;

        

        binding = new SearchBinding(dbConn, db);
        binding.add(new LikeSearchBinder(YEBPlanCode, "YEBPlanCode"));
        binding.add(new LikeSearchBinder(YEBPlanDesc, "YEBPlanDesc"));

        binding.init(DecryptedRequest, null);

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

    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = binding.createFilter();


        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);

        string select = "c.*";
        string from = "from [" + db.dbclass.tableName + "] c ";

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);

        view = new DataView(table);

        ListFooter.Refresh();

        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }
        return view;
    }
    protected void Search_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);

    }
    protected void Reset_Click(object sender, EventArgs e)
    {
        binding.clear();
        view = loadData(info, db, Repeater);

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


        WebFormUtils.LoadKeys(db, row, cb);
    }
    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ArrayList list = WebUtils.SelectedRepeaterItemToBaseObjectList(db, Repeater, "ItemSelect");

        foreach (EYEBPlan o in list)
        {
            if (db.select(dbConn, o))
            {
                DBFilter empYEBPlanFilter = new DBFilter();
                empYEBPlanFilter.add(new Match("YEBPlanID", o.YEBPlanID));
                empYEBPlanFilter.add("empid", true);
                ArrayList empPosList = EEmpPositionInfo.db.select(dbConn, empYEBPlanFilter);
                if (empPosList.Count > 0)
                {
                    int curEmpID = 0;
                    errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_USED_BY_EMPLOYEE, new string[] { HROne.Common.WebUtility.GetLocalizedString("YEB Plan Code"), o.YEBPlanCode }));
                    foreach (EEmpPositionInfo empPos in empPosList)
                    {
                        EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                        empInfo.EmpID = empPos.EmpID;
                        if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                            if (curEmpID != empPos.EmpID)
                            {
                                errors.addError("- " + empInfo.EmpNo + ", " + empInfo.EmpEngFullName);
                                curEmpID = empPos.EmpID;
                            }
                            else
                                EEmpPositionInfo.db.delete(dbConn, empPos);

                    }
                    errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
                }
                else
                {
                    WebUtils.StartFunction(Session, FUNCTION_CODE);
                    db.delete(dbConn, o);
                    WebUtils.EndFunction(dbConn);

                }
            }
        }
        loadData(info, db, Repeater);
    }
    public void New_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "YEBPlan_Edit.aspx");
    }

}
