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

public partial class UserGroup_View : HROneWebPage
{
    private const string FUNCTION_CODE = "SEC002";

    public Binding binding;
    public DBManager db = EUserGroup.db;
    public EUserGroup obj;
    public int CurID = -1;
    public Hashtable CurPermissions = new Hashtable();
    public Hashtable CurWritePermissions = new Hashtable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
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
            PersonnelReportsPanel.Visible = true;
            PayrollReportsPanel.Visible = true;
            TaxationReportsPanel.Visible = true;
            eChannelPanel.Visible = true;
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
            PersonnelReportsPanel.Visible = false;
            PayrollReportsPanel.Visible = false;
            TaxationReportsPanel.Visible = false;
            eChannelPanel.Visible = false;

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
            else if (cboReportFilter.SelectedValue.Equals("eChannel", StringComparison.CurrentCultureIgnoreCase))
                eChannelPanel.Visible = true;
        }

        HROne.ProductLicense license = WebUtils.productLicense(Session);
        if (license != null)
        {
            if (license.IsLeaveManagement || license.IsPayroll || license.IsTaxation)
            {
                cboReportFilter.Items.FindByValue("System").Enabled = true;
            }
            else
            {
                cboReportFilter.Items.FindByValue("System").Enabled = false;
                this.SystemPanel.Visible = false;
            }
            if (license.IsLeaveManagement)
            {
                cboReportFilter.Items.FindByValue("Personnel").Enabled = true;
                cboReportFilter.Items.FindByValue("Leave").Enabled = true;
                cboReportFilter.Items.FindByValue("PersonnelReports").Enabled = true;
            }
            else
            {
                cboReportFilter.Items.FindByValue("Personnel").Enabled = false;
                cboReportFilter.Items.FindByValue("Leave").Enabled = false;
                cboReportFilter.Items.FindByValue("PersonnelReports").Enabled = false;
                this.PersonnelPanel.Visible = false;
                this.LeavePanel.Visible = false;
                this.PersonnelReportsPanel.Visible = false;
            }
            if (license.IsPayroll)
            {
                cboReportFilter.Items.FindByValue("Payroll").Enabled = true;
                cboReportFilter.Items.FindByValue("MPF").Enabled = true;
                cboReportFilter.Items.FindByValue("PayrollReports").Enabled = true;
            }
            else
            {
                cboReportFilter.Items.FindByValue("Payroll").Enabled = false;
                cboReportFilter.Items.FindByValue("MPF").Enabled = false;
                cboReportFilter.Items.FindByValue("PayrollReports").Enabled = false;
                this.PayrollPanel.Visible = false;
                this.MPFPanel.Visible = false;
                this.PayrollReportsPanel.Visible = false;
            }
            if (license.IsTaxation)
            {
                cboReportFilter.Items.FindByValue("Taxation").Enabled = true;
                cboReportFilter.Items.FindByValue("TaxationReports").Enabled = true;
            }
            else
            {
                cboReportFilter.Items.FindByValue("Taxation").Enabled = false;
                cboReportFilter.Items.FindByValue("TaxationReports").Enabled = false;
                this.TaxationPanel.Visible = false;
                this.TaxationReportsPanel.Visible = false;
            }
            if (license.IsCostCenter)
            {
                cboReportFilter.Items.FindByValue("CostCenter").Enabled = true;
                //this.CostCenterPanel.Visible = true;
            }
            else
            {
                cboReportFilter.Items.FindByValue("CostCenter").Enabled = false;
                this.CostCenterPanel.Visible = false;
            }
            if (license.IsAttendance)
            {
                cboReportFilter.Items.FindByValue("Attendance").Enabled = true;
                //this.AttendancePanel.Visible = true;
            }
            else
            {
                cboReportFilter.Items.FindByValue("Attendance").Enabled = false;
                this.AttendancePanel.Visible = false;
            }
            if (license.IsTraining)
            {
                cboReportFilter.Items.FindByValue("Training").Enabled = true;
                //this.TrainingPanel.Visible = true;
            }
            else
            {
                cboReportFilter.Items.FindByValue("Training").Enabled = false;
                this.TrainingPanel.Visible = false;
            }
            if (Session["CompanyDBID"] != null)
            {
                cboReportFilter.Items.FindByValue("eChannel").Enabled = true;
                //this.TrainingPanel.Visible = true;
            }
            else
            {
                cboReportFilter.Items.FindByValue("eChannel").Enabled = false;
                this.eChannelPanel.Visible = false;
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
        binding = new Binding(dbConn, db);
        binding.add(UserGroupID);
        binding.add(UserGroupName);
        binding.add(UserGroupDesc);
        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["UserGroupID"], out CurID))
            CurID = -1;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

		if(!Page.IsPostBack) 
		{
            if (CurID > 0)
                loadObject();
            else
                toolBar.DeleteButton_Visible = false;

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
            loadPermissions(PersonnelReportsPermissions, "Personnel Reports");
            loadPermissions(PayrollReportsPermissions, "Payroll & MPF Reports");
            loadPermissions(TaxationReportsPermissions, "Taxation Reports");
            loadPermissions(eChannelPermissions, "e-channel");
        }
    }
    protected bool loadObject() 
    {
	    obj=new EUserGroup();
	    bool isNew=WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
	    if(!db.select(dbConn, obj))
		    return false;

	    Hashtable values=new Hashtable();
	    db.populate(obj,values);
	    binding.toControl(values);


        DBFilter filter = new DBFilter();
        filter.add(new Match("UserGroupID", this.CurID));
        ArrayList list;
        list = EUserGroupFunction.db.select(dbConn, filter);
        foreach (EUserGroupFunction o in list)
        {
            if(o.FunctionAllowRead)
                CurPermissions.Add(o.FunctionID, o);
            if(o.FunctionAllowWrite)
                CurWritePermissions.Add(o.FunctionID, o);
        }

        return true;
    }


    public void DoRepeaterItem(RepeaterItem item, ArrayList list)
    {
        HtmlInputCheckBox selected = (HtmlInputCheckBox)item.FindControl("Permission");
        HtmlInputCheckBox wselected = (HtmlInputCheckBox)item.FindControl("WritePermission");
        EUserGroupFunction a = new EUserGroupFunction();
        a.FunctionID = int.Parse(selected.Attributes["id"]);
        if (selected.Checked)
            a.FunctionAllowRead = true;
        if (wselected.Checked)
            a.FunctionAllowWrite = true;
        if (a.FunctionAllowRead || a.FunctionAllowWrite)
            list.Add(a);
    }
    public void loadPermissions(Repeater repeater, string category)
    {
        DBFilter filter = new DBFilter();
        filter.add(new Match("FunctionCategory", category));
        filter.add(new Match("FunctionIsHidden", false));
        filter.add("FunctionCode", true);
        DataTable table = ESystemFunction.db.loadDataSet(dbConn, null, filter);
        DataView view = new DataView(table);
        repeater.DataSource = view;
        repeater.DataBind();
    }

    protected void ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        HROne.Common.WebUtility.WebControlsLocalization(this, e.Item.Controls);

        HtmlInputCheckBox selected = (HtmlInputCheckBox)e.Item.FindControl("Permission");
        HtmlInputCheckBox wselected = (HtmlInputCheckBox)e.Item.FindControl("WritePermission");
        int id = (int)((DataRowView)e.Item.DataItem)["FunctionID"];
        selected.Attributes["id"] = id.ToString();
        Label lblFunctionCode = (Label)e.Item.FindControl("FunctionCode");
        if (lblFunctionCode != null)
            lblFunctionCode.Text = (string)((DataRowView)e.Item.DataItem)["FunctionCode"];
        Label lblDescription = (Label)e.Item.FindControl("Description");
        if (lblDescription != null)
            lblDescription.Text = HROne.Common.WebUtility.GetLocalizedStringByCode("FUNCTION_" + (string)((DataRowView)e.Item.DataItem)["FunctionCode"], (string)((DataRowView)e.Item.DataItem)["Description"]);


        if (CurPermissions.ContainsKey(id))
            selected.Checked = true;
        if (CurWritePermissions.ContainsKey(id))
            wselected.Checked = true;
    }
    protected void Delete_Click(object sender, EventArgs e)
    {

        EUserGroup c = new EUserGroup();
        c.UserGroupID = CurID;
        if (EUserGroup.db.select(dbConn, c))
        {
            DBFilter userGroupFilter = new DBFilter();
            userGroupFilter.add(new Match("UserGroupID", CurID));
            EUserGroupAccess.db.delete(dbConn, userGroupFilter);
            EUserGroupFunction.db.delete(dbConn, userGroupFilter);

            WebUtils.StartFunction(Session, FUNCTION_CODE);
            db.delete(dbConn, c);
            WebUtils.EndFunction(dbConn);
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "UserGroup_List.aspx");
    }
    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "UserGroup_Edit.aspx?UserGroupID=" + CurID);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "UserGroup_List.aspx");
    }
    protected void cboReportFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        cboReportFilter.Focus();
    }
}
