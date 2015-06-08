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
using HROne.Translation;

public partial class Payroll_BonusProcess_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "PAY020";
    public Binding binding;
    public DBManager db = EBonusProcess.db;
    public EBonusProcess obj;
    public int CurID = -1;

    protected SearchBinding sbinding;
    protected ListInfo info;
    private DataView view;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        binding = new Binding(dbConn, db);
        binding.add(BonusProcessID);
        binding.add(BonusProcessDesc);
        binding.add(new TextBoxBinder(db, BonusProcessSalaryMonth.TextBox, BonusProcessSalaryMonth.ID));
        binding.add(BonusProcessStatus);
        binding.add(new TextBoxBinder(db, BonusProcessMonth.TextBox, BonusProcessMonth.ID));
        binding.add(new TextBoxBinder(db, BonusProcessPeriodFr.TextBox, BonusProcessPeriodFr.ID));
        binding.add(new TextBoxBinder(db, BonusProcessPeriodTo.TextBox, BonusProcessPeriodTo.ID));
        binding.add(new DropDownVLBinder(db, BonusProcessPayCodeID, EPaymentCode.VLPaymentCode));
        binding.add(new TextBoxBinder(db, BonusProcessPayDate.TextBox, BonusProcessPayDate.ID));
        binding.add(BonusProcessStdRate);
        binding.add(BonusProcessRank1);
        binding.add(BonusProcessRank2);
        binding.add(BonusProcessRank3);
        binding.add(BonusProcessRank4);
        binding.add(BonusProcessRank5);

        binding.init(Request, Session);

        sbinding = new SearchBinding(dbConn, EBonusProcess.db);

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        //info = ListFooter.ListInfo;

        if (!int.TryParse(DecryptedRequest["BonusProcessID"], out CurID))
            CurID = -1;

        if (!Page.IsPostBack)
        {
            //view = loadData(info, EBonusProcess.db, Repeater);

            if (CurID > 0)
            {
                loadObject();
            }
            else
            {
                BonusProcessStatus.Text = EBonusProcess.STATUS_NORMAL;
                BonusProcessStatusDesc.Text = EBonusProcess.STATUS_NORMAL_DESC;

                toolBar.DeleteButton_Visible = false;
            }
        }
    }
    protected bool loadObject() 
    {
	    obj=new EBonusProcess();
	    bool isNew=WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
	    if(!db.select(dbConn, obj))
		    return false;

        //if (string.IsNullOrEmpty(obj.AVCPlanEmployerRoundingRule) && string.IsNullOrEmpty(obj.AVCPlanEmployeeRoundingRule))
        //{
        //    obj.AVCPlanEmployerRoundingRule = Values.ROUNDING_RULE_ROUND_TO;
        //    obj.AVCPlanEmployerDecimalPlace = 2;
        //    obj.AVCPlanEmployeeRoundingRule = Values.ROUNDING_RULE_ROUND_TO;
        //    obj.AVCPlanEmployeeDecimalPlace = 2;
        //}

	    Hashtable values=new Hashtable();
	    db.populate(obj,values);
	    binding.toControl(values);

        switch (obj.BonusProcessStatus)
        {
            case EBonusProcess.STATUS_NORMAL:
                BonusProcessStatusDesc.Text = EBonusProcess.STATUS_NORMAL_DESC;
                break;
            case EBonusProcess.STATUS_CONFIRMED:
                BonusProcessStatusDesc.Text = EBonusProcess.STATUS_CONFIRMED_DESC;
                break;
            case EBonusProcess.STATUS_CANCELLED:
                BonusProcessStatusDesc.Text = EBonusProcess.STATUS_CANCELLED_DESC;
                break;
        }

        return true;
    }

    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = sbinding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = " c.* ";
        string from = "from  " + db.dbclass.tableName + " c ";

        filter.add(new Match("BonusProcessStatus", EBonusProcess.STATUS_NORMAL));
        //filter.add("PaymentCode", true);

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);

        view = new DataView(table);

