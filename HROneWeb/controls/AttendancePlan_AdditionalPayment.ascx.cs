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

public partial class AttendancePlan_AdditionalPayment : HROneWebControl 
{
    private const string FUNCTION_CODE = "ATT003";

    protected SearchBinding sbinding;
    // add
    protected DBManager db = EAttendancePlanAdditionalPayment.db;
    protected ListInfo info;
    protected DataView view;
    public Binding binding;
    public Binding ebinding;
    public EAttendancePlanAdditionalPayment obj;
    private bool IsAllowEdit = false;

    public int CurID = -1;

    public int CurrentAttendancePlanID
    {
        get { return CurID; }
        set { CurID = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //Add
        binding = new Binding(dbConn, db);
        binding.add(AttendancePlanID);
        binding.add(new DropDownVLBinder(db, PaymentCodeID, EPaymentCode.VLPaymentCode));
        binding.add(AttendancePlanAdditionalPaymentAmount);
        binding.add(AttendancePlanAdditionalPaymentMaxLateMins);
        binding.add(AttendancePlanAdditionalPaymentMaxEarlyLeaveMins);
        binding.add(AttendancePlanAdditionalPaymentMinOvertimeMins);
        binding.add(AttendancePlanAdditionalPaymentRosterAcrossTime);
        binding.init(Request, Session);

        sbinding = new SearchBinding(dbConn, db);
        sbinding.add(new HiddenMatchBinder(AttendancePlanID));
        sbinding.initValues("PaymentCodeID", null, EPaymentCode.VLPaymentCode, null);
        sbinding.init(DecryptedRequest, null);

        info = ListFooter.ListInfo;

        //if (!int.TryParse(DecryptedRequest["AttendancePlanID"], out CurID))
        //    CurID = -1;

        Delete.OnClientClick = HROne.Translation.PromptMessage.CreateDeleteConfirmDialogJavascript(Delete);

        IsAllowEdit = WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite);

        AttendancePlanAdditionalPaymentRosterAcrossTime.MaxLength = 0;
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        AttendancePlanID.Value = CurID.ToString();
        if (!Page.IsPostBack)
        {

            view = loadData(info, db, Repeater); //add by Ben

            if (CurID > 0)
            {
                AddPanel.Visible = IsAllowEdit;

                Delete.Visible = IsAllowEdit;

            }
        }
    }

    public DataView loadData(ListInfo info, DBManager AttendancePlanDetaildb, DataList repeater)
    {
        DBFilter filter = sbinding.createFilter();

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);

