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

public partial class System_EmailAuditTrail : HROneWebPage
{
    private const string FUNCTION_CODE = "SYS000";
    public Hashtable selectedFunctionList = new Hashtable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        
        toolBar.FunctionCode = FUNCTION_CODE;

        if (cboReportFilter.SelectedValue.Equals("ALL", StringComparison.CurrentCultureIgnoreCase))
        {
            SystemPanel.Visible = true;
            SecurityPanel.Visible = true;
            PersonnelPanel.Visible = true;
            LeavePanel.Visible = true;
            PayrollPanel.Visible = true;
            MPFPanel.Visible = true;
            CostCenterPanel.Visible = true;
            AttendancePanel.Visible = true;
            TaxationPanel.Visible = true;
            TrainingPanel.Visible = true;
            //PersonnelReportsPanel.Visible = true;
            //PayrollReportsPanel.Visible = true;
            //TaxationReportsPanel.Visible = true;
        }
        else
        {
            SystemPanel.Visible = false;
            SecurityPanel.Visible = false;
            PersonnelPanel.Visible = false;
            LeavePanel.Visible = false;
            PayrollPanel.Visible = false;
            MPFPanel.Visible = false;
            CostCenterPanel.Visible = false;
            AttendancePanel.Visible = false;
            TaxationPanel.Visible = false;
            TrainingPanel.Visible = false;
            //PersonnelReportsPanel.Visible = false;
            //PayrollReportsPanel.Visible = false;
            //TaxationReportsPanel.Visible = false;

            if (cboReportFilter.SelectedValue.Equals("System", StringComparison.CurrentCultureIgnoreCase))
                SystemPanel.Visible = true;
            else if (cboReportFilter.SelectedValue.Equals("Security", StringComparison.CurrentCultureIgnoreCase))
                SecurityPanel.Visible = true;
            else if (cboReportFilter.SelectedValue.Equals("Personnel", StringComparison.CurrentCultureIgnoreCase))
                PersonnelPanel.Visible = true;
            else if (cboReportFilter.SelectedValue.Equals("Leave", StringComparison.CurrentCultureIgnoreCase))
                LeavePanel.Visible = true;
            else if (cboReportFilter.SelectedValue.Equals("Payroll", StringComparison.CurrentCultureIgnoreCase))
                PayrollPanel.Visible = true;
            else if (cboReportFilter.SelectedValue.Equals("MPF", StringComparison.CurrentCultureIgnoreCase))
                MPFPanel.Visible = true;
            else if (cboReportFilter.SelectedValue.Equals("CostCenter", StringComparison.CurrentCultureIgnoreCase))
                CostCenterPanel.Visible = true;
            else if (cboReportFilter.SelectedValue.Equals("Attendance", StringComparison.CurrentCultureIgnoreCase))
                AttendancePanel.Visible = true;
            else if (cboReportFilter.SelectedValue.Equals("Taxation", StringComparison.CurrentCultureIgnoreCase))
                TaxationPanel.Visible = true;
            else if (cboReportFilter.SelectedValue.Equals("Training", StringComparison.CurrentCultureIgnoreCase))
                TrainingPanel.Visible = true;
            else if (cboReportFilter.SelectedValue.Equals("PersonnelReports", StringComparison.CurrentCultureIgnoreCase))
                PersonnelReportsPanel.Visible = true;
            else if (cboReportFilter.SelectedValue.Equals("PayrollReports", StringComparison.CurrentCultureIgnoreCase))
                PayrollReportsPanel.Visible = true;
            else if (cboReportFilter.SelectedValue.Equals("TaxationReports", StringComparison.CurrentCultureIgnoreCase))
                TaxationReportsPanel.Visible = true;
        }

        if (WebUtils.productLicense(Session) != null)
        {
            if (WebUtils.productLicense(Session).IsCostCenter)
            {
                cboReportFilter.Items.FindByValue("CostCenter").Enabled = true;
                //this.CostCenterPanel.Visible = true;
            }
            else
            {
                cboReportFilter.Items.FindByValue("CostCenter").Enabled = false;
                this.CostCenterPanel.Visible = false;
            }
            if (WebUtils.productLicense(Session).IsAttendance)
            {
                cboReportFilter.Items.FindByValue("Attendance").Enabled = true;
                //this.AttendancePanel.Visible = true;
            }
            else
            {
                cboReportFilter.Items.FindByValue("Attendance").Enabled = false;
                this.AttendancePanel.Visible = false;
            }
            if (WebUtils.productLicense(Session).IsTraining)
            {
                cboReportFilter.Items.FindByValue("Training").Enabled = true;
                //this.TrainingPanel.Visible = true;
            }
            else
            {
                cboReportFilter.Items.FindByValue("Training").Enabled = false;
                this.TrainingPanel.Visible = false;
            }
        }
        else
        {
            this.CostCenterPanel.Visible = false;
            this.AttendancePanel.Visible = false;
            this.TrainingPanel.Visible = false;
            cboReportFilter.Items.FindByValue("CostCenter").Enabled = false;
            cboReportFilter.Items.FindByValue("Attendance").Enabled = false;
            cboReportFilter.Items.FindByValue("Training").Enabled = false;

        }


        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

