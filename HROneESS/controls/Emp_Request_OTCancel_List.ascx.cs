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
using HROne.Lib.Entities;

public partial class Emp_Request_OTCancel_List : HROneWebControl
{
    protected DBManager db = ERequestOTClaimCancel.db;
    protected SearchBinding binding;
    protected Binding ebinding;
    protected ListInfo info;
    protected DataView view;

    public int CurID = -1;

    protected void Page_Load(object sender, EventArgs e)
    {

        binding = new SearchBinding(dbConn, db);
        binding.initValues("EmpRequestStatus", null, EEmpRequest.VLRequestStatus, null);

        binding.init(Request.QueryString, null);

        info = ListFooter.ListInfo;

        EESSUser user = WebUtils.GetCurUser(Session);
        if (user != null)
        {
            CurID = user.EmpID;
            EmpID.Value = CurID.ToString();
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (view == null)
        {
            view = loadData(info, db, Repeater);
        }
    }

    public DataView loadData(ListInfo info, DBManager db, DataList repeater)
    {
        DBFilter filter = binding.createFilter();

        DateTime dtPeriodFr, dtPeriodTo;
        if (DateTime.TryParse(RequestFromDate.Value, out dtPeriodFr))
            filter.add(new Match("RequestOTClaimCancelCreateDateTime", ">=", dtPeriodFr));
        if (DateTime.TryParse(RequestToDate.Value, out dtPeriodTo))
            filter.add(new Match("RequestOTClaimCancelCreateDateTime", "<", dtPeriodTo.AddDays(1)));

        filter.add(new Match("c.EmpID", this.CurID));

        if (!EmpRequestStatus.SelectedValue.Equals(string.Empty))
        {
            if (EmpRequestStatus.SelectedValue.Equals("REQUEESTSTATUS_PROCESSING"))
            {
                filter.add(EEmpRequest.EndStatusDBTerms("EmpRequestStatus", true));
            }
            else if (EmpRequestStatus.SelectedValue.Equals("REQUEESTSTATUS_END_PROCESS"))
            {
                filter.add(EEmpRequest.EndStatusDBTerms("EmpRequestStatus", false));
            }
        }

        string select = "c.RequestOTClaimCancelID, c.RequestOTClaimCancelCreateDateTime, r.EmpRequestStatus, r.EmpRequestID, r.EmpRequestLastAuthorizationWorkFlowIndex, la.* ";
        string from = "from " + db.dbclass.tableName + " c " +
            " LEFT JOIN " + EOTClaim.db.dbclass.tableName + " la ON c.OTClaimID = la.OTClaimID " +
            " LEFT JOIN " + EEmpRequest.db.dbclass.tableName + " r On r.EmpRequestRecordID = c.RequestOTClaimCancelID and r.EmpRequestType = '" + EEmpRequest.TYPE_EEOTCLAIM + "'";


        DataTable table = filter.loadData(dbConn, info, select, from);

        view = new DataView(table);

        ListFooter.Refresh();

        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }
        return view;

    }

    protected void Repeater_ItemDataBound(object sender, DataListItemEventArgs e)
    {

        HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("_RequestOTClaimCancelID");
            h.Value = ((DataRowView)e.Item.DataItem)["RequestOTClaimCancelID"].ToString();

        string submitStatus=((DataRowView)e.Item.DataItem)["EmpRequestStatus"].ToString() ;
        if ((submitStatus.Equals(EEmpRequest.STATUS_SUBMITTED) || submitStatus.Equals(EEmpRequest.STATUS_ACCEPTED)) && !submitStatus.Equals(EEmpRequest.STATUS_APPROVED))
            e.Item.FindControl("Cancel").Visible = true;
        else
            e.Item.FindControl("Cancel").Visible = false;
        HROne.Common.WebUtility.WebControlsLocalization(Session, e.Item.Controls);

    }
    protected void Repeater_ItemCommand(object source, DataListCommandEventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(EOTClaim.db, Page.Master);

        DBFilter filterStatus = new DBFilter();
        DBManager Requestdb = EEmpRequest.db;
        HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("_RequestOTClaimCancelID");

        //------------------------------------------------------
        //Select Filter record from the EmpRequest table by EmpRequestRecordID and Request Status
        filterStatus.add(new Match("EmpRequestRecordID", Int32.Parse(h.Value)));
        OR orStatus = new OR();

        orStatus.add(new Match("EmpRequestStatus", EEmpRequest.STATUS_SUBMITTED));
        orStatus.add(new Match("EmpRequestStatus", EEmpRequest.STATUS_ACCEPTED));

        filterStatus.add(orStatus);
        filterStatus.add(new Match("EmpRequestStatus", "<>", EEmpRequest.STATUS_APPROVED));
        //------------------------------------------------------
        if (Requestdb.count(dbConn, filterStatus) > 0)
        {
            ArrayList EmpRequestList = Requestdb.select(dbConn, filterStatus);
            EEmpRequest EmpRequest = (EEmpRequest)EmpRequestList[0];
            ESSAuthorizationProcess authorization = new ESSAuthorizationProcess(dbConn);
            authorization.CancelAction(EmpRequest);
            errors.addError(HROne.Common.WebUtility.GetLocalizedString("The CL Requisition is cancelled"));
        }
        view = loadData(info, db, Repeater);




    }
    protected void Search_Click(object sender, EventArgs e)
    {
        info.page = 0;
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
        
        view = loadData(info, db, Repeater);

    }
    protected void EmpRequestStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);
    }
}
