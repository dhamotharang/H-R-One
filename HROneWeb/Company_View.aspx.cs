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

public partial class Company_View : HROneWebPage
{
    private const string FUNCTION_CODE = "SYS001";

    protected Binding binding;
    protected DBManager db = ECompany.db;
    protected SearchBinding companyBankAccountSearchBinding;
    protected int CurID = -1;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        

        binding = new Binding(dbConn, db);
        binding.add(CompanyID);
        binding.add(CompanyCode);
        binding.add(CompanyName);
        binding.add(CompanyAddress);
        binding.add(CompanyContactPerson);
        binding.add(CompanyContactNo);
        binding.add(CompanyFaxNo);
        binding.add(CompanyBRNo);
        binding.init(Request, Session);

        companyBankAccountSearchBinding = new SearchBinding(dbConn, ECompanyBankAccount.db);
        companyBankAccountSearchBinding.add(new HiddenMatchSearchBinder(CompanyID, "cbam.CompanyID"));
        companyBankAccountSearchBinding.init(DecryptedRequest, null);

        if (!int.TryParse(DecryptedRequest["CompanyID"], out CurID))
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
            LoadCompanyBankAccountList(ListFooter.ListInfo, companyBankAccountRepeater);
        }
    }
    protected bool loadObject() 
    {
        ECompany obj = new ECompany();
	    bool isNew=WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
        if (!db.select(dbConn, obj))
            return false;

	    Hashtable values=new Hashtable();
	    db.populate(obj,values);
	    binding.toControl(values);

        return true;
    }

    public DataView LoadCompanyBankAccountList(ListInfo info, Repeater repeater)
    {
        DBFilter filter = companyBankAccountSearchBinding.createFilter();
        filter.add(new MatchField("cba.CompanyBankAccountID", "cbam.CompanyBankAccountID"));
        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);

        string select = "cba.*";
        string from = "from " + ECompanyBankAccount.db.dbclass.tableName + " cba, "
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

    protected void companyBankAccountRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView row = (DataRowView)e.Item.DataItem;
            CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
            Label CompanyBankAccountIDLabel = (Label)e.Item.FindControl("CompanyBankAccountID");
            WebFormUtils.LoadKeys(ECompanyBankAccount.db, row, cb);

            ECompanyBankAccount companyBankAccount = new ECompanyBankAccount();
            ECompanyBankAccount.db.toObject(row.Row, companyBankAccount);
            CompanyBankAccountIDLabel.Text = string.Format("{0}-{1}-{2} ({3})", new string[] { companyBankAccount.CompanyBankAccountBankCode, companyBankAccount.CompanyBankAccountBranchCode, companyBankAccount.CompanyBankAccountAccountNo, companyBankAccount.CompanyBankAccountHolderName });
        }
    }
    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ECompany c = new ECompany();
        c.CompanyID = CurID;
        if (ECompany.db.select(dbConn, c))
        {
            DBFilter empPosFilter = new DBFilter();
            empPosFilter.add(new Match("CompanyID", c.CompanyID));
            empPosFilter.add("empid", true);
            ArrayList empPosList = EEmpPositionInfo.db.select(dbConn, empPosFilter);
            if (empPosList.Count > 0)
            {
                errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_USED_BY_EMPLOYEE, new string[] { HROne.Common.WebUtility.GetLocalizedString("Company Code"), c.CompanyCode }));
                foreach (EEmpPositionInfo empPos in empPosList)
                {
                    EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                    empInfo.EmpID = empPos.EmpID;
                    if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                        errors.addError("- " + empInfo.EmpNo + ", " + empInfo.EmpEngFullName);
                    else
                        EEmpPositionInfo.db.delete(dbConn, empPos);

                }
                errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
                return;

            }
            else
            {
                WebUtils.StartFunction(Session, FUNCTION_CODE);
                db.delete(dbConn, c);
                EUserCompany.DeleteCompanyForAllUsers(dbConn, c.CompanyID);
                DBFilter companyBankAccountMapFilter = new DBFilter();
                companyBankAccountMapFilter.add(new Match("CompanyID", c.CompanyID));
                ECompanyBankAccountMap.db.delete(dbConn, companyBankAccountMapFilter);
                WebUtils.EndFunction(dbConn);
            }
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Company_List.aspx");
    }

    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Company_Edit.aspx?CompanyID=" + CurID);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Company_List.aspx");
    }
}
