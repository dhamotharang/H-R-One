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
using HROne.LeaveCalc;

public partial class Emp_LeaveApplication_List : HROneWebControl
{
    private const string FUNCTION_CODE = "PER009";
    private bool IsAllowEdit = true;

    public int CurID = -1;

    protected SearchBinding sbinding;
    public DBManager sdb = ELeaveApplication.db;
    protected ListInfo info;
    protected DataView view;
    
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
        {
            IsAllowEdit = false;
            AddPanel.Visible = false;
            AddPanelSelectAll.Visible = false;
        }

        Delete.OnClientClick = HROne.Translation.PromptMessage.CreateDeleteConfirmDialogJavascript(Delete);

        if (!int.TryParse(DecryptedRequest["EmpID"], out CurID))
            CurID = -1;
        EmpID.Value = CurID.ToString();


        DBFilter leaveYearFilter = new DBFilter();
        leaveYearFilter.add(new Match("EmpID", CurID));
        sbinding = new SearchBinding(dbConn, sdb);
        sbinding.add(new DropDownVLSearchBinder(Year, "Year(LeaveAppDateFrom)", ELeaveApplication.VLLeaveAppYear).setFilter(leaveYearFilter));//, null, "Year(pp.PayPeriodFR)"));
        // Start 0000011, Miranda, 2014-06-10
        sbinding.add(new DropDownVLSearchBinder(LeaveCode, "LeaveCodeID", ELeaveCode.VLLeaveCode).setFilter(new DBFilter()));
        // End 0000011, Miranda, 2014-06-10
        sbinding.initValues("LeaveAppUnit", null, Values.VLLeaveUnit, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        sbinding.initValues("LeaveCodeID", null, ELeaveCode.VLLeaveCode, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        //sbinding.initValues("LeaveApplicationID", null, ELeaveApplication.VLLeaveApplication, HROne.Common.WebUtility.GetSessionCultureInfo(Session));
        sbinding.init(DecryptedRequest, null);

        info = ListFooter.ListInfo;
        if (!IsPostBack)
        {
            ListFooter.ListOrderBy = "LeaveAppDateFrom";
            ListFooter.ListOrder = false;
        }

        
    }

    void Page_PreRender(object sender, EventArgs e)
    {
        if (view == null)
        {
            //loadState();
            view = loadData(info, sdb, Repeater);
        }
    }

    //public void loadState()
    //{
    //    info = new ListInfo();
    //    int page = 0;
    //    if (!CurPage.Value.Equals(""))
    //        page = Int32.Parse(CurPage.Value);
    //    info.loadState(Request, page);
    //    info.order = !Order.Value.Equals("false", StringComparison.CurrentCultureIgnoreCase);
    //    info.orderby = OrderBy.Value;
    //    if (string.IsNullOrEmpty(info.orderby))
    //        info.orderby = "LeaveAppDateFrom";
    //}
    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = sbinding.createFilter();
        filter.add(new Match("EmpID", this.CurID));
        OR leaveCancelIDOrTerm = new OR();
        leaveCancelIDOrTerm.add(new NullTerm("LeaveAppCancelID"));
        leaveCancelIDOrTerm.add(new Match("LeaveAppCancelID", "<=", 0));
        filter.add(leaveCancelIDOrTerm);

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);

        string select = "c.*";
        string from = "from [" + sdb.dbclass.tableName + "] c ";

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

    protected void FirstPage_Click(object sender, EventArgs e)
    {
        //loadState();
        //info.page = 0;
        view = loadData(info, sdb, Repeater);

    }
    protected void PrevPage_Click(object sender, EventArgs e)
    {
        //loadState();
        //info.page--;
        view = loadData(info, sdb, Repeater);

    }
    protected void NextPage_Click(object sender, EventArgs e)
    {
        //loadState();
        //info.page++;
        view = loadData(info, sdb, Repeater);

    }
    protected void LastPage_Click(object sender, EventArgs e)
    {
        //loadState();

        //info.page = Int32.Parse(NumPage.Value);
        view = loadData(info, sdb, Repeater);

    }
    protected void ChangeOrder_Click(object sender, EventArgs e)
    {
        LinkButton l = (LinkButton)sender;
        String id = l.ID.Substring(1);
        //loadState();
        if (info.orderby == null)
            info.order = true;
        else if (info.orderby.Equals(id))
            info.order = !info.order;
        else
            info.order = true;
        info.orderby = id;

        view = loadData(info, sdb, Repeater);

    }
    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(sdb, row, cb);

        ELeaveApplication obj=new ELeaveApplication();
        sdb.toObject(row.Row, obj);

        if (obj.EmpPaymentID != 0 || obj.EmpPayrollID != 0)
            cb.Visible = false;
        else
            cb.Visible = IsAllowEdit;
    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        //ArrayList list = new ArrayList();
        //foreach (RepeaterItem i in Repeater.Items)
        //{
        //    CheckBox cb = (CheckBox)i.FindControl("ItemSelect");
        //    if (cb.Checked)
        //    {
        //        ELeaveApplication o = new ELeaveApplication();
        //        WebFormUtils.GetKeys(sdb, o, cb);
        //        list.Add(o);
        //    }

        //}
        ArrayList list = WebUtils.SelectedRepeaterItemToBaseObjectList(sdb, Repeater, "ItemSelect");

        foreach (ELeaveApplication o in list)
        {

            if (sdb.select(dbConn, o))
            {
                //LeaveBalanceCalc leaaveBalCal = new LeaveBalanceCalc(dbConn, o.EmpID);
                WebUtils.StartFunction(Session, FUNCTION_CODE, o.EmpID);
                sdb.delete(dbConn, o);
                WebUtils.EndFunction(dbConn);
                //leaaveBalCal.RecalculateAfter(o.LeaveAppDateFrom);
            }
        }
        //loadState();
        loadData(info, sdb, Repeater);
    }
    protected void New_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_LeaveApplication_Edit.aspx?EmpID=" + EmpID.Value);
    }
    protected void Year_SelectedIndexChanged(object sender, EventArgs e)
    {
        view = loadData(info, sdb, Repeater);
    }
    // Start 0000011, Miranda, 2014-06-10
    protected void LeaveCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        view = loadData(info, sdb, Repeater);
    }
    // End 0000011, Miranda, 2014-06-10
}
