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

public partial class RosterCode_AdditionalPayment : HROneWebControl 
{
    private const string FUNCTION_CODE = "ATT001";

    protected SearchBinding sbinding;
    // add
    protected DBManager db = ERosterCodeAdditionalPayment.db;
    protected ListInfo info;
    protected DataView view;
    public Binding binding;
    public Binding ebinding;
    public ERosterCodeAdditionalPayment obj;
    private bool IsAllowEdit = false;

    public int CurID = -1;

    public int CurrentRosterCodeID
    {
        get { return CurID; }
        set { CurID = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //Add
        binding = new Binding(dbConn, db);
        binding.add(RosterCodeID);
        binding.add(new DropDownVLBinder(db, PaymentCodeID, EPaymentCode.VLPaymentCode));
        binding.add(RosterCodeAdditionalPaymentAmount);
        binding.init(Request, Session);

        sbinding = new SearchBinding(dbConn, db);
        sbinding.add(new HiddenMatchBinder(RosterCodeID));
        sbinding.initValues("PaymentCodeID", null, EPaymentCode.VLPaymentCode, null);
        sbinding.init(DecryptedRequest, null);

        info = ListFooter.ListInfo;

        //if (!int.TryParse(DecryptedRequest["RosterCodeID"], out CurID))
        //    CurID = -1;

        Delete.OnClientClick = HROne.Translation.PromptMessage.CreateDeleteConfirmDialogJavascript(Delete);

        IsAllowEdit = WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite);
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        RosterCodeID.Value = CurID.ToString();
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

    public DataView loadData(ListInfo info, DBManager RosterCodeDetaildb, DataList repeater)
    {
        DBFilter filter = sbinding.createFilter();

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);

        string select = "c.*";
        string from = "from " + RosterCodeDetaildb.dbclass.tableName + " c ";

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
        ERosterCodeAdditionalPayment c = new ERosterCodeAdditionalPayment();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);

        //if (!AppUtils.checkDuplicate(dbConn, RosterCodeDetaildb, c, errors, "RosterCodeDetailYearOfService"))
        //    return;
        WebUtils.StartFunction(Session, FUNCTION_CODE);
        db.insert(dbConn, c);
        WebUtils.EndFunction(dbConn);
        PaymentCodeID.SelectedIndex = 0;
        RosterCodeAdditionalPaymentAmount.Text = string.Empty;
        view = loadData(info, db, Repeater);
    }

    protected void Repeater_ItemDataBound(object sender, DataListItemEventArgs e)
    {


        if (e.Item.ItemIndex == Repeater.EditItemIndex)
        {
            ebinding = new Binding(dbConn, db);
            ebinding.add((HtmlInputHidden)e.Item.FindControl("RosterCodeAdditionalPaymentID"));
            ebinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("PaymentCodeID"), EPaymentCode.VLPaymentCode));
            ebinding.add((TextBox)e.Item.FindControl("RosterCodeAdditionalPaymentAmount"));

            ebinding.init(Request, Session);


            ERosterCodeAdditionalPayment obj = new ERosterCodeAdditionalPayment();
            db.toObject(((DataRowView)e.Item.DataItem).Row, obj);
            Hashtable values = new Hashtable();
            db.populate(obj, values);
            ebinding.toControl(values);
        }
        else
        {
            e.Item.FindControl("Edit").Visible = IsAllowEdit;
            e.Item.FindControl("DeleteItem").Visible = IsAllowEdit;
            HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("RosterCodeAdditionalPaymentID");
            h.Value = ((DataRowView)e.Item.DataItem)["RosterCodeAdditionalPaymentID"].ToString();

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
            ebinding.add((HtmlInputHidden)e.Item.FindControl("RosterCodeAdditionalPaymentID"));
            ebinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("PaymentCodeID"), EPaymentCode.VLPaymentCode));
            ebinding.add((TextBox)e.Item.FindControl("RosterCodeAdditionalPaymentAmount"));
            ebinding.init(Request, Session);


            ERosterCodeAdditionalPayment obj = new ERosterCodeAdditionalPayment();
            Hashtable values = new Hashtable();

            PageErrors errors = PageErrors.getErrors(db, Page);
            errors.clear();


            ebinding.toValues(values);
            db.validate(errors, values);

            if (!errors.isEmpty())
                return;

            db.parse(values, obj);
            //if (!AppUtils.checkDuplicate(dbConn, RosterCodeDetaildb, obj, errors, "RosterCodeID"))
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
        //ERosterCode c = new ERosterCode();
        //c.RosterCodeID = CurID;
        //db.delete(dbConn, c);
        //HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "RosterCode_List.aspx");
        ArrayList list = new ArrayList();
        foreach (DataListItem item in Repeater.Items)
        {
            CheckBox c = (CheckBox)item.FindControl("DeleteItem");
            HtmlInputHidden h = (HtmlInputHidden)item.FindControl("RosterCodeAdditionalPaymentID");
            if (c.Checked)
            {
                ERosterCodeAdditionalPayment obj = new ERosterCodeAdditionalPayment();
                obj.RosterCodeAdditionalPaymentID = Int32.Parse(h.Value);
                list.Add(obj);
            }
        }
        foreach (ERosterCodeAdditionalPayment obj in list)
        {
            WebUtils.StartFunction(Session, FUNCTION_CODE);
            db.delete(dbConn, obj);
            WebUtils.EndFunction(dbConn);
        }
        view = loadData(info, db, Repeater);

    }

}
