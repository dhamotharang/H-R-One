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

public partial class Payroll_MPFRecordList : HROneWebControl
{
    protected DBManager db = EMPFRecord.db;
    protected SearchBinding sBinding;
    protected ListInfo info;
    protected DataView view;
    public Binding newBinding;
    private double dblTotalMPFMCRI = 0;
    private double dblTotalMPFMCER = 0;
    private double dblTotalMPFMCEE = 0;
    private double dblTotalMPFVCER = 0;
    private double dblTotalMPFVCEE = 0;

    public bool IsAllowEdit = true;
    private bool m_IsTrialMode = true;
    

    private int CurID = 0;
    private string m_EmpPayStatus = "";
    

    public event EventHandler Recalculate;
    public int CurrentEmpPayrollID
    {
        get 
        {
            if (string.IsNullOrEmpty(EmpPayrollID.Value))
                return 0;
            else

                return int.Parse(EmpPayrollID.Value); 
        }
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
        sBinding.initValues("MPFPlanID", null, EMPFPlan.VLMPFPlan, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));

        sBinding.init(DecryptedRequest, null);

        newBinding = new Binding(dbConn, db);
        newBinding.add(EmpPayrollID);
        newBinding.add(new TextBoxBinder(db, MPFRecPeriodFr.TextBox, MPFRecPeriodFr.ID));
        newBinding.add(new TextBoxBinder(db, MPFRecPeriodTo.TextBox, MPFRecPeriodTo.ID));
        newBinding.add(MPFRecActMCRI);
        newBinding.add(MPFRecActMCEE);
        newBinding.add(MPFRecActMCER);
        newBinding.add(MPFRecActVCEE);
        newBinding.add(MPFRecActVCER);
        newBinding.add(new DropDownVLBinder(db, MPFPlanID, EMPFPlan.VLMPFPlan));

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
                btnRecalculate.Visible = false;
            }

        }
        else
        {
            if (!IsAllowEdit)
            {
                AddPanel.Visible = false;
                Delete.Visible = false;
                btnRecalculate.Visible = false;
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

            lblMPFMCRITotal.Text = dblTotalMPFMCRI.ToString("$#,##0.00");
            lblMPFMCERTotal.Text = dblTotalMPFMCER.ToString("$#,##0.00");
            lblMPFMCEETotal.Text = dblTotalMPFMCEE.ToString("$#,##0.00");
            lblMPFVCERTotal.Text = dblTotalMPFVCER.ToString("$#,##0.00");
            lblMPFVCEETotal.Text = dblTotalMPFVCEE.ToString("$#,##0.00");
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
        EMPFRecord c = new EMPFRecord();

        Hashtable values = new Hashtable();
        newBinding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);
        c.AVCPlanID = 0;
        c.MPFRecType = "A";
        EEmpPayroll empPayroll = new EEmpPayroll();
        empPayroll.EmpPayrollID = c.EmpPayrollID;
        if (EEmpPayroll.db.select(dbConn, empPayroll))
        {
            EPayrollPeriod payPeriod = new EPayrollPeriod();
            payPeriod.PayPeriodID = empPayroll.PayPeriodID;

            if (EPayrollPeriod.db.select(dbConn, payPeriod))
                if (c.MPFRecPeriodFr <= payPeriod.PayPeriodTo && c.MPFRecPeriodTo <= payPeriod.PayPeriodTo)
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

        MPFRecPeriodFr.TextBox.Text = string.Empty;
        MPFRecPeriodTo.TextBox.Text = string.Empty;
        MPFRecActMCRI.Text = string.Empty;
        MPFRecActMCEE.Text = string.Empty;
        MPFRecActMCER.Text = string.Empty;
        MPFRecActVCEE.Text = string.Empty;
        MPFRecActVCER.Text = string.Empty;
        MPFPlanID.SelectedIndex = 0;

        view = loadData(info, db, Repeater);
        //Response.Redirect(Request.Url.LocalPath + "?" + Request.QueryString);
    }

    protected void Repeater_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        EMPFRecord obj = new EMPFRecord();
        db.toObject(((DataRowView)e.Item.DataItem).Row, obj);
        Hashtable values = new Hashtable();
        db.populate(obj, values);
        dblTotalMPFMCRI += obj.MPFRecActMCRI;
        dblTotalMPFMCER += obj.MPFRecActMCER;
        dblTotalMPFMCEE += obj.MPFRecActMCEE;
        dblTotalMPFVCER += obj.MPFRecActVCER;
        dblTotalMPFVCEE += obj.MPFRecActVCEE;

        if (e.Item.ItemIndex == Repeater.EditItemIndex)
        {
            Binding eBinding;
            eBinding = new Binding(dbConn, db);
            eBinding.add((HtmlInputHidden)e.Item.FindControl("MPFRecordID"));
            eBinding.add(new TextBoxBinder(db, ((WebDatePicker)e.Item.FindControl("MPFRecPeriodFr")).TextBox, "MPFRecPeriodFr"));
            eBinding.add(new TextBoxBinder(db, ((WebDatePicker)e.Item.FindControl("MPFRecPeriodTo")).TextBox, "MPFRecPeriodTo"));
            eBinding.add((TextBox)e.Item.FindControl("MPFRecActMCRI"));
            eBinding.add((TextBox)e.Item.FindControl("MPFRecActMCER"));
            eBinding.add((TextBox)e.Item.FindControl("MPFRecActMCEE"));
            eBinding.add((TextBox)e.Item.FindControl("MPFRecActVCER"));
            eBinding.add((TextBox)e.Item.FindControl("MPFRecActVCEE"));
            eBinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("MPFPlanID"), EMPFPlan.VLMPFPlan));



            eBinding.init(Request, Session);

            eBinding.toControl(values);
            if (EmpPayStatus.Value == "C" && obj.MPFRecType != "A")
            {
                ((WebDatePicker)e.Item.FindControl("MPFRecPeriodFr")).Enabled = false; ;
                ((WebDatePicker)e.Item.FindControl("MPFRecPeriodTo")).Enabled = false; ;
                ((TextBox)e.Item.FindControl("MPFRecActMCRI")).Enabled = false;
                ((TextBox)e.Item.FindControl("MPFRecActMCER")).Enabled = false;
                ((TextBox)e.Item.FindControl("MPFRecActMCEE")).Enabled = false;
                ((TextBox)e.Item.FindControl("MPFRecActVCER")).Enabled = false;
                ((TextBox)e.Item.FindControl("MPFRecActVCEE")).Enabled = false;
                e.Item.FindControl("DeleteItem").Visible = false;
            }
        }
        else
        {
            e.Item.FindControl("Edit").Visible = IsAllowEdit;

            if (EmpPayStatus.Value == "C" && obj.MPFRecType != "A")
            {
                //e.Item.FindControl("Edit").Visible = false;
                e.Item.FindControl("DeleteItem").Visible = false;
            }
            //else if (obj.MPFRecType != "A")
            //{
            //    e.Item.FindControl("Edit").Visible = true & IsAllowEdit;
            //    e.Item.FindControl("DeleteItem").Visible = true;
            //}
            else
            {
                e.Item.FindControl("DeleteItem").Visible = true & IsAllowEdit;
            }

            HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("MPFRecordID");
            h.Value = ((DataRowView)e.Item.DataItem)["MPFRecordID"].ToString();
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
            eBinding.add((HtmlInputHidden)e.Item.FindControl("MPFRecordID"));
            eBinding.add(new TextBoxBinder(db, ((WebDatePicker)e.Item.FindControl("MPFRecPeriodFr")).TextBox, "MPFRecPeriodFr"));
            eBinding.add(new TextBoxBinder(db, ((WebDatePicker)e.Item.FindControl("MPFRecPeriodTo")).TextBox, "MPFRecPeriodTo"));
            eBinding.add((TextBox)e.Item.FindControl("MPFRecActMCRI"));
            eBinding.add((TextBox)e.Item.FindControl("MPFRecActMCER"));
            eBinding.add((TextBox)e.Item.FindControl("MPFRecActMCEE"));
            eBinding.add((TextBox)e.Item.FindControl("MPFRecActVCER"));
            eBinding.add((TextBox)e.Item.FindControl("MPFRecActVCEE"));
            eBinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("MPFPlanID"), EMPFPlan.VLMPFPlan));


            eBinding.init(Request, Session);


            EMPFRecord obj = new EMPFRecord();
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
                    if (obj.MPFRecPeriodFr <= payPeriod.PayPeriodTo && obj.MPFRecPeriodTo <= payPeriod.PayPeriodTo)
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
            AddPanel.Visible = IsAllowEdit;
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
            HtmlInputHidden h = (HtmlInputHidden)item.FindControl("MPFRecordID");
            if (c.Checked)
            {
                EMPFRecord obj = new EMPFRecord();
                obj.MPFRecordID = Int32.Parse(h.Value);
                list.Add(obj);
            }
        }
        EEmpPayroll empPayroll = new EEmpPayroll();
        empPayroll.EmpPayrollID = int.Parse(EmpPayrollID.Value);
        if (EEmpPayroll.db.select(dbConn, empPayroll))
        {
            WebUtils.StartFunction(Session, m_FunctionCode, empPayroll.EmpID);
            foreach (EMPFRecord obj in list)
                if (db.select(dbConn, obj))
                    db.delete(dbConn, obj);
            WebUtils.EndFunction(dbConn);
        }
        view = loadData(info, db, Repeater);
    }

    protected void Recalculate_Click(object sender, EventArgs e)
    {
        EEmpPayroll empPayroll = new EEmpPayroll();
        empPayroll.EmpPayrollID = CurrentEmpPayrollID;
        if (EEmpPayroll.db.select(dbConn, empPayroll))
        {
            WebUtils.StartFunction(Session, m_FunctionCode, empPayroll.EmpID);
            HROne.Payroll.MPFProcess.Recalculate(dbConn, empPayroll);
            WebUtils.EndFunction(dbConn);
        }
        view = loadData(info, db, Repeater);
        Recalculate(this, EventArgs.Empty);
    }
}
