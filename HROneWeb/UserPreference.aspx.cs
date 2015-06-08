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

public partial class UserPreference : HROneWebPage
{
    private int CurID;
    protected SearchBinding reminderBinding;
    protected ListInfo reminderInfo;
    private DataView reminderView;
    HROne.ProductLicense productLicense = null;

    //protected void Page_Init(object sender, EventArgs e)
    //{
    //    EUser defaultUser = (HROne.Lib.Entities.EUser)Session["User"];
    //    if (defaultUser != null)
    //    {
    //        EUser user = new EUser();
    //        user.UserID = defaultUser.UserID;
    //        user.UserLanguage = cboLanguage.SelectedValue;
    //        WebUtils.SetSessionLanguage(Session, user);

    //        HROne.Common.WebUtility.initLanguage(Session);
    //    }
    //}


    protected void Page_Load(object sender, EventArgs e)
    {

        if (!WebUtils.CheckAccess(Response, Session))
            return;


        reminderBinding = new SearchBinding(dbConn, EReminderType.db);
        EUser user = WebUtils.GetCurUser(this.Session);
        if (user != null)
            CurID = user.UserID;

        string selectedLanguage = cboLanguage.SelectedValue;
        cboLanguage.Items.Clear();
        cboLanguage.Items.Add(new ListItem("System Default", ""));
        WebUtils.AddLanguageOptionstoDropDownList(cboLanguage);
        cboLanguage.SelectedIndex = cboLanguage.Items.IndexOf(cboLanguage.Items.FindByValue(selectedLanguage));
        reminderInfo = ListFooter.ListInfo;

        productLicense = WebUtils.productLicense(Session);

        if (productLicense.ProductType != HROne.ProductLicense.ProductLicenseType.HROne)
        {
            UserIsKeepConnectedRow.Visible = false;
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
        if (!Page.IsPostBack)
        {
            if (CurID > 0)
            {
                loadObject();
                reminderView = loadReminderData(reminderInfo, EReminderType.db, ReminderRepeater);
            }

        }

    }

    protected bool loadObject()
    {
        EUser user = new EUser();
        user.UserID = CurID;
        if (EUser.db.select(dbConn, user))
        {
            cboLanguage.SelectedValue = user.UserLanguage;
            UserIsKeepConnected.Checked = user.UserIsKeepConnected;
        }
        else
            return false;
        //ETaxPayment obj = new ETaxPayment();
        //bool isNew = WebFormWorkers.loadKeys(ETaxPayment.db, obj, DecryptedRequest);
        //if (!ETaxPayment.db.select(dbConn, obj))
        //    return false;

        //Hashtable values = new Hashtable();
        //db.populate(obj, values);
        //binding.toControl(values);

        //if (obj.TaxPayCode.Length == 2)   // a.k.a k1,k2,k3 with nature
        //    TaxPayNature.Visible = true;
        //else
        //    TaxPayNature.Visible = false;


        return true;
    }

    public DataView loadReminderData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = reminderBinding.createFilter();

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);

        string select = " * ";
        string from = "from  " + EReminderType.db.dbclass.tableName ;

        if (productLicense.ProductType != HROne.ProductLicense.ProductLicenseType.HROne)
        {
            filter.add(new Match("ReminderTypeCode", "<>", EInbox.INBOX_TYPE_WORKPERMITEXPIRY));
        }