//        ListFooter.Refresh();

        if (repeater != null)
        {
            repeater.DataSource = table;
            repeater.DataBind();
        }

        return view;
    }

    protected void BonusProcessMonth_Changed(object sender, EventArgs e)
    {
        DateTime m_value;

        if (DateTime.TryParse(BonusProcessMonth.Value, out m_value))
        {
            if (string.IsNullOrEmpty(BonusProcessSalaryMonth.Value))
            {
                // Start 0000127, Ricky So, 2014/11/13
                //BonusProcessSalaryMonth.Value = AppUtils.LastDayInMonth(m_value.AddMonths(-1)).ToString("yyyy-MM-dd");

                //m_value = new DateTime(m_value.Year - 1, 12, 31);
                //BonusProcessSalaryMonth.Value = m_value.ToString("yyyy-MM-dd");
                BonusProcessSalaryMonth.Value = (new DateTime(m_value.Year - 1, 12, 31)).ToString("yyyy-MM-dd");

                // End 0000127, Ricky So, 2014/11/13
            }
            if (string.IsNullOrEmpty(BonusProcessPeriodFr.Value) && string.IsNullOrEmpty(BonusProcessPeriodTo.Value))
            {
                BonusProcessPeriodFr.Value = (new DateTime(m_value.Year - 1, 1, 1)).ToString("yyyy-MM-dd");
                BonusProcessPeriodTo.Value = (new DateTime(m_value.Year - 1, 12, 31)).ToString("yyyy-MM-dd");
            }
        }
    }

    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        EBonusProcess c = new EBonusProcess();
        
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
            db.insert(dbConn, c);
            CurID = c.BonusProcessID;
        }
        else
        {
            db.update(dbConn, c);
        }

        //foreach (EPaymentCode o in unselectedCeilingList)
        //{
        //    DBFilter avcPlanCeilingFilter = new DBFilter();
        //    avcPlanCeilingFilter.add(new Match("AVCPlanID", c.AVCPlanID));
        //    avcPlanCeilingFilter.add(new Match("PaymentCodeID", o.PaymentCodeID));
        //    ArrayList avcPlanCeilingList = EAVCPlanPaymentCeiling.db.select(dbConn, avcPlanCeilingFilter);
        //    if (avcPlanCeilingList.Count != 0)
        //    {
        //        foreach (EAVCPlanPaymentCeiling avcPlanCeiling in avcPlanCeilingList)
        //            EAVCPlanPaymentCeiling.db.delete(dbConn, avcPlanCeiling);
        //    }
        //}

        WebUtils.EndFunction(dbConn);

        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_BonusProcess_View.aspx?BonusProcessID="+CurID);

    }
    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        EBonusProcess o = new EBonusProcess();
        o.BonusProcessID = CurID;

        if (db.select(dbConn, o))
        {
            if (o.BonusProcessStatus != EBonusProcess.STATUS_NORMAL)
            {
                errors.addError("Bonus Process remove failed.  Status is not " + EBonusProcess.STATUS_NORMAL_DESC);

            }
            else
            {
                DBFilter m_filter = new DBFilter();
                m_filter.add(new Match("BonusProcessID", CurID));
                EEmpBonusProcess.db.delete(dbConn, m_filter);
                if (EBonusProcess.db.delete(dbConn, m_filter))
                {
                    errors.addError("Bonus Process removed");
                    HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_BonusProcess_List.aspx");
                }
                else
                    errors.addError("Bonus Process remove failed.");
            }
        }
    }
//    protected void ChangeOrder_Click(object sender, EventArgs e)
//    {
//        LinkButton l = (LinkButton)sender;
//        String id = l.ID.Substring(1);
//        if (info.orderby == null)
//            info.order = true;
//        else if (info.orderby.Equals(id))
//            info.order = !info.order;
//        else
//            info.order = true;
//        info.orderby = id;

////        view = loadData(info, EPaymentCode.db, Repeater);

//    }
    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_BonusProcess_View.aspx?BonusProcessID=" + CurID);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_BonusProcess_List.aspx");

    }
}
