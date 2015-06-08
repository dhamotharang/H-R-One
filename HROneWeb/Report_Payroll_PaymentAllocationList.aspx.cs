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

public partial class Report_Payroll_PaymentAllocationList : HROneWebPage
{
    private const string FUNCTION_CODE = "RPT209";
    protected SearchBinding sbinding;
    // Start 0000185, KuangWei, 2015-05-05
    protected DBManager db = EEmpPersonalInfo.db;
    protected SearchBinding binding;
    protected ListInfo info;
    protected DataView view;
    // End 0000185, KuangWei, 2015-05-05
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        sbinding = new SearchBinding(dbConn, EHierarchyElement.db);
        //sbinding.add(new DropDownVLSearchBinder(HLevelID, "HLevelID", EHierarchyLevel.VLHierarchy, false));
        //sbinding.init(DecryptedRequest, null);


        DBFilter hierarchyLevelFilter = new DBFilter();
        // additional grouping method not limited to hierarchy setting
        hierarchyLevelFilter.add("HLevelSeqNo", true);
        ArrayList hierarchyLevelList = EHierarchyLevel.db.select(dbConn, hierarchyLevelFilter);

        foreach (EHierarchyLevel hlevel in hierarchyLevelList)
        {
            HLevelID.Items.Add(new ListItem(hlevel.HLevelCode + " - " + hlevel.HLevelDesc, hlevel.HLevelID.ToString()));
        }

        
        HLevelID.Items.Add(new ListItem("STAFF - Individual Staff", "-1"));

        // Start 0000185, KuangWei, 2015-05-05
        binding = new SearchBinding(dbConn, db);
        binding.init(DecryptedRequest, null);

