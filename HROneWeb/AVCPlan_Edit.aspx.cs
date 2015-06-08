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

public partial class AVCPlan_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "MPF003";
    public Binding binding;
    public DBManager db = EAVCPlan.db;
    public EAVCPlan obj;
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
        binding.add(AVCPlanID);
        binding.add(AVCPlanCode);
        binding.add(AVCPlanDesc);

        binding.add(new CheckBoxBinder(db, AVCPlanEmployerResidual));
        binding.add(AVCPlanEmployerResidualCap);
        binding.add(new CheckBoxBinder(db, AVCPlanEmployeeResidual));
        binding.add(AVCPlanEmployeeResidualCap);
        binding.add(new CheckBoxBinder(db, AVCPlanUseMPFExemption));
        binding.add(new CheckBoxBinder(db, AVCPlanJoinDateStart));
        binding.add(new CheckBoxBinder(db, AVCPlanContributeMaxAge));
        binding.add(new CheckBoxBinder(db, AVCPlanContributeMinRI));
        binding.add(AVCPlanMaxEmployerVC);
        binding.add(AVCPlanMaxEmployeeVC);
        binding.add(new DropDownVLBinder(db, AVCPlanEmployerRoundingRule, Values.VLRoundingRule));
        binding.add(new DropDownVLBinder(db, AVCPlanEmployerDecimalPlace, Values.VLDecimalPlace));
        binding.add(new DropDownVLBinder(db, AVCPlanEmployeeRoundingRule, Values.VLRoundingRule));
        binding.add(new DropDownVLBinder(db, AVCPlanEmployeeDecimalPlace, Values.VLDecimalPlace));
        binding.add(new CheckBoxBinder(db, AVCPlanNotRemoveContributionFromTopUp));


        binding.init(Request, Session);

        sbinding = new SearchBinding(dbConn, EPaymentCode.db);

        HROne.ProductLicense productLicense = WebUtils.productLicense(Session);

        if (productLicense.ProductType != HROne.ProductLicense.ProductLicenseType.HROne)
        {
            NotRemoveContributionFromTopUpRow.Visible = false;
            PaymentCeilingSection.Visible = false;
        }
        else
        {
            NotRemoveContributionFromTopUpRow.Visible = true;
            PaymentCeilingSection.Visible = true;
        }
        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        info = ListFooter.ListInfo;

        if (!int.TryParse(DecryptedRequest["AVCPlanID"], out CurID))
            CurID = -1;

        if (!Page.IsPostBack)
        {
            view = loadData(info, EPaymentCode.db, Repeater);

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
	    obj=new EAVCPlan();
	    bool isNew=WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
	    if(!db.select(dbConn, obj))
		    return false;

        if (string.IsNullOrEmpty(obj.AVCPlanEmployerRoundingRule) && string.IsNullOrEmpty(obj.AVCPlanEmployeeRoundingRule))
        {
            obj.AVCPlanEmployerRoundingRule = Values.ROUNDING_RULE_ROUND_TO;
            obj.AVCPlanEmployerDecimalPlace = 2;
            obj.AVCPlanEmployeeRoundingRule = Values.ROUNDING_RULE_ROUND_TO;
            obj.AVCPlanEmployeeDecimalPlace = 2;
        }

	    Hashtable values=new Hashtable();
	    db.populate(obj,values);
	    binding.toControl(values);
        return true;
    }

    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = sbinding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = " pc.* ";
        string from = "from  " + db.dbclass.tableName + " pc ";

        filter.add(new Match("PaymentCodeIsTopUp", true));
        //filter.add("PaymentCode", true);

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);

        view = new DataView(table);

        ListFooter.Refresh();

        if (repeater != null)
        {
            repeater.DataSource = table;
            repeater.DataBind();
        }

        return view;
    }

    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(EPaymentCode.db, row, cb);



        DBFilter avcPlanCeilingFilter = new DBFilter();
        avcPlanCeilingFilter.add(new Match("AVCPlanID", CurID));
        avcPlanCeilingFilter.add(new Match("PaymentCodeID", row["PaymentCodeID"]));
        ArrayList avcPlanCeilingList = EAVCPlanPaymentCeiling.db.select(dbConn, avcPlanCeilingFilter);
        if (avcPlanCeilingList.Count != 0)
        {
            TextBox textBox = (TextBox)e.Item.FindControl("AVCPlanPaymentCeilingAmount");
            textBox.Text = ((EAVCPlanPaymentCeiling)avcPlanCeilingList[0]).AVCPlanPaymentCeilingAmount.ToString("0.00");
            //((CheckBox)e.Item.FindControl("AVCPlanConsiderAfterMPFOnly")).Checked = ((EAVCPlanPaymentCeiling)avcPlanCeilingList[0]).AVCPlanConsiderAfterMPFOnly;
            //cb.Checked = true;
        }
        ArrayList avcPlanConsiderList = EAVCPlanPaymentConsider.db.select(dbConn, avcPlanCeilingFilter);
        if (avcPlanConsiderList.Count != 0)
        {
            ((CheckBox)e.Item.FindControl("AVCPlanPaymentConsiderAfterMPF")).Checked = ((EAVCPlanPaymentConsider)avcPlanConsiderList[0]).AVCPlanPaymentConsiderAfterMPF;
            //cb.Checked = true;
        }
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        EAVCPlan c = new EAVCPlan();

        if (AVCPlanMaxEmployeeVC.Text.EndsWith(".00"))
            AVCPlanMaxEmployeeVC.Text = AVCPlanMaxEmployeeVC.Text.Substring(0, AVCPlanMaxEmployeeVC.Text.Length - 3);
        if (AVCPlanMaxEmployerVC.Text.EndsWith(".00"))
        {
            AVCPlanMaxEmployerVC.Text = AVCPlanMaxEmployerVC.Text.Substring(0, AVCPlanMaxEmployerVC.Text.Length - 3);
        }
        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);
        if (!AppUtils.checkDuplicate(dbConn, db, c, errors, "AVCPlanCode"))
            return;

        ArrayList selectedCeilingList = new ArrayList();
        ArrayList unselectedCeilingList = new ArrayList();

        ArrayList selectedConsiderList = new ArrayList();
        ArrayList unselectedConsiderList = new ArrayList();

        foreach (RepeaterItem i in Repeater.Items)
        {
            CheckBox cb = (CheckBox)i.FindControl("ItemSelect");
            TextBox textBox = (TextBox)i.FindControl("AVCPlanPaymentCeilingAmount");
            CheckBox checkbox = (CheckBox)i.FindControl("AVCPlanPaymentConsiderAfterMPF");
            EPaymentCode o = new EPaymentCode();
            WebFormUtils.GetKeys(EPaymentCode.db, o, cb);
            if (EPaymentCode.db.select(dbConn, o))
            {
                if (!string.IsNullOrEmpty(textBox.Text.Trim()))
                {
                    double amount = 0;
                    if (double.TryParse(textBox.Text, out amount))
                    {
                        EAVCPlanPaymentCeiling avcPlanCeiling = new EAVCPlanPaymentCeiling();
                        avcPlanCeiling.PaymentCodeID = o.PaymentCodeID;
                        avcPlanCeiling.AVCPlanPaymentCeilingAmount = amount;
                        selectedCeilingList.Add(avcPlanCeiling);
                    }
                    else
                        errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_AVCPLAN_CEILING_NOT_NUMERIC, new string[] { o.PaymentCode }));
                }
                else
                    unselectedCeilingList.Add(o);

                if (checkbox.Checked)
                {
                    EAVCPlanPaymentConsider avcPlanConsider = new EAVCPlanPaymentConsider();
                    avcPlanConsider.PaymentCodeID = o.PaymentCodeID;
                    avcPlanConsider.AVCPlanPaymentConsiderAfterMPF = checkbox.Checked;
                    selectedConsiderList.Add(avcPlanConsider);
                }
                else
                    unselectedConsiderList.Add(o);
            }
        }

        WebUtils.StartFunction(Session, FUNCTION_CODE);
        if (CurID < 0)
        {
//            Utils.MarkCreate(Session, c);

            db.insert(dbConn, c);
            CurID = c.AVCPlanID;
//            url = Utils.BuildURL(-1, CurID);
        }
        else
        {
//            Utils.Mark(Session, c);
            db.update(dbConn, c);
        }
        foreach (EAVCPlanPaymentCeiling o in selectedCeilingList)
        {
            DBFilter avcPlanCeilingFilter = new DBFilter();
            avcPlanCeilingFilter.add(new Match("AVCPlanID", c.AVCPlanID));
            avcPlanCeilingFilter.add(new Match("PaymentCodeID", o.PaymentCodeID));
            ArrayList avcPlanCeilingList = EAVCPlanPaymentCeiling.db.select(dbConn, avcPlanCeilingFilter);
            if (avcPlanCeilingList.Count == 0)
            {
                o.AVCPlanID = c.AVCPlanID;
                EAVCPlanPaymentCeiling.db.insert(dbConn, o);
            }
            else
            {
                //  Check if more than 1 record exists in Database.
                int count = 0;
                foreach (EAVCPlanPaymentCeiling avcPlanCeiling in avcPlanCeilingList)
                {
                    if (count == 0)
                    {
                        avcPlanCeiling.AVCPlanPaymentCeilingAmount = o.AVCPlanPaymentCeilingAmount;
                        EAVCPlanPaymentCeiling.db.update(dbConn, avcPlanCeiling);
                    }
                    else
                        EAVCPlanPaymentCeiling.db.delete(dbConn, avcPlanCeiling);
                    count++;
                }
            }
        }

        foreach (EPaymentCode o in unselectedCeilingList)
        {
            DBFilter avcPlanCeilingFilter = new DBFilter();
            avcPlanCeilingFilter.add(new Match("AVCPlanID", c.AVCPlanID));
            avcPlanCeilingFilter.add(new Match("PaymentCodeID", o.PaymentCodeID));
            ArrayList avcPlanCeilingList = EAVCPlanPaymentCeiling.db.select(dbConn, avcPlanCeilingFilter);
            if (avcPlanCeilingList.Count != 0)
            {
                foreach (EAVCPlanPaymentCeiling avcPlanCeiling in avcPlanCeilingList)
                    EAVCPlanPaymentCeiling.db.delete(dbConn, avcPlanCeiling);
            }
        }

        foreach (EAVCPlanPaymentConsider o in selectedConsiderList)
        {
            DBFilter avcPlanCeilingFilter = new DBFilter();
            avcPlanCeilingFilter.add(new Match("AVCPlanID", c.AVCPlanID));
            avcPlanCeilingFilter.add(new Match("PaymentCodeID", o.PaymentCodeID));
            ArrayList avcPlanConsiderList = EAVCPlanPaymentConsider.db.select(dbConn, avcPlanCeilingFilter);
            if (avcPlanConsiderList.Count == 0)
            {
                o.AVCPlanID = c.AVCPlanID;
                EAVCPlanPaymentConsider.db.insert(dbConn, o);
            }
            else
            {
                //  Check if more than 1 record exists in Database.
                int count = 0;
                foreach (EAVCPlanPaymentConsider avcPlanConsider in avcPlanConsiderList)
                {
                    if (count == 0)
                    {
                        avcPlanConsider.AVCPlanPaymentConsiderAfterMPF = o.AVCPlanPaymentConsiderAfterMPF;
                        EAVCPlanPaymentConsider.db.update(dbConn, avcPlanConsider);
                    }
                    else
                        EAVCPlanPaymentConsider.db.delete(dbConn, avcPlanConsider);
                    count++;
                }
            }
        }

        foreach (EPaymentCode o in unselectedConsiderList)
        {
            DBFilter avcPlanCeilingFilter = new DBFilter();
            avcPlanCeilingFilter.add(new Match("AVCPlanID", c.AVCPlanID));
            avcPlanCeilingFilter.add(new Match("PaymentCodeID", o.PaymentCodeID));
            ArrayList avcPlanConsiderList = EAVCPlanPaymentConsider.db.select(dbConn, avcPlanCeilingFilter);
            if (avcPlanConsiderList.Count != 0)
            {
                foreach (EAVCPlanPaymentConsider avcPlanConsider in avcPlanConsiderList)
                    EAVCPlanPaymentConsider.db.delete(dbConn, avcPlanConsider);
            }
        }

        WebUtils.EndFunction(dbConn);


        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "AVCPlan_View.aspx?AVCPlanID="+CurID);


    }
    protected void Delete_Click(object sender, EventArgs e)
    {

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        EAVCPlan o = new EAVCPlan();
        o.AVCPlanID = CurID;
        if (db.select(dbConn, o))
        {
            DBFilter empAVCFilter = new DBFilter();
            empAVCFilter.add(new Match("AVCPlanID", o.AVCPlanID));
            empAVCFilter.add("empid", true);
            ArrayList empAVCList = EEmpAVCPlan.db.select(dbConn, empAVCFilter);
            if (empAVCList.Count > 0)
            {
                int curEmpID = 0;
                errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_USED_BY_EMPLOYEE, new string[] { HROne.Common.WebUtility.GetLocalizedString("AVC Plan Code"), o.AVCPlanCode }));
                foreach (EEmpAVCPlan empAVCPlan in empAVCList)
                {
                    EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                    empInfo.EmpID = empAVCPlan.EmpID;
                    if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                        if (curEmpID != empAVCPlan.EmpID)
                        {
                            errors.addError("- " + empInfo.EmpNo + ", " + empInfo.EmpEngFullName);
                            curEmpID = empAVCPlan.EmpID;
                        }
                        else
                            EEmpAVCPlan.db.delete(dbConn, empAVCPlan);

                }
                errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
                return;
            }
            else
            {
                WebUtils.StartFunction(Session, FUNCTION_CODE);
                db.delete(dbConn, o);
                DBFilter obj = new DBFilter();
                obj.add(new Match("AVCPlanID", o.AVCPlanID));
                ArrayList objList = EAVCPlanDetail.db.select(dbConn, obj);
                foreach (EAVCPlanDetail match in objList)
                    EAVCPlanDetail.db.delete(dbConn, match);

                objList = EAVCPlanPaymentCeiling.db.select(dbConn, obj);
                foreach (EAVCPlanPaymentCeiling match in objList)
                    EAVCPlanPaymentCeiling.db.delete(dbConn, match);

                objList = EAVCPlanPaymentConsider.db.select(dbConn, obj);
                foreach (EAVCPlanPaymentConsider match in objList)
                    EAVCPlanPaymentConsider.db.delete(dbConn, match);
                 
                WebUtils.EndFunction(dbConn);

            }
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "AVCPlan_List.aspx");
    }
    protected void ChangeOrder_Click(object sender, EventArgs e)
    {
        LinkButton l = (LinkButton)sender;
        String id = l.ID.Substring(1);
        if (info.orderby == null)
            info.order = true;
        else if (info.orderby.Equals(id))
            info.order = !info.order;
        else
            info.order = true;
        info.orderby = id;

        view = loadData(info, EPaymentCode.db, Repeater);

    }
    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "AVCPlan_View.aspx?AVCPlanID=" + CurID);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "AVCPlan_List.aspx");

    }
}
