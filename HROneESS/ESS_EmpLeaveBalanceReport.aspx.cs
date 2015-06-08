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

public partial class ESS_EmpLeaveBalanceReport : HROneWebPage
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

        DateTime currentDate = AppUtils.ServerDateTime().Date;
        DateTime asOfDate = currentDate;
        if (!DateTime.TryParse(BalanceAsOfDate.Value, out asOfDate))
            asOfDate = currentDate;
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

            DBFilter empPosFilter = EEmpPositionInfo.CreateDateRangeDBFilter("epi", currentDate, currentDate);
            empPosFilter.add(new IN("epi.AuthorizationWorkFlowIDLeaveApp", "SELECT DISTINCT awfd.AuthorizationWorkFlowID FROM " + EAuthorizationWorkFlowDetail.db.dbclass.tableName + " awfd", authWorkFlowDetailFilter));

            DBFilter empInfoFilter = new DBFilter();
            empInfoFilter.add(new IN("EmpID", "SELECT DISTINCT epi.EmpID FROM " + EEmpPositionInfo.db.dbclass.tableName + " epi", empPosFilter));

            DBFilter empTermFilter = new DBFilter();
            empTermFilter.add(new Match("EmpTermLastDate","<", currentDate <asOfDate?currentDate:asOfDate));
            empInfoFilter.add(new IN("NOT EMPID", "SELECT et.EmpID FROM " + EEmpTermination.db.dbclass.tableName + " et", empTermFilter));

            ArrayList EmpList = EEmpPersonalInfo.db.select(dbConn, empInfoFilter);
            foreach (EEmpPersonalInfo empInfo in EmpList)
            {
                EmpIDList.Add(empInfo.EmpID);
            }
        }            
        
        // Start Ricky So, 0000119, 2014/10/28 
        EmpIDList.Add(WebUtils.GetCurUser(Session).EmpID);
        // End Ricky So, 0000119, 2014/10/28 

        if (EmpIDList.Count > 0)
        {
            ArrayList leaveTypeList = new ArrayList();
            leaveTypeList.Add(ELeaveType.ANNUAL_LEAVE_TYPE(dbConn));
            // Start 0000057, Ricky So, 2014/07/03


            DBFilter m_leaveTypeFilter = new DBFilter();
            DBFilter m_leaveCodeFilter = new DBFilter();

            OR m_orLeaveType = new OR();
            m_orLeaveType.add(new Match("LeaveType", "SLCAT1"));
            m_orLeaveType.add(new Match("LeaveType", "SLCAT2"));

            m_leaveTypeFilter.add(m_orLeaveType);

            m_leaveCodeFilter.add(new IN("LeaveTypeID", "SELECT LeaveTypeID FROM LeaveType", m_leaveTypeFilter));

            ArrayList m_leaveCodeList = ELeaveCode.db.select(dbConn, m_leaveCodeFilter);
            if (m_leaveCodeList.Count > 0)
            {
                leaveTypeList.Add(ELeaveType.SLCAT1_LEAVE_TYPE(dbConn));
                leaveTypeList.Add(ELeaveType.SLCAT2_LEAVE_TYPE(dbConn));
            }

            // End 0000057, Ricky So, 2014/07/03
            HROne.Reports.Employee.LeaveBalanceProcess rpt = new HROne.Reports.Employee.LeaveBalanceProcess(dbConn, EmpIDList, asOfDate, leaveTypeList);
            string reportFileName = WebUtils.GetLocalizedReportFile(Server.MapPath("~/Report_Employee_LeaveBalance.rpt"));
            WebUtils.ReportExport(Response, rpt, reportFileName, "PDF", "LeaveBalance");
        }
        else
            errors.addError("No employee can be generated");
    }

}