        info = ListFooter.ListInfo;
        // End 0000185, KuangWei, 2015-05-05

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }


    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(EPayrollGroup.db, Page.Master);

        DateTime dtPeriodFrom = new DateTime();
        DateTime dtPeriodTo = new DateTime();
        int intHierarchyLevelID=0;
        if (!string.IsNullOrEmpty(PayPeriodFr.Value))
            if (!DateTime.TryParse(PayPeriodFr.Value, out dtPeriodFrom))
            {
                errors.addError(HROne.Translation.PageErrorMessage.ERROR_DATE_FORMAT);
                return;
            }
        if (!string.IsNullOrEmpty(PayPeriodTo.Value))
            if (!DateTime.TryParse(PayPeriodTo.Value, out dtPeriodTo))
            {
                errors.addError(HROne.Translation.PageErrorMessage.ERROR_DATE_FORMAT);
                return;
            }
        if (!int.TryParse(HLevelID.SelectedValue,out intHierarchyLevelID))
        {
            errors.addError("Invalid Hierarchy Level");
        }

        // Start 0000185, KuangWei, 2015-05-05
        ArrayList empList = WebUtils.SelectedRepeaterItemToBaseObjectList(db, Repeater, "ItemSelect");

        if (empList.Count <= 0)
        {
            errors.addError("No Employees selected");
        }

        if (errors.isEmpty())
        {
            HROne.Reports.Payroll.PaymentAllocationListProcess reportProcess = new HROne.Reports.Payroll.PaymentAllocationListProcess(dbConn, ci, dtPeriodFrom, dtPeriodTo, intHierarchyLevelID, WebUtils.GetCurUser(Session), empList);
            // End 0000185, KuangWei, 2015-05-05

            if (Response.IsClientConnected)
            {
                HROneConfig config = HROneConfig.GetCurrentHROneConfig();
                if (config.GenerateReportAsInbox)
                {
                    if (EInboxAttachment.GetTotalSize(dbConn, 0) < WebUtils.productLicense(Session).MaxInboxSizeMB * 1000 * 1000)
                    {
                        HROne.TaskService.GenericExcelReportTaskFactory reportTask = new HROne.TaskService.GenericExcelReportTaskFactory(dbConn, user, lblReportHeader.Text, reportProcess, "PaymentAllocationList");
                        AppUtils.reportTaskQueueService.AddTask(reportTask);
                        errors.addError(HROne.Translation.PageMessage.REPORT_GENERATING_TO_INBOX);
                    }
                    else
                        errors.addError(HROne.Translation.PageMessage.INBOX_SIZE_EXCEEDED);
                }
                else
                {
                    System.IO.FileInfo excelFile = reportProcess.GenerateExcelReport();
                    WebUtils.TransmitFile(Response, excelFile.FullName, "PaymentAllocationList_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true);
                }
            }
        }
        //{
        //    string hierarchyLevelName = string.Empty;

        //    DataSet dataSet = new DataSet();

        //    DataTable empInfoTable = new DataTable("EmpInfo");
        //    DataTable payrollTable = new DataTable("Payment");

        //    dataSet.Tables.Add(empInfoTable);
        //    dataSet.Tables.Add(payrollTable);

        //    payrollTable.Columns.Add("EmpPayrollID", typeof(string));
        //    int firstDetailColumnPos = payrollTable.Columns.Count;


        //    empInfoTable.Columns.Add("EmployeeID", typeof(string));
        //    empInfoTable.Columns.Add("EmpPayrollID", typeof(int));
        //    empInfoTable.Columns.Add("EnglishName", typeof(string));
        //    empInfoTable.Columns.Add("Alias", typeof(string));

        //    empInfoTable.Columns.Add("Company", typeof(string));
        //    DBFilter hierarchyLevelFilter = new DBFilter();
        //    Hashtable hierarchyLevelHashTable = new Hashtable();
        //    hierarchyLevelFilter.add("HLevelSeqNo", true);
        //    ArrayList hierarchyLevelList = EHierarchyLevel.db.select(dbConn, hierarchyLevelFilter);
        //    foreach (EHierarchyLevel hlevel in hierarchyLevelList)
        //    {

        //        if (hlevel.HLevelID.Equals(intHierarchyLevelID))
        //            hierarchyLevelName = hlevel.HLevelDesc;
        //        empInfoTable.Columns.Add(hlevel.HLevelDesc, typeof(string));
        //        hierarchyLevelHashTable.Add(hlevel.HLevelID, hlevel);
        //    }
        //    empInfoTable.Columns.Add("Payroll Group", typeof(string));

        //    empInfoTable.Columns.Add("Date Join", typeof(DateTime));
        //    empInfoTable.Columns.Add("Date Left", typeof(DateTime));
        //    empInfoTable.Columns.Add("Net Payable", typeof(double));
        //    empInfoTable.Columns.Add("MCEE", typeof(double));
        //    empInfoTable.Columns.Add("MCER", typeof(double));
        //    empInfoTable.Columns.Add("VCEE", typeof(double));
        //    empInfoTable.Columns.Add("VCER", typeof(double));
        //    empInfoTable.Columns.Add("PFundEE", typeof(double));
        //    empInfoTable.Columns.Add("PFundER", typeof(double));

        //    DBFilter payPeriodFilter = new DBFilter();
        //    if (!dtPeriodFrom.Ticks.Equals(0))
        //        payPeriodFilter.add(new Match("PayPeriodTo", ">=", dtPeriodFrom));
        //    if (!dtPeriodTo.Ticks.Equals(0))
        //        payPeriodFilter.add(new Match("PayPeriodTo", "<=", dtPeriodTo));


        //    ArrayList payPeriodList = EPayrollPeriod.db.select(dbConn, payPeriodFilter);
        //    if (payPeriodList.Count > 0)
        //    {
        //        // reset period
        //        dtPeriodFrom = new DateTime();
        //        dtPeriodTo = new DateTime();
        //    }
        //    foreach (EPayrollPeriod payPeriod in payPeriodList)
        //    {
        //        if (dtPeriodFrom > payPeriod.PayPeriodFr || dtPeriodFrom.Ticks.Equals(0))
        //            dtPeriodFrom = payPeriod.PayPeriodFr;
        //        if (dtPeriodTo < payPeriod.PayPeriodTo || dtPeriodTo.Ticks.Equals(0))
        //            dtPeriodTo = payPeriod.PayPeriodTo;



        //        DBFilter empPayrollFilter = new DBFilter();
        //        empPayrollFilter.add(new Match("PayPeriodID", payPeriod.PayPeriodID));
        //        ArrayList empPayrollList = EEmpPayroll.db.select(dbConn, empPayrollFilter);

        //        foreach (EEmpPayroll empPayroll in empPayrollList)
        //        {
        //            EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
        //            empInfo.EmpID = empPayroll.EmpID;
        //            EEmpPersonalInfo.db.select(dbConn, empInfo);

        //            DataRow row = empInfoTable.NewRow();
        //            DataRow paymentRow = payrollTable.NewRow();

        //            row["EmployeeID"] = empInfo.EmpNo;
        //            row["EmpPayrollID"] = empPayroll.EmpPayrollID;
        //            paymentRow["EmpPayrollID"] = empPayroll.EmpPayrollID;
        //            row["EnglishName"] = empInfo.EmpEngFullName;
        //            row["Alias"] = empInfo.EmpAlias;

        //            row["Date Join"] = empInfo.EmpDateOfJoin;
        //            DBFilter empTerminationFilter = new DBFilter();
        //            empTerminationFilter.add(new Match("EmpID", empInfo.EmpID));
        //            ArrayList empTerminationList = EEmpTermination.db.select(dbConn, empTerminationFilter);
        //            if (empTerminationList.Count > 0)
        //                row["Date Left"] = ((EEmpTermination)empTerminationList[0]).EmpTermLastDate;

        //            DBFilter empPosFilter = new DBFilter();

        //            EEmpPositionInfo empPos = AppUtils.GetLastPositionInfo(dbConn, payPeriod.PayPeriodTo, empInfo.EmpID);
        //            if (empPos != null)
        //            {
        //                ECompany company = new ECompany();
        //                company.CompanyID = empPos.CompanyID;
        //                if (ECompany.db.select(dbConn, company))
        //                    row["Company"] = company.CompanyName;

        //                EPayrollGroup payrollGroup = new EPayrollGroup();
        //                payrollGroup.PayGroupID = empPos.PayGroupID;
        //                if (EPayrollGroup.db.select(dbConn, payrollGroup))
        //                    row["Payroll Group"] = payrollGroup.PayGroupDesc;
        //                DBFilter empHierarchyFilter = new DBFilter();
        //                empHierarchyFilter.add(new Match("EmpPosID", empPos.EmpPosID));
        //                ArrayList empHierarchyList = EEmpHierarchy.db.select(dbConn, empHierarchyFilter);
        //                foreach (EEmpHierarchy empHierarchy in empHierarchyList)
        //                {
        //                    EHierarchyLevel hierarchyLevel = (EHierarchyLevel)hierarchyLevelHashTable[empHierarchy.HLevelID];
        //                    if (hierarchyLevel != null)
        //                    {
        //                        EHierarchyElement hierarchyElement = new EHierarchyElement();
        //                        hierarchyElement.HElementID = empHierarchy.HElementID;
        //                        if (EHierarchyElement.db.select(dbConn, hierarchyElement))
        //                            row[hierarchyLevel.HLevelDesc] = hierarchyElement.HElementDesc;
        //                    }
        //                }
        //            }


        //            double netAmount = 0, releventIncome = 0, nonRelevantIncome = 0, MCEE = 0, MCER = 0, VCEE=0, VCER=0, PFUNDEE = 0, PFUNDER = 0;

        //            DBFilter paymentRecordFilter = new DBFilter();
        //            paymentRecordFilter.add(new Match("EmpPayrollID", empPayroll.EmpPayrollID));
        //            paymentRecordFilter.add(new Match("PayRecStatus", "A"));
        //            ArrayList paymentRecords = EPaymentRecord.db.select(dbConn, paymentRecordFilter);

        //            foreach (EPaymentRecord paymentRecord in paymentRecords)
        //            {
        //                EPaymentCode payCode = new EPaymentCode();
        //                payCode.PaymentCodeID = paymentRecord.PaymentCodeID;
        //                EPaymentCode.db.select(dbConn, payCode);
        //                string fieldName = payCode.PaymentCodeDesc;
        //                if (payrollTable.Columns[fieldName] == null)
        //                    payrollTable.Columns.Add(new DataColumn(fieldName, typeof(double)));
        //                if (paymentRow[fieldName] == null || paymentRow[fieldName] == DBNull.Value)
        //                    paymentRow[fieldName] = 0;
        //                paymentRow[fieldName] = (double)paymentRow[fieldName] + paymentRecord.PayRecActAmount;


        //                netAmount += paymentRecord.PayRecActAmount;
        //                if (payCode.PaymentCodeIsMPF)
        //                    releventIncome += paymentRecord.PayRecActAmount;
        //                else
        //                    nonRelevantIncome += paymentRecord.PayRecActAmount;

        //            }

        //            row["Net Payable"] = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(netAmount, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), ExchangeCurrency.DefaultCurrencyDecimalPlaces());
        //            //row["Relevant Income"] = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(releventIncome, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), ExchangeCurrency.DefaultCurrencyDecimalPlaces());
        //            //row["Non-Relevant Income"] = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(nonRelevantIncome, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), ExchangeCurrency.DefaultCurrencyDecimalPlaces());

        //            DBFilter mpfRecordFilter = new DBFilter();
        //            mpfRecordFilter.add(new Match("EmpPayrollID", empPayroll.EmpPayrollID));
        //            ArrayList mpfRecords = EMPFRecord.db.select(dbConn, mpfRecordFilter);
        //            foreach (EMPFRecord mpfRecord in mpfRecords)
        //            {
        //                VCER += mpfRecord.MPFRecActVCER;
        //                MCER += mpfRecord.MPFRecActMCER;
        //                VCEE += mpfRecord.MPFRecActVCEE;
        //                MCEE += mpfRecord.MPFRecActMCEE;
        //            }
        //            row["MCEE"] = MCEE;
        //            row["VCEE"] = VCEE;
        //            row["MCER"] = MCER;
        //            row["VCER"] = VCER;
        //            ArrayList orsoRecords = EORSORecord.db.select(dbConn, mpfRecordFilter);
        //            foreach (EORSORecord orsoRecord in orsoRecords)
        //            {
        //                PFUNDER += orsoRecord.ORSORecActER;
        //                PFUNDEE += orsoRecord.ORSORecActEE;
        //            }
        //            row["PFundEE"] = PFUNDEE;
        //            row["PFundER"] = PFUNDER;

        //            empInfoTable.Rows.Add(row);
        //            payrollTable.Rows.Add(paymentRow);
        //        }
        //    }
        //    DBFilter paymentCodeFilter = new DBFilter();
        //    paymentCodeFilter.add("PaymentCodeDisplaySeqNo", false);
        //    paymentCodeFilter.add("PaymentCode", false);
        //    ArrayList paymentCodeList = EPaymentCode.db.select(dbConn, paymentCodeFilter);
        //    foreach (EPaymentCode paymentCode in paymentCodeList)
        //    {
        //        if (payrollTable.Columns.Contains(paymentCode.PaymentCodeDesc))
        //            payrollTable.Columns[paymentCode.PaymentCodeDesc].SetOrdinal(firstDetailColumnPos);
        //    }
        //    GenerateReport(dataSet, dtPeriodFrom, dtPeriodTo, hierarchyLevelName);
        //    return;

        //}
    }

    // Start 0000185, KuangWei, 2015-05-05
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            view = loadData(info, db, Repeater);
        }
    }

    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = binding.createFilter();

        string select = "e.*";
        string from = "from [" + db.dbclass.tableName + "] e ";

        DBFilter empInfoFilter = EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime(), AppUtils.ServerDateTime());
        empInfoFilter.add(new MatchField("e.EmpID", "ee.EmpID"));
        filter.add(new Exists(EEmpPersonalInfo.db.dbclass.tableName + " ee", empInfoFilter));

        DataTable table = filter.loadData(dbConn, null, select, from);
        table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);

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
        binding.clear();
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

    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        cb.Checked = true; 
        WebFormUtils.LoadKeys(db, row, cb);
    }

    protected void Gender_SelectedIndexChanged(object sender, EventArgs e)
    {
        info.page = 0;
        view = loadData(info, db, Repeater);
    }
    // End 0000185, KuangWei, 2015-05-05

