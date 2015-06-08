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

public partial class Payroll_WorkingSummary_View : HROneWebPage
{
    private const string FUNCTION_CODE = "PAY013";
    protected SearchBinding sbinding;
    protected ListInfo info;
    //protected DataView view;

    //public Binding binding;
    public Binding newBinding;
    public DBManager db = EEmpWorkingSummary.db;
    public EEmpPersonalInfo obj;
    public int CurID = -1;

    private bool IsAllowEdit = true;
    


    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;
        if (!toolBar.DeleteButton_Visible)
        {
            WorkingSummaryAddPanel.Visible = false;
            //    Delete.Visible = false;
            IsAllowEdit = false;
        }

        //binding = new Binding(dbConn, EEmpPersonalInfo.db);
        //binding.add(EmpID);


        sbinding = new SearchBinding(dbConn, db);
        sbinding.add(new HiddenMatchBinder(EmpID));
        sbinding.init(DecryptedRequest, null);

        newBinding = new Binding(dbConn, db);
        newBinding.add(EmpID);
        newBinding.add(new TextBoxBinder(db, EmpWorkingSummaryAsOfDate.TextBox, EmpWorkingSummaryAsOfDate.ID));
        newBinding.add(EmpWorkingSummaryRestDayEntitled);
        newBinding.add(EmpWorkingSummaryRestDayTaken);
        newBinding.add(EmpWorkingSummaryTotalWorkingDays);
        newBinding.add(EmpWorkingSummaryTotalWorkingHours);
        newBinding.add(EmpWorkingSummaryTotalLunchTimeHours);

