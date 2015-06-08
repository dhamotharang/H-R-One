//using System;
//using System.Data;
//using System.Configuration;
//using System.Collections;
//using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Web.UI.HtmlControls;
//using HROne.DataAccess;
////using perspectivemind.validation;
//using HROne.Lib.Entities;

//public partial class Emp_RecurringPayment : HROneWebControl
//{
//    private const string FUNCTION_CODE = "PER007";
//    public Binding binding;
//    public DBManager db = EEmpRecurringPayment.db;
//    public EEmpRecurringPayment obj;
//    public int CurID = -1;


//    protected void Page_Load(object sender, EventArgs e)
//    {
//        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
//            return;
        
//        PreRender += new EventHandler(Emp_RecurringPayment_PreRender);
//        binding = new Binding(dbConn, db);
//        binding.add(EmpRPEffFr);
//        binding.add(EmpRPEffTo);
//        binding.add(EmpRPAmount);
//        binding.add(CurrencyID);

//        binding.add(new LabelVLBinder(db, EmpRPMethod, Values.VLPaymentMethod));
//        binding.add(new LabelVLBinder(db, EmpRPUnit, Values.VLPaymentUnit));
//        binding.add(new LabelRelationBinder(db, EmpAccID, EEmpBankAccount.db, "EmpBankAccountID", "EmpAccountNo"));
//        binding.add(PayCodeID);
//        binding.init(Request, Session);



//        try
//        {
//            CurID = Int32.Parse(DecryptedRequest["EmpID"]);
//        }
//        catch (Exception ex)
//        {
//        }

//    }

//    void Emp_RecurringPayment_PreRender(object sender, EventArgs e)
//    {
//        loadObject();        
//    }

//    protected bool loadObject()
//    {
//        DBFilter filter = new DBFilter();
//        filter.add(new Match("EmpID", CurID));
//        filter.add(new NullTerm("EmpRPEffTo"));
//        ArrayList list=db.select(dbConn, filter);
//        if (list.Count == 0)
//            return false;
//        obj = (EEmpRecurringPayment)list[0];

//        Hashtable values = new Hashtable();
//        db.populate(obj, values);
//        binding.toControl(values);
//        return true;
//    }

//}
