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

public partial class Emp_Document_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "PER014";
    public Binding binding;
    public DBManager db = EEmpDocument.db;
    public EEmpDocument obj;
    public int CurID = -1;
    public int CurEmpID = -1;

    private string uploadFolder = string.Empty;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        binding = new Binding(dbConn, db);
        binding.add(EmpDocumentID);
        binding.add(EmpID);
        binding.add(new DropDownVLBinder(db, DocumentTypeID, EDocumentType.VLDocumentType));
        binding.add(EmpDocumentOriginalFileName);
        binding.add(EmpDocumentDesc);
        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["EmpDocumentID"], out CurID))
            CurID = -1;

        if (CurID > 0)
        {
            ExistingFilePanel.Visible = true;
            NewFilePanel.Visible = false;
        }
        else
        {
            ExistingFilePanel.Visible = false;
            NewFilePanel.Visible = true;
        }

        if (!int.TryParse(DecryptedRequest["EmpID"], out CurEmpID))
            CurEmpID = -1;


        EmpID.Value = CurEmpID.ToString();

        //  MUST applied Label Change before translation
        if (CurID > 0)
            ActionHeader.Text = "Edit";
        else
            ActionHeader.Text = "Add";

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (!Page.IsPostBack)
        {
            if (CurID > 0)
                loadObject();
            else
                toolBar.DeleteButton_Visible = false;
        }

        uploadFolder = AppUtils.GetDocumentUploadFolder(dbConn);
    }
    protected bool loadObject()
    {
        obj = new EEmpDocument();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);

        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpID", CurEmpID));
        filter.add(WebUtils.AddRankFilter(Session, "EmpID", true));
        if (EEmpPersonalInfo.db.count(dbConn, filter) == 0)
            if (CurEmpID <= 0)
                return false;
            else
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");

        if (!db.select(dbConn, obj))
            if (CurID <= 0)
                return false;
            else
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");

        if (obj.EmpID != CurEmpID)
            if (CurID <= 0)
                return false;
            else
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);


        return true;
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        EEmpDocument c = new EEmpDocument();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);

        if (CurID < 0)
        {
            if (!UploadDocumentFile.HasFile)
            {
                errors.addError(HROne.Translation.PageErrorMessage.ERROR_INVALID_FILE);
                return;
            }
            string errorMessage = string.Empty;
            if (HROne.CommonLib.FileIOProcess.IsFolderAllowWritePermission(uploadFolder, out errorMessage))
            {
                //            Utils.MarkCreate(Session, c);
                string uploadFullName = UploadToDocumentFolder();
                c.EmpDocumentOriginalFileName = UploadDocumentFile.FileName;
                c.EmpDocumentStoredFileName = uploadFullName;
                c.EmpDocumentIsCompressed = true;
                WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
                db.insert(dbConn, c);
                WebUtils.EndFunction(dbConn);
                CurID = c.EmpDocumentID;
            }
            else
            {
                errors.addError(errorMessage);
                return;
            }
            //            url = Utils.BuildURL(-1, CurID);
        }
        else
        {
            //            Utils.Mark(Session, c);
            WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
            db.update(dbConn, c);
            WebUtils.EndFunction(dbConn);
        }

        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Emp_Document_View.aspx?EmpDocumentID=" + CurID + "&EmpID=" + c.EmpID);


    }
    protected void Delete_Click(object sender, EventArgs e)
    {

        EEmpDocument c = new EEmpDocument();
        c.EmpDocumentID = CurID;
        db.select(dbConn, c);
        WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
        db.delete(dbConn, c);
        WebUtils.EndFunction(dbConn);

        try
        {
            string UploadFile = System.IO.Path.Combine(uploadFolder, c.EmpDocumentStoredFileName);
            if (System.IO.File.Exists(UploadFile))
                System.IO.File.Delete(UploadFile);
        }
        catch
        {
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/EmpTab_Document_View.aspx?EmpID=" + EmpID.Value);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Emp_Document_View.aspx?EmpDocumentID=" + CurID + "&EmpID=" + EmpID.Value);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/EmpTab_Document_View.aspx?EmpID=" + EmpID.Value);

    }

    protected string UploadToDocumentFolder()
    {
        string strTmpFolder = HROne.Common.Folder.GetOrCreateSessionTempFolder(Session.SessionID).FullName;// System.IO.Path.GetTempPath(); //Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);
        string strTmpFile = System.IO.Path.Combine(strTmpFolder, AppUtils.ServerDateTime().ToString("~yyyyMMddHHmmss_") + Server.UrlEncode(UploadDocumentFile.FileName));
        UploadDocumentFile.SaveAs(strTmpFile);

        string relativePath = System.IO.Path.Combine(dbConn.Connection.Database, CurEmpID + "_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".hrd");
        string destinationFile = System.IO.Path.Combine(uploadFolder, relativePath);
        System.IO.DirectoryInfo documentDirectoryInfo = new System.IO.DirectoryInfo(System.IO.Path.GetDirectoryName(destinationFile));
        if (!documentDirectoryInfo.Exists)
            documentDirectoryInfo.Create();

        zip.Compress(strTmpFolder, System.IO.Path.GetFileName(strTmpFile), destinationFile);
        System.IO.File.Delete(strTmpFile);
        return relativePath;
    }
}
