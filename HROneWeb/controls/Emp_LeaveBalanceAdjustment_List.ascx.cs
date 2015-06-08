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

public partial class Emp_LeaveBalanceAdjustment_List : HROneWebControl
{
    private const string FUNCTION_CODE = "PER010";

    public int CurID = -1;

    protected SearchBinding sbinding;
    public DBManager sdb = ELeaveBalanceAdjustment.db;
    protected ListInfo info;
    protected DataView view;

    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
            toolBar.FunctionCode = FUNCTION_CODE;
            SelectAllPanel.Visible = toolBar.DeleteButton_Visible;

        PreRender += new EventHandler(Emp_LeaveBalanceAdjustment_List_PreRender);


        sbinding = new SearchBinding(dbConn, sdb);
        sbinding.initValues("LeaveBalAdjType", null, ELeaveBalanceAdjustment.VLLeaveBalAdjType, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        sbinding.initValues("LeaveTypeID", null, ELeaveType.VLLeaveType, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        sbinding.init(DecryptedRequest, null);

        if (!int.TryParse(DecryptedRequest["EmpID"], out CurID))
            CurID = -1;
        EmpID.Value = CurID.ToString();

        info = ListFooter.ListInfo;
        if (!IsPostBack)
        {
            ListFooter.ListOrderBy = "LeaveBalAdjDate";
            ListFooter.ListOrder = false;
        }

    }

    void Emp_LeaveBalanceAdjustment_List_PreRender(object sender, EventArgs e)
    {
        if (view == null)
        {
            view = loadData(info, sdb, Repeater);
        }
    }

    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpID", this.CurID));

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
        view = loadData(info, sdb, Repeater);

    }
    protected void PrevPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, sdb, Repeater);

    }
    protected void NextPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, sdb, Repeater);

    }
    protected void LastPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, sdb, Repeater);

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

        view = loadData(info, sdb, Repeater);

    }
    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(sdb, row, cb);
        e.Item.FindControl("ItemSelect").Visible = toolBar.DeleteButton_Visible;

        ELeaveBalanceAdjustment obj = new ELeaveBalanceAdjustment();
        sdb.toObject(row.Row, obj);


    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        //ArrayList list = new ArrayList();
        //foreach (RepeaterItem i in Repeater.Items)
        //{
        //    CheckBox cb = (CheckBox)i.FindControl("ItemSelect");
        //    if (cb.Checked)
        //    {
        //        ELeaveBalanceAdjustment o = new ELeaveBalanceAdjustment();
        //        WebFormUtils.GetKeys(sdb, o, cb);
        //        list.Add(o);
        //    }

        //}

        ArrayList list = WebUtils.SelectedRepeaterItemToBaseObjectList(sdb, Repeater, "ItemSelect");

        foreach (ELeaveBalanceAdjustment o in list)
        {
            if (sdb.select(dbConn, o))
            {
                WebUtils.StartFunction(Session, FUNCTION_CODE, o.EmpID);
                sdb.delete(dbConn, o);
                WebUtils.EndFunction(dbConn);
                //LeaveBalanceCalc leaaveBalCal = new LeaveBalanceCalc(dbConn, o.EmpID);
                //leaaveBalCal.RecalculateAfter(o.LeaveBalAdjDate, o.LeaveTypeID);
            }
        }
        loadData(info, sdb, Repeater);
    }

    public void New_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_LeaveBalanceAdjustment_Edit.aspx?EmpID=" + EmpID.Value);
    }
}
