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
using HROne.Import;
//using perspectivemind.validation;
using HROne.Lib.Entities;
using HROne.Translation;

public partial class Payroll_SalaryIncrementBatch_View : HROneWebPage
{
    private const string FUNCTION_CODE = "PAY016";

    public Binding binding;
    public SearchBinding detailBinding;
    public DBManager db = ESalaryIncrementBatch.db;
    public ESalaryIncrementBatch obj;
    protected ListInfo info;
    protected DataView view;

    public int CurID = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        if (ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_PAYSCALE_POINT_SYSTEM) != "Y")
        {
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");
            return;
        }

        toolBar.FunctionCode = FUNCTION_CODE;

        #region "binding for Process information"
        binding = new Binding(dbConn, db);
        //binding.add(BackpayProcessID);
        binding.add(AsAtDate);
        binding.add(new CheckBoxBinder(db, DeferredBatch));
        binding.add(PaymentDate);
        binding.add(new LabelVLBinder(db, Status, ESalaryIncrementBatch.VLStatusDesc));

        binding.add(UploadDateTime);
        binding.add(ConfirmDateTime);
        // binding.add(UploadEmpID); // load employee name from LoadData
        // binding.add(ConfirmEmpID);   // load employee name from LoadData

        binding.init(Request, Session);
        #endregion

        #region"binding for Process Detail"
        detailBinding = new SearchBinding(dbConn, db);
        detailBinding.init(DecryptedRequest, null);
        #endregion

        if (!int.TryParse(DecryptedRequest["BatchID"], out CurID))
            CurID = -1;

        if (!Page.IsPostBack)
        {
            if (CurID > 0)
            {
                loadObject();
                
                if (Status.Text != ESalaryIncrementBatch.STATUS_OPEN_DESC)
                {
                    btnConfirm.Visible = false;
                    btnExport.Visible = false;
                    btnImport.Visible = false;

                    toolBar.DeleteButton_Visible = false;
                    toolBar.EditButton_Visible = false;

                    if (Status.Text == ESalaryIncrementBatch.STATUS_CONFIRMED_DESC)
                    {
                        btnGenerateCND.Visible = DeferredBatch.Checked;
                    }
                }

                if (obj != null)
                {
                    EPaymentCode m_paymentCodeObj = new EPaymentCode();
                    m_paymentCodeObj = EPaymentCode.GetObject(dbConn, obj.PaymentCodeID);
                    if (m_paymentCodeObj != null)
                    {
                        PaymentCode.Text = m_paymentCodeObj.PaymentCode;
                        PaymentCodeDesc.Text = m_paymentCodeObj.PaymentCodeDesc;
                    }
                }

                CNDRow.Visible = DeferredBatch.Checked;
            }
            else
            {
                btnConfirm.Visible = false;
                btnExport.Visible = false;
                btnImport.Visible = false;

                toolBar.DeleteButton_Visible = false;
                toolBar.EditButton_Visible = false;
            }
        }

        info = ListFooter.ListInfo;
        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            view = loadData(info, db, Repeater);
        }
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

    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(db, row, cb);
        cb.Checked = true;
    }

    protected DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = detailBinding.createFilter();

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);
        else
            filter.add("emp.EmpNo", true);

        filter.add(new Match("d.BatchID", CurID));

        //filter.add(WebUtils.AddRankFilter(Session, "e.EmpID", true));

        /****************************************************************************  
        * SELECT d.BatchID, d.DetailID, emp.EmpNo, emp.EmpEngSurname, emp.EmpEngOthername, emp.EmpAlias, d.SchemeCode, d.Capacity, d.CurrentPoint, d.NewPoint 
        * FROM [PS_SalaryIncrementBatchDetail] AS d
        * INNER JOIN [EmpPersonalInfo] AS emp ON d.EmpID = emp.EmpID
         ****************************************************************************/

        string select = "d.BatchID, d.DetailID, emp.EmpID, emp.EmpNo, emp.EmpEngSurname, emp.EmpEngOthername, emp.EmpAlias, d.SchemeCode, d.Capacity, d.CurrentPoint, d.NewPoint ";
        string from = "FROM [PS_SalaryIncrementBatchDetail] AS d " +
                      "INNER JOIN [EmpPersonalInfo] AS emp ON d.EmpID = emp.EmpID ";

        DataTable table = filter.loadData(dbConn, null, select, from);

        foreach (DataColumn col in table.Columns)
        {
            if (col.DataType.Equals(typeof(string)))
            {
                DBAESEncryptStringFieldAttribute.decode(table, col.ColumnName);
            }
        }

        //table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);
        view = new DataView(table);

        ListFooter.Refresh();
        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }

        // get BackpayProcess
        ESalaryIncrementBatch m_process = ESalaryIncrementBatch.GetObject(dbConn, CurID);
        if (m_process != null)
        {
            EUser m_user;
            int m_id;

            UploadBy.Text = "";
            ConfirmBy.Text = "";
            if (m_process.UploadBy > 0)
            {
                m_user = EUser.GetObject(dbConn, m_process.UploadBy);
                if (m_user != null)
                {
                    UploadBy.Text = m_user.UserName;
                }
            }
            if (m_process.ConfirmBy > 0)
            {
                m_user = EUser.GetObject(dbConn, m_process.ConfirmBy);
                if (m_user != null)
                {
                    ConfirmBy.Text = m_user.UserName;
                }
            }
        }
        return view;
    }

    protected bool loadObject()
    {
        obj = new ESalaryIncrementBatch();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
        if (!db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);

        return true;
    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ESalaryIncrementBatch o = new ESalaryIncrementBatch();
        o.BatchID = CurID;
        if (ESalaryIncrementBatch.db.select(dbConn, o))
        {
            if (o.Status == ESalaryIncrementBatch.STATUS_OPEN)
            {
                dbConn.BeginTransaction();

                WebUtils.StartFunction(Session, FUNCTION_CODE);

                DBFilter detailFilter = new DBFilter();
                detailFilter.add(new Match("BatchID", o.BatchID));
                foreach (ESalaryIncrementBatchDetail d in ESalaryIncrementBatchDetail.db.select(dbConn, detailFilter))
                {
                    ESalaryIncrementBatchDetail.db.delete(dbConn, d);
                }

                db.delete(dbConn, o);
                WebUtils.EndFunction(dbConn);

                dbConn.CommitTransaction();
            }
            else if (o.Status == ESalaryIncrementBatch.STATUS_CONFIRMED || o.Status == ESalaryIncrementBatch.STATUS_APPLIED)
            {
                errors.addError(HROne.Translation.PageErrorMessage.ERROR_REMOVE_BATCH);
            }
        }

        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_SalaryIncrementBatch_List.aspx");
    }
    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_SalaryIncrementBatch_Edit.aspx?BatchID=" + CurID);
    }
    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_SalaryIncrementBatch_List.aspx");
    }

    protected DataSet GenerateCND(int ProcessID, int PaymentCodeID, DateTime PaymentDate)
    {
        DateTime m_paymentDate = new DateTime();
        string m_paymentCode = "";

        ESalaryIncrementBatch m_process = ESalaryIncrementBatch.GetObject(dbConn, ProcessID);

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
        m_detailFilter.add(new Match("BatchID", m_process.BatchID));
        m_detailFilter.add("DetailID", true);

        foreach (ESalaryIncrementBatchDetail m_detail in ESalaryIncrementBatchDetail.db.select(dbConn, m_detailFilter))
        {
            DBFilter m_rpFilter = new DBFilter();
            m_rpFilter.add(new Match("EmpRPID", m_detail.EmpRPID));
            //m_rpFilter.add(new IN("EmpRPID", "SELECT EmpRPID FROM PS_SalaryIncrementBatchDetail ", m_detailFilter));
            foreach (EEmpRecurringPayment m_empRP in EEmpRecurringPayment.db.select(dbConn, m_rpFilter))
            {
                DBFilter m_mapFilter = new DBFilter();
                m_mapFilter.add(new Match("SchemeCode", AppUtils.Encode(EPayScaleMap.db.getField("SchemeCode"), m_empRP.SchemeCode)));
                m_mapFilter.add(new Match("Point", m_detail.NewPoint));
                m_mapFilter.add(new NullTerm("ExpiryDate"));

                foreach (EPayScaleMap m_map in EPayScaleMap.db.select(dbConn, m_mapFilter))
                {
                    decimal m_newSalary = m_map.Salary;
                    string m_remarks = "";
                    double m_amount = System.Convert.ToDouble(m_newSalary) - m_empRP.EmpRPAmount;

                    if (Math.Abs(m_amount) > 0.01)
                    {
                        m_remarks = String.Format("Backpay {0}: {1}-{2}.", m_process.AsAtDate.ToString("yyyy-MM"),
                                                                           m_newSalary.ToString("#,##0.00"),
                                                                           m_empRP.EmpRPAmount.ToString("#,##0.00"));

                        EEmpPersonalInfo m_empInfo = new EEmpPersonalInfo();
                        m_empInfo.EmpID = m_empRP.EmpID;
                        EEmpPersonalInfo.db.select(dbConn, m_empInfo);

                        DataRow m_row = dataTable.NewRow();
                        m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_EMP_NO] = m_empInfo.EmpNo;
                        m_row["English Name"] = m_empInfo.EmpEngFullName;
                        m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_EFFECTIVE_DATE] = m_paymentDate;
                        m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_PAYMENT_CODE] = m_paymentCode;
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

                        EEmpBankAccount m_bank = new EEmpBankAccount();
                        m_bank.EmpBankAccountID = m_empRP.EmpAccID;
                        if (EEmpBankAccount.db.select(dbConn, m_bank))
                            m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_BANK_ACCOUNT_NO] = m_bank.EmpAccountNo;

                        ECostCenter m_costCenter = new ECostCenter();
                        m_costCenter.CostCenterID = m_empRP.CostCenterID;
                        if (ECostCenter.db.select(dbConn, m_costCenter))
                            m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_COST_CENTER] = m_costCenter.CostCenterCode;
                        dataTable.Rows.Add(m_row);
                    }
                }
            }
        }
        return dataSet;
    }

    protected void btnGenerateCND_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

