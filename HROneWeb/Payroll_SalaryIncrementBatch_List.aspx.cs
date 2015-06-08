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

public partial class Payroll_SalaryIncrementBatch_List : HROneWebPage
{
    private const string FUNCTION_CODE = "PAY016";

    protected DBManager db = ESalaryIncrementBatch.db;
    protected SearchBinding binding;
    protected ListInfo info;
    protected DataView view;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        if (ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_PAYSCALE_POINT_SYSTEM) != "Y")
        {
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");
            return;
        }

        toolBar.FunctionCode = FUNCTION_CODE;
        //SelectAllPanel.Visible = toolBar.DeleteButton_Visible;

        binding = new SearchBinding(dbConn, db);
        //binding.add(new TextBoxBinder(db, AsAtDate.TextBox, AsAtDate.ID));
        
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
        //DBFilter filter = binding.createFilter();
        DBFilter filter = new DBFilter();
        DateTime m_datevalue;
        if (DateTime.TryParse(AsAtDate.Value, out m_datevalue))
        {
            filter.add(new Match("AsAtDate", m_datevalue));
        }

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);

        string select = "c.BatchID, c.AsAtDate, CASE c.DeferredBatch WHEN 1 THEN 'Yes' ELSE 'No' END as DeferredBatchDesc, " +
                        "CASE c.Status WHEN 'A' THEN 'Applied' " +
                                    "WHEN 'C' THEN 'Confirmed' " +
                                    "ELSE 'Open' END AS StatusDesc, " +
                      "COUNT(d.EmpID) AS EmpCount ";
        string from = "from PS_SalaryIncrementBatch c LEFT JOIN PS_SalaryIncrementBatchDetail d ON " +
                      "c.BatchID = d.BatchID ";

        filter.addGroupBy("c.BatchID, c.AsAtDate, c.DeferredBatch, c.Status");

        DataTable table = AppUtils.runSelectSQL(select, from, filter, dbConn);

        //DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);

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
        AsAtDate.TextBox.Text = "";
        view = loadData(info, db, Repeater);
    }
    protected void FirstPage_Click(object sender, EventArgs e)
    {
        //view = loadData(info, db, Repeater);
    }
    protected void PrevPage_Click(object sender, EventArgs e)
    {
        //view = loadData(info, db, Repeater);
    }
    protected void NextPage_Click(object sender, EventArgs e)
    {
        //view = loadData(info, db, Repeater);
    }
    protected void LastPage_Click(object sender, EventArgs e)
    {
        //view = loadData(info, db, Repeater);
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

        //view = loadData(info, db, Repeater);

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

        foreach (ESalaryIncrementBatch o in list)
        {
            if (ESalaryIncrementBatch.db.select(dbConn, o))
            {
                if (o.Status == ESalaryIncrementBatch.STATUS_OPEN)
                {
                    dbConn.BeginTransaction();

                    WebUtils.StartFunction(Session, FUNCTION_CODE);

                    DBFilter detailFilter = new DBFilter();
                    detailFilter.add(new Match("BatchID", o.BatchID));
                    foreach (ESalaryIncrementBatchDetail d in ESalaryIncrementBatchDetail.db.select(dbConn, detailFilter))
                    {
                        ESalaryIncrementBatchDetail.db.delete(dbConn, d);
                    }
                    
                    db.delete(dbConn, o);
                    WebUtils.EndFunction(dbConn);

                    dbConn.CommitTransaction();
                }
                else if (o.Status == ESalaryIncrementBatch.STATUS_CONFIRMED || o.Status == ESalaryIncrementBatch.STATUS_APPLIED)
                {
                    errors.addError("Cannot remove Salary Increment Batch if they are not in OPEN status");
                }
            }
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_SalaryIncrementBatch_List.aspx");
    }

    public void New_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_SalaryIncrementBatch_Edit.aspx");
    }
}
