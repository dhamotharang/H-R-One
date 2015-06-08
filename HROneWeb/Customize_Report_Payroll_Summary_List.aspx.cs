using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Collections.Generic;
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
using System.Text;

public partial class Customize_Report_Payroll_Summary_List : HROneWebPage
{
    private const string FUNCTION_CODE = "KHL_CPSL"; 
    public Binding binding;

    protected SearchBinding sbinding;

    protected ListInfo info;
    protected DataView view;
    protected int DefaultMonthPeriod = 1;

    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(EEmpPersonalInfo.db, row, cb);
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


    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter m_payrolPeriodFilter = new DBFilter();
        DBFilter m_empPayrollFilter = new DBFilter();

        OR m_orStatus = new OR();
        OR m_orEffDateTo = new OR();

        DateTime m_payPeriodFr;
        DateTime m_payPeriodTo;
        int m_month;
        int m_year;

        if (cboPayrollStatus.SelectedValue == "C")
        {
            m_month = int.Parse(PayrollMonth.SelectedValue);
            if (int.TryParse(PayrollYear.Text, out m_year) && m_year > 1980 && m_year < 2199)
            {
                m_payPeriodFr = new DateTime(m_year, m_month, 1);
                m_payPeriodTo = m_payPeriodFr.AddMonths(1).AddDays(-1);

                m_payrolPeriodFilter.add(new Match("PayPeriodFr", "<=", m_payPeriodTo));

                m_orEffDateTo.add(new Match("PayPeriodTo", ">=", m_payPeriodFr));
                m_orEffDateTo.add(new NullTerm("PayPeriodTo"));
                m_payrolPeriodFilter.add(m_orEffDateTo);
            }
            m_empPayrollFilter.add(new Match("EmpPayStatus", "C"));
            m_empPayrollFilter.add(new IN("ep.PayPeriodID", "SELECT PayPeriodID FROM PayrollPeriod", m_payrolPeriodFilter));
        }
        else
        {
            m_empPayrollFilter.add(new Match("EmpPayStatus", "T"));
        }

        m_empPayrollFilter.add(new MatchField("e.EmpID", "ep.EmpID"));

        //DBFilter m_empPersonalInfoFilter = new DBFilter();
        //m_empPersonalInfoFilter.add(new IN("EmpID", "SELECT EmpID FROM EmpPayroll", m_empPayrollFilter));

        
        DBFilter filter = new DBFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = "e.* ";
        string from = "from " + EEmpPersonalInfo.db.dbclass.tableName + " e ";

