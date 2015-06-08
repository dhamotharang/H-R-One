using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using HROne.DataAccess;
//using perspectivemind.validation;
using HROne.Lib.Entities;
using HROne.Payroll;
using HROne.MPFFile;


public partial class Payroll_GenerateMPFFile_List : HROneWebPage
{
    private const string FUNCTION_CODE = "PAY011";
    protected SearchBinding empSBinding, sbinding;
    public DBManager db = EPayrollGroup.db;
    public EPayrollGroup obj;

    private bool IsAllowEdit = true;

    protected ListInfo empInfo;
    protected DataView empView;

    private MPFFileControlInterface mpfFileControl;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        if (!WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
        {
            EmployeeSelectAllPanel.Visible = false;
            IsAllowEdit = false;
        }

        empSBinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);
        empSBinding.add(new DropDownVLSearchBinder(MPFPlanID, "MPFPlanID",EMPFPlan.VLMPFPlan));
        empSBinding.initValues("EmpStatus", null, EEmpPersonalInfo.VLEmpStatus, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        empSBinding.init(DecryptedRequest, null);

        empInfo = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        mpfFileControl = getMPFFileControl();

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {

            empView = emploadData(empInfo, EEmpPayroll.db, empRepeater);
        }

    }


    public DataView emploadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DateTime dtPayPeriodFr, dtPayPeriodTo;
        if (DateTime.TryParse(PayPeriodFr.Value, out dtPayPeriodFr) && DateTime.TryParse(PayPeriodTo.Value, out dtPayPeriodTo))
        {
            DBFilter filter = new DBFilter();// empSBinding.createFilter();

            //if (info != null && info.orderby != null && !info.orderby.Equals(""))
            //    filter.add(info.orderby, info.order);

            string select = "* ";
            string from = "from [" + EEmpPersonalInfo.db.dbclass.tableName + "] e ";

            DBFilter empPayrollFilter = new DBFilter();
            empPayrollFilter.add(new MatchField("e.EmpID", "ep.EmpID"));
            empPayrollFilter.add(new Match("ep.EmpPayStatus", "C"));
            //        empPayrollFilter.add(new Match("ep.PayPeriodID", CurPayPeriodID));


            DBFilter payPeriodFilter = new DBFilter();

            // refer to Date To of payperiod
            payPeriodFilter.add(new Match("pp.PayPeriodTo", "<=", dtPayPeriodTo));
            payPeriodFilter.add(new Match("pp.PayPeriodTo", ">=", dtPayPeriodFr));
            empPayrollFilter.add(new IN("PayperiodID ", "Select payperiodID from PayrollPeriod pp", payPeriodFilter));

            DBFilter mpfRecordFilter = new DBFilter();
            mpfRecordFilter.add(new Match("MPFPlanID", MPFPlanID.SelectedValue));
            empPayrollFilter.add(new IN("EmpPayrollID", "Select EmpPayrollID from MPFRecord", mpfRecordFilter));

            Exists exists = new Exists(EEmpPayroll.db.dbclass.tableName + " ep", empPayrollFilter);

            filter.add(exists);
            filter.add(WebUtils.AddRankFilter(Session, "e.EmpID", true));

            DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);


            if (table.Rows.Count != 0)
            {
                btnGenerate.Visible = true & IsAllowEdit;
            }
            else
            {
                btnGenerate.Visible = false;
            }
            empView = new DataView(table);
            //if (info != null)
            //{
            //    info.loadPageList(null, empPrevPage, empNextPage, empFirstPage, empLastPage);