        newBinding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["EmpID"], out CurID))
            CurID = -1;
        EmpID.Value = CurID.ToString();

        if (CurID <= 0)
        {
            WorkingSummaryAddPanel.Visible = false;
            toolBar.DeleteButton_Visible = false;
            IsAllowEdit = false;
        }

        info = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (CurID > 0)
                if (!loadObject())
                {
                    WorkingSummaryAddPanel.Visible = false;
                    toolBar.DeleteButton_Visible = false;
                    IsAllowEdit = false;
                }
            loadData(info, db , Repeater);
        }
        
    }


    public DataView loadData(ListInfo info, DBManager db, DataList repeater)
    {
        DBFilter filter = sbinding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //{
        //    info.orderby = "is_processed, " + info.orderby;
        //}
        //else
        //{
        //    info.orderby = "is_processed, CNDEffDate";
        //    info.order = false;
        //}

        //string select = "c.*, (CASE WHEN payrecid>0 THEN 1 Else 0 END) is_processed ";
        //string from = "from " + db.dbclass.tableName + " c ";

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
        {
        }
        else
        {
            //info.orderby = "CNDEffDate";
            //info.order = false;
        }

        string select = "c.* ";
        string from = "from " + db.dbclass.tableName + " c ";

        filter.add(WebUtils.AddRankFilter(Session, "c.EmpID", true));
        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);
        DataView view = new DataView(table);
        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }

        return view;
    }

    protected bool loadObject()
    {
        obj = new EEmpPersonalInfo();
        bool isNew = WebFormWorkers.loadKeys(EEmpPersonalInfo.db, obj, DecryptedRequest);

        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpID", obj.EmpID));
        filter.add(WebUtils.AddRankFilter(Session, "EmpID", true));
        ArrayList empInfoList = EEmpPersonalInfo.db.select(dbConn, filter);
        if (empInfoList.Count == 0)
            return false;
        obj = (EEmpPersonalInfo)empInfoList[0];

        //Hashtable values = new Hashtable();
        //EEmpPersonalInfo.db.populate(obj, values);
        //binding.toControl(values);
        ucEmp_Header.CurrentEmpID  = obj.EmpID ;
        return true;
    }

 
    protected void ChangeOrder_Click(object sender, EventArgs e)
    {
        LinkButton l = (LinkButton)sender;
        String id = l.ID.Substring(1);
        if (info.orderby == null)
            info.order = true;
        else if (info.orderby.Contains(id))
            info.order = !info.order;
        else
            info.order = true;
        info.orderby = id;

        Repeater.EditItemIndex = -1;
        loadData(info, db, Repeater);

    }

    protected void Add_Click(object sender, EventArgs e)
    {

        Repeater.EditItemIndex = -1;
        EEmpWorkingSummary c = new EEmpWorkingSummary();

        Hashtable values = new Hashtable();
        newBinding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);

        if (!errors.isEmpty())
            return;

        WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
        db.insert(dbConn, c);
        WebUtils.EndFunction(dbConn);


        EmpWorkingSummaryAsOfDate.Value = string.Empty;
        EmpWorkingSummaryRestDayEntitled.Text = string.Empty;
        EmpWorkingSummaryRestDayTaken.Text = string.Empty;
        EmpWorkingSummaryTotalWorkingDays.Text = string.Empty;
        EmpWorkingSummaryTotalWorkingHours.Text = string.Empty;
        EmpWorkingSummaryTotalLunchTimeHours.Text = string.Empty;


        loadData(info, db, Repeater);
    }

    protected void Repeater_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        Binding ebinding;

        EEmpWorkingSummary obj = new EEmpWorkingSummary();
        db.toObject(((DataRowView)e.Item.DataItem).Row, obj);

        if (e.Item.ItemIndex == Repeater.EditItemIndex)
        {
            ebinding = new Binding(dbConn, db);



            ebinding.add(new TextBoxBinder(db, ((WebDatePicker)e.Item.FindControl("EmpWorkingSummaryAsOfDate")).TextBox, "EmpWorkingSummaryAsOfDate"));
            ebinding.add((HtmlInputHidden)e.Item.FindControl("EmpWorkingSummaryID"));
            ebinding.add((TextBox)e.Item.FindControl("EmpWorkingSummaryRestDayEntitled"));
            ebinding.add((TextBox)e.Item.FindControl("EmpWorkingSummaryRestDayTaken"));
            ebinding.add((TextBox)e.Item.FindControl("EmpWorkingSummaryTotalWorkingDays"));
            ebinding.add((TextBox)e.Item.FindControl("EmpWorkingSummaryTotalWorkingHours"));
            ebinding.add((TextBox)e.Item.FindControl("EmpWorkingSummaryTotalLunchTimeHours"));



            ebinding.init(Request, Session);

            Hashtable values = new Hashtable();
            db.populate(obj, values);

            ebinding.toControl(values);
        }
        else
        {
            ((Button)e.Item.FindControl("Edit")).Visible = IsAllowEdit;
            ((CheckBox)e.Item.FindControl("DeleteItem")).Visible = IsAllowEdit;
            HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("EmpWorkingSummaryID");
            h.Value = ((DataRowView)e.Item.DataItem)["EmpWorkingSummaryID"].ToString();
        }

        HROne.Common.WebUtility.WebControlsLocalization(this, e.Item.Controls);

    }

    protected void Repeater_ItemCommand(object source, DataListCommandEventArgs e)
    {
        Button b = (Button)e.CommandSource;




        if (b.ID.Equals("Edit"))
        {
            Repeater.EditItemIndex = e.Item.ItemIndex;
            //WorkingSummaryAddPanel.Visible = false;
            loadData(info, db, Repeater);
            WebUtils.SetEnabledControlSection(WorkingSummaryAddPanel, false);
        }
        else if (b.ID.Equals("Cancel"))
        {
            Repeater.EditItemIndex = -1;
            //WorkingSummaryAddPanel.Visible = IsAllowEdit;
            loadData(info, db, Repeater);
            WebUtils.SetEnabledControlSection(WorkingSummaryAddPanel, true);
        }
        else if (b.ID.Equals("Save"))
        {
            Binding ebinding;


            ebinding = new Binding(dbConn, db);
            ebinding.add(EmpID);
            ebinding.add(new TextBoxBinder(db, ((WebDatePicker)e.Item.FindControl("EmpWorkingSummaryAsOfDate")).TextBox, "EmpWorkingSummaryAsOfDate"));
            ebinding.add((HtmlInputHidden)e.Item.FindControl("EmpWorkingSummaryID"));
            ebinding.add((TextBox)e.Item.FindControl("EmpWorkingSummaryRestDayEntitled"));
            ebinding.add((TextBox)e.Item.FindControl("EmpWorkingSummaryRestDayTaken"));
            ebinding.add((TextBox)e.Item.FindControl("EmpWorkingSummaryTotalWorkingDays"));
            ebinding.add((TextBox)e.Item.FindControl("EmpWorkingSummaryTotalWorkingHours"));
            ebinding.add((TextBox)e.Item.FindControl("EmpWorkingSummaryTotalLunchTimeHours"));

            ebinding.init(Request, Session);


            EEmpWorkingSummary obj = new EEmpWorkingSummary();
            Hashtable values = new Hashtable();

            PageErrors errors = PageErrors.getErrors(db, Page.Master);
            errors.clear();


            ebinding.toValues(values);
            db.validate(errors, values);

            if (!errors.isEmpty())
            {
                return;
            }


            db.parse(values, obj);


            if (!errors.isEmpty())
            {
                return;
            }

            WebUtils.StartFunction(Session, FUNCTION_CODE, obj.EmpID);
            db.update(dbConn, obj);
            WebUtils.EndFunction(dbConn);

            Repeater.EditItemIndex = -1;
            //WorkingSummaryAddPanel.Visible = IsAllowEdit;
            loadData(info, db, Repeater);
            WebUtils.SetEnabledControlSection(WorkingSummaryAddPanel, true);
        }


    }

    protected void Delete_Click(object sender, EventArgs e)
    {

        ArrayList list = new ArrayList();
        foreach (DataListItem item in Repeater.Items)
        {
            CheckBox c = (CheckBox)item.FindControl("DeleteItem");
            HtmlInputHidden h = (HtmlInputHidden)item.FindControl("EmpWorkingSummaryID");
            if (c.Checked)
            {
                EEmpWorkingSummary obj = new EEmpWorkingSummary();
                obj.EmpWorkingSummaryID = Int32.Parse(h.Value);
                list.Add(obj);
            }
        }
        WebUtils.StartFunction(Session, FUNCTION_CODE, CurID);
        foreach (EEmpWorkingSummary obj in list)
        {
            if (db.select(dbConn, obj))
                db.delete(dbConn, obj);
        }
        WebUtils.EndFunction(dbConn);
        loadData(info, db, Repeater);
    }
    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_WorkingSummary_List.aspx");
    }
}