        DBFilter empInfoFilter = EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime().Date, AppUtils.ServerDateTime().Date);
        empInfoFilter.add(new MatchField("e.EmpID", "ee.EmpID"));
        filter.add(new Exists(EEmpPersonalInfo.db.dbclass.tableName + " ee", empInfoFilter));
        filter.add(new Exists(EEmpPayroll.db.dbclass.tableName + " ep", m_empPayrollFilter));

        DataTable table = filter.loadData(dbConn, null, select, from);
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
        table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);


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

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
        //    return;

        info = ListFooter.ListInfo;

        sbinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);
        sbinding.init(DecryptedRequest, null);

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //init_company_dropdown();
            init_grouping_on();
            PayrollMonth.SelectedValue = AppUtils.ServerDateTime().Month.ToString();
            PayrollYear.Text = AppUtils.ServerDateTime().Year.ToString("0000");


            cboPaymentMethod.Items.Add(new ListItem("All", ""));
            cboPaymentMethod.Items.Add(new ListItem("Autopay", "A"));
            cboPaymentMethod.Items.Add(new ListItem("Cash", "C"));
            cboPaymentMethod.Items.Add(new ListItem("Cheque", "Q"));
            cboPaymentMethod.Items.Add(new ListItem("Other", "O")); 
        
        }
    }

    protected void init_grouping_on()
    {
        DBFilter hierarchyLevelFilter = new DBFilter();
        hierarchyLevelFilter.add(new Match("HLevelSeqNo", "<=", 3));
        hierarchyLevelFilter.add("HLevelSeqNo", true);

        ArrayList hierarchyLevelList = EHierarchyLevel.db.select(dbConn, hierarchyLevelFilter);
        foreach (EHierarchyLevel hlevel in hierarchyLevelList)
        {
            cboSummaryOnHierarchyLevel.Items.Add(new ListItem(hlevel.HLevelDesc, hlevel.HLevelSeqNo.ToString("0")));
        }
    }

    //protected void init_company_dropdown()
    //{
    //    int m_userID = WebUtils.GetCurUser(Session).UserID;

    //    cboCompany.Items.Add(new ListItem("Not Selected", "0"));

    //    if (m_userID > 0)
    //    {
    //        DBFilter m_UserCompanyFilter = new DBFilter();
    //        DBFilter m_filter = new DBFilter();

    //        DataTable m_companyList = new DataTable();
    //        m_companyList.Columns.Add("CompanyCode", typeof(string));
    //        m_companyList.Columns.Add("CompanyDesc", typeof(string));
    //        m_companyList.Columns.Add("CompanyID", typeof(int));

    //        m_UserCompanyFilter.add(new Match("userID", m_userID));

    //        m_filter.add(new IN("CompanyID", "SELECT CompanyID FROM UserCompany", m_UserCompanyFilter));
    //        m_filter.add("CompanyCode", true);

    //        foreach (ECompany m_company in ECompany.db.select(dbConn, m_filter))
    //        {
    //            DataRow m_row = m_companyList.NewRow();
    //            m_row["CompanyCode"] = m_company.CompanyCode;
    //            m_row["CompanyDesc"] = m_company.CompanyName;
    //            m_row["CompanyID"] = m_company.CompanyID;

    //            m_companyList.Rows.Add(m_row);
    //        }

    //        DataRow[] m_rows = m_companyList.Select("", "CompanyCode ASC");

    //        foreach (DataRow m_row in m_rows)
    //        {
    //            cboCompany.Items.Add(new ListItem(m_row["CompanyCode"].ToString(), m_row["CompanyID"].ToString()));
    //        }
    //    }
    //}
    protected void cboPayrollStatus_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        PayrollCycleRow.Visible = (cboPayrollStatus.SelectedValue == "C");
    }

    protected void Search_Click(object sender, EventArgs e)
    {
        view = loadData(info, EEmpPayroll.db, Repeater);
    }

    protected void Reset_Click(object sender, EventArgs e)
    {
        EmployeeSearchControl1.Reset();
        cboPayrollStatus.SelectedIndex = 0;
        PayrollCycleRow.Visible = false;
        cboSummaryOnHierarchyLevel.SelectedIndex = 0;
        // Start 0000185, KuangWei, 2015-04-21
        sbinding.clear();
        view = loadData(info, EEmpPayroll.db, Repeater);
        // End 0000185, KuangWei, 2015-04-21
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(EEmpPersonalInfo.db, Page.Master);
        errors.clear();

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
            //int m_companyID;
            string m_payrollStatus;
            DateTime m_payPeriodFr;
            DateTime m_payPeriodTo;
            int m_month;
            int m_year;

            //if (int.TryParse(cboCompany.SelectedValue, out m_companyID) && m_companyID > 0)
            //{
                m_payrollStatus = cboPayrollStatus.SelectedValue;

                m_month = int.Parse(PayrollMonth.SelectedValue);
                if (int.TryParse(PayrollYear.Text, out m_year) && m_year > 1980 && m_year < 2199)
                {
                    m_payPeriodFr = new DateTime(m_year, m_month, 1);
                    m_payPeriodTo = m_payPeriodFr.AddMonths(1).AddDays(-1);
                    //generate_report(m_companyID, m_payrollStatus, m_payPeriodFr, m_payPeriodTo, WebUtils.GetCurUser(Session).UserID, errors);
                    generate_report(empList, m_payrollStatus, m_payPeriodFr, m_payPeriodTo, cboPaymentMethod.SelectedValue, WebUtils.GetCurUser(Session).UserID, errors);
                }
                else
                    errors.addError("Invalid Payroll Year");
        }
        else
        {
            errors.addError("No employees selected");
        }
    }

    protected void generate_report(ArrayList EmpList, string PayrollStatus, DateTime PayPeriodFr, DateTime PayPeriodTo, string PaymentMethod, int UserID, PageErrors errors)
    {
        Customize_Report_Payroll_Summary_List_Process reportProcess = new Customize_Report_Payroll_Summary_List_Process(dbConn, ci, EmpList, PayrollStatus, PayPeriodFr, PayPeriodTo, PaymentMethod, UserID, int.Parse(cboSummaryOnHierarchyLevel.SelectedValue));

        if (Response.IsClientConnected)
        {
            HROneConfig config = HROneConfig.GetCurrentHROneConfig();
            if (config.GenerateReportAsInbox)
            {
                if (EInboxAttachment.GetTotalSize(dbConn, 0) < WebUtils.productLicense(Session).MaxInboxSizeMB * 1000 * 1000)
                {
                    HROne.TaskService.GenericExcelReportTaskFactory reportTask = new HROne.TaskService.GenericExcelReportTaskFactory(dbConn, user, lblReportHeader.Text, reportProcess, "PaymentList");
                    AppUtils.reportTaskQueueService.AddTask(reportTask);
                    errors.addError(HROne.Translation.PageMessage.REPORT_GENERATING_TO_INBOX);
                }
                else
                    errors.addError(HROne.Translation.PageMessage.INBOX_SIZE_EXCEEDED);
            }
            else
            {
                System.IO.FileInfo excelFile = reportProcess.GenerateExcelReport();
                WebUtils.TransmitFile(Response, excelFile.FullName, "PaymentList_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true);
            }
        }
    }


    protected class Customize_Report_Payroll_Summary_List_Process : HROne.Common.GenericExcelReportProcess
    {
        const string PAYMENTCODE_PREFIX = "_PAYCODE_";
        const string HIERARCHY_PREFIX = "_HIERARCHY_";

        int TOTAL_REPORT_COLUMNS = 0;

        // special columns
        //int m_remarksCol = 0;
        //int m_blankCol = 0;


        //int m_CompanyID;
        string m_PayrollStatus;
        DateTime m_PayPeriodFr;
        DateTime m_PayPeriodTo;
        int m_UserID;
        int m_firstSummaryColumnPos = 0;
        bool m_ShowCompanyTotal = true;
        bool m_ShowHierarchy1Total = false;
        bool m_ShowHierarchy2Total = true;
        bool m_ShowHierarchy3Total = false;
        int m_HierarchyLevelGrouping;
        string m_PaymentMethod;

        ArrayList EmpList;
        ArrayList PayPeriodList;
        //ArrayList PayBatchList;
        string exportFileName;
        //public Customize_Report_Payroll_Summary_List_Process(DatabaseConnection dbConn, System.Globalization.CultureInfo reportCultureInfo,
        //                                                     int CompanyID,
        //                                                     string PayrollStatus,
        //                                                     DateTime PayPeriodFr,
        //                                                     DateTime PayPeriodTo,
        //                                                     int UserID,
        //                                                     int HierarchyLevelGrouping)
        //    : base(dbConn, reportCultureInfo)
        //{
        //    m_CompanyID = CompanyID;
        //    m_PayrollStatus = PayrollStatus;
        //    m_PayPeriodFr = PayPeriodFr;
        //    m_PayPeriodTo = PayPeriodTo;
        //    m_UserID = UserID;
        //    m_HierarchyLevelGrouping = HierarchyLevelGrouping;
        //}

        public Customize_Report_Payroll_Summary_List_Process(DatabaseConnection dbConn, System.Globalization.CultureInfo reportCultureInfo,
                                                             ArrayList EmpList,
                                                             string PayrollStatus,
                                                             DateTime PayPeriodFr,
                                                             DateTime PayPeriodTo,
                                                             string PaymentMethod,
                                                             int UserID,
                                                             int HierarchyLevelGrouping)
            : base(dbConn, reportCultureInfo)
        {
            this.EmpList = new ArrayList();
            this.EmpList.AddRange(EmpList);
            //m_CompanyID = CompanyID;
            m_PayrollStatus = PayrollStatus;
            m_PayPeriodFr = PayPeriodFr;
            m_PayPeriodTo = PayPeriodTo;
            m_UserID = UserID;
            m_HierarchyLevelGrouping = HierarchyLevelGrouping;
            m_PaymentMethod = PaymentMethod;
        }

        //private ArrayList PrepareEmpList(int pUserID, DateTime pPayPeriodFr, DateTime pPayPeriodTo, string pPayrollStatus)
        //{
        //    // prepare Employee List
        //    DBFilter m_empPersonalInfoFilter = new DBFilter();

        //    // Start 0000082, Ricky So, 2014/12/18
        //    //DBFilter m_empPosInfoFilter = new DBFilter();
        //    //DBFilter m_userCompanyFilter = new DBFilter();
        //    //DBFilter m_userRankFilter = new DBFilter();

        //    //m_userCompanyFilter.add(new Match("UserID", pUserID));
        //    //m_userRankFilter.add(new Match("UserID", pUserID));

        //    //m_empPosInfoFilter.add(new IN("CompanyID", "SELECT CompanyID FROM UserCompany", m_userCompanyFilter));
        //    //m_empPosInfoFilter.add(new IN("RankID", "SELECT RankID FROM UserRank", m_userRankFilter));
        //    //m_empPersonalInfoFilter.add(new IN("EmpID", "SELECT EmpID FROM EmpPositionInfo", m_empPosInfoFilter));
        //    m_empPersonalInfoFilter.add(AppUtils.AddRankDBTerm(pUserID, "EmpID", true));

        //    // End 0000082, Ricky So, 2014/12/18

        //    DBFilter m_empPayrollFilter = new DBFilter();
        //    m_empPayrollFilter.add(new Match("EmpPayStatus", pPayrollStatus));
        //    if (pPayrollStatus == "C")
        //    {
        //        DBFilter m_payPeriodFilter = new DBFilter();
        //        m_payPeriodFilter.add(new Match("PayPeriodFr", "<=", pPayPeriodTo));
        //        m_payPeriodFilter.add(new Match("PayPeriodTo", ">=", pPayPeriodFr));

        //        m_empPayrollFilter.add(new IN("PayPeriodID", "SELECT PayPeriodID FROM PayrollPeriod", m_payPeriodFilter)); 
        //    }
        //    m_empPersonalInfoFilter.add(new IN("EmpID", "SELECT EmpID FROM EmpPayroll", m_empPayrollFilter));           

        //    return EEmpPersonalInfo.db.select(dbConn, m_empPersonalInfoFilter);
        //}

        private ArrayList PreparePayPeriodList(DateTime pPayPeriodFr, DateTime pPayPeriodTo, string pPayrollStatus)
        {
            DBFilter m_PayPeriodFilter = new DBFilter();
            DBFilter m_PayGroupFilter = new DBFilter();
            //OR m_payGroupOR = new OR();

            DBFilter m_empPayrollFilter = new DBFilter();
            m_empPayrollFilter.add(new Match("EmpPayStatus", pPayrollStatus));

            m_PayPeriodFilter.add(new IN("PayPeriodID", "SELECT PayPeriodID FROM EmpPayroll", m_empPayrollFilter));

            if (pPayrollStatus == "C")
            {
                m_PayPeriodFilter.add(new Match("PayPeriodFr", "<=", pPayPeriodTo));
                m_PayPeriodFilter.add(new Match("PayPeriodTo", ">=", pPayPeriodFr));
            }

            return EPayrollPeriod.db.select(dbConn, m_PayPeriodFilter);
        }
        

        protected override System.Data.DataSet CreateDataSource()
        {

            System.Data.DataSet dataSet = new System.Data.DataSet(); //export.GetDataSet();

#region "Define Table"
            DataTable dataTable = new DataTable("Payroll$");
            dataSet.Tables.Add(dataTable);

            dataTable.Columns.Add("Company", typeof(string));
            // column name prefixed with FUNCTION_CODE will be shown onto the EXCEL Column header after mapping with translation
            dataTable.Columns.Add(FUNCTION_CODE + ".Seq", typeof(int));
            dataTable.Columns.Add(FUNCTION_CODE + ".EmpNo", typeof(string));
            dataTable.Columns.Add(FUNCTION_CODE + ".EnglishName", typeof(string));
            dataTable.Columns.Add(FUNCTION_CODE + ".Alias", typeof(string));

            DBFilter hierarchyLevelFilter = new DBFilter();
            hierarchyLevelFilter.add(new Match("HLevelSeqNo", "<=", 3));
            hierarchyLevelFilter.add("HLevelSeqNo", true);

            Hashtable hierarchyLevelHashTable = new Hashtable();
            ArrayList hierarchyLevelList = EHierarchyLevel.db.select(dbConn, hierarchyLevelFilter);
            foreach (EHierarchyLevel hlevel in hierarchyLevelList)
            {
                dataTable.Columns.Add(FUNCTION_CODE + "." + HIERARCHY_PREFIX + hlevel.HLevelDesc, typeof(string));
                hierarchyLevelHashTable.Add(hlevel.HLevelID, hlevel);
            }
            dataTable.Columns.Add(FUNCTION_CODE + ".Position", typeof(string));
            dataTable.Columns.Add(FUNCTION_CODE + ".DateJoin", typeof(string));
            dataTable.Columns.Add(FUNCTION_CODE + ".RPBasicSalary", typeof(string));
            dataTable.Columns.Add("PayrollGroup", typeof(string));
            dataTable.Columns.Add("ChineseName", typeof(string));
            dataTable.Columns.Add("HKID", typeof(string));
            dataTable.Columns.Add("PayPeriodFr", typeof(DateTime));
            dataTable.Columns.Add("PayPeriodTo", typeof(DateTime));
            int firstSummaryColumnPos = dataTable.Columns.Count;
            dataTable.Columns.Add(FUNCTION_CODE + ".NetPayment", typeof(double));
            dataTable.Columns.Add(FUNCTION_CODE + ".Remarks", typeof(string));
            dataTable.Columns.Add(FUNCTION_CODE + ".BLANK", typeof(string));
            dataTable.Columns.Add(FUNCTION_CODE + ".RelevantIncome", typeof(double));
            dataTable.Columns.Add(FUNCTION_CODE + ".NonRelevantIncome", typeof(double));
            dataTable.Columns.Add(FUNCTION_CODE + ".WagesPayableForMinWages", typeof(double));
            dataTable.Columns.Add(FUNCTION_CODE + ".TotalHoursWorked", typeof(double));
            dataTable.Columns.Add(FUNCTION_CODE + ".MinWagesRequired", typeof(double));
            dataTable.Columns.Add(FUNCTION_CODE + ".EmployerMandatoryContribution", typeof(double));
            //dataTable.Columns.Add("Employee Mandatory Contribution", typeof(double));
            dataTable.Columns.Add(FUNCTION_CODE + ".EmployerVoluntaryContribution", typeof(double));
            //dataTable.Columns.Add("Employee Voluntary Contribution", typeof(double));
            dataTable.Columns.Add(FUNCTION_CODE + ".EmployerPFundContribution", typeof(double));
            //dataTable.Columns.Add("Employee P-Fund Contribution", typeof(double));
            dataTable.Columns.Add(FUNCTION_CODE + ".TotalEmployerContribution", typeof(double));
            dataTable.Columns.Add(FUNCTION_CODE + ".TotalEmployeeContribution", typeof(double));
            dataTable.Columns.Add(FUNCTION_CODE + ".TotalTaxablePayment", typeof(double));
            dataTable.Columns.Add(FUNCTION_CODE + ".TotalNonTaxablePayment", typeof(double));

            int firstDetailColumnPos = dataTable.Columns.Count;

#endregion "Define Table"

            PayPeriodList = PreparePayPeriodList(m_PayPeriodFr, m_PayPeriodTo, m_PayrollStatus);
            //EmpList = PrepareEmpList(m_UserID, m_PayPeriodFr, m_PayPeriodTo, m_PayrollStatus);

            foreach (EEmpPersonalInfo empInfo in EmpList)
            {
                EEmpPersonalInfo.db.select(dbConn, empInfo);

                foreach (EPayrollPeriod payPeriod in PayPeriodList)
                {
                    DBFilter empPayrollFilter = new DBFilter();
                    empPayrollFilter.add(new Match("EmpID", empInfo.EmpID));
                    empPayrollFilter.add(new Match("PayPeriodID", payPeriod.PayPeriodID));

                    //  Check if the EmpPayroll record for that payroll period exists
                    if (EEmpPayroll.db.count(dbConn, empPayrollFilter) > 0 && EPayrollPeriod.db.select(dbConn, payPeriod))
                    {

                        EPayrollGroup payrollGroup = new EPayrollGroup();
                        payrollGroup.PayGroupID = payPeriod.PayGroupID;
                        EPayrollGroup.db.select(dbConn, payrollGroup);

                        DataRow row = dataTable.NewRow();
                        row[FUNCTION_CODE + ".EmpNo"] = empInfo.EmpNo;
                        row[FUNCTION_CODE + ".EnglishName"] = empInfo.EmpEngFullName;
                        row[FUNCTION_CODE + ".Alias"] = empInfo.EmpAlias;
                        row["ChineseName"] = empInfo.EmpChiFullName;
                        row["HKID"] = empInfo.EmpHKID;
                        row["PayPeriodFr"] = payPeriod.PayPeriodFr;
                        row["PayPeriodTo"] = payPeriod.PayPeriodTo;
                        row["PayrollGroup"] = payrollGroup.PayGroupDesc;
                        DBFilter empPosFilter = new DBFilter();

                        EEmpPositionInfo empPos = AppUtils.GetLastPositionInfo(dbConn, payPeriod.PayPeriodTo, empInfo.EmpID);
                        if (empPos != null)
                        {
                            ECompany company = new ECompany();
                            company.CompanyID = empPos.CompanyID;
                            if (ECompany.db.select(dbConn, company))
                                row["Company"] = company.CompanyCode;

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
                                        // Start 0000082, Ricky So, 2015-04-10
                                        row[FUNCTION_CODE + "." + HIERARCHY_PREFIX + hierarchyLevel.HLevelDesc] = hierarchyElement.HElementCode;
                                        //row[FUNCTION_CODE + "." + HIERARCHY_PREFIX + hierarchyLevel.HLevelDesc] = hierarchyElement.HElementDesc;
                                        // End 0000082, Ricky So, 2015-04-10
                                    }
                                }
                            }
                            EPosition position = new EPosition();
                            position.PositionID = empPos.PositionID;
                            if (EPosition.db.select(dbConn, position))
                                row[FUNCTION_CODE + ".Position"] = position.PositionDesc;
                        }
                        if (empInfo.EmpDateOfJoin.Ticks > 0)
                            row[FUNCTION_CODE + ".DateJoin"] = empInfo.EmpDateOfJoin.ToString("dd-MMM-yy");

                        // calculate basic salary from recurring payment

                        DBFilter m_rpFilter = new DBFilter();
                        m_rpFilter.add(new Match("EmpID", empInfo.EmpID));
                        m_rpFilter.add(new Match("EmpRPEffFr", "<=", this.m_PayPeriodTo));
                        OR m_orEffTo = new OR();
                        m_orEffTo.add(new Match("EmpRPEffTo", ">=", this.m_PayPeriodFr));
                        m_orEffTo.add(new NullTerm("EmpRPEffTo"));

                        m_rpFilter.add(m_orEffTo);
                        m_rpFilter.add("EmpRPEffFr", false);
                        m_rpFilter.add(AppUtils.GetPayemntCodeDBTermByPaymentType(dbConn, "PayCodeID", "BASICSAL"));

                        ArrayList m_rpList = EEmpRecurringPayment.db.select(dbConn, m_rpFilter);
                        if (m_rpList.Count > 0)
                        {
                            row[FUNCTION_CODE + ".RPBasicSalary"] = ((EEmpRecurringPayment)m_rpList[0]).EmpRPAmount;
                        }

                        double netAmount = 0, releventIncome = 0, nonRelevantIncome = 0, taxableAmount = 0, nonTaxableAmount = 0;
                        double mcER = 0, mcEE = 0;
                        double vcER = 0, vcEE = 0;
                        double pFundER = 0, pFundEE = 0;

                        IN inEmpPayroll = new IN("EmpPayrollID", "Select ep.EmpPayrollID from " + EEmpPayroll.db.dbclass.tableName + " ep", empPayrollFilter);

                        DBFilter empPaymentRecordFilter = new DBFilter();
                        empPaymentRecordFilter.add(inEmpPayroll);
                        if (m_PaymentMethod != "")
                            empPaymentRecordFilter.add(new Match("PayRecMethod", m_PaymentMethod));

                        // for mpf record and orso record only (not for payment records)
                        DBFilter empPayrollFilterForPaymentRecord = new DBFilter();
                        empPayrollFilterForPaymentRecord.add(inEmpPayroll);
//                        ArrayList paymentRecords = EPaymentRecord.db.select(dbConn, empPayrollFilterForPaymentRecord);

                        ArrayList paymentRecords = EPaymentRecord.db.select(dbConn, empPaymentRecordFilter);

                        int m_PayCodeCount = 0;
                        string m_payCode = "";


                        foreach (EPaymentRecord paymentRecord in paymentRecords)
                        {
                            EPaymentCode payCode = new EPaymentCode();
                            payCode.PaymentCodeID = paymentRecord.PaymentCodeID;
                            EPaymentCode.db.select(dbConn, payCode);
                            //  Always Use Payment Code Description for grouping payment code with same description
                            string fieldName = FUNCTION_CODE + "." + PAYMENTCODE_PREFIX + payCode.PaymentCodeDesc;
                            if (dataTable.Columns[fieldName] == null)
                            {
                                dataTable.Columns.Add(new DataColumn(fieldName, typeof(double)));

                                m_PayCodeCount++;
                                m_payCode = m_payCode + Environment.NewLine + fieldName;                            
                            }
                            if (row[fieldName] == null || row[fieldName] == DBNull.Value)
                                row[fieldName] = 0;
                            row[fieldName] = (double)row[fieldName] + paymentRecord.PayRecActAmount;

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
                        }

                        row[FUNCTION_CODE + ".NetPayment"] = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(netAmount, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), ExchangeCurrency.DefaultCurrencyDecimalPlaces());
                        row[FUNCTION_CODE + ".RelevantIncome"] = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(releventIncome, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), ExchangeCurrency.DefaultCurrencyDecimalPlaces());
                        row[FUNCTION_CODE + ".NonRelevantIncome"] = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(nonRelevantIncome, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), ExchangeCurrency.DefaultCurrencyDecimalPlaces());
                        row[FUNCTION_CODE + ".WagesPayableForMinWages"] = HROne.Payroll.PayrollProcess.GetTotalWagesWithoutRestDayPayment(dbConn, empInfo.EmpID, payPeriod.PayPeriodFr, payPeriod.PayPeriodTo, null);
                        row[FUNCTION_CODE + ".TotalHoursWorked"] = HROne.Payroll.PayrollProcess.GetTotalEmpPayrollWorkingHours(dbConn, empInfo.EmpID, payPeriod.PayPeriodID);
                        row[FUNCTION_CODE + ".MinWagesRequired"] = (double)row[FUNCTION_CODE + ".TotalHoursWorked"] * HROne.Payroll.PayrollProcess.GetMinimumWages(dbConn, empInfo.EmpID, payPeriod.PayPeriodTo);
                        row[FUNCTION_CODE + ".TotalTaxablePayment"] = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(taxableAmount, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), ExchangeCurrency.DefaultCurrencyDecimalPlaces());
                        row[FUNCTION_CODE + ".TotalNonTaxablePayment"] = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(nonTaxableAmount, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), ExchangeCurrency.DefaultCurrencyDecimalPlaces());

                        ArrayList mpfRecords = EMPFRecord.db.select(dbConn, empPayrollFilterForPaymentRecord);
                        foreach (EMPFRecord mpfRecord in mpfRecords)
                        {
                            vcER += mpfRecord.MPFRecActVCER;
                            mcER += mpfRecord.MPFRecActMCER;
                            vcEE += mpfRecord.MPFRecActVCEE;
                            mcEE += mpfRecord.MPFRecActMCEE;
                        }
                        ArrayList orsoRecords = EORSORecord.db.select(dbConn, empPayrollFilterForPaymentRecord);
                        foreach (EORSORecord orsoRecord in orsoRecords)
                        {
                            pFundER += orsoRecord.ORSORecActER;
                            pFundEE += orsoRecord.ORSORecActEE;
                        }
                        row[FUNCTION_CODE + ".EmployerMandatoryContribution"] = mcER;
                        //row["Employee Mandatory Contribution"] = mcEE;
                        row[FUNCTION_CODE + ".EmployerVoluntaryContribution"] = vcER;
                        //row["Employee Voluntary Contribution"] = vcEE;
                        row[FUNCTION_CODE + ".EmployerPFundContribution"] = pFundER;
                        //row["Employee P-Fund Contribution"] = pFundEE;

                        row[FUNCTION_CODE + ".TotalEmployerContribution"] = mcER + vcER + pFundER;
                        row[FUNCTION_CODE + ".TotalEmployeeContribution"] = mcEE + vcEE + pFundEE;

                        dataTable.Rows.Add(row);
                    }
                }
            }

            // get payment code in reverse order, and then insert into correct position in correct order
            DBFilter paymentCodeFilter = new DBFilter();
            paymentCodeFilter.add("PaymentCodeDisplaySeqNo", false);
            paymentCodeFilter.add("PaymentCode", false);
            ArrayList paymentCodeList = EPaymentCode.db.select(dbConn, paymentCodeFilter);
            foreach (EPaymentCode paymentCode in paymentCodeList)
            {
                DataColumn paymentColumn = dataTable.Columns[FUNCTION_CODE + "." + PAYMENTCODE_PREFIX + paymentCode.PaymentCodeDesc];

                if (paymentColumn != null)
                {
                    paymentColumn.SetOrdinal(firstSummaryColumnPos);
                }
            }
            return dataSet;
        }

        protected override void CreateWorkBookStyle(NPOI.HSSF.UserModel.HSSFWorkbook workBook)
        {
        }

        protected void WriteCellFormula(NPOI.HSSF.UserModel.HSSFRow pExcelRow, int pColIndex, string pFormula, NPOI.HSSF.UserModel.HSSFCellStyle pStyle)
        {
            NPOI.HSSF.UserModel.HSSFCell m_cell;
            m_cell = (NPOI.HSSF.UserModel.HSSFCell)pExcelRow.CreateCell(pColIndex);
            if (pStyle != null)
                m_cell.CellStyle = pStyle;
            m_cell.SetCellFormula(pFormula);
        }

        protected void WriteCellValue(NPOI.HSSF.UserModel.HSSFRow pExcelRow, int pColIndex, string pValue, NPOI.HSSF.UserModel.HSSFCellStyle pStyle)
        {
            NPOI.HSSF.UserModel.HSSFCell m_cell;
            m_cell = (NPOI.HSSF.UserModel.HSSFCell)pExcelRow.CreateCell(pColIndex);
            if (pStyle != null)
                m_cell.CellStyle = pStyle;
            m_cell.SetCellValue(pValue);
        }

        protected void WriteCellValue(NPOI.HSSF.UserModel.HSSFRow pExcelRow, int pColIndex, double pValue, NPOI.HSSF.UserModel.HSSFCellStyle pStyle)
        {
            NPOI.HSSF.UserModel.HSSFCell m_cell;
            m_cell = (NPOI.HSSF.UserModel.HSSFCell)pExcelRow.CreateCell(pColIndex);
            m_cell.CellStyle = pStyle;
            m_cell.SetCellValue(HROne.CommonLib.GenericRoundingFunctions.RoundingTo(pValue, 2, 2));
        }

        protected void SetColumnWidth(NPOI.HSSF.UserModel.HSSFSheet pWorksheet, int pHeaderRow)
        {
            pWorksheet.SetColumnWidth(0, 5 * 254); // Seq
            pWorksheet.SetColumnWidth(1, 15 * 254); // EmpNo
            pWorksheet.SetColumnWidth(2, 20 * 254); // English Name
            pWorksheet.SetColumnWidth(3, 15 * 254); // EmpNo
            pWorksheet.SetColumnWidth(4, 20 * 254); // Division
            pWorksheet.SetColumnWidth(5, 20 * 254); // Department
            pWorksheet.SetColumnWidth(6, 15 * 254); // Team
            pWorksheet.SetColumnWidth(7, 20 * 254); // Position

            for (int i = 8; i < TOTAL_REPORT_COLUMNS; i++)
            {

                NPOI.HSSF.UserModel.HSSFRow m_headerRow = (NPOI.HSSF.UserModel.HSSFRow)pWorksheet.GetRow(pHeaderRow);
                NPOI.HSSF.UserModel.HSSFCell m_headerCell = (NPOI.HSSF.UserModel.HSSFCell)m_headerRow.GetCell(i);

                if (m_headerCell.StringCellValue.Equals(FUNCTION_CODE + ".Remarks"))
                    pWorksheet.SetColumnWidth(i, 30 * 254);
                else if (m_headerCell.StringCellValue.Equals(FUNCTION_CODE + ".BLANK"))
                    pWorksheet.SetColumnWidth(i, 3 * 254);
                else
                    pWorksheet.SetColumnWidth(i, 15 * 254);                
            }
        }

        protected void TranslateColumnHeader(NPOI.HSSF.UserModel.HSSFSheet pWorksheet, int m_headerRow)
        {
            NPOI.HSSF.UserModel.HSSFRow m_excelHeaderRow = (NPOI.HSSF.UserModel.HSSFRow)pWorksheet.GetRow(m_headerRow);  // m_headerRow is marked in WriteCompanyHeader()
            //NPOI.HSSF.UserModel.HSSFCell m_headerCell;
            //for (int i= 0; i < m_excelHeaderRow.Cells)

            foreach(NPOI.HSSF.UserModel.HSSFCell m_cell in m_excelHeaderRow.Cells)
            {
                if (m_cell.StringCellValue.Equals(FUNCTION_CODE + ".Seq") || m_cell.StringCellValue.Equals(FUNCTION_CODE + ".BLANK"))
                {
                    m_cell.SetCellValue("");
                    continue;
                }

                if (m_cell.StringCellValue.StartsWith(FUNCTION_CODE + "." + HIERARCHY_PREFIX))
                {
                    m_cell.SetCellValue(m_cell.StringCellValue.Replace(FUNCTION_CODE + "." + HIERARCHY_PREFIX, ""));
                }
                else if (m_cell.StringCellValue.StartsWith(FUNCTION_CODE + "." + PAYMENTCODE_PREFIX))
                {
                    m_cell.SetCellValue(m_cell.StringCellValue.Replace(FUNCTION_CODE + "." + PAYMENTCODE_PREFIX, ""));
                }
                else if (m_cell.StringCellValue.StartsWith (FUNCTION_CODE + "."))
                {
                    m_cell.SetCellValue(HROne.Common.WebUtility.GetLocalizedString(m_cell.StringCellValue).Replace("\"", ""));
                }
            }
        }

        protected int WriteCompanyHeader(NPOI.HSSF.UserModel.HSSFSheet pWorksheet, 
                                         NPOI.HSSF.UserModel.HSSFCellStyle pReportHeaderStyle,
                                         NPOI.HSSF.UserModel.HSSFCellStyle pColumnHeaderNumberStyle,
                                         NPOI.HSSF.UserModel.HSSFCellStyle pColumnHeaderTextStyle, 
                                         int pRow, string pCompanyName, DateTime pPayPeriodFr,
                                         string pHLevel1Name, string pHLevel2Name, string pHLevel3Name, 
                                         DataColumnCollection pColumns)

        {
            NPOI.HSSF.UserModel.HSSFRow m_excelRow;
            //NPOI.HSSF.UserModel.HSSFCell m_cell;

            m_excelRow = (NPOI.HSSF.UserModel.HSSFRow)pWorksheet.CreateRow(pRow);
            WriteCellValue(m_excelRow, 0, "Company: " + pCompanyName, pReportHeaderStyle);

            pRow++;
            m_excelRow = (NPOI.HSSF.UserModel.HSSFRow)pWorksheet.CreateRow(pRow);
            WriteCellValue(m_excelRow, 0, "Payroll Month: " + pPayPeriodFr.ToString("MMM-yyyy"), pReportHeaderStyle);

            pRow++;
            pRow++;
            m_excelRow = (NPOI.HSSF.UserModel.HSSFRow)pWorksheet.CreateRow(pRow);

            int m_Col2Write = -1;
            // write employee information
            int r = 0;
            for (; m_Col2Write < 9 && r < pColumns.Count; r++)
            {
                if (pColumns[r].ColumnName.StartsWith(FUNCTION_CODE + "."))
                {
                    m_Col2Write++;
                    TOTAL_REPORT_COLUMNS = m_Col2Write + 1;
                    WriteCellValue(m_excelRow, m_Col2Write, pColumns[r].ColumnName, pColumnHeaderTextStyle);
                }
            }
            // write payment code columns
            for (int i = r; i < pColumns.Count; i++)
            {
                if (pColumns[i].ColumnName.Contains(FUNCTION_CODE + "." + PAYMENTCODE_PREFIX))
                {
                    m_Col2Write++;
                    TOTAL_REPORT_COLUMNS = m_Col2Write + 1;
                    WriteCellValue(m_excelRow, m_Col2Write, pColumns[i].ColumnName, pColumnHeaderNumberStyle);
                }
            }
            // write other information
            for (int i = r; i < pColumns.Count; i++)
            {
                //if (pColumns[i].ColumnName.Equals(FUNCTION_CODE + ".Remarks"))
                //{
                //    m_remarksCol = i;
                //}
                //else if (pColumns[i].ColumnName.Equals(FUNCTION_CODE + ".BLANK"))
                //{
                //    m_blankCol = i;
                //}

                {
                    if (pColumns[i].ColumnName.Contains(FUNCTION_CODE + ".") && !pColumns[i].ColumnName.Contains(PAYMENTCODE_PREFIX))
                    {
                        m_Col2Write++;
                        TOTAL_REPORT_COLUMNS = m_Col2Write + 1;
                        if (pColumns[i].DataType.Equals(typeof(double)))
                            WriteCellValue(m_excelRow, m_Col2Write, pColumns[i].ColumnName, pColumnHeaderNumberStyle);
                        else if (pColumns[i].ColumnName.Equals(FUNCTION_CODE + ".BLANK"))
                            WriteCellValue(m_excelRow, m_Col2Write, pColumns[i].ColumnName, null);
                        else
                            WriteCellValue(m_excelRow, m_Col2Write, pColumns[i].ColumnName, pColumnHeaderTextStyle);
                    }
                }

            }

            return pRow;
        }

        protected double ToDouble(object pObj)
        {
            double m_value = 0;

            if (double.TryParse(pObj.ToString(), out m_value))
                return m_value;
            else
                return 0;
        }

        protected string BuildSQLTerm(string pFieldName, string pCriteria)
        {
            return BuildSQLTerm(pFieldName, "=", pCriteria);
        }

        protected string BuildSQLTerm(string pFieldName, string pOperator, string pCriteria)
        {
            string m_term;

            if (string.IsNullOrEmpty(pCriteria))
            {
                m_term = "[" + pFieldName + "] IS NULL";
            }else
                m_term = "[" + pFieldName + "] " + pOperator + " '" + pCriteria.Replace("'", "''") + "' ";

            return m_term;
        }

        protected override void GenerateWorkbookDetail(NPOI.HSSF.UserModel.HSSFWorkbook workBook, System.Data.DataSet dataSet)
        {
            NPOI.HSSF.UserModel.HSSFCellStyle reportHeaderStyle;
            NPOI.HSSF.UserModel.HSSFCellStyle columnHeaderNumberStyle;
            NPOI.HSSF.UserModel.HSSFCellStyle columnHeaderTextStyle;
            NPOI.HSSF.UserModel.HSSFCellStyle detailTextStyle;
            NPOI.HSSF.UserModel.HSSFCellStyle detailAmountStyle;
            NPOI.HSSF.UserModel.HSSFCellStyle subtotalNumberStyle;
            NPOI.HSSF.UserModel.HSSFCellStyle subtotalTextStyle;
            NPOI.HSSF.UserModel.HSSFCellStyle grandTotalNumberStyle;
            NPOI.HSSF.UserModel.HSSFCellStyle grandTotalTextStyle;

#region "Define Workbook Style"

            NPOI.HSSF.UserModel.HSSFFont boldFont10Blue = (NPOI.HSSF.UserModel.HSSFFont)workBook.CreateFont();
            boldFont10Blue.Boldweight = 700;
            boldFont10Blue.Color = NPOI.SS.UserModel.IndexedColors.BLUE.Index;
            boldFont10Blue.FontHeightInPoints = 10;
            
            NPOI.HSSF.UserModel.HSSFFont boldFont10 = (NPOI.HSSF.UserModel.HSSFFont)workBook.CreateFont();
            boldFont10.Boldweight = 700;
            boldFont10.FontHeightInPoints = 10;

            NPOI.HSSF.UserModel.HSSFFont boldFont14 = (NPOI.HSSF.UserModel.HSSFFont)workBook.CreateFont();
            boldFont14.Boldweight = 700;
            boldFont14.FontHeightInPoints = 10;

            NPOI.HSSF.UserModel.HSSFFont normalFont10 = (NPOI.HSSF.UserModel.HSSFFont)workBook.CreateFont();
            normalFont10.FontHeightInPoints = 10;

            reportHeaderStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workBook.CreateCellStyle();
            reportHeaderStyle.SetFont(boldFont14);
            reportHeaderStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.LEFT;

            //groupSummaryStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workBook.CreateCellStyle();
            //groupSummaryStyle.SetFont(boldFont10Blue);
            
            columnHeaderNumberStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workBook.CreateCellStyle();
            columnHeaderNumberStyle.SetFont(boldFont10);
            columnHeaderNumberStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
            columnHeaderNumberStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
            columnHeaderNumberStyle.WrapText = true;
            columnHeaderNumberStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
            columnHeaderNumberStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;
            columnHeaderNumberStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
            columnHeaderNumberStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;

            columnHeaderTextStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workBook.CreateCellStyle();
            columnHeaderTextStyle.SetFont(boldFont10);
            columnHeaderTextStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
            columnHeaderTextStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
            columnHeaderTextStyle.WrapText = true;
            columnHeaderTextStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
            columnHeaderTextStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;
            columnHeaderTextStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
            columnHeaderTextStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;

            detailTextStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workBook.CreateCellStyle();
            detailTextStyle.SetFont(normalFont10);
            detailTextStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.LEFT;
            detailTextStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;
            detailTextStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;

            detailAmountStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workBook.CreateCellStyle();
            detailAmountStyle.SetFont(normalFont10);
            detailAmountStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.RIGHT;
            detailAmountStyle.DataFormat = workBook.CreateDataFormat().GetFormat("#,##0.00;(#,##0.00);-");
            detailAmountStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;
            detailAmountStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;

            subtotalNumberStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workBook.CreateCellStyle();
            subtotalNumberStyle.SetFont(boldFont10Blue);
            subtotalNumberStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.RIGHT;
            subtotalNumberStyle.DataFormat = workBook.CreateDataFormat().GetFormat("#,##0.00;(#,##0.00);-");
            subtotalNumberStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
            subtotalNumberStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;
            subtotalNumberStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
            subtotalNumberStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;

            subtotalTextStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workBook.CreateCellStyle();
            subtotalTextStyle.SetFont(boldFont10Blue);
            subtotalTextStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.LEFT;
            subtotalTextStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
            subtotalTextStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;
            subtotalTextStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
            subtotalTextStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;
            //subtotalTextStyle.DataFormat = workBook.CreateDataFormat().GetFormat("#,##0.00;(#,##0.00);-");

            grandTotalNumberStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workBook.CreateCellStyle();
            grandTotalNumberStyle.SetFont(boldFont10Blue);
            grandTotalNumberStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.RIGHT;
            grandTotalNumberStyle.DataFormat = workBook.CreateDataFormat().GetFormat("#,##0.00;(#,##0.00);-");
            grandTotalNumberStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;

            grandTotalTextStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workBook.CreateCellStyle();
            grandTotalTextStyle.SetFont(boldFont10Blue);
            grandTotalTextStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.LEFT;
            //grandTotalTextStyle.DataFormat = workBook.CreateDataFormat().GetFormat("#,##0.00;(#,##0.00);-");
            grandTotalTextStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;

#endregion "Define Workbook Style"


            int m_headerRow = 0;
            int m_row = 0;
            int m_col = 0;
            int m_level1Row = 0;
            int m_level2Row = 0;
            int m_level3Row = 0;

            string m_HLevelFieldName1;
            string m_HLevelFieldName2;
            string m_HLevelFieldName3;

            string m_grandTotalFormula = "";

            DataTable m_table = dataSet.Tables[0];

            // facilitate looping order of hierarchy level, in term determine the grouping sub-total position.
            m_HLevelFieldName1 = m_table.Columns[5].ColumnName; // e.g. division
            m_HLevelFieldName2 = m_table.Columns[6].ColumnName; // e.g. Department
            m_HLevelFieldName3 = m_table.Columns[7].ColumnName; // e.g. Team

            if (m_HierarchyLevelGrouping == 2)
            {
                m_HLevelFieldName1 = m_table.Columns[6].ColumnName;
                m_HLevelFieldName2 = m_table.Columns[5].ColumnName;
                m_HLevelFieldName3 = m_table.Columns[7].ColumnName;
            }
            else if (m_HierarchyLevelGrouping == 3)
            {
                m_HLevelFieldName1 = m_table.Columns[7].ColumnName;
                m_HLevelFieldName2 = m_table.Columns[5].ColumnName;
                m_HLevelFieldName3 = m_table.Columns[6].ColumnName;
            }
            
            m_table.DefaultView.Sort = "Company, " +
                                       m_HLevelFieldName1 + ", " +
                                       m_HLevelFieldName2 + ", " +
                                       m_HLevelFieldName3 + ", " +
                                       FUNCTION_CODE + ".EmpNo, " +
                                       "PayPeriodFr ";

            DataTable m_companyTable = dataSet.Tables[0].DefaultView.ToTable(true, new string[] { "Company", "PayPeriodFr" });

            DataTable m_hierarchy1Table = m_table.DefaultView.ToTable(true, new string[] { "Company", "PayPeriodFr", m_HLevelFieldName1 });

            DataTable m_hierarchy2Table = m_table.DefaultView.ToTable(true, new string[] { "Company", "PayPeriodFr", m_HLevelFieldName1, 
                                                                                                                     m_HLevelFieldName2 });

            DataTable m_hierarchy3Table = m_table.DefaultView.ToTable(true, new string[] { "Company", "PayPeriodFr", m_HLevelFieldName1, 
                                                                                                                     m_HLevelFieldName2, 
                                                                                                                     m_HLevelFieldName3});

            NPOI.HSSF.UserModel.HSSFSheet m_worksheet = (NPOI.HSSF.UserModel.HSSFSheet)workBook.CreateSheet();

            foreach (DataRow m_companyRow in m_companyTable.Rows)
            {
                DateTime m_date;
                DateTime.TryParse(m_companyRow["PayPeriodFr"].ToString(), out m_date);

                m_row = WriteCompanyHeader(m_worksheet, reportHeaderStyle, columnHeaderNumberStyle, columnHeaderTextStyle,
                                           m_row, m_companyRow["Company"].ToString(), m_date,
                                           m_table.Columns[1].ColumnName, m_table.Columns[2].ColumnName, m_table.Columns[3].ColumnName, m_table.Columns);

                // Start 0000082, Ricky So, 2015-04-10
                int m_detailSeq = 1;
                // End 0000082, Ricky So, 2015-04-10

                m_headerRow = m_row;

#region "Write Level 1"
                foreach (DataRow m_hierarchy1Row in m_hierarchy1Table.Select("Company = '" + m_companyRow["Company"] + "' AND " +
                                                                             "PayPeriodFr = '" + m_date.ToString("yyyy-MM-dd") + "' ",
                                                                             m_HLevelFieldName1))
                {
                    m_level1Row = m_row + 1;

                    #region "Write Level 2"
                    //foreach (DataRow m_hierarchy2Row in m_hierarchy2Table.Select("Company = '" + m_companyRow["Company"] + "' AND " +
                    //                                                             "PayPeriodFr = '" + m_date.ToString("yyyy-MM-dd") + "' AND " +
                    //                                                             BuildSQLTerm(m_HLevelFieldName1, m_hierarchy1Row[m_HLevelFieldName1].ToString()) + " ",
                    //                                                             m_HLevelFieldName2))
                    {
                        m_level2Row = m_row + 1;

                        #region "Write Level 3"
                        //foreach (DataRow m_hierarchy3Row in m_hierarchy3Table.Select("Company = '" + m_companyRow["Company"] + "' AND " +
                        //                                                             "PayPeriodFr = '" + m_date.ToString("yyyy-MM-dd") + "' AND " +
                        //                                                             BuildSQLTerm(m_HLevelFieldName1, m_hierarchy2Row[m_HLevelFieldName1].ToString()) + " AND " +
                        //                                                             BuildSQLTerm(m_HLevelFieldName2, m_hierarchy2Row[m_HLevelFieldName2].ToString()),
                        //                                                             m_HLevelFieldName3))
                        {
                            m_level3Row = m_row + 1;

                            #region "Write Detail"
                            foreach (DataRow m_employeeRow in m_table.Select("Company = '" + m_companyRow["Company"] + "' AND " +
                                                                             "PayPeriodFr = '" + m_date.ToString("yyyy-MM-dd") + "' AND " +
                                                                              BuildSQLTerm(m_HLevelFieldName1, m_hierarchy1Row[m_HLevelFieldName1].ToString()), // + " AND " +
                                                                              //BuildSQLTerm(m_HLevelFieldName1, m_hierarchy3Row[m_HLevelFieldName1].ToString()), // + " AND " +
                                                                              // BuildSQLTerm(m_HLevelFieldName2, m_hierarchy3Row[m_HLevelFieldName2].ToString()) + " AND " +
                                                                              // BuildSQLTerm(m_HLevelFieldName3, m_hierarchy3Row[m_HLevelFieldName3].ToString()),
                                                                              FUNCTION_CODE + ".EnglishName"))
                            {
                                m_row++;

                                NPOI.HSSF.UserModel.HSSFRow m_excelHeaderRow = (NPOI.HSSF.UserModel.HSSFRow)m_worksheet.GetRow(m_headerRow);  // m_headerRow is marked in WriteCompanyHeader()
                                NPOI.HSSF.UserModel.HSSFRow m_detailRow = (NPOI.HSSF.UserModel.HSSFRow)m_worksheet.CreateRow(m_row);
                                NPOI.HSSF.UserModel.HSSFCell m_headerCell;

                                // Start 0000082, Ricky So, 2015-04-10
                                WriteCellValue(m_detailRow, 0, m_detailSeq++, detailTextStyle);
                                // End 0000082, Ricky So, 2015-04-10

                                // write detail according to the header captions
                                for (int i = 1; i < TOTAL_REPORT_COLUMNS; i++)
                                {
                                    m_headerCell = (NPOI.HSSF.UserModel.HSSFCell)m_excelHeaderRow.GetCell(i);
                                    if (m_headerCell.StringCellValue.StartsWith(FUNCTION_CODE + "."))
                                    {
                                        if (i <= 8 || m_headerCell.StringCellValue.Equals(FUNCTION_CODE + ".Remarks") || m_headerCell.StringCellValue.Equals(FUNCTION_CODE + ".BLANK"))
                                        {
                                            WriteCellValue(m_detailRow, i, m_employeeRow[m_headerCell.StringCellValue].ToString(), detailTextStyle);
                                        }
                                        else
                                        {                                           
                                            WriteCellValue(m_detailRow, i, ToDouble(m_employeeRow[m_headerCell.StringCellValue].ToString()), detailAmountStyle);
                                        }
                                    }
                                }
                            }
                            #endregion "Write Detail"

                        }
                        #endregion "Write Level 3"

                    }
#endregion "Write Level 2"
                    //write sub-total
                    {
                        m_row++;
                        WriteGroupTotal(m_worksheet, subtotalNumberStyle, subtotalTextStyle, m_HierarchyLevelGrouping, m_hierarchy1Row[2].ToString(), m_row, m_level1Row, m_headerRow);

                        if (m_grandTotalFormula != "")
                            m_grandTotalFormula = m_grandTotalFormula + "+" + BuildSumFormula("%", m_level1Row, m_row - 1);
                        else
                            m_grandTotalFormula = BuildSumFormula("%", m_level1Row, m_row - 1);

                        // Start 0000082, Ricky So, 2015-10-04
                        m_detailSeq = 1;
                        // End 0000082, Ricky So, 2015-10-04
                    }
                }
#endregion "Write Level 1"
                m_row++;
                WriteGrandTotal(m_worksheet, grandTotalNumberStyle, grandTotalTextStyle, m_HierarchyLevelGrouping, "Grand Total", m_row, m_grandTotalFormula, m_headerRow);
                SetColumnWidth(m_worksheet, m_headerRow);
                TranslateColumnHeader(m_worksheet, m_headerRow);
                m_row += 5;
                // Start 0000082, Ricky So, 2015-10-04
                m_detailSeq = 1;
                // End 0000082, Ricky So, 2015-10-04
            }


            //set printer properties
            m_worksheet.PrintSetup.Landscape = true;
            m_worksheet.PrintSetup.FitWidth = (short)1;

            m_worksheet.SetMargin(NPOI.SS.UserModel.MarginType.TopMargin, (double)0);
            m_worksheet.SetMargin(NPOI.SS.UserModel.MarginType.BottomMargin, (double)0);
            m_worksheet.SetMargin(NPOI.SS.UserModel.MarginType.LeftMargin, (double)0);
            m_worksheet.SetMargin(NPOI.SS.UserModel.MarginType.RightMargin, (double)0);

            m_worksheet.PrintSetup.HeaderMargin = (double)1.3;
            m_worksheet.PrintSetup.FooterMargin = (double)1.3;

        }

        protected string BuildSumFormula(string pCol, int pStartRow, int pEndRow)
        {
            // e.g. "SUM(A1:A30)"
            return "SUM(" + pCol + (pStartRow+1).ToString("0") + ":" + pCol + (pEndRow+1).ToString("0") + ")";
        }

        protected string ToExcelColumn(int pColIndex)
        {
            string m_cellString = "";

            if (pColIndex >= 26)
            {
                m_cellString = Convert.ToChar((pColIndex / 26) - 1 + 65).ToString();
            }

            pColIndex = pColIndex % 26;
            m_cellString += Convert.ToChar(pColIndex + 65).ToString();

            return m_cellString;
        }

        protected void WriteGroupTotal(NPOI.HSSF.UserModel.HSSFSheet pWorksheet,
                                          NPOI.HSSF.UserModel.HSSFCellStyle pGroupTotalNumberStyle,
                                          NPOI.HSSF.UserModel.HSSFCellStyle pGroupTotalTextStyle,
                                          int pHierarchyLevel, string pHierarchyDesc, 
                                          int pRow, int pLevelStartRow, int pHeaderRow)
        {
            NPOI.HSSF.UserModel.HSSFRow m_excelRow;
            NPOI.HSSF.UserModel.HSSFRow m_headerRow;

            m_excelRow = (NPOI.HSSF.UserModel.HSSFRow)pWorksheet.CreateRow(pRow);
            m_headerRow = (NPOI.HSSF.UserModel.HSSFRow)pWorksheet.GetRow(pHeaderRow);

            for (int i = 0; i <= 9; i++)
            {
                WriteCellValue(m_excelRow, i, "", pGroupTotalTextStyle);
            }

            if (string.IsNullOrEmpty(pHierarchyDesc))
                WriteCellValue(m_excelRow, 3 + pHierarchyLevel, "n/a", pGroupTotalTextStyle);
            else
                WriteCellValue(m_excelRow, 3 + pHierarchyLevel, pHierarchyDesc, pGroupTotalTextStyle);

            for (int i = 10; i < TOTAL_REPORT_COLUMNS; i++)
            {
                NPOI.HSSF.UserModel.HSSFCell m_headerCell = (NPOI.HSSF.UserModel.HSSFCell)m_headerRow.GetCell(i);


                if (m_headerCell.StringCellValue.Equals(FUNCTION_CODE + ".Remarks"))
                {
                    string m_colName = ToExcelColumn(i);
                    WriteCellFormula(m_excelRow, i, "", pGroupTotalTextStyle);
                }
                else if (!m_headerCell.StringCellValue.Equals(FUNCTION_CODE + ".BLANK"))
                {
                    string m_colName = ToExcelColumn(i);
                    WriteCellFormula(m_excelRow, i, BuildSumFormula(ToExcelColumn(i), pLevelStartRow, pRow - 1), pGroupTotalNumberStyle);
                }
            }
        }

        protected void WriteGrandTotal(NPOI.HSSF.UserModel.HSSFSheet pWorksheet,
                                          NPOI.HSSF.UserModel.HSSFCellStyle pGrandTotalNumberStyle,
                                          NPOI.HSSF.UserModel.HSSFCellStyle pGrandTotalTextStyle,
                                          int pHierarchyLevel, string pDesc, 
                                          int pRow, string pGrandTotalFormulaTemplate, int pHeaderRow)
        {
            NPOI.HSSF.UserModel.HSSFRow m_excelRow;
            NPOI.HSSF.UserModel.HSSFRow m_headerRow;

            m_excelRow = (NPOI.HSSF.UserModel.HSSFRow)pWorksheet.CreateRow(pRow);
            m_headerRow = (NPOI.HSSF.UserModel.HSSFRow)pWorksheet.GetRow(pHeaderRow);

            // write total headcount
            WriteCellFormula(m_excelRow, 0, pGrandTotalFormulaTemplate.Replace("%", "A").Replace("SUM", "COUNT"), pGrandTotalTextStyle);

            // ensure no value cell will have cell borders
            for (int i = 1; i <= 8; i++)
            {
                WriteCellValue(m_excelRow, i, "", pGrandTotalTextStyle);
            }

            WriteCellValue(m_excelRow, 2 + pHierarchyLevel, pDesc, pGrandTotalTextStyle);


            for (int i = 10; i < TOTAL_REPORT_COLUMNS; i++)
            {
                NPOI.HSSF.UserModel.HSSFCell m_headerCell = (NPOI.HSSF.UserModel.HSSFCell)m_headerRow.GetCell(i);
                if (m_headerCell.StringCellValue.Equals(FUNCTION_CODE + ".Remarks"))
                {
                    string m_colName = ToExcelColumn(i);
                    WriteCellFormula(m_excelRow, i, "", pGrandTotalTextStyle);
                }
                else if (!m_headerCell.StringCellValue.Equals(FUNCTION_CODE + ".BLANK"))
                {
                    string m_colName = ToExcelColumn(i);
                    WriteCellFormula(m_excelRow, i, pGrandTotalFormulaTemplate.Replace("%", m_colName), pGrandTotalNumberStyle);
                }
            }
        }
    }
}
