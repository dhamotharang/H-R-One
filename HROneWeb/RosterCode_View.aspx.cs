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

public partial class RosterCode_View : HROneWebPage
{
    private const string FUNCTION_CODE = "ATT001";
    
    public Binding binding;
    public DBManager db = ERosterCode.db;
    public ERosterCode obj;





    public int CurID = -1;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        

        binding = new Binding(dbConn, db);
        binding.add(RosterCodeID);
        binding.add(RosterCode);
        binding.add(RosterCodeDesc);
        binding.add(new LabelVLBinder(db, RosterCodeType, ERosterCode.VLRosterType));
        binding.add(new BlankZeroLabelVLBinder(db, RosterClientID, ERosterClient.VLRosterClient));
        binding.add(new BlankZeroLabelVLBinder(db, LeaveCodeID, ELeaveCode.VLLeaveCode));
        binding.add(new BlankZeroLabelVLBinder(db, RosterClientSiteID, ERosterClientSite.VLRosterClientSite));
        binding.add(new BlankZeroLabelVLBinder(db, CostCenterID, ECostCenter.VLCostCenter).setTextDisplayForZero(HROne.Common.WebUtility.GetLocalizedString(ECostCenter.DEFAULT_COST_CENTER_TEXT)));
        binding.add(new LabelVLBinder(db, RosterCodeCountWorkHourOnly, Values.VLTrueFalseYesNo));
        binding.add(RosterCodeInTime);
        binding.add(RosterCodeOutTime);
        binding.add(RosterCodeGraceInTime);
        binding.add(RosterCodeGraceOutTime);
        binding.add(new LabelVLBinder(db, RosterCodeHasLunch, Values.VLTrueFalseYesNo));
        binding.add(RosterCodeLunchStartTime);
        binding.add(RosterCodeLunchEndTime);
        binding.add(RosterCodeLunchDeductMinimumWorkHour);
        binding.add(RosterCodeLunchDurationHour);
        binding.add(new LabelVLBinder(db, RosterCodeLunchIsDeductWorkingHour, Values.VLTrueFalseYesNo));
        binding.add(RosterCodeLunchDeductWorkingHourMinsUnit);
        binding.add(new LabelVLBinder(db, RosterCodeLunchDeductWorkingHourMinsRoundingRule, Values.VLRoundingRule));

        binding.add(new LabelVLBinder(db, RosterCodeHasOT, Values.VLTrueFalseYesNo));
        binding.add(RosterCodeOTStartTime);
        binding.add(RosterCodeOTEndTime);
        binding.add(RosterCodeCountOTAfterWorkHourMin);
        binding.add(new LabelVLBinder(db, RosterCodeIsOTStartFromOutTime, Values.VLTrueFalseYesNo));
        binding.add(new LabelVLBinder(db, RosterCodeOTIncludeLunch, Values.VLTrueFalseYesNo));


        binding.add(RosterCodeCutOffTime);
        binding.add(RosterCodeOTMinsUnit);
        binding.add(new LabelVLBinder(db, RosterCodeOTMinsRoundingRule, Values.VLRoundingRule));
        binding.add(RosterCodeWorkingDayUnit);
        binding.add(RosterCodeDailyWorkingHour);
        binding.add(new CheckBoxBinder(db, RosterCodeUseHalfWorkingDaysHours));
        binding.add(RosterCodeUseHalfWorkingDaysHoursMaxWorkingHours);

        binding.add(new LabelVLBinder(db, RosterCodeIsOverrideHourlyPayment, Values.VLTrueFalseYesNo));
        binding.add(RosterCodeOverrideHoulyAmount);
        binding.add(RosterCodeColorCode);

        binding.add(new LabelVLBinder(db, RosterCodeOTShiftStartTimeForLate, Values.VLTrueFalseYesNo));

        binding.add(RosterCodeLateMinsUnit);
        binding.add(new LabelVLBinder(db, RosterCodeLateMinsRoundingRule, Values.VLRoundingRule));
        binding.add(RosterCodeEarlyLeaveMinsUnit);
        binding.add(new LabelVLBinder(db, RosterCodeEarlyLeaveMinsRoundingRule, Values.VLRoundingRule));
        binding.add(RosterCodeLunchLateMinsUnit);
        binding.add(new LabelVLBinder(db, RosterCodeLunchLateMinsRoundingRule, Values.VLRoundingRule));
        binding.add(RosterCodeLunchEarlyLeaveMinsUnit);
        binding.add(new LabelVLBinder(db, RosterCodeLunchEarlyLeaveMinsRoundingRule, Values.VLRoundingRule));



        binding.init(Request, Session);


        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (!int.TryParse(DecryptedRequest["RosterCodeID"], out CurID))
            CurID = -1;
        RosterCode_OTRatioList1.CurrentRosterCodeID = CurID;
        RosterCode_AdditionalPayment1.CurrentRosterCodeID = CurID;

        CostCenterRow.Visible = WebUtils.productLicense(Session).IsCostCenter;
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            if (CurID > 0)
            {
                loadObject();

                if (!string.IsNullOrEmpty(RosterCodeColorCode.Text))
                {
                    try
                    {
                        RosterCodeColorCode.BackColor = System.Drawing.ColorTranslator.FromHtml(RosterCodeColorCode.Text);
                        RosterCodeColorCode.ForeColor = AppUtils.ComputeTextColor(RosterCodeColorCode.BackColor);
                        //double brightness = RosterCodeColorCode.BackColor.GetBrightness();
                        //double hue = RosterCodeColorCode.BackColor.GetHue();
                        //double saturation = RosterCodeColorCode.BackColor.GetSaturation();
                        //if (brightness * saturation < 0.5)
                        //    RosterCodeColorCode.ForeColor = System.Drawing.Color.White;
                        //else
                        //    RosterCodeColorCode.ForeColor = System.Drawing.Color.Black;
                    }
                    catch
                    {
                    }
                }
            }
            else
                toolBar.DeleteButton_Visible = false;
        }
    }



    //by Ben
    protected bool loadObject() 
    {
	    obj=new ERosterCode();
	    bool isNew=WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
	    if(!db.select(dbConn, obj))
		    return false;

        if (obj.RosterCodeHasLunch)
            RosterCodeLunchPanel.Visible = true;
        else
            RosterCodeLunchPanel.Visible = false;

        if (obj.RosterCodeHasOT)
            RosterCodeOTPanel.Visible = true;
        else
            RosterCodeOTPanel.Visible = false;

        if (obj.RosterCodeCountWorkHourOnly)
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

        if (obj.RosterCodeIsOverrideHourlyPayment)
            OverrideHourlyPaymentPanel.Visible = true;
        else
            OverrideHourlyPaymentPanel.Visible = false;

        if (obj.RosterCodeType.Equals("L"))
            LeaveCodeSettingPanel.Visible = true;
        else
            LeaveCodeSettingPanel.Visible = false;
        
        Hashtable values = new Hashtable();
	    db.populate(obj,values);
	    binding.toControl(values);


        return true;
    }

    protected void Delete_ClickTop(object sender, EventArgs e)
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
    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "RosterCode_Edit.aspx?RosterCodeID=" + CurID);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "RosterCode_List.aspx");
    }
}
