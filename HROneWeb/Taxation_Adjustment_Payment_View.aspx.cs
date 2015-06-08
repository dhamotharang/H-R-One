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

public partial class Taxation_Adjustment_Payment_View : HROneWebPage
{
    private const string FUNCTION_CODE = "TAX004";

    protected SearchBinding sbinding;
    protected ListInfo info;
    protected DataView view;

    public Binding ebinding;
    public Binding binding;
    public Binding newBinding;
    public DBManager db = ETaxEmpPayment.db;
    public ETaxEmp obj;
    public int CurID = -1;
    protected DBFilter taxPaymentEditFilter;
    private IN inPaymentFilter;
    private bool IsAllowEdit = true;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        if (!WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
        {
            IsAllowEdit = false;
        }
        TaxEmpPaymentAddPanel.Visible = IsAllowEdit;
        binding = new Binding(dbConn, ETaxEmp.db);
        binding.add(TaxEmpID);

        if (!int.TryParse(DecryptedRequest["TaxEmpID"], out CurID))
            CurID=-1;
        TaxEmpID.Value = CurID.ToString();

        if (!Page.IsPostBack)
        {
            if (CurID > 0)
                loadObject();
        }

        sbinding = new SearchBinding(dbConn, db);
        sbinding.add(new HiddenMatchBinder(TaxEmpID));
        sbinding.initValues("TaxPayID", null, ETaxPayment.VLTaxPayment, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));

        sbinding.init(DecryptedRequest, null);

        if (!string.IsNullOrEmpty(TaxFormType.Value))
        {
            taxPaymentEditFilter = new DBFilter();
            taxPaymentEditFilter.add(new Match("TaxFormType", TaxFormType.Value));


            DBFilter TaxEmpPaymentFilter = new DBFilter();
            TaxEmpPaymentFilter.add(new Match("TaxEmpID", CurID));
            inPaymentFilter = new IN("Not TaxPayID", "Select TaxPayID from TaxEmpPayment", TaxEmpPaymentFilter);
            taxPaymentEditFilter.add(inPaymentFilter);


        }

        if (newBinding == null)
        {
            newBinding = new Binding(dbConn, db);
            newBinding.add(TaxEmpID);
            newBinding.add(new DropDownVLBinder(db, TaxPayID, ETaxPayment.VLTaxPaymentWithNature, taxPaymentEditFilter));
            newBinding.add(new TextBoxBinder(db, TaxEmpPayPeriodFr.TextBox, TaxEmpPayPeriodFr.ID));
            newBinding.add(new TextBoxBinder(db, TaxEmpPayPeriodTo.TextBox, TaxEmpPayPeriodTo.ID));
            newBinding.add(TaxEmpPayAmount);

        }
        newBinding.init(Request, Session);

