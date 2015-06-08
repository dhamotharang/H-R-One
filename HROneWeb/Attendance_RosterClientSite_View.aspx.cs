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
using HROne.Translation;

public partial class Attendance_RosterClientSite_View : HROneWebPage
{
    private const string FUNCTION_CODE = "ATT012";
    
    public Binding binding;
    public DBManager db = ERosterClientSite.db;
    public ERosterClientSite obj;

    public int CurRosterClientID = -1;
    public int CurID = -1;

    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        

        binding = new Binding(dbConn, db);
        binding.add(RosterClientID);
        binding.add(RosterClientSiteID);
        binding.add(RosterClientSiteCode);
        binding.add(RosterClientSitePropertyName);
        binding.add(RosterClientSiteLocation);
        binding.add(RosterClientSitePremisesNature);
        binding.add(RosterClientSiteInCharge);
        binding.add(RosterClientSiteInChargeContactNo);
        binding.add(RosterClientSiteServiceHours);
        binding.add(RosterClientSiteShift);
        binding.add(new BlankZeroLabelVLBinder(db, CostCenterID, ECostCenter.VLCostCenter).setTextDisplayForZero(HROne.Common.WebUtility.GetLocalizedString(ECostCenter.DEFAULT_COST_CENTER_TEXT)));

        binding.init(Request, Session);

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (!int.TryParse(DecryptedRequest["RosterClientID"], out CurRosterClientID))
            CurRosterClientID = -1;

        if (!int.TryParse(DecryptedRequest["RosterClientSiteID"], out CurID))
            CurID = -1;

		if(!Page.IsPostBack) 
		{


            if (CurID > 0)
            {
                loadObject();
            }
            else
                toolBar.DeleteButton_Visible = false;
        }

        CostCenterRow.Visible = WebUtils.productLicense(Session).IsCostCenter;
    }

    protected bool loadObject()
    {
        obj = new ERosterClientSite();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
        if (!db.select(dbConn, obj))
            return false;

        if (CurRosterClientID <= 0)
            CurRosterClientID = obj.RosterClientID;


        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);
        return true;
    }





    protected void Delete_ClickTop(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ERosterClientSite o = new ERosterClientSite();
        o.RosterClientSiteID = CurID;
        db.select(dbConn, o);
        DBFilter rosterCodeFilter = new DBFilter();
        rosterCodeFilter.add(new Match("RosterClientSiteID", o.RosterClientSiteID));
        ArrayList RosterClientSiteSiteList = ERosterCode.db.select(dbConn, rosterCodeFilter);
        if (RosterClientSiteSiteList.Count > 0)
        {
            errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_IS_IN_USE, new string[] { HROne.Common.WebUtility.GetLocalizedString("Roster Client Code"), o.RosterClientSiteCode }));
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
            return;
        }
        else
        {

            WebUtils.StartFunction(Session, FUNCTION_CODE);
            db.delete(dbConn, o);
            WebUtils.EndFunction(dbConn);
        }

        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Attendance_RosterClient_View.aspx?RosterClientID=" + RosterClientID.Value);

    }
    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Attendance_RosterClientSite_Edit.aspx?RosterClientSiteID=" + CurID);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Attendance_RosterClient_View.aspx?RosterClientID=" + RosterClientID.Value);
    }
}
