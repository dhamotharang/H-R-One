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

//public partial class Emp_PositionInfo : HROneWebControl
//{
//    public Binding binding;
//    public DBManager db = EEmpPositionInfo.db;
//    public EEmpPositionInfo obj;
//    public int CurID = -1;


//    protected void Page_Load(object sender, EventArgs e)
//    {
//        PreRender += new EventHandler(Emp_PositionInfo_PreRender);
//        binding = new Binding(dbConn, db);
//        binding.add(CompanyID);
//        binding.add(EmpPosEffFr);
//        binding.add(EmpPosEffTo);
//        binding.add(new BlankZeroLabelVLBinder(db, PositionID, EPosition.VLPosition));
//        binding.add(new BlankZeroLabelVLBinder(db, RankID, ERank.VLRank));
//        binding.add(new BlankZeroLabelVLBinder(db, StaffTypeID, EStaffType.VLStaffType));
//        binding.add(new BlankZeroLabelVLBinder(db, PayGroupID, EPayrollGroup.VLPayrollGroup));
//        binding.add(new BlankZeroLabelVLBinder(db, LeavePlanID, ELeavePlan.VLLeavePlan));
//        binding.init(Request, Session);



//        try
//        {
//            CurID = Int32.Parse(DecryptedRequest["EmpID"]);
//        }
//        catch (Exception ex)
//        {
//        }

//    }

//    void Emp_PositionInfo_PreRender(object sender, EventArgs e)
//    {
//        loadObject();        
//    }

//    protected bool loadObject()
//    {
//        obj = AppUtils.GetLastPositionInfo(dbConn, CurID);
//        if (obj==null)
//            return false;

//        Hashtable values = new Hashtable();
//        db.populate(obj, values);
//        binding.toControl(values);
//        return true;
//    }

//}