//    private void GenerateReport(DataSet dataSet, DateTime PeriodFrom, DateTime PeriodTo, string hierarchyLevelGroupingFieldName)
//    {
//        //org.in2bits.MyXls.XlsDocument xlsDoc= new org.in2bits.MyXls.XlsDocument();
//        //org.in2bits.MyXls.Workbook workbook = xlsDoc.Workbook;
//        //org.in2bits.MyXls.Worksheet workSheet= workbook.Worksheets.Add("Payroll Detail");

//        NPOI.HSSF.UserModel.HSSFWorkbook workBook = new NPOI.HSSF.UserModel.HSSFWorkbook();
//        NPOI.HSSF.UserModel.HSSFSheet workSheet = (NPOI.HSSF.UserModel.HSSFSheet)workBook.CreateSheet("Payroll Detail");

//        ushort rowPos = 0;

//        DataTable empInfoTable = dataSet.Tables["EmpInfo"];
//        DataTable paymentTable = dataSet.Tables["payment"];
//        DataTable costCenterDetailTable = dataSet.Tables["CostCenterDetail"];

//        NPOI.HSSF.UserModel.HSSFFont boldFont = (NPOI.HSSF.UserModel.HSSFFont)workBook.CreateFont();
//        boldFont.Boldweight = 700;  

