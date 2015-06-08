using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using HROne.DataAccess;
//using perspectivemind.validation;
using HROne.Lib.Entities;
using HROne.Payroll;
using HROne.BankFile;

public partial class Report_Payroll_NetPaymentListByPaymentMethod_List : HROneWebPage
{

    private const string FUNCTION_CODE = "RPT207";

    protected SearchBinding empSBinding;
    protected SearchBinding payBatchSBinding;
    public DBManager db = EPayrollGroup.db;
    public EPayrollGroup obj;
    public int CurID = -1;
    public int CurPayPeriodID = -1;

    protected ListInfo empInfo;
    protected DataView empView;
    protected ListInfo payBatchInfo;
    protected DataView payBatchView;

    private static WFValueList VLUserName = new WFDBList(EUser.db, "UserID", "UserName");

    private bool IsAllowEdit = true;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        //if (!WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
        //{
        //    EmployeeSelectAllPanel.Visible = false;
        //    IsAllowEdit = false;
        //} 

        empSBinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);
        payBatchSBinding = new SearchBinding(dbConn, EPayrollGroup.db);
        payBatchSBinding.initValues("PayBatchFileGenBy", null, VLUserName, null);


        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        empInfo = EmpListFooter.ListInfo;
        payBatchInfo = PayBatchListFooter.ListInfo;


        if (!Page.IsPostBack)
        {
            //loadObject();
            //if (CurID > 0)
            //{
            //    panelPayPeriod.Visible = true;
            //    payBatchView = payBatchloadData(payBatchInfo, EPayrollBatch.db, payBatchRepeater);

            //    empView = emploadData(empInfo, EEmpPayroll.db, empRepeater);
            //}
            //else
            //    panelPayPeriod.Visible = false;
            panelPayPeriod.Visible = false;

        }

    }

    //protected bool loadObject()
    //{
    //    obj = new EPayrollGroup();
    //    bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
    //    if (!db.select(dbConn, obj))
    //        return false;

    //    Hashtable values = new Hashtable();
    //    db.populate(obj, values);
    //    binding.toControl(values);

    //    if (CurPayPeriodID <= 0)
    //    {
    //        CurPayPeriodID = obj.CurrentPayPeriodID;
    //    }

    //    if (obj.CurrentPayPeriodID > 0)
    //        panelBankFileInfo.Visible = true;
    //    else
    //        panelBankFileInfo.Visible = false;


    //    return true;
    //}



    public DataView payBatchloadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DateTime dtPeriodFr = new DateTime(), dtPeriodTo = new DateTime();

        if (DateTime.TryParse(PayPeriodFr.Value, out dtPeriodFr) && DateTime.TryParse(PayPeriodTo.Value, out dtPeriodTo))
        {

            DBFilter filter = payBatchSBinding.createFilter();

            //if (info != null && info.orderby != null && !info.orderby.Equals(""))
            //    filter.add(info.orderby, info.order);

            string select = "pg.PayGroupCode, pg.PayGroupDesc, pp.PayPeriodFr, pp.PayPeriodTo, pb.* ";
            string from = "from [" + EPayrollGroup.db.dbclass.tableName + "] pg, [" + EPayrollPeriod.db.dbclass.tableName + "] pp, [" + EPayrollBatch.db.dbclass.tableName + "] pb ";

            filter.add(new MatchField("pg.PayGroupID", "pp.PayGroupID"));
            filter.add(new Match("pp.PayPeriodFr", ">=", dtPeriodFr));
            filter.add(new Match("pp.PayPeriodTo", "<=", dtPeriodTo));


            DBFilter empPayrollFilter = new DBFilter();
            //empPayrollFilter.add(new MatchField("pb.payBatchID", "ep.payBatchID"));
            empPayrollFilter.add(new Match("ep.EmpPayStatus", "C"));
            empPayrollFilter.add(new MatchField("ep.PayPeriodID", "pp.PayPeriodID"));

            DBFilter payRecordFilter = new DBFilter();
            //payRecordFilter.add(new MatchField("ep.EmpPayrollID", "pr.EmpPayrollID"));

            //Exists existsPayRec = new Exists(EPaymentRecord.db.dbclass.tableName + " pr", payRecordFilter);
            //empPayrollFilter.add(existsPayRec);

            //Exists exists = new Exists("EmpPayroll ep", empPayrollFilter);
            //        IN existsPayRec = new IN("ep.EmpPayrollID", "Select distinct pr.EmpPayrollID from (select EmpPayrollID,PayRecMethod from " + EPaymentRecord.db.dbclass.tableName + " group by EmpPayrollID, PayRecMethod) pr", payRecordFilter);
            IN existsPayRec = new IN("ep.EmpPayrollID", "Select distinct pr.EmpPayrollID from (select distinct EmpPayrollID,PayRecMethod from " + EPaymentRecord.db.dbclass.tableName + ") pr", payRecordFilter);
            empPayrollFilter.add(existsPayRec);

            IN exists = new IN("pb.payBatchID", "Select distinct ep.payBatchID from " + EEmpPayroll.db.dbclass.tableName + " ep", empPayrollFilter);


            filter.add(exists);
            DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, payBatchInfo);

            if (table.Rows.Count != 0)
            {
                panelPayPeriod.Visible = true;
                ReportExportControl1.Visible = true;
                ReportExportControl2.Visible = true;
            }
            else
            {
                panelPayPeriod.Visible = false;
                ReportExportControl1.Visible = false;
                ReportExportControl2.Visible = false;
            }
            payBatchView = new DataView(table);

            PayBatchListFooter.Refresh();

            if (repeater != null)
            {
                repeater.DataSource = payBatchView;
                repeater.DataBind();
            }

            return payBatchView;
        }
        else
        {
            panelPayPeriod.Visible = false;
            ReportExportControl1.Visible = false;
            ReportExportControl2.Visible = false;
            return null;
        }
    }
    protected void payBatchChangeOrder_Click(object sender, EventArgs e)
    {
        LinkButton l = (LinkButton)sender;
        String id = l.ID.Substring(1);
        if (payBatchInfo.orderby == null)
            payBatchInfo.order = true;
        else if (payBatchInfo.orderby.Equals(id))
            payBatchInfo.order = !payBatchInfo.order;
        else
            payBatchInfo.order = true;
        payBatchInfo.orderby = id;

        payBatchView = payBatchloadData(payBatchInfo, EPayrollBatch.db, payBatchRepeater);

    }
    protected void payBatchRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(EPayrollBatch.db, row, cb);
        cb.Checked = true;
    }

    public DataView emploadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DateTime dtPeriodFr = new DateTime(), dtPeriodTo = new DateTime();

        if (DateTime.TryParse(PayPeriodFr.Value, out dtPeriodFr) && DateTime.TryParse(PayPeriodTo.Value, out dtPeriodTo))
        {
            DBFilter filter = empSBinding.createFilter();

            //if (info != null && info.orderby != null && !info.orderby.Equals(""))
            //    filter.add(info.orderby, info.order);

            string select = "* ";
            string from = "from [" + EEmpPersonalInfo.db.dbclass.tableName + "] e ";

            DBFilter empPayrollFilter = new DBFilter();
            //empPayrollFilter.add(new MatchField("e.EmpID", "ep.EmpID"));
            //empPayrollFilter.add(WebUtils.AddRankFilter(Session, "e.EmpID", true));

            empPayrollFilter.add(new Match("ep.EmpPayStatus", "C"));
            empPayrollFilter.add(new MatchField("ep.PayPeriodID", "pp.PayPeriodID"));
            empPayrollFilter.add(new Match("pp.PayPeriodFr", ">=", dtPeriodFr));
            empPayrollFilter.add(new Match("pp.PayPeriodTo", "<=", dtPeriodTo));

            DBFilter payRecordFilter = new DBFilter();
            //payRecordFilter.add(new MatchField("ep.EmpPayrollID", "pr.EmpPayrollID"));


            //        IN existsPayRec = new IN("ep.EmpPayrollID", "Select distinct pr.EmpPayrollID from (select EmpPayrollID,PayRecMethod from " + EPaymentRecord.db.dbclass.tableName + " group by EmpPayrollID, PayRecMethod) pr", payRecordFilter);
            IN existsPayRec = new IN("ep.EmpPayrollID", "Select distinct pr.EmpPayrollID from (select distinct EmpPayrollID,PayRecMethod from " + EPaymentRecord.db.dbclass.tableName + " ) pr", payRecordFilter);
            empPayrollFilter.add(existsPayRec);

            IN exists = new IN("e.EmpID", "Select distinct ep.EmpID from " + EEmpPayroll.db.dbclass.tableName + " ep, " + EPayrollPeriod.db.dbclass.tableName + " pp", empPayrollFilter);

            filter.add(exists);

            // Start 0000185, KuangWei, 2015-04-21
            //filter.add(WebUtils.AddRankFilter(Session, "e.EmpID", true));
            DBFilter empInfoFilter = EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime(), AppUtils.ServerDateTime());
            empInfoFilter.add(new MatchField("e.EmpID", "ee.EmpID"));
            filter.add(new Exists(EEmpPersonalInfo.db.dbclass.tableName + " ee", empInfoFilter));

            DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);
            table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);
            // End 0000185, KuangWei, 2015-04-21


            if (table.Rows.Count != 0)
            {
                panelPayPeriod.Visible = true;
                ReportExportControl1.Visible = true;
                ReportExportControl2.Visible = true;
            }
            else
            {
                panelPayPeriod.Visible = false;
                ReportExportControl1.Visible = false;
                ReportExportControl2.Visible = false;
            }
            empView = new DataView(table);

            EmpListFooter.Refresh();

            if (repeater != null)
            {
                repeater.DataSource = empView;
                repeater.DataBind();
            }

            return empView;
        }
        else
        {
            panelPayPeriod.Visible = false;
            ReportExportControl1.Visible = false;
            ReportExportControl2.Visible = false;
            return null;
        }
    }
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
        cb.Visible = IsAllowEdit;
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ArrayList empList = new ArrayList();
        ArrayList payBatchList = new ArrayList();

        foreach (RepeaterItem i in empRepeater.Items)
        {
            CheckBox cb = (CheckBox)i.FindControl("ItemSelect");
            if (cb.Checked)
            {
                EEmpPersonalInfo o = (EEmpPersonalInfo)EEmpPersonalInfo.db.createObject();
                WebFormUtils.GetKeys(EEmpPersonalInfo.db, o, cb);
                empList.Add(o);
            }

        }
        foreach (RepeaterItem i in payBatchRepeater.Items)
        {
            CheckBox cb = (CheckBox)i.FindControl("ItemSelect");
            if (cb.Checked)
            {
                EPayrollBatch o = (EPayrollBatch)EPayrollBatch.db.createObject();
                WebFormUtils.GetKeys(EPayrollBatch.db, o, cb);
                payBatchList.Add(o);
            }

        }

        //string strEmpIDList = string.Empty;
        //string strPayBatchIDList = string.Empty;

        if (empList.Count > 0 && payBatchList.Count > 0)
        {

            //foreach (EEmpPersonalInfo o in empList)
            //{
            //    if (strEmpIDList == string.Empty)
            //        strEmpIDList = o.EmpID.ToString();
            //    else
            //        strEmpIDList += "_" + o.EmpID.ToString();

            //}
            //foreach (EPayrollBatch  o in payBatchList)
            //{
            //    if (strPayBatchIDList == string.Empty)
            //        strPayBatchIDList = o.PayBatchID.ToString();
            //    else
            //        strPayBatchIDList += "_" + o.PayBatchID.ToString();

            //}
        }
        else 
            errors.addError("Employee or Payroll Batch not selected");

        if (errors.isEmpty())
        {
            HROne.Reports.Payroll.NetPaymentListByPaymentMethodProcess rpt = new HROne.Reports.Payroll.NetPaymentListByPaymentMethodProcess(dbConn, empList, payBatchList, chkAutoPay.Checked, chkCheque.Checked, chkCash.Checked, chkOthers.Checked);

            string reportFileName = WebUtils.GetLocalizedReportFile(Server.MapPath("~/Report_Payroll_NetPaymentListByPaymentMethod.rpt"));

            WebUtils.ReportExport(dbConn, user, errors, lblReportHeader.Text, Response, rpt, reportFileName, ((Button)sender).CommandArgument, "NetPaymentList", true);

        }


    }
    protected void Search_Click(object sender, EventArgs e)
    {
        payBatchView = payBatchloadData(payBatchInfo, EPayrollBatch.db, payBatchRepeater);

        empView = emploadData(empInfo, EEmpPayroll.db, empRepeater);

    }

    // Start 0000004, Miranda, 2014-06-19
    protected void PayPeriod_Changed(object sender, EventArgs e)
    {
        DateTime dtPeriodFr = new DateTime(), dtPeriodTo = new DateTime();
        DateTime.TryParse(PayPeriodFr.Value, out dtPeriodFr);
        DateTime.TryParse(PayPeriodTo.Value, out dtPeriodTo);
        if (!dtPeriodFr.Ticks.Equals(0) && !dtPeriodTo.Ticks.Equals(0))
        {
            if (dtPeriodFr >= dtPeriodTo)
            {
                if (sender == PayPeriodFr.TextBox)
                    PayPeriodTo.Value = dtPeriodFr.AddMonths(DefaultMonthPeriod).AddDays(-1).ToString("yyyy-MM-dd");
                else if (sender == PayPeriodTo.TextBox)
                    PayPeriodFr.Value = dtPeriodTo.AddDays(1).AddMonths(-DefaultMonthPeriod).ToString("yyyy-MM-dd");
            }
        }
        else
        {
            if (sender == PayPeriodFr.TextBox && !dtPeriodFr.Ticks.Equals(0) && dtPeriodTo.Ticks.Equals(0))
            {
                PayPeriodTo.Value = dtPeriodFr.AddMonths(DefaultMonthPeriod).AddDays(-1).ToString("yyyy-MM-dd");

            }
            else if (sender == PayPeriodTo.TextBox && !dtPeriodTo.Ticks.Equals(0) && dtPeriodFr.Ticks.Equals(0))
                PayPeriodFr.Value = dtPeriodTo.AddDays(1).AddMonths(-DefaultMonthPeriod).ToString("yyyy-MM-dd");
        }
    }

    public int DefaultMonthPeriod
    {
        set { hiddenFieldDefaultMonthPeriod.Value = value.ToString(); }
        get { return int.Parse(hiddenFieldDefaultMonthPeriod.Value); }
    }
    // End 0000004, Miranda, 2014-06-19

    // Start 0000185, KuangWei, 2015-04-21
    protected void EmployeeSearch_Click(object sender, EventArgs e)
    {
        empView = emploadData(empInfo, EEmpPayroll.db, empRepeater);
    }

    protected void Reset_Click(object sender, EventArgs e)
    {
        empSBinding.clear();
        EmployeeSearchControl1.Reset();
        EmployeeSearchControl1.EmpStatusValue = "A";
        empView = emploadData(empInfo, EEmpPayroll.db, empRepeater);
    }
    // End 0000185, KuangWei, 2015-04-21  
}
