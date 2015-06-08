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
using HROne.DataAccess;
using HROne.Lib.Entities;

public partial class Emp_RecurringPayment_List : HROneWebControl
{
    private const string FUNCTION_CODE = "PER007-1";
    protected SearchBinding sbinding;
    public DBManager sdb = EEmpRecurringPayment.db;
    protected ListInfo info;
    protected DataView view;
    //protected bool m_ShowHistory = false;
    protected bool m_AllowModify = true;
    protected bool m_ShowPayrollItem = true;
    protected bool m_ShowNonPayrollItem = false;
    

    public int CurrentEmpID
    {
        set { EmpID.Value = value.ToString(); }
        get
        {
            int tmpID;
            if (int.TryParse(EmpID.Value, out tmpID))
            {
                return tmpID;
            }
            else
                return 0;
        }
    }

    public int CurrentPayCodeID
    {
        set { PayCodeID.Value = value.ToString(); }
        get
        {
            int tmpID;
            if (int.TryParse(PayCodeID.Value, out tmpID))
            {
                return tmpID;
            }
            else
                return 0;
        }
    }

    public bool ShowHistory
    {
        set { ShowHistoryFlag.Value = (value ? "True" : "False"); }
        get { return  (ShowHistoryFlag.Value.Equals("True", StringComparison.CurrentCultureIgnoreCase)); }
    }

    public bool AllowModify
    {
        set { m_AllowModify = value; }
        get { return m_AllowModify; }
    }

    public bool ShowPayrollItem
    {
        set { m_ShowPayrollItem = value; }
        get { return m_ShowPayrollItem; }
    }

    public bool ShowNonPayrollItem
    {
        set { m_ShowNonPayrollItem = value; }
        get { return m_ShowNonPayrollItem; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            this.Visible = false;
        toolBar.FunctionCode = FUNCTION_CODE;
        toolBar.NewButton_Visible = toolBar.NewButton_Visible && m_AllowModify;
        toolBar.DeleteButton_Visible = toolBar.DeleteButton_Visible && m_AllowModify;
        SelectAllPanel.Visible = toolBar.DeleteButton_Visible;

        PreRender += new EventHandler(Emp_RecurringPayment_List_PreRender);


        sbinding = new SearchBinding(dbConn, sdb);
        sbinding.initValues("PayCodeID", null, EPaymentCode.VLPaymentCode, null);
        sbinding.initValues("EmpRPUnit", null, Values.VLPaymentUnit, null);
        sbinding.initValues("EmpRPMethod", null, Values.VLPaymentMethod, null);
        sbinding.add(new HiddenMatchBinder(EmpID, "erp.EmpID"));
        sbinding.add(new HiddenMatchSearchBinder(PayCodeID, "erp.PayCodeID"));
        sbinding.initValues("CostCenterID", null, ECostCenter.VLCostCenter, null);
        sbinding.init(DecryptedRequest, null);
        

        //try
        //{
        //    CurID = Int32.Parse(DecryptedRequest["EmpID"]);
        //    EmpID.Value = CurID.ToString();
        //}
        //catch (Exception ex)
        //{
        //}
        info = ListFooter.ListInfo;
        //if (!IsPostBack)
        //{
        //    ListFooter.ListOrderBy = "";
        //    ListFooter.ListOrder = false;
        //}
        CostCenterHeaderCell.Visible = WebUtils.productLicense(Session).IsCostCenter;
        FooterCell.ColSpan = HeaderRow.Cells.Count - 1 - (WebUtils.productLicense(Session).IsCostCenter ? 0 : 1);
    }

    void Emp_RecurringPayment_List_PreRender(object sender, EventArgs e)
    {
        if (view == null)
        {
            view = loadData(info, sdb, Repeater);
        }

        if (ShowHistory)
        {
            btnShowAll.Visible = false;
            btnShowLatestOnly.Visible = true;
        }
        else
        {
            btnShowAll.Visible = true;
            btnShowLatestOnly.Visible = false;
        }


    }
    public void Refresh()
    {
        view = loadData(info, sdb, Repeater);
    }
    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = sbinding.createFilter();

        DateTime serverDate = AppUtils.ServerDateTime().Date;

        if (!ShowHistory)
        {
            OR orEmpRPTerms = new OR();
            {
                AND andEmpRPEffRangeTerm = new AND();
                //andEmpRPEffRangeTerm.add(new Match("erp.EmpRPEffFr", "<=", serverDate));
                {
                    OR orEmpRpEffToTerm = new OR();
                    orEmpRpEffToTerm.add(new Match("erp.EmpRPEffTo", ">=", serverDate));
                    orEmpRpEffToTerm.add(new NullTerm("erp.EmpRPEffTo"));
                    andEmpRPEffRangeTerm.add(orEmpRpEffToTerm);
                }

                orEmpRPTerms.add(andEmpRPEffRangeTerm);
            }
            {
                DBFilter maxEffFilter = new DBFilter();
                orEmpRPTerms.add(new Exists(db.dbclass.tableName + " tmperp GROUP BY tmperp.paycodeid, tmperp.empid having max(tmperp.emprpefffr)=erp.emprpefffr and tmperp.paycodeid=erp.paycodeid and tmperp.empid=erp.empid", maxEffFilter));
            }
            filter.add(orEmpRPTerms);
        }

        OR orEmpRPIsNonPayrollItem = new OR();
        if (m_ShowPayrollItem)
        {
            orEmpRPIsNonPayrollItem.add(new Match("EmpRPIsNonPayrollItem", false));
            orEmpRPIsNonPayrollItem.add(new NullTerm("EmpRPIsNonPayrollItem"));
        }
        if (m_ShowNonPayrollItem)
            orEmpRPIsNonPayrollItem.add(new Match("EmpRPIsNonPayrollItem", true));
        filter.add(orEmpRPIsNonPayrollItem);

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);
        //filter.add("erp.paycodeID", true);

