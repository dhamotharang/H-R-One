//using System;
//using System.Data;
//using System.Configuration;
//using System.Collections;
//using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Web.UI.HtmlControls;
//using HROne.DataAccess;
////using perspectivemind.validation;
//using HROne.Lib.Entities;
//using HROne.Taxation;

//public partial class Taxation_Adjustment_Edit_update : HROneWebPage
//{
//    private const string FUNCTION_CODE = "TAX004";
//    public Binding binding;
//    public DBManager db = ETaxEmp.db;
//    public int CurID = -1;
    

//    protected void Page_Load(object sender, EventArgs e)
//    {
//        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
//            return;
//        binding = new Binding(dbConn, db);
//        binding.add(TaxEmpID);
//        binding.add(TaxFormID);
//        binding.add(TaxEmpSurname);
//        binding.add(TaxEmpOtherName);
//        binding.add(TaxEmpChineseName);
//        binding.add(new HKIDBinder(db, TaxEmpHKID,TaxEmpHKIDDigit ));
//        binding.add(new DropDownVLBinder(db, TaxEmpSex, Values.VLGender));
//        binding.add(new DropDownVLBinder(db, TaxEmpMartialStatus, ETaxEmp.VLTaxMaritalStatus));
//        binding.add(TaxEmpPassportNo);
//        binding.add(TaxEmpIssuedCountry);
//        binding.add(TaxEmpSpouseName);
//        binding.add(new HKIDBinder(db, TaxEmpSpouseHKID, TaxEmpSpouseHKIDDigit));
//        binding.add(TaxEmpSpousePassportNo);
//        binding.add(TaxEmpSpouseIssuedCountry);
//        binding.add(TaxEmpResAddr);
//        binding.add(new DropDownVLBinder(db, TaxEmpResAddrAreaCode, Values.VLArea));
//        binding.add(TaxEmpCorAddr);
//        binding.add(TaxEmpCapacity);
//        binding.add(TaxEmpPartTimeEmployer);
//        binding.add(new TextBoxBinder(db, TaxEmpStartDate.TextBox, TaxEmpStartDate.ID));
//        binding.add(new TextBoxBinder(db, TaxEmpEndDate.TextBox, TaxEmpEndDate.ID));
//        binding.add(new DropDownVLBinder(db, TaxEmpOvearseasIncomeIndicator, Values.VLYesNo));
//        binding.add(TaxEmpOverseasCompanyName);
//        binding.add(TaxEmpOverseasCompanyAddress);
//        binding.add(TaxEmpOverseasCompanyAmount);
//        binding.add(TaxEmpTaxFileNo);
//        binding.add(TaxEmpRemark);
//        binding.add(TaxEmpNewEmployerNameddress);
//        binding.add(TaxEmpFutureCorAddr);
//        binding.add(TaxEmpCessationReason);
//        binding.add(new TextBoxBinder(db, TaxEmpLeaveHKDate.TextBox, TaxEmpLeaveHKDate.ID));
//        binding.add(new DropDownVLBinder(db, TaxEmpIsERBearTax, ETaxEmp.VLTaxYesNo));
//        binding.add(new DropDownVLBinder(db, TaxEmpIsMoneyHoldByOrdinance, ETaxEmp.VLTaxYesNo));
//        binding.add(TaxEmpHoldAmount);
//        binding.add(TaxEmpReasonForNotHold);
//        binding.add(new DropDownVLBinder(db, TaxEmpReasonForDepartureReason, ETaxEmp.VLTaxDepartureReason));
//        binding.add(TaxEmpReasonForDepartureOtherReason);

//        binding.add(new DropDownVLBinder(db, TaxEmpIsEEReturnHK, ETaxEmp.VLTaxYesNo));
//        binding.add(new TextBoxBinder(db, TaxEmpEEReturnHKDate.TextBox, TaxEmpEEReturnHKDate.ID));
//        binding.add(new DropDownVLBinder(db, TaxEmpIsShareOptionsGrant, ETaxEmp.VLTaxYesNo));
//        binding.add(TaxEmpShareOptionsGrantCount);
//        binding.add(new TextBoxBinder(db, TaxEmpShareOptionsGrantDate.TextBox, TaxEmpShareOptionsGrantDate.ID));
//        binding.add(TaxEmpPreviousEmployerNameddress);
//        binding.init(Request, Session);


