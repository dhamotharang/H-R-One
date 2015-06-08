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

public partial class LeavePlan_List : HROneWebPage
{
    private const string FUNCTION_CODE = "LEV003";

    protected DBManager db = ELeavePlan.db;
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
        binding.add(new LikeSearchBinder(LeavePlanCode, "LeavePlanCode"));
        binding.add(new LikeSearchBinder(LeavePlanDesc, "LeavePlanDesc"));

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

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);

        string select = "c.*";
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
    }

    // Start 0000006, Miranda, 2014-06-16 
    protected void Copy_Click(object sender, EventArgs e)
    {
        ArrayList list = WebUtils.SelectedRepeaterItemToBaseObjectList(db, Repeater, "ItemSelect");
        foreach (ELeavePlan o in list)
        {
            if (ELeavePlan.db.select(dbConn, o))
            {
                ELeavePlan newLeavePlan = o.Copy(dbConn);
                ELeavePlan.db.update(dbConn, newLeavePlan);

                DBFilter dbFilter = new DBFilter();
                dbFilter.add(new Match("LeavePlanID", o.LeavePlanID));
                ArrayList oldLeaveEntitleDetailList = ELeavePlanEntitle.db.select(dbConn, dbFilter);
                foreach (ELeavePlanEntitle leaveEntitlement in oldLeaveEntitleDetailList)
                {
                    leaveEntitlement.LeavePlanID = newLeavePlan.LeavePlanID;
                    ELeavePlanEntitle.db.insert(dbConn, leaveEntitlement);
                }
                // Start 0000006, Miranda, 2014-07-16
                DBFilter dbFilterBF = new DBFilter();
                dbFilterBF.add(new Match("LeavePlanID", o.LeavePlanID));
                ArrayList listBF = ELeavePlanBroughtForward.db.select(dbConn, dbFilterBF);
                if (listBF.Count > 0)
                {
                    foreach (ELeavePlanBroughtForward leavePlanBroughtForward in listBF)
                    {
                        leavePlanBroughtForward.LeavePlanID = newLeavePlan.LeavePlanID;
                        ELeavePlanBroughtForward.db.insert(dbConn, leavePlanBroughtForward);
                    }
                }
                // End 0000006, Miranda, 2014-07-16
            }
        }
        loadData(info, db, Repeater);
    }
    // End 0000006, Miranda, 2014-06-16

    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ArrayList list = WebUtils.SelectedRepeaterItemToBaseObjectList(db, Repeater, "ItemSelect");

        foreach (ELeavePlan o in list)
        {
            if (ELeavePlan.db.select(dbConn, o))
            {

                DBFilter empPosFilter = new DBFilter();
                empPosFilter.add(new Match("LeavePlanID", o.LeavePlanID));
                empPosFilter.add("empid", true);
                ArrayList empPosList = EEmpPositionInfo.db.select(dbConn, empPosFilter);
                if (empPosList.Count > 0)
                {
                    errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_USED_BY_EMPLOYEE, new string[] { HROne.Common.WebUtility.GetLocalizedString("Leave Plan Code"), o.LeavePlanCode }));
                    foreach (EEmpPositionInfo empPos in empPosList)
                    {
                        EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                        empInfo.EmpID = empPos.EmpID;
                        if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                            errors.addError("- " + empInfo.EmpNo + ", " + empInfo.EmpEngFullName);
                        else
                            EEmpPositionInfo.db.delete(dbConn, empPos);

                    }
                    errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);

                }
                else
                {
                    WebUtils.StartFunction(Session, FUNCTION_CODE);
                    db.delete(dbConn, o);
                    DBFilter dbFilter = new DBFilter();
                    dbFilter.add(new Match("LeavePlanID", o.LeavePlanID));
                    ArrayList leaveEntitleDetailList = ELeavePlanEntitle.db.select(dbConn, dbFilter);
                    foreach (ELeavePlanEntitle leaveEntitlement in leaveEntitleDetailList)
                        ELeavePlanEntitle.db.delete(dbConn, leaveEntitlement);
                    WebUtils.EndFunction(dbConn);
                }
            }
        }
        loadData(info, db, Repeater);
    }

    public void New_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "LeavePlan_Edit.aspx");
    }
}
