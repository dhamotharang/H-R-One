using System;
using System.Collections;
using System.Collections.Generic ;
using System.IO;
using System.Text;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;
using System.Xml;
using HROne.DataAccess;
using HROne.Lib.Entities;
using HROne.Common;
using HROne;
using CrystalDecisions.CrystalReports.Engine;

public class HROneWebMasterPage : System.Web.UI.MasterPage
{
    protected System.Globalization.CultureInfo m_ci = null;
    protected DatabaseConnection m_dbConn = null;
    protected System.Collections.Specialized.NameValueCollection m_decryptedRequest = null;

    public System.Collections.Specialized.NameValueCollection DecryptedRequest 
    {
        get
        {
            if (m_decryptedRequest == null)
                m_decryptedRequest = HROne.Common.WebUtility.getDecryptQueryStringCollection(Session, Request.Url.Query);
            return m_decryptedRequest;
        }
 
    }

    public DatabaseConnection dbConn
    {
        get
        {
            if (m_dbConn == null)
                m_dbConn = HROne.Common.WebUtility.GetDatabaseConnection(Session);
            return m_dbConn;
        }
    }
    public System.Globalization.CultureInfo ci
    {
        get
        {
            if (m_ci == null)
                m_ci = HROne.Common.WebUtility.GetSessionUICultureInfo(Session);
            return m_ci;
        }
    }
}

public class HROneWebPage : System.Web.UI.Page
{
    protected System.Globalization.CultureInfo m_ci = null;
    protected DatabaseConnection m_dbConn = null;
    protected EUser m_user = null;

    protected System.Collections.Specialized.NameValueCollection m_decryptedRequest = null;

    public System.Collections.Specialized.NameValueCollection DecryptedRequest
    {
        get
        {
            if (this.Master is HROneWebMasterPage)
                return ((HROneWebMasterPage)Page.Master).DecryptedRequest;
            if (m_decryptedRequest == null)
                m_decryptedRequest = HROne.Common.WebUtility.getDecryptQueryStringCollection(Session, Request.Url.Query);
            return m_decryptedRequest;
        }

    }
    
    public DatabaseConnection dbConn
    {
        get
        {
            if (this.Master is HROneWebMasterPage)
                return ((HROneWebMasterPage)Page.Master).dbConn;
            if (m_dbConn == null)
                m_dbConn = HROne.Common.WebUtility.GetDatabaseConnection(Session);
            return m_dbConn;
        }
    }
    public EUser user
    {
        get
        {
            if (m_user == null)
                m_user = WebUtils.GetCurUser(Session);
            return m_user;
        }
    }
    public System.Globalization.CultureInfo ci
    {
        get
        {
            if (this.Master is HROneWebMasterPage)
                return ((HROneWebMasterPage)Page.Master).ci;
            if (m_ci == null)
                m_ci = HROne.Common.WebUtility.GetSessionUICultureInfo(Session);
            return m_ci;
        }
    }
}

public class HROneWebControl : System.Web.UI.UserControl
{
    protected System.Globalization.CultureInfo m_ci = null;
    protected DatabaseConnection m_dbConn = null;

    protected System.Collections.Specialized.NameValueCollection m_decryptedRequest = null;

    public System.Collections.Specialized.NameValueCollection DecryptedRequest
    {
        get
        {
            if (this.Page is HROneWebPage)
                return ((HROneWebMasterPage)Page.Master).DecryptedRequest;
            if (m_decryptedRequest == null)
                m_decryptedRequest = HROne.Common.WebUtility.getDecryptQueryStringCollection(Session, Request.Url.Query);
            return m_decryptedRequest;
        }

    }

    public DatabaseConnection dbConn
    {
        get
        {
            if (this.Page is HROneWebPage)
                return ((HROneWebMasterPage)Page.Master).dbConn;
            if (m_dbConn == null)
                m_dbConn = HROne.Common.WebUtility.GetDatabaseConnection(Session);
            return m_dbConn;
        }
    }
    public System.Globalization.CultureInfo ci
    {
        get
        {
            if (this.Page is HROneWebPage)
                return ((HROneWebMasterPage)Page.Master).ci;
            if (m_ci == null)
                m_ci = HROne.Common.WebUtility.GetSessionUICultureInfo(Session);
            return m_ci;
        }
    }
}

public class EmpUtils
{
    public static void DeleteEmp(DatabaseConnection dbConn, int EmpID)
    {
        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpID", EmpID));

        ArrayList empDocumentList = EEmpDocument.db.select(dbConn, filter);
        string uploadFolder = AppUtils.GetDocumentUploadFolder(dbConn);
        foreach (EEmpDocument empDocument in empDocumentList)
        {
            string UploadFile = System.IO.Path.Combine(uploadFolder, empDocument.EmpDocumentStoredFileName);
            try
            {
                if (System.IO.File.Exists(UploadFile))
                    System.IO.File.Delete(UploadFile);
            }
            catch(Exception ex)
            {
                //  Fail to remove document
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

        }

        EEmpAVCPlan.db.delete(dbConn, filter);
        EEmpBankAccount.db.delete(dbConn, filter);
        EEmpContractTerms.db.delete(dbConn, filter);

        ArrayList empCostCenterList = EEmpCostCenter.db.select(dbConn, filter);
        foreach (EEmpCostCenter empCostCenter in empCostCenterList)
        {
            DBFilter empCostCenterDetailFilter = new DBFilter();
            empCostCenterDetailFilter.add(new Match("EmpCostCenterID", empCostCenter.EmpCostCenterID));
            EEmpCostCenterDetail.db.delete(dbConn, empCostCenterDetailFilter);
        }
        EEmpCostCenter.db.delete(dbConn, filter);
        EEmpDependant.db.delete(dbConn, filter);


        EEmpEmergencyContact.db.delete(dbConn, filter);
        EEmpExtraFieldValue.db.delete(dbConn, filter);
        EEmpFinalPayment.db.delete(dbConn, filter);
        EEmpHierarchy.db.delete(dbConn, filter);
        EEmpMPFPlan.db.delete(dbConn, filter);
        EEmpORSOPlan.db.delete(dbConn, filter);
        EEmpPermit.db.delete(dbConn, filter);
        EEmpPersonalInfo.db.delete(dbConn, filter);
        EEmpPlaceOfResidence.db.delete(dbConn, filter);
        EEmpPositionInfo.db.delete(dbConn, filter);
        EEmpQualification.db.delete(dbConn, filter);
        EEmpRecurringPayment.db.delete(dbConn, filter);
        EEmpSkill.db.delete(dbConn, filter);
        EEmpSpouse.db.delete(dbConn, filter);
        EEmpUniform.db.delete(dbConn, filter);
        EEmpWorkExp.db.delete(dbConn, filter);
        EEmpWorkInjuryRecord.db.delete(dbConn, filter);
        EEmpTermination.db.delete(dbConn, filter);
        EEmpFinalPayment.db.delete(dbConn, filter);

        ELeaveApplication.db.delete(dbConn, filter);
        ELeaveBalance.db.delete(dbConn, filter);
        ELeaveBalanceAdjustment.db.delete(dbConn, filter);

        EClaimsAndDeductions.db.delete(dbConn, filter);
        ArrayList empPayrollList = EEmpPayroll.db.select(dbConn, filter);
        foreach (EEmpPayroll empPayroll in empPayrollList)
        {
            DBFilter empPayrollFilter = new DBFilter();
            empPayrollFilter.add(new Match("EmpPayrollID", empPayroll.EmpPayrollID));
            EPaymentRecord.db.delete(dbConn, empPayrollFilter);
            EMPFRecord.db.delete(dbConn, empPayrollFilter);
            EORSORecord.db.delete(dbConn, empPayrollFilter);
            EEmpPayroll.db.delete(dbConn, empPayroll);
        }

        EEmpPayroll.db.delete(dbConn, filter);


    }
    public static void CopyEmpDetail(DatabaseConnection dbConn, int OldEmpID, int NewEmpID)
    {
        DBFilter empIDFilter = new DBFilter();
        empIDFilter.add(new Match("EmpID", OldEmpID));
        
        ArrayList list = EEmpBankAccount.db.select(dbConn, empIDFilter);
        foreach (EEmpBankAccount obj in list)
        {
            obj.EmpID = NewEmpID;
            EEmpBankAccount.db.insert(dbConn, obj);
        }

        list = EEmpDependant.db.select(dbConn, empIDFilter);
        foreach (EEmpDependant obj in list)
        {
            obj.EmpID = NewEmpID;
            EEmpDependant.db.insert(dbConn, obj);
        }

        list = EEmpEmergencyContact.db.select(dbConn, empIDFilter);
        foreach (EEmpEmergencyContact obj in list)
        {
            obj.EmpID = NewEmpID;
            EEmpEmergencyContact.db.insert(dbConn, obj);
        }

        list = EEmpPermit.db.select(dbConn, empIDFilter);
        foreach (EEmpPermit obj in list)
        {
            obj.EmpID = NewEmpID;
            EEmpPermit.db.insert(dbConn, obj);
        }

        list = EEmpQualification.db.select(dbConn, empIDFilter);
        foreach (EEmpQualification obj in list)
        {
            obj.EmpID = NewEmpID;
            EEmpQualification.db.insert(dbConn, obj);
        }

        list = EEmpSkill.db.select(dbConn, empIDFilter);
        foreach (EEmpSkill obj in list)
        {
            obj.EmpID = NewEmpID;
            EEmpSkill.db.insert(dbConn, obj);
        }

        list = EEmpSpouse.db.select(dbConn, empIDFilter);
        foreach (EEmpSpouse obj in list)
        {
            obj.EmpID = NewEmpID;
            EEmpSpouse.db.insert(dbConn, obj);
        }

        list = EEmpUniform.db.select(dbConn, empIDFilter);
        foreach (EEmpUniform obj in list)
        {
            obj.EmpID = NewEmpID;
            EEmpUniform.db.insert(dbConn, obj);
        }

        list = EEmpWorkExp.db.select(dbConn, empIDFilter);
        foreach (EEmpWorkExp obj in list)
        {
            obj.EmpID = NewEmpID;
            EEmpWorkExp.db.insert(dbConn, obj);
        }
    }
}

/// <summary>
/// Summary description for WebUtils
/// </summary>
public class WebUtils
{
    private const string keyString = "HROne";

