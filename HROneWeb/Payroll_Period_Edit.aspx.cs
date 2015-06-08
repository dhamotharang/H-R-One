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

public partial class Payroll_Period_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "PAY001";
    public Binding binding;
    public DBManager db = EPayrollPeriod.db;
    public EPayrollPeriod obj;
    public int CurID = -1;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        binding = new Binding(dbConn, db);
        binding.add(new HiddenBinder(db, HiddenPayGroupID, "PayGroupID"));
        binding.add(new LabelVLBinder(db, PayGroupID, EPayrollGroup.VLPayrollGroup));
        binding.add(PayPeriodID);
        binding.add(new TextBoxBinder(db, PayPeriodFr.TextBox, PayPeriodFr.ID));
        binding.add(new TextBoxBinder(db, PayPeriodTo.TextBox, PayPeriodTo.ID));
        binding.add(new TextBoxBinder(db, PayPeriodLeaveCutOffDate.TextBox, PayPeriodLeaveCutOffDate.ID));
        binding.add(new TextBoxBinder(db, PayPeriodAttnFr.TextBox, PayPeriodAttnFr.ID));
        binding.add(new TextBoxBinder(db, PayPeriodAttnTo.TextBox, PayPeriodAttnTo.ID));
        binding.add(new CheckBoxBinder(db, PayPeriodIsAutoCreate));

        if (DecryptedRequest["PayGroupID"] != null)
            HiddenPayGroupID.Value = DecryptedRequest["PayGroupID"];

        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["PayPeriodID"], out CurID))
            CurID = -1;

        HROne.Common.WebUtility.WebControlsLocalization(Page, Page.Controls);

        if (!Page.IsPostBack)
        {
            if (CurID > 0)
            {
                loadObject();
            }
            else
            {
                if (DecryptedRequest["PayGroupID"] != null)
                {
                    Hashtable values = new Hashtable();
                    values.Add("PayGroupID", DecryptedRequest["PayGroupID"]);
                    binding.toControl(values);
                }
//                    HiddenPayGroupID.Value = DecryptedRequest["PayGroupID"];
                toolBar.DeleteButton_Visible = false;
            }

        }

    }

    protected bool loadObject()
    {
        obj = new EPayrollPeriod();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
        if (!db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);

        if (!obj.PayPeriodStatus.Equals("N"))
        {

            toolBar.SaveButton_Visible = false;
            toolBar.DeleteButton_Visible = false;
        }

        if (obj.PayGroupID>0)
        {
            EPayrollGroup payGroup = new EPayrollGroup();
            payGroup.PayGroupID=obj.PayGroupID;
            HiddenPayGroupID.Value = obj.PayGroupID.ToString();
            if (EPayrollGroup.db.select(dbConn, payGroup))
            {
                if (payGroup.CurrentPayPeriodID==obj.PayPeriodID )
                    toolBar.DeleteButton_Visible = false;
            }
        }

        return true;
    }
    protected void Save_Click(object sender, EventArgs e)
    {
        EPayrollPeriod c = new EPayrollPeriod();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);

        DBFilter payPeriodFilter = new DBFilter();
        payPeriodFilter.add(new Match("PayGroupID", "=", c.PayGroupID));
        payPeriodFilter.add(new Match("PayPeriodFr", "<=", c.PayPeriodTo));
        if (c.PayPeriodIsAutoCreate)
            //  If not auto created, do not allow future terms
        payPeriodFilter.add(new Match("PayPeriodTo", ">=", c.PayPeriodFr));
        if (c.PayPeriodID > 0)
        {
            payPeriodFilter.add(new Match("PayPeriodID", "<>", c.PayPeriodID));
        }
        ArrayList overlapPayperiod = EPayrollPeriod.db.select(dbConn, payPeriodFilter);

        if (overlapPayperiod.Count > 0)
        {
            errors.addError("PayPeriodFr", HROne.Translation.PageErrorMessage.ERROR_INVALID_PERIOD);
        }

        if (!errors.isEmpty())
            return;

        WebUtils.StartFunction(Session, FUNCTION_CODE);
        if (CurID < 0)
        {
            //            Utils.MarkCreate(Session, c);
            c.PayPeriodStatus = "N";
            db.insert(dbConn, c);
            CurID = c.PayPeriodID;
            //            url = Utils.BuildURL(-1, CurID);
        }
        else
        {
            //            Utils.Mark(Session, c);
            db.update(dbConn, c);
        }
        WebUtils.EndFunction(dbConn);
        db.select(dbConn, c);

        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_Group_View.aspx?PayGroupID=" + c.PayGroupID + "&PayPeriodID=" + CurID);


    }
    protected void Delete_Click(object sender, EventArgs e)
    {

        EPayrollPeriod c = new EPayrollPeriod();
        c.PayPeriodID = CurID;
        if (EPayrollPeriod.db.select(dbConn, c))
        {
            WebUtils.StartFunction(Session, FUNCTION_CODE);
            db.delete(dbConn, c);
            WebUtils.EndFunction(dbConn);
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_Group_View.aspx?PayGroupID=" + c.PayGroupID);
    }

    protected void Back_Click(object sender, EventArgs e)
    {

        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_Group_View.aspx?PayGroupID=" + HiddenPayGroupID.Value + "&PayPeriodID=" + CurID);
    }
}
