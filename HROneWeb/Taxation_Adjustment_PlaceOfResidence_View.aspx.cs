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

public partial class Taxation_Adjustment_PlaceOfResidence_View : HROneWebPage
{
    private const string FUNCTION_CODE = "TAX004";
    protected SearchBinding sbinding;
    protected ListInfo info;
    protected DataView view;

    public Binding ebinding;
    public Binding binding;
    public Binding newBinding;
    public DBManager db = ETaxEmpPlaceOfResidence.db;
    public ETaxEmp obj;
    public int CurID = -1;
    protected string TaxFormType = string.Empty;
    protected DBFilter TaxPaymentFilter;
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
        TaxEmpPoRAddPanel.Visible = IsAllowEdit;

        binding = new Binding(dbConn, ETaxEmp.db);
        binding.add(TaxEmpID);

        if (!int.TryParse(DecryptedRequest["TaxEmpID"], out CurID))
            CurID = -1;
        TaxEmpID.Value = CurID.ToString();


        if (!Page.IsPostBack)
        {
            if (CurID > 0)
                loadObject();
        } 
        
        sbinding = new SearchBinding(dbConn, db);
        sbinding.add(new HiddenMatchBinder(TaxEmpID));

        sbinding.init(DecryptedRequest, null);

        if (string.IsNullOrEmpty(TaxFormType))
            TaxFormType = DecryptedRequest["TaxFormType"];
        if (!string.IsNullOrEmpty(TaxFormType))
        {
            TaxPaymentFilter = new DBFilter();
            TaxPaymentFilter.add(new Match("TaxFormType", TaxFormType));
        }

        newBinding = new Binding(dbConn, db);
        newBinding.add(TaxEmpID);
        newBinding.add(TaxEmpPlaceAddress);
        newBinding.add(TaxEmpPlaceNature);
        newBinding.add(new TextBoxBinder(db, TaxEmpPlacePeriodFr.TextBox, TaxEmpPlacePeriodFr.ID));
        newBinding.add(new TextBoxBinder(db, TaxEmpPlacePeriodTo.TextBox, TaxEmpPlacePeriodTo.ID));
        newBinding.add(TaxEmpPlaceERRent);
        newBinding.add(TaxEmpPlaceEERent);
        newBinding.add(TaxEmpPlaceEERentRefunded);
        newBinding.add(TaxEmpPlaceERRentByEE);


        newBinding.init(Request, Session);

