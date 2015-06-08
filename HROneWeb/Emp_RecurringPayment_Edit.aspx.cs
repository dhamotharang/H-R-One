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

public partial class Emp_RecurringPayment_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "PER007-1";
    private const string CUSTOM003_FUNCTION_CODE = "CUSTOM003";
    public int CurID = -1;
    public int CurEmpID = -1;
    public int OldEmpRPID = -1;
    //public Hashtable CurEmpRecurringPaymentGroups = new Hashtable();
    //public Hashtable CurRanks = new Hashtable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;


        if (!int.TryParse(DecryptedRequest["EmpID"], out CurEmpID))
            CurEmpID = -1;
        else
            Emp_RecurringPayment_Edit1.CurrentEmpID = CurEmpID;



        if (!int.TryParse(DecryptedRequest["EmpRPID"], out CurID))
            CurID = -1;
        else
            Emp_RecurringPayment_Edit1.CurrentEmpRPID = CurID;

        if (!int.TryParse(DecryptedRequest["OldEmpRPID"], out OldEmpRPID))
            OldEmpRPID = -1;
        else
            Emp_RecurringPayment_Edit1.PrevEmpRPID = OldEmpRPID;



        if (CurID > 0)
            ActionHeader.Text = "Edit";
        else
            ActionHeader.Text = "Add";
        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
        // Start 0000166, KuangWei, 2015-02-04
        init_ShiftDutyCodeDropdown();
        init_PayCalFormulaCodeDropdown();
        // End 0000166, KuangWei, 2015-02-04

        //Start 0000168, Ricky So, 2015-05-07
        ESystemFunction m_function = ESystemFunction.GetObjectByCode(dbConn, CUSTOM003_FUNCTION_CODE);
        if (m_function != null && !m_function.FunctionIsHidden && WebUtils.CheckPermission(Session, CUSTOM003_FUNCTION_CODE, WebUtils.AccessLevel.Read))
        {
            winson_header.Visible = true;
            winson_content.Visible = true;
        }
        //End 0000168, Ricky So, 2015-05-07
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (CurID <= 0)
            {
                toolBar.DeleteButton_Visible = false;
                if (OldEmpRPID <= 0)
                    RecurringPaymentHistoryPanel.Visible = false;
            }
        }
    }    
    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        Emp_RecurringPayment_List1.CurrentEmpID = Emp_RecurringPayment_Edit1.CurrentEmpID;
        Emp_RecurringPayment_List1.CurrentPayCodeID = Emp_RecurringPayment_Edit1.CurrentPayCodeID;
        Emp_RecurringPayment_List1.Refresh(); 
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        // Start 0000166, KuangWei, 2015-02-04

        string m_errorMsg;
        if (saveToWinson(out m_errorMsg))
        {
            if (Emp_RecurringPayment_Edit1.Save())
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_RecurringPayment_View.aspx?EmpID=" + Emp_RecurringPayment_Edit1.CurrentEmpID + "&EmpRPID=" + Emp_RecurringPayment_Edit1.CurrentEmpRPID);
        }
        else
        {
            PageErrors m_errors = PageErrors.getErrors(EEmpRPWinson.db, Page.Master);

            m_errors.addError(m_errorMsg);

        }
        // End 0000166, KuangWei, 2015-02-04


    }
    protected void Delete_Click(object sender, EventArgs e)
    {

        if (Emp_RecurringPayment_Edit1.Delete())
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_Position_View.aspx?EmpID=" + CurEmpID);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_RecurringPayment_View.aspx?EmpRPID=" + CurID + "&EmpID=" + CurEmpID);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_Position_View.aspx?EmpID=" + CurEmpID);

    }
    // Start 0000166, KuangWei, 2015-02-04
    protected void init_ShiftDutyCodeDropdown()
    {
        if (ShiftDutyCode.Items.Count <= 0)
        {
            ShiftDutyCode.Items.Add(new ListItem("Not Selected", "0"));

            DBFilter m_filter = new DBFilter();

            foreach (EShiftDutyCode o in EShiftDutyCode.db.select(dbConn, m_filter))
            {
                string FromTime = o.ShiftDutyFromTime.ToString("HH:mm");
                string ToTime = o.ShiftDutyToTime.ToString("HH:mm");
                ShiftDutyCode.Items.Add(new ListItem(string.Format("{0} - {1} to {2}", o.ShiftDutyCode, FromTime, ToTime), o.ShiftDutyCodeID.ToString()));
            }

            DBFilter s_filter = new DBFilter();
            s_filter.add(new Match("EmpRPID", CurID));
            ArrayList list = EEmpRPWinson.db.select(dbConn, s_filter);
            if (list.Count > 0)
            {
                EEmpRPWinson winson = (EEmpRPWinson)list[0];
                ShiftDutyCode.Items.FindByValue(winson.EmpRPShiftDutyID.ToString()).Selected = true;
            }
        }
    }

    protected void init_PayCalFormulaCodeDropdown()
    {
        if (PayCalFormulaCode.Items.Count <= 0)
        {
            PayCalFormulaCode.Items.Add(new ListItem("Not Selected", "0"));

            DBFilter m_filter = new DBFilter();

            foreach (EPaymentCalculationFormula o in EPaymentCalculationFormula.db.select(dbConn, m_filter)) 
            {
                PayCalFormulaCode.Items.Add(new ListItem(o.PayCalFormulaCode, o.PayCalFormulaID.ToString()));
            }

            DBFilter s_filter = new DBFilter();
            s_filter.add(new Match("EmpRPID", CurID));
            ArrayList list = EEmpRPWinson.db.select(dbConn, s_filter);
            if (list.Count > 0)
            {
                EEmpRPWinson winson = (EEmpRPWinson) list[0];
                PayCalFormulaCode.Items.FindByValue(winson.EmpRPPayCalFormulaID.ToString()).Selected = true;
            }
        }
    }

    protected bool saveToWinson(out string pErrorMsg)
    {
        int dutyCode = 0;
        int payCode = 0;

        if (!int.TryParse(ShiftDutyCode.SelectedValue, out dutyCode))
        {
            pErrorMsg = "Missing Shift Duty Code";
            return false;
        }
        if (!int.TryParse(PayCalFormulaCode.SelectedValue, out payCode))
        {
            pErrorMsg = "Missing Payment Calculation Formula Code";
            return false;
        }

        if ((dutyCode > 0) != (payCode > 0))
        {
            pErrorMsg = "Shift Duty Code and Payment Calculation Formula are required";
            return false; 
        }

        EEmpRPWinson m_winsonRP = EEmpRPWinson.GetObjectByRPID(dbConn, CurID);

        if (m_winsonRP != null)
        {
            m_winsonRP.EmpRPShiftDutyID = dutyCode;
            m_winsonRP.EmpRPPayCalFormulaID = payCode;

            EEmpRPWinson.db.update(dbConn, m_winsonRP);
        }
        else
        {
            m_winsonRP = new EEmpRPWinson();

            m_winsonRP.EmpRPID = CurID;
            m_winsonRP.EmpRPShiftDutyID = dutyCode;
            m_winsonRP.EmpRPPayCalFormulaID = payCode;

            EEmpRPWinson.db.insert(dbConn, m_winsonRP);
        }
        pErrorMsg = "";
        return true;
    }
    // End 0000166, KuangWei, 2015-02-04
}
