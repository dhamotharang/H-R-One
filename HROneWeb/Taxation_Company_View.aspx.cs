using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using HROne.DataAccess;
//using perspectivemind.validation;
using HROne.Lib.Entities;

public partial class Taxation_Company_View : HROneWebPage
{
    private const string FUNCTION_CODE = "TAX001";
    public Binding binding;
    public DBManager db = ETaxCompany.db;
    public int CurID = -1;
    public int CurPayPeriodID = -1;

    protected SearchBinding sbinding;
    protected ListInfo info;
    private DataView view;
    
    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        binding = new Binding(dbConn, db);
        binding.add(TaxCompID );
        binding.add(TaxCompEmployerName);
        binding.add(TaxCompEmployerAddress);
        binding.add(TaxCompSection);
        binding.add(TaxCompERN);
        binding.add(TaxCompDesignation);
        binding.init(Request, Session);
 
        sbinding = new SearchBinding(dbConn, ECompany.db);
        sbinding.add(new HiddenMatchSearchBinder(TaxCompID));
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
            if (CurID > 0)
            {
                view = loadData(info, ETaxCompanyMap.db, Repeater);
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

        string select = " c.* , tcm.*";
        string from = "from [" + db.dbclass.tableName + "] tcm, Company c ";

        filter.add(new MatchField("tcm.CompanyID", "c.CompanyID"));

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);

        ListFooter.Refresh();
        
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
        view = loadData(info, ETaxCompanyMap.db, Repeater);

    }
    protected void PrevPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, ETaxCompanyMap.db, Repeater);

    }
    protected void NextPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, ETaxCompanyMap.db, Repeater);

    }
    protected void LastPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, ETaxCompanyMap.db, Repeater);

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

        view = loadData(info, ETaxCompanyMap.db, Repeater);

    }
    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(db, row, cb);
    }
    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Taxation_Company_Edit.aspx?TaxCompID=" + CurID);
    }
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
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Taxation_Company_List.aspx");
    }
}