//        NPOI.HSSF.UserModel.HSSFCellStyle reportHeaderStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workBook.CreateCellStyle();
//        reportHeaderStyle.SetFont(boldFont);
//        reportHeaderStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;

//        NPOI.HSSF.UserModel.HSSFCellStyle groupHeaderStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workBook.CreateCellStyle();
//        groupHeaderStyle.SetFont(boldFont);

//        NPOI.HSSF.UserModel.HSSFCellStyle columnHeaderStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workBook.CreateCellStyle();
//        columnHeaderStyle.SetFont(boldFont);
//        columnHeaderStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;

//        NPOI.HSSF.UserModel.HSSFCellStyle detailStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workBook.CreateCellStyle();
//        detailStyle.DataFormat = workBook.CreateDataFormat().GetFormat("#,##0.00;(#,##0.00)");

//        NPOI.HSSF.UserModel.HSSFCellStyle subTotalStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workBook.CreateCellStyle();
//        subTotalStyle.DataFormat = workBook.CreateDataFormat().GetFormat("#,##0.00;(#,##0.00)");
//        subTotalStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;

//        //rowPos = GenerateHeader(xlsDoc, workSheet, PeriodFrom, PeriodTo, rowPos);
//        rowPos = GenerateHeader(workSheet, PeriodFrom, PeriodTo, rowPos, reportHeaderStyle, groupHeaderStyle);

//        DataRow[] rows = dataSet.Tables["EmpInfo"].Select("", "Company, " + empInfoTable.Columns[hierarchyLevelGroupingFieldName].ColumnName + ", EnglishName, Alias");

//        string currentCompany = string.Empty;
//        string currentHierarchyGroup = string.Empty;
//        ushort groupRowCount = 0;
//        Hashtable groupedTotalCostCenterHash = new Hashtable();
//        ArrayList subTotalRowList = new ArrayList();
//        Hashtable paymentHashTable = new Hashtable();
//        Hashtable companyTotalHashTable = new Hashtable();
//        double netPayment = 0;
//        double companyNetPayment = 0;

//        double employerContribution = 0;
//        double companyEmployerContribution = 0;

//        ArrayList companyTotalRowNumList = new ArrayList();

//        foreach (DataRow row in rows)
//        {

//            string tmpCompany = row["Company"].ToString();
//            string tmpHierarchy = row[hierarchyLevelGroupingFieldName].ToString();


//            if (!currentCompany.Equals(tmpCompany, StringComparison.CurrentCultureIgnoreCase) || rows[0] == row)
//            {
//                if (rows[0] != row)
//                {
//                    groupRowCount++;

//                    rowPos = GenerateHierarchyTotal(workSheet, paymentTable, currentHierarchyGroup, paymentHashTable, netPayment, employerContribution, rowPos, detailStyle);

//                    paymentHashTable = new Hashtable();


//                    rowPos = GenerateCompanyTotal(workSheet, paymentTable, groupRowCount, companyEmployerContribution, rowPos, subTotalStyle);

//                    companyTotalRowNumList.Add(rowPos);

//                    companyTotalHashTable = new Hashtable();
//                }
//                groupRowCount = 0;
                
//                netPayment = 0;
//                employerContribution = 0;
//                currentHierarchyGroup = tmpHierarchy;

//                companyNetPayment = 0;
//                companyEmployerContribution = 0;
//                currentCompany = tmpCompany;
//                rowPos = GenerateColumnHeader(workSheet, dataSet, currentCompany, hierarchyLevelGroupingFieldName, rowPos, groupHeaderStyle, columnHeaderStyle);
//            }
//            else if (!currentHierarchyGroup.Equals(tmpHierarchy, StringComparison.CurrentCultureIgnoreCase))
//            {
//                groupRowCount++;

//                rowPos = GenerateHierarchyTotal(workSheet, paymentTable, currentHierarchyGroup, paymentHashTable, netPayment, employerContribution, rowPos, detailStyle);

