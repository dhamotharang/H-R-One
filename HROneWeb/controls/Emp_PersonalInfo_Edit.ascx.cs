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
using HROne.Translation;
using HROne.Lib;

public partial class Emp_PersonalInfo_Edit : HROneWebControl
{

    private const string FUNCTION_CODE = "PER001";
    // Start 0000071, Miranda, 2014-08-04
    private const string SUB_FUNCTION_CODE = "PER001-1";
    // End 0000071, Miranda, 2014-08-04

    public Binding binding;
    private DBManager db = EEmpPersonalInfo.db;

    private Hashtable CurElements = new Hashtable();

    protected bool m_IsCreateNewRoleEmployee = false;
	// Start 0000044, Miranda, 2014-05-09
    private bool isAutoGenerateEmpNo = false;
    private string empNoFormat = null;
	// End 0000044, Miranda, 2014-05-09

    public int EmpID
    {
        get { int CurID = -1; if (int.TryParse(ID.Value, out CurID)) return CurID; else return -1; }
        set { ID.Value = value.ToString(); }
    }

    public int PrevEmpID
    {
        get { int PrevCurID = -1; if (int.TryParse(OldID.Value, out PrevCurID)) return PrevCurID; else return -1; }
        set { OldID.Value = value.ToString(); }
    }

    public bool IsCreateNewRoleEmployee
    {
        get { return m_IsCreateNewRoleEmployee; }
        set { m_IsCreateNewRoleEmployee = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        bool isAutoGenerateHKIDCheckDigit = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_HKID_CHECKDIGIT_AUTO_GENERATE).Equals("Y", StringComparison.CurrentCultureIgnoreCase) ? true : false;
        if (WebUtils.productLicense(Session).IsAttendance)
            AttendanceRow.Visible = true;
        else
            AttendanceRow.Visible = false;

        if (WebUtils.productLicense(Session).IsESS)
            ESSTable.Visible = true;
        else
            ESSTable.Visible = false;

        binding = new Binding(dbConn, db);
        binding.add(new HiddenBinder(db,ID,"EmpID"));
        binding.add(EmpNo);
        binding.add(EmpChiFullName);
        binding.add(EmpEngSurname);
        binding.add(EmpEngOtherName);
        binding.add(EmpAlias);
        binding.add(new HKIDBinder(db, EmpHKID, EmpHKID_Digit, isAutoGenerateHKIDCheckDigit));
        binding.add(new DropDownVLBinder(db, EmpGender, Values.VLGender));
        binding.add(new DropDownVLBinder(db, EmpMaritalStatus, Values.VLMaritalStatus));
        binding.add(new TextBoxBinder(db, EmpDateOfBirth.TextBox, EmpDateOfBirth.ID));
        // Start 0000092, KuangWei, 2014-09-10
        //binding.add(EmpPlaceOfBirth);
        //binding.add(EmpNationality);
        //binding.add(EmpPassportIssuedCountry);
        binding.add(new DropDownVLBinder(db, EmpPlaceOfBirthID, EPlaceOfBirth.VLPlaceOfBirth));
        binding.add(new DropDownVLBinder(db, EmpPassportIssuedCountryID, EIssueCountry.VLCountry));
        binding.add(new DropDownVLBinder(db, EmpNationalityID, ENationality.VLNationality)); 
        // End 0000092, KuangWei, 2014-09-10
        binding.add(EmpPassportNo);
        binding.add(new TextBoxBinder(db, EmpPassportExpiryDate.TextBox, EmpPassportExpiryDate.ID));
        binding.add(EmpHomePhoneNo);
        binding.add(EmpOfficePhoneNo);
        binding.add(EmpMobileNo);
        binding.add(EmpEmail);

        binding.add(EmpResAddr);
        binding.add(new DropDownVLBinder(db, EmpResAddrAreaCode, Values.VLArea));
        binding.add(EmpCorAddr);
        binding.add(new TextBoxBinder(db, EmpDateOfJoin.TextBox, EmpDateOfJoin.ID));
        binding.add(new TextBoxBinder(db, EmpServiceDate.TextBox, EmpServiceDate.ID));
        binding.add(EmpNoticePeriod);
        binding.add(new DropDownVLBinder(db, EmpNoticeUnit, Values.VLEmpUnit));
        binding.add(EmpProbaPeriod);
        binding.add(new DropDownVLBinder(db, EmpProbaUnit, Values.VLEmpUnit));
        binding.add(new TextBoxBinder(db, EmpProbaLastDate.TextBox, EmpProbaLastDate.ID));
        binding.add(new TextBoxBinder(db, EmpNextSalaryIncrementDate.TextBox, EmpNextSalaryIncrementDate.ID));
        binding.add(Remark);

        binding.add(EmpTimeCardNo);
        binding.add(EmpInternalEmail);

        binding.add(new CheckBoxBinder(db, EmpIsOverrideMinimumWage));
        binding.add(EmpOverrideMinimumHourlyRate);

        binding.add(new CheckBoxBinder(db, EmpIsCombinePaySlip));
        binding.add(new CheckBoxBinder(db, EmpIsCombineMPF));
        binding.add(new CheckBoxBinder(db, EmpIsCombineTax));
        // Start 0000067, Miranda, 2014-08-07
        binding.add(new TextBoxBinder(db, EmpOriginalHireDate.TextBox, EmpOriginalHireDate.ID));
        // End 0000067, Miranda, 2014-08-07

        binding.init(Request, Session);

        HROne.ProductLicense productLicense = WebUtils.productLicense(Session);

        if (productLicense.ProductType != HROne.ProductLicense.ProductLicenseType.HROne)
        {
            EmpHKID.MaxLength = 8;
        }

        if (ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_PAYSCALE_POINT_SYSTEM) != "Y")
        {
            PayscaleRow.Visible = false;
        }

