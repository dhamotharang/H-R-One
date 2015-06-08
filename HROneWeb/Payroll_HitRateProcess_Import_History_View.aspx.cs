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

public partial class Payroll_HitRateProcess_Import_History_View : HROneWebPage
{
    private const string FUNCTION_CODE = "PAY022";

    private DBManager db = EHitRateProcess.db;
    protected SearchBinding sbinding;
    protected Binding binding;
    protected ListInfo info;
    private DataView view;
    private int CurID;


    protected void Page_Load(object sender, EventArgs e)
    {
        //toolBar.CustomButton1_ClientClick = HROne.Translation.PromptMessage.CreateConfirmDialogJavascript(HROne.Common.WebUtility.GetLocalizedString("PAYROLL_UNDO_CLICK_MESSAGE", ci));
        btnUndo.OnClientClick = HROne.Translation.PromptMessage.CreateConfirmDialogJavascript(HROne.Common.WebUtility.GetLocalizedString("ARE_YOU_SURE_TO_CONFIRM", ci), btnUndo);

        
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;

        if (!WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
        {
            toolBar.CustomButton1_Visible = false;
        }

        sbinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);
        sbinding.add(new HiddenMatchBinder(HitRateProcessImportBatchID));

        sbinding.init(DecryptedRequest, null);

        binding = new Binding(dbConn, EHitRateProcessImportBatch.db);
        binding.add(HitRateProcessImportBatchDateTime);
        binding.add(HitRateProcessImportBatchID);
        binding.add(new LabelVLBinder(EHitRateProcessImportBatch.db, HitRateProcessImportBatchUploadedBy, EUser.VLUserName));
        binding.add(HitRateProcessImportBatchRemark);
        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["HitRateProcessImportBatchID"], out CurID))
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
        EHitRateProcessImportBatch obj = new EHitRateProcessImportBatch();
        bool isNew = WebFormWorkers.loadKeys(EHitRateProcessImportBatch.db, obj, DecryptedRequest);
        if (!EHitRateProcessImportBatch.db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        EHitRateProcessImportBatch.db.populate(obj, values);
        binding.toControl(values);
        return true;
    }

    public DataView loadData(ListInfo info, DBManager db, DataList repeater)
    {
        DBFilter filter = sbinding.createFilter();
        filter.add(new MatchField("c.EmpID", "e.EmpID"));

        DataTable table = filter.loadData(dbConn, null, "e.*, c.*", "from " + db.dbclass.tableName + " c, EmpPersonalInfo e ");

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

        Label m_PaymentCodeLabel = (Label)e.Item.FindControl("PaymentCode");
        int m_payCodeID = (int)((DataRowView)e.Item.DataItem)["PayCodeID"];

        EPaymentCode m_payCode = EPaymentCode.GetObject(dbConn, m_payCodeID);
        if (m_payCode != null)
        {
            m_PaymentCodeLabel.Text = m_payCode.PaymentCode;
        }

    }

    protected void Undo_Click(object sender, EventArgs e)
    {
        DBFilter filter = sbinding.createFilter();
        filter.add("EmpID", true);
//        filter.add(new IN("PayRecID", "Select PayRecID from " + EPaymentRecord.db.dbclass.tableName + " pr", new DBFilter()));

        WebUtils.StartFunction(Session, FUNCTION_CODE);
        EHitRateProcess.db.delete(dbConn, filter);
        EHitRateProcessImportBatch.db.delete(dbConn, filter);
        WebUtils.EndFunction(dbConn);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.addError("Upload Batch undone.");

        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_HitRateProcess_Import_History.aspx");
    }
    protected void ChangePage(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);
    }
    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_HitRateProcess_Import_History.aspx");
    }
}