//        if (!int.TryParse(DecryptedRequest["TaxEmpID"], out CurID))
//            CurID = -1;

//        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

//        if (!Page.IsPostBack)
//        {
//            if (CurID > 0)
//            {
//                loadObject();
//                Delete.Visible = true;
//            }
//            else
//                Delete.Visible = false;

//        }

//    }



//    protected bool loadObject()
//    {
//        ETaxEmp obj = new ETaxEmp();
//        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
//        if (!db.select(dbConn, obj))
//            return false;

//        Hashtable values = new Hashtable();
//        db.populate(obj, values);
//        binding.toControl(values);
//        ucTaxation_Form_Header.CurrentTaxFormID = obj.TaxFormID;

//        ETaxForm taxForm = new ETaxForm();
//        taxForm.TaxFormID = obj.TaxFormID;
//        if (ETaxForm.db.select(dbConn, taxForm))
//        {
//            if (taxForm.TaxFormType.Equals("B"))
//            {
//                TaxFormBFGPanel1.Visible = true;
//                TaxFormBFGPanel2.Visible = true;
//                TaxFormBFGPanel3.Visible = true;

//                TaxFormBPanel.Visible = true;

//                TaxFormEPanel1.Visible = false;
//                TaxFormEPanel2.Visible = false;

//                TaxFormFPanel1.Visible=false;
//                TaxFormFPanel2.Visible=false;
//                TaxFormGPanel1.Visible = false;
//                TaxFormGPanel2.Visible = false;
//                TaxFormGPanel3.Visible = false;
//                TaxFormGPanel4.Visible = false;
//                TaxFormFGPanel.Visible = false;
//                TaxFormEGPanel1.Visible = false;
//            }
//            if (taxForm.TaxFormType.Equals("F"))
//            {
//                TaxFormBFGPanel1.Visible = true;
//                TaxFormBFGPanel2.Visible = true;
//                TaxFormBFGPanel3.Visible = true;

//                TaxFormBPanel.Visible = false;
                
//                TaxFormEPanel1.Visible = false;
//                TaxFormEPanel2.Visible = false;
                
//                TaxFormFPanel1.Visible = true;
//                TaxFormFPanel2.Visible = true;
//                TaxFormFPanel3.Visible = true;
//                TaxFormGPanel1.Visible = false;
//                TaxFormGPanel2.Visible = false;
//                TaxFormGPanel3.Visible = false;
//                TaxFormGPanel4.Visible = false;
//                TaxFormFGPanel.Visible = true;
//                TaxFormEGPanel1.Visible = false;
//            }
//            if (taxForm.TaxFormType.Equals("G"))
//            {
//                TaxFormBFGPanel1.Visible = true;
//                TaxFormBFGPanel2.Visible = true;
//                TaxFormBFGPanel3.Visible = true;

//                TaxFormBPanel.Visible = false;

//                TaxFormEPanel1.Visible = false;
//                TaxFormEPanel2.Visible = false;

//                TaxFormFPanel1.Visible = false;
//                TaxFormFPanel2.Visible = false;
//                TaxFormFPanel3.Visible = false;
//                TaxFormGPanel1.Visible = true;
//                TaxFormGPanel2.Visible = true;
//                TaxFormGPanel3.Visible = true;
//                TaxFormGPanel4.Visible = true;
//                TaxFormFGPanel.Visible = true;
//                TaxFormEGPanel1.Visible = true;
//            }
//            if (taxForm.TaxFormType.Equals("E"))
//            {
//                TaxFormBFGPanel1.Visible = false;
//                TaxFormBFGPanel2.Visible = false;
//                TaxFormBFGPanel3.Visible = false;

//                TaxFormBPanel.Visible = false;

