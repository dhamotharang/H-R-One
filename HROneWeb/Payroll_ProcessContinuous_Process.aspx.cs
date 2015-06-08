//using System;
//using System.Collections;
//using System.Configuration;
//using System.Data;
//using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.HtmlControls;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using HROne.DataAccess;
//using HROne.Lib.Entities;


//public partial class Payroll_ProcessContinuous_Process : HROneWebPage
//{
//    private const string TRIALRUN_FUNCTION_CODE = "PAY004";
//    private const string CONFIRM_FUNCTION_CODE = "PAY007";
//    private const string PROCESSEND_FUNCTION_CODE = "PAY008";

//    protected void Page_Load(object sender, EventArgs e)
//    {
//        if (!WebUtils.CheckAccess(Response, Session, TRIALRUN_FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
//            return;
//        if (!WebUtils.CheckAccess(Response, Session, CONFIRM_FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
//            return;
//        if (!WebUtils.CheckAccess(Response, Session, PROCESSEND_FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
//            return;

//        //string strEmpIDList = DecryptedRequest["EmpID"];
//        int noOfCycleLeft = int.Parse(DecryptedRequest["NoOfCycleLeft"]);
//        int PayPeriodID = int.Parse(DecryptedRequest["PayPeriodID"]);
//        int PayBatchID = 0;
//        if (!int.TryParse(DecryptedRequest["PayBatchID"], out PayBatchID))
//            PayBatchID = 0;

//        int intTotal;
//        if (int.TryParse(DecryptedRequest["Total"], out intTotal))
//            intTotal = 0;
//        if (Session["PayrollContinuousProcess_EmpList"] == null)
//        {
//            DBFilter empPayrollCountFilter = new DBFilter();
//            empPayrollCountFilter.add(new Match("PayPeriodID", PayPeriodID));

//            if (EEmpPayroll.db.count(dbConn, empPayrollCountFilter) > 0)
//            {
//                lblProgressMessage.Text = "Payroll has been processed manually. Please complete this payroll cycle manually.";
//                return;
//            }

//            EPayrollPeriod payPeriod = new EPayrollPeriod();
//            payPeriod.PayPeriodID = PayPeriodID;
//            payPeriod.PayPeriodStatus = "C";
//            payPeriod.PayPeriodConfirmDate = AppUtils.ServerDateTime();
//            payPeriod.PayPeriodConfirmBy = WebUtils.GetCurUser(Session).UserID;
//            EPayrollPeriod.db.update(dbConn, payPeriod);

//            EPayrollBatch payBatch = new EPayrollBatch();
//            payBatch.PayBatchConfirmDate = AppUtils.ServerDateTime();
//            EPayrollBatch.db.insert(dbConn, payBatch);

//            PayBatchID = payBatch.PayBatchID;


//            DBFilter filter= new DBFilter();

//            IN inTerm = new IN("EmpID", "Select ep.EmpID from [EmpPositionInfo] ep, [PayrollPeriod] pp ", filter);

//            filter.add(new MatchField("EmpID", "ep.EmpID"));
//            filter.add(new MatchField("ep.PayGroupID", "pp.PayGroupID"));
//            filter.add(new MatchField("ep.EmpPosEffFr", "<=", "pp.PayPeriodTo"));
//            filter.add(new Match("pp.PayPeriodID", PayPeriodID));
//            filter.add(WebUtils.AddRankFilter(Session, "EmpID", true));

//            OR orFilter = new OR();
//            orFilter.add(new MatchField("ep.EmpPosEffTo", ">=", "pp.PayPeriodFr"));
//            orFilter.add(new NullTerm("ep.EmpPosEffTo"));

//            filter.add(orFilter);

//            filter.add(new MatchField("EmpDateOfJoin", "<=", "pp.PayPeriodTo "));


//            OR orIncludeEmployeeTermination= new OR();

//            DBFilter empTerminationFilter = new DBFilter();
//            empTerminationFilter.add(new MatchField("et.EmpTermLastDate", ">=", "pp.PayPeriodFr"));
//            orIncludeEmployeeTermination.add(new IN(" empid", "Select et.empid from [EmpTermination] et ", empTerminationFilter));

//            orIncludeEmployeeTermination.add(new IN("NOT empid", "Select et.empid from [EmpTermination] et ", new DBFilter()));

//            filter.add(orIncludeEmployeeTermination);

//            DBFilter resultFilter = new DBFilter();
//            resultFilter.add(inTerm);
//            ArrayList empList = EEmpPersonalInfo.db.select(dbConn, resultFilter);
//            Session["PayrollContinuousProcess_EmpList"] = empList;
//            intTotal = empList.Count;
//        }


