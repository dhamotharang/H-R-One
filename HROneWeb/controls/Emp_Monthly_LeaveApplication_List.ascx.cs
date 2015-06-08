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

public partial class Emp_Monthly_LeaveApplication_List : HROneWebControl
{
    private const string FUNCTION_CODE = "PER009";

    public int CurID = -1;
    public DateTime dtDateFr = DateTime.Now;

    protected SearchBinding sbinding;
    public DBManager sdb = ELeaveApplication.db;
    protected ListInfo info;
    protected DataView view;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!int.TryParse(DecryptedRequest["EmpID"], out CurID))
            CurID = -1;
        EmpID.Value = CurID.ToString();

        sbinding = new SearchBinding(dbConn, sdb);
        sbinding.initValues("LeaveAppUnit", null, Values.VLLeaveUnit, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        sbinding.init(DecryptedRequest, null);
    }

    void Page_PreRender(object sender, EventArgs e)
    {
        if (view == null)
        {
            view = loadData(info, sdb, Repeater);
        }
    }

    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = sbinding.createFilter();
        filter.add(new Match("EmpID", this.CurID));

        DateTime startMonth = dtDateFr.AddDays(1 - dtDateFr.Day);
        DateTime endMonth = startMonth.AddMonths(1).AddDays(-1);

        filter.add(new Match("LeaveAppDateFrom", ">=", startMonth));
        filter.add(new Match("LeaveAppDateFrom", "<=", endMonth));

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);

        string select = "c.*, lc.LeaveCodeDesc";
        string from = "from [" + sdb.dbclass.tableName + "] c LEFT JOIN " +
            ELeaveCode.db.dbclass.tableName + " lc ON c.LeaveCodeID=lc.LeaveCodeID";

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);

        view = new DataView(table);

        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }

        return view;
    }

    public DateTime dtDateFrom
    {
        set { dtDateFr = value; }
    }
}
