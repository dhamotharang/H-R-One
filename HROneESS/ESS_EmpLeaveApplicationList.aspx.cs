using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using HROne.DataAccess;
//using perspectivemind.validation;
using HROne.Lib.Entities;

public partial class ESS_EmpLeaveApplicationList : HROneWebPage
{
    public Binding binding;
    public DBManager db = EEmpPersonalInfo.db;
    public int CurID = -1;



    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session))
            return;
        binding = new Binding(dbConn, db);

        EESSUser user = WebUtils.GetCurUser(Session);
        if (user != null)
            CurID = user.EmpID;


        if (!IsPostBack)
        {

        }

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);




        if (!Page.IsPostBack)
        {

            if (CurID > 0)
            {
                loadObject();
            }
            else
            {

            }
        }
    }

    protected bool loadObject()
    {
        EEmpPersonalInfo obj = new EEmpPersonalInfo();
        obj.EmpID = CurID;
        // bool isNew = WebFormWorkers.loadKeys(db, obj, Request);
        if (!db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);
        return true;
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        DateTime dtLeaveAppDateFr = new DateTime();
        DateTime dtLeaveAppDateTo = new DateTime();

        if (!DateTime.TryParse(periodFrom.Value, out dtLeaveAppDateFr))
            dtLeaveAppDateFr = new DateTime(1940, 1, 1);
            

        if (!DateTime.TryParse(periodTo.Value, out dtLeaveAppDateTo))
            dtLeaveAppDateTo = AppUtils.ServerDateTime().Date;



        // Start 0000182, Ricky So, 2015-04-06
        ESSAuthorizationProcess authProcess = new ESSAuthorizationProcess(dbConn);
        List<EAuthorizationGroup> groupList = authProcess.GetAuthorizerAuthorizationGroupList(CurID);
        ArrayList EmpIDList = new ArrayList();
        if (groupList.Count > 0)
        {
            DBFilter authWorkFlowDetailFilter = new DBFilter();
            string authGroupIDList = string.Empty;
            foreach (EAuthorizationGroup group in groupList)
            {
                if (string.IsNullOrEmpty(authGroupIDList))
                    authGroupIDList = group.AuthorizationGroupID.ToString();
                else
                    authGroupIDList += ", " + group.AuthorizationGroupID.ToString();
            }
            authWorkFlowDetailFilter.add(new IN("AuthorizationGroupID", authGroupIDList, null));

            DBFilter empPosFilter = EEmpPositionInfo.CreateDateRangeDBFilter("epi", dtLeaveAppDateTo, dtLeaveAppDateTo);
            empPosFilter.add(new IN("epi.AuthorizationWorkFlowIDLeaveApp", "SELECT DISTINCT awfd.AuthorizationWorkFlowID FROM " + EAuthorizationWorkFlowDetail.db.dbclass.tableName + " awfd", authWorkFlowDetailFilter));

            DBFilter empInfoFilter = new DBFilter();
            empInfoFilter.add(new IN("EmpID", "SELECT DISTINCT epi.EmpID FROM " + EEmpPositionInfo.db.dbclass.tableName + " epi", empPosFilter));

            DBFilter empTermFilter = new DBFilter();
            empTermFilter.add(new Match("EmpTermLastDate", "<", dtLeaveAppDateTo));
            empInfoFilter.add(new IN("NOT EMPID", "SELECT et.EmpID FROM " + EEmpTermination.db.dbclass.tableName + " et", empTermFilter));

            ArrayList EmpList = EEmpPersonalInfo.db.select(dbConn, empInfoFilter);
            foreach (EEmpPersonalInfo empInfo in EmpList)
            {
                EmpIDList.Add(empInfo.EmpID);
            }
        }

        EmpIDList.Add(CurID);

        HROne.Reports.Employee.LeaveApplicationProcess rpt = new HROne.Reports.Employee.LeaveApplicationProcess(dbConn, EmpIDList, dtLeaveAppDateFr, dtLeaveAppDateTo);
        string reportFileName = WebUtils.GetLocalizedReportFile(Server.MapPath("~/LeaveApplicationProcessList.rpt"));
        WebUtils.ReportExport(Response, rpt, reportFileName, "PDF", "LeaveApplicationList");

        //ArrayList values = new ArrayList();

        //values.Add(CurID);

        //if (errors.isEmpty())
        //{
        //    HROne.Reports.Employee.LeaveApplicationProcess rpt = new HROne.Reports.Employee.LeaveApplicationProcess(dbConn, values, dtLeaveAppDateFr, dtLeaveAppDateTo);
        //    string reportFileName = WebUtils.GetLocalizedReportFile(Server.MapPath("~/LeaveApplicationProcessList.rpt"));
        //    //WebUtils.ReportExport(dbConn, user, errors, lblReportHeader.Text, Response, rpt, reportFileName, ((Button)sender).CommandArgument, "LeaveApplicationList", true);
        //    WebUtils.ReportExport(Response, rpt, reportFileName, "PDF", "LeaveApplicationList");
        //}

        // End 0000182, Ricky So, 2015-04-06
    }

}

