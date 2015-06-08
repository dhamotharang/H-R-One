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

public partial class Taxation_ESS_Release_List : HROneWebPage
{
    private const string FUNCTION_CODE = "TAX006";
    protected SearchBinding sbinding;
    protected Binding binding;
    public DBManager db = ETaxEmp.db;
//    public EPayrollGroup obj;
    public int CurID = -1;

    protected ListInfo info;
    protected DataView view;
    protected int CurrentTaxFormID;
    protected string CurTaxFormType;
    private const string TAXATION_NAME_OF_SIGNATURE = "TAXATION_NAME_OF_SIGNATURE";
    bool IsAllowEdit = false;

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;

        IsAllowEdit = WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite);

        binding = new Binding(dbConn, ETaxCompany.db);
        binding.add(new DropDownVLBinder(ETaxCompany.db, TaxCompID, ETaxCompany.VLTaxCompany));

        binding.init(Request, Session);

        if (!int.TryParse(TaxCompID.SelectedValue, out CurID))
            if (!int.TryParse(DecryptedRequest["TaxCompID"], out CurID))
                CurID = -1;
            else
                try
                {
                    TaxCompID.SelectedValue = CurID.ToString();
                }
                catch
                {
                    CurID = -1;
                    TaxCompID.SelectedIndex = 0;
                }
        if (CurID <= 0 && TaxCompID.Items.Count > 1)
        {
            TaxCompID.SelectedIndex = 1;
            int.TryParse(TaxCompID.SelectedValue, out CurID);
        }

        CurTaxFormType = "B";



        info = ListFooter.ListInfo;



        sbinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);

        DBFilter taxFormFilter = new DBFilter();
        taxFormFilter.add(new Match("TaxCompID", CurID));
        taxFormFilter.add(new Match("TaxFormType", CurTaxFormType));
        taxFormFilter.add("TaxFormYear", false);
        sbinding.add(new DropDownVLSearchBinder(TaxFormID, "TaxFormID", ETaxForm.VLTaxFormYear).setFilter(taxFormFilter));
        sbinding.init(DecryptedRequest, null);

        if (!int.TryParse(TaxFormID.SelectedValue, out CurrentTaxFormID))
            if (!int.TryParse(DecryptedRequest["TaxFormID"], out CurrentTaxFormID))
                CurrentTaxFormID = -1;
            else
                try
                {
                    TaxFormID.SelectedValue = CurrentTaxFormID.ToString();
                }
                catch
                {
                    CurID = -1;
                    TaxFormID.SelectedIndex = 0;
                }

        if (CurrentTaxFormID <= 0 && TaxFormID.Items.Count > 1)
        {
            TaxFormID.SelectedIndex = 1;
            int.TryParse(TaxFormID.SelectedValue, out CurrentTaxFormID);
        }

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        //if (!WebUtils.CheckAccess(Response, Session))
        //    return;

        //sbinding = new SearchBinding(dbConn, db);
        //sbinding.add(new DropDownVLSearchBinder( TaxCompID,"TaxCompID", ETaxCompany.VLTaxCompany));
        //sbinding.add(new DropDownVLSearchBinder(YearSelect, "",new WFYearList(AppUtils.ServerDateTime().Date.Year- 2001, 0)));

        //sbinding.init(DecryptedRequest, null);

        //if (!Page.IsPostBack)
        //{
        //    YearSelect.SelectedValue = AppUtils.ServerDateTime().Year.ToString();
        //    loadState();
        //    if (CurID > 0)
        //    {
        //        //loadObject();
        //        view = loadData(info, EEmpPayroll.db, Repeater);
        //        panelEmpList.Visible=true;
        //    }
        //    else
        //        panelEmpList.Visible = false;

        //}
        //else
        //    panelEmpList.Visible = true;


    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {


            if (CurID > 0)
            {
                panelEmpList.Visible = true;
                loadObject();

                view = loadData(info, db, Repeater);
            }
            else
                panelEmpList.Visible = false;
        }
    }
    protected bool loadObject()
    {
        ETaxCompany obj = new ETaxCompany();
        bool isNew = WebFormWorkers.loadKeys(ETaxCompany.db, obj, DecryptedRequest);
        if (!ETaxCompany.db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        ETaxCompany.db.populate(obj, values);
        binding.toControl(values);

        if (CurrentTaxFormID <= 0)
        {
            if (TaxFormID.Items.Count >1)
                CurrentTaxFormID = int.Parse(TaxFormID.Items[1].Value);

        }
        if (CurrentTaxFormID > 0)
            TaxFormID.SelectedValue = CurrentTaxFormID.ToString();
        else
            if (TaxFormID.Items.Count >0)
                TaxFormID.SelectedIndex = 1;


        return true;
    }

    protected void TaxCompID_SelectedIndexChanged(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);
    }

    protected void TaxFormID_SelectedIndexChanged(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);
    }
 
    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {

        DBFilter filter = sbinding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = "e.*, TE.TaxEmpID, TE.TaxEmpIsReleasePrintoutToESS, et.EmpTermLastDate ";
        string from = "from [" + db.dbclass.tableName + "] TE, EmpPersonalInfo e Left Join EmpTermination et on e.EmpID=et.EmpID";

        //DBFilter taxFormFilter = new DBFilter();
        //taxFormFilter.add(new Match("Tf.TaxFormYear", int.Parse(YearSelect.SelectedValue)));
        //taxFormFilter.add(new Match("Tf.TaxFormType","B"));
        //taxFormFilter.add(new Match("Tf.TaxCompID", int.Parse(TaxCompID.SelectedValue)));

        DBFilter taxEmpFilter = new DBFilter();
        filter.add (new MatchField("TE.EmpID","E.EmpID"));
        filter.add(new Match("TaxFormID", CurrentTaxFormID));
        //filter.add(new IN("Te.TaxFormID","Select Tf.TaxformID from TaxForm tf",taxFormFilter));


        DBFilter empInfoFilter = EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime().Date, AppUtils.ServerDateTime().Date);
        empInfoFilter.add(new MatchField("e.EmpID", "ee.EmpID"));
        filter.add(new Exists(EEmpPersonalInfo.db.dbclass.tableName + " ee", empInfoFilter));


        DateTime dtLastEmploymentDateFrom, dtLastEmploymentDateTo;
        if (DateTime.TryParse(((WebDatePicker)EmployeeSearchControl1.AdditionElementControl.FindControl("LastEmploymentDateFrom")).Value,out dtLastEmploymentDateFrom))
            filter.add(new Match("et.EmpTermLastDate",">=",dtLastEmploymentDateFrom));
        if (DateTime.TryParse(((WebDatePicker)EmployeeSearchControl1.AdditionElementControl.FindControl("LastEmploymentDateTo")).Value ,out dtLastEmploymentDateTo))
            filter.add(new Match("et.EmpTermLastDate","<=",dtLastEmploymentDateTo));


        DataTable table = filter.loadData(dbConn, null, select, from);
        table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);

        if (table.Rows.Count != 0)
            btnPostToESS.Visible = true & IsAllowEdit;
        else
            btnPostToESS.Visible = false & IsAllowEdit;

        view = new DataView(table);
        //if (info != null)
        //{
        //    info.loadPageList(null, PrevPage, NextPage, FirstPage, LastPage);

        //    CurPage.Value = info.page.ToString();
        //    NumPage.Value = info.numPage.ToString();
        //}
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

        view = loadData(info, db, Repeater);

    }
    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(db, row, cb);

        ETaxEmp obj = new ETaxEmp();
        db.toObject(((DataRowView)e.Item.DataItem).Row, obj);

        if (obj.TaxEmpIsReleasePrintoutToESS)
            cb.Checked = true;
        else
            cb.Checked = false;
    }

    protected void btnPostToESS_Click(object sender, EventArgs e)
    {
        HROne.DataAccess.PageErrors errors = HROne.DataAccess.PageErrors.getErrors(null, Page.Master);
        errors.clear();

        foreach (RepeaterItem i in Repeater.Items)
        {
            CheckBox cb = (CheckBox)i.FindControl("ItemSelect");
            ETaxEmp o = (ETaxEmp)ETaxEmp.db.createObject();
            WebFormUtils.GetKeys(db, o, cb);
            if (cb.Checked)
                o.TaxEmpIsReleasePrintoutToESS = true;
            else
                o.TaxEmpIsReleasePrintoutToESS = false;
            ETaxEmp.db.update(dbConn, o);
        }

        errors.addError("Taxation Report is released to ESS.");

    }

    protected void Search_Click(object sender, EventArgs e)
    {
        info.page = 0;
        view = loadData(info, db, Repeater);

    }
    protected void Reset_Click(object sender, EventArgs e)
    {
        EmployeeSearchControl1.Reset();
        sbinding.clear();
        info.page = 0;

        view = loadData(info, db, Repeater);


    }
}
