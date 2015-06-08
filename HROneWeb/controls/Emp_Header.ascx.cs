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

public partial class Emp_Header : HROneWebControl
{
    public Binding binding;
    public DBManager db = EEmpPersonalInfo.db;
    public EEmpPersonalInfo obj;
    public int CurID = -1;


    protected void Page_Load(object sender, EventArgs e)
    {
        PreRender += new EventHandler(Emp_Header_PreRender);
        binding = new Binding(dbConn, db);
        binding.add(EmpID);
        binding.add(EmpNo);
        binding.add(EmpChiFullName);
        binding.add(EmpEngSurname);
        binding.add(EmpEngOtherName);
        binding.add(EmpAlias);
        binding.init(Request, Session);




    }

    public int CurrentEmpID
    {
        get { return CurID; }
        set { CurID = value; }
    }

    void Emp_Header_PreRender(object sender, EventArgs e)
    {
        loadObject();        
    }

    protected bool loadObject()
    {
        obj = new EEmpPersonalInfo();
        obj.EmpID = CurID;
        if (!db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);
        return true;
    }

}
