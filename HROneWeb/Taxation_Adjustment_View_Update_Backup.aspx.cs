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

public partial class Taxation_Adjustment_View_Update_Backup : HROneWebPage
{
    private const string FUNCTION_CODE = "TAX004";
    
    public Binding binding;
    public DBManager db = ETaxEmp.db;
    public int CurID = -1;
    public int CurPayPeriodID = -1;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
        {
            TaxEmpEditPanel.Visible = false;

        } 
        binding = new Binding(dbConn, db);
        binding.add(TaxEmpID);
        binding.add( TaxEmpSurname );
        binding.add(TaxEmpOtherName );
        binding.add(TaxEmpChineseName );
        binding.add(new HKIDLabel(db, TaxEmpHKID, "TaxEmpHKID"));
        binding.add(new LabelVLBinder(db,TaxEmpSex,Values.VLGender));
        binding.add(new LabelVLBinder(db, TaxEmpMartialStatus, ETaxEmp.VLTaxMaritalStatus));
        binding.add(TaxEmpPassportNo);
        binding.add(TaxEmpIssuedCountry);
        binding.add(TaxEmpSpouseName);
        binding.add(new HKIDLabel(db, TaxEmpSpouseHKID, "TaxEmpSpouseHKID"));
        binding.add(TaxEmpSpousePassportNo);
        binding.add(TaxEmpSpouseIssuedCountry);
        binding.add(TaxEmpResAddr);
        binding.add(new LabelVLBinder(db, TaxEmpResAddrAreaCode, Values.VLArea));
        binding.add(TaxEmpCorAddr);
        binding.add(TaxEmpCapacity);
        binding.add(TaxEmpPartTimeEmployer);
        binding.add(TaxEmpStartDate);
        binding.add(TaxEmpEndDate);
        binding.add(new LabelVLBinder(db, TaxEmpOvearseasIncomeIndicator, Values.VLYesNo));
        binding.add(TaxEmpOverseasCompanyName);
        binding.add(TaxEmpOverseasCompanyAddress);
        binding.add(TaxEmpOverseasCompanyAmount);
        binding.add(TaxEmpTaxFileNo);
        binding.add(TaxEmpRemark);
        binding.add(TaxEmpNewEmployerNameddress);
        binding.add(TaxEmpFutureCorAddr);
        binding.add(TaxEmpCessationReason);
        binding.add(TaxEmpLeaveHKDate);
        binding.add(new LabelVLBinder(db, TaxEmpIsERBearTax, ETaxEmp.VLTaxYesNo));
        binding.add(new LabelVLBinder(db, TaxEmpIsMoneyHoldByOrdinance, ETaxEmp.VLTaxYesNo));
        binding.add(TaxEmpHoldAmount);
        binding.add(TaxEmpReasonForNotHold);
        binding.add(new LabelVLBinder(db, TaxEmpReasonForDepartureReason, ETaxEmp.VLTaxDepartureReason));
        binding.add(TaxEmpReasonForDepartureOtherReason);

        binding.add(new LabelVLBinder(db, TaxEmpIsEEReturnHK, ETaxEmp.VLTaxYesNo));
        binding.add(TaxEmpEEReturnHKDate);
        binding.add(new LabelVLBinder(db, TaxEmpIsShareOptionsGrant, ETaxEmp.VLTaxYesNo));
        binding.add(TaxEmpShareOptionsGrantCount);
        binding.add(TaxEmpShareOptionsGrantDate);
        binding.add(TaxEmpPreviousEmployerNameddress);

        binding.init(Request, Session);


