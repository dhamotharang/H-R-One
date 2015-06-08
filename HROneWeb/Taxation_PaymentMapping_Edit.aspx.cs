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

public partial class Taxation_PaymentMapping_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "TAX002";
    private DBManager db = ETaxPayment.db;
    protected int CurID;
    private Binding binding;
    protected SearchBinding sbinding;
    protected ListInfo info;
    private DataView view;
    protected ArrayList selectedItemList = new ArrayList();
    
    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;

        binding = new Binding(dbConn, db);

        binding.add(TaxPayID);
        binding.add(new LabelVLBinder(db, TaxPayCode,"TaxPayID", ETaxPayment.VLTaxPayment));
        binding.add(TaxFormType);
        binding.add(TaxPayNature);
        binding.init(Request, Session);

        sbinding = new SearchBinding(dbConn, EPaymentCode.db);

        if (!int.TryParse(DecryptedRequest["TaxPayID"], out CurID))
            CurID = -1;

        info = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (CurID > 0)
            {
                loadObject();
                view = loadData(info, db, Repeater);
            }

        }
    }

    protected bool loadObject()
    {
        ETaxPayment obj = new ETaxPayment();
        bool isNew = WebFormWorkers.loadKeys(ETaxPayment.db, obj, DecryptedRequest);
        if (!ETaxPayment.db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);
        // Start 0000020, KuangWei, 2014-08-19
        if (obj.TaxPayCode.Length == 2 || obj.TaxPayCode == "Others (d)" || obj.TaxPayCode == "Others (e)")   // a.k.a k1,k2,k3 with nature, others (d), others (e)
        // End 0000020, KuangWei, 2014-08-19
            TaxPayNature.Visible = true;
        else
            TaxPayNature.Visible = false;


        return true;
    }

    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        if (IsPostBack)
            selectedItemList = WebUtils.SelectedRepeaterItemToBaseObjectList(EPaymentCode.db, Repeater, "ItemSelect");

        DBFilter filter = sbinding.createFilter();

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);

        string select = " pc.* ";
        string from = "from  PaymentCode pc ";


        DBFilter notTaxPaymentFilter = new DBFilter();
        notTaxPaymentFilter.add(new Match("TaxFormType", "=", TaxFormType.Text));

        DBFilter nottaxPaymentMapFilter = new DBFilter();
        nottaxPaymentMapFilter.add(new Match("TaxPayID", "<>", CurID));
        nottaxPaymentMapFilter.add(new IN("TaxPayID", "Select TaxPayID from TaxPayment", notTaxPaymentFilter));
        
        IN notinTerms = new IN("NOT PaymentCodeID", "select PaymentCodeID from TaxPaymentMap", nottaxPaymentMapFilter);
        DBFilter taxPaymentMapFilter = new DBFilter();
        taxPaymentMapFilter.add(new Match("TaxPayID", CurID));
        IN inTerms = new IN("PaymentCodeID", "select PaymentCodeID from TaxPaymentMap", taxPaymentMapFilter);

        OR orTerms = new OR();
        orTerms.add(notinTerms);
        orTerms.add(inTerms);
        filter.add(orTerms);
        filter.add("PaymentCode", true);

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);

        view = new DataView(table);
        //if (info != null)
        //{
        //    info.loadPageList(null, PrevPage, NextPage, FirstPage, LastPage);

        //    CurPage.Value = info.page.ToString();
        //    NumPage.Value = info.numPage.ToString();
        //}
        if (repeater != null)
        {
            repeater.DataSource = table;
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
        WebFormUtils.LoadKeys(EPaymentCode.db, row, cb);

        EPaymentCode obj= new EPaymentCode();
        EPaymentCode.db.toObject(row.Row, obj);

        if (!Page.IsPostBack)
        {
            DBFilter taxPaymentMapFilter = new DBFilter();
            taxPaymentMapFilter.add(new Match("TaxPayID", CurID));
            taxPaymentMapFilter.add(new Match("PaymentCodeID", row["PaymentCodeID"]));
            ArrayList taxPaymentMaps = ETaxPaymentMap.db.select(dbConn, taxPaymentMapFilter);
            if (taxPaymentMaps.Count != 0)
                cb.Checked = true;
        }
        else
        {
            foreach (BaseObject selectedObj in selectedItemList)
            {
                bool isSelected = true;
                foreach (DBField keyObj in EPaymentCode.db.keys)
                {
                    if (!keyObj.getValue(selectedObj).Equals(keyObj.getValue(obj)))
                    {
                        isSelected = false;
                    }
                }
                if (isSelected)
                {
                    cb.Checked = true;
                    break;
                }
            }
        }
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        ETaxPayment c = new ETaxPayment();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
        {
            if (CurID > 0)
            {
                loadObject();
                view = loadData(info, db, Repeater);
            } 
            return;
        }

        db.parse(values, c);


        if (!errors.isEmpty())
        {
            if (CurID > 0)
            {
                loadObject();
                view = loadData(info, db, Repeater);
            }
            return;
        }

        WebUtils.StartFunction(Session, FUNCTION_CODE);
        db.update(dbConn, c);

        ArrayList selectedList = new ArrayList();
        ArrayList unselectedList = new ArrayList();
        foreach (RepeaterItem i in Repeater.Items)
        {
            CheckBox cb = (CheckBox)i.FindControl("ItemSelect");
                BaseObject  o = new EPaymentCode();
                WebFormUtils.GetKeys(EPaymentCode.db, o, cb);
            if (cb.Checked)
                selectedList.Add(o);
            else
                unselectedList.Add(o);

        }
        foreach (EPaymentCode o in selectedList)
        {
            DBFilter taxPaymentMapFilter = new DBFilter();
            taxPaymentMapFilter.add(new Match("TaxPayID",c.TaxPayID ));
            taxPaymentMapFilter.add(new Match("PaymentCodeID",o.PaymentCodeID ));
            ArrayList taxPaymentMaps = ETaxPaymentMap.db.select(dbConn, taxPaymentMapFilter);
            if (taxPaymentMaps.Count == 0)
            {
                ETaxPaymentMap taxPaymentMap = new ETaxPaymentMap();
                taxPaymentMap.TaxPayID = c.TaxPayID;
                taxPaymentMap.PaymentCodeID = o.PaymentCodeID;
                ETaxPaymentMap.db.insert(dbConn, taxPaymentMap);
            }
        }

        foreach (EPaymentCode o in unselectedList)
        {
            DBFilter taxPaymentMapFilter = new DBFilter();
            taxPaymentMapFilter.add(new Match("TaxPayID", c.TaxPayID));
            taxPaymentMapFilter.add(new Match("PaymentCodeID", o.PaymentCodeID));
            ArrayList taxPaymentMaps = ETaxPaymentMap.db.select(dbConn, taxPaymentMapFilter);
            if (taxPaymentMaps.Count != 0)
            {
                foreach (ETaxPaymentMap taxPaymentMap in taxPaymentMaps)
                    ETaxPaymentMap.db.delete(dbConn, taxPaymentMap);
            }
        }
        WebUtils.EndFunction(dbConn);

        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Taxation_PaymentMapping_View.aspx?TaxFormType=" + TaxFormType.Text + "&TaxPayID=" + c.TaxPayID);
    }
    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Taxation_PaymentMapping_View.aspx?TaxFormType=" + TaxFormType.Text + "&TaxPayID=" + TaxPayID.Value);
    }

}
