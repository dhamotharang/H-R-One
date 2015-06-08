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
using HROne.Lib.Entities;

public partial class Payroll_TrialRunProcess : HROneWebPage 
{
    private const string FUNCTION_CODE = "PAY004";

    protected class TrialRunParameter
    {
        public DatabaseConnection dbConn;
        public ArrayList EmpInfoList;
        public int PayPeriodID;
        public bool isRecurringPayment;
        public bool isClaimsAndDeduction;
        public bool isAdditionalRemuneration;
        public bool isYearEndBonus;
        public bool isSkipMPFCal;
        public int UserID;

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;

        HROne.Common.WebUtility.WebControlsLocalization(Session, this.Controls);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(TrialRunProcess));
            thread.IsBackground = true;

            TrialRunParameter parameter = new TrialRunParameter();
            parameter.dbConn = dbConn.createClone();
            parameter.EmpInfoList = (ArrayList)Session["PayrollTrialRun_EmpList"];
            parameter.PayPeriodID = int.Parse(DecryptedRequest["PayPeriodID"]);
            parameter.isRecurringPayment = DecryptedRequest["RecurringPayment"] == "1";
            parameter.isClaimsAndDeduction = DecryptedRequest["ClaimsAndDeduction"] == "1";
            parameter.isAdditionalRemuneration = DecryptedRequest["AdditionalRemuneration"] == "1";
            parameter.isYearEndBonus = DecryptedRequest["YearEndBonus"] == "1";
            parameter.isSkipMPFCal = DecryptedRequest["SkipMPFCal"] == "1";
            parameter.UserID = WebUtils.GetCurUser(Session).UserID;
            if (parameter.EmpInfoList != null)
            {
                thread.Start(parameter);
                if (Session["PayrollTrialRunThread"] != null)
                {
                    ((System.Threading.Thread)Session["PayrollTrialRunThread"]).Abort();
                    Session.Remove("PayrollTrialRunThread");
                }
                Session.Add("PayrollTrialRunThread", thread);
                Timer1.Enabled = true;
            }
            else
            {
                lblProgressMessage.Text = HROne.Common.WebUtility.GetLocalizedString("Payroll trial run process is completed");
            }
        }
    }

    protected void TrialRunProcess(object obj)
    {
        TrialRunParameter parameter = (TrialRunParameter)obj;
        //HROne.DataAccess.DatabaseConnection.SetDefaultDatabaseConnection(parameter.dbConn);
        if (Session["PayrollTrialRunThreadMessage"] == null)
            Session.Add("PayrollTrialRunThreadMessage", string.Empty);
        //string strEmpIDList = DecryptedRequest["EmpID"];
        ArrayList list = parameter.EmpInfoList;
        int intTotal = list.Count;
        
        //if (int.TryParse(DecryptedRequest["Total"], out intTotal) && list.Count > 0)
        //{
        int payPeriodID = parameter.PayPeriodID;
        bool isRecurringPayment = parameter.isRecurringPayment;
        bool isClaimsAndDeduction = parameter.isClaimsAndDeduction;
        bool isYearEndBonus = parameter.isYearEndBonus;
        bool isAdditionalRemuneration = parameter.isAdditionalRemuneration;
        bool isSkipMPFCal = parameter.isSkipMPFCal;

            //string[] strEmpIDListArray;
            //int intProgress = 0;

            //strEmpIDListArray = strEmpIDList.Split(new char[] { '_' });

            //intProgress = 0;
            DateTime dtStartTime = AppUtils.ServerDateTime();
            int UserID = parameter.UserID;

//            while (AppUtils.ServerDateTime().Subtract(dtStartTime).TotalMilliseconds < 200 && list.Count > 0)
            AppUtils.StartFunction(parameter.dbConn, UserID, FUNCTION_CODE, 0, true);
            HROne.Payroll.PayrollProcess payrollProcess = new HROne.Payroll.PayrollProcess(parameter.dbConn);
            while (list.Count > 0)
            {
                EEmpPersonalInfo empInfo = (EEmpPersonalInfo)list[0];

                //int intEmpID;
                //if (int.TryParse(strEmpIDListArray[intProgress], out intEmpID))
                //{
                DBFilter filter = new DBFilter();
                filter.add(new Match("EmpID", empInfo.EmpID));
                filter.add(AppUtils.AddRankDBTerm(UserID, "EmpID", true));
                if (EEmpPersonalInfo.db.count(parameter.dbConn, filter) > 0)
                {
                    bool hasError = false;
                    AppUtils.StartChildFunction(parameter.dbConn, empInfo.EmpID);
                    try
                    {
                        payrollProcess.PayrollTrialRun(payPeriodID, empInfo.EmpID, isRecurringPayment, isClaimsAndDeduction, isAdditionalRemuneration, isYearEndBonus, isSkipMPFCal, UserID);
                    }
                    catch (Exception ex)
                    {
                        Session["PayrollTrialRunThreadProgressMessage"] = ex.ToString();
                        hasError = true;
                    }
                    AppUtils.EndChildFunction(parameter.dbConn);
                    if (hasError)
                        break;
                }
                //}
                //intProgress++;
                //if (intProgress == strEmpIDListArray.GetLength(0))
                //    break;
                list.Remove(empInfo);
                Session["PayrollTrialRunThreadProgressMessage"] = (intTotal - list.Count).ToString() + " of " + intTotal.ToString();
            }
            AppUtils.EndFunction(parameter.dbConn);

            Session.Remove("PayrollTrialRunThread");
            Session.Remove("PayrollTrialRun_EmpList");

            //if (list.Count == 0)
            //{
            //    //Timer1.Enabled = false;
            //    //System.Threading.Thread thread = ((System.Threading.Thread)Session["PayrollTrialRunThread"])..Abort();
            //    Session.Remove("PayrollTrialRunThread");
            //    Session.Remove("PayrollTrialRun_EmpList");
            //}
            //else
            //{
            //    //Session["PayrollTrialRun_EmpList"] = list;

            //    ////string strRemainder = string.Join("_", strEmpIDListArray, intProgress, strEmpIDListArray.GetLength(0) - intProgress);
            //    //Response.Write(@"<script language='javascript'>");
            //    //Response.Write(@"setTimeout('Redirect()',500);");
            //    //Response.Write(@"function Redirect()");
            //    //Response.Write(@"{");
            //    //Response.Write(@"window.location = 'Payroll_TrialRunProcess.aspx?"
            //    //    + "PayPeriodID=" + DecryptedRequest["PayPeriodID"]
            //    //    + "&RecurringPayment=" + DecryptedRequest["RecurringPayment"]
            //    //    + "&ClaimsAndDeduction=" + DecryptedRequest["ClaimsAndDeduction"]
            //    //    + "&YearEndBonus=" + DecryptedRequest["YearEndBonus"]
            //    //    + "&SkipMPFCal=" + DecryptedRequest["SkipMPFCal"]
            //    //    + "&Total=" + intTotal
            //    //    //+ "&EmpID=" + strRemainder 
            //    //    + "';");
            //    //Response.Write(@"}");
            //    //Response.Write(@"</script>");
            //}
        //}
    }
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        if (Session["PayrollTrialRunThreadProgressMessage"] != null)
            lblProgress.Text = Session["PayrollTrialRunThreadProgressMessage"].ToString().Replace("\r\n", "<br/>");
        if (Session["PayrollTrialRunThread"] == null)
        {
            lblProgressMessage.Text = HROne.Common.WebUtility.GetLocalizedString("Payroll trial run process is completed");
            Session.Remove("PayrollTrialRunThreadProgressMessage");
            Timer1.Enabled = false;
        }
    }
}
