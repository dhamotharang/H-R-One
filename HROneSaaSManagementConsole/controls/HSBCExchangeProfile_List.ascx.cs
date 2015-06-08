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
using HROne.SaaS.Entities;

public partial class HSBCExchangeProfile_List : HROneWebControl
{
    private bool IsAllowEdit = true;

    public int CompanyDBID = -1;

    protected SearchBinding sbinding;
    public DBManager db = EHSBCExchangeProfile.db;
    protected ListInfo info;
    protected DataView view;

    public Binding ebinding;

    protected void Page_Load(object sender, EventArgs e)
    {

        //if (!int.TryParse(DecryptedRequest["CompanyDBID"], out CompanyDBID))
        //    CompanyDBID = -1;
        //CompanyDBID.Value = CompanyDBID.ToString();


        sbinding = new SearchBinding(dbConn, db);
        sbinding.initValues("HSBCExchangeProfileIsLocked", null, HROne.Lib.Entities.Values.VLYesNo, ci);
        sbinding.init(DecryptedRequest, null);

        info = ListFooter.ListInfo;

        
    }

    void Page_PreRender(object sender, EventArgs e)
    {
        if (view == null)
        {
            //loadState();
            view = loadData(info, db, Repeater);
        }
    }

    //public void loadState()
    //{
    //    info = new ListInfo();
    //    int page = 0;
    //    if (!CurPage.Value.Equals(""))
    //        page = Int32.Parse(CurPage.Value);
    //    info.loadState(Request, page);
    //    info.order = !Order.Value.Equals("false", StringComparison.CurrentCultureIgnoreCase);
    //    info.orderby = OrderBy.Value;
    //    if (string.IsNullOrEmpty(info.orderby))
    //        info.orderby = "LeaveAppDateFrom";
    //}
    public DataView loadData(ListInfo info, DBManager db, DataList repeater)
    {
        DBFilter filter = sbinding.createFilter();
        filter.add(new Match("CompanyDBID", this.CompanyDBID));

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = "*";
        string from = "from " + db.dbclass.tableName + " ";

        DataTable table = filter.loadData(dbConn, info, select, from);

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
        //loadState();
        //info.page = 0;
        view = loadData(info, db, Repeater);

    }
    protected void PrevPage_Click(object sender, EventArgs e)
    {
        //loadState();
        //info.page--;
        view = loadData(info, db, Repeater);

    }
    protected void NextPage_Click(object sender, EventArgs e)
    {
        //loadState();
        //info.page++;
        view = loadData(info, db, Repeater);

    }
    protected void LastPage_Click(object sender, EventArgs e)
    {
        //loadState();

        //info.page = Int32.Parse(NumPage.Value);
        view = loadData(info, db, Repeater);

    }
    protected void ChangeOrder_Click(object sender, EventArgs e)
    {
        LinkButton l = (LinkButton)sender;
        String id = l.ID.Substring(1);
        //loadState();
        if (info.orderby == null)
            info.order = true;
        else if (info.orderby.Equals(id))
            info.order = !info.order;
        else
            info.order = true;
        info.orderby = id;

        view = loadData(info, db, Repeater);

    }

