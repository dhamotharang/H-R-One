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
//using perspectivemind.validation;
using HROne.Lib.Entities;
using HROne.Import;

public partial class Payroll_HitRateProcess_Generate_CND : HROneWebPage
{
    protected DBManager db = EHitRateProcess.db;
    protected SearchBinding sbinding;
    protected ListInfo info;
    protected DataView view;
    private const string FUNCTION_CODE = "PAY022";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        sbinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);
        //sbinding.add(new DropDownVLSearchBinder(BatchID, "HitRateProcessImportBatchID", EHitRateProcessImportBatch.VLBatchID, false));
        sbinding.init(DecryptedRequest, null);

        info = ListFooter.ListInfo;

        if (!Page.IsPostBack)
        {
            init_BatchIDDropdown();
            //init_PaymentCodeDropdown();
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //if (!Page.IsPostBack)
        {
            //EmpStatus.SelectedValue = "A"
            
            EmployeeSearchControl1.EmpStatusValue = "A";
            view = loadData(info, db, Repeater);
        }
    }

    //public DataView loadData2(ListInfo info, DBManager db, Repeater repeater)
    //{
    //    DBFilter filter = new DBFilter();
    //    filter.add(WebUtils.AddRankFilter(Session, "e.EmpID", true));

    //    int m_batchID = -1;

    //    if (!int.TryParse(BatchID.SelectedValue, out m_batchID))
    //    {
    //        m_batchID = -1;
    //    }

    //    DBFilter m_batchFilter = new DBFilter();
    //    m_batchFilter.add(new Match("HitRateProcessImportBatchID", m_batchID));

    //    filter.add(new IN("EmpID", "SELECT EmpID FROM HitRateProcess ", m_batchFilter));

    //    string select = "e.* ";
    //    string from = "from [" + db.dbclass.tableName + "] e ";

    //    DBFilter empInfoFilter = EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime(), AppUtils.ServerDateTime());
    //    empInfoFilter.add(new MatchField("e.EmpID", "ee.EmpID"));
    //    filter.add(new Exists(EEmpPersonalInfo.db.dbclass.tableName + " ee", empInfoFilter));

    //    DataTable table = filter.loadData(dbConn, null, select, from);
    //    table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);
    //    view = new DataView(table);

    //    ListFooter.Refresh();
    //    if (repeater != null)
    //    {
    //        repeater.DataSource = view;
    //        repeater.DataBind();
    //    }
    //    return view;
    //}

    public DataView loadData(ListInfo info, DBManager db, DataList repeater)
    {
        DBFilter filter = new DBFilter();  // sbinding.createFilter();

        int m_batchID = -1;

        int.TryParse(BatchID.SelectedValue, out m_batchID);

        filter.add(new Match("c.HitRateProcessImportBatchID", m_batchID));
        filter.add(new MatchField("c.EmpID", "e.EmpID"));

        DataTable table = filter.loadData(dbConn, null, "e.*, c.* ", " from " + EHitRateProcess.db.dbclass.tableName + " c, " + EEmpPersonalInfo.db.dbclass.tableName + " e ");

        if (info != null)
            if (!string.IsNullOrEmpty(info.orderby))
                if (info.orderby.Equals("EmpEngFullName", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (!table.Columns.Contains("EmpEngFullName"))
                    {
                        table.Columns.Add("EmpEngFullName", typeof(string));
                        foreach (System.Data.DataRow row in table.Rows)
                        {
                            EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                            empInfo.EmpID = (int)row["EmpID"];
                            if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                                row["EmpEngFullName"] = empInfo.EmpEngFullName;
                        }
                    }
                }
        table = WebUtils.DataTableSortingAndPaging(table, info);

        view = new DataView(table);
        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }

        return view;
    }

    protected void Search_Click(object sender, EventArgs e)
    {
        info.page = 0;
        view = loadData(info, db, Repeater);
    }
    protected void Reset_Click(object sender, EventArgs e)
    {
        sbinding.clear();
        EmployeeSearchControl1.Reset();
        EmployeeSearchControl1.EmpStatusValue = "A";

        info.page = 0;
        view = loadData(info, db, Repeater);
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

        view = loadData(info, db, Repeater);
    }

    protected void Repeater_ItemDataBound2(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(db, row, cb);
        cb.Checked = true;
    }

    protected void Repeater_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        Label m_PaymentCodeLabel = (Label)e.Item.FindControl("PaymentCode");
        int m_payCodeID = (int)((DataRowView)e.Item.DataItem)["PayCodeID"];

        EPaymentCode m_payCode = EPaymentCode.GetObject(dbConn, m_payCodeID);
        if (m_payCode != null)
        {
            m_PaymentCodeLabel.Text = m_payCode.PaymentCode;
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Payroll_HitRateProcess_List.aspx");
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        DateTime m_paymentDate = new DateTime();
        if (PaymentDate.Value == "" || !(DateTime.TryParse(PaymentDate.Value, out m_paymentDate)))
        {
            errors.addError("Please provide a Payment Date");
            return;
        }

        int m_batchID = -1;
        if (!int.TryParse(BatchID.SelectedValue, out m_batchID))
        {
            errors.addError("Please select a Import Batch");
            return; 
        }

        DataSet dataSet = GenerateCND(m_batchID, m_paymentDate);

        string exportFileName = System.IO.Path.GetTempFileName();
        System.IO.File.Delete(exportFileName);
        exportFileName += ".xls";
        HROne.Export.ExcelExport export = new HROne.Export.ExcelExport(exportFileName);
        export.Update(dataSet);
        WebUtils.TransmitFile(Response, exportFileName, "CND_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true);
    }

    protected void init_BatchIDDropdown()
    {
        if (BatchID.Items.Count <= 0)
        {
            BatchID.Items.Add(new ListItem("Not Selected", "-1"));

            DBFilter m_filter = new DBFilter();
            m_filter.add("HitRateProcessImportBatchID", false);

            int i = 0;
            foreach(EHitRateProcessImportBatch o in EHitRateProcessImportBatch.db.select(dbConn, m_filter))
            {
                i++;
                if (o.HitRateProcessImportBatchRemark != null && o.HitRateProcessImportBatchRemark != "")
                {     
                    BatchID.Items.Add(new ListItem(o.HitRateProcessImportBatchRemark, o.HitRateProcessImportBatchID.ToString()));
                }
                else
                {
                    BatchID.Items.Add(new ListItem("Batch: " + o.HitRateProcessImportBatchID.ToString(), o.HitRateProcessImportBatchID.ToString()));
                }

                if (i >= 20)
                    break; 
            }
        }
    }

    //protected void init_PaymentCodeDropdown()
    //{
    //    if (PaymentCode.Items.Count <= 0)
    //    {
    //        PaymentCode.Items.Add(new ListItem("Not Selected", "-1"));

    //        DBFilter m_filter = new DBFilter();
    //        m_filter.add("PaymentCode", true);

    //        foreach (EPaymentCode o in EPaymentCode.db.select(dbConn, m_filter)) 
    //        {
    //            PaymentCode.Items.Add(new ListItem(string.Format("{0} - {1}", o.PaymentCode, o.PaymentCodeDesc), o.PaymentCodeID.ToString()));
    //        }
    //    }
    //}

    protected DataSet GenerateCND(int ProcessID, DateTime PaymentDate)
    {
        EHitRateProcessImportBatch m_process = EHitRateProcessImportBatch.GetObject(dbConn, ProcessID);

        DataSet dataSet = new DataSet();//export.GetDataSet();
        DataTable dataTable = new DataTable("ClaimsAndDeduction$");
        dataSet.Tables.Add(dataTable);

        dataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_EMP_NO, typeof(string));
        dataTable.Columns.Add("English Name", typeof(string));
        dataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_EFFECTIVE_DATE, typeof(DateTime));
        dataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_PAYMENT_CODE, typeof(string));
        dataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_PAYMENT_METHOD, typeof(string));
        dataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_BANK_ACCOUNT_NO, typeof(string));
        dataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_AMOUNT, typeof(double));
        dataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_NUM_OF_DAY_ADJUST, typeof(double));
        dataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_REST_PAYMENT, typeof(string));
        dataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_REMARK, typeof(string));
        dataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_COST_CENTER, typeof(string));

        DBFilter m_detailFilter = new DBFilter();

        m_detailFilter.add(new Match("HitRateProcessImportBatchID", ProcessID));
        m_detailFilter.add("EmpID", true);

        foreach (EHitRateProcess m_detail in EHitRateProcess.db.select(dbConn, m_detailFilter))
        {
            DBFilter m_rpFilter = new DBFilter();
            m_rpFilter.add(new NullTerm("EmpRPEffTo"));
            m_rpFilter.add(new Match("PayCodeID", m_detail.payCodeID));
            m_rpFilter.add(new Match("EmpID", m_detail.EmpID));
            m_rpFilter.add("EmpRPEffTo", true);

            ArrayList m_empRPList = EEmpRecurringPayment.db.select(dbConn, m_rpFilter);

            if (m_empRPList.Count > 0)
            {
                EEmpRecurringPayment m_empRP = (EEmpRecurringPayment)m_empRPList[0];

                double m_amount = m_empRP.EmpRPAmount * m_detail.HitRate / (double)100;
                string m_remarks;

//                if (m_amount > 0 && Math.Abs(m_amount) >= 0.01)
                {
                    m_remarks = String.Format("{0}*{1}%", m_empRP.EmpRPAmount.ToString("#,##0.00"),
                                                           m_detail.HitRate.ToString("#,##0.00"));

                    EEmpPersonalInfo m_empInfo = new EEmpPersonalInfo();
                    m_empInfo.EmpID = m_empRP.EmpID;
                    EEmpPersonalInfo.db.select(dbConn, m_empInfo);

                    DataRow m_row = dataTable.NewRow();
                    m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_EMP_NO] = m_empInfo.EmpNo;
                    m_row["English Name"] = m_empInfo.EmpEngFullName;
                    m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_EFFECTIVE_DATE] = PaymentDate;

                    EPaymentCode m_paymentCode = EPaymentCode.GetObject(dbConn, m_empRP.PayCodeID);
                    m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_PAYMENT_CODE] = m_paymentCode.PaymentCode;
                    
                    switch (m_empRP.EmpRPMethod)
                    {
                        case "A":
                            m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_PAYMENT_METHOD] = "Autopay";
                            break;
                        case "Q":
                            m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_PAYMENT_METHOD] = "Cheque";
                            break;
                        case "C":
                            m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_PAYMENT_METHOD] = "Cash";
                            break;
                        default:
                            m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_PAYMENT_METHOD] = "Other";
                            break;
                    }
                    m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_AMOUNT] = Math.Round(m_amount, 2);
                    m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_NUM_OF_DAY_ADJUST] = 0; //DateTime.DaysInMonth(m_process.AsAtDate);
                    m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_REST_PAYMENT] = "No";
                    m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_REMARK] = m_remarks;

                    if (m_empRP.EmpAccID > 0)   // account number specified
                    {
                        EEmpBankAccount m_bank = new EEmpBankAccount();
                        m_bank.EmpBankAccountID = m_empRP.EmpAccID;
                        if (EEmpBankAccount.db.select(dbConn, m_bank))
                            m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_BANK_ACCOUNT_NO] = m_bank.EmpAccountNo;
                    }
                    //else
                    //{
                    //    // get default bank account
                    //    DBFilter m_bankAccFilter = new DBFilter();
                    //    m_bankAccFilter.add(new Match("EmpID", m_empRP.EmpID));
                    //    m_bankAccFilter.add(new Match("EmpAccDefault", true));
                    //    m_bankAccFilter.add("empBankAccountID", true);
                    //    ArrayList m_bankAccList = EEmpBankAccount.db.select(dbConn, m_bankAccFilter);
                    //    if (m_bankAccList.Count > 0)
                    //    {
                    //        m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_BANK_ACCOUNT_NO] = ((EEmpBankAccount)m_bankAccList[0]).EmpAccountNo;
                    //    }
                    //}
                    
                    ECostCenter m_costCenter = new ECostCenter();
                    m_costCenter.CostCenterID = m_empRP.CostCenterID;
                    if (ECostCenter.db.select(dbConn, m_costCenter))
                        m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_COST_CENTER] = m_costCenter.CostCenterCode;
                    dataTable.Rows.Add(m_row);
                }
            }
        }
        return dataSet;
    }
    protected void ChangePage(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);
    }

}

