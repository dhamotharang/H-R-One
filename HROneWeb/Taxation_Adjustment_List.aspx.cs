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
using HROne.Taxation;

public partial class Taxation_Adjustment_List : HROneWebPage
{
    private const string FUNCTION_CODE = "TAX004";

    protected SearchBinding sbinding;
    protected Binding binding;
    public DBManager db = ETaxEmp.db;
//    public EPayrollGroup obj;
    public int CurID = -1;

    protected ListInfo info;
    protected DataView view;
    protected int CurrentTaxFormID;
    protected string CurTaxFormType;
    protected string CurTaxFormYear;
    private bool IsAllowEdit = true;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;
        toolBar.DeleteButton_Visible = false;
        SelectAllPanel.Visible = false;

        binding = new Binding(dbConn, ETaxCompany.db);
        binding.add(new DropDownVLBinder(ETaxCompany.db, TaxCompID, ETaxCompany.VLTaxCompany));
        binding.add(new DropDownVLBinder(ETaxForm.db, TaxFormType, ETaxForm.VLTaxFormType));
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

        if (TaxFormType.SelectedValue.Equals(string.Empty))
        {
            if (!string.IsNullOrEmpty(DecryptedRequest["TaxFormType"]) && !DecryptedRequest["TaxFormType"].Equals("") && !Page.IsPostBack)
            {
                try
                {
                    CurTaxFormType = DecryptedRequest["TaxFormType"];
                    TaxFormType.SelectedValue = CurTaxFormType;
                }
                catch
                {
                    CurTaxFormType = string.Empty;
                }
            }
        }
        else
            CurTaxFormType = TaxFormType.SelectedValue;

        if (string.IsNullOrEmpty(CurTaxFormType) && TaxFormType.Items.Count > 1)
        {
            TaxFormType.SelectedIndex = 1;
            CurTaxFormType = TaxFormType.SelectedValue;
        }


        info = ListFooter.ListInfo;





        sbinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);

        DBFilter taxFormFilter = new DBFilter();
        taxFormFilter.add(new Match("TaxCompID", CurID));
        taxFormFilter.add(new Match("TaxFormType", CurTaxFormType));
        taxFormFilter.add("TaxFormYear", false);
        sbinding.add(new DropDownVLSearchBinder(TaxFormID, "TaxFormID", ETaxForm.VLTaxFormYear).setFilter(taxFormFilter));
        sbinding.init(DecryptedRequest, null);

        if (!int.TryParse(TaxFormID.SelectedValue, out CurrentTaxFormID))
            if (!int.TryParse(DecryptedRequest["TaxFormID"] , out CurrentTaxFormID))
                CurrentTaxFormID=-1;
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
        //try
        //{
        //    CurID = Int32.Parse(DecryptedRequest["TaxCompID"]);
        //    if (DecryptedRequest["TaxFormID"] != null)
        //        CurrentTaxFormID = Int32.Parse(DecryptedRequest["TaxFormID"]);
        //    else if (!Int32.TryParse(TaxFormID.SelectedValue, out CurrentTaxFormID))
        //    {
        //        CurrentTaxFormID = 0;
        //        //EPayrollGroup obj = new EPayrollGroup();
        //        //obj.PayGroupID = CurID;
        //        //if (!db.select(dbConn, obj))
        //        //    CurPayPeriodID = obj.CurrentPayPeriodID;
        //    }

        //}
        //catch (Exception ex)
        //{
        //}

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
        if (CurID > 0)
        {
            panelEmpList.Visible = true;
            loadObject();

            if (!Page.IsPostBack)
            {
                view = loadData(info, db, Repeater);
            }
        }
        else
            panelEmpList.Visible = false;

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

        try
        {
            TaxFormType.SelectedValue = CurTaxFormType;
        }
        catch
        {
        }

        return true;
    }

    protected void TaxCompID_SelectedIndexChanged(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);
        //Response.Redirect(Request.Url.LocalPath + "?TaxCompID=" + TaxCompID.SelectedValue + "&TaxFormType=" + TaxFormType.SelectedValue);
    }

    protected void TaxFormType_SelectedIndexChanged(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);
        //Response.Redirect(Request.Url.LocalPath + "?TaxCompID=" + TaxCompID.SelectedValue + "&TaxFormType=" + TaxFormType.SelectedValue );
    }

    protected void TaxFormID_SelectedIndexChanged(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);
        //Response.Redirect(Request.Url.LocalPath + "?TaxCompID=" + TaxCompID.SelectedValue + "&TaxFormType=" + TaxFormType.SelectedValue + "&TaxFormID=" + TaxFormID.SelectedValue);
    }
 
    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {

        DBFilter filter = new DBFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = "TE.TaxEmpID, e.* ";
        string from = "FROM " + db.dbclass.tableName + " TE LEFT JOIN EmpPersonalInfo E ON TE.EmpID=E.EmpID ";

        //DBFilter taxFormFilter = new DBFilter();
        //taxFormFilter.add(new Match("Tf.TaxFormYear", int.Parse(YearSelect.SelectedValue)));
        //taxFormFilter.add(new Match("Tf.TaxFormType","B"));
        //taxFormFilter.add(new Match("Tf.TaxCompID", int.Parse(TaxCompID.SelectedValue)));

        DBFilter taxEmpFilter = new DBFilter();
        //filter.add (new MatchField("TE.EmpID","E.EmpID"));
        filter.add(new Match("TaxFormID", CurrentTaxFormID));
        OR orEmpIDTerm = new OR();
        orEmpIDTerm.add(new IN("TE.EmpID", "Select EE.EmpID from " + EEmpPersonalInfo.db.dbclass.tableName + " ee", EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime().Date, AppUtils.ServerDateTime().Date)));
        orEmpIDTerm.add(new IN("NOT TE.EmpID", "SELECT EE2.EmpID from " + EEmpPersonalInfo.db.dbclass.tableName + " ee2", null));
        filter.add(orEmpIDTerm);
        //filter.add(new IN("Te.TaxFormID","Select Tf.TaxformID from TaxForm tf",taxFormFilter));



        DataTable table = filter.loadData(dbConn, null, select, from);
        table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);

        view = new DataView(table);

        ListFooter.Refresh();

        if (repeater != null)
        {
            repeater.DataSource = view;
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
        WebFormUtils.LoadKeys(db, row, cb);
        
        cb.Visible = IsAllowEdit;
        HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("TaxEmpID");
        h.Value = ((DataRowView)e.Item.DataItem)["TaxEmpID"].ToString();
        e.Item.FindControl("ItemSelect").Visible = false;
    }

    protected void Delete_Click(object sender, EventArgs e)
    {

        ArrayList list = new ArrayList();
        foreach (RepeaterItem item in Repeater.Items)
        {
            CheckBox c = (CheckBox)item.FindControl("ItemSelect");
            HtmlInputHidden h = (HtmlInputHidden)item.FindControl("TaxEmpID");
            if (c.Checked)
            {
                ETaxEmp obj = new ETaxEmp();
                obj.TaxEmpID = Int32.Parse(h.Value);
                list.Add(obj);
            }
        }

        foreach (ETaxEmp obj in list)
        {
            if (ETaxEmp.db.select(dbConn, obj))
            {
                WebUtils.StartFunction(Session, FUNCTION_CODE, obj.EmpID);
                db.delete(dbConn, obj);
                {
                    DBFilter taxEmpPaymentFilter = new DBFilter();
                    taxEmpPaymentFilter.add(new Match("TaxEmpID", obj.TaxEmpID));
                    ArrayList taxEmpPaymentList = ETaxEmpPayment.db.select(dbConn, taxEmpPaymentFilter);
                    foreach (ETaxEmpPayment taxEmpPayment in taxEmpPaymentList)
                        ETaxEmpPayment.db.delete(dbConn, taxEmpPayment);
                }
                {
                    DBFilter taxEmpPoRFilter = new DBFilter();
                    taxEmpPoRFilter.add(new Match("TaxEmpID", obj.TaxEmpID));
                    ArrayList taxEmpPoRList = ETaxEmpPlaceOfResidence.db.select(dbConn, taxEmpPoRFilter);
                    foreach (ETaxEmpPlaceOfResidence taxEmpPoR in taxEmpPoRList)
                        ETaxEmpPlaceOfResidence.db.delete(dbConn, taxEmpPoR);
                }
                WebUtils.EndFunction(dbConn);
            }
        }

        //if (DecryptedRequest["TaxFormID"] != null)
        //{
        //    try
        //    {
        //        CurrentTaxFormID = Int32.Parse(DecryptedRequest["TaxFormID"]);
        //    }
        //    catch
        //    {
        
        //    }
        //}
        //else if (!Int32.TryParse(TaxFormID.SelectedValue, out CurrentTaxFormID))
        //{
        //        CurrentTaxFormID = 0;
        //        //EPayrollGroup obj = new EPayrollGroup();
        //        //obj.PayGroupID = CurID;
        //        //if (!db.select(dbConn, obj))
        //        //    CurPayPeriodID = obj.CurrentPayPeriodID;
        //}
        if (CurrentTaxFormID > 0)
        {
            ETaxForm taxForm = new ETaxForm();
            taxForm.TaxFormID = CurrentTaxFormID;
            ETaxForm.db.select(dbConn, taxForm);

            if (taxForm.TaxFormType.Equals("B", StringComparison.CurrentCultureIgnoreCase))
            {
                TaxationGeneration.RearrangeSheetNo(dbConn, CurrentTaxFormID);

            }
        }
        view = loadData(info, db, Repeater);
        //Response.Redirect(Request.Url.LocalPath + "?" + Request.QueryString);
    }

    protected void Search_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);

    }
    protected void Reset_Click(object sender, EventArgs e)
    {
        sbinding.clear();
        EmployeeSearchControl1.Reset();
        info.page = 0;
        view = loadData(info, db, Repeater);

    }
}
