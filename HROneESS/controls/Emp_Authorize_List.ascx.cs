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

public partial class Emp_Authorize_List : HROneWebControl
{
    protected DBManager db = EEmpRequest.db;
    protected DBManager RequestEmpdb = ERequestEmpPersonalInfo.db;
    protected SearchBinding binding;
    protected Binding ebinding;
    protected ListInfo info;
    protected DataView view;
    //public static WFValueList VLStatusUnit = new WFTextList(new string[] { EEmpRequest.STATUS_USRSUBMIT, EEmpRequest.STATUS_FSTAPP }, new string[] { EEmpRequest.STATUS_USRSUBMIT, EEmpRequest.STATUS_FSTAPP });
    //public static WFValueList VLStatusUnit = new WFTextList(new string[] { EEmpRequest.STATUS_SUBMITTED, EEmpRequest.STATUS_ACCEPTED }, new string[] { EEmpRequest.STATUS_SUBMITTED, EEmpRequest.STATUS_ACCEPTED });

    public int CurID = -1;


    protected void Page_Load(object sender, EventArgs e)
    {
        //PreRender += new EventHandler(Emp_Authorize_List_PreRender);

        binding = new SearchBinding(dbConn, EEmpPersonalInfo.db);
        //binding.add(new DropDownVLSearchBinder(EmpRequestStatus2, "R.EmpRequestStatus", VLStatusUnit));
        //binding.add(new FieldDateRangeSearchBinder(EmpRequestFromDate, EmpRequestToDate, "EmpRequestCreateDate").setUseCurDate(false));

        if (!ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ENABLE_OTCLAIM).Equals("Y", StringComparison.CurrentCultureIgnoreCase))
        {
            binding.add(new DropDownVLSearchBinder(EmpRequestType, "R.EmpRequestType", EEmpRequest.VLRequestType));
        }else
        {
            binding.add(new DropDownVLSearchBinder(EmpRequestType, "R.EmpRequestType", EEmpRequest.VLRequestType2));
        }
        
        binding.initValues("EmpRequestType", null, EEmpRequest.VLRequestType, null);
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
        string strDelegateEmpNoList = string.Empty;
        DBFilter authorizerDBFilter = new DBFilter();
        authorizerDBFilter.add(new Match("ad.EmpID", CurID));

        DBFilter delegateEmpInfoFilter = new DBFilter();
        delegateEmpInfoFilter.add(new IN("EmpID", "SELECT AuthorizerDelegateEmpID FROM " + EAuthorizerDelegate.db.dbclass.tableName + " ad", authorizerDBFilter));
        ArrayList delegateEmpInfoList = EEmpPersonalInfo.db.select(dbConn, delegateEmpInfoFilter);
        foreach (EEmpPersonalInfo delegateEmpInfo in delegateEmpInfoList)
        {
            if (string.IsNullOrEmpty(strDelegateEmpNoList))
                strDelegateEmpNoList = delegateEmpInfo.EmpNo;
            else
                strDelegateEmpNoList += "; " + delegateEmpInfo.EmpNo;
        }
        txtDelegateEmpNoList.Text = strDelegateEmpNoList;

        ESSAuthorizationProcess authorization = new ESSAuthorizationProcess(dbConn);
        DelegateRow.Visible = authorization.IsPrimaryAuthorizer(CurID);
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
        ESSAuthorizationProcess authorization = new ESSAuthorizationProcess(dbConn);
        List<EAuthorizationGroup> authGroupList = authorization.GetAuthorizerAuthorizationGroupList(CurID);

