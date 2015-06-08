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

public partial class Taxation_Form_Header : HROneWebControl
{
    public Binding taxFormBinding;
    public DBManager db = ETaxForm.db;
    public int CurID = -1;


    protected void Page_Load(object sender, EventArgs e)
    {
        PreRender += new EventHandler(Taxation_Emp_Header_PreRender);
        taxFormBinding = new Binding(dbConn, ETaxForm.db);

        taxFormBinding.add(TaxFormEmployerName);
        taxFormBinding.add(TaxFormYear);
        taxFormBinding.add(TaxFormType);


    }

    public int CurrentTaxFormID
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
        ETaxForm taxForm = new ETaxForm();

        taxForm.TaxFormID = CurID;
        if (!ETaxForm.db.select(dbConn, taxForm))
            return false;

        Hashtable taxFormValues = new Hashtable();
        ETaxForm.db.populate(taxForm, taxFormValues);
        taxFormBinding.toControl(taxFormValues);

        return true;
    }

}