            //    CurPage.Value = info.page.ToString();
            //    NumPage.Value = info.numPage.ToString();
            //}
            if (repeater != null)
            {
                repeater.DataSource = empView;
                repeater.DataBind();
            }
            panelEmployeeList.Visible = true;
        }
        else
        {
            empView = new DataView();
            panelEmployeeList.Visible = false;
        }
        return empView;
            
    }
    protected void empChangeOrder_Click(object sender, EventArgs e)
    {
        LinkButton l = (LinkButton)sender;
        String id = l.ID.Substring(1);
        if (empInfo.orderby == null)
            empInfo.order = true;
        else if (empInfo.orderby.Equals(id))
            empInfo.order = !empInfo.order;
        else
            empInfo.order = true;
        empInfo.orderby = id;

        empView = emploadData(empInfo, EEmpPayroll.db, empRepeater);

    }
    protected void empRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(EEmpPersonalInfo.db, row, cb);
        cb.Visible = IsAllowEdit;

    }

    protected void Search_Click(object sender, EventArgs e)
    {
        empInfo.page = 0;
        empView = emploadData(empInfo, db, empRepeater);

    }
    protected void Reset_Click(object sender, EventArgs e)
    {
        empSBinding.clear();
        empInfo.page = 0;
        empView = emploadData(empInfo, db, empRepeater);
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ArrayList empList = new ArrayList();

        foreach (RepeaterItem i in empRepeater.Items)
        {
            CheckBox cb = (CheckBox)i.FindControl("ItemSelect");
            if (cb.Checked)
            {
                EEmpPersonalInfo o = (EEmpPersonalInfo)EEmpPersonalInfo.db.createObject();
                WebFormUtils.GetKeys(EEmpPersonalInfo.db, o, cb);
                empList.Add(o);
            }

        }


        string strEmpList = string.Empty;
        string strPayBatchList = string.Empty;
        string strPayPeriodRequest = string.Empty;
        DateTime dtPayPeriodFr=new DateTime();
        DateTime dtPayPeriodTo=new DateTime();

        if (empList.Count > 0)
        {

            foreach (EEmpPersonalInfo o in empList)
            {
                if (strEmpList == string.Empty)
                    strEmpList = ((EEmpPersonalInfo)o).EmpID.ToString();
                else
                    strEmpList += "_" + ((EEmpPersonalInfo)o).EmpID.ToString();

            }
            if (DateTime.TryParse(PayPeriodFr.Value, out dtPayPeriodFr) && DateTime.TryParse(PayPeriodTo.Value, out dtPayPeriodTo))
                strPayPeriodRequest = "&PayPeriodFr=" + dtPayPeriodFr.Ticks + "&PayPeriodTo=" + dtPayPeriodTo.Ticks;
            else
                errors.addError("Invalid Date Format"); 
            

            //            errors.addError("Complete");
            //Response.Write("<script>alert('Completed'); </script>");
        }
        else
            errors.addError("Employee or Payroll Batch not selected");

        if (errors.isEmpty())
        {
            GenericMPFFile mpfFileProcess = getMPFFileObject();
            if (mpfFileProcess == null)
            {
                errors.addError("Generate MPF File is not available for that MPF Scheme");
                return;
            }
            if (mpfFileProcess is HSBCMPFGatewayFileEncrypted)
            {
                string keyFolderPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BankKey");

                string keyPath = string.Empty;
                string HSBCKeyFileName = "HSBC.pub";
                string HangSengKeyFileName = "HASE.pub";
                string password = "11111111";

                int intMPFPlanID = 0;

                if (int.TryParse(MPFPlanID.SelectedValue, out intMPFPlanID))
                {
                    EMPFPlan mpfPlan = new EMPFPlan();
                    mpfPlan.MPFPlanID = intMPFPlanID;
                    if (EMPFPlan.db.select(dbConn, mpfPlan))
                    {
                        EMPFScheme mpfScheme = new EMPFScheme();
                        mpfScheme.MPFSchemeID = mpfPlan.MPFSchemeID;
                        if (EMPFScheme.db.select(dbConn, mpfScheme))
                        {
                            if (mpfScheme.MPFSchemeTrusteeCode.Equals("HSBC"))
                            {
                                keyPath = System.IO.Path.Combine(keyFolderPath, HSBCKeyFileName);
                                if (Application["MasterDBConfig"] != null)
                                {
                                    DatabaseConnection masterDBConn = ((DatabaseConfig)Application["MasterDBConfig"]).CreateDatabaseConnectionObject();
                                    string HSBCKeyPath = HROne.SaaS.Entities.ESystemParameter.getParameter(masterDBConn, HROne.SaaS.Entities.ESystemParameter.PARAM_CODE_BANKKEY_HSBC_PATH);
                                    string HSBCPassword = HROne.SaaS.Entities.ESystemParameter.getParameterWithEncryption(masterDBConn, HROne.SaaS.Entities.ESystemParameter.PARAM_CODE_BANKKEY_HSBC_PASSWORD);
                                    if (!string.IsNullOrEmpty(HSBCKeyPath) && !string.IsNullOrEmpty(HSBCPassword))
                                    {
                                        keyPath = HSBCKeyPath;
                                        password = HSBCPassword;
                                    }
                                }
                            }
                            else if (mpfScheme.MPFSchemeTrusteeCode.Equals("HangSeng"))
                            {
                                keyPath = System.IO.Path.Combine(keyFolderPath, HangSengKeyFileName);
                                if (Application["MasterDBConfig"] != null)
                                {
                                    DatabaseConnection masterDBConn = ((DatabaseConfig)Application["MasterDBConfig"]).CreateDatabaseConnectionObject();
                                    string HASEKeyPath = HROne.SaaS.Entities.ESystemParameter.getParameter(masterDBConn, HROne.SaaS.Entities.ESystemParameter.PARAM_CODE_BANKKEY_HASE_PATH);
                                    string HASEPassword = HROne.SaaS.Entities.ESystemParameter.getParameterWithEncryption(masterDBConn, HROne.SaaS.Entities.ESystemParameter.PARAM_CODE_BANKKEY_HASE_PASSWORD);
                                    if (!string.IsNullOrEmpty(HASEKeyPath) && !string.IsNullOrEmpty(HASEPassword))
                                    {
                                        keyPath = HASEKeyPath;
                                        password = HASEPassword;
                                    }
                                }
                            }
                        }
                    }
                }

                HSBCMPFGatewayFileEncrypted mpfFileEncrypted = (HSBCMPFGatewayFileEncrypted)mpfFileProcess;

                mpfFileEncrypted.publicKeyFile = keyPath;
                mpfFileEncrypted.publicKeyPassword = password;
            }

            HROneConfig config = HROneConfig.GetCurrentHROneConfig();
            if (config.GenerateReportAsInbox)
            {
                if (EInboxAttachment.GetTotalSize(dbConn, 0) < WebUtils.productLicense(Session).MaxInboxSizeMB * 1000 * 1000)
                {
                    HROne.TaskService.GenerateMPFContributionFileTaskFactory reportTask = new HROne.TaskService.GenerateMPFContributionFileTaskFactory(dbConn, user, lblReportHeader.Text, mpfFileProcess, empList, int.Parse(MPFPlanID.SelectedValue), dtPayPeriodFr, dtPayPeriodTo, ci);
                    AppUtils.reportTaskQueueService.AddTask(reportTask);
                    errors.addError(HROne.Translation.PageMessage.REPORT_GENERATING_TO_INBOX);
                }
                else
                    errors.addError(HROne.Translation.PageMessage.INBOX_SIZE_EXCEEDED);
            }
            else
            {
                try
                {
                    mpfFileProcess.LoadMPFFileDetail(empList, int.Parse(MPFPlanID.SelectedValue), dtPayPeriodFr, dtPayPeriodTo);
                    FileInfo mpfFileInfo = mpfFileProcess.GenerateMPFFile();

                    string mpfFilename = mpfFileProcess.ActualMPFFileName();
                    WebUtils.TransmitFile(Response, mpfFileInfo.FullName, mpfFilename, true);
                }
                catch (Exception ex)
                {
                    errors.addError(ex.Message);
                }
            }
        }
//        emploadData(empInfo, EEmpPayroll.db, empRepeater);
    }

    protected void btnRefreshEmpList_Click(object sender, EventArgs e)
    {

    }
    private MPFFileControlInterface getMPFFileControl()
    {
        HROne.ProductLicense license = WebUtils.productLicense(Session);

        MPFFileControlInterface tmpMPFFileControl = null;

        //btnSubmit.Visible = false;
        string strMPDPlanID = MPFPlanID.SelectedValue;
        int intMPFPlanID=0;

        if (int.TryParse(strMPDPlanID, out intMPFPlanID))
        {
            EMPFPlan mpfPlan = new EMPFPlan();
            mpfPlan.MPFPlanID = intMPFPlanID;
            if (EMPFPlan.db.select(dbConn, mpfPlan))
            {
                EMPFScheme mpfScheme = new EMPFScheme();
                mpfScheme.MPFSchemeID = mpfPlan.MPFSchemeID;
                if (EMPFScheme.db.select(dbConn, mpfScheme))
                {
                    if (mpfScheme.MPFSchemeTrusteeCode.Equals("HSBC") || mpfScheme.MPFSchemeTrusteeCode.Equals("HangSeng"))
                    {
                        Payroll_GenerateMPFFile_HSBCControl.Visible = true;
                        Payroll_GenerateMPFFile_BOCIControl.Visible = false;
                        Payroll_GenerateMPFFile_ManulifeControl.Visible = false;
                        Payroll_GenerateMPFFile_AIAControl.Visible = false;
                        Payroll_GenerateMPFFile_HSBCOISControl.Visible = false;
                        tmpMPFFileControl = Payroll_GenerateMPFFile_HSBCControl;
                        Payroll_GenerateMPFFile_HSBCControl.BankCode = mpfScheme.MPFSchemeTrusteeCode;
                        //if (Payroll_GenerateMPFFile_HSBCControl.GetSelectedFileTypeValue().Equals("AMPFF") && Session["CompanyDBID"] != null)
                        //    btnSubmit.Visible = true;
                    }
                    else if (mpfScheme.MPFSchemeTrusteeCode.Equals("BOCI") && (Session["CompanyDBID"] == null || license.HasAutopayMPFFileOthers))
                    {
                        Payroll_GenerateMPFFile_HSBCControl.Visible = false;
                        Payroll_GenerateMPFFile_BOCIControl.Visible = true;
                        Payroll_GenerateMPFFile_ManulifeControl.Visible = false;
                        Payroll_GenerateMPFFile_AIAControl.Visible = false;
                        Payroll_GenerateMPFFile_HSBCOISControl.Visible = false;
                        tmpMPFFileControl = Payroll_GenerateMPFFile_BOCIControl;
                    }
                    else if (mpfScheme.MPFSchemeTrusteeCode.Equals("Manulife") && (Session["CompanyDBID"] == null || license.HasAutopayMPFFileOthers))
                    {
                        Payroll_GenerateMPFFile_HSBCControl.Visible = false;
                        Payroll_GenerateMPFFile_BOCIControl.Visible = false;
                        Payroll_GenerateMPFFile_ManulifeControl.Visible = true;
                        Payroll_GenerateMPFFile_AIAControl.Visible = false;
                        Payroll_GenerateMPFFile_HSBCOISControl.Visible = false;
                        tmpMPFFileControl = Payroll_GenerateMPFFile_ManulifeControl;
                    }
                    else if (mpfScheme.MPFSchemeTrusteeCode.Equals("AIA") && (Session["CompanyDBID"] == null || license.HasAutopayMPFFileOthers))
                    {
                        Payroll_GenerateMPFFile_HSBCControl.Visible = false;
                        Payroll_GenerateMPFFile_BOCIControl.Visible = false;
                        Payroll_GenerateMPFFile_ManulifeControl.Visible = false;
                        Payroll_GenerateMPFFile_AIAControl.Visible = true;
                        Payroll_GenerateMPFFile_HSBCOISControl.Visible = false;
                        tmpMPFFileControl = Payroll_GenerateMPFFile_AIAControl;
                    }
                    else if (mpfScheme.MPFSchemeCode.Equals("MT00288") && (Session["CompanyDBID"] == null || license.HasAutopayMPFFileOthers))
                    {
                        // Fidelity is using HSBC's Open-Interface-System
                        Payroll_GenerateMPFFile_HSBCControl.Visible = false;
                        Payroll_GenerateMPFFile_BOCIControl.Visible = false;
                        Payroll_GenerateMPFFile_ManulifeControl.Visible = false;
                        Payroll_GenerateMPFFile_AIAControl.Visible = false;
                        Payroll_GenerateMPFFile_HSBCOISControl.Visible = true;
                        tmpMPFFileControl = Payroll_GenerateMPFFile_HSBCOISControl; 
                    }
                    else
                    {
                        Payroll_GenerateMPFFile_HSBCControl.Visible = false;
                        Payroll_GenerateMPFFile_BOCIControl.Visible = false;
                        Payroll_GenerateMPFFile_ManulifeControl.Visible = false;
                        Payroll_GenerateMPFFile_AIAControl.Visible = false;
                        Payroll_GenerateMPFFile_HSBCOISControl.Visible = false;
                    }
                    return tmpMPFFileControl;
                }
            }
        }
        Payroll_GenerateMPFFile_HSBCControl.Visible = false;
        Payroll_GenerateMPFFile_BOCIControl.Visible = false;
        Payroll_GenerateMPFFile_ManulifeControl.Visible = false;
        Payroll_GenerateMPFFile_AIAControl.Visible = false;
        return null;
    }

    private GenericMPFFile getMPFFileObject()
    {
        GenericMPFFile mpfFileObject = mpfFileControl.CreateMPFFileObject();

        //else
        //    mpfFileObject = new GenericMPFFile(dbConn);
        //if (MPFFileType.SelectedValue.Equals("HSBC"))
        //    bankFileObject = bankFileControlCreateMPFFileObject();
        //if (MPFFileType.SelectedValue.Equals("SCB"))
        //    bankFileObject = Payroll_GenerateMPFFile_SCBControl.CreateMPFFileObject();
        //if (MPFFileType.SelectedValue.Equals("CitiBank"))
        //    bankFileObject = Payroll_GenerateMPFFile_CitiBankControl.CreateMPFFileObject();
        //if (bankFileObject == null)
        //    bankFileObject=new GenericMPFFile();
        return mpfFileObject;
    }
    protected void MPFPlanID_SelectedIndexChanged(object sender, EventArgs e)
    {
        empView = emploadData(empInfo, db, empRepeater);

    }
    protected void PayPeriod_Changed(object sender, EventArgs e)
    {
        // Start 0000004, Miranda, 2014-06-19
        SetPayPeriod_AutoValue(sender, e);
        // End 0000004, Miranda, 2014-06-19
        empView = emploadData(empInfo, db, empRepeater);
    }

    // Start 0000004, Miranda, 2014-06-19
    private void SetPayPeriod_AutoValue(object sender, EventArgs e)
    {
        DateTime dtPeriodFr = new DateTime(), dtPeriodTo = new DateTime();
        DateTime.TryParse(PayPeriodFr.Value, out dtPeriodFr);
        DateTime.TryParse(PayPeriodTo.Value, out dtPeriodTo);
        if (!dtPeriodFr.Ticks.Equals(0) && !dtPeriodTo.Ticks.Equals(0))
        {
            if (dtPeriodFr >= dtPeriodTo)
            {
                if (sender == PayPeriodFr.TextBox)
                    PayPeriodTo.Value = dtPeriodFr.AddMonths(DefaultMonthPeriod).AddDays(-1).ToString("yyyy-MM-dd");
                else if (sender == PayPeriodTo.TextBox)
                    PayPeriodFr.Value = dtPeriodTo.AddDays(1).AddMonths(-DefaultMonthPeriod).ToString("yyyy-MM-dd");
            }
        }
        else
        {
            if (sender == PayPeriodFr.TextBox && !dtPeriodFr.Ticks.Equals(0) && dtPeriodTo.Ticks.Equals(0))
            {
                PayPeriodTo.Value = dtPeriodFr.AddMonths(DefaultMonthPeriod).AddDays(-1).ToString("yyyy-MM-dd");

            }
            else if (sender == PayPeriodTo.TextBox && !dtPeriodTo.Ticks.Equals(0) && dtPeriodFr.Ticks.Equals(0))
                PayPeriodFr.Value = dtPeriodTo.AddDays(1).AddMonths(-DefaultMonthPeriod).ToString("yyyy-MM-dd");
        }
    }

    public int DefaultMonthPeriod
    {
        set { hiddenFieldDefaultMonthPeriod.Value = value.ToString(); }
        get { return int.Parse(hiddenFieldDefaultMonthPeriod.Value); }
    }
    // End 0000004, Miranda, 2014-06-19

    //protected void btnSubmit_Click(object sender, EventArgs e)
    //{
    //    PageErrors errors = PageErrors.getErrors(db, Page.Master);
    //    errors.clear();

    //    ArrayList empList = new ArrayList();

    //    foreach (RepeaterItem i in empRepeater.Items)
    //    {
    //        CheckBox cb = (CheckBox)i.FindControl("ItemSelect");
    //        if (cb.Checked)
    //        {
    //            EEmpPersonalInfo o = (EEmpPersonalInfo)EEmpPersonalInfo.db.createObject();
    //            WebFormUtils.GetKeys(EEmpPersonalInfo.db, o, cb);
    //            empList.Add(o);
    //        }

    //    }


    //    string strEmpList = string.Empty;
    //    string strPayBatchList = string.Empty;
    //    string strPayPeriodRequest = string.Empty;
    //    DateTime dtPayPeriodFr = new DateTime();
    //    DateTime dtPayPeriodTo = new DateTime();

    //    if (empList.Count > 0)
    //    {

    //        foreach (EEmpPersonalInfo o in empList)
    //        {
    //            if (strEmpList == string.Empty)
    //                strEmpList = ((EEmpPersonalInfo)o).EmpID.ToString();
    //            else
    //                strEmpList += "_" + ((EEmpPersonalInfo)o).EmpID.ToString();

    //        }
    //        if (DateTime.TryParse(PayPeriodFr.Value, out dtPayPeriodFr) && DateTime.TryParse(PayPeriodTo.Value, out dtPayPeriodTo))
    //            strPayPeriodRequest = "&PayPeriodFr=" + dtPayPeriodFr.Ticks + "&PayPeriodTo=" + dtPayPeriodTo.Ticks;
    //        else
    //            errors.addError("Invalid Date Format");


    //        //            errors.addError("Complete");
    //        //Response.Write("<script>alert('Completed'); </script>");
    //    }
    //    else
    //        errors.addError("Employee or Payroll Batch not selected");

    //    if (errors.isEmpty())
    //    {
    //        GenericMPFFile mpfFileProcess = getMPFFileObject();
    //        try
    //        {
    //            mpfFileProcess.LoadMPFFileDetail(empList, int.Parse(MPFPlanID.SelectedValue), dtPayPeriodFr, dtPayPeriodTo);
    //            UploadMPFFile(mpfFileProcess);
    //            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Payroll_UploadMPFFile.aspx");
    //        }
    //        catch (Exception ex)
    //        {
    //            errors.addError(ex.Message);
    //        }
    //    }
    //    //        emploadData(empInfo, EEmpPayroll.db, empRepeater);
    //}

    //protected void UploadMPFFile(GenericMPFFile mpfFileProcess)
    //{
    //    FileInfo mpfFileInfo = mpfFileProcess.GenerateMPFFile();

    //    string mpfFilename = mpfFileProcess.MPFFileName();

    //    DateTime currentDateTime = AppUtils.ServerDateTime();
    //    if (mpfFileInfo.Exists)
    //    {
    //        DatabaseConnection masterDBConn;
    //        if (Application["MasterDBConfig"] != null)
    //        {
    //            masterDBConn = ((DatabaseConfig)Application["MasterDBConfig"]).CreateDatabaseConnectionObject();

    //            string UploadMPFFilePath = HROne.SaaS.Entities.ESystemParameter.getParameter(masterDBConn, HROne.SaaS.Entities.ESystemParameter.PARAM_CODE_BANKFILE_UPLOAD_FOLDER);
    //            if (System.IO.Directory.Exists(UploadMPFFilePath))
    //            {
    //                string relativePath = System.IO.Path.Combine(System.IO.Path.Combine(System.IO.Path.Combine(Session["CompanyDBID"].ToString(), "MPF"), "AMPFF"), mpfFilename);
    //                string fullPath = System.IO.Path.Combine(UploadMPFFilePath, relativePath);

    //                System.IO.Directory.CreateDirectory((new System.IO.FileInfo(fullPath)).Directory.FullName);
    //                mpfFileInfo.MoveTo(fullPath);

    //                HROne.SaaS.Entities.ECompanyMPFFile companyMPFFile = new HROne.SaaS.Entities.ECompanyMPFFile();
    //                companyMPFFile.CompanyDBID = (int)Session["CompanyDBID"];
    //                companyMPFFile.CompanyMPFFileTrusteeCode = mpfFileProcess.MPFSchemeTrusteeCode;
    //                companyMPFFile.CompanyMPFFileFileType = "AMPFF";
    //                companyMPFFile.CompanyMPFFileDataFileRelativePath = relativePath;
    //                companyMPFFile.CompanyMPFFileSubmitDateTime = currentDateTime;
    //                companyMPFFile.CompanyMPFFileConfirmDateTime = currentDateTime;
    //                HROne.SaaS.Entities.ECompanyMPFFile.db.insert(masterDBConn, companyMPFFile);

    //            }
    //        }
    //    }
    //}
}
