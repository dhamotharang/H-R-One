using System;
using System.Collections;
using System.Configuration;
using System.Data;
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
//using System.Text;

public partial class Taxation_GenerateDisk : HROneWebPage
{
    private const string FUNCTION_CODE = "TAX005";
    protected Binding binding;
    public DBManager db = ETaxEmp.db;
//    public EPayrollGroup obj;
    public int CurID = -1;

    protected int CurrentTaxFormID;
    private const string TAXATION_NAME_OF_SIGNATURE = "TAXATION_NAME_OF_SIGNATURE";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        if (!WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
        {
            btnGenerateFile.Visible = false;
            btnPrint.Visible = false;

        }

        if (Session["CompanyDBID"] != null)
        {
            btnPrint.Visible = false;
        }
        //if (!int.TryParse(TaxCompID.SelectedValue, out CurID))
        //    if (DecryptedRequest["TaxCompID"] != null && !Page.IsPostBack)
        //    {
        //        try
        //        {
        //            CurID = Int32.Parse(DecryptedRequest["TaxCompID"]);
        //            TaxCompID.SelectedValue = CurID.ToString();
        //        }
        //        catch
        //        {
        //            CurID = -1;
        //        }
        //    }
        //if (CurID <= 0 && TaxCompID.Items.Count > 1)
        //{
        //    TaxCompID.SelectedIndex = 1;
        //    int.TryParse(TaxCompID.SelectedValue, out CurID);
        //}

        binding = new Binding(dbConn, ETaxCompany.db);
        binding.add(new DropDownVLBinder(ETaxCompany.db, TaxCompID, ETaxCompany.VLTaxCompany));

        DBFilter taxFormFilter = new DBFilter();
        //        payPeriodFilter.add(new Match("PayPeriodStatus", "!=", "N"));
        //if (DecryptedRequest["TaxCompID"] != null)
        //    taxFormFilter.add(new Match("TaxCompID", DecryptedRequest["TaxCompID"]));
        //else
        //    taxFormFilter.add(new Match("TaxCompID", 0));
        taxFormFilter.add(new Match("TaxCompID", TaxCompID.SelectedValue));
        taxFormFilter.add(new Match("TaxFormType", "B"));
        taxFormFilter.add("TaxFormYear", false);
        binding.add(new DropDownVLBinder(ETaxForm.db, TaxFormID, ETaxForm.VLTaxFormYear, taxFormFilter));

        binding.init(Request, Session);

        //try
        //{
        //    CurID = Int32.Parse(DecryptedRequest["TaxCompID"]);
        //    if (DecryptedRequest["TaxFormID"] != null)
        //        CurrentTaxFormID = Int32.Parse(DecryptedRequest["TaxFormID"]);
        //    else if (!Int32.TryParse(TaxFormID.SelectedValue, out CurrentTaxFormID))
        //    {
        //        CurrentTaxFormID = 0;
        //        //EPayrollGroup obj = new EPayrollGroup();
        //        //obj.PayGroupID = CurID;
        //        //if (!db.select(dbConn, obj))
        //        //    CurPayPeriodID = obj.CurrentPayPeriodID;
        //    }

        //    if (CurrentTaxFormID <= 0)
        //    {
        //        if (TaxFormID.Items.Count > 1)
        //            CurrentTaxFormID = int.Parse(TaxFormID.Items[1].Value);

        //    }
        //    if (CurrentTaxFormID > 0)
        //        TaxFormID.SelectedValue = CurrentTaxFormID.ToString();

        //}
        //catch (Exception ex)
        //{
        //}

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        //if (!Page.IsPostBack)
        //{
        //    if (CurID > 0)
        //    {
        //        loadObject();

        //    }

        //}
        //if (!WebUtils.CheckAccess(Response, Session))
        //    return;

        //sbinding = new SearchBinding(dbConn, db);
        //sbinding.add(new DropDownVLSearchBinder( TaxCompID,"TaxCompID", ETaxCompany.VLTaxCompany));
        //sbinding.add(new DropDownVLSearchBinder(YearSelect, "",new WFYearList(AppUtils.ServerDateTime().Date.Year- 2001, 0)));

        //sbinding.init(DecryptedRequest, null);

        if (!Page.IsPostBack)
        {
            txtNameOfSignature.Text = ESystemParameter.getParameter(dbConn, TAXATION_NAME_OF_SIGNATURE);

        }

    }

    //protected bool loadObject()
    //{
    //    ETaxCompany obj = new ETaxCompany();
    //    bool isNew = WebFormWorkers.loadKeys(ETaxCompany.db, obj, DecryptedRequest);
    //    if (!ETaxCompany.db.select(dbConn, obj))
    //        return false;

    //    Hashtable values = new Hashtable();
    //    ETaxCompany.db.populate(obj, values);
    //    binding.toControl(values);

    //    return true;
    //}

