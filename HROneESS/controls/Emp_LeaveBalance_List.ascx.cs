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

    protected SearchBinding sbinding;
    public DBManager sdb = ELeaveBalance.db;
    protected DataView view;

    protected void Page_Load(object sender, EventArgs e)
    {
        sbinding = new SearchBinding(dbConn, sdb);
        sbinding.initValues("LeaveTypeID", null, ELeaveType.VLLeaveType, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        sbinding.init(Request.QueryString, null);

//        m_isSimpleView.Value = "true";
//        m_isViewChangeable.Value = "true";

        btnCollapseView.Visible = isViewChangeable;
        btnExpandView.Visible = isViewChangeable;
    }

    public DateTime AsOfDate
    {
        get
        {
            DateTime asOfDate = new DateTime();
            if (!DateTime.TryParseExact(txtAsOfDate.Value, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out asOfDate))
            {
                asOfDate = AppUtils.ServerDateTime().Date;
                txtAsOfDate.Value = asOfDate.ToString("yyyy-MM-dd");
            }
            return asOfDate;
        }
        set
        {
            txtAsOfDate.Value = value.ToString("yyyy-MM-dd");
        }
    }

    public string ShowHideStyle
    {
        get
        {
            if (isSimpleView == true)
                return "display:none;";
            else
                return "font-size:10px;";
        }
    }

    public bool isSimpleView
    {
        get
        {
            return (m_isSimpleView.Value == "true");
        }
        set
        {

            m_isSimpleView.Value = (value) ? "true" : "false";
        }
    }

    public bool isViewChangeable
    {
        get
        {
            return (m_isViewChangeable.Value == "true");
        }
        set
        {
            //btnExpand.visible = false;
            m_isViewChangeable.Value = (value) ? "true" : "false";
        }
    }


    public int EmpID
    {
        get
        {
            int intEmpID = 0;
            if (int.TryParse(CurrentEmpID.Value, out intEmpID))
                return intEmpID;
            return 0;
        }
        set
        {
            CurrentEmpID.Value = value.ToString();
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (view == null)
                view = loadData(sdb, Repeater);

            //isSimpleView = true;
        }
    }

    public DataView loadData(DBManager db, Repeater repeater)
    {
        DateTime dtAsOfDate = AsOfDate;


        LeaveBalanceCalc calc = new LeaveBalanceCalc(dbConn, EmpID, dtAsOfDate);

        ArrayList balanceItems = calc.getCurrentBalanceList(true);
        if (repeater != null)
        {
            repeater.DataSource = balanceItems;
            repeater.DataBind();
        }

        return view;

    }

    protected void Go_Click(object sender, EventArgs e)
    {
        view = loadData(sdb, Repeater);

    }

    protected void btnExpandView_Click(object sender, EventArgs e)
    {
        m_isSimpleView.Value = "false";

    }
    protected void btnCollapseView_Click(object sender, EventArgs e)
    {
        m_isSimpleView.Value = "true";

    }

}
