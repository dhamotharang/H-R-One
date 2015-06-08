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
using HROne.Payroll;

public partial class Payroll_SetPaidDate : HROneWebPage
{
    private const string FUNCTION_CODE = "PAY009";

    protected SearchBinding sbinding;

    protected ListInfo info;
    protected DataView view;
    private bool IsAllowEdit = true;
    
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        if (!WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
        {
            IsAllowEdit = false;
            btnUndo.Visible = false;
            UndoPayrollPanel.Visible = false;
        }

        


        sbinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);


        info = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

        }

    }


    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = sbinding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = "e.*, ep.*";
        string from = "from [" + db.dbclass.tableName + "] ep, EmpPersonalInfo e ";

        filter.add(new MatchField("e.EmpID", "ep.EmpID"));
        filter.add(Payroll_PeriodSelectionList1.GetEmpPayrollDBTerm("ep"));
        filter.add(WebUtils.AddRankFilter(Session, "e.EmpID", true));

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);
        if (table.Rows.Count > 0)
            btnUndo.Visible = IsAllowEdit;
        else
            btnUndo.Visible = false;



        view = new DataView(table);

        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }

        return view;
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

        view = loadData(info, EEmpPayroll.db, Repeater);

    }
    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(EEmpPayroll.db, row, cb);
        cb.Visible = IsAllowEdit;

        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Binding ebinding = new Binding(dbConn, EEmpPayroll.db);
            ebinding.add(new BlankZeroLabelVLBinder(EEmpPayroll.db, (Label)e.Item.FindControl("EmpPayConfirmBy"), EUser.VLUserName));
            ebinding.init(Request, Session);


            EEmpPayroll obj = new EEmpPayroll();
            EEmpPayroll.db.toObject(((DataRowView)e.Item.DataItem).Row, obj);
            Hashtable values = new Hashtable();
            EEmpPayroll.db.populate(obj, values);
            ebinding.toControl(values);

            string[] payrollProcessTypeDescriptionList = obj.PayrollProcessTypeDescription();
            for (int i = 0; i < payrollProcessTypeDescriptionList.Length; i++)
                payrollProcessTypeDescriptionList[i] = HROne.Common.WebUtility.GetLocalizedString(payrollProcessTypeDescriptionList[i], ci);

            ((Label)e.Item.FindControl("Type")).Text = string.Join(" + ", payrollProcessTypeDescriptionList);

            ((Label)e.Item.FindControl("TotalAmount")).Text = obj.GetTotalPaymentAmount(dbConn).ToString("$#,##0.00");
            
        }
    }

    protected void btnUndo_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(EEmpPayroll.db, Page.Master);
        errors.clear();

        ArrayList list = new ArrayList();
        foreach (RepeaterItem i in Repeater.Items)
        {
            CheckBox cb = (CheckBox)i.FindControl("ItemSelect");
            if (cb.Checked)
            {
                EEmpPayroll o = (EEmpPayroll)EEmpPayroll.db.createObject();
                WebFormUtils.GetKeys(EEmpPayroll.db, o, cb);
                list.Add(o);
            }

        }
        if (list.Count > 0)
        {
            DateTime paidDate = new DateTime();

            if (!string.IsNullOrEmpty(SetPaidDate.Value))
                paidDate = DateTime.Parse(SetPaidDate.Value);
            foreach (EEmpPayroll o in list)
            {
                if (EEmpPayroll.db.select(dbConn, o))
                {
                    if (!o.EmpPayValueDate.Equals(paidDate))
                    {
                        WebUtils.StartFunction(Session, FUNCTION_CODE, o.EmpID, true);
                        o.EmpPayValueDate = paidDate;
                        EEmpPayroll.db.update(dbConn, o);
                        WebUtils.EndFunction(dbConn);
                    }
                }
            }


            errors.addError("Paid Date is updated");
            SetPaidDate.Value = string.Empty;
        }
        loadData(info, EEmpPayroll.db, Repeater);
    }
    protected void Payroll_PeriodSelectionList1_PayrollBatchChecked(object sender, EventArgs e)
    {
        info.page = 0;
        view = loadData(info, EEmpPayroll.db, Repeater);
    }
        
}
