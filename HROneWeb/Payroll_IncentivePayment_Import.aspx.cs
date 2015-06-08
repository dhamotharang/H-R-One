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
using System.Data.OleDb;
using HROne.Import;
using HROne.Lib.Entities;
using HROne.DataAccess;
//using perspectivemind.validation;

public partial class Payroll_IncentivePayment_Import : HROneWebPage
{
    private const string FUNCTION_CODE = "PAY018";

    private DBManager db = EUploadIncentivePayment.db;
    protected SearchBinding sbinding;
    protected Binding ebinding;
    protected ListInfo info;
    private DataView view;


    protected void Page_Load(object sender, EventArgs e)
    {

        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;

        sbinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);
        //sbinding.add(new HiddenMatchBinder(EmpID));
        //sbinding.initValues("PayCodeID", null, EPaymentCode.VLPaymentCode, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        //sbinding.initValues("CurrencyID", null, Values.VLCurrency, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        //sbinding.initValues("CNDPayMethod", null, Values.VLPaymentMethod, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        //sbinding.initValues("CNDIsRestDayPayment", null, Values.VLYesNo, null);

        sbinding.init(DecryptedRequest, null);

        info = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //if (!Page.IsPostBack)
        {
            view = loadData(info, db, Repeater);
        }
    }

    public DataView loadData(ListInfo info, DBManager db, DataList repeater)
    {
        //filter.add(new Match("EmpPayrollID",EmpPayrollID.Value));


        ImportIncentivePaymentProcess IPImport = new ImportIncentivePaymentProcess(dbConn, Session.SessionID, WebUtils.GetCurUser(Session).UserID);
        DataTable table = IPImport.GetImportDataFromTempDatabase(info);

        if (info != null)
        {
            if (!string.IsNullOrEmpty(info.orderby))
            {
                if (info.orderby.Equals("EmpEngFullName", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (!table.Columns.Contains("EmpEngFullName"))
                    {
                        table.Columns.Add("EmpEngFullName", typeof(string));
                        foreach (System.Data.DataRow row in table.Rows)
                        {
                            EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                            empInfo.EmpID = (int)row["EmpID"];
                            if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                                row["EmpEngFullName"] = empInfo.EmpEngFullName;
                        }
                    }
                }
            }
        }
        table = WebUtils.DataTableSortingAndPaging(table, info);

        view = new DataView(table);
        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }
        if (table.Rows.Count > 0)
            ImportSection.Visible = true;
        else
            ImportSection.Visible = false;

        return view;
    }

    protected void Upload_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        if (IPImportFile.HasFile)
        {
            string strTmpFolder = HROne.Common.Folder.GetOrCreateSessionTempFolder(Session.SessionID).FullName; ; //Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);
            string strTmpFile = System.IO.Path.Combine(strTmpFolder, AppUtils.ServerDateTime().ToString("~yyyyMMddHHmmss_") + IPImportFile.FileName);
            IPImportFile.SaveAs(strTmpFile);

            ImportIncentivePaymentProcess IPImport = new ImportIncentivePaymentProcess(dbConn, Session.SessionID, WebUtils.GetCurUser(Session).UserID);

            try
            {
                DataTable table = IPImport.UploadToTempDatabase(strTmpFile, WebUtils.GetCurUser(Session).UserID, string.Empty);
                ImportSection.Visible = true;
                txtRemark.Text = string.Empty;
            }
            catch (HRImportException ex)
            {
                if (IPImport.errors.List.Count > 0)
                    foreach (string errorString in IPImport.errors.List)
                        errors.addError(errorString);
                else
                    errors.addError(ex.Message);
                IPImport.ClearTempTable();
            }
            System.IO.File.Delete(strTmpFile);
        }
        else
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_INVALID_FILE);
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
        //view = loadData(info, db, Repeater);

    }

    protected void Repeater_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        EUploadIncentivePayment obj = new EUploadIncentivePayment();
        db.toObject(((DataRowView)e.Item.DataItem).Row, obj);

        if (e.Item.ItemIndex == Repeater.EditItemIndex)
        {
            ebinding = new Binding(dbConn, db);
            ebinding.add((HtmlInputHidden)e.Item.FindControl("IPID"));
            ebinding.add((TextBox)e.Item.FindControl("IPPercent"));

            ebinding.init(Request, Session);

            Hashtable values = new Hashtable();
            db.populate(obj, values);

            ebinding.toControl(values);
        }
        else
        {
            HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("UploadIPID");
            h.Value = ((DataRowView)e.Item.DataItem)["UploadIPID"].ToString();

            ebinding = new Binding(dbConn, db);

            ebinding.init(Request, Session);

            Hashtable values = new Hashtable();
            db.populate(obj, values);

            ebinding.toControl(values);
        }
        HROne.Common.WebUtility.WebControlsLocalization(this, e.Item.Controls);
    }

    protected void Import_Click(object sender, EventArgs e)
    {
        if (txtRemark.Text != "")
        {

            ImportIncentivePaymentProcess IPImport = new ImportIncentivePaymentProcess(dbConn, Session.SessionID, WebUtils.GetCurUser(Session).UserID);
            IPImport.Remark = txtRemark.Text;
            WebUtils.StartFunction(Session, FUNCTION_CODE);
            IPImport.ImportToDatabase();
            WebUtils.EndFunction(dbConn);

            PageErrors.getErrors(db, Page).addError(HROne.Translation.PageMessage.IMPORT_SUCCESSFUL);
        }
        else
        {
            PageErrors.getErrors(db, Page).addError("Please input batch remarks");
        }
    }

    protected void ChangePage(object sender, EventArgs e)
    {
        //view = loadData(info, db, Repeater);
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Payroll_IncentivePayment_List.aspx");
    }


}
