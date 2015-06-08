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
using System.Data.OleDb;
using HROne.Import;
using HROne.Lib.Entities;
using HROne.DataAccess;
//using perspectivemind.validation;

public partial class Attendance_TimeCardRecord_Delete : HROneWebPage
{
    private const string FUNCTION_CODE = "ATT006";
    protected DBManager db = ETimeCardRecord.db;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();
        DateTime dtDeleteFrom, dtDeleteTo;
        DBFilter timeCardRecordFilter = new DBFilter();
        if (DateTime.TryParse(DeleteFrom.Value, out dtDeleteFrom) && DateTime.TryParse(DeleteTo.Value, out dtDeleteTo))
        {
            timeCardRecordFilter.add(new Match("TimeCardRecordDateTime", "<", dtDeleteTo.AddDays(1)));
            timeCardRecordFilter.add(new Match("TimeCardRecordDateTime", ">=", dtDeleteFrom));

            //    WebUtils.StartFunction(Session, FUNCTION_CODE);
            db.delete(dbConn, timeCardRecordFilter);
            //    WebUtils.EndFunction(dbConn);
            errors.addError("Time card record is removed successfully.");
        }
    }


}