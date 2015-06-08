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
using HROne.Lib.Entities;
using HROne.Payroll;
using HROne.Lib;

public partial class Report_Payroll_PaymentList_WaiJi : HROneWebPage
{
    private const string FUNCTION_CODE = "CUSTOM004";
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
        sbinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);
        sbinding.initValues("EmpStatus", null, EEmpPersonalInfo.VLEmpStatus, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));

        info = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            view = loadData(info, EEmpPayroll.db, Repeater);
        }
    }

    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = sbinding.createFilter();

        string select = "e.* ";
        string from = "from [" + EEmpPersonalInfo.db.dbclass.tableName + "] e ";

        DBFilter empPayrollFilter = new DBFilter();
        empPayrollFilter.add(Payroll_PeriodSelectionList1.GetEmpPayrollDBTerm());

        filter.add(new IN("e.EmpID", "SELECT DISTINCT ep.EmpID FROM " + EEmpPayroll.db.dbclass.tableName + " ep", empPayrollFilter));

        DBFilter empInfoFilter = EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime().Date, AppUtils.ServerDateTime().Date );
        empInfoFilter.add(new MatchField("e.EmpID", "ee.EmpID"));
        filter.add(new Exists(EEmpPersonalInfo.db.dbclass.tableName + " ee", empInfoFilter));

        DataTable table = filter.loadData(dbConn, null, select, from);
        table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);

        if (table.Rows.Count != 0)
            btnGenerate.Visible = true;
        else
            btnGenerate.Visible = false;

        view = new DataView(table);

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
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ArrayList list = WebUtils.SelectedRepeaterItemToBaseObjectList(EEmpPersonalInfo.db, Repeater, "ItemSelect");

        if (list.Count > 0)
        {
            const string PAYMENTCODE_PREFIX = "[Payment] ";
            string exportFileName = System.IO.Path.GetTempFileName();
            System.IO.File.Delete(exportFileName);
            exportFileName += ".xls";

            HROne.Export.ExcelExport export = new HROne.Export.ExcelExport(exportFileName);
            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable("Payroll$");
            dataSet.Tables.Add(dataTable);
            dataTable.Columns.Add("Company", typeof(string));

            DBFilter hierarchyLevelFilter = new DBFilter();
            Hashtable hierarchyLevelHashTable = new Hashtable();
            hierarchyLevelFilter.add("HLevelSeqNo", true);
            ArrayList hierarchyLevelList = EHierarchyLevel.db.select(dbConn, hierarchyLevelFilter);
            foreach (EHierarchyLevel hlevel in hierarchyLevelList)
            {
                dataTable.Columns.Add(hlevel.HLevelDesc, typeof(string));
                hierarchyLevelHashTable.Add(hlevel.HLevelID, hlevel);
            }
            dataTable.Columns.Add("Payroll Group", typeof(string));
            dataTable.Columns.Add("Position", typeof(string));
            dataTable.Columns.Add("EmpNo", typeof(string));
            dataTable.Columns.Add("English Name", typeof(string));
            dataTable.Columns.Add("Chinese Name", typeof(string));
            dataTable.Columns.Add("HKID", typeof(string));
            dataTable.Columns.Add("From", typeof(DateTime));
            dataTable.Columns.Add("To", typeof(DateTime));
            int firstSummaryColumnPos = dataTable.Columns.Count;
            dataTable.Columns.Add("Net Payment", typeof(double));
            int firstDetailColumnPos = dataTable.Columns.Count;

            foreach (EEmpPersonalInfo empInfo in list)
            {
                EEmpPersonalInfo.db.select(dbConn, empInfo);

                DBFilter empPayrollFilterForPayrollPeriod = new DBFilter();
                empPayrollFilterForPayrollPeriod.add(new Match("ep.EmpID", empInfo.EmpID));
                empPayrollFilterForPayrollPeriod.add(Payroll_PeriodSelectionList1.GetEmpPayrollDBTerm());

                DBFilter payPeriodFilter = new DBFilter();
                payPeriodFilter.add(new IN("PayPeriodID", "SELECT PayPeriodID from " + EEmpPayroll.db.dbclass.tableName + " ep", empPayrollFilterForPayrollPeriod));
                ArrayList payPeriodList = EPayrollPeriod.db.select(dbConn, payPeriodFilter);

                foreach (EPayrollPeriod payPeriod in payPeriodList)
                {
                    if (EPayrollPeriod.db.select(dbConn, payPeriod))
                    {
                        EPayrollGroup payrollGroup = new EPayrollGroup();
                        payrollGroup.PayGroupID = payPeriod.PayGroupID;
                        EPayrollGroup.db.select(dbConn, payrollGroup);

                        DataRow row = dataTable.NewRow();
                        row["EmpNo"] = empInfo.EmpNo;
                        row["English Name"] = empInfo.EmpEngFullName;
                        row["Chinese Name"] = empInfo.EmpChiFullName;
                        row["HKID"] = empInfo.EmpHKID;
                        row["From"] = payPeriod.PayPeriodFr;
                        row["To"] = payPeriod.PayPeriodTo;
                        row["Payroll Group"] = payrollGroup.PayGroupDesc;
                        DBFilter empPosFilter = new DBFilter();

                        EEmpPositionInfo empPos = AppUtils.GetLastPositionInfo(dbConn, payPeriod.PayPeriodTo, empInfo.EmpID);
                        if (empPos != null)
                        {
                            ECompany company = new ECompany();
                            company.CompanyID = empPos.CompanyID;
                            if (ECompany.db.select(dbConn, company))
                            {
                                row["Company"] = company.CompanyCode;
                            }

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
                                    {
                                        row[hierarchyLevel.HLevelDesc] = hierarchyElement.HElementDesc;
                                    }
                                }
                            }
                            EPosition position = new EPosition();
                            position.PositionID = empPos.PositionID;
                            if (EPosition.db.select(dbConn, position))
                            {
                                row["Position"] = position.PositionDesc;
                            }
                        }

                        double netAmount = 0;

                        DBFilter empPayrollFilterForPaymentRecord = new DBFilter(empPayrollFilterForPayrollPeriod);
                        empPayrollFilterForPaymentRecord.add(new Match("PayPeriodID", payPeriod.PayPeriodID));
                        DBFilter paymentRecordFilter = new DBFilter();
                        paymentRecordFilter.add(new IN("EmpPayrollID", "Select EmpPayrollID from " + EEmpPayroll.db.dbclass.tableName + " ep ", empPayrollFilterForPaymentRecord));
                        paymentRecordFilter.add(new Match("PayRecStatus", "A"));
                        ArrayList paymentRecords = EPaymentRecord.db.select(dbConn, paymentRecordFilter);

                        foreach (EPaymentRecord paymentRecord in paymentRecords)
                        {
                            EPaymentCode payCode = new EPaymentCode();
                            payCode.PaymentCodeID = paymentRecord.PaymentCodeID;
                            EPaymentCode.db.select(dbConn, payCode);
                            //  Always Use Payment Code Description for grouping payment code with same description
                            string fieldName = PAYMENTCODE_PREFIX + payCode.PaymentCodeDesc;
                            if (dataTable.Columns[fieldName] == null)
                            {
                                dataTable.Columns.Add(new DataColumn(fieldName, typeof(double)));
                            }
                            if (row[fieldName] == null || row[fieldName] == DBNull.Value)
                            {
                                row[fieldName] = 0;
                            }
                            row[fieldName] = (double)row[fieldName] + paymentRecord.PayRecActAmount;

                            netAmount += paymentRecord.PayRecActAmount;
                        }

                        row["Net Payment"] = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(netAmount, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), ExchangeCurrency.DefaultCurrencyDecimalPlaces());

                        dataTable.Rows.Add(row);
                    }
                }
            }

            DBFilter paymentCodeFilter = new DBFilter();
            paymentCodeFilter.add("PaymentCodeDisplaySeqNo", false);
            paymentCodeFilter.add("PaymentCode", false);
            ArrayList paymentCodeList = EPaymentCode.db.select(dbConn, paymentCodeFilter);
            foreach (EPaymentCode paymentCode in paymentCodeList)
            {
                if (dataTable.Columns.Contains(PAYMENTCODE_PREFIX + paymentCode.PaymentCodeDesc))
                {
                    DataColumn paymentColumn = dataTable.Columns[PAYMENTCODE_PREFIX + paymentCode.PaymentCodeDesc];
                    paymentColumn.SetOrdinal(firstDetailColumnPos);
                    if (!dataTable.Columns.Contains(paymentCode.PaymentCodeDesc))
                    {
                        paymentColumn.ColumnName = paymentCode.PaymentCodeDesc;
                    }
                    else
                    {
                        System.Diagnostics.Debug.Write("System reserved payment column is used");
                    }
                }
            }
            for (int i = 0; i < firstDetailColumnPos; i++)
            {
                dataTable.Columns[i].ColumnName = HROne.Common.WebUtility.GetLocalizedString(dataTable.Columns[i].ColumnName);
            }

            for (int i = firstSummaryColumnPos; i < firstDetailColumnPos; i++)
            {
                dataTable.Columns[firstSummaryColumnPos].SetOrdinal(dataTable.Columns.Count - 1);
            }

            export.Update(dataSet);
            WebUtils.TransmitFile(Response, exportFileName, "PaymentListWaiJi_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true);
            return;
        }
        else
        {
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
    protected void Payroll_PeriodSelectionList1_PayrollBatchChecked(object sender, EventArgs e)
    {
        info.page = 0;
        view = loadData(info, EEmpPayroll.db, Repeater);
    }
}