    //[ThreadStatic]
    //public static ProductLicense productLicense = null;
    private const int PRODUCTKEY_POPUPPERIOD = 15;
    private const string SESSION_PRODUCTLICENSE = "ProductLicense";
    private const string SESSION_TRIALVERSION = "TrialVersion";
    public static ProductLicense productLicense(HttpSessionState Session)
    {
        return (ProductLicense)Session[SESSION_PRODUCTLICENSE];
    }
    //public static DatabaseType LoadDBType()
    //{

    //    HROneConfig configure = new HROneConfig();
    //    DatabaseType DBType = configure.GetDatabaseType();
    //    if (DBType == null)
    //    {

    //        DatabaseType oldDBType = perspectivemind.common.DBUtil.type;

    //        if (oldDBType != null)
    //        {
    //            if (oldDBType is SQLType)
    //            {
    //                configure.DBType = HROneConfig.DBTypeEmun.MSSQL;
    //                configure.ConnectionString = ((SQLType)oldDBType).url;


    //            }
    //        }
    //        configure.Save();
    //        configure.load();
    //        DBType = configure.GetDatabaseType();
    //    }


    //    return DBType;
    //}
    public static DatabaseConnection GetDatabaseConnection()
    {
        HROneConfig configure = HROneConfig.GetCurrentHROneConfig();
        DatabaseConnection DBType = configure.GetDatabaseConnection();

        return DBType;
    }
    public static DatabaseConnection GetDatabaseConnection(string DatabaseName)
    {
        HROneConfig configure = HROneConfig.GetCurrentHROneConfig();
        DatabaseConnection DBType = configure.GetDatabaseConnection(DatabaseName);

        return DBType;
    }
    public interface HROneDBConfigUIInterface
    {
        DatabaseConfig GenerateDBType();
    }
    public enum DBTypeEmun
    {
        None = 0,
        MSSQL = 1

    }

    public static bool IsTrialVersion(HttpSessionState Session)
    {
        if (Session[SESSION_TRIALVERSION] == null)
            return false;
        else
            return (bool)Session[SESSION_TRIALVERSION];

    }

    public static int TotalActiveUser(DatabaseConnection dbConn, int ExcludeUserID)
    {
        DBFilter userCountFilter = new DBFilter();
        userCountFilter.add(new Match("UserAccountStatus", "A"));
        userCountFilter.add(new Match("UserID", "<>", ExcludeUserID));
        return EUser.db.count(dbConn, userCountFilter);
    }

    public static int TotalActiveCompany(DatabaseConnection dbConn, int ExcludeCompanyID)
    {
        DBFilter companyCountFilter = new DBFilter();
        companyCountFilter.add(new Match("CompanyID", "<>", ExcludeCompanyID));
        return ECompany.db.count(dbConn, companyCountFilter);
    }

    public static int TotalActiveEmployee(DatabaseConnection dbConn, int ExcludeEmpID)
    {
        DBFilter empCountFilter = new DBFilter();
        empCountFilter.add(new Match("EmpStatus", "A"));
        empCountFilter.add(new Match("EmpID", "<>", ExcludeEmpID));
        return EEmpPersonalInfo.db.count(dbConn, empCountFilter);
    }
    public static void AddLanguageOptionstoDropDownList(DropDownList dropDownList)
    {
        dropDownList.Items.Add(new ListItem("English", "en"));
        dropDownList.Items.Add(new ListItem("中文(繁體)", "big5"));
        dropDownList.Items.Add(new ListItem("中文(簡體)", "gb"));
    }

    public static void SetSessionLanguage(HttpSessionState Session, EUser user)
    {
        string defaultLang = string.Empty;
        DatabaseConnection dbConn = HROne.Common.WebUtility.GetDatabaseConnection(Session);
        if (user != null)
        {
            if (EUser.db.select(dbConn, user))
                defaultLang = user.UserLanguage;
        }
        if (string.IsNullOrEmpty(defaultLang))
        {
            try
            {
                defaultLang = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_DEFAULT_LANGUAGE);
            }
            catch
            {
                //HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/DatabaseConfiguration.aspx");
            }
            if (string.IsNullOrEmpty(defaultLang))
            {
                defaultLang = "en";
            }
            if (defaultLang.Equals("big5", StringComparison.CurrentCultureIgnoreCase))
                defaultLang = "zh-cht";
            else if (defaultLang.Equals("gb", StringComparison.CurrentCultureIgnoreCase))
                defaultLang = "zh-chs";
        }
        Session.Add("lang", defaultLang);
    }

    public static void SetSessionDatabaseConnection(HttpSessionState Session, DatabaseConnection dbConn)
    {
        Session["DatabaseConnection"] = dbConn;
        //DatabaseConnection.SetDefaultDatabaseConnection(dbConn);
        WebUtils.LoadProductKey(Session);
        WebUtils.SetSessionLanguage(Session, null);
        
        HROneConfig config = HROneConfig.GetCurrentHROneConfig();
        Session["EncryptQueryString"] = config.EncryptedURLQueryString;

        HROne.ProductVersion.Database database = new HROne.ProductVersion.Database(dbConn);
        if (!database.UpdateDatabaseVersion(true))
            Session["NeedDBUpgrade"] = true;

        string parameterValue = string.Empty;
        try
        {

            parameterValue = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_SESSION_TIMEOUT);
            if (!string.IsNullOrEmpty(parameterValue))
            {
                int sessionTimeout = 0;
                if (int.TryParse(parameterValue, out sessionTimeout))
                {
                    if (sessionTimeout > 0)
                        Session.Timeout = sessionTimeout;
                }
            }
        }
        catch
        {
            parameterValue = string.Empty;
        }

        if (!string.IsNullOrEmpty(Session.SessionID))
            ClearTempTable(dbConn, Session.SessionID);

