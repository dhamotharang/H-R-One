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
using HROne.Lib.Entities;

public partial class Attendance_AttendanceRecordList : HROneWebControl
{
    protected DBManager db = EAttendanceRecord.db;
    protected SearchBinding sBinding;
    protected ListInfo info;
    protected DataView view;
    public const string FUNCTION_CODE = "ATT008";

    //public Binding newBinding;

    public bool IsAllowEdit = true;

    public int ImetritisReapeter = 0;

    private int CurID = 0;
    //private int m_Year = AppUtils.ServerDateTime().Year;
    //private int m_Month = AppUtils.ServerDateTime().Month;

    public int CurrentEmpID
    {
        get { return CurID; }
        set
        {
            CurID = value;
            EmpID.Value = CurID.ToString();
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;
        IsAllowEdit = toolBar.DeleteButton_Visible;
        Recalculate.Visible = IsAllowEdit;
        SelectAllPanel.Visible = IsAllowEdit;



        sBinding = new SearchBinding(dbConn, db);
        sBinding.add(new HiddenMatchBinder(EmpID));
        sBinding.initValues("RosterCodeID", null, ERosterCode.VLRosterCode, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        sBinding.add(new DropDownVLSearchBinder(Year, "Year(AttendanceRecordDate)", EAttendanceRecord.VLAttendanceRecordYear, false));//, null, "Year(pp.PayPeriodFR)"));
        sBinding.add(new DropDownVLSearchBinder(Month, "Month(AttendanceRecordDate)", Values.VLMonth, false));
        sBinding.initValues("AttendanceRecordIsAbsent", null, Values.VLYesNo, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        sBinding.init(DecryptedRequest, null);

        //newBinding = new Binding(dbConn, db);
        //newBinding.add(EmpID);
        //newBinding.add(new DropDownVLBinder(db, RosterCodeID, ERosterCode.VLRosterCode));
        //newBinding.add(AttendanceRecordDate);
        //newBinding.add(AttendanceRecordRemark);


        //newBinding.init(DecryptedRequest, null);
        info = ListFooter.ListInfo;

        if (!Page.IsPostBack)
        {
            try
            {
                DateTime Today = AppUtils.ServerDateTime();
                Month.SelectedValue = Today.Month.ToString();
                Year.SelectedValue = Today.Year.ToString();

            }
            catch
            {
            }
            if (CurID > 0)
                //if (loadObject())
                //{
                view = loadData(info, db, Repeater);
            //}
        }

    }

    public void Page_PreRender(object sender, EventArgs e)
    {

        //if (!IsAllowEdit)
        //{
        //    Delete.Visible = false;
        //}
    }


    public DataView loadData(ListInfo info, DBManager db, DataList repeater)
    {
        DBFilter filter = sBinding.createFilter();
        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);
        else
            filter.add("AttendanceRecordDate", true);

        string select = "c.*";
        string from = "from " + db.dbclass.tableName + " c ";

        //DateTime startDate = new DateTime(m_Year, m_Month, 1);
        //DateTime endDate = new DateTime(m_Year, m_Month, DateTime.DaysInMonth(m_Year, m_Month));


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

        Repeater.EditItemIndex = -1;
        view = loadData(info, db, Repeater);

    }

    protected void Repeater_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item
    || e.Item.ItemType == ListItemType.AlternatingItem
    || e.Item.ItemType == ListItemType.SelectedItem
    || e.Item.ItemType == ListItemType.EditItem)
        {
            EAttendanceRecord obj = new EAttendanceRecord();
            db.toObject(((DataRowView)e.Item.DataItem).Row, obj);
            Hashtable values = new Hashtable();
            db.populate(obj, values);

            if (e.Item.ItemIndex == Repeater.EditItemIndex)
            {
                Binding eBinding;
                eBinding = new Binding(dbConn, db);
                eBinding.add(EmpID);
                eBinding.add((HtmlInputHidden)e.Item.FindControl("AttendanceRecordID"));
                eBinding.add((Label)e.Item.FindControl("AttendanceRecordDate"));
                eBinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("RosterCodeID"), ERosterCode.VLRosterCode));
                eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordRosterCodeInTimeOverride"));
                eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordRosterCodeLunchStartTimeOverride"));
                eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordRosterCodeLunchEndTimeOverride"));
                eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordRosterCodeOutTimeOverride"));
                eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordWorkStart"));
                eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordLunchOut"));
                eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordLunchIn"));
                eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordWorkEnd"));
                eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordWorkStartLocation"));
                eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordLunchOutLocation"));
                eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordLunchInLocation"));
                eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordWorkEndLocation"));
                eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordActualLateMins"));
                eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordActualEarlyLeaveMins"));
                eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordActualLunchLateMins"));
                eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordActualLunchEarlyLeaveMins"));
                eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordActualOvertimeMins"));
                eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordActualLunchOvertimeMins"));
                eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordActualWorkingHour"));
                eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordActualWorkingDay"));
                eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordActualLunchTimeMins"));
                eBinding.add(new CheckBoxBinder(db, (CheckBox)e.Item.FindControl("AttendanceRecordIsAbsent")));
                eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordRemark"));


                eBinding.init(Request, Session);

                eBinding.toControl(values);

                if (obj.AttendanceRecordOverrideBonusEntitled)
                    if (obj.AttendanceRecordHasBonus)
                        ((DropDownList)e.Item.FindControl("AttendanceRecordHasBonus")).SelectedValue = "Y";
                    else
                        ((DropDownList)e.Item.FindControl("AttendanceRecordHasBonus")).SelectedValue = "N";
                else
                    ((DropDownList)e.Item.FindControl("AttendanceRecordHasBonus")).SelectedValue = "";

                ((TextBox)e.Item.FindControl("AttendanceRecordRosterCodeInTimeOverride")).MaxLength = 0;
                ((TextBox)e.Item.FindControl("AttendanceRecordRosterCodeLunchStartTimeOverride")).MaxLength = 0;
                ((TextBox)e.Item.FindControl("AttendanceRecordRosterCodeLunchEndTimeOverride")).MaxLength = 0;
                ((TextBox)e.Item.FindControl("AttendanceRecordRosterCodeOutTimeOverride")).MaxLength = 0;
                ((TextBox)e.Item.FindControl("AttendanceRecordWorkStart")).MaxLength = 0;
                ((TextBox)e.Item.FindControl("AttendanceRecordLunchOut")).MaxLength = 0;
                ((TextBox)e.Item.FindControl("AttendanceRecordLunchIn")).MaxLength = 0;
                ((TextBox)e.Item.FindControl("AttendanceRecordWorkEnd")).MaxLength = 0;
            }
            else
            {
                // Start 0000112, Miranda, 2015-01-11
                e.Item.FindControl("Edit").Visible = true & IsAllowEdit ; //&& !hasSubmitLateWaive(obj.EmpID, obj.AttendanceRecordID);
                // End 0000112, Miranda, 2015-01-11
                e.Item.FindControl("DeleteItem").Visible = true & IsAllowEdit;
                Binding eBinding;
                eBinding = new Binding(dbConn, db);
                eBinding.add((HtmlInputHidden)e.Item.FindControl("AttendanceRecordID"));
                eBinding.add(new BlankZeroLabelVLBinder(db, (Label)e.Item.FindControl("RosterCodeID"), ERosterCode.VLRosterCode));
                eBinding.init(Request, Session);
                eBinding.toControl(values);

                //HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("AttendanceRecordID");
                //h.Value = ((DataRowView)e.Item.DataItem)["AttendanceRecordID"].ToString();
                if (obj.AttendanceRecordOverrideBonusEntitled)
                    if (obj.AttendanceRecordHasBonus)
                        ((Label)e.Item.FindControl("AttendanceRecordHasBonus")).Text = "Yes";
                    else
                        ((Label)e.Item.FindControl("AttendanceRecordHasBonus")).Text = "No";
                else
                    ((Label)e.Item.FindControl("AttendanceRecordHasBonus")).Text = "By Attendance Plan";
            }
        }
        HROne.Common.WebUtility.WebControlsLocalization(Session, e.Item.Controls);
    }

    //// Start 0000112, Miranda, 2015-01-11
    //private bool hasSubmitLateWaive(int empID, int attendanceRecordID)
    //{
    //    DBFilter filter = new DBFilter();
    //    filter.add(new Match("EmpID", empID));
    //    filter.add(new Match("AttendanceRecordID", attendanceRecordID));
    //    ArrayList lwList = ELateWaive.db.select(dbConn, filter);
    //    if (lwList != null && lwList.Count > 0)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}
    //// End 0000112, Miranda, 2015-01-11

    protected void Repeater_ItemCommand(object source, DataListCommandEventArgs e)
    {
        Button b = (Button)e.CommandSource;




        if (b.ID.Equals("Edit"))
        {
            int intAttendanceRecordID = 0;
            if (int.TryParse(((HtmlInputHidden)e.Item.FindControl("AttendanceRecordID")).Value, out intAttendanceRecordID))
            {
                Attendance_AttendanceRecordDetail_Edit1.LoadAttendanceRecord(intAttendanceRecordID);
                Attendance_AttendanceRecordDetail_Edit1.Show();

            }
            //Repeater.EditItemIndex = e.Item.ItemIndex;
            //view = loadData(info, db, Repeater);
        }
        else if (b.ID.Equals("Cancel"))
        {
            Repeater.EditItemIndex = -1;

            view = loadData(info, db, Repeater);
        }
        else if (b.ID.Equals("Save"))
        {
            Binding eBinding;


            eBinding = new Binding(dbConn, db);
            eBinding.add(EmpID);
            eBinding.add((HtmlInputHidden)e.Item.FindControl("AttendanceRecordID"));
            eBinding.add((Label)e.Item.FindControl("AttendanceRecordDate"));
            eBinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("RosterCodeID"), ERosterCode.VLRosterCode));
            eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordRosterCodeInTimeOverride"));
            eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordRosterCodeLunchStartTimeOverride"));
            eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordRosterCodeLunchEndTimeOverride"));
            eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordRosterCodeOutTimeOverride"));
            eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordWorkStart"));
            eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordLunchOut"));
            eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordLunchIn"));
            eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordWorkEnd"));
            eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordWorkStartLocation"));
            eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordLunchOutLocation"));
            eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordLunchInLocation"));
            eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordWorkEndLocation"));
            eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordActualLateMins"));
            eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordActualEarlyLeaveMins"));
            eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordActualLunchLateMins"));
            eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordActualLunchEarlyLeaveMins"));
            eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordActualOvertimeMins"));
            eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordActualLunchOvertimeMins"));
            eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordActualWorkingHour"));
            eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordActualWorkingDay"));
            eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordActualLunchTimeMins"));
            eBinding.add(new CheckBoxBinder(db, (CheckBox)e.Item.FindControl("AttendanceRecordIsAbsent")));
            eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordRemark"));

            eBinding.init(Request, Session);


            EAttendanceRecord obj = new EAttendanceRecord();
            Hashtable values = new Hashtable();

            PageErrors errors = PageErrors.getErrors(db, Page.Master);
            errors.clear();


            eBinding.toValues(values);
            db.validate(errors, values);

            if (!errors.isEmpty())
            {
                return;
            }

            db.parse(values, obj);

            DropDownList rosterCodeID = ((DropDownList)e.Item.FindControl("AttendanceRecordHasBonus"));
            if (rosterCodeID.SelectedValue.Equals("Y", StringComparison.CurrentCultureIgnoreCase))
            {
                obj.AttendanceRecordOverrideBonusEntitled = true;
                obj.AttendanceRecordHasBonus = true;
            }
            else if (rosterCodeID.SelectedValue.Equals("N", StringComparison.CurrentCultureIgnoreCase))
            {
                obj.AttendanceRecordOverrideBonusEntitled = true;
                obj.AttendanceRecordHasBonus = false;
            }
            else
            {
                obj.AttendanceRecordOverrideBonusEntitled = false;
                obj.AttendanceRecordHasBonus = false;
            }

            if (!errors.isEmpty())
            {
                return;
            }

            WebUtils.StartFunction(Session, FUNCTION_CODE, obj.EmpID);
            db.update(dbConn, obj);
            WebUtils.EndFunction(dbConn);

            Repeater.EditItemIndex = -1;
            view = loadData(info, db, Repeater);

        }


    }

    protected void Delete_Click(object sender, EventArgs e)
    {

        ArrayList list = new ArrayList();
        foreach (DataListItem item in Repeater.Items)
        {
            CheckBox c = (CheckBox)item.FindControl("DeleteItem");
            HtmlInputHidden h = (HtmlInputHidden)item.FindControl("AttendanceRecordID");
            if (c.Checked)
            {
                EAttendanceRecord obj = new EAttendanceRecord();
                obj.AttendanceRecordID = Int32.Parse(h.Value);
                list.Add(obj);
            }
        }
        WebUtils.StartFunction(Session, FUNCTION_CODE, CurrentEmpID);

        foreach (EAttendanceRecord obj in list)
        {
            if (db.select(dbConn, obj))
                db.delete(dbConn, obj);
        }
        WebUtils.EndFunction(dbConn);
        view = loadData(info, db, Repeater);

    }
    protected void YearMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        Repeater.EditItemIndex = -1;
        view = loadData(info, db, Repeater);
    }
    protected void Recalculate_Click(object sender, EventArgs e)
    {

        ArrayList list = new ArrayList();
        foreach (DataListItem item in Repeater.Items)
        {
            CheckBox c = (CheckBox)item.FindControl("DeleteItem");
            HtmlInputHidden h = (HtmlInputHidden)item.FindControl("AttendanceRecordID");
            if (c.Checked)
            {
                EAttendanceRecord obj = new EAttendanceRecord();
                obj.AttendanceRecordID = Int32.Parse(h.Value);
                list.Add(obj);
            }
        }
        HROne.Attendance.AttendanceProcess attendanceProcess = new HROne.Attendance.AttendanceProcess(dbConn);
        WebUtils.StartFunction(Session, FUNCTION_CODE, CurrentEmpID);
        foreach (EAttendanceRecord obj in list)
        {
            EAttendanceRecord.db.select(dbConn, obj);
            attendanceProcess.GetAttendanceTimeResult(obj);
            EAttendanceRecord.db.update(dbConn, obj);
        }
        WebUtils.EndFunction(dbConn);
        view = loadData(info, db, Repeater);

    }
    protected void Attendance_AttendanceRecordDetail_Closed(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);
    }
    protected void ChangePage_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);
    }

}