        info = ListFooter.ListInfo;        

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            view = loadData(info, db , Repeater);
        }
        
    }

    public DataView loadData(ListInfo info, DBManager db, DataList repeater)
    {
        DBFilter filter = sbinding.createFilter();

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);
        else
        {
            info.orderby = "TaxPayID";
            info.order = true;
        }

        string select = "c.*";
        string from = "from " + db.dbclass.tableName + " c ";

        //filter.add(new Match("EmpPayrollID",EmpPayrollID.Value));
        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);

        view = new DataView(table);
        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }

        return view;
    }
    protected bool loadObject()
    {
        obj = new ETaxEmp();
        bool isNew = WebFormWorkers.loadKeys(ETaxEmp.db, obj, DecryptedRequest);
        if (!ETaxEmp.db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        ETaxEmp.db.populate(obj, values);
        binding.toControl(values);
        ucTaxation_Emp_Header.CurrentTaxEmpID = obj.TaxEmpID;

        ETaxForm taxForm = new ETaxForm();
        taxForm.TaxFormID = obj.TaxFormID;
        if (ETaxForm.db.select(dbConn, taxForm))
            TaxFormType.Value = taxForm.TaxFormType;

        if (taxForm.TaxFormType.Equals("E"))
        {
            TaxFormBFGPanel1.Visible = false;
            TaxFormBFGPanel2.Visible = false;
            TaxEmpPayPeriodFr.Value = obj.TaxEmpStartDate.ToString("yyyy-MM-dd");
            TaxEmpPayPeriodTo.Value = obj.TaxEmpStartDate.ToString("yyyy-MM-dd");
        }
        else
        {
            TaxFormBFGPanel1.Visible = true;
            TaxFormBFGPanel2.Visible = true;
            TaxEmpPayPeriodFr.Value = obj.TaxEmpStartDate.ToString("yyyy-MM-dd");
            TaxEmpPayPeriodTo.Value = obj.TaxEmpEndDate.ToString("yyyy-MM-dd");
        }
        return true;
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

        Repeater.EditItemIndex = -1;
        view = loadData(info, db, Repeater);

    }
    protected void Add_Click(object sender, EventArgs e)
    {
        Repeater.EditItemIndex = -1;
        ETaxEmpPayment c = new ETaxEmpPayment();

        Hashtable values = new Hashtable();
        newBinding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);

        ValidateData(c, errors);

        if (!errors.isEmpty())
        {
            return;
        }

        ETaxPayment taxPay = new ETaxPayment();
        taxPay.TaxPayID = c.TaxPayID;
        ETaxPayment.db.select(dbConn, taxPay);
        if (taxPay.TaxPayIsShowNature.Equals("Y"))
            c.TaxEmpPayNature = taxPay.TaxPayNature;

        ETaxEmp taxEmp = new ETaxEmp();
        taxEmp.TaxEmpID = c.TaxEmpID;
        if (ETaxEmp.db.select(dbConn, taxEmp))
        {
            WebUtils.StartFunction(Session, FUNCTION_CODE, taxEmp.EmpID);
            db.insert(dbConn, c);
            WebUtils.EndFunction(dbConn);
        }
        TaxPayID.SelectedIndex = 0;
        TaxEmpPayAmount.Text = string.Empty;

        newBinding.init(Request, Session);
        loadObject();
        view = loadData(info, db, Repeater);
    }

    protected void Repeater_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        ETaxEmpPayment obj = new ETaxEmpPayment();
        db.toObject(((DataRowView)e.Item.DataItem).Row, obj);
        if (TaxFormType.Value.Equals("E"))
            ((Panel)e.Item.FindControl("TaxFormBFGPanel")).Visible = false;
        else
            ((Panel)e.Item.FindControl("TaxFormBFGPanel")).Visible = true;

        if (e.Item.ItemIndex == Repeater.EditItemIndex)
        {
            ETaxEmpPayment old_obj = new ETaxEmpPayment();
            old_obj.TaxEmpPayID = obj.TaxEmpPayID;
            ETaxEmpPayment.db.select(dbConn, old_obj);
            OR orTerms = new OR();
            orTerms.add(inPaymentFilter);
            orTerms.add(new Match("TaxPayID", old_obj.TaxPayID));

            DBFilter taxPaymentSelectedFilter = new DBFilter();
            taxPaymentSelectedFilter.add(orTerms);
            taxPaymentSelectedFilter.add(new Match("TaxFormType", TaxFormType.Value));


            ebinding = new Binding(dbConn, db);
            ebinding.add((HtmlInputHidden)e.Item.FindControl("TaxEmpPayID"));
            ebinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("TaxPayID"), ETaxPayment.VLTaxPayment, taxPaymentSelectedFilter));
            ebinding.add(new TextBoxBinder(db, ((WebDatePicker)e.Item.FindControl("TaxEmpPayPeriodFr")).TextBox, "TaxEmpPayPeriodFr"));
            ebinding.add(new TextBoxBinder(db, ((WebDatePicker)e.Item.FindControl("TaxEmpPayPeriodTo")).TextBox, "TaxEmpPayPeriodTo"));
            ebinding.add((TextBox)e.Item.FindControl("TaxEmpPayAmount"));
            ebinding.add((TextBox)e.Item.FindControl("TaxEmpPayNature"));

            ebinding.init(Request, Session);

            Hashtable values = new Hashtable();
            db.populate(obj, values);

            ebinding.toControl(values);

            ETaxPayment taxPay = new ETaxPayment();
            taxPay.TaxPayID = obj.TaxPayID;
            ETaxPayment.db.select(dbConn, taxPay);
            TextBox TaxPayNature = (TextBox)e.Item.FindControl("TaxEmpPayNature");
            if (taxPay.TaxPayIsShowNature.Equals("Y"))
                TaxPayNature.Visible = true;
            else
                TaxPayNature.Visible = false;

        }
        else
        {
            DataRowView row = (DataRowView)e.Item.DataItem;
            CheckBox cb = (CheckBox)e.Item.FindControl("DeleteItem");
            WebFormUtils.LoadKeys(db, row, cb);
            cb.Visible = IsAllowEdit;
            ((Button)e.Item.FindControl("Edit")).Visible = IsAllowEdit;
            HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("TaxEmpPayID");
            h.Value = ((DataRowView)e.Item.DataItem)["TaxEmpPayID"].ToString();

            ETaxPayment taxPay = new ETaxPayment();
            taxPay.TaxPayID = obj.TaxPayID;
            ETaxPayment.db.select(dbConn, taxPay);
            Label TaxPayNature = (Label)e.Item.FindControl("TaxEmpPayNature");
            if (taxPay.TaxPayIsShowNature.Equals("Y"))
                TaxPayNature.Visible = true;
            else
                TaxPayNature.Visible = false;
        }
        HROne.Common.WebUtility.WebControlsLocalization(Session, e.Item.Controls);

    }

    protected void Repeater_ItemCommand(object source, DataListCommandEventArgs e)
    {
        Button b = (Button)e.CommandSource;




        if (b.ID.Equals("Edit"))
        {
            Repeater.EditItemIndex = e.Item.ItemIndex;
            view = loadData(info, db, Repeater);
            WebUtils.SetEnabledControlSection(TaxEmpPaymentAddPanel, false);
        }
        else if (b.ID.Equals("Cancel"))
        {
            Repeater.EditItemIndex = -1;
            view = loadData(info, db, Repeater);
            WebUtils.SetEnabledControlSection(TaxEmpPaymentAddPanel, true);
        }
        else if (b.ID.Equals("Save"))
        {
            ETaxEmpPayment obj = new ETaxEmpPayment();
            //db.toObject(((DataRowView)e.Item.DataItem).Row, obj);

            OR orTerms = new OR();
            orTerms.add(inPaymentFilter);
            orTerms.add(new Match("TaxPayID", ((DropDownList)e.Item.FindControl("TaxPayID")).SelectedValue));

            DBFilter taxPaymentSelectedFilter = new DBFilter();
            taxPaymentSelectedFilter.add(orTerms);
            taxPaymentSelectedFilter.add(new Match("TaxFormType", TaxFormType.Value));

            ebinding = new Binding(dbConn, db);
            ebinding.add(TaxEmpID);
            ebinding.add((HtmlInputHidden)e.Item.FindControl("TaxEmpPayID"));
            ebinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("TaxPayID"), ETaxPayment.VLTaxPayment, taxPaymentSelectedFilter));
            ebinding.add(new TextBoxBinder(db, ((WebDatePicker)e.Item.FindControl("TaxEmpPayPeriodFr")).TextBox, "TaxEmpPayPeriodFr"));
            ebinding.add(new TextBoxBinder(db, ((WebDatePicker)e.Item.FindControl("TaxEmpPayPeriodTo")).TextBox, "TaxEmpPayPeriodTo"));
            ebinding.add((TextBox)e.Item.FindControl("TaxEmpPayAmount"));
            ebinding.add((TextBox)e.Item.FindControl("TaxEmpPayNature"));

            ebinding.init(Request, Session);


            //            ETaxEmpPayment obj = new ETaxEmpPayment();
            Hashtable values = new Hashtable();

            PageErrors errors = PageErrors.getErrors(db, Page.Master);
            errors.clear();


            ebinding.toValues(values);
            db.validate(errors, values);

            if (!errors.isEmpty())
            {
                return;
            }

            db.parse(values, obj);

            ValidateData(obj, errors);

            if (!errors.isEmpty())
            {
                return;
            }

            ETaxPayment taxPay = new ETaxPayment();
            taxPay.TaxPayID = obj.TaxPayID;
            ETaxPayment.db.select(dbConn, taxPay);
            if (!taxPay.TaxPayIsShowNature.Equals("Y"))
                obj.TaxEmpPayNature = string.Empty;
            else if (obj.TaxEmpPayNature.Equals(string.Empty))
                obj.TaxEmpPayNature = taxPay.TaxPayNature;


            ETaxEmp taxEmp = new ETaxEmp();
            taxEmp.TaxEmpID = obj.TaxEmpID;
            if (ETaxEmp.db.select(dbConn, taxEmp))
            {
                WebUtils.StartFunction(Session, FUNCTION_CODE, taxEmp.EmpID);
                db.update(dbConn, obj);
                WebUtils.EndFunction(dbConn);

                Repeater.EditItemIndex = -1;
                view = loadData(info, db, Repeater);
                WebUtils.SetEnabledControlSection(TaxEmpPaymentAddPanel, true);
            }
        }

    }

    protected void Delete_Click(object sender, EventArgs e)
    {

        ArrayList list = new ArrayList();
        foreach (DataListItem item in Repeater.Items)
        {
            CheckBox c = (CheckBox)item.FindControl("DeleteItem");
            HtmlInputHidden h = (HtmlInputHidden)item.FindControl("TaxEmpPayID");
            if (c.Checked)
            {
                ETaxEmpPayment obj = new ETaxEmpPayment();
                obj.TaxEmpPayID = Int32.Parse(h.Value);
                list.Add(obj);
            }
        }
        ETaxEmp taxEmp = new ETaxEmp();
        taxEmp.TaxEmpID = int.Parse(TaxEmpID.Value);
        if (ETaxEmp.db.select(dbConn, taxEmp))
        {
            WebUtils.StartFunction(Session, FUNCTION_CODE, taxEmp.EmpID);
            foreach (ETaxEmpPayment obj in list)
                if (db.select(dbConn, obj))
                    db.delete(dbConn, obj);
            WebUtils.EndFunction(dbConn);
        }
        newBinding.init(Request, Session);
        view = loadData(info, db, Repeater);
    }

    private void ValidateData(ETaxEmpPayment obj, PageErrors errors)
    {
        if (obj.TaxEmpPayPeriodFr > obj.TaxEmpPayPeriodTo)
        {
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_INCORRECT_DATE_RANGE);
            return;
        }
        ETaxEmp taxEmp = new ETaxEmp();
        taxEmp.TaxEmpID = obj.TaxEmpID;
        if (ETaxEmp.db.select(dbConn, taxEmp))
            if (taxEmp.TaxEmpStartDate > obj.TaxEmpPayPeriodFr || taxEmp.TaxEmpEndDate < obj.TaxEmpPayPeriodTo)
                errors.addError(HROne.Translation.PageErrorMessage.ERROR_INCORRECT_DATE_RANGE);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Taxation_Adjustment_View.aspx?TaxEmpID=" + TaxEmpID.Value);
    }

}
