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

public partial class Attendance_GenerateRecord_Process : HROneWebPage
{

    private const string FUNCTION_CODE = "ATT007";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;

        //string strEmpIDList = DecryptedRequest["EmpID"];
        ArrayList list = (ArrayList)Session["GenerateAttendance_EmpList"];
        int intTotal;
        if (int.TryParse(DecryptedRequest["Total"], out intTotal) && list!=null)
        {

            //string[] strEmpIDListArray;
            //int intProgress = 0;

            //strEmpIDListArray = strEmpIDList.Split(new char[] { '_' });

            //intProgress = 0;
            DateTime dtStartTime = AppUtils.ServerDateTime();

            DateTime dtPeriodFrom = new DateTime(long.Parse(DecryptedRequest["PeriodFrom"]));
            DateTime dtPeriodTo = new DateTime(long.Parse(DecryptedRequest["PeriodTo"]));
            bool hasTimeCardRecord = true;
            if (DecryptedRequest["NoTimeCardRecord"] != null)
                if (DecryptedRequest["NoTimeCardRecord"].ToString().Equals("Y"))
                    hasTimeCardRecord = false;
            HROne.Attendance.AttendanceProcess attendanceProcess = new HROne.Attendance.AttendanceProcess(dbConn);
            while (AppUtils.ServerDateTime().Subtract(dtStartTime).Seconds < 10 && list.Count > 0)
            {
                EEmpPersonalInfo empInfo = (EEmpPersonalInfo)list[0];
                DBFilter filter = new DBFilter();
                filter.add(new Match("EmpID", empInfo.EmpID));
                filter.add(WebUtils.AddRankFilter(new Page().Session, "EmpID", true));
                if (EEmpPersonalInfo.db.count(dbConn, filter) > 0)
                {
                    WebUtils.StartFunction(Session, FUNCTION_CODE, empInfo.EmpID, true);
                    attendanceProcess.GenerateAttendanceData(empInfo.EmpID, dtPeriodFrom, dtPeriodTo, hasTimeCardRecord);
                    WebUtils.EndFunction(dbConn);
                }
                list.Remove(empInfo);
                Session["GenerateAttendance_EmpList"] = list;

            }


            lblProgress.Text = (intTotal -list.Count).ToString() + " of " + intTotal.ToString();
            if (list.Count == 0)
            {
                Session.Remove("GenerateAttendance_EmpList");
                lblProgressMessage.Text = "Attendance Record Generation is completed";
            }
            else
            {

                //string strRemainder = string.Join("_", strEmpIDListArray, intProgress, strEmpIDListArray.GetLength(0) - intProgress);


                //string strRedirectScript = string.Empty;
                //strRedirectScript += @"<script language='javascript'>" + "\n";
                //strRedirectScript += @"function Redirect()" + "\n";
                //strRedirectScript += @"{" + "\n";
                //strRedirectScript += "window.location = \"" + HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Attendance_GenerateRecord_Process.aspx?"
                //    + "PeriodFrom=" + DecryptedRequest["PeriodFrom"]
                //    + "&PeriodTo=" + DecryptedRequest["PeriodTo"]
                //    + "&Total=" + intTotal
                //    + "&NoTimeCardRecord=" + DecryptedRequest["NoTimeCardRecord"])
                //    + "\";";
                //strRedirectScript += @"}" + "\n";
                //strRedirectScript += @"setTimeout('Redirect()',500);" + "\n";
                //strRedirectScript += @"</script>" + "\n";
                //Response.Write(strRedirectScript);

                string url = "Attendance_GenerateRecord_Process.aspx?"
                    + "PeriodFrom=" + DecryptedRequest["PeriodFrom"]
                    + "&PeriodTo=" + DecryptedRequest["PeriodTo"]
                    + "&Total=" + intTotal
                    + "&NoTimeCardRecord=" + DecryptedRequest["NoTimeCardRecord"];
                WebUtils.RegisterRedirectJavaScript(Page, url, 500);
            }
        }
        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

    }
}
