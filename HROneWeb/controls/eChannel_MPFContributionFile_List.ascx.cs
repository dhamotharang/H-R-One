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

public partial class eChannel_MPFContributionFile_List : HROneWebControl
{
    private bool IsAllowEdit = true;
    private const string FUNCTION_CODE = "ECH002";

    public int CurID = -1;

    protected SearchBinding sbinding;
    public DBManager sdb = ECompanyMPFFile.db;
    protected ListInfo info;
    protected DataView view;
    protected DatabaseConnection masterDBConn;
    DateTime bankFileLastCancelTime = new DateTime();
    protected HROne.Lib.Entities.EUser currentUser;
    protected bool isAuthorizer = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Application["MasterDBConfig"] != null)
            masterDBConn = ((DatabaseConfig)Application["MasterDBConfig"]).CreateDatabaseConnectionObject();
        else
            Response.Redirect("~/AccessDeny.aspx");

        IsAllowEdit = WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite);

        if (Session["CompanyDBID"] != null)
            CurID = (int)Session["CompanyDBID"];
        CompanyDBID.Value = CurID.ToString();


        sbinding = new SearchBinding(masterDBConn, sdb);
        sbinding.initValues("HSBCExchangeProfileID", null, EHSBCExchangeProfile.VLRemoteProfileList, ci);
        sbinding.init(DecryptedRequest, null);

        info = ListFooter.ListInfo;


        string tmpDateTimeString = ESystemParameter.getParameter(masterDBConn, ESystemParameter.PARAM_CODE_BANKFILE_LAST_CANCEL_TIME);
        if (!DateTime.TryParseExact(tmpDateTimeString, "HH:mm", null, System.Globalization.DateTimeStyles.None, out bankFileLastCancelTime))
            bankFileLastCancelTime = new DateTime();


        currentUser = WebUtils.GetCurUser(Session);
        if (currentUser != null)
        {
            DBFilter signatureFilter = new DBFilter();
            signatureFilter.add(new Match("UserID", currentUser.UserID));
            if (HROne.Lib.Entities.EeChannelAuthorizedSignature.db.count(dbConn, signatureFilter) > 0)
                isAuthorizer = true;
        }
    }

    void Page_PreRender(object sender, EventArgs e)
    {
        if (view == null)
        {
            //loadState();
            view = loadData(info, sdb, Repeater);
        }
    }

    //public void loadState()
    //{
    //    info = new ListInfo();
    //    int page = 0;
    //    if (!CurPage.Value.Equals(""))
    //        page = Int32.Parse(CurPage.Value);
    //    info.loadState(Request, page);
    //    info.order = !Order.Value.Equals("false", StringComparison.CurrentCultureIgnoreCase);
    //    info.orderby = OrderBy.Value;
    //    if (string.IsNullOrEmpty(info.orderby))
    //        info.orderby = "LeaveAppDateFrom";
    //}
    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = sbinding.createFilter();
        filter.add(new Match("CompanyDBID", this.CurID));

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = "*";
        string from = "from " + sdb.dbclass.tableName + " ";

        DataTable table = filter.loadData(masterDBConn, info, select, from);

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
        //loadState();
        //info.page = 0;
        view = loadData(info, sdb, Repeater);

    }
    protected void PrevPage_Click(object sender, EventArgs e)
    {
        //loadState();
        //info.page--;
        view = loadData(info, sdb, Repeater);

    }
    protected void NextPage_Click(object sender, EventArgs e)
    {
        //loadState();
        //info.page++;
        view = loadData(info, sdb, Repeater);

    }
    protected void LastPage_Click(object sender, EventArgs e)
    {
        //loadState();

        //info.page = Int32.Parse(NumPage.Value);
        view = loadData(info, sdb, Repeater);

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

        view = loadData(info, sdb, Repeater);

    }
    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(sdb, row, cb);
        Button cancelButton = (Button)e.Item.FindControl("Cancel");
        Button SignButton = (Button)e.Item.FindControl("Sign");
        Label SignedBy = (Label)e.Item.FindControl("SignedBy");

        ECompanyMPFFile obj = new ECompanyMPFFile();
        sdb.toObject(row.Row, obj);
        cb.Visible = false;

        if (!isAuthorizer || !obj.CompanyMPFFileConfirmDateTime.Ticks.Equals(0))
            SignButton.Visible = false;
        else
        {
            SignButton.Visible = true;
        }

        if (!obj.CompanyMPFFileConsolidateDateTime.Ticks.Equals(0) || bankFileLastCancelTime < AppUtils.ServerDateTime())
            cancelButton.Visible = false;
        else
        {
            cancelButton.Visible = IsAllowEdit;
            cancelButton.OnClientClick = HROne.Translation.PromptMessage.CreateDeleteConfirmDialogJavascript(cancelButton);
        }

        DBFilter countSignatureFilter = new DBFilter();
        countSignatureFilter.add(new Match("CompanyMPFFileID", obj.CompanyMPFFileID));
        countSignatureFilter.add(new Match("CompanyDBID", CurID));
        ArrayList signedList = ECompanyMPFFileSignature.db.select(masterDBConn, countSignatureFilter);
        foreach (ECompanyMPFFileSignature signature in signedList)
        {
            if (string.IsNullOrEmpty(SignedBy.Text))
                SignedBy.Text = signature.CompanyMPFFileSignatureUserName;
            else
                SignedBy.Text += ",<br/>" + signature.CompanyMPFFileSignatureUserName;

        }
    }

    protected void Repeater_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(sdb, Page.Master);
        errors.clear();

        Button b = (Button)e.CommandSource;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");

        DataRowView row = (DataRowView)e.Item.DataItem;
        ECompanyMPFFile obj = new ECompanyMPFFile();
        WebFormUtils.GetKeys(sdb, obj, cb);
        if (sdb.select(masterDBConn, obj))
        {

            if (b.ID.Equals("Cancel"))
            {
                if (!obj.CompanyMPFFileConsolidateDateTime.Ticks.Equals(0) || bankFileLastCancelTime < AppUtils.ServerDateTime())
                {
                    errors.addError("The system fail to remove the request because the bank file is submitting to bank.");
                }
                else
                {
                    string UploadBankFilePath = ESystemParameter.getParameter(masterDBConn, ESystemParameter.PARAM_CODE_BANKFILE_UPLOAD_FOLDER);
                    if (System.IO.Directory.Exists(UploadBankFilePath))
                    {
                        string fullPath = System.IO.Path.Combine(UploadBankFilePath, obj.CompanyMPFFileDataFileRelativePath);
                        System.IO.File.Delete(fullPath);

                    }

                    ECompanyMPFFile.db.delete(masterDBConn, obj);
                }
            }
            else if (b.ID.Equals("Sign"))
            {
                DateTime currentDateTime = AppUtils.ServerDateTime();

                int numOfSignatureRequired = 0;
                if (!int.TryParse(HROne.Lib.Entities.ESystemParameter.getParameter(dbConn, HROne.Lib.Entities.ESystemParameter.PARAM_CODE_ECHANNEL_NUM_SIGNATURE_REQUIRED), out numOfSignatureRequired))
                    numOfSignatureRequired = 0;

                DBFilter currentSignatureFilter = new DBFilter();
                currentSignatureFilter.add(new Match("CompanyMPFFileID", obj.CompanyMPFFileID));
                currentSignatureFilter.add(new Match("CompanyDBID", CurID));
                currentSignatureFilter.add(new Match("UserID", currentUser.UserID));

                if (ECompanyMPFFileSignature.db.count(masterDBConn, currentSignatureFilter) <= 0)
                {
                    ECompanyMPFFileSignature signature = new ECompanyMPFFileSignature();
                    signature.CompanyMPFFileID = obj.CompanyMPFFileID;
                    signature.CompanyDBID = CurID;
                    signature.UserID = currentUser.UserID;
                    signature.CompanyMPFFileSignatureUserName = currentUser.UserName;
                    signature.CompanyMPFFileSignatureDateTime = currentDateTime;
                    ECompanyMPFFileSignature.db.insert(masterDBConn, signature);
                }

                DBFilter countSignatureFilter = new DBFilter();
                countSignatureFilter.add(new Match("CompanyMPFFileID", obj.CompanyMPFFileID));
                countSignatureFilter.add(new Match("CompanyDBID", CurID));
                if (ECompanyMPFFileSignature.db.count(masterDBConn, currentSignatureFilter) >= numOfSignatureRequired)
                {
                    obj.CompanyMPFFileConfirmDateTime = currentDateTime;
                    ECompanyMPFFile.db.update(masterDBConn, obj);
                }
            }
        }
        view = loadData(info, sdb, Repeater);



    }
}
