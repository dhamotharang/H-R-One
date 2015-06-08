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

public partial class Payroll_HistoryAdjust_List : HROneWebPage
{
    protected DBManager db = EEmpPersonalInfo.db;
    protected SearchBinding binding;
    protected ListInfo info;
    protected DataView view;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, "PAY009", WebUtils.AccessLevel.Read))
            return;

        
        binding = new SearchBinding(dbConn, db);
        //binding.add(new LikeSearchBinder(EmpNo, "EmpNo"));
        //binding.add(new LikeSearchBinder(EmpName, "EmpEngSurname", "EmpEngOtherName", "EmpChiFullName"));
        //binding.add(new FieldDateRangeSearchBinder(JoinDateFrom, JoinDateTo, "EmpDateOfJoin").setUseCurDate(false));
        //binding.add(new DropDownVLSearchBinder(EmpStatus, "EmpStatus", EEmpPersonalInfo.VLEmpStatus).setLocale(ci));
        //binding.add(new DropDownVLSearchBinder(PayGroup, "pp.PayGroupID", EPayrollGroup.VLPayrollGroup).setLocale(ci));
       
        binding.init(DecryptedRequest, null);

        info = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(Page, Page.Controls);

        if (!Page.IsPostBack)
        {
            //view = loadEmpData(info, db, Repeater);
        }
    }
    public DataView loadEmpData(ListInfo info, DBManager db, Repeater repeater)
    {
        WebDatePicker PayPeriodAsOFDate = (WebDatePicker)EmployeeSearchControl1.AdditionElementControl.FindControl("PayPeriodAsOFDate");

        DBFilter filter = binding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = "e.*, ep.*, pp.PayPeriodFr, pp.PayperiodTo, pp.PayPeriodConfirmDate";
        string from = "from [" + db.dbclass.tableName + "] e, EmpPayroll ep, PayrollPeriod pp ";

        filter.add(new MatchField("e.EmpID", "ep.EmpID"));
        filter.add(new MatchField("ep.PayperiodID", "pp.PayperiodID"));
        filter.add(new Match("ep.EmpPayStatus", "C"));
        filter.add(WebUtils.AddRankFilter(Session, "e.EmpID", true));

        DateTime dtPayPeriodAsOFDate;
        if (DateTime.TryParse(PayPeriodAsOFDate.Value, out dtPayPeriodAsOFDate))
        {
            filter.add(new Match("pp.PayPeriodFr", "<=", dtPayPeriodAsOFDate));
            filter.add(new Match("pp.PayPeriodTo", ">=", dtPayPeriodAsOFDate));
        }
        if (dtPayPeriodAsOFDate.Ticks.Equals(0))
        {
            dtPayPeriodAsOFDate=AppUtils.ServerDateTime().Date;
        }
        filter.add(new IN("e.EmpID", "Select EmpID from " + EEmpPersonalInfo.db.dbclass.tableName + " ee", EmployeeSearchControl1.GetEmpInfoFilter(dtPayPeriodAsOFDate, dtPayPeriodAsOFDate)));
        //if (filter.getOrderClause().IndexOf("PayPeriodFr")<0)
        //    filter.add("EmpNo", true);

        DataTable table = filter.loadData(dbConn, null, select, from);
        table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);



        view = new DataView(table);

        ListFooter.Refresh();

        if (repeater != null)
        {
            repeater.DataSource = table;
            repeater.DataBind();
        }

        return view;
    }
    protected void Search_Click(object sender, EventArgs e)
    {
        info.page = 0;
        view = loadEmpData(info, db, Repeater);

    }
    protected void Reset_Click(object sender, EventArgs e)
    {
        binding.clear();
        EmployeeSearchControl1.Reset();
        info.page = 0;
        view = loadEmpData(info, db, Repeater);

    }
    protected void FirstPage_Click(object sender, EventArgs e)
    {
        view = loadEmpData(info, db, Repeater);
    }
    protected void PrevPage_Click(object sender, EventArgs e)
    {
        view = loadEmpData(info, db, Repeater);
    }
    protected void NextPage_Click(object sender, EventArgs e)
    {
        view = loadEmpData(info, db, Repeater);
    }
    protected void LastPage_Click(object sender, EventArgs e)
    {
        view = loadEmpData(info, db, Repeater);
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

        view = loadEmpData(info, db, Repeater);

    }
    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(db, row, cb);

        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            //Binding ebinding = new Binding(dbConn, EEmpPayroll.db);
            //ebinding.add(new BlankZeroLabelVLBinder(EEmpPayroll.db, (Label)e.Item.FindControl("EmpPayConfirmBy"), EUser.VLUserName));
            //ebinding.init(Request, Session);


            EEmpPayroll obj = new EEmpPayroll();
            EEmpPayroll.db.toObject(((DataRowView)e.Item.DataItem).Row, obj);
            //Hashtable values = new Hashtable();
            //EEmpPayroll.db.populate(obj, values);
            //ebinding.toControl(values);


            string[] payrollProcessTypeDescriptionList = obj.PayrollProcessTypeDescription();
            for (int i = 0; i < payrollProcessTypeDescriptionList.Length; i++)
                payrollProcessTypeDescriptionList[i] = HROne.Common.WebUtility.GetLocalizedString(payrollProcessTypeDescriptionList[i], ci);

            ((Label)e.Item.FindControl("Type")).Text = string.Join(" + ", payrollProcessTypeDescriptionList);
        }
    }


}
