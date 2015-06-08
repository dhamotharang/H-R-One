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

public partial class Payroll_TrialRun_List : HROneWebPage
{
    public Binding binding;
    public DBManager db = EPayrollPeriod.db;
    public EPayrollPeriod obj;
    public int CurID = -1;

    protected DataView existingEmployeeView;
    protected DataView newJoinView;
    protected DataView finalPaymentView;

    protected ListInfo existingEmployeeInfo;
    protected ListInfo newJoinInfo;
    protected ListInfo finalPaymentInfo;

    protected SearchBinding existingEmployeeSearchBinding;
    protected SearchBinding newJoinSearchBinding;
    protected SearchBinding finalPaymentSearchBinding;

    private bool IsAllowEdit = true;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, "PAY004", WebUtils.AccessLevel.Read))
            return;
        if (!WebUtils.CheckPermission(Session, "PAY004", WebUtils.AccessLevel.ReadWrite))
        {
            ExistingEmployeeSelectAllPanel.Visible = false;
            NewJoinSelectAllPanel.Visible = false;
            FinalPaymentSelectAllPanel.Visible = false;
            btnProcess.Visible = false;
            chkSkipMPFCal.Visible = false;
            IsAllowEdit = false;
        }

        binding = new Binding(dbConn, db);
        binding.add(PayGroupID);
        binding.add(PayPeriodID);

        existingEmployeeSearchBinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);

        newJoinSearchBinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);

        finalPaymentSearchBinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);
    
        binding.init(Request, Session);

        existingEmployeeInfo = existingEmployeeListFooter.ListInfo;
        newJoinInfo = newJoinListFooter.ListInfo;
        finalPaymentInfo = finalPayment.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(Page, Page.Controls);

        if (!int.TryParse(DecryptedRequest["PayPeriodID"], out CurID))
            CurID = -1;

        if (CurID > 0)
            loadObject();
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            loadData();

        }

    }

    protected bool loadObject()
    {
        obj = new EPayrollPeriod();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
        if (!db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);
        ucPayroll_PeriodInfo.CurrentPayPeriodID = obj.PayPeriodID;
        return true;
    }


    public void loadData()
    {
        if (DecryptedRequest["existingEmployee"] == "1")
        {
            panelExistingEmployee.Visible = true;
            existingEmployeeView = LoadExistingEmployeeData(existingEmployeeInfo, db, ExistingEmployeeRepeater);
        }
        if (DecryptedRequest["newJoin"] == "1")
        {
            panelNewJoin.Visible = true;
            newJoinView = LoadNewJoinData(newJoinInfo, db, NewJoinRepeater);
        }
        if (DecryptedRequest["finalPayment"] == "1")
        {
            panelFinalPayment.Visible = true;
            finalPaymentView = LoadFinalPaymentData(finalPaymentInfo, db, FinalPaymentRepeater);
        }
        //Start 0000178, Miranda, 2015-03-14
        if (DecryptedRequest["RecurringPayment"] == "1")
        {
            chkSkipMPFCal.Visible = false;
        }
        //End 0000178, Miranda, 2015-03-14
    }

    public DataView LoadExistingEmployeeData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = existingEmployeeSearchBinding.createFilter();
        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = " e.* ";
        string from = "from EmpPersonalInfo e ";

        IN inTerm = new IN("e.EmpID", "Select DISTINCT epi.EmpID from EmpPositionInfo epi, PayrollPeriod pp ", filter);

        filter.add(new MatchField("e.EmpID", "epi.EmpID"));
        filter.add(new MatchField("epi.PayGroupID", "pp.PayGroupID"));
        filter.add(new MatchField("epi.EmpPosEffFr", "<=", "pp.PayPeriodTo"));
        filter.add(new Match("pp.PayPeriodID",CurID));
        filter.add(WebUtils.AddRankFilter(Session, "e.EmpID", true));

        OR orPayPeriodFilter = new OR();
        orPayPeriodFilter.add(new MatchField("epi.EmpPosEffTo", ">=", "pp.PayPeriodFr"));
        orPayPeriodFilter.add(new NullTerm("epi.EmpPosEffTo"));

        filter.add(orPayPeriodFilter);

        OR orDateJoinFilter = new OR();

        orDateJoinFilter.add(new MatchField("e.EmpDateOfJoin", "<", "pp.PayPeriodFr "));
        orDateJoinFilter.add(new NullTerm("e.EmpDateOfJoin"));
        filter.add(orDateJoinFilter);

        filter.add(new IN("Not e.empid", "Select DISTINCT ep2.EmpID from EmpPayroll ep2 where ep2.EmpPayStatus='T'", new DBFilter()));
        filter.add(new IN("Not e.empid", "Select DISTINCT et.EmpID from EmpTermination et where et.EmpTermLastDate<=pp.PayPeriodTo", new DBFilter()));

        if (DecryptedRequest["RecurringPayment"] == "1")
        {
            //  Recurring Payment only
            //  Recurring Payment + Claims and Deductions
            //  Recurring Payment + Year End Bonus
            //  Recurring Payment + Claims and Deductions + Year End Bonus
            filter.add(new IN("Not e.empid", "Select DISTINCT epp.empid from EmpPayroll epp where pp.PayPeriodID=epp.PayPeriodID and epp.EmpPayIsRP='Y'", new DBFilter()));
        }
        else if (DecryptedRequest["ClaimsAndDeduction"] == "1")
        {
            // Claims and Deductions only
            // Claims and Deductions + Year End Bonus
            DBFilter CNDFilter = new DBFilter();
            CNDFilter.add(new MatchField("CNDEffDate", "<=", "pp.PayperiodTo"));
            OR orCNDEffDateEmpPosFr = new OR();
            orCNDEffDateEmpPosFr.add(new MatchField("CNDEffDate", ">=", "epi.EmpPosEffFr"));
            orCNDEffDateEmpPosFr.add(new MatchField("epi.EmpPosEffFr", "<=", "pp.PayperiodFr"));
            CNDFilter.add(orCNDEffDateEmpPosFr);

            OR orCNDPos = new OR();
            orCNDPos.add(new MatchField("CNDEffDate", "<=", "epi.EmpPosEffTo"));
            orCNDPos.add(new NullTerm("epi.EmpPosEffTo"));

            CNDFilter.add(orCNDPos);
            OR orCNDPayRecID = new OR();
            orCNDPayRecID.add(new Match("PayRecID", 0));
            orCNDPayRecID.add(new NullTerm("PayRecID"));
            CNDFilter.add(orCNDPayRecID);
            filter.add(new IN("e.empid", "Select DISTINCT cnd.EmpID from " + EClaimsAndDeductions.db.dbclass.tableName + " cnd ", CNDFilter));
        }
        else if (DecryptedRequest["AdditionalRemuneration"] == "1" && (DecryptedRequest["ClaimsAndDeduction"] != "1" && DecryptedRequest["RecurringPayment"] != "1"))
        {
            filter.add(new IN("e.empid", "Select DISTINCT epp.empid from [EmpPayroll] epp where pp.PayPeriodID=epp.PayPeriodID", new DBFilter()));

        }
        else if (DecryptedRequest["YearEndBonus"] == "1")
        {
            //  Year End Bonus Only
            filter.add(new IN("Not e.empid", "Select DISTINCT epp.empid from [EmpPayroll] epp where pp.PayPeriodID=epp.PayPeriodID and epp.EmpPayIsYEB='Y'", new DBFilter()));
        }
        DBFilter resultFilter = new DBFilter();
        resultFilter.add(inTerm);

        resultFilter.add(new IN("e.EmpID", "Select DISTINCT EmpID From " + EEmpPersonalInfo.db.dbclass.tableName + " ee", EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime(), AppUtils.ServerDateTime())));