    protected void Add_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db,Page.Master);
        Repeater.EditItemIndex = -1;
        EHSBCExchangeProfile c = new EHSBCExchangeProfile();
        c.HSBCExchangeProfileBankCode = HSBCExchangeProfileBankCode.SelectedValue;
        AND checkConstraintTerm = new AND();
        checkConstraintTerm.add(new Match("CompanyDBID", CompanyDBID));

        if (!AppUtils.checkDuplicate(dbConn, EHSBCExchangeProfile.db, c, errors, "HSBCExchangeProfileBankCode", checkConstraintTerm))
            return;
        c.CompanyDBID = CompanyDBID;
        c.HSBCExchangeProfileRemoteProfileID = "DUMMY";

        EHSBCExchangeProfile.db.insert(dbConn, c);
        int tempID = c.HSBCExchangeProfileID;
        while (c.HSBCExchangeProfileRemoteProfileID == "DUMMY")
        {
            string prefix = string.Empty;
            if (c.HSBCExchangeProfileBankCode.Equals("HSBC", StringComparison.CurrentCultureIgnoreCase))
                prefix = "H";
            else if (c.HSBCExchangeProfileBankCode.Equals("HangSeng", StringComparison.CurrentCultureIgnoreCase))
                prefix = "X";
            string tmpRemoteProfileID = CreateRemoteProfileID(prefix, tempID);

            DBFilter exchangeProfileFilter = new DBFilter();
            exchangeProfileFilter.add(new Match("HSBCExchangeProfileRemoteProfileID", " LIKE ", "%" + tmpRemoteProfileID.Substring(prefix.Length) + "%"));
            if (EHSBCExchangeProfile.db.count(dbConn, exchangeProfileFilter) <= 0)

                //if (AppUtils.checkDuplicate(dbConn, EHSBCExchangeProfile.db, c, errors, "HSBCExchangeProfileRemoteProfileID"))
                c.HSBCExchangeProfileRemoteProfileID = tmpRemoteProfileID;
            else
                tempID++;
        }
        EHSBCExchangeProfile.db.update(dbConn, c);

        view = loadData(info, db, Repeater);
    }

    protected void Repeater_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        HROne.Common.WebUtility.WebControlsLocalization(Session, e.Item.Controls);

        e.Item.FindControl("DeleteItem").Visible = false;//IsAllowEdit;
        if (e.Item.ItemIndex == Repeater.EditItemIndex)
        {
            ebinding = new Binding(dbConn, db);
            ebinding.add((HtmlInputHidden)e.Item.FindControl("HSBCExchangeProfileID"));
            ebinding.add(new CheckBoxBinder(db, (CheckBox)e.Item.FindControl("HSBCExchangeProfileIsLocked")));
            ebinding.init(Request, Session);


            EHSBCExchangeProfile obj = new EHSBCExchangeProfile();
            db.toObject(((DataRowView)e.Item.DataItem).Row, obj);
            Hashtable values = new Hashtable();
            db.populate(obj, values);
            ebinding.toControl(values);
        }
        else
        {
            e.Item.FindControl("Edit").Visible = IsAllowEdit;
            HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("HSBCExchangeProfileID");
            h.Value = ((DataRowView)e.Item.DataItem)["HSBCExchangeProfileID"].ToString();


        }
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
            ebinding.add((HtmlInputHidden)e.Item.FindControl("HSBCExchangeProfileID"));
            ebinding.add(new CheckBoxBinder(db, (CheckBox)e.Item.FindControl("HSBCExchangeProfileIsLocked")));
            ebinding.init(Request, Session);


            EHSBCExchangeProfile obj = new EHSBCExchangeProfile();
            Hashtable values = new Hashtable();

            PageErrors errors = PageErrors.getErrors(db, Page.Master);
            errors.clear();


            ebinding.toValues(values);
            db.validate(errors, values);

            if (!errors.isEmpty())
            {
                return;
            }

            db.parse(values, obj);
            //if (!AppUtils.checkDuplicate(dbConn, db, obj, errors, "PublicHolidayDate"))
            //    return;

            //WebUtils.StartFunction(Session, FUNCTION_CODE);
            db.update(dbConn, obj);
            //WebUtils.EndFunction(dbConn);

            Repeater.EditItemIndex = -1;
            view = loadData(info, db, Repeater);
            WebUtils.SetEnabledControlSection(AddPanel, true);
        }


    }

    protected string CreateRemoteProfileID(string Prefix, int ID)
    {
        string IDString = ID.ToString("000000");

        int checkSum = 0;
        for (int idx = 0; idx < IDString.Length; idx++)
        {
            char digit = IDString[idx];
            int multiplier = (Convert.ToInt32(digit) - 48) * ((idx % 2) + 1);
            checkSum += multiplier / 10 + multiplier % 10;
        }
        checkSum = checkSum % 10;
        IDString += (10 - checkSum) % 10;
        return Prefix + IDString;
    }

}
