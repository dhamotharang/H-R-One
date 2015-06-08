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

public partial class MPFSchemeCessationReason_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "MPF005";

    public Binding binding;
    public DBManager db = EMPFSchemeCessationReason.db;
    public int CurID = -1;
    public EMPFScheme MPFScheme;
    protected SearchBinding sbinding;
    protected ListInfo info;
    private DataView view;

    
    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;



        binding = new Binding(dbConn, db);
        binding.add(MPFSchemeCessationReasonID);
        binding.add(MPFSchemeID);
        binding.add(MPFSchemeCessationReasonCode);
        binding.add(MPFSchemeCessationReasonDesc);
        binding.init(Request, Session);
        sbinding = new SearchBinding(dbConn, ECessationReason.db);

        if (!int.TryParse(DecryptedRequest["MPFSchemeCessationReasonID"], out CurID))
            CurID = -1;

        info = ListFooter.ListInfo;
        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
		if(!Page.IsPostBack) 
		{
            if (CurID > 0)
            {
                loadObject();
            }
            else
            {
                MPFSchemeID.Value = DecryptedRequest["MPFSchemeID"];
                toolBar.DeleteButton_Visible = false;
            }
            view = loadData(info, db, Repeater);

		}


        MPFScheme = new EMPFScheme();
        MPFScheme.MPFSchemeID = Int32.Parse(MPFSchemeID.Value);
        EMPFScheme.db.select(dbConn, MPFScheme);
    }

    protected bool loadObject() 
    {
        EMPFSchemeCessationReason obj = new EMPFSchemeCessationReason();
	    bool isNew=WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
	    if(!db.select(dbConn, obj))
		    return false;

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

        string select = " cr.* ";
        string from = "from  CessationReason cr ";


        DBFilter notMPFSchemeCessationReasonFilter = new DBFilter();
        notMPFSchemeCessationReasonFilter.add(new Match("MPFSchemeID", "=", MPFSchemeID.Value));

        DBFilter notMPFSchemeCessationReasonMapFilter = new DBFilter();
        notMPFSchemeCessationReasonMapFilter.add(new Match("MPFSchemeCessationReasonID", "<>", CurID));
        notMPFSchemeCessationReasonMapFilter.add(new IN("MPFSchemeCessationReasonID", "Select MPFSchemeCessationReasonID from MPFSchemeCessationReason", notMPFSchemeCessationReasonFilter));

        IN notinTerms = new IN("NOT CessationReasonID", "select CessationReasonID from MPFSchemeCessationReasonMapping", notMPFSchemeCessationReasonMapFilter);
        DBFilter mpfSchemeCessationReasontMapFilter = new DBFilter();
        mpfSchemeCessationReasontMapFilter.add(new Match("MPFSchemeCessationReasonID", CurID));
        IN inTerms = new IN("CessationReasonID", "select CessationReasonID from MPFSchemeCessationReasonMapping", mpfSchemeCessationReasontMapFilter);

        OR orTerms = new OR();
        orTerms.add(notinTerms);
        orTerms.add(inTerms);
        filter.add(orTerms);
        filter.add("CessationReasonCode", true);

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);

        view = new DataView(table);

        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }

        return view;
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
        WebFormUtils.LoadKeys(ECessationReason.db, row, cb);



        //if (!Page.IsPostBack)
        //{
            DBFilter mpfSchemeCessationReasonMapFilter = new DBFilter();
            mpfSchemeCessationReasonMapFilter.add(new Match("MPFSchemeCessationReasonID", CurID));
            mpfSchemeCessationReasonMapFilter.add(new Match("CessationReasonID", row["CessationReasonID"]));
            ArrayList mpfSchemeCessationReasonMaps = EMPFSchemeCessationReasonMapping.db.select(dbConn, mpfSchemeCessationReasonMapFilter);
            if (mpfSchemeCessationReasonMaps.Count != 0)
                cb.Checked = true;
        //}
    }
    protected void Save_Click(object sender, EventArgs e)
    {
        EMPFSchemeCessationReason c = new EMPFSchemeCessationReason();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);
        if (!AppUtils.checkDuplicate(dbConn, db, c, errors, "MPFSchemeCessationReasonCode", new Match("MPFSchemeID", c.MPFSchemeID)))
            return;

        WebUtils.StartFunction(Session, FUNCTION_CODE);
        if (CurID < 0)
        {
//            Utils.MarkCreate(Session, c);

            db.insert(dbConn, c);
            CurID = c.MPFSchemeCessationReasonID;
//            url = Utils.BuildURL(-1, CurID);
        }
        else
        {
//            Utils.Mark(Session, c);
            db.update(dbConn, c);
        }

        ArrayList selectedList = new ArrayList();
        ArrayList unselectedList = new ArrayList();
        foreach (RepeaterItem i in Repeater.Items)
        {
            CheckBox cb = (CheckBox)i.FindControl("ItemSelect");
            BaseObject o = new ECessationReason();
            WebFormUtils.GetKeys(ECessationReason.db, o, cb);
            if (cb.Checked)
                selectedList.Add(o);
            else
                unselectedList.Add(o);

        }
        foreach (ECessationReason o in selectedList)
        {
            DBFilter mpfSchemeCessationReasonMapFilter = new DBFilter();
            mpfSchemeCessationReasonMapFilter.add(new Match("MPFSchemeCessationReasonID", c.MPFSchemeCessationReasonID));
            mpfSchemeCessationReasonMapFilter.add(new Match("CessationReasonID", o.CessationReasonID));
            ArrayList taxPaymentMaps = EMPFSchemeCessationReasonMapping.db.select(dbConn, mpfSchemeCessationReasonMapFilter);
            if (taxPaymentMaps.Count == 0)
            {
                EMPFSchemeCessationReasonMapping mpfSchemeCessationReasonMap = new EMPFSchemeCessationReasonMapping();
                mpfSchemeCessationReasonMap.MPFSchemeCessationReasonID = c.MPFSchemeCessationReasonID;
                mpfSchemeCessationReasonMap.CessationReasonID = o.CessationReasonID;
                EMPFSchemeCessationReasonMapping.db.insert(dbConn, mpfSchemeCessationReasonMap);
            }
        }

        foreach (ECessationReason o in unselectedList)
        {
            DBFilter mpfSchemeCessationReasonMapFilter = new DBFilter();
            mpfSchemeCessationReasonMapFilter.add(new Match("MPFSchemeCessationReasonID", c.MPFSchemeCessationReasonID));
            mpfSchemeCessationReasonMapFilter.add(new Match("CessationReasonID", o.CessationReasonID));
            ArrayList mpfSchemeCessationReasonMaps = EMPFSchemeCessationReasonMapping.db.select(dbConn, mpfSchemeCessationReasonMapFilter);
            if (mpfSchemeCessationReasonMaps.Count != 0)
            {
                foreach (EMPFSchemeCessationReasonMapping taxPaymentMap in mpfSchemeCessationReasonMaps)
                    EMPFSchemeCessationReasonMapping.db.delete(dbConn, taxPaymentMap);
            }
        }
        WebUtils.EndFunction(dbConn);
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "MPFSchemeCessationReason_View.aspx?MPFSchemeCessationReasonID=" + CurID);


    }
    protected void Delete_Click(object sender, EventArgs e)
    {

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


    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "MPFSchemeCessationReason_View.aspx?MPFSchemeCessationReasonID=" + CurID);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "MPFSchemeCessationReason_List.aspx?MPFSchemeID=" + MPFSchemeID.Value);

    }
}
