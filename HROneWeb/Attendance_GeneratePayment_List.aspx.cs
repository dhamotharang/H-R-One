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
using HROne.Payroll;
using HROne.BankFile;

public partial class Attendance_GeneratePayment_List : HROneWebPage
{
    private const string FUNCTION_CODE = "ATT011";
    public Binding binding;

    protected SearchBinding sbinding;
    public DBManager db = EPayrollGroup.db;
    public EPayrollGroup obj;
    public int CurID = -1;
    public int CurPayPeriodID = -1;

    protected ListInfo info;
    protected DataView view;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;

        if (!int.TryParse(PayGroupID.SelectedValue, out CurID))
            if (!int.TryParse(DecryptedRequest["PayGroupID"], out CurID))
                CurID = -1;

        if (!int.TryParse(PayPeriodID.SelectedValue, out CurPayPeriodID))
            if (!int.TryParse(DecryptedRequest["PayPeriodID"], out CurPayPeriodID))
            {
                EPayrollGroup obj = new EPayrollGroup();
                obj.PayGroupID = CurID;
                if (EPayrollGroup.db.select(dbConn, obj))
                    CurPayPeriodID = obj.CurrentPayPeriodID;
                else
                    CurPayPeriodID = -1;
            }


        binding = new Binding(dbConn, db);
        binding.add(new DropDownVLBinder(db, PayGroupID, EPayrollGroup.VLPayrollGroup));
        binding.add(CurrentPayPeriodID);


        
        DBFilter payPeriodFilter = new DBFilter();
        payPeriodFilter.add(new Match("PayPeriodStatus", "<>", "E"));

        payPeriodFilter.add(new Match("PayGroupID", CurID));

        payPeriodFilter.add("PayPeriodFr", false);

        binding.add(new DropDownVLBinder(EPayrollPeriod.db, PayPeriodID, EPayrollPeriod.VLPayrollPeriod, payPeriodFilter));

        binding.init(Request, Session);

        sbinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);
        sbinding.initValues("EmpStatus", null, EEmpPersonalInfo.VLEmpStatus, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));

        try
        {
            PayPeriodID.SelectedValue = CurPayPeriodID.ToString();
        }
        catch
        { }

        info = ListFooter.ListInfo;
        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);


    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        loadObject();
        if (!Page.IsPostBack)
        {
            if (CurID > 0)
            {
                panelPayPeriod.Visible = true;
                view = loadData(info, EEmpPayroll.db, Repeater);
            }
            else
                panelPayPeriod.Visible = false;

        }
    }
    protected bool loadObject()
    {
        //obj = new EPayrollGroup();
        //bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
        //if (!db.select(dbConn, obj))
        //    return false;

        //Hashtable values = new Hashtable();
        //db.populate(obj, values);
        //binding.toControl(values);

        //if (CurPayPeriodID <= 0)
        //{
        //    CurPayPeriodID = obj.CurrentPayPeriodID;
        //}
        try
        {
            PayPeriodID.SelectedValue = CurPayPeriodID.ToString();
        }
        catch
        {
            CurPayPeriodID = 0;
        }
        ucPayroll_PeriodInfo.CurrentPayPeriodID = CurPayPeriodID;

        if (CurPayPeriodID > 0)
            panelEmployeeList.Visible = true;
        else
            panelEmployeeList.Visible = false;


        return true;
    }

    protected void PayGroupID_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (CurID > 0)
        {
            panelPayPeriod.Visible = true;
            view = loadData(info, EEmpPayroll.db, Repeater);
        }
        else
            panelPayPeriod.Visible = false;
    }
    protected void PayPeriodID_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (CurID > 0)
        {
            panelPayPeriod.Visible = true;
            view = loadData(info, EEmpPayroll.db, Repeater);
        }
        else
            panelPayPeriod.Visible = false;
    }

    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = sbinding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = "* ";
        string from = "from [" + EEmpPersonalInfo.db.dbclass.tableName + "] e ";

        IN inTerm = new IN("e.EmpID", "Select ep.EmpID from [EmpPositionInfo] ep, [PayrollPeriod] pp ", filter);

        filter.add(new MatchField("e.EmpID", "ep.EmpID"));
        filter.add(new MatchField("ep.PayGroupID", "pp.PayGroupID"));
        filter.add(new MatchField("ep.EmpPosEffFr", "<=", "pp.PayPeriodTo"));
        filter.add(new Match("pp.PayPeriodID", CurPayPeriodID));
        filter.add(WebUtils.AddRankFilter(Session, "e.EmpID", true));

        OR orPayPeriodFilter = new OR();
        orPayPeriodFilter.add(new MatchField("ep.EmpPosEffTo", ">=", "pp.PayPeriodFr"));
        orPayPeriodFilter.add(new NullTerm("ep.EmpPosEffTo"));

        filter.add(orPayPeriodFilter);


        filter.add(new IN("Not e.empid", "Select et.empid from " + EEmpTermination.db.dbclass.tableName + " et where et.EmpTermLastDate<pp.PayPeriodFr", new DBFilter()));

        DBFilter resultFilter = new DBFilter();
        resultFilter.add(inTerm);

        DBFilter empInfoFilter = EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime().Date, AppUtils.ServerDateTime().Date );
        empInfoFilter.add(new MatchField("e.EmpID", "ee.EmpID"));
        resultFilter.add(new Exists(EEmpPersonalInfo.db.dbclass.tableName + " ee", empInfoFilter));

        DataTable table = resultFilter.loadData(dbConn, null, select, from);
        table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);

        if (table.Rows.Count != 0)
        {
            btnGenerate.Visible = true;
            btnGenerate2.Visible = true;
        }
        else
        {
            btnGenerate.Visible = false;
            btnGenerate2.Visible = false;
        }

        view = new DataView(table);
        //if (info != null)
        //{
        //    info.loadPageList(null, PrevPage, NextPage, FirstPage, LastPage);

        //    CurPage.Value = info.page.ToString();
        //    NumPage.Value = info.numPage.ToString();
        //}
        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }

        return view;
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

        view = loadData(info, EEmpPayroll.db, Repeater);

    }
    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(EEmpPersonalInfo.db, row, cb);
    }

    protected void btnGenerate_Command(object sender, CommandEventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);

        ArrayList empList = new ArrayList();
        foreach (RepeaterItem i in Repeater.Items)
        {
            CheckBox cb = (CheckBox)i.FindControl("ItemSelect");
            if (cb.Checked)
            {
                EEmpPersonalInfo o = (EEmpPersonalInfo)EEmpPersonalInfo.db.createObject();
                WebFormUtils.GetKeys(EEmpPersonalInfo.db, o, cb);
                empList.Add(o);
            }

        }



        if (empList.Count > 0)
        {
            DataSet dataSet = GenerateAttendanceRecordDataSet(empList, CurPayPeriodID);

            if (e.CommandName.Equals("Export", StringComparison.CurrentCultureIgnoreCase))
            {
                string exportFileName = System.IO.Path.GetTempFileName();
                System.IO.File.Delete(exportFileName);
                exportFileName += ".xls";
                //System.IO.File.Copy(Server.MapPath("~/template/HistoryList_Template.xls"), exportFileName, true);
                HROne.Export.ExcelExport export = new HROne.Export.ExcelExport(exportFileName);
                export.Update(dataSet);
                //WebUtils.RegisterDownloadFileJavaScript(this, exportFileName, "AttendancePaymentCND_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true, 0);
                WebUtils.TransmitFile(Response, exportFileName, "AttendancePaymentCND_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true);
            }
            else if (e.CommandName.Equals("ImportCND", StringComparison.CurrentCultureIgnoreCase))
            {

                HROne.Import.ImportClaimsAndDeductionsProcess CNDImport = new HROne.Import.ImportClaimsAndDeductionsProcess(dbConn, Session.SessionID, WebUtils.GetCurUser(Session).UserID);
                CNDImport.ClearTempTable();

                EPayrollPeriod payPeriod = new EPayrollPeriod();
                payPeriod.PayPeriodID = CurPayPeriodID;
                if (EPayrollPeriod.db.select(dbConn, payPeriod))
                {
                    CNDImport.Remark = "Attendance Payment from " + payPeriod.PayPeriodAttnFr.ToString("yyyy-MM-dd") + " to " + payPeriod.PayPeriodAttnTo.ToString("yyyy-MM-dd");
                    EPayrollGroup payGroup = new EPayrollGroup();
                    payGroup.PayGroupID = payPeriod.PayGroupID;
                    if (EPayrollGroup.db.select(dbConn, payGroup))
                        CNDImport.Remark += " (" + payGroup.PayGroupCode + ")";
                }
                try
                {
                    CNDImport.UploadToTempDatabase(dataSet.Tables[0], WebUtils.GetCurUser(Session).UserID);
                    CNDImport.ImportToDatabase();
                    errors.addError(HROne.Translation.PageMessage.IMPORT_SUCCESSFUL);
                }
                catch (HROne.Import.HRImportException ex)
                {
                    if (CNDImport.errors.List.Count > 0)
                        foreach (string errorString in CNDImport.errors.List)
                            errors.addError(errorString);
                    else
                        errors.addError(ex.Message);
                }
            }
        }
        else
        {
            errors.addError("Employee not selected");
        }
    }


    protected void Search_Click(object sender, EventArgs e)
    {
        info.page = 0;

        view = loadData(info, EEmpPayroll.db, Repeater);

    }
    protected void Reset_Click(object sender, EventArgs e)
    {
        EmployeeSearchControl1.Reset();
        sbinding.clear();
        info.page = 0;


        view = loadData(info, EEmpPayroll.db, Repeater);


    }

    private DataSet GenerateAttendanceRecordDataSet(ArrayList EmpInfoList, int PayperiodID)
    {
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

        AttendanceProcess attendanceProcess = new AttendanceProcess(dbConn);
        foreach (EEmpPersonalInfo empInfo in EmpInfoList)
        {
            DateTime dateFrom = new DateTime();
            DateTime dateTo = new DateTime();

            if (EEmpPersonalInfo.db.select(dbConn, empInfo))
            {
                dateFrom = empInfo.EmpDateOfJoin;


                EPayrollPeriod payPeriod = new EPayrollPeriod();
                payPeriod.PayPeriodID = PayperiodID;
                if (EPayrollPeriod.db.select(dbConn, payPeriod))
                {
                    dateFrom = payPeriod.PayPeriodAttnFr;
                    dateTo = payPeriod.PayPeriodAttnTo;

                    if (dateFrom < empInfo.EmpDateOfJoin)
                        dateFrom = empInfo.EmpDateOfJoin;

                    DBFilter empTermFilter=new DBFilter();
                    empTermFilter.add(new Match("EmpID",empInfo.EmpID));

                    ArrayList empTermList = EEmpTermination.db.select(dbConn, empTermFilter);
                    foreach (EEmpTermination empTerm in empTermList)
                        if (dateTo > empTerm.EmpTermLastDate)
                            dateTo = empTerm.EmpTermLastDate;

                    DBFilter empPosFilter = new DBFilter();
                    empPosFilter.add(new Match("EmpID", empInfo.EmpID));
                    empPosFilter.add(new Match("PayGroupID", payPeriod.PayGroupID));
                    empPosFilter.add(new Match("EmpPosEffFr", "<=", dateTo));
                    OR orEmpPosEffToTerm = new OR();
                    orEmpPosEffToTerm.add(new Match("EmpPosEffTo", ">=", dateFrom));
                    orEmpPosEffToTerm.add(new NullTerm("EmpPosEffTo"));
                    empPosFilter.add("EmpPosEffFr", false);
                    empPosFilter.add(orEmpPosEffToTerm);

                    ArrayList empPosList = EEmpPositionInfo.db.select(dbConn, empPosFilter);
                    foreach (EEmpPositionInfo currentEmpPos in empPosList)
                    {

                        DateTime payGroupDateFrom = dateFrom;
                        DateTime payGroupDateTo = dateTo;

                        if (currentEmpPos.EmpPosEffFr > payGroupDateFrom)
                            payGroupDateFrom = currentEmpPos.EmpPosEffFr;

                        if (currentEmpPos.EmpPosEffTo < payGroupDateTo && !currentEmpPos.EmpPosEffTo.Ticks.Equals(0))
                            payGroupDateTo = currentEmpPos.EmpPosEffTo;

                        ArrayList paymentRecordList = attendanceProcess.AttendancePaymentTrialRun(empInfo.EmpID, payPeriod, payGroupDateFrom, payGroupDateTo);
                        foreach (EPaymentRecord paymentRecord in paymentRecordList)
                        {
                            DataRow row = dataTable.NewRow();
                            row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_EMP_NO] = empInfo.EmpNo;
                            row["English Name"] = empInfo.EmpEngFullName;
                            row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_EFFECTIVE_DATE] = dateTo;
                            if (currentEmpPos != null)
                                if (!currentEmpPos.EmpPosEffTo.Ticks.Equals(0))
                                    if (currentEmpPos.EmpPosEffTo < dateTo)
                                        row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_EFFECTIVE_DATE] = currentEmpPos.EmpPosEffTo;

                            EPaymentCode payCode = new EPaymentCode();
                            payCode.PaymentCodeID = paymentRecord.PaymentCodeID;
                            if (EPaymentCode.db.select(dbConn, payCode))

                                row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_PAYMENT_CODE] = payCode.PaymentCode;
                            if (paymentRecord.PayRecMethod.Equals("A"))
                                row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_PAYMENT_METHOD] = "Autopay";
                            else if (paymentRecord.PayRecMethod.Equals("Q"))
                                row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_PAYMENT_METHOD] = "Cheque";
                            else if (paymentRecord.PayRecMethod.Equals("C"))
                                row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_PAYMENT_METHOD] = "Cash";
                            else
                                row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_PAYMENT_METHOD] = string.Empty;

                            row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_AMOUNT] = paymentRecord.PayRecActAmount;
                            row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_NUM_OF_DAY_ADJUST] = paymentRecord.PayRecNumOfDayAdj;
                            row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_REST_PAYMENT] = paymentRecord.PayRecIsRestDayPayment ? "Yes" : "No";

                            row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_REMARK] = paymentRecord.PayRecRemark;

                            ECostCenter costCenter = new ECostCenter();
                            costCenter.CostCenterID = paymentRecord.CostCenterID;
                            if (ECostCenter.db.select(dbConn, costCenter))
                                row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_COST_CENTER] = costCenter.CostCenterCode;

                            dataTable.Rows.Add(row);

                        }
                    }   
                }
            }
        }
        return dataSet;

    }

    //public void AddCNDDataRow(DataTable dataTable, EEmpPersonalInfo empInfo, DateTime effectiveDate, EPaymentRecord paymentRecord)
    //{
    //    DataRow row = dataTable.NewRow();
    //    row["Emp No"] = empInfo.EmpNo;
    //    row["EnglishName"] = empInfo.EmpEngFullName;
    //    row["Effective Date"] = effectiveDate;

    //    EPaymentCode payCode = new EPaymentCode();
    //    payCode.PaymentCodeID = paymentRecord.PaymentCodeID;
    //    if (EPaymentCode.db.select(dbConn, payCode))

    //        row["Payment Code"] = payCode.PaymentCode;
    //    row["Payment Method"] = string.Empty;
    //    row["Amount"] = paymentRecord.PayRecActAmount;
    //    row["Remark"] = paymentRecord.PayRecRemark;

    //    dataTable.Rows.Add(row);

    //}

}