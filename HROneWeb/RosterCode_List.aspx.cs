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

public partial class RosterCode_List : HROneWebPage
{
    private const string FUNCTION_CODE = "ATT001";

    protected DBManager db = ERosterCode.db;
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
        //binding.initValues("RosterCodeLunch", null, Values.VLYesNo, HROne.Common.WebUtility.GetSessionCultureInfo(Session));
        //binding.initValues("RosterCodeOT", null, Values.VLYesNo, HROne.Common.WebUtility.GetSessionCultureInfo(Session));
        binding.initValues("RosterCodeType", null, ERosterCode.VLRosterType, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));

        binding.add(new LikeSearchBinder(RosterCode, "RosterCode"));
        binding.add(new LikeSearchBinder(RosterCodeDesc, "RosterCodeDesc"));
        binding.add(new DropDownVLSearchBinder(RosterCodeType, "RosterCodeType", ERosterCode.VLRosterType));

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
        RosterCodeType.SelectedIndex = 0;
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
    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ArrayList list = WebUtils.SelectedRepeaterItemToBaseObjectList(db, Repeater, "ItemSelect");

        foreach (ERosterCode o in list)
        {
            db.select(dbConn, o);
            DBFilter rosterFilter = new DBFilter();
            rosterFilter.add(new Match("RosterCodeID", o.RosterCodeID));
            rosterFilter.add("empid", true);
            ArrayList rosterList = ERosterTable.db.select(dbConn, rosterFilter);
            if (rosterList.Count > 0)
            {
                int curEmpID = 0;
                errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_USED_BY_EMPLOYEE, new string[] { HROne.Common.WebUtility.GetLocalizedString("Roster Code"), o.RosterCode }));
                foreach (ERosterTable empRosterTable in rosterList)
                {
                    EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                    empInfo.EmpID = empRosterTable.EmpID;
                    if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                        if (curEmpID != empRosterTable.EmpID)
                        {
                            errors.addError("- " + empInfo.EmpNo + ", " + empInfo.EmpEngFullName);
                            curEmpID = empRosterTable.EmpID;
                        }
                        else
                            ERosterTable.db.delete(dbConn, empRosterTable);

                }
                errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
            }
            else
            {
                DBFilter obj = new DBFilter();
                obj.add(new Match("RosterCodeID", o.RosterCodeID));
                ArrayList objList = ERosterCodeDetail.db.select(dbConn, obj);
                foreach (ERosterCodeDetail match in objList)
                    ERosterCodeDetail.db.delete(dbConn, match);

                WebUtils.StartFunction(Session, FUNCTION_CODE);
                db.delete(dbConn, o);
                WebUtils.EndFunction(dbConn);

            }
        }
        loadData(info, db, Repeater);
    }

    public void New_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "RosterCode_Edit.aspx");
    }
}