		if(!Page.IsPostBack) 
		{
            loadObject();

            loadPermissions(SystemPermissions, "System");
            loadPermissions(SecurityPermissions, "Security");
            loadPermissions(PersonnelPermissions, "Personnel");
            loadPermissions(LeavePermissions, "Leave");
            loadPermissions(PayrollPermissions, "Payroll");
            loadPermissions(MPFPermissions, "MPF");
            loadPermissions(CostCenterPermissions, "Cost Center");
            loadPermissions(AttendancePermissions, "Attendance");
            loadPermissions(TaxationPermissions, "Taxation");
            loadPermissions(TrainingPermissions, "Training");
            //loadPermissions(ReportPermissions, "Report");
            //loadPermissions(PersonnelReportsPermissions, "Personnel Reports");
            //loadPermissions(PayrollReportsPermissions, "Payroll & MPF Reports");
            //loadPermissions(TaxationReportsPermissions, "Taxation Reports");
        }
    }
    protected bool loadObject() 
    {
        txtEMAIL_AUDIT_TRAIL_ADDRESS.Text = string.Join("\r\n", ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_EMAIL_AUDIT_TRAIL_ADDRESS).Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));


        ArrayList list= ESystemFunctionEmailAlert.db.select(dbConn, new DBFilter());
        foreach (ESystemFunctionEmailAlert o in list)
            selectedFunctionList.Add(o.FunctionID, o);

        return true;
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        EUserGroup c = new EUserGroup();


        PageErrors errors = PageErrors.getErrors(ESystemFunctionEmailAlert.db, Page.Master);
        errors.clear();



        ArrayList selectedList = new ArrayList();
        ArrayList unselectedList = new ArrayList();
        foreach (RepeaterItem item in SystemPermissions.Items)
            DoRepeaterItem(item, selectedList, unselectedList);
        foreach (RepeaterItem item in SecurityPermissions.Items)
            DoRepeaterItem(item, selectedList, unselectedList);
        foreach (RepeaterItem item in PersonnelPermissions.Items)
            DoRepeaterItem(item, selectedList, unselectedList);
        foreach (RepeaterItem item in LeavePermissions.Items)
            DoRepeaterItem(item, selectedList, unselectedList);
        foreach (RepeaterItem item in PayrollPermissions.Items)
            DoRepeaterItem(item, selectedList, unselectedList);
        foreach (RepeaterItem item in MPFPermissions.Items)
            DoRepeaterItem(item, selectedList, unselectedList);
        foreach (RepeaterItem item in CostCenterPermissions.Items)
            DoRepeaterItem(item, selectedList, unselectedList);
        foreach (RepeaterItem item in AttendancePermissions.Items)
            DoRepeaterItem(item, selectedList, unselectedList);
        foreach (RepeaterItem item in TaxationPermissions.Items)
            DoRepeaterItem(item, selectedList, unselectedList);
        foreach (RepeaterItem item in TrainingPermissions.Items)
            DoRepeaterItem(item, selectedList, unselectedList);
        //foreach (RepeaterItem item in ReportPermissions.Items)
        //    DoRepeaterItem(item, list);
        //foreach (RepeaterItem item in PersonnelReportsPermissions.Items)
        //    DoRepeaterItem(item, list);
        //foreach (RepeaterItem item in PayrollReportsPermissions.Items)
        //    DoRepeaterItem(item, list);
        //foreach (RepeaterItem item in TaxationReportsPermissions.Items)
        //    DoRepeaterItem(item, list);


        if (!errors.isEmpty())
            return;

        WebUtils.StartFunction(Session, FUNCTION_CODE);

        ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_EMAIL_AUDIT_TRAIL_ADDRESS, string.Join(";", txtEMAIL_AUDIT_TRAIL_ADDRESS.Text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)));
        foreach (ESystemFunctionEmailAlert o in selectedList)
        {
            DBFilter filter = new DBFilter();
            filter.add(new Match("FunctionID",o.FunctionID));

            ArrayList alertList = ESystemFunctionEmailAlert.db.select(dbConn, filter);
            if (alertList.Count == 0)
                ESystemFunctionEmailAlert.db.insert(dbConn, o);
            else
            {
                ESystemFunctionEmailAlert oldValue= ((ESystemFunctionEmailAlert)alertList[0]);
                if (oldValue.SystemFunctionEmailAlertDelete != o.SystemFunctionEmailAlertDelete || oldValue.SystemFunctionEmailAlertInsert != o.SystemFunctionEmailAlertInsert || oldValue.SystemFunctionEmailAlertUpdate != o.SystemFunctionEmailAlertUpdate)
                {
                    o.SystemFunctionEmailAlertID = oldValue.SystemFunctionEmailAlertID;
                    ESystemFunctionEmailAlert.db.update(dbConn, o);
                }
            }
        }

        foreach (ESystemFunctionEmailAlert o in unselectedList)
        {
            DBFilter filter = new DBFilter();
            filter.add(new Match("FunctionID", o.FunctionID));

            ArrayList alertList = ESystemFunctionEmailAlert.db.select(dbConn, filter);
            if (alertList.Count != 0)
                foreach (ESystemFunctionEmailAlert alert in alertList)
                    ESystemFunctionEmailAlert.db.delete(dbConn, alert);
        }

        WebUtils.EndFunction(dbConn);
        errors.addError(HROne.Common.WebUtility.GetLocalizedString("Updated Successful"));

    }

    public void DoRepeaterItem(RepeaterItem item, ArrayList selectedList, ArrayList unselectedList)
    {
        HtmlInputCheckBox insertSelected = (HtmlInputCheckBox)item.FindControl("SystemFunctionEmailAlertInsert");
        HtmlInputCheckBox updateSelected = (HtmlInputCheckBox)item.FindControl("SystemFunctionEmailAlertUpdate");
        HtmlInputCheckBox deleteSelected = (HtmlInputCheckBox)item.FindControl("SystemFunctionEmailAlertDelete");
        ESystemFunctionEmailAlert a = new ESystemFunctionEmailAlert();
        a.FunctionID = int.Parse(insertSelected.Attributes["id"]);
        a.SystemFunctionEmailAlertInsert = insertSelected.Checked;
        a.SystemFunctionEmailAlertUpdate = updateSelected.Checked;
        a.SystemFunctionEmailAlertDelete = deleteSelected.Checked;
        if (a.SystemFunctionEmailAlertInsert || a.SystemFunctionEmailAlertUpdate || a.SystemFunctionEmailAlertDelete)
            selectedList.Add(a);
        else
            unselectedList.Add(a);
    }
    public void loadPermissions(Repeater repeater, string category)
    {
        DBFilter filter = new DBFilter();
        //foreach (string allowFunctionCode in AllowEmailFunctionList)
        //    filter.add(new Match("FunctionCode", allowFunctionCode));
        filter.add(new Match("FunctionCategory", category));
        //filter.add(new Match("FunctionIsHidden", "<>", true));
        filter.add("FunctionCode", true);
        DataTable table = ESystemFunction.db.loadDataSet(dbConn, null, filter);
        DataView view = new DataView(table);
        repeater.DataSource = view;
        repeater.DataBind();
    }

    protected void ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        HROne.Common.WebUtility.WebControlsLocalization(this, e.Item.Controls);

        HtmlInputCheckBox insertSelected = (HtmlInputCheckBox)e.Item.FindControl("SystemFunctionEmailAlertInsert");
        HtmlInputCheckBox updateSelected = (HtmlInputCheckBox)e.Item.FindControl("SystemFunctionEmailAlertUpdate");
        HtmlInputCheckBox deleteSelected = (HtmlInputCheckBox)e.Item.FindControl("SystemFunctionEmailAlertDelete");
        int id = (int)((DataRowView)e.Item.DataItem)["FunctionID"];
        insertSelected.Attributes["id"] = id.ToString();

        Label lblFunctionCode = (Label)e.Item.FindControl("FunctionCode");
        if (lblFunctionCode != null)
            lblFunctionCode.Text = (string)((DataRowView)e.Item.DataItem)["FunctionCode"];
        Label lblDescription = (Label)e.Item.FindControl("Description");
        if (lblDescription != null)
            lblDescription.Text = HROne.Common.WebUtility.GetLocalizedStringByCode("FUNCTION_" + (string)((DataRowView)e.Item.DataItem)["FunctionCode"], (string)((DataRowView)e.Item.DataItem)["Description"]);

        if (selectedFunctionList.ContainsKey(id))
        {
            ESystemFunctionEmailAlert o = (ESystemFunctionEmailAlert)selectedFunctionList[id];
            insertSelected.Checked = o.SystemFunctionEmailAlertInsert;
            updateSelected.Checked = o.SystemFunctionEmailAlertUpdate;
            deleteSelected.Checked = o.SystemFunctionEmailAlertDelete;
        }
        //if (WebUtils.IsTrialVersion(Session))
        //{
        //    selected.Checked = true;
        //    selected.Disabled = true;
        //    wselected.Checked = true;
        //    wselected.Disabled = true;
        //}
    }
    protected void Back_Click(object sender, EventArgs e)
    {
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/SystemParameter.aspx");

    }

    protected void cboReportFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        cboReportFilter.Focus();
    }

}