//                TaxFormEPanel1.Visible = true;
//                TaxFormEPanel2.Visible = true;

//                TaxFormFPanel1.Visible = false;
//                TaxFormFPanel2.Visible = false;
//                TaxFormGPanel1.Visible = false;
//                TaxFormGPanel2.Visible = false;
//                TaxFormGPanel3.Visible = false;
//                TaxFormGPanel4.Visible = false;
//                TaxFormFGPanel.Visible = false;
//                TaxFormEGPanel1.Visible = true;
//            }
//        }

//        //TaxEmpHoldAmountPanel.Visible = false;
//        //TaxEmpReasonForNotHoldPanel.Visible = false;
//        //if (obj.TaxEmpIsMoneyHoldByOrdinance != null)
//        //{
//        //    if (obj.TaxEmpIsMoneyHoldByOrdinance.Equals("Y"))
//        //        TaxEmpHoldAmountPanel.Visible = true;
//        //    if (obj.TaxEmpIsMoneyHoldByOrdinance.Equals("N"))
//        //        TaxEmpReasonForNotHoldPanel.Visible = true;
//        //}
//        TaxEmpIsMoneyHoldByOrdinance_SelectedIndexChanged(TaxEmpIsMoneyHoldByOrdinance, null);
//        TaxEmpReasonForDepartureReason_SelectedIndexChanged(TaxEmpReasonForDepartureReason,null);
//        TaxEmpIsEEReturnHK_SelectedIndexChanged(TaxEmpIsEEReturnHK,null);
//        TaxEmpIsShareOptionsGrant_SelectedIndexChanged(TaxEmpIsShareOptionsGrant, null);
//        return true;
//    }
//    protected void Save_Click(object sender, EventArgs e)
//    {
//        ETaxEmp c = new ETaxEmp();

//        Hashtable values = new Hashtable();
//        binding.toValues(values);

//        PageErrors errors = PageErrors.getErrors(db, Page.Master);
//        errors.clear();


//        db.validate(errors, values);

//        if (!errors.isEmpty())
//        {
//            if (CurID > 0)
//            {
//                loadObject();
//            }
//            return;
//        }

//        db.parse(values, c);


//        if (!errors.isEmpty())
//        {
//            if (CurID > 0)
//            {
//                loadObject();
//            }
//            return;
//        }

//        int taxFormYear=0;
//        ETaxForm taxForm = new ETaxForm();
//        taxForm.TaxFormID = c.TaxFormID;
//        if (ETaxForm.db.select(dbConn, taxForm))
//        {
//            taxFormYear = taxForm.TaxFormYear;
//        }


//        DateTime dtStartDate = new DateTime(taxFormYear - 1, 4, 1);
//        DateTime dtEndDate = new DateTime(taxFormYear, 3, 31);

//        if (dtStartDate > c.TaxEmpStartDate || dtEndDate < c.TaxEmpStartDate)
//            errors.addError("TaxEmpStartDate", HROne.Translation.PageErrorMessage.ERROR_INCORRECT_DATE_RANGE);

//        if (dtStartDate > c.TaxEmpEndDate || dtEndDate < c.TaxEmpEndDate)
//            errors.addError("TaxEmpEndDate", HROne.Translation.PageErrorMessage.ERROR_INCORRECT_DATE_RANGE);

//        if (c.TaxEmpStartDate > c.TaxEmpEndDate)
//            errors.addError(HROne.Translation.PageErrorMessage.ERROR_DATE_END_TOO_EARLY);

//        if (!errors.isEmpty())
//        {
//            if (CurID > 0)
//            {
//             //   loadObject();
//            }
//            return;
//        }

//        WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);

//        if (CurID < 0)
//        {
//            //            Utils.MarkCreate(Session, c);

//            db.insert(dbConn, c);
//            CurID = c.TaxEmpID;
//            //            url = Utils.BuildURL(-1, CurID);
//        }
//        else
//        {
//            //            Utils.Mark(Session, c);
//            db.update(dbConn, c);
//        }
//        WebUtils.EndFunction(dbConn);
//        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Taxation_Adjustment_View.aspx?TaxEmpID=" + c.TaxEmpID );
//    }

