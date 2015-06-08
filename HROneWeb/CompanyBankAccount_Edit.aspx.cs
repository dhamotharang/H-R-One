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
using HROne.Translation;

public partial class CompanyBankAccount_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "SYS001-1";

    protected Binding binding;
    protected DBManager db = ECompanyBankAccount.db;
    protected SearchBinding companySearchBinding;
    protected int CurID = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;



        binding = new Binding(dbConn, db);
        binding.add(CompanyBankAccountID);
        binding.add(CompanyBankAccountHolderName);
        binding.add(CompanyBankAccountAccountNo);
        binding.add(CompanyBankAccountBankCode);
        binding.add(CompanyBankAccountBranchCode);
        binding.init(Request, Session);

        companySearchBinding = new SearchBinding(dbConn, ECompany.db);
        companySearchBinding.add(new HiddenMatchSearchBinder(CompanyBankAccountID, "cbam.CompanyBankAccountID"));
        companySearchBinding.init(DecryptedRequest, null);

        if (!int.TryParse(DecryptedRequest["CompanyBankAccountID"], out CurID))
            CurID = -1;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (!Page.IsPostBack)
        {
            if (CurID > 0)
            {
                loadObject();

            }
            else
                toolBar.DeleteButton_Visible = false;
            LoadCompanyList(ListFooter.ListInfo, companyRepeater);
        }
    }
    protected bool loadObject()
    {
        ECompanyBankAccount obj = new ECompanyBankAccount();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
        if (!db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);

        return true;
    }

    public DataView LoadCompanyList(ListInfo info, Repeater repeater)
    {
        DBFilter filter = new DBFilter();
        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);

        string select = "c.*";
        string from = "from " + ECompany.db.dbclass.tableName + " c  ";

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);


        DataView view = new DataView(table);
        ListFooter.Refresh();

        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }

        return view;
    }

    protected void companyRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView row = (DataRowView)e.Item.DataItem;
            CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
            WebFormUtils.LoadKeys(ECompany.db, row, cb);

            ECompany company = new ECompany();
            ECompany.db.toObject(row.Row, company);
            cb.Text = string.Format("{0} - {1}", new string[] { company.CompanyCode, company.CompanyName });

            DBFilter companyBankAccountMapFilter = new DBFilter();
            companyBankAccountMapFilter.add(new Match("CompanyID", company.CompanyID));
            companyBankAccountMapFilter.add(new Match("CompanyBankAccountID",CurID));
            if (ECompanyBankAccountMap.db.count(dbConn, companyBankAccountMapFilter) > 0)
                cb.Checked = true;
            else
                cb.Checked = false;
        }
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        ECompanyBankAccount c = new ECompanyBankAccount();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);


        if (!errors.isEmpty())
            return;
        db.parse(values, c);

        if (c.CompanyBankAccountAccountNo.Length > 0)
            if (!c.CompanyBankAccountBankCode.Equals("152") && !c.CompanyBankAccountBankCode.Equals("055"))
            {
                if (c.CompanyBankAccountAccountNo.Length > 9)
                    errors.addError("CompanyBankAccountAccountNo", "validate.maxlength", 9);
                long dummyValue;
                if (!long.TryParse(c.CompanyBankAccountAccountNo, out dummyValue))
                    errors.addError("CompanyBankAccountAccountNo", "validate.int");
            }

        if (!errors.isEmpty())
            return;

        WebUtils.StartFunction(Session, FUNCTION_CODE);

        if (CurID < 0)
        {
            //            Utils.MarkCreate(Session, c);

            db.insert(dbConn, c);
            CurID = c.CompanyBankAccountID;
            //            url = Utils.BuildURL(-1, CurID);
        }
        else
        {
            //            Utils.Mark(Session, c);
            db.update(dbConn, c);
        }

        ArrayList selectedList = new ArrayList();
        ArrayList unselectedList = new ArrayList();

        foreach (RepeaterItem i in companyRepeater.Items)
        {
            CheckBox cb = (CheckBox)i.FindControl("ItemSelect");
            ECompany o = new ECompany();
            WebFormUtils.GetKeys(ECompany.db, o, cb);
            if (cb.Checked)
                selectedList.Add(o);
            else
                unselectedList.Add(o);

        }
        foreach (ECompany o in selectedList)
        {
            DBFilter companyBankAccountMapFilter = new DBFilter();
            companyBankAccountMapFilter.add(new Match("CompanyID", o.CompanyID));
            companyBankAccountMapFilter.add(new Match("CompanyBankAccountID", CurID));
            ArrayList companyBankAccountMapList = ECompanyBankAccountMap.db.select(dbConn, companyBankAccountMapFilter);
            if (companyBankAccountMapList.Count == 0)
            {
                ECompanyBankAccountMap companyBankAccountMap = new ECompanyBankAccountMap();
                companyBankAccountMap.CompanyID = o.CompanyID;
                companyBankAccountMap.CompanyBankAccountID = CurID;
                ECompanyBankAccountMap.db.insert(dbConn, companyBankAccountMap);
            }
        }

        foreach (ECompany o in unselectedList)
        {
            DBFilter companyBankAccountMapFilter = new DBFilter();
            companyBankAccountMapFilter.add(new Match("CompanyID", o.CompanyID));
            companyBankAccountMapFilter.add(new Match("CompanyBankAccountID", CurID));
            ArrayList companyBankAccountMapList = ECompanyBankAccountMap.db.select(dbConn, companyBankAccountMapFilter);
            if (companyBankAccountMapList.Count != 0)
            {
                foreach (ECompanyBankAccountMap companyBankAccountMap in companyBankAccountMapList)
                    ECompanyBankAccountMap.db.delete(dbConn, companyBankAccountMap);
            }
        }
        WebUtils.EndFunction(dbConn);
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "CompanyBankAccount_View.aspx?CompanyBankAccountID=" + CurID);
    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ECompanyBankAccount c = new ECompanyBankAccount();
        c.CompanyBankAccountID = CurID;
        if (db.select(dbConn, c))
        {
            WebUtils.StartFunction(Session, FUNCTION_CODE);
            db.delete(dbConn, c);
            DBFilter companyBankAccountMapFilter = new DBFilter();
            companyBankAccountMapFilter.add(new Match("CompanyBankAccountID", c.CompanyBankAccountID));
            ECompanyBankAccountMap.db.delete(dbConn, companyBankAccountMapFilter);
            WebUtils.EndFunction(dbConn);
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "CompanyBankAccount_List.aspx");
    }
    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "CompanyBankAccount_View.aspx?CompanyBankAccountID=" + CurID);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "CompanyBankAccount_List.aspx");

    }

}
