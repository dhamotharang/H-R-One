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

public partial class RosterCode_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "ATT001";
    public Binding binding;
    public DBManager db = ERosterCode.db;
    public ERosterCode obj;
    public int CurID = -1;
    
    
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;


        DBFilter LeaveCodeFilter = new DBFilter();
        {
            DBFilter LeaveTypeFilter = new DBFilter();
            LeaveTypeFilter.add(new Match("lt.LeaveTypeIsDisabled", false));
            LeaveCodeFilter.add(new IN("LeaveTypeID", "SELECT lt.LeaveTypeID FROM " + ELeaveType.db.dbclass.tableName + " lt ", LeaveTypeFilter));
        }

        binding = new Binding(dbConn, db);
        binding.add(RosterCodeID);
        binding.add(RosterCode);
        binding.add(RosterCodeDesc);
        binding.add(new DropDownVLBinder(db, RosterCodeType, ERosterCode.VLRosterType));
        binding.add(new DropDownVLBinder(db, LeaveCodeID, ELeaveCode.VLLeaveCode, LeaveCodeFilter));
        binding.add(new DropDownVLBinder(db, RosterClientID, ERosterClient.VLRosterClient));
        if (IsPostBack)
        {
            DBFilter rosterClientSiteFilter = new DBFilter();
            rosterClientSiteFilter.add(new Match("RosterClientID", RosterClientID.SelectedValue));
            binding.add(new DropDownVLBinder(db, RosterClientSiteID, ERosterClientSite.VLRosterClientSite, rosterClientSiteFilter));
        }
        binding.add(new DropDownVLBinder(db, CostCenterID, ECostCenter.VLCostCenter).setNotSelected(HROne.Common.WebUtility.GetLocalizedString(ECostCenter.DEFAULT_COST_CENTER_TEXT)));
        binding.add(new CheckBoxBinder(db, RosterCodeCountWorkHourOnly));
        binding.add(RosterCodeInTime);
        binding.add(RosterCodeOutTime);
        binding.add(RosterCodeGraceInTime);
        binding.add(RosterCodeGraceOutTime);
        binding.add(new CheckBoxBinder(db, RosterCodeHasLunch));
        binding.add(RosterCodeLunchStartTime);
        binding.add(RosterCodeLunchEndTime);
        binding.add(RosterCodeLunchDeductMinimumWorkHour);
        binding.add(RosterCodeLunchDurationHour);
        binding.add(new CheckBoxBinder(db, RosterCodeLunchIsDeductWorkingHour));
        binding.add(RosterCodeLunchDeductWorkingHourMinsUnit);
        binding.add(new DropDownVLBinder(db, RosterCodeLunchDeductWorkingHourMinsRoundingRule, Values.VLRoundingRule));

        binding.add(new CheckBoxBinder(db, RosterCodeHasOT));
        binding.add(RosterCodeOTStartTime);
        binding.add(RosterCodeOTEndTime);
        binding.add(RosterCodeCountOTAfterWorkHourMin);
        binding.add(new CheckBoxBinder(db, RosterCodeIsOTStartFromOutTime));
        binding.add(new CheckBoxBinder(db, RosterCodeOTIncludeLunch));
        binding.add(RosterCodeOTMinsUnit);
        binding.add(new DropDownVLBinder(db, RosterCodeOTMinsRoundingRule, Values.VLRoundingRule));

        binding.add(new CheckBoxBinder(db, RosterCodeOTShiftStartTimeForLate));

        binding.add(RosterCodeLateMinsUnit);
        binding.add(new DropDownVLBinder(db, RosterCodeLateMinsRoundingRule, Values.VLRoundingRule));
        binding.add(RosterCodeEarlyLeaveMinsUnit);
        binding.add(new DropDownVLBinder(db, RosterCodeEarlyLeaveMinsRoundingRule, Values.VLRoundingRule));
        binding.add(RosterCodeLunchLateMinsUnit);
        binding.add(new DropDownVLBinder(db, RosterCodeLunchLateMinsRoundingRule, Values.VLRoundingRule));
        binding.add(RosterCodeLunchEarlyLeaveMinsUnit);
        binding.add(new DropDownVLBinder(db, RosterCodeLunchEarlyLeaveMinsRoundingRule, Values.VLRoundingRule));

        binding.add(RosterCodeCutOffTime);
        binding.add(RosterCodeWorkingDayUnit);
        binding.add(RosterCodeDailyWorkingHour);
        binding.add(new CheckBoxBinder(db, RosterCodeUseHalfWorkingDaysHours));
        binding.add(RosterCodeUseHalfWorkingDaysHoursMaxWorkingHours);
        binding.add(new CheckBoxBinder(db, RosterCodeIsOverrideHourlyPayment));
        binding.add(RosterCodeOverrideHoulyAmount);
        binding.add(RosterCodeColorCode);
        
        binding.init(Request, Session);

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (!int.TryParse(DecryptedRequest["RosterCodeID"], out CurID))
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

        OverrideHourlyPaymentPanel.Visible = RosterCodeIsOverrideHourlyPayment.Checked;

        ScriptManager.RegisterStartupScript(this, this.GetType(), "ColorPicker", "ProColor.prototype.attachButton('" + RosterCodeColorCode.ClientID + "', { imgPath:'colorpicker/img/procolor_win_', showInField: true });", true);

        CostCenterRow.Visible = WebUtils.productLicense(Session).IsCostCenter;
    }
    protected bool loadObject() 
    {
	    obj=new ERosterCode();
	    bool isNew=WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
	    if(!db.select(dbConn, obj))
		    return false;

        if (!IsPostBack)
        {
            DBFilter rosterClientSiteFilter = new DBFilter();
            rosterClientSiteFilter.add(new Match("RosterClientID", obj.RosterClientID));
            binding.add(new DropDownVLBinder(db, RosterClientSiteID, ERosterClientSite.VLRosterClientSite, rosterClientSiteFilter));
            binding.init(Request, Session);
            HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
        }

	    Hashtable values=new Hashtable();
	    db.populate(obj,values);
	    binding.toControl(values);

        return true;
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (RosterCodeType.SelectedValue.Equals("L"))
            LeaveCodeSettingPanel.Visible = true;
        else
            LeaveCodeSettingPanel.Visible = false;

        if (RosterCodeHasLunch.Checked)
            RosterCodeLunchPanel.Visible = true;
        else
            RosterCodeLunchPanel.Visible = false;

        if (RosterCodeHasOT.Checked)
            RosterCodeOTPanel.Visible = true;
        else
            RosterCodeOTPanel.Visible = false;

        if (RosterCodeCountWorkHourOnly.Checked)
        {
            RosterCodeNormalWorkInOut.Visible = false;
            RosterCodeNormalWorkInOutOT.Visible = false;
            RosterCodeCountWorkHourOnlyOT.Visible = true;
            RosterCodeCountWorkHourOnlyLunchPanel.Visible = true;
            RosterCodeCountWorkHourOnlyHalfUnitPanel.Visible = true;
        }
        else
        {
            RosterCodeNormalWorkInOut.Visible = true;
            RosterCodeNormalWorkInOutOT.Visible = true;
            RosterCodeCountWorkHourOnlyOT.Visible = false;
            RosterCodeCountWorkHourOnlyLunchPanel.Visible = false;
            RosterCodeCountWorkHourOnlyHalfUnitPanel.Visible = false;
        }
    }
    protected void Save_Click(object sender, EventArgs e)
    {
        ERosterCode c = new ERosterCode();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        if (!RosterCodeNormalWorkInOut.Visible || RosterCodeType.SelectedValue.Equals(ERosterCode.ROSTERTYPE_CODE_LEAVE) 
            || RosterCodeType.SelectedValue.Equals(ERosterCode.ROSTERTYPE_CODE_RESTDAY) 
            || RosterCodeType.SelectedValue.Equals(ERosterCode.ROSTERTYPE_CODE_STATUTORYHOLIDAY)
            || RosterCodeType.SelectedValue.Equals(ERosterCode.ROSTERTYPE_CODE_PUBLICHOLIDAY)
            )
        {
            values["RosterCodeInTime"] = "00:00";
            values["RosterCodeOutTime"] = "00:00";

        }
        if (!RosterCodeOTPanel.Visible)
        {
            values["RosterCodeOTMinsUnit"] = "0";
            values["RosterCodeOTMinsRoundingRule"] = "-";
        }
        if (!RosterCodeLunchIsDeductWorkingHour.Checked)
        {
            values["RosterCodeLunchDeductWorkingHourMinsUnit"] = "0";
            values["RosterCodeLunchDeductWorkingHourMinsRoundingRule"] = "-";
        }
        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);
        if (!AppUtils.checkDuplicate(dbConn, db, c, errors, "RosterCode"))
            return;

        WebUtils.StartFunction(Session, FUNCTION_CODE);
        if (CurID < 0)
        {
//            Utils.MarkCreate(Session, c);

            db.insert(dbConn, c);
            CurID = c.RosterCodeID;
//            url = Utils.BuildURL(-1, CurID);
        }
        else
        {
//            Utils.Mark(Session, c);
            db.update(dbConn, c);
        }
        WebUtils.EndFunction(dbConn);


        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "RosterCode_View.aspx?RosterCodeID="+CurID);


    }
    protected void Delete_Click(object sender, EventArgs e)
    {

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ERosterCode o = new ERosterCode();
        o.RosterCodeID = CurID;
        db.select(dbConn, o);
        DBFilter rosterTableFilter = new DBFilter();
        rosterTableFilter.add(new Match("RosterCodeID", o.RosterCodeID));
        rosterTableFilter.add("empid", true);
        ArrayList empRosterTableList = ERosterTable.db.select(dbConn, rosterTableFilter);
        if (empRosterTableList.Count > 0)
        {
            int curEmpID = 0;
            errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_USED_BY_EMPLOYEE, new string[] { HROne.Common.WebUtility.GetLocalizedString("AVC Plan Code"), o.RosterCode }));
            foreach (ERosterTable empRosterTable in empRosterTableList)
            {
                EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                empInfo.EmpID = empRosterTable.EmpID;
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                    if (curEmpID != empRosterTable.EmpID)
                    {
                        errors.addError("- " + empInfo.EmpNo + ", " + empInfo.EmpEngFullName);
                        curEmpID = empRosterTable.EmpID;
                    }
                    else
                        ERosterTable.db.delete(dbConn, empRosterTable);

            }
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
            return;
        }
        else
        {
            WebUtils.StartFunction(Session, FUNCTION_CODE);
            db.delete(dbConn, o);
            WebUtils.EndFunction(dbConn);

            DBFilter obj = new DBFilter();
            obj.add(new Match("RosterCodeID", CurID));
            ArrayList objList = ERosterCodeDetail.db.select(dbConn, obj);
            foreach (ERosterCodeDetail match in objList)
                ERosterCodeDetail.db.delete(dbConn, match);
        }

        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "RosterCode_List.aspx");
    }
    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "RosterCode_View.aspx?RosterCodeID=" + CurID);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "RosterCode_List.aspx");

    }
    protected void RosterCodeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (RosterCodeType.SelectedValue.Equals("L"))
        //    LeaveCodeSettingPanel.Visible = true;
        //else
        //    LeaveCodeSettingPanel.Visible = false;

    }
}
