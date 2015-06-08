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

public partial class Payroll_CND_Import_History_View : HROneWebPage
{
    private const string FUNCTION_CODE = "PAY003";

    private DBManager db = EClaimsAndDeductions.db;
    protected SearchBinding sbinding;
    protected Binding binding;
    protected ListInfo info;
    private DataView view;
    private int CurID;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        btnUndo.OnClientClick = HROne.Translation.PromptMessage.CreateConfirmDialogJavascript(HROne.Common.WebUtility.GetLocalizedString("PAYROLL_UNDO_CLICK_MESSAGE", ci), btnUndo);
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;

        if (!WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
        {
            toolBar.CustomButton1_Visible = false;
        }

        sbinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);
        sbinding.add(new HiddenMatchBinder(CNDImportBatchID));
        sbinding.initValues("PayCodeID", null, EPaymentCode.VLPaymentCode, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        sbinding.initValues("CurrencyID", null, Values.VLCurrency, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        sbinding.initValues("CNDPayMethod", null, Values.VLPaymentMethod, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        sbinding.initValues("EmpAccID", null, EEmpBankAccount.VLBankAccount,null);

        sbinding.init(DecryptedRequest, null);

        binding = new Binding(dbConn, EClaimsAndDeductionsImportBatch.db);
        binding.add(CNDImportBatchDateTime);
        binding.add(CNDImportBatchID);
        binding.add(new LabelVLBinder(EClaimsAndDeductionsImportBatch.db, CNDImportBatchUploadedBy, EUser.VLUserName));
        binding.add(CNDImportBatchRemark);
        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["CNDImportBatchID"], out CurID))
            CurID = -1;


        info = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (CurID > 0)
                if (!loadObject())
                {
                    //CNDAddPanel.Visible = false;
                    //toolBar.DeleteButton_Visible = false;
                    //IsAllowEdit = false;
                }
            view = loadData(info, db, Repeater);
        }

        //CNDImportFile.ControlStyle.CssClass = "button";
    }
    protected bool loadObject()
    {
        EClaimsAndDeductionsImportBatch obj = new EClaimsAndDeductionsImportBatch();
        bool isNew = WebFormWorkers.loadKeys(EClaimsAndDeductionsImportBatch.db, obj, DecryptedRequest);
        if (!EClaimsAndDeductionsImportBatch.db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        EClaimsAndDeductionsImportBatch.db.populate(obj, values);
        binding.toControl(values);
        return true;
    }

    public DataView loadData(ListInfo info, DBManager db, DataList repeater)
    {
        DBFilter filter = sbinding.createFilter();
        filter.add(new MatchField("c.EmpID", "e.EmpID"));

        DataTable table = filter.loadData(dbConn, null, "e.*, c.* ", " from " + db.dbclass.tableName + " c, " + EEmpPersonalInfo.db.dbclass.tableName + " e");

        if (info != null)
            if (!string.IsNullOrEmpty(info.orderby))
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
        table = WebUtils.DataTableSortingAndPaging(table, info);

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

        Repeater.EditItemIndex = -1;
        view = loadData(info, db, Repeater);

    }

    protected void Repeater_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        EClaimsAndDeductions obj = new EClaimsAndDeductions();
        db.toObject(((DataRowView)e.Item.DataItem).Row, obj);
        Hashtable values = new Hashtable();
        db.populate(obj, values);

        {
            //((Button)e.Item.FindControl("Edit")).Visible = IsAllowEdit;
            //if (obj.PayRecID != null)
            //{
            //    ((Button)e.Item.FindControl("Edit")).Visible = false;
            //    ((CheckBox)e.Item.FindControl("DeleteItem")).Visible = false;
            //}
            HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("CNDID");
            h.Value = obj.CNDID.ToString();



            Label CNDAmount = (Label)e.Item.FindControl("CNDAmount");
            Label CNDRemark = (Label)e.Item.FindControl("CNDRemark");
            Label EmpAccID = (Label)e.Item.FindControl("EmpAccID");

            Binding ebinding = new Binding(dbConn, db);
            ebinding.add(CNDAmount);
            ebinding.add(CNDRemark);
            ebinding.add(new BlankZeroLabelVLBinder(EEmpBankAccount.db, EmpAccID, "EmpAccID", EEmpBankAccount.VLBankAccount).setTextDisplayForZero(HROne.Common.WebUtility.GetLocalizedString(EEmpBankAccount.DEFAULT_BANK_ACCOUNT_TEXT)));
            ebinding.init(Request, Session);
            ebinding.toControl(values);

            DBFilter empRankFilter = new DBFilter();
            empRankFilter.add(WebUtils.AddRankFilter(Session, "EmpID", true));
            empRankFilter.add(new Match("EmpID", obj.EmpID));
            if (EEmpPersonalInfo.db.count(dbConn, empRankFilter) > 0)
            {
                CNDAmount.Text = obj.CNDAmount.ToString("$#,##0.00");
            }
            else
            {
                CNDAmount.Text = "******";
                CNDRemark.Text = "******";
                if (obj.CNDPayMethod.Equals("A", StringComparison.CurrentCultureIgnoreCase))
                    EmpAccID.Text = "******";
            }
        }
    }
    protected void Undo_Click(object sender, EventArgs e)
    {
        DBFilter filter = sbinding.createFilter();
        filter.add("EmpID", true);
        filter.add(new IN("PayRecID", "Select PayRecID from " + EPaymentRecord.db.dbclass.tableName + " pr", new DBFilter()));

        ArrayList CNDList = EClaimsAndDeductions.db.select(dbConn, filter);
        if (CNDList.Count > 0)
        {
            PageErrors errors = PageErrors.getErrors(db, Page.Master);
            errors.clear();

            int curEmpID = 0;
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_CND_IMPORT_BATCH_PAYROLL_PROCESSING);
            foreach (EClaimsAndDeductions CND in CNDList)
            {
                EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                empInfo.EmpID = CND.EmpID;
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                {
                    if (curEmpID != CND.EmpID)
                    {
                        errors.addError("- " + empInfo.EmpNo + ", " + empInfo.EmpEngFullName);
                        curEmpID = CND.EmpID;
                    }
                }

            }
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
            return;
        }
        else
        {
            CNDList = EClaimsAndDeductions.db.select(dbConn, sbinding.createFilter());
            

            WebUtils.StartFunction(Session, FUNCTION_CODE);
            foreach (EClaimsAndDeductions CND in CNDList)
                db.delete(dbConn, CND);
            EClaimsAndDeductionsImportBatch batch = new EClaimsAndDeductionsImportBatch();
            batch.CNDImportBatchID = CurID;
            if (EClaimsAndDeductionsImportBatch.db.select(dbConn, batch))
                EClaimsAndDeductionsImportBatch.db.delete(dbConn, batch);
            WebUtils.EndFunction(dbConn);

            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_CND_Import_History.aspx");
        }
    }
    protected void ChangePage(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);
    }
    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_CND_Import_History.aspx");
    }
}
