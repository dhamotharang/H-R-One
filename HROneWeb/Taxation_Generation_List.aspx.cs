using System;
using System.Collections;
using System.Collections.Generic;
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


public partial class Taxation_Generation_List : HROneWebPage
{
    private const string FUNCTION_CODE = "TAX003";
    protected SearchBinding sbinding;
    public DBManager db = EEmpPersonalInfo.db;
//    public EPayrollGroup obj;
    public int CurID = -1;

    protected ListInfo info;
    protected DataView view;

    protected ListInfo TaxGeneratedInfo;
    protected DataView TaxGeneratedView;

    private bool IsAllowEdit = true;

    public class WFTaxYearList : WFValueList 
    {
        private int maxYear;
        public WFTaxYearList(int maxYear)
        {
            this.maxYear = maxYear;
        }
        public List<WFSelectValue> getValues(DatabaseConnection DBAccess, DBFilter filter, System.Globalization.CultureInfo ci)
        {
            //int maxYear= AppUtils.ServerDateTime().Date.AddMonths(9).Year ;
            List<WFSelectValue> arrayList = new List<WFSelectValue>();
            for (int i = maxYear; i >= 2001; i--)
                arrayList.Add(new WFSelectValue(Convert.ToString(i), (i - 1) + " - " + i));
            return arrayList;
        }
    }

    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        if (!WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
        {
            IsAllowEdit = false;
        }
        TaxGenEmpSelectAllPanel.Visible = true;



        sbinding = new SearchBinding(dbConn, db);
        sbinding.add(new DropDownVLSearchBinder( TaxCompID,"TaxCompID", ETaxCompany.VLTaxCompany).setLocale(HROne.Common.WebUtility.GetSessionUICultureInfo(Session)));
        sbinding.add(new DropDownVLSearchBinder(TaxFormType, "TaxFormType", ETaxForm.VLTaxFormType, false));


        if (TaxFormType.SelectedValue.Equals("B", StringComparison.CurrentCultureIgnoreCase))
            sbinding.add(new DropDownVLSearchBinder(YearSelect, "TaxFormYear", new WFTaxYearList(AppUtils.ServerDateTime().Date.AddMonths(-2).Year), false));
        //sbinding.add(new DropDownVLSearchBinder(YearSelect, "", new WFYearList(AppUtils.ServerDateTime().Date.Year - 2001, 0)));
        else
            sbinding.add(new DropDownVLSearchBinder(YearSelect, "TaxFormYear", new WFTaxYearList(AppUtils.ServerDateTime().Date.AddMonths(9).Year), false));
        //sbinding.add(new DropDownVLSearchBinder(YearSelect, "", new WFYearList(AppUtils.ServerDateTime().Date.Year - 2001, 1)));

        sbinding.init(DecryptedRequest, null);

        if (!string.IsNullOrEmpty(TaxCompID.SelectedValue))
        {
            DBFilter taxFormFilter = sbinding.createFilter();// new DBFilter();
            //taxFormFilter.add(new Match("TaxCompID", int.Parse(TaxCompID.SelectedValue)));
            //taxFormFilter.add(new Match("TaxFormYear", int.Parse(YearSelect.SelectedValue)));
            //taxFormFilter.add(new Match("TaxFormType", TaxFormType.SelectedValue));
            ArrayList taxForms = ETaxForm.db.select(dbConn, taxFormFilter);
            if (taxForms.Count > 0)
                CurID = ((ETaxForm)taxForms[0]).TaxFormID;
        }

        info = ListFooter.ListInfo;
        TaxGeneratedInfo = TaxGenerated_ListFooter.ListInfo;
        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //YearSelect.SelectedValue = AppUtils.ServerDateTime().Year.ToString();
            if (CurID > 0)
            {
                //loadObject();
                view = loadData(info, db, Repeater, false);
                TaxGeneratedView = loadData(TaxGeneratedInfo, db, TaxGenerated_Repeater, true);
                panelEmpList.Visible=true;
            }
            else
                panelEmpList.Visible = false;

        }
        else
            panelEmpList.Visible = true;


    }

    protected void TaxCompID_SelectedIndexChanged(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater, false);
        TaxGeneratedView = loadData(TaxGeneratedInfo, db, TaxGenerated_Repeater, true);
        //        Response.Redirect(Request.Url.LocalPath + "?TaxCompID=" + TaxCompID.SelectedValue + "&Year=" + YearSelect.SelectedValue);
    }

    protected void TaxFormType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (TaxFormType.SelectedValue.Equals("F", StringComparison.CurrentCultureIgnoreCase) || TaxFormType.SelectedValue.Equals("G", StringComparison.CurrentCultureIgnoreCase))
        {
            ListFooter.ListOrderBy = "EmpTermLastDate";
            ListFooter.ListOrder = false;
            TaxGenerated_ListFooter.ListOrderBy = "EmpTermLastDate";
            TaxGenerated_ListFooter.ListOrder = false; 
        }
        else if (TaxFormType.SelectedValue.Equals("E", StringComparison.CurrentCultureIgnoreCase))
        {
            ListFooter.ListOrderBy = "EmpDateOfJoin";
            ListFooter.ListOrder = false;
            TaxGenerated_ListFooter.ListOrderBy = "EmpDateOfJoin";
            TaxGenerated_ListFooter.ListOrder = false;
        }
        else
        {
            ListFooter.ListOrderBy = string.Empty;
            ListFooter.ListOrder = true;
            TaxGenerated_ListFooter.ListOrderBy = string.Empty;
            TaxGenerated_ListFooter.ListOrder = true;
        }
        view = loadData(info, db, Repeater, false);
        TaxGeneratedView = loadData(TaxGeneratedInfo, db, TaxGenerated_Repeater, true);
        //        Response.Redirect(Request.Url.LocalPath + "?TaxCompID=" + TaxCompID.SelectedValue + "&Year=" + YearSelect.SelectedValue);
    }

    protected void YearSelect_SelectedIndexChanged(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater, false);
        TaxGeneratedView = loadData(TaxGeneratedInfo, db, TaxGenerated_Repeater, true);
        //        Response.Redirect(Request.Url.LocalPath + "?TaxCompID=" + TaxCompID.SelectedValue + "&Year=" + YearSelect.SelectedValue);
    }
 
    public DataView loadData(ListInfo info, DBManager db, Repeater repeater, bool IsGenerated)
    {
        if (!string.IsNullOrEmpty(TaxCompID.SelectedValue))
        {
            //if (YearSelect.SelectedIndex == 0) YearSelect.SelectedIndex = YearSelect.Items.Count - 1;
            DateTime lastTaxDate = new DateTime(int.Parse(YearSelect.SelectedValue), 3, 31);
            DateTime firstTaxDate = new DateTime(int.Parse(YearSelect.SelectedValue) - 1, 4, 1);

            DBFilter filter = new DBFilter();//sbinding.createFilter();

            //if (info != null && info.orderby != null && !info.orderby.Equals(""))
            //    filter.add(info.orderby, info.order);

            string select = "e.*,  et.EmpTermLastDate ";
            string from = "from [" + db.dbclass.tableName + "] e LEFT JOIN [" + EEmpTermination.db.dbclass.tableName + "] et ON et.EmpID=e.EmpID ";

            if (IsGenerated)
            {
                select += ", te.TaxEmpID, te.TaxEmpGeneratedDate, te.TaxEmpGeneratedByUserID ";
                from += " INNER JOIN [" + ETaxEmp.db.dbclass.tableName + "] te ON e.EmpID=te.EmpID AND te.TaxFormID=" + CurID.ToString() + " ";
            }
            else
            {
                DBFilter taxEmpFilter = new DBFilter();
                //Start 0000205, Miranda, 2015-5-30
                //taxEmpFilter.add(new Match("TaxFormID", CurID));
                int[] arr = new int[2]  ;
                arr[0] = CurID;
                if (TaxFormType.SelectedValue.Equals("F", StringComparison.CurrentCultureIgnoreCase))
                {
                    arr[1] = getFormIDByType("G");
                }
                else if (TaxFormType.SelectedValue.Equals("G", StringComparison.CurrentCultureIgnoreCase))
                {
                    arr[1] = getFormIDByType("F");
                }
                taxEmpFilter.add(new IN("TaxFormID", arr));
                //taxEmpFilter.add(new Match("TaxFormID", CurID));
                //End 0000205, Miranda, 2015-5-30
                taxEmpFilter.add(new MatchField("te.EmpID", "e.EmpID"));
                filter.add(new Exists(ETaxEmp.db.dbclass.tableName + " te", taxEmpFilter, true));
            }
            if (TaxFormType.SelectedValue.Equals("B", StringComparison.CurrentCultureIgnoreCase))
            {
                OR joinDateInTerms = new OR();
                joinDateInTerms.add(new Match("e.EmpDateOfJoin", "<=", lastTaxDate));
                joinDateInTerms.add(new NullTerm("e.EmpDateOfJoin"));
                filter.add(joinDateInTerms);

                OR termDateTerms = new OR();
                termDateTerms.add(new IN("not e.empid", "Select et.empid from EmpTermination et ", new DBFilter()));
                DBFilter termDateFilter = new DBFilter();
                termDateFilter.add(new Match("et.EmpTermLastDate", ">", lastTaxDate));
                termDateTerms.add(new IN("e.empid", "Select et.empid from EmpTermination et ", termDateFilter));
                filter.add(termDateTerms);

                DBFilter positionFilter = new DBFilter();
                positionFilter.add(new Match("ep.EmpPosEffFr", "<=", lastTaxDate));
                OR positonEffToTerms = new OR();
                positonEffToTerms.add(new Match("ep.EmpPosEffTo", ">=", lastTaxDate));
                positonEffToTerms.add(new NullTerm("ep.EmpPosEffTo"));
                positionFilter.add(positonEffToTerms);
                DBFilter taxCompFilter = new DBFilter();
                taxCompFilter.add(new Match("tcm.TaxCompID", TaxCompID.SelectedIndex == 0 ? "0" : TaxCompID.SelectedValue));
                positionFilter.add(new IN("ep.CompanyID", "Select tcm.CompanyID from TaxCompanyMap tcm", taxCompFilter));
                filter.add(new IN("e.EmpID", "Select ep.EmpID from EmpPositionInfo ep", positionFilter));
            }
            else if (TaxFormType.SelectedValue.Equals("F", StringComparison.CurrentCultureIgnoreCase) || TaxFormType.SelectedValue.Equals("G", StringComparison.CurrentCultureIgnoreCase))
            {
                OR joinDateInTerms = new OR();
                joinDateInTerms.add(new Match("e.EmpDateOfJoin", "<=", lastTaxDate));
                joinDateInTerms.add(new NullTerm("e.EmpDateOfJoin"));
                filter.add(joinDateInTerms);

                DBFilter termDateFilter = new DBFilter();
                termDateFilter.add(new Match("et.EmpTermLastDate", "<=", lastTaxDate));
                termDateFilter.add(new Match("et.EmpTermLastDate", ">=", firstTaxDate));
                filter.add(new IN("e.empid", "Select et.empid from EmpTermination et ", termDateFilter));

                DBFilter positionFilter = new DBFilter();
                positionFilter.add(new Match("ep.EmpPosEffFr", "<=", lastTaxDate));
                OR positonEffToTerms = new OR();
                positonEffToTerms.add(new Match("ep.EmpPosEffTo", ">", lastTaxDate));
                positonEffToTerms.add(new NullTerm("ep.EmpPosEffTo"));
                positionFilter.add(positonEffToTerms);
                DBFilter taxCompFilter = new DBFilter();
                taxCompFilter.add(new Match("tcm.TaxCompID", TaxCompID.SelectedIndex == 0 ? "0" : TaxCompID.SelectedValue));
                positionFilter.add(new IN("ep.CompanyID", "Select tcm.CompanyID from TaxCompanyMap tcm", taxCompFilter));
                filter.add(new IN("e.EmpID", "Select ep.EmpID from EmpPositionInfo ep", positionFilter));

            }
            else if (TaxFormType.SelectedValue.Equals("E", StringComparison.CurrentCultureIgnoreCase))
            {
                filter.add(new Match("e.EmpDateOfJoin", ">=", firstTaxDate));
                filter.add(new Match("e.EmpDateOfJoin", "<=", lastTaxDate));

                DBFilter positionFilter = new DBFilter();
                positionFilter.add(new Match("ep.EmpPosEffFr", "<=", lastTaxDate));
                OR positonEffToTerms = new OR();
                positonEffToTerms.add(new Match("ep.EmpPosEffTo", ">", firstTaxDate));
                positonEffToTerms.add(new NullTerm("ep.EmpPosEffTo"));
                positionFilter.add(positonEffToTerms);
                DBFilter taxCompFilter = new DBFilter();
                taxCompFilter.add(new Match("tcm.TaxCompID", TaxCompID.SelectedIndex == 0 ? "0" : TaxCompID.SelectedValue));
                positionFilter.add(new IN("ep.CompanyID", "Select tcm.CompanyID from TaxCompanyMap tcm", taxCompFilter));
                filter.add(new IN("e.EmpID", "Select ep.EmpID from EmpPositionInfo ep", positionFilter));
            }
            else if (TaxFormType.SelectedValue.Equals("M", StringComparison.CurrentCultureIgnoreCase))
            {
                filter.add(new Match("et.EmpTermLastDate", ">=", firstTaxDate));
                filter.add(new Match("et.EmpTermLastDate", "<=", lastTaxDate));
            }

            DBFilter empInfoFilter = EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime(), AppUtils.ServerDateTime());
            empInfoFilter.add(new MatchField("e.EmpID", "ee.EmpID"));
            filter.add(new Exists(EEmpPersonalInfo.db.dbclass.tableName + " ee", empInfoFilter));

            DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);
            table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);

            //  Remove condition for record count = 0
            //  Since there are 2 employee list, the system cannot detect whether all list are empty.
            //if (table.Rows.Count != 0)
            btnGenerate.Visible = true & IsAllowEdit;
            //else
            //    btnGenerate.Visible = false;

            DataView view = new DataView(table);
            //if (info != null)
            //{
            //    info.loadPageList(null, PrevPage, NextPage, FirstPage, LastPage);

            //    CurPage.Value = info.page.ToString();
            //    NumPage.Value = info.numPage.ToString();
            //}
            if (repeater != null)
            {
                repeater.DataSource = view;
                repeater.DataBind();
            }


            return view;
        }
        else
        {
            repeater.DataSource = null;
            repeater.DataBind();
            return null;
        }
    }
    //public DataView TaxGenerated_LoadData(ListInfo info, DBManager db, Repeater repeater)
    //{
    //    if (!string.IsNullOrEmpty(TaxCompID.SelectedValue))
    //    {
    //        //if (YearSelect.SelectedIndex == 0) YearSelect.SelectedIndex = YearSelect.Items.Count - 1;
    //        DateTime lastTaxDate = new DateTime(int.Parse(YearSelect.SelectedValue), 3, 31);
    //        DateTime firstTaxDate = new DateTime(int.Parse(YearSelect.SelectedValue) - 1, 4, 1);

    //        DBFilter filter = new DBFilter();//sbinding.createFilter();

    //        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
    //        //    filter.add(info.orderby, info.order);

    //        string select = "e.*,  et.EmpTermLastDate, te.TaxEmpID, te.TaxEmpGeneratedDate, te.TaxEmpGeneratedByUserID ";
    //        string from = "from [" + db.dbclass.tableName + "] e LEFT JOIN [" + EEmpTermination.db.dbclass.tableName + "] et ON et.EmpID=e.EmpID INNER JOIN [" + ETaxEmp.db.dbclass.tableName + "] te ON e.EmpID=te.EmpID AND te.TaxFormID=" + CurID.ToString() + " ";

    //        if (TaxFormType.SelectedValue.Equals("B", StringComparison.CurrentCultureIgnoreCase))
    //        {
    //            OR joinDateInTerms = new OR();
    //            joinDateInTerms.add(new Match("e.EmpDateOfJoin", "<=", lastTaxDate));
    //            joinDateInTerms.add(new NullTerm("e.EmpDateOfJoin"));
    //            filter.add(joinDateInTerms);

    //            OR termDateTerms = new OR();
    //            termDateTerms.add(new IN("not e.empid", "Select et.empid from EmpTermination et ", new DBFilter()));
    //            DBFilter termDateFilter = new DBFilter();
    //            termDateFilter.add(new Match("et.EmpTermLastDate", ">", lastTaxDate));
    //            termDateTerms.add(new IN("e.empid", "Select et.empid from EmpTermination et ", termDateFilter));
    //            filter.add(termDateTerms);

    //            DBFilter positionFilter = new DBFilter();
    //            positionFilter.add(new Match("ep.EmpPosEffFr", "<=", lastTaxDate));
    //            OR positonEffToTerms = new OR();
    //            positonEffToTerms.add(new Match("ep.EmpPosEffTo", ">=", lastTaxDate));
    //            positonEffToTerms.add(new NullTerm("ep.EmpPosEffTo"));
    //            positionFilter.add(positonEffToTerms);
    //            DBFilter taxCompFilter = new DBFilter();
    //            taxCompFilter.add(new Match("tcm.TaxCompID", TaxCompID.SelectedIndex == 0 ? "0" : TaxCompID.SelectedValue));
    //            positionFilter.add(new IN("ep.CompanyID", "Select tcm.CompanyID from TaxCompanyMap tcm", taxCompFilter));
    //            filter.add(new IN("e.EmpID", "Select ep.EmpID from EmpPositionInfo ep", positionFilter));
    //        }
    //        else if (TaxFormType.SelectedValue.Equals("F", StringComparison.CurrentCultureIgnoreCase) || TaxFormType.SelectedValue.Equals("G", StringComparison.CurrentCultureIgnoreCase))
    //        {
    //            OR joinDateInTerms = new OR();
    //            joinDateInTerms.add(new Match("e.EmpDateOfJoin", "<=", lastTaxDate));
    //            joinDateInTerms.add(new NullTerm("e.EmpDateOfJoin"));
    //            filter.add(joinDateInTerms);

    //            DBFilter termDateFilter = new DBFilter();
    //            termDateFilter.add(new Match("et.EmpTermLastDate", "<=", lastTaxDate));
    //            termDateFilter.add(new Match("et.EmpTermLastDate", ">=", firstTaxDate));
    //            filter.add(new IN("e.empid", "Select et.empid from EmpTermination et ", termDateFilter));

    //            DBFilter positionFilter = new DBFilter();
    //            positionFilter.add(new Match("ep.EmpPosEffFr", "<=", lastTaxDate));
    //            OR positonEffToTerms = new OR();
    //            positonEffToTerms.add(new Match("ep.EmpPosEffTo", ">", lastTaxDate));
    //            positonEffToTerms.add(new NullTerm("ep.EmpPosEffTo"));
    //            positionFilter.add(positonEffToTerms);
    //            DBFilter taxCompFilter = new DBFilter();
    //            taxCompFilter.add(new Match("tcm.TaxCompID", TaxCompID.SelectedIndex == 0 ? "0" : TaxCompID.SelectedValue));
    //            positionFilter.add(new IN("ep.CompanyID", "Select tcm.CompanyID from TaxCompanyMap tcm", taxCompFilter));
    //            filter.add(new IN("e.EmpID", "Select ep.EmpID from EmpPositionInfo ep", positionFilter));

    //        }
    //        else if (TaxFormType.SelectedValue.Equals("E", StringComparison.CurrentCultureIgnoreCase))
    //        {
    //            filter.add(new Match("e.EmpDateOfJoin", ">=", firstTaxDate));
    //            filter.add(new Match("e.EmpDateOfJoin", "<=", lastTaxDate));
    //        }

    //        //DBFilter taxEmpFilter = new DBFilter();
    //        //taxEmpFilter.add(new Match("TaxFormID", CurID));
    //        //taxEmpFilter.add(new MatchField("te.EmpID", "e.EmpID"));
    //        //filter.add(new Exists(ETaxEmp.db.dbclass.tableName + " te", taxEmpFilter));

    //        DBFilter empInfoFilter = EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime(), AppUtils.ServerDateTime());
    //        empInfoFilter.add(new MatchField("e.EmpID", "ee.EmpID"));
    //        filter.add(new Exists(EEmpPersonalInfo.db.dbclass.tableName + " ee", empInfoFilter));

    //        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);
    //        table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);



    //        //if (table.Rows.Count != 0)
    //        //    btnGenerate.Visible = true & IsAllowEdit;
    //        //else
    //        //    btnGenerate.Visible = false;

    //        view = new DataView(table);
    //        //if (info != null)
    //        //{
    //        //    info.loadPageList(null, PrevPage, NextPage, FirstPage, LastPage);

    //        //    CurPage.Value = info.page.ToString();
    //        //    NumPage.Value = info.numPage.ToString();
    //        //}
    //        if (repeater != null)
    //        {
    //            repeater.DataSource = view;
    //            repeater.DataBind();
    //        }


    //        return view;
    //    }
    //    else
    //    {
    //        repeater.DataSource = null;
    //        repeater.DataBind();
    //        return null;
    //    }
    //}
    protected void Search_Click(object sender, EventArgs e)
    {
        info.page = 0;
        view = loadData(info, db, Repeater, false);
        TaxGeneratedView = loadData(TaxGeneratedInfo, db, TaxGenerated_Repeater, true);
    }
    protected void Reset_Click(object sender, EventArgs e)
    {
        sbinding.clear();
        EmployeeSearchControl1.Reset();
        info.page = 0;

        view = loadData(info, db, Repeater, false);
        TaxGeneratedView = loadData(TaxGeneratedInfo, db, TaxGenerated_Repeater, true);

    }
    protected void ChangeOrder_Click(object sender, EventArgs e)
    {
        LinkButton l = (LinkButton)sender;
        String id = l.ID.Substring(1);
        if (info.orderby == null)
            info.order = true;
        else if (info.orderby.Equals(id))
            info.order = !info.order;
        else
            info.order = true;
        info.orderby = id;

        view = loadData(info, db, Repeater, false);

    }

    protected void TaxGenerated_ChangeOrder_Click(object sender, EventArgs e)
    {
        LinkButton l = (LinkButton)sender;
        String id = l.ID.Substring(l.ID.IndexOf("_") + 1);
        if (TaxGeneratedInfo.orderby == null)
            TaxGeneratedInfo.order = true;
        else if (TaxGeneratedInfo.orderby.Equals(id))
            TaxGeneratedInfo.order = !TaxGeneratedInfo.order;
        else
            TaxGeneratedInfo.order = true;
        TaxGeneratedInfo.orderby = id;

        TaxGeneratedView = loadData(TaxGeneratedInfo, db, TaxGenerated_Repeater, true);

    }

    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(db, row, cb);
        cb.Visible = IsAllowEdit;
        if (CurID > 0)
        {
            DataRow dataRow = row.Row;

            if (dataRow.Table.Columns.Contains("TaxEmpID"))
            {
                cb.Checked = false;
                if (!dataRow.IsNull("TaxEmpGeneratedByUserID"))
                {
                    EUser user = new EUser();
                    if (user.LoadDBObject(dbConn, dataRow["TaxEmpGeneratedByUserID"]))
                    {
                        Label TaxEmpGeneratedByUserID = (Label)e.Item.FindControl("TaxEmpGeneratedByUserID");
                        TaxEmpGeneratedByUserID.Text = user.UserName;
                    }
                }
            }
            else
                cb.Checked = true;

        }
        else
            cb.Checked = true;
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        ArrayList list = WebUtils.SelectedRepeaterItemToBaseObjectList(EEmpPersonalInfo.db,Repeater,"ItemSelect");
        list.AddRange(WebUtils.SelectedRepeaterItemToBaseObjectList(EEmpPersonalInfo.db, TaxGenerated_Repeater, "ItemSelect"));

        DateTime processDateTime = AppUtils.ServerDateTime() ;


        if (list.Count > 0)
        {
            Session["GenerateTax_EmpList"] = list;
            //string strEmpIDList = string.Empty;
            //foreach (EEmpPersonalInfo o in list)
            //{
            //    if (strEmpIDList == string.Empty)
            //        strEmpIDList = ((EEmpPersonalInfo)o).EmpID.ToString();
            //    else
            //        strEmpIDList += "_" + ((EEmpPersonalInfo)o).EmpID.ToString();

            //}

            int taxFormID = HROne.Taxation.TaxationGeneration.GetOrCreateTaxFormID(dbConn, int.Parse(TaxCompID.SelectedValue), int.Parse(YearSelect.SelectedValue), TaxFormType.SelectedValue);

            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Taxation_Generation_Process.aspx?"
                + "TaxFormID=" + taxFormID
                + "&Total=" + list.Count);
                //+ "&EmpID=" + strEmpIDList);


            //HROne.Taxation.TaxationGeneration.GenerationFormTaxation(int.Parse(TaxCompID.SelectedValue), int.Parse(YearSelect.SelectedValue), TaxFormType.SelectedValue, list);
            //errors.addError("Complete");
            //Response.Write("<script>alert('Completed'); </script>");
        }
        else
        {
            PageErrors errors = PageErrors.getErrors(EEmpPayroll.db, Page.Master);
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_NO_EMPLOYEE_SELECTED);
        }
        //view = loadData(info, db, Repeater, false);
    }

    //Start 0000205, Miranda, 2015-5-30
    private int getFormIDByType(string type)
    {
        int id = 0;
        DBFilter filter = new DBFilter();
        filter.add(new Match("TaxFormType", type));
        ArrayList taxForms = ETaxForm.db.select(dbConn, filter);
        if (taxForms.Count > 0)
            id = ((ETaxForm)taxForms[0]).TaxFormID;
        return id;
    }
    //End 0000205, Miranda, 2015-5-30
}
