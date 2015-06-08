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
using HROne.Translation;
using HROne.Lib.Entities;

public partial class HierarchyElement_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "SYS003";

    public Binding binding;
    public DBManager db = EHierarchyElement.db;
    public EHierarchyElement obj;
    public int CurID = -1;
    public EHierarchyLevel HLevel;
    public ECompany Company;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;



        binding = new Binding(dbConn, db);
        binding.add(HElementID);
        binding.add(CompanyID);
        binding.add(HLevelID);
        binding.add(HElementCode);
        binding.add(HElementDesc);
        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["HElementID"], out CurID))
            CurID = -1;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (!Page.IsPostBack)
        {
            if (CurID > 0)
            {
                loadObject();
            }
            else
            {
                HLevelID.Value = DecryptedRequest["HLevelID"];
                CompanyID.Value = DecryptedRequest["CompanyID"];
                toolBar.DeleteButton_Visible = false;
            }

        }
        int intHLevelID, intCompanyID;
        HLevel = new EHierarchyLevel();
        if (int.TryParse(HLevelID.Value, out intHLevelID))
        {
            HLevel.HLevelID = intHLevelID;
            EHierarchyLevel.db.select(dbConn, HLevel);
        }

        Company = new ECompany();
        if (int.TryParse(CompanyID.Value, out intCompanyID))
        {
            Company.CompanyID = intCompanyID;
            ECompany.db.select(dbConn, Company);
        }
    }
    protected bool loadObject()
    {
        obj = new EHierarchyElement();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
        if (!db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);
        return true;
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        EHierarchyElement c = new EHierarchyElement();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);
        AND andFilterTerm = new AND();
        andFilterTerm.add(new Match("CompanyID", c.CompanyID));
        andFilterTerm.add(new Match("HLevelID", c.HLevelID));
        if (!AppUtils.checkDuplicate(dbConn, db, c, errors, "HElementCode", andFilterTerm))
            return;

        WebUtils.StartFunction(Session, FUNCTION_CODE);
        if (CurID < 0)
        {
            //            Utils.MarkCreate(Session, c);

            db.insert(dbConn, c);
            CurID = c.HElementID;
            //            url = Utils.BuildURL(-1, CurID);
        }
        else
        {
            //            Utils.Mark(Session, c);
            db.update(dbConn, c);
        }
        WebUtils.EndFunction(dbConn);
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "HierarchyElement_View.aspx?HElementID=" + CurID);


    }
    protected void Delete_Click(object sender, EventArgs e)
    {

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        EHierarchyElement obj = new EHierarchyElement();
        obj.HElementID = CurID;
        db.select(dbConn, obj);

        DBFilter hierarchyElementFilter = new DBFilter();
        hierarchyElementFilter.add(new Match("HElementID", obj.HElementID));

        IN inTerms = new IN("EmpPosID", "Select EmpPosID From " + EEmpHierarchy.db.dbclass.tableName, hierarchyElementFilter);

        DBFilter empPosFilter = new DBFilter();
        empPosFilter.add(inTerms);
        empPosFilter.add("empid", true);
        ArrayList empPosList = EEmpPositionInfo.db.select(dbConn, empPosFilter);
        if (empPosList.Count > 0)
        {
            errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_USED_BY_EMPLOYEE, new string[] { HROne.Common.WebUtility.GetLocalizedString("Code"), obj.HElementCode }));
            foreach (EEmpPositionInfo empPos in empPosList)
            {
                EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                empInfo.EmpID = empPos.EmpID;
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                    errors.addError("- " + empInfo.EmpNo + ", " + empInfo.EmpEngFullName);
                else
                    EEmpPositionInfo.db.delete(dbConn, empPos);

            }
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
        }
        else
        {
            WebUtils.StartFunction(Session, FUNCTION_CODE);
            db.delete(dbConn, obj);
            WebUtils.EndFunction(dbConn);
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "HierarchyElement_List.aspx");
        }

    }


    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "HierarchyElement_View.aspx?HElementID=" + CurID);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "HierarchyElement_List.aspx");

    }
}
