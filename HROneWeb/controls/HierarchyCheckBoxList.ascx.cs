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
using System.Globalization;
public partial class HierarchyCheckBoxList : HROneWebControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptInclude(Page, Page.GetType(), "popUpListHandler", ResolveClientUrl("~/javascript/popUpListHandler.js"));
        ScriptManager.RegisterClientScriptInclude(Page, Page.GetType(), "advancedCheckBoxListHandler", ResolveClientUrl("~/javascript/advancedCheckBoxListHandler.js"));

        if (!IsPostBack)
        {
            ArrayList list = ECompany.db.select(dbConn, new DBFilter());
            CompanyRepeater.DataSource = list;
            CompanyRepeater.DataBind();
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (this.Visible)
        {

            btnMore.Attributes.Add("onclick", "popUpListHandler(document.getElementById('" + checkBoxListDiv.ClientID + "'),document.getElementById('" + btnClose.ClientID + "'),this,true); return false;");
            ScriptManager.RegisterStartupScript(this, this.GetType(), this.ClientID, "advancedCheckBoxListHandler('" + CompanyRepeater.ClientID + "', '" + lblOptionList.ClientID + "', document.getElementById('" + btnSelectAll.ClientID + "'), document.getElementById('" + btnClearAll.ClientID + "'),'');\r\n", true);


            foreach (RepeaterItem companyRepeaterItem in CompanyRepeater.Items)
            {
                {
                    Image btnMoreCompany = (Image)companyRepeaterItem.FindControl("btnExpand");
                    Image btnCloseCompany = (Image)companyRepeaterItem.FindControl("btnCollapse");
                    HtmlGenericControl checkBoxListDivCompany = (HtmlGenericControl)companyRepeaterItem.FindControl("checkBoxListDiv");

                    btnMoreCompany.Attributes.Add("onclick", "popUpListHandler(document.getElementById('" + checkBoxListDivCompany.ClientID + "'),document.getElementById('" + btnCloseCompany.ClientID + "'),this,false); return false;");
                }
                Repeater hLevelRepeater = (Repeater)companyRepeaterItem.FindControl("HierarchyLevel");
                foreach (RepeaterItem hLevelRepeaterItem in hLevelRepeater.Items)
                {
                    Image btnMoreHierarchy = (Image)hLevelRepeaterItem.FindControl("btnExpand");
                    Image btnCloseHierarchy = (Image)hLevelRepeaterItem.FindControl("btnCollapse");
                    HtmlGenericControl checkBoxListDivHierarchy = (HtmlGenericControl)hLevelRepeaterItem.FindControl("checkBoxListDiv");
                    if (this.Visible)
                        btnMoreHierarchy.Attributes.Add("onclick", "popUpListHandler(document.getElementById('" + checkBoxListDivHierarchy.ClientID + "'),document.getElementById('" + btnCloseHierarchy.ClientID + "'),this,false); return false;");

                    CheckBoxList HierarchyElementList = (CheckBoxList)hLevelRepeaterItem.FindControl("HierarchyElementList");
                    Button btnSelectAllHierarchyButton = (Button)hLevelRepeaterItem.FindControl("btnSelectAll");
                    Button btnClearAllHierarchyButton = (Button)hLevelRepeaterItem.FindControl("btnClearAll");
                    Label lblHierarchyOptionList = (Label)hLevelRepeaterItem.FindControl("lblOptionList");
                    if (this.Visible)
                        ScriptManager.RegisterStartupScript(hLevelRepeaterItem, hLevelRepeaterItem.GetType(), hLevelRepeaterItem.ClientID, "advancedCheckBoxListHandler('" + HierarchyElementList.ClientID + "', '" + lblHierarchyOptionList.ClientID + "', document.getElementById('" + btnSelectAllHierarchyButton.ClientID + "'), document.getElementById('" + btnClearAllHierarchyButton.ClientID + "'),'');\r\n", true);

                }
            }
        }

    }

    protected void CompanyRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        ECompany company = (ECompany)e.Item.DataItem;

        Binding binding = new Binding(dbConn, ECompany.db);

        CheckBox chcekBox = (CheckBox)e.Item.FindControl("ItemSelect");
        chcekBox.Text = company.CompanyCode + " - " + company.CompanyName;
        HtmlInputHidden hiddenInput = (HtmlInputHidden)e.Item.FindControl("CompanyID");
        hiddenInput.Value = company.CompanyID.ToString();

        binding.add(hiddenInput);
        binding.init(Request, Session);



        DBFilter filter = new DBFilter();
        filter.add("HLevelSeqNo", true);
        ArrayList list = EHierarchyLevel.db.select(dbConn, filter);
        Repeater HierarchyLevel = (Repeater)e.Item.FindControl("HierarchyLevel");
        HierarchyLevel.DataSource = list;
        HierarchyLevel.DataBind();


    }

    protected void HierarchyLevel_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        HROne.Common.WebUtility.WebControlsLocalization(Session, e.Item.Controls);

        EHierarchyLevel level = (EHierarchyLevel)e.Item.DataItem;

        Label lblHLevel = (Label)e.Item.FindControl("HLevel");
        lblHLevel.Text = level.HLevelDesc;



        DBFilter filter = new DBFilter();
        filter.add(new Match("HLevelID", level.HLevelID));
        filter.add(new Match("CompanyID", ((HtmlInputHidden)e.Item.Parent.Parent.FindControl("CompanyID")).Value));
       
        CheckBoxList c = (CheckBoxList)e.Item.FindControl("HierarchyElementList");
        string selected = c.SelectedValue;
        WebFormUtils.loadValues(dbConn, c, EHierarchyElement.VLHierarchyElement, filter, ci, null, "combobox.notselected");
        c.Attributes["HLevelID"] = level.HLevelID.ToString();


    }

    public DBTerm GetFilters(string TableName, string EmpPosIDFieldName)
    {
        if (!string.IsNullOrEmpty(TableName))
        {
            if (!TableName.Trim().Equals(string.Empty))
                TableName = TableName.Trim() + ".";
        }
        OR companyTerm = null;

        string selectedCompanyValueList = string.Empty;
        //OR companyOrTerm = null;
        foreach (RepeaterItem companyRepeaterItem in CompanyRepeater.Items)
        {
            string selectedHierarchyElementValueList = string.Empty;
            string selectedNotSelectedHierarchyLevelList = string.Empty;

            CheckBox companyChcekBox = (CheckBox)companyRepeaterItem.FindControl("ItemSelect");
            HtmlInputHidden hiddenInput = (HtmlInputHidden)companyRepeaterItem.FindControl("CompanyID");


            Repeater hLevelRepeater = (Repeater)companyRepeaterItem.FindControl("HierarchyLevel");
            foreach (RepeaterItem hLevelRepeaterItem in hLevelRepeater.Items)
            {
                OR orTerm = null;

                CheckBoxList hElementList = (CheckBoxList)hLevelRepeaterItem.FindControl("HierarchyElementList");


                foreach (ListItem item in hElementList.Items)
                {
                    if (item.Selected)
                    {

                        if (!string.IsNullOrEmpty(item.Value))
                        {
                            //if (orTerm == null)
                            //    orTerm = new OR();
                            //orTerm.add(new Match("eh.HElementID", item.Value));
                            if (!string.IsNullOrEmpty(selectedHierarchyElementValueList))
                                selectedHierarchyElementValueList += ",";
                            selectedHierarchyElementValueList += item.Value;
                        }
                        else if (hElementList is CheckBoxList)
                        {
                            //if (orTerm == null)
                            //    orTerm = new OR();
                            //DBFilter dbFilter = new DBFilter();
                            //dbFilter.add(new MatchField("eh.HElementID", "he.HElementID"));
                            //AND andHElement = new AND();
                            //andHElement.add(new Match("eh.HLevelID", hElementList.Attributes["HLevelID"]));
                            //orTerm.add(new Exists("HierarchyElement he", dbFilter, true));
                            if (!string.IsNullOrEmpty(selectedNotSelectedHierarchyLevelList))
                                selectedNotSelectedHierarchyLevelList += ",";
                            selectedNotSelectedHierarchyLevelList += hElementList.Attributes["HLevelID"];
                        }
                    }
                }
                if (orTerm != null)
                {
                    DBFilter sub = new DBFilter();
                    sub.add(new MatchField(TableName + EmpPosIDFieldName, "eh.EmpPosID"));
                    sub.add(orTerm);
                    if (companyTerm == null)
                        companyTerm = new OR();
                    companyTerm.add(new Exists(EEmpHierarchy.db.dbclass.tableName + " eh", sub));
                }

            }
            if (!string.IsNullOrEmpty(selectedHierarchyElementValueList) || !string.IsNullOrEmpty(selectedNotSelectedHierarchyLevelList))
            {
                if (companyTerm == null)
                    companyTerm = new OR();
                OR orHierarchyElementTerm = new OR();
                if (!string.IsNullOrEmpty(selectedHierarchyElementValueList))
                    orHierarchyElementTerm.add(new IN("eh.HElementID", selectedHierarchyElementValueList, null));
                if (!string.IsNullOrEmpty(selectedNotSelectedHierarchyLevelList))
                {
                    DBFilter dbFilter = new DBFilter();
                    dbFilter.add(new MatchField("eh.HElementID", "he.HElementID"));
                    AND andHElement = new AND();
                    andHElement.add(new Exists("HierarchyElement he", dbFilter, true));
                    andHElement.add(new IN("eh.HLevelID", selectedNotSelectedHierarchyLevelList, null));
                    orHierarchyElementTerm.add(andHElement);
                }
                DBFilter sub = new DBFilter();
                sub.add(new MatchField(TableName + EmpPosIDFieldName, "eh.EmpPosID"));
                sub.add(new IN(TableName + "CompanyID", hiddenInput.Value, null));
                sub.add(orHierarchyElementTerm);
                companyTerm.add(new Exists(EEmpHierarchy.db.dbclass.tableName + " eh", sub));
            }
            else if (companyChcekBox.Checked)
            {
                //if (companyOrTerm == null)
                //{
                //    companyOrTerm = new OR();
                //    if (companyTerm == null)
                //        companyTerm = new AND();

                //    companyTerm.add(companyOrTerm);
                //}
                if (!string.IsNullOrEmpty(selectedCompanyValueList))
                    selectedCompanyValueList += ",";
                selectedCompanyValueList += hiddenInput.Value;

                //companyOrTerm.add(new Match(TableName + "CompanyID", hiddenInput.Value));
            }


        }
        if (!string.IsNullOrEmpty(selectedCompanyValueList))
        {
            if (companyTerm == null)
                companyTerm = new OR();
            companyTerm.add(new IN(TableName + "CompanyID", selectedCompanyValueList, null));
        }
        return companyTerm;
    }
    //public void LoadListControl(WFValueList ValueList, bool hasNotSelected)
    //{
    //    WebFormUtils.loadValues(CheckBoxListObject, ValueList, new DBFilter(), System.Threading.Thread.CurrentThread.CurrentUICulture, string.Empty, (string)"combobox.notselected");
    //    if (!hasNotSelected)
    //        if (CheckBoxListObject.Items[0].Value.Equals(string.Empty))
    //            CheckBoxListObject.Items.RemoveAt(0);

    //}

    public void Reset()
    {
        foreach (RepeaterItem companyRepeaterItem in CompanyRepeater.Items)
        {
            CheckBox companyChcekBox = (CheckBox)companyRepeaterItem.FindControl("ItemSelect");
            companyChcekBox.Checked = false;

            Repeater hLevelRepeater = (Repeater)companyRepeaterItem.FindControl("HierarchyLevel");
            foreach (RepeaterItem hLevelRepeaterItem in hLevelRepeater.Items)
            {
                CheckBoxList hElementList = (CheckBoxList)hLevelRepeaterItem.FindControl("HierarchyElementList");
                foreach (ListItem listItem in hElementList.Items)
                    listItem.Selected = false;
            }
        }
    }

    //public ListControl ListControl
    //{
    //    get
    //    {
    //        return CheckBoxListObject;
    //    }

    //}

}