//                paymentHashTable = new Hashtable();
//                netPayment = 0;
//                employerContribution = 0;
//                currentHierarchyGroup = tmpHierarchy;
//            }
//            DataRow[] paymentRows = paymentTable.Select("[EmpPayrollID]='" + row["EmpPayrollID"].ToString() + "'");

//            foreach (DataRow paymentRow in paymentRows)
//            {


//                foreach (DataColumn column in paymentTable.Columns)
//                {
//                    if (column.ColumnName.Equals("EmpPayrollID"))
//                        continue;
//                    double amount = 0;
//                    double companyTotalAmount = 0;
//                    if (paymentHashTable.ContainsKey(column.ColumnName))
//                        amount = (double)paymentHashTable[column.ColumnName];
//                    else
//                        paymentHashTable.Add(column.ColumnName, amount);

//                    if (companyTotalHashTable.ContainsKey(column.ColumnName))
//                        companyTotalAmount = (double)companyTotalHashTable[column.ColumnName];
//                    else
//                        companyTotalHashTable.Add(column.ColumnName, companyTotalAmount);

//                    if (!string.IsNullOrEmpty(paymentRow[column.ColumnName].ToString()))
//                    {
//                        amount += (double)paymentRow[column.ColumnName];
//                        companyTotalAmount += (double)paymentRow[column.ColumnName];
//                        netPayment += (double)paymentRow[column.ColumnName];
//                        companyNetPayment += (double)paymentRow[column.ColumnName];
//                        paymentHashTable[column.ColumnName] = amount;
//                        companyTotalHashTable[column.ColumnName] = companyTotalAmount;
//                    }
//                }
                
//            }
//            if (!row.IsNull("MCER"))
//            {
//                double contribution=(double)row["MCER"];
//                employerContribution += contribution;
//                companyEmployerContribution += contribution;
//            }
//            if (!row.IsNull("VCER"))
//            {
//                double contribution = (double)row["VCER"];
//                employerContribution += contribution;
//                companyEmployerContribution += contribution;
//            }
//            if (!row.IsNull("PFundER"))
//            {
//                double contribution = (double)row["PFundER"];
//                employerContribution += contribution;
//                companyEmployerContribution += contribution;
//            }
//        }

//        if (paymentHashTable.Count > 0)
//        {
//            groupRowCount++;

//            rowPos = GenerateHierarchyTotal(workSheet, paymentTable, currentHierarchyGroup, paymentHashTable, netPayment, employerContribution, rowPos, detailStyle);

//            paymentHashTable = new Hashtable();
//            netPayment = 0;
//            employerContribution = 0;
//        }

//        if (groupRowCount > 0)
//        {
//            rowPos = GenerateCompanyTotal(workSheet, paymentTable, groupRowCount, companyEmployerContribution, rowPos, subTotalStyle);
//            companyTotalRowNumList.Add(rowPos);

//        }
//        if (companyTotalRowNumList.Count > 0)
//            rowPos = GenerateTotal(workSheet, paymentTable, companyTotalRowNumList, rowPos, subTotalStyle);
//        //workSheet.Cells.Merge(1, 1, 1, paymentTable.Columns.Count + 2);
//        //workSheet.Rows[1].GetCell(1).Font.Bold = true;
//        //workSheet.Rows[1].GetCell(1).Font.Height = 300;
//        //workSheet.Rows[1].GetCell(1).HorizontalAlignment = org.in2bits.MyXls.HorizontalAlignments.Centered;
//        workSheet.AddMergedRegion(new NPOI.SS.Util.Region( 0, 0, 0, paymentTable.Columns.Count + 1));

//        for (int i = 0; i <= paymentTable.Columns.Count + 1; i++)
//        {
//            //org.in2bits.MyXls.ColumnInfo columnInfo = new org.in2bits.MyXls.ColumnInfo(xlsDoc, workSheet);
//            //workSheet.AddColumnInfo(columnInfo);
//            //columnInfo.ColumnIndexStart = 0;
//            //columnInfo.ColumnIndexEnd = (ushort)(paymentTable.Columns.Count + 2);
//            //columnInfo.Width = 15 * 254;
//            //columnInfo.Collapsed = true;
//            workSheet.SetColumnWidth(i, 15 * 254);
//        }
        
//        //#region Add Grant Total Row
//        //workSheet.Table.Rows.Add(new CarlosAg.ExcelXmlWriter.WorksheetRow());
//        //CarlosAg.ExcelXmlWriter.WorksheetRow grantTotalRow = new CarlosAg.ExcelXmlWriter.WorksheetRow();
//        //workSheet.Table.Rows.Add(grantTotalRow);
//        //grantTotalRow.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("Grand Total", CarlosAg.ExcelXmlWriter.DataType.String, "subTotal"));
//        //grantTotalRow.Cells[0].MergeAcross = 2;//.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(string.Empty, CarlosAg.ExcelXmlWriter.DataType.String, "subTotal"));

//        //string grandTotalFormula=string.Empty;
//        //foreach (int subTotalRowIndex in subTotalRowList)
//        //{
//        //    string cellString="R[" + (subTotalRowIndex-workSheet.Table.Rows.Count  )+ "]C";
//        //    if (string.IsNullOrEmpty(grandTotalFormula))
//        //        grandTotalFormula = cellString;
//        //    else
//        //        grandTotalFormula += "+" + cellString;

