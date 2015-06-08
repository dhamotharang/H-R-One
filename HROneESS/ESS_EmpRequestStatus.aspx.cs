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

public partial class ESS_EmpRequestStatus : HROneWebPage
{
    public Binding binding, RequestBinding;
    public DBManager db = EEmpPersonalInfo.db;
    public EEmpPersonalInfo obj;

    public int CurID = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session))
            return;
        binding = new Binding(dbConn, db);
        binding.init(Request, Session);


        EESSUser user = WebUtils.GetCurUser(Session);
        if (user != null)
        {
            CurID = user.EmpID;
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

    protected void Page_Prerender(object sender, EventArgs e)
    {
        Emp_Request_OT_List.Visible = !(ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ENABLE_OTCLAIM).Equals("N", StringComparison.CurrentCultureIgnoreCase));
        Emp_Request_OTCancel_List.Visible = !(ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ENABLE_OTCLAIM).Equals("N", StringComparison.CurrentCultureIgnoreCase));
        // Start 000112, Ricky So, 2014/12/18
        Emp_Request_LateWaive_List.Visible = !(ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ENABLE_LATE_WAIVE).Equals("N", StringComparison.CurrentCultureIgnoreCase));
        Emp_Request_LateWaiveCancel_List.Visible = !(ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ENABLE_LATE_WAIVE).Equals("N", StringComparison.CurrentCultureIgnoreCase));
        // End 000112, Ricky So, 2014/12/18
    }

    protected bool loadObject()
    {
        obj = new EEmpPersonalInfo();
        obj.EmpID = CurID;
        if (!db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);

        //------------------------------------------------------
        //Select EmpRequestID from the EmpRequest table. Check Employee has submit the employee information
        DBFilter filterStatus = new DBFilter();
        DBManager Requestdb = EEmpRequest.db;

        filterStatus.add(new Match("EmpID", CurID));
        filterStatus.add(new Match("EmpRequestType", EEmpRequest.TYPE_EEPROFILE));
        filterStatus.add(new Match("EmpRequestStatus", "<>", EEmpRequest.STATUS_REJECTED));
        filterStatus.add(new Match("EmpRequestStatus", "<>", EEmpRequest.STATUS_APPROVED));
        filterStatus.add(new Match("EmpRequestStatus", "<>", EEmpRequest.STATUS_CANCELLED));

        if (Requestdb.count(dbConn, filterStatus) > 0)
        {
            ArrayList EmpRequestList = Requestdb.select(dbConn, filterStatus);
            EEmpRequest EmpRequest = (EEmpRequest)EmpRequestList[0];
            uc3Emp_Request_Empinfo.CurrentRequestID = EmpRequest.EmpRequestRecordID;
            uc3Emp_Request_Empinfo.CurrentRequestStatus = HROne.Common.WebUtility.GetLocalizedString(EmpRequest.EmpRequestStatus);
            uc3Emp_Request_Empinfo.Visible = true;
            uc1Emp_info.Visible = false;           

        }
        else
        {
            uc3Emp_Request_Empinfo.Visible = false;
            uc1Emp_info.Visible = true;
        }

        return true;
    }
}
