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

public partial class LeavePlanEntitle_List : HROneWebControl
{
    private const string FUNCTION_CODE = "LEV003";

    public int CurID = -1;

    protected SearchBinding sbinding;
    protected Binding binding;
    public DBManager sdb = ELeavePlanEntitle.db;
    protected ListInfo info;
    protected DataView view;
    

    protected int m_LeaveTypeID;
    public int CurrentLeaveTypeID
    {
        get { return m_LeaveTypeID; }
        set { m_LeaveTypeID = value; LeaveTypeID.Value = value.ToString(); }
    }
    protected string m_Title;
    public string Title
    {
        get { return m_Title; }
        set { m_Title = value; LeaveTypeDesc.Text = value; }
    }

    
    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        //toolBar.FunctionCode = FUNCTION_CODE;

        if (!WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
        {
            Delete.Visible = false;
            //New.Visible = false;
            AddPanel.Visible = false;
            btnEditLeavePlanBroughtForward.Visible = false;
        }

        sbinding = new SearchBinding(dbConn, sdb);
        sbinding.init(DecryptedRequest, null);

        binding = new Binding(dbConn, sdb);
        binding.add(LeavePlanEntitleYearOfService);
        binding.add(LeavePlanEntitleDays);
        binding.add(LeavePlanID);
        binding.add(LeaveTypeID);
        binding.init(Request, Session);

        info = ListFooter.ListInfo;

        if (!int.TryParse(DecryptedRequest["LeavePlanID"], out CurID))
            CurID = -1;
        LeavePlanID.Value = CurID.ToString();

        Delete.OnClientClick = HROne.Translation.PromptMessage.CreateDeleteConfirmDialogJavascript(Delete);

        HROne.Common.WebUtility.WebControlsLocalization(Session,this.Controls);
        
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            info.orderby = "LeavePlanEntitleYearOfService";
            info.order = true;
            loadObject();
            view = loadData(info, sdb, Repeater);
        }
    }

    public void loadObject()
    {
        DBFilter dbFilter = new DBFilter();
        dbFilter.add(new Match("LeaveTypeID", LeaveTypeID.Value));
        dbFilter.add(new Match("LeavePlanID", LeavePlanID.Value));

        ArrayList list = ELeavePlanBroughtForward.db.select(dbConn, dbFilter);
        if (list.Count > 0)
        {
            ELeavePlanBroughtForward leavePlanBroughtForward = (ELeavePlanBroughtForward)list[0];
            txtLeavePlanBroughtForward.Text = leavePlanBroughtForward.LeavePlanBroughtForwardMax.ToString();
            txtLeavePlanBroughtForwardNumOfMonthExpired.Text = leavePlanBroughtForward.LeavePlanBroughtForwardNumOfMonthExpired.ToString();
            if (leavePlanBroughtForward.LeavePlanBroughtForwardMax == 9999)
            {
                lblLeavePlanBroughtForward.Text = "¡Û";
//                lblLeavePlanBroughtForward.Font.Size = 14;
            }
            else
            {
                lblLeavePlanBroughtForward.Text = leavePlanBroughtForward.LeavePlanBroughtForwardMax.ToString();
//                lblLeavePlanBroughtForward.Font.Size = 12;
            }

            if (leavePlanBroughtForward.LeavePlanBroughtForwardNumOfMonthExpired == 9999)
            {
                lblLeavePlanBroughtForwardNumOfMonthExpired.Text = "¡Û";
//                lblLeavePlanBroughtForward.Font.Size = 14;
            }  
            else
            {
                lblLeavePlanBroughtForwardNumOfMonthExpired.Text = leavePlanBroughtForward.LeavePlanBroughtForwardNumOfMonthExpired.ToString();
//                lblLeavePlanBroughtForward.Font.Size = 12;
            }
            // ****** Start 2013/11/15, Ricky So, remove forfeit checkbox
            // LeavePlanBroughtForwardForfeitLastYearBroughtForwardOnly.Checked = leavePlanBroughtForward.LeavePlanBroughtForwardForfeitLastYearBroughtForwardOnly;
            // chkLeavePlanBroughtForwardForfeitLastYearBroughtForwardOnly.Checked = leavePlanBroughtForward.LeavePlanBroughtForwardForfeitLastYearBroughtForwardOnly;
            // ****** End 2013/11/15, Ricky So, remove forfeit checkbox
        }
        else
        {
            lblLeavePlanBroughtForward.Text = "¡Û";
            txtLeavePlanBroughtForward.Text = "9999";
            // ****** Start 2013/11/15, Ricky So, remove forfeit checkbox
            // lblLeavePlanBroughtForwardNumOfMonthExpired.Text = "-";
            // LeavePlanBroughtForwardForfeitLastYearBroughtForwardOnly.Checked = false;
            // chkLeavePlanBroughtForwardForfeitLastYearBroughtForwardOnly.Checked = false;
            // ****** End 2013/11/15, Ricky So, remove forfeit checkbox
            txtLeavePlanBroughtForwardNumOfMonthExpired.Text = "9999";
            lblLeavePlanBroughtForwardNumOfMonthExpired.Text = "¡Û";
        }


    }

    public DataView loadData(ListInfo info, DBManager db, DataList repeater)
    {
        DBFilter filter = new DBFilter();
        filter.add(new Match("LeavePlanID", this.CurID));
        filter.add(new Match("LeaveTypeID", this.LeaveTypeID.Value));

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);

        string select = "c.*";
        string from = "from [" + sdb.dbclass.tableName + "] c ";

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

    //protected void FirstPage_Click(object sender, EventArgs e)
    //{
    //    loadState();
    //    info.page = 0;
    //    view = loadData(info, sdb, Repeater);

    //}
    //protected void PrevPage_Click(object sender, EventArgs e)
    //{
    //    loadState();
    //    info.page--;
    //    view = loadData(info, sdb, Repeater);

    //}
    //protected void NextPage_Click(object sender, EventArgs e)
    //{
    //    loadState();
    //    info.page++;
    //    view = loadData(info, sdb, Repeater);

    //}
    //protected void LastPage_Click(object sender, EventArgs e)
    //{
    //    loadState();

    //    info.page = Int32.Parse(NumPage.Value);
    //    view = loadData(info, sdb, Repeater);

    //}

    protected void Add_Click(object sender, EventArgs e)
    {
        Repeater.EditItemIndex = -1;
        ELeavePlanEntitle c = new ELeavePlanEntitle();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(sdb, Page.Master);
        errors.clear();


        sdb.validate(errors, values);

        if (!errors.isEmpty())
            return;


        sdb.parse(values, c);

        DBFilter filter = new DBFilter();
        filter.add(new Match("LeavePlanID", c.LeavePlanID));
        filter.add(new Match("LeaveTypeID", c.LeaveTypeID));
        filter.add(new Match("LeavePlanEntitleID", "<>", c.LeavePlanEntitleID));
        filter.add(new Match("LeavePlanEntitleYearOfService", c.LeavePlanEntitleYearOfService));
        if (sdb.count(dbConn, filter) > 0)
            errors.addError("LeavePlanEntitleYearOfService", HROne.Translation.PageErrorMessage.ERROR_DUPLICATE_YEAR_SERVICE);
        if (!errors.isEmpty())
            return;

        WebUtils.StartFunction(Session, FUNCTION_CODE);
        sdb.insert(dbConn, c);
        WebUtils.EndFunction(dbConn);

        LeavePlanEntitleYearOfService.Text = string.Empty;
        LeavePlanEntitleDays.Text = string.Empty;
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
    protected void Repeater_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        e.Item.FindControl("DeleteItem").Visible = Delete.Visible;
        if (e.Item.ItemIndex == Repeater.EditItemIndex)
        {
            Binding ebinding = new Binding(dbConn, sdb);
            ebinding.add((HtmlInputHidden)e.Item.FindControl("LeavePlanEntitleID"));
            ebinding.add((TextBox)e.Item.FindControl("LeavePlanEntitleYearOfService"));
            ebinding.add((TextBox)e.Item.FindControl("LeavePlanEntitleDays"));

            ebinding.init(Request, Session);


            ELeavePlanEntitle obj = new ELeavePlanEntitle();
            sdb.toObject(((DataRowView)e.Item.DataItem).Row, obj);
            Hashtable values = new Hashtable();
            sdb.populate(obj, values);
            ebinding.toControl(values);
        }
        else
        {
            e.Item.FindControl("Edit").Visible = Delete.Visible;
            HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("LeavePlanEntitleID");
            h.Value = ((DataRowView)e.Item.DataItem)["LeavePlanEntitleID"].ToString();
        }
        HROne.Common.WebUtility.WebControlsLocalization(Session, e.Item.Controls);


        //DataRowView row = (DataRowView)e.Item.DataItem;
        //CheckBox cb = (CheckBox)e.Item.FindControl("DeleteItem");
        //WebFormUtils.LoadKeys(sdb, row, cb);
        //e.Item.FindControl("DeleteItem").Visible = Delete.Visible;//toolBar.DeleteButton_Visible;
    }

    protected void Repeater_ItemCommand(object source, DataListCommandEventArgs e)
    {
        Button b = (Button)e.CommandSource;




        if (b.ID.Equals("Edit"))
        {
            Repeater.EditItemIndex = e.Item.ItemIndex;
            view = loadData(info, sdb, Repeater);
            WebUtils.SetEnabledControlSection(AddPanel, false);
        }
        else if (b.ID.Equals("Cancel"))
        {
            Repeater.EditItemIndex = -1;
            view = loadData(info, sdb, Repeater);
            WebUtils.SetEnabledControlSection(AddPanel, true);
        }
        else if (b.ID.Equals("Save"))
        {
            Binding ebinding = new Binding(dbConn, sdb);
            ebinding.add((HtmlInputHidden)e.Item.FindControl("LeavePlanEntitleID"));
            ebinding.add((TextBox)e.Item.FindControl("LeavePlanEntitleYearOfService"));
            ebinding.add((TextBox)e.Item.FindControl("LeavePlanEntitleDays"));
            ebinding.add(LeavePlanID);
            ebinding.add(LeaveTypeID);

            ebinding.init(Request, Session);


            ELeavePlanEntitle obj = new ELeavePlanEntitle();
            Hashtable values = new Hashtable();

            PageErrors errors = PageErrors.getErrors(sdb, Page.Master);
            errors.clear();


            ebinding.toValues(values);
            sdb.validate(errors, values);

            if (!errors.isEmpty())
                return;

            sdb.parse(values, obj);

            DBFilter filter = new DBFilter();
            filter.add(new Match("LeavePlanID", obj.LeavePlanID));
            filter.add(new Match("LeaveTypeID", obj.LeaveTypeID));
            filter.add(new Match("LeavePlanEntitleID", "<>", obj.LeavePlanEntitleID));
            filter.add(new Match("LeavePlanEntitleYearOfService", obj.LeavePlanEntitleYearOfService));
            if (sdb.count(dbConn, filter) > 0)
                errors.addError("LeavePlanEntitleYearOfService", HROne.Translation.PageErrorMessage.ERROR_DUPLICATE_YEAR_SERVICE);
            if (!errors.isEmpty())
                return;

            WebUtils.StartFunction(Session, FUNCTION_CODE);
            sdb.update(dbConn, obj);
            WebUtils.EndFunction(dbConn);

            Repeater.EditItemIndex = -1;

            view = loadData(info, sdb, Repeater);
            WebUtils.SetEnabledControlSection(AddPanel, true);
        }


    }
    protected void Delete_Click(object sender, EventArgs e)
    {
        ArrayList list = new ArrayList();
        foreach (DataListItem item in Repeater.Items)
        {
            CheckBox c = (CheckBox)item.FindControl("DeleteItem");
            HtmlInputHidden h = (HtmlInputHidden)item.FindControl("LeavePlanEntitleID");
            if (c.Checked)
            {
                ELeavePlanEntitle obj = new ELeavePlanEntitle();
                obj.LeavePlanEntitleID = Int32.Parse(h.Value);
                list.Add(obj);
            }
        }

        if (list.Count > 0)
        {
            WebUtils.StartFunction(Session, FUNCTION_CODE);
            foreach (BaseObject o in list)
                if (sdb.select(dbConn, o))
                    sdb.delete(dbConn, o);
            WebUtils.EndFunction(dbConn);
        }
        loadData(info, sdb, Repeater);
    }
    //public void New_Click(object sender, EventArgs e)
    //{
    //    HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "LeavePlanEntitle_Edit.aspx?LeavePlanID=" + LeavePlanID.Value + "&LeaveTypeID=" + LeaveTypeID.Value);
    //}

    protected void btnEditLeavePlanBroughtForward_Click(object sender, EventArgs e)
    {
        LeavePlanBroughtForwardEditCell.Visible = true;
        LeavePlanBroughtForwardViewCell.Visible = false;

        loadObject();
        view = loadData(info, sdb, Repeater);
    
    }


    protected void btnSaveLeavePlanBroughtForward_Click(object sender, EventArgs e)
    {
        int maxBF;
        int numMonthExpiry;
        if (!int.TryParse(txtLeavePlanBroughtForward.Text, out maxBF))
        {
            PageErrors errors = PageErrors.getErrors(ELeavePlanBroughtForward.db, this.Page);
            errors.addError(HROne.Common.WebUtility.GetLocalizedString("validate.int.prompt").Replace("{0}", LeavePlanBroughtForwardName.Text));
            return;
        }
        if (!int.TryParse(txtLeavePlanBroughtForwardNumOfMonthExpired.Text, out numMonthExpiry))
        {
            PageErrors errors = PageErrors.getErrors(ELeavePlanBroughtForward.db, this.Page);
            errors.addError(HROne.Common.WebUtility.GetLocalizedString("validate.int.prompt").Replace("{0}", LeavePlanBroughtForwardNumOfMonthExpiredName.Text));
            return;
        }
        if (maxBF == 0 && numMonthExpiry > 0)
        {
            PageErrors errors = PageErrors.getErrors(ELeavePlanBroughtForward.db, this.Page);
            errors.addError(HROne.Common.WebUtility.GetLocalizedString("Expiry For must be 0 when Max Days B/F is 0"));
            return;
        }
        else if (maxBF > 0 && numMonthExpiry == 0)
        {
            PageErrors errors = PageErrors.getErrors(ELeavePlanBroughtForward.db, this.Page);
            errors.addError(HROne.Common.WebUtility.GetLocalizedString("B/F leaves should not expire immediately.  Please enter correct No. Of Months before expiry"));
            return;

        }
        
        DBFilter dbFilter = new DBFilter();
        dbFilter.add(new Match("LeaveTypeID", LeaveTypeID.Value));
        dbFilter.add(new Match("LeavePlanID", LeavePlanID.Value));

        WebUtils.StartFunction(Session, FUNCTION_CODE);
        ArrayList list = ELeavePlanBroughtForward.db.select(dbConn, dbFilter);
        if (list.Count > 0)
        {
            ELeavePlanBroughtForward leavePlanBroughtForward = (ELeavePlanBroughtForward)list[0];
            leavePlanBroughtForward.LeavePlanBroughtForwardMax = maxBF;
            leavePlanBroughtForward.LeavePlanBroughtForwardNumOfMonthExpired = numMonthExpiry;
            // ****** Start 2013/11/15, Ricky So, Remove the frofeit last year b/f in leave balance calculation
            // leavePlanBroughtForward.LeavePlanBroughtForwardForfeitLastYearBroughtForwardOnly = LeavePlanBroughtForwardForfeitLastYearBroughtForwardOnly.Checked;
            // ****** End 2013/11/15, Ricky So, Remove the frofeit last year b/f in leave balance calculation
            ELeavePlanBroughtForward.db.update(dbConn, leavePlanBroughtForward);
        }
        else
        {
            ELeavePlanBroughtForward leavePlanBroughtForward = new ELeavePlanBroughtForward();
            leavePlanBroughtForward.LeavePlanID = Convert.ToInt32(LeavePlanID.Value);
            leavePlanBroughtForward.LeaveTypeID = Convert.ToInt32(LeaveTypeID.Value);
            leavePlanBroughtForward.LeavePlanBroughtForwardMax = maxBF;
            leavePlanBroughtForward.LeavePlanBroughtForwardNumOfMonthExpired = numMonthExpiry;
            // ****** Start 2013/11/15, Ricky So, Remove the frofeit last year b/f in leave balance calculation
            // leavePlanBroughtForward.LeavePlanBroughtForwardForfeitLastYearBroughtForwardOnly = LeavePlanBroughtForwardForfeitLastYearBroughtForwardOnly.Checked;
            // ****** End 2013/11/15, Ricky So, Remove the frofeit last year b/f in leave balance calculation
            ELeavePlanBroughtForward.db.insert(dbConn, leavePlanBroughtForward);
        }
        WebUtils.EndFunction(dbConn);

        LeavePlanBroughtForwardEditCell.Visible = false;
        LeavePlanBroughtForwardViewCell.Visible = true;

        loadObject();
        view = loadData(info, sdb, Repeater);

    }
    protected void btnCancelLeavePlanBroughtForward_Click(object sender, EventArgs e)
    {
        LeavePlanBroughtForwardEditCell.Visible = false;
        LeavePlanBroughtForwardViewCell.Visible = true;

        loadObject();
        view = loadData(info, sdb, Repeater);

    }
}