//    protected void Delete_Click(object sender, EventArgs e)
//    {
//        int taxFormID=-1;
//        ETaxEmp c = new ETaxEmp();
//        c.TaxEmpID = CurID;
//        if (db.select(dbConn, c))
//        {
//            taxFormID = c.TaxFormID;
//        }

//        WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
//        db.delete(dbConn, c);
//        {
//            DBFilter taxEmpPaymentFilter = new DBFilter();
//            taxEmpPaymentFilter.add(new Match("TaxEmpID", CurID));
//            ArrayList taxEmpPaymentList = ETaxEmpPayment.db.select(dbConn, taxEmpPaymentFilter);
//            foreach (ETaxEmpPayment taxEmpPayment in taxEmpPaymentList)
//                ETaxEmpPayment.db.delete(dbConn, taxEmpPayment);
//        }
//        {
//            DBFilter taxEmpPoRFilter = new DBFilter();
//            taxEmpPoRFilter.add(new Match("TaxEmpID", CurID));
//            ArrayList taxEmpPoRList = ETaxEmpPlaceOfResidence.db.select(dbConn, taxEmpPoRFilter);
//            foreach (ETaxEmpPlaceOfResidence taxEmpPoR in taxEmpPoRList)
//                ETaxEmpPlaceOfResidence.db.delete(dbConn, taxEmpPoR);
//        }
//        WebUtils.EndFunction(dbConn);

//        ETaxForm taxForm = new ETaxForm();
//        taxForm.TaxFormID = taxFormID;
//        ETaxForm.db.select(dbConn, taxForm);

//        if (taxForm.TaxFormType.Equals("B", StringComparison.CurrentCultureIgnoreCase))
//            TaxationGeneration.RearrangeSheetNo(dbConn, taxFormID);
//        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Taxation_Adjustment_List.aspx");
//    }
//    protected void TaxEmpIsMoneyHoldByOrdinance_SelectedIndexChanged(object sender, EventArgs e)
//    {
//        TaxEmpHoldAmountPanel.Visible = false;
//        TaxEmpReasonForNotHoldPanel.Visible = false;

//        DropDownList dropdownbox = (DropDownList)sender;
//        if (dropdownbox.SelectedValue.Equals("Y"))
//            TaxEmpHoldAmountPanel.Visible=true;
//        if (dropdownbox.SelectedValue.Equals("N"))
//            TaxEmpReasonForNotHoldPanel.Visible = true;


//    }
//    protected void TaxEmpReasonForDepartureReason_SelectedIndexChanged(object sender, EventArgs e)
//    {

//        DropDownList dropdownlist = (DropDownList)sender;
//        if (dropdownlist.SelectedValue.Equals("Other"))
//            TaxEmpReasonForDepartureOtherReasonPanel.Visible = true;
//        else
//            TaxEmpReasonForDepartureOtherReasonPanel.Visible = false;


//    }
//    protected void TaxEmpIsEEReturnHK_SelectedIndexChanged(object sender, EventArgs e)
//    {

//        DropDownList dropdownlist = (DropDownList)sender;
//        if (dropdownlist.SelectedValue.Equals("Y"))
//            TaxEmpEEReturnHKDatePanel.Visible = true;
//        else
//            TaxEmpEEReturnHKDatePanel.Visible = false;


//    }
//    protected void TaxEmpIsShareOptionsGrant_SelectedIndexChanged(object sender, EventArgs e)
//    {

//        DropDownList dropdownlist = (DropDownList)sender;
//        if (dropdownlist.SelectedValue.Equals("Y"))
//            TaxEmpIsShareOptionsGrantPanel.Visible = true;
//        else
//            TaxEmpIsShareOptionsGrantPanel.Visible = false;


//    }
//    protected void Back_Click(object sender, EventArgs e)
//    {
//        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Taxation_Adjustment_View.aspx?TaxEmpID=" + TaxEmpID.Value);
//    }
//}
