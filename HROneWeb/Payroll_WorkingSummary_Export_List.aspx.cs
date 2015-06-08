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
//using perspectivemind.validation;

public partial class Payroll_WorkingSummary_Export_List : HROneWebPage
{
    private const string FUNCTION_CODE = "PAY013";
    protected SearchBinding empSBinding, sbinding;
    public DBManager db = EEmpPersonalInfo.db;
    public EEmpPersonalInfo obj;

    protected ListInfo empInfo;
    protected DataView empView;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;


        empSBinding = new SearchBinding(dbConn, db);
        empSBinding.initValues("EmpStatus", null, EEmpPersonalInfo.VLEmpStatus, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        empSBinding.init(DecryptedRequest, null);

        empInfo = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            empView = emploadData(empInfo, db, empRepeater);
        }

    }


    public DataView emploadData(ListInfo info, DBManager db, Repeater repeater)
    {

        DBFilter filter = new DBFilter();// empSBinding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = "* ";
        string from = "from [" + EEmpPersonalInfo.db.dbclass.tableName + "] e ";

        filter.add(WebUtils.AddRankFilter(Session, "e.EmpID", true));

        DBFilter empWorkingSummaryFilter = new DBFilter();
        DateTime dtPeriodFr, dtPeriodTo;
        if (DateTime.TryParse(PeriodFr.Value, out dtPeriodFr) && DateTime.TryParse(PeriodTo.Value, out dtPeriodTo))
        {

            empWorkingSummaryFilter.add(new Match("ews.EmpWorkingSummaryAsOfDate", "<=", dtPeriodTo));
            empWorkingSummaryFilter.add(new Match("ews.EmpWorkingSummaryAsOfDate", ">=", dtPeriodFr));
        }
        else
        {
            btnGenerate.Visible = false;
            btnGenerate1.Visible = false;
            empView = null;
            repeater.DataSource = null;
            repeater.DataBind();

            return null;
        }

        filter.add(new IN("EmpID", "Select distinct EmpID from " + EEmpWorkingSummary.db.dbclass.tableName + " ews", empWorkingSummaryFilter));


        DBFilter empInfoFilter = EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime(), AppUtils.ServerDateTime());
        empInfoFilter.add(new MatchField("e.EmpID", "ee.EmpID"));
        //if (DateTime.TryParse(PeriodFr.Value, out dtPeriodFr) && DateTime.TryParse(PeriodTo.Value, out dtPeriodTo))
        //{
        //    empInfoFilter.add(new Match("EmpDateOfJoin", "<=", dtPeriodTo));
        //    DBFilter empTermFilter = new DBFilter();
        //    empTermFilter.add(new MatchField("ee.EmpID", "et.EmpID"));
        //    empTermFilter.add(new Match("et.EmpTermLastDate", "<", dtPeriodFr));
        //    empInfoFilter.add(new Exists(EEmpTermination.db.dbclass.tableName + " et ", empTermFilter, true));

        //}

        filter.add(new Exists(EEmpPersonalInfo.db.dbclass.tableName + " ee", empInfoFilter));




        DataTable table = filter.loadData(dbConn, null, select, from);
        table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);

        if (table.Rows.Count != 0)
        {
            btnGenerate.Visible = true;
            btnGenerate1.Visible = true;
        }
        else
        {
            btnGenerate.Visible = false;
            btnGenerate1.Visible = false;
        }
        empView = new DataView(table);
        //if (info != null)
        //{
        //    info.loadPageList(null, empPrevPage, empNextPage, empFirstPage, empLastPage);

        //    CurPage.Value = info.page.ToString();
        //    NumPage.Value = info.numPage.ToString();
        //}
        if (repeater != null)
        {
            repeater.DataSource = empView;
            repeater.DataBind();
        }

        return empView;
    }
    protected void empChangeOrder_Click(object sender, EventArgs e)
    {
        LinkButton l = (LinkButton)sender;
        String id = l.ID.Substring(1);
        if (empInfo.orderby == null)
            empInfo.order = true;
        else if (empInfo.orderby.Equals(id))
            empInfo.order = !empInfo.order;
        else
            empInfo.order = true;
        empInfo.orderby = id;

        empView = emploadData(empInfo, db, empRepeater);

    }
    protected void empRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(EEmpPersonalInfo.db, row, cb);

    }

    protected void Search_Click(object sender, EventArgs e)
    {
        empInfo.page = 0;
        empView = emploadData(empInfo, db, empRepeater);

    }
    protected void Reset_Click(object sender, EventArgs e)
    {
        EmployeeSearchControl1.Reset();
        empSBinding.clear();
        empInfo.page = 0;

        empView = emploadData(empInfo, db, empRepeater);
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        DateTime dtPeriodFr = new DateTime();
        DateTime dtPeriodTo = new DateTime();

        if (!(DateTime.TryParse(PeriodFr.Value, out dtPeriodFr) && DateTime.TryParse(PeriodTo.Value, out dtPeriodTo)))
        {
            errors.addError("Invalid Date Format"); 
        }

        if (errors.isEmpty())
        {

            ArrayList list = new ArrayList();
            list = WebUtils.SelectedRepeaterItemToBaseObjectList(EEmpPersonalInfo.db, empRepeater, "ItemSelect");

            if (list.Count > 0)
            {
                GenerateAttendanceRecordData(list, dtPeriodFr, dtPeriodTo);
            }
        }
        emploadData(empInfo, db, empRepeater);
    }

    private void GenerateAttendanceRecordData(ArrayList EmpInfoList, DateTime PeriodFrom, DateTime PeriodTo)
    {
        string exportFileName = System.IO.Path.GetTempFileName();
        System.IO.File.Delete(exportFileName);
        exportFileName += ".xls";
        //System.IO.File.Copy(Server.MapPath("~/template/HistoryList_Template.xls"), exportFileName, true);
        HROne.Export.ExcelExport export = new HROne.Export.ExcelExport(exportFileName);
        DataSet dataSet = new DataSet();// export.GetDataSet();

        dataSet.Tables.Add(HROne.Import.ImportEmpWorkingSummaryProcess.Export(dbConn, EmpInfoList, PeriodFrom, PeriodTo));

        export.Update(dataSet);
        //WebUtils.RegisterDownloadFileJavaScript(this, exportFileName, "WorkingSummary_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true, 0);
        WebUtils.TransmitFile(Response, exportFileName, "WorkingSummary_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true);
        return;

    }
}