    protected void TaxCompID_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Response.Redirect(Request.Url.LocalPath + "?TaxCompID=" + TaxCompID.SelectedValue);
    }


    protected void btnGenerateFile_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        int taxFormID;
        if (!int.TryParse(TaxFormID.SelectedValue, out taxFormID))
        {
            errors.addError("Year Return is not selected");
        }
        else if (taxFormID <= 0)
        {
            errors.addError("Year Return is not selected");
        }

        string taxFileFormat = TaxFileFormat.SelectedValue;
        if (taxFileFormat == "")
        {
            errors.addError("File Format is not selected");
        }

        //if (errors.isEmpty())
        //{
        //    //  skip converting content to big5.
        //    //  It should be done inside HROne.Taxation.TaxationGeneration.GenerateTaxationFileData function
        //    System.Text.Encoding big5 = HROne.Taxation.TaxationGeneration.GetTaxationFileEncoding();
        //    //Encoding utf8 = System.Text.Encoding.GetEncoding("utf-8");
        //    string taxFileData = HROne.Taxation.TaxationGeneration.GenerateTaxationFileData(dbConn, taxFormID);
        //    //byte[] taxFileByteArray = utf8.GetBytes(taxFileData);
        //    //byte[] taxFileByteArrayBig5 = System.Text.Encoding.Convert(utf8, big5, taxFileByteArray);
        //    byte[] taxFileByteArrayBig5 = big5.GetBytes(taxFileData);

        //    char[] taxFileCharArrayBig5 = big5.GetChars(taxFileByteArrayBig5);

        //    Response.Clear();
        //    Response.ContentEncoding = big5;
        //    Response.AddHeader("Content-Type", "text/plain");
        //    Response.AddHeader("Content-Disposition", "attachment;filename=TaxationFile" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".txt");
        //    Response.AppendHeader("Content-Length", taxFileByteArrayBig5.Length.ToString());
        //    Response.Expires = -1;
        //    Response.Write(taxFileCharArrayBig5, 0, taxFileCharArrayBig5.Length);
        //    Response.End();
        //}
        if (errors.isEmpty() && Response.IsClientConnected)
        {
            ESystemParameter.setParameter(dbConn, TAXATION_NAME_OF_SIGNATURE, txtNameOfSignature.Text);

            HROneConfig config = HROneConfig.GetCurrentHROneConfig();
            if (config.GenerateReportAsInbox)
            {
                if (EInboxAttachment.GetTotalSize(dbConn, 0) < WebUtils.productLicense(Session).MaxInboxSizeMB * 1000 * 1000)
                {
                    HROne.TaskService.TaxationDiskTaskFactory reportTask = new HROne.TaskService.TaxationDiskTaskFactory(dbConn, user, lblReportHeader.Text + " ( " + TaxFormID.SelectedItem.Text + " ) " + TaxFileFormat.SelectedItem.Text, taxFormID, txtNameOfSignature.Text, taxFileFormat);
                    AppUtils.reportTaskQueueService.AddTask(reportTask);
                    errors.addError(HROne.Translation.PageMessage.REPORT_GENERATING_TO_INBOX);
                }
                else
                    errors.addError(HROne.Translation.PageMessage.INBOX_SIZE_EXCEEDED);
            }
            else
            {
                HROne.Taxation.TaxationDiskProcess process = new HROne.Taxation.TaxationDiskProcess(dbConn, taxFormID);

                if (taxFileFormat.Equals("TEXT", StringComparison.CurrentCultureIgnoreCase))
                {
                    string outputFileName = "TaxationFile_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".txt";
                    WebUtils.TransmitFile(Response, process.GenerateToFile(), outputFileName, true);
                }
                else if (taxFileFormat.Equals("XML", StringComparison.CurrentCultureIgnoreCase))
                {
                    // generate taxation file in XML format
                    string outputFileName = "TaxationFile_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xml";
                    WebUtils.TransmitFile(Response, process.GenerateToXML(), outputFileName, true);
                }
            }        
        }
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        int taxFormID;
        if (!int.TryParse(TaxFormID.SelectedValue, out taxFormID))
        {
            errors.addError("Year Return is not selected");
        }
        else if (taxFormID <= 0)
        {
            errors.addError("Year Return is not selected");
        }

        if (errors.isEmpty())
        {


            HROne.Reports.Taxation.ControlListProcess rpt = new HROne.Reports.Taxation.ControlListProcess(dbConn, taxFormID, txtNameOfSignature.Text);
            string reportFileName = WebUtils.GetLocalizedReportFile(Server.MapPath("~/Report_Taxation_ControlList.rpt"));
            WebUtils.ReportExport(dbConn, user, errors, btnPrint.Text, Response, rpt, reportFileName, "PDF", "TaxControlList", false);


            //Session.Add("NameOfSignature", txtNameOfSignature.Text);
            //Response.Write("<Script language='Javascript'>");
            //Response.Write("window.open('"
            //    + "Report_Taxation_ControlList_View.aspx?"
            //    + "TaxFormID=" + CurrentTaxFormID
            //    + "','_blank','toolbar=no, location=no,directories=no,status=yes,menubar=no,scrollbars=yes,copyhistory=no, resizable=yes')");
            //Response.Write("</Script>");


        }
    }
}
