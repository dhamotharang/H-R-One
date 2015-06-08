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

public partial class Taxation_Emp_Header : HROneWebControl
{
    public Binding binding;
    public Binding empBinding;
    public Binding taxFormBinding;
    public DBManager db = ETaxEmp.db;
    public int CurID = -1;


    protected void Page_Load(object sender, EventArgs e)
    {
        PreRender += new EventHandler(Taxation_Emp_Header_PreRender);
        binding = new Binding(dbConn, db);
        empBinding = new Binding(dbConn, EEmpPersonalInfo.db);
        taxFormBinding = new Binding(dbConn, ETaxForm.db);

        binding.add(EmpID);
        binding.add(TaxEmpSurname);
        binding.add(TaxEmpOtherName);
        binding.add(TaxEmpChineseName);
        binding.init(Request, Session);

        empBinding.add(EmpNo);
        empBinding.add(EmpAlias);
        empBinding.init(Request,Session);
        taxFormBinding.add(TaxFormEmployerName);
        taxFormBinding.add(TaxFormYear);
        taxFormBinding.add(TaxFormType);


    }

    public int CurrentTaxEmpID
    {
        get { return CurID; }
        set { CurID = value; }
    }

    void Taxation_Emp_Header_PreRender(object sender, EventArgs e)
    {
        loadObject();        
    }

    protected bool loadObject()
    {
        ETaxEmp obj = new ETaxEmp();
        obj.TaxEmpID = CurID;
        if (!db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);

        if (obj.EmpID != 0)
        {
            EEmpPersonalInfo empInfo = new EEmpPersonalInfo();

            empInfo.EmpID = obj.EmpID;
            if (EEmpPersonalInfo.db.select(dbConn, empInfo))
            {

                Hashtable empValues = new Hashtable();
                EEmpPersonalInfo.db.populate(empInfo, empValues);
                empBinding.toControl(empValues);
            }

        }
        if (obj.TaxFormID!=0)
        {
            ETaxForm taxForm = new ETaxForm();

            taxForm.TaxFormID = obj.TaxFormID;
            if (ETaxForm.db.select(dbConn, taxForm))
            {

                Hashtable taxFormValues = new Hashtable();
                ETaxForm.db.populate(taxForm, taxFormValues);
                taxFormBinding.toControl(taxFormValues);
            }

        }
        return true;
    }

}
