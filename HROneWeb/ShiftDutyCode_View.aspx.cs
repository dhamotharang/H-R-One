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
using HROne.Lib.Entities;
using HROne.Translation;

public partial class ShiftDutyCode_View : HROneWebPage
{
    private const string FUNCTION_CODE = "SYS025";
    public Binding binding;
    public DBManager db = EShiftDutyCode.db;
    public EShiftDutyCode obj;
    public int CurID = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        binding = new Binding(dbConn, db);
        binding.add(ShiftDutyCodeID);
        binding.add(ShiftDutyCode);
        binding.add(ShiftDutyCodeDesc);
        binding.add(ShiftDutyFromTime);
        binding.add(ShiftDutyToTime);

        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["ShiftDutyCodeID"], out CurID))
            CurID = -1;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (!Page.IsPostBack)
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
        obj = new EShiftDutyCode();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
        if (!db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);
        return true;
    }

    protected void Delete_Click(object sender, EventArgs e)
    {
         PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        EShiftDutyCode o = new EShiftDutyCode();
        o.ShiftDutyCodeID = CurID;
        db.select(dbConn, o);
        WebUtils.StartFunction(Session, FUNCTION_CODE);
        db.delete(dbConn, o);
        WebUtils.EndFunction(dbConn);

        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "ShiftDutyCode_List.aspx");
    }

    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "ShiftDutyCode_Edit.aspx?ShiftDutyCodeID=" + CurID);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "ShiftDutyCode_List.aspx");
    }
}
