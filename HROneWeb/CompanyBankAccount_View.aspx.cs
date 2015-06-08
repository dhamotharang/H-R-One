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

public partial class CompanyBankAccount_View : HROneWebPage
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
        companySearchBinding.add(new HiddenMatchSearchBinder(CompanyBankAccountID,"cbam.CompanyBankAccountID"));
        companySearchBinding.init(DecryptedRequest, null);

        if (!int.TryParse(DecryptedRequest["CompanyBankAccountID"], out CurID))
            CurID = -1;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

		if(!Page.IsPostBack) 
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
	    bool isNew=WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
        if (!db.select(dbConn, obj))
            return false;

	    Hashtable values=new Hashtable();
	    db.populate(obj,values);
	    binding.toControl(values);

        return true;
    }

    public DataView LoadCompanyList(ListInfo info, Repeater repeater)
    {
        DBFilter filter = companySearchBinding.createFilter();
        filter.add(new MatchField("c.CompanyID", "cbam.CompanyID"));
        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);

        string select = "c.*";
        string from = "from " + ECompany.db.dbclass.tableName + " c, " 
            + ECompanyBankAccountMap.db.dbclass.tableName + " cbam ";

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
            Label CompanyIDLabel = (Label)e.Item.FindControl("CompanyID");
            WebFormUtils.LoadKeys(ECompany.db, row, cb);

            ECompany company = new ECompany();
            ECompany.db.toObject(row.Row, company);
            CompanyIDLabel.Text = string.Format("{0} - {1}", new string[] { company.CompanyCode, company.CompanyName });
        }
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

    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "CompanyBankAccount_Edit.aspx?CompanyBankAccountID=" + CurID);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "CompanyBankAccount_List.aspx");
    }
}