//        string m_schemeCode = "";
        string m_paymentCode = "";
        int m_paymentCodeID = -1;
        DateTime m_paymentDate = new DateTime();
        EPaymentCode m_paymentCodeObj;

        if (PaymentDate.Text != "")
        {
            m_paymentDate = DateTime.Parse(PaymentDate.Text);
        }
        else
        {
            errors.addError("Please provide a Backpay Payment Date");
            return;
        }

        if (PaymentCode.Text != "")
        {
            m_paymentCode = PaymentCode.Text;
            m_paymentCodeObj = EPaymentCode.GetObject(dbConn, PaymentCode.Text);

            if (m_paymentCodeObj == null)
            {
                errors.addError("Cannot resolve Backpay Payment Code");
                return;
            }
        }
        else
        {
            errors.addError("Pelase select a Backpay Payment Code");
            return;
        }

        DataSet dataSet = GenerateCND(CurID, m_paymentCodeObj.PaymentCodeID, m_paymentDate);

        string exportFileName = System.IO.Path.GetTempFileName();
        System.IO.File.Delete(exportFileName);
        exportFileName += ".xls";
        HROne.Export.ExcelExport export = new HROne.Export.ExcelExport(exportFileName);
        export.Update(dataSet);
        WebUtils.TransmitFile(Response, exportFileName, "BackpayCND_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true);
    }

    protected void btnExport_Click(object sender, EventArgs e)
    { 
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        if (errors.isEmpty())
        {
            string exportFileName = System.IO.Path.GetTempFileName();
            System.IO.File.Delete(exportFileName);
            exportFileName += ".xls";
            HROne.Export.ExcelExport export = new HROne.Export.ExcelExport(exportFileName);
            DataSet dataSet = new DataSet();// export.GetDataSet();

            dataSet.Tables.Add(ExportBatchDetail(dbConn, CurID).Copy());

            export.Update(dataSet);
            WebUtils.TransmitFile(Response, exportFileName, "SalaryIncrement_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true);
        }
        return;
    }

    private static DataTable ExportBatchDetail(DatabaseConnection dbConn, int batchID) //, DateTime asAtDate)
    {

        DBFilter m_filter = new DBFilter();

        m_filter.add(new Match("d.BatchID", batchID));
        m_filter.add("f.EmpNo", true);

        string m_select = "f.EmpNo as '" + ImportPayScaleSalaryIncrement.FIELD_EMP_NO + "', " + 
                          "f.EmpEngSurname as '" + ImportPayScaleSalaryIncrement.FIELD_ENG_SURNAME + "', " + 
                          "f.EmpEngOtherName as '" + ImportPayScaleSalaryIncrement.FIELD_ENG_OTHERNAME + "', " + 
                          "f.EmpChiFullName as '" + ImportPayScaleSalaryIncrement.FIELD_CHI_NAME + "', " +
                          "d.SchemeCode as '" + ImportPayScaleSalaryIncrement.FIELD_SCHEME_CODE + "', " +
                          "d.Capacity as '" + ImportPayScaleSalaryIncrement.FIELD_CAPACITY + "', " +
                          "ps.FirstPoint as '" + ImportPayScaleSalaryIncrement.FIELD_MIN_POINT + "', " +
                          "ps.LastPoint as '" + ImportPayScaleSalaryIncrement.FIELD_MAX_POINT + "', " +
                          "d.CurrentPoint as '" + ImportPayScaleSalaryIncrement.FIELD_CURRENT_POINT + "', " + 
                          "CASE d.CurrentPoint WHEN ps.LastPoint THEN ps.LastPoint ELSE d.NewPoint END as '" + ImportPayScaleSalaryIncrement.FIELD_NEW_POINT + "' " ;

        string m_from = "FROM PS_SalaryIncrementBatchDetail d LEFT JOIN EmpPersonalInfo f ON d.empID = f.empID INNER JOIN PayScale ps ON d.SchemeCode = ps.SchemeCode AND d.Capacity = ps.Capacity ";

        DataTable m_table = AppUtils.runSelectSQL(m_select, m_from, m_filter, dbConn);

        return m_table;
    }


    protected void btnImport_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_SalaryIncrementBatch_Import.aspx?ID=" + CurID);

        //ImportBackpayProcess BackpayImport = new ImportBackpayProcess(dbConn, Session.SessionID, WebUtils.GetCurUser(Session).UserID);
        //WebUtils.StartFunction(Session, FUNCTION_CODE);
        //BackpayImport.UploadToTempDatabase();
        //BackpayImport.ImportToDatabase();
        //WebUtils.EndFunction(dbConn);
        ////loadData(info, db, Repeater);
        //PageErrors.getErrors(db, Page).addError(HROne.Translation.PageMessage.IMPORT_SUCCESSFUL);
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        if (CurID > 0)
        {
            ESalaryIncrementBatch m_process = ESalaryIncrementBatch.GetObject(dbConn, CurID);

            if (m_process.Status == ESalaryIncrementBatch.STATUS_OPEN) // not Confirmed/Applied
            {
                DBFilter m_filterByBatchID = new DBFilter();
                m_filterByBatchID.add(new Match("BatchID", m_process.BatchID));
                m_filterByBatchID.add("EmpID", true);

                try
                {
                    WebUtils.StartFunction(Session, FUNCTION_CODE, true);
                    dbConn.BeginTransaction();
                    foreach (ESalaryIncrementBatchDetail d in ESalaryIncrementBatchDetail.db.select(dbConn, m_filterByBatchID))
                    {
                        if (d.CurrentPoint != d.NewPoint)
                        {
                            DBFilter m_cpFilter = new DBFilter();
                            m_cpFilter.add(new NullTerm("EmpRPEffTo"));
                            m_cpFilter.add(new Match("EmpID", d.EmpID));
                            m_cpFilter.add(new Match("SchemeCode", AppUtils.Encode(EEmpRecurringPayment.db.getField("SchemeCode"), d.SchemeCode)));
                            m_cpFilter.add(new Match("Capacity", AppUtils.Encode(EEmpRecurringPayment.db.getField("Capacity"), d.Capacity)));
                            m_cpFilter.add(new Match("Point", d.CurrentPoint));

                            DBFilter m_payTypeCodeFilter = new DBFilter();
                            m_payTypeCodeFilter.add(new Match("PaymentTypeCode", "BASICSAL"));

                            DBFilter m_payTypeFilter = new DBFilter();
                            m_payTypeFilter.add(new IN("PaymentTypeID", "SELECT PaymentTypeID FROM PaymentType", m_payTypeCodeFilter));
                            m_cpFilter.add(new IN("PayCodeID", "SELECT PaymentCodeID FROM PaymentCode", m_payTypeFilter));

                            foreach (EEmpRecurringPayment m_cp in EEmpRecurringPayment.db.select(dbConn, m_cpFilter))
                            {
                                DBFilter m_payScaleFilter = new DBFilter();

                                m_payScaleFilter.add(new Match("SchemeCode", AppUtils.Encode(EEmpRecurringPayment.db.getField("SchemeCode"), d.SchemeCode)));
                                m_payScaleFilter.add(new Match("EffectiveDate", "<=", m_process.AsAtDate));

                                OR m_orDate = new OR();
                                m_orDate.add(new Match("ExpiryDate", ">=", m_process.AsAtDate));
                                m_orDate.add(new NullTerm("ExpiryDate"));

                                m_payScaleFilter.add(m_orDate);
                                m_payScaleFilter.add(new Match("Point", d.NewPoint));

                                ArrayList m_payScaleMapList = EPayScaleMap.db.select(dbConn, m_payScaleFilter);
                                if (m_payScaleMapList.Count > 0)
                                {
                                    EEmpRecurringPayment m_newCp = new EEmpRecurringPayment();

                                    m_newCp.EmpRPAmount = System.Convert.ToDouble(((EPayScaleMap)m_payScaleMapList[0]).Salary);
                                    m_newCp.EmpRPAmount = Math.Round(m_newCp.EmpRPAmount, 2);
                                    m_newCp.EmpRPEffFr = m_process.AsAtDate;
                                    m_newCp.Point = d.NewPoint;

                                    //m_newCp.EmpRPEffTo = m_cp.EmpRPEffTo;
                                    m_newCp.Capacity = m_cp.Capacity;
                                    m_newCp.CostCenterID = m_cp.CostCenterID;
                                    m_newCp.CurrencyID = m_cp.CurrencyID;
                                    m_newCp.EmpAccID = m_cp.EmpAccID;
                                    m_newCp.EmpID = m_cp.EmpID;
                                    m_newCp.EmpRPID = m_cp.EmpRPID;
                                    m_newCp.EmpRPIsNonPayrollItem = m_cp.EmpRPIsNonPayrollItem;
                                    m_newCp.EmpRPMethod = m_cp.EmpRPMethod;
                                    m_newCp.EmpRPRemark = m_cp.EmpRPRemark;
                                    m_newCp.EmpRPUnit = m_cp.EmpRPUnit;
                                    m_newCp.EmpRPUnitPeriodAsDaily = m_cp.EmpRPUnitPeriodAsDaily;
                                    m_newCp.EmpRPUnitPeriodAsDailyPayFormID = m_cp.EmpRPUnitPeriodAsDailyPayFormID;
                                    m_newCp.PayCodeID = m_cp.PayCodeID;
                                    m_newCp.SchemeCode = m_cp.SchemeCode;

                                    m_cp.EmpRPEffTo = m_process.AsAtDate.AddDays(-1);
                                    if (EEmpRecurringPayment.db.update(dbConn, m_cp))
                                    {
                                        EEmpRecurringPayment.db.insert(dbConn, m_newCp);
                                    }
                                }
                                else
                                {
                                    errors.addError(string.Format("Cannot map salary setting (Scheme Code={0}, Capacity={1}, Point={2})", new string[] { d.SchemeCode, d.Capacity, d.NewPoint.ToString("0.00") }));
                                    dbConn.RollbackTransaction();
                                    WebUtils.EndFunction(dbConn);
                                    return;
                                }
                            }
                        }
                        EEmpPersonalInfo m_empInfo = EEmpPersonalInfo.GetObject(dbConn, d.EmpID);
                        if (m_empInfo.EmpNextSalaryIncrementDate == m_process.AsAtDate)
                        {
                            m_empInfo.EmpNextSalaryIncrementDate = m_process.AsAtDate.AddYears(1);
                            EEmpPersonalInfo.db.update(dbConn, m_empInfo);
                        }
                    }

                    m_process.Status = ESalaryIncrementBatch.STATUS_CONFIRMED;
                    m_process.ConfirmDateTime = AppUtils.ServerDateTime();
                    m_process.ConfirmBy = WebUtils.GetCurUser(Session).UserID;

                    ESalaryIncrementBatch.db.update(dbConn, m_process);

                    //errors.addError("Salary Increment Batch confirmed!");

                    dbConn.CommitTransaction();
                    WebUtils.EndFunction(dbConn);

                }
                catch (Exception ex)
                {
                    dbConn.RollbackTransaction();
                    errors.addError(ex.Message);
                    WebUtils.EndFunction(dbConn);
                    return;
                }
                if (errors.isEmpty())
                {
                    string m_message = "Salary Increment Batch confirmed!";
                    string m_url = "Payroll_SalaryIncrementBatch_View.aspx?BatchID=" + CurID;

                    WebUtils.RegisterRedirectJavaScript(this, m_url, HROne.Common.WebUtility.GetLocalizedString(m_message));
                }
            }
            else
            {
                errors.addError("Batch is confirmed.");
            }
        }
    }


}
