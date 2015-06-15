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
using HROne.DataAccess;
using HROne.Lib.Entities;

public partial class Emp_OTHistory_Form : HROneWebControl
{
    protected DBManager sdb = ECompensationLeaveEntitle.db;
    protected SearchBinding sbinding;
    public Binding binding;
    protected ListInfo info;
    protected DataView view;
    public int CurID = -1;
    public int CurEntitleID = -1;
    public bool isOTClaim = true;

    protected void Page_Load(object sender, EventArgs e)
    {
        EESSUser user = WebUtils.GetCurUser(Session);
        if (user != null)
        {
            CurID = user.EmpID;
            EmpID.Value = CurID.ToString();
        }

        DBFilter leaveYearFilter = new DBFilter();
        leaveYearFilter.add(new Match("EmpID", CurID));
        leaveYearFilter.add(new Match("CompensationLeaveEntitleIsOTClaim", isOTClaim));
        sbinding = new SearchBinding(dbConn, sdb);
        sbinding.add(new DropDownVLSearchBinder(Year, "Year(CompensationLeaveEntitleEffectiveDate)", ECompensationLeaveEntitle.VLLeaveEntitleYear).setFilter(leaveYearFilter));//, null, "Year(pp.PayPeriodFR)"));

        sbinding.init(Request.QueryString, null);

        binding = new Binding(dbConn, sdb);
        binding.add(CompensationLeaveEntitleID);
        binding.add(EmpID);
        binding.add(CompensationLeaveEntitleEffectiveDate);
        binding.add(CompensationLeaveEntitleDateExpiry);
        binding.add(CompensationLeaveEntitleClaimPeriodFrom);
        binding.add(CompensationLeaveEntitleClaimPeriodTo);
        binding.add(CompensationLeaveEntitleClaimHourFrom);
        binding.add(CompensationLeaveEntitleClaimHourTo);
        binding.add(CompensationLeaveEntitleHoursClaim);
        binding.add(CompensationLeaveEntitleApprovedBy);
        binding.add(CompensationLeaveEntitleRemark);
        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["CompensationLeaveEntitleID"], out CurEntitleID))
            CurEntitleID = -1;

        info = ListFooter.ListInfo;
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (view == null)
            {
                view = loadData(info, sdb, Repeater);
            }
        }
        OTDetailsTable.Visible = CurEntitleID != -1;
    }

    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = sbinding.createFilter();
        filter.add(new Match("EmpID", this.CurID));
        filter.add(new Match("CompensationLeaveEntitleIsOTClaim", this.isOTClaim));
        //Start 0000210, Miranda, 2015-06-14
        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);
        //End 0000210, Miranda, 2015-06-14

        string select = "c.* ";
        string from = "from [" + sdb.dbclass.tableName + "] c";


        DataTable table = filter.loadData(dbConn, info, select, from);

        view = new DataView(table);

        ListFooter.Refresh();

        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }
        if (CurEntitleID != -1)
        {
            DBFilter detailFilter = sbinding.createFilter();
            detailFilter.add(new Match("CompensationLeaveEntitleID", this.CurEntitleID));
            ArrayList detailList = sdb.select(dbConn, detailFilter);
            if (detailList.Count > 0)
            {
                ECompensationLeaveEntitle detail = (ECompensationLeaveEntitle)detailList[0];
                Hashtable values = new Hashtable();
                sdb.populate(detail, values);

                binding.toControl(values);
            }
        }
        return view;

    }

    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        ECompensationLeaveEntitle obj = new ECompensationLeaveEntitle();
        ECompensationLeaveEntitle.db.toObject(row.Row, obj);

    }

    protected void Search_Click(object sender, EventArgs e)
    {
        info.page = 0;
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

    protected void Year_SelectedIndexChanged(object sender, EventArgs e)
    {
        view = loadData(info, sdb, Repeater);
    }

}
