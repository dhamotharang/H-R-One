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

public partial class Emp_PositionInfo_Edit_Control : HROneWebControl
{

    private const string FUNCTION_CODE = "PER007";
    protected Binding binding;
    protected DBManager db = EEmpPositionInfo.db;
    protected Hashtable CurElements = new Hashtable();
    //protected System.Globalization.CultureInfo ci;

    public int EmpPosID
    {
        get { int CurID = -1; if (int.TryParse(ID.Value, out CurID)) return CurID; else return -1; }
        set { ID.Value = value.ToString(); }
    }

    public int EmpID
    {
        get { int PrevCurID = -1; if (int.TryParse(HiddenEmpID.Value, out PrevCurID)) return PrevCurID; else return -1; }
        set { HiddenEmpID.Value = value.ToString(); }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
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
        binding.add(new HiddenBinder(db, ID, "EmpPosID"));
        binding.add(new HiddenBinder(db, HiddenEmpID, "EmpID"));
        binding.add(new TextBoxBinder(db, EmpPosEffFr.TextBox, EmpPosEffFr.ID));
        binding.add(new TextBoxBinder(db, EmpPosEffTo.TextBox, EmpPosEffTo.ID));

        binding.add(new DropDownVLBinder(db, CompanyID, ECompany.VLCompany).setNotSelected(null));
        binding.add(new DropDownVLBinder(db, PositionID, EPosition.VLPosition));
        binding.add(new DropDownVLBinder(db, RankID, ERank.VLRank));
        binding.add(new DropDownVLBinder(db, EmploymentTypeID, EEmploymentType.VLEmploymentType));
        binding.add(new DropDownVLBinder(db, StaffTypeID, EStaffType.VLStaffType));

        binding.add(new DropDownVLBinder(db, PayGroupID, EPayrollGroup.VLPayrollGroup));
        binding.add(new DropDownVLBinder(db, LeavePlanID, ELeavePlan.VLLeavePlan));
        binding.add(new CheckBoxBinder(db, EmpPosIsLeavePlanResetEffectiveDate, "EmpPosIsLeavePlanResetEffectiveDate"));
        binding.add(new DropDownVLBinder(db, WorkHourPatternID, EWorkHourPattern.VLWorkHourPattern));
        binding.add(new DropDownVLBinder(db, YEBPlanID, EYEBPlan.VLYEBPlan));
        binding.add(new DropDownVLBinder(db, AuthorizationWorkFlowIDLeaveApp, EAuthorizationWorkFlow.VLAuthorizationWorkFlow));
        binding.add(new DropDownVLBinder(db, AuthorizationWorkFlowIDEmpInfoModified, EAuthorizationWorkFlow.VLAuthorizationWorkFlow));
        // Start 0000060, Miranda, 2014-07-13
        binding.add(new DropDownVLBinder(db, AuthorizationWorkFlowIDOTClaims, EAuthorizationWorkFlow.VLAuthorizationWorkFlow));
        // End 0000060, Miranda, 2014-07-13
        // Start 0000112, Miranda, 2014-12-10
        binding.add(new DropDownVLBinder(db, AuthorizationWorkFlowIDLateWaive, EAuthorizationWorkFlow.VLAuthorizationWorkFlow));
        // End 0000112, Miranda, 2014-12-10
        binding.add(new DropDownVLBinder(db, AttendancePlanID, EAttendancePlan.VLAttendancePlan));
        //binding.add(new DropDownVLBinder(db, RosterTableGroupID, ERosterTableGroup.VLRosterTableGroup));
        //binding.add(new CheckBoxBinder(db, EmpPosIsRosterTableGroupSupervisor));
        binding.add(EmpPosRemark);
        //binding.add(new DropDownVLBinder(db, EmpPosDefaultRosterCodeID, ERosterCode.VLRosterCode));
        binding.init(Request, Session);

        HROne.Common.WebUtility.WebControlsLocalization(Session, this.Controls);

    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {


            if (EmpPosID > 0)
            {
                loadObject();
            }

            if (EmpPosID <= 0)
            {
                DBFilter filter = new DBFilter();
                filter.add(new Match("EmpID", EmpID));
                if (db.count(dbConn, filter) == 0)
                {
                    EEmpPersonalInfo pi = new EEmpPersonalInfo();
                    pi.EmpID = EmpID;
                    EEmpPersonalInfo.db.select(dbConn, pi);
                    EmpPosEffFr.Value = pi.EmpDateOfJoin > pi.EmpServiceDate ? pi.EmpDateOfJoin.ToString("yyyy-MM-dd") : pi.EmpServiceDate.ToString("yyyy-MM-dd");
                    EmpPosEffFr.Enabled = false;
                }
            }
            loadHierarchy(EmpPosID);
        }

        eot_row.Visible = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ENABLE_OTCLAIM).Equals("Y", StringComparison.CurrentCultureIgnoreCase);
    }


    protected void loadHierarchy(int EmpCurPosID)
    {
        ArrayList list;

        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpPosID", EmpCurPosID));
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
        DBFilter filter = new DBFilter();
        filter.add(new Match("HLevelID", level.HLevelID));
        filter.add(new Match("CompanyID", CompanyID.SelectedValue.Equals(string.Empty) ? "0" : CompanyID.SelectedValue));
        DropDownList c = (DropDownList)e.Item.FindControl("HElementID");
        EEmpHierarchy h = (EEmpHierarchy)CurElements[level.HLevelID];
        string selected = null;
        if (h != null)
            selected = h.HElementID.ToString();
        WebFormUtils.loadValues(dbConn, c, EHierarchyElement.VLHierarchyElement, filter, null, selected, "combobox.notselected");
        c.Attributes["HLevelID"] = level.HLevelID.ToString();


    }
    protected bool loadObject()
    {
        EEmpPositionInfo obj = new EEmpPositionInfo();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);

        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpID", EmpID));
        filter.add(WebUtils.AddRankFilter(Session, "EmpID", true));
        if (EEmpPersonalInfo.db.count(dbConn, filter) == 0)
            if (EmpID <= 0)
                return false;
            else
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");

        if (!db.select(dbConn, obj))
            if (EmpPosID <= 0)
                return false;
            else
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");

        if (obj.EmpID != EmpID)
            if (EmpPosID <= 0)
                return false;
            else
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);
        EmpID = obj.EmpID;

        return true;
    }

    protected void CompanyID_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadHierarchy(EmpPosID);
    }

    public void LoadLastEmpPositionInfo(int EmpID)
    {
        EEmpPositionInfo obj = AppUtils.GetLastPositionInfo(dbConn, EmpID);
        if (obj != null)
        {
            int lastEmpPosID = obj.EmpPosID;
            obj.EmpPosID = -1;

            Hashtable values = new Hashtable();
            db.populate(obj, values);
            binding.toControl(values);
            loadHierarchy(lastEmpPosID);
        }
    }

    public bool Save()
    {
        EEmpPositionInfo c = new EEmpPositionInfo();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return false;


        db.parse(values, c);

        //DBFilter filter = new DBFilter();
        //filter.add(new Match("EmpID", CurEmpID));
        //if (db.count(dbConn, filter) == 0)
        //{
        //    EEmpPersonalInfo pi = new EEmpPersonalInfo();
        //    pi.EmpID = CurEmpID;
        //    EEmpPersonalInfo.db.select(dbConn, pi);
        //    c.EmpPosEffFr = pi.EmpServiceDate;
        //}


        if (c.EmpPosEffTo > DateTime.MinValue && c.EmpPosEffTo < c.EmpPosEffFr)
            errors.addError("EmpPosEffTo", HROne.Translation.PageErrorMessage.ERROR_DATE_TO_TOO_EARLY);

        AND andTerms = new AND();
        andTerms.add(new Match("EmpPosID", "<>", c.EmpPosID));
        andTerms.add(new Match("EmpPosEffFr", "<=", c.EmpPosEffFr));
        EEmpPositionInfo lastObj = (EEmpPositionInfo)AppUtils.GetLastObj(dbConn, db, "EmpPosEffFr", c.EmpID, andTerms);
        if (lastObj != null && (c.EmpPosEffFr <= lastObj.EmpPosEffTo || c.EmpPosEffFr == lastObj.EmpPosEffFr))
            errors.addError("EmpPosEffFr", HROne.Translation.PageErrorMessage.ERROR_DATE_FROM_OVERLAP);

        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpID", c.EmpID));
        filter.add(new Match("EmpPosID", "<>", c.EmpPosID));
        OR or = new OR();
        AND and;
        and = new AND();
        and.add(new Match("EmpPosEffFr", "<=", c.EmpPosEffFr));
        and.add(new Match("EmpPosEffTo", ">=", c.EmpPosEffFr));
        or.add(and);
        // do not allow early terms without "TO" date
        if (c.EmpPosEffTo.Ticks.Equals(0))
            or.add(new Match("EmpPosEffFr", ">", c.EmpPosEffFr));
        if (c.EmpPosEffTo > DateTime.MinValue)
        {
            and = new AND();
            and.add(new Match("EmpPosEffFr", "<=", c.EmpPosEffTo));
            and.add(new Match("EmpPosEffTo", ">=", c.EmpPosEffTo));
            or.add(and);

            and = new AND();
            and.add(new Match("EmpPosEffFr", ">=", c.EmpPosEffFr));
            and.add(new Match("EmpPosEffFr", "<=", c.EmpPosEffTo));
            or.add(and);
        }
        filter.add(or);
        if (db.count(dbConn, filter) > 0)
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_POS_OVERLAP);

        if (!errors.isEmpty())
            return false;

        WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);

        if (lastObj != null)
        {
            if (lastObj.EmpPosEffTo.Ticks.Equals(0))
            {
                if (lastObj.EmpPosEffFr <= c.EmpPosEffFr.AddDays(-1))
                {
                    lastObj.EmpPosEffTo = c.EmpPosEffFr.AddDays(-1);
                    db.update(dbConn, lastObj);
                }
                else
                {
                    if (c.EmpPosEffTo.Ticks.Equals(0))
                        errors.addError(HROne.Translation.PageErrorMessage.ERROR_POS_OVERLAP);

                }
            }
        }
        else
        {
            EEmpPositionInfo prev = AppUtils.GetLastPositionInfo(dbConn, EmpID);
            if (prev != null && prev.EmpPosID != EmpPosID)
            {
                if (prev.EmpPosEffTo.Ticks.Equals(0))

                    if (prev.EmpPosEffFr <= c.EmpPosEffFr.AddDays(-1))
                    {
                        prev.EmpPosEffTo = c.EmpPosEffFr.AddDays(-1);
                        db.update(dbConn, prev);

                    }
                    else
                        if (c.EmpPosEffTo.Ticks.Equals(0))
                            errors.addError(HROne.Translation.PageErrorMessage.ERROR_POS_OVERLAP);
            }
        }
        if (!errors.isEmpty())
        {
            WebUtils.EndFunction(dbConn);
            return false;
        }
        if (c.EmpPosID <= 0)
        {
            //            Utils.MarkCreate(Session, c);


            db.insert(dbConn, c);
            EmpPosID = c.EmpPosID;
            //            url = Utils.BuildURL(-1, CurID);
        }
        else
        {
            //            Utils.Mark(Session, c);
            db.update(dbConn, c);
        }

        ArrayList list = new ArrayList();
        foreach (RepeaterItem item in HierarchyLevel.Items)
        {
            DropDownList d = (DropDownList)item.FindControl("HElementID");
            int HLevelID = Int32.Parse(d.Attributes["HLevelID"]);

            filter = new DBFilter();
            filter.add(new Match("EmpPosID", EmpPosID));
            filter.add(new Match("HLevelID", HLevelID));
            ArrayList existingHierarchyList = EEmpHierarchy.db.select(dbConn, filter);

            EEmpHierarchy h = null;
            if (existingHierarchyList.Count > 0)
                h = (EEmpHierarchy)existingHierarchyList[0];
            else
            {
                h = new EEmpHierarchy();
                h.EmpPosID = EmpPosID;
                h.HLevelID = HLevelID;
            }
            h.EmpID = EmpID;
            if (d.SelectedIndex == 0)
                h.HElementID = 0;
            else
                h.HElementID = Int32.Parse(d.SelectedValue);
            list.Add(h);
        }
        //filter = new DBFilter();
        //filter.add(new Match("EmpID", CurEmpID));
        //filter.add(new Match("EmpPosID", CurID));
        //EEmpHierarchy.db.delete(dbConn, filter);
        foreach (EEmpHierarchy h in list)
        {
            if (h.EmpHierarchyID == 0)
                EEmpHierarchy.db.insert(dbConn, h);
            else
                EEmpHierarchy.db.update(dbConn, h);
        }

        WebUtils.EndFunction(dbConn);

        return true;
    }

    public bool Delete()
    {
        EEmpPositionInfo c = new EEmpPositionInfo();
        c.EmpPosID = EmpPosID;
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
        return true;
    }

}