        filter.add("ReminderTypeCode", true);

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);

        reminderView = new DataView(table);
        //if (info != null)
        //{
        //    info.loadPageList(null, PrevPage, NextPage, FirstPage, LastPage);

        //    CurPage.Value = info.page.ToString();
        //    NumPage.Value = info.numPage.ToString();
        //}
        if (repeater != null)
        {
            repeater.DataSource = table;
            repeater.DataBind();
        }
        //lblEmpRecordCount.Text = view.Count.ToString();

        return reminderView;
    }
    //protected void FirstPage_Click(object sender, EventArgs e)
    //{
    //    loadState();
    //    info.page = 0;
    //    view = loadData(info, db, Repeater);

    //}
    //protected void PrevPage_Click(object sender, EventArgs e)
    //{
    //    loadState();
    //    info.page--;
    //    view = loadData(info, db, Repeater);

    //}
    //protected void NextPage_Click(object sender, EventArgs e)
    //{
    //    loadState();
    //    info.page++;
    //    view = loadData(info, db, Repeater);

    //}
    //protected void LastPage_Click(object sender, EventArgs e)
    //{
    //    loadState();

    //    info.page = Int32.Parse(NumPage.Value);
    //    view = loadData(info, db, Repeater);

    //}
    protected void ChangeOrder_Click(object sender, EventArgs e)
    {
        LinkButton l = (LinkButton)sender;
        String id = l.ID.Substring(1);
        if (reminderInfo.orderby == null)
            reminderInfo.order = true;
        else if (reminderInfo.orderby.Equals(id))
            reminderInfo.order = !reminderInfo.order;
        else
            reminderInfo.order = true;
        reminderInfo.orderby = id;

        reminderView = loadReminderData(reminderInfo, EReminderType.db, ReminderRepeater);

    }
    protected void ReminderRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(EReminderType.db, row, cb);

        ((Label)e.Item.FindControl("ReminderTypeDesc")).Text = row["ReminderTypeDesc"].ToString();

        HROne.Common.WebUtility.WebControlsLocalization(this, e.Item.Controls);

        if (!Page.IsPostBack)
        {
            DBFilter userReminderOptionFilter = new DBFilter();
            userReminderOptionFilter.add(new Match("UserID", CurID));
            userReminderOptionFilter.add(new Match("ReminderTypeID", row["ReminderTypeID"]));

            ArrayList userReminderList = EUserReminderOption.db.select(dbConn, userReminderOptionFilter);
            if (userReminderList.Count > 0)
            {
                cb.Checked = true;
                EUserReminderOption userReminder = (EUserReminderOption)userReminderList[0];
                if (userReminder.UserReminderOptionRemindDaysBefore >= 0)
                    ((TextBox)e.Item.FindControl("UserReminderOptionRemindDaysBefore")).Text = userReminder.UserReminderOptionRemindDaysBefore.ToString();
                if (userReminder.UserReminderOptionRemindDaysAfter >= 0)
                    ((TextBox)e.Item.FindControl("UserReminderOptionRemindDaysAfter")).Text = userReminder.UserReminderOptionRemindDaysAfter.ToString();
            }
        }
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        EUser c = new EUser();

        c.UserID = CurID;
        c.UserLanguage = cboLanguage.SelectedValue;
        c.UserIsKeepConnected = UserIsKeepConnected.Checked;
        //Hashtable values = new Hashtable();
        //binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(EUser.db, Page.Master);
        errors.clear();


        //db.validate(errors, values);


        //db.parse(values, c);


        ArrayList selectedList = new ArrayList();
        ArrayList unselectedList = new ArrayList();

        foreach (RepeaterItem i in ReminderRepeater.Items)
        {
            CheckBox cb = (CheckBox)i.FindControl("ItemSelect");

            TextBox txtReminderBefore = (TextBox)i.FindControl("UserReminderOptionRemindDaysBefore");
            TextBox txtReminderAfter = (TextBox)i.FindControl("UserReminderOptionRemindDaysAfter");


            EReminderType o = new EReminderType();
            WebFormUtils.GetKeys(EReminderType.db, o, cb);
            if (EReminderType.db.select(dbConn, o))
            {
                if (cb.Checked)
                {
                    EUserReminderOption userReminderOption = new EUserReminderOption();
                    userReminderOption.UserID = CurID;
                    userReminderOption.ReminderTypeID = o.ReminderTypeID;

                    int ReminderDaysBefore = 0, ReminderDaysAfter = 0;
                    if (int.TryParse(txtReminderBefore.Text, out ReminderDaysBefore) && int.TryParse(txtReminderAfter.Text, out ReminderDaysAfter))
                    {
                        userReminderOption.UserReminderOptionRemindDaysAfter = ReminderDaysAfter;
                        userReminderOption.UserReminderOptionRemindDaysBefore = ReminderDaysBefore;
                        selectedList.Add(userReminderOption);
                    }
                    else
                        errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_REMINDER_DAYS_NOT_NUMERIC, new string[] { o.ReminderTypeDesc }));
                }
                else
                    unselectedList.Add(o);
            }
        }

        if (!errors.isEmpty())
        {
            //if (CurID > 0)
            //{
            //    loadObject();
            //    reminderView = loadReminderData(reminderInfo, EReminderType.db, ReminderRepeater);
            //}
            return;
        }

        foreach (EUserReminderOption o in selectedList)
        {
            DBFilter userReminderOptionFilter = new DBFilter();
            userReminderOptionFilter.add(new Match("UserID", CurID));
            userReminderOptionFilter.add(new Match("ReminderTypeID", o.ReminderTypeID));

            ArrayList list = EUserReminderOption.db.select(dbConn, userReminderOptionFilter);
            if (list.Count <= 0)
            {
                EUserReminderOption.db.insert(dbConn, o);
            }
            else
            {
                int count = 0;
                foreach (EUserReminderOption userReminderOption in list)
                {
                    if (count == 0)
                    {
                        userReminderOption.UserReminderOptionRemindDaysBefore = o.UserReminderOptionRemindDaysBefore;
                        userReminderOption.UserReminderOptionRemindDaysAfter = o.UserReminderOptionRemindDaysAfter;
                        EUserReminderOption.db.update(dbConn, userReminderOption);
                    }
                    else
                        EUserReminderOption.db.delete(dbConn, userReminderOption);
                    count++;
                }

            }
        }

        foreach (EReminderType o in unselectedList)
        {
            DBFilter userReminderOptionFilter = new DBFilter();
            userReminderOptionFilter.add(new Match("UserID", CurID));
            userReminderOptionFilter.add(new Match("ReminderTypeID", o.ReminderTypeID));
            ArrayList userReminderOptionList = EUserReminderOption.db.select(dbConn, userReminderOptionFilter);
            foreach (EUserReminderOption userReminderOption in userReminderOptionList)
                EUserReminderOption.db.delete(dbConn, userReminderOption);
        }

        //WebUtils.StartFunction(Session, FUNCTION_CODE);
        EUser.db.update(dbConn, c);

        WebUtils.SetSessionLanguage(Session, WebUtils.GetCurUser(Session));
        HROne.Common.WebUtility.initLanguage(Session);

        errors.addError(HROne.Common.WebUtility.GetLocalizedString("Updated Successful"));

        //HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Default.aspx");
        //WebUtils.EndFunction(dbConn);


//        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Taxation_PaymentMapping_View.aspx?TaxFormType=" + TaxFormType.Text + "&TaxPayID=" + c.TaxPayID);
    }
    protected void Back_Click(object sender, EventArgs e)
    {
//        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Taxation_PaymentMapping_View.aspx?TaxFormType=" + TaxFormType.Text + "&TaxPayID=" + TaxPayID.Value);
    }
}