        try
        {
            parameterValue = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_FORCE_DECRYPT_ON_STARTUP);
        }
        catch
        {
            parameterValue = string.Empty;
        }
        if (!string.IsNullOrEmpty(parameterValue))
            if (getDateFromKey(parameterValue).Equals(AppUtils.ServerDateTime().Date))
            {
                DBAESEncryptStringFieldAttribute.skipEncryptionToDB = true;
                ArrayList emplist = EEmpPersonalInfo.db.select(dbConn, new DBFilter());
                foreach (EEmpPersonalInfo empInfo in emplist)
                {
                    EEmpPersonalInfo.db.update(dbConn, empInfo);
                }
                ArrayList empSpouseList = EEmpSpouse.db.select(dbConn, new DBFilter());
                foreach (EEmpSpouse empSpouse in empSpouseList)
                {
                    EEmpSpouse.db.update(dbConn, empSpouse);
                }
                ArrayList empDependantList = EEmpDependant.db.select(dbConn, new DBFilter());
                foreach (EEmpDependant empDependant in empDependantList)
                {
                    EEmpDependant.db.update(dbConn, empDependant);
                }
                ArrayList empBankAccountList = EEmpBankAccount.db.select(dbConn, new DBFilter());
                foreach (EEmpBankAccount empBankAccount in empBankAccountList)
                {
                    EEmpBankAccount.db.update(dbConn, empBankAccount);
                }
                ArrayList companyList = ECompany.db.select(dbConn, new DBFilter());
                foreach (ECompany company in companyList)
                {
                    ECompany.db.update(dbConn, company);
                }
                ArrayList positionList = EPosition.db.select(dbConn, new DBFilter());
                foreach (EPosition position in positionList)
                {
                    EPosition.db.update(dbConn, position);
                }
                ArrayList rankList = ERank.db.select(dbConn, new DBFilter());
                foreach (ERank rank in rankList)
                {
                    ERank.db.update(dbConn, rank);
                }
                ArrayList staffTypeList = EStaffType.db.select(dbConn, new DBFilter());
                foreach (EStaffType staffType in staffTypeList)
                {
                    EStaffType.db.update(dbConn, staffType);
                }
                ArrayList payrollGroupList = EPayrollGroup.db.select(dbConn, new DBFilter());
                foreach (EPayrollGroup payrolGroup in payrollGroupList)
                {
                    EPayrollGroup.db.update(dbConn, payrolGroup);
                }
                ArrayList leavePlanList = ELeavePlan.db.select(dbConn, new DBFilter());
                foreach (ELeavePlan leavePlan in leavePlanList)
                {
                    ELeavePlan.db.update(dbConn, leavePlan);
                }
                ArrayList hierarchyElementList = EHierarchyElement.db.select(dbConn, new DBFilter());
                foreach (EHierarchyElement hierarchyElement in hierarchyElementList)
                {
                    EHierarchyElement.db.update(dbConn, hierarchyElement);
                }
                ArrayList costCenterList = ECostCenter.db.select(dbConn, new DBFilter());
                foreach (ECostCenter costCenter in costCenterList)
                {
                    ECostCenter.db.update(dbConn, costCenter);
                }
                ArrayList cessationReasonList = ECessationReason.db.select(dbConn, new DBFilter());
                foreach (ECessationReason cessationReason in cessationReasonList)
                {
                    ECessationReason.db.update(dbConn, cessationReason);
                }
                ArrayList empTerminationReasonList = EEmpTermination.db.select(dbConn, new DBFilter());
                foreach (EEmpTermination empTermination in empTerminationReasonList)
                {
                    EEmpTermination.db.update(dbConn, empTermination);
                }
                ArrayList empPoRList = EEmpPlaceOfResidence.db.select(dbConn, new DBFilter());
                foreach (EEmpPlaceOfResidence empPoR in empPoRList)
                {
                    EEmpPlaceOfResidence.db.update(dbConn, empPoR);
                }
                ArrayList empFinalPaymentList = EEmpFinalPayment.db.select(dbConn, new DBFilter());
                foreach (EEmpFinalPayment empFinalPayment in empFinalPaymentList)
                {
                    EEmpFinalPayment.db.update(dbConn, empFinalPayment);
                }
                ArrayList paymentCodeList = EPaymentCode.db.select(dbConn, new DBFilter());
                foreach (EPaymentCode paymentCode in paymentCodeList)
                {
                    EPaymentCode.db.update(dbConn, paymentCode);
                }
                ArrayList auditTrailDetailList = EAuditTrailDetail.db.select(dbConn, new DBFilter());
                foreach (EAuditTrailDetail auditTrialDetail in auditTrailDetailList)
                {
                    EAuditTrailDetail.db.update(dbConn, auditTrialDetail);
                }
                ArrayList companyBankAccountList = ECompanyBankAccount.db.select(dbConn, new DBFilter());
                foreach (ECompanyBankAccount companyBankAccount in companyBankAccountList)
                {
                    ECompanyBankAccount.db.update(dbConn, companyBankAccount);
                }
                ArrayList taxCompanyList = ETaxCompany.db.select(dbConn, new DBFilter());
                foreach (ETaxCompany taxCompany in taxCompanyList)
                {
                    ETaxCompany.db.update(dbConn, taxCompany);
                }
                ArrayList taxEmpList = ETaxEmp.db.select(dbConn, new DBFilter());
                foreach (ETaxEmp taxEmp in taxEmpList)
                {
                    ETaxEmp.db.update(dbConn, taxEmp);
                }
                ArrayList userGroupList = EUserGroup.db.select(dbConn, new DBFilter());
                foreach (EUserGroup usergroup in userGroupList)
                {
                    EUserGroup.db.update(dbConn, usergroup);
                }

                DBFilter filter = new DBFilter();
                filter.add(new Match("ParameterCode", ESystemParameter.PARAM_CODE_FORCE_DECRYPT_ON_STARTUP));

                ESystemParameter.db.delete(dbConn, filter);
                //HttpRuntime.UnloadAppDomain();
            }
        try
        {

            parameterValue = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_FORCE_ENCRYPT_ON_STARTUP);
        }
        catch
        {
            parameterValue = string.Empty;
        }
        if (!string.IsNullOrEmpty(parameterValue))
            if (parameterValue.Equals("Y"))
            {
                DBAESEncryptStringFieldAttribute.skipEncryptionToDB = false;
                ArrayList emplist = EEmpPersonalInfo.db.select(dbConn, new DBFilter());
                foreach (EEmpPersonalInfo empInfo in emplist)
                {
                    EEmpPersonalInfo.db.update(dbConn, empInfo);
                }
                ArrayList empSpouseList = EEmpSpouse.db.select(dbConn, new DBFilter());
                foreach (EEmpSpouse empSpouse in empSpouseList)
                {
                    EEmpSpouse.db.update(dbConn, empSpouse);
                }
                ArrayList empDependantList = EEmpDependant.db.select(dbConn, new DBFilter());
                foreach (EEmpDependant empDependant in empDependantList)
                {
                    EEmpDependant.db.update(dbConn, empDependant);
                }
                ArrayList empBankAccountList = EEmpBankAccount.db.select(dbConn, new DBFilter());
                foreach (EEmpBankAccount empBankAccount in empBankAccountList)
                {
                    EEmpBankAccount.db.update(dbConn, empBankAccount);
                }
                ArrayList companyList = ECompany.db.select(dbConn, new DBFilter());
                foreach (ECompany company in companyList)
                {
                    ECompany.db.update(dbConn, company);
                }
                ArrayList positionList = EPosition.db.select(dbConn, new DBFilter());
                foreach (EPosition position in positionList)
                {
                    EPosition.db.update(dbConn, position);
                }
                ArrayList rankList = ERank.db.select(dbConn, new DBFilter());
                foreach (ERank rank in rankList)
                {
                    ERank.db.update(dbConn, rank);
                }
                ArrayList staffTypeList = EStaffType.db.select(dbConn, new DBFilter());
                foreach (EStaffType staffType in staffTypeList)
                {
                    EStaffType.db.update(dbConn, staffType);
                }
                ArrayList payrollGroupList = EPayrollGroup.db.select(dbConn, new DBFilter());
                foreach (EPayrollGroup payrolGroup in payrollGroupList)
                {
                    EPayrollGroup.db.update(dbConn, payrolGroup);
                }
                ArrayList leavePlanList = ELeavePlan.db.select(dbConn, new DBFilter());
                foreach (ELeavePlan leavePlan in leavePlanList)
                {
                    ELeavePlan.db.update(dbConn, leavePlan);
                }
                ArrayList hierarchyElementList = EHierarchyElement.db.select(dbConn, new DBFilter());
                foreach (EHierarchyElement hierarchyElement in hierarchyElementList)
                {
                    EHierarchyElement.db.update(dbConn, hierarchyElement);
                }
                ArrayList costCenterList = ECostCenter.db.select(dbConn, new DBFilter());
                foreach (ECostCenter costCenter in costCenterList)
                {
                    ECostCenter.db.update(dbConn, costCenter);
                }
                ArrayList cessationReasonList = ECessationReason.db.select(dbConn, new DBFilter());
                foreach (ECessationReason cessationReason in cessationReasonList)
                {
                    ECessationReason.db.update(dbConn, cessationReason);
                }
                ArrayList empTerminationReasonList = EEmpTermination.db.select(dbConn, new DBFilter());
                foreach (EEmpTermination empTermination in empTerminationReasonList)
                {
                    EEmpTermination.db.update(dbConn, empTermination);
                }
                ArrayList empPoRList = EEmpPlaceOfResidence.db.select(dbConn, new DBFilter());
                foreach (EEmpPlaceOfResidence empPoR in empPoRList)
                {
                    EEmpPlaceOfResidence.db.update(dbConn, empPoR);
                }
                ArrayList empFinalPaymentList = EEmpFinalPayment.db.select(dbConn, new DBFilter());
                foreach (EEmpFinalPayment empFinalPayment in empFinalPaymentList)
                {
                    EEmpFinalPayment.db.update(dbConn, empFinalPayment);
                }
                ArrayList paymentCodeList = EPaymentCode.db.select(dbConn, new DBFilter());
                foreach (EPaymentCode paymentCode in paymentCodeList)
                {
                    EPaymentCode.db.update(dbConn, paymentCode);
                }
                ArrayList auditTrailDetailList = EAuditTrailDetail.db.select(dbConn, new DBFilter());
                foreach (EAuditTrailDetail auditTrialDetail in auditTrailDetailList)
                {
                    EAuditTrailDetail.db.update(dbConn, auditTrialDetail);
                }
                ArrayList companyBankAccountList = ECompanyBankAccount.db.select(dbConn, new DBFilter());
                foreach (ECompanyBankAccount companyBankAccount in companyBankAccountList)
                {
                    ECompanyBankAccount.db.update(dbConn, companyBankAccount);
                }
                ArrayList taxCompanyList = ETaxCompany.db.select(dbConn, new DBFilter());
                foreach (ETaxCompany taxCompany in taxCompanyList)
                {
                    ETaxCompany.db.update(dbConn, taxCompany);
                }
                ArrayList taxEmpList = ETaxEmp.db.select(dbConn, new DBFilter());
                foreach (ETaxEmp taxEmp in taxEmpList)
                {
                    ETaxEmp.db.update(dbConn, taxEmp);
                }
                ArrayList userGroupList = EUserGroup.db.select(dbConn, new DBFilter());
                foreach (EUserGroup usergroup in userGroupList)
                {
                    EUserGroup.db.update(dbConn, usergroup);
                }

                DBFilter filter = new DBFilter();
                filter.add(new Match("ParameterCode", ESystemParameter.PARAM_CODE_FORCE_ENCRYPT_ON_STARTUP));

                ESystemParameter.db.delete(dbConn, filter);
                //HttpRuntime.UnloadAppDomain();
            }

    }

    public static void ClearTempTable(DatabaseConnection dbConn, string SessionID)
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("Clear Upload Claims and Deduction Table...");
            HROne.Import.ImportClaimsAndDeductionsProcess.ClearTempTable(dbConn, SessionID);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Fail to clear temp table: " + ex.Message);
        }

        try
        {
            System.Diagnostics.Debug.WriteLine("Clear Upload Employee Table...");
            HROne.Import.ImportEmpPersonalInfoProcess.ClearTempTable(dbConn, SessionID);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Fail to clear temp table: " + ex.Message);
        }

        try
        {
            System.Diagnostics.Debug.WriteLine("Clear Upload Employee Working Summary...");
            HROne.Import.ImportEmpWorkingSummaryProcess.ClearTempTable(dbConn, SessionID);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Fail to clear temp table: " + ex.Message);
        }

        try
        {
            System.Diagnostics.Debug.WriteLine("Clear Upload Attendance Record Table...");
            HROne.Import.ImportAttendanceRecordProcess.ClearTempTable(dbConn, SessionID);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Fail to clear temp table: " + ex.Message);
        }

        try
        {
            System.Diagnostics.Debug.WriteLine("Clear Upload Roster Table...");
            HROne.Import.ImportRosterTableProcess.ClearTempTable(dbConn, SessionID);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Fail to clear temp table: " + ex.Message);
        }

        try
        {
            System.Diagnostics.Debug.WriteLine("Clear Upload Leave Balance Adjustment Table...");
            HROne.Import.ImportLeaveBalanceAdjustmentProcess.ClearTempTable(dbConn, SessionID);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Fail to clear temp table: " + ex.Message);
        }
    }
    //public static DataTable GetDataTableFromSelectQueryWithFilter(string select, string from, DBFilter filter, ListInfo info)
    //{
    //    return GetDataTableFromSelectQueryWithFilter(null, select, from, filter, info);
    //}
    public static DataTable GetDataTableFromSelectQueryWithFilter(DatabaseConnection dbConn, string select, string from, DBFilter filter, ListInfo info)
    {
        DBFilter queryFilter = new DBFilter();
        ArrayList afterFilterList = new ArrayList();
        foreach (DBTerm term in filter.terms())
        {
            if (term is Match)
            {
                Match match = (Match)term;
                if (match.value is string && match.op.Equals("Like", StringComparison.CurrentCultureIgnoreCase))
                {
                    afterFilterList.Add(term);
                }
                else
                    queryFilter.add(term);
            }
            else
                queryFilter.add(term);
        }

        DataTable table =null;
        if (dbConn != null)
            table = queryFilter.loadData(dbConn, null, select, from, null);
        //else
        //{
        //    dbConn = DatabaseConnection.GetDatabaseConnection();
        //    table = queryFilter.loadData(dbConn, null, select, from);
        //}
        foreach (Match match in afterFilterList)
        {
            DBAESEncryptStringFieldAttribute.decode(table, match.name);
            DataView view = new DataView(table);
            view.RowFilter = match.name + " " + (string.IsNullOrEmpty(match.op) ? "=" : match.op) + " '" + ((string)match.value).Replace("'", "''") + "' ";
            table = view.ToTable();
        }

        if (info != null)
        {
            if (!string.IsNullOrEmpty(info.orderby))
                if (info.orderby.Equals("EmpEngFullName", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (!table.Columns.Contains("EmpEngFullName"))
                    {
                        table.Columns.Add("EmpEngFullName", typeof(string));
                        foreach (System.Data.DataRow row in table.Rows)
                        {
                            EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                            empInfo.EmpID = (int)row["EmpID"];
                            if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                                row["EmpEngFullName"] = empInfo.EmpEngFullName;
                        }
                    }
                }
            return DataTableSortingAndPaging(table, info);
        }
        else
            return table;
    }

    public static DataTable DataTableSortingAndPaging(DataTable sourceTable, ListInfo info)
    {
        DataTable table = sourceTable.Copy();
        if (info != null)
        {
            if (!string.IsNullOrEmpty(info.orderby))
            {
                string[] orderbyList = info.orderby.Split(new char[] { ',' });
                string actualOrderString = string.Empty;
                foreach (string orderBy in orderbyList)
                {
                    DBAESEncryptStringFieldAttribute.decode(table, orderBy.Trim());
                    string orderString = orderBy + (info.order ? "" : " DESC");
                    if (string.IsNullOrEmpty(actualOrderString))
                        actualOrderString = orderString;
                    else
                        actualOrderString += "," + orderString;
                }
                DataView resultView = new DataView(table);
                resultView.Sort = actualOrderString;
                table = resultView.ToTable();
            }

            if (info.recordPerPage > 0)
            {
                info.numRecord = table.Rows.Count;
                info.numPage = info.numRecord / info.recordPerPage;
                if (info.numRecord % info.recordPerPage > 0)
                    info.numPage++;
                if (info.page == info.numPage && info.numPage > 0)
                    info.page--;
                int startIndex = info.recordPerPage * (info.page);
                int endIndex = info.recordPerPage * (info.page + 1) - 1;

                for (int i = table.Rows.Count - 1; i >= 0; i--)
                {
                    if (i < startIndex || i > endIndex)
                        table.Rows.Remove(table.Rows[i]);
                }
            }
            else
                info.numRecord = table.Rows.Count;

        }
        return table;
    }

    public static ArrayList SelectedRepeaterItemToBaseObjectList(DBManager db, Repeater RepeaterControl, string CheckBoxName)
    {
        ArrayList list = new ArrayList();
        foreach (RepeaterItem i in RepeaterControl.Items)
        {
            CheckBox cb = (CheckBox)i.FindControl(CheckBoxName);
            if (cb.Checked)
            {
                BaseObject o = (BaseObject)db.createObject();
                WebFormUtils.GetKeys(db, o, cb);
                list.Add(o);
            }

        }
        return list;
    }

    public enum AccessLevel
    {
        None,
        Read,
        ReadWrite
    };

    public static void loadPageList(LinkButton PrevPage, HtmlImage PrevPageImg,
        LinkButton NextPage, HtmlImage NextPageImg,
        LinkButton FirstPage, HtmlImage FirstPageImg,
        LinkButton LastPage, HtmlImage LastPageImg)
    {
        string s = "~";//PrevPage.Page.Request.ApplicationPath;
        if (PrevPage.Enabled)
            PrevPageImg.Src = s + "/images/previous.gif";
        else
            PrevPageImg.Src = s + "/images/previous_off.gif";
        if (NextPage.Enabled)
            NextPageImg.Src = s + "/images/next.gif";
        else
            NextPageImg.Src = s + "/images/next_off.gif";
        if (FirstPage.Enabled)
            FirstPageImg.Src = s + "/images/start.gif";
        else
            FirstPageImg.Src = s + "/images/start_off.gif";
        if (LastPage.Enabled)
            LastPageImg.Src = s + "/images/end.gif";
        else
            LastPageImg.Src = s + "/images/end_off.gif";
    }

    protected static bool LoadProductKey(HttpSessionState Session)
    {
        DatabaseConnection dbConn = HROne.Common.WebUtility.GetDatabaseConnection(Session);
        HROne.ProductLicense tmpProductKey = new HROne.ProductLicense();
        //  will create dummy product license if not exists
        tmpProductKey.LoadProductLicense(dbConn);
        //tmpProductKey.SetFeatureByCode(ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_PRODUCTFEATURECODE));
        Session[SESSION_PRODUCTLICENSE] = tmpProductKey;
        if (tmpProductKey.IsValidProductKey)
        {
            return true;
        }
        else
        {
            return false;
        }
    }



    public static bool CheckAccess(HttpResponse Response, HttpSessionState Session)
    {
        //DatabaseConnection dbConn = HROne.Common.WebUtility.GetDatabaseConnection(Session);
        //if (GetCurUser(Session) == null)
        //{
        //    HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Login.aspx");
        //    return false;
        //}
        ////HROne.Common.WebUtility.initLanguage(Session);
        //if ((!productLicense(Session).IsValidProductKey && !IsTrialVersion(Session)))
        //{
        //    //if (WebUtils.getLastTrialDate().AddDays(-DEFAULT_TRIALPERIOD) < AppUtils.ServerDateTime().Date)
        //    {
        //        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "ProductKey.aspx");
        //        return false;
        //    }
        //}
        //else
        //{
        //    if (!productLicense(Session).IsValidAuthorizationCode())
        //    {
        //        if (productLicense(Session).IsTrialPeriodExpiry())
        //        {
        //            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "ProductKey.aspx");
        //            return false;
        //        }
        //        if (Session["TrialVersion"] == null)
        //        {
        //            if (WebUtils.productLicense(Session).LastTrialDate.AddDays(-PRODUCTKEY_POPUPPERIOD) < AppUtils.ServerDateTime().Date)
        //            {
        //                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "ProductKey.aspx");
        //                return false;
        //            }
        //        }
        //    }
        //}
        //if (IsSuperUserMissing(dbConn) && ((bool)Session["IgnoreEM"]) != true)
        //{
        //    HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/EM_Warning.aspx");
        //    return false;
        //}
        ////return true;
        //if (Session["ForceChangePassword"] != null)
        //    if (Session["ForceChangePassword"].Equals(true))
        //    {
        //        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/ChangePassword.aspx");
        //    }
        //return true;
        return CheckAccess(Response, Session, string.Empty, AccessLevel.None);
    }
    public static bool CheckAccess(HttpResponse Response, HttpSessionState Session, string function, AccessLevel level)
    {
        DatabaseConnection dbConn = HROne.Common.WebUtility.GetDatabaseConnection(Session);
        if (GetCurUser(Session) == null)
        {
            string LastURL = string.Empty;
            if (Session["LastURL"] != null)
                LastURL = Session["LastURL"].ToString();
            Session.Abandon();
            if (!string.IsNullOrEmpty(LastURL))
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Login.aspx?LastURL=" + Convert.ToBase64String(Encoding.ASCII.GetBytes(LastURL)));
            else
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Login.aspx");
            return false;
        }
        if ((!productLicense(Session).IsValidProductKey && !IsTrialVersion(Session)))
        {
            //if (WebUtils.getLastTrialDate().AddDays(-DEFAULT_TRIALPERIOD) < AppUtils.ServerDateTime().Date)
            {
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "ProductKey.aspx");
                return false;
            }
        }
        else
        {
            if (!productLicense(Session).IsValidAuthorizationCode())
            {
                if (productLicense(Session).IsTrialPeriodExpiry())
                {
                    HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "ProductKey.aspx");
                    return false;
                }
                if (Session["TrialVersion"] == null)
                {
                    if (WebUtils.productLicense(Session).LastTrialDate.AddDays(-PRODUCTKEY_POPUPPERIOD) < AppUtils.ServerDateTime().Date)
                    {
                        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "ProductKey.aspx");
                        return false;
                    }
                }
            }
        }
        if (IsSuperUserMissing(dbConn) && (Session["IgnoreEM"] == null || ((bool)Session["IgnoreEM"]) != true))
        {
            if (function.Equals("SEC001") || function.Equals("SEC002"))
                return true;
            //else if (Session["CompanyDBID"] == null)
            //{
            //    HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/EM_Warning.aspx");
            //    return false;
            //}
        }
        if (Session["ForceChangePassword"] != null)
            if (Session["ForceChangePassword"].Equals(true))
            {
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/ChangePassword.aspx");
            }



        ////return true;
        //if (!CheckAccess(Response, Session))
        //{
        //    return false;
        //}
        EUser user = GetCurUser(Session);
        if (IsEMUser(user))
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Logout.aspx");
        if (level!=AccessLevel.None)
            if (!CheckPermission(Session, function, level))
            {
                //Response.StatusCode = 403;
                //Response.Write("Unauthorized Access");
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");
                Response.End();
                return false;
            }
        return true;
    }

    public static bool IsSuperUserMissing(DatabaseConnection dbConn)
    {
        DBFilter sec001Filter = new DBFilter();
        sec001Filter.add(new MatchField("f.FunctionID", "fg.FunctionID"));
        sec001Filter.add(new MatchField("g.UserGroupID", "fg.UserGroupID"));
        sec001Filter.add(new Match("f.FunctionCode", "SEC001"));
        sec001Filter.add(new Match("fg.FunctionAllowWrite", true));
        DBFilter sec002Filter = new DBFilter();
        sec002Filter.add(new MatchField("f.FunctionID", "fg.FunctionID"));
        sec002Filter.add(new MatchField("g.UserGroupID", "fg.UserGroupID"));
        sec002Filter.add(new Match("f.FunctionCode", "SEC002"));
        sec002Filter.add(new Match("fg.FunctionAllowWrite", true));

        DBFilter userGrpAccessSec001Filter = new DBFilter();
        DBFilter userGrpAccessSec002Filter = new DBFilter();

        userGrpAccessSec001Filter.add(new IN("UserGroupID", "Select distinct g.UserGroupID from " + ESystemFunction.db.dbclass.tableName + " f, " + EUserGroupFunction.db.dbclass.tableName + " fg, " + EUserGroup.db.dbclass.tableName + " g ", sec001Filter));
        userGrpAccessSec002Filter.add(new IN("UserGroupID", "Select distinct g.UserGroupID from " + ESystemFunction.db.dbclass.tableName + " f, " + EUserGroupFunction.db.dbclass.tableName + " fg, " + EUserGroup.db.dbclass.tableName + " g ", sec002Filter));

        DBFilter userSec001Filter = new DBFilter();
        DBFilter userSec002Filter = new DBFilter();
        userSec001Filter.add(new IN("UserID", "Select distinct UserID from " + EUserGroupAccess.db.dbclass.tableName, userGrpAccessSec001Filter));
        userSec001Filter.add(new Match("UserAccountStatus", "A"));
        userSec002Filter.add(new IN("UserID", "Select distinct UserID from " + EUserGroupAccess.db.dbclass.tableName, userGrpAccessSec002Filter));
        userSec002Filter.add(new Match("UserAccountStatus", "A"));

        int userSec001Count = EUser.db.count(dbConn, userSec001Filter);
        int userSec002Count = EUser.db.count(dbConn, userSec002Filter);

        if (userSec001Count > 0 && userSec002Count > 0)
            return false;
        else
            return true;
    }
    //public static Hashtable GetPermissions(EUser user)
    //{
    //    Hashtable permissions = new Hashtable();
    //    DBFilter filter = new DBFilter();
    //    filter.add(new Match("UserID", user.UserID));
    //    ArrayList list = EUserGroupAccess.db.select(dbConn, filter);
    //    foreach (EUserGroupAccess a in list)
    //    {
    //        filter = new DBFilter();
    //        filter.add(new Match("UserGroupID", a.UserGroupID));
    //        ArrayList functions = EUserGroupFunction.db.select(dbConn, filter);
    //        foreach (EUserGroupFunction f in functions)
    //        {
    //            ESystemFunction function = new ESystemFunction();
    //            function.FunctionID = f.FunctionID;
    //            ESystemFunction.db.select(dbConn, function);
    //            if (f.FunctionAllowWrite)
    //                permissions[function.FunctionCode] = AccessLevel.ReadWrite;
    //            else if (f.FunctionAllowRead)
    //                permissions[function.FunctionCode] = AccessLevel.Read;
    //            else
    //                permissions[function.FunctionCode] = AccessLevel.None;
    //        }
    //    }
    //    return permissions;
    //}
    //public static void RefreshPermission(HttpSessionState Session)
    //{
    //    EUser user = WebUtils.GetCurUser(Session);
    //    Hashtable permissions = GetPermissions(user);
    //    Session["Permissions"] = permissions;
    //}
    public static bool CheckPermission(HttpSessionState Session, string function, AccessLevel l)
    {
        EUser user = WebUtils.GetCurUser(Session);
        DatabaseConnection dbConn = HROne.Common.WebUtility.GetDatabaseConnection(Session);
        if (user != null)
        {
            if (WebUtils.IsEMUser(user) && (function.Equals("SEC001") || function.Equals("SEC002")))
                if (IsSuperUserMissing(dbConn))
                    return true;
                else
                    return false;

            DBFilter UserGroupfilter = new DBFilter();
            UserGroupfilter.add(new Match("UserID", user.UserID));

            DBFilter UserGroupFunctionfilter = new DBFilter();
            UserGroupFunctionfilter.add(new IN("UserGroupID", "Select UserGroupID From " + EUserGroupAccess.db.dbclass.tableName, UserGroupfilter));

            DBFilter systemFunctionFilter = new DBFilter();
            systemFunctionFilter.add(new Match("FunctionCode", function));
            UserGroupFunctionfilter.add(new IN("FunctionID", "Select FunctionID from " + ESystemFunction.db.dbclass.tableName, systemFunctionFilter));
            ArrayList functions = EUserGroupFunction.db.select(dbConn, UserGroupFunctionfilter);
            foreach (EUserGroupFunction f in functions)
            {
                if (f.FunctionAllowWrite && (l == AccessLevel.ReadWrite || l == AccessLevel.Read))
                    return true;
                else if (f.FunctionAllowRead && l == AccessLevel.Read)
                    return true;
            }
        }
        return false;
        //Hashtable permissions = (Hashtable)Session["Permissions"];
        //if (permissions.Count == 0 || !permissions.ContainsKey(function))
        //    return false;

        //AccessLevel level = (AccessLevel)permissions[function];
        //if (level == null)
        //    return false;
        //if (l == AccessLevel.ReadWrite)
        //{
        //    return level == AccessLevel.ReadWrite;
        //}
        //else if (l == AccessLevel.Read)
        //{
        //    return level != AccessLevel.None;
        //}
        //else
        //{
        //    return true;
        //}
    }

    //public static EAuditTrail PrepareAuditTrail(HttpSessionState session, string FunctionCode)
    //{
    //    return HROne.Common.AuditTrail.PrepareAuditTrail(GetCurUser(session), FunctionCode);
    //}

    public static EAuditTrail StartFunction(HttpSessionState session, string FunctionCode)
    {
        return AppUtils.StartFunction(WebUtility.GetDatabaseConnection(session), GetCurUser(session), FunctionCode, 0, true);
    }
    public static EAuditTrail StartFunction(HttpSessionState session, string FunctionCode, int EmpID)
    {
        return AppUtils.StartFunction(WebUtility.GetDatabaseConnection(session), GetCurUser(session), FunctionCode, EmpID, true);
    }

    public static EAuditTrail StartFunction(HttpSessionState session, string FunctionCode, bool LogDetail)
    {
        return AppUtils.StartFunction(WebUtility.GetDatabaseConnection(session), GetCurUser(session), FunctionCode, 0, LogDetail);
    }
    public static EAuditTrail StartFunction(HttpSessionState session, string FunctionCode, int EmpID, bool LogDetail)
    {
        return AppUtils.StartFunction(WebUtility.GetDatabaseConnection(session), GetCurUser(session), FunctionCode, EmpID, LogDetail);
    }

    public static void EndFunction(DatabaseConnection dbConn)
    {
        AppUtils.EndFunction(dbConn);
    }

    public static EUser GetCurUser(HttpSessionState Session)
    {
        DatabaseConnection dbConn = HROne.Common.WebUtility.GetDatabaseConnection(Session);
        EUser CurrentUser = null;
        if (Session["User"] == null)
        {
            if (Session["LoginID"] != null)
            {
                HROne.CommonLib.Crypto crypto = new HROne.CommonLib.Crypto(HROne.CommonLib.Crypto.SymmProvEnum.Rijndael);
                if (Session["LoginID"].ToString().Equals(crypto.Encrypting("EM", Session.SessionID)))
                {
                    CurrentUser = new EUser();
                    CurrentUser.LoginID = "EM";
                    CurrentUser.UserID = -1;
                }
                if (Session["PasswordEncrypted"] != null)
                {
                    EUser user = null;
                    if (ValidateUser(dbConn, Session["LoginID"].ToString(), Session["PasswordEncrypted"].ToString(), false, false, out user))
                        CurrentUser = user;
                    else
                    {
                        Session.Remove("LoginID");
                        Session.Remove("PasswordEncrypted");
                    }
                }
            }
            if (CurrentUser != null)
                Session["User"] = CurrentUser;
        }
        else
            CurrentUser = (EUser)Session["User"];
        return CurrentUser;
    }

    public static string GetLatestPopupMessage(HttpSessionState Session)
    {
        EUser user = GetCurUser(Session);
        if (user != null)
        {
            DatabaseConnection dbConn = HROne.Common.WebUtility.GetDatabaseConnection(Session);
            DateTime lastCheckDateTime = new DateTime();

            if (Session["LastInboxCheckDate"] != null)
                if (Session["LastInboxCheckDate"] is DateTime)
                    lastCheckDateTime = (DateTime)Session["LastInboxCheckDate"];
            EInbox.GenerateInboxMessage(dbConn, user.UserID);

            DBFilter inboxfilter = new DBFilter();
            inboxfilter.add(new Match("UserID", user.UserID));
            inboxfilter.add(new NullTerm("InboxReadDate"));
            inboxfilter.add(new NullTerm("InboxDeleteDate"));
            if (!lastCheckDateTime.Ticks.Equals(0))
                inboxfilter.add(new Match("InboxCreateDate", ">=", lastCheckDateTime));
            DateTime currentCheckDateTime = DateTime.Now;
            ArrayList list = EInbox.db.select(dbConn, inboxfilter);
            string message = string.Empty;
            if (list.Count > 0)
            {
                message = string.Empty;
                if (!lastCheckDateTime.Ticks.Equals(0))
                    message = string.Format(WebUtility.GetLocalizedStringByCode("NEW_MESSAGE_POPUP_MESSAGE", "You have {0} new message(s)"), list.Count.ToString());
                else
                    message = string.Format(WebUtility.GetLocalizedStringByCode("UNREAD_MESSAGE_POPUP_MESSAGE", "You have {0} unread message(s)"), list.Count.ToString());


            }
            Session["LastInboxCheckDate"] = currentCheckDateTime;
            return message;
        }
        else
            return string.Empty;
    }

    //public static void RefreshRanks(HttpSessionState Session)
    //{
    //    EUser user = WebUtils.GetCurUser(Session);
    //    DBFilter filter = new DBFilter();
    //    filter.add(new Match("UserID", user.UserID));
    //    ArrayList list = EUserRank.db.select(dbConn, filter);
    //    Session["Ranks"] = list;
    //}


    public static DBTerm AddRankFilter(HttpSessionState Session, string EmpID, bool isList)
    {
        EUser user = WebUtils.GetCurUser(Session);
        return AppUtils.AddRankDBTerm(user.UserID, EmpID, isList);
    }
    public static void TransmitFile(HttpResponse Response, string FilenameWithFullPath, string clientSideFileName, bool DeleteAfterTransmit)
    {
        FileInfo transmiteFileInfo = new System.IO.FileInfo(FilenameWithFullPath);
        if (transmiteFileInfo.Exists)
        {
            if (Response.IsClientConnected)
            {
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + clientSideFileName);
                Response.ContentType = "application/download";
                Response.AppendHeader("Content-Length", transmiteFileInfo.Length.ToString());
                Response.Expires = -1;
                if (DeleteAfterTransmit)
                {
                    Response.WriteFile(FilenameWithFullPath, true);
                    Response.Flush();
                    System.IO.File.Delete(FilenameWithFullPath);
                }
                else
                {
                    Response.TransmitFile(FilenameWithFullPath);
                    Response.Flush();
                }
                Response.End();
            }
            else
                transmiteFileInfo.Delete();
        }
        else
            throw new System.IO.FileNotFoundException("Internal File Not Found: " + FilenameWithFullPath, FilenameWithFullPath);
    }

    public static string GetLocalizedReportFile(string Reportfilename)
    {
        System.Globalization.CultureInfo ci = System.Threading.Thread.CurrentThread.CurrentUICulture;
        string extension = Reportfilename.Substring(Reportfilename.LastIndexOf('.'));
        string checkFileName = Reportfilename.Replace(extension, string.Empty) + "_" + ci.Name + extension;
        if (System.IO.File.Exists(checkFileName))
            return checkFileName;
        else if (System.IO.File.Exists(Reportfilename))
            return Reportfilename;
        else
            return string.Empty;
    }

    public static string ReportExportToFile(HROne.Common.GenericReportProcess reportProcess, string reportTemplateFileName, string ExportFormat, bool IsLocalize)
    {
        HROneConfig config = HROneConfig.GetCurrentHROneConfig();
        HROne.Common.GenericReportProcess.UsePDFCreator = config.UsePDFCreator;
        if (config.UsePDFCreator)
            HROne.Common.PDFCreaterPrinter.PDFCreaterPrinterName = config.PDFCreatorPrinterName;
        return reportProcess.ReportExportToFile(reportTemplateFileName, ExportFormat, IsLocalize);

        //if (reportTemplateFileName != string.Empty)
        //{
        //    CrystalDecisions.CrystalReports.Engine.ReportDocument rptDocument = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        //    rptDocument.Load(reportTemplateFileName);
        //    reportProcess.LoadReportDocument(rptDocument);
        //}




        //CrystalDecisions.CrystalReports.Engine.ReportDocument result = reportProcess.GenerateReport();

        //if (result.ParameterFields["ChineseFontName"] != null)
        //{
        //    System.Drawing.FontFamily chineseFontFamily = AppUtils.GetChineseFontFamily();
        //    if (chineseFontFamily != null)
        //        result.SetParameterValue("ChineseFontName", chineseFontFamily.Name);
        //    else
        //        result.SetParameterValue("ChineseFontName", string.Empty);
        //}

        ////        result.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.CrystalReport, System.IO.Path.GetTempPath() + @"\" + OutputFilenamePrefix + "_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss"));

        ////result.Load(System.IO.Path.GetTempPath() + @"\" + OutputFilenamePrefix + "_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss"));
        //if (reportTemplateFileName == string.Empty && IsLocalize)
        //    reportProcess.ReportSectionsLocalization(!ExportFormat.Equals("EXCEL", StringComparison.CurrentCultureIgnoreCase));

        ////  Do NOT set the papersize by default for customized paper size
        ////result.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;

        //CrystalDecisions.Shared.ExportFormatType exportFormatType;
        //string exportFileNameExtension = string.Empty;
        //if (ExportFormat.Equals("EXCEL", StringComparison.CurrentCultureIgnoreCase))
        //{
        //    result.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperEsheet;


        //    //ExcelLibrary.SpreadSheet.Workbook workbook = ExcelLibrary.SpreadSheet.Workbook.Load(result.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel));
        //    //foreach (ExcelLibrary.SpreadSheet.Worksheet workSheet in workbook.Worksheets)
        //    //{
        //    //    ExcelLibrary.SpreadSheet.CellCollection cellCollection = new ExcelLibrary.SpreadSheet.CellCollection();
        //    //    int count = 1;
        //    //    foreach (int key in workSheet.Cells.Rows.Keys)
        //    //    {
        //    //        ExcelLibrary.SpreadSheet.Row row = workSheet.Cells.Rows[key];
        //    //        cellCollection.Rows.Add(count, row);
        //    //        count++;
        //    //    }
        //    //    workSheet.Cells.Rows.Clear();

        //    //    foreach (int key in cellCollection.Rows.Keys)
        //    //    {
        //    //        workSheet.Cells.Rows.Add(key, cellCollection.Rows[key]);
        //    //    }
        //    //}
        //    //string exportFileName = System.IO.Path.GetTempFileName();
        //    //System.IO.File.Delete(exportFileName);
        //    //exportFileName += ".xls";
        //    //workbook.Save(exportFileName);
        //    //WebUtils.TransmitFile(Response, exportFileName, OutputFilenamePrefix + "_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true);

        //    //result.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.Excel, Response, true, OutputFilenamePrefix + "_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss"));

        //    exportFileNameExtension = ".xls";
        //    exportFormatType = CrystalDecisions.Shared.ExportFormatType.Excel;

        //}
        //else if (ExportFormat.Equals("WORD", StringComparison.CurrentCultureIgnoreCase))
        //{
        //    //result.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.WordForWindows, Response, true, OutputFilenamePrefix + "_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss"));
        //    exportFileNameExtension = ".doc";
        //    exportFormatType = CrystalDecisions.Shared.ExportFormatType.WordForWindows;
        //}
        //else if (ExportFormat.Equals("PDF", StringComparison.CurrentCultureIgnoreCase))
        //{
        //    //result.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, OutputFilenamePrefix + "_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss"));

        //    exportFileNameExtension = ".pdf";
        //    exportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat;
        //}
        //else
        //{
        //    //result.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, false, OutputFilenamePrefix + "_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss"));
        //    exportFileNameExtension = ".pdf";
        //    exportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat;
        //}
        //string exportFileName = System.IO.Path.GetTempFileName();
        //System.IO.File.Delete(exportFileName);
        //exportFileName += exportFileNameExtension;

        //result.ExportToDisk(exportFormatType, exportFileName);
        //result.Close();
        //result.Dispose();
        //reportProcess.LoadReportDocument(null);
        //return exportFileName;

    }
    public static void ReportExport(HttpResponse Response, HROne.Common.GenericReportProcess reportProcess, string reportTemplateFileName, string ExportFormat, string OutputFilenamePrefix)
    {
        ReportExport(Response, reportProcess, reportTemplateFileName, ExportFormat, OutputFilenamePrefix, true);
    }
    public static void ReportExport(HttpResponse Response, HROne.Common.GenericReportProcess reportProcess, string reportTemplateFileName, string ExportFormat, string OutputFilenamePrefix, bool IsLocalize)
    {
        if (Response.IsClientConnected)
        {
            string exportFileName = ReportExportToFile(reportProcess, reportTemplateFileName, ExportFormat, IsLocalize);
            string exportFileNameExtension = exportFileName.Substring(exportFileName.LastIndexOf("."));
            WebUtils.TransmitFile(Response, exportFileName, OutputFilenamePrefix + "_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + exportFileNameExtension, true);
        }
    }
    public static void ReportExport(DatabaseConnection dbConn, EUser user, PageErrors errors, string ReportName, HttpResponse Response, HROne.Common.GenericReportProcess reportProcess, string reportTemplateFileName, string ExportFormat, string OutputFilenamePrefix, bool IsLocalize)
    {
        if (Response.IsClientConnected)
        {
            HROneConfig config = HROneConfig.GetCurrentHROneConfig();
            if (config.GenerateReportAsInbox)
            {
                HROne.Common.GenericReportProcess.UsePDFCreator = config.UsePDFCreator;
                if (config.UsePDFCreator)
                    HROne.Common.PDFCreaterPrinter.PDFCreaterPrinterName = config.PDFCreatorPrinterName;

                ProductLicense license= new ProductLicense();
                license.LoadProductLicense(dbConn);
                if (EInboxAttachment.GetTotalSize(dbConn, 0) < license.MaxInboxSizeMB * 1000 * 1000)
                {
                    HROne.TaskService.GenericReportTaskFactory reportTask = new HROne.TaskService.GenericReportTaskFactory(dbConn, user, ReportName, reportProcess, reportTemplateFileName, ExportFormat, OutputFilenamePrefix, IsLocalize);
                    AppUtils.reportTaskQueueService.AddTask(reportTask);
                    errors.addError(HROne.Translation.PageMessage.REPORT_GENERATING_TO_INBOX);
                }
                else
                    errors.addError(HROne.Translation.PageMessage.INBOX_SIZE_EXCEEDED);
            }
            else
            {
                string exportFileName = ReportExportToFile(reportProcess, reportTemplateFileName, ExportFormat, IsLocalize);
                string exportFileNameExtension = exportFileName.Substring(exportFileName.LastIndexOf("."));

                string outputFileName = OutputFilenamePrefix + "_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + exportFileNameExtension;
                WebUtils.TransmitFile(Response, exportFileName, outputFileName, true);
            }
        }
    }

    [Obsolete("Still have unsolved issue on IE8 security warning")]
    public static void RegisterReportExport(Control ctrl, HROne.Common.GenericReportProcess reportProcess, string reportTemplateFileName, string ExportFormat, string OutputFilenamePrefix, bool IsLocalize)
    {
        if (ctrl.Page.Response.IsClientConnected)
        {
            string exportFileName = ReportExportToFile(reportProcess, reportTemplateFileName, ExportFormat, IsLocalize);
            string exportFileNameExtension = exportFileName.Substring(exportFileName.LastIndexOf("."));
            WebUtils.RegisterDownloadFileJavaScript(ctrl, exportFileName, OutputFilenamePrefix + "_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + exportFileNameExtension, true, 0);
        }
    }
    public static bool IsEMUser(EUser user)
    {
        if (user == null)
            return false;
        else
            return user.UserID.Equals(-1) && user.LoginID.Equals("EM");
    }


    protected static string CreateDownloadFileURL(string path, string ActualFilename, bool RemoveAfterDownload, string SessionID)
    {
        DBAESEncryptStringFieldAttribute encrypt = null;
        if (string.IsNullOrEmpty(SessionID))
            encrypt = new DBAESEncryptStringFieldAttribute();
        else
            encrypt = new DBAESEncryptStringFieldAttribute(SessionID);
        string encryptPath = encrypt.toDB(path).ToString();
        string encryptFilename = encrypt.toDB(ActualFilename).ToString();
        string destinationURL = "./FileDownload.aspx?path=" + System.Web.HttpUtility.UrlEncode(encryptPath) + "&name=" + System.Web.HttpUtility.UrlEncode(encryptFilename) + "&RemoveAfterDownload=" + (RemoveAfterDownload ? "Y" : "N");
        return destinationURL;
    }
    [Obsolete("Still have unsolved issue on IE8 security warning")]
    public static void RegisterDownloadFileJavaScript(Control ctrl, string path, string ActualFilename, bool RemoveAfterDownload, int delayMillSecond)
    {
        //  Generate Download File 1 time only
        if (RemoveAfterDownload)
        {
            System.IO.FileInfo fileInfo = new FileInfo(path);
            fileInfo.MoveTo(System.IO.Path.Combine(HROne.Common.Folder.GetOrCreateSessionTempFolder(ctrl.Page.Session.SessionID).FullName, fileInfo.Name));
            path = fileInfo.FullName;
        }

        string url = CreateDownloadFileURL(path, ActualFilename, RemoveAfterDownload, ctrl.Page.Session.SessionID);
        if (ctrl.Page.Response.IsClientConnected)
        {
            RegisterRedirectJavaScript(ctrl, url, delayMillSecond);
            //string javascriptString = "initDownloadDialog('" + "Download here" + "','" + url + "');";
            //ScriptManager.RegisterStartupScript(ctrl, ctrl.GetType(), "download", javascriptString, true);
        }
        else if (RemoveAfterDownload)
            System.IO.File.Delete(path);

    }
    public static void RegisterRedirectJavaScript(Control ctrl, string url, int delayMillSecond)
    {
        url = HROne.Common.WebUtility.URLwithEncryptQueryString(ctrl.Page.Session, url);
        string javascriptString = "window.open('" + url + "','_self');";
        if (delayMillSecond < 100)
            delayMillSecond = 100;
        if (delayMillSecond > 0)
        {
            javascriptString = "setTimeout(\"" + javascriptString + "\"," + delayMillSecond.ToString() + ");";
        }
        ScriptManager.RegisterStartupScript(ctrl, ctrl.GetType(), "redirect", javascriptString, true);
    }
    public static void RegisterRedirectJavaScript(Control ctrl, string url, string alertMessage)
    {
        url = HROne.Common.WebUtility.URLwithEncryptQueryString(ctrl.Page.Session, url);
        string javascriptString = "window.open('" + url + "','_self');";
        if (!string.IsNullOrEmpty(alertMessage))
        {
            alertMessage = alertMessage.Replace("\\", "\\\\").Replace("\r", "\\r").Replace("\n", "\\n").Replace("\"", "\\\"");
            //javascriptString = "alert(\"" + alertMessage + "\"); " + javascriptString + "";
            javascriptString = "messagePopupPostBackScript=\"" + javascriptString + "\"; messagePopupDetail=\"" + alertMessage + "\";";
        }
        ScriptManager.RegisterStartupScript(ctrl, ctrl.GetType(), "redirect", javascriptString, true);
    }

    public static bool ValidateUser(DatabaseConnection dbConn, string username, string encryptedPassword, bool throwException, bool CheckFailCount, out EUser user)
    {
        string message = string.Empty;
        user = null;

        if (dbConn == null)
        {
            if (throwException)
            {
                message = HROne.Common.WebUtility.GetLocalizedString("Invalid User Name or Password");
                throw new Exception(message);
            }
            return false;
        }
        DBFilter filter = new DBFilter();
        filter.add(new Match("LoginID", username));
        filter.add(new Match("UserAccountStatus", "<>", "D"));
        ArrayList list = EUser.db.select(dbConn, filter);
        if (list.Count == 0)
        {
            if (throwException)
            {
                message = HROne.Common.WebUtility.GetLocalizedString("Invalid User Name or Password");
                throw new Exception(message);
            }
            return false;
        }
        user = (EUser)list[0];
        if (user.UserPassword == null)
            user.UserPassword = "";
        if (!(user.UserAccountStatus == "A"))
        {
            if (throwException)
            {
                message = HROne.Common.WebUtility.GetLocalizedString("Account is Inactive/Locked");
                throw new Exception(message);
            }
            return false;
        }

        if (!user.UserPassword.Equals(encryptedPassword))
        {
            message = HROne.Common.WebUtility.GetLocalizedString("Invalid User Name or Password");
            if (CheckFailCount)
            {
                user.FailCount++;
                string maxFailCountParameterString = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_LOGIN_MAX_FAIL_COUNT);
                if (!maxFailCountParameterString.Equals(string.Empty))
                {
                    int MaxFailCount = 0;
                    if (int.TryParse(maxFailCountParameterString, out MaxFailCount))
                        if (MaxFailCount > 0)
                            if (user.FailCount >= MaxFailCount)
                            {
                                user.UserAccountStatus = "I";
                                user.FailCount = 0;
                                message += "\r\n" + HROne.Common.WebUtility.GetLocalizedString("Account is Locked");
                            }
                            else if (MaxFailCount - user.FailCount == 1)
                            {
                                message += "\r\n" + HROne.Common.WebUtility.GetLocalizedString("The account will be locked if you fail to login 1 more time");
                            }


                }
                EUser.db.update(dbConn, user);
            }
            if (throwException)
            {
                throw new Exception(message);
            }
            return false;
        }

        if (CheckFailCount)
        {
            user.FailCount = 0;
            EUser.db.update(dbConn, user);
        }
        return true;

    }

    //protected static string GetDefaultApplicationTempFolder()
    //{
    //    string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

    //    while (path.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
    //        path = path.Substring(0, path.Length - 1);
    //    string applicationName = path.Substring(path.LastIndexOf(System.IO.Path.DirectorySeparatorChar) + 1);
    //    //applicationName = "~" + applicationName + "_Temp"; 
    //    return System.IO.Path.Combine(System.IO.Path.GetTempPath(), applicationName);
    //}

    [Obsolete("replaced by HROne.Folder.ClearApplicationTempFolder()")]
    public static void ClearApplicationTempFolder()
    {
        HROne.Common.Folder.ClearApplicationTempFolder();
    }
    [Obsolete("replaced by HROne.Folder.GetOrCreateSessionTempFolder(string SessionID)")]
    public static DirectoryInfo GetOrCreateSessionTempFolder(string SessionID)
    {
        return HROne.Common.Folder.GetOrCreateSessionTempFolder(SessionID);
    }
    [Obsolete("replaced by HROne.Folder.GetOrCreateApplicationTempFolder()")]
    public static DirectoryInfo GetOrCreateApplicationTempFolder()
    {
        return HROne.Common.Folder.GetOrCreateApplicationTempFolder();

    }

    public static void SetEnabledControlSection(Control ctrl, bool isEnabled)
    {
        foreach (Control obj in ctrl.Controls)
        {
            if (obj is WebControl)
                ((WebControl)obj).Enabled = isEnabled;
            if (obj.HasControls())
                SetEnabledControlSection(obj, isEnabled);
        }
    }
    private static DateTime getDateFromKey(string trialKey)
    {
        HROne.CommonLib.Crypto crypto = new HROne.CommonLib.Crypto(HROne.CommonLib.Crypto.SymmProvEnum.DES);
        try
        {
            trialKey = HROne.CommonLib.base32.ConvertBase32ToBase64(trialKey);

            string realTrialKey = crypto.Decrypting(trialKey, "HROne");
            string strYear = realTrialKey.Substring(0, 4);
            string strMonth = realTrialKey.Substring(4, 2);
            string strDay = realTrialKey.Substring(6, 2);

            return new DateTime(int.Parse(strYear), int.Parse(strMonth), int.Parse(strDay));

        }
        catch
        {
            return new DateTime();
        }
    }
    public static void Logout(HttpSessionState Session)
    {
        if (Session != null)
        {
            DatabaseConnection dbConn = HROne.Common.WebUtility.GetDatabaseConnection(Session);
            if (dbConn != null)
            {
                ClearTempTable(dbConn, Session.SessionID);
            }
            Session.Clear();
            Session.Abandon();
            //dbConn = WebUtils.GetDatabaseConnection();
            //if (dbConn != null)
            //{
            //    WebUtils.SetSessionDatabaseConnection(Session, dbConn);
            //}

        }
    }

    // Start 0000060, Miranda, 2014-07-22
    public static void AddYearMonthDayOptionstoDropDownList(DropDownList dropDownList)
    {
        dropDownList.Items.Add(new ListItem("Year", "Y"));
        dropDownList.Items.Add(new ListItem("Month", "M"));
        dropDownList.Items.Add(new ListItem("Day", "D"));
    }
    // End 0000060, Miranda, 2014-07-22
}
