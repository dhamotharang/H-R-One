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

public partial class Taxation_Company_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "TAX001";
    public Binding binding;
    public DBManager db = ETaxCompany.db;
    public int CurID = -1;
    protected SearchBinding sbinding;
    protected ListInfo info;
    private DataView view;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        binding = new Binding(dbConn, db);
        binding.add(TaxCompID);
        binding.add(TaxCompEmployerName);
        binding.add(TaxCompEmployerAddress);
        binding.add(TaxCompSection);
        binding.add(TaxCompERN);
        binding.add(TaxCompDesignation);

        binding.init(Request, Session);

        sbinding = new SearchBinding(dbConn, ECompany.db);
        sbinding.init(DecryptedRequest, null);

        if (!int.TryParse(DecryptedRequest["TaxCompID"], out CurID))
            CurID = -1;

        info=ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            view = loadData(info, db, Repeater);
            if (CurID > 0)
            {
                loadObject();
            }
            else
                toolBar.DeleteButton_Visible = false;

        }

    }


    protected bool loadObject()
    {
        ETaxCompany obj = new ETaxCompany();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
        if (!db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);
        return true;
    }
    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = sbinding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = " c.* ";
        string from = "from  Company c ";

        DBFilter nottaxCompanyMapFilter = new DBFilter();
        nottaxCompanyMapFilter.add(new Match("TaxCompID", "<>", CurID));
        IN notinTerms = new IN("NOT CompanyID", "select CompanyID from TaxCompanyMap", nottaxCompanyMapFilter);
        DBFilter taxCompanyMapFilter = new DBFilter();
        taxCompanyMapFilter.add(new Match("TaxCompID", CurID));
        IN inTerms = new IN("CompanyID", "select CompanyID from TaxCompanyMap", taxCompanyMapFilter);

        OR orTerms = new OR();
        orTerms.add(notinTerms);
        orTerms.add(inTerms);
        filter.add(orTerms);
        filter.add("CompanyCode", true);

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);

        //if (info != null)
        //{


        //    info.loadPageList(null, PrevPage, NextPage, FirstPage, LastPage);
        //    WebUtils.loadPageList(PrevPage, PrevPageImg, NextPage, NextPageImg, FirstPage, FirstPageImg, LastPage, LastPageImg);

        //    CurPage.Value = info.page.ToString();
        //    NumPage.Value = info.numPage.ToString();
        //}

        view = new DataView(table);

        if (repeater != null)
        {
            repeater.DataSource = table;
            repeater.DataBind();
        }

        return view;
    }
    protected void FirstPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);

    }
    protected void PrevPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);

    }
    protected void NextPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);

    }
    protected void LastPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);

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

        view = loadData(info, db, Repeater);

    }
    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(ECompany.db, row, cb);



        //if (!Page.IsPostBack)
        //{
            DBFilter taxCompanyMapFilter = new DBFilter();
            taxCompanyMapFilter.add(new Match("TaxCompID", CurID));
            taxCompanyMapFilter.add(new Match("CompanyID", row["CompanyID"]));
            ArrayList taxPaymentMaps = ETaxCompanyMap.db.select(dbConn, taxCompanyMapFilter);
            if (taxPaymentMaps.Count != 0)
                cb.Checked = true;
        //}
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        ETaxCompany c = new ETaxCompany();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
        {
            return;
        }

        db.parse(values, c);


        if (!errors.isEmpty())
        {
            return;
        }

        WebUtils.StartFunction(Session, FUNCTION_CODE);
        if (CurID < 0)
        {
            //            Utils.MarkCreate(Session, c);

            db.insert(dbConn, c);
            CurID = c.TaxCompID;
            //            url = Utils.BuildURL(-1, CurID);
        }
        else
        {
            //            Utils.Mark(Session, c);
            db.update(dbConn, c);
        }


        ArrayList selectedList = new ArrayList();
        ArrayList unselectedList = new ArrayList();
        foreach (RepeaterItem i in Repeater.Items)
        {
            CheckBox cb = (CheckBox)i.FindControl("ItemSelect");
            BaseObject o = new ECompany();
            WebFormUtils.GetKeys(ECompany.db, o, cb);
            if (cb.Checked)
                selectedList.Add(o);
            else
                unselectedList.Add(o);

        }

        foreach (ECompany o in selectedList)
        {
            DBFilter taxCompanyMapFilter = new DBFilter();
            taxCompanyMapFilter.add(new Match("TaxCompID", c.TaxCompID));
            taxCompanyMapFilter.add(new Match("CompanyID", o.CompanyID ));
            ArrayList taxCompanyMaps = ETaxCompanyMap.db.select(dbConn, taxCompanyMapFilter);
            if (taxCompanyMaps.Count == 0)
            {
                ETaxCompanyMap taxCompanytMap = new ETaxCompanyMap();
                taxCompanytMap.TaxCompID = c.TaxCompID;
                taxCompanytMap.CompanyID = o.CompanyID;
                ETaxCompanyMap.db.insert(dbConn, taxCompanytMap);
            }
        }

        foreach (ECompany o in unselectedList)
        {
            DBFilter taxCompanyMapFilter = new DBFilter();
            taxCompanyMapFilter.add(new Match("TaxCompID", c.TaxCompID));
            taxCompanyMapFilter.add(new Match("CompanyID", o.CompanyID));
            ArrayList taxCompanyMaps = ETaxCompanyMap.db.select(dbConn, taxCompanyMapFilter);
            if (taxCompanyMaps.Count != 0)
            {
                foreach (ETaxCompanyMap taxCompanytMap in taxCompanyMaps)
                    ETaxCompanyMap.db.delete(dbConn, taxCompanytMap);
            }
        }
        WebUtils.EndFunction(dbConn);
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Taxation_Company_View.aspx?TaxCompID=" + c.TaxCompID );
    }

