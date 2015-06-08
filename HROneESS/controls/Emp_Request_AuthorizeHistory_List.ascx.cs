using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using HROne.DataAccess;
//using perspectivemind.validation;
using HROne.Lib.Entities;
using System.Net.Mail;

public partial class Emp_Request_AuthorizeHistory_List : HROneWebControl
{
    protected DBManager db = EEmpRequest.db;
    protected DBManager RequestEmpdb = ERequestEmpPersonalInfo.db;
    protected SearchBinding binding;
    protected Binding ebinding;
    protected ListInfo info;
    protected DataView view;
    //public static WFValueList VLStatusUnit = new WFTextList(new string[] { EEmpRequest.STATUS_USRSUBMIT, EEmpRequest.STATUS_FSTAPP }, new string[] { EEmpRequest.STATUS_USRSUBMIT, EEmpRequest.STATUS_FSTAPP });
    public static WFValueList VLStatusUnit = new WFTextList(new string[] { EEmpRequest.STATUS_SUBMITTED, EEmpRequest.STATUS_ACCEPTED }, new string[] { EEmpRequest.STATUS_SUBMITTED, EEmpRequest.STATUS_ACCEPTED });

    public int CurID = -1;
    public int CurRequestID = -1;


    protected void Page_Load(object sender, EventArgs e)
    {
        //PreRender += new EventHandler(Emp_Authorize_List_PreRender);

        binding = new SearchBinding(dbConn, EEmpPersonalInfo.db);

        binding.initValues("EmpRequestApprovalHistoryStatusBefore", null, EEmpRequest.VLRequestStatus, null);
        binding.initValues("EmpRequestApprovalHistoryStatusAfter", null, EEmpRequest.VLRequestStatus, null);

        binding.init(Request.QueryString, null);

        info = ListFooter.ListInfo;

        //EESSUser user = WebUtils.GetCurUser(Session);
        //if (user != null)
        //{
        //    UserID = user.EmpID;
        //}
        //if (!Int32.TryParse(DecryptedRequest["TargetEmpID"], out CurEmpID))
        //    CurEmpID = -1;
        if (!Int32.TryParse(DecryptedRequest["EmpRequestID"], out CurRequestID))
            CurRequestID = -1;
        //EmpID.Value = CurEmpID.ToString();

    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (view == null)
        {
            view = loadData(info, db, Repeater);
        }

    }

    //public void loadState()
    //{
    //    info = new ListInfo();
    //    int page = 0;
    //    if (!CurPage.Value.Equals(""))
    //        page = Int32.Parse(CurPage.Value);
    //    info.loadState(Request, page);
    //    info.order = Order.Value.Equals("True");
    //    info.orderby = OrderBy.Value;
    //    if (info.orderby == "")
    //        info.orderby = null;
    //    info.recordPerPage = -1;
    //}
    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
            DBFilter filter = binding.createFilter();

            filter.add(new Match("rah.EmpRequestID", CurRequestID));

            string select = "rah.* ";
            string from = "from " + EEmpRequestApprovalHistory.db.dbclass.tableName + " rah ";

            //DBFilter authorizerFilter = new DBFilter();
            //authorizerFilter.add(new Match("EmpID", CurID));

            //OR orFirstGrpStatusTerms = new OR();
            //orFirstGrpStatusTerms.add(new Match("R.EmpRequestStatus", EEmpRequest.STATUS_SUBMITTED));

            //DBFilter firstGrpFilter = new DBFilter();
            //firstGrpFilter.add(orFirstGrpStatusTerms);
            //firstGrpFilter.add(orAuthorizationGrpApplicationTerms);
            //firstGrpFilter.add(new IN("AuthorizationGroupID", "Select AuthorizationGroupID from " + EAuthorizer.db.dbclass.tableName, authorizerFilter));


            //OR orSecondGrpStatusTerms = new OR();
            //orSecondGrpStatusTerms.add(new Match("R.EmpRequestStatus", EEmpRequest.STATUS_SUBMITTED));
            //orSecondGrpStatusTerms.add(new Match("R.EmpRequestStatus", EEmpRequest.STATUS_ACCEPTED));

            //DBFilter secondGrpFilter = new DBFilter();
            //secondGrpFilter.add(orSecondGrpStatusTerms);
            //secondGrpFilter.add(orAuthorizationGrpApplicationTerms);
            //secondGrpFilter.add(new IN("AuthorizationGroupID", "Select AuthorizationGroupID from " + EAuthorizer.db.dbclass.tableName, authorizerFilter));

            //OR orAuthorizationGrpFilter = new OR();
            //orAuthorizationGrpFilter.add(new IN("EP.EmpFirstAuthorizationGp", "Select AuthorizationGroupID from " + EAuthorizationGroup.db.dbclass.tableName, firstGrpFilter));
            //orAuthorizationGrpFilter.add(new IN("EP.EmpSecondAuthorizationGp", "Select AuthorizationGroupID from " + EAuthorizationGroup.db.dbclass.tableName, secondGrpFilter));
            //filter.add(orAuthorizationGrpFilter);

            DataTable table = filter.loadData(dbConn, info, select, from);


            view = new DataView(table);

            ListFooter.Refresh();

            if (table.Rows.Count > 0)
            {
                if (repeater != null)
                {
                    repeater.DataSource = view;
                    repeater.DataBind();
                }
                Visible = true;
            }
            else
                Visible = false;