        string select = "erp.*, pc.PaymentCode, cc.CostCenterCode ";
        string from = "from " + db.dbclass.tableName + " erp "
            + " LEFT JOIN " + ECostCenter.db.dbclass.tableName + " cc on erp.CostCenterID=cc.CostCenterID "
            + " LEFT JOIN " + EPaymentCode.db.dbclass.tableName + " pc on erp.PayCodeID=pc.PaymentCodeID ";
        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);

        view = new DataView(table);
        ListFooter.Refresh();
        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }

        return view;
    }

    protected void FirstPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, sdb, Repeater);
    }
    protected void PrevPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, sdb, Repeater);
    }
    protected void NextPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, sdb, Repeater);
    }
    protected void LastPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, sdb, Repeater);
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

        view = loadData(info, sdb, Repeater);

    }
    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(sdb, row, cb);
        e.Item.FindControl("ItemSelect").Visible = toolBar.DeleteButton_Visible;

        EEmpRecurringPayment obj = new EEmpRecurringPayment();
        sdb.toObject(((DataRowView)e.Item.DataItem).Row, obj);
        Hashtable values = new Hashtable();
        sdb.populate(obj, values);

        Binding eBinding = new Binding(dbConn, sdb);
        eBinding.add(new BlankZeroLabelVLBinder(sdb, (Label)e.Item.FindControl("CostCenterID"), ECostCenter.VLCostCenter).setTextDisplayForZero(HROne.Common.WebUtility.GetLocalizedString(ECostCenter.DEFAULT_COST_CENTER_TEXT)));

        eBinding.init(Request, Session);

        eBinding.toControl(values);

        ((HtmlTableCell)e.Item.FindControl("CostCenterDetailCell")).Visible = WebUtils.productLicense(Session).IsCostCenter;
        //((HtmlTableCell)e.Item.FindControl("RemarkCell")).ColSpan = ((HtmlTableRow)e.Item.FindControl("detailRow")).Cells.Count - 1 - (WebUtils.productLicense(Session).IsCostCenter ? 0 : 1);

    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        ArrayList list = new ArrayList();
        foreach (RepeaterItem i in Repeater.Items)
        {
            CheckBox cb = (CheckBox)i.FindControl("ItemSelect");
            if (cb.Checked)
            {
                EEmpRecurringPayment o = new EEmpRecurringPayment();
                WebFormUtils.GetKeys(sdb, o, cb);
                list.Add(o);
            }

        }
        if (list.Count > 0)
        {
            WebUtils.StartFunction(Session, FUNCTION_CODE, CurrentEmpID);
            foreach (BaseObject o in list)
            {
                if (sdb.select(dbConn, o))
                    sdb.delete(dbConn, o);
            }
            WebUtils.EndFunction(dbConn);
        }
        loadData(info, sdb, Repeater);
    }
    public void New_Click(object sender, EventArgs e)
    {

        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Emp_RecurringPayment_Edit.aspx?EmpID=" + EmpID.Value + (ShowNonPayrollItem.Equals(true) && ShowPayrollItem.Equals(false) ? "&NonPayrollItem=Y" : string.Empty));
    }
    protected void btnShowAll_Click(object sender, EventArgs e)
    {
        ShowHistory = true;
    }
    protected void btnShowLatestOnly_Click(object sender, EventArgs e)
    {
        ShowHistory = false;

    }
}
