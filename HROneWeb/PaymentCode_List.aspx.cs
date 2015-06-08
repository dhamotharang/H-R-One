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

public partial class PaymentCode_List : HROneWebPage
{
    private const string FUNCTION_CODE = "SYS004";
    protected DBManager db = EPaymentCode.db;
    protected SearchBinding binding;
    protected ListInfo info;
    protected DataView view;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;

        //System.Globalization.CultureInfo ci = HROne.Common.WebUtility.GetSessionCultureInfo(Session);

        toolBar.FunctionCode = FUNCTION_CODE;
        SelectAllPanel.Visible = toolBar.DeleteButton_Visible;

        

        binding = new SearchBinding(dbConn, db);
        binding.add(new LikeSearchBinder(PaymentCode, "PaymentCode"));
        binding.add(new LikeSearchBinder(PaymentCodeDesc, "PaymentCodeDesc"));
        binding.add(new DropDownVLSearchBinder(PaymentTypeID, "c.PaymentTypeID", EPaymentType.VLPaymentType));//.setLocale(ci));
        binding.add(new DropDownVLSearchBinder(PaymentCodeIsMPF, "PaymentCodeIsMPF", Values.VLYesNo));//.setLocale(ci));
        binding.add(new DropDownVLSearchBinder(PaymentCodeIsWages, "PaymentCodeIsWages", Values.VLYesNo));//.setLocale(ci));

        binding.initValues("PaymentCodeIsProrata", null, Values.VLYesNo, null);
        binding.initValues("PaymentCodeIsWages", null, Values.VLYesNo, null);
        binding.initValues("PaymentCodeIsMPF", null, Values.VLYesNo, null);
        binding.initValues("PaymentCodeIsTopUp", null, Values.VLYesNo, null);
        binding.initValues("PaymentCodeIsORSO", null, Values.VLYesNo, null);

        
            
        
        binding.init(DecryptedRequest, null);

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
        info = ListFooter.ListInfo;
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            view = loadData(info, db, Repeater);
        }
        
    }
    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = binding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = "c.*, t.PaymentTypeDesc";
        string from = "from " + db.dbclass.tableName + " c LEFT OUTER JOIN "+EPaymentType.db.dbclass.tableName+" t on c.PaymentTypeID=t.PaymentTypeID";

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);

        ListFooter.Refresh();

        view = new DataView(table);
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

        PaymentTypeID.SelectedIndex = 0;
        PaymentCodeIsMPF.SelectedIndex = 0;
        PaymentCodeIsWages.SelectedIndex = 0;

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
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(db, row, cb);
    }
    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ArrayList list = WebUtils.SelectedRepeaterItemToBaseObjectList(db, Repeater, "ItemSelect");

        foreach (EPaymentCode obj in list)
        {
            db.select(dbConn, obj);
            DBFilter paymentRecordFilter = new DBFilter();
            paymentRecordFilter.add(new Match("PaymentCodeID", obj.PaymentCodeID));

            IN inTerms = new IN("EmpPayrollID", "Select EmpPayrollID From " + EPaymentRecord.db.dbclass.tableName, paymentRecordFilter);

            DBFilter empPayrollFilter = new DBFilter();
            empPayrollFilter.add(inTerms);
            empPayrollFilter.add("empid", true);
            ArrayList empPayrollList = EEmpPayroll.db.select(dbConn, empPayrollFilter);
            if (empPayrollList.Count > 0)
            {
                int lastEmpID = 0;
                errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_USED_BY_EMPLOYEE, new string[] { HROne.Common.WebUtility.GetLocalizedString("Payment Code"), obj.PaymentCode }));
                foreach (EEmpPayroll empPayroll in empPayrollList)
                {
                    if (lastEmpID != empPayroll.EmpID)
                    {
                        EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                        empInfo.EmpID = empPayroll.EmpID;
                        if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                        {
                            errors.addError("- " + empInfo.EmpNo + ", " + empInfo.EmpEngFullName);
                            lastEmpID = empPayroll.EmpID;
                        }
                        else
                        {
                            EEmpPayroll.db.delete(dbConn, empPayroll);
                        }
                    }
                } 
                errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
            }
            else
            {
                WebUtils.StartFunction(Session, FUNCTION_CODE);
                db.delete(dbConn, obj);
                WebUtils.EndFunction(dbConn);
            }
        }
        loadData(info, db, Repeater);
    }

    public void New_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "PaymentCode_Edit.aspx");
    }
}
