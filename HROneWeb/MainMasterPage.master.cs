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
//using perspectivemind.validation;

public partial class MainMasterPage : HROneWebMasterPage
{

    HROne.Lib.Entities.EUser user = null;
    bool hasMessage = true;
    protected void Page_Init(object sender, EventArgs e)
    {
        Session["LastURL"] = Request.Url.PathAndQuery;

        user = WebUtils.GetCurUser(Session);
        //WebUtils.SetSessionLanguage(Session, user);

        lblVersionNo.Text = ProductVersion.CURRENT_PROGRAM_VERSION;

        HROne.ProductLicense productLicense = WebUtils.productLicense(Session);

        if (productLicense != null && productLicense.ProductType == HROne.ProductLicense.ProductLicenseType.iMGR)
        {
            Page.Title = "iMGR";
            lblCopyrightCompanyName.Text = "iGlobe Technology Limited";
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        string dbTitle = HROne.Lib.Entities.ESystemParameter.getParameter(dbConn, HROne.Lib.Entities.ESystemParameter.PARAM_CODE_DB_TITLE);
        if (!string.IsNullOrEmpty(dbTitle))
            Page.Title += " (" + dbTitle + ")";

        if (WebUtils.IsSuperUserMissing(dbConn) && (Session["IgnoreEM"] == null || ((bool)Session["IgnoreEM"]) != true))
        {
            if (!IsPostBack)
                Page.Title += " (Emergency Mode)";
        }

        if (user != null)
        {
            if (!string.IsNullOrEmpty(user.UserName))
                UserName.Text = "[" + user.UserName + "]";
            popupWinTimer.Enabled = user.UserIsKeepConnected;
        }

        popupWinTimer.Interval = Session.Timeout * 30 * 1000;
        this.form1.Attributes.Add("AUTOCOMPLETE", "OFF");

        ////String scriptString = "setTimeout(function(){ messagePopupDetail=\"" + @"Session timeout" + "\";showModalPopupMessage();}," + ((long)(Session.Timeout * 60 * 1000)).ToString() + ");";
        ////ScriptManager.RegisterStartupScript(Page, Page.GetType(), "timeout", scriptString, true);

        HROne.Common.WebUtility.AddBrowserCompatibilityMeta(Page);
    }

    protected override void AddedControl(Control control, int index)
    {
        if (Request.UserAgent.IndexOf("AppleWebKit") >= 0)
            //    this.Page.ClientTarget = "uplevel";
            Request.Browser.Adapters.Clear();
        base.AddedControl(control, index);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        Page page = this.Page;// (Page)HttpContext.Current.Handler;
        if (page != null)
        {

            //// Remove all white spacing from the DOM layout of the dynamic table
            //ScriptManager.RegisterStartupScript(page, page.GetType(), "ie9TableFix",
            //    @"var expr = new RegExp('>[ \t\r\n\v\f]*<', 'g');" + "\r\n" + @"document.body.innerHTML = document.body.innerHTML.replace(expr, '><');"
            //, true);

            if (!this.pageError.getErrors().isEmpty())
            //if (this.pageError.getErrors().errors.Count > 0)
            {
                Dictionary<string, Error> errorTable = this.pageError.getErrors().errors;

                foreach (string key in errorTable.Keys)
                {
                    object newValue = errorTable[key];
                    if (newValue is Error)
                    {
                        ((Error)newValue).name = HROne.Common.WebUtility.GetLocalizedString(((Error)newValue).name);
                        //object[] parameters = ((Error)newValue).parameters;
                        //for (int i = 0; i < parameters.GetLength(0); i++)
                        //{
                        //    if (parameters[i] is string)
                        //        parameters[i] = HROne.Common.WebUtility.GetLocalizedString(((string)parameters[i]));
                        //}
                    }
                }
                string errorMessage = pageError.getErrors().getPrompt();
                errorMessage = errorMessage.Replace(HROne.Common.WebUtility.GetLocalizedString("validate.prompt"), "");
                errorMessage = errorMessage.Replace("\\", "\\\\").Replace("\r", "\\r").Replace("\n", "\\n").Replace("\"", "\\\"");
                //ScriptManager.RegisterStartupScript(page, page.GetType(), "errorMessage", "alert(\"" + errorMessage + "\");", true);

                string[] errorMessageLineArray = errorMessage.Split(new string[]{"\\r\\n"},StringSplitOptions.None );
                if (errorMessageLineArray.Length > 20)
                {
                    errorMessage = string.Empty;
                    for (int idx = errorMessageLineArray.GetLowerBound(0); idx < errorMessageLineArray.GetLowerBound(0) + 20; idx++)
                        if (string.IsNullOrEmpty(errorMessage))
                            errorMessage = errorMessageLineArray[idx];
                        else
                            errorMessage += "\\r\\n" + errorMessageLineArray[idx];
                    errorMessage += "\\r\\n" + "... and more";
                }

                String scriptString = "messagePopupDetail=\"" + errorMessage + "\";";
                ScriptManager.RegisterStartupScript(page, page.GetType(), "errorMessage", scriptString, true);
                //lblMessage.Text = errorMessage;
                //lnkDelete_ModalPopupExtender.Show();
            }
            string inboxMessage = WebUtils.GetLatestPopupMessage(Session);
            if (!string.IsNullOrEmpty(inboxMessage))
            {
                //popupAnchor.PopupToShow = "popupWin";

                // Popup win is visible ..
                //popupWin.Visible = true;
                // .. and will be displayed when page is loaded
                //popupWin.AutoShow = true;
                popupWin.Title = Page.Title;
                popupWin.Message = "<br/>" + inboxMessage;
                popupWin.Link = "Inbox_List.aspx";
                popupWin.LinkTarget = "_self";
                popupWin.ActionType = EeekSoft.Web.PopupAction.OpenLink;
                hasMessage = true;
            }
            else
            {
                hasMessage = false;
                //popupWin.AutoShow = false;
            }
            if (hasMessage)
            {
                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.UpdatePanel1.GetType(), "msg", popupWin.ClientID + "espopup_winLoad();", true);
                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.UpdatePanel1.GetType(), "msg",  popupWin.ClientID + "resetTimer=true;\r\n" + popupWin.ClientID + "espopup_ShowPopup(null);", true);
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.UpdatePanel1.GetType(), "msg", "window.onload=null;" + popupWin.ClientID + "espopup_winLoad();\r\n" + popupWin.ClientID + "resetTimer=true;\r\n" + popupWin.ClientID + "espopup_ShowPopup(null);", true);
            }
            ScriptManager.RegisterStartupScript(page, page.GetType(), "showMessage", "setTimeout(\"showModalPopupMessage()\",500);", true);