//        //}
//        //foreach (DataColumn column in paymentTable.Columns)
//        //{
//        //    if (column.ColumnName.Equals("EmployeeID"))
//        //        continue;
//        //    CarlosAg.ExcelXmlWriter.WorksheetCell subTotalCell = new CarlosAg.ExcelXmlWriter.WorksheetCell();//"0", CarlosAg.ExcelXmlWriter.DataType.Number, "detail");

//        //    subTotalCell.Formula = "=" + grandTotalFormula;
//        //    subTotalCell.StyleID = "subTotal";
//        //    grantTotalRow.Cells.Add(subTotalCell);
//        //}
//        //CarlosAg.ExcelXmlWriter.WorksheetCell netPaymentTotalCell = new CarlosAg.ExcelXmlWriter.WorksheetCell();//"0", CarlosAg.ExcelXmlWriter.DataType.Number, "detail");
//        //netPaymentTotalCell.Formula = "=" + grandTotalFormula;
//        //netPaymentTotalCell.StyleID = "subTotal";
//        //grantTotalRow.Cells.Add(netPaymentTotalCell);
//        //#endregion

//        string exportFileName = System.IO.Path.GetTempFileName();
//        System.IO.File.Delete(exportFileName);
//        exportFileName += ".xls";

//        //xlsDoc.FileName = exportFileName;
//        //xlsDoc.Save();
//        System.IO.FileStream file = new System.IO.FileStream(exportFileName, System.IO.FileMode.Create);
//        workBook.Write(file);
//        file.Close();

//        //WebUtils.RegisterDownloadFileJavaScript(this, exportFileName, "PaymentAllocationList_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true, 0);
//        WebUtils.TransmitFile(Response, exportFileName, "PaymentAllocationList_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true);
//    }

//    //private ushort GenerateColumnHeader(org.in2bits.MyXls.XlsDocument xlsDoc, org.in2bits.MyXls.Worksheet workSheet, DataSet dataSet, string CompanyHeader, string HierarchyHeader, ushort RowPos)
//    private ushort GenerateColumnHeader(NPOI.HSSF.UserModel.HSSFSheet workSheet, DataSet dataSet, string CompanyHeader, string HierarchyHeader, ushort RowPos, NPOI.HSSF.UserModel.HSSFCellStyle GroupHeaderStyle, NPOI.HSSF.UserModel.HSSFCellStyle ColumnHeaderStyle)
//    {
//        DataTable paymentTable = dataSet.Tables["payment"];
//        //org.in2bits.MyXls.XF xf = xlsDoc.NewXF();
//        //xf.Font.Bold = true;
//        //xf.BottomLineStyle = (ushort)1;

//        RowPos+=2;
//        //workSheet.Cells.Add((int)RowPos, 1, "Company");
//        //workSheet.Cells.Add((int)RowPos, 2, CompanyHeader);
//        //workSheet.Rows[RowPos].GetCell(1).Font.Bold = true;
//        //workSheet.Rows[RowPos].GetCell(2).Font.Bold = true;

//        NPOI.HSSF.UserModel.HSSFRow row= (NPOI.HSSF.UserModel.HSSFRow)workSheet.CreateRow((int)RowPos);
//        NPOI.HSSF.UserModel.HSSFCell cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(0);
//        cell.SetCellValue("Company");
//        cell.CellStyle = GroupHeaderStyle;
//        cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(1);
//        cell.SetCellValue(CompanyHeader);
//        cell.CellStyle = GroupHeaderStyle;

//        RowPos+=2;
//        //workSheet.Cells.Add((int)RowPos, 1, HierarchyHeader, xf);
//        row = (NPOI.HSSF.UserModel.HSSFRow)workSheet.CreateRow((int)RowPos);
//        cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(0);
//        cell.SetCellValue(HierarchyHeader);
//        cell.CellStyle = ColumnHeaderStyle;

//        ushort colPos = 0;
//        foreach (DataColumn column in paymentTable.Columns)
//        {
//            if (column.ColumnName.Equals("EmpPayrollID"))
//                continue;
//            colPos++;
//            //workSheet.Cells.Add(RowPos, colPos, column.ColumnName, xf);
//            cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(colPos);
//            cell.SetCellValue(column.ColumnName);
//            cell.CellStyle = ColumnHeaderStyle;

//        }
//        colPos++;
//        //workSheet.Cells.Add(RowPos, colPos, "Net Pay", xf);
//        cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(colPos);
//        cell.SetCellValue("Net Pay");
//        cell.CellStyle = ColumnHeaderStyle;
//        colPos++;
//        //workSheet.Cells.Add(RowPos, colPos, "Employer Contribution", xf);
//        cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(colPos);
//        cell.SetCellValue("Employer Contribution");
//        cell.CellStyle = ColumnHeaderStyle;

//        //workSheet.Cells.Merge((int)RowPos - 2, (int)RowPos - 2, 2, colPos);
//        workSheet.AddMergedRegion(new NPOI.SS.Util.Region((int)RowPos - 2, 1, (int)RowPos - 2, colPos));

//        return RowPos;
//    }


//    //private ushort GenerateHeader(org.in2bits.MyXls.XlsDocument xlsDoc, org.in2bits.MyXls.Worksheet workSheet, DateTime PeriodFrom, DateTime PeriodTo, ushort RowPos)
//    private ushort GenerateHeader(NPOI.HSSF.UserModel.HSSFSheet workSheet, DateTime PeriodFrom, DateTime PeriodTo, ushort RowPos, NPOI.HSSF.UserModel.HSSFCellStyle ReportHeaderStyle, NPOI.HSSF.UserModel.HSSFCellStyle ReportSubHeaderStyle)
//    {

