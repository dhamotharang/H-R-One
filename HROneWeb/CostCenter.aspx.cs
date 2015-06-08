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

public partial class CostCenter : HROneWebPage
{
    private const string FUNCTION_CODE = "CST000";
    
    protected DBManager db = ECostCenter.db;
    protected SearchBinding sbinding;
    protected ListInfo info;
    protected DataView view;
    public Binding binding;
    public Binding ebinding;
    public ECostCenter obj;
    public int CurID = -1;
    private bool IsAllowEdit = true;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;
        if (!WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
        {
            IsAllowEdit = false;
        }
        AddPanel.Visible = IsAllowEdit;

        info = ListFooter.ListInfo;
        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        binding = new Binding(dbConn, db);
        binding.add(CostCenterCode);
        binding.add(CostCenterDesc);
        binding.init(Request, Session);

        sbinding = new SearchBinding(dbConn, db);
        sbinding.init(DecryptedRequest, null);


    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            view = loadData(info, db, Repeater);
        }

    }

    public DataView loadData(ListInfo info, DBManager db, DataList repeater)
    {
        DBFilter filter = sbinding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = "c.*";
        string from = "from " + db.dbclass.tableName + " c ";

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);

        foreach (DataRow row in table.Rows)
            foreach (DBField field in db.fields)
            {
                if (table.Columns.Contains(field.name))
                    if (row[field.name] != null)
                        if (field.transcoder != null)

                            row[field.name] = field.transcoder.fromDB(row[field.name]);
            }

        view = new DataView(table);
        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }

        return view;
    }
    protected void Search_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);

    }
    protected void Reset_Click(object sender, EventArgs e)
    {
        sbinding.clear();
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

        Repeater.EditItemIndex = -1;
        view = loadData(info, db, Repeater);

    }
    protected void Add_Click(object sender, EventArgs e)
    {
        Repeater.EditItemIndex = -1;
        ECostCenter c = new ECostCenter();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);
        if (!AppUtils.checkDuplicate(dbConn, db, c, errors, "CostCenterCode"))
            return;

        WebUtils.StartFunction(Session, FUNCTION_CODE);
        db.insert(dbConn, c);
        WebUtils.EndFunction(dbConn);

        CostCenterCode.Text = string.Empty;
        CostCenterDesc.Text = string.Empty;

        view = loadData(info, db, Repeater);
    }

    protected void Repeater_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        HROne.Common.WebUtility.WebControlsLocalization(Session, e.Item.Controls);
        e.Item.FindControl("DeleteItem").Visible = IsAllowEdit;
        if (e.Item.ItemIndex == Repeater.EditItemIndex)
        {
            ebinding = new Binding(dbConn, db);
            ebinding.add((HtmlInputHidden)e.Item.FindControl("CostCenterID"));
            ebinding.add((TextBox)e.Item.FindControl("CostCenterCode"));
            ebinding.add((TextBox)e.Item.FindControl("CostCenterDesc"));
            ebinding.init(Request, Session);


            ECostCenter obj = new ECostCenter();
            db.toObject(((DataRowView)e.Item.DataItem).Row, obj);
            Hashtable values = new Hashtable();
            db.populate(obj, values);
            ebinding.toControl(values);
        }
        else
        {
            e.Item.FindControl("Edit").Visible = IsAllowEdit;
            HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("CostCenterID");
            h.Value=((DataRowView)e.Item.DataItem)["CostCenterID"].ToString();
        }
    }

    protected void Repeater_ItemCommand(object source, DataListCommandEventArgs e)
    {
        Button b = (Button)e.CommandSource;


        

        if (b.ID.Equals("Edit"))
        {
            Repeater.EditItemIndex = e.Item.ItemIndex;
            view = loadData(info, db, Repeater);
            WebUtils.SetEnabledControlSection(AddPanel, false);
        }
        else if (b.ID.Equals("Cancel"))
        {
            Repeater.EditItemIndex = -1;
            view = loadData(info, db, Repeater);
            WebUtils.SetEnabledControlSection(AddPanel, true);
        }
        else if (b.ID.Equals("Save"))
        {
            ebinding = new Binding(dbConn, db);
            ebinding.add((HtmlInputHidden)e.Item.FindControl("CostCenterID"));
            ebinding.add((TextBox)e.Item.FindControl("CostCenterCode"));
            ebinding.add((TextBox)e.Item.FindControl("CostCenterDesc"));
            ebinding.init(Request, Session);


            ECostCenter obj = new ECostCenter();
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
            if (!AppUtils.checkDuplicate(dbConn, db, obj, errors, "CostCenterCode"))
                return;

            WebUtils.StartFunction(Session, FUNCTION_CODE);
            db.update(dbConn, obj);
            WebUtils.EndFunction(dbConn);

            Repeater.EditItemIndex = -1;
            view = loadData(info, db, Repeater);
            WebUtils.SetEnabledControlSection(AddPanel, true);
        }

        
    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ArrayList list = new ArrayList();
        foreach(DataListItem item in Repeater.Items) 
        {
            CheckBox c=(CheckBox)item.FindControl("DeleteItem");
            HtmlInputHidden h = (HtmlInputHidden)item.FindControl("CostCenterID");
            if (c.Checked)
            {
                ECostCenter obj = new ECostCenter();
                obj.CostCenterID = Int32.Parse(h.Value);
                list.Add(obj);
            }
        }
        foreach (ECostCenter obj in list)
        {
            if (ECostCenter.db.select(dbConn, obj))
            {
                {
                    DBFilter costAllocationDetailFilter = new DBFilter();
                    costAllocationDetailFilter.add(new Match("cad.CostCenterID", obj.CostCenterID));

                    DBFilter costAllocationFilter = new DBFilter();
                    costAllocationFilter.add(new IN("CostAllocationID", "SELECT DISTINCT cad.CostAllocationID FROM " + ECostAllocationDetail.db.dbclass.tableName + " cad", costAllocationDetailFilter));
                    costAllocationFilter.add("empid", true);
                    ArrayList CostAllocationList = ECostAllocation.db.select(dbConn, costAllocationFilter);
                    if (CostAllocationList.Count > 0)
                    {
                        int lastEmpID = 0;
                        errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_USED_BY_EMPLOYEE, new string[] { HROne.Common.WebUtility.GetLocalizedString("Cost Center Code"), obj.CostCenterCode }));
                        foreach (ECostAllocation costAllocation in CostAllocationList)
                        {
                            if (lastEmpID != costAllocation.EmpID)
                            {
                                EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                                empInfo.EmpID = costAllocation.EmpID;
                                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                                {
                                    errors.addError("- " + empInfo.EmpNo + ", " + empInfo.EmpEngFullName);
                                    lastEmpID = costAllocation.EmpID;
                                }
                                else
                                    ECostAllocation.db.delete(dbConn, costAllocation);

                            }
                        }
                        errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
                        view = loadData(info, db, Repeater);
                        return;

                    }
                }
                {
                    DBFilter empCostCenterDetailFilter = new DBFilter();
                    empCostCenterDetailFilter.add(new Match("eccd.CostCenterID", obj.CostCenterID));

                    DBFilter empCostCenterFilter = new DBFilter();
                    empCostCenterFilter.add(new IN("EmpCostCenterID", "SELECT DISTINCT eccd.CostCenterID FROM " + EEmpCostCenterDetail.db.dbclass.tableName + " eccd", empCostCenterDetailFilter));
                    empCostCenterFilter.add("empid", true);
                    ArrayList empCostCenterList = EEmpCostCenter.db.select(dbConn, empCostCenterFilter);
                    if (empCostCenterList.Count > 0)
                    {
                        int lastEmpID = 0;
                        errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_USED_BY_EMPLOYEE, new string[] { HROne.Common.WebUtility.GetLocalizedString("Cost Center Code"), obj.CostCenterCode }));
                        foreach (EEmpCostCenter empCostCenter in empCostCenterList)
                        {
                            if (lastEmpID != empCostCenter.EmpID)
                            {
                                EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                                empInfo.EmpID = empCostCenter.EmpID;
                                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                                {
                                    errors.addError("- " + empInfo.EmpNo + ", " + empInfo.EmpEngFullName);
                                    lastEmpID = empCostCenter.EmpID;
                                }
                                else
                                    EEmpCostCenter.db.delete(dbConn, empCostCenter);
                            }
                        }
                        errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
                        view = loadData(info, db, Repeater);
                        return;

                    }
                }
                if (errors.isEmpty())
                {
                    WebUtils.StartFunction(Session, FUNCTION_CODE);
                    db.delete(dbConn, obj);
                    WebUtils.EndFunction(dbConn);
                }
            }
        }
        view = loadData(info, db, Repeater);
    }
}