        if (!int.TryParse(DecryptedRequest["TaxEmpID"], out CurID))
            CurID = -1;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (!Page.IsPostBack)
        {
            if (CurID > 0)
            {
                loadObject();
            }
        }

    }


    protected bool loadObject()
    {
        ETaxEmp obj = new ETaxEmp();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
        if (!db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);
        ucTaxation_Form_Header.CurrentTaxFormID = obj.TaxFormID;

        ETaxForm taxForm = new ETaxForm();
        taxForm.TaxFormID = obj.TaxFormID;
        if (ETaxForm.db.select(dbConn, taxForm))
        {
            if (taxForm.TaxFormType.Equals("B"))
            {
                TaxFormBFGPanel1.Visible = true;
                TaxFormBFGPanel2.Visible = true;
                TaxFormBFGPanel3.Visible = true;

                TaxFormBPanel.Visible = true;

                TaxFormEPanel1.Visible = false;
                TaxFormEPanel2.Visible = false;

                TaxFormFPanel1.Visible = false;
                TaxFormFPanel2.Visible = false;
                TaxFormGPanel1.Visible = false;
                TaxFormGPanel2.Visible = false;
                TaxFormGPanel3.Visible = false;
                TaxFormGPanel4.Visible = false;
                TaxFormFGPanel.Visible = false;
                TaxFormEGPanel1.Visible = false;
            }
            if (taxForm.TaxFormType.Equals("F"))
            {
                TaxFormBFGPanel1.Visible = true;
                TaxFormBFGPanel2.Visible = true;
                TaxFormBFGPanel3.Visible = true;

                TaxFormBPanel.Visible = false;

                TaxFormEPanel1.Visible = false;
                TaxFormEPanel2.Visible = false;

                TaxFormFPanel1.Visible = true;
                TaxFormFPanel2.Visible = true;
                TaxFormFPanel3.Visible = true;
                TaxFormGPanel1.Visible = false;
                TaxFormGPanel2.Visible = false;
                TaxFormGPanel3.Visible = false;
                TaxFormGPanel4.Visible = false;
                TaxFormFGPanel.Visible = true;
                TaxFormEGPanel1.Visible = false;
            }
            if (taxForm.TaxFormType.Equals("G"))
            {
                TaxFormBFGPanel1.Visible = true;
                TaxFormBFGPanel2.Visible = true;
                TaxFormBFGPanel3.Visible = true;

                TaxFormBPanel.Visible = false;

                TaxFormEPanel1.Visible = false;
                TaxFormEPanel2.Visible = false;

                TaxFormFPanel1.Visible = false;
                TaxFormFPanel2.Visible = false;
                TaxFormFPanel3.Visible = false;
                TaxFormGPanel1.Visible = true;
                TaxFormGPanel2.Visible = true;
                TaxFormGPanel3.Visible = true;
                TaxFormGPanel4.Visible = true;
                TaxFormFGPanel.Visible = true;
                TaxFormEGPanel1.Visible = true;
            }
            if (taxForm.TaxFormType.Equals("E"))
            {
                TaxFormBFGPanel1.Visible = false;
                TaxFormBFGPanel2.Visible = false;
                TaxFormBFGPanel3.Visible = false;

                TaxFormBPanel.Visible = false;

                TaxFormEPanel1.Visible = true;
                TaxFormEPanel2.Visible = true;

                TaxFormFPanel1.Visible = false;
                TaxFormFPanel2.Visible = false;
                TaxFormGPanel1.Visible = false;
                TaxFormGPanel2.Visible = false;
                TaxFormGPanel3.Visible = false;
                TaxFormGPanel4.Visible = false;
                TaxFormFGPanel.Visible = false;
                TaxFormEGPanel1.Visible = true;
            }
        }

        TaxEmpHoldAmountPanel.Visible = false;
        TaxEmpReasonForNotHoldPanel.Visible = false;

        if (!string.IsNullOrEmpty(obj.TaxEmpIsMoneyHoldByOrdinance))
            if (obj.TaxEmpIsMoneyHoldByOrdinance.Equals("Y"))
                TaxEmpHoldAmountPanel.Visible = true;
            else if (obj.TaxEmpIsMoneyHoldByOrdinance.Equals("N"))
                TaxEmpReasonForNotHoldPanel.Visible = true;

        if (!string.IsNullOrEmpty(obj.TaxEmpReasonForDepartureReason))
            if (obj.TaxEmpReasonForDepartureReason.Equals("Other"))
                TaxEmpReasonForDepartureOtherReasonPanel.Visible = true;
            else
                TaxEmpReasonForDepartureOtherReasonPanel.Visible = false;

        if (!string.IsNullOrEmpty(obj.TaxEmpIsEEReturnHK))
            if (obj.TaxEmpIsEEReturnHK.Equals("Y"))
                TaxEmpEEReturnHKDatePanel.Visible = true;
            else
                TaxEmpEEReturnHKDatePanel.Visible = false;

        if (!string.IsNullOrEmpty(obj.TaxEmpIsShareOptionsGrant))
            if (obj.TaxEmpIsShareOptionsGrant.Equals("Y"))
                TaxEmpIsShareOptionsGrantPanel.Visible = true;
            else
                TaxEmpIsShareOptionsGrantPanel.Visible = false;

        return true;
    }
    protected void TaxEmpIsMoneyHoldByOrdinance_SelectedIndexChanged(object sender, EventArgs e)
    {


    }
    protected void TaxEmpReasonForDepartureReason_SelectedIndexChanged(object sender, EventArgs e)
    {



    }
    protected void TaxEmpIsEEReturnHK_SelectedIndexChanged(object sender, EventArgs e)
    {



    }
    protected void TaxEmpIsShareOptionsGrant_SelectedIndexChanged(object sender, EventArgs e)
    {



    }
    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Taxation_Adjustment_List.aspx");
    }
}