//        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(select, from, resultFilter, info);
        DataTable table = resultFilter.loadData(dbConn, null, select, from);
        table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);


        existingEmployeeView = new DataView(table);
        if (info != null)
        {
            //info.loadPageList(null, PrevPage, NextPage, FirstPage, LastPage);

            //CurPage.Value = info.page.ToString();
            //NumPage.Value = info.numPage.ToString();
        }
        if (repeater != null)
        {
            repeater.DataSource = existingEmployeeView;
            repeater.DataBind();
        }
        return existingEmployeeView;
    }

    public DataView LoadNewJoinData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = newJoinSearchBinding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = "e.* ";
        string from = "from EmpPersonalInfo e ";

        IN inTerm = new IN("e.EmpID", "Select DISTINCT epi.EmpID from EmpPositionInfo epi, PayrollPeriod pp ", filter);

        filter.add(new MatchField("e.EmpID", "epi.EmpID"));
        filter.add(new MatchField("epi.PayGroupID", "pp.PayGroupID"));
        filter.add(new MatchField("epi.EmpPosEffFr", "<=", "pp.PayPeriodTo"));
        filter.add(new Match("pp.PayPeriodID", CurID));
        filter.add(WebUtils.AddRankFilter(Session, "e.EmpID", true));

        OR orFilter = new OR();
        orFilter.add(new MatchField("epi.EmpPosEffTo", ">=", "pp.PayPeriodFr"));
        orFilter.add(new NullTerm("epi.EmpPosEffTo"));

        filter.add(orFilter);

        filter.add(new MatchField("e.EmpDateOfJoin", ">=", "pp.PayPeriodFr "));
        filter.add(new MatchField("e.EmpDateOfJoin", "<=", "pp.PayPeriodTo "));



        filter.add(new IN("Not e.empid", "Select DISTINCT empid from EmpPayroll  where EmpPayStatus='T'", new DBFilter()));
        filter.add(new IN("Not e.empid", "Select DISTINCT et.empid from EmpTermination et where et.EmpTermLastDate<=pp.PayPeriodTo", new DBFilter()));

        if (DecryptedRequest["RecurringPayment"] == "1")
        {
            filter.add(new IN("Not e.empid", "Select DISTINCT epp.empid from EmpPayroll epp where pp.PayPeriodID=epp.PayPeriodID and epp.EmpPayIsRP='Y'", new DBFilter()));
        }
        else if (DecryptedRequest["ClaimsAndDeduction"] == "1")
        {
            // Claims and Deductions only
            DBFilter CNDFilter = new DBFilter();
            CNDFilter.add(new MatchField("CNDEffDate", "<=", "pp.PayperiodTo"));
            CNDFilter.add(new MatchField("CNDEffDate", ">=", "epi.EmpPosEffFr"));

            OR orCNDPos = new OR();
            orCNDPos.add(new MatchField("CNDEffDate", "<=", "epi.EmpPosEffTo"));
            orCNDPos.add(new NullTerm("epi.EmpPosEffTo"));

            CNDFilter.add(orCNDPos);
            OR orCNDPayRecID = new OR();
            orCNDPayRecID.add(new Match("PayRecID", 0));
            orCNDPayRecID.add(new NullTerm("PayRecID"));
            CNDFilter.add(orCNDPayRecID);
            filter.add(new IN("e.empid", "Select DISTINCT empid from ClaimsAndDeductions ", CNDFilter));
        }
        else if (DecryptedRequest["AdditionalRemuneration"] == "1" && (DecryptedRequest["ClaimsAndDeduction"] != "1" && DecryptedRequest["RecurringPayment"] != "1"))
        {
            filter.add(new IN("e.empid", "Select DISTINCT epp.empid from EmpPayroll epp where pp.PayPeriodID=epp.PayPeriodID", new DBFilter()));

        }
        else if (DecryptedRequest["YearEndBonus"] == "1")
        {
            //  Year End Bonus Only
            filter.add(new IN("Not e.empid", "Select DISTINCT epp.empid from EmpPayroll epp where pp.PayPeriodID=epp.PayPeriodID and epp.EmpPayIsYEB='Y'", new DBFilter()));
        }

        DBFilter resultFilter = new DBFilter();
        resultFilter.add(inTerm);
        resultFilter.add(new IN("e.EmpID", "Select DISTINCT EmpID From " + EEmpPersonalInfo.db.dbclass.tableName + " ee", EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime(), AppUtils.ServerDateTime())));

        //DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(select, from, resultFilter, info);
        DataTable table = resultFilter.loadData(dbConn, null, select, from);
        table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);


        newJoinView = new DataView(table);
        if (info != null)
        {
            //info.loadPageList(null, PrevPage, NextPage, FirstPage, LastPage);

            //CurPage.Value = info.page.ToString();
            //NumPage.Value = info.numPage.ToString();
        }
        if (repeater != null)
        {
            repeater.DataSource = newJoinView;
            repeater.DataBind();
        }
        return newJoinView;
    }

    public DataView LoadFinalPaymentData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = finalPaymentSearchBinding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = "e.* ";
        string from = "from EmpPersonalInfo e ";

        IN inTerm = new IN("e.EmpID", "Select DISTINCT epi.EmpID from [EmpPositionInfo] epi, [PayrollPeriod] pp ", filter);

        filter.add(new MatchField("e.EmpID", "epi.EmpID"));
        filter.add(new MatchField("epi.PayGroupID", "pp.PayGroupID"));
        filter.add(new MatchField("epi.EmpPosEffFr", "<=", "pp.PayPeriodTo"));
        filter.add(new Match("pp.PayPeriodID", CurID));
        filter.add(WebUtils.AddRankFilter(Session, "e.EmpID", true));

        OR orFilter = new OR();
        orFilter.add(new MatchField("epi.EmpPosEffTo", ">=", "pp.PayPeriodFr"));
        orFilter.add(new NullTerm("epi.EmpPosEffTo"));

        filter.add(orFilter);

        filter.add(new IN("Not e.empid", "Select DISTINCT empid from EmpPayroll  where EmpPayStatus='T'", new DBFilter()));

        DBFilter empTerminationFilter = new DBFilter();
        if (DecryptedRequest["RecurringPayment"] == "1" || DecryptedRequest["YearEndBonus"] == "1")
            empTerminationFilter.add(new MatchField("et.EmpTermLastDate", ">=", "pp.PayPeriodFr"));
        empTerminationFilter.add(new MatchField("et.EmpTermLastDate", "<=", "pp.PayPeriodTo"));

        filter.add(new IN(" e.empid", "Select DISTINCT et.empid from EmpTermination et ", empTerminationFilter));

        if (DecryptedRequest["RecurringPayment"] == "1")
        {
            // PayRecType should be 'F'
            filter.add(new IN("Not e.empid", "Select DISTINCT epp.empid from EmpPayroll epp where pp.PayPeriodID=epp.PayPeriodID and epp.EmpPayIsRP='Y'", new DBFilter()));
        }
        else if (DecryptedRequest["ClaimsAndDeduction"] == "1")
        {
            // Claims and Deductions only
            DBFilter CNDFilter = new DBFilter();
            CNDFilter.add(new MatchField("CNDEffDate", "<=", "pp.PayperiodTo"));
            CNDFilter.add(new MatchField("CNDEffDate", ">=", "epi.EmpPosEffFr"));

            OR orCNDPos = new OR();
            orCNDPos.add(new MatchField("CNDEffDate", "<=", "epi.EmpPosEffTo"));
            orCNDPos.add(new NullTerm("epi.EmpPosEffTo"));

            CNDFilter.add(orCNDPos);
            OR orCNDPayRecID = new OR();
            orCNDPayRecID.add(new Match("PayRecID", 0));
            orCNDPayRecID.add(new NullTerm("PayRecID"));
            CNDFilter.add(orCNDPayRecID);
            filter.add(new IN("e.empid", "Select DISTINCT empid from ClaimsAndDeductions ", CNDFilter));
        }
        else if (DecryptedRequest["AdditionalRemuneration"] == "1" && (DecryptedRequest["ClaimsAndDeduction"] != "1" && DecryptedRequest["RecurringPayment"] != "1"))
        {
            filter.add(new IN("e.empid", "Select DISTINCT epp.empid from EmpPayroll epp where pp.PayPeriodID=epp.PayPeriodID", new DBFilter()));

        }
        else if (DecryptedRequest["YearEndBonus"] == "1")
        {
            //  Year End Bonus Only
            filter.add(new IN("Not e.empid", "Select DISTINCT epp.empid from EmpPayroll epp where pp.PayPeriodID=epp.PayPeriodID and epp.EmpPayIsYEB='Y'", new DBFilter()));
        }

        DBFilter resultFilter = new DBFilter();
        resultFilter.add(inTerm);
        resultFilter.add(new IN("e.EmpID", "Select DISTINCT EmpID From " + EEmpPersonalInfo.db.dbclass.tableName + " ee", EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime(), AppUtils.ServerDateTime())));

        //DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(select, from, resultFilter, info);
        DataTable table = resultFilter.loadData(dbConn, null, select, from);
        table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);


        newJoinView = new DataView(table);
        if (info != null)
        {
            //info.loadPageList(null, PrevPage, NextPage, FirstPage, LastPage);

            //CurPage.Value = info.page.ToString();
            //NumPage.Value = info.numPage.ToString();
        }
        if (repeater != null)
        {
            repeater.DataSource = newJoinView;
            repeater.DataBind();
        }
        return newJoinView;
    }

    protected void ExistingEmployeeChangeOrder_Click(object sender, EventArgs e)
    {
        LinkButton l = (LinkButton)sender;
        String id = l.ID.Substring("ExistingEmployee_".Length);
        if (existingEmployeeInfo.orderby == null)
            existingEmployeeInfo.order = true;
        else if (existingEmployeeInfo.orderby.Equals(id))
            existingEmployeeInfo.order = !existingEmployeeInfo.order;
        else
            existingEmployeeInfo.order = true;
        existingEmployeeInfo.orderby = id;

        existingEmployeeView = LoadExistingEmployeeData(existingEmployeeInfo, EEmpPersonalInfo.db, ExistingEmployeeRepeater);

    }
    protected void NewJoinChangeOrder_Click(object sender, EventArgs e)
    {
        LinkButton l = (LinkButton)sender;
        String id = l.ID.Substring("NewJoin_".Length);
        if (newJoinInfo.orderby == null)
            newJoinInfo.order = true;
        else if (newJoinInfo.orderby.Equals(id))
            newJoinInfo.order = !newJoinInfo.order;
        else
            newJoinInfo.order = true;
        newJoinInfo.orderby = id;

        newJoinView = LoadNewJoinData(newJoinInfo, EEmpPersonalInfo.db, NewJoinRepeater);
    }

    protected void FinalPaymentChangeOrder_Click(object sender, EventArgs e)
    {
        LinkButton l = (LinkButton)sender;
        String id = l.ID.Substring("FinalPayment_".Length);
        if (finalPaymentInfo.orderby == null)
            finalPaymentInfo.order = true;
        else if (finalPaymentInfo.orderby.Equals(id))
            finalPaymentInfo.order = !finalPaymentInfo.order;
        else
            finalPaymentInfo.order = true;
        finalPaymentInfo.orderby = id;

        finalPaymentView = LoadFinalPaymentData(finalPaymentInfo, EEmpPersonalInfo.db, FinalPaymentRepeater);
    }
    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(EEmpPersonalInfo.db, row, cb);
        cb.Visible = IsAllowEdit;
        cb.Checked = true;
    }
    protected void btnProcess_Click(object sender, EventArgs e)
    {
        string strEmpIDList=string.Empty ;
        ArrayList list = new ArrayList();
        foreach (RepeaterItem i in ExistingEmployeeRepeater.Items)
        {
            CheckBox cb = (CheckBox)i.FindControl("ItemSelect");
            if (cb.Checked)
            {
                BaseObject o = (BaseObject)EEmpPersonalInfo.db.createObject();
                WebFormUtils.GetKeys(EEmpPersonalInfo.db, o, cb);
                list.Add(o);
            }

        }
        foreach (RepeaterItem i in NewJoinRepeater.Items)
        {
            CheckBox cb = (CheckBox)i.FindControl("ItemSelect");
            if (cb.Checked)
            {
                BaseObject o = (BaseObject)EEmpPersonalInfo.db.createObject();
                WebFormUtils.GetKeys(EEmpPersonalInfo.db, o, cb);
                list.Add(o);
            }

        }
        foreach (RepeaterItem i in FinalPaymentRepeater.Items)
        {
            CheckBox cb = (CheckBox)i.FindControl("ItemSelect");
            if (cb.Checked)
            {
                BaseObject o = (BaseObject)EEmpPersonalInfo.db.createObject();
                WebFormUtils.GetKeys(EEmpPersonalInfo.db, o, cb);
                list.Add(o);
            }

        }
        if (list.Count > 0)
        {
            EPayrollPeriod payPeriod = new EPayrollPeriod();
            payPeriod.PayPeriodID=CurID;
            payPeriod.PayPeriodStatus = "T";
            payPeriod.PayPeriodTrialRunDate = AppUtils.ServerDateTime();
            payPeriod.PayPeriodTrialRunBy = WebUtils.GetCurUser(Session).UserID;
            EPayrollPeriod.db.update(dbConn, payPeriod);

            Session["PayrollTrialRun_EmpList"] = list;


            //foreach (BaseObject o in list)
            //{
            //    if (strEmpIDList == string.Empty)
            //        strEmpIDList = ((EEmpPersonalInfo)o).EmpID.ToString();
            //    else
            //        strEmpIDList += "_" + ((EEmpPersonalInfo)o).EmpID.ToString();

            //}
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_TrialRunProcess.aspx?"
                + "PayPeriodID=" + DecryptedRequest["PayPeriodID"]
                + "&RecurringPayment=" + DecryptedRequest["RecurringPayment"]
                + "&ClaimsAndDeduction=" + DecryptedRequest["ClaimsAndDeduction"]
                + "&AdditionalRemuneration=" + DecryptedRequest["AdditionalRemuneration"]
                + "&YearEndBonus=" + DecryptedRequest["YearEndBonus"]
                // Start 0000178, Miranda, 2015-03-14
                //+ "&SkipMPFCal=" + (chkSkipMPFCal.Checked ? "1" : "0")
                + "&SkipMPFCal=" + (chkSkipMPFCal.Visible ? (chkSkipMPFCal.Checked ? "1" : "0") : "0")
                // End 0000178, Miranda, 2015-03-14
                + "&Total=" + list.Count
                //                + "&EmpID=" + strEmpIDList
                );
        }
    }
    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_TrialRun.aspx?PayGroupID=" + PayGroupID.Value + "&PayPeriodID=" + PayPeriodID.Value);
    }
    protected void Search_Click(object sender, EventArgs e)
    {
        loadData();

    }
    protected void Reset_Click(object sender, EventArgs e)
    {
        EmployeeSearchControl1.Reset();
        loadData();
    }
}