        string select = "c.*";
        string from = "from " + AttendancePlanDetaildb.dbclass.tableName + " c ";

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);

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
        EAttendancePlanAdditionalPayment c = new EAttendancePlanAdditionalPayment();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);

        //if (!AppUtils.checkDuplicate(dbConn, AttendancePlanDetaildb, c, errors, "AttendancePlanDetailYearOfService"))
        //    return;
        WebUtils.StartFunction(Session, FUNCTION_CODE);
        db.insert(dbConn, c);
        WebUtils.EndFunction(dbConn);
        PaymentCodeID.SelectedIndex = 0;
        AttendancePlanAdditionalPaymentAmount.Text = string.Empty;
        view = loadData(info, db, Repeater);
    }

    protected void Repeater_ItemDataBound(object sender, DataListItemEventArgs e)
    {


        if (e.Item.ItemIndex == Repeater.EditItemIndex)
        {
            ebinding = new Binding(dbConn, db);
            ebinding.add((HtmlInputHidden)e.Item.FindControl("AttendancePlanAdditionalPaymentID"));
            ebinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("PaymentCodeID"), EPaymentCode.VLPaymentCode));
            ebinding.add((TextBox)e.Item.FindControl("AttendancePlanAdditionalPaymentAmount"));
            ebinding.add((TextBox)e.Item.FindControl("AttendancePlanAdditionalPaymentMaxLateMins"));
            ebinding.add((TextBox)e.Item.FindControl("AttendancePlanAdditionalPaymentMaxEarlyLeaveMins"));
            ebinding.add((TextBox)e.Item.FindControl("AttendancePlanAdditionalPaymentMinOvertimeMins"));
            ebinding.add((TextBox)e.Item.FindControl("AttendancePlanAdditionalPaymentRosterAcrossTime"));

            ebinding.init(Request, Session);


            EAttendancePlanAdditionalPayment obj = new EAttendancePlanAdditionalPayment();
            db.toObject(((DataRowView)e.Item.DataItem).Row, obj);
            Hashtable values = new Hashtable();
            db.populate(obj, values);
            ebinding.toControl(values);

            ((TextBox)e.Item.FindControl("AttendancePlanAdditionalPaymentRosterAcrossTime")).MaxLength = 0;

        }
        else
        {
            e.Item.FindControl("Edit").Visible = IsAllowEdit;
            e.Item.FindControl("DeleteItem").Visible = IsAllowEdit;

            ebinding = new Binding(dbConn, db);
            ebinding.add((HtmlInputHidden)e.Item.FindControl("AttendancePlanAdditionalPaymentID"));
            //ebinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("PaymentCodeID"), EPaymentCode.VLPaymentCode));
            //ebinding.add((TextBox)e.Item.FindControl("AttendancePlanAdditionalPaymentAmount"));
            //ebinding.add((TextBox)e.Item.FindControl("AttendancePlanAdditionalPaymentMaxLateMins"));
            //ebinding.add((TextBox)e.Item.FindControl("AttendancePlanAdditionalPaymentMaxEarlyLeaveMins"));
            //ebinding.add((TextBox)e.Item.FindControl("AttendancePlanAdditionalPaymentMinOvertimeMins"));
            ebinding.add((Label)e.Item.FindControl("AttendancePlanAdditionalPaymentRosterAcrossTime"));

            ebinding.init(Request, Session);


            EAttendancePlanAdditionalPayment obj = new EAttendancePlanAdditionalPayment();
            db.toObject(((DataRowView)e.Item.DataItem).Row, obj);
            Hashtable values = new Hashtable();
            db.populate(obj, values);
            ebinding.toControl(values);

            if (obj.AttendancePlanAdditionalPaymentRosterAcrossTime.Ticks.Equals(0))
                e.Item.FindControl("AttendancePlanAdditionalPaymentRosterAcrossTimePanel").Visible = false;
            //HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("AttendancePlanAdditionalPaymentID");
            //h.Value = ((DataRowView)e.Item.DataItem)["AttendancePlanAdditionalPaymentID"].ToString();

        }
        HROne.Common.WebUtility.WebControlsLocalization(Session, e.Item.Controls);
    }

    protected void Repeater_ItemCommand(object source, DataListCommandEventArgs e)
    {
        Button b = (Button)e.CommandSource;




        if (b.ID.Equals("Edit"))
        {
            Repeater.EditItemIndex = e.Item.ItemIndex;
            view = loadData(info, db, Repeater);
            WebUtils.SetEnabledControlSection(AddPanel, false);
        }
        else if (b.ID.Equals("Cancel"))
        {
            Repeater.EditItemIndex = -1;
            view = loadData(info, db, Repeater);
            WebUtils.SetEnabledControlSection(AddPanel, true);
        }
        else if (b.ID.Equals("Save"))
        {
            ebinding = new Binding(dbConn, db);
            ebinding.add((HtmlInputHidden)e.Item.FindControl("AttendancePlanAdditionalPaymentID"));
            ebinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("PaymentCodeID"), EPaymentCode.VLPaymentCode));
            ebinding.add((TextBox)e.Item.FindControl("AttendancePlanAdditionalPaymentAmount"));
            ebinding.add((TextBox)e.Item.FindControl("AttendancePlanAdditionalPaymentMaxLateMins"));
            ebinding.add((TextBox)e.Item.FindControl("AttendancePlanAdditionalPaymentMaxEarlyLeaveMins"));
            ebinding.add((TextBox)e.Item.FindControl("AttendancePlanAdditionalPaymentMinOvertimeMins"));
            ebinding.add((TextBox)e.Item.FindControl("AttendancePlanAdditionalPaymentRosterAcrossTime"));
            ebinding.init(Request, Session);


            EAttendancePlanAdditionalPayment obj = new EAttendancePlanAdditionalPayment();
            Hashtable values = new Hashtable();

            PageErrors errors = PageErrors.getErrors(db, Page);
            errors.clear();


            ebinding.toValues(values);
            db.validate(errors, values);

            if (!errors.isEmpty())
                return;

            db.parse(values, obj);
            //if (!AppUtils.checkDuplicate(dbConn, AttendancePlanDetaildb, obj, errors, "AttendancePlanID"))
            //return;

            WebUtils.StartFunction(Session, FUNCTION_CODE);
            db.update(dbConn, obj);
            WebUtils.EndFunction(dbConn);

            Repeater.EditItemIndex = -1;
            view = loadData(info, db, Repeater);
            WebUtils.SetEnabledControlSection(AddPanel, true);
        }


    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        //EAttendancePlan c = new EAttendancePlan();
        //c.AttendancePlanID = CurID;
        //db.delete(dbConn, c);
        //HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "AttendancePlan_List.aspx");
        ArrayList list = new ArrayList();
        foreach (DataListItem item in Repeater.Items)
        {
            CheckBox c = (CheckBox)item.FindControl("DeleteItem");
            HtmlInputHidden h = (HtmlInputHidden)item.FindControl("AttendancePlanAdditionalPaymentID");
            if (c.Checked)
            {
                EAttendancePlanAdditionalPayment obj = new EAttendancePlanAdditionalPayment();
                obj.AttendancePlanAdditionalPaymentID = Int32.Parse(h.Value);
                list.Add(obj);
            }
        }
        foreach (EAttendancePlanAdditionalPayment obj in list)
        {
            WebUtils.StartFunction(Session, FUNCTION_CODE);
            db.delete(dbConn, obj);
            WebUtils.EndFunction(dbConn);
        }
        view = loadData(info, db, Repeater);

    }

}
