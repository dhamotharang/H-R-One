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
using HROne.LeaveCalc;

public partial class Emp_info : HROneWebControl
{
    public Binding binding;
    public DBManager db = EEmpPersonalInfo.db;
    public EEmpPersonalInfo obj;
    public int CurID = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session))
            return;
        binding = new Binding(dbConn, db);
        binding.add(EmpID);
        binding.add(EmpNo);
        binding.add(EmpAlias);
        binding.add(EmpEngSurname);
        binding.add(EmpEngOtherName);
        //binding.add(EmpHKID);
        binding.add(EmpChiFullName);
        binding.add(new LabelVLBinder(db, EmpGender, Values.VLGender));
        //binding.add(EmpDateOfBirth);
        binding.add(EmpDateOfJoin);
        binding.add(EmpServiceDate);
        binding.add(EmpProbaLastDate);
        binding.init(Request, Session);



        if (CurID <= 0)
        {
            //CurID = Int32.Parse(DecryptedRequest["EmpID"]);
            EESSUser user = WebUtils.GetCurUser(Session);
            if (user != null)
            {
                CurID = user.EmpID;
                EmpID.Value = CurID.ToString();
            }
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {

            if (CurID > 0)
            {
                loadObject();
            }
            else
            {

            }
        }
    }

    protected bool loadObject()
    {
        obj = new EEmpPersonalInfo();
        obj.EmpID = CurID;
        // bool isNew = WebFormWorkers.loadKeys(db, obj, Request);
        if (!db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);

        if (obj.EmpProbaLastDate >= AppUtils.ServerDateTime().Date)
        {
            EmpProbaLastDate.ForeColor = System.Drawing.Color.Red;
            EmpProbaLastDate.Font.Bold = true;
        }

        EEmpTermination empTerm = EEmpTermination.GetObjectByEmpID(dbConn, obj.EmpID);
        if (empTerm == null)
            EmpTermLastDate.Text = string.Empty;
        else
        {
            EmpTermLastDate.Text = empTerm.EmpTermLastDate.ToString("yyyy-MM-dd");
            EmpTermLastDate.ForeColor = System.Drawing.Color.Red;
            EmpTermLastDate.Font.Bold = true;
        }

        return true;
    }
}
