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

public partial class Training_Seminar_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "TRA002";
    public Binding binding;
    public DBManager db = ETrainingSeminar.db;
    public ETrainingSeminar obj;
    public int CurID = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        
        toolBar.FunctionCode = FUNCTION_CODE;

        binding = new Binding(dbConn, db);
        binding.add(TrainingSeminarID);
        binding.add(new DropDownVLBinder(db, TrainingCourseID, ETrainingCourse.VLTrainingCourse));
        binding.add(TrainingSeminarDesc);
        binding.add(new TextBoxBinder(db, TrainingSeminarDateFrom.TextBox, TrainingSeminarDateFrom.ID));
        binding.add(new TextBoxBinder(db, TrainingSeminarDateTo.TextBox, TrainingSeminarDateTo.ID));
        binding.add(TrainingSeminarDuration);
        binding.add(new DropDownVLBinder(db, TrainingSeminarDurationUnit, ETrainingSeminar.VLTrainingDurationUnit));
        binding.add(TrainingSeminarTrainer);
        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["TrainingSeminarID"], out CurID))
            CurID = -1;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

		if(!Page.IsPostBack) 
		{
            if (CurID > 0)
                loadObject();
            else
                toolBar.DeleteButton_Visible = false;

        }
    }
    protected bool loadObject() 
    {
	    obj=new ETrainingSeminar();
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
        ETrainingSeminar c = new ETrainingSeminar();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);

        WebUtils.StartFunction(Session, FUNCTION_CODE);
        if (CurID < 0)
        {
//            Utils.MarkCreate(Session, c);

            db.insert(dbConn, c);
            CurID = c.TrainingSeminarID;
//            url = Utils.BuildURL(-1, CurID);
        }
        else
        {
//            Utils.Mark(Session, c);
            db.update(dbConn, c);
        }

        DBFilter filter = new DBFilter();
        filter.add(new Match("TrainingSeminarID", c.TrainingSeminarID));

        WebUtils.EndFunction(dbConn);

        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Training_Seminar_View.aspx?TrainingSeminarID="+CurID);


    }

    protected void Delete_Click(object sender, EventArgs e)
    {

        ETrainingSeminar c = new ETrainingSeminar();
        c.TrainingSeminarID = CurID;
        if (db.select(dbConn, c))
        {
            DBFilter trainingSeminarFilter = new DBFilter();
            trainingSeminarFilter.add(new Match("TrainingSeminarID", CurID));
            EEmpTrainingEnroll.db.delete(dbConn, trainingSeminarFilter);

            WebUtils.StartFunction(Session, FUNCTION_CODE);
            db.delete(dbConn, c);
            WebUtils.EndFunction(dbConn);
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Training_Seminar_List.aspx");
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Training_Seminar_View.aspx?TrainingSeminarID=" + CurID);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Training_Seminar_List.aspx");

    }
}
