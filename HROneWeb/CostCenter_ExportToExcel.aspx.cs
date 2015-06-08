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
using HROne.Lib.Entities;
using HROne.DataAccess;

public partial class CostCenter_ExportToExcel : HROneWebPage
{
    private const string FUNCTION_CODE = "CST006";

    protected SearchBinding sbinding;
    public DBManager db = EEmpPersonalInfo.db;

    protected ListInfo info;
    protected DataView view;

    private bool IsAllowEdit = true;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        if (!WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
        {
            IsAllowEdit = false;
            ConfirmPayrollSelectAllPanel.Visible = false;
        }

        sbinding = new SearchBinding(dbConn, db);
        sbinding.init(DecryptedRequest, null);

        info = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //view = loadData(info, db, Repeater);
    }

    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        string m_year = this.Year.Text; // ((TextBox)EmployeeSearchControl1.AdditionElementControl.FindControl("Year")).Text;
        string m_month = this.Month.SelectedValue; //((DropDownList)EmployeeSearchControl1.AdditionElementControl.FindControl("Month")).SelectedValue;

        if (!string.IsNullOrEmpty(m_year) && !string.IsNullOrEmpty(m_month))
        {
            DBFilter filter = new DBFilter();
            filter.add(getEffRangeDBTerm(m_year, m_month, null));

            DBFilter empfilter = sbinding.createFilter();
            empfilter.add(new IN("e.EmpID", "Select distinct EmpID from " + EEmpCostCenter.db.dbclass.tableName, filter));

            // Start 0000110, Ricky So, 2014/12/16
            DBFilter m_securityFilter = EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime(), AppUtils.ServerDateTime());
            m_securityFilter.add(new MatchField("e.EmpID", "ee.EmpID"));
            empfilter.add(new Exists(EEmpPersonalInfo.db.dbclass.tableName + " ee", m_securityFilter));

            //// Start 0000129, Ricky So, 2014/11/11
            //empfilter.add(WebUtils.AddRankFilter(Session, "e.EmpID", true));
            //// End 0000129, Ricky So, 2014/11/11
            // End 0000110, Ricky So, 2014/12/16

            string select = " e.* ";
            string from = " from " + db.dbclass.tableName + " e ";

            DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, empfilter, info);
            table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);

            if (table.Rows.Count > 0)
                panelPayPeriod.Visible = true;

            view = new DataView(table);

            ListFooter.Refresh();

            if (repeater != null)
            {
                repeater.DataSource = view;
                repeater.DataBind();
            }
        }

        return view;
    }

    protected void Search_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);
    }

    protected void Reset_Click(object sender, EventArgs e)
    {
        //binding.clear();
        EmployeeSearchControl1.Reset();
        this.EmployeeSearchControl1.EmpStatusValue = "A";
        this.Year.Text = "";
        this.Month.SelectedIndex = 0;
        panelPayPeriod.Visible = false;

        //((TextBox)EmployeeSearchControl1.AdditionElementControl.FindControl("Year")).Text = "";
        //((DropDownList)EmployeeSearchControl1.AdditionElementControl.FindControl("Month")).SelectedIndex = 0;

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
        cb.Visible = IsAllowEdit;
    }
    //Start 0000192, miranda, 2015-05-08
    public const string TABLE_NAME = "costCenter_export";
    //End 0000192, miranda, 2015-05-08
    private const string FIELD_EMP_NO = "Emp No";
    private const string FIELD_EMP_NAME = "Staff Name";
    private const string FIELD_JOB_TITLE = "Job Title";
    private const string FIELD_FROM = "From";
    private const string FIELD_TO = "To";
    private const string FIELD_COST_CENTER = "Cost Center";
    private const string FIELD_PERCENTAGE = "%";

    protected void btnExport1_Click(object sender, EventArgs e)
    {
        btnExport_Click(sender, e);
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        ArrayList list = WebUtils.SelectedRepeaterItemToBaseObjectList(db, Repeater, "ItemSelect");
        int GroupingHierarchyLevelID = 1;
        string GroupingHierarchyLevelDesc = string.Empty;

        if (list.Count > 0)
        {
            string exportFileName = System.IO.Path.GetTempFileName();
            System.IO.File.Delete(exportFileName);
            exportFileName += ".xls";
            HROne.Export.ExcelExport export = new HROne.Export.ExcelExport(exportFileName);
            DataSet dataSet = new DataSet();

            DataTable tmpDataTable = dataSet.Tables.Add(TABLE_NAME);
            tmpDataTable.Columns.Add(FIELD_EMP_NO, typeof(string));
            tmpDataTable.Columns.Add(FIELD_EMP_NAME, typeof(string));
            tmpDataTable.Columns.Add(FIELD_JOB_TITLE, typeof(string));
            tmpDataTable.Columns.Add(FIELD_FROM, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_TO, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_COST_CENTER, typeof(string));
            tmpDataTable.Columns.Add(FIELD_PERCENTAGE, typeof(double));

            foreach (EEmpPersonalInfo obj in list)
            {
                if (EEmpPersonalInfo.db.select(dbConn, obj))
                {
                    // get job title
                    string empJobTitle = null;
                    EEmpPositionInfo empPositionInfo = AppUtils.GetLastPositionInfo(dbConn, AppUtils.ServerDateTime().Date, obj.EmpID);
                    if (empPositionInfo != null)
                    {
                        EPosition position = new EPosition();
                        position.PositionID = empPositionInfo.PositionID;
                        if (EPosition.db.select(dbConn, position))
                        {
                            empJobTitle = position.PositionDesc;
                        }
                    }

                    DBFilter filter = new DBFilter();
                    filter.add(new Match("EmpID", obj.EmpID));

                    string m_year = this.Year.Text; // ((TextBox)EmployeeSearchControl1.AdditionElementControl.FindControl("Year")).Text;
                    string m_month = this.Month.Text; // ((DropDownList)EmployeeSearchControl1.AdditionElementControl.FindControl("Month")).SelectedValue;

                    
                    filter.add(getEffRangeDBTerm(m_year, m_month, null));
                    ArrayList empCsts = EEmpCostCenter.db.select(dbConn, filter);
                    
                    foreach(EEmpCostCenter empCostCenter in empCsts)
                    {
                        DBFilter empCstDetailFilter = new DBFilter();
                        empCstDetailFilter.add(new Match("EmpCostCenterID", empCostCenter.EmpCostCenterID));
                        ArrayList empCstDetailList = EEmpCostCenterDetail.db.select(dbConn, empCstDetailFilter);
                        foreach (EEmpCostCenterDetail detail in empCstDetailList)
                        {
                            DataRow row = tmpDataTable.NewRow();
                            row[FIELD_EMP_NO] = obj.EmpNo;
                            row[FIELD_EMP_NAME] = obj.EmpEngFullNameWithAlias;
                            //for field Job Title
                            if (!string.IsNullOrEmpty(empJobTitle))
                            {
                                row[FIELD_JOB_TITLE] = empJobTitle;
                            }
                            //for fields From and To
                            row[FIELD_FROM] = empCostCenter.EmpCostCenterEffFr;
                            row[FIELD_TO] = empCostCenter.EmpCostCenterEffTo;
                            //for field Cost Center
                            ECostCenter costCenter = new ECostCenter();
                            costCenter.CostCenterID = detail.CostCenterID;
                            if (ECostCenter.db.select(dbConn, costCenter))
                            {
                                row[FIELD_COST_CENTER] = costCenter.CostCenterDesc;//costCenter.CostCenterCode
                            }
                            //for field Percentage
                            row[FIELD_PERCENTAGE] = detail.EmpCostCenterPercentage;

                            //add row data to data table
                            tmpDataTable.Rows.Add(row);
                        }
                    }

                }

            }

            export.Update(dataSet);
            WebUtils.TransmitFile(Response, exportFileName, "CostCenterExportReport_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true);
            Response.End();
        }

        else
        {
            PageErrors errors = PageErrors.getErrors(db, Page.Master);
            errors.addError("Employee not selected");
        }

        view = loadData(info, db, Repeater);

    }

    public static DBTerm getEffRangeDBTerm(string year, string month, string tableName)
    {
        return getEffRangeDBTerm(year, month, year, month, tableName);
    }

    public static DBTerm getEffRangeDBTerm(string year, string month, string year2, string month2, string tableName)
    {
        // Start 000192, Ricky So, 2015-05-11
        int m_year;
        int m_month;
        int m_year2;
        int m_month2;

        if (int.TryParse(year, out m_year) && m_year > 1980)
        {
            if (int.TryParse(month, out m_month) && m_month > 0)
            {
                if (int.TryParse(year2, out m_year2) && m_year2 > 1980)
                {
                    if (int.TryParse(month2, out m_month2) && m_month2 > 0)
                    {
                        if (string.IsNullOrEmpty(tableName))
                        {
                            DateTime m_fromDate = new DateTime(m_year, m_month, 1);
                            DateTime m_toDate = new DateTime(m_year2, m_month2, DateTime.DaysInMonth(m_year2, m_month2));

                            AND andTerm = new AND();
                            DBFilter filter = new DBFilter();
                            OR orEffTo = new OR();

                            andTerm.add(new Match("EmpCostCenterEffFr", "<=", m_toDate));
                            orEffTo.add(new Match("EmpCostCenterEffTo", ">=", m_fromDate));
                            orEffTo.add(new NullTerm("EmpCostCenterEffTo"));
                            andTerm.add(orEffTo);
                            return andTerm;
                        }
                        else
                        {
                            DateTime m_fromDate = new DateTime(m_year, m_month, 1);
                            DateTime m_toDate = new DateTime(m_year2, m_month2, DateTime.DaysInMonth(m_year2, m_month2));

                            AND andTerm = new AND();
                            OR orEffTo = new OR();

                            andTerm.add(new Match(tableName + ".EmpCostCenterEffFr", "<=", m_toDate));
                            orEffTo.add(new Match(tableName + ".EmpCostCenterEffTo", ">=", m_fromDate));
                            orEffTo.add(new NullTerm(tableName + ".EmpCostCenterEffTo"));
                            andTerm.add(orEffTo);
                            return andTerm;
                        }
                    }
                }
            }
        }
        return null;
        // End 000192, Ricky So, 2015-05-11

        //AND andEffRangeTerm = new AND();
        //if (!string.IsNullOrEmpty(tableName))
        //{
        //    andEffRangeTerm.add(new Match("Year(" + tableName + ".EmpCostCenterEffFr)", "<=", year));
        //    andEffRangeTerm.add(new Match("Month(" + tableName + ".EmpCostCenterEffFr)", "<=", month));
        //    AND andEffToTerm = new AND();
        //    andEffToTerm.add(new Match("Year(" + tableName + ".EmpCostCenterEffTo)", ">=", year));
        //    andEffToTerm.add(new Match("Month(" + tableName + ".EmpCostCenterEffTo)", ">=", month));
        //    OR orEffToTerm = new OR();
        //    orEffToTerm.add(new NullTerm(tableName + ".EmpCostCenterEffTo"));
        //    orEffToTerm.add(andEffToTerm);
        //    andEffRangeTerm.add(orEffToTerm);
        //}
        //else
        //{
        //    andEffRangeTerm.add(new Match("Year(EmpCostCenterEffFr)", "<=", year));
        //    andEffRangeTerm.add(new Match("Month(EmpCostCenterEffFr)", "<=", month));
        //    AND andEffToTerm = new AND();
        //    andEffToTerm.add(new Match("Year(EmpCostCenterEffTo)", ">=", year));
        //    andEffToTerm.add(new Match("Month(EmpCostCenterEffTo)", ">=", month));
        //    OR orEffToTerm = new OR();
        //    orEffToTerm.add(new NullTerm("EmpCostCenterEffTo"));
        //    orEffToTerm.add(andEffToTerm);
        //    andEffRangeTerm.add(orEffToTerm);
        //}
        //return andEffRangeTerm;
    }
}
