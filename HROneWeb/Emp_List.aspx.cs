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
using System.Globalization;
using HROne.Lib.Entities;
using HROne.Translation;

public partial class Emp_List : HROneWebPage
{
    private const string FUNCTION_CODE = "PER001";
    protected DBManager db = EEmpPersonalInfo.db;
    protected SearchBinding binding;
    protected ListInfo info;
    protected DataView view;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;
        info = ListFooter.ListInfo;
        SelectAllPanel.Visible = toolBar.DeleteButton_Visible;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        binding = new SearchBinding(dbConn, db);
        //binding.add(new LikeSearchBinder(EmpNo, "EmpNo"));
        //binding.add(new LikeSearchBinder(EmpEngSurname, "EmpEngSurname"));
        //binding.add(new LikeSearchBinder(EmpEngOtherName, "EmpEngOtherName"));
        //binding.add(new LikeSearchBinder(EmpChiFullName, "EmpChiFullName"));
        //binding.add(new LikeSearchBinder(EmpAlias, "EmpAlias"));
        //binding.add(new DropDownVLSearchBinder(EmpGender, "EmpGender", Values.VLGender));
        //binding.add(new FieldDateRangeSearchBinder(JoinDateFrom, JoinDateTo, "EmpDateOfJoin").setUseCurDate(false));
        //binding.add(new DropDownVLSearchBinder(EmpStatus, "EmpStatus", EEmpPersonalInfo.VLEmpStatus));
        binding.initValues("EmpStatus", null, EEmpPersonalInfo.VLEmpStatus, null);
        binding.init(DecryptedRequest, null);
    }

    protected void SetColumnHeader()
    {
        if (ESystemParameter.IsEnabled(dbConn, ESystemParameter.PARAM_CODE_EMP_LIST_SHOW_COMPANY))
            _Company.Text = "Company";
        else
            _Company.Text = "";

        if (ESystemParameter.IsEnabled(dbConn, ESystemParameter.PARAM_CODE_EMP_LIST_SHOW_H1))
        {
            DBFilter m_filter = new DBFilter();
            m_filter.add(new Match("HLevelSeqNo", 1));
            foreach (EHierarchyLevel m_level in EHierarchyLevel.db.select(dbConn, m_filter))
            {
                _Hierarchy1.Text = m_level.HLevelDesc;
                break;
            }
        }else
            _Hierarchy1.Text = "";


        if (ESystemParameter.IsEnabled(dbConn, ESystemParameter.PARAM_CODE_EMP_LIST_SHOW_H2))
        {
            DBFilter m_filter = new DBFilter();
            m_filter.add(new Match("HLevelSeqNo", 2));
            foreach (EHierarchyLevel m_level in EHierarchyLevel.db.select(dbConn, m_filter))
            {
                _Hierarchy2.Text = m_level.HLevelDesc;
                break;
            }
        }
        else
            _Hierarchy2.Text = "";

        if (ESystemParameter.IsEnabled(dbConn, ESystemParameter.PARAM_CODE_EMP_LIST_SHOW_H3))
        {
            DBFilter m_filter = new DBFilter();
            m_filter.add(new Match("HLevelSeqNo", 3));
            foreach (EHierarchyLevel m_level in EHierarchyLevel.db.select(dbConn, m_filter))
            {
                _Hierarchy3.Text = m_level.HLevelDesc;
                break;
            }
        }
        else
            _Hierarchy3.Text = "";
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //---------------------------------------
            // Show active employee 
            this.EmployeeSearchControl1.EmpStatusValue = "A";
            //            EmpStatus.SelectedValue = "A";
            //---------------------------------------
            //loadHierarchy();
            //loadState();


            //Start 0000068, Ricky So, 2014/08/06
            SetColumnHeader();
            //End 0000068, Ricky So, 2014/08/06
            view = loadData(info, db, Repeater);
        }

        if (WebUtils.TotalActiveEmployee(dbConn, 0) >= WebUtils.productLicense(Session).NumOfEmployees)
            toolBar.NewButton_Visible = false;
        else
            toolBar.NewButton_Visible = toolBar.DeleteButton_Visible;


    }

    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = binding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = "e.*";
        string from = "from [" + db.dbclass.tableName + "] e ";//LEFT JOIN " + EEmpPositionInfo.db.dbclass.tableName + " p ON c.EmpID=p.EmpID AND p.EmpPosEffTo IS NULL";
        //filter.add(WebUtils.AddRankFilter(Session, "e.EmpID", true));

        DBFilter empInfoFilter = EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime(), AppUtils.ServerDateTime());
        empInfoFilter.add(new MatchField("e.EmpID", "ee.EmpID"));


        filter.add(new Exists(EEmpPersonalInfo.db.dbclass.tableName + " ee", empInfoFilter));

        DataTable table = filter.loadData(dbConn, null, select, from);
        table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);
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
        if (view.Count == 1)
        {
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Emp_View.aspx?EmpID=" + view[0]["EmpID"]);
        }
    }
    protected void Reset_Click(object sender, EventArgs e)
    {
        binding.clear();
        EmployeeSearchControl1.Reset();
        EmployeeSearchControl1.EmpStatusValue = "A";
        info.page = 0;

        view = loadData(info, db, Repeater);

    }
    protected void FirstPage_Click(object sender, EventArgs e)
    {
        //loadState();
        //info.page = 0;
        view = loadData(info, db, Repeater);

    }
    protected void PrevPage_Click(object sender, EventArgs e)
    {
        //loadState();
        //info.page--;
        view = loadData(info, db, Repeater);

    }
    protected void NextPage_Click(object sender, EventArgs e)
    {
        //loadState();
        //info.page++;
        view = loadData(info, db, Repeater);

    }
    protected void LastPage_Click(object sender, EventArgs e)
    {
        //loadState();

        //info.page = Int32.Parse(NumPage.Value);
        view = loadData(info, db, Repeater);

    }
    protected void ChangeOrder_Click(object sender, EventArgs e)
    {
        LinkButton l = (LinkButton)sender;
        String id = l.ID.Substring(1);
        //loadState();
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


        //Start 0000068, Ricky So, 2014/08/06
        if (ESystemParameter.IsEnabled(dbConn, ESystemParameter.PARAM_CODE_EMP_LIST_SHOW_COMPANY) ||
            ESystemParameter.IsEnabled(dbConn, ESystemParameter.PARAM_CODE_EMP_LIST_SHOW_H1) ||
            ESystemParameter.IsEnabled(dbConn, ESystemParameter.PARAM_CODE_EMP_LIST_SHOW_H2) ||
            ESystemParameter.IsEnabled(dbConn, ESystemParameter.PARAM_CODE_EMP_LIST_SHOW_H3))
        {
            EEmpPersonalInfo obj = new EEmpPersonalInfo();
            db.toObject(((DataRowView)e.Item.DataItem).Row, obj);
            Hashtable values = new Hashtable();
            db.populate(obj, values);

            {
                HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("EmpID");
                h.Value = obj.EmpID.ToString();

                Label m_EmpCompany = (Label)e.Item.FindControl("EmpCompany");
                Label m_EmpHierarchy1 = (Label)e.Item.FindControl("EmpHierarchy1");
                Label m_EmpHierarchy2 = (Label)e.Item.FindControl("EmpHierarchy2");
                Label m_EmpHierarchy3 = (Label)e.Item.FindControl("EmpHierarchy3");            

                Binding ebinding = new Binding(dbConn, db);
                ebinding.init(Request, Session);
                ebinding.toControl(values);

                // find latest position
                DBFilter m_posFilter = new DBFilter();
                m_posFilter.add(new Match("EmpID", obj.EmpID));
                m_posFilter.add(new NullTerm("EmpPosEffTo"));

                foreach (EEmpPositionInfo m_pos in EEmpPositionInfo.db.select(dbConn, m_posFilter))
                {
                    if (ESystemParameter.IsEnabled(dbConn, ESystemParameter.PARAM_CODE_EMP_LIST_SHOW_COMPANY))
                    {
                        ECompany m_company = ECompany.GetObject(dbConn, m_pos.CompanyID);

                        if (m_company != null)
                            m_EmpCompany.Text = m_company.CompanyCode;
                    }

                    for (int i=1; i <= 3; i ++) // get first 3 hierarchy
                    {
                        DBFilter m_hierarchyLevelFilter = new DBFilter();
                        m_hierarchyLevelFilter.add(new Match("HLevelSeqNo", i));

                        foreach (EHierarchyLevel m_level in EHierarchyLevel.db.select(dbConn, m_hierarchyLevelFilter))
                        {
                            DBFilter m_empHierarchyFilter = new DBFilter();
                            m_empHierarchyFilter.add(new Match("EmpPosID", m_pos.EmpPosID));
                            m_empHierarchyFilter.add(new Match("HLevelID", m_level.HLevelID));

                            foreach (EEmpHierarchy m_empHierarchy in EEmpHierarchy.db.select(dbConn, m_empHierarchyFilter))
                            {
                                EHierarchyElement m_element = EHierarchyElement.GetObject(dbConn, m_empHierarchy.HElementID);
                                if (m_element != null)
                                {
                                    if (i == 1 && ESystemParameter.IsEnabled(dbConn, ESystemParameter.PARAM_CODE_EMP_LIST_SHOW_H1))
                                        m_EmpHierarchy1.Text = m_element.HElementCode;
                                    else if (i == 2 && ESystemParameter.IsEnabled(dbConn, ESystemParameter.PARAM_CODE_EMP_LIST_SHOW_H2))
                                        m_EmpHierarchy2.Text = m_element.HElementCode;
                                    else if (i == 3 && ESystemParameter.IsEnabled(dbConn, ESystemParameter.PARAM_CODE_EMP_LIST_SHOW_H3))
                                        m_EmpHierarchy3.Text = m_element.HElementCode;
                                }
                            }
                        }
                    }
                }
            }
        }
        //End 0000068, Ricky So, 2014/08/06
    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ArrayList list = WebUtils.SelectedRepeaterItemToBaseObjectList(db, Repeater, "ItemSelect");
        foreach (EEmpPersonalInfo o in list)
        {
            if (EEmpPersonalInfo.db.select(dbConn, o))
            {


                DBFilter empPayrollFilter = new DBFilter();
                empPayrollFilter.add(new Match("EmpID", o.EmpID));
                DBFilter paymentRecordFilter = new DBFilter();
                paymentRecordFilter.add(new IN("EmpPayrollID", "Select EmpPayrollID from [" + EEmpPayroll.db.dbclass.tableName + "]", empPayrollFilter));

                if (EPaymentRecord.db.count(dbConn, paymentRecordFilter) <= 0)
                {

                    WebUtils.StartFunction(Session, FUNCTION_CODE, o.EmpID);
                    EmpUtils.DeleteEmp(dbConn, o.EmpID);
                    db.delete(dbConn, o);
                    WebUtils.EndFunction(dbConn);
                }
                else
                {
                    errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_DELETE_EMP_PAYMENT_EXISTS, new string[] { o.EmpNo }));
                }
            }
        }
        //loadState();
        loadData(info, db, Repeater);
    }
    public void New_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_Edit.aspx");
    }
    //protected void loadHierarchy()
    //{
    //    DBFilter filter;
    //    ArrayList list;


    //    filter = new DBFilter();
    //    filter.add("HLevelSeqNo", true);
    //    list = EHierarchyLevel.db.select(dbConn, filter);
    //    HierarchyLevel.DataSource = list;
    //    HierarchyLevel.DataBind();


    //}
    protected void HierarchyLevel_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        EHierarchyLevel level = (EHierarchyLevel)e.Item.DataItem;
        DBFilter filter = new DBFilter();
        filter.add(new Match("HLevelID", level.HLevelID));
       
        DropDownList c = (DropDownList)e.Item.FindControl("HElementID");
        string selected = c.SelectedValue;
        WebFormUtils.loadValues(dbConn, c, EHierarchyElement.VLHierarchyElement, filter, ci, selected, "combobox.notselected");
        c.Attributes["HLevelID"] = level.HLevelID.ToString();


    }
}
