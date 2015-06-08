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

public partial class Emp_Document_View : HROneWebPage
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
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        binding = new Binding(dbConn, db);
        binding.add(EmpDocumentID);
        binding.add(EmpID);
        binding.add(new LabelVLBinder(db,DocumentTypeID, EDocumentType.VLDocumentType));
        binding.add(EmpDocumentOriginalFileName);
        binding.add(EmpDocumentDesc);
        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["EmpDocumentID"], out CurID))
            CurID = -1;

        if (!int.TryParse(DecryptedRequest["EmpID"], out CurEmpID))
            CurEmpID = -1;

        EmpID.Value = CurEmpID.ToString();

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
	    obj=new EEmpDocument();
	    bool isNew=WebFormWorkers.loadKeys(db, obj, DecryptedRequest);

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

	    Hashtable values=new Hashtable();
	    db.populate(obj,values);
	    binding.toControl(values);

        if (obj.EmpDocumentIsProfilePhoto)
            toolBar.CustomButton1_Visible = false;
        else
        {
            EDocumentType documentType = new EDocumentType();
            documentType.DocumentTypeID = obj.DocumentTypeID;
            EDocumentType.db.select(dbConn, documentType);
            if (documentType.DocumentTypeCode.Equals("PHOTO"))
                toolBar.CustomButton1_Visible = true;
            else
                toolBar.CustomButton1_Visible = false;
    }
        return true;
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
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_Document_View.aspx?EmpID=" + EmpID.Value);
    }
    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_Document_Edit.aspx?EmpDocumentID=" + EmpDocumentID.Value + "&EmpID=" + EmpID.Value);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_Document_View.aspx?EmpID=" + EmpID.Value);
    }
    protected void SetAsProfilePhoto_Click(object sender, EventArgs e)
    {
        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpDocumentID", "<>", CurID));
        filter.add(new Match("EmpID", CurEmpID));
        filter.add(new Match("EmpDocumentIsProfilePhoto", "<>", false));
        EEmpDocument t = new EEmpDocument();
        t.EmpDocumentIsProfilePhoto = false;
        db.updateByTemplate(dbConn, t, filter);

        EEmpDocument empDocument = new EEmpDocument();
        empDocument.EmpDocumentID = CurID;
        empDocument.EmpDocumentIsProfilePhoto = true;
        EEmpDocument.db.update(dbConn, empDocument);

        toolBar.CustomButton1_Visible = false;

    }
}
