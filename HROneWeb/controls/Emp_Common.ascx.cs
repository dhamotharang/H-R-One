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

public partial class Emp_Common : HROneWebControl
{
    public Binding binding;
    public DBManager db = EEmpPersonalInfo.db;
    public EEmpPersonalInfo obj;
    public int CurID = -1;

    public bool hasMasterEmpNo
    {
        get { return MasterEmpNoRow.Visible; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PreRender += new EventHandler(Emp_Common_PreRender);
        binding = new Binding(dbConn, db);
        binding.add(EmpID);
        binding.add(EmpNo);
        binding.add(EmpChiFullName);
        binding.add(EmpEngSurname);
        binding.add(EmpEngOtherName);
        binding.add(EmpAlias);
        binding.add(new HKIDLabel(db, EmpHKID, "EmpHKID"));
        binding.add(EmpDateOfBirth);
        binding.add(EmpDateOfJoin);
        binding.add(EmpServiceDate);
        binding.add(new LabelVLBinder(db, EmpGender, Values.VLGender));
        
        binding.add(new LabelVLBinder(db, EmpStatus, EEmpPersonalInfo.VLEmpStatus));

        binding.add(new CheckBoxBinder(db, EmpIsCombinePaySlip));
        binding.add(new CheckBoxBinder(db, EmpIsCombineMPF));
        binding.add(new CheckBoxBinder(db, EmpIsCombineTax));

        binding.init(Request, Session);


        PrivateSection.Visible= WebUtils.CheckPermission(Session, "PER001", WebUtils.AccessLevel.Read);


        if (!int.TryParse(DecryptedRequest["EmpID"], out CurID))
            CurID = -1;

    }

    void Emp_Common_PreRender(object sender, EventArgs e)
    {
        loadObject();        
    }

    protected bool loadObject()
    {
        lblLastEmploymentDateValue.Visible = false;
        obj = new EEmpPersonalInfo();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);

        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpID", obj.EmpID));
        filter.add(WebUtils.AddRankFilter(Session, "EmpID", true));
        ArrayList empInfoList = EEmpPersonalInfo.db.select(dbConn, filter);
        if (empInfoList.Count == 0)
            if (CurID <= 0)
                return false;
            else
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");
        obj = (EEmpPersonalInfo)empInfoList[0];

        EEmpTermination empTerm = EEmpTermination.GetObjectByEmpID(dbConn, obj.EmpID);
        NewEmpNoPanel.Visible = false;
        lblLastEmploymentDateValue.Visible = false;
        if (empTerm != null)
        {
            lblLastEmploymentDateValue.Visible = true;
            lblLastEmploymentDateValue.Text = empTerm.EmpTermLastDate.ToString("yyyy-MM-dd");

            EEmpPersonalInfo newEmpInfo = new EEmpPersonalInfo();
            newEmpInfo.EmpID = empTerm.NewEmpID;
            if (EEmpPersonalInfo.db.select(dbConn, newEmpInfo))
            {
                NewEmpNoPanel.Visible = true;

                hlNewEmpNo.Text = newEmpInfo.EmpNo;
                hlNewEmpNo.NavigateUrl = "~/Emp_View.aspx?EmpID=" + newEmpInfo.EmpID;
            }
        }

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);

        EEmpDocument empDocument = EEmpDocument.GetProfilePhotoEmpDocument(dbConn, obj.EmpID);
        if (empDocument != null)
        {
            ProfilePhotoCell.Visible = true;
            ProfilePhoto.ImageUrl = "~/Emp_Document_Download.aspx?EmpID=" + empDocument.EmpID + "&EmpDocumentID=" + empDocument.EmpDocumentID;
            ProfilePhoto.Width = new Unit(100);
        }
        else
            ProfilePhotoCell.Visible = false;

        EEmpPersonalInfo prevEmpInfo = obj.GetPreviousEmpInfo(dbConn);
        if (prevEmpInfo == null)
            PrevEmpNoRow.Visible = false;
        else
        {
            hlPreviousEmpNo.Text = prevEmpInfo.EmpNo;
            hlPreviousEmpNo.NavigateUrl = "~/Emp_View.aspx?EmpID=" + prevEmpInfo.EmpID;
            PrevEmpNoRow.Visible = true;
        }
        MasterEmpNoRow.Visible = false;
        if (obj.MasterEmpID > 0)
        {
            EEmpPersonalInfo masterEmpInfo = new EEmpPersonalInfo();
            masterEmpInfo.EmpID = obj.MasterEmpID;
            if (EEmpPersonalInfo.db.select(dbConn, masterEmpInfo))
            {
                hlMasterEmpNo.Text = masterEmpInfo.EmpNo;
                hlMasterEmpNo.NavigateUrl = "~/Emp_View.aspx?EmpID=" + masterEmpInfo.EmpID;
                MasterEmpNoRow.Visible = true;
            }
        }

        ArrayList childRoleList = obj.GetOtherRoleList(dbConn);
        if (childRoleList.Count <= 0)
            ChildRoleEmpNoRow.Visible = false;
        else
        {
            ChildRoleEmpNoRow.Visible = true;

            ChildRoleEmpNoRepeater.DataSource = childRoleList;
            ChildRoleEmpNoRepeater.DataBind();
        }
        double age = AppUtils.GetAge(obj.EmpDateOfBirth);
        if (!double.IsNaN(age))
            lblAge.Text = HROne.CommonLib.GenericRoundingFunctions.RoundingDown(age, 0, 3).ToString("0");
        else
            lblAge.Text = string.Empty;

        double yearOfService = obj.GetYearOfServer(dbConn, AppUtils.ServerDateTime());
        if (!double.IsNaN(yearOfService))
            lblYearOfService.Text = HROne.CommonLib.GenericRoundingFunctions.RoundingDown(yearOfService, 1, 3).ToString("0.0");
        else
            lblYearOfService.Text = string.Empty;

        return true;
    }

    protected void ChildRoleEmpNoRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            EEmpPersonalInfo childRoleEmpInfo = (EEmpPersonalInfo)e.Item.DataItem;
            HyperLink hlEmpInfo = (HyperLink)e.Item.FindControl("hlChildRoleEmpNo");
            hlEmpInfo.Text = childRoleEmpInfo.EmpNo;
            hlEmpInfo.NavigateUrl = "~/Emp_View.aspx?EmpID=" + childRoleEmpInfo.EmpID;

        }
    }

}
