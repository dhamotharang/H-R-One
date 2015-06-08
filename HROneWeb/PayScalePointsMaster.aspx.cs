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

public partial class PayScalePointsMaster : HROneWebPage
{
    private const string FUNCTION_CODE = "SYS021";
    private const string TABLE_NAME = "PayScaleMap";
    protected DBManager db = EPayScaleMap.db;
    protected SearchBinding sbinding;
    protected ListInfo info;
    protected DataView view;
    public Binding binding;
    public Binding ebinding;
    public EPayScaleMap obj;
    public int CurID = -1;
    private bool IsAllowEdit = true;


    protected void Page_Load(object sender, EventArgs e)
    {

        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;

        if (ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_PAYSCALE_POINT_SYSTEM) != "Y")
        {
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");
            return;
        }

        toolBar.FunctionCode = FUNCTION_CODE;
        if (!WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
        {
            IsAllowEdit = false;
        }
        AddPanel.Visible = IsAllowEdit;
        ImportPanel.Visible = IsAllowEdit;



        //binding = new Binding(dbConn, db);
        //binding.add(new TextBoxBinder(db, StatutoryHolidayDate.TextBox, StatutoryHolidayDate.ID));
        //binding.add(StatutoryHolidayDesc);
        //binding.init(Request, Session);


        binding = new Binding(dbConn, db);
        binding.add(new TextBoxBinder(db, EffectiveDate.TextBox, EffectiveDate.ID));
        binding.add(new TextBoxBinder(db, ExpiryDate.TextBox, ExpiryDate.ID));
        //binding.add(new TextBoxBinder(db, EffectiveDate.TextBox, EffectiveDate.ID));
        //binding.add(new TextBoxBinder(db, ExpiryDate.TextBox, ExpiryDate.ID));
        binding.add(SchemeCode);
        binding.add(Point);
        binding.add(Salary);

        binding.init(Request, Session);

        sbinding = new SearchBinding(dbConn, db);
        sbinding.add(new DropDownVLSearchBinder(SchemeSelect, "SchemeCode", EPayScaleMap.VLDistinctSchemeList, true));
        sbinding.init(DecryptedRequest, null);

        info = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //            YearSelect.SelectedValue = AppUtils.ServerDateTime().Year.ToString();
            view = loadData(info, db, Repeater);
        }
    }

    public DataView loadData(ListInfo info, DBManager db, DataList repeater)
    {
        //DBFilter filter = sbinding.createFilter();
        
        DBFilter filter = new DBFilter();
        OR m_OR = new OR();

        DateTime m_asAtDate = new DateTime();
        if (DateTime.TryParse(AsAtDate.TextBox.Text, out m_asAtDate))
        {
            filter.add(new Match("EffectiveDate", "<=", m_asAtDate));
            m_OR.add(new Match("ExpiryDate", ">=", m_asAtDate));
            m_OR.add(new NullTerm("ExpiryDate"));
            filter.add(m_OR);
        }
        if (SchemeSelect.SelectedIndex > 0)
        {
            DBFieldTranscoder idTranscoder = EPayScale.db.getField("SchemeCode").transcoder;
            if (idTranscoder != null)
                filter.add(new Match("SchemeCode", AppUtils.Encode(EPayScaleMap.db.getField("SchemeCode"), SchemeSelect.SelectedValue.ToString())));
        }

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
        {
            //filter.add(info.orderby, info.order);
        }
        else
        {
            info.orderby = "SchemeCode, Point";
            info.order = false;
        }
        string select = "*";
        string from = "from PayscaleMap ";

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);

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
        String orderColumn = l.ID.Substring(1);
        if (info.orderby == null)
            info.order = true;
        else if (info.orderby.Equals(orderColumn))
            info.order = !info.order;
        else
            info.order = true;
        info.orderby = orderColumn;

        Repeater.EditItemIndex = -1;
        view = loadData(info, db, Repeater);

    }
    protected void Add_Click(object sender, EventArgs e)
    {
        Repeater.EditItemIndex = -1;
        EPayScaleMap c = new EPayScaleMap();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);
        if (!AppUtils.checkDuplicate(dbConn, db, c, errors, "Point"))
            return;

        WebUtils.StartFunction(Session, FUNCTION_CODE);
        db.insert(dbConn, c);
        WebUtils.EndFunction(dbConn);

        EffectiveDate.Value = string.Empty;
        ExpiryDate.Value = string.Empty;
        Point.Text = "0.00";
        Salary.Text = "0.00";

        view = loadData(info, db, Repeater);
    }

    protected void Repeater_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        HROne.Common.WebUtility.WebControlsLocalization(Session, e.Item.Controls);

        e.Item.FindControl("DeleteItem").Visible = IsAllowEdit;
        if (e.Item.ItemIndex == Repeater.EditItemIndex)
        {
            ebinding = new Binding(dbConn, db);
            ebinding.add((HtmlInputHidden)e.Item.FindControl("PayScaleMapID"));
            //ebinding.add(new TextBoxBinder(db, ((WebDatePicker)e.Item.FindControl("StatutoryHolidayDate")).TextBox, "StatutoryHolidayDate"));
            ebinding.add(new TextBoxBinder(db, ((WebDatePicker)e.Item.FindControl("EffectiveDate")).TextBox, EffectiveDate.ID));
            ebinding.add(new TextBoxBinder(db, ((WebDatePicker)e.Item.FindControl("ExpiryDate")).TextBox, ExpiryDate.ID));
            ebinding.add((TextBox)e.Item.FindControl("SchemeCode"));
            ebinding.add((TextBox)e.Item.FindControl("Point"));
            ebinding.add((TextBox)e.Item.FindControl("Salary"));
            ebinding.init(Request, Session);

            EPayScaleMap obj = new EPayScaleMap();
            db.toObject(((DataRowView)e.Item.DataItem).Row, obj);
            Hashtable values = new Hashtable();
            db.populate(obj, values);
            ebinding.toControl(values);
        }
        else
        {
            e.Item.FindControl("Edit").Visible = IsAllowEdit;
            HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("PayScaleMapID");
            h.Value = ((DataRowView)e.Item.DataItem)["PayScaleMapID"].ToString();
        }
    }

    protected void Repeater_ItemCommand(object source, DataListCommandEventArgs e)
    {
        Button b = (Button)e.CommandSource;

        if (b.ID.Equals("Edit"))
        {
            Repeater.EditItemIndex = e.Item.ItemIndex;
            view = loadData(info, db, Repeater);
            WebUtils.SetEnabledControlSection(AddPanel, false);
        }
        else if (b.ID.Equals("Cancel"))
        {
            Repeater.EditItemIndex = -1;
            view = loadData(info, db, Repeater);
            WebUtils.SetEnabledControlSection(AddPanel, true);
        }
        else if (b.ID.Equals("Save"))
        {
            ebinding = new Binding(dbConn, db);
            ebinding.add((HtmlInputHidden)e.Item.FindControl("PayScaleMapID"));
            //ebinding.add(new TextBoxBinder(db, ((WebDatePicker)e.Item.FindControl("StatutoryHolidayDate")).TextBox, "StatutoryHolidayDate"));
            ebinding.add(new TextBoxBinder(db, ((WebDatePicker)e.Item.FindControl("EffectiveDate")).TextBox, "EffectiveDate"));
            ebinding.add(new TextBoxBinder(db, ((WebDatePicker)e.Item.FindControl("ExpiryDate")).TextBox, "ExpiryDate"));
//            ebinding.add((TextBox)e.Item.FindControl("EffectiveDate"));
//            ebinding.add((TextBox)e.Item.FindControl("ExpiryDate"));
            ebinding.add((TextBox)e.Item.FindControl("SchemeCode"));
            ebinding.add((TextBox)e.Item.FindControl("Point"));
            ebinding.add((TextBox)e.Item.FindControl("Salary"));
            ebinding.init(Request, Session);

            EPayScaleMap obj = new EPayScaleMap();
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

            if (!AppUtils.checkDuplicate(dbConn, db, obj, errors, "Point", new Match("EffectiveDate", obj.EffectiveDate)))
                return;

            WebUtils.StartFunction(Session, FUNCTION_CODE);
            db.update(dbConn, obj);
            WebUtils.EndFunction(dbConn);

            Repeater.EditItemIndex = -1;
            view = loadData(info, db, Repeater);
            WebUtils.SetEnabledControlSection(AddPanel, true);
        }


    }

    protected void Edit_Click(object sender, EventArgs e)
    {

    }

    protected void Delete_Click(object sender, EventArgs e)
    {

        ArrayList list = new ArrayList();
        foreach (DataListItem item in Repeater.Items)
        {
            CheckBox c = (CheckBox)item.FindControl("DeleteItem");
            HtmlInputHidden h = (HtmlInputHidden)item.FindControl("PayScaleMapID");
            if (c.Checked)
            {
                EPayScaleMap obj = new EPayScaleMap();
                obj.PayScaleMapID = Int32.Parse(h.Value);
                list.Add(obj);
            }
        }
        foreach (EPayScaleMap obj in list)
        {
            if (EPayScaleMap.db.select(dbConn, obj))
            {
                WebUtils.StartFunction(Session, FUNCTION_CODE);
                db.delete(dbConn, obj);
                WebUtils.EndFunction(dbConn);
            }
        }
        view = loadData(info, db, Repeater);
        //Response.Redirect(Request.Url.LocalPath);
    }

    protected void AsAtDate_Changed(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {

        DataTable tmpDataTable = new DataTable(TABLE_NAME);
        //tmpDataTable.Columns.Add("ID", typeof(Int32));
        tmpDataTable.Columns.Add("Effective Date", typeof(DateTime));
        tmpDataTable.Columns.Add("Expiry Date", typeof(DateTime));
        tmpDataTable.Columns.Add("Scheme Code", typeof(String));
        tmpDataTable.Columns.Add("Point", typeof(Decimal));
        tmpDataTable.Columns.Add("Salary", typeof(Decimal));

        DBFilter filter = new DBFilter(); // sbinding.createFilter();
        OR m_OR = new OR();
        DateTime m_asAtDate = new DateTime();

        if (SchemeSelect.SelectedIndex > 0)
        {
            filter.add(new Match("SchemeCode", AppUtils.Encode(EPayScale.db.getField("SchemeCode"), SchemeSelect.SelectedValue.ToString())));
        }

        if (DateTime.TryParse(AsAtDate.TextBox.Text, out m_asAtDate))
        {
            filter.add(new Match("EffectiveDate", "<=", m_asAtDate));
            m_OR.add(new Match("ExpiryDate", ">=", m_asAtDate));
            m_OR.add(new NullTerm("ExpiryDate"));
            filter.add(m_OR);
        }

        filter.add("Point", false);

        ArrayList list = EPayScaleMap.db.select(dbConn, filter);
        foreach (EPayScaleMap c in list)
        {
            DataRow row = tmpDataTable.NewRow();
            //row["ID"] = c.PayScaleID;
            row["Effective Date"] = c.EffectiveDate;
            row["Expiry Date"] = c.ExpiryDate;
            row["Scheme Code"] = c.SchemeCode.ToString();
            row["Point"] = c.Point;
            row["Salary"] = c.Salary;

            tmpDataTable.Rows.Add(row);
        }
        string exportFileName = System.IO.Path.GetTempFileName();
        System.IO.File.Delete(exportFileName);
        exportFileName += ".xls";
        HROne.Export.ExcelExport excelExport = new HROne.Export.ExcelExport(exportFileName);
        excelExport.Update(tmpDataTable);
        WebUtils.TransmitFile(Response, exportFileName, "PayScalePoints_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true);
        return;

    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        // m_list determine if generation of Recurring Payment is necessary
        ArrayList m_list = EPayScaleMap.db.select(dbConn, new DBFilter());

        if (BatchFile.HasFile)
        {
            string strTmpFolder = HROne.Common.Folder.GetOrCreateSessionTempFolder(Session.SessionID).FullName; ; //Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);
            string strTmpFile = System.IO.Path.Combine(strTmpFolder, AppUtils.ServerDateTime().ToString("~yyyyMMddHHmmss_") + BatchFile.FileName);
            BatchFile.SaveAs(strTmpFile);

            ArrayList pendingList = new ArrayList();

            try
            {

                DataTable rawDataTable = HROne.Import.ExcelImport.parse(strTmpFile, string.Empty).Tables[TABLE_NAME];

                if (rawDataTable.Columns.Contains("Effective Date") && rawDataTable.Columns.Contains("Expiry Date") &&
                    rawDataTable.Columns.Contains("Scheme Code") && 
                    rawDataTable.Columns.Contains("Point") && rawDataTable.Columns.Contains("Salary"))
                {
                    int row_count = 0;
                    Decimal m_point;
                    DateTime m_date;
                    Decimal m_salary;

                    int id;
                    foreach (DataRow row in rawDataTable.Rows)
                    {
                        row_count++;

                        EPayScaleMap c = new EPayScaleMap();

                        if (!row.IsNull("Effective Date") && DateTime.TryParse(((String)row["Effective Date"]), out m_date))
                        {
                            c.EffectiveDate = m_date;
                        }
                        else
                        {
                            errors.addError("Missing Effective Date in row " + row_count.ToString());
                            if (!errors.isEmpty()) return;
                        }

                        if (!row.IsNull("Expiry Date") && DateTime.TryParse(((String)row["Expiry Date"]), out m_date))
                        {
                            c.ExpiryDate = m_date;
                        }

                        if (!row.IsNull("Scheme Code") && row["Scheme Code"].ToString().Trim() != "")
                        {
                            c.SchemeCode = row["Scheme Code"].ToString().Trim();
                        }
                        else
                        {
                            errors.addError("Missing Scheme Code in row " + row_count.ToString());
                            if (errors.errors.Count > 10) return;
                        }

                        if (!row.IsNull("Point") && Decimal.TryParse(((String)row["Point"]), out m_point))
                        {
                            c.Point = m_point;
                        }
                        else
                        {
                            errors.addError("Missing Point in row " + row_count.ToString());
                            if (errors.errors.Count > 10) return;
                        }

                        if (!row.IsNull("Salary") && Decimal.TryParse(((String)row["Salary"]), out m_salary))
                        {
                            c.Salary = m_salary;
                        }
                        else
                        {
                            errors.addError("Missing Salary in row " + row_count.ToString());
                            if (errors.errors.Count > 10) return;
                        }

                        pendingList.Add(c);
                    }
                }
                else
                {
                    errors.addError(HROne.Translation.PageErrorMessage.ERROR_INVALID_FILE);
                }
            }
            catch (Exception)
            {
                errors.addError(HROne.Translation.PageErrorMessage.ERROR_INVALID_FILE);
            }

            if (errors.isEmpty())
            {
                try
                {
                    WebUtils.StartFunction(Session, FUNCTION_CODE);
                    dbConn.BeginTransaction();

                    foreach (EPayScaleMap c in pendingList)
                    {
                        // check existing point-salary-mapping
                        DBFilter filter = new DBFilter();
                        OR m_OR = new OR();

                        DBFieldTranscoder idTranscoder = EPayScale.db.getField("SchemeCode").transcoder;
                        if (idTranscoder != null)
                            filter.add(new Match("SchemeCode", idTranscoder.toDB(c.SchemeCode).ToString()));
                        
                        //filter.add(new Match("SchemeCode", c.SchemeCode));
                        filter.add(new Match("Point", c.Point));
                        filter.add(new Match("EffectiveDate", "<=", c.EffectiveDate));

                        m_OR.add(new NullTerm("ExpiryDate"));
                        m_OR.add(new Match("ExpiryDate", ">=", c.EffectiveDate));
                        filter.add(m_OR);
                        filter.add("EffectiveDate", true);

                        ArrayList currentList = EPayScaleMap.db.select(dbConn, filter);

                        if (currentList.Count > 0)
                        {
                            foreach (EPayScaleMap m_currentItem in currentList)
                            {
                                if ((m_currentItem.ExpiryDate.Ticks != 0) && m_currentItem.ExpiryDate.CompareTo(c.EffectiveDate) < 0) // current expiry before new become effective
                                {
                                    ;
                                }
                                else if (m_currentItem.ExpiryDate.Ticks == 0)
                                {
                                    m_currentItem.ExpiryDate = c.EffectiveDate.AddDays(-1);
                                    EPayScaleMap.db.update(dbConn, m_currentItem);
                                }
                                else
                                {
                                    errors.addError("You are trying to insert a new point (" + c.SchemeCode + " - " + c.Point.ToString("0.00") + ") with invalid Effective Date(" + c.EffectiveDate.ToString("yyyy.MM.dd") + "). ");
                                    break;
                                }
                            }

                            if (errors.isEmpty())
                            {
                                EPayScaleMap.db.insert(dbConn, c);
                            }
                        }
                        else
                        {
                            EPayScaleMap.db.insert(dbConn, c);
                        }
                    }


                    if (!errors.isEmpty())
                        dbConn.RollbackTransaction();
                    else
                    {
                        if (m_list.Count > 0)
                            GenerateRecurringPayment();

                        dbConn.CommitTransaction();
                    }

                    WebUtils.EndFunction(dbConn);
                }
                catch (Exception)
                {
                    errors.addError(HROne.Translation.PageErrorMessage.ERROR_INVALID_FILE);

                    dbConn.RollbackTransaction();
                    WebUtils.EndFunction(dbConn);
                }
            }
            else
            {
                errors.addError(HROne.Translation.PageErrorMessage.ERROR_NO_RECORD_IMPORT);
            }
            System.IO.File.Delete(strTmpFile);
        }
        else
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_INVALID_FILE);

//        Repeater.EditItemIndex = -1;
        view = loadData(info, db, Repeater);
        if (errors.isEmpty())
            errors.addError(HROne.Translation.PageMessage.IMPORT_SUCCESSFUL);
    }

    protected void GenerateRecurringPayment()
    {
        DBFilter m_filter = new DBFilter();
        //ArrayList m_existing = 

        m_filter.add(new NullTerm("ExpiryDate"));
        m_filter.add("SchemeCode", true);
        m_filter.add("Point", true);

        foreach (EPayScaleMap o in EPayScaleMap.db.select(dbConn, m_filter))
        {
            DBFilter m_filter2 = new DBFilter();
            m_filter2.add(new NullTerm("EmpRPEffTo"));
            m_filter2.add(new Match("SchemeCode", AppUtils.Encode(EPayScaleMap.db.getField("SchemeCode"), o.SchemeCode)));
            m_filter2.add(new Match("Point", o.Point));

            foreach (EEmpRecurringPayment p in EEmpRecurringPayment.db.select(dbConn, m_filter2))
            {
                if (p.EmpRPAmount != System.Convert.ToDouble(o.Salary))
                {
                    EEmpRecurringPayment m_newRP = new EEmpRecurringPayment();
                    m_newRP.Capacity = p.Capacity;
                    m_newRP.CostCenterID = p.CostCenterID;
                    m_newRP.CurrencyID = p.CurrencyID;
                    m_newRP.EmpAccID = p.EmpAccID;
                    m_newRP.EmpID = p.EmpID;
                    m_newRP.EmpRPAmount = System.Convert.ToDouble(o.Salary);

                    // get employee position history -> get payroll group -> get payroll period 
                    DBFilter m_filter3 = new DBFilter();
                    DBFilter m_filter4 = new DBFilter();
                    m_filter3.add(new Match("EmpID", p.EmpID));
                    m_filter3.add(new NullTerm("EmpPosEffTo"));

                    m_filter4.add(new IN("payGroupID", "SELECT payGroupID FROM EmpPositionInfo", m_filter3));
                    m_filter4.add(new Match("PayPeriodStatus", EPayrollPeriod.PAYPERIOD_STATUS_NORMAL_FLAG));
                    
                    foreach (EPayrollPeriod m_payPeriod in EPayrollPeriod.db.select(dbConn, m_filter4))
                    {
                        m_newRP.EmpRPEffFr = m_payPeriod.PayPeriodFr;
                        p.EmpRPEffTo = m_payPeriod.PayPeriodFr.AddDays(-1);
                    }

                    //m_newRP.EmpRPEffFr = p.
                    //m_newRP.EmpRPEffTo = null;
                    m_newRP.EmpRPIsNonPayrollItem = p.EmpRPIsNonPayrollItem;
                    m_newRP.EmpRPMethod = p.EmpRPMethod;
                    m_newRP.EmpRPRemark = p.EmpRPRemark;
                    m_newRP.EmpRPUnit = p.EmpRPUnit;
                    m_newRP.EmpRPUnitPeriodAsDaily = p.EmpRPUnitPeriodAsDaily;
                    m_newRP.EmpRPUnitPeriodAsDailyPayFormID = p.EmpRPUnitPeriodAsDailyPayFormID;
                    m_newRP.PayCodeID = p.PayCodeID;
                    m_newRP.Point = p.Point;
                    m_newRP.SchemeCode = p.SchemeCode;

                    EEmpRecurringPayment.db.update(dbConn, p);
                    EEmpRecurringPayment.db.insert(dbConn, m_newRP);
                }
            }
        }
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


}
