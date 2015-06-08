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
using HROne.SaaS.Entities;

public partial class DBServer_Edit : HROneWebPage
{
    const string FUNCTION_CODE = "ADM001";

    public Binding binding;
    public DBManager db = EDatabaseServer.db;
    public int CurID = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE))
            return;

        binding = new Binding(dbConn, db);
        binding.add(DBServerID);
        binding.add(DBServerCode);
        binding.add(DBServerDBType);
        binding.add(DBServerLocation);
        binding.add(DBServerSAUserID);
        binding.add(DBServerSAPassword);
        binding.add(DBServerUserID);
        binding.add(DBServerPassword);
        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["DBServerID"], out CurID))
            CurID = -1;

        if (!Page.IsPostBack)
        {
            if (CurID > 0)
            {
                loadObject();
            }
            else
                toolBar.DeleteButton_Visible = false;
        }
    }
    protected bool loadObject()
    {
        EDatabaseServer obj = new EDatabaseServer();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);

        if (!db.select(dbConn, obj))
            if (CurID <= 0)
                return false;
            else
                Response.Redirect("~/AccessDeny.aspx");


        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);

        return true;
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        EDatabaseServer c = new EDatabaseServer();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);

        if (c.DBServerDBType.Equals("MSSQL"))
        {
            System.Data.SqlClient.SqlConnectionStringBuilder connStringBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder();
            connStringBuilder.DataSource = DBServerLocation.Text;
            connStringBuilder.UserID = DBServerSAUserID.Text;
            connStringBuilder.Password = DBServerSAPassword.Text;

            DatabaseConfig config = new DatabaseConfig();
            config.DBType = WebUtils.DBTypeEmun.MSSQL;
            config.ConnectionString = connStringBuilder.ConnectionString;
            if (!config.TestServerConnectionWithoutDatabase())
            {
                errors.addError("connection fail");
                return;
            }
            //c.SetConnectionString(config.ConnectionString);
        }
        if (CurID < 0)
        {
            //            Utils.MarkCreate(Session, c);

            db.insert(dbConn, c);
            CurID = c.DBServerID;
            //            url = Utils.BuildURL(-1, CurID);
        }
        else
        {
            //            Utils.Mark(Session, c);
            db.update(dbConn, c);
        }


        Response.Redirect("~/DBServer_List.aspx");


    }
    protected void Delete_Click(object sender, EventArgs e)
    {

        EDatabaseServer c = new EDatabaseServer();
        c.DBServerID = CurID;
        db.select(dbConn, c);
        db.delete(dbConn, c);
        Response.Redirect("~/DBServer_List.aspx");
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/DBServer_List.aspx");
    }
}
