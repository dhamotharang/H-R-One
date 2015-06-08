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
using HROne.Lib.Entities;
using HROne.DataAccess;
//using perspectivemind.validation;

public partial class Attendance_GenerateRecord_List : HROneWebPage
{
    private const string FUNCTION_CODE = "ATT007";
    protected SearchBinding empSBinding, sbinding;
    public DBManager db = EPayrollGroup.db;
    public EPayrollGroup obj;

    protected ListInfo empInfo;
    protected DataView empView;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;


        empSBinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);
        empSBinding.initValues("EmpStatus", null, EEmpPersonalInfo.VLEmpStatus, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        empSBinding.init(DecryptedRequest, null);

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
        empInfo = ListFooter.ListInfo;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            empView = emploadData(empInfo, EEmpPayroll.db, empRepeater);
        }

    }




    public DataView emploadData(ListInfo info, DBManager db, Repeater repeater)
    {

        DBFilter filter = new DBFilter();// empSBinding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = "* ";
        string from = "from [" + EEmpPersonalInfo.db.dbclass.tableName + "] e ";


        DBFilter empInfoFilter = EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime(), AppUtils.ServerDateTime());
        empInfoFilter.add(new MatchField("e.EmpID", "ee.EmpID"));
        DateTime dtPeriodFr, dtPeriodTo;
        if (DateTime.TryParse(PeriodFr.Value, out dtPeriodFr) && DateTime.TryParse(PeriodTo.Value, out dtPeriodTo))
        {
            empInfoFilter.add(new Match("EmpDateOfJoin", "<=", dtPeriodTo));
            DBFilter empTermFilter = new DBFilter();
            empTermFilter.add(new MatchField("ee.EmpID", "et.EmpID"));
            empTermFilter.add(new Match("et.EmpTermLastDate", "<", dtPeriodFr));
            empInfoFilter.add(new Exists(EEmpTermination.db.dbclass.tableName + " et ", empTermFilter, true));

        }
        else
        {
            btnGenerate.Visible = false;
            empView = null;
            repeater.DataSource = null;
            repeater.DataBind();

            return null;
        }

        filter.add(new Exists(EEmpPersonalInfo.db.dbclass.tableName + " ee", empInfoFilter));




        DataTable table = filter.loadData(dbConn, null, select, from);
        table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);

        if (table.Rows.Count != 0)
        {
            btnGenerate.Visible = true;
        }
        else
        {
            btnGenerate.Visible = false;
        }
        empView = new DataView(table);
        ListFooter.Refresh();

        if (repeater != null)
        {
            repeater.DataSource = empView;
            repeater.DataBind();
        }

        return empView;
    }
    //protected void empFirstPage_Click(object sender, EventArgs e)
    //{
    //    empView = emploadData(empInfo, EEmpPayroll.db, empRepeater);
    //}
    //protected void empPrevPage_Click(object sender, EventArgs e)
    //{
    //    empView = emploadData(empInfo, EEmpPayroll.db, empRepeater);
    //}
    //protected void empNextPage_Click(object sender, EventArgs e)
    //{
    //    empView = emploadData(empInfo, EEmpPayroll.db, empRepeater);
    //}
    //protected void empLastPage_Click(object sender, EventArgs e)
    //{
    //    empView = emploadData(empInfo, EEmpPayroll.db, empRepeater);
    //}
    protected void empChangeOrder_Click(object sender, EventArgs e)
    {
        LinkButton l = (LinkButton)sender;
        String id = l.ID.Substring(1);
        if (empInfo.orderby == null)
            empInfo.order = true;
        else if (empInfo.orderby.Equals(id))
            empInfo.order = !empInfo.order;
        else
            empInfo.order = true;
        empInfo.orderby = id;

        empView = emploadData(empInfo, EEmpPayroll.db, empRepeater);

    }
    protected void empRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(EEmpPersonalInfo.db, row, cb);

    }

    protected void Search_Click(object sender, EventArgs e)
    {
        empInfo.page = 0;

        empView = emploadData(empInfo, db, empRepeater);

    }
    protected void Reset_Click(object sender, EventArgs e)
    {
        EmployeeSearchControl1.Reset();
        empSBinding.clear();
        empInfo.page = 0;

        empView = emploadData(empInfo, db, empRepeater);
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        DateTime dtPeriodFr = new DateTime();
        DateTime dtPeriodTo = new DateTime();

        if (!(DateTime.TryParse(PeriodFr.Value, out dtPeriodFr) && DateTime.TryParse(PeriodTo.Value, out dtPeriodTo)))
        {
            errors.addError("Invalid Date Format"); 
        }

        if (errors.isEmpty())
        {

            ArrayList list = new ArrayList();
            list = WebUtils.SelectedRepeaterItemToBaseObjectList(EEmpPersonalInfo.db, empRepeater, "ItemSelect");

            if (list.Count > 0)
            {
                Session["GenerateAttendance_EmpList"] = list;
                //string strEmpIDList = string.Empty;
                //foreach (EEmpPersonalInfo o in list)
                //{
                //    if (strEmpIDList == string.Empty)
                //        strEmpIDList = ((EEmpPersonalInfo)o).EmpID.ToString();
                //    else
                //        strEmpIDList += "_" + ((EEmpPersonalInfo)o).EmpID.ToString();

                //}


                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Attendance_GenerateRecord_Process.aspx?"
                    + "PeriodFrom=" + dtPeriodFr.Ticks
                    + "&PeriodTo=" + dtPeriodTo.Ticks
                    + "&Total=" + list.Count
                    + "&NoTimeCardRecord=" + (NoTimeCardRecord.Checked ? "Y" : "N")
                    );

                //+ "&EmpID=" + strEmpIDList);


                //HROne.Taxation.TaxationGeneration.GenerationFormTaxation(int.Parse(TaxCompID.SelectedValue), int.Parse(YearSelect.SelectedValue), TaxFormType.SelectedValue, list);
                //errors.addError("Complete");
                //Response.Write("<script>alert('Completed'); </script>");
            }
        }
        emploadData(empInfo, db, empRepeater);
    }

    // Start 0000004, Miranda, 2014-06-19
    protected void PayPeriod_Changed(object sender, EventArgs e)
    {
        DateTime dtPeriodFr = new DateTime(), dtPeriodTo = new DateTime();
        DateTime.TryParse(PeriodFr.Value, out dtPeriodFr);
        DateTime.TryParse(PeriodTo.Value, out dtPeriodTo);
        if (!dtPeriodFr.Ticks.Equals(0) && !dtPeriodTo.Ticks.Equals(0))
        {
            if (dtPeriodFr >= dtPeriodTo)
            {
                if (sender == PeriodFr.TextBox)
                    PeriodTo.Value = dtPeriodFr.AddMonths(DefaultMonthPeriod).AddDays(-1).ToString("yyyy-MM-dd");
                else if (sender == PeriodTo.TextBox)
                    PeriodFr.Value = dtPeriodTo.AddDays(1).AddMonths(-DefaultMonthPeriod).ToString("yyyy-MM-dd");
            }
        }
        else
        {
            if (sender == PeriodFr.TextBox && !dtPeriodFr.Ticks.Equals(0) && dtPeriodTo.Ticks.Equals(0))
            {
                PeriodTo.Value = dtPeriodFr.AddMonths(DefaultMonthPeriod).AddDays(-1).ToString("yyyy-MM-dd");

            }
            else if (sender == PeriodTo.TextBox && !dtPeriodTo.Ticks.Equals(0) && dtPeriodFr.Ticks.Equals(0))
                PeriodFr.Value = dtPeriodTo.AddDays(1).AddMonths(-DefaultMonthPeriod).ToString("yyyy-MM-dd");
        }
    }

    public int DefaultMonthPeriod
    {
        set { hiddenFieldDefaultMonthPeriod.Value = value.ToString(); }
        get { return int.Parse(hiddenFieldDefaultMonthPeriod.Value); }
    }
    // End 0000004, Miranda, 2014-06-19

}
