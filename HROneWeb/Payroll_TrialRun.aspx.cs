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

public partial class Payroll_TrialRun : HROneWebPage
{
    public Binding binding;
    public DBManager db = EPayrollGroup.db;
    public EPayrollGroup obj;
    public int CurID = -1;
    public int CurPayPeriodID = -1;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, "PAY004", WebUtils.AccessLevel.Read))
            return;

        

        binding = new Binding(dbConn, db);
        //binding.add(new DropDownVLBinder(db,PayGroupID,EPayrollGroup.VLPayrollGroup ) );
        binding.add(CurrentPayPeriodID);

        DBFilter payPeriodFilter = new DBFilter();
        payPeriodFilter.add(new Match("PayPeriodStatus", "<>", "E"));
        if (!int.TryParse(PayGroupID.SelectedValue, out CurID))
            if (!int.TryParse(DecryptedRequest["PayGroupID"], out CurID))
                CurID = -1;
        payPeriodFilter.add(new Match("PayGroupID", CurID));
        payPeriodFilter.add("PayPeriodFr", false);

        binding.add(new DropDownVLBinder(EPayrollPeriod.db, PayPeriodID, EPayrollPeriod.VLPayrollPeriod, payPeriodFilter));

        binding.init(Request, Session);
        if (!int.TryParse(PayPeriodID.SelectedValue, out CurPayPeriodID))
            if (!int.TryParse(DecryptedRequest["PayPeriodID"], out CurPayPeriodID))
            {
                EPayrollGroup obj = new EPayrollGroup();
                obj.PayGroupID = CurID;
                if (EPayrollGroup.db.select(dbConn, obj))
                    CurPayPeriodID = obj.CurrentPayPeriodID;
                else
                    CurPayPeriodID = -1;
            }


        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
        if (!Page.IsPostBack)
        {
            if (CurID > 0)
            {
                loadObject();
            }
            else
                panelPayPeriod.Visible = false;

            initPayrollGroup();
        }
    }

    protected void initPayrollGroup()
    {
        DBFilter m_filter = new DBFilter();
        DBFilter m_userFilter = new DBFilter();

        PayGroupID.Items.Add(new ListItem("Not Selected", "-1"));

        m_userFilter.add(new Match("UserID", WebUtils.GetCurUser(Session).UserID));

        OR m_or = new OR();

        m_or.add(new IN("PayGroupID", "SELECT PayGroupID FROM PayrollGroupUsers ", m_userFilter));
        m_or.add(new Match("PayGroupIsPublic", true));

        m_filter.add(m_or);
        m_filter.add("PayGroupCode", true);

        // since sorting is not feasible directly using DBFilter (because of the encrypted data), we use a local data table as a temp buffer
        DataTable m_localTable = new DataTable();
        m_localTable.Columns.Add("PayGroupCode", typeof(string));
        m_localTable.Columns.Add("PayGroupDesc", typeof(string));
        m_localTable.Columns.Add("PayGroupID", typeof(int));

        foreach (EPayrollGroup o in EPayrollGroup.db.select(dbConn, m_filter))
        {
            DataRow m_row = m_localTable.NewRow();
            m_row["PayGroupCode"] = o.PayGroupCode;
            m_row["PayGroupDesc"] = o.PayGroupDesc;
            m_row["PayGroupID"] = o.PayGroupID;
            m_localTable.Rows.Add(m_row);
        }

        foreach (DataRow m_o in m_localTable.Select("", "payGroupCode"))
        {
            PayGroupID.Items.Add(new ListItem(m_o["PayGroupCode"].ToString() + " - " + m_o["PayGroupDesc"].ToString(), m_o["PayGroupID"].ToString()));
        }
    }

    // Start 0000069, KuangWei, 2014-08-28
    //protected void CompanyID_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    PayGroupID.Items.Clear();
    //    initPayrollGroup();
    //    PayPeriodID.Items.Clear();
    //    PayPeriodID.Items.Add(new ListItem("Not Selected", "-1"));
    //    loadObject();
    //}
    // End 0000069, KuangWei, 2014-08-28

    protected bool loadObject()
    {
        obj = new EPayrollGroup();
        obj.PayGroupID = CurID;
        db.select(dbConn, obj);
        //if (!db.select(dbConn, obj))
            //return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);

        ListItem selectedPayPeriod = PayPeriodID.Items.FindByValue(CurPayPeriodID.ToString());
        if (selectedPayPeriod != null)
            PayPeriodID.SelectedValue = selectedPayPeriod.Value;
        else
        {
            CurPayPeriodID = obj.CurrentPayPeriodID;
            selectedPayPeriod = PayPeriodID.Items.FindByValue(CurPayPeriodID.ToString());
            if (selectedPayPeriod != null)
                PayPeriodID.SelectedValue = selectedPayPeriod.Value;
            else
                CurPayPeriodID = 0;
        }

        ucPayroll_PeriodInfo.CurrentPayPeriodID = CurPayPeriodID;


        if (CurPayPeriodID > 0)
        {
            panelPayPeriod.Visible = true;
            panelTrialRunDetail.Visible = true;
        }
        else
        {
            panelPayPeriod.Visible = false;
            panelTrialRunDetail.Visible = false;
        }
        if (CurPayPeriodID > 0)
        {
            EPayrollPeriod payPeriod = new EPayrollPeriod();
            payPeriod.PayPeriodID = CurPayPeriodID;
            if (EPayrollPeriod.db.select(dbConn, payPeriod))
                if (payPeriod.PayPeriodFr.Month.Equals(obj.PayGroupYEBStartPayrollMonth) || payPeriod.PayPeriodTo.Month.Equals(obj.PayGroupYEBStartPayrollMonth))
                {
                    cbxYearEndBonus.Visible = true;
                    cbxYearEndBonus.Checked = true;
                }
                else
                {
                    cbxYearEndBonus.Visible = false;
                    cbxYearEndBonus.Checked = false;
                }
            else
            {
                cbxYearEndBonus.Visible = true;
                cbxYearEndBonus.Checked = true;
            }
        }
        return true;
    }

    protected void PayGroupID_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadObject();
        //Response.Redirect(Request.Url.LocalPath + "?PayGroupID=" + PayGroupID.SelectedValue );
    }
    protected void PayPeriodID_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadObject();
        //Response.Redirect(Request.Url.LocalPath + "?PayGroupID=" + PayGroupID.SelectedValue + "&PayPeriodID=" + PayPeriodID.SelectedValue);
    }
    protected void btnDisplay_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        if (!cbxClaimsAndDeduction.Checked && !cbxRecurringPayment.Checked && !cbxYearEndBonus.Checked && !cbxAdditionalRemuneration.Checked)
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_REC_CND_NOT_SELECT);

        if (!cbxExistingEE.Checked && !cbxNewJoinEE.Checked && !cbxFinalPayment.Checked)
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_EMP_TYPE_NOT_SELECT);

        if (errors.isEmpty())
        {
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_TrialRun_List.aspx?PayPeriodID=" + CurPayPeriodID
                + "&RecurringPayment=" + (this.cbxRecurringPayment.Checked ? "1" : "0")
                + "&ClaimsAndDeduction=" + (this.cbxClaimsAndDeduction.Checked ? "1" : "0")
                + "&AdditionalRemuneration=" + (this.cbxAdditionalRemuneration.Checked ? "1" : "0")
                + "&YearEndBonus=" + (this.cbxYearEndBonus.Checked ? "1" : "0")
                + "&ExistingEmployee=" + (this.cbxExistingEE.Checked ? "1" : "0")
                + "&NewJoin=" + (this.cbxNewJoinEE.Checked ? "1" : "0")
                + "&FinalPayment=" + (this.cbxFinalPayment.Checked ? "1" : "0")
                );
        }
    }
}