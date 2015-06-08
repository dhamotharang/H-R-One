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
using HROne.Translation;
using HROne.Lib.Entities;

public partial class Payroll_BonusProcess_List : HROneWebPage
{
    private const string FUNCTION_CODE = "PAY020";

    protected DBManager db = EBonusProcess.db;
    protected SearchBinding binding;
    protected ListInfo info;
    protected DataView view;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;
        SelectAllPanel.Visible = toolBar.DeleteButton_Visible;

        binding = new SearchBinding(dbConn, db);
        //binding.add(new LikeSearchBinder(MPFPlanCode, "MPFPlanCode"));
        //binding.add(new LikeSearchBinder(MPFPlanDesc, "MPFPlanDesc"));
        //binding.add(new LikeSearchBinder(MPFPlanSchemeName, "MPFSchemeDesc"));
        //binding.add(new LikeSearchBinder(MPFSchemeCode, "MPFSchemeCode"));
        //binding.add(new LikeSearchBinder(MPFPlanCompanyName, "MPFPlanCompanyName"));

        binding.init(DecryptedRequest, null);

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
        info = ListFooter.ListInfo;

        if (!Page.IsPostBack)
        {
            view = loadData(info, db, Repeater);
        }
        
    }
    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = binding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = "c.*, CONVERT(nvarchar(7), c.BonusProcessMonth, 21) BonusProcessMonth2 ";
        //string select = "c.* ";

        //string select = "c.*, l.MPFSchemeCode,l.MPFSchemeDesc";
        string from = "from [" + db.dbclass.tableName + "] c ";

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);
        
        view = new DataView(table);
        ListFooter.Refresh();

        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }

        return view;
    }
    protected void Search_Click(object sender, EventArgs e)
    {
        info.page = 0;
        view = loadData(info, db, Repeater);

    }
    protected void Reset_Click(object sender, EventArgs e)
    {
        binding.clear();
        info.page = 0;
        view = loadData(info, db, Repeater);

    }
    protected void FirstPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);
    }
    protected void PrevPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);
    }
    protected void NextPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);
    }
    protected void LastPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);
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

        view = loadData(info, db, Repeater);
    }

    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(db, row, cb);
        e.Item.FindControl("ItemSelect").Visible = toolBar.DeleteButton_Visible;

 
        Binding ebinding;

        EBonusProcess obj = new EBonusProcess();
        db.toObject(((DataRowView)e.Item.DataItem).Row, obj);

        if (obj.BonusProcessPayCodeID > 0)
        {
            //((HtmlAnchor)e.Item.FindControl("BonusProcessMonth")).InnerText = obj.BonusProcessMonth.ToString("yyyy-MM");
            ((Label)e.Item.FindControl("BonusProcessPayDate")).Text = obj.BonusProcessPayDate.ToString("yyyy-MM-dd");
            switch(obj.BonusProcessStatus)
            {
                case EBonusProcess.STATUS_CANCELLED:
                    ((Label)e.Item.FindControl("BonusProcessStatus")).Text = EBonusProcess.STATUS_CANCELLED_DESC;
                    break;
                case EBonusProcess.STATUS_NORMAL:
                    ((Label)e.Item.FindControl("BonusProcessStatus")).Text = EBonusProcess.STATUS_NORMAL_DESC;
                    break;
                case EBonusProcess.STATUS_CONFIRMED:
                    ((Label)e.Item.FindControl("BonusProcessStatus")).Text = EBonusProcess.STATUS_CONFIRMED_DESC;
                    break;
            }

            EPaymentCode m_payCode = EPaymentCode.GetObject(dbConn, obj.BonusProcessPayCodeID);
            if (m_payCode != null)
            {
//                ((Label)e.Item.FindControl("BonusProcessPayCode")).Text = m_payCode.PaymentCodeDesc;
                ((Label)e.Item.FindControl("BonusProcessPayCode")).Text = m_payCode.PaymentCodeDesc;
            }
        }

    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ArrayList list = WebUtils.SelectedRepeaterItemToBaseObjectList(db, Repeater, "ItemSelect");

        foreach (EBonusProcess o in list)
        {           
            if (db.select(dbConn, o))
            {
                if (o.BonusProcessStatus != EBonusProcess.STATUS_NORMAL)
                {
                    errors.addError("Bonus Process remove failed. (Status = '" + o.BonusProcessStatus + "')");
                }
                else
                {
                    WebUtils.StartFunction(Session, FUNCTION_CODE);
                    DBFilter m_filter = new DBFilter();
                    m_filter.add(new Match("BonusProcessID", o.BonusProcessID));
                    EEmpBonusProcess.db.delete(dbConn, m_filter);
                    db.delete(dbConn, o);
                    WebUtils.EndFunction(dbConn);
                }
            }

        }
        loadData(info, db, Repeater);
    }

    public void New_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_BonusProcess_Edit.aspx");
    }

}
