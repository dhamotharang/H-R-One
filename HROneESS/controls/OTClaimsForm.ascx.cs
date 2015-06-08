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
using HROne.Lib.Entities;
using System.Net.Mail;

public partial class OTClaimsForm : HROneWebControl 
{
    public Binding binding;
    public SearchBinding sbinding;
    public DBManager db = ERequestOTClaim.db;

    public ERequestOTClaim obj;
    public int CurID = -1;
    public int CurEmpID = -1;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session))
            return;
        binding = new Binding(dbConn, db);
        binding.add(EmpID);

        binding.add(new TextBoxBinder(db, RequestOTClaimPeriodFrom.TextBox, RequestOTClaimPeriodFrom.ID));
        binding.add(new TextBoxBinder(db, RequestOTClaimPeriodTo.TextBox, RequestOTClaimPeriodTo.ID));
        binding.add(RequestOTClaimHourFrom);
        binding.add(RequestOTClaimHourTo);
        binding.add(RequestOTHours);
        binding.add(RequestOTClaimRemark);
        binding.init(Request, Session);

        HROne.Common.WebUtility.WebControlsLocalization(Session, this.Controls);

        EESSUser user = WebUtils.GetCurUser(Session);
        if (user != null)
        {
            CurEmpID = user.EmpID;
            EmpID.Value = CurEmpID.ToString();
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (CurID > 0)
            {
                loadObject();
            }
        }
    }
    protected bool loadObject()
    {
        obj = new ERequestOTClaim();
        bool isNew = WebFormWorkers.loadKeys(db, obj, Request);
        if (!db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);

        return true;
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        ERequestOTClaim c = new ERequestOTClaim();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);

        if (c.RequestOTClaimHourTo < c.RequestOTClaimHourFrom)
            errors.addError("RequestOTClaimTimeFrom", "Invald hours");
        if (c.RequestOTClaimPeriodTo < c.RequestOTClaimPeriodFrom)
                errors.addError("RequestOTClaimPeriodFrom", "Date To cannot be earlier than Date From");

        if (c.RequestOTHours <= 0)
            errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_FIELD_REQUIRED, new string[] { lblOTHours.Text }));

        if (!errors.isEmpty())
            return;

        // Start Ricky So, 2014-09-05, Avoid checking time overlap for CL Requisition
        //ArrayList overlapOTClaimList = new ArrayList();
        //if (c.IsOverlapOTClaim(dbConn, out overlapOTClaimList))
        //{
        //    string strHourlyOverlapMessage = string.Empty;

        //    foreach (BaseObject overlapOTClaim in overlapOTClaimList)
        //    {
        //        if (overlapOTClaim is EOTClaim)
        //        {
        //            EOTClaim previousOTClaim = (EOTClaim)overlapOTClaim;
        //            if (string.IsNullOrEmpty(strHourlyOverlapMessage))
        //                strHourlyOverlapMessage = "Leave time cannot overlap with previous CL Requisition";
        //        }
        //        else if (overlapOTClaim is ERequestOTClaim)
        //        {
        //            ERequestOTClaim previousRequestOTClaim = (ERequestOTClaim)overlapOTClaim;
        //            if (string.IsNullOrEmpty(strHourlyOverlapMessage))
        //                strHourlyOverlapMessage = "Leave time cannot overlap with previous CL Requisition";

        //        }
        //    }

        //    if (!string.IsNullOrEmpty(strHourlyOverlapMessage))
        //        errors.addError(strHourlyOverlapMessage);
        //}
        // End Ricky So, 2014-09-05

        if (!errors.isEmpty())
            return;

        try
        {
            ESSAuthorizationProcess authorization = new ESSAuthorizationProcess(dbConn);
            authorization.SubmitOTClaim(c);
        }
        catch (Exception ex)
        {
            errors.addError(ex.Message);

        }

        if (!errors.isEmpty())
            return;
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/ESS_EmpRequestStatus.aspx");


    }
 
    protected void OTClaimDateFrom_Changed(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(RequestOTClaimPeriodFrom.Value))
        {
            if (string.IsNullOrEmpty(RequestOTClaimPeriodTo.Value))
            {
                RequestOTClaimPeriodTo.Value = RequestOTClaimPeriodFrom.Value;
                OTClaimDateTo_Changed(sender, e);
            }
            else
            {
                DateTime dtFrom, dtTo;
                if (DateTime.TryParse(RequestOTClaimPeriodFrom.Value, out dtFrom) && DateTime.TryParse(RequestOTClaimPeriodTo.Value, out dtTo))
                {
                    if (dtFrom > dtTo)
                        RequestOTClaimPeriodTo.Value = RequestOTClaimPeriodFrom.Value;
                    OTClaimDateTo_Changed(sender, e);
                }
            }
        }
    }

    protected void OTClaimDateTo_Changed(object sender, EventArgs e)
    {
    }

    protected void OTClaimTime_TextChanged(object sender, EventArgs e)
    {
            DateTime dtTimeFrom = new DateTime();
            if (!DateTime.TryParseExact(RequestOTClaimHourFrom.Text, "HH:mm", null, System.Globalization.DateTimeStyles.None, out dtTimeFrom))
                RequestOTClaimHourFrom.Text = string.Empty;

            DateTime dtTimeTo = new DateTime();
            if (!DateTime.TryParseExact(RequestOTClaimHourTo.Text, "HH:mm", null, System.Globalization.DateTimeStyles.None, out dtTimeTo))
                RequestOTClaimHourTo.Text = string.Empty;

            if (dtTimeFrom.Ticks.Equals(0) || dtTimeTo.Ticks.Equals(0))
                return;

            DateTime tmpOTClaimDateFrom;
            if (DateTime.TryParse(RequestOTClaimPeriodFrom.Value, out tmpOTClaimDateFrom))
            {
                double workhour = 0;
                EEmpPositionInfo currentEmpPos = AppUtils.GetLastPositionInfo(dbConn, tmpOTClaimDateFrom, CurEmpID);
                if (currentEmpPos != null)
                {
                    EWorkHourPattern workPattern = new EWorkHourPattern();
                    workPattern.WorkHourPatternID = currentEmpPos.WorkHourPatternID;
                    if (EWorkHourPattern.db.select(dbConn, workPattern))
                        workhour = workPattern.GetDefaultWorkHour(dbConn, tmpOTClaimDateFrom);
                }
                if (workhour > 0)
                {
                    double timeDiff = ((TimeSpan)dtTimeTo.Subtract(dtTimeFrom)).TotalHours;
                    if (timeDiff < 0) timeDiff += 1;
                    RequestOTHours.Text = timeDiff.ToString("0.####");
                }
            }
    }
}
