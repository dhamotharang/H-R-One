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
using HROne.Lib.Entities;
using HROne.DataAccess;

public partial class Attendance_ImportFormatParameterControl : HROneWebControl
{
    public const string PARAM_CODE_TIMECARD_COLUMN_DELIMITER = "COLUMN_DELIMITER";

    public const string PARAM_CODE_TIMECARD_DATE_SEQUENCE = "TIMECARD_DATE_SEQUENCE";
    public const string PARAM_CODE_TIMECARD_DATE_YEAR_FORMAT = "TIMECARD_DATE_YEAR_FORMAT";
    public const string PARAM_CODE_TIMECARD_DATE_SEPARATOR = "TIMECARD_DATE_SEPARATOR";
    public const string PARAM_CODE_TIMECARD_TIME_SEPARATOR = "TIMECARD_TIME_SEPARATOR";

    public const string PARAM_CODE_TIMECARD_DATE_COLUMNINDEX = "TIMECARD_DATE_COLUMNINDEX";
    public const string PARAM_CODE_TIMECARD_TIME_COLUMNINDEX = "TIMECARD_TIME_COLUMNINDEX";
    public const string PARAM_CODE_TIMECARD_DATE_COLUMNINDEX_2 = "TIMECARD_DATE_COLUMNINDEX_2";
    public const string PARAM_CODE_TIMECARD_TIME_COLUMNINDEX_2 = "TIMECARD_TIME_COLUMNINDEX_2";
    public const string PARAM_CODE_TIMECARD_LOCATION_COLUMNINDEX = "TIMECARD_LOCATION_COLUMNINDEX";
    public const string PARAM_CODE_TIMECARD_TIMECARDNUM_COLUMNINDEX = "TIMECARD_TIMECARDNUM_COLUMNINDEX";

    public const string PARAM_CODE_TIMECARD_FILE_HAS_HEADER = "TIMECARD_FILE_HAS_HEADER";

    protected void Page_Load(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.WebControlsLocalization(Session, this.Controls);

        if (!IsPostBack)
        {
            try
            {
                cbxColumnDelimiter.SelectedValue = ESystemParameter.getParameter(dbConn, PARAM_CODE_TIMECARD_COLUMN_DELIMITER);
            }
            catch { }

            try
            {
                cbxDateSequence.SelectedValue = ESystemParameter.getParameter(dbConn, PARAM_CODE_TIMECARD_DATE_SEQUENCE);
            }
            catch { }
            
            try
            {
                cbxYearFormat.SelectedValue = ESystemParameter.getParameter(dbConn, PARAM_CODE_TIMECARD_DATE_YEAR_FORMAT);
            }
            catch { }
            
            try
            {
                cbxDateSeparator.SelectedValue = ESystemParameter.getParameter(dbConn, PARAM_CODE_TIMECARD_DATE_SEPARATOR);
            }
            catch { }
            
            try
            {
                cbxTimeSeparator.SelectedValue = ESystemParameter.getParameter(dbConn, PARAM_CODE_TIMECARD_TIME_SEPARATOR);
            }
            catch { }
            
            try
            {
                cbxDateColumnIndex.SelectedValue = ESystemParameter.getParameter(dbConn, PARAM_CODE_TIMECARD_DATE_COLUMNINDEX);
            }
            catch { }

            try
            {
                cbxTimeColumnIndex.SelectedValue = ESystemParameter.getParameter(dbConn, PARAM_CODE_TIMECARD_TIME_COLUMNINDEX);
            }
            catch { }

            try
            {
                cbxDateColumnIndex2.SelectedValue = ESystemParameter.getParameter(dbConn, PARAM_CODE_TIMECARD_DATE_COLUMNINDEX_2);
            }
            catch { }

            try
            {
                cbxTimeColumnIndex2.SelectedValue = ESystemParameter.getParameter(dbConn, PARAM_CODE_TIMECARD_TIME_COLUMNINDEX_2);
            }
            catch { }

            try
            {
                cbxLocationColumnIndex.SelectedValue = ESystemParameter.getParameter(dbConn, PARAM_CODE_TIMECARD_LOCATION_COLUMNINDEX);
            }
            catch { }
            
            try
            {
                cbxTimeCardNumColumnIndex.SelectedValue = ESystemParameter.getParameter(dbConn, PARAM_CODE_TIMECARD_TIMECARDNUM_COLUMNINDEX);
            }
            catch { }

            try
            {
                chkHasHeader.Checked = ESystemParameter.getParameter(dbConn, PARAM_CODE_TIMECARD_FILE_HAS_HEADER).Equals("Y");
            }
            catch { }
        }
    }

