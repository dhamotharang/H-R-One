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

public partial class Emp_LeaveBalance_List : HROneWebControl
{
    public int CurID = -1;

    protected SearchBinding sbinding;
    public DBManager sdb = ELeaveBalance.db;
    protected DataView view;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       

        sbinding = new SearchBinding(dbConn, sdb);
        //sbinding.initValues("LeaveTypeID", null, ELeaveType.VLLeaveType, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        sbinding.init(DecryptedRequest, null);


        if (!int.TryParse(DecryptedRequest["EmpID"], out CurID))
            CurID = -1;
        EmpID.Value = CurID.ToString();


    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //try
        {
            if (view == null)
                view = loadData(sdb, Repeater);
        }
    }
    public DataView loadData(DBManager db, Repeater repeater)
    {
        DateTime asOfDate = new DateTime();
        if (!DateTime.TryParseExact(AsOfDate.Value, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out asOfDate))
        {
            asOfDate = AppUtils.ServerDateTime().Date;
            AsOfDate.Value = asOfDate.ToString("yyyy-MM-dd");
        }
        LeaveBalanceCalc calc = new LeaveBalanceCalc(dbConn, CurID, asOfDate);

        ArrayList balanceItems = calc.getCurrentBalanceList();
        if (repeater != null)
        {
            repeater.DataSource = balanceItems;
            repeater.DataBind();
        }
        
        return view;
    }
 
    protected void ReCalc_Click(object sender, EventArgs e)
    {
        LeaveBalanceCalc calc = new LeaveBalanceCalc(dbConn, CurID);

        calc.Recalculate();

        view = loadData(sdb, Repeater);

    }
}
