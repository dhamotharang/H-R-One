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

public partial class Emp_PositionInfo_List : HROneWebControl
{
    public int CurID = -1;
    private const string FUNCTION_CODE = "PER007";
    protected SearchBinding sbinding;
    public DBManager sdb = EEmpPositionInfo.db;
    protected ListInfo info;
    protected DataView view;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            this.Visible = false;
        toolBar.FunctionCode = FUNCTION_CODE;
        SelectAllPanel.Visible = toolBar.DeleteButton_Visible;

        PreRender += new EventHandler(Emp_PositionInfo_List_PreRender);


        sbinding = new SearchBinding(dbConn, sdb);
        sbinding.init(DecryptedRequest, null);
        sbinding.initValues("CompanyID", null, ECompany.VLCompany, null);
        sbinding.initValues("PositionID", null, EPosition.VLPosition, null);
        sbinding.initValues("RankID", null, ERank.VLRank, null);
        sbinding.initValues("StaffTypeID", null, EStaffType.VLStaffType, null);
        sbinding.initValues("LeavePlanID", null, ELeavePlan.VLLeavePlan, null);
        sbinding.initValues("PayGroupID", null, EPayrollGroup.VLPayrollGroup, null);

        if (int.TryParse(DecryptedRequest["EmpID"], out CurID))
            EmpID.Value = CurID.ToString();
        else
            CurID = -1;

        info = ListFooter.ListInfo;
        if (!IsPostBack)
        {
            ListFooter.ListOrderBy = "EmpPosEffFr";
            ListFooter.ListOrder = false;
        }
    }

    void Emp_PositionInfo_List_PreRender(object sender, EventArgs e)
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
        //else
        //    filter.add("EmpPosEffFr", true);

        string select = "c.* ";
        string from = "from [" + sdb.dbclass.tableName + "] c ";

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, null);
        table = loadExtraData(table);
        table = WebUtils.DataTableSortingAndPaging(table, info);
        view = new DataView(table);
        ListFooter.Refresh();
        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }

        return view;
    }
    protected DataTable loadExtraData(DataTable sourceTable)
    {
        DataTable destTable = sourceTable.Copy();
        destTable.Columns.Add("CompanyCode", typeof(string));
        destTable.Columns.Add("BusinessHierarchy", typeof(string));
        destTable.Columns.Add("PositionCode", typeof(string));
        destTable.Columns.Add("RankCode", typeof(string));
        destTable.Columns.Add("StaffTypeCode", typeof(string));
        destTable.Columns.Add("LeavePlanCode", typeof(string));
        destTable.Columns.Add("PayGroupCode", typeof(string));

        foreach (DataRow row in destTable.Rows)
        {
            EEmpPositionInfo empPos = EEmpPositionInfo.GetObject(dbConn, row["EmpPosID"]);
            if (empPos != null)
            {
                ECompany company = ECompany.GetObject(dbConn, row["CompanyID"]);
                if (company != null)
                    row["CompanyCode"] = company.CompanyCode;

                row["BusinessHierarchy"] = empPos.GetBusinessHierarchyString(dbConn);

                EPosition position = EPosition.GetObject(dbConn, row["PositionID"]);
                if (position != null)
                    row["PositionCode"] = position.PositionCode;

                ERank rank = ERank.GetObject(dbConn, row["RankID"]);
                if (rank != null)
                    row["RankCode"] = rank.RankCode;

                EStaffType staffType = EStaffType.GetObject(dbConn, row["StaffTypeID"]);
                if (staffType != null)
                    row["StaffTypeCode"] = staffType.StaffTypeCode;

                ELeavePlan leavePlan = ELeavePlan.GetObject(dbConn, row["LeavePlanID"]);
                if (leavePlan != null)
                    row["LeavePlanCode"] = leavePlan.LeavePlanCode;

                EPayrollGroup payGroup = EPayrollGroup.GetObject(dbConn, (int)row["PayGroupID"]);
                if (payGroup != null)
                    row["PayGroupCode"] = payGroup.PayGroupCode;

            }
        }
        return destTable;
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


        EEmpPositionInfo obj = new EEmpPositionInfo();
        EEmpPositionInfo.db.toObject(((DataRowView)e.Item.DataItem).Row, obj);
        Hashtable values = new Hashtable();
        EEmpPositionInfo.db.populate(obj, values);

        Binding eBinding;
        eBinding = new Binding(dbConn, EEmpPositionInfo.db);
        eBinding.add(new BlankZeroLabelVLBinder(EEmpPositionInfo.db, (Label)e.Item.FindControl("CompanyID"), ECompany.VLCompany));
        eBinding.add(new BlankZeroLabelVLBinder(EEmpPositionInfo.db, (Label)e.Item.FindControl("PositionID"), EPosition.VLPosition));
        eBinding.add(new BlankZeroLabelVLBinder(EEmpPositionInfo.db, (Label)e.Item.FindControl("RankID"), ERank.VLRank));
        eBinding.add(new BlankZeroLabelVLBinder(EEmpPositionInfo.db, (Label)e.Item.FindControl("StaffTypeID"), EStaffType.VLStaffType));
        eBinding.add(new BlankZeroLabelVLBinder(EEmpPositionInfo.db, (Label)e.Item.FindControl("LeavePlanID"), ELeavePlan.VLLeavePlan));
        eBinding.add(new BlankZeroLabelVLBinder(EEmpPositionInfo.db, (Label)e.Item.FindControl("PayGroupID"), EPayrollGroup.VLPayrollGroup));



        eBinding.init(Request, Session);

        eBinding.toControl(values);

    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        ArrayList list = new ArrayList();
        foreach (RepeaterItem i in Repeater.Items)
        {
            CheckBox cb = (CheckBox)i.FindControl("ItemSelect");
            if (cb.Checked)
            {
                EEmpPositionInfo o = new EEmpPositionInfo();
                WebFormUtils.GetKeys(sdb, o, cb);
                list.Add(o);
            }

        }
        if (list.Count > 0)
        {
            WebUtils.StartFunction(Session, FUNCTION_CODE, CurID);
            foreach (EEmpPositionInfo o in list)
            {
                if (sdb.select(dbConn, o))
                {
                    sdb.delete(dbConn, o);
                    DBFilter filter = new DBFilter();
                    filter.add(new Match("EmpPosID", o.EmpPosID));
                    ArrayList existingHierarchyList = EEmpHierarchy.db.select(dbConn, filter);
                    foreach (EEmpHierarchy h in existingHierarchyList)
                        EEmpHierarchy.db.delete(dbConn, h);
                }
            }
            WebUtils.EndFunction(dbConn);
        }
        loadData(info, sdb, Repeater);
    }
    public void New_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_PositionInfo_Edit.aspx?EmpID=" + EmpID.Value);
    }
}
