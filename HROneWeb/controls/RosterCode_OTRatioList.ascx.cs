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

public partial class controls_RosterCode_OTRatioList : HROneWebControl 
{
    private const string FUNCTION_CODE = "ATT001";

    protected SearchBinding sbinding;
    // add
    protected DBManager RosterCodeDetaildb = ERosterCodeDetail.db;
    protected ListInfo info;
    protected DataView view;
    public Binding RosterCodeDetailbinding;
    public Binding RosterCodeDetailebinding;
    public ERosterCodeDetail RosterCodeDetailobj;
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
        RosterCodeDetailbinding = new Binding(dbConn, RosterCodeDetaildb);
        RosterCodeDetailbinding.add(RosterCodeID);
        RosterCodeDetailbinding.add(RosterCodeDetailNoOfHour);
        RosterCodeDetailbinding.add(RosterCodeDetailRate);
        RosterCodeDetailbinding.init(Request, Session);

        sbinding = new SearchBinding(dbConn, RosterCodeDetaildb);
        sbinding.add(new HiddenMatchBinder(RosterCodeID));
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

            view = loadData(info, RosterCodeDetaildb, Repeater); //add by Ben

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
        view = loadData(info, RosterCodeDetaildb, Repeater);

    }
    protected void Add_Click(object sender, EventArgs e)
    {
        Repeater.EditItemIndex = -1;
        ERosterCodeDetail c = new ERosterCodeDetail();

        Hashtable values = new Hashtable();
        RosterCodeDetailbinding.toValues(values);

        PageErrors errors = PageErrors.getErrors(RosterCodeDetaildb, Page);
        errors.clear();


        RosterCodeDetaildb.validate(errors, values);

        if (!errors.isEmpty())
            return;


        RosterCodeDetaildb.parse(values, c);

        //if (!AppUtils.checkDuplicate(dbConn, RosterCodeDetaildb, c, errors, "RosterCodeDetailYearOfService"))
        //    return;
        WebUtils.StartFunction(Session, FUNCTION_CODE);
        RosterCodeDetaildb.insert(dbConn, c);
        WebUtils.EndFunction(dbConn);
        RosterCodeDetailNoOfHour.Text = string.Empty;
        RosterCodeDetailRate.Text = string.Empty;
        view = loadData(info, RosterCodeDetaildb, Repeater);
    }

    protected void Repeater_ItemDataBound(object sender, DataListItemEventArgs e)
    {


        if (e.Item.ItemIndex == Repeater.EditItemIndex)
        {
            RosterCodeDetailebinding = new Binding(dbConn, RosterCodeDetaildb);
            RosterCodeDetailebinding.add((HtmlInputHidden)e.Item.FindControl("RosterCodeDetailID"));
            RosterCodeDetailebinding.add((TextBox)e.Item.FindControl("RosterCodeDetailNoOfHour"));
            RosterCodeDetailebinding.add((TextBox)e.Item.FindControl("RosterCodeDetailRate"));

            RosterCodeDetailebinding.init(Request, Session);


            ERosterCodeDetail obj = new ERosterCodeDetail();
            RosterCodeDetaildb.toObject(((DataRowView)e.Item.DataItem).Row, obj);
            Hashtable values = new Hashtable();
            RosterCodeDetaildb.populate(obj, values);
            RosterCodeDetailebinding.toControl(values);
        }
        else
        {
            e.Item.FindControl("Edit").Visible = IsAllowEdit;
            e.Item.FindControl("DeleteItem").Visible = IsAllowEdit;
            HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("RosterCodeDetailID");
            h.Value = ((DataRowView)e.Item.DataItem)["RosterCodeDetailID"].ToString();

        }
        HROne.Common.WebUtility.WebControlsLocalization(Session, e.Item.Controls);
    }

    protected void Repeater_ItemCommand(object source, DataListCommandEventArgs e)
    {
        Button b = (Button)e.CommandSource;




        if (b.ID.Equals("Edit"))
        {
            Repeater.EditItemIndex = e.Item.ItemIndex;
            view = loadData(info, RosterCodeDetaildb, Repeater);
            WebUtils.SetEnabledControlSection(AddPanel, false);
        }
        else if (b.ID.Equals("Cancel"))
        {
            Repeater.EditItemIndex = -1;
            view = loadData(info, RosterCodeDetaildb, Repeater);
            WebUtils.SetEnabledControlSection(AddPanel, true);
        }
        else if (b.ID.Equals("Save"))
        {
            RosterCodeDetailebinding = new Binding(dbConn, RosterCodeDetaildb);
            RosterCodeDetailebinding.add((HtmlInputHidden)e.Item.FindControl("RosterCodeDetailID"));
            // RosterCodeDetailebinding.add((TextBox)e.Item.FindControl("RosterCodeID"));              // not defined by ben
            RosterCodeDetailebinding.add((TextBox)e.Item.FindControl("RosterCodeDetailNoOfHour"));
            RosterCodeDetailebinding.add((TextBox)e.Item.FindControl("RosterCodeDetailRate"));
            RosterCodeDetailebinding.init(Request, Session);


            ERosterCodeDetail obj = new ERosterCodeDetail();
            Hashtable values = new Hashtable();

            PageErrors errors = PageErrors.getErrors(RosterCodeDetaildb, Page);
            errors.clear();


            RosterCodeDetailebinding.toValues(values);
            RosterCodeDetaildb.validate(errors, values);

            if (!errors.isEmpty())
                return;

            RosterCodeDetaildb.parse(values, obj);
            //if (!AppUtils.checkDuplicate(dbConn, RosterCodeDetaildb, obj, errors, "RosterCodeID"))
            //return;

            WebUtils.StartFunction(Session, FUNCTION_CODE);
            RosterCodeDetaildb.update(dbConn, obj);
            WebUtils.EndFunction(dbConn);

            Repeater.EditItemIndex = -1;
            view = loadData(info, RosterCodeDetaildb, Repeater);
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
            HtmlInputHidden h = (HtmlInputHidden)item.FindControl("RosterCodeDetailID");
            if (c.Checked)
            {
                ERosterCodeDetail obj = new ERosterCodeDetail();
                obj.RosterCodeDetailID = Int32.Parse(h.Value);
                list.Add(obj);
            }
        }
        foreach (ERosterCodeDetail obj in list)
        {
            WebUtils.StartFunction(Session, FUNCTION_CODE);
            RosterCodeDetaildb.delete(dbConn, obj);
            WebUtils.EndFunction(dbConn);
        }
        view = loadData(info, RosterCodeDetaildb, Repeater);

    }

}
