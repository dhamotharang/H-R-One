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

public partial class Emp_Permit_List : HROneWebControl
{


    public int CurID = -1;
    private const string FUNCTION_CODE = "PER013";
    protected SearchBinding sbinding;
    public DBManager sdb = EEmpPermit.db;
    protected ListInfo info;
    protected DataView view;

    public Binding newBinding;
    public bool IsAllowEdit = true;

    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        //Delete.OnClientClick = HROne.Translation.PromptMessage.DELETE_GENERIC_JAVASCRIPT;

        //if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            //IsAllowEdit = false;
        toolBar.FunctionCode = FUNCTION_CODE;
        IsAllowEdit = toolBar.DeleteButton_Visible;
        SelectAllPanel.Visible = IsAllowEdit;

        //PreRender += new EventHandler(Emp_Permit_List_PreRender);

        if (!int.TryParse(DecryptedRequest["EmpID"], out CurID))
            CurID = -1;
        EmpID.Value = CurID.ToString();

        info = ListFooter.ListInfo;

        sbinding = new SearchBinding(dbConn, sdb);
        sbinding.add(new HiddenMatchBinder(EmpID));
        sbinding.initValues("PermitTypeID", null, EPermitType.VLPermitType, null);

        sbinding.init(DecryptedRequest, null);

        newBinding = new Binding(dbConn, sdb);
        newBinding.add(EmpID);
        newBinding.add(new DropDownVLBinder(sdb, PermitTypeID, EPermitType.VLPermitType));
        newBinding.add(EmpPermitNo);
        newBinding.add(new TextBoxBinder(sdb, EmpPermitIssueDate.TextBox, EmpPermitIssueDate.ID));
        newBinding.add(new TextBoxBinder(sdb, EmpPermitExpiryDate.TextBox, EmpPermitExpiryDate.ID));
        newBinding.add(EmpPermitRemark);