    public void SaveSettings()
    {
        ESystemParameter.setParameter(dbConn, PARAM_CODE_TIMECARD_COLUMN_DELIMITER, cbxColumnDelimiter.SelectedValue);
        ESystemParameter.setParameter(dbConn, PARAM_CODE_TIMECARD_DATE_SEQUENCE, cbxDateSequence.SelectedValue);
        ESystemParameter.setParameter(dbConn, PARAM_CODE_TIMECARD_DATE_YEAR_FORMAT, cbxYearFormat.SelectedValue);
        ESystemParameter.setParameter(dbConn, PARAM_CODE_TIMECARD_DATE_SEPARATOR, cbxDateSeparator.SelectedValue);
        ESystemParameter.setParameter(dbConn, PARAM_CODE_TIMECARD_TIME_SEPARATOR, cbxTimeSeparator.SelectedValue);
        ESystemParameter.setParameter(dbConn, PARAM_CODE_TIMECARD_DATE_COLUMNINDEX, cbxDateColumnIndex.SelectedValue);
        ESystemParameter.setParameter(dbConn, PARAM_CODE_TIMECARD_TIME_COLUMNINDEX, cbxTimeColumnIndex.SelectedValue);
        ESystemParameter.setParameter(dbConn, PARAM_CODE_TIMECARD_DATE_COLUMNINDEX_2, cbxDateColumnIndex2.SelectedValue);
        ESystemParameter.setParameter(dbConn, PARAM_CODE_TIMECARD_TIME_COLUMNINDEX_2, cbxTimeColumnIndex2.SelectedValue);
        ESystemParameter.setParameter(dbConn, PARAM_CODE_TIMECARD_LOCATION_COLUMNINDEX, cbxLocationColumnIndex.SelectedValue);
        ESystemParameter.setParameter(dbConn, PARAM_CODE_TIMECARD_TIMECARDNUM_COLUMNINDEX, cbxTimeCardNumColumnIndex.SelectedValue);
        ESystemParameter.setParameter(dbConn, PARAM_CODE_TIMECARD_FILE_HAS_HEADER, chkHasHeader.Checked ? "Y" : "N");
    }

    public string ColumnDelimiter
    {
        get { return cbxColumnDelimiter.SelectedValue; }
    }

    public string DateSequence
    {
        get { return cbxDateSequence.SelectedValue; }
    }

    public string YearFormat
    {
        get { return cbxYearFormat.SelectedValue; }
    }

    public string DateSeparator
    {
        get { return cbxDateSeparator.SelectedValue; }
    }
    public string TimeSeparator
    {
        get { return cbxTimeSeparator.SelectedValue; }
    }

    public int DateColumnIndex
    {
        get { return int.Parse(cbxDateColumnIndex.SelectedValue); }
    }

    public int TimeColumnIndex
    {
        get { return int.Parse(cbxTimeColumnIndex.SelectedValue); }
    }

    public int DateColumnIndex2
    {
        get { return int.Parse(cbxDateColumnIndex2.SelectedValue); }
    }

    public int TimeColumnIndex2
    {
        get { return int.Parse(cbxTimeColumnIndex2.SelectedValue); }
    }

    public int LocationColumnIndex
    {
        get { return int.Parse(cbxLocationColumnIndex.SelectedValue); }
    }
    public int TimeCardNumColumnIndex
    {
        get { return int.Parse(cbxTimeCardNumColumnIndex.SelectedValue); }
    }
    public bool UploadFileHasHeader
    {
        get { return chkHasHeader.Checked; }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(ESystemParameter.db, Page.Master);
        errors.clear();
        SaveSettings();
        errors.addError("Save Successfully");
    }
}
