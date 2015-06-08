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

public partial class Attendance_RosterClient_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "ATT012";
    public Binding binding;
    public DBManager db = ERosterClient.db;
    public ERosterClient obj;
    public int CurID = -1;
    

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        

        binding = new Binding(dbConn, db);
        binding.add(RosterClientID);
        binding.add(RosterClientCode);
        binding.add(RosterClientName);
        binding.add(new DropDownVLBinder(db, RosterClientMappingSiteCodeToHLevelID, EHierarchyLevel.VLHierarchy));
        binding.add(new DropDownVLBinder(db, CostCenterID, ECostCenter.VLCostCenter).setNotSelected(HROne.Common.WebUtility.GetLocalizedString(ECostCenter.DEFAULT_COST_CENTER_TEXT)));

        binding.init(Request, Session);

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);


        if (!int.TryParse(DecryptedRequest["RosterClientID"], out CurID))
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
	    obj=new ERosterClient();
	    bool isNew=WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
	    if(!db.select(dbConn, obj))
		    return false;

	    Hashtable values=new Hashtable();
	    db.populate(obj,values);
	    binding.toControl(values);
        return true;
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        ERosterClient c = new ERosterClient();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);
        if (!AppUtils.checkDuplicate(dbConn, db, c, errors, "RosterClientCode"))
            return;

        WebUtils.StartFunction(Session, FUNCTION_CODE);
        if (CurID < 0)
        {
//            Utils.MarkCreate(Session, c);

            db.insert(dbConn, c);
            CurID = c.RosterClientID;
//            url = Utils.BuildURL(-1, CurID);
        }
        else
        {
//            Utils.Mark(Session, c);
            db.update(dbConn, c);
        }
        WebUtils.EndFunction(dbConn);


        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Attendance_RosterClient_View.aspx?RosterClientID="+CurID);


    }
    protected void Delete_Click(object sender, EventArgs e)
    {

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ERosterClient o = new ERosterClient();
        o.RosterClientID = CurID;
        db.select(dbConn, o);
        DBFilter rosterCodeFilter = new DBFilter();
        rosterCodeFilter.add(new Match("RosterClientID", o.RosterClientID));
        ArrayList rosterClientSiteList = ERosterCode.db.select(dbConn, rosterCodeFilter);
        if (rosterClientSiteList.Count > 0)
        {
            errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_IS_IN_USE, new string[] { HROne.Common.WebUtility.GetLocalizedString("Roster Client Code"), o.RosterClientCode }));
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
            return;
        }
        else
        {
            DBFilter obj = new DBFilter();
            obj.add(new Match("RosterClientID", o.RosterClientID));
            ArrayList objList = ERosterClientSite.db.select(dbConn, obj);
            foreach (ERosterClientSite match in objList)
                ERosterClientSite.db.delete(dbConn, match);

            WebUtils.StartFunction(Session, FUNCTION_CODE);
            db.delete(dbConn, o);
            WebUtils.EndFunction(dbConn);
        }

        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Attendance_RosterClient_List.aspx");
    }
    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Attendance_RosterClient_View.aspx?RosterClientID=" + CurID);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Attendance_RosterClient_List.aspx");

    }
}