            //  Menubar hover replaced by onclick
            page.ClientScript.RegisterStartupScript
            (page.GetType(), "addClickBehavior",
            "addClickBehavior(document.getElementById('" +
            UpdatePanel2.ClientID + "'));", true);
        }
    }

    protected void mainMenu_MenuItemDataBound(object sender, MenuEventArgs e)
    {
        string target = ((SiteMapNode)e.Item.DataItem)["target"];
        if (target != null && target.Length > 0)
            e.Item.Target = target;
    }
    protected void mainMenu_PreRender(object sender, EventArgs e)
    {
        if (IsPostBack)
            mainMenu.DataBind();
        ArrayList removeSubMenuItemArray = GetRemoveMenuItemList(mainMenu.Items);
        //foreach (MenuItem subMenuItem in mainMenu.Items)
        //{
        //    if (subMenuItem.Text.Equals("Cost Allocation", StringComparison.CurrentCultureIgnoreCase))
        //        if (!productLicense.IsCostCenter)
        //            removeSubMenuItemArray.Add(subMenuItem);
        //    if (subMenuItem.Text.Equals("Attendance", StringComparison.CurrentCultureIgnoreCase))
        //        if (!productLicense.IsAttendance)
        //            removeSubMenuItemArray.Add(subMenuItem);
        //    if (subMenuItem.Text.Equals("ESS Authorizer Setup", StringComparison.CurrentCultureIgnoreCase))
        //        if (!productLicense.IsESS)
        //            removeSubMenuItemArray.Add(subMenuItem);
            
        //}
        foreach (MenuItem subMenuItem in removeSubMenuItemArray)
        {
            if (subMenuItem.Parent == null)
                mainMenu.Items.Remove(subMenuItem);
            else
                subMenuItem.Parent.ChildItems.Remove(subMenuItem);
        }

        HROne.Common.WebUtility.MenuLocalization(mainMenu);
    }

    private ArrayList GetRemoveMenuItemList(MenuItemCollection menuItemCollection)
    {
        HROne.ProductLicense productLicense = WebUtils.productLicense(Session);
        ArrayList removeSubMenuItemArray = new ArrayList();
        foreach (MenuItem subMenuItem in menuItemCollection)
        {
            if (WebUtils.IsEMUser(user))
            {
                if (!subMenuItem.Text.Equals("Security", StringComparison.CurrentCultureIgnoreCase))
                {
                    removeSubMenuItemArray.Add(subMenuItem);
                }
                continue;
            }
            if (productLicense != null)
            {
                if (subMenuItem.Text.Equals("Code Setup", StringComparison.CurrentCultureIgnoreCase)
                || subMenuItem.Text.Equals("Report", StringComparison.CurrentCultureIgnoreCase)
                    )
                    if (!productLicense.IsLeaveManagement && !productLicense.IsPayroll && !productLicense.IsTaxation)
                    {
                        removeSubMenuItemArray.Add(subMenuItem);
                        continue;
                    } 
                if (subMenuItem.Text.Equals("Employee", StringComparison.CurrentCultureIgnoreCase)
                || subMenuItem.Text.Equals("Leave", StringComparison.CurrentCultureIgnoreCase) 
                    )
                    if (!productLicense.IsLeaveManagement)
                    {
                        removeSubMenuItemArray.Add(subMenuItem);
                        continue;
                    }
                if (subMenuItem.Text.Equals("Payroll", StringComparison.CurrentCultureIgnoreCase)
                || subMenuItem.Text.Equals("MPF Setup", StringComparison.CurrentCultureIgnoreCase)
                    )
                    if (!productLicense.IsPayroll)
                    {
                        removeSubMenuItemArray.Add(subMenuItem);
                        continue;
                    }
                if (subMenuItem.Text.Equals("Taxation", StringComparison.CurrentCultureIgnoreCase))
                    if (!productLicense.IsTaxation)
                    {
                        removeSubMenuItemArray.Add(subMenuItem);
                        continue;
                    }
                if (subMenuItem.Text.IndexOf("P-Fund ", StringComparison.CurrentCultureIgnoreCase) >= 0
                    || subMenuItem.Text.IndexOf("ORSO ", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    if (!HROne.Lib.Entities.ESystemParameter.getParameter(dbConn, HROne.Lib.Entities.ESystemParameter.PARAM_CODE_USE_ORSO).Equals("Y", StringComparison.CurrentCultureIgnoreCase))
                    {
                        removeSubMenuItemArray.Add(subMenuItem);
                        continue;
                    }
                }

                if (subMenuItem.Text.IndexOf("Commission Achievement Process", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    if (!HROne.Lib.Entities.ESystemParameter.getParameter(dbConn, HROne.Lib.Entities.ESystemParameter.PARAM_CODE_MONTHLY_ACHIEVEMENT_COMMISSION).Equals("Y", StringComparison.CurrentCultureIgnoreCase))
                    {
                        removeSubMenuItemArray.Add(subMenuItem);
                        continue;
                    }
                }
                if (subMenuItem.Text.IndexOf("Incentive Payment Process", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    if (!HROne.Lib.Entities.ESystemParameter.getParameter(dbConn, HROne.Lib.Entities.ESystemParameter.PARAM_CODE_INCENTIVE_PAYMENT).Equals("Y", StringComparison.CurrentCultureIgnoreCase))
                    {
                        removeSubMenuItemArray.Add(subMenuItem);
                        continue;
                    }
                }
                if (subMenuItem.Text.IndexOf("Double Pay Adjustment", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    if (!HROne.Lib.Entities.ESystemParameter.getParameter(dbConn, HROne.Lib.Entities.ESystemParameter.PARAM_CODE_ENABLE_DOUBLE_PAY_ADJUSTMENT).Equals("Y", StringComparison.CurrentCultureIgnoreCase))
                    {
                        removeSubMenuItemArray.Add(subMenuItem);
                        continue;
                    }
                }
                if (subMenuItem.Text.IndexOf("Bonus Process", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    if (!HROne.Lib.Entities.ESystemParameter.getParameter(dbConn, HROne.Lib.Entities.ESystemParameter.PARAM_CODE_ENABLE_BONUS_PROCESS).Equals("Y", StringComparison.CurrentCultureIgnoreCase))
                    {
                        removeSubMenuItemArray.Add(subMenuItem);
                        continue;
                    }
                }
                if (subMenuItem.Text.IndexOf("E-mail ", StringComparison.CurrentCultureIgnoreCase) >= 0
                    || subMenuItem.Text.IndexOf("Email ", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    if (productLicense.ProductType != HROne.ProductLicense.ProductLicenseType.HROne)
                    {
                        removeSubMenuItemArray.Add(subMenuItem);
                        continue;
                    }
                }
                if (subMenuItem.Text.Equals("Cost Allocation", StringComparison.CurrentCultureIgnoreCase))
                    if (!productLicense.IsCostCenter)
                    {
                        removeSubMenuItemArray.Add(subMenuItem);
                        continue;
                    }
                if (subMenuItem.Text.Equals("Attendance", StringComparison.CurrentCultureIgnoreCase))
                    if (!productLicense.IsAttendance)
                    {
                        removeSubMenuItemArray.Add(subMenuItem);
                        continue;
                    }
                if (subMenuItem.Text.Equals("ESS Authorization Group Setup", StringComparison.CurrentCultureIgnoreCase))
                    if (!productLicense.IsESS)
                    {
                        removeSubMenuItemArray.Add(subMenuItem);
                        continue;
                    }
                if (subMenuItem.Text.Equals("ESS Authorization Workflow Setup", StringComparison.CurrentCultureIgnoreCase))
                    if (!productLicense.IsESS)
                    {
                        removeSubMenuItemArray.Add(subMenuItem);
                        continue;
                    }
                if (subMenuItem.Text.Equals("Release Pay Slip to ESS", StringComparison.CurrentCultureIgnoreCase))
                    if (!productLicense.IsESS)
                    {
                        removeSubMenuItemArray.Add(subMenuItem);
                        continue;
                    }
                if (subMenuItem.Text.Equals("Release Tax Report to ESS", StringComparison.CurrentCultureIgnoreCase))
                    if (!productLicense.IsESS)
                    {
                        removeSubMenuItemArray.Add(subMenuItem);
                        continue;
                    }
                if (subMenuItem.Text.Equals("Training", StringComparison.CurrentCultureIgnoreCase))
                    if (!productLicense.IsTraining)
                    {
                        removeSubMenuItemArray.Add(subMenuItem);
                        continue;
                    }
                if (subMenuItem.Text.Equals("e-channel", StringComparison.CurrentCultureIgnoreCase))
                    if (productLicense.ProductType != HROne.ProductLicense.ProductLicenseType.HROneSaaS || Session["CompanyDBID"] == null)
                    {
                        removeSubMenuItemArray.Add(subMenuItem);
                        continue;
                    }
                //if (subMenuItem.Text.Equals("Work Hour Pattern Setup", StringComparison.CurrentCultureIgnoreCase))
                //    if (productLicense.ProductType != HROne.ProductLicense.ProductLicenseType.HROne)
                //    {
                //        removeSubMenuItemArray.Add(subMenuItem);
                //        continue;
                //    }
                if (subMenuItem.Text.Equals("Annual Leave Prorata Rounding Rule Setup", StringComparison.CurrentCultureIgnoreCase))
                    if (productLicense.ProductType != HROne.ProductLicense.ProductLicenseType.HROne)
                    {
                        removeSubMenuItemArray.Add(subMenuItem);
                        continue;
                    }
                if (subMenuItem.Text.Equals("Import Leave Balance Adjustment", StringComparison.CurrentCultureIgnoreCase))
                    if (productLicense.ProductType != HROne.ProductLicense.ProductLicenseType.HROne)
                    {
                        removeSubMenuItemArray.Add(subMenuItem);
                        continue;
                    }
                if (subMenuItem.Text.Equals("MPF Termination Code Mapping", StringComparison.CurrentCultureIgnoreCase))
                    if (productLicense.ProductType != HROne.ProductLicense.ProductLicenseType.HROne)
                    {
                        removeSubMenuItemArray.Add(subMenuItem);
                        continue;
                    }
                if (subMenuItem.Text.Equals("Statutory Minimum Wage Setup", StringComparison.CurrentCultureIgnoreCase))
                    if (productLicense.ProductType != HROne.ProductLicense.ProductLicenseType.HROne)
                    {
                        removeSubMenuItemArray.Add(subMenuItem);
                        continue;
                    }
                //if (subMenuItem.Text.Equals("User Guide", StringComparison.CurrentCultureIgnoreCase))
                //{
                //    if (productLicense.ProductType != HROne.ProductLicense.ProductLicenseType.iMGR)
                //    {
                //        removeSubMenuItemArray.Add(subMenuItem);
                //        continue;
                //    }
                //}
                if (subMenuItem.Text.Equals("Import Employee Information", StringComparison.CurrentCultureIgnoreCase))
                {
                    HROne.Lib.Entities.ESystemFunction function = HROne.Lib.Entities.ESystemFunction.GetObjectByCode(dbConn, "PER999");
                    if (function != null)
                    {
                        if (function.FunctionIsHidden)
                        {
                            removeSubMenuItemArray.Add(subMenuItem);
                            continue;
                        }
                    }
                    else
                    {
                        removeSubMenuItemArray.Add(subMenuItem);
                        continue;
                    }
                }

                if (subMenuItem.Text.Equals("Shift Duty Code", StringComparison.CurrentCultureIgnoreCase))
                {
                    ESystemFunction m_function = ESystemFunction.GetObjectByCode(dbConn, "SYS025");
                    if (m_function == null || m_function.FunctionIsHidden)
                        removeSubMenuItemArray.Add(subMenuItem);
                }
                if (subMenuItem.Text.Equals("Payment Calculation Formula", StringComparison.CurrentCultureIgnoreCase))
                {
                    ESystemFunction m_function = ESystemFunction.GetObjectByCode(dbConn, "SYS026");
                    if (m_function == null || m_function.FunctionIsHidden)
                        removeSubMenuItemArray.Add(subMenuItem);
                }
                if (subMenuItem.Text.Equals("Report Builder", StringComparison.CurrentCultureIgnoreCase))
                {
                    ESystemFunction m_function = ESystemFunction.GetObjectByCode(dbConn, "RPT004");
                    if (m_function == null || m_function.FunctionIsHidden)
                        removeSubMenuItemArray.Add(subMenuItem);
                }
                if (subMenuItem.Text.Equals("Customized Attendance Preparation Process", StringComparison.CurrentCultureIgnoreCase))
                {
                    ESystemFunction m_function = ESystemFunction.GetObjectByCode(dbConn, "CUSTOM003");
                    if (m_function == null || m_function.FunctionIsHidden)
                        removeSubMenuItemArray.Add(subMenuItem);
                }

                    //{
                //    HROne.Lib.Entities.ESystemFunction function = HROne.Lib.Entities.ESystemFunction.GetObjectByCode(dbConn, "CUSTOM003");
                //    if (function != null)
                //    {
                //        if (function.FunctionIsHidden)
                //        {
                //            removeSubMenuItemArray.Add(subMenuItem);
                //            continue;
                //        }
                //    }
                //    else
                //    {
                //        removeSubMenuItemArray.Add(subMenuItem);
                //        continue;
                //    }
                //}                
                removeSubMenuItemArray.AddRange(GetRemoveMenuItemList(subMenuItem.ChildItems));
            }
            else
                if (!subMenuItem.Text.Equals("Security", StringComparison.CurrentCultureIgnoreCase))
                    removeSubMenuItemArray.Add(subMenuItem);


        }
        return removeSubMenuItemArray;
    }
    protected void popupWin_LinkClicked(object sender, EventArgs e)
    {

    }
    protected void popupWinTimer_Tick(object sender, EventArgs e)
    {
        //string message = WebUtils.GetLatestPopupMessage(Session);
        //if (!string.IsNullOrEmpty(message))
        //{
        //    //popupAnchor.PopupToShow = "popupWin";

        //    // Popup win is visible ..
        //    //popupWin.Visible = true;
        //    // .. and will be displayed when page is loaded
        //    //popupWin.AutoShow = true;
        //    popupWin.Title = "HROne";
        //    popupWin.Message = "<br/>" + message;
        //    popupWin.Link = "Inbox_List.aspx";
        //    popupWin.LinkTarget = "_self";
        //    popupWin.ActionType = EeekSoft.Web.PopupAction.OpenLink;
        //}
    }


}
