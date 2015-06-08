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
//using perspectivemind.validation;
using HROne.Lib.Entities;

public partial class Payroll_CostAllocationList : HROneWebControl
{
    protected DBManager db = ECostAllocationDetail.db;
    protected SearchBinding sBinding;
    protected ListInfo info;
    protected DataView view;
    public Binding newBinding;
    private double dblTotalAmount = 0;

    public bool IsAllowEdit = true;
    private bool m_IsTrialMode;
    

    private int CurID = 0;
    //private string m_EmpPayStatus = "";
    

    public int CurrentCostAllocationID
    {
        get { return CurID; }
        set
        {
            CurID = value;
            CostAllocationID.Value = CurID.ToString();
        }
    }

    //public string CurrentEmpPayStatus
    //{
    //    get { return m_EmpPayStatus; }
    //    set
    //    {
    //        m_EmpPayStatus = value;
    //        EmpPayStatus.Value = m_EmpPayStatus;
    //    }
    //}

    public bool IsTrialMode
    {
        get { return m_IsTrialMode; }
        set
        {
            m_IsTrialMode = value;
        }
    }

    private string m_FunctionCode;
    public string FunctionCode
    {
        get { return m_FunctionCode; }
        set { m_FunctionCode = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        Delete.OnClientClick = HROne.Translation.PromptMessage.CreateDeleteConfirmDialogJavascript(Delete);

        dblTotalAmount = 0;

        sBinding = new SearchBinding(dbConn, db);
        sBinding.add(new HiddenMatchBinder(CostAllocationID));
        sBinding.initValues("PaymentCodeID", null, EPaymentCode.VLPaymentCode, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        sBinding.initValues("CostCenterID", null, ECostCenter.VLCostCenter , HROne.Common.WebUtility.GetSessionUICultureInfo(Session));

        sBinding.init(DecryptedRequest, null);

        newBinding = new Binding(dbConn, db);
        newBinding.add(CostAllocationID);
        newBinding.add(new DropDownVLBinder(db, PaymentCodeID, EPaymentCode.VLPaymentCode));
        newBinding.add(new DropDownVLBinder(db, CostCenterID, EPaymentCode.VLPaymentCode));
        newBinding.add(CostAllocationDetailAmount);


        newBinding.init(Request, Session);

        info = ListFooter.ListInfo;
    }


    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (CurID > 0)
                //if (loadObject())
                //{
                    view = loadData(info, db, Repeater);
                //}
        }

        lblCostAllocationTotal.Text = dblTotalAmount.ToString("$#,##0.00");

        AddPanel.Visible = false;
        Delete.Visible = false;
    }


    public DataView loadData(ListInfo info, DBManager db, DataList repeater)
    {
        DBFilter filter = sBinding.createFilter();
        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);

        string select = "c.*";
        string from = "from " + db.dbclass.tableName + " c ";

