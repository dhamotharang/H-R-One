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
using HROne.Translation;
using HROne.Lib.Entities;
public partial class LeaveCode_View : HROneWebPage
{
    private const string FUNCTION_CODE = "LEV002";
    public Binding binding;
    public DBManager db = ELeaveCode .db;
    public ELeaveCode obj;
    public int CurID = -1;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        

        binding = new Binding(dbConn, db);
        binding.add(LeaveCodeID);
        binding.add(new LabelVLBinder(db, LeaveTypeID, ELeaveType.VLLeaveType));
        binding.add(LeaveCode); 
        binding.add(LeaveCodeDesc);
        binding.add(LeaveCodePayRatio);
        binding.add(new BlankZeroLabelVLBinder(db, LeaveCodeLeaveAllowPaymentCodeID, EPaymentCode.VLPaymentCode));
        binding.add(new BlankZeroLabelVLBinder(db, LeaveCodeLeaveDeductPaymentCodeID, EPaymentCode.VLPaymentCode));
        binding.add(new LabelVLBinder(db, LeaveCodeLeaveDeductFormula, EPayrollProrataFormula.VLPayrollProrataFormula));
        binding.add(new LabelVLBinder(db, LeaveCodeLeaveAllowFormula, EPayrollProrataFormula.VLPayrollProrataFormula));
        binding.add(new LabelVLBinder(db, LeaveCodePayAdvance, Values.VLTrueFalseYesNo));
        binding.add(new LabelVLBinder(db, LeaveCodeIsPayrollProcessNextMonth, Values.VLTrueFalseYesNo));
        binding.add(new LabelVLBinder(db, LeaveCodeIsShowMedicalCertOption, Values.VLTrueFalseYesNo));
        binding.add(new LabelVLBinder(db, LeaveCodeHideInESS, Values.VLTrueFalseYesNo));
        binding.add(new LabelVLBinder(db, LeaveCodeIsSkipPayrollProcess, Values.VLTrueFalseYesNo));
        binding.add(new CheckBoxBinder(db, LeaveCodeUseAllowancePaymentCodeIfSameAmount));
        binding.add(new LabelVLBinder(db, LeaveCodeIsCNDProrata, Values.VLTrueFalseYesNo));
        binding.add(LeaveAppUnit);
        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["LeaveCodeID"], out CurID))
            CurID = -1;

        HROne.ProductLicense productLicense = WebUtils.productLicense(Session);
        if (productLicense.IsESS)
            ESSRow.Visible = true;
        else
            ESSRow.Visible = false;
        if (productLicense.ProductType != HROne.ProductLicense.ProductLicenseType.HROne)
        {
            LeaveCodeIsCNDProrataRow.Visible = false;
        }
        else
        {
            LeaveCodeIsCNDProrataRow.Visible = true;
        }

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

		if(!Page.IsPostBack) 
		{
            if (CurID > 0)
            {
                loadObject();
            }
            else
                toolBar.DeleteButton_Visible = false;
        }
    }

    protected bool loadObject() 
    {
	    obj=new ELeaveCode ();
	    bool isNew=WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
	    if(!db.select(dbConn, obj))
		    return false;

	    Hashtable values=new Hashtable();
	    db.populate(obj,values);
	    binding.toControl(values);

        PayrollProcessPanel.Visible = !obj.LeaveCodeIsSkipPayrollProcess;

        return true;
    }


    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ELeaveCode obj = new ELeaveCode();
        obj.LeaveCodeID = CurID;
        db.select(dbConn, obj);
        DBFilter leaveApplicationFilter = new DBFilter();
        leaveApplicationFilter.add(new Match("LeaveCodeID", obj.LeaveCodeID));
        leaveApplicationFilter.add("empid", true);
        ArrayList leaveApplicationList = ELeaveApplication.db.select(dbConn, leaveApplicationFilter);
        if (leaveApplicationList.Count > 0)
        {
            int curEmpID = 0;
            errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_USED_BY_EMPLOYEE, new string[] { HROne.Common.WebUtility.GetLocalizedString("Leave Code"), obj.LeaveCode }));
            foreach (ELeaveApplication leaveApplication in leaveApplicationList)
            {
                EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                empInfo.EmpID = leaveApplication.EmpID;
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                {
                    if (curEmpID != leaveApplication.EmpID)
                    {
                        errors.addError("- " + empInfo.EmpNo + ", " + empInfo.EmpEngFullName);
                        curEmpID = leaveApplication.EmpID;
                    }
                }
                else
                    ELeaveApplication.db.delete(dbConn, leaveApplication);

            }
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
            return;
        }
        else
        {

            WebUtils.StartFunction(Session, FUNCTION_CODE);
            db.delete(dbConn, obj);
            WebUtils.EndFunction(dbConn);
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "LeaveCode_List.aspx");
    }

    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "LeaveCode_Edit.aspx?LeaveCodeID=" + CurID);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "LeaveCode_List.aspx");
    }
}
