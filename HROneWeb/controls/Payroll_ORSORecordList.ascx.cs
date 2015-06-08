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

public partial class Payroll_ORSORecordList : HROneWebControl
{
    protected DBManager db = EORSORecord.db;
    protected SearchBinding sBinding;
    protected ListInfo info;
    protected DataView view;
    public Binding newBinding;
    private double dblTotalORSORI = 0;
    private double dblTotalORSOER = 0;
    private double dblTotalORSOEE = 0;

    public bool IsAllowEdit = true;
    private bool m_IsTrialMode = true;
    

    private int CurID = 0;
    private string m_EmpPayStatus = "";

    public event EventHandler Recalculate;

    public int CurrentEmpPayrollID
    {
        get { return CurID; }
        set
        {
            CurID = value;
            EmpPayrollID.Value = CurID.ToString();
        }
    }

    public string CurrentEmpPayStatus
    {
        get { return m_EmpPayStatus; }
        set
        {
            m_EmpPayStatus = value;
            EmpPayStatus.Value = m_EmpPayStatus;
        }
    }

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

        sBinding = new SearchBinding(dbConn, db);
        sBinding.add(new HiddenMatchBinder(EmpPayrollID));
        sBinding.initValues("ORSOPlanID", null, EORSOPlan.VLORSOPlan, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));

        sBinding.init(DecryptedRequest, null);

        newBinding = new Binding(dbConn, db);
        newBinding.add(EmpPayrollID);
        newBinding.add(new TextBoxBinder(db, ORSORecPeriodFr.TextBox, ORSORecPeriodFr.ID));
        newBinding.add(new TextBoxBinder(db, ORSORecPeriodTo.TextBox, ORSORecPeriodTo.ID));
        newBinding.add(ORSORecActRI);
        newBinding.add(ORSORecActEE);
        newBinding.add(ORSORecActER);
        newBinding.add(new DropDownVLBinder(db, ORSOPlanID, EORSOPlan.VLORSOPlan));

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

        if (IsTrialMode)
        {
            if (!IsAllowEdit)
            {
                Delete.Visible = false;
                AddPanel.Visible = false;
            }

        }
        else
        {
            if (!IsAllowEdit)
            {
                AddPanel.Visible = false;
                Delete.Visible = false;
            }
        }
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

        ListFooter.Refresh();

        view = new DataView(table);
        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();

            lblORSORITotal.Text = dblTotalORSORI.ToString("$#,##0.00");
            lblORSOERTotal.Text = dblTotalORSOER.ToString("$#,##0.00");
            lblORSOEETotal.Text = dblTotalORSOEE.ToString("$#,##0.00");
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
        EORSORecord c = new EORSORecord();

        Hashtable values = new Hashtable();
        newBinding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);
        c.ORSORecType = "A";

        EEmpPayroll empPayroll = new EEmpPayroll();
        empPayroll.EmpPayrollID = c.EmpPayrollID;
        if (EEmpPayroll.db.select(dbConn, empPayroll))
        {
            EPayrollPeriod payPeriod = new EPayrollPeriod();
            payPeriod.PayPeriodID = empPayroll.PayPeriodID;

            if (EPayrollPeriod.db.select(dbConn, payPeriod))
                if (c.ORSORecPeriodFr <= payPeriod.PayPeriodTo && c.ORSORecPeriodTo <= payPeriod.PayPeriodTo)
                {
                    WebUtils.StartFunction(Session, m_FunctionCode, empPayroll.EmpID);
                    db.insert(dbConn, c);
                    WebUtils.EndFunction(dbConn);
                }
                else
                {
                    errors.addError(HROne.Translation.PageErrorMessage.ERROR_INVALID_PERIOD);
                    return;
                }
        }
        ORSORecPeriodFr.TextBox.Text = string.Empty;
        ORSORecPeriodTo.TextBox.Text = string.Empty;
        ORSORecActRI.Text = string.Empty;
        ORSORecActER.Text = string.Empty;
        ORSORecActEE.Text = string.Empty;
        ORSOPlanID.SelectedIndex = 0;

        view = loadData(info, db, Repeater);
        //Response.Redirect(Request.Url.LocalPath + "?" + Request.QueryString);
    }

    protected void Repeater_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        EORSORecord obj = new EORSORecord();
        db.toObject(((DataRowView)e.Item.DataItem).Row, obj);
        Hashtable values = new Hashtable();
        db.populate(obj, values);
        dblTotalORSORI += obj.ORSORecActRI;
        dblTotalORSOER += obj.ORSORecActER;
        dblTotalORSOEE += obj.ORSORecActEE;

        if (e.Item.ItemIndex == Repeater.EditItemIndex)
        {
            Binding eORSOBinding;
            eORSOBinding = new Binding(dbConn, db);
            eORSOBinding.add((HtmlInputHidden)e.Item.FindControl("ORSORecordID"));
            eORSOBinding.add(new TextBoxBinder(db, ((WebDatePicker)e.Item.FindControl("ORSORecPeriodFr")).TextBox, "ORSORecPeriodFr"));
            eORSOBinding.add(new TextBoxBinder(db, ((WebDatePicker)e.Item.FindControl("ORSORecPeriodTo")).TextBox, "ORSORecPeriodTo"));
            eORSOBinding.add((TextBox)e.Item.FindControl("ORSORecActRI"));
            eORSOBinding.add((TextBox)e.Item.FindControl("ORSORecActER"));
            eORSOBinding.add((TextBox)e.Item.FindControl("ORSORecActEE"));
            eORSOBinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("ORSOPlanID"), EORSOPlan.VLORSOPlan));



            eORSOBinding.init(Request, Session);

            eORSOBinding.toControl(values);
        }
        else
        {
            if (EmpPayStatus.Value == "C" && obj.ORSORecType != "A")
            {
                e.Item.FindControl("Edit").Visible = false;
                e.Item.FindControl("DeleteItem").Visible = false;
            }
            else if (obj.ORSORecType != "A")
            {
                e.Item.FindControl("Edit").Visible = true & IsAllowEdit;
                e.Item.FindControl("DeleteItem").Visible = true;
            }
            else
            {
                e.Item.FindControl("Edit").Visible = true & IsAllowEdit;
                e.Item.FindControl("DeleteItem").Visible = true & IsAllowEdit;
            }

            HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("ORSORecordID");
            h.Value = ((DataRowView)e.Item.DataItem)["ORSORecordID"].ToString();
        }
        HROne.Common.WebUtility.WebControlsLocalization(Session, e.Item.Controls);
    }

    protected void Repeater_ItemCommand(object source, DataListCommandEventArgs e)
    {
        Button b = (Button)e.CommandSource;




        if (b.ID.Equals("Edit"))
        {
            Repeater.EditItemIndex = e.Item.ItemIndex;
            //AddPanel.Visible = false;
            view = loadData(info, db, Repeater);
            WebUtils.SetEnabledControlSection(AddPanel, false);
        }
        else if (b.ID.Equals("Cancel"))
        {
            Repeater.EditItemIndex = -1;
            //AddPanel.Visible = IsAllowEdit;
            view = loadData(info, db, Repeater);
            WebUtils.SetEnabledControlSection(AddPanel, true);
        }
        else if (b.ID.Equals("Save"))
        {
            Binding eBinding;
            eBinding = new Binding(dbConn, db);
            eBinding.add(EmpPayrollID);
            eBinding.add((HtmlInputHidden)e.Item.FindControl("ORSORecordID"));
            eBinding.add(new TextBoxBinder(db, ((WebDatePicker)e.Item.FindControl("ORSORecPeriodFr")).TextBox, "ORSORecPeriodFr"));
            eBinding.add(new TextBoxBinder(db, ((WebDatePicker)e.Item.FindControl("ORSORecPeriodTo")).TextBox, "ORSORecPeriodTo"));
            eBinding.add((TextBox)e.Item.FindControl("ORSORecActRI"));
            eBinding.add((TextBox)e.Item.FindControl("ORSORecActER"));
            eBinding.add((TextBox)e.Item.FindControl("ORSORecActEE"));
            eBinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("ORSOPlanID"), EORSOPlan.VLORSOPlan));


            eBinding.init(Request, Session);


            EORSORecord obj = new EORSORecord();
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

            EEmpPayroll empPayroll = new EEmpPayroll();
            empPayroll.EmpPayrollID = obj.EmpPayrollID;
            if (EEmpPayroll.db.select(dbConn, empPayroll))
            {
                EPayrollPeriod payPeriod = new EPayrollPeriod();
                payPeriod.PayPeriodID = empPayroll.PayPeriodID;

                if (EPayrollPeriod.db.select(dbConn, payPeriod))
                    if (obj.ORSORecPeriodFr <= payPeriod.PayPeriodTo && obj.ORSORecPeriodTo <= payPeriod.PayPeriodTo)
                    {
                        WebUtils.StartFunction(Session, m_FunctionCode, empPayroll.EmpID);
                        db.update(dbConn, obj);
                        WebUtils.EndFunction(dbConn);
                    }
                    else
                    {
                        errors.addError(HROne.Translation.PageErrorMessage.ERROR_INVALID_PERIOD);
                        return;
                    }
            }
            Repeater.EditItemIndex = -1;
            //AddPanel.Visible = IsAllowEdit;
            view = loadData(info, db, Repeater);
            WebUtils.SetEnabledControlSection(AddPanel, true);
        }


    }

    protected void Delete_Click(object sender, EventArgs e)
    {

        ArrayList list = new ArrayList();
        foreach (DataListItem item in Repeater.Items)
        {
            CheckBox c = (CheckBox)item.FindControl("DeleteItem");
            HtmlInputHidden h = (HtmlInputHidden)item.FindControl("ORSORecordID");
            if (c.Checked)
            {
                EORSORecord obj = new EORSORecord();
                obj.ORSORecordID = Int32.Parse(h.Value);
                list.Add(obj);
            }
        }

        EEmpPayroll empPayroll = new EEmpPayroll();
        empPayroll.EmpPayrollID = int.Parse(EmpPayrollID.Value);
        if (EEmpPayroll.db.select(dbConn, empPayroll))
        {
            WebUtils.StartFunction(Session, m_FunctionCode, empPayroll.EmpID);
            foreach (EORSORecord obj in list)
                if (db.select(dbConn, obj))
                    db.delete(dbConn, obj);
            WebUtils.EndFunction(dbConn);
        }
        view = loadData(info, db, Repeater);
    }

    // Start 0000162, Ricky So, 2015-01-25
    protected void Recalculate_Click(object sender, EventArgs e)
    {
        EEmpPayroll empPayroll = new EEmpPayroll();
        empPayroll.EmpPayrollID = CurrentEmpPayrollID;
        if (EEmpPayroll.db.select(dbConn, empPayroll))
        {
            WebUtils.StartFunction(Session, m_FunctionCode, empPayroll.EmpID);
            HROne.Payroll.ORSOProcess.Recalculate(dbConn, empPayroll);
            
            WebUtils.EndFunction(dbConn);
        }
        view = loadData(info, db, Repeater);
        Recalculate(this, EventArgs.Empty);
    }
    // End 0000162, Ricky So, 2015-01-25
}
