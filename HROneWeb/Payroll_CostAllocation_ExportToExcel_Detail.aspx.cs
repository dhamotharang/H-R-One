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

public partial class Payroll_CostAllocation_ExportToExcel_Detail : HROneWebPage
{
    private const string FUNCTION_CODE = "CST004";

    public Binding binding;

    protected SearchBinding sbinding;
    public DBManager db = ECostAllocation.db;
    public EPayrollGroup obj;
    public int CurID = -1;
    public int CurPayPeriodID = -1;

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
        if (!Page.IsPostBack)
        {
            if (DecryptedRequest["CostAllocationStatus"] != null)
            {
                try
                {
                    CostAllocationStatus.SelectedValue = DecryptedRequest["CostAllocationStatus"];
                }
                catch { }
            }
        }
        binding = new Binding(dbConn, db);
        //binding.add(new DropDownVLBinder(EPayrollGroup.db, PayGroupID, EPayrollGroup.VLPayrollGroup));
        //binding.add(CurrentPayPeriodID);

        //DBFilter payPeriodFilter = new DBFilter();
        //if (DecryptedRequest["PayGroupID"] != null)
        //    payPeriodFilter.add(new Match("PayGroupID", DecryptedRequest["PayGroupID"]));
        //else
        //    payPeriodFilter.add(new Match("PayGroupID", 0));
        //payPeriodFilter.add("PayPeriodFr", false);

        //DBFilter costAllocationFilter = new DBFilter();
        //costAllocationFilter.add(new Match("CostAllocationStatus", CostAllocationStatus.SelectedValue));

        //DBFilter empPayrollFilter = new DBFilter();
        //empPayrollFilter.add(new Match("EmpPayStatus", "C"));
        //empPayrollFilter.add(new IN("EmpPayrollID", "Select EmpPayrollID from " + ECostAllocation.db.dbclass.tableName, costAllocationFilter));


        //payPeriodFilter.add(new IN("PayPeriodID", "Select PayPeriodID from " + EEmpPayroll.db.dbclass.tableName, empPayrollFilter));


        //binding.add(new DropDownVLBinder(EPayrollPeriod.db, PayPeriodID, EPayrollPeriod.VLPayrollPeriod, payPeriodFilter));

        binding.init(Request, Session);

        sbinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);
        sbinding.add(new DropDownVLSearchBinder(Year, "Year(pp.PayPeriodFr)", EPayrollPeriod.VLPayrollPeriodYear, false));//, null, "Year(pp.PayPeriodFR)"));
        sbinding.add(new DropDownVLSearchBinder(Month, "Month(pp.PayPeriodFr)", Values.VLMonth, false));
        sbinding.init(DecryptedRequest, null);

        //try
        //{
        //    CurID = Int32.Parse(DecryptedRequest["PayGroupID"]);
        //    if (!string.IsNullOrEmpty(DecryptedRequest["PayPeriodID"]))
        //        CurPayPeriodID = Int32.Parse(DecryptedRequest["PayPeriodID"]);
        //    else if (!Int32.TryParse(PayPeriodID.SelectedValue, out CurPayPeriodID))
        //    {
        //        EPayrollGroup obj = new EPayrollGroup();
        //        obj.PayGroupID = CurID;
        //        if (db.select(dbConn, obj))
        //            CurPayPeriodID = obj.CurrentPayPeriodID;
        //    }
        //}
        //catch (Exception ex)
        //{
        //}

        info = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);


    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //if (!Page.IsPostBack)
        {
            //if (CurID > 0)
            //{
            panelPayPeriod.Visible = true;
            //    loadObject();
            view = loadData(info, db, Repeater);
            //}
            //else
            //    panelPayPeriod.Visible = false;

        }
    }

    //protected bool loadObject()
    //{
    //    obj = new EPayrollGroup();
    //    bool isNew = WebFormWorkers.loadKeys(EPayrollGroup.db, obj, DecryptedRequest);
    //    if (!EPayrollGroup.db.select(dbConn, obj))
    //        return false;

    //    Hashtable values = new Hashtable();
    //    EPayrollGroup.db.populate(obj, values);
    //    binding.toControl(values);

    //    if (CurPayPeriodID <= 0)
    //    {
    //        CurPayPeriodID = obj.CurrentPayPeriodID;
    //    }
    //    try
    //    {
    //        PayPeriodID.SelectedValue = CurPayPeriodID.ToString();
    //        ucPayroll_PeriodInfo.CurrentPayPeriodID = CurPayPeriodID;
    //    }
    //    catch (Exception ex)
    //    {
    //        CurPayPeriodID = 0;
    //    }
    //    if (PayPeriodID.SelectedIndex == 0)
    //    {
    //        CurPayPeriodID = 0;
    //        ucPayroll_PeriodInfo.CurrentPayPeriodID = 0;
    //    }
    //    if (CurPayPeriodID > 0)
    //        panelCostAllocationAdjustmentDetail.Visible = true;
    //    else
    //        panelCostAllocationAdjustmentDetail.Visible = false;


    //    return true;
    //}

    //protected void CostAllocationStatus_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    Response.Redirect(Request.Url.LocalPath + "?PayGroupID=" + PayGroupID.SelectedValue + "&CostAllocationStatus=" + CostAllocationStatus.SelectedValue);
    //}
    //protected void PayGroupID_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    Response.Redirect(Request.Url.LocalPath + "?PayGroupID=" + PayGroupID.SelectedValue + "&CostAllocationStatus=" + CostAllocationStatus.SelectedValue);
    //}
    //protected void PayPeriodID_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    Response.Redirect(Request.Url.LocalPath + "?PayGroupID=" + PayGroupID.SelectedValue + "&PayPeriodID=" + PayPeriodID.SelectedValue + "&CostAllocationStatus=" + CostAllocationStatus.SelectedValue);
    //}


    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = sbinding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = " e.*, ca.CostAllocationID ";
        string from = "from " + EEmpPersonalInfo.db.dbclass.tableName + " e, " + ECostAllocation.db.dbclass.tableName + " ca, " + EEmpPayroll.db.dbclass.tableName + " ep, " + EPayrollPeriod.db.dbclass.tableName + " pp";

        filter.add(new MatchField("e.EmpID", "ca.EmpID"));
        filter.add(new MatchField("ca.EmpPayrollID", "ep.EmpPayrollID"));
        filter.add(new MatchField("ep.PayPeriodID", "pp.PayPeriodID"));
        filter.add(new Match("ca.CostAllocationStatus", CostAllocationStatus.SelectedValue));

        //DBFilter empPayrollFilter = new DBFilter();
        //empPayrollFilter.add(new Match("ep.EmpPayStatus", "C"));
        //empPayrollFilter.add(new Match("ep.PayPeriodID", CurPayPeriodID));

        // Start 0000129, Ricky So, 2014/11/11
        filter.add(WebUtils.AddRankFilter(Session, "e.EmpID", true));
        // End 0000129, Ricky So, 2014/11/11

        //DBFilter inCostAllocationFilter = new DBFilter();
        //filter.add(new IN("ca.EmpPayrollID", "Select EmpPayrollID from " + EEmpPayroll.db.dbclass.tableName + " ep", empPayrollFilter));

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);


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
        WebFormUtils.LoadKeys(db, row, cb);
        cb.Visible = IsAllowEdit;
    }

    protected void btnUndo_Click(object sender, EventArgs e)
    {

    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        ArrayList list = WebUtils.SelectedRepeaterItemToBaseObjectList(ECostAllocation.db, Repeater, "ItemSelect");

        if (list.Count > 0)
        {
            string exportFileName = System.IO.Path.GetTempFileName();
            System.IO.File.Delete(exportFileName);
            exportFileName += ".xls";
            //System.IO.File.Copy(Server.MapPath("~/template/HistoryList_Template.xls"), exportFileName, true);
            HROne.Export.ExcelExport export = new HROne.Export.ExcelExport(exportFileName);
            DataSet dataSet = new DataSet();// export.GetDataSet();
            DataTable dataTable = new DataTable("CostAllocation$");
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

            dataTable.Columns.Add("EmployeeID", typeof(string));
            dataTable.Columns.Add("EnglishName", typeof(string));
            dataTable.Columns.Add("ChineseName", typeof(string));
            dataTable.Columns.Add("PaymentCodeDesc", typeof(string));
            dataTable.Columns.Add("CostCenterCode", typeof(string));
            dataTable.Columns.Add("Amount", typeof(double));

            foreach (ECostAllocation obj in list)
            {
                if (ECostAllocation.db.select(dbConn, obj))
                {
                    EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                    empInfo.EmpID = obj.EmpID;
                    EEmpPersonalInfo.db.select(dbConn, empInfo);



                    DBFilter costAllocationDetailFilter = new DBFilter();
                    costAllocationDetailFilter.add(new Match("CostAllocationID", obj.CostAllocationID));

                    ArrayList costAllocationDetailList = ECostAllocationDetail.db.select(dbConn, costAllocationDetailFilter);

                    foreach (ECostAllocationDetail detail in costAllocationDetailList)
                    {
                        ECompany company = new ECompany();
                        company.CompanyID = detail.CompanyID;
                        ECompany.db.select(dbConn, company);

                        DataRow row = dataTable.NewRow();
                        row["EmployeeID"] = empInfo.EmpNo;
                        row["EnglishName"] = empInfo.EmpEngFullName; ;
                        row["ChineseName"] = empInfo.EmpChiFullName;
                        row["Company"] = company.CompanyCode;

                        DBFilter costAllocationDetailHierarchyFilter = new DBFilter();
                        costAllocationDetailHierarchyFilter.add(new Match("CostAllocationDetailID", detail.CostAllocationDetailID));

                        ArrayList empHierarchyList = ECostAllocationDetailHElement.db.select(dbConn, costAllocationDetailHierarchyFilter);
                        foreach (ECostAllocationDetailHElement empHierarchy in empHierarchyList)
                        {
                            EHierarchyLevel hierarchyLevel = (EHierarchyLevel)hierarchyLevelHashTable[empHierarchy.HLevelID];
                            if (hierarchyLevel != null)
                            {
                                EHierarchyElement hierarchyElement = new EHierarchyElement();
                                hierarchyElement.HElementID = empHierarchy.HElementID;
                                if (EHierarchyElement.db.select(dbConn, hierarchyElement))
                                    row[hierarchyLevel.HLevelDesc] = hierarchyElement.HElementCode;
                            }
                        }
                        EPaymentCode paymentCode = new EPaymentCode();
                        paymentCode.PaymentCodeID = detail.PaymentCodeID;
                        EPaymentCode.db.select(dbConn, paymentCode);
                        if (detail.CostAllocationDetailIsContribution)
                            row["PaymentCodeDesc"] = paymentCode.PaymentCodeDesc + "(Contribution)";
                        else
                            row["PaymentCodeDesc"] = paymentCode.PaymentCodeDesc;

                        ECostCenter costCenter = new ECostCenter();
                        costCenter.CostCenterID = detail.CostCenterID;
                        ECostCenter.db.select(dbConn, costCenter);
                        row["CostCenterCode"] = costCenter.CostCenterCode;
                        row["Amount"] = detail.CostAllocationDetailAmount;

                        dataTable.Rows.Add(row);

                    }

                }

            }
            export.Update(dataSet);
            WebUtils.TransmitFile(Response, exportFileName, "CostALlocation" + (CostAllocationStatus.SelectedValue.Equals("T")?"Trial":"Confirm") + "_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true);
            return;
        }

        else
        {
            PageErrors errors = PageErrors.getErrors(db, Page.Master);
            errors.addError("Employee not selected");
        }

        view = loadData(info, EEmpPayroll.db, Repeater);

    }
}