        return view;
    }

    //protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    //{
    //    DataRowView row = (DataRowView)e.Item.DataItem;
    //    CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
    //    HtmlAnchor requestLink = (HtmlAnchor)e.Item.FindControl("requestLink");

    //    WebFormUtils.LoadKeys(db, row, cb);
    //    cb.Checked=false;
    //    cb.Visible = false;

    //    EEmpRequest obj = new EEmpRequest();
    //    EEmpRequest.db.toObject(((DataRowView)e.Item.DataItem).Row, obj);

    //    requestLink.HRef = HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "~/ESS_EmpRequestDetail.aspx?TargetEmpID=" + obj.EmpID + "&EmpRequestRecordID=" + obj.EmpRequestRecordID + "&EmpRequestID=" + obj.EmpRequestID);
    //    //requestLink.InnerText = row["EmpNo"].ToString();
    //    requestLink.Title = generateToolTipMessage(obj);
    //    ESSAuthorizationProcess authorization = new ESSAuthorizationProcess(dbConn);
    //    ArrayList workFlowDetailList = authorization.GetAuthorizationWorkFlowDetailList(obj.EmpID, obj.EmpRequestType);
    //    EAuthorizationWorkFlowDetail workFlowDetail = authorization.GetCurrentWorkFlowDetailObject(workFlowDetailList, obj.EmpRequestLastAuthorizationWorkFlowIndex, CurID);
    //    if (workFlowDetail != null)
    //    {
    //        ArrayList authorizerList =  workFlowDetail.GetActualAutorizerObjectList(dbConn, CurID);
    //        foreach (EAuthorizer authorizer in authorizerList)
    //            if (!authorizer.AuthorizerIsReadOnly)
    //                cb.Visible = true;
    //    }
    //}

    //protected void Search_Click(object sender, EventArgs e)
    //{
    //    info.page = 0;
    //    view = loadData(info, db, Repeater);

    //}

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

    //protected string generateToolTipMessage(EEmpRequest empRequest)
    //{
    //    string message = "Employee No:\t%EMP_NO%\r\n" +
    //                        "Employee:\t%EMP_NAME%\r\n" +
    //                        "Leave Date:\t%LEAVEAPP_PERIOD%\r\n" +
    //                        "Leave Type:\t%LEAVE_CODE%\r\n" +
    //                        "Application Type:\t%LEAVEAPP_UNIT%\r\n" +
    //                        "Days Taken:\t%LEAVEAPP_DAYS%\r\n" +
    //                        "Hours Claims:\t%LEAVEAPP_HOURSCLAIM%\r\n" +
    //                        "Reason:\t%LEAVEAPPCANCEL_REMARK%\r\n";

    //    ESSAuthorizationProcess process = new ESSAuthorizationProcess(dbConn);

    //    EEmpPersonalInfo ApplicantEmpInfo = process.GetEmpInfo(empRequest.EmpID);

    //    Hashtable mailContentParameterTable = new Hashtable();
    //    mailContentParameterTable = process.GenerateEmpInfoHashTable(ApplicantEmpInfo, mailContentParameterTable);

    //    if (empRequest.EmpRequestType == EEmpRequest.TYPE_EELEAVEAPP)
    //    {
    //        DBFilter RequestEmpFilter = new DBFilter();
    //        RequestEmpFilter.add(new Match("RequestLeaveAppID", empRequest.EmpRequestRecordID));
    //        ArrayList empRequestList = ERequestLeaveApplication.db.select(dbConn, RequestEmpFilter);
    //        if (empRequestList.Count > 0)
    //        {
    //            ERequestLeaveApplication empLeaveRequest = (ERequestLeaveApplication)empRequestList[0];

    //            mailContentParameterTable = process.GenerateRequestLeaveApplicationHashTable(empLeaveRequest, mailContentParameterTable);
    //        }
    //    }
    //    else if (empRequest.EmpRequestType == EEmpRequest.TYPE_EELEAVECANCEL)
    //    {
    //        DBFilter RequestEmpFilter = new DBFilter();
    //        RequestEmpFilter.add(new Match("RequestLeaveAppCancelID", empRequest.EmpRequestRecordID));
    //        ArrayList empRequestList = ERequestLeaveApplicationCancel.db.select(dbConn, RequestEmpFilter);
    //        if (empRequestList.Count > 0)
    //        {
    //            ERequestLeaveApplicationCancel requestLeaveAppCancel = (ERequestLeaveApplicationCancel)empRequestList[0];
    //            mailContentParameterTable.Add("%LEAVEAPPCANCEL_REMARK%", requestLeaveAppCancel.RequestLeaveAppCancelReason);
    //            ELeaveApplication leaveApp = new ELeaveApplication();
    //            leaveApp.LeaveAppID = requestLeaveAppCancel.LeaveAppID;
    //            if (ELeaveApplication.db.select(dbConn, leaveApp))
    //            {
    //                ERequestLeaveApplication dummyRequestLeave = ERequestLeaveApplication.CreateDummyRequestLeaveAppliction(leaveApp);
    //                mailContentParameterTable = process.GenerateRequestLeaveApplicationHashTable(dummyRequestLeave, mailContentParameterTable);
    //            }
    //        }
    //    }
    //    string MessageBodyTemplate = message;
    //    foreach (string key in mailContentParameterTable.Keys)
    //    {
    //        MessageBodyTemplate = MessageBodyTemplate.Replace(key, mailContentParameterTable[key].ToString());
    //    }
    //    string[] messageBodyLineArray = MessageBodyTemplate.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
    //    string newMessageBody = string.Empty;
    //    foreach (string messageLine in messageBodyLineArray)
    //        if (string.IsNullOrEmpty(messageLine) || messageLine.IndexOf("%") == messageLine.LastIndexOf("%"))
    //            if (string.IsNullOrEmpty(newMessageBody))
    //                newMessageBody = messageLine;
    //            else
    //                newMessageBody += "\r\n" + messageLine;
    //    return newMessageBody;
    //}
}
