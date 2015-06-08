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

public partial class Emp_PersonalInfo : HROneWebControl
{
    public Binding binding;
    public Binding uniformBinding;
    public DBManager db = EEmpPersonalInfo.db;
    public EEmpPersonalInfo obj;
    public int CurID = -1;
    public Hashtable CurElements = new Hashtable();
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (WebUtils.productLicense(Session).IsAttendance)
            AttendanceRow.Visible = true;
        else
            AttendanceRow.Visible = false;

        //if (WebUtils.productLicense(Session).IsESS)
        //    ESSTable.Visible = true;
        //else
        //    ESSTable.Visible = false;

        PreRender += new EventHandler(Emp_PersonalInfo_PreRender);
        binding = new Binding(dbConn, db);
        binding.add(EmpResAddr);
        binding.add(new LabelVLBinder(db,EmpResAddrAreaCode, Values.VLArea));
        binding.add(EmpCorAddr);
        binding.add(EmpNoticePeriod);
        binding.add(new LabelVLBinder(db, EmpNoticeUnit, Values.VLEmpUnit));
        binding.add(EmpProbaPeriod);
        binding.add(new LabelVLBinder(db, EmpProbaUnit, Values.VLEmpUnit));
        binding.add(EmpProbaLastDate);
        binding.add(Remark);

        binding.add(new LabelVLBinder(db, EmpMaritalStatus, Values.VLMaritalStatus));
        binding.add(EmpPassportExpiryDate);
        // Start 0000092, KuangWei, 2014-09-10
        binding.add(EmpPlaceOfBirth);
        binding.add(EmpNationality);
        binding.add(EmpPassportIssuedCountry);
        //binding.add(new LabelVLBinder(db, EmpPlaceOfBirthID, EPlaceOfBirth.VLPlaceOfBirth));
        //binding.add(new LabelVLBinder(db, EmpNationalityID, ENationality.VLNationality));
        //binding.add(new LabelVLBinder(db, EmpPassportIssuedCountryID, EIssueCountry.VLCountry));
        // End 0000092, KuangWei, 2014-09-10
        binding.add(EmpPassportNo);
        binding.add(EmpHomePhoneNo);
        binding.add(EmpOfficePhoneNo);
        binding.add(EmpMobileNo);
        binding.add(EmpEmail);
        binding.add(EmpInternalEmail);
        binding.add(EmpTimeCardNo);
        binding.add(EmpNextSalaryIncrementDate);
        binding.add(new LabelVLBinder(db, EmpIsOverrideMinimumWage, Values.VLTrueFalseYesNo));
        binding.add(EmpOverrideMinimumHourlyRate);
        // Start 0000067, Miranda, 2014-08-07
        binding.add(EmpOriginalHireDate);
        // End 0000067, Miranda, 2014-08-07
        binding.init(Request, Session);


        if (!int.TryParse(DecryptedRequest["EmpID"], out CurID))
            CurID = -1;

        if (!IsPostBack)
        {
        }

