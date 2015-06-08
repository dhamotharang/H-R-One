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

public partial class Report_Builder : HROneWebPage
{
    public Binding binding;
    // Start 0000167, Miranda, 2015-03-09
    private String NOT_SELECTED = "Not Selected";
    public DBManager db = EReportBuilder.db;
    //public DBManager db = EReportBuilderField.db;
    //public EReportBuilderField obj;
    public static String currentDescription;
    public static String curentFieldName;
    // End 0000167, Miranda, 2015-03-09
    public static String currentDBField;
    public static String currentType;
    public int CurID = -1;
    public static WFValueList CharCriteriaMethod = new AppUtils.NewWFTextList(new string[] { "IS", "NOT IS", "CONTAIN", "NOT CONTAIN", "START WITH", "END WITH" });
    public static WFValueList SizeCriteriaMethod = new AppUtils.NewWFTextList(new string[] { "IS", "NOT IS", "LESS THAN", "LESS OR EQUAL", "GRATER THAN", "GRATER OR EQUAL" });

    protected void Page_Load(object sender, EventArgs e)
    {

        binding = new Binding(dbConn, db);
        string charSelected = CharCriteriaD.SelectedValue;
        string sizeSelected = SizeCriteriaD.SelectedValue;
        WebFormUtils.loadValues(dbConn, CharCriteriaD, CharCriteriaMethod, new DBFilter(), ci, (string)charSelected, null);
        WebFormUtils.loadValues(dbConn, SizeCriteriaD, SizeCriteriaMethod, new DBFilter(), ci, (string)sizeSelected, null);
        //binding.add(new DropDownVLBinder(db, ModuleName, EReportBuilderField.VLModuleCode));
        init_ModuleNameDropdown();
        init_FavriouteTemplates();
        binding.init(Request, Session);
        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void init_ModuleNameDropdown()
    {
        // Start 0000167, Ricky So, 2015-03-04
        // Start 0000167, Miranda, 2015-03-09
        //if (ModuleName.Items.Count <= 0)
        //{
        //    ModuleName.Items.Add("Not Selected");
        //    DataTable m_table = AppUtils.runSelectSQL("DISTINCT rb.ModuleName as ModuleName", "From ReportBuilderField rb ", null, dbConn);

        //    foreach (DataRow row in m_table.Rows)
        //    {
        //        ModuleName.Items.Add(row["ModuleName"].ToString());
        //    }
        //}

        if (ModuleName.Items.Count <= 0)
        {
            DBFilter m_moduleFilter = new DBFilter();
            m_moduleFilter.add(new Match("name", "like", "rb_%"));

            ModuleName.Items.Add(NOT_SELECTED);
            DataTable m_table = AppUtils.runSelectSQL("name", "FROM sys.views ", m_moduleFilter, dbConn);

            foreach (DataRow m_row in m_table.Rows)
            {
                ModuleName.Items.Add(m_row["name"].ToString().Substring(3).Replace("_", " "));
            }
        }
        // End 0000167, Miranda, 2015-03-09
        // End 000167, Ricky So, 2015-03-04
    }

    protected void init_FavriouteTemplates()
    {
        DBFilter m_filter = new DBFilter();
        if (Step1FavriteTemplateList.Items.Count <= 0)
        {
            foreach (EReportBuilder m_rb in EReportBuilder.db.select(dbConn, m_filter))
            {
                // Start 0000167, Miranda, 2015-03-09
                Step1FavriteTemplateList.Items.Add(new ListItem(m_rb.ReportName.ToString(), string.Format("{0}-{1}-{2}", m_rb.SelectedFieldNames, m_rb.ReportModule, m_rb.DBFilterExpressions)));
                // End 0000167, Miranda, 2015-03-09
            }
        }
    }

    protected void ModuleName_Changed(object sender, EventArgs e)
    {
        // Start 000167, Ricky So, 2015-03-04
        String m_selectedModule = ModuleName.SelectedItem.Text;
        // Start 0000167, Miranda, 2015-03-09
        if (!m_selectedModule.Equals(NOT_SELECTED))
        {
            // get the view structure through select empty records
            DataTable m_table = AppUtils.runSelectSQL("TOP 0 *", "FROM rb_" + m_selectedModule.Replace(" ", "_"), null, dbConn);
            CheckBoxListObject.Items.Clear();
            foreach (DataColumn m_column in m_table.Columns)
            {
                //Start 0000167, Miranda, 2015-04-06
                if (!m_column.ColumnName.Equals("EmpID"))
                {
                    CheckBoxListObject.Items.Add(new ListItem("[" + m_column.ColumnName + "]", string.Format("{0};{1}", m_column.ColumnName, m_column.DataType)));
                }
                //End 0000167, Miranda, 2015-04-06
            }
        }
        // End 0000167, Miranda, 2015-03-09

        //String m_selectedModule = ModuleName.SelectedItem.Text;
        //DBFilter m_filter = new DBFilter();
        //m_filter.add(new Match("ModuleName", m_selectedModule));
        //CheckBoxListObject.Items.Clear();
        //if (CheckBoxListObject.Items.Count <= 0)
        //{
        //    foreach (EReportBuilderField m_field in EReportBuilderField.db.select(dbConn, m_filter))
        //    {
        //        CheckBoxListObject.Items.Add(new ListItem(m_field.Description, m_field.FieldID.ToString()));
        //    }
        //}
        // End 000167, Ricky So, 2015-03-04
    }

    protected void SelectedField_Changed(object sender, EventArgs e)
    {
        String m_selectedItem = FilterFieldCheckBoxList.SelectedItem.Value;
        String[] m_values = m_selectedItem.Split(new Char[] { ';' });
        currentDescription = FilterFieldCheckBoxList.SelectedItem.Text;
        // Start 0000167, Miranda, 2015-03-09
        curentFieldName = m_values[0];
        //currentDBField = m_values[1];
        currentType = m_values[1];
        if (!currentDescription.Equals(NOT_SELECTED))
        {
            if (currentType.Equals("System.String") || currentType.Equals("System.Char"))
            // End 0000167, Miranda, 2015-03-09
            {
                CharCriteriaD.Visible = true;
                SizeCriteriaD.Visible = false;
                Step3CriteriaTextField.Text = "";
                Step3CriteriaTextField.Visible = true;
                FilterDatePicker.Visible = false;
                Step3AddFilterBtn.Enabled = true;
            }
            // Start 0000167, Miranda, 2015-03-09
            else if (currentType.Equals("System.DateTime") || currentType.Equals("System.TimeSpan"))
            // End 0000167, Miranda, 2015-03-09
            {
                CharCriteriaD.Visible = false;
                SizeCriteriaD.Visible = true;
                Step3CriteriaTextField.Visible = false;
                FilterDatePicker.Visible = true;
                Step3AddFilterBtn.Enabled = true;
            }
            // Start 0000167, Miranda, 2015-03-09
            else if (!currentType.Equals("System.Boolean"))
            // End 0000167, Miranda, 2015-03-09
            {
                CharCriteriaD.Visible = false;
                SizeCriteriaD.Visible = true;
                Step3CriteriaTextField.Text = "";
                Step3CriteriaTextField.Visible = true;
                FilterDatePicker.Visible = false;
                Step3AddFilterBtn.Enabled = true;
            }
        }
    }

    protected void Step1NextBtn_OnClick(object sender, EventArgs e)
    {
        // Start 0000167, Miranda, 2015-03-09
        if ((Step1ChooseTemplateRad.Checked && ModuleName.SelectedItem.Text.Equals(NOT_SELECTED)) || (Step1SelectFavariteRad.Checked && Step1FavriteTemplateList.SelectedItem == null))
        // End 0000167, Miranda, 2015-03-09
        {
            PageErrors m_errors = PageErrors.getErrors(db, Page.Master);
            m_errors.addError("Please Select Template First!");
            return;
        }
        rbstep1.Visible = false;
        rbstep2.Visible = true;
        rbstep3.Visible = false;
        rbstep4.Visible = false;

    }

    protected void Step2NextBtn_OnClick(object sender, EventArgs e)
    {
        if (CheckBoxListObject.SelectedItem == null)
        {
            PageErrors m_errors = PageErrors.getErrors(db, Page.Master);
            m_errors.addError("Please Select Field First!");
            return;
        }
        rbstep1.Visible = false;
        rbstep2.Visible = false;
        rbstep3.Visible = true;
        rbstep4.Visible = false;
        // Start 0000167, Miranda, 2015-03-09
        String m_selectedFieldName = "";
        FilterFieldCheckBoxList.Items.Clear();
        FilterFieldCheckBoxList.Items.Add(NOT_SELECTED);
        foreach (ListItem m_li in CheckBoxListObject.Items)
        {
            if (m_li.Selected)
            {
                FilterFieldCheckBoxList.Items.Add(new ListItem(m_li.Text, m_li.Value));
            }
        }

        //DBFilter m_filter = new DBFilter();
        //String[] m_idArrays = m_selectedFieldName.Split(new Char[] { ';' });
        //m_filter.add(new IN("FieldID", m_idArrays));
        //FilterFieldCheckBoxList.Items.Clear();
        //if (FilterFieldCheckBoxList.Items.Count <= 0)
        //{
        //    FilterFieldCheckBoxList.Items.Add("Not Selected");
        //    foreach (EReportBuilderField m_field in EReportBuilderField.db.select(dbConn, m_filter))
        //    {
        //        FilterFieldCheckBoxList.Items.Add(new ListItem(m_field.Description.ToString(), string.Format("{0};{1};{2}", m_field.FieldID.ToString(), m_field.DBField, m_field.Type)));
        //    }
        //}
        // End 0000167, Miranda, 2015-03-09
    }

    protected void Step2BackBtn_OnClick(object sender, EventArgs e)
    {
        rbstep1.Visible = true;
        rbstep2.Visible = false;
        rbstep3.Visible = false;
        rbstep4.Visible = false;
    }

    protected void Step3NextBtn_OnClick(object sender, EventArgs e)
    {
        rbstep1.Visible = false;
        rbstep2.Visible = false;
        rbstep3.Visible = false;
        rbstep4.Visible = true;

    }

    protected void Step3BackBtn_OnClick(object sender, EventArgs e)
    {
        rbstep1.Visible = false;
        rbstep2.Visible = true;
        rbstep3.Visible = false;
        rbstep4.Visible = false;
    }

    protected void rdRadio_Change(object sender, EventArgs e)
    {
        if (Step1ChooseTemplateRad.Checked)
        {
            ModuleName.Enabled = true;
            Step1FavriteTemplateList.ClearSelection();
            Step1FavriteTemplateList.Enabled = false;
        }
        else
        {
            ModuleName.Enabled = false;
            Step1FavriteTemplateList.Enabled = true;
        }
    }

    protected void Step3AddFilterBtn_OnClick(object sender, EventArgs e)
    {
        String criteriaLabel = "";
        String criteriaValue = "";
        // Start 0000167, Miranda, 2015-03-09
        if (FilterFieldCheckBoxList.SelectedItem.Text.Equals(NOT_SELECTED))
        // End 0000167, Miranda, 2015-03-09
        {
            return;
        }
        if (currentType != null)
        {
            // Start 0000167, Miranda, 2015-03-09
            if (currentType.Equals("System.DateTime") || currentType.Equals("System.TimeSpan"))
            {
                if (FilterDatePicker.Value.Equals("") || FilterDatePicker.Value == null)
                {
                    return;
                }
            }
            else
            {
                if (Step3CriteriaTextField.Text == null || Step3CriteriaTextField.Text.Equals(""))
                {
                    return;
                }
            }
            if (currentType.Equals("System.String") || currentType.Equals("System.Char"))
            {
                criteriaLabel = currentDescription + " | " + CharCriteriaD.Text + " | " + Step3CriteriaTextField.Text;
                criteriaValue = currentDBField + "|" + CharCriteriaD.Text + "|" + Step3CriteriaTextField.Text;
            }
            else if (currentType.Equals("System.DateTime") || currentType.Equals("System.TimeSpan"))
            {
                criteriaLabel = currentDescription + " | " + SizeCriteriaD.Text + " | " + FilterDatePicker.Value;
                criteriaValue = currentDBField + "|" + SizeCriteriaD.Text + "|" + FilterDatePicker.Value;
            }
            else if (!currentType.Equals("System.Boolean"))
            {
                criteriaLabel = currentDescription + " | " + SizeCriteriaD.Text + " | " + Step3CriteriaTextField.Text;
                criteriaValue = currentDBField + "|" + SizeCriteriaD.Text + "|" + Step3CriteriaTextField.Text;
            }
            CriteriaFieldCBL.Items.Add(new ListItem(criteriaLabel, criteriaValue));
            //Start 0000167, Miranda, 2015-04-02
            Step3CriteriaTextField.Text = "";
            FilterDatePicker.Value = "";
            //End 0000167, Miranda, 2015-04-02
        }
        // End 0000167, Miranda, 2015-03-09
    }

    //Start 0000167, Miranda, 2015-04-02
    protected void Step3DeleteFilterBtn_OnClick(object sender, EventArgs e)
    {
        ListItemCollection deleteList = new ListItemCollection();

        foreach (ListItem filterItem in CriteriaFieldCBL.Items)
        {
            if (filterItem.Selected)
            {
                deleteList.Add(filterItem);
            }
        }
        foreach (ListItem deleteItem in deleteList)
        {
            CriteriaFieldCBL.Items.Remove(deleteItem);
        }

    }
    //End 0000167, Miranda, 2015-04-02

    protected void Step4ExportBtn_OnClick(object sender, EventArgs e)
    {
        if (ExportFileName.Text.Equals("") || ExportFileName.Text == null)
        {
            PageErrors errors = PageErrors.getErrors(db, Page.Master);
            errors.addError("Please input report name");
            return;
        }
        ExportReport();

    }

    protected void SaveTemplateBtn_OnClick(object sender, EventArgs e)
    {
        SaveReportBuilderTemplate();
    }

    protected void ExportReport()
    {
        DBFilter m_filter = new DBFilter();
        foreach (ListItem li in CriteriaFieldCBL.Items)
        {
            // Start 0000167, Miranda, 2015-03-09
            String[] str = li.Text.Split(new Char[] { '|' });
            String DBField = str[0].ToString().Trim();
            // End 0000167, Miranda, 2015-03-09
            // Start 0000167, Miranda, 2015-03-11
            String criteria = str[1].ToString().Trim();
            String value = str[2].ToString().Trim();
            // End 0000167, Miranda, 2015-03-11

            switch (criteria)
            {
                case "IS": m_filter.add(new Match(DBField, value)); break;
                case "NOT IS": m_filter.add(new Match(DBField, "!=", value)); break;
                // Start 0000167, Miranda, 2015-03-11
                case "CONTAIN": m_filter.add(new Match(DBField, "LIKE", "%" + value + "%")); break;
                case "NOT CONTAIN": m_filter.add(new Match(DBField, "NOT LIKE", "%" + value + "%")); break;
                case "START WITH": m_filter.add(new Match(DBField, "LIKE", value + "%")); break;
                case "END WITH": m_filter.add(new Match(DBField, "LIKE", "%" + value)); break;
                // End 0000167, Miranda, 2015-03-11
                case "LESS THAN": m_filter.add(new Match(DBField, "<", value)); break;
                case "GRATER THAN": m_filter.add(new Match(DBField, ">", value)); break;
                case "LESS OR EQUAL": m_filter.add(new Match(DBField, "<=", value)); break;
                case "GRATER OR EQUAL": m_filter.add(new Match(DBField, ">=", value)); break;
            }
        }

        // Start 0000167, Miranda, 2015-03-09
        //ArrayList empList = EEmpPersonalInfo.db.select(dbConn, m_filter);
        // load data
        String SelectedFieldNames = getSelectedFields();
        // Start 0000167, Miranda, 2015-04-06
        String from = " FROM rb_" + ModuleName.SelectedItem.Text.Replace(" ", "_");
        m_filter.add(WebUtils.AddRankFilter(Session, "EmpID", true));
        DataTable m_table = AppUtils.runSelectSQL(SelectedFieldNames, from, m_filter, dbConn);
        // End 0000167, Miranda, 2015-04-06

        if (m_table.Rows.Count > 0)
        // End 0000167, Miranda, 2015-03-09
        {
            string exportFileName = System.IO.Path.GetTempFileName();
            System.IO.File.Delete(exportFileName);
            exportFileName += ".xls";
            HROne.Export.ExcelExport export = new HROne.Export.ExcelExport(exportFileName);
            DataSet dataSet = new DataSet();

            String fileName = ExportFileName.Text;
            DataTable tmpDataTable = dataSet.Tables.Add(fileName);

            // Start 0000167, Miranda, 2015-03-09
            //foreach (ListItem item in FilterFieldCheckBoxList.Items)
            //{
            //    if (!item.Text.Equals("Not Selected"))
            //    {
            //        String[] values = item.Value.Split(new Char[] { ';' });
            //        string Description = item.Text;
            //        string FieldID = values[0];
            //        string Type = values[1];
            //        //string Type = values[2];

            //        switch (Type)
            //        {
            //            case "tvarchar": tmpDataTable.Columns.Add(Description, typeof(string)); break;
            //            case "tdate": tmpDataTable.Columns.Add(Description, typeof(DateTime)); break;
            //            case "tinteger": tmpDataTable.Columns.Add(Description, typeof(double)); break;
            //        }
            //    }
            //}
            foreach (DataColumn fieldName in m_table.Columns)
            {
                string fieldType = fieldName.DataType.ToString();
                switch (fieldType)
                {
                    case "System.String":
                    case "System.Char": tmpDataTable.Columns.Add(fieldName.ColumnName, typeof(string)); break;
                    case "System.DateTime":
                    case "System.TimeSpan": tmpDataTable.Columns.Add(fieldName.ColumnName, typeof(DateTime)); break;
                    case "System.Boolean": break;
                    default: tmpDataTable.Columns.Add(fieldName.ColumnName, typeof(double)); break;
                }
            }

            //foreach (EEmpPersonalInfo obj in empList)
            foreach (DataRow curRow in m_table.Rows)
            {
                DataRow row = tmpDataTable.NewRow();
                //foreach (ListItem item in FilterFieldCheckBoxList.Items)
                //{
                //    if (!item.Text.Equals("Not Selected"))
                //    {
                //        String[] values = item.Value.Split(new Char[] { ';' });
                //        string Description = item.Text;
                //        string FieldID = values[0];
                //        string Type = values[1];
                //        //string Type = values[2];
                //        foreach (System.Reflection.PropertyInfo p in obj.GetType().GetProperties())
                //        {
                //            if (p.Name.ToString().Equals(DBField))
                //            {
                //              row[Description] = p.GetValue(obj, null);
                //            }
                //        }
                //    }
                //}
                Object[] o = curRow.ItemArray;
                for (int i = 0; i < o.Length; i++)
                {
                    row[i] = o[i];
                }
                // End 0000167, Miranda, 2015-03-09
                tmpDataTable.Rows.Add(row);
            }

            export.Update(dataSet);
            WebUtils.TransmitFile(Response, exportFileName, fileName + "_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true);
            Response.End();
        }
        else
        {
            PageErrors errors = PageErrors.getErrors(db, Page.Master);
            // Start 0000167, Miranda, 2015-03-09
            errors.addError("No data");
            // End 0000167, Miranda, 2015-03-09
        }
    }

    // Start 0000167, Miranda, 2015-03-09
    private string getSelectedFields()
    {
        String SelectedFieldNames = "";
        bool isFirstSelField = true;
        foreach (ListItem li in CheckBoxListObject.Items)
        {
            if (li.Selected)
            {
                if (isFirstSelField)
                {
                    SelectedFieldNames = li.Text;
                    isFirstSelField = false;
                }
                else
                {
                    SelectedFieldNames += "," + li.Text;
                }
            }
        }
        return SelectedFieldNames;
    }
    // End 0000167, Miranda, 2015-03-09

    protected void SaveReportBuilderTemplate()
    {
        String reportName = TemplateName.Text;
        // Start 0000167, Miranda, 2015-03-09
        String reportModule = ModuleName.SelectedItem.Text;
        String dbFilterExpressions = "";
        String SelectedFieldNames = "";
        // End 0000167, Miranda, 2015-03-09

        if (reportName.Equals("") || reportName == null)
        {
            PageErrors errors = PageErrors.getErrors(db, Page.Master);
            errors.addError("Please input template name!");
        }

        //check if report name duplicate
        DBFilter r_filter = new DBFilter();
        r_filter.add(new Match("ReportName", reportName));
        if (EReportBuilder.db.select(dbConn, r_filter).Count > 0)
        {
            PageErrors errors = PageErrors.getErrors(db, Page.Master);
            errors.addError("Template name has been existed");
            return;
        }

        // Start 0000167, Miranda, 2015-03-09
        SelectedFieldNames = getSelectedFields();

        bool isFirstFilterExp = true;
        foreach (ListItem li in CriteriaFieldCBL.Items)
        {
            if (isFirstFilterExp)
            {
                dbFilterExpressions = li.Text;
                isFirstFilterExp = false;
            }
            else
            {
                dbFilterExpressions += ";" + li.Text;
            }
        }
        EReportBuilder reportBuilder = new EReportBuilder();
        reportBuilder.ReportName = reportName;
        reportBuilder.ReportModule = reportModule;
        reportBuilder.DBFilterExpressions = dbFilterExpressions;
        reportBuilder.SelectedFieldNames = SelectedFieldNames;
        // End 0000167, Miranda, 2015-03-09

        EReportBuilder.db.insert(dbConn, reportBuilder);
    }

    protected void FinishBtn_OnClick(object sender, EventArgs e)
    {
        SaveReportBuilderTemplate();
        ExportReport();
        //Response.Redirect("/HROneWeb/Report_Builder.aspx");
    }

    protected void StartOverBtn_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("/HROneWeb/Report_Builder.aspx");
    }

    protected void Step1FavriteTemplateList_OnChange(object sender, EventArgs e)
    {
        ListItem item = Step1FavriteTemplateList.SelectedItem;
        String reportName = item.Text;
        // Start 0000167, Miranda, 2015-03-09
        String[] values = item.Value.Split(new Char[] { '-' });//fieldNames, reportModule, criteriaFieldNames
        String[] SelectedFieldNamesArray = values[0].Split(new Char[] { ',' });
        String moduleName = values[1];
        String[] dbFilterExpresstionsArray = values[2].Split(new Char[] { ';' });

        ModuleName.SelectedItem.Text = moduleName;
        ModuleName_Changed(sender, e);//load all fields

        FilterFieldCheckBoxList.Items.Clear();//reset criteria fields combo-box for user to set criteria
        FilterFieldCheckBoxList.Items.Add(NOT_SELECTED);
        foreach (String selField in SelectedFieldNamesArray)
        {
            ListItem li = CheckBoxListObject.Items.FindByText(selField);//set selected fields
            if (li != null)
            {
                li.Selected = true;

                FilterFieldCheckBoxList.Items.Add(new ListItem(li.Text, li.Value));//set selected field to criteria to criteria fields combo-box
            }
            else
            {
                li.Selected = false;
            }
        }

        CriteriaFieldCBL.Items.Clear();//reset criteria
        for (int i = 0; i < dbFilterExpresstionsArray.Length; i++)
        {
            if (!dbFilterExpresstionsArray[i].Equals(""))
            {
                CriteriaFieldCBL.Items.Add(new ListItem(dbFilterExpresstionsArray[i], dbFilterExpresstionsArray[i]));
            }
        }
        // End 0000167, Miranda, 2015-03-09

        TemplateName.Text = reportName;
    }

}