//        ArrayList list = (ArrayList)Session["PayrollContinuousProcess_EmpList"];
//        if (list.Count > 0)
//        {

//            //string[] strEmpIDListArray;
//            //int intProgress = 0;

//            //strEmpIDListArray = strEmpIDList.Split(new char[] { '_' });

//            //intProgress = 0;
//            DateTime dtStartTime = AppUtils.ServerDateTime();
//            int UserID = WebUtils.GetCurUser(Session).UserID;

//            while (AppUtils.ServerDateTime().Subtract(dtStartTime).Seconds < 30 && list.Count > 0)
//            {
//                EEmpPersonalInfo empInfo = (EEmpPersonalInfo)list[0];

//                //int intEmpID;
//                //if (int.TryParse(strEmpIDListArray[intProgress], out intEmpID))
//                //{
//                DBFilter filter = new DBFilter();
//                filter.add(new Match("EmpID", empInfo.EmpID));
//                filter.add(WebUtils.AddRankFilter(new Page().Session, "EmpID", true));
//                if (EEmpPersonalInfo.db.count(dbConn, filter) > 0)
//                {
//                    WebUtils.StartFunction(Session, CONFIRM_FUNCTION_CODE, empInfo.EmpID, false);
//                    HROne.Payroll.PayrollProcess.PayrollConfirm(HROne.Payroll.PayrollProcess.PayrollTrialRun(int.Parse(DecryptedRequest["PayPeriodID"]), empInfo.EmpID, true, true, true, true, true, UserID), PayBatchID, UserID);
//                    WebUtils.EndFunction(dbConn);
//                }
//                //}
//                //intProgress++;
//                //if (intProgress == strEmpIDListArray.GetLength(0))
//                //    break;
//                list.Remove(empInfo);
//            }

//            lblProgress.Text = (intTotal - list.Count).ToString() + " of " + intTotal.ToString();
//            if (list.Count == 0)
//            {
//                Session.Remove("PayrollContinuousProcess_EmpList");
                
//                EPayrollPeriod payPeriod = new EPayrollPeriod();
//                payPeriod.PayPeriodID = PayPeriodID;
//                EPayrollPeriod.db.select(dbConn, payPeriod);

//                WebUtils.StartFunction(Session, PROCESSEND_FUNCTION_CODE, false);
//                HROne.Payroll.PayrollProcess.PayrollProcessEnd(payPeriod.PayPeriodID, WebUtils.GetCurUser(Session).UserID);
//                payPeriod = HROne.Payroll.PayrollProcess.GenerateNextPayrollPeriod(payPeriod.PayGroupID);
//                WebUtils.EndFunction(dbConn);

//                noOfCycleLeft--;

//                if (noOfCycleLeft > 0)
//                {
//                    if (payPeriod != null)
//                    {
//                        Response.Write(@"<script language='javascript'>");
//                        Response.Write(@"setTimeout('Redirect()',500);");
//                        Response.Write(@"function Redirect()");
//                        Response.Write(@"{");
//                        Response.Write(@"window.location = 'Payroll_ProcessContinuous_Process.aspx?"
//                            + "PayPeriodID=" + payPeriod.PayPeriodID 
//                            + "&PayBatchID=" + 0
//                            + "&NoOfCycleLeft=" + noOfCycleLeft
//                            + "&Total=" + 0
//                            //+ "&EmpID=" + strRemainder 
//                            + "';");
//                        Response.Write(@"}");
//                        Response.Write(@"</script>");
//                    }
//                }
//            }
//            else
//            {

//                //string strRemainder = string.Join("_", strEmpIDListArray, intProgress, strEmpIDListArray.GetLength(0) - intProgress);
//                Response.Write(@"<script language='javascript'>");
//                Response.Write(@"setTimeout('Redirect()',500);");
//                Response.Write(@"function Redirect()");
//                Response.Write(@"{");
//                Response.Write(@"window.location = 'Payroll_ProcessContinuous_Process.aspx?"
//                    + "PayPeriodID=" + PayPeriodID
//                    + "&PayBatchID=" + PayBatchID
//                    + "&NoOfCycleLeft=" + noOfCycleLeft
//                    + "&Total=" + intTotal
//                    //+ "&EmpID=" + strRemainder 
//                    + "';");
//                Response.Write(@"}");
//                Response.Write(@"</script>");
//            }
//        }
//        if (noOfCycleLeft > 0)
//            lblProgressMessage.Text = "Payroll process is in progress (" + noOfCycleLeft + " cycle(s) left)";
//        else
//            lblProgressMessage.Text = "Payroll process is completed";
//        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

//    }
//}
