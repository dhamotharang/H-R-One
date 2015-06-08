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
using HROne.Payroll;

public partial class Report_Taxation_Report_List : HROneWebPage
{
    private const string FUNCTION_CODE = "RPT300";
    protected SearchBinding sbinding;
    protected Binding binding;
    public DBManager db = ETaxEmp.db;
//    public EPayrollGroup obj;
    public int CurID = -1;

    protected ListInfo info;
    protected DataView view;
    protected int CurrentTaxFormID;
    protected string CurTaxFormType;
    private const string TAXATION_NAME_OF_SIGNATURE = "TAXATION_NAME_OF_SIGNATURE";


    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;

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

        SignatureRow.Visible = !CurTaxFormType.Equals("B");



        sbinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);

        DBFilter taxFormFilter = new DBFilter();
        taxFormFilter.add(new Match("TaxCompID", CurID));
        taxFormFilter.add(new Match("TaxFormType", CurTaxFormType));
        taxFormFilter.add("TaxFormYear", false);
        sbinding.add(new DropDownVLSearchBinder(TaxFormID, "TaxFormID", ETaxForm.VLTaxFormYear).setFilter(taxFormFilter));
        sbinding.init(DecryptedRequest, null);

        if (!int.TryParse(TaxFormID.SelectedValue, out CurrentTaxFormID))
            if (!int.TryParse(DecryptedRequest["TaxFormID"], out CurrentTaxFormID))
                CurrentTaxFormID = -1;
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
        if (!Page.IsPostBack)
        {
            //  may require to add ListItem manually
            WebUtils.AddLanguageOptionstoDropDownList(ReportLanguage);
            if (CurID > 0)
            {
                panelEmpList.Visible = true;
                loadObject();

                view = loadData(info, db, Repeater);
            }
            else
                panelEmpList.Visible = false;
            txtNameOfSignature.Text = ESystemParameter.getParameter(dbConn, TAXATION_NAME_OF_SIGNATURE);
        }
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
    }

    protected void TaxFormType_SelectedIndexChanged(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);
    }

    protected void TaxFormID_SelectedIndexChanged(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);
    }
 
    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {

        DBFilter filter = sbinding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = "e.*, TE.TaxEmpID, et.EmpTermLastDate ";
        string from = "from [" + db.dbclass.tableName + "] TE, EmpPersonalInfo e Left Join EmpTermination et on e.EmpID=et.EmpID";

        //DBFilter taxFormFilter = new DBFilter();
        //taxFormFilter.add(new Match("Tf.TaxFormYear", int.Parse(YearSelect.SelectedValue)));
        //taxFormFilter.add(new Match("Tf.TaxFormType","B"));
        //taxFormFilter.add(new Match("Tf.TaxCompID", int.Parse(TaxCompID.SelectedValue)));

        DBFilter taxEmpFilter = new DBFilter();
        filter.add (new MatchField("TE.EmpID","E.EmpID"));
        filter.add(new Match("TaxFormID", CurrentTaxFormID));
        //filter.add(new IN("Te.TaxFormID","Select Tf.TaxformID from TaxForm tf",taxFormFilter));


        DBFilter empInfoFilter = EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime().Date, AppUtils.ServerDateTime().Date);
        empInfoFilter.add(new MatchField("e.EmpID", "ee.EmpID"));
        filter.add(new Exists(EEmpPersonalInfo.db.dbclass.tableName + " ee", empInfoFilter));


        DateTime dtLastEmploymentDateFrom, dtLastEmploymentDateTo;
        if (DateTime.TryParse(((WebDatePicker)EmployeeSearchControl1.AdditionElementControl.FindControl("LastEmploymentDateFrom")).Value,out dtLastEmploymentDateFrom))
            filter.add(new Match("et.EmpTermLastDate",">=",dtLastEmploymentDateFrom));
        if (DateTime.TryParse(((WebDatePicker)EmployeeSearchControl1.AdditionElementControl.FindControl("LastEmploymentDateTo")).Value ,out dtLastEmploymentDateTo))
            filter.add(new Match("et.EmpTermLastDate","<=",dtLastEmploymentDateTo));


        DataTable table = filter.loadData(dbConn, null, select, from);
        table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);

        if (table.Rows.Count != 0)
            btnPrint.Visible = true;
        else
            btnPrint.Visible = false;

        view = new DataView(table);
        //if (info != null)
        //{
        //    info.loadPageList(null, PrevPage, NextPage, FirstPage, LastPage);

        //    CurPage.Value = info.page.ToString();
        //    NumPage.Value = info.numPage.ToString();
        //}
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

        view = loadData(info, db, Repeater);

    }
    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        cb.Checked = true; 
        WebFormUtils.LoadKeys(db, row, cb);
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        HROne.DataAccess.PageErrors errors = HROne.DataAccess.PageErrors.getErrors(null, Page.Master);
        errors.clear();

        ArrayList list = new ArrayList();
        foreach (RepeaterItem i in Repeater.Items)
        {
            CheckBox cb = (CheckBox)i.FindControl("ItemSelect");
            if (cb.Checked)
            {
                ETaxEmp o = (ETaxEmp)ETaxEmp.db.createObject();
                WebFormUtils.GetKeys(db, o, cb);
                list.Add(o);
            }

        }

        DateTime processDateTime = AppUtils.ServerDateTime() ;


        if (list.Count > 0)
        {
            ESystemParameter.setParameter(dbConn, TAXATION_NAME_OF_SIGNATURE, txtNameOfSignature.Text);

            HROne.Reports.Taxation.TaxationFormProcess.ReportLanguage lang = HROne.Reports.Taxation.TaxationFormProcess.ReportLanguage.English;
            if (ReportLanguage.SelectedValue.Equals("big5"))
                lang = HROne.Reports.Taxation.TaxationFormProcess.ReportLanguage.TraditionalChinese;
            HROne.Reports.Taxation.TaxationFormProcess rpt = new HROne.Reports.Taxation.TaxationFormProcess(dbConn, list, CurrentTaxFormID, CurTaxFormType, txtNameOfSignature.Text, lang);

            string reportFileName = WebUtils.GetLocalizedReportFile(Server.MapPath("~/Report_Taxation_IR56" + CurTaxFormType + ".rpt"));

            WebUtils.ReportExport(dbConn, user, errors, lblReportHeader.Text + " ( " + TaxFormID.SelectedItem.Text + " IR56" + CurTaxFormType + " )", Response, rpt, reportFileName, string.Empty, "IR56" + CurTaxFormType, false);


            //string strTaxEmpIDList = string.Empty;
            //foreach (ETaxEmp o in list)
            //{
            //    if (strTaxEmpIDList == string.Empty)
            //        strTaxEmpIDList = ((ETaxEmp)o).TaxEmpID.ToString();
            //    else
            //        strTaxEmpIDList += "_" + ((ETaxEmp)o).TaxEmpID.ToString();

            //}
            //Server.Transfer("Report_Taxation_Report_View.aspx?"
            //    + "TaxFormID=" + CurrentTaxFormID
            //    + "&TaxFormType=" + CurTaxFormType
            //    + "&TaxEmpID=" + strTaxEmpIDList);            

        }
    }

    protected void Search_Click(object sender, EventArgs e)
    {
        info.page = 0;
        view = loadData(info, db, Repeater);

    }
    protected void Reset_Click(object sender, EventArgs e)
    {
        EmployeeSearchControl1.Reset();
        sbinding.clear();
        info.page = 0;

        view = loadData(info, db, Repeater);


    }
}
