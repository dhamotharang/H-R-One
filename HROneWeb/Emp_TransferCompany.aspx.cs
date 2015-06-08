using System;
using System.Data;
using System.Globalization;
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

public partial class Emp_TransferCompany : HROneWebPage
{
    private const string FUNCTION_CODE = "PER011";

    public Binding empPosBinding;
    public DBManager empPosDB = EEmpPositionInfo.db;
    public Binding empBinding;
    public DBManager empDB = EEmpPersonalInfo.db;
    public int CurID = -1;
    public int CurEmpID = -1;
    public Hashtable CurElements = new Hashtable();
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;

        
        empPosBinding = new Binding(dbConn, empPosDB);
        empPosBinding.add(EmpPosID);
        empPosBinding.add(EmpID);
        empPosBinding.add(new DropDownVLBinder(empPosDB, CompanyID, ECompany.VLCompany).setNotSelected(null));
        empPosBinding.add(new DropDownVLBinder(empPosDB, PositionID, EPosition.VLPosition));
        empPosBinding.add(new DropDownVLBinder(empPosDB, RankID, ERank.VLRank));
        empPosBinding.add(new DropDownVLBinder(empPosDB, StaffTypeID, EStaffType.VLStaffType));
        empPosBinding.add(new DropDownVLBinder(empPosDB, PayGroupID, EPayrollGroup.VLPayrollGroup));
        empPosBinding.add(new DropDownVLBinder(empPosDB, LeavePlanID, ELeavePlan.VLLeavePlan));

        empPosBinding.add(new LabelVLBinder(empPosDB, OldCompanyID, "CompanyID", ECompany.VLCompany));
        empPosBinding.add(new LabelVLBinder(empPosDB, OldPositionID, "PositionID", EPosition.VLPosition));
        empPosBinding.add(new LabelVLBinder(empPosDB, OldRankID, "RankID", ERank.VLRank));
        empPosBinding.add(new LabelVLBinder(empPosDB, OldStaffTypeID, "StaffTypeID", EStaffType.VLStaffType));
        empPosBinding.add(new LabelVLBinder(empPosDB, OldPayGroupID, "PayGroupID", EPayrollGroup.VLPayrollGroup));
        empPosBinding.add(new LabelVLBinder(empPosDB, OldLeavePlanID, "LeavePlanID", ELeavePlan.VLLeavePlan));

        empPosBinding.init(Request, Session);

