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

public partial class Report_Payroll_PaySlip_List : HROneWebPage
{
    private const string FUNCTION_CODE = "RPT205";
    public Binding binding;

    protected SearchBinding sbinding;

    protected ListInfo info;
    protected DataView view;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;


        WebFormUtils.loadValues(dbConn, HLevel1, EHierarchyLevel.VLHierarchy, null, null, HLevel1.SelectedValue, (string)"combobox.notselected");
        WebFormUtils.loadValues(dbConn, HLevel2, EHierarchyLevel.VLHierarchy, null, null, HLevel2.SelectedValue, (string)"combobox.notselected");



        sbinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);
        sbinding.initValues("EmpStatus", null, EEmpPersonalInfo.VLEmpStatus, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));


        info = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            string hLevelIDListString = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_PAY_SLIP_HIERARCHY_DISPLAY_SEQUENCE);

            if (!string.IsNullOrEmpty(hLevelIDListString))
            {
                string[] hlevelIDList = hLevelIDListString.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                for (int count = 0; count < hlevelIDList.Length; count++)
                {
                    Control ctrl = this.Form.FindControl("mainContentPlaceHolder").FindControl("HLevel" + (count + 1));
                    if (ctrl is DropDownList)
                    {
                        DropDownList tmpDropDownList = (DropDownList)ctrl;
                        try
                        {
                            if (tmpDropDownList.Items.FindByValue(hlevelIDList[count]) != null)
                                tmpDropDownList.SelectedValue = hlevelIDList[count];
                        }
                        catch
                        {
                        }
                    }
                }
            }

            view = loadData(info, EEmpPayroll.db, Repeater);

        }
    }




    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = sbinding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = "e.* ";
        string from = "from " + EEmpPersonalInfo.db.dbclass.tableName + " e ";

        DBFilter empPayrollFilter = new DBFilter();
        empPayrollFilter.add(Payroll_PeriodSelectionList1.GetEmpPayrollDBTerm());


        filter.add(new IN("e.EmpID", "SELECT DISTINCT ep.EmpID FROM " + EEmpPayroll.db.dbclass.tableName + " ep", empPayrollFilter));

        DBFilter empInfoFilter = EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime().Date, AppUtils.ServerDateTime().Date);
        empInfoFilter.add(new MatchField("e.EmpID", "ee.EmpID"));
        filter.add(new Exists(EEmpPersonalInfo.db.dbclass.tableName + " ee", empInfoFilter));

        DataTable table = filter.loadData(dbConn, null, select, from);
        table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);

        if (table.Rows.Count != 0)
        {
            ReportExportControl1.Visible = true;
            ReportExportControl2.Visible = true;
        }
        else
        {
            ReportExportControl1.Visible = false;
            ReportExportControl2.Visible = false;
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


    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(EPayrollPeriod.db, Page.Master);
        errors.clear();

        ArrayList empList = WebUtils.SelectedRepeaterItemToBaseObjectList(EEmpPersonalInfo.db, Repeater, "ItemSelect");
        ArrayList payBatchList = this.Payroll_PeriodSelectionList1.GetPayBatchList();



        //string strEmpList = string.Empty;
        //string strPayBatchList = string.Empty;
        if (empList.Count > 0 && payBatchList.Count>0)
        {

            //foreach (EEmpPersonalInfo o in empList)
            //{
            //    if (strEmpList == string.Empty)
            //        strEmpList = ((EEmpPersonalInfo)o).EmpID.ToString();
            //    else
            //        strEmpList += "_" + ((EEmpPersonalInfo)o).EmpID.ToString();

            //}
            //foreach (EPayrollBatch o in payBatchList)
            //{
            //    if (strPayBatchList == string.Empty)
            //        strPayBatchList = ((EPayrollBatch)o).PayBatchID.ToString();
            //    else
            //        strPayBatchList += "_" + ((EPayrollBatch)o).PayBatchID.ToString();

            //}

            ArrayList hLevelIDList = new ArrayList();
            string hLevelIDListString = string.Empty;
            int tmpHLevelID=0;
            if (int.TryParse(HLevel1.SelectedValue, out tmpHLevelID))
                if (tmpHLevelID > 0)
                {
                    hLevelIDList.Add(tmpHLevelID);
                    if (string.IsNullOrEmpty(hLevelIDListString))
                        hLevelIDListString = tmpHLevelID.ToString();
                    else
                        hLevelIDListString += "|" + tmpHLevelID.ToString();
                }

            if (int.TryParse(HLevel2.SelectedValue, out tmpHLevelID))
                if (tmpHLevelID > 0)
                    hLevelIDList.Add(tmpHLevelID);
            if (string.IsNullOrEmpty(hLevelIDListString))
                hLevelIDListString = tmpHLevelID.ToString();
            else
                hLevelIDListString += "|" + tmpHLevelID.ToString();

            HROne.Reports.Payroll.PayrollDetailProcess rpt = new HROne.Reports.Payroll.PayrollDetailProcess(dbConn, empList, HROne.Reports.Payroll.PayrollDetailProcess.ReportType.PaySlip, null, payBatchList, hLevelIDList);

            string reportFileName = WebUtils.GetLocalizedReportFile(Server.MapPath("~/Report_Payroll_PaySlip.rpt"));

            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_PAY_SLIP_HIERARCHY_DISPLAY_SEQUENCE, hLevelIDListString);
            //WebUtils.RegisterReportExport(this, rpt, reportFileName, ((Button)sender).CommandArgument, "PaySlip", true);
            WebUtils.ReportExport(dbConn, user, errors, lblReportHeader.Text, Response, rpt, reportFileName, ((Button)sender).CommandArgument, "PaySlip", true);
            
        }
        else
            errors.addError("Employee or Payroll Batch not selected");

//        emploadData(empInfo, EEmpPayroll.db, empRepeater);
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
    protected void Payroll_PeriodSelectionList1_PayrollBatchChecked(object sender, EventArgs e)
    {
        info.page = 0;
        view = loadData(info, EEmpPayroll.db, Repeater);
    }


}
