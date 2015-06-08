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
using AjaxControlToolkit;
using HROne.DataAccess;
//using perspectivemind.validation;
using HROne.Lib.Entities;
using HROne.Import;
using Utility = HROne.CommonLib.Utility;

public partial class SelectEmployee_List : HROneWebPage
{
    protected DBManager db = EEmpPersonalInfo.db;
    protected SearchBinding binding;
    protected ListInfo info;
    protected DataView view;

    public int gPID;
    public string gProcessName;
    public string gP1;
    static string prevPage = String.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            prevPage = Request.UrlReferrer.ToString();
        }

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);


        binding = new SearchBinding(dbConn, db);

        if (!string.IsNullOrEmpty(DecryptedRequest["Process"]))
            gProcessName = DecryptedRequest["Process"].ToString();

        if (!int.TryParse(DecryptedRequest["PID"], out gPID))
            gPID = -1;

        if (!string.IsNullOrEmpty(DecryptedRequest["p1"]))
            gP1 = DecryptedRequest["p1"].ToString();

        binding.init(DecryptedRequest, null);

        info = ListFooter.ListInfo;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            view = loadData(info, db, Repeater);
        }
    }

    protected DBTerm CreateFilterByProcess(string pProcessName, int pProcessID)
    {
        if (pProcessName == "BonusProcess")
        {
            EBonusProcess m_process = EBonusProcess.GetObject(dbConn, pProcessID);
            Match m_match = new Match("E.EmpProbaLastDate", "<=", m_process.BonusProcessPeriodTo);
            return m_match;
        }
        else if (pProcessName == "DoublePayAdjustment")
        {
            // only staffs with commission calculation is configured through latest Recurring Payment
            DBFilter m_rpFilter = new DBFilter();
            OR m_or = new OR();
            m_or.add(new NullTerm("EmpRPEffTo"));
            m_or.add(new Match("EmpRPEffTo", ">=", Utility.LastDateOfMonth(AppUtils.ServerDateTime())));

            //m_rpFilter.add(new NullTerm("NOT EmpRPFPS"));
            m_rpFilter.add(m_or);
            m_rpFilter.add(new Match("EmpRPEffFr", "<=", Utility.LastDateOfMonth(AppUtils.ServerDateTime())));
            m_rpFilter.add(new Match("EmpRPFPS", ">", 0));
            m_rpFilter.add(new Match("EmpRPFPS", "<", 100));
            //m_rpFilter.add(new NullTerm("NOT EmpRPBasicSalary"));
            m_rpFilter.add(new Match("EmpRPBasicSalary", ">", 0));
            m_rpFilter.add(AppUtils.GetPayemntCodeDBTermByPaymentType(dbConn, "PayCodeID", "BASICSAL"));

            // check probation end date
            DBFilter m_EmpPersonalFilter = new DBFilter();
            m_EmpPersonalFilter.add(new Match("EmpProbaLastDate", "<=", new DateTime(AppUtils.ServerDateTime().Year - 1, 12, 31)));
            m_EmpPersonalFilter.add(new IN("EmpID", "SELECT EmpID FROM EmpRecurringPayment", m_rpFilter));

            return new IN("EmpID", "SELECT EmpID FROM EmpPersonalInfo", m_EmpPersonalFilter);
        }
        else if (pProcessName == "HitRateProcess")
        {
            // only staffs with Recurring Payment (where PaymentType == isHitRateBased)
            DBFilter m_rpFilter = new DBFilter();
            OR m_or = new OR();
            m_or.add(new NullTerm("EmpRPEffTo"));
            m_or.add(new Match("EmpRPEffTo", ">=", Utility.LastDateOfMonth(AppUtils.ServerDateTime())));

            m_rpFilter.add(m_or);
            m_rpFilter.add(new Match("EmpRPEffFr", "<=", Utility.LastDateOfMonth(AppUtils.ServerDateTime())));
            
            DBFilter m_isHitRateBasedFilter = new DBFilter();
            m_isHitRateBasedFilter.add(new Match("PaymentCodeIsHitRateBased", true));
            m_rpFilter.add(new IN("PayCodeID", "SELECT PaymentCodeID FROM PaymentCode", m_isHitRateBasedFilter));

            DBFilter m_EmpPersonalFilter = new DBFilter();
            //m_EmpPersonalFilter.add(new Match("EmpStatus", "A"));
            m_EmpPersonalFilter.add(new IN("EmpID", "SELECT EmpID FROM EmpRecurringPayment", m_rpFilter));

            return new IN("EmpID", "SELECT EmpID FROM EmpPersonalInfo", m_EmpPersonalFilter);
        }
        // Start 0000168, KuangWei, 2015-02-09
        else if (pProcessName == "AttendancePreparationProcess")
        {
            // only staffs with Recurring Payment (where PaymentType == isHitRateBased)
            EAttendancePreparationProcess m_process = EAttendancePreparationProcess.GetObject(dbConn, pProcessID);
            
            DBFilter m_rpFilter = new DBFilter();
            OR m_or = new OR();
            m_or.add(new NullTerm("EmpRPEffTo"));
            m_or.add(new Match("EmpRPEffTo", ">=", m_process.AttendancePreparationProcessPeriodTo));

            m_rpFilter.add(m_or);
            m_rpFilter.add(new Match("EmpRPEffFr", "<=", m_process.AttendancePreparationProcessPeriodFr));

            DBFilter m_isRPWinsonFilter = new DBFilter();
            m_rpFilter.add(new IN("EmpRPID", "SELECT EmpRPID FROM EmpRPWinson", m_isRPWinsonFilter));

            DBFilter m_EmpPersonalFilter = new DBFilter();
            //m_EmpPersonalFilter.add(new Match("EmpStatus", "A"));
            m_EmpPersonalFilter.add(new IN("EmpID", "SELECT EmpID FROM EmpRecurringPayment", m_rpFilter));

            return new IN("EmpID", "SELECT EmpID FROM EmpPersonalInfo", m_EmpPersonalFilter);
        }
        // End 0000168, KuangWei, 2015-02-09
        return null; 
    }

    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = binding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);
        filter.add(WebUtils.AddRankFilter(Session, "e.EmpID", true));

        // only staffs with commission calculation is configured through latest Recurring Payment
        DBTerm m_inCondition = CreateFilterByProcess(gProcessName, gPID);
        if (m_inCondition != null)
            filter.add(m_inCondition);
        
        string select = "e.* ";
        string from = "from [" + db.dbclass.tableName + "] e ";

        DBFilter empInfoFilter = EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime(), AppUtils.ServerDateTime());
        empInfoFilter.add(new MatchField("e.EmpID", "ee.EmpID"));
        filter.add(new Exists(EEmpPersonalInfo.db.dbclass.tableName + " ee", empInfoFilter));

        DataTable table = filter.loadData(dbConn, null, select, from);
        table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);
        view = new DataView(table);

        ListFooter.Refresh();
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
        WebFormUtils.LoadKeys(db, row, cb);
        cb.Checked = true;
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(prevPage))
        {
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, prevPage);
        }else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Emp_list.aspx");
    }

    protected DataTable PrepareExportData(string pProcessName, int pPID, ArrayList pEmpList, PageErrors pErrors)
    {
        if (string.Compare(gProcessName, "BonusProcess", true)==0) // ignore case compare
        {
            HROne.Import.ImportBonusProcess m_process = new HROne.Import.ImportBonusProcess(dbConn, Session.SessionID, WebUtils.GetCurUser(Session).UserID, pPID);
            if (gP1 == "S") // Standard Bonus
            {
                m_process.ClearUploadedStandardData(pErrors);
                m_process.GenerateStandardBonusData(pEmpList);
                pErrors.addError("Standard Bonus generation completed");
                return null; 
            }
            else if (gP1 == "D") // Discretionary Bonus
                return m_process.ExportDiscretionaryBonusTemplate(pEmpList, true);
        }else if (string.Compare(gProcessName, "DoublePayAdjustment", true) == 0)
        {
            return HROne.Import.ImportDoublePayAdjustmentProcess.ExportTemplate(dbConn, pEmpList, true);
        }
        else if (string.Compare(gProcessName, "HitRateProcess", true) == 0)
        {
            return HROne.Import.ImportHitRateBasedPaymentProcess.ExportTemplate(dbConn, pEmpList, true);
        }

        return null;
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);

        ArrayList empList = WebUtils.SelectedRepeaterItemToBaseObjectList(EEmpPersonalInfo.db, Repeater, "ItemSelect");

        if (empList.Count > 0)
        {
            string exportFileName = System.IO.Path.GetTempFileName();
            System.IO.File.Delete(exportFileName);
            exportFileName += ".xls";
            HROne.Export.ExcelExport export = new HROne.Export.ExcelExport(exportFileName);
            DataSet dataSet = new DataSet();// export.GetDataSet();

            // Start 0000168, KuangWei, 2015-02-11
            if (string.Compare(gProcessName, "AttendancePreparationProcess", true) == 0)
            {
                HROne.Import.ImportAttendancePreparationProcess.ExportTemplate(dbConn, empList, exportFileName, gPID);
                WebUtils.TransmitFile(Response, exportFileName, "AttendanceRecordDataEntry_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true);
            }
            else
            {
                DataTable m_table = PrepareExportData(gProcessName, gPID, empList, errors);
                if (m_table != null)
                {
                    dataSet.Tables.Add(m_table);
                    export.Update(dataSet);
                    WebUtils.TransmitFile(Response, exportFileName, m_table.TableName + "_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true);

                    errors.addError("Export Completed");
                }
            }
                // End 0000168, KuangWei, 2015-02-11
                return;
        }
        else
        {
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_NO_EMPLOYEE_SELECTED);
        }
    }

    protected void FirstPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);
    }
    protected void PrevPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);
    }
    protected void NextPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);
    }
    protected void LastPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);
    }

}