        HROne.Common.WebUtility.WebControlsLocalization(Session, this.Controls);

        //if (!int.TryParse(DecryptedRequest["EmpID"], out CurID))
        //    CurID = -1;

        //if (!int.TryParse(DecryptedRequest["PrevEmpID"], out PrevCurID))
        //    PrevCurID = -1;
		// Start 0000044, Miranda, 2014-05-09
        isAutoGenerateEmpNo = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_EMP_NO_AUTO_GENERATE).Equals("Y", StringComparison.CurrentCultureIgnoreCase) ? true : false;
        if (isAutoGenerateEmpNo)
        {
            empNoFormat = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_EMP_NO_FORMAT);
            if (empNoFormat != null && EmpID < 0 && EmpNo.Text.Equals(""))
            {
                // auto get next empNo base on format setting.
                EmpNo.Text = getNextEmpNo(empNoFormat);
            }
        }
		// End 0000044, Miranda, 2014-05-09
    }

    // Start 0000092, KuangWei, 2014-09-09
    //protected void initPlaceOfBirth()
    //{
    //    DBFilter m_filter = new DBFilter();
    //    //DBFilter m_userFilter = new DBFilter();

    //    //m_userFilter.add(new Match("UserID", WebUtils.GetCurUser(Session).UserID));

    //    //m_filter.add(new IN("CompanyID", "SELECT CompanyID FROM UserCompany ", m_userFilter));
    //    //m_filter.add("CompanyCode", true);
    //    foreach (EPlaceOfBirth o in EPlaceOfBirth.db.select(dbConn, m_filter))
    //    {
    //        EmpPlaceOfBirth.Items.Add(new ListItem(o.PlaceOfBirthDesc, o.PlaceOfBirthID.ToString()));
    //    }
    //}
    // End 0000092, KuangWei, 2014-09-09

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (EmpID > 0)
            {

                loadObject(EmpID);
            }
            else
            {

                if (PrevEmpID > 0)
                {

                    loadObject(PrevEmpID);
                    ID.Value = string.Empty;
                    EmpDateOfJoin.Value = string.Empty;
                    EmpProbaLastDate.Value = string.Empty;
                }
            }
            loadEmpExtraField();

        }

        // Start 0000071, Miranda, 2014-08-04
        if (EmpID < 0) // Is New Mode
        {
            EmpNo.Enabled = !m_IsCreateNewRoleEmployee;
        }
        else // Is Edit Mode
        {
            EmpNo.Enabled = (!m_IsCreateNewRoleEmployee && WebUtils.CheckPermission(Session, SUB_FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite));
        }
        // End 0000071, Miranda, 2014-08-04
        if (m_IsCreateNewRoleEmployee)
        {
            RoleEmpInfoPanel.Visible = true;
            NewRoleEmpNoRoleRow.Visible = true;
        }
    }

	// Start 0000044, Miranda, 2014-05-09
    //protected string getNextEmpNo(string empNoFormat)
    //{
    //    if (empNoFormat != null)
    //    {
    //        string empNoFlt = "";
    //        bool isStartWithNumber = false;
    //        int firstNumIndex = empNoFormat.IndexOf('9');
    //        if (firstNumIndex == 0)// start with number
    //        {
    //            isStartWithNumber = true;
    //            empNoFlt = empNoFormat.Substring(empNoFormat.LastIndexOf('9') + 1);
    //        }
    //        else
    //        {
    //            empNoFlt = empNoFormat.Substring(0, firstNumIndex);
    //        }

    //        SearchBinding empNoBinding = new SearchBinding(dbConn, db);
    //        empNoBinding.init(DecryptedRequest, null);
    //        DBFilter filter = empNoBinding.createFilter();
    //        string select = "c.EmpNo ";
    //        string from = "from [" + EEmpPersonalInfo.db.dbclass.tableName + "] c ";
    //        DataTable table = filter.loadData(dbConn, null, select, from);

    //        DBAESEncryptStringFieldAttribute.decode(table, "EmpNo");
    //        DataView view = new DataView(table);
    //        view.RowFilter = " EmpNo like '" + (isStartWithNumber ? ("%" + empNoFlt) : (empNoFlt + "%")) + "' ";
    //        view.Sort = "EmpNo DESC";
    //        table = view.ToTable();

    //        String zeroStr = "";
    //        int nextEmpNum = 1;
    //        if (table.Rows.Count > 0)
    //        {
    //            int rowCount = table.Rows.Count;
    //            string maxEmpNo = null;
    //            for (int i = 0; i < rowCount; i++)
    //            {
    //                maxEmpNo = (string)table.Rows[i]["EmpNo"];
    //                if (isAlignToDefinedFormat(maxEmpNo, empNoFormat))
    //                {
    //                    break;
    //                }
    //            }
    //            if (maxEmpNo != null)
    //            {
    //                string numStr = "";
    //                if (isStartWithNumber)
    //                {
    //                    numStr = maxEmpNo.Substring(0, maxEmpNo.Length - empNoFlt.Length);
    //                }
    //                else
    //                {
    //                    numStr = maxEmpNo.Substring(empNoFlt.Length);
    //                }
    //                nextEmpNum = 1 + Convert.ToInt32(numStr);
    //            }
    //            else
    //            {
    //                nextEmpNum = rowCount + 1;
    //            }
    //        }
    //        int j = empNoFormat.Length - (Convert.ToString(nextEmpNum).Length + empNoFlt.Length);
    //        for (int i = 0; i < j; i++)
    //        {
    //            zeroStr += "0";
    //        }
    //        return isStartWithNumber ? (zeroStr + Convert.ToString(nextEmpNum) + empNoFlt) : (empNoFlt + zeroStr + Convert.ToString(nextEmpNum));
    //    }
    //    else
    //    {
    //        return "";
    //    }
    //}

    protected string getNextEmpNo(string empNoFormat)
    {
        if (empNoFormat != null)
        {
            SearchBinding empNoBinding = new SearchBinding(dbConn, db);
            empNoBinding.init(DecryptedRequest, null);
            DBFilter filter = empNoBinding.createFilter();
            string select = "c.EmpNo ";
            string from = "from [" + EEmpPersonalInfo.db.dbclass.tableName + "] c ";
            DataTable table = filter.loadData(dbConn, null, select, from);

            DBAESEncryptStringFieldAttribute.decode(table, "EmpNo");
            DataView view = new DataView(table);
            view.RowFilter = " EmpNo <= '" + empNoFormat + "' ";
            view.Sort = "EmpNo DESC";
            table = view.ToTable();

            string m_prefix = empNoFormat.Replace("9", "");
            string m_currentEmpNo;
            string m_currentNo;
            string m_currentPrefix = "";
            int m_currentNoValue = 0; // numeric value of m_currentNo

            string m_nextEmpNo = "";
            int m_newNoValue = 0;

            if (table.Rows.Count > 0)
                m_currentEmpNo = table.Rows[0][0].ToString();
            else
                m_currentEmpNo = empNoFormat.Replace("9", "0");

            string m_numberFormat = empNoFormat.Substring(m_prefix.Length).Replace("9", "0");
            m_currentPrefix = m_currentEmpNo.Substring(0, m_prefix.Length);
            m_currentNo = m_currentEmpNo.Substring(m_prefix.Length);
            int.TryParse(m_currentNo, out m_currentNoValue);
           
            if (isAlignToDefinedFormat(m_currentEmpNo, empNoFormat))
            {
                m_newNoValue = m_currentNoValue + 1;
            }
            else
                m_newNoValue = 1;

            m_nextEmpNo = m_prefix + (m_newNoValue).ToString(m_numberFormat);
            return m_nextEmpNo;
        }
        else
        {
            return "";
        }
    }

    protected bool isAlignToDefinedFormat(string empNo, string empNoFormat)
    {
        int m_length = empNoFormat.Length;
        string m_prefix = empNoFormat.Replace("9", "");
        string m_currentNo = "";
        string m_currentEmpNo = empNo;
        int m_currentNoValue = 0; // numeric value of m_currentNo
        string m_currentPrefix = "";

        m_currentPrefix = m_currentEmpNo.Substring(0, m_prefix.Length);
        m_currentNo = m_currentEmpNo.Substring(m_prefix.Length);
        int.TryParse(m_currentNo, out m_currentNoValue);

        string m_numberFormat = empNoFormat.Substring(m_prefix.Length).Replace("9", "0");

        if (m_currentPrefix == m_prefix && m_currentNo == m_currentNoValue.ToString(m_numberFormat)) // current record meet system parameter format 
        {
            return true; 
        }
        else
        {
            return false;
        }
    }
	// End 0000044, Miranda, 2014-05-09

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

        //DBFilter filter;
        //ArrayList list;

        //filter = new DBFilter();
        //filter.add(new Match("EmpID", EmpID));
        //list = EEmpExtraFieldValue.db.select(dbConn, filter);
        //foreach (EEmpExtraFieldValue element in list)
        //{
        //    CurElements[element.EmpExtraFieldID] = element;
        //}

        //filter = new DBFilter();
        //list = EEmpExtraField.db.select(dbConn, filter);
        //if (list.Count <= 0)
        //    EmpExtraFieldPanel.Visible = false;
        //else
        //    EmpExtraFieldPanel.Visible = true;
        //EmpExtraField.DataSource = list;
        //EmpExtraField.DataBind();


    }

    protected void EmpExtraField_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType.Equals(ListItemType.Item) || e.Item.ItemType.Equals(ListItemType.AlternatingItem))
        {
            TextBox textBox = (TextBox)e.Item.FindControl("EmpExtraFieldValue");
            WebDatePicker datePicker = (WebDatePicker)e.Item.FindControl("EmpExtraFieldValueDateControl");

            EEmpExtraField empExtraField = (EEmpExtraField)e.Item.DataItem;


            EEmpExtraFieldValue h = (EEmpExtraFieldValue)CurElements[empExtraField.EmpExtraFieldID];
            string value = string.Empty;
            if (h != null)
                value = h.EmpExtraFieldValue;
            if (string.IsNullOrEmpty(empExtraField.EmpExtraFieldControlType))
                empExtraField.EmpExtraFieldControlType = EEmpExtraField.FIELD_CONTROL_TYPE_TEXTBOX;

            if (empExtraField.EmpExtraFieldControlType.Equals(EEmpExtraField.FIELD_CONTROL_TYPE_TEXTAREA, StringComparison.CurrentCultureIgnoreCase))
            {
                textBox.TextMode = TextBoxMode.MultiLine;
                textBox.Rows = 5;
                textBox.Text = value;
                textBox.Visible = true;
                datePicker.Visible = false;
            }
            else if (empExtraField.EmpExtraFieldControlType.Equals(EEmpExtraField.FIELD_CONTROL_TYPE_DATE, StringComparison.CurrentCultureIgnoreCase))
            {
                datePicker.TextBox.Text = value;
                datePicker.Visible = true;
                textBox.Visible = false;
            }
            else
            {
                textBox.Text = value;
                textBox.Visible = true;
                datePicker.Visible = false;
            }
            textBox.Attributes.Add("EmpExtraFieldID", empExtraField.EmpExtraFieldID.ToString());

        }
    }

    protected void EmpExtraFieldGroupRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        string strEmpExtraFieldGroupName = null;

        Label EmpExtraFieldGroupName = (Label)e.Item.FindControl("EmpExtraFieldGroupName");
        DataRow row = ((DataRowView)e.Item.DataItem).Row;
        if (!row.IsNull("EmpExtraFieldGroupName"))
            if (!string.IsNullOrEmpty(row["EmpExtraFieldGroupName"].ToString().Trim()))
                strEmpExtraFieldGroupName = row["EmpExtraFieldGroupName"].ToString();
        if (strEmpExtraFieldGroupName != null)
            EmpExtraFieldGroupName.Text = strEmpExtraFieldGroupName.Trim();

        DBFilter filter;
        ArrayList list;

        DBFilter empExtraFieldFilter = new DBFilter();
        filter = new DBFilter();
        if (PrevEmpID > 0)
            filter.add(new Match("EmpID", PrevEmpID));
        else
            filter.add(new Match("EmpID", EmpID));
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
    }

    protected bool loadObject(int EmpID)
    {

        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpID", EmpID));
        filter.add(WebUtils.AddRankFilter(Session, "EmpID", true));
        ArrayList empInfoList = EEmpPersonalInfo.db.select(dbConn, filter);
        if (empInfoList.Count == 0)
            if (EmpID <= 0)
                return false;
            else
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");
        EEmpPersonalInfo obj = (EEmpPersonalInfo)empInfoList[0];

        //if (obj.MasterEmpID > 0)
        //{
        //    NewRoleEmpNo.Text = obj.EmpNo;
        //    EEmpPersonalInfo masterEmpInfo = new EEmpPersonalInfo();
        //    obj.EmpID = obj.MasterEmpID;
        //    if (!EEmpPersonalInfo.db.select(obj))
        //}

        //DBFilter empUniformFilter = new DBFilter();
        //empUniformFilter.add(new Match("EmpID", obj.EmpID));
        //ArrayList empUniformList = EEmpUniform.db.select(dbConn, empUniformFilter);
        //if (empUniformList.Count > 0)
        //{
        //    EEmpUniform empUniform = (EEmpUniform)empUniformList[0];
        //    EmpUniformB.Text = empUniform.EmpUniformB;
        //    EmpUniformW.Text = empUniform.EmpUniformW;
        //    EmpUniformH.Text = empUniform.EmpUniformH;
        //}

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);
        //EmpFullName.Text = obj.EmpEngFullName;

        NewRoleEmpNoRoleRow.Visible = false;
        if (obj.MasterEmpID > 0)
            RoleEmpInfoPanel.Visible = true;
        else
            RoleEmpInfoPanel.Visible = false;

        return true;
    }

    public bool Save()
    {
        if (!FullResAddrPanel.Visible)
        {
            EmpResAddr.Text = EmpResAddr1.Text + "\r\n" + EmpResAddr2.Text + "\r\n" + EmpResAddr3.Text;

        }


        EEmpPersonalInfo c = new EEmpPersonalInfo();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return false;


        db.parse(values, c);

        if (this.IsResetESSPassword.Checked)
            c.EmpPW = string.Empty;

        if (PrevEmpID > 0)
            if (IsCreateNewRoleEmployee)
            {
                c.EmpNo = NewRoleEmpNo.Text.Trim();
                c.MasterEmpID = PrevEmpID;
                if (string.IsNullOrEmpty(c.EmpNo))
                {
                    errors.addError(lblNewRoleEmpNo.Text, "validate.required.prompt");
                    return false;
                }
            }
        c.EmpNo = c.EmpNo.ToUpper();
		// Start 0000044, Miranda, 2014-05-09
        string oldEmpNo = "";
        if (!AppUtils.checkDuplicate(dbConn, db, c, errors, "EmpNo"))
        {
            if (isAutoGenerateEmpNo && empNoFormat != null && EmpID < 0)
            {
                // Check if EmpNo format is same as EmpNoFormat setting of System Parameter
                bool alignToFormat = isAlignToDefinedFormat(c.EmpNo, empNoFormat);
                if (alignToFormat)
                {
                    errors.clear();
                    oldEmpNo = c.EmpNo;
                    c.EmpNo = getNextEmpNo(empNoFormat).ToUpper();
                    EmpNo.Text = c.EmpNo;
                    if (!AppUtils.checkDuplicate(dbConn, db, c, errors, "EmpNo"))
                    {
                        return false;
                    }
                    else
                    {
                        errors.addError(string.Format(HROne.Translation.PageErrorMessage.ALERT_NEW_STAFF_NO_BEEN_USED, new string[] { oldEmpNo, c.EmpNo }));
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
		// End 0000044, Miranda, 2014-05-09
        // Start 0000066, Miranda, 2014-08-08
        if (SuppressHKIDWarning.Text == "false" && !AppUtils.checkDuplicate(dbConn, db, c, errors, "EmpHKID"))
        {
            SuppressHKIDWarning.Text = "true";
            
            errors.clear();
            errors.addError("HKID is already input in system, click SAVE button again to bypass this warning and save to database.");
        }
        // End 0000066, Miranda, 2014-08-08

        if (c.EmpDateOfBirth > AppUtils.ServerDateTime().Date)
        {
            errors.addError("EmpDateOfBirth", HROne.Translation.PageErrorMessage.ERROR_INVALID_DOB);
        }

        if (c.EmpEngSurname.Contains(" "))
        {
            errors.addError("EmpEngSurname", HROne.Translation.PageErrorMessage.ERROR_SURNAME_CONTAIN_SPACE);
        }

        if (!errors.isEmpty())
            return false;

        //if (c.EmpProbaLastDate.Ticks.Equals(0))
        //{
        //    if (c.EmpProbaPeriod > 0)
        //    {
        //        c.EmpProbaLastDate = c.EmpDateOfJoin;
        //        if (c.EmpProbaUnit.Equals("D"))
        //            c.EmpProbaLastDate = c.EmpProbaLastDate.AddDays(c.EmpProbaPeriod);
        //        else if (c.EmpProbaUnit.Equals("M"))
        //            c.EmpProbaLastDate = c.EmpProbaLastDate.AddMonths(c.EmpProbaPeriod);
        //        c.EmpProbaLastDate = c.EmpProbaLastDate.AddDays(-1);
        //    }
        //}
        if (string.IsNullOrEmpty(c.EmpStatus) || c.EmpStatus.Equals("A"))
        {
            if (WebUtils.TotalActiveEmployee(dbConn, c.EmpID) >= WebUtils.productLicense(Session).NumOfEmployees)
            {
                errors.addError(string.Format(PageErrorMessage.ERROR_MAX_LICENSE_LIMITCH_REACH, new string[] { WebUtils.productLicense(Session).NumOfEmployees + " " + HROne.Common.WebUtility.GetLocalizedString("Employee") }));
                return false;
            }
        }

        // Start 0000092, Ricky So, 2014-09-09
        if (c.EmpNationalityID > 0)
        {
            ENationality m_nationality = new ENationality();
            m_nationality.NationalityID = c.EmpNationalityID;
            if (ENationality.db.select(dbConn, m_nationality))
            {
                c.EmpNationality = m_nationality.NationalityDesc;
            }
        }

        if (c.EmpPlaceOfBirthID > 0)
        {
            EPlaceOfBirth m_placeOfBirth = new EPlaceOfBirth();
            m_placeOfBirth.PlaceOfBirthID = c.EmpPlaceOfBirthID;
            if (EPlaceOfBirth.db.select(dbConn, m_placeOfBirth))
            {
                c.EmpPlaceOfBirth = m_placeOfBirth.PlaceOfBirthDesc;
            }
        }

        if (c.EmpPassportIssuedCountryID > 0)
        {
            EIssueCountry m_issueCountry = new EIssueCountry();
            m_issueCountry.CountryID = c.EmpPassportIssuedCountryID;
            if (EIssueCountry.db.select(dbConn, m_issueCountry))
            {
                c.EmpPassportIssuedCountry = m_issueCountry.CountryDesc;
            }
        }

        // End 0000092, Ricky So, 2014-09-09


        WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
        if (EmpID < 0)
        {
            //            Utils.MarkCreate(Session, c);
            c.EmpStatus = "A";
            db.insert(dbConn, c);
            EmpID = c.EmpID;
            if (PrevEmpID > 0)
                if (!IsCreateNewRoleEmployee)
                    EmpUtils.CopyEmpDetail(dbConn, PrevEmpID, EmpID);

            //            url = Utils.BuildURL(-1, CurID);
        }
        else
        {
            //            Utils.Mark(Session, c);
            db.update(dbConn, c);
        }

        //DBFilter empUniformFilter = new DBFilter();
        //empUniformFilter.add(new Match("EmpID", c.EmpID));
        //ArrayList empUniformList = EEmpUniform.db.select(dbConn, empUniformFilter);
        //EEmpUniform empUniform = null;
        //if (empUniformList.Count > 0)
        //    empUniform = (EEmpUniform)empUniformList[0];
        //else
        //{
        //    empUniform = new EEmpUniform();
        //    empUniform.EmpID = c.EmpID;
        //}
        //empUniform.EmpUniformB = EmpUniformB.Text;
        //empUniform.EmpUniformW = EmpUniformW.Text;
        //empUniform.EmpUniformH = EmpUniformH.Text;

        //if (empUniform.EmpUniformID > 0)
        //    EEmpUniform.db.update(dbConn, empUniform);
        //else
        //    EEmpUniform.db.insert(dbConn, empUniform);


        ArrayList list = new ArrayList();
        foreach (RepeaterItem groupItem in EmpExtraFieldGroupRepeater.Items)
        {
            Repeater EmpExtraField = (Repeater)groupItem.FindControl("EmpExtraField");
            foreach (RepeaterItem item in EmpExtraField.Items)
            {
                TextBox textBox = (TextBox)item.FindControl("EmpExtraFieldValue");
                WebDatePicker datePicker = (WebDatePicker)item.FindControl("EmpExtraFieldValueDateControl");

                int EmpExtraFieldID = Int32.Parse(textBox.Attributes["EmpExtraFieldID"]);
                DBFilter filter = new DBFilter();
                filter.add(new Match("EmpID", c.EmpID));
                filter.add(new Match("EmpExtraFieldID", EmpExtraFieldID));
                ArrayList existingExtraFieldValueList = EEmpExtraFieldValue.db.select(dbConn, filter);

                EEmpExtraFieldValue h = null;
                if (existingExtraFieldValueList.Count > 0)
                    h = (EEmpExtraFieldValue)existingExtraFieldValueList[0];
                else
                {
                    h = new EEmpExtraFieldValue();
                    h.EmpID = c.EmpID;
                    h.EmpExtraFieldID = EmpExtraFieldID;
                }
                if (textBox.Visible)
                    h.EmpExtraFieldValue = textBox.Text.Trim();
                else if (datePicker.Visible)
                    h.EmpExtraFieldValue = datePicker.Value.Trim();

                list.Add(h);
            }
        }
        //filter = new DBFilter();
        //filter.add(new Match("EmpID", CurEmpID));
        //filter.add(new Match("EmpPosID", CurID));
        //EEmpHierarchy.db.delete(dbConn, filter);
        foreach (EEmpExtraFieldValue h in list)
        {
            if (h.EmpExtraFieldValueID == 0)
                EEmpExtraFieldValue.db.insert(dbConn, h);
            else
                EEmpExtraFieldValue.db.update(dbConn, h);
        }

        WebUtils.EndFunction(dbConn);
        return true;
    }

    public bool Delete()
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        EEmpPersonalInfo c = new EEmpPersonalInfo();
        c.EmpID = EmpID;
        if (EEmpPersonalInfo.db.select(dbConn, c))
        {
            DBFilter empPayrollFilter = new DBFilter();
            empPayrollFilter.add(new Match("EmpID", c.EmpID));
            DBFilter paymentRecordFilter = new DBFilter();
            paymentRecordFilter.add(new IN("EmpPayrollID", "Select EmpPayrollID from [" + EEmpPayroll.db.dbclass.tableName + "]", empPayrollFilter));

            if (EPaymentRecord.db.count(dbConn, paymentRecordFilter) <= 0)
            {
                WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
                EmpUtils.DeleteEmp(dbConn, c.EmpID);
                db.delete(dbConn, c);
                WebUtils.EndFunction(dbConn);
                return true;
            }
            else
            {
                errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_DELETE_EMP_PAYMENT_EXISTS, new string[] { c.EmpNo }));
                return false;
            }
        }
        return true;
    }

    protected void btnChangeAddressMode_Click(object sender, EventArgs e)
    {
        if (FullResAddrPanel.Visible)
        {
            FullResAddrPanel.Visible = false;
            ThreeLineResAddrPanel.Visible = true;
            string[] address = EmpResAddr.Text.Split(new string[] { "\r\n", "\r" }, StringSplitOptions.None);
            if (address.GetLength(0) >= 1)
                EmpResAddr1.Text = address[0];
            if (address.GetLength(0) >= 2)
                EmpResAddr2.Text = address[1];
            if (address.GetLength(0) >= 3)
            {
                EmpResAddr3.Text = address[2];
                for (int i = 3; i < address.GetLength(0); i++)
                    EmpResAddr3.Text += " " + address[i];
            }
        }
        else
        {
            FullResAddrPanel.Visible = true;
            ThreeLineResAddrPanel.Visible = false;
            EmpResAddr.Text = EmpResAddr1.Text + "\r\n" + EmpResAddr2.Text + "\r\n" + EmpResAddr3.Text;

        }
    }

    protected void empDateOfJoin_Changed(object sender, EventArgs e)
    {
        if (Utils.IsDate(EmpDateOfJoin.Value.ToString()))
        {
            if (EmpServiceDate.Value.ToString() == "")
            {
                EmpServiceDate.Value = EmpDateOfJoin.Value;
            }

            if (EmpNextSalaryIncrementDate.Value.ToString() == "" && ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_PAYSCALE_POINT_SYSTEM) == "Y")
            {
                DateTime m_joinDate = DateTime.Parse(EmpDateOfJoin.Value);
                DateTime m_nextSalaryIncrementDate = new DateTime(); //AppUtils.ServerDateTime().;
                string m_method = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_PAYSCALE_SALARY_INCREMENT_METHOD);

                // method = 1(default): if Join-Date between 2010-01-16 and 2010-02-15, next salary increment date = 2011-01-01
                // method = 2: if Join-Date between 2010-01-01 and 2010-01-31, next salary increment date = 2011-01-01
                // method = 3: if Join-Date between 2010-02-01 and 2010-02-28, next salary increment date = 2011-01-01

                switch (m_method)
                {
                    case "3":
                        m_nextSalaryIncrementDate =  m_joinDate.AddYears(1).AddMonths(-1).AddDays(-m_joinDate.Day + 1);
                        break;
                    case "2":
                        m_nextSalaryIncrementDate = m_joinDate.AddYears(1).AddDays(-m_joinDate.Day + 1);
                        break;
                    default:
                        if (m_joinDate.Day >= 16)
                        {
                            // 2010-02-16 + 1year + (-16 + 1)day + 1month => 2011-03-01
                            m_nextSalaryIncrementDate = m_joinDate.AddYears(1).AddDays(-m_joinDate.Day + 1).AddMonths(1);
                        }
                        else if (m_joinDate.Day <= 15)
                        {   
                            // 2010-02-15 + 1 year + (-15 + 1)day => 2011-02-01
                            m_nextSalaryIncrementDate = m_joinDate.AddYears(1).AddDays(-m_joinDate.Day + 1);
                        }
                        break;
                }
                if (m_nextSalaryIncrementDate.Year < AppUtils.ServerDateTime().Year)
                {
                    EmpNextSalaryIncrementDate.Value = AppUtils.ServerDateTime().ToString("yyyy") + m_nextSalaryIncrementDate.ToString("-MM-dd");
                }else
                    EmpNextSalaryIncrementDate.Value = m_nextSalaryIncrementDate.ToString("yyy-MM-dd");
            }
        }
    }

}
