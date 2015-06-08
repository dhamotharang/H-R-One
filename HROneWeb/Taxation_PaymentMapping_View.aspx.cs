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
using HROne.Lib.Entities;
using HROne.DataAccess;
//using perspectivemind.validation;

public partial class Taxation_PaymentMapping_View : HROneWebPage
{
    private const string FUNCTION_CODE = "TAX002";
    
    private DBManager db = ETaxPaymentMap.db;
    //private string CurTaxFormType;
    private int CurID = -1;
    protected Binding binding;
    protected SearchBinding taxFormSBinding;
    protected ListInfo paymentMappedInfo;
    protected ListInfo paymentNotMappedInfo;
    private DataView view;
    //private bool IsAllowEdit = true;

    
    protected void Page_Load(object sender, EventArgs e)
    {
       

        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;

        toolBar.FunctionCode = FUNCTION_CODE;


        binding = new Binding(dbConn, db);
        taxFormSBinding = new SearchBinding(dbConn, EPaymentCode.db);
        // Start 0000020, KuangWei, 2014-08-01
        taxFormSBinding.add(new DropDownVLSearchBinder(TaxFormType, "TaxFormType", new AppUtils.NewWFTextList(new string[] { "B", "E", "F", "G", "M" }, new string[] { "B", "E", "F", "G", "M" }), false));
        // End 0000020, KuangWei, 2014-08-01
        taxFormSBinding.init(DecryptedRequest, null);
        if (!Page.IsPostBack)
        {
            if (DecryptedRequest["TaxFormType"] != null)
            {
                try
                {
                    TaxFormType.SelectedValue = DecryptedRequest["TaxFormType"];
                }
                catch
                {
                }
            }
        }
        binding.add(new DropDownVLBinder(ETaxPayment.db, TaxPayID, ETaxPayment.VLTaxPaymentWithNature, taxFormSBinding.createFilter()).setNotSelected(""));

        binding.init(Request, Session);


        //try
        //{
        //    CurID = Int32.Parse(DecryptedRequest["TaxPayID"]);
        //}
        //catch
        //{
        //    if (TaxPayID.Items.Count > 1)
        //        CurID = Int32.Parse(TaxPayID.Items[1].Value);
        //}
        if (!int.TryParse(TaxPayID.SelectedValue, out CurID))
            if (!int.TryParse(DecryptedRequest["TaxPayID"], out CurID))
                CurID = -1;
            else
            {
                try
                {
                    TaxPayID.SelectedValue = CurID.ToString();
                }
                catch
                {
                    CurID = -1;
                    TaxPayID.SelectedIndex = 0;
                }
            }
        if (CurID <= 0 && TaxPayID.Items.Count > 1)
        {
            TaxPayID.SelectedIndex = 1;
            int.TryParse(TaxPayID.SelectedValue, out CurID);
        }
        paymentMappedInfo = paymentMappedListFooter.ListInfo;
        paymentNotMappedInfo = paymentNotMappedListFooter.ListInfo;
        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (CurID > 0)
            {
                loadObject();
                view = paymentMappedLoadData(paymentMappedInfo, db, paymentMappedRepeater);
                view = paymentNotMappedLoadData(paymentNotMappedInfo, db, paymentNotMappedRepeater);
            }
            else
                toolBar.EditButton_Visible = false;

        }

    }

    protected bool loadObject()
    {
        paymentMappedInfo.page = 0;
        paymentNotMappedInfo.page = 0;

        ETaxPayment obj = new ETaxPayment();
        bool isNew = WebFormWorkers.loadKeys(ETaxPayment.db, obj, DecryptedRequest);
        if (!ETaxPayment.db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
//        db.populate(obj, values);
//        binding.toControl(values);



        return true;
    }

    public DataView paymentMappedLoadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = new DBFilter();

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);

        string select = " pc.* , tpm.*";
        string from = "from [" + db.dbclass.tableName + "] tpm, PaymentCode pc ";

        filter.add(new MatchField("tpm.PaymentCodeID", "pc.PaymentCodeID"));
        filter.add(new Match("TaxPayID", CurID));
        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);

        view = new DataView(table);

        paymentMappedListFooter.Refresh();

        if (repeater != null)
        {
            repeater.DataSource = table;
            repeater.DataBind();
        }

        return view;
    }

    public DataView paymentNotMappedLoadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = new DBFilter();

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);

        string select = " pc.* ";
        string from = "from " + EPaymentCode.db.dbclass.tableName + " pc ";

        DBFilter notExistFilter = new DBFilter();
        notExistFilter.add(new MatchField("tpm.PaymentCodeID", "pc.PaymentCodeID"));

        DBFilter notTaxPaymentFilter = new DBFilter();
        notTaxPaymentFilter.add(new Match("TaxFormType", "=", TaxFormType.Text));
        notExistFilter.add(new IN("TaxPayID", "Select TaxPayID from TaxPayment", notTaxPaymentFilter));

        filter.add(new Exists(db.dbclass.tableName + " tpm", notExistFilter, true));
        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);

        view = new DataView(table);

        paymentNotMappedListFooter.Refresh();

        if (repeater != null)
        {
            repeater.DataSource = table;
            repeater.DataBind();
        }

        return view;
    }

    protected void paymentMappedFirstPage_Click(object sender, EventArgs e)
    {
        view = paymentMappedLoadData(paymentMappedInfo, db, paymentMappedRepeater);

    }
    protected void paymentMappedPrevPage_Click(object sender, EventArgs e)
    {
        view = paymentMappedLoadData(paymentMappedInfo, db, paymentMappedRepeater);

    }
    protected void paymentMappedNextPage_Click(object sender, EventArgs e)
    {
        view = paymentMappedLoadData(paymentMappedInfo, db, paymentMappedRepeater);

    }
    protected void paymentMappedLastPage_Click(object sender, EventArgs e)
    {
        view = paymentMappedLoadData(paymentMappedInfo, db, paymentMappedRepeater);

    }
    protected void paymentMappedChangeOrder_Click(object sender, EventArgs e)
    {
        LinkButton l = (LinkButton)sender;
        String id = l.ID.Substring(l.ID.IndexOf("_") + 1);
        if (paymentMappedInfo.orderby == null)
            paymentMappedInfo.order = true;
        else if (paymentMappedInfo.orderby.Equals(id))
            paymentMappedInfo.order = !paymentMappedInfo.order;
        else
            paymentMappedInfo.order = true;
        paymentMappedInfo.orderby = id;

        view = paymentMappedLoadData(paymentMappedInfo, db, paymentMappedRepeater);

    }

    protected void paymentNotMappedFirstPage_Click(object sender, EventArgs e)
    {
        view = paymentNotMappedLoadData(paymentNotMappedInfo, db, paymentNotMappedRepeater);

    }
    protected void paymentNotMappedPrevPage_Click(object sender, EventArgs e)
    {
        view = paymentNotMappedLoadData(paymentNotMappedInfo, db, paymentNotMappedRepeater);

    }
    protected void paymentNotMappedNextPage_Click(object sender, EventArgs e)
    {
        view = paymentNotMappedLoadData(paymentNotMappedInfo, db, paymentNotMappedRepeater);

    }
    protected void paymentNotMappedLastPage_Click(object sender, EventArgs e)
    {
        view = paymentNotMappedLoadData(paymentNotMappedInfo, db, paymentNotMappedRepeater);

    }
    protected void paymentNotMappedChangeOrder_Click(object sender, EventArgs e)
    {
        LinkButton l = (LinkButton)sender;
        String id = l.ID.Substring(l.ID.IndexOf("_") + 1);
        if (paymentNotMappedInfo.orderby == null)
            paymentNotMappedInfo.order = true;
        else if (paymentNotMappedInfo.orderby.Equals(id))
            paymentNotMappedInfo.order = !paymentNotMappedInfo.order;
        else
            paymentNotMappedInfo.order = true;
        paymentNotMappedInfo.orderby = id;

        view = paymentNotMappedLoadData(paymentNotMappedInfo, db, paymentNotMappedRepeater);

    }

    protected void paymentMappedRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(db, row, cb);
    }

    protected void paymentNotMappedRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(EPaymentCode.db, row, cb);
    }

    protected void TaxPayID_SelectedIndexChanged(object sender, EventArgs e)
    {
        //HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Taxation_PaymentMapping_View.aspx?TaxFormType=" + TaxFormType.SelectedValue + "&TaxPayID=" + TaxPayID.SelectedValue);
        loadObject();
        view = paymentMappedLoadData(paymentMappedInfo, db, paymentMappedRepeater);
        view = paymentNotMappedLoadData(paymentNotMappedInfo, db, paymentNotMappedRepeater);

    }
    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Taxation_PaymentMapping_Edit.aspx?TaxPayID=" + TaxPayID.SelectedValue);
    }
    protected void TaxFormType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Taxation_PaymentMapping_View.aspx?TaxFormType=" + TaxFormType.SelectedValue + "&TaxPayID=" + TaxPayID.SelectedValue);
        loadObject();
        view = paymentMappedLoadData(paymentMappedInfo, db, paymentMappedRepeater);
        view = paymentNotMappedLoadData(paymentNotMappedInfo, db, paymentNotMappedRepeater);
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        HROne.DataAccess.PageErrors errors = HROne.DataAccess.PageErrors.getErrors(null, Page.Master);
        errors.clear();

        HROne.Reports.Taxation.TaxationPaymentMappingProcess rpt = new HROne.Reports.Taxation.TaxationPaymentMappingProcess(dbConn);
        string reportFileName = WebUtils.GetLocalizedReportFile(Server.MapPath("~/Report_Taxation_PaymentMapping.rpt"));
        WebUtils.ReportExport(dbConn, user, errors, lblReportHeader.Text, Response, rpt, reportFileName, "PDF", "Taxation_PaymentMapping", true);

    }
}
