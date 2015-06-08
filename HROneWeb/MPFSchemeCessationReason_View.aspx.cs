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

public partial class MPFSchemeCessationReason_View : HROneWebPage
{
    private const string FUNCTION_CODE = "MPF005";

    public Binding binding;
    public DBManager db = EMPFSchemeCessationReason.db;
    public int CurID = -1;
    public EMPFScheme MPFScheme;
    protected SearchBinding cessationReasonSBinding;
    protected ListInfo info;
    private DataView view;
    //private bool IsAllowEdit = true;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;



        binding = new Binding(dbConn, db);
        binding.add(MPFSchemeCessationReasonID);
        binding.add(MPFSchemeID);
        binding.add(MPFSchemeCessationReasonCode);
        binding.add(MPFSchemeCessationReasonDesc);
        binding.init(Request, Session);

        cessationReasonSBinding = new SearchBinding(dbConn, ECessationReason.db);
        cessationReasonSBinding.init(DecryptedRequest, null);

        if (!int.TryParse(DecryptedRequest["MPFSchemeCessationReasonID"], out CurID))
            CurID = -1;

        info = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack) 
		{
            if (CurID > 0)
            {
                loadObject();
                loadData(info,EMPFSchemeCessationReasonMapping.db,Repeater);
            }
            else
            {
                MPFSchemeID.Value = DecryptedRequest["MPFSchemeID"];
            }

		}

        MPFScheme = new EMPFScheme();
        MPFScheme.MPFSchemeID = Int32.Parse(MPFSchemeID.Value);
        EMPFScheme.db.select(dbConn, MPFScheme);
    }

    protected bool loadObject() 
    {
        EMPFSchemeCessationReason obj = new EMPFSchemeCessationReason();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
        if (!db.select(dbConn, obj))
            return false;

	    Hashtable values=new Hashtable();
	    db.populate(obj,values);
	    binding.toControl(values);
        return true;
    }
    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = new DBFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = " cr.* , mscrm.*";
        string from = "from [" + db.dbclass.tableName + "] mscrm, CessationReason cr ";

        filter.add(new MatchField("mscrm.CessationReasonID", "cr.CessationReasonID"));
        filter.add(new Match("MPFSchemeCessationReasonID", CurID));
        if (string.IsNullOrEmpty(MPFSchemeID.Value))
            filter.add(new Match("MPFSchemeID", MPFSchemeID.Value));
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
    protected void FirstPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, EMPFSchemeCessationReasonMapping.db, Repeater);

    }
    protected void PrevPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, EMPFSchemeCessationReasonMapping.db, Repeater);

    }
    protected void NextPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, EMPFSchemeCessationReasonMapping.db, Repeater);

    }
    protected void LastPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, EMPFSchemeCessationReasonMapping.db, Repeater);
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

        view = loadData(info, EMPFSchemeCessationReasonMapping.db, Repeater);

    }
    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(EMPFSchemeCessationReasonMapping.db, row, cb);
    }

    protected void Delete_Click(object sender, EventArgs e)
    {

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        EMPFSchemeCessationReason obj = new EMPFSchemeCessationReason();
        obj.MPFSchemeCessationReasonID = CurID;
        if (EMPFSchemeCessationReason.db.select(dbConn, obj))
        {

            WebUtils.StartFunction(Session, FUNCTION_CODE);
            db.delete(dbConn, obj);
            DBFilter dbFilter = new DBFilter();
            dbFilter.add(new Match("MPFSchemeCessationReasonID", CurID));
            EMPFSchemeCessationReasonMapping.db.delete(dbConn, dbFilter);

            WebUtils.EndFunction(dbConn);
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "MPFSchemeCessationReason_List.aspx?MPFSchemeID=" + MPFSchemeID.Value);

    }
    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "MPFSchemeCessationReason_Edit.aspx?MPFSchemeCessationReasonID=" + CurID);
    }
    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "MPFSchemeCessationReason_List.aspx?MPFSchemeID=" + MPFSchemeID.Value);
    }
}
