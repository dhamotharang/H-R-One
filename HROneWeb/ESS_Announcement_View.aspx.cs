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
public partial class ESS_Announcement_View : HROneWebPage
{
    private const string FUNCTION_CODE = "TLS001";

    public Binding binding;
    public SearchBinding rankBinding;
    public SearchBinding companyBinding;
    public DBManager db = EESSAnnouncement .db;
    public EESSAnnouncement obj;
    public int CurID = -1;
    public Hashtable CurRanks = new Hashtable();
    public Hashtable CurCompanies = new Hashtable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;

        binding = new Binding(dbConn, db);
        binding.add(ESSAnnouncementID);
        binding.add(ESSAnnouncementCode);
        binding.add(ESSAnnouncementContent);
        rankBinding = new SearchBinding(dbConn, ERank.db);
        companyBinding = new SearchBinding(dbConn, ECompany.db);
        binding.add(ESSAnnouncementEffectiveDate);
        binding.add(ESSAnnouncementExpiryDate);
        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["ESSAnnouncementID"], out CurID))
            CurID = -1;

        if (!Page.IsPostBack)
        {
            if (CurID > 0)
            {
                loadObject();
            }
            else
            {
                toolBar.DeleteButton_Visible = false;
            }
            loadCompanies();
            loadRanks();
        }
    }

    protected bool loadObject() 
    {
	    obj=new EESSAnnouncement ();
	    bool isNew=WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
	    if(!db.select(dbConn, obj))
		    return false;

	    Hashtable values=new Hashtable();
	    db.populate(obj,values);
	    binding.toControl(values);
        string companies = obj.ESSAnnouncementTargetCompanies;
        string ranks = obj.ESSAnnouncementTargetRanks;
        string[] list;
        if(companies != null)
        {
            list = companies.Split(new Char[] { ';' });
            foreach (string o in list)
            {
                if (o != "")
                    CurCompanies.Add(int.Parse(o), o);
            }
        }
        if(ranks != null)
        {
            list = ranks.Split(new Char[] { ';' });
            foreach (string o in list)
            {
                if (o != "")
                    CurRanks.Add(int.Parse(o), o);
            }
        }
        return true;
    }


    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        EESSAnnouncement obj = new EESSAnnouncement();
        obj.ESSAnnouncementID = CurID;
        db.select(dbConn, obj);
        DBFilter filter = new DBFilter();
        filter.add(new Match("ESSAnnouncementID", obj.ESSAnnouncementID));
        ArrayList essAnnouncementList = EESSAnnouncement.db.select(dbConn, filter);
        if (essAnnouncementList.Count > 0)
        {
            foreach (EESSAnnouncement essAnnouncement in essAnnouncementList)
            {

                EESSAnnouncement.db.delete(dbConn, essAnnouncement);

            }
        }
        else
        {
            db.delete(dbConn, obj);
            WebUtils.EndFunction(dbConn);
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "ESS_Announcement_List.aspx");
    }

    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "ESS_Announcement_Edit.aspx?ESSAnnouncementID=" + CurID);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "ESS_Announcement_List.aspx");
    }

    public void loadCompanies()
    {
        DBFilter filter = new DBFilter();
        DataTable table = ECompany.db.loadDataSet(dbConn, null, filter);
        DataView view = new DataView(table);
        ESSAnnouncementTargetCompanies.DataSource = view;
        ESSAnnouncementTargetCompanies.DataBind();
    }
    public void loadRanks()
    {
        DBFilter filter = new DBFilter();
        DataTable table = ERank.db.loadDataSet(dbConn, null, filter);
        DataView view = new DataView(table);
        ESSAnnouncementTargetRanks.DataSource = view;
        ESSAnnouncementTargetRanks.DataBind();
    }

    protected void Companies_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        HtmlInputCheckBox selected = (HtmlInputCheckBox)e.Item.FindControl("CompanySelect");
        int id = (int)((DataRowView)e.Item.DataItem)["CompanyID"];
        selected.Attributes["id"] = id.ToString(); ;

        if (CurCompanies.ContainsKey(id))
            selected.Checked = true;
    }
    protected void Ranks_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        HtmlInputCheckBox selected = (HtmlInputCheckBox)e.Item.FindControl("RankSelect");
        int id = (int)((DataRowView)e.Item.DataItem)["RankID"];
        selected.Attributes["id"] = id.ToString();

        if (CurRanks.ContainsKey(id))
            selected.Checked = true;
    }
}
