using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using HROne.DataAccess;
//using perspectivemind.validation;
using HROne.Lib.Entities;

public partial class Taxation_Generation_Process : HROneWebPage
{

    private const string FUNCTION_CODE = "TAX003";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;

        if (!IsPostBack)
        {
            PageErrors errors = PageErrors.getErrors(ETaxEmp.db, Page.Master);
            errors.clear();

            //string strEmpIDList = DecryptedRequest["EmpID"];
            ArrayList list = (ArrayList)Session["GenerateTax_EmpList"];
            int intTotal;
            if (int.TryParse(DecryptedRequest["Total"], out intTotal) && list != null)
            {

                //string[] strEmpIDListArray;
                //int intProgress = 0;

                //strEmpIDListArray = strEmpIDList.Split(new char[] { '_' });

                //intProgress = 0;
                DateTime dtStartTime = AppUtils.ServerDateTime();

                while (AppUtils.ServerDateTime().Subtract(dtStartTime).Seconds < 10 && list.Count > 0)
                {
                    EEmpPersonalInfo empInfo = (EEmpPersonalInfo)list[0];
                    //int intEmpID;
                    //if (int.TryParse(strEmpIDListArray[intProgress], out intEmpID))
                    //{
                    DBFilter filter = new DBFilter();
                    filter.add(new Match("EmpID", empInfo.EmpID));
                    filter.add(WebUtils.AddRankFilter(Session, "EmpID", true));
                    if (EEmpPersonalInfo.db.count(dbConn, filter) > 0)
                    {
                        WebUtils.StartFunction(Session, FUNCTION_CODE, empInfo.EmpID, true);
                        try
                        {
                            HROne.Taxation.TaxationGeneration.GenerationFormTaxation(dbConn, int.Parse(DecryptedRequest["TaxFormID"]), empInfo.EmpID, WebUtils.GetCurUser(Session).UserID);
                        }
                        catch (Exception ex)
                        {
                            errors.addError(ex.Message);
                            break;
                        }
                        WebUtils.EndFunction(dbConn);
                    }
                    //}
                    //intProgress++;
                    //if (intProgress == strEmpIDListArray.GetLength(0))
                    //    break;
                    list.Remove(empInfo);
                }


                lblProgress.Text = (intTotal - list.Count).ToString() + " of " + intTotal.ToString();
                if (list.Count == 0)
                {
                    Session.Remove("GenerateTax_EmpList");
                    ETaxForm taxForm = new ETaxForm();
                    taxForm.TaxFormID = int.Parse(DecryptedRequest["TaxFormID"]);
                    if (ETaxForm.db.select(dbConn, taxForm))
                    {
                        if (taxForm.TaxFormType.Equals("B", StringComparison.CurrentCultureIgnoreCase))
                            HROne.Taxation.TaxationGeneration.RearrangeSheetNo(dbConn, taxForm.TaxFormID);
                        // Start 0000020, KuangWei, 2014-07-22
                        if (taxForm.TaxFormType.Equals("M", StringComparison.CurrentCultureIgnoreCase))
                            HROne.Taxation.TaxationGeneration.RearrangeSheetNoForM(dbConn, taxForm.TaxFormID);
                        // End 0000020, KuangWei, 2014-07-22

                        lblProgressMessage.Text = HROne.Common.WebUtility.GetLocalizedString("Taxation records are generated successfully");
                    }
                }
                else
                {
                    if (errors.isEmpty())
                    {
                        Session["GenerateTax_EmpList"] = list;

                        ////string strRemainder = string.Join("_", strEmpIDListArray, intProgress, strEmpIDListArray.GetLength(0) - intProgress);
                        //string strRedirectScript = string.Empty;
                        //strRedirectScript += @"<script language='javascript'>" + "\n";
                        //strRedirectScript += @"function Redirect()" + "\n";
                        //strRedirectScript += @"{" + "\n";
                        //strRedirectScript += "window.location = \"" + HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Taxation_Generation_Process.aspx?"
                        //    + "TaxFormID=" + DecryptedRequest["TaxFormID"]
                        //    + "&Total=" + intTotal
                        //    //+ "&EmpID=" + strRemainder 
                        //    )
                        //    + "\";";
                        //strRedirectScript += @"}" + "\n";
                        //strRedirectScript += @"setTimeout('Redirect()',500);" + "\n";
                        //strRedirectScript += @"</script>" + "\n";
                        //Response.Write(strRedirectScript);

                        string url = "Taxation_Generation_Process.aspx?"
                            + "TaxFormID=" + DecryptedRequest["TaxFormID"]
                            + "&Total=" + intTotal
                            //+ "&EmpID=" + strRemainder 
                        ;
                        WebUtils.RegisterRedirectJavaScript(Page, url, 500);
                    }
                    else
                    {
                    }
                }
            }
        }
        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

    }
}