        //filter.add(new Match("EmpPayrollID",EmpPayrollID.Value));
        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);
        //HROne.Common.DBAESEncryptDoubleFieldAttribute.decode(table, "PayRecActAmountEnc", "PayRecActAmount");
        //HROne.Common.DBAESEncryptDoubleFieldAttribute.decode(table, "PayRecCalAmountEnc", "PayRecCalAmount");

        ListFooter.Refresh();
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

        Repeater.EditItemIndex = -1;
        view = loadData(info, db, Repeater);

    }
    protected void Add_Click(object sender, EventArgs e)
    {
        Repeater.EditItemIndex = -1;
        ECostAllocationDetail c = new ECostAllocationDetail();

        Hashtable values = new Hashtable();
        newBinding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);

        if (!errors.isEmpty())
            return;


        ECostAllocation costAllocation = new ECostAllocation();
        costAllocation.CostAllocationID = c.CostAllocationID;
        if (ECostAllocation.db.select(dbConn, costAllocation))
        {
            WebUtils.StartFunction(Session, m_FunctionCode, costAllocation.EmpID);
            db.insert(dbConn, c);
            WebUtils.EndFunction(dbConn);
        }

        view = loadData(info, db, Repeater);

    }

    protected void Repeater_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        ECostAllocationDetail obj = new ECostAllocationDetail();
        db.toObject(((DataRowView)e.Item.DataItem).Row, obj);
        Hashtable values = new Hashtable();
        db.populate(obj, values);
        dblTotalAmount += obj.CostAllocationDetailAmount;

        if (e.Item.ItemIndex == Repeater.EditItemIndex)
        {
            Binding eBinding;
            eBinding = new Binding(dbConn, db);
            eBinding.add(CostAllocationID);
            eBinding.add((HtmlInputHidden)e.Item.FindControl("CostAllocationDetailID"));
            eBinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("PaymentCodeID"), EPaymentCode.VLPaymentCode));
            eBinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("CostCenterID"), ECostCenter.VLCostCenter));
            eBinding.add((TextBox)e.Item.FindControl("CostAllocationDetailAmount"));




            eBinding.init(Request, Session);

            eBinding.toControl(values);
        }
        else
        {
            e.Item.FindControl("Edit").Visible = true & IsAllowEdit;
            //if (EmpPayStatus.Value == "C" && obj.PayRecType != "A")
            //{
            //    e.Item.FindControl("Edit").Visible = false & IsAllowEdit;
            //    e.Item.FindControl("DeleteItem").Visible = false;
            //}
            //else
            //{
            //    e.Item.FindControl("Edit").Visible = true & IsAllowEdit;
            //    e.Item.FindControl("DeleteItem").Visible = true & IsAllowEdit;
            //}
            HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("CostAllocationDetailID");
            h.Value = ((DataRowView)e.Item.DataItem)["CostAllocationDetailID"].ToString();
        }
        e.Item.FindControl("DeleteItem").Visible = false;// true & IsAllowEdit;
        HROne.Common.WebUtility.WebControlsLocalization(Session, e.Item.Controls);
    }

    protected void Repeater_ItemCommand(object source, DataListCommandEventArgs e)
    {
        Button b = (Button)e.CommandSource;




        if (b.ID.Equals("Edit"))
        {
            Repeater.EditItemIndex = e.Item.ItemIndex;
            AddPanel.Visible = false;
            view = loadData(info, db, Repeater);
        }
        else if (b.ID.Equals("Cancel"))
        {
            Repeater.EditItemIndex = -1;
            AddPanel.Visible = IsAllowEdit;
            view = loadData(info, db, Repeater);
        }
        else if (b.ID.Equals("Save"))
        {
            Binding eBinding;

            eBinding = new Binding(dbConn, db);
            eBinding.add(CostAllocationID);
            eBinding.add((HtmlInputHidden)e.Item.FindControl("CostAllocationDetailID"));
            eBinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("PaymentCodeID"), EPaymentCode.VLPaymentCode));
            eBinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("CostCenterID"), ECostCenter.VLCostCenter));
            eBinding.add((TextBox)e.Item.FindControl("CostAllocationDetailAmount"));

            eBinding.init(Request, Session);


            ECostAllocationDetail obj = new ECostAllocationDetail();
            Hashtable values = new Hashtable();

            PageErrors errors = PageErrors.getErrors(db, Page.Master);
            errors.clear();


            eBinding.toValues(values);
            db.validate(errors, values);

            if (!errors.isEmpty())
            {
                return;
            }

            db.parse(values, obj);

            ECostAllocation costAllocation = new ECostAllocation();
            costAllocation.CostAllocationID = obj.CostAllocationID;
            if (ECostAllocation.db.select(dbConn, costAllocation))
            {
                WebUtils.StartFunction(Session, m_FunctionCode, costAllocation.EmpID);
                db.update(dbConn, obj);
                WebUtils.EndFunction(dbConn);
            }
            Repeater.EditItemIndex = -1;
            AddPanel.Visible = IsAllowEdit;
            view = loadData(info, db, Repeater);

        }


    }

    protected void Delete_Click(object sender, EventArgs e)
    {

        ArrayList list = new ArrayList();
        foreach (DataListItem item in Repeater.Items)
        {
            CheckBox c = (CheckBox)item.FindControl("DeleteItem");
            HtmlInputHidden h = (HtmlInputHidden)item.FindControl("PayRecID");
            if (c.Checked)
            {
                ECostAllocationDetail obj = new ECostAllocationDetail();
                obj.PayRecID = Int32.Parse(h.Value);
                list.Add(obj);
            }
        }
        ECostAllocation costAllocation = new ECostAllocation();
        costAllocation.CostAllocationID = int.Parse(CostAllocationID.Value);
        if (ECostAllocation.db.select(dbConn, costAllocation))
        {
            WebUtils.StartFunction(Session, m_FunctionCode, costAllocation.EmpID);
            foreach (ECostAllocationDetail obj in list)
            {
                db.delete(dbConn, obj);
            }
            WebUtils.EndFunction(dbConn);
        }
        view = loadData(info, db, Repeater);

    }
}
