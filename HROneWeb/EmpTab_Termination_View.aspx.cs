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

public partial class EmpTab_Termination_View : HROneWebPage
{
    private const string FUNCTION_CODE = "PER011";
    public int CurID = -1;
    private bool IsAllowEdit = true;
    //private bool m_NeedSuperUser = false;

    protected void Page_Load(object sender, EventArgs e)
    {
		//Start 0000173, Miranda, 2015-06-03
        this.Page.Form.DefaultButton = "mainContentPlaceHolder$Jump";
        //End 0000173, Miranda, 2015-06-03
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;

        if (!WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
        {
            IsAllowEdit = false;
            Delete.Visible = false;
            TerminateButton.Visible = false;
        }

        if (!int.TryParse(DecryptedRequest["EmpID"], out CurID))
            CurID = -1;
        EmpID.Value = CurID.ToString();

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

    }


    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (Emp_Termination1.hasTerminated)
        {
            TerminateButton.Text = HROne.Common.WebUtility.GetLocalizedString("Edit");
            Delete.Visible = true & IsAllowEdit;

            //orPayRecIDTerms.add(new NullTerm("PayRecID"));
            DBFilter paidFinalPaymentFilter = new DBFilter();
            paidFinalPaymentFilter.add(new Match("EmpID", CurID));
            paidFinalPaymentFilter.add(new Match("PayRecID", ">", 0));
            if (EEmpFinalPayment.db.count(dbConn, paidFinalPaymentFilter) > 0)
            {
                //m_NeedSuperUser = true;
                Delete.Text = HROne.Common.WebUtility.GetLocalizedString("Re-join");
                Delete.OnClientClick = HROne.Translation.PromptMessage.REJOIN_TERMINATED_EE_JAVASCRIPT;
            }
            else
                Delete.OnClientClick = HROne.Translation.PromptMessage.CreateDeleteConfirmDialogJavascript(Delete);
        }
        else
        {
            TerminateButton.Text = HROne.Common.WebUtility.GetLocalizedString("Terminate");
            Delete.Visible = false;
        }
        Emp_FinalPayment_List1.Visible = Emp_Termination1.hasTerminated;
        TerminateButton.OnClientClick = "document.location='Emp_Termination_Edit.aspx?EmpID=" + CurID + "'; return false;";
    }
    protected void Delete_Click(object sender, EventArgs e)
    {
        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpID", CurID));
        ArrayList empTermList = EEmpTermination.db.select(dbConn, filter);
        ArrayList empFinalPaymentList = EEmpFinalPayment.db.select(dbConn, filter);


        WebUtils.StartFunction(Session, FUNCTION_CODE, CurID);
        foreach (EEmpTermination empTerm in empTermList)
            EEmpTermination.db.delete(dbConn, empTerm);
        foreach (EEmpFinalPayment finalPay in empFinalPaymentList)
            EEmpFinalPayment.db.delete(dbConn, finalPay);
        WebUtils.EndFunction(dbConn);

        EEmpPersonalInfo pi = new EEmpPersonalInfo();
        pi.EmpID = CurID;
        EEmpPersonalInfo.db.select(dbConn, pi);
        pi.EmpStatus = "A";
        EEmpPersonalInfo.db.update(dbConn, pi);


        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_Termination_View.aspx?EmpID=" + CurID);

    }
    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_List.aspx");
    }

    //Start 0000173, Miranda, 2015-04-26
    protected void Jump_Click(object sender, EventArgs e)
    {
        DBFilter dbFilter = new DBFilter();
        OR orEmpNo = new OR();
        orEmpNo.add(new Match("EmpNo", EmpNo.Text));
        DBFieldTranscoder transcoder = EEmpPersonalInfo.db.getField("EmpNo").transcoder;
        if (transcoder != null)
            orEmpNo.add(new Match("EmpNo", transcoder.toDB(EmpNo.Text)));
        dbFilter.add(orEmpNo);
        ArrayList list = EEmpPersonalInfo.db.select(dbConn, dbFilter);
        if (list.Count > 0)
        {
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Emp_View.aspx?EmpID=" + ((EEmpPersonalInfo)list[0]).EmpID);
        }
		//Start 0000173, Miranda, 2015-06-03
        else {
            PageErrors errors = PageErrors.getErrors(EEmpPositionInfo.db, this.Master);
            errors.addError("Employee Not Found");
        }
        //End 0000173, Miranda, 2015-06-03
    }
    //End 0000173, Miranda, 2015-04-26
}
