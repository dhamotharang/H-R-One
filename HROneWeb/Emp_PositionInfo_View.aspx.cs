using System;
using System.Data;
using System.Configuration;
using System.Collections;
// Start 0000125, Miranda, 2014-11-19
using System.Collections.Generic;
// End 0000125, Miranda, 2014-11-19
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using HROne.DataAccess;
//using perspectivemind.validation;
using HROne.Lib.Entities;

public partial class Emp_PositionInfo_View : HROneWebPage
{
    private const string FUNCTION_CODE = "PER007";
    public Binding binding;
    public DBManager db = EEmpPositionInfo.db;
    public EEmpPositionInfo obj;
    public int CurID = -1;
    public int CurEmpID = -1;
    public Hashtable CurElements = new Hashtable();

    protected void Page_PreRender(object sender, EventArgs e)
    {
        eot_row.Visible = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ENABLE_OTCLAIM).Equals("Y", StringComparison.CurrentCultureIgnoreCase);
        lateWaiveRow.Visible = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ENABLE_LATE_WAIVE).Equals("Y", StringComparison.CurrentCultureIgnoreCase);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        if (WebUtils.productLicense(Session).IsESS)
            ESSAuthorizationPanel.Visible = true;
        else
            ESSAuthorizationPanel.Visible = false;

        if (WebUtils.productLicense(Session).IsAttendance)
            AttendancePanel.Visible = true;
        else
            AttendancePanel.Visible = false;

        //if (WebUtils.productLicense(Session).ProductType == HROne.ProductLicense.ProductLicenseType.HROne)
        //{
        //    WorkHourPatternRow.Visible = true;
        //}
        //else
        //{
        //    WorkHourPatternRow.Visible = false;
        //}

        binding = new Binding(dbConn, db);
        binding.add(EmpPosID);
        binding.add(EmpID);
        binding.add(EmpPosEffFr);
        binding.add(EmpPosEffTo);

        // Start 0000125, Miranda, 2014-11-19
        binding.add(new BlankZeroLabelVLBinder(db, CompanyName, "CompanyID", ECompany.VLCompany));
        binding.add(CompanyID);
        // End 0000125, Miranda, 2014-11-19
        binding.add(new BlankZeroLabelVLBinder(db, PositionID, EPosition.VLPosition));
        binding.add(new BlankZeroLabelVLBinder(db, RankID, ERank.VLRank));
        binding.add(new BlankZeroLabelVLBinder(db, EmploymentTypeID, EEmploymentType.VLEmploymentType));
        binding.add(new BlankZeroLabelVLBinder(db, StaffTypeID, EStaffType.VLStaffType));

        // Start 0000125, Miranda, 2014-11-19
        binding.add(new BlankZeroLabelVLBinder(db, PayGroupName, "PayGroupID", EPayrollGroup.VLPayrollGroup));
        binding.add(PayGroupID);
        binding.add(new BlankZeroLabelVLBinder(db, LeavePlanDesc, "LeavePlanID", ELeavePlan.VLLeavePlan));
        binding.add(LeavePlanID);
        // End 0000125, Miranda, 2014-11-19
        binding.add(new CheckBoxBinder(db, EmpPosIsLeavePlanResetEffectiveDate, "EmpPosIsLeavePlanResetEffectiveDate"));
        // Start 0000125, Miranda, 2014-11-19
        binding.add(new BlankZeroLabelVLBinder(db, WorkHourPatternDesc, "WorkHourPatternID", EWorkHourPattern.VLWorkHourPattern));
        binding.add(WorkHourPatternID);
        binding.add(new BlankZeroLabelVLBinder(db, YEBPlanDesc, "YEBPlanID", EYEBPlan.VLYEBPlan));
        binding.add(YEBPlanID);
        // End 0000125, Miranda, 2014-11-19
        binding.add(new BlankZeroLabelVLBinder(db, AuthorizationWorkFlowIDLeaveApp, EAuthorizationWorkFlow.VLAuthorizationWorkFlow));
        binding.add(new BlankZeroLabelVLBinder(db, AuthorizationWorkFlowIDEmpInfoModified, EAuthorizationWorkFlow.VLAuthorizationWorkFlow));
        // Start 0000060, Miranda, 2014-07-13
        binding.add(new BlankZeroLabelVLBinder(db, AuthorizationWorkFlowIDOTClaims, EAuthorizationWorkFlow.VLAuthorizationWorkFlow));
        // End 0000060, Miranda, 2014-07-13
        // Start 0000112, Miranda, 2014-12-10
        binding.add(new BlankZeroLabelVLBinder(db, AuthorizationWorkFlowIDLateWaive, EAuthorizationWorkFlow.VLAuthorizationWorkFlow));
        // End 0000112, Miranda, 2014-12-10
        // Start 0000125, Miranda, 2014-11-19
        binding.add(new BlankZeroLabelVLBinder(db, AttendancePlanDesc, "AttendancePlanID", EAttendancePlan.VLAttendancePlan));
        binding.add(AttendancePlanID);
        // End 0000125, Miranda, 2014-11-19
        //binding.add(new BlankZeroLabelVLBinder(db, RosterTableGroupID, ERosterTableGroup.VLRosterTableGroup));
        binding.add(EmpPosRemark);
        //binding.add(new BlankZeroLabelVLBinder(db, EmpPosDefaultRosterCodeID, ERosterCode.VLRosterCode));
        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["EmpPosID"], out CurID))
            CurID = -1;

        if (!int.TryParse(DecryptedRequest["EmpID"], out CurEmpID))
            CurEmpID = -1;


        EmpID.Value = CurEmpID.ToString();

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (!Page.IsPostBack)
        {
            if (CurID > 0)
                loadObject();
            else
                toolBar.DeleteButton_Visible = false;

            loadHierarchy();
            // Start 0000125, Miranda, 2014-11-19
            setNavigateUrl();
            // End 0000125, Miranda, 2014-11-19
        }
    }
    protected void loadHierarchy()
    {
        DBFilter filter;
        ArrayList list;

        filter = new DBFilter();
        filter.add(new Match("EmpPosID", CurID));
        list = EEmpHierarchy.db.select(dbConn, filter);
        foreach (EEmpHierarchy element in list)
        {
            CurElements[element.HLevelID] = element;
        }

        filter = new DBFilter();
        filter.add("HLevelSeqNo", true);
        list = EHierarchyLevel.db.select(dbConn, filter);
        HierarchyLevel.DataSource = list;
        HierarchyLevel.DataBind();


    }

    protected void HierarchyLevel_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        EHierarchyLevel level = (EHierarchyLevel)e.Item.DataItem;
        Label c = (Label)e.Item.FindControl("HElementID");
        // Start 0000125, Miranda, 2014-11-19
        HyperLink hl = (HyperLink)e.Item.FindControl("hlHElementID");
        // End 0000125, Miranda, 2014-11-19
        EEmpHierarchy h = (EEmpHierarchy)CurElements[level.HLevelID];
        //string selected = null;
        if (h != null)
        {
            EHierarchyElement element = new EHierarchyElement();
            element.HElementID = h.HElementID;
            EHierarchyElement.db.select(dbConn, element);
            c.Text = element.HElementCode + " - " + element.HElementDesc;
            // Start 0000125, Miranda, 2014-11-19
            if (h.HElementID > 0)
            {
                hl.NavigateUrl += h.HElementID;
                hl.ForeColor = System.Drawing.Color.RoyalBlue;
            }
            else
            {
                hl.NavigateUrl = "";
            }
            // End 0000125, Miranda, 2014-11-19
        }
        else
        {
            c.Text = "";
            // Start 0000125, Miranda, 2014-11-19
            hl.NavigateUrl = "";
            // End 0000125, Miranda, 2014-11-19
        }


    }
    protected bool loadObject() 
    {
	    obj=new EEmpPositionInfo();
	    bool isNew=WebFormWorkers.loadKeys(db, obj, DecryptedRequest);

        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpID", CurEmpID));
        filter.add(WebUtils.AddRankFilter(Session, "EmpID", true));
        if (EEmpPersonalInfo.db.count(dbConn, filter) == 0)
            if (CurEmpID <= 0)
                return false;
            else
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");

        if (!db.select(dbConn, obj))
            if (CurID <= 0)
                return false;
            else
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");

        if (obj.EmpID != CurEmpID)
            if (CurID <= 0)
                return false;
            else
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");

        //if (obj.EmpPosIsRosterTableGroupSupervisor)
        //    EmpPosIsRosterTableGroupSupervisor.Visible = true;
        //else
        //    EmpPosIsRosterTableGroupSupervisor.Visible = false;

	    Hashtable values=new Hashtable();
	    db.populate(obj,values);
	    binding.toControl(values);


        return true;
    }

    protected void Delete_Click(object sender, EventArgs e)
    {

        EEmpPositionInfo c = new EEmpPositionInfo();
        c.EmpPosID = CurID;
        if (EEmpPositionInfo.db.select(dbConn, c))
        {
            WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
            db.delete(dbConn, c);
            DBFilter filter = new DBFilter();
            filter.add(new Match("EmpPosID", c.EmpPosID));
            ArrayList existingHierarchyList = EEmpHierarchy.db.select(dbConn, filter);
            foreach (EEmpHierarchy h in existingHierarchyList)
                EEmpHierarchy.db.delete(dbConn, h);
            WebUtils.EndFunction(dbConn);
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_Position_View.aspx?EmpID=" + EmpID.Value);
    }
    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_PositionInfo_Edit.aspx?EmpPosID=" + EmpPosID.Value + "&EmpID=" + EmpID.Value);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_Position_View.aspx?EmpID=" + EmpID.Value);
    }
    // Start 0000125, Miranda, 2014-11-19
    private void setNavigateUrl()
    {
        if (CompanyID.Value.Equals("0"))
        {
            hlCompanyID.NavigateUrl = "";
        } else {
            hlCompanyID.NavigateUrl += CompanyID.Value;
            hlCompanyID.ForeColor = System.Drawing.Color.RoyalBlue;
        }

        if (PayGroupID.Value.Equals("0"))
        {
            hlPayGroupID.NavigateUrl = "";
        } else {
            hlPayGroupID.NavigateUrl += PayGroupID.Value;
            hlPayGroupID.ForeColor = System.Drawing.Color.RoyalBlue;
        }

        if (LeavePlanID.Value.Equals("0"))
        {
            hlLeavePlanID.NavigateUrl = "";
        }
        else
        {
            hlLeavePlanID.NavigateUrl += LeavePlanID.Value;
            hlLeavePlanID.ForeColor = System.Drawing.Color.RoyalBlue;
        }

        if (WorkHourPatternID.Value.Equals("0"))
        {
            hlWorkHourPatternID.NavigateUrl = "";
        } else {
            hlWorkHourPatternID.NavigateUrl += WorkHourPatternID.Value;
            hlWorkHourPatternID.ForeColor = System.Drawing.Color.RoyalBlue;
        }

        if (YEBPlanID.Value.Equals("0"))
        {
            hlYEBPlanID.NavigateUrl = "";
        } else {
            hlYEBPlanID.NavigateUrl += YEBPlanID.Value;
            hlYEBPlanID.ForeColor = System.Drawing.Color.RoyalBlue;
        }

        if (AttendancePlanID.Value.Equals("0"))
        {
            hlAttendancePlanID.NavigateUrl = "";
        }
        else
        {
            hlAttendancePlanID.NavigateUrl += AttendancePlanID.Value;
            hlAttendancePlanID.ForeColor = System.Drawing.Color.RoyalBlue;
        }

        Hashtable wfHash = new Hashtable();
        List<WFSelectValue> workflowList = EAuthorizationWorkFlow.VLAuthorizationWorkFlow.getValues(dbConn, new DBFilter(), null);
        foreach(WFSelectValue sv in workflowList)
        {
            wfHash.Add(sv.name, sv.key);
        }
        string leaveAppWorkflow = AuthorizationWorkFlowIDLeaveApp.Text;
        if (wfHash[leaveAppWorkflow] == null)
        {
            hlAuthWorkFlowIDLeaveApp.NavigateUrl = "";
        }
        else
        {
            hlAuthWorkFlowIDLeaveApp.NavigateUrl += wfHash[leaveAppWorkflow].ToString();
            hlAuthWorkFlowIDLeaveApp.ForeColor = System.Drawing.Color.RoyalBlue;
        }
        string empInfoWorkflow = AuthorizationWorkFlowIDEmpInfoModified.Text;
        if (wfHash[empInfoWorkflow] == null)
        {
            hlAuthWorkflowIDEmpInfo.NavigateUrl = "";
        }
        else
        {
            hlAuthWorkflowIDEmpInfo.NavigateUrl += wfHash[empInfoWorkflow].ToString();
            hlAuthWorkflowIDEmpInfo.ForeColor = System.Drawing.Color.RoyalBlue;
        }
        string otClaimsWorkflow = AuthorizationWorkFlowIDOTClaims.Text;
        if (wfHash[otClaimsWorkflow] == null)
        {
            hlAuthWorkflowIDOTClaims.NavigateUrl = "";
        }
        else
        {
            hlAuthWorkflowIDOTClaims.NavigateUrl += wfHash[otClaimsWorkflow].ToString();
            hlAuthWorkflowIDOTClaims.ForeColor = System.Drawing.Color.RoyalBlue;
        }
        // Start 0000112, Miranda, 2014-12-10
        string lateWaiveWorkflow = AuthorizationWorkFlowIDLateWaive.Text;
        if (wfHash[lateWaiveWorkflow] == null)
        {
            hlAuthWorkflowIDLateWaive.NavigateUrl = "";
        }
        else
        {
            hlAuthWorkflowIDLateWaive.NavigateUrl += wfHash[lateWaiveWorkflow].ToString();
            hlAuthWorkflowIDLateWaive.ForeColor = System.Drawing.Color.RoyalBlue;
        }
        // End 0000112, Miranda, 2014-12-10
    }
    // End 0000125, Miranda, 2014-11-19
}
