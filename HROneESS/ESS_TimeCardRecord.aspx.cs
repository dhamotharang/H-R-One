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

public partial class Customize_ESS_TimeCardRecord : HROneWebPage
{
    public Binding binding;
    public DBManager db = EEmpPersonalInfo.db;
    public EEmpPersonalInfo obj;
    public int CurID = -1;

    protected SearchBinding sbinding;
    protected ListInfo info;
    protected DataView view;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session))
            return;
        binding = new Binding(dbConn, db);

        binding.init(Request, Session);

        EESSUser user = WebUtils.GetCurUser(Session);
        if (user != null)
        {
            CurID = user.EmpID;
        }

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (!Page.IsPostBack)
        {

            if (CurID > 0)
            {
                loadObject();
            }
            else
            {

            }
        }

        sbinding = new SearchBinding(dbConn, ETimeCardRecord.db);
        ////binding.initValues("LeaveAppUnit", null, Values.VLLeaveUnit, null);
        //binding.add(new DropDownVLSearchBinder(LeaveType, "l.LeaveTypeID", ELeaveType.VLLeaveType));
        ////binding.add(new FieldDateRangeSearchBinder(LeaveAppDateFrom.TextBox, LeaveAppDateTo.TextBox, "LeaveAppDateFrom").setUseCurDate(false));

        sbinding.init(Request.QueryString, null);

        info = ListFooter.ListInfo;


    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);

    }
    protected bool loadObject()
    {
        obj = new EEmpPersonalInfo();
        obj.EmpID = CurID;
        if (!db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);
        return true;
    }

    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        
        // Start 0000183, KuangWei, 2015-04-10
        if (isSupervisor(CurID))
        {
            TeamRecordExport.Visible = true;
        }
        // End 0000183, KuangWei, 2015-04-10

        DBFilter filter = sbinding.createFilter();
        OR orTimeCardRecordHolder = new OR();
        orTimeCardRecordHolder.add(new Match("EmpID", CurID));
        //if (!string.IsNullOrEmpty(timeCardNo))
            orTimeCardRecordHolder.add(new Match("TimeCardRecordCardNo", GetTimeCardNo(CurID)));
        filter.add(orTimeCardRecordHolder);

        DateTime dtTimeCardRecordDate;
        if (!DateTime.TryParse(TimeCardRecordDate.Value, out dtTimeCardRecordDate))
            dtTimeCardRecordDate = AppUtils.ServerDateTime().Date;
        filter.add(new Match("TimeCardRecordDateTime", ">=", dtTimeCardRecordDate));
        filter.add(new Match("TimeCardRecordDateTime", "<", dtTimeCardRecordDate.AddDays(1)));


        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);


        string select = "*";
        string from = "from " + ETimeCardRecord.db.dbclass.tableName + "";


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

    private string GetTimeCardNo(int EmpID)
    {
        EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
        empInfo.EmpID = EmpID;
        if (EEmpPersonalInfo.db.select(dbConn, empInfo))
            if (!string.IsNullOrEmpty(empInfo.EmpTimeCardNo))
                return empInfo.EmpTimeCardNo;
            else
                return empInfo.EmpNo;
        else
            return string.Empty;
    }
    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        //DataRowView row = (DataRowView)e.Item.DataItem;
        //ELeaveApplication obj = new ELeaveApplication();
        //ELeaveApplication.db.toObject(row.Row, obj);

        //Button cancelButton = (Button)e.Item.FindControl("Cancel");
        //if (obj.EmpPayrollID > 0 || obj.EmpPaymentID > 0)
        //    cancelButton.Visible = false;
        //else
        //{
        //    ELeaveCode leaveCode = new ELeaveCode();
        //    leaveCode.LeaveCodeID = obj.LeaveCodeID;
        //    if (ELeaveCode.db.select(dbConn, leaveCode))
        //    {
        //        if (leaveCode.LeaveCodeHideInESS)
        //            cancelButton.Visible = false;
        //        else
        //        {
        //            //  Temporary set to invisible to add more constraint before launch
        //            cancelButton.Visible = false;
        //            cancelButton.Attributes["LeaveAppID"] = obj.LeaveAppID.ToString();
        //        }
        //    }
        //    else
        //        cancelButton.Visible = false;
        //}
        HROne.Common.WebUtility.WebControlsLocalization(Session, e.Item.Controls);
    }

    protected void Repeater_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        //PageErrors errors = PageErrors.getErrors(ELeaveApplication.db, Page.Master);

        //int LeaveAppID = 0;
        //if (int.TryParse(((Button)e.Item.FindControl("Cancel")).Attributes["LeaveAppID"], out LeaveAppID))
        //{
        //    ELeaveApplication obj = new ELeaveApplication();
        //    obj.LeaveAppID = LeaveAppID;
        //    if (ELeaveApplication.db.select(dbConn, obj))
        //    {
        //        if (obj.EmpPayrollID > 0 || obj.EmpPaymentID > 0)
        //        {
        //            //  message prompt to user
        //        }
        //        else
        //        {
        //            ELeaveCode leaveCode = new ELeaveCode();
        //            leaveCode.LeaveCodeID = obj.LeaveCodeID;
        //            if (ELeaveCode.db.select(dbConn, leaveCode))
        //            {
        //                if (leaveCode.LeaveCodeHideInESS)
        //                {
        //                    //  message prompt to user
        //                }
        //                else
        //                {
        //                    AppUtils.StartFunction(dbConn, 0, "PER009", obj.EmpID, true);
        //                    ELeaveApplication.db.delete(dbConn, obj);
        //                    AppUtils.EndFunction(dbConn);
        //                    ESSAuthorizationProcess authorization = new ESSAuthorizationProcess(dbConn);
        //                    authorization.CancelLeaveApplicatoin(obj);
        //                    errors.addError(HROne.Common.WebUtility.GetLocalizedString("The leave application is cancelled"));
        //                }
        //            }
        //            else
        //            {
        //                //  message prompt to user
        //            }
        //        }
        //    }
        //}
        //view = loadData(info, db, Repeater);



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

    // Start 0000183, KuangWei, 2015-04-10
    public const string TABLE_NAME = "TeamRecordExport";
    public const string FIELD_EMP_NO = "Emp No";
    public const string FIELD_EMP_NAME = "Employee Name";
    public const string FIELD_TIME_CARD_RECORD_NO = "Time Card Record No";
    public const string FIELD_TIME_CARD_DATE_TIME = "Time Card Date Time";
    public const string FIELD_TIME_CARD_RECORD_LOCATION = "Time Card Record Location";

    protected void Export_Click(object sender, EventArgs e)
    {
        string exportFileName = System.IO.Path.GetTempFileName();
        System.IO.File.Delete(exportFileName);
        exportFileName += ".xls";
        HROne.Export.ExcelExport export = new HROne.Export.ExcelExport(exportFileName);
        DataSet dataSet = new DataSet();

        DataTable tmpDataTable = dataSet.Tables.Add(TABLE_NAME);
        tmpDataTable.Columns.Add(FIELD_EMP_NO, typeof(string));
        tmpDataTable.Columns.Add(FIELD_EMP_NAME, typeof(string));
        tmpDataTable.Columns.Add(FIELD_TIME_CARD_RECORD_NO, typeof(string));
        tmpDataTable.Columns.Add(FIELD_TIME_CARD_DATE_TIME, typeof(string));//DateTime
        tmpDataTable.Columns.Add(FIELD_TIME_CARD_RECORD_LOCATION, typeof(string));

        //DBFilter filter = new DBFilter();
        //filter.add(new Match("EmpID", CurID));
        //ArrayList empRosterTableGroupList = EEmpRosterTableGroup.db.select(dbConn, filter);

        //if (empRosterTableGroupList.Count > 0)
        //{
        //    EEmpRosterTableGroup empRosterTableGroup = (EEmpRosterTableGroup)empRosterTableGroupList[0];
        //    DBFilter subordinateFilter = new DBFilter();
        //    subordinateFilter.add(new Match("EmpID", "!=", empRosterTableGroup.EmpID));
        //    subordinateFilter.add(new Match("RosterTableGroupID", empRosterTableGroup.RosterTableGroupID));
    
        DBFilter m_empIDFilter = new DBFilter();
        DBFilter m_inFilter = new DBFilter();

        m_inFilter.add(new Match("EmpID", CurID));
        m_inFilter.add(new Match("EmpRosterTableGroupIsSupervisor", "!=", 0));

        m_empIDFilter.add(new IN("RosterTableGroupID", "SELECT RosterTableGroupID FROM EmpRosterTableGroup", m_inFilter));

        DBFilter m_staffFilter = new DBFilter();
        m_staffFilter.add(new IN("EmpID", "SELECT EmpID FROM EmpRosterTableGroup", m_empIDFilter));

        ArrayList m_subordinateList = EEmpPersonalInfo.db.select(dbConn, m_staffFilter);

        if (m_subordinateList.Count <= 0)
        {
            PageErrors errors = PageErrors.getErrors(db, Page.Master);
            errors.addError("Employee not selected");
        }else
        {
            foreach (EEmpPersonalInfo m_subordinate in m_subordinateList)
            {

                DBFilter timeCardRecordFilter = new DBFilter();
                OR orTimeCardRecordHolder = new OR();
                orTimeCardRecordHolder.add(new Match("EmpID", m_subordinate.EmpID));
                orTimeCardRecordHolder.add(new Match("TimeCardRecordCardNo", GetTimeCardNo(m_subordinate.EmpID)));

                DateTime dtTimeCardRecordDate;
                if (!DateTime.TryParse(TimeCardRecordDate.Value, out dtTimeCardRecordDate))
                    dtTimeCardRecordDate = AppUtils.ServerDateTime().Date;
                timeCardRecordFilter.add(new Match("TimeCardRecordDateTime", ">=", dtTimeCardRecordDate));
                timeCardRecordFilter.add(new Match("TimeCardRecordDateTime", "<", dtTimeCardRecordDate.AddDays(1)));

                timeCardRecordFilter.add(orTimeCardRecordHolder);

                ArrayList timeCardRecordList = ETimeCardRecord.db.select(dbConn, timeCardRecordFilter);
                if (timeCardRecordList.Count > 0)
                {
                    foreach (ETimeCardRecord timeCardRecord in timeCardRecordList)
                    {
                        DataRow row = tmpDataTable.NewRow();
                        EEmpPersonalInfo empPersonInfo = new EEmpPersonalInfo();
                        empPersonInfo.EmpID = timeCardRecord.EmpID;
                        if (EEmpPersonalInfo.db.select(dbConn, empPersonInfo))
                        {
                            row[FIELD_EMP_NO] = empPersonInfo.EmpNo;
                            row[FIELD_EMP_NAME] = empPersonInfo.EmpEngFullName;
                        }

                        row[FIELD_TIME_CARD_RECORD_NO] = timeCardRecord.TimeCardRecordCardNo;
                        row[FIELD_TIME_CARD_DATE_TIME] = timeCardRecord.TimeCardRecordDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                        row[FIELD_TIME_CARD_RECORD_LOCATION] = timeCardRecord.TimeCardRecordLocation;

                        tmpDataTable.Rows.Add(row);
                    }
                }
            }

            export.Update(dataSet);
            WebUtils.TransmitFile(Response, exportFileName, "TeamRecordExport_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true);
            Response.End();
        }
    }

    private bool isSupervisor(int EmpID)
    {
        DBFilter dbFilter = new DBFilter();
        dbFilter.add(new Match("EmpID", EmpID));
        dbFilter.add(new Match("NOT empRosterTableGroupIsSupervisor", 0));

        ArrayList empRosterTableGroupList = EEmpRosterTableGroup.db.select(dbConn, dbFilter);
        if (empRosterTableGroupList.Count > 0)
        {
            EEmpRosterTableGroup empRosterTableGroup = (EEmpRosterTableGroup)empRosterTableGroupList[0];
            if (empRosterTableGroup.EmpRosterTableGroupIsSupervisor)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
    // End 0000183, KuangWei, 2015-04-10
}
