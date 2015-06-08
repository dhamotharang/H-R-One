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
using HROne.Translation;
using HROne.Lib.Entities;

public partial class Company_List : HROneWebPage
{
    private const string FUNCTION_CODE = "SYS001";

    protected DBManager db = ECompany.db;
    protected SearchBinding binding;
    protected ListInfo info;
    protected DataView view;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       

        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;
        SelectAllPanel.Visible = toolBar.DeleteButton_Visible;



        binding = new SearchBinding(dbConn, db);
        binding.add(new LikeSearchBinder(CompanyCode, "CompanyCode"));
        binding.add(new LikeSearchBinder(CompanyName, "CompanyName"));
        binding.add(new LikeSearchBinder(CompanyAddress, "CompanyAddress"));
        binding.add(new LikeSearchBinder(CompanyContactPerson, "CompanyContactPerson"));
        binding.add(new LikeSearchBinder(CompanyBRNo, "CompanyBRNo"));
        binding.init(DecryptedRequest, null);

        HROne.Common.WebUtility.WebControlsLocalization(this, Page.Controls);

        info = ListFooter.ListInfo;

        if (!Page.IsPostBack)
        {
            view = loadData(info, db, Repeater);
        }

        if (WebUtils.TotalActiveCompany(dbConn, 0) >= WebUtils.productLicense(Session).NumOfCompanies)
            toolBar.NewButton_Visible = false;
        else
            toolBar.NewButton_Visible = toolBar.DeleteButton_Visible;

    }
    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = binding.createFilter();

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);

        string select = "c.*";
        string from = "from " + db.dbclass.tableName + " c";

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

    protected void Search_Click(object sender, EventArgs e)
    {
        info.page = 0;
        view = loadData(info, db, Repeater);

    }
    protected void Reset_Click(object sender, EventArgs e)
    {
        binding.clear();
        info.page = 0;
        view = loadData(info, db, Repeater);

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
        DataRowView row=(DataRowView)e.Item.DataItem;
        CheckBox cb=(CheckBox) e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(db, row, cb);
        cb.Visible = cb.Visible & toolBar.DeleteButton_Visible;
    }
    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ArrayList list = new ArrayList();
        foreach (RepeaterItem i in Repeater.Items)
        {
            CheckBox cb=(CheckBox) i.FindControl("ItemSelect");
            if (cb.Checked)
            {
                ECompany o=new ECompany();
                WebFormUtils.GetKeys(db, o, cb);
                list.Add(o);
            }
            
        }
        foreach (ECompany o in list)
        {
            if (ECompany.db.select(dbConn, o))
            {
                DBFilter empPosFilter = new DBFilter();
                empPosFilter.add(new Match("CompanyID", o.CompanyID));
                empPosFilter.add("empid", true);
                ArrayList empPosList = EEmpPositionInfo.db.select(dbConn, empPosFilter);
                if (empPosList.Count > 0)
                {
                    errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_USED_BY_EMPLOYEE, new string[] { HROne.Common.WebUtility.GetLocalizedString("Company Code"), o.CompanyCode }));
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

                }
                else
                {
                    WebUtils.StartFunction(Session, FUNCTION_CODE);
                    db.delete(dbConn, o);
                    EUserCompany.DeleteCompanyForAllUsers(dbConn, o.CompanyID);
                    DBFilter companyBankAccountMapFilter = new DBFilter();
                    companyBankAccountMapFilter.add(new Match("CompanyID", o.CompanyID));
                    ECompanyBankAccountMap.db.delete(dbConn, companyBankAccountMapFilter);
                    WebUtils.EndFunction(dbConn);
                }
            }
        }
        loadData(info, db, Repeater);
    }
    public void New_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Company_Edit.aspx");
    }
}
