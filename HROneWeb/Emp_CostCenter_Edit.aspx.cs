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

public partial class Emp_CostCenter_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "PER012";
    public Binding binding;
    public DBManager db = EEmpCostCenter.db;
    public EEmpCostCenter obj;
    public int CurID = -1;
    public int CurEmpID = -1;
    //public Hashtable CurEmpCostCenterGroups=new Hashtable();
    //public Hashtable CurRanks = new Hashtable();

    protected SearchBinding sbinding;
    protected ListInfo info;
    private DataView view;
    
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        
        binding = new Binding(dbConn, db);
        binding.add(EmpCostCenterID);
        binding.add(EmpID);
        binding.add(new TextBoxBinder(db, EmpCostCenterEffFr.TextBox,EmpCostCenterEffFr.ID));
        binding.add(new TextBoxBinder(db, EmpCostCenterEffTo.TextBox, EmpCostCenterEffTo.ID));
        binding.init(Request, Session);

        sbinding = new SearchBinding(dbConn, ECostCenter.db);

        info = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (!int.TryParse(DecryptedRequest["EmpCostCenterID"], out CurID))
            CurID = -1;

        if (!int.TryParse(DecryptedRequest["EmpID"], out CurEmpID))
            CurEmpID = -1;

        EmpID.Value = CurEmpID.ToString();
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            view = loadData(info, ECostCenter.db, Repeater);
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
	    obj=new EEmpCostCenter();
	    bool isNew=WebFormWorkers.loadKeys(db, obj, DecryptedRequest);

        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpID", CurEmpID));
        filter.add(WebUtils.AddRankFilter(Session, "EmpID", true));
        if (EEmpPersonalInfo.db.count(dbConn, filter) == 0)
            if (CurEmpID <= 0)
                return false;
            else
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");

        if (!db.select(dbConn, obj))
            if (CurID <= 0)
                return false;
            else
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");

        if (obj.EmpID != CurEmpID)
            if (CurID <= 0)
                return false;
            else
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");

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

        string select = " cc.* ";
        string from = "from  " + db.dbclass.tableName + " cc ";


        filter.add("CostCenterCode", true);

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);

        view = new DataView(table);



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
        WebFormUtils.LoadKeys(ECostCenter.db, row, cb);



        //if (!Page.IsPostBack)
        //{
            DBFilter empCostCenterDetailFilter = new DBFilter();
            empCostCenterDetailFilter.add(new Match("EmpCostCenterID", CurID));
            empCostCenterDetailFilter.add(new Match("CostCenterID", row["CostCenterID"]));
            ArrayList empCostCenterDetailList = EEmpCostCenterDetail.db.select(dbConn, empCostCenterDetailFilter);
            if (empCostCenterDetailList.Count != 0)
            {
                TextBox textBox = (TextBox)e.Item.FindControl("EmpCostCenterPercentage");
                textBox.Text = ((EEmpCostCenterDetail)empCostCenterDetailList[0]).EmpCostCenterPercentage.ToString("0.00");
                //cb.Checked = true;
            }
        //}
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        EEmpCostCenter c = new EEmpCostCenter();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);

        if (c.EmpCostCenterEffTo.Ticks > 0 && c.EmpCostCenterEffTo < c.EmpCostCenterEffFr)
        {
            errors.addError("EmpCostCenterEffTo", HROne.Translation.PageErrorMessage.ERROR_DATE_TO_TOO_EARLY);
            return;
        }

        AND andTerms = new AND();
        andTerms.add(new Match("EmpCostCenterID", "<>", c.EmpCostCenterID));
        andTerms.add(new Match("EmpCostCenterEffFr", "<=", c.EmpCostCenterEffFr));
        EEmpCostCenter lastObj = (EEmpCostCenter)AppUtils.GetLastObj(dbConn, db, "EmpCostCenterEffFr", c.EmpID, andTerms);
        if (lastObj != null && (c.EmpCostCenterEffFr <= lastObj.EmpCostCenterEffTo || c.EmpCostCenterEffFr == lastObj.EmpCostCenterEffFr))
        {
            errors.addError("EmpCostCenterEffFr", HROne.Translation.PageErrorMessage.ERROR_DATE_FROM_OVERLAP);
            return;
        }

        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpID", c.EmpID));
        filter.add(new Match("EmpCostCenterID", "<>", c.EmpCostCenterID));
        OR or = new OR();
        AND and;
        and = new AND();
        and.add(new Match("EmpCostCenterEffFr", "<=", c.EmpCostCenterEffFr));
        and.add(new Match("EmpCostCenterEffTo", ">=", c.EmpCostCenterEffFr));
        or.add(and);
        // do not allow early terms without "TO" date
        if (c.EmpCostCenterEffTo.Ticks.Equals(0))
            or.add(new Match("EmpCostCenterEffFr", ">", c.EmpCostCenterEffFr));
        if (c.EmpCostCenterEffTo > DateTime.MinValue)
        {
            and = new AND();
            and.add(new Match("EmpCostCenterEffFr", "<=", c.EmpCostCenterEffTo));
            and.add(new Match("EmpCostCenterEffTo", ">=", c.EmpCostCenterEffTo));
            or.add(and);

            and = new AND();
            and.add(new Match("EmpCostCenterEffFr", ">=", c.EmpCostCenterEffFr));
            and.add(new Match("EmpCostCenterEffFr", "<=", c.EmpCostCenterEffTo));
            or.add(and);
        }
        filter.add(or);
        if (db.count(dbConn, filter) > 0)
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_TERMS_OVERLAP);

        if (!errors.isEmpty())
            return;

        double totalPercentage=0;
        ArrayList selectedList = new ArrayList();
        ArrayList unselectedList = new ArrayList();

        foreach (RepeaterItem i in Repeater.Items)
        {
            CheckBox cb = (CheckBox)i.FindControl("ItemSelect");
            TextBox textBox = (TextBox)i.FindControl("EmpCostCenterPercentage");
            ECostCenter o = new ECostCenter();
            WebFormUtils.GetKeys(ECostCenter.db, o, cb);
            if (ECostCenter.db.select(dbConn, o))
            {
                double percentage = 0;
                if (!string.IsNullOrEmpty(textBox.Text.Trim()))
                    if (double.TryParse(textBox.Text, out percentage))
                    {
                        EEmpCostCenterDetail empCostCenterDetail = new EEmpCostCenterDetail();
                        empCostCenterDetail.CostCenterID = o.CostCenterID;
                        empCostCenterDetail.EmpCostCenterPercentage = percentage;
                        selectedList.Add(empCostCenterDetail);
                        totalPercentage += percentage;
                    }
                    else
                        errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_COSTCENTER_PERECNTAGE_NOT_NUMERIC, new string[] { o.CostCenterCode }));
                else
                    unselectedList.Add(o);
            }
        }

        if (Math.Abs(Math.Round(totalPercentage, 2) - 100) >= 0.01)
        {
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_TOTAL_PERCENTAGE_NOT_100);
        }

        if (!errors.isEmpty())
            return;

        WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
        if (CurID < 0)
        {
//            Utils.MarkCreate(Session, c);

            db.insert(dbConn, c);
            CurID = c.EmpCostCenterID;
//            url = Utils.BuildURL(-1, CurID);
        }
        else
        {
//            Utils.Mark(Session, c);
            db.update(dbConn, c);
        }

        foreach (EEmpCostCenterDetail o in selectedList)
        {
            DBFilter costCenterDetailFilter = new DBFilter();
            costCenterDetailFilter.add(new Match("EmpCostCenterID", c.EmpCostCenterID));
            costCenterDetailFilter.add(new Match("CostCenterID", o.CostCenterID));
            ArrayList empCostCenterDetailList = EEmpCostCenterDetail.db.select(dbConn, costCenterDetailFilter);
            if (empCostCenterDetailList.Count == 0)
            {
                o.EmpCostCenterID = c.EmpCostCenterID;
                EEmpCostCenterDetail.db.insert(dbConn, o);
            }
            else
            {
                int count = 0;
                foreach (EEmpCostCenterDetail empCostCenterDetail in empCostCenterDetailList)
                {
                    if (count == 0)
                    {
                        empCostCenterDetail.EmpCostCenterPercentage = o.EmpCostCenterPercentage;
                        EEmpCostCenterDetail.db.update(dbConn, empCostCenterDetail);
                    }
                    else
                        EEmpCostCenterDetail.db.delete(dbConn, empCostCenterDetail);
                    count++;
                }
            }
        }

        foreach (ECostCenter o in unselectedList)
        {
            DBFilter costCenterDetailFilter = new DBFilter();
            costCenterDetailFilter.add(new Match("EmpCostCenterID", c.EmpCostCenterID));
            costCenterDetailFilter.add(new Match("CostCenterID", o.CostCenterID));
            ArrayList costCenterDetailList = EEmpCostCenterDetail.db.select(dbConn, costCenterDetailFilter);
            if (costCenterDetailList.Count != 0)
            {
                foreach (EEmpCostCenterDetail empCostCenterDetail in costCenterDetailList)
                    EEmpCostCenterDetail.db.delete(dbConn, empCostCenterDetail);
            }
        }


        if (lastObj != null)
        {
            if (lastObj.EmpCostCenterEffTo < lastObj.EmpCostCenterEffFr)
            {
                lastObj.EmpCostCenterEffTo = c.EmpCostCenterEffFr.AddDays(-1);
                db.update(dbConn, lastObj);
            }
        }
        WebUtils.EndFunction(dbConn);

        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_CostCenter_View.aspx?EmpCostCenterID="+CurID+ "&EmpID=" + c.EmpID);


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

        view = loadData(info, ECostCenter.db, Repeater);

    }
    protected void Delete_Click(object sender, EventArgs e)
    {

        EEmpCostCenter c = new EEmpCostCenter();
        c.EmpCostCenterID = CurID;
        if (EEmpCostCenter.db.select(dbConn, c))
        {
            WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
            db.delete(dbConn, c);
            DBFilter costCenterDetailFilter = new DBFilter();
            costCenterDetailFilter.add(new Match("EmpCostCenterID", c.EmpCostCenterID));
            ArrayList empCostCenterDetailList = EEmpCostCenterDetail.db.select(dbConn, costCenterDetailFilter);
            foreach (EEmpCostCenterDetail empCostCenterDetail in empCostCenterDetailList)
            {
                EEmpCostCenterDetail.db.delete(dbConn, empCostCenterDetail);
            }
            WebUtils.EndFunction(dbConn);
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_CostCenter_View.aspx?EmpID=" + EmpID.Value);
 
   }

    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_CostCenter_View.aspx?EmpCostCenterID=" + CurID + "&EmpID=" + EmpID.Value);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_CostCenter_View.aspx?EmpID=" + EmpID.Value);

    }
}
