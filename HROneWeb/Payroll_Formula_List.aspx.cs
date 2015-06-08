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

public partial class Payroll_Formula_List : HROneWebPage
{
    private const string FUNCTION_CODE = "PAY002";

    protected DBManager db = EPayrollProrataFormula.db;
    protected SearchBinding binding;
    protected ListInfo info;
    protected DataView view;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;
        

        binding = new SearchBinding(dbConn, db);
        binding.add(new LikeSearchBinder(PayFormCode, "PayFormCode"));
        binding.add(new LikeSearchBinder(PayFormDesc, "PayFormDesc"));
        binding.init(DecryptedRequest, null);

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
        info = ListFooter.ListInfo;

        if (!Page.IsPostBack)
        {
            view = loadData(info, db, Repeater);
        }

    }

    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = binding.createFilter();

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);

        string select = "c.*";
        string from = "from [" + db.dbclass.tableName + "] c ";

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
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(db, row, cb);
        cb.Visible = toolBar.DeleteButton_Visible & !row["PayFormIsSys"].ToString().Equals("Y");
    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ArrayList list = WebUtils.SelectedRepeaterItemToBaseObjectList(db, Repeater, "ItemSelect");
        foreach (EPayrollProrataFormula o in list)
        {
            if (EPayrollProrataFormula.db.select(dbConn, o))
            {

                if (IsInUsed(o, errors))
                    return;

                WebUtils.StartFunction(Session, FUNCTION_CODE);
                db.delete(dbConn, o);
                WebUtils.EndFunction(dbConn);
            }
        }
        loadData(info, db, Repeater);
    }
    protected bool IsInUsed(EPayrollProrataFormula o, PageErrors errors)
    {
        int PayrollProrataFormulaID = o.PayFormID;
        DBFilter leaveFormulaFilter = new DBFilter();
        OR orLeaveFormula = new OR();
        orLeaveFormula.add(new Match("LeaveCodeLeaveAllowFormula", PayrollProrataFormulaID));
        orLeaveFormula.add(new Match("LeaveCodeLeaveDeductFormula", PayrollProrataFormulaID));
        leaveFormulaFilter.add(orLeaveFormula);
        ArrayList leaveCodeList = ELeaveCode.db.select(dbConn, leaveFormulaFilter);
        if (leaveCodeList.Count > 0)
        {
            errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_IS_IN_USE, new string[] { o.PayFormCode }));
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
            return true;
        }

        DBFilter payGroupFormulaFilter = new DBFilter();
        OR orPayGroupFormula = new OR();
        orPayGroupFormula.add(new Match("PayGroupDefaultProrataFormula", PayrollProrataFormulaID));
        orPayGroupFormula.add(new Match("PayGroupNewJoinFormula", PayrollProrataFormulaID));
        orPayGroupFormula.add(new Match("PayGroupStatHolAllowFormula", PayrollProrataFormulaID));
        orPayGroupFormula.add(new Match("PayGroupStatHolDeductFormula", PayrollProrataFormulaID));
        orPayGroupFormula.add(new Match("PayGroupTerminatedALCompensationDailyFormula", PayrollProrataFormulaID));
        orPayGroupFormula.add(new Match("PayGroupTerminatedFormula", PayrollProrataFormulaID));
        orPayGroupFormula.add(new Match("PayGroupTerminatedPaymentInLieuDailyFormula", PayrollProrataFormulaID));

        payGroupFormulaFilter.add(orPayGroupFormula);
        ArrayList payGroupList = EPayrollGroup.db.select(dbConn, payGroupFormulaFilter);
        if (payGroupList.Count > 0)
        {
            errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_IS_IN_USE, new string[] { o.PayFormCode }));
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
            return true;
        }

        DBFilter attnFormulaFormulaFilter = new DBFilter();
        OR orAttnFormulaFormula = new OR();
        orAttnFormulaFormula.add(new Match("AttendanceFormulaPayFormID", PayrollProrataFormulaID));

        attnFormulaFormulaFilter.add(orAttnFormulaFormula);
        ArrayList attnFormulaList = EAttendanceFormula.db.select(dbConn, attnFormulaFormulaFilter);
        if (attnFormulaList.Count > 0)
        {
            errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_IS_IN_USE, new string[] { o.PayFormCode }));
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
            return true;
        }
        return false;
    }
    public void New_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_Formula_Edit.aspx");
    }
}