        if (authGroupList.Count > 0)
        {
            DBFilter filter = binding.createFilter();

            DateTime dtPeriodFr, dtPeriodTo;
            if (DateTime.TryParse(EmpRequestFromDate.Value, out dtPeriodFr))
                filter.add(new Match("EmpRequestCreateDate", ">=", dtPeriodFr));
            if (DateTime.TryParse(EmpRequestToDate.Value, out dtPeriodTo))
                filter.add(new Match("EmpRequestCreateDate", "<", dtPeriodTo.AddDays(1)));

            //if (info != null && info.orderby != null && !info.orderby.Equals(""))
            //    filter.add(info.orderby, info.order);

            filter.add(new Match("R.EmpRequestStatus", "<>", EEmpRequest.STATUS_CANCELLED));
            filter.add(new Match("R.EmpRequestStatus", "<>", EEmpRequest.STATUS_REJECTED));
            filter.add(new Match("R.EmpRequestStatus", "<>", EEmpRequest.STATUS_APPROVED));

            DBFilter workFlowDetailFilter = new DBFilter();
            OR orWorkFlowIndexTerms = new OR();
            if (EmpRequestStatus2.SelectedValue == "Y")
                orWorkFlowIndexTerms.add(new MatchField("awfd.AuthorizationWorkFlowIndex", "=", "R.EmpRequestLastAuthorizationWorkFlowIndex + 1"));
            else if (EmpRequestStatus2.SelectedValue == "O")
                orWorkFlowIndexTerms.add(new MatchField("awfd.AuthorizationWorkFlowIndex", ">", "R.EmpRequestLastAuthorizationWorkFlowIndex + 1"));
            else
                orWorkFlowIndexTerms.add(new MatchField("awfd.AuthorizationWorkFlowIndex", ">", "R.EmpRequestLastAuthorizationWorkFlowIndex"));
            AND andMaxWorkFlowIndexTerms = new AND();
            andMaxWorkFlowIndexTerms.add(new MatchField("awfd.AuthorizationWorkFlowIndex", "=", "(SELECT MAX(maxAWFD.AuthorizationWorkFlowIndex) FROM " + EAuthorizationWorkFlowDetail.db.dbclass.tableName + " maxAWFD WHERE maxAWFD.AuthorizationWorkFlowID=awfd.AuthorizationWorkFlowID)"));
            andMaxWorkFlowIndexTerms.add(new MatchField("R.EmpRequestLastAuthorizationWorkFlowIndex", ">=", "awfd.AuthorizationWorkFlowIndex"));

            orWorkFlowIndexTerms.add(andMaxWorkFlowIndexTerms);
            workFlowDetailFilter.add(orWorkFlowIndexTerms);
            DBFilter authorizerFilter = new DBFilter();

            string strAuthGroupInList = string.Empty;
            foreach (EAuthorizationGroup authGroup in authGroupList)
                if (string.IsNullOrEmpty(strAuthGroupInList))
                    strAuthGroupInList = authGroup.AuthorizationGroupID.ToString();
                else
                    strAuthGroupInList += "," + authGroup.AuthorizationGroupID.ToString();
            workFlowDetailFilter.add(new IN("awfd.AuthorizationGroupID", strAuthGroupInList, null));

            AND andLeaveApplicationTerms = new AND();
            andLeaveApplicationTerms.add(new IN("R.EmpRequestType", new string[] { EEmpRequest.TYPE_EELEAVEAPP, EEmpRequest.TYPE_EELEAVECANCEL }));
            andLeaveApplicationTerms.add(new IN("EP.AuthorizationWorkFlowIDLeaveApp", "SELECT awfd.AuthorizationWorkFlowID FROM " + EAuthorizationWorkFlowDetail.db.dbclass.tableName + " awfd", workFlowDetailFilter));

            //andLeaveApplicationTerms.add(new Match("AuthorizationGroupIsApproveLeave", "<>", false));

            AND andEEProfileTerms = new AND();
            andEEProfileTerms.add(new Match("R.EmpRequestType", EEmpRequest.TYPE_EEPROFILE));
            andEEProfileTerms.add(new IN("EP.AuthorizationWorkFlowIDEmpInfoModified", "SELECT awfd.AuthorizationWorkFlowID FROM " + EAuthorizationWorkFlowDetail.db.dbclass.tableName + " awfd", workFlowDetailFilter));

            // Start 0000060, Miranda, 2014-07-13
            AND andOTClaimTerms = new AND();
            andOTClaimTerms.add(new IN("R.EmpRequestType", new string[] { EEmpRequest.TYPE_EEOTCLAIM, EEmpRequest.TYPE_EEOTCLAIMCANCEL }));
            andOTClaimTerms.add(new IN("EP.AuthorizationWorkFlowIDOTClaims", "SELECT awfd.AuthorizationWorkFlowID FROM " + EAuthorizationWorkFlowDetail.db.dbclass.tableName + " awfd", workFlowDetailFilter));
            // End 0000060, Miranda, 2014-07-13

            // Start 0000112, Miranda, 2014-12-10
            AND andLateWaiveTerms = new AND();
            andLateWaiveTerms.add(new IN("R.EmpRequestType", new string[] { EEmpRequest.TYPE_EELATEWAIVE, EEmpRequest.TYPE_EELATEWAIVECANCEL }));
            andLateWaiveTerms.add(new IN("EP.AuthorizationWorkFlowIDLateWaive", "SELECT awfd.AuthorizationWorkFlowID FROM " + EAuthorizationWorkFlowDetail.db.dbclass.tableName + " awfd", workFlowDetailFilter));
            // End 0000112, Miranda, 2014-12-10

            OR orAuthorizationGrpApplicationTerms = new OR();
            orAuthorizationGrpApplicationTerms.add(andLeaveApplicationTerms);
            orAuthorizationGrpApplicationTerms.add(andEEProfileTerms);
            // Start 0000060, Miranda, 2014-07-13
            orAuthorizationGrpApplicationTerms.add(andOTClaimTerms);
            // End 0000060, Miranda, 2014-07-13
            // Start 0000112, Miranda, 2014-12-10
            orAuthorizationGrpApplicationTerms.add(andLateWaiveTerms);
            // End 0000112, Miranda, 2014-12-10
            filter.add(orAuthorizationGrpApplicationTerms);
            //string select = "R.*, E.EmpNo, E.EmpEngSurname, E.EmpEngOtherName, E.EmpAlias ";
            //string from = "from (select * from [" + db.dbclass.tableName + "] where EmpRequestStatus = '"
            //    + EEmpRequest.STATUS_USRSUBMIT.ToString() + "' or EmpRequestStatus = '"
            //    + EEmpRequest.STATUS_FSTAPP.ToString() + "' ) R LEFT JOIN " + EEmpPersonalInfo.db.dbclass.tableName + " E on R.EmpID = E.EmpID AND  R.EmpID in (Select P.EmpID From "
            //    + EEmpPositionInfo.db.dbclass.tableName + " P where P.EmpPosEffFr < '" + DateTime.Today.ToString("yyyy-MM-dd") + "' and ( P.EmpPosEffFr > '" + DateTime.Today.ToString("yyyy-MM-dd") + "' or P.EmpPosEffTo is Null ) and ( P.EmpFirstAuthorizationGp in ( Select A.AuthorizationGroupID From "
            //    + EAuthorizer.db.dbclass.tableName + " A where A.EmpID = " + CurID + " and R.EmpRequestStatus = '"
            //    + EEmpRequest.STATUS_USRSUBMIT.ToString() + "')) or ( P.EmpSecondAuthorizationGp in ( Select A.AuthorizationGroupID From "
            //    + EAuthorizer.db.dbclass.tableName + " A where A.EmpID = " + CurID + " and (R.EmpRequestStatus = '"
            //    + EEmpRequest.STATUS_FSTAPP.ToString() + "' or R.EmpRequestStatus = '"
            //    + EEmpRequest.STATUS_USRSUBMIT.ToString() + "'))))";

            // Start 0000105, KuangWei, 2014-10-21
            // Start 0000232, Miranda, 2015-07-02
            string select = "R.*, E.EmpNo, E.EmpEngSurname, E.EmpEngOtherName, E.EmpAlias, ISNULL ( L.LeaveCode , L2.LeaveCode ) as LeaveCode, ISNULL ( L.LeaveCodeDesc , L2.LeaveCodeDesc) as LeaveCodeDesc";
			// End 0000232, Miranda, 2015-07-02
            string from = "from " + db.dbclass.tableName + " R "
                + " LEFT JOIN " + EEmpPersonalInfo.db.dbclass.tableName + " E on R.EmpID = E.EmpID "
            + " Left Join " + EEmpPositionInfo.db.dbclass.tableName + " EP on EP.EmpID = E.EmpID "
            + " and EP.EmpPosEffFr <= '" + DateTime.Today.ToString("yyyy-MM-dd") + "' AND (EP.EmpPosEffTO >= '" + DateTime.Today.ToString("yyyy-MM-dd") + "' or EP.EmpPosEffTo is Null )"
            // Start 0000112, Miranda, 2015-01-11
			// Start 0000232, Miranda, 2015-07-02
            + " LEFT JOIN " + ERequestLeaveApplication.db.dbclass.tableName + " C on R.EmpRequestRecordID = C.RequestLeaveAppID and R.EmpRequestType = '" + EEmpRequest.TYPE_EELEAVEAPP + "'"
            +" LEFT JOIN " + ERequestLeaveApplicationCancel.db.dbclass.tableName + " CC on R.EmpRequestRecordID = CC.RequestLeaveAppCancelID and R.EmpRequestType = '" + EEmpRequest.TYPE_EELEAVECANCEL + "'"
            + " LEFT JOIN " + ELeaveApplication.db.dbclass.tableName + " LA on LA.LeaveAppID = CC.LeaveAppID"
            // End 0000112, Miranda, 2015-01-11
            + " LEFT JOIN " + ELeaveCode.db.dbclass.tableName + " L on C .RequestLeaveCodeID = L.LeaveCodeID "
            + " LEFT JOIN " + ELeaveCode.db.dbclass.tableName + " L2 on LA.LeaveCodeID = L2.LeaveCodeID";
            // End 0000232, Miranda, 2015-07-02
            // End 0000105, KuangWei, 2014-10-21

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

            if (repeater != null)
            {
                repeater.DataSource = view;
                repeater.DataBind();
            }
            return view;
        }
        else
            return null;
    }

    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        HtmlAnchor requestLink = (HtmlAnchor)e.Item.FindControl("requestLink");

        WebFormUtils.LoadKeys(db, row, cb);
        cb.Checked=false;
        cb.Visible = false;

        EEmpRequest obj = new EEmpRequest();
        EEmpRequest.db.toObject(((DataRowView)e.Item.DataItem).Row, obj);

        // resolve Request Type --> Description 
        Label lblEmpRequestType = (Label)e.Item.FindControl("EmpRequestType");

        switch (obj.EmpRequestType)
	    {
	        case EEmpRequest.TYPE_EELEAVEAPP:
                lblEmpRequestType.Text = "Leave Application";
                break;
            case EEmpRequest.TYPE_EELEAVECANCEL:
                lblEmpRequestType.Text = "Leave Application Cancellation";
                break;
            case EEmpRequest.TYPE_EEOTCLAIM:
                lblEmpRequestType.Text = "CL Requisition";
                break;
            case EEmpRequest.TYPE_EEOTCLAIMCANCEL:
                lblEmpRequestType.Text = "CL Requisition Cancel";
                break;
            // Start 0000112, Miranda, 2014-12-10
            case EEmpRequest.TYPE_EELATEWAIVE:
                lblEmpRequestType.Text = "Late Waive";
                break;
            case EEmpRequest.TYPE_EELATEWAIVECANCEL:
                lblEmpRequestType.Text = "Late Waive Cancel";
                break;
            // End 0000112, Miranda, 2014-12-10
            case EEmpRequest.TYPE_EEPROFILE:
                lblEmpRequestType.Text = "Personal Information";
                break;

            default:
		        lblEmpRequestType.Text = obj.EmpRequestType;
                break; 
	    }


        requestLink.HRef = HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "~/ESS_EmpRequestDetail.aspx?TargetEmpID=" + obj.EmpID + "&EmpRequestRecordID=" + obj.EmpRequestRecordID + "&EmpRequestID=" + obj.EmpRequestID);
        //requestLink.InnerText = row["EmpNo"].ToString();
        requestLink.Title = generateToolTipMessage(obj);
        ESSAuthorizationProcess authorization = new ESSAuthorizationProcess(dbConn);
        ArrayList workFlowDetailList = authorization.GetAuthorizationWorkFlowDetailList(obj.EmpID, obj.EmpRequestType);
        EAuthorizationWorkFlowDetail workFlowDetail = authorization.GetCurrentWorkFlowDetailObject(workFlowDetailList, obj.EmpRequestLastAuthorizationWorkFlowIndex, CurID);
        if (workFlowDetail != null)
        {
            ArrayList authorizerList =  workFlowDetail.GetActualAutorizerObjectList(dbConn, CurID);
            foreach (EAuthorizer authorizer in authorizerList)
                if (!authorizer.AuthorizerIsReadOnly)
                    cb.Visible = true;
        }
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

    protected void Authorize_Click(object sender, EventArgs e)
    {

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();
        ArrayList list = new ArrayList();
        foreach (RepeaterItem i in Repeater.Items)
        {
            CheckBox cb = (CheckBox)i.FindControl("ItemSelect");
            if (cb.Checked)
            {
                DBObject o = db.createObject();
                WebFormUtils.GetKeys(db, o, cb);
                list.Add(o);
            }

        }
        foreach (EEmpRequest o in list)
        {
            db.select(dbConn, o);
            // Start 0000063, KuangWei, 2014-08-25
            o.EmpRequestRejectReason = RejectReason.Text;
            // End 0000063, KuangWei, 2014-08-25
            try
            {
                ESSAuthorizationProcess authorization = new ESSAuthorizationProcess(dbConn);
                    

                ERequestOTClaim empOTRequest = new ERequestOTClaim();
                empOTRequest.RequestOTClaimID = o.EmpRequestRecordID;

                if (ERequestOTClaim.db.select(dbConn, empOTRequest))
                {

                    // handle the case where CL Requisition requests are approved through Application Approval screen
                    if (empOTRequest.RequestOTClaimEffectiveDate.Ticks.Equals(0))
                    {
                        empOTRequest.RequestOTClaimEffectiveDate = AppUtils.ServerDateTime();
                        empOTRequest.RequestOTClaimDateExpiry = calExpiryDate(empOTRequest.RequestOTClaimPeriodFrom);
                        ERequestOTClaim.db.update(dbConn, empOTRequest);
                    }
                }

                authorization.AuthorizeAction(o, CurID);
            }
            catch(Exception ex)
            {
                errors.addError(ex.Message);
            }
        }
        view = loadData(info, db, Repeater);

    }

    private DateTime calExpiryDate(DateTime effectiveDate)
    {
        DateTime dtFrom = effectiveDate;
        DateTime dtTo = DateTime.Today;
        int expiryInt;

        string essDefEotExpiry = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_DEF_EOT_EXPIRY);
        string essDefEotExpiryType = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_DEF_EOT_EXPIRY_TYPE);

        if (string.IsNullOrEmpty(essDefEotExpiry)) // Default is one Month 
        {
            expiryInt = 1;
        }
        else
        {
            int.TryParse(essDefEotExpiry, out expiryInt);
        }

        if (string.IsNullOrEmpty(essDefEotExpiryType) || essDefEotExpiryType.Equals("M")) // Default type is Month, "M" means Month
        {
            dtTo = dtFrom.AddMonths(expiryInt).AddDays(-1);
        }
        else if (essDefEotExpiryType.Equals("Y")) // "Y" means Year
        {
            dtTo = dtFrom.AddYears(expiryInt).AddDays(-1);
        }
        else if (essDefEotExpiryType.Equals("D")) // "D" means Day
        {
            dtTo = dtFrom.AddDays(expiryInt).AddDays(-1);
        }

        // start Ricky So, 2014-08-21, as requested by HK Ballet, default to last day of the month
        dtTo = dtTo.AddDays(-1 * (dtTo.Day-1)).AddMonths(1).AddDays(-1);
        // end

        return dtTo;
    }


    protected void Reject_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);;
        errors.clear();
        ArrayList list = new ArrayList();
        foreach (RepeaterItem i in Repeater.Items)
        {
            CheckBox cb = (CheckBox)i.FindControl("ItemSelect");
            if (cb.Checked)
            {
                DBObject o = db.createObject();
                WebFormUtils.GetKeys(db, o, cb);
                list.Add(o);
            }

        }
        foreach (EEmpRequest o in list)
        {
            if (db.select(dbConn, o))
            {
                o.EmpRequestRejectReason = RejectReason.Text;
                try
                {
                    ESSAuthorizationProcess authorization = new ESSAuthorizationProcess(dbConn);
                    authorization.RejectedAction(o, CurID);

                }
                catch (Exception ex)
                {
                    errors.addError(ex.Message);
                }
            }
        }
        view = loadData(info, db, Repeater);

    }
    protected void btnSaveDelegate_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        string[] strDelegateEmpNoList = txtDelegateEmpNoList.Text.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
        ESSAuthorizationProcess authorizationProcess = new ESSAuthorizationProcess(dbConn);
        List<EEmpPersonalInfo> delegateEmpInfoList = new List<EEmpPersonalInfo>();
        foreach (string delegateEmpNo in strDelegateEmpNoList)
        {
            EEmpPersonalInfo delegateEmpInfo = authorizationProcess.GetEmpInfo(delegateEmpNo);
            if (delegateEmpInfo == null)
                errors.addError("Invalid Employee No: " + delegateEmpNo);
            else
            {
                delegateEmpInfoList.Add(delegateEmpInfo);
                EEmpTermination empTerm = EEmpTermination.GetObjectByEmpID(dbConn, delegateEmpInfo.EmpID);
                if (empTerm != null)
                    if (empTerm.EmpTermLastDate < AppUtils.ServerDateTime().Date)
                        errors.addError("Employee " + delegateEmpNo + " is Terminated");
            }
        }
        if (!errors.isEmpty())
            return;

        DBFilter authorizerDBFilter = new DBFilter();
        authorizerDBFilter.add(new Match("EmpID", CurID));
        EAuthorizerDelegate.db.delete(dbConn, authorizerDBFilter);

        foreach (EEmpPersonalInfo delegateEmpInfo in delegateEmpInfoList)
        {
            EAuthorizerDelegate authorDelegate = new EAuthorizerDelegate();
            authorDelegate.EmpID = CurID;
            authorDelegate.AuthorizerDelegateEmpID = delegateEmpInfo.EmpID;
            EAuthorizerDelegate.db.insert(dbConn, authorDelegate);
        }

        errors.addError("Delegate employee is submitted");

    }

    protected string generateToolTipMessage(EEmpRequest empRequest)
    {
        string message = "Employee No:\t%EMP_NO%\r\n" +
                            "Employee:\t%EMP_NAME%\r\n" +
                            "Leave Date:\t%LEAVEAPP_PERIOD%\r\n" +
                            "Leave Type:\t%LEAVE_CODE%\r\n" +
                            "Taken     :\t%LEAVEAPP_DAYS% %LEAVEAPP_UNIT%\r\n" +
                            "Hours Claims:\t%LEAVEAPP_HOURSCLAIM%\r\n" +
                            "Reason:\t%LEAVEAPPCANCEL_REMARK%\r\n";

        ESSAuthorizationProcess process = new ESSAuthorizationProcess(dbConn);

        EEmpPersonalInfo ApplicantEmpInfo = process.GetEmpInfo(empRequest.EmpID);

        Hashtable mailContentParameterTable = new Hashtable();
        mailContentParameterTable = process.GenerateEmpInfoHashTable(ApplicantEmpInfo, mailContentParameterTable);

        if (empRequest.EmpRequestType == EEmpRequest.TYPE_EELEAVEAPP)
        {
            DBFilter RequestEmpFilter = new DBFilter();
            RequestEmpFilter.add(new Match("RequestLeaveAppID", empRequest.EmpRequestRecordID));
            ArrayList empRequestList = ERequestLeaveApplication.db.select(dbConn, RequestEmpFilter);
            if (empRequestList.Count > 0)
            {
                ERequestLeaveApplication empLeaveRequest = (ERequestLeaveApplication)empRequestList[0];

                mailContentParameterTable = process.GenerateRequestLeaveApplicationHashTable(empLeaveRequest, mailContentParameterTable);
            }
        }
        else if (empRequest.EmpRequestType == EEmpRequest.TYPE_EELEAVECANCEL)
        {
            DBFilter RequestEmpFilter = new DBFilter();
            RequestEmpFilter.add(new Match("RequestLeaveAppCancelID", empRequest.EmpRequestRecordID));
            ArrayList empRequestList = ERequestLeaveApplicationCancel.db.select(dbConn, RequestEmpFilter);
            if (empRequestList.Count > 0)
            {
                ERequestLeaveApplicationCancel requestLeaveAppCancel = (ERequestLeaveApplicationCancel)empRequestList[0];
                mailContentParameterTable.Add("%LEAVEAPPCANCEL_REMARK%", requestLeaveAppCancel.RequestLeaveAppCancelReason);
                ELeaveApplication leaveApp = new ELeaveApplication();
                leaveApp.LeaveAppID = requestLeaveAppCancel.LeaveAppID;
                if (ELeaveApplication.db.select(dbConn, leaveApp))
                {
                    ERequestLeaveApplication dummyRequestLeave = ERequestLeaveApplication.CreateDummyRequestLeaveAppliction(leaveApp);
                    mailContentParameterTable = process.GenerateRequestLeaveApplicationHashTable(dummyRequestLeave, mailContentParameterTable);
                }
            }
        }
        string MessageBodyTemplate = message;
        foreach (string key in mailContentParameterTable.Keys)
        {
            MessageBodyTemplate = MessageBodyTemplate.Replace(key, mailContentParameterTable[key].ToString());
        }
        string[] messageBodyLineArray = MessageBodyTemplate.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        string newMessageBody = string.Empty;
        foreach (string messageLine in messageBodyLineArray)
            if (string.IsNullOrEmpty(messageLine) || messageLine.IndexOf("%") == messageLine.LastIndexOf("%"))
                if (string.IsNullOrEmpty(newMessageBody))
                    newMessageBody = messageLine;
                else
                    newMessageBody += "\r\n" + messageLine;
        return newMessageBody;
    }
}
