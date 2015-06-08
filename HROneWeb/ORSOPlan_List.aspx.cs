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

public partial class ORSOPlan_List : HROneWebPage
{
    private const string FUNCTION_CODE = "MPF004";

    protected DBManager db = EORSOPlan.db;
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
        binding.initValues("ORSOPlanIsResidual", null, Values.VLYesNo, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        binding.add(new LikeSearchBinder(ORSOPlanCode, "ORSOPlanCode"));
        binding.add(new LikeSearchBinder(ORSOPlanDesc, "ORSOPlanDesc"));


        binding.init(DecryptedRequest, null);

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
        info = ListFooter.ListInfo;

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
        info.page = 0;
        view = loadData(info, db, Repeater);

    }
    protected void Reset_Click(object sender, EventArgs e)
    {
        binding.clear();
        info.page = 0;
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
        e.Item.FindControl("ItemSelect").Visible = toolBar.DeleteButton_Visible;
    }
    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ArrayList list = WebUtils.SelectedRepeaterItemToBaseObjectList(db, Repeater, "ItemSelect");
        foreach (EORSOPlan o in list)
        {
            if (db.select(dbConn, o))
            {
                DBFilter empORSOFilter = new DBFilter();
                empORSOFilter.add(new Match("ORSOPlanID", o.ORSOPlanID));
                empORSOFilter.add("empid", true);
                ArrayList empORSOList = EEmpORSOPlan.db.select(dbConn, empORSOFilter);
                if (empORSOList.Count > 0)
                {
                    int curEmpID = 0;
                    errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_USED_BY_EMPLOYEE, new string[] { HROne.Common.WebUtility.GetLocalizedString("P-Fund Plan Code"), o.ORSOPlanCode }));
                    foreach (EEmpORSOPlan empORSOPlan in empORSOList)
                    {
                        EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                        empInfo.EmpID = empORSOPlan.EmpID;
                        if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                            if (curEmpID != empORSOPlan.EmpID)
                            {
                                errors.addError("- " + empInfo.EmpNo + ", " + empInfo.EmpEngFullName);
                                curEmpID = empORSOPlan.EmpID;
                            }
                            else
                                EEmpORSOPlan.db.delete(dbConn, empORSOPlan);

                    }
                    errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);

                }
                else
                {

                    WebUtils.StartFunction(Session, FUNCTION_CODE);
                    db.delete(dbConn, o);
                    DBFilter obj = new DBFilter();
                    obj.add(new Match("ORSOPlanID", o.ORSOPlanID));
                    ArrayList objList = EORSOPlanDetail.db.select(dbConn, obj);
                    foreach (EORSOPlanDetail match in objList)
                        EORSOPlanDetail.db.delete(dbConn, match);
                    WebUtils.EndFunction(dbConn);

                }
            }
        }
        loadData(info, db, Repeater);
    }
    public void New_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "ORSOPlan_Edit.aspx");
    }


}
