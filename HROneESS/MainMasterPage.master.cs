using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using HROne.Lib.Entities;
using HROne.DataAccess;

public partial class MainMasterPage : HROneWebMasterPage
{
    EESSUser user = null;
    bool hasMessage = true;

    protected void Page_Init(object sender, EventArgs e)
    {
        Session["LastURL"] = Request.Url.PathAndQuery;
        Session["CurrentURI"] = Request.Url;
        if (!Request.AppRelativeCurrentExecutionFilePath.Equals("~/ESS_EmpChangePassword.aspx"))
        {
            if (Session["ForceChangePassword"] != null)
            {
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/ESS_EmpChangePassword.aspx");
            }
        }

        user = WebUtils.GetCurUser(Session);
        //WebUtils.SetSessionLanguage(Session, user);

        //lblVersionNo.Text = ProductVersion.CURRENT_PROGRAM_VERSION;

        HROne.ProductLicense productLicense = WebUtils.productLicense(Session);

        if (productLicense != null && productLicense.ProductType == HROne.ProductLicense.ProductLicenseType.iMGR)
        {
            Page.Title = "iStaff";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        string dbTitle = HROne.Lib.Entities.ESystemParameter.getParameter(dbConn, HROne.Lib.Entities.ESystemParameter.PARAM_CODE_DB_TITLE);
        if (!string.IsNullOrEmpty(dbTitle))
            Page.Title += " (" + dbTitle + ")";
        this.form1.Attributes.Add("AUTOCOMPLETE", "OFF");

        HROne.Common.WebUtility.AddBrowserCompatibilityMeta(Page);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!this.pageError.getErrors().isEmpty())
        {
            Dictionary<string, Error> errorTable = this.pageError.getErrors().errors;

            foreach (string key in errorTable.Keys)
            {
                object newValue = errorTable[key];
                if (newValue is Error)
                {
                    ((Error)newValue).name = HROne.Common.WebUtility.GetLocalizedString(((Error)newValue).name);
                    //object[] parameters = ((perspectivemind.validation.Error)newValue).parameters;
                    //for (int i = 0; i < parameters.GetLength(0); i++)
                    //{
                    //    if (parameters[i] is string)
                    //        parameters[i] = HROne.Common.WebUtility.GetLocalizedString(((string)parameters[i]));
                    //}
                }

            }
            Page page = this.Page;// (Page)HttpContext.Current.Handler;
            if (page != null)
            {
                string errorMessage = pageError.getErrors().getPrompt();
                errorMessage = errorMessage.Replace("\\", "\\\\").Replace("\r", "\\r").Replace("\n", "\\n").Replace("\"", "\\\"");
                errorMessage = errorMessage.Replace(HROne.Common.WebUtility.GetLocalizedString("validate.prompt"), "");
                ScriptManager.RegisterStartupScript(page, page.GetType(), "errorMessage", "alert(\"" + errorMessage + "\");", true);

//     ScriptManager.RegisterStartupScript(page, page.GetType(), "errorMessage2", "popupDialog(\"testing2\", \"abc\");", true);

            }
        }
    }

    protected void mainMenu_PreRender(object sender, EventArgs e)
    {
        ArrayList removeSubMenuItemArray = GetRemoveMenuItemList(mainMenu.Items);
        foreach (MenuItem subMenuItem in removeSubMenuItemArray)
            if (subMenuItem.Parent == null)
                mainMenu.Items.Remove(subMenuItem);
            else
                subMenuItem.Parent.ChildItems.Remove(subMenuItem);

        HROne.Common.WebUtility.MenuLocalization(mainMenu);
    }
    protected void mainMenu_MenuItemDataBound(object sender, MenuEventArgs e)
    {
        string imageURL = ((SiteMapNode)e.Item.DataItem)["ImageURL"];
        if (imageURL != null && imageURL.Length > 0)
            e.Item.ImageUrl = imageURL;

        string m_target = ((SiteMapNode)e.Item.DataItem)["target"];
        if (m_target != null && m_target.Length > 0)
            e.Item.Target = m_target;

    }

