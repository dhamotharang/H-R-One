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

public partial class CessationReason : HROneWebPage
{
    private const string FUNCTION_CODE = "SYS011";
    
    protected DBManager db = ECessationReason.db;
    protected SearchBinding sbinding;
    protected ListInfo info;
    protected DataView view;
    public Binding binding;
    public Binding ebinding;
    public ECessationReason obj;
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



        binding = new Binding(dbConn, db);
        binding.add(CessationReasonCode);
        binding.add(CessationReasonDesc);
        binding.add(new CheckBoxBinder(db, CessationReasonIsSeverancePay));
        binding.add(new CheckBoxBinder(db, CessationReasonIsLongServicePay));
        binding.add(new CheckBoxBinder(db, CessationReasonHasProrataYEB));

        binding.init(Request, Session);

        sbinding = new SearchBinding(dbConn, db);
        sbinding.init(DecryptedRequest, null);
        sbinding.initValues("CessationReasonIsSeverancePay", null, Values.VLYesNo, null);
        sbinding.initValues("CessationReasonIsLongServicePay", null, Values.VLYesNo, null);
        sbinding.initValues("CessationReasonHasProrataYEB", null, Values.VLYesNo, null);

        info = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
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

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);

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
        ECessationReason c = new ECessationReason();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);
        if (!AppUtils.checkDuplicate(dbConn, db, c, errors, "CessationReasonCode"))
            return;

        WebUtils.StartFunction(Session, FUNCTION_CODE);
        db.insert(dbConn, c);
        WebUtils.EndFunction(dbConn);

        CessationReasonCode.Text = string.Empty;
        CessationReasonDesc.Text = string.Empty;
        CessationReasonIsLongServicePay.Checked = false;
        CessationReasonIsSeverancePay.Checked = false;
        CessationReasonHasProrataYEB.Checked = false;

        view = loadData(info, db, Repeater);
    }

    protected void Repeater_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        e.Item.FindControl("DeleteItem").Visible = IsAllowEdit;
        if (e.Item.ItemIndex == Repeater.EditItemIndex)
        {
            ebinding = new Binding(dbConn, db);
            ebinding.add((HtmlInputHidden)e.Item.FindControl("CessationReasonID"));
            ebinding.add((TextBox)e.Item.FindControl("CessationReasonCode"));
            ebinding.add((TextBox)e.Item.FindControl("CessationReasonDesc"));
            CheckBox SPcb = (CheckBox)e.Item.FindControl("CessationReasonIsSeverancePay");
            ebinding.add(new CheckBoxBinder(db, SPcb));
            CheckBox LSPcb = (CheckBox)e.Item.FindControl("CessationReasonIsLongServicePay");
            ebinding.add(new CheckBoxBinder(db, LSPcb));
            ebinding.add(new CheckBoxBinder(db, (CheckBox)e.Item.FindControl("CessationReasonHasProrataYEB")));
            ebinding.init(Request, Session);


            ECessationReason obj = new ECessationReason();
            db.toObject(((DataRowView)e.Item.DataItem).Row, obj);
            Hashtable values = new Hashtable();
            db.populate(obj, values);
            ebinding.toControl(values);
        }
        else
        {
            e.Item.FindControl("Edit").Visible = IsAllowEdit;
            HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("CessationReasonID");
            h.Value=((DataRowView)e.Item.DataItem)["CessationReasonID"].ToString();
        }
        HROne.Common.WebUtility.WebControlsLocalization(Session, e.Item.Controls);
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
            ebinding.add((HtmlInputHidden)e.Item.FindControl("CessationReasonID"));
            ebinding.add((TextBox)e.Item.FindControl("CessationReasonCode"));
            ebinding.add((TextBox)e.Item.FindControl("CessationReasonDesc"));
            CheckBox SPcb = (CheckBox)e.Item.FindControl("CessationReasonIsSeverancePay");
            ebinding.add(new CheckBoxBinder(db, SPcb));
            CheckBox LSPcb = (CheckBox)e.Item.FindControl("CessationReasonIsLongServicePay");
            ebinding.add(new CheckBoxBinder(db, LSPcb));
            ebinding.add(new CheckBoxBinder(db, (CheckBox)e.Item.FindControl("CessationReasonHasProrataYEB")));

            ebinding.init(Request, Session);


            ECessationReason obj = new ECessationReason();
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
            if (!AppUtils.checkDuplicate(dbConn, db, obj, errors, "CessationReasonCode"))
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
            HtmlInputHidden h = (HtmlInputHidden)item.FindControl("CessationReasonID");
            if (c.Checked)
            {
                ECessationReason obj = new ECessationReason();
                obj.CessationReasonID = Int32.Parse(h.Value);
                list.Add(obj);
            }
        }
        foreach (ECessationReason obj in list)
        {
            db.select(dbConn, obj);
            DBFilter empTerminationFilter = new DBFilter();
            empTerminationFilter.add(new Match("CessationReasonID", obj.CessationReasonID));
            empTerminationFilter.add("empid", true);
            ArrayList empTermList = EEmpTermination.db.select(dbConn, empTerminationFilter);
            if (empTermList.Count > 0)
            {
                errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_USED_BY_EMPLOYEE, new string[] { HROne.Common.WebUtility.GetLocalizedString("Cessation Reason Code"), obj.CessationReasonCode }));
                foreach (EEmpTermination empTerm in empTermList)
                {
                    EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                    empInfo.EmpID = empTerm.EmpID;
                    if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                        errors.addError("- " + empInfo.EmpNo + ", " + empInfo.EmpEngFullName);
                    else
                        EEmpTermination.db.delete(dbConn, empTerm);

                }
                errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
                view = loadData(info, db, Repeater);
                return;

            }
            else
            {

                WebUtils.StartFunction(Session, FUNCTION_CODE);
                db.delete(dbConn, obj);
                WebUtils.EndFunction(dbConn);
            }
        }
        view = loadData(info, db, Repeater);
    }
}
