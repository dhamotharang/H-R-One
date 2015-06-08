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
using HROne.Lib.Entities;
using HROne.DataAccess;

public partial class Payroll_CostAllocation_TrialRun_View : HROneWebPage
{
    private const string FUNCTION_CODE = "CST002";
    //private string FUNCTION_CODE = string.Empty;

    public Binding binding;
    public DBManager db = ECostAllocation.db;
    public int CurID = -1;



    //

    //private bool IsAllowEdit = true;
    protected void Page_Load(object sender, EventArgs e)
    {

            if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
                return;
            if (!WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            {
                Payroll_CostAllocationList.IsAllowEdit = false;

                //IsAllowEdit = false;
            }
            Payroll_CostAllocationList.FunctionCode = FUNCTION_CODE;



        binding = new Binding(dbConn, db);
        binding.add(EmpPayrollID);
        binding.add(EmpPayStatus);
        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["CostAllocationID"], out CurID))
            CurID = -1;
        loadState();

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        //if (!Page.IsPostBack)
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
        ECostAllocation obj = new ECostAllocation();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);

        DBFilter filter = new DBFilter();
        filter.add(new Match("CostAllocationID", obj.CostAllocationID));
        filter.add(WebUtils.AddRankFilter(Session, "EmpID", true));
        ArrayList costAllocationList = db.select(dbConn, filter);
        if (costAllocationList.Count == 0)
        {
            return false;
        }
        obj = (ECostAllocation)costAllocationList[0];

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);
        ucEmp_Header.CurrentEmpID = obj.EmpID;

        EEmpPayroll empPayroll = new EEmpPayroll();
        empPayroll.EmpPayrollID = obj.EmpPayrollID;
        EEmpPayroll.db.select(dbConn, empPayroll);
        ucPayroll_PeriodInfo.CurrentPayPeriodID = empPayroll.PayPeriodID;
        Payroll_CostAllocationList.CurrentCostAllocationID = obj.CostAllocationID;
        //Payroll_PaymentRecordList.CurrentEmpPayrollID = obj.EmpPayrollID;
        //Payroll_PaymentRecordList.CurrentEmpPayStatus = obj.EmpPayStatus;

        //Payroll_MPFRecordList.CurrentEmpPayrollID = obj.EmpPayrollID;
        //Payroll_MPFRecordList.CurrentEmpPayStatus = obj.EmpPayStatus;

        //Payroll_ORSORecordList.CurrentEmpPayrollID = obj.EmpPayrollID;
        //Payroll_ORSORecordList.CurrentEmpPayStatus = obj.EmpPayStatus;

        return true;
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_CostAllocation_TrialRun_Adjustment_List.aspx");
    }
}
