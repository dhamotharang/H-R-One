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


public partial class Payroll_GroupProcessContinuous_Process : HROneWebPage
{
    private const string TRIALRUN_FUNCTION_CODE = "PAY004";
    private const string CONFIRM_FUNCTION_CODE = "PAY007";
    private const string PROCESSEND_FUNCTION_CODE = "PAY008";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, TRIALRUN_FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        if (!WebUtils.CheckAccess(Response, Session, CONFIRM_FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        if (!WebUtils.CheckAccess(Response, Session, PROCESSEND_FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;

        DateTime dtPayPeriodFr = new DateTime(long.Parse(DecryptedRequest["PayPeriodFr"]));
        DateTime dtPayPeriodTo = new DateTime(long.Parse(DecryptedRequest["PayPeriodTo"]));

        DateTime NextPayPeriodFr = dtPayPeriodTo.AddDays(1);
        long nextPayPeriodTick = 0;
        if (long.TryParse(DecryptedRequest["NextPayPeriodFr"], out nextPayPeriodTick))
            NextPayPeriodFr = new DateTime(nextPayPeriodTick);

        int PayBatchID = 0;
        if (!int.TryParse(DecryptedRequest["PayBatchID"], out PayBatchID))
            PayBatchID = 0;

        string[] payGroupStringList = DecryptedRequest["PayGroupIDList"].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        int currentSelectedPayGroupSeq = 0;
        if (!int.TryParse(DecryptedRequest["SelectedPayGroupSeq"], out currentSelectedPayGroupSeq))
            currentSelectedPayGroupSeq = 0;
        else
        {
            if (currentSelectedPayGroupSeq > payGroupStringList.GetUpperBound(0))
            {
                currentSelectedPayGroupSeq = 0;
                dtPayPeriodFr = NextPayPeriodFr;
                NextPayPeriodFr = dtPayPeriodTo.AddDays(1);
                PayBatchID = 0;
            }
        }

        bool SkipRecurringPaymentProcess = DecryptedRequest["SkipRecurringPaymentProcess"].Equals("Yes", StringComparison.CurrentCultureIgnoreCase);
        bool SkipClaimsAndDeductionsProcess = DecryptedRequest["SkipClaimsAndDeductionsProcess"].Equals("Yes", StringComparison.CurrentCultureIgnoreCase);
        bool SkipYearEndBonusProcess = DecryptedRequest["SkipYearEndBonusProcess"].Equals("Yes", StringComparison.CurrentCultureIgnoreCase);
        bool SkipAdditionalRenumerationProcess = DecryptedRequest["SkipAdditionalRenumerationProcess"].Equals("Yes", StringComparison.CurrentCultureIgnoreCase);

        if (dtPayPeriodFr <= dtPayPeriodTo)
        {
            EPayrollPeriod currentPayPeriod = null;
            int currentPayPeriodID = 0;
            if (!int.TryParse(DecryptedRequest["PayPeriodID"], out currentPayPeriodID))
            {
                DBFilter payPeriodFilter = new DBFilter();
                payPeriodFilter.add(new Match("PayGroupID", payGroupStringList[currentSelectedPayGroupSeq]));
                payPeriodFilter.add(new Match("PayPeriodFr", "<=", dtPayPeriodFr));
                payPeriodFilter.add(new Match("PayPeriodTo", ">=", dtPayPeriodFr));
                //  Skip checking payperiodstatus since the next step will check if payroll is being processed
                //payPeriodFilter.add(new Match("PayPeriodStatus", "<>", EPayrollPeriod.PAYPERIOD_STATUS_PROCESSEND_FLAG));
                ArrayList payPeriodList = EPayrollPeriod.db.select(dbConn, payPeriodFilter);
                if (payPeriodList.Count > 0)
                {
                    currentPayPeriod = ((EPayrollPeriod)payPeriodList[0]);
                    currentPayPeriodID = currentPayPeriod.PayPeriodID;
                    if (NextPayPeriodFr > currentPayPeriod.PayPeriodTo.AddDays(1))
                    {
                        NextPayPeriodFr = currentPayPeriod.PayPeriodTo.AddDays(1);
                    }
                }
            }
            else
            {
                currentPayPeriod = new EPayrollPeriod();
                currentPayPeriod.LoadDBObject(dbConn, currentPayPeriodID);
            }
            int intTotal;
            if (int.TryParse(DecryptedRequest["Total"], out intTotal))
                intTotal = 0;
            if (Session["PayrollContinuousProcess_EmpList"] == null)
            {
                DBFilter empPayrollCountFilter = new DBFilter();
                empPayrollCountFilter.add(new Match("PayPeriodID", currentPayPeriodID));

                if (EEmpPayroll.db.count(dbConn, empPayrollCountFilter) > 0)
                {
                    lblProgressMessage.Text = "Payroll has been processed manually. Please complete this payroll cycle manually.";
                    return;
                }

                EPayrollPeriod payPeriod = new EPayrollPeriod();
                payPeriod.PayPeriodID = currentPayPeriodID;
                payPeriod.PayPeriodStatus = "C";
                payPeriod.PayPeriodConfirmDate = AppUtils.ServerDateTime();
                payPeriod.PayPeriodConfirmBy = WebUtils.GetCurUser(Session).UserID;
                EPayrollPeriod.db.update(dbConn, payPeriod);

                EPayrollBatch payBatch = new EPayrollBatch();
                payBatch.PayBatchConfirmDate = AppUtils.ServerDateTime();
                EPayrollBatch.db.insert(dbConn, payBatch);

                PayBatchID = payBatch.PayBatchID;


                DBFilter filter = new DBFilter();

                IN inTerm = new IN("EmpID", "Select epi.EmpID from [EmpPositionInfo] epi, [PayrollPeriod] pp ", filter);

                filter.add(new MatchField("EmpID", "epi.EmpID"));
                filter.add(new MatchField("epi.PayGroupID", "pp.PayGroupID"));
                filter.add(new MatchField("epi.EmpPosEffFr", "<=", "pp.PayPeriodTo"));
                filter.add(new Match("pp.PayPeriodID", currentPayPeriodID));
                filter.add(WebUtils.AddRankFilter(Session, "EmpID", true));

                OR orFilter = new OR();
                orFilter.add(new MatchField("epi.EmpPosEffTo", ">=", "pp.PayPeriodFr"));
                orFilter.add(new NullTerm("epi.EmpPosEffTo"));

                filter.add(orFilter);

                filter.add(new MatchField("EmpDateOfJoin", "<=", "pp.PayPeriodTo "));

                OR otherConstraint = new OR();

                OR orIncludeEmployeeTermination = new OR();

                DBFilter empTerminationFilter = new DBFilter();
                empTerminationFilter.add(new MatchField("et.EmpTermLastDate", ">=", "pp.PayPeriodFr"));
                orIncludeEmployeeTermination.add(new IN(" empid", "Select et.empid from " + EEmpTermination.db.dbclass.tableName + " et ", empTerminationFilter));

                orIncludeEmployeeTermination.add(new IN("NOT empid", "Select et.empid from " + EEmpTermination.db.dbclass.tableName + " et ", new DBFilter()));

                otherConstraint.add(orIncludeEmployeeTermination);

                //OR orIncludeBackPay = new OR();
                DBFilter empCNDFilter = new DBFilter();
                empCNDFilter.add(new MatchField("CNDEffDate", "<=", "pp.PayperiodTo"));
                empCNDFilter.add(new MatchField("CNDEffDate", ">=", "epi.EmpPosEffFr"));

                OR orCNDPos = new OR();
                orCNDPos.add(new MatchField("CNDEffDate", "<=", "epi.EmpPosEffTo"));
                orCNDPos.add(new NullTerm("epi.EmpPosEffTo"));

                empCNDFilter.add(orCNDPos);
                OR orCNDPayRecID = new OR();
                orCNDPayRecID.add(new Match("PayRecID", 0));
                orCNDPayRecID.add(new NullTerm("PayRecID"));
                empCNDFilter.add(orCNDPayRecID);
                otherConstraint.add(new IN(" empid", "Select cnd.empid from " + EClaimsAndDeductions.db.dbclass.tableName + " cnd ", empCNDFilter));
                //otherConstraint.add(orIncludeBackPay);

                filter.add(otherConstraint); 

                DBFilter resultFilter = new DBFilter();
                resultFilter.add(inTerm);
                ArrayList empList = EEmpPersonalInfo.db.select(dbConn, resultFilter);
                Session["PayrollContinuousProcess_EmpList"] = empList;
                intTotal = empList.Count;
            }


            ArrayList list = (ArrayList)Session["PayrollContinuousProcess_EmpList"];
            if (list.Count > 0)
            {

                //string[] strEmpIDListArray;
                //int intProgress = 0;

                //strEmpIDListArray = strEmpIDList.Split(new char[] { '_' });

                //intProgress = 0;
                DateTime dtStartTime = AppUtils.ServerDateTime();
                int UserID = WebUtils.GetCurUser(Session).UserID;

                HROne.Payroll.PayrollProcess payrollProcess = new HROne.Payroll.PayrollProcess(dbConn);

                while (AppUtils.ServerDateTime().Subtract(dtStartTime).Seconds < 30 && list.Count > 0)
                {
                    EEmpPersonalInfo empInfo = (EEmpPersonalInfo)list[0];

                    //int intEmpID;
                    //if (int.TryParse(strEmpIDListArray[intProgress], out intEmpID))
                    //{
                    DBFilter filter = new DBFilter();
                    filter.add(new Match("EmpID", empInfo.EmpID));
                    filter.add(WebUtils.AddRankFilter(new Page().Session, "EmpID", true));
                    if (EEmpPersonalInfo.db.count(dbConn, filter) > 0)
                    {
                        bool ActualRecurringPaymentProcess = !SkipRecurringPaymentProcess;
                        bool ActualYearEndBonusProcess = !SkipYearEndBonusProcess;

                        EEmpTermination empTerm = EEmpTermination.GetObjectByEmpID(dbConn, empInfo.EmpID);
                        if (empTerm!=null)
                            if (empTerm.EmpTermLastDate < dtPayPeriodFr)
                            {
                                ActualRecurringPaymentProcess = false;
                                ActualYearEndBonusProcess = false;
                            }
                        WebUtils.StartFunction(Session, CONFIRM_FUNCTION_CODE, empInfo.EmpID, false);
                        payrollProcess.PayrollConfirm(payrollProcess.PayrollTrialRun(currentPayPeriodID, empInfo.EmpID, ActualRecurringPaymentProcess, !SkipClaimsAndDeductionsProcess, !SkipAdditionalRenumerationProcess, ActualYearEndBonusProcess, false, UserID), PayBatchID, UserID);
                        WebUtils.EndFunction(dbConn);
                    }
                    //}
                    //intProgress++;
                    //if (intProgress == strEmpIDListArray.GetLength(0))
                    //    break;
                    list.Remove(empInfo);
                }
            }
            lblProgress.Text = (intTotal - list.Count).ToString() + " of " + intTotal.ToString();
            if (list.Count == 0)
            {
                Session.Remove("PayrollContinuousProcess_EmpList");

                EPayrollPeriod payPeriod = new EPayrollPeriod();
                payPeriod.PayPeriodID = currentPayPeriodID;
                EPayrollPeriod.db.select(dbConn, payPeriod);
                HROne.Payroll.PayrollProcess payrollProcess=new HROne.Payroll.PayrollProcess(dbConn);
                WebUtils.StartFunction(Session, PROCESSEND_FUNCTION_CODE, false);
                payrollProcess.PayrollProcessEnd(payPeriod.PayPeriodID, WebUtils.GetCurUser(Session).UserID);
                payrollProcess.GenerateNextPayrollPeriod(payPeriod.PayGroupID);
                WebUtils.EndFunction(dbConn);

                currentSelectedPayGroupSeq++;
                string url = Request.Url.AbsolutePath + "?PayGroupIDList=" + string.Join("|", payGroupStringList)
                        + "&PayPeriodFr=" + dtPayPeriodFr.Ticks
                        + "&PayPeriodTo=" + dtPayPeriodTo.Ticks
                        + "&NextPayPeriodFr=" + NextPayPeriodFr.Ticks
                        + "&SelectedPayGroupSeq=" + currentSelectedPayGroupSeq
                        + "&SkipRecurringPaymentProcess=" + (SkipRecurringPaymentProcess ? "Yes" : "No")
                        + "&SkipClaimsAndDeductionsProcess=" + (SkipClaimsAndDeductionsProcess ? "Yes" : "No")
                        + "&SkipYearEndBonusProcess=" + (SkipYearEndBonusProcess ? "Yes" : "No")
                        + "&SkipAdditionalRenumerationProcess=" + (SkipAdditionalRenumerationProcess ? "Yes" : "No")
                        ;
                WebUtils.RegisterRedirectJavaScript(this, url, 500);
                //payPeriod = HROne.Payroll.PayrollProcess.GenerateNextPayrollPeriod(payPeriod.PayGroupID);

                //noOfCycleLeft--;

                //if (noOfCycleLeft > 0)
                //{
                //    if (payPeriod != null)
                //    {
                //        Response.Write(@"<script language='javascript'>");
                //        Response.Write(@"setTimeout('Redirect()',500);");
                //        Response.Write(@"function Redirect()");
                //        Response.Write(@"{");
                //        Response.Write(@"window.location = 'Payroll_ProcessContinuous_Process.aspx?"
                //            + "PayPeriodID=" + payPeriod.PayPeriodID 
                //            + "&PayBatchID=" + 0
                //            + "&NoOfCycleLeft=" + noOfCycleLeft
                //            + "&Total=" + 0
                //            //+ "&EmpID=" + strRemainder 
                //            + "';");
                //        Response.Write(@"}");
                //        Response.Write(@"</script>");
                //    }
                //}
            }
            else
            {
                string url = Request.Url.AbsolutePath + "?PayGroupIDList=" + string.Join("|", payGroupStringList)
                    + "&PayPeriodFr=" + dtPayPeriodFr.Ticks
                    + "&PayPeriodTo=" + dtPayPeriodTo.Ticks
                    + "&PayPeriodID=" + currentPayPeriodID
                    + "&PayBatchID=" + PayBatchID
                    + "&Total=" + intTotal
                    + "&NextPayPeriodFr=" + NextPayPeriodFr.Ticks
                    + "&SelectedPayGroupSeq=" + currentSelectedPayGroupSeq
                        + "&SkipRecurringPaymentProcess=" + (SkipRecurringPaymentProcess ? "Yes" : "No")
                        + "&SkipClaimsAndDeductionsProcess=" + (SkipClaimsAndDeductionsProcess ? "Yes" : "No")
                        + "&SkipYearEndBonusProcess=" + (SkipYearEndBonusProcess ? "Yes" : "No")
                        + "&SkipAdditionalRenumerationProcess=" + (SkipAdditionalRenumerationProcess ? "Yes" : "No")
                    ;
                WebUtils.RegisterRedirectJavaScript(this, url, 500);

            }

            EPayrollGroup payGroup = new EPayrollGroup();
            payGroup.LoadDBObject(dbConn, currentPayPeriod.PayGroupID);
            lblProgressMessage.Text = "Payroll process is in progress (" + payGroup.PayGroupCode + ":" + currentPayPeriod.PayPeriodFr.ToString("yyyy-MM-dd") + " To " + currentPayPeriod.PayPeriodTo.ToString("yyyy-MM-dd") + ")";
        }
        else
            lblProgressMessage.Text = "Payroll process is completed";
        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

    }
}
