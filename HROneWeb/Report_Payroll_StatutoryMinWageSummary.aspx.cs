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
using HROne.Lib;

public partial class Report_Payroll_StatutoryMinWageSummary : HROneWebPage
{
    private const string FUNCTION_CODE = "RPT210";
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
        binding = new Binding(dbConn, db);
        binding.add(new DropDownVLBinder(db, PayGroupID, EPayrollGroup.VLPayrollGroup));


        binding.init(Request, Session);

        sbinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);
        sbinding.initValues("EmpStatus", null, EEmpPersonalInfo.VLEmpStatus, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));

        if (!int.TryParse(PayGroupID.SelectedValue, out CurID))
            CurID = -1;
        Payroll_ConfirmedPeriod_List1.PayGroupID = CurID;
        Payroll_ConfirmedPeriod_List1.PayrollStatus = PayrollStatus.SelectedValue;


        info = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);


    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        {
            if (CurID > 0)
            {
                panelPayPeriod.Visible = true;
                loadObject();
                if (!Page.IsPostBack)
                    view = loadData(info, EEmpPersonalInfo.db, Repeater);
            }
            else
                panelPayPeriod.Visible = false;

        }
    }

    protected bool loadObject()
    {
        obj = new EPayrollGroup();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
        if (!db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);

        if (obj.CurrentPayPeriodID > 0)
            panelEmployeeList.Visible = true;
        else
            panelEmployeeList.Visible = false;


        return true;
    }

    protected void PayGroupID_SelectedIndexChanged(object sender, EventArgs e)
    {
        Payroll_ConfirmedPeriod_List1.Refresh();

        //Response.Redirect(Request.Url.LocalPath + "?PayGroupID=" + PayGroupID.SelectedValue);
    }
    //protected void PayPeriodID_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    Response.Redirect(Request.Url.LocalPath + "?PayGroupID=" + PayGroupID.SelectedValue + "&PayPeriodID=" + PayPeriodID.SelectedValue);
    //}

    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = sbinding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = "e.* ";
        string from = "from [" + db.dbclass.tableName + "] e ";

        DBFilter empPayrollFilter = new DBFilter();

        if (PayrollStatus.SelectedValue.Equals("T"))
            empPayrollFilter.add(new Match("ep.EmpPayStatus", "=", "T"));
        else
            empPayrollFilter.add(new Match("ep.EmpPayStatus", "<>", "T"));

        //DBFilter payPeriodFilter = new DBFilter();
        //if (DecryptedRequest["PayGroupID"] != null)
        //    payPeriodFilter.add(new Match("PayGroupID", DecryptedRequest["PayGroupID"]));
        //else
        //    payPeriodFilter.add(new Match("PayGroupID", 0));


        empPayrollFilter.add(new IN("ep.PayPeriodID", " Select PayPeriodID from PayrollPeriod pp", Payroll_ConfirmedPeriod_List1.getPayrollPeriodFilter()));

        filter.add(new IN("e.EmpID", "Select ep.EmpID from EmpPayroll ep ", empPayrollFilter));

        DBFilter empInfoFilter = EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime().Date, AppUtils.ServerDateTime().Date );
        empInfoFilter.add(new MatchField("e.EmpID", "ee.EmpID"));
        filter.add(new Exists(EEmpPersonalInfo.db.dbclass.tableName + " ee", empInfoFilter));

        DataTable table = filter.loadData(dbConn, null, select, from);
        table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);

        if (table.Rows.Count != 0 && Payroll_ConfirmedPeriod_List1.RecordCount > 0)
            btnGenerate.Visible = true;
        else
            btnGenerate.Visible = false;

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

        view = loadData(info, EEmpPersonalInfo.db, Repeater);

    }
    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(EEmpPersonalInfo.db, row, cb);
    }


    protected void btnGenerate_Click(object sender, EventArgs e)
    {

        const string FIELD_COMPANY = "Company";
        const string FIELD_POSITION = "Position";
        const string FIELD_PAYROLLGROUP = "Payroll Group";
        const string FIELD_EMPNO = "EmployeeID";
        const string FIELD_EMPENGFULLNAME = "English Name";
        const string FIELD_CHINESENAME = "¤¤¤å©m¦W";
        const string FIELD_HKID = @"HKID/Passport";
        const string FIELD_PERIODFROM = "From";
        const string FIELD_PERIODTO = "To";
        const string FIELD_WAGESWORK = "Wages Paid";
        const string FIELD_WORKHOURTOTAL = "Total Working Hours";
        const string FIELD_RESTDAYTOTAL = "No. of Rest Day";
        const string FIELD_STATUTORYHOLIDAYTOTAL = "No. of SH";
        const string FIELD_FULLPAIDLEAVETOTAL = "No. of Full Paid Leave";
        const string FIELD_NONFULLPAIDLEAVETOTAL = "Non-Full Paid Leave";

        ArrayList list = new ArrayList();
        foreach (RepeaterItem i in Repeater.Items)
        {
            CheckBox cb = (CheckBox)i.FindControl("ItemSelect");
            if (cb.Checked)
            {
                EEmpPersonalInfo o = (EEmpPersonalInfo)EEmpPersonalInfo.db.createObject();
                WebFormUtils.GetKeys(EEmpPersonalInfo.db, o, cb);
                list.Add(o);
            }

        }

        ArrayList payPeriodList = Payroll_ConfirmedPeriod_List1.GetSelectedBaseObjectList();



        if (list.Count > 0 && payPeriodList.Count > 0)
        {
            //const string PAYMENTCODE_PREFIX = "[StatutoryMinimumWageSummary] ";
            string exportFileName = System.IO.Path.GetTempFileName();
            System.IO.File.Delete(exportFileName);
            exportFileName += ".xls";
            //System.IO.File.Copy(Server.MapPath("~/template/HistoryList_Template.xls"), exportFileName, true);
            HROne.Export.ExcelExport export = new HROne.Export.ExcelExport(exportFileName);
            DataSet dataSet = new DataSet(); //export.GetDataSet();
            DataTable dataTable = new DataTable("Payroll$");
            dataSet.Tables.Add(dataTable);
            dataTable.Columns.Add(FIELD_COMPANY, typeof(string));

            DBFilter hierarchyLevelFilter = new DBFilter();
            Hashtable hierarchyLevelHashTable = new Hashtable();
            hierarchyLevelFilter.add("HLevelSeqNo", true);
            ArrayList hierarchyLevelList = EHierarchyLevel.db.select(dbConn, hierarchyLevelFilter);
            foreach (EHierarchyLevel hlevel in hierarchyLevelList)
            {
                dataTable.Columns.Add(hlevel.HLevelDesc, typeof(string));
                hierarchyLevelHashTable.Add(hlevel.HLevelID, hlevel);
            }
            dataTable.Columns.Add(FIELD_POSITION, typeof(string));
            dataTable.Columns.Add(FIELD_PAYROLLGROUP, typeof(string));
            dataTable.Columns.Add(FIELD_EMPNO, typeof(string));
            dataTable.Columns.Add(FIELD_EMPENGFULLNAME, typeof(string));
            dataTable.Columns.Add(FIELD_CHINESENAME, typeof(string));
            dataTable.Columns.Add(FIELD_HKID, typeof(string));
            dataTable.Columns.Add(FIELD_PERIODFROM, typeof(DateTime));
            dataTable.Columns.Add(FIELD_PERIODTO, typeof(DateTime));

            dataTable.Columns.Add(FIELD_WAGESWORK, typeof(double));
            dataTable.Columns.Add(FIELD_WORKHOURTOTAL, typeof(double));
            dataTable.Columns.Add(FIELD_RESTDAYTOTAL, typeof(double));
            dataTable.Columns.Add(FIELD_STATUTORYHOLIDAYTOTAL, typeof(double));
            dataTable.Columns.Add(FIELD_FULLPAIDLEAVETOTAL, typeof(double));
            dataTable.Columns.Add(FIELD_NONFULLPAIDLEAVETOTAL, typeof(double));



            int firstSummaryColumnPos = dataTable.Columns.Count;
            int firstDetailColumnPos = dataTable.Columns.Count;

            foreach (EPayrollPeriod payPeriod in payPeriodList)
            {
                if (EPayrollPeriod.db.select(dbConn, payPeriod))
                {

                    EPayrollGroup payrollGroup = new EPayrollGroup();
                    payrollGroup.PayGroupID = payPeriod.PayGroupID;
                    EPayrollGroup.db.select(dbConn, payrollGroup);

                    foreach (EEmpPersonalInfo empInfo in list)
                    {
                        EEmpPersonalInfo.db.select(dbConn, empInfo);
                        EEmpTermination empTerm = EEmpTermination.GetObjectByEmpID(dbConn, empInfo.EmpID);

                        DBFilter empPayrollFilter = new DBFilter();
                        empPayrollFilter.add(new Match("ep.EmpID", empInfo.EmpID));
                        empPayrollFilter.add(new Match("ep.PayPeriodID", payPeriod.PayPeriodID));
                        if (PayrollStatus.SelectedValue.Equals("T"))
                            empPayrollFilter.add(new Match("ep.EmpPayStatus", "=", "T"));
                        else
                            empPayrollFilter.add(new Match("ep.EmpPayStatus", "<>", "T"));

                        DataRow row = dataTable.NewRow();
                        row[FIELD_EMPNO] = empInfo.EmpNo;
                        row[FIELD_EMPENGFULLNAME] = empInfo.EmpEngFullName;
                        row[FIELD_CHINESENAME] = empInfo.EmpChiFullName;
                        row[FIELD_HKID] = empInfo.EmpHKID;
                        row[FIELD_PERIODFROM] = payPeriod.PayPeriodFr;
                        row[FIELD_PERIODTO] = payPeriod.PayPeriodTo;
                        DBFilter empPosFilter = new DBFilter();

                        EEmpPositionInfo empPos = AppUtils.GetLastPositionInfo(dbConn, payPeriod.PayPeriodTo, empInfo.EmpID);
                        if (empPos != null)
                        {
                            ECompany company = new ECompany();
                            company.CompanyID = empPos.CompanyID;
                            if (ECompany.db.select(dbConn, company))
                                row[FIELD_COMPANY] = company.CompanyCode;

                            EPosition position = new EPosition();
                            position.PositionID = empPos.PositionID;
                            if (EPosition.db.select(dbConn, position))
                                row[FIELD_POSITION] = position.PositionDesc;

                            DBFilter empHierarchyFilter = new DBFilter();
                            empHierarchyFilter.add(new Match("EmpPosID", empPos.EmpPosID));
                            ArrayList empHierarchyList = EEmpHierarchy.db.select(dbConn, empHierarchyFilter);
                            foreach (EEmpHierarchy empHierarchy in empHierarchyList)
                            {
                                EHierarchyLevel hierarchyLevel = (EHierarchyLevel)hierarchyLevelHashTable[empHierarchy.HLevelID];
                                if (hierarchyLevel != null)
                                {
                                    EHierarchyElement hierarchyElement = new EHierarchyElement();
                                    hierarchyElement.HElementID = empHierarchy.HElementID;
                                    if (EHierarchyElement.db.select(dbConn, hierarchyElement))
                                        row[hierarchyLevel.HLevelDesc] = hierarchyElement.HElementDesc;
                                }
                            }

                            EPayrollGroup curentPayGroup = new EPayrollGroup();
                            curentPayGroup.PayGroupID = empPos.PayGroupID;
                            if (EPayrollGroup.db.select(dbConn, curentPayGroup))
                                row[FIELD_PAYROLLGROUP] = curentPayGroup.PayGroupDesc;
                        }

                        double netAmount = 0, releventIncome = 0, nonRelevantIncome = 0, taxableAmount = 0, nonTaxableAmount = 0;
                        double mcER = 0, mcEE = 0;
                        double vcER = 0, vcEE = 0;
                        double pFundER = 0, pFundEE = 0;

                        double wagesByWork = 0;
                        double wagesByRest = 0;
                        double fullPaidLeaveDays = 0;
                        double nonFullPaidLeaveDays = 0;
                        DBFilter paymentRecordFilter = new DBFilter();
                        paymentRecordFilter.add(new IN("EmpPayrollID", "Select EmpPayrollID from " + EEmpPayroll.db.dbclass.tableName + " ep ", empPayrollFilter));
                        paymentRecordFilter.add(new Match("PayRecStatus", "A"));
                        ArrayList paymentRecords = EPaymentRecord.db.select(dbConn, paymentRecordFilter);

                        foreach (EPaymentRecord paymentRecord in paymentRecords)
                        {
                            EPaymentCode payCode = new EPaymentCode();
                            payCode.PaymentCodeID = paymentRecord.PaymentCodeID;
                            EPaymentCode.db.select(dbConn, payCode);
                            //  Always Use Payment Code Description for grouping payment code with same description
                            //string fieldName = PAYMENTCODE_PREFIX + payCode.PaymentCodeDesc;
                            //if (dataTable.Columns[fieldName] == null)
                            //    dataTable.Columns.Add(new DataColumn(fieldName, typeof(double)));
                            //if (row[fieldName] == null || row[fieldName] == DBNull.Value)
                            //    row[fieldName] = 0;
                            //row[fieldName] = (double)row[fieldName] + paymentRecord.PayRecActAmount;


                            netAmount += paymentRecord.PayRecActAmount;
                            if (payCode.PaymentCodeIsMPF)
                                releventIncome += paymentRecord.PayRecActAmount;
                            else
                                nonRelevantIncome += paymentRecord.PayRecActAmount;

                            DBFilter taxPaymentMapFilter = new DBFilter();
                            taxPaymentMapFilter.add(new Match("PaymentCodeID", paymentRecord.PaymentCodeID));
                            if (ETaxPaymentMap.db.count(dbConn, taxPaymentMapFilter) > 0)
                                taxableAmount += paymentRecord.PayRecActAmount;
                            else
                                nonTaxableAmount += paymentRecord.PayRecActAmount;

                            if (payCode.PaymentCodeIsWages)
                                if (paymentRecord.PayRecIsRestDayPayment)
                                    wagesByRest += paymentRecord.PayRecActAmount;
                                else
                                    wagesByWork += paymentRecord.PayRecActAmount;
                        }


                        DBFilter mpfRecordFilter = new DBFilter();
                        mpfRecordFilter.add(new IN("EmpPayrollID", "Select EmpPayrollID from " + EEmpPayroll.db.dbclass.tableName + " ep ", empPayrollFilter));
                        ArrayList mpfRecords = EMPFRecord.db.select(dbConn, mpfRecordFilter);
                        foreach (EMPFRecord mpfRecord in mpfRecords)
                        {
                            vcER += mpfRecord.MPFRecActVCER;
                            mcER += +mpfRecord.MPFRecActMCER;
                            vcEE += mpfRecord.MPFRecActVCEE;
                            mcEE += mpfRecord.MPFRecActMCEE;
                        }
                        ArrayList orsoRecords = EORSORecord.db.select(dbConn, mpfRecordFilter);
                        foreach (EORSORecord orsoRecord in orsoRecords)
                        {
                            pFundER += orsoRecord.ORSORecActER;
                            pFundEE += orsoRecord.ORSORecActEE;
                        }
                        row[FIELD_WAGESWORK] = wagesByWork;

                        DBFilter workingSummaryFilter = new DBFilter();
                        workingSummaryFilter.add(new Match("EmpWorkingSummaryAsOfDate", ">=", payPeriod.PayPeriodFr < empInfo.EmpDateOfJoin ? empInfo.EmpDateOfJoin : payPeriod.PayPeriodFr));
                        if (empTerm != null)
                            workingSummaryFilter.add(new Match("EmpWorkingSummaryAsOfDate", "<=", payPeriod.PayPeriodTo > empTerm.EmpTermLastDate ? empTerm.EmpTermLastDate : payPeriod.PayPeriodTo));
                        else
                            workingSummaryFilter.add(new Match("EmpWorkingSummaryAsOfDate", "<=", payPeriod.PayPeriodTo));
                        workingSummaryFilter.add(new Match("EmpID", empInfo.EmpID));

                        ArrayList empWorkingSummaryList = EEmpWorkingSummary.db.select(dbConn, workingSummaryFilter);

                        double workHourTotal = 0, restDayTotal = 0;


                        foreach (EEmpWorkingSummary empWorkSummary in empWorkingSummaryList)
                        {
                            workHourTotal += empWorkSummary.EmpWorkingSummaryTotalWorkingHours;
                            restDayTotal += empWorkSummary.EmpWorkingSummaryRestDayEntitled;
                        }

                        row[FIELD_WORKHOURTOTAL] = workHourTotal;
                        row[FIELD_RESTDAYTOTAL] = restDayTotal;

                        DBFilter statutoryHolidayFilter = new DBFilter();
                        statutoryHolidayFilter.add(new Match("StatutoryHolidayDate", ">=", payPeriod.PayPeriodFr < empInfo.EmpDateOfJoin ? empInfo.EmpDateOfJoin : payPeriod.PayPeriodFr));
                        if (empTerm != null)
                            statutoryHolidayFilter.add(new Match("StatutoryHolidayDate", "<=", payPeriod.PayPeriodTo > empTerm.EmpTermLastDate ? empTerm.EmpTermLastDate : payPeriod.PayPeriodTo));
                        else
                            statutoryHolidayFilter.add(new Match("StatutoryHolidayDate", "<=", payPeriod.PayPeriodTo));

                        ArrayList statutoryHolidayList = EStatutoryHoliday.db.select(dbConn, statutoryHolidayFilter);

                        double restDayCount = 0;
                        foreach (EStatutoryHoliday statutoryHoliday in statutoryHolidayList)
                            restDayCount++;

                        row[FIELD_STATUTORYHOLIDAYTOTAL] = restDayCount;

                        DBFilter LeaveAppEmpPayrollFilter = new DBFilter();
                        LeaveAppEmpPayrollFilter.add(new IN("EmpPayrollID", "Select EmpPayrollID from " + EEmpPayroll.db.dbclass.tableName + " ep ", empPayrollFilter));
                        ArrayList LeaveAppEmpPayrollLists = ELeaveApplication.db.select(dbConn, LeaveAppEmpPayrollFilter);
                        foreach (ELeaveApplication leaveApp in LeaveAppEmpPayrollLists)
                        {

                            ELeaveCode leaveCode = new ELeaveCode();
                            leaveCode.LeaveCodeID = leaveApp.LeaveCodeID;
                            if (ELeaveCode.db.select(dbConn, leaveCode))
                            {
                                if (leaveCode.LeaveCodePayRatio >= 1)
                                    fullPaidLeaveDays += leaveApp.LeaveAppDays;
                                else
                                    nonFullPaidLeaveDays += leaveApp.LeaveAppDays;
                            }
                        }
                        row[FIELD_FULLPAIDLEAVETOTAL] = fullPaidLeaveDays;
                        row[FIELD_NONFULLPAIDLEAVETOTAL] = nonFullPaidLeaveDays;

                        dataTable.Rows.Add(row);
                    }
                }
            }

            //DBFilter paymentCodeFilter = new DBFilter();
            //paymentCodeFilter.add("PaymentCodeDisplaySeqNo", false);
            //paymentCodeFilter.add("PaymentCode", false);
            //ArrayList paymentCodeList = EPaymentCode.db.select(dbConn, paymentCodeFilter);
            //foreach (EPaymentCode paymentCode in paymentCodeList)
            //{
            //    if (dataTable.Columns.Contains(PAYMENTCODE_PREFIX + paymentCode.PaymentCodeDesc))
            //    {
            //        DataColumn paymentColumn = dataTable.Columns[PAYMENTCODE_PREFIX + paymentCode.PaymentCodeDesc];
            //        paymentColumn.SetOrdinal(firstDetailColumnPos);
            //        if (!dataTable.Columns.Contains(paymentCode.PaymentCodeDesc))
            //            paymentColumn.ColumnName = paymentCode.PaymentCodeDesc;
            //        else
            //        {
            //            Console.Write("System reserved payment column is used");
            //        }
            //    }
            //}

            //for (int i = firstSummaryColumnPos; i < firstDetailColumnPos; i++)
            //    dataTable.Columns[firstSummaryColumnPos].SetOrdinal(dataTable.Columns.Count - 1);


            export.Update(dataSet);

            System.IO.FileStream excelfileStream = new System.IO.FileStream(exportFileName, System.IO.FileMode.Open);
            NPOI.HSSF.UserModel.HSSFWorkbook workbook = new NPOI.HSSF.UserModel.HSSFWorkbook(excelfileStream);
            NPOI.HSSF.UserModel.HSSFSheet workSheet = (NPOI.HSSF.UserModel.HSSFSheet)workbook.GetSheetAt(0);
            workSheet.ShiftRows(workSheet.FirstRowNum, workSheet.LastRowNum, 1);
            NPOI.HSSF.UserModel.HSSFRow excelRow = (NPOI.HSSF.UserModel.HSSFRow)workSheet.GetRow(0);
            if (excelRow == null)
                excelRow = (NPOI.HSSF.UserModel.HSSFRow)workSheet.CreateRow(0);
            NPOI.HSSF.UserModel.HSSFCell excelCell = (NPOI.HSSF.UserModel.HSSFCell)excelRow.GetCell(0);
            if (excelCell == null)
                excelCell = (NPOI.HSSF.UserModel.HSSFCell)excelRow.CreateCell(0);
            excelCell.SetCellValue("Statutory Minimum Wage Summary Report");

            excelfileStream = new System.IO.FileStream(exportFileName, System.IO.FileMode.Open);
            workbook.Write(excelfileStream);
            excelfileStream.Close();

            WebUtils.TransmitFile(Response, exportFileName, "StatutoryMinimumWageSummary_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true);
            return;
        }
        else
        {
            PageErrors errors = PageErrors.getErrors(db, Page.Master);
            errors.addError("Employee not selected");
        }
    }

    protected void Search_Click(object sender, EventArgs e)
    {
        info.page = 0;
        view = loadData(info, EEmpPersonalInfo.db, Repeater);

    }
    protected void Reset_Click(object sender, EventArgs e)
    {
        EmployeeSearchControl1.Reset();
        sbinding.clear();
        info.page = 0;
        view = loadData(info, EEmpPersonalInfo.db, Repeater);


    }
    protected void Payroll_ConfirmedPeriod_List1_AfterSearch(object sender, EventArgs e)
    {
        view = loadData(info, EEmpPersonalInfo.db, Repeater);
    }
}