//        //RowPos++;
//        //workSheet.Cells.Add((int)RowPos, 1, "Payment Allocation Report");
//        //workSheet.Rows[RowPos].GetCell(1).Font.Bold = true;
//        NPOI.HSSF.UserModel.HSSFCell cell = (NPOI.HSSF.UserModel.HSSFCell)workSheet.CreateRow((int)RowPos).CreateCell(0);
//        cell.SetCellValue("Payment Allocation Report");
//        cell.CellStyle = ReportHeaderStyle;

//        RowPos++;
//        //workSheet.Cells.Add((int)RowPos, 1, "From");
//        //workSheet.Rows[RowPos].GetCell(1).Font.Bold = true;
//        //workSheet.Cells.Add((int)RowPos, 2, PeriodFrom.ToString("yyyy-MM-dd"));
//        //workSheet.Rows[RowPos].GetCell(2).Font.Bold = true;
//        //workSheet.Cells.Add((int)RowPos, 3, "To");
//        //workSheet.Rows[RowPos].GetCell(3).Font.Bold = true;
//        //workSheet.Cells.Add((int)RowPos, 4, PeriodTo.ToString("yyyy-MM-dd"));
//        //workSheet.Rows[RowPos].GetCell(4).Font.Bold = true;
//        NPOI.HSSF.UserModel.HSSFRow row = (NPOI.HSSF.UserModel.HSSFRow)workSheet.CreateRow((int)RowPos);
//        cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(0);
//        cell.SetCellValue("From");
//        cell.CellStyle = ReportSubHeaderStyle;
//        cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(1);
//        cell.SetCellValue(PeriodFrom.ToString("yyyy-MM-dd"));
//        cell.CellStyle = ReportSubHeaderStyle;
//        cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(2);
//        cell.SetCellValue("To");
//        cell.CellStyle = ReportSubHeaderStyle;
//        cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(3);
//        cell.SetCellValue(PeriodTo.ToString("yyyy-MM-dd"));
//        cell.CellStyle = ReportSubHeaderStyle;

//        return RowPos;

//    }
////    public ushort GenerateHierarchyTotal(org.in2bits.MyXls.XlsDocument xlsDoc, org.in2bits.MyXls.Worksheet workSheet, DataTable paymentTable, string currentHierarchyGroup, Hashtable paymentHashTable, double netPayment, double employerContribution, ushort rowPos)
//    public ushort GenerateHierarchyTotal(NPOI.HSSF.UserModel.HSSFSheet workSheet, DataTable paymentTable, string currentHierarchyGroup, Hashtable paymentHashTable, double netPayment, double employerContribution, ushort rowPos, NPOI.HSSF.UserModel.HSSFCellStyle style)
//    {
//        rowPos++;
//        ushort colPos = 0;
//        //workSheet.Cells.Add(rowPos, colPos, currentHierarchyGroup);

//        NPOI.HSSF.UserModel.HSSFRow row = (NPOI.HSSF.UserModel.HSSFRow)workSheet.CreateRow(rowPos);
//        NPOI.HSSF.UserModel.HSSFCell cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(colPos);
//        cell.SetCellValue(currentHierarchyGroup);
//        cell.CellStyle = style;

//        foreach (DataColumn column in paymentTable.Columns)
//        {
//            if (column.ColumnName.Equals("EmpPayrollID"))
//                continue;
//            colPos++;
//            //workSheet.Cells.Add(rowPos, colPos, Math.Round((double)paymentHashTable[column.ColumnName], 2));
//            //workSheet.Rows[rowPos].CellAtCol(colPos).Format = "#,##0.00;(#,##0.00)";
//            cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(colPos);
//            cell.SetCellValue(Math.Round((double)paymentHashTable[column.ColumnName], 2));
//            cell.CellStyle = style;

//        }
//        colPos++;
//        //workSheet.Cells.Add(rowPos, colPos, Math.Round(netPayment, 2));
//        //workSheet.Rows[rowPos].CellAtCol(colPos).Format = "#,##0.00;(#,##0.00)";
//        cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(colPos);
//        cell.CellFormula = "SUM(" + ToCellString(rowPos, 1) + ":" + ToCellString(rowPos, colPos - 1) + ")";
//        cell.CellStyle = style;

//        colPos++;
//        //workSheet.Cells.Add(rowPos, colPos, Math.Round(employerContribution, 2));
//        //workSheet.Rows[rowPos].CellAtCol(colPos).Format = "#,##0.00;(#,##0.00)";
//        cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(colPos);
//        cell.SetCellValue(Math.Round(employerContribution, 2));
//        cell.CellStyle = style;

//        return rowPos;
//    }
//    public ushort GenerateCompanyTotal(NPOI.HSSF.UserModel.HSSFSheet workSheet, DataTable paymentTable, int RecordCount, double companyEmployerTotal, ushort rowPos, NPOI.HSSF.UserModel.HSSFCellStyle style)
//    {
//        //org.in2bits.MyXls.XF xf = xlsDoc.NewXF();
//        //xf.TopLineStyle = (ushort)1;