//    protected void Save_Click(object sender, EventArgs e)
//    {
//        ETaxCompany c = new ETaxCompany();

//        Hashtable values = new Hashtable();
//        binding.toValues(values);

//        PageErrors errors = PageErrors.getErrors(db, Page.Master);
//        errors.clear();


//        db.validate(errors, values);

//        if (!errors.isEmpty())
//            return;


//        db.parse(values, c);


//        if (!errors.isEmpty())
//            return;


//        if (CurID < 0)
//        {
//            //            Utils.MarkCreate(Session, c);

//            db.insert(dbConn, c);
//            CurID = c.TaxCompID;
//            //            url = Utils.BuildURL(-1, CurID);
//        }
//        else
//        {
//            //            Utils.Mark(Session, c);
//            db.update(dbConn, c);
//        }

////        Response.Redirect(Request.Url.LocalPath + "?PayGroupID=" + CurID);
//        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Taxation_Company_View.aspx?TaxCompID=" + CurID);


//    }
    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        DBFilter taxCompanyFilter = new DBFilter();
        taxCompanyFilter.add(new Match("TaxCompID", CurID));

        DBFilter taxFormFilter = new DBFilter();
        taxFormFilter.add(new IN("taxFormID", "Select taxFormID from " + ETaxForm.db.dbclass.tableName + " tf", taxCompanyFilter));

        if (ETaxEmp.db.count(dbConn, taxFormFilter) > 0)
            errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_IS_IN_USE, new string[] { TaxCompEmployerName.Text }));

        if (!errors.isEmpty())
        {
            view = loadData(info, ETaxCompanyMap.db, Repeater);
            return;
        }
        ETaxCompany c = new ETaxCompany();
        c.TaxCompID = CurID;
        if (ETaxCompany.db.select(dbConn, c))
        {
            WebUtils.StartFunction(Session, FUNCTION_CODE);
            db.delete(dbConn, c);

            ArrayList taxCompanyMapList = ETaxCompanyMap.db.select(dbConn, taxCompanyFilter);
            foreach (ETaxCompanyMap taxCompanyMap in taxCompanyMapList)
                ETaxCompanyMap.db.delete(dbConn, taxCompanyMap);

            WebUtils.EndFunction(dbConn);
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Taxation_Company_List.aspx");
    }
    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Taxation_Company_View.aspx?TaxCompID=" + CurID);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Taxation_Company_List.aspx");

    }
}
