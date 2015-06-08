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

public partial class Attendance_AttendanceRecordAdjust_View : HROneWebPage
{
    private const string FUNCTION_CODE = "ATT008";
    //private string FUNCTION_CODE = string.Empty;

    public Binding binding;
    public DBManager db = EEmpPersonalInfo.db;
    public EEmpPersonalInfo obj;
    public int CurID = -1;



    //

    //private bool IsAllowEdit = true;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        if (!WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            Attendance_AttendanceRecordList.IsAllowEdit = false;
        else
            Attendance_AttendanceRecordList.IsAllowEdit = true;

        binding = new Binding(dbConn, db);
        binding.add(EmpID);
        binding.init(Request, Session);


        if (!int.TryParse(DecryptedRequest["EmpID"], out CurID))
            CurID = -1;
        Attendance_AttendanceRecordList.CurrentEmpID = CurID;

        if (CurID <= 0)
        {
            //IsAllowEdit = false;
            Attendance_AttendanceRecordList.Visible = false;

        }
        loadState();

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (!Page.IsPostBack)
        {
            if (CurID > 0)
                if (loadObject())
                {
                }
        }

    }


    public void loadState()
    {
    }
    protected bool loadObject()
    {
        obj = new EEmpPersonalInfo();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
        
        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpID", obj.EmpID));
        filter.add(WebUtils.AddRankFilter(Session, "EmpID", true));
        ArrayList empPayrollList = EEmpPersonalInfo.db.select(dbConn, filter);
        if (empPayrollList.Count == 0)
        {
            //IsAllowEdit = false;

            Attendance_AttendanceRecordList.Visible = false;

            return false;
        }
        obj = (EEmpPersonalInfo)empPayrollList[0];

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);
        ucEmp_Header.CurrentEmpID = obj.EmpID;




        return true;
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Attendance_AttendanceRecordAdjust_List.aspx");
    }
}
