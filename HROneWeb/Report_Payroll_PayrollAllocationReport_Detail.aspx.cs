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
using System.Collections.Generic;

public partial class Report_Payroll_PayrollAllocationReport_Detail : HROneWebPage
{
    private const string FUNCTION_CODE = "RPT255";
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

        // Start 0000185, KuangWei, 2015-05-05
        binding = new SearchBinding(dbConn, db);
        binding.init(DecryptedRequest, null);

        info = ListFooter.ListInfo;
        // End 0000185, KuangWei, 2015-05-05

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            init_company_dropdown();
            // Start 0000185, KuangWei, 2015-05-05
            view = loadData(info, db, Repeater);
            // End 0000185, KuangWei, 2015-05-05
        }
    }

    protected void init_company_dropdown()
    {
        int m_userID = WebUtils.GetCurUser(Session).UserID;

        CompanyID.Items.Add(new ListItem("Not Selected", "0"));

        if (m_userID > 0)
        {
            DBFilter m_UserCompanyFilter = new DBFilter();
            DBFilter m_filter = new DBFilter();

            DataTable m_companyList = new DataTable();
            m_companyList.Columns.Add("CompanyCode", typeof(string));
            m_companyList.Columns.Add("CompanyDesc", typeof(string));
            m_companyList.Columns.Add("CompanyID", typeof(int));

            m_UserCompanyFilter.add(new Match("userID", m_userID));

            m_filter.add(new IN("CompanyID", "SELECT CompanyID FROM UserCompany", m_UserCompanyFilter));
            m_filter.add("CompanyCode", true);

            foreach (ECompany m_company in ECompany.db.select(dbConn, m_filter))
            {
                DataRow m_row = m_companyList.NewRow();
                m_row["CompanyCode"] = m_company.CompanyCode;
                m_row["CompanyDesc"] = m_company.CompanyName;
                m_row["CompanyID"] = m_company.CompanyID;

                m_companyList.Rows.Add(m_row);
            }

            DataRow[] m_rows = m_companyList.Select("", "CompanyCode ASC");

            foreach (DataRow m_row in m_rows)
            {
                CompanyID.Items.Add(new ListItem(m_row["CompanyCode"].ToString(), m_row["CompanyID"].ToString()));
            }
        }
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(EPayrollGroup.db, Page.Master);

        DateTime dtPeriodFrom = new DateTime();
        DateTime dtPeriodTo = new DateTime();
        int intHierarchyLevelID = 0;
        int m_companyID = int.Parse(CompanyID.SelectedValue);

        //if (!string.IsNullOrEmpty(PayPeriodFr.Value))
            if (!DateTime.TryParse(PayPeriodFr.Value, out dtPeriodFrom))
            {
                errors.addError(HROne.Translation.PageErrorMessage.ERROR_DATE_FORMAT);
                return;
            }

        //if (!string.IsNullOrEmpty(PayPeriodTo.Value))
            if (!DateTime.TryParse(PayPeriodTo.Value, out dtPeriodTo))
            {
                errors.addError(HROne.Translation.PageErrorMessage.ERROR_DATE_FORMAT);
                return;
            }

        if ((dtPeriodTo.Year * 12 + dtPeriodTo.Month) - (dtPeriodFrom.Year * 12 + dtPeriodFrom.Month) >= 36)
        {
            errors.addError("Payroll period exceed maximum (36 months)");
            return; 
        }

        if (!int.TryParse(HLevelID.SelectedValue, out intHierarchyLevelID))
        {
            errors.addError("Invalid Hierarchy Level");
            return; 
        }

        // Start 0000185, KuangWei, 2015-05-05
        ArrayList empList = WebUtils.SelectedRepeaterItemToBaseObjectList(db, Repeater, "ItemSelect");
        if (empList.Count <= 0)
        {
            errors.addError("No Employees are selected");
            return; 
        }

        HROne.Reports.Payroll.PayrollAllocationReport_Detail_Process reportProcess = new HROne.Reports.Payroll.PayrollAllocationReport_Detail_Process(dbConn, ci, dtPeriodFrom, dtPeriodTo, intHierarchyLevelID, chkShowIndividuals.Checked, WebUtils.GetCurUser(Session), m_companyID, empList);
        // End 0000185, KuangWei, 2015-05-05

        if (Response.IsClientConnected)
        {
            HROneConfig config = HROneConfig.GetCurrentHROneConfig();
            if (config.GenerateReportAsInbox)
            {
                if (EInboxAttachment.GetTotalSize(dbConn, 0) < WebUtils.productLicense(Session).MaxInboxSizeMB * 1000 * 1000)
                {
                    HROne.TaskService.GenericExcelReportTaskFactory reportTask = new HROne.TaskService.GenericExcelReportTaskFactory(dbConn, user, lblReportHeader.Text, reportProcess, "PFundContributionSummary");
                    AppUtils.reportTaskQueueService.AddTask(reportTask);
                    errors.addError(HROne.Translation.PageMessage.REPORT_GENERATING_TO_INBOX);
                }
                else
                    errors.addError(HROne.Translation.PageMessage.INBOX_SIZE_EXCEEDED);
            }
            else
            {
                System.IO.FileInfo excelFile = reportProcess.GenerateExcelReport();
                WebUtils.TransmitFile(Response, excelFile.FullName, "PFundContributionSummary_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true);
            }
        }
    }

    // Start 0000185, KuangWei, 2015-05-05
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
}
