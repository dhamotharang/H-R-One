using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using HROne.DataAccess;
using System.Diagnostics;
//using perspectivemind.validation;

using HROne.Lib.Entities;

public partial class PayScaleMaster : HROneWebPage
{

    private const string FUNCTION_CODE = "SYS020";
    private const string TABLE_NAME = "PayScale";
    protected DBManager db = EPayScale.db;
    protected SearchBinding sbinding;
    protected ListInfo info;
    protected DataView view;
    public Binding binding;
    public Binding ebinding;
    public EPayScale obj;
    public int CurID = -1;
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
        if (ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_PAYSCALE_POINT_SYSTEM) != "Y")
        {
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");
            return;
        }


        AddPanel.Visible = IsAllowEdit;
        IMPORT.Visible = IsAllowEdit;

        binding = new Binding(dbConn, db);
        
        binding.add(SchemeCode);
        binding.add(Capacity);
        binding.add(FirstPoint);
        binding.add(MidPoint);
        binding.add(LastPoint);

        binding.init(Request, Session);

        sbinding = new SearchBinding(dbConn, EPayScale.db);
        sbinding.add(new DropDownVLSearchBinder(SchemeSelect, "SchemeCode", EPayScale.VLDistinctSchemeList, true));       

        
        sbinding.init(DecryptedRequest, null);

        info = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            sbinding.clear();
            sbinding.add(new DropDownVLSearchBinder(SchemeSelect, "SchemeCode", EPayScale.VLDistinctSchemeList, true));
            
            view = loadData(info, db, Repeater);
        }

    }

    public DataView loadData(ListInfo info, DBManager db, DataList repeater)
    {
        //DBFilter filter = sbinding.createFilter();
        DBFilter filter = new DBFilter();

        if (SchemeSelect.SelectedIndex > 0)
        {   
            filter.add(new Match("SchemeCode", AppUtils.Encode(EPayScale.db.getField("SchemeCode"), SchemeSelect.SelectedValue.ToString())));
        }

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
        {
        }
        else
        {
            info.orderby = "SchemeCode, LastPoint";
            info.order = false;
        }
        string select = "c.*";
        string from = "from " + db.dbclass.tableName + " c ";

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
        EPayScale c = new EPayScale();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);

        AND andFilterTerm = new AND();
        andFilterTerm.add(new Match("SchemeCode", AppUtils.Encode(EPayScale.db.getField("SchemeCode"), c.SchemeCode)));
        andFilterTerm.add(new Match("Capacity", AppUtils.Encode(EPayScale.db.getField("Capacity"), c.Capacity)));
        if (!AppUtils.checkDuplicate(dbConn, db, c, errors, "Capacity", andFilterTerm))
            return;

        WebUtils.StartFunction(Session, FUNCTION_CODE);
        db.insert(dbConn, c);
        WebUtils.EndFunction(dbConn);
        
        SchemeCode.Text = string.Empty;
        Capacity.Text = string.Empty;
        FirstPoint.Text = "0.00";
        MidPoint.Text = "0.00";
        LastPoint.Text = "0.00";

        view = loadData(info, db, Repeater);
    }

    protected void Repeater_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        HROne.Common.WebUtility.WebControlsLocalization(Session, e.Item.Controls);

        e.Item.FindControl("DeleteItem").Visible = IsAllowEdit;
        if (e.Item.ItemIndex == Repeater.EditItemIndex)
        {
            ebinding = new Binding(dbConn, db);
            ebinding.add((HtmlInputHidden)e.Item.FindControl("PayScaleID"));
            ebinding.add((TextBox)e.Item.FindControl("SchemeCode"));
            ebinding.add((TextBox)e.Item.FindControl("Capacity"));
            ebinding.add((TextBox)e.Item.FindControl("FirstPoint"));
            ebinding.add((TextBox)e.Item.FindControl("MidPoint"));
            ebinding.add((TextBox)e.Item.FindControl("LastPoint"));
            ebinding.init(Request, Session);


            EPayScale obj = new EPayScale();
            db.toObject(((DataRowView)e.Item.DataItem).Row, obj);
            Hashtable values = new Hashtable();
            db.populate(obj, values);
            ebinding.toControl(values);
        }
        else
        {
            e.Item.FindControl("Edit").Visible = IsAllowEdit;
            HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("PayScaleID");
            h.Value = ((DataRowView)e.Item.DataItem)["PayScaleID"].ToString();
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
            ebinding.add((HtmlInputHidden)e.Item.FindControl("PayScaleID"));
            //ebinding.add(new TextBoxBinder(db, ((WebDatePicker)e.Item.FindControl("StatutoryHolidayDate")).TextBox, "StatutoryHolidayDate"));
            ebinding.add((TextBox)e.Item.FindControl("SchemeCode"));
            ebinding.add((TextBox)e.Item.FindControl("Capacity"));
            ebinding.add((TextBox)e.Item.FindControl("FirstPoint"));
            ebinding.add((TextBox)e.Item.FindControl("MidPoint"));
            ebinding.add((TextBox)e.Item.FindControl("LastPoint"));
            ebinding.init(Request, Session);


            EPayScale obj = new EPayScale();
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
            //if (!AppUtils.checkDuplicate(dbConn, db, obj, errors, "PayScaleID"))
            //    return;

            WebUtils.StartFunction(Session, FUNCTION_CODE);
            db.update(dbConn, obj);
            WebUtils.EndFunction(dbConn);

            Repeater.EditItemIndex = -1;
            view = loadData(info, db, Repeater);
            WebUtils.SetEnabledControlSection(AddPanel, true);
        }
    }

    protected void Delete_Click(object sender, EventArgs e)
    {

        ArrayList list = new ArrayList();
        foreach (DataListItem item in Repeater.Items)
        {
            CheckBox c = (CheckBox)item.FindControl("DeleteItem");
            HtmlInputHidden h = (HtmlInputHidden)item.FindControl("PayScaleID");
            if (c.Checked)
            {
                EPayScale obj = new EPayScale();
                obj.PayScaleID = Int32.Parse(h.Value);
                list.Add(obj);
            }
        }
        foreach (EPayScale obj in list)
        {
            if (EPayScale.db.select(dbConn, obj))
            {
                WebUtils.StartFunction(Session, FUNCTION_CODE);
                db.delete(dbConn, obj);
                WebUtils.EndFunction(dbConn);
            }
        }
        view = loadData(info, db, Repeater);
        //Response.Redirect(Request.Url.LocalPath);
    }

    protected void SchemeSelect_SelectedIndexChanged (object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {

        DataTable tmpDataTable = new DataTable(TABLE_NAME);
        
        tmpDataTable.Columns.Add("Scheme Code", typeof(string));
        tmpDataTable.Columns.Add("Capacity", typeof(string));
        tmpDataTable.Columns.Add("First Point", typeof(Decimal));
        tmpDataTable.Columns.Add("Mid Point", typeof(Decimal));
        tmpDataTable.Columns.Add("Last Point", typeof(Decimal));

        DBFilter filter = sbinding.createFilter();

        filter.add("LastPoint", false);

        ArrayList list = EPayScale.db.select(dbConn, filter);
        foreach (EPayScale c in list)
        {
            DataRow row = tmpDataTable.NewRow();
            //row["ID"] = c.PayScaleID;
            row["Scheme Code"] = c.SchemeCode;
            row["Capacity"] = c.Capacity;
            row["First Point"] = c.FirstPoint;
            row["Mid Point"] = c.MidPoint;
            row["Last Point"] = c.LastPoint;

            tmpDataTable.Rows.Add(row);
        }
        string exportFileName = System.IO.Path.GetTempFileName();
        System.IO.File.Delete(exportFileName);
        exportFileName += ".xls";
        HROne.Export.ExcelExport excelExport = new HROne.Export.ExcelExport(exportFileName);
        excelExport.Update(tmpDataTable);
        WebUtils.TransmitFile(Response, exportFileName, "PayScale_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true);
        return;
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

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        if (BatchFile.HasFile)
        {
            string strTmpFolder = HROne.Common.Folder.GetOrCreateSessionTempFolder(Session.SessionID).FullName; ; //Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);
            string strTmpFile = System.IO.Path.Combine(strTmpFolder, AppUtils.ServerDateTime().ToString("~yyyyMMddHHmmss_") + BatchFile.FileName);
            BatchFile.SaveAs(strTmpFile);

            ArrayList pendingList = new ArrayList();

            try
            {
               
                DataTable rawDataTable = HROne.Import.ExcelImport.parse(strTmpFile, string.Empty).Tables[TABLE_NAME];

                if (rawDataTable.Columns.Contains("Scheme Code") && 
                    rawDataTable.Columns.Contains("Capacity") && 
                    rawDataTable.Columns.Contains("First Point") && 
                    rawDataTable.Columns.Contains("Mid Point") && 
                    rawDataTable.Columns.Contains("Last Point"))
                {
                    int row_count = 0;
                    Decimal point;
                    //int id;
                    foreach (DataRow row in rawDataTable.Rows)
                    {
                        row_count ++;

                        EPayScale c = new EPayScale();

                        if (!row.IsNull("Scheme Code"))
                        {
                            c.SchemeCode = ((String)row["Scheme Code"]).Trim();
                        }
                        else
                        {
                            errors.addError("Missing Scheme Code in row " + row_count.ToString());
                            if (errors.errors.Count > 10) return;
                        }

                        if (!row.IsNull("Capacity"))
                        {
                            c.Capacity = ((String)row["Capacity"]).Trim();
                        }
                        else
                        {
                            errors.addError("Missing Capacity in row " + row_count.ToString());
                            if (errors.errors.Count > 10) return;
                        }

                        if (!row.IsNull("First Point") && Decimal.TryParse(((String)row["First Point"]), out point))
                        {
                            c.FirstPoint = point;
                        }else
                        {
                            errors.addError("Missing First Point in row " + row_count.ToString());
                            if (errors.errors.Count > 10) return;
                        }

                        if (!row.IsNull("Mid Point") && Decimal.TryParse(((String)row["Mid Point"]), out point))
                        {
                            c.MidPoint = point;
                        }
                        else
                        {
                             errors.addError("Missing Mid Point in row " + row_count.ToString());
                            if (errors.errors.Count > 10) return;
                        }

                        if (!row.IsNull("Last Point") && Decimal.TryParse(((String)row["Last Point"]), out point))
                        {
                            c.LastPoint = point;
                        }
                        else
                        {
                            errors.addError("Missing Mid Point in row " + row_count.ToString());
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

            pendingList.Sort(new PayScaleComparer());

            if (pendingList.Count > 0)
            {
                try
                {
                    dbConn.BeginTransaction();
                    WebUtils.StartFunction(Session, FUNCTION_CODE);

                    DBFilter filter = sbinding.createFilter();
                    filter.add("SchemeCode", true);
                    filter.add("Capacity", true);
                    ArrayList currentList = EPayScale.db.select(dbConn, filter);
                    foreach (EPayScale currentItem in currentList)
                    {
                        EPayScale updateItem = null;
                        foreach (EPayScale pendingItem in pendingList)
                        { 
                            if (pendingItem.SchemeCode.Equals(currentItem.SchemeCode) &&  pendingItem.Capacity.Equals(currentItem.Capacity))
                            {
                                updateItem = pendingItem;

                                if (//!currentItem.PayScaleCDesc.Equals(pendingItem.PayScaleCDesc) ||
                                    !currentItem.FirstPoint.Equals(pendingItem.FirstPoint) ||
                                    !currentItem.MidPoint.Equals(pendingItem.MidPoint) ||
                                    !currentItem.LastPoint.Equals(pendingItem.LastPoint))
                                {
                                    //currentItem.FirstPoint = pendingItem.FirstPoint;
                                    //currentItem.MidPoint = pendingItem.MidPoint;
                                    //currentItem.LastPoint = pendingItem.LastPoint;
                                    EPayScale.db.update(dbConn, currentItem);
                                }
                                break;
                            }
                        }
                        if (updateItem != null)
                            pendingList.Remove(updateItem);
                        else
                        {
                            if (!EPayScale.db.delete(dbConn, currentItem))
                            {
                                // failed to delete
                            }
                        }
                    }
                    foreach (EPayScale insertItem in pendingList)
                    {
                        AND andFilterTerm = new AND();
                        andFilterTerm.add(new Match("SchemeCode", insertItem.SchemeCode));
                        andFilterTerm.add(new Match("Capacity", insertItem.Capacity));
                        //if (!AppUtils.checkDuplicate(dbConn, db, insertItem, errors, "Capacity", andFilterTerm))
                        //{
                        //    //errors.addError("Duplicated Rank Description : " + insertItem.PayScaleDesc);
                        //    break;
                        //}
                        try
                        {
                            //                            throw new SqlException();

                            EPayScale.db.insert(dbConn, insertItem);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("Fail to clear temp table: " + ex.Message);

                        }

                    }

                    if (errors.errors.Count > 0)
                        dbConn.RollbackTransaction();
                    else
                        dbConn.CommitTransaction();

                    WebUtils.EndFunction(dbConn);
                }

                catch (Exception ex)
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

        Repeater.EditItemIndex = -1;
        view = loadData(info, db, Repeater);
        if (errors.isEmpty())
            errors.addError(HROne.Translation.PageMessage.IMPORT_SUCCESSFUL);
    }
}
