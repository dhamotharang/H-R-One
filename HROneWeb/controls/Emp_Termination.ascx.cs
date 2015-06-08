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

public partial class Emp_Termination : HROneWebControl
{
    public Binding binding;
    public DBManager db = EEmpTermination.db;
    public EEmpTermination obj;
    public int CurID = -1;

    public bool hasTerminated=false;
    protected void Page_Load(object sender, EventArgs e)
    {
        binding = new Binding(dbConn, db);
        binding.add(EmpID);
        binding.add(new LabelVLBinder(db, CessationReasonID, ECessationReason.VLCessationReason));
        binding.add(EmpTermResignDate);
        binding.add(EmpTermNoticePeriod);
        binding.add(new LabelVLBinder(db, EmpTermNoticeUnit,Values.VLEmpUnit));
        binding.add(EmpTermLastDate);
        binding.add(EmpTermRemark);
        binding.add(new LabelVLBinder(db, EmpTermIsTransferCompany, Values.VLYesNo));
        binding.init(Request, Session);




        if (!int.TryParse(DecryptedRequest["EmpID"], out CurID))
            CurID = -1;


       hasTerminated= loadObject();


    }


    protected bool loadObject()
    {
        obj = new EEmpTermination();
        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpID", CurID));
        
        ArrayList list=db.select(dbConn, filter);
        if (list.Count == 0)
            return false;
        obj = (EEmpTermination)list[0];
        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);

        if (!obj.EmpTermResignDate.Ticks.Equals(0) && obj.EmpTermNoticePeriod >= 0 && obj.EmpTermNoticeUnit != string.Empty)
            ExpectedLastDate.Text = obj.GetExpectedLastEmploymentDate().ToString("yyyy-MM-dd");

        return true;
    }

}