        info = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            view = loadData(info, db, Repeater);
        }
    }


    public DataView loadData(ListInfo info, DBManager db, DataList repeater)
    {
        DBFilter filter = sbinding.createFilter();

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);
        else
        {
            info.orderby="TaxEmpPlacePeriodFr";
            info.order = true;
        }
        string select = "c.*";
        string from = "from " + db.dbclass.tableName + " c ";

        //filter.add(new Match("EmpPayrollID",EmpPayrollID.Value));
        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);

        foreach (DataRow row in table.Rows)
            foreach (DBField field in db.fields)
            {
                if (table.Columns.Contains(field.name))
                    if (row[field.name] != null)
                        if (field.transcoder != null)

                            row[field.name] = field.transcoder.fromDB(row[field.name]);
            }

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
        ucTaxation_Emp_Header.CurrentTaxEmpID  = obj.TaxEmpID ;
        
        ETaxForm taxForm = new ETaxForm();
        taxForm.TaxFormID = obj.TaxFormID;
        if (ETaxForm.db.select(dbConn, taxForm))
            TaxFormType = taxForm.TaxFormType;

        EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
        empInfo.EmpID = obj.EmpID;
        if (EEmpPersonalInfo.db.select(dbConn, empInfo))
        {
            TaxEmpPlaceAddress.Text = empInfo.EmpResAddr;
            if (empInfo.EmpResAddrAreaCode.Equals("H"))
                TaxEmpPlaceAddress.Text += ", " + "Hong Kong";
            else if (empInfo.EmpResAddrAreaCode.Equals("K"))
                TaxEmpPlaceAddress.Text += ", " + "Kowloon";
            else if (empInfo.EmpResAddrAreaCode.Equals("N"))
                TaxEmpPlaceAddress.Text += ", " + "New Territories";
        }
        TaxEmpPlaceNature.Text = "Flat";
        TaxEmpPlacePeriodFr.Value = obj.TaxEmpStartDate.ToString("yyyy-MM-dd");
        TaxEmpPlacePeriodTo.Value = obj.TaxEmpEndDate.ToString("yyyy-MM-dd");
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
        ETaxEmpPlaceOfResidence c = new ETaxEmpPlaceOfResidence();

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
            return;

        ETaxEmp taxEmp = new ETaxEmp();
        taxEmp.TaxEmpID = c.TaxEmpID;
        if (ETaxEmp.db.select(dbConn, taxEmp))
        {
            WebUtils.StartFunction(Session, FUNCTION_CODE, taxEmp.EmpID);
            db.insert(dbConn, c);
            WebUtils.EndFunction(dbConn);
        }
        TaxEmpPlaceAddress.Text = string.Empty;
        TaxEmpPlaceEERent.Text = string.Empty;
        TaxEmpPlaceEERentRefunded.Text = string.Empty;
        TaxEmpPlaceERRent.Text = string.Empty;
        TaxEmpPlaceERRentByEE.Text = string.Empty;
        TaxEmpPlaceNature.Text = string.Empty;
        TaxEmpPlacePeriodFr.TextBox.Text = string.Empty;
        TaxEmpPlacePeriodTo.TextBox.Text = string.Empty;

        loadObject();
        view = loadData(info, db, Repeater);
    }

    protected void Repeater_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        ETaxEmpPlaceOfResidence obj = new ETaxEmpPlaceOfResidence();
        db.toObject(((DataRowView)e.Item.DataItem).Row, obj);

        if (e.Item.ItemIndex == Repeater.EditItemIndex)
        {

            ebinding = new Binding(dbConn, db);

            ebinding.add((HtmlInputHidden)e.Item.FindControl("TaxEmpPlaceID"));
            ebinding.add((TextBox)e.Item.FindControl("TaxEmpPlaceAddress"));
            ebinding.add((TextBox)e.Item.FindControl("TaxEmpPlaceNature"));
            ebinding.add(new TextBoxBinder(db, ((WebDatePicker)e.Item.FindControl("TaxEmpPlacePeriodFr")).TextBox, "TaxEmpPlacePeriodFr"));
            ebinding.add(new TextBoxBinder(db, ((WebDatePicker)e.Item.FindControl("TaxEmpPlacePeriodTo")).TextBox, "TaxEmpPlacePeriodTo"));
            ebinding.add((TextBox)e.Item.FindControl("TaxEmpPlaceERRent"));
            ebinding.add((TextBox)e.Item.FindControl("TaxEmpPlaceEERent"));
            ebinding.add((TextBox)e.Item.FindControl("TaxEmpPlaceEERentRefunded"));
            ebinding.add((TextBox)e.Item.FindControl("TaxEmpPlaceERRentByEE"));





            ebinding.init(Request, Session);

            Hashtable values = new Hashtable();
            db.populate(obj, values);

            ebinding.toControl(values);
        }
        else
        {
            DataRowView row = (DataRowView)e.Item.DataItem;
            CheckBox cb = (CheckBox)e.Item.FindControl("DeleteItem");
            WebFormUtils.LoadKeys(db, row, cb);
            cb.Visible = IsAllowEdit;
            ((Button)e.Item.FindControl("Edit")).Visible = IsAllowEdit;
            HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("TaxEmpPlaceID");
            h.Value = ((DataRowView)e.Item.DataItem)["TaxEmpPlaceID"].ToString();
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
            WebUtils.SetEnabledControlSection(TaxEmpPoRAddPanel, false);
        }
        else if (b.ID.Equals("Cancel"))
        {
            Repeater.EditItemIndex = -1;
            view = loadData(info, db, Repeater);
            WebUtils.SetEnabledControlSection(TaxEmpPoRAddPanel, true);
        }
        else if (b.ID.Equals("Save"))
        {
            ebinding = new Binding(dbConn, db);

            ebinding.add(TaxEmpID);
            ebinding.add((HtmlInputHidden)e.Item.FindControl("TaxEmpPlaceID"));
            ebinding.add((TextBox)e.Item.FindControl("TaxEmpPlaceAddress"));
            ebinding.add((TextBox)e.Item.FindControl("TaxEmpPlaceNature"));
            ebinding.add(new TextBoxBinder(db, ((WebDatePicker)e.Item.FindControl("TaxEmpPlacePeriodFr")).TextBox, "TaxEmpPlacePeriodFr"));
            ebinding.add(new TextBoxBinder(db, ((WebDatePicker)e.Item.FindControl("TaxEmpPlacePeriodTo")).TextBox, "TaxEmpPlacePeriodTo"));
            ebinding.add((TextBox)e.Item.FindControl("TaxEmpPlaceERRent"));
            ebinding.add((TextBox)e.Item.FindControl("TaxEmpPlaceEERent"));
            ebinding.add((TextBox)e.Item.FindControl("TaxEmpPlaceEERentRefunded"));
            ebinding.add((TextBox)e.Item.FindControl("TaxEmpPlaceERRentByEE"));

            ebinding.init(Request, Session);


            ETaxEmpPlaceOfResidence obj = new ETaxEmpPlaceOfResidence();
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

            ETaxEmp taxEmp = new ETaxEmp();
            taxEmp.TaxEmpID = obj.TaxEmpID;
            if (ETaxEmp.db.select(dbConn, taxEmp))
            {
                WebUtils.StartFunction(Session, FUNCTION_CODE, taxEmp.EmpID);
                db.update(dbConn, obj);
                WebUtils.EndFunction(dbConn);
            }
            Repeater.EditItemIndex = -1;
            view = loadData(info, db, Repeater);
            WebUtils.SetEnabledControlSection(TaxEmpPoRAddPanel, true);
        }


    }

    protected void Delete_Click(object sender, EventArgs e)
    {

        ArrayList list = new ArrayList();
        foreach (DataListItem item in Repeater.Items)
        {
            CheckBox c = (CheckBox)item.FindControl("DeleteItem");
            HtmlInputHidden h = (HtmlInputHidden)item.FindControl("TaxEmpPlaceID");
            if (c.Checked)
            {
                ETaxEmpPlaceOfResidence obj = new ETaxEmpPlaceOfResidence();
                obj.TaxEmpPlaceID = Int32.Parse(h.Value);
                list.Add(obj);
            }
        }

        ETaxEmp taxEmp = new ETaxEmp();
        taxEmp.TaxEmpID = int.Parse(TaxEmpID.Value);
        if (ETaxEmp.db.select(dbConn, taxEmp))
        {

            WebUtils.StartFunction(Session, FUNCTION_CODE, taxEmp.EmpID);
            foreach (ETaxEmpPlaceOfResidence obj in list)
                if (db.select(dbConn, obj))
                    db.delete(dbConn, obj);
            WebUtils.EndFunction(dbConn);
        }
        view = loadData(info, db, Repeater);
    }

    private void ValidateData(ETaxEmpPlaceOfResidence obj, PageErrors errors)
    {
        DBFilter overlapCheckingFilter = new DBFilter();
        overlapCheckingFilter.add(new Match("TaxEmpID", obj.TaxEmpID));
        if (obj.TaxEmpPlaceID > 0)
            overlapCheckingFilter.add(new Match("TaxEmpPlaceID", "<>", obj.TaxEmpPlaceID));
        overlapCheckingFilter.add(new Match("TaxEmpPlacePeriodTo", ">=", obj.TaxEmpPlacePeriodFr));
        overlapCheckingFilter.add(new Match("TaxEmpPlacePeriodFr", "<=", obj.TaxEmpPlacePeriodTo));

        if (ETaxEmpPlaceOfResidence.db.count(dbConn, overlapCheckingFilter) > 0)
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_INCORRECT_DATE_RANGE);
        else
        {
            if (obj.TaxEmpPlacePeriodFr > obj.TaxEmpPlacePeriodTo)
            {
                errors.addError(HROne.Translation.PageErrorMessage.ERROR_INCORRECT_DATE_RANGE);
                return;
            }
            ETaxEmp taxEmp = new ETaxEmp();
            taxEmp.TaxEmpID = obj.TaxEmpID;
            if (ETaxEmp.db.select(dbConn, taxEmp))
                if (taxEmp.TaxEmpStartDate > obj.TaxEmpPlacePeriodFr || taxEmp.TaxEmpEndDate < obj.TaxEmpPlacePeriodTo)
                    errors.addError(HROne.Translation.PageErrorMessage.ERROR_INCORRECT_DATE_RANGE);


        }
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Taxation_Adjustment_View.aspx?TaxEmpID=" + TaxEmpID.Value);
    }
}