        newBinding.init(Request, Session);

        
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (view == null)
        {
            view = loadData(info, sdb, Repeater);
        }
    }

    public DataView loadData(ListInfo info, DBManager db, DataList repeater)
    {
        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpID", this.CurID));

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);

        string select = "c.* ";
        string from = "from [" + db.dbclass.tableName + "] c";

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

    protected void FirstPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, sdb, Repeater);
    }
    protected void PrevPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, sdb, Repeater);
    }
    protected void NextPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, sdb, Repeater);
    }
    protected void LastPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, sdb, Repeater);
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

        view = loadData(info, sdb, Repeater);

    }
    protected void Repeater_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        EEmpPermit obj = new EEmpPermit();
        sdb.toObject(((DataRowView)e.Item.DataItem).Row, obj);
        Hashtable values = new Hashtable();
        sdb.populate(obj, values);

        if (e.Item.ItemIndex == Repeater.EditItemIndex)
        {
            Binding eBinding;
            eBinding = new Binding(dbConn, sdb);
            eBinding.add((HtmlInputHidden)e.Item.FindControl("EmpPermitID"));
            eBinding.add(new DropDownVLBinder(sdb, (DropDownList)e.Item.FindControl("PermitTypeID"), EPermitType.VLPermitType));
            eBinding.add((TextBox)e.Item.FindControl("EmpPermitNo"));
            eBinding.add(new TextBoxBinder(sdb, ((WebDatePicker)e.Item.FindControl("EmpPermitIssueDate")).TextBox, "EmpPermitIssueDate"));
            eBinding.add(new TextBoxBinder(sdb, ((WebDatePicker)e.Item.FindControl("EmpPermitExpiryDate")).TextBox, "EmpPermitExpiryDate"));
            eBinding.add((TextBox)e.Item.FindControl("EmpPermitRemark"));


            eBinding.init(Request, Session);

            eBinding.toControl(values);
        }
        else
        {
            //e.Item.FindControl("Edit").Visible = true & IsAllowEdit;
            //e.Item.FindControl("DeleteItem").Visible = true & IsAllowEdit;

            HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("EmpPermitID");
            h.Value = ((DataRowView)e.Item.DataItem)["EmpPermitID"].ToString();
        }
        HROne.Common.WebUtility.WebControlsLocalization(Session, e.Item.Controls);
    }

    protected void Repeater_ItemCommand(object source, DataListCommandEventArgs e)
    {
        Button b = (Button)e.CommandSource;




        if (b.ID.Equals("Edit"))
        {
            Repeater.EditItemIndex = e.Item.ItemIndex;
            AddPanel.Visible = false;
            view = loadData(info, sdb, Repeater);
        }
        else if (b.ID.Equals("Cancel"))
        {
            Repeater.EditItemIndex = -1;
            AddPanel.Visible = IsAllowEdit;
            view = loadData(info, sdb, Repeater);
        }
        else if (b.ID.Equals("Save"))
        {
            Binding eBinding;
            eBinding = new Binding(dbConn, sdb);
            eBinding.add((HtmlInputHidden)e.Item.FindControl("EmpPermitID"));
            eBinding.add(new DropDownVLBinder(sdb, (DropDownList)e.Item.FindControl("PermitTypeID"), EPermitType.VLPermitType));
            eBinding.add((TextBox)e.Item.FindControl("EmpPermitNo"));
            eBinding.add(new TextBoxBinder(sdb, ((WebDatePicker)e.Item.FindControl("EmpPermitIssueDate")).TextBox, "EmpPermitIssueDate"));
            eBinding.add(new TextBoxBinder(sdb, ((WebDatePicker)e.Item.FindControl("EmpPermitExpiryDate")).TextBox, "EmpPermitExpiryDate"));
            eBinding.add((TextBox)e.Item.FindControl("EmpPermitRemark"));

            eBinding.init(Request, Session);


            EEmpPermit obj = new EEmpPermit();
            Hashtable values = new Hashtable();

            PageErrors errors = PageErrors.getErrors(sdb, Page.Master);
            errors.clear();


            eBinding.toValues(values);
            sdb.validate(errors, values);

            if (!errors.isEmpty())
            {
                return;
            }

            sdb.parse(values, obj);

            WebUtils.StartFunction(Session, FUNCTION_CODE, obj.EmpID);
            sdb.update(dbConn, obj);
            WebUtils.EndFunction(dbConn);

            Repeater.EditItemIndex = -1;
            AddPanel.Visible = IsAllowEdit;
            view = loadData(info, sdb, Repeater);
        }

    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        ArrayList list = new ArrayList();
        foreach (DataListItem item in Repeater.Items)
        {
            CheckBox c = (CheckBox)item.FindControl("DeleteItem");
            HtmlInputHidden h = (HtmlInputHidden)item.FindControl("EmpPermitID");
            if (c.Checked)
            {
                EEmpPermit obj = new EEmpPermit();
                obj.EmpPermitID = Int32.Parse(h.Value);
                list.Add(obj);
            }

        }
        if (list.Count > 0)
        {
            WebUtils.StartFunction(Session, FUNCTION_CODE, CurID);
            foreach (BaseObject o in list)
            {
                if (sdb.select(dbConn, o))
                    sdb.delete(dbConn, o);
            }
            WebUtils.EndFunction(dbConn);
        }
        loadData(info, sdb, Repeater);
    }
    public void New_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_Permit_Edit.aspx?EmpID=" + EmpID.Value);
    }
    protected void Add_Click(object sender, EventArgs e)
    {
        Repeater.EditItemIndex = -1;
        EEmpPermit c = new EEmpPermit();

        Hashtable values = new Hashtable();
        newBinding.toValues(values);

        PageErrors errors = PageErrors.getErrors(sdb, Page.Master);
        errors.clear();


        sdb.validate(errors, values);

        if (!errors.isEmpty())
            return;


        sdb.parse(values, c);

        WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
        sdb.insert(dbConn, c);
        WebUtils.EndFunction(dbConn);

        view = loadData(info, sdb, Repeater);
    }
}