    private ArrayList GetRemoveMenuItemList(MenuItemCollection menuItemCollection)
    {
        HROne.ProductLicense productLicense = WebUtils.productLicense(Session);
        EESSUser essUser = WebUtils.GetCurUser(Session);
        ArrayList removeSubMenuItemArray = new ArrayList();
        foreach (MenuItem subMenuItem in menuItemCollection)
        {

            if (productLicense != null)
            {
                // ***** Start 2013/11/15, Ricky So, revise the show/hide menu routine
                // if (subMenuItem.Text.Equals("Leave Application", StringComparison.CurrentCultureIgnoreCase))
                 if (subMenuItem.NavigateUrl.EndsWith("/ESS_EmpLeaveApplication.aspx", StringComparison.CurrentCultureIgnoreCase))
                // ***** End 2013/11/15, Ricky So, revise the show/hide menu routine
                    if (!(ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_LEAVE_APPLICATION).Equals("N", StringComparison.CurrentCultureIgnoreCase) ? false : true))
                        removeSubMenuItemArray.Add(subMenuItem);

                // ***** Start 2013/11/15, Ricky So, revise the show/hide menu routine
                // if (subMenuItem.Text.Equals("Request Status", StringComparison.CurrentCultureIgnoreCase))
                if (subMenuItem.NavigateUrl.EndsWith("/ESS_EmpRequestStatus.aspx", StringComparison.CurrentCultureIgnoreCase))
                // ***** End 2013/11/15, Ricky So, revise the show/hide menu routine
                    if (!(ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_LEAVE_APPLICATION).Equals("N", StringComparison.CurrentCultureIgnoreCase) ? false : true)
                        //Start 0000060, Miranda, 2014-07-22
                        && !(ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_OT_CLAIMS).Equals("N", StringComparison.CurrentCultureIgnoreCase) ? false : true)
                        //End 0000060, Miranda, 2014-07-22
                        && !(ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_CHANGE_EE_INFO).Equals("N", StringComparison.CurrentCultureIgnoreCase) ? false : true)
                        )
                        removeSubMenuItemArray.Add(subMenuItem);

                
                // ***** Start 2013/11/15, Ricky So, revise the show/hide menu routine
                // if (subMenuItem.Text.Equals("Application Approval", StringComparison.CurrentCultureIgnoreCase)
                //    || subMenuItem.NavigateUrl.EndsWith("/ESS_EmpSupervisorApproval.aspx", StringComparison.CurrentCultureIgnoreCase))
                if (subMenuItem.NavigateUrl.EndsWith("/ESS_EmpSupervisorApproval.aspx", StringComparison.CurrentCultureIgnoreCase))
                // ***** End 2013/11/15, Ricky So, revise the show/hide menu routine
                {
                    ESSAuthorizationProcess authorizationProcess = new ESSAuthorizationProcess(dbConn);
                    int groupCount = authorizationProcess.GetAuthorizerAuthorizationGroupList(user.EmpID).Count;
                    if (groupCount <= 0)
                        removeSubMenuItemArray.Add(subMenuItem);
                    else if (!(ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_LEAVE_APPLICATION).Equals("N", StringComparison.CurrentCultureIgnoreCase) ? false : true)
                        //Start 0000060, Miranda, 2014-07-22
                        && !(ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_OT_CLAIMS).Equals("N", StringComparison.CurrentCultureIgnoreCase) ? false : true)
                        //End 0000060, Miranda, 2014-07-22
                        && !(ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_CHANGE_EE_INFO).Equals("N", StringComparison.CurrentCultureIgnoreCase) ? false : true)
                        )
                        removeSubMenuItemArray.Add(subMenuItem);
                }

                // ***** Start 2013/11/15, Ricky So, revise the show/hide menu routine
                // if (subMenuItem.Text.Equals("Leave Balance Enquiry", StringComparison.CurrentCultureIgnoreCase))
                if (subMenuItem.NavigateUrl.EndsWith("/ESS_EmpLeaveBalanceEnquiry.aspx", StringComparison.CurrentCultureIgnoreCase))
                // ***** End 2013/11/15, Ricky So, revise the show/hide menu routine
                    if (!(ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_LEAVE_BALANCE_ENQUIRY).Equals("N", StringComparison.CurrentCultureIgnoreCase) ? false : true))
                        removeSubMenuItemArray.Add(subMenuItem);



                // ***** Start 2013/11/15, Ricky So, revise the show/hide menu routine
                // if (subMenuItem.Text.Equals("Leave History Enquiry", StringComparison.CurrentCultureIgnoreCase))
                if (subMenuItem.NavigateUrl.EndsWith("/ESS_EmpLeaveHistoryEnquiry.aspx", StringComparison.CurrentCultureIgnoreCase))
                // ***** End 2013/11/15, Ricky So, revise the show/hide menu routine
                    if (!(ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_LEAVE_HISTORY).Equals("N", StringComparison.CurrentCultureIgnoreCase) ? false : true))
                        removeSubMenuItemArray.Add(subMenuItem);


                // ***** Start 2013/11/15, Ricky So, revise the show/hide menu routine
                // if (subMenuItem.Text.Equals("Leave Balance Report", StringComparison.CurrentCultureIgnoreCase))
                if (subMenuItem.NavigateUrl.EndsWith("/ESS_EmpLeaveBalanceReport.aspx", StringComparison.CurrentCultureIgnoreCase))
                // ***** End 2013/11/15, Ricky So, revise the show/hide menu routine
                    if (!(ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_LEAVE_BALANCE_REPORT).Equals("N", StringComparison.CurrentCultureIgnoreCase) ? false : true))
                        removeSubMenuItemArray.Add(subMenuItem);
                    else
                    {
                        if (essUser != null)
                        {
                            ESSAuthorizationProcess authorization = new ESSAuthorizationProcess(dbConn);
                            int groupCount = authorization.GetAuthorizerAuthorizationGroupList(essUser.EmpID).Count;
                            if (groupCount <= 0)
                                removeSubMenuItemArray.Add(subMenuItem);
                        }
                    }

                if (subMenuItem.NavigateUrl.EndsWith("/ESS_EmpLeaveApplicationList.aspx", StringComparison.CurrentCultureIgnoreCase))
                    if (!(ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_LEAVE_APPLICATION_LIST).Equals("N", StringComparison.CurrentCultureIgnoreCase) ? false : true))
                        removeSubMenuItemArray.Add(subMenuItem);

                // ***** Start 2013/11/15, Ricky So, revise the show/hide menu routine
                // if (subMenuItem.Text.Equals("Payslip Printing", StringComparison.CurrentCultureIgnoreCase))
                if (subMenuItem.NavigateUrl.EndsWith("/ESS_EmpPayslipPrint.aspx", StringComparison.CurrentCultureIgnoreCase))
                // ***** End 2013/11/15, Ricky So, revise the show/hide menu routine
                    if (!(ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_PRINT_PAYSLIP).Equals("N", StringComparison.CurrentCultureIgnoreCase) ? false : true))
                        removeSubMenuItemArray.Add(subMenuItem);


                // ***** Start 2013/11/15, Ricky So, revise the show/hide menu routine
                // if (subMenuItem.Text.Equals("Tax Report Printing", StringComparison.CurrentCultureIgnoreCase))
                if (subMenuItem.NavigateUrl.EndsWith("/ESS_EmpTaxReport.aspx", StringComparison.CurrentCultureIgnoreCase))
                // ***** End 2013/11/15, Ricky So, revise the show/hide menu routine
                    if (!(ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_PRINT_TAXREPORT).Equals("N", StringComparison.CurrentCultureIgnoreCase) ? false : true))
                        removeSubMenuItemArray.Add(subMenuItem);

                // ***** Start 2013/11/15, Ricky So, revise the show/hide menu routine
                // if (subMenuItem.Text.Equals("Roster Table", StringComparison.CurrentCultureIgnoreCase))
                if (subMenuItem.NavigateUrl.EndsWith("/ESS_RosterTable_View.aspx", StringComparison.CurrentCultureIgnoreCase))
                // ***** End 2013/11/15, Ricky So, revise the show/hide menu routine
                    if (!productLicense.IsAttendance || !(ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_ROSTER_TABLE).Equals("N", StringComparison.CurrentCultureIgnoreCase) ? false : true))
                        removeSubMenuItemArray.Add(subMenuItem);

                if (subMenuItem.NavigateUrl.EndsWith("/ESS_EmpOverallPaymentSummary.aspx", StringComparison.CurrentCultureIgnoreCase))
                    if (!(ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_OVERALL_PAYMENT_SUMMARY).Equals("N", StringComparison.CurrentCultureIgnoreCase) ? false : true))
                        removeSubMenuItemArray.Add(subMenuItem);

                // Start 0000057, KuangWei, 2014-07-09  
                if (subMenuItem.NavigateUrl.EndsWith("/ESS_MonthlyAttendanceReport.aspx", StringComparison.CurrentCultureIgnoreCase))
                {
                    // Start 000118, Ricky So, 2014/12/24
                    //ESSAuthorizationProcess authorizationProcess = new ESSAuthorizationProcess(dbConn);
                    //int groupCount = authorizationProcess.GetAuthorizerAuthorizationGroupList(user.EmpID).Count;
                    //if (groupCount <= 0)
                    //    removeSubMenuItemArray.Add(subMenuItem);
                    //else
                    // End 000118, Ricky So, 2014/12/24
                        if ((!ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ENABLE_MONTHLY_ATTENDANCE_REPORT).Equals("Y", StringComparison.CurrentCultureIgnoreCase)) ||
                        (!ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_MONTHLY_ATTENDANCE_REPORT).Equals("Y", StringComparison.CurrentCultureIgnoreCase)))
                        removeSubMenuItemArray.Add(subMenuItem);
                }
                // End 0000057, KuangWei, 2014-07-09


                if (subMenuItem.NavigateUrl.EndsWith("/ESS_TimeCardRecord.aspx", StringComparison.CurrentCultureIgnoreCase))
                    if ((!ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ENABLE_TIMECARD_RECORD).Equals("Y", StringComparison.CurrentCultureIgnoreCase)) ||
                        (!ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_TIMECARD_RECORD).Equals("Y", StringComparison.CurrentCultureIgnoreCase)))
                        removeSubMenuItemArray.Add(subMenuItem);

                if (subMenuItem.NavigateUrl.EndsWith("/ESS_Attendance_TimeEntry_List.aspx", StringComparison.CurrentCultureIgnoreCase))
                    if ((!ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ENABLE_ATTENDANCE_TIMEENTRY_LIST).Equals("Y", StringComparison.CurrentCultureIgnoreCase)) ||
                        (!ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_ATTENDANCE_TIMEENTRY_LIST).Equals("Y", StringComparison.CurrentCultureIgnoreCase)))
                        removeSubMenuItemArray.Add(subMenuItem);
                
                // Start 0000076, Miranda, 2014-08-21
                if (subMenuItem.NavigateUrl.EndsWith("/ESS_AttendanceTimeEntryReport.aspx", StringComparison.CurrentCultureIgnoreCase))
                    if ((!ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_ATTENDANCE_TIMEENTRY_REPORT).Equals("Y", StringComparison.CurrentCultureIgnoreCase)))
                        removeSubMenuItemArray.Add(subMenuItem);
                // End 0000076, Miranda, 2014-08-21
                
                removeSubMenuItemArray.AddRange(GetRemoveMenuItemList(subMenuItem.ChildItems));

                // ***** Start 2013/11/15, Ricky So, revise the show/hide menu routine
                // if (subMenuItem.Text.Equals("Approval History", StringComparison.CurrentCultureIgnoreCase))
                if (subMenuItem.NavigateUrl.EndsWith("/ESS_EmpSupervisorApprovalHistory.aspx", StringComparison.CurrentCultureIgnoreCase))
                // ***** End 2013/11/15, Ricky So, revise the show/hide menu routine
                {
                    DBFilter approvalHistoryFilter = new DBFilter();
                    approvalHistoryFilter.add(new Match("EmpRequestApprovalHistoryActionByEmpID", user.EmpID));
                    if (EEmpRequestApprovalHistory.db.count(dbConn,approvalHistoryFilter)<=0)
                        removeSubMenuItemArray.Add(subMenuItem);
                }
                // Start 0000060, Miranda, 2014-07-15
                //if (subMenuItem.NavigateUrl.EndsWith("/ESS_EmpOTClaims.aspx", StringComparison.CurrentCultureIgnoreCase))
                //    if (!(ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_ENABLE_OTCLAIM).Equals("N", StringComparison.CurrentCultureIgnoreCase) ? false : true))
                //        removeSubMenuItemArray.Add(subMenuItem);
                // End 0000060, Miranda, 2014-07-15
                // Start 0000060, Miranda, 2014-07-22
                if (subMenuItem.NavigateUrl.EndsWith("/ESS_EmpOTClaims.aspx", StringComparison.CurrentCultureIgnoreCase))
                    if ((!ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ENABLE_OTCLAIM).Equals("Y", StringComparison.CurrentCultureIgnoreCase)) ||
                        (!ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_OT_CLAIMS).Equals("Y", StringComparison.CurrentCultureIgnoreCase)))
                            removeSubMenuItemArray.Add(subMenuItem);
                if (subMenuItem.NavigateUrl.EndsWith("/ESS_EmpOTClaimsHistory.aspx", StringComparison.CurrentCultureIgnoreCase))
                    if ((!ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ENABLE_OTCLAIM).Equals("Y", StringComparison.CurrentCultureIgnoreCase)) ||
                        (!ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_OT_CLAIMS_HISTORY).Equals("Y", StringComparison.CurrentCultureIgnoreCase)))
                            removeSubMenuItemArray.Add(subMenuItem);
                // End 0000060, Miranda, 2014-07-22
                // Start 0000112, Miranda, 2014-12-10
                if (subMenuItem.NavigateUrl.EndsWith("/ESS_EmpLateWaive.aspx", StringComparison.CurrentCultureIgnoreCase))
                    if ((!ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ENABLE_LATE_WAIVE).Equals("Y", StringComparison.CurrentCultureIgnoreCase)) ||
                        (!ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_LATE_WAIVE).Equals("Y", StringComparison.CurrentCultureIgnoreCase)))
                        removeSubMenuItemArray.Add(subMenuItem);
                if (subMenuItem.NavigateUrl.EndsWith("/ESS_EmpLateWaiveHistory.aspx", StringComparison.CurrentCultureIgnoreCase))
                    if ((!ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ENABLE_LATE_WAIVE).Equals("Y", StringComparison.CurrentCultureIgnoreCase)) ||
                        (!ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_LATE_WAIVE_HISTORY).Equals("Y", StringComparison.CurrentCultureIgnoreCase)))
                        removeSubMenuItemArray.Add(subMenuItem);
                // End 0000112, Miranda, 2014-12-10
            }
            else
                removeSubMenuItemArray.Add(subMenuItem);

        }
        return removeSubMenuItemArray;
    }

}
