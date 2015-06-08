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

public partial class Payroll_PeriodInfo : HROneWebControl
{
    public Binding binding;
    public DBManager db = EPayrollPeriod.db;
    public EPayrollPeriod obj;
    public int CurID = -1;


    protected void Page_Load(object sender, EventArgs e)
    {
        PreRender += new EventHandler(Payroll_PeriodInfo_PreRender);
        binding = new Binding(dbConn, db);
        binding.add(PayPeriodID);
        binding.add(new BlankZeroLabelVLBinder(db, PayGroupID, EPayrollGroup.VLPayrollGroup));
        binding.add(new LabelVLBinder(db, PayPeriodStatus, EPayrollPeriod.VLPayPeriodStatus));
        binding.add(PayPeriodFr);
        binding.add(PayPeriodTo);
        binding.add(PayPeriodLeaveCutOffDate);
        binding.add(PayPeriodAttnFr);
        binding.add(PayPeriodAttnTo);
        binding.add(PayPeriodTrialRunDate);
        binding.add(new BlankZeroLabelVLBinder(db, PayPeriodTrialRunBy, EUser.VLUserName));
        binding.add(PayPeriodRollbackDate);
        binding.add(new BlankZeroLabelVLBinder(db, PayPeriodRollbackBy, EUser.VLUserName));
        binding.add(PayPeriodConfirmDate);
        binding.add(new BlankZeroLabelVLBinder(db, PayPeriodConfirmBy, EUser.VLUserName));
        binding.add(PayPeriodProcessEndDate);
        binding.add(new BlankZeroLabelVLBinder(db, PayPeriodProcessEndBy, EUser.VLUserName));
        binding.init(Request, Session);

        


    }

    public int CurrentPayPeriodID
    {
        get
        {
            if (string.IsNullOrEmpty(PayPeriodID.Value))
                return 0;
            else
                return int.Parse(PayPeriodID.Value);
        }
        set { PayPeriodID.Value = value.ToString(); }
    }

    void Payroll_PeriodInfo_PreRender(object sender, EventArgs e)
    {
        loadObject();
    }

    protected bool loadObject()
    {
        obj = new EPayrollPeriod();
        if (string.IsNullOrEmpty(PayPeriodID.Value))
            obj.PayPeriodID = 0;
        else
            obj.PayPeriodID = int.Parse(PayPeriodID.Value);

        bool result = !db.select(dbConn, obj);

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);
        return result;

    }

}