//        rowPos++;
//        ushort colPos = 0;
//        //workSheet.Cells.Add(rowPos, colPos, "Total", xf);
//        NPOI.HSSF.UserModel.HSSFRow row = (NPOI.HSSF.UserModel.HSSFRow)workSheet.CreateRow(rowPos);
//        NPOI.HSSF.UserModel.HSSFCell cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(colPos);
//        cell.SetCellValue("Total");
//        cell.CellStyle = style;


//        foreach (DataColumn column in paymentTable.Columns)
//        {
//            if (column.ColumnName.Equals("EmpPayrollID"))
//                continue;
//            colPos++;
//            //workSheet.Cells.Add(rowPos, colPos, Math.Round((double)companyTotalHashTable[column.ColumnName], 2), xf);
//            //workSheet.Rows[rowPos].CellAtCol(colPos).Format = "#,##0.00;(#,##0.00)";
//            cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(colPos);
//            cell.CellFormula = "SUM(" + ToCellString(rowPos - RecordCount, colPos) + ":" + ToCellString(rowPos - 1, colPos) + ")";
//            cell.CellStyle = style;
//        }
//        colPos++;
//        //workSheet.Cells.Add(rowPos, colPos, Math.Round(companyNetPayment, 2), xf);
//        //workSheet.Rows[rowPos].CellAtCol(colPos).Format = "#,##0.00;(#,##0.00)";
//        cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(colPos);
//        cell.CellFormula = "SUM(" + ToCellString(rowPos - RecordCount, colPos) + ":" + ToCellString(rowPos - 1, colPos) + ")";
//        cell.CellStyle = style;

//        colPos++;
//        //workSheet.Cells.Add(rowPos, colPos, Math.Round(companyEmployerTotal, 2), xf);
//        //workSheet.Rows[rowPos].CellAtCol(colPos).Format = "#,##0.00;(#,##0.00)";
//        cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(colPos);
//        cell.CellFormula = "SUM(" + ToCellString(rowPos - RecordCount, colPos) + ":" + ToCellString(rowPos - 1, colPos) + ")";
//        cell.CellStyle = style;

//        return rowPos;
//    }

//    public ushort GenerateTotal(NPOI.HSSF.UserModel.HSSFSheet workSheet, DataTable paymentTable, ArrayList CompanyTotalRowList, ushort rowPos, NPOI.HSSF.UserModel.HSSFCellStyle style)
//    {
//        //org.in2bits.MyXls.XF xf = xlsDoc.NewXF();
//        //xf.TopLineStyle = (ushort)1;

//        rowPos+=2;
//        ushort colPos = 0;
//        //workSheet.Cells.Add(rowPos, colPos, "Total", xf);
//        NPOI.HSSF.UserModel.HSSFRow row = (NPOI.HSSF.UserModel.HSSFRow)workSheet.CreateRow(rowPos);
//        NPOI.HSSF.UserModel.HSSFCell cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(colPos);
//        cell.SetCellValue("Grand Total");
//        cell.CellStyle = style;


//        foreach (DataColumn column in paymentTable.Columns)
//        {
//            if (column.ColumnName.Equals("EmpPayrollID"))
//                continue;
//            colPos++;

//            string cellFormula = string.Empty;
//            foreach (ushort rowNum in CompanyTotalRowList)
//            {
//                if (string.IsNullOrEmpty(cellFormula))
//                    cellFormula = ToCellString(rowNum, colPos);
//                else
//                    cellFormula += "," + ToCellString(rowNum, colPos);
//            }

//            //workSheet.Cells.Add(rowPos, colPos, Math.Round((double)companyTotalHashTable[column.ColumnName], 2), xf);
//            //workSheet.Rows[rowPos].CellAtCol(colPos).Format = "#,##0.00;(#,##0.00)";
//            cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(colPos);
//            cell.CellFormula = "SUM(" + cellFormula + ")";
//            cell.CellStyle = style;
//        }
//        colPos++;
//        string cellNetPaymentFormula = string.Empty;
//        foreach (ushort rowNum in CompanyTotalRowList)
//        {
//            if (string.IsNullOrEmpty(cellNetPaymentFormula))
//                cellNetPaymentFormula = ToCellString(rowNum, colPos);
//            else
//                cellNetPaymentFormula += "," + ToCellString(rowNum, colPos);
//        }

//        cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(colPos);
//        cell.CellFormula = "SUM(" + cellNetPaymentFormula + ")";
//        cell.CellStyle = style;
//        cell.CellStyle = style;

//        colPos++;

//        string cellERMPFFormula = string.Empty;
//        foreach (ushort rowNum in CompanyTotalRowList)
//        {
//            if (string.IsNullOrEmpty(cellERMPFFormula))
//                cellERMPFFormula = ToCellString(rowNum, colPos);
//            else
//                cellERMPFFormula += "," + ToCellString(rowNum, colPos);
//        }

//        cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(colPos);
//        cell.CellFormula = "SUM(" + cellERMPFFormula + ")";
//        cell.CellStyle = style;
//        cell.CellStyle = style;

//        return rowPos;
//    }
//    private string ToCellString(int row, int col)
//    {
//        string cellString = "";

//        int tmpCol=col;

//        while (tmpCol>=26)
//        {
//            byte value = (byte)(tmpCol / 26);
//            cellString += Convert.ToChar(value + 64);
//            tmpCol = tmpCol % 26;
//        }
//        cellString += Convert.ToChar((byte)tmpCol + 65);
//        return cellString + (row + 1);
//    }
}