        PayscaleRow1.Visible = !(ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_PAYSCALE_POINT_SYSTEM) == "N");
    }

    protected void loadEmpExtraField()
    {
        DBFilter filter = new DBFilter();
        DataTable table = filter.loadData(dbConn, "Select distinct EmpExtraFieldGroupName from EmpExtraField order by EmpExtraFieldGroupName");

        if (table.Rows.Count > 0)
        {
            EmpExtraFieldPanel.Visible = true;
            EmpExtraFieldGroupRepeater.DataSource = table;
            EmpExtraFieldGroupRepeater.DataBind();
        }
        else
            EmpExtraFieldPanel.Visible = false;


    }

    void Emp_PersonalInfo_PreRender(object sender, EventArgs e)
    {
        loadObject();
        loadEmpExtraField();

        EmpNoticePeriod.Visible = obj.EmpNoticePeriod != 0;
        EmpNoticeUnit.Visible = obj.EmpNoticePeriod != 0;

        EmpProbaPeriod.Visible = obj.EmpProbaPeriod != 0;
        EmpProbaUnit.Visible = obj.EmpProbaPeriod != 0;
    }

    protected void EmpExtraField_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        EEmpExtraField empExtraField = (EEmpExtraField)e.Item.DataItem;
        Label c = (Label)e.Item.FindControl("EmpExtraFieldValue");
        EEmpExtraFieldValue h = (EEmpExtraFieldValue)CurElements[empExtraField.EmpExtraFieldID];
        if (h != null)
        {
            c.Text = h.EmpExtraFieldValue.Replace("\r", "<br/>").Replace("\n", "");
        }
        else
        {
            c.Text = "";
        }

        HROne.Common.WebUtility.WebControlsLocalization(Session, e.Item.Controls);
    }

    protected void EmpExtraFieldGroupRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        string strEmpExtraFieldGroupName = null;

        Label EmpExtraFieldGroupName = (Label)e.Item.FindControl("EmpExtraFieldGroupName");
        DataRow row = ((DataRowView)e.Item.DataItem).Row;
        if (!row.IsNull("EmpExtraFieldGroupName"))
            if (!string.IsNullOrEmpty(row["EmpExtraFieldGroupName"].ToString().Trim()))
                strEmpExtraFieldGroupName = row["EmpExtraFieldGroupName"].ToString();
        if (strEmpExtraFieldGroupName!=null)
            EmpExtraFieldGroupName.Text = strEmpExtraFieldGroupName.Trim();

        DBFilter filter;
        ArrayList list;

        DBFilter empExtraFieldFilter = new DBFilter();
        filter = new DBFilter();
        filter.add(new Match("EmpID", CurID));
        if (strEmpExtraFieldGroupName != null)
        {
            empExtraFieldFilter.add(new Match("EmpExtraFieldGroupName", strEmpExtraFieldGroupName));
            filter.add(new IN("EmpExtraFieldID", "Select EmpExtraFieldID from EmpExtraField", empExtraFieldFilter));
        }
        else
        {
            empExtraFieldFilter.add(new NullTerm("EmpExtraFieldGroupName"));
            filter.add(new IN("EmpExtraFieldID", "Select EmpExtraFieldID from EmpExtraField", empExtraFieldFilter));
        }
        list = EEmpExtraFieldValue.db.select(dbConn, filter);

        foreach (EEmpExtraFieldValue element in list)
        {
            CurElements[element.EmpExtraFieldID] = element;
        }

        list = EEmpExtraField.db.select(dbConn, empExtraFieldFilter);
        //if (list.Count <= 0)
        //    EmpExtraFieldPanel.Visible = false;
        //else
        //    EmpExtraFieldPanel.Visible = true;

        Repeater EmpExtraField = (Repeater)e.Item.FindControl("EmpExtraField");
        EmpExtraField.DataSource = list;
        EmpExtraField.DataBind();

        HROne.Common.WebUtility.WebControlsLocalization(Session, e.Item.Controls);
    }

    protected bool loadObject()
    {
        obj = new EEmpPersonalInfo();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);

        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpID", obj.EmpID));
        filter.add(WebUtils.AddRankFilter(Session, "EmpID", true));
        ArrayList empInfoList = EEmpPersonalInfo.db.select(dbConn, filter);
        if (empInfoList.Count == 0)
            return false;
        obj = (EEmpPersonalInfo)empInfoList[0];

        DBFilter empUniformFilter = new DBFilter();
        empUniformFilter.add(new Match("EmpID", obj.EmpID));
        ArrayList empUniformList = EEmpUniform.db.select(dbConn, empUniformFilter);
        if (empUniformList.Count > 0)
        {
            EEmpUniform empUniform = (EEmpUniform)empUniformList[0];
            EmpUniformB.Text = empUniform.EmpUniformB;
            EmpUniformW.Text = empUniform.EmpUniformW;
            EmpUniformH.Text = empUniform.EmpUniformH;
        }
        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);
        return true;
    }

}