        empBinding = new Binding(dbConn, empDB);
        empBinding.add(EmpID);
        //empBinding.add(new LabelBinder(empDB, OldEmpNo, "EmpNo"));
        empBinding.add(EmpNo);
        empBinding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["EmpID"], out CurEmpID))
            CurEmpID = -1;


        EmpID.Value = CurEmpID.ToString();
        if (!Page.IsPostBack)
        {
            if ("true" == DecryptedRequest["Flow"])
            {
                Flow.Value = "true";
            }
            else
            {
                Flow.Value = "false";
            }

            if (CurEmpID > 0)
            {
                loadObject();
            }
            loadHierarchy();
        }

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (CurID < 0)
        {
            DBFilter filter = new DBFilter();
            filter.add(new Match("EmpID", CurEmpID));
            if (empPosDB.count(dbConn, filter) == 0)
            {
                EEmpPersonalInfo pi = new EEmpPersonalInfo();
                pi.EmpID = CurEmpID;
                EEmpPersonalInfo.db.select(dbConn, pi);
            }
        }

    }
    protected void loadHierarchy()
    {
        DBFilter filter;
        ArrayList list;

        filter = new DBFilter();
        filter.add(new Match("EmpID", CurEmpID));
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
    protected void CompanyID_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadHierarchy();
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
        {
            selected = h.HElementID.ToString();
            EHierarchyElement element = new EHierarchyElement();
            element.HElementID = h.HElementID;
            EHierarchyElement.db.select(dbConn, element);
            Label HierarchyElementLabel = (Label)e.Item.FindControl("OldHElementID");

            HierarchyElementLabel.Text = element.HElementCode + " - " + element.HElementDesc;
        }
        else
        {
            c.Text = "";
        }
        WebFormUtils.loadValues(dbConn, c, EHierarchyElement.VLHierarchyElement, filter, null, selected, "combobox.notselected");
        c.Attributes["HLevelID"] = level.HLevelID.ToString();


    }
    protected bool loadObject()
    {
        EEmpPersonalInfo obj = new EEmpPersonalInfo();
        bool isNew = WebFormWorkers.loadKeys(empDB, obj, DecryptedRequest);

        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpID", obj.EmpID));
        filter.add(WebUtils.AddRankFilter(Session, "EmpID", true));
        ArrayList empInfoList = EEmpPersonalInfo.db.select(dbConn, filter);
        if (empInfoList.Count == 0)
            return false;
        obj = (EEmpPersonalInfo)empInfoList[0];

        Hashtable values = new Hashtable();
        empDB.populate(obj, values);
        empBinding.toControl(values);
        OldEmpNo.Text = EmpNo.Text;

        EEmpPositionInfo empPos = AppUtils.GetLastPositionInfo(dbConn, CurEmpID);

        values = new Hashtable();
        empPosDB.populate(empPos, values);
        empPosBinding.toControl(values);


        return true;
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        EEmpPositionInfo newEmpPos = new EEmpPositionInfo();

        Hashtable values = new Hashtable();
        empPosBinding.toValues(values);
        PageErrors errors = PageErrors.getErrors(empPosDB, Page);
        errors.clear();
        empPosDB.validate(errors, values);
        if (!errors.isEmpty())
            return;
        empPosDB.parse(values, newEmpPos);
        int OldEmpPosID = newEmpPos.EmpPosID;

        EEmpPersonalInfo newEmpInfo = new EEmpPersonalInfo();
        values = new Hashtable();
        empBinding.toValues(values);
        errors = PageErrors.getErrors(empDB, Page);
        errors.clear();
        empDB.validate(errors, values);
        if (!errors.isEmpty())
            return;
        empDB.parse(values, newEmpInfo);
        int OldEmpID = newEmpInfo.EmpID;
        newEmpInfo.EmpID = 0;

        newEmpInfo.EmpNo = newEmpInfo.EmpNo.ToUpper();
        if (!AppUtils.checkDuplicate(dbConn, empDB, newEmpInfo, errors, "EmpNo"))
            return;

        DBFilter empTermFilter = new DBFilter();
        empTermFilter.add(new Match("EmpID", OldEmpID));
        empTermFilter.add(WebUtils.AddRankFilter(Session, "EmpID", true));
        ArrayList empTermList = EEmpTermination.db.select(dbConn, empTermFilter);
        if (empTermList.Count == 0)
        {
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_TERMINATION_RECORD_NOT_FOUND);
            return;
        }
        EEmpTermination empTerm = (EEmpTermination)empTermList[0];

        if (!empTerm.EmpTermIsTransferCompany)
        {
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_EMP_NOT_TRANSFERABLE);
            return;
        }
        if (empTerm.NewEmpID >= 0)
        {
            EEmpPersonalInfo NextEmpInfo = new EEmpPersonalInfo();
            NextEmpInfo.EmpID = empTerm.NewEmpID;
            if (EEmpPersonalInfo.db.select(dbConn, NextEmpInfo))
            {
                errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_EMP_TRANSFERRED_BEFORE, new string[] { NextEmpInfo.EmpNo }));
                return;
            }
        }
        empTerm.NewEmpID = CopyEmployeeDetail(OldEmpID);
        EEmpTermination.db.update(dbConn, empTerm);

        newEmpInfo.EmpID = empTerm.NewEmpID;
        newEmpInfo.EmpStatus = "A";
        newEmpInfo.EmpDateOfJoin = empTerm.EmpTermLastDate.AddDays(1);
        newEmpInfo.PreviousEmpID = OldEmpID;
        EEmpPersonalInfo.db.update(dbConn, newEmpInfo);

        EEmpPositionInfo oldEmpPos = new EEmpPositionInfo();
        oldEmpPos.EmpPosID = OldEmpPosID;
        if (EEmpPositionInfo.db.select(dbConn, oldEmpPos))
        {
            //oldEmpPos.EmpPosEffTo = empTerm.EmpTermLastDate;
            EEmpPositionInfo.db.update(dbConn, oldEmpPos);
        }
        newEmpPos.EmpPosEffFr = empTerm.EmpTermLastDate.AddDays(1);
        newEmpPos.EmpID = newEmpInfo.EmpID;
        newEmpPos.EmpPosID = 0;
        EEmpPositionInfo.db.insert(dbConn, newEmpPos);

        ArrayList list = new ArrayList();
        foreach (RepeaterItem item in HierarchyLevel.Items)
        {
            DropDownList d = (DropDownList)item.FindControl("HElementID");
            int HLevelID = Int32.Parse(d.Attributes["HLevelID"]);
            if (d.SelectedIndex > 0)
            {
                EEmpHierarchy h = new EEmpHierarchy();
                h.EmpID = newEmpInfo.EmpID;
                h.EmpPosID = newEmpPos.EmpPosID;
                h.HLevelID = HLevelID;
                h.HElementID = Int32.Parse(d.SelectedValue);
                list.Add(h);
            }
        }

        DBFilter empHierarchyFilter = new DBFilter();
        empHierarchyFilter.add(new Match("EmpID", newEmpInfo.EmpID));
        empHierarchyFilter.add(new Match("EmpPosID", newEmpPos.EmpPosID));
        EEmpHierarchy.db.delete(dbConn, empHierarchyFilter);
        foreach (EEmpHierarchy h in list)
        {
            EEmpHierarchy.db.insert(dbConn, h);
        }

        HROne.LeaveCalc.LeaveBalanceCalc bal = new HROne.LeaveCalc.LeaveBalanceCalc(dbConn, OldEmpID, empTerm.EmpTermLastDate);
        ArrayList balList =bal.getCurrentBalanceList();
        foreach (ELeaveBalance leaveBalance in balList)
        {
            ELeaveBalanceAdjustment leaveAdj = new ELeaveBalanceAdjustment();
            leaveAdj.EmpID = newEmpInfo.EmpID;
            leaveAdj.LeaveBalAdjDate = empTerm.EmpTermLastDate;
            leaveAdj.LeaveBalAdjType = ELeaveBalanceAdjustment.ADJUST_TYPE_RESET_BALANCE;
            leaveAdj.LeaveTypeID = leaveBalance.LeaveTypeID;
            leaveAdj.LeaveBalAdjValue = leaveBalance.getBalance();
            leaveAdj.LeaveBalAdjRemark = "System adjustment by transfer company";
            ELeaveBalanceAdjustment.db.insert(dbConn, leaveAdj);
            
        }


        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_PositionInfo_View.aspx?EmpPosID=" + newEmpPos.EmpPosID + "&EmpID=" + newEmpInfo.EmpID);


    }

    /// <summary>
    /// Copy the Employee with new EmpID
    /// </summary>
    /// <param name="OldEmpID">Old EmpID to be copy</param>
    /// <returns>New EMPID</returns>
    private int CopyEmployeeDetail(int OldEmpID)
    {
        EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
        empInfo.EmpID = OldEmpID;
        if (EEmpPersonalInfo.db.select(dbConn, empInfo))
        {
            empInfo.EmpID = 0;
            EEmpPersonalInfo.db.insert(dbConn, empInfo);

            DBFilter empFilter = new DBFilter();
            empFilter.add(new Match("EmpID", OldEmpID));

            {
                ArrayList bankAccounts = EEmpBankAccount.db.select(dbConn, empFilter);
                foreach (EEmpBankAccount bankAccount in bankAccounts)
                {
                    bankAccount.EmpID = empInfo.EmpID;
                    EEmpBankAccount.db.insert(dbConn, bankAccount);
                }
            }
            {
                ArrayList dependants = EEmpDependant.db.select(dbConn, empFilter);
                foreach (EEmpDependant dependant in dependants)
                {
                    dependant.EmpID = empInfo.EmpID;
                    EEmpDependant.db.insert(dbConn, dependant);
                }
            }
            {
                ArrayList empPoRs = EEmpPlaceOfResidence.db.select(dbConn, empFilter);
                foreach (EEmpPlaceOfResidence empPoR in empPoRs)
                {
                    empPoR.EmpID = empInfo.EmpID;
                    EEmpPlaceOfResidence.db.insert(dbConn, empPoR);
                }
            }
            {
                ArrayList empQualifications = EEmpQualification.db.select(dbConn, empFilter);
                foreach (EEmpQualification empQualification in empQualifications)
                {
                    empQualification.EmpID = empInfo.EmpID;
                    EEmpQualification.db.insert(dbConn, empQualification);
                }
            }
            {
                ArrayList empSkills = EEmpSkill.db.select(dbConn, empFilter);
                foreach (EEmpSkill empSkill in empSkills)
                {
                    empSkill.EmpID = empInfo.EmpID;
                    EEmpSkill.db.insert(dbConn, empSkill);
                }
            }
            {
                ArrayList empSpouses = EEmpSpouse.db.select(dbConn, empFilter);
                foreach (EEmpSpouse empSpouse in empSpouses)
                {
                    empSpouse.EmpID = empInfo.EmpID;
                    EEmpSpouse.db.insert(dbConn, empSpouse);
                }
            }
            {
                ArrayList empPermits = EEmpPermit.db.select(dbConn, empFilter);
                foreach (EEmpPermit empPermit in empPermits)
                {
                    empPermit.EmpID = empInfo.EmpID;
                    EEmpPermit.db.insert(dbConn, empPermit);
                }
            }
            {
                ArrayList empWorkExps = EEmpWorkExp.db.select(dbConn, empFilter);
                foreach (EEmpWorkExp empWorkExp in empWorkExps)
                {
                    empWorkExp.EmpID = empInfo.EmpID;
                    EEmpWorkExp.db.insert(dbConn, empWorkExp);
                }
            }
            {
                ArrayList empEmergencyContacts = EEmpEmergencyContact.db.select(dbConn, empFilter);
                foreach (EEmpEmergencyContact empEmergencyContact in empEmergencyContacts)
                {
                    empEmergencyContact.EmpID = empInfo.EmpID;
                    EEmpEmergencyContact.db.insert(dbConn, empEmergencyContact);
                }
            }
            {             
                ArrayList empEmpExtraFieldValues = EEmpExtraFieldValue.db.select(dbConn, empFilter);
                foreach (EEmpExtraFieldValue empEmpExtraFieldValue in empEmpExtraFieldValues)
                {
                    empEmpExtraFieldValue.EmpID = empInfo.EmpID;
                    EEmpExtraFieldValue.db.insert(dbConn, empEmpExtraFieldValue);
                }
            } 
            {
                ArrayList empUniforms = EEmpUniform.db.select(dbConn, empFilter);
                foreach (EEmpUniform empUniform in empUniforms)
                {
                    empUniform.EmpID = empInfo.EmpID;
                    EEmpUniform.db.insert(dbConn, empUniform);
                }
            } 
            return empInfo.EmpID;
        }
        return 0;
    }
}
