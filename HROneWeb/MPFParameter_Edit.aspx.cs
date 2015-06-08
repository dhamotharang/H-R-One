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

public partial class MPFParameter_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "MPF002";
    public Binding binding;
    public DBManager db = EMPFParameter.db;
    public EMPFParameter obj;
    public int CurID = -1;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        binding = new Binding(dbConn, db);
        binding.add(MPFParamID);
        binding.add(new TextBoxBinder(db, MPFParamEffFr.TextBox, MPFParamEffFr.ID));
        binding.add(MPFParamMinMonthly);
        binding.add(MPFParamMaxMonthly);
        binding.add(MPFParamMinDaily);
        binding.add(MPFParamMaxDaily);
        binding.add(MPFParamEEPercent);
        binding.add(MPFParamERPercent);
        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["MPFParamID"], out CurID))
            CurID = -1;

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
	    obj=new EMPFParameter();
	    bool isNew=WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
	    if(!db.select(dbConn, obj))
		    return false;

	    Hashtable values=new Hashtable();
	    db.populate(obj,values);
	    binding.toControl(values);
        return true;
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        EMPFParameter c = new EMPFParameter();

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
//            Utils.MarkCreate(Session, c);

            db.insert(dbConn, c);
            CurID = c.MPFParamID;
//            url = Utils.BuildURL(-1, CurID);
        }
        else
        {
//            Utils.Mark(Session, c);
            db.update(dbConn, c);
        }



        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/MPFParameter_List.aspx");
        //HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/MPFParameter_View.aspx?MPFParamID=" + CurID);


    }
    protected void Delete_Click(object sender, EventArgs e)
    {

        EMPFParameter c = new EMPFParameter();
        c.MPFParamID = CurID;
        db.delete(dbConn, c);
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/MPFParameter_List.aspx");
    }
    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/MPFParameter_List.aspx");
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/MPFParameter_List.aspx");

    }
}
