using System;
using System.Data;
using System.Configuration;
using System.Collections;
using HROne.DataAccess;
using System.Xml;
//using perspectivemind.validation;
using HROne.Lib.Entities;

namespace HROne.Taxation
{
    /// <summary>
    /// Summary description for TaxationProcess
    /// </summary>
    public class TaxationGeneration
    {

        public static int GetOrCreateTaxFormID(DatabaseConnection dbConn, int TaxCompID, int TaxFormYear, string TaxFormType)
        {
            DBFilter taxFilter = new DBFilter();
            taxFilter.add(new Match("TaxCompID", TaxCompID));
            taxFilter.add(new Match("TaxFormYear", TaxFormYear));
            taxFilter.add(new Match("TaxFormType", TaxFormType));
            ArrayList taxForms = ETaxForm.db.select(dbConn, taxFilter);

            ETaxForm taxForm = null;
            if (taxForms.Count > 0)
                taxForm = (ETaxForm)taxForms[0];
            else
                taxForm = new ETaxForm();

            ETaxCompany taxComp = new ETaxCompany();
            taxComp.TaxCompID = TaxCompID;
            ETaxCompany.db.select(dbConn, taxComp);

            taxForm.TaxCompID = TaxCompID;
            taxForm.TaxFormBatchNo = 0;
            taxForm.TaxFormDesignation = taxComp.TaxCompDesignation;
            taxForm.TaxFormEmployerName = taxComp.TaxCompEmployerName;
            taxForm.TaxFormEmployerAddress = taxComp.TaxCompEmployerAddress;
            taxForm.TaxFormERN = taxComp.TaxCompERN;
            taxForm.TaxFormSection = taxComp.TaxCompSection;
            taxForm.TaxFormYear = TaxFormYear;
            taxForm.TaxFormSubmissionDate = AppUtils.ServerDateTime();
            taxForm.TaxFormType = TaxFormType;

            if (taxForm.TaxFormID == 0)
                ETaxForm.db.insert(dbConn, taxForm);
            else
                ETaxForm.db.update(dbConn, taxForm);
            return taxForm.TaxFormID;
        }

        public static void GenerationFormTaxation(DatabaseConnection dbConn, int TaxFormID, int EmpID, int UserID)
        {
            ArrayList taxEmpPaymentList = new ArrayList();
            ArrayList taxEmpPoRList = new ArrayList();


            ETaxForm taxForm = new ETaxForm();
            taxForm.TaxFormID = TaxFormID;

            EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
            empInfo.EmpID = EmpID;
            if (EEmpPersonalInfo.db.select(dbConn, empInfo) && ETaxForm.db.select(dbConn, taxForm))
            {
                DateTime dtStart = new DateTime(taxForm.TaxFormYear - 1, 4, 1);
                DateTime dtEnd = new DateTime(taxForm.TaxFormYear, 3, 31);


                ETaxEmp taxEmp = null;

                DBFilter taxEmpFilter = new DBFilter();
                taxEmpFilter.add(new Match("EmpID", empInfo.EmpID));
                taxEmpFilter.add(new Match("TaxFormID", TaxFormID));
                ArrayList tmpTaxEmps = ETaxEmp.db.select(dbConn, taxEmpFilter);
                if (tmpTaxEmps.Count > 0)
                    taxEmp = (ETaxEmp)tmpTaxEmps[0];
                else
                    taxEmp = new ETaxEmp();

                if (empInfo.MasterEmpID > 0 && empInfo.EmpIsCombineTax)
                {
                    if (taxEmp.TaxEmpID > 0)
                    {
                        DBFilter deleteTaxEmpFilter = new DBFilter();
                        deleteTaxEmpFilter.add(new Match("TaxEmpID", taxEmp.TaxEmpID));
                        ArrayList deleteTaxEmpPaymentList = ETaxEmpPayment.db.select(dbConn, deleteTaxEmpFilter);
                        foreach (ETaxEmpPayment taxEmpPayment in deleteTaxEmpPaymentList)
                            ETaxEmpPayment.db.delete(dbConn, taxEmpPayment);
                        ArrayList deletedTaxEmpPlaceOfResidenceList = ETaxEmpPlaceOfResidence.db.select(dbConn, deleteTaxEmpFilter);
                        foreach (ETaxEmpPlaceOfResidence taxEmpPlaceOfResidence in deletedTaxEmpPlaceOfResidenceList)
                            ETaxEmpPlaceOfResidence.db.delete(dbConn, taxEmpPlaceOfResidence);
                        ETaxEmp.db.delete(dbConn, taxEmp);
                    }
                    return;
                } 
                
                taxEmp.EmpID = empInfo.EmpID;
                taxEmp.TaxEmpSheetNo = 0;
                taxEmp.TaxEmpHKID = empInfo.EmpHKID.Trim();//.Replace("(", "").Replace(")", "");
                //if (taxEmp.TaxEmpHKID.Length < 9)
                //    taxEmp.TaxEmpHKID = taxEmp.TaxEmpHKID.PadLeft(9);
                taxEmp.TaxEmpStatus = "O";
                taxEmp.TaxEmpSurname = empInfo.EmpEngSurname.ToUpper().Replace(" ", "");
                taxEmp.TaxEmpOtherName = empInfo.EmpEngOtherName.ToUpper().Replace("  ", " ");                           
                if (ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_TAXATION_USE_CHINESE_NAME).Equals("Y"))
                    taxEmp.TaxEmpChineseName = empInfo.EmpChiFullName;
                else
                    taxEmp.TaxEmpChineseName = string.Empty;
                taxEmp.TaxEmpSex = empInfo.EmpGender;

                if (taxEmp.TaxEmpHKID.Replace("()", "").Trim().Length == 0)
                {
                    taxEmp.TaxEmpPassportNo = empInfo.EmpPassportNo;
                    taxEmp.TaxEmpIssuedCountry = empInfo.EmpPassportIssuedCountry;
                }
                if (string.IsNullOrEmpty(taxEmp.TaxEmpHKID.Replace("()", "").Trim()) && (string.IsNullOrEmpty(taxEmp.TaxEmpPassportNo.Trim()) || string.IsNullOrEmpty(taxEmp.TaxEmpIssuedCountry.Trim())))
                    throw new Exception(HROne.Common.WebUtility.GetLocalizedStringByCode("ERROR_HKID_PASSPORTCOUNTRY_REQUIRED", "Either HKID or Passport Number with Issued Country is required") + "(" + HROne.Common.WebUtility.GetLocalizedString("EmpNo") + ": " + empInfo.EmpNo + ")");

                if (empInfo.EmpMaritalStatus.Equals("Married", StringComparison.CurrentCultureIgnoreCase))
                {
                    taxEmp.TaxEmpMartialStatus = "2";

                    DBFilter empSpouseFilter = new DBFilter();
                    empSpouseFilter.add(new Match("EmpID", empInfo.EmpID));
                    ArrayList empSpouseList = EEmpSpouse.db.select(dbConn, empSpouseFilter);
                    if (empSpouseList.Count > 0)
                    {
                        EEmpSpouse empSpouse = (EEmpSpouse)empSpouseList[0];
                        taxEmp.TaxEmpSpouseName = empSpouse.EmpSpouseSurname.ToUpper().Replace(" ", "") + ", " + empSpouse.EmpSpouseOtherName.ToUpper().Replace("  ", " ");
                        taxEmp.TaxEmpSpouseHKID = empSpouse.EmpSpouseHKID.Trim();//.Replace("(", "").Replace(")", "");
                        ////if (taxEmp.TaxEmpSpouseHKID.Length < 9)
                        ////    taxEmp.TaxEmpSpouseHKID = taxEmp.TaxEmpSpouseHKID.PadLeft(9 - taxEmp.TaxEmpHKID.Length, ' ');

                        if (taxEmp.TaxEmpSpouseHKID.Replace("()", "").Trim().Length == 0)
                        {
                            taxEmp.TaxEmpSpousePassportNo = empSpouse.EmpSpousePassportNo;
                            taxEmp.TaxEmpSpouseIssuedCountry = empSpouse.EmpSpousePassportIssuedCountry;
                        }
                    }
                }
                else
                {
                    taxEmp.TaxEmpMartialStatus = "1";
                }
                taxEmp.TaxEmpResAddr = empInfo.EmpResAddr.Replace("\r", " ").Replace("\n", " ").Replace("  ", " ").PadRight(90, ' ').Substring(0, 90).Trim();
                taxEmp.TaxEmpResAddrAreaCode = empInfo.EmpResAddrAreaCode;
                taxEmp.TaxEmpCorAddr = empInfo.EmpCorAddr.Replace("\r", " ").Replace("\n", " ").Replace("  ", " ").PadRight(60, ' ').Substring(0, 60).Trim();

                DBFilter empPosFilter = new DBFilter();
                empPosFilter.add(new Match("EmpID", empInfo.EmpID));
                empPosFilter.add(new Match("EmpPosEffFr", "<=", dtEnd));
                OR orPosEffTerms = new OR();
                orPosEffTerms.add(new Match("EmpPosEffTo", ">=", dtStart));
                orPosEffTerms.add(new NullTerm("EmpPosEffTo"));
                empPosFilter.add(orPosEffTerms);
                empPosFilter.add("EmpPosEffFr", false);
                ArrayList empPosList = EEmpPositionInfo.db.select(dbConn, empPosFilter);

                if (empPosList.Count > 0)
                {
                    EEmpPositionInfo empPos = (EEmpPositionInfo)empPosList[0];
                    EPosition position = new EPosition();
                    position.PositionID = empPos.PositionID;
                    EPosition.db.select(dbConn, position);
                    if (string.IsNullOrEmpty(position.PositionCapacity))
                    {
                        taxEmp.TaxEmpCapacity = position.PositionDesc.Trim();
                        if (taxEmp.TaxEmpCapacity.Length > 40)
                            taxEmp.TaxEmpCapacity = taxEmp.TaxEmpCapacity.Substring(0, 40).Trim();
                    }
                    else
                        taxEmp.TaxEmpCapacity = position.PositionCapacity;

                }
                taxEmp.TaxEmpPartTimeEmployer = string.Empty;

                taxEmp.TaxEmpStartDate = dtStart < empInfo.EmpDateOfJoin ? empInfo.EmpDateOfJoin : dtStart;

                DBFilter empTermFilter = new DBFilter();
                empTermFilter.add(new Match("EmpID", empInfo.EmpID));
                empTermFilter.add(new Match("EmpTermLastDate", "<=", dtEnd));
                empTermFilter.add(new Match("EmpTermLastDate", ">=", dtStart));
                empTermFilter.add("EmpTermLastDate", false);


                ArrayList empTermList = EEmpTermination.db.select(dbConn, empTermFilter);
                if (empTermList.Count == 0)
                    taxEmp.TaxEmpEndDate = dtEnd;
                else
                {
                    EEmpTermination empTermation = (EEmpTermination)empTermList[0];
                    taxEmp.TaxEmpEndDate = empTermation.EmpTermLastDate;
                    ECessationReason reason = new ECessationReason();
                    reason.CessationReasonID = empTermation.CessationReasonID;
                    if (ECessationReason.db.select(dbConn, reason))
                        taxEmp.TaxEmpCessationReason = reason.CessationReasonDesc;
                    else
                        taxEmp.TaxEmpCessationReason = string.Empty;



                }
                DBFilter empPlaceOfResidenceFilter = new DBFilter();
                empPlaceOfResidenceFilter.add(new Match("EmpID", empInfo.EmpID));
                empPlaceOfResidenceFilter.add(new Match("EmpPoRFrom", "<=", dtEnd));
                OR orPoRToTerms = new OR();
                orPoRToTerms.add(new Match("EmpPoRTo", ">=", dtStart));
                orPoRToTerms.add(new NullTerm("EmpPoRTo"));
                empPlaceOfResidenceFilter.add(orPoRToTerms);
                empPlaceOfResidenceFilter.add("EmpPoRFrom", true);

                ArrayList empPlaceOfResidenceList = EEmpPlaceOfResidence.db.select(dbConn, empPlaceOfResidenceFilter);
                if (empPlaceOfResidenceList.Count > 0)
                {
                    foreach (EEmpPlaceOfResidence empPoR in empPlaceOfResidenceList)
                    {
                        //taxEmp.TaxEmpPlaceOfResidenceIndicator = 1;
                        ETaxEmpPlaceOfResidence taxEmpPoR = new ETaxEmpPlaceOfResidence();
                        taxEmpPoR.TaxEmpPlaceAddress = empPoR.EmpPoRPropertyAddr.Replace("\r", " ").Replace("\n", " ").Replace("  ", " ");
                        taxEmpPoR.TaxEmpPlaceEERent = Convert.ToInt32(empPoR.EmpPoRPayToLandEE);
                        taxEmpPoR.TaxEmpPlaceEERentRefunded = Convert.ToInt32(empPoR.EmpPoRRefundToEE);
                        taxEmpPoR.TaxEmpPlaceERRent = Convert.ToInt32(empPoR.EmpPoRPayToLandER);
                        taxEmpPoR.TaxEmpPlaceERRentByEE = Convert.ToInt32(empPoR.EmpPoRPayToERByEE);
                        taxEmpPoR.TaxEmpPlaceNature = empPoR.EmpPoRNature;
                        taxEmpPoR.TaxEmpPlacePeriodFr = empPoR.EmpPoRFrom < taxEmp.TaxEmpStartDate ? taxEmp.TaxEmpStartDate : empPoR.EmpPoRFrom;
                        if (empPoR.EmpPoRTo.Ticks != 0)
                            taxEmpPoR.TaxEmpPlacePeriodTo = empPoR.EmpPoRTo > taxEmp.TaxEmpEndDate ? taxEmp.TaxEmpEndDate : empPoR.EmpPoRTo;
                        else
                            taxEmpPoR.TaxEmpPlacePeriodTo = taxEmp.TaxEmpEndDate;
                        taxEmpPoR.RelatedTaxEmp = taxEmp;
                        taxEmpPoRList.Add(taxEmpPoR);


                    }
                }
                //else
                //    taxEmp.TaxEmpPlaceOfResidenceIndicator = 0;

                taxEmp.TaxEmpOvearseasIncomeIndicator = 0;
                // Start 0000020, KuangWei, 2014-07-22
                taxEmp.TaxEmpSumWithheldIndicator = 0;
                taxEmp.TaxEmpSumWithheldAmount = string.Empty;
                // End 0000020, KuangWei, 2014-07-22
                taxEmp.TaxEmpOverseasCompanyAmount = string.Empty;
                taxEmp.TaxEmpOverseasCompanyName = string.Empty;
                taxEmp.TaxEmpOverseasCompanyAddress = string.Empty;
                taxEmp.TaxEmpTaxFileNo = string.Empty;

                taxEmp.TaxEmpIsERBearTax = "N";
                taxEmp.TaxEmpIsShareOptionsGrant = "N";

                ArrayList taxPaymentList = TaxPaymentList(dbConn, taxForm.TaxFormType);
                foreach (ETaxPayment taxPaymentType in taxPaymentList)
                {
                    ETaxEmpPayment taxEmpPayment = null;
                    if (taxEmp.TaxEmpID != 0)
                    {
                        DBFilter taxEmpPaymentFilter = new DBFilter();
                        taxEmpPaymentFilter.add(new Match("TaxEmpID", taxEmp.TaxEmpID));
                        taxEmpPaymentFilter.add(new Match("TaxPayID", taxPaymentType.TaxPayID));
                        ArrayList oldTaxEmpPaymentList = ETaxEmpPayment.db.select(dbConn, taxEmpPaymentFilter);

                        if (oldTaxEmpPaymentList.Count > 0)
                            taxEmpPayment = (ETaxEmpPayment)oldTaxEmpPaymentList[0];
                        else
                            taxEmpPayment = new ETaxEmpPayment();

                    }
                    else
                        taxEmpPayment = new ETaxEmpPayment();
                    taxEmpPayment.TaxPayID = taxPaymentType.TaxPayID;
                    if (taxPaymentType.TaxPayIsShowNature.Equals("Y", StringComparison.CurrentCultureIgnoreCase))
                        taxEmpPayment.TaxEmpPayNature = taxPaymentType.TaxPayNature;
                    taxEmpPayment.TaxEmpPayPeriodFr = taxEmp.TaxEmpStartDate;
                    taxEmpPayment.TaxEmpPayPeriodTo = taxEmp.TaxEmpEndDate;
                    taxEmpPayment.RelatedTaxEmp = taxEmp;


                    if (taxForm.TaxFormType.Equals("E", StringComparison.CurrentCultureIgnoreCase))
                    {

                        if (empPosList.Count > 0)
                        {
                            //  Get First terms
                            EEmpPositionInfo empPos = (EEmpPositionInfo)empPosList[empPosList.Count - 1];

                            double totalTaxPaymentAmount = 0;
                            DBFilter empRPFilter = new DBFilter();

                            DBFilter taxPaymentFilter = new DBFilter();
                            taxPaymentFilter.add(new Match("TaxPayID", taxPaymentType.TaxPayID));
                            empRPFilter.add(new IN("PayCodeID", "Select PaymentCodeID from TaxPaymentMap", taxPaymentFilter));
                            empRPFilter.add(new Match("EmpRPEffFr", empPos.EmpPosEffFr));
                            empRPFilter.add(new Match("EmpID", empInfo.EmpID));


                            ArrayList empRPList = EEmpRecurringPayment.db.select(dbConn, empRPFilter);
                            foreach (EEmpRecurringPayment empRP in empRPList)
                            {
                                if (empRP.EmpRPUnit.Equals("P"))
                                {
                                    EPayrollGroup payGroup = new EPayrollGroup();
                                    payGroup.PayGroupID = empPos.PayGroupID;
                                    if (EPayrollGroup.db.select(dbConn, payGroup))
                                    {
                                        if (payGroup.PayGroupFreq.Equals("M", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            totalTaxPaymentAmount += empRP.EmpRPAmount;
                                        }
                                        else if (payGroup.PayGroupFreq.Equals("S", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            totalTaxPaymentAmount += empRP.EmpRPAmount * 2;
                                        }
                                    }
                                    else
                                        //  Assume monthly payment
                                        totalTaxPaymentAmount += empRP.EmpRPAmount;

                                }
                            }
                            taxEmpPayment.TaxEmpPayAmount = Convert.ToInt64(Math.Truncate(HROne.CommonLib.GenericRoundingFunctions.RoundingTo(totalTaxPaymentAmount, 2, 2)));
                            taxEmpPaymentList.Add(taxEmpPayment);

                        }


                    }
                    else
                    {
                        double totalTaxPaymentAmount = 0;
                        DBFilter paymentRecordFilter = new DBFilter();

                        DBFilter taxPaymentFilter = new DBFilter();
                        taxPaymentFilter.add(new Match("TaxPayID", taxPaymentType.TaxPayID));
                        paymentRecordFilter.add(new IN("PaymentCodeID", "Select PaymentCodeID from TaxPaymentMap", taxPaymentFilter));

                        DBFilter empPayrollFilter = new DBFilter();
                        empPayrollFilter.add(empInfo.GetAllRoleEmpIDTerms(dbConn, "EmpID", EEmpPersonalInfo.RoleFilterOptionEnum.Tax));

                        DBFilter payrollPeriodFilter = new DBFilter();
                        payrollPeriodFilter.add(new Match("PayPeriodTo", "<=", dtEnd));
                        payrollPeriodFilter.add(new Match("PayPeriodTo", ">=", dtStart));
                        empPayrollFilter.add(new IN("PayPeriodID", "Select payperiodID from PayrollPeriod", payrollPeriodFilter));

                        paymentRecordFilter.add(new IN("EmpPayrollID", "Select EmpPayrollID from empPayroll", empPayrollFilter));
                        ArrayList paymentRecordList = EPaymentRecord.db.select(dbConn, paymentRecordFilter);
                        foreach (EPaymentRecord paymentRecord in paymentRecordList)
                        {
                            totalTaxPaymentAmount += paymentRecord.PayRecActAmount;
                        }
                        taxEmpPayment.TaxEmpPayAmount = Convert.ToInt64(Math.Truncate(HROne.CommonLib.GenericRoundingFunctions.RoundingTo(totalTaxPaymentAmount, 2, 2)));
                        taxEmpPaymentList.Add(taxEmpPayment);
                    }
                }


                taxEmp.TaxFormID = TaxFormID;
                taxEmp.TaxEmpGeneratedDate = AppUtils.ServerDateTime();
                taxEmp.TaxEmpGeneratedByUserID = UserID;
                if (taxEmp.TaxEmpID == 0)
                    ETaxEmp.db.insert(dbConn, taxEmp);
                else
                    ETaxEmp.db.update(dbConn, taxEmp); 
                DBFilter taxPoRDeleteFilter = new DBFilter();
                taxPoRDeleteFilter.add(new Match("TaxEmpID", taxEmp.TaxEmpID));
                ETaxEmpPlaceOfResidence.db.delete(dbConn, taxPoRDeleteFilter);

                foreach (ETaxEmpPayment taxEmpPayment in taxEmpPaymentList)
                {
                    taxEmpPayment.TaxEmpID = taxEmpPayment.RelatedTaxEmp.TaxEmpID;
                    if (taxEmpPayment.TaxEmpPayAmount != 0)
                    {
                        if (taxEmpPayment.TaxEmpPayID == 0)
                            ETaxEmpPayment.db.insert(dbConn, taxEmpPayment);
                        else
                            ETaxEmpPayment.db.update(dbConn, taxEmpPayment);
                    }
                    else
                        if (taxEmpPayment.TaxEmpPayID != 0)
                            ETaxEmpPayment.db.delete(dbConn, taxEmpPayment);

                }
                foreach (ETaxEmpPlaceOfResidence taxEmpPlaceOfResidence in taxEmpPoRList)
                {
                    taxEmpPlaceOfResidence.TaxEmpID = taxEmpPlaceOfResidence.RelatedTaxEmp.TaxEmpID;
                    if (taxEmpPlaceOfResidence.TaxEmpPlaceID == 0)
                        ETaxEmpPlaceOfResidence.db.insert(dbConn, taxEmpPlaceOfResidence);
                    else
                        ETaxEmpPlaceOfResidence.db.update(dbConn, taxEmpPlaceOfResidence);
                }
            }

        }

        //public static void GenerationFormTaxation(DatabaseConnection dbConn, int TaxCompID, int TaxFormYear, string TaxFormType, ArrayList EmpList,int UserID)
        //{

        //    int taxFormID = GetOrCreateTaxFormID(TaxCompID, TaxFormYear, TaxFormType);



        //    foreach (EEmpPersonalInfo empInfo in EmpList)
        //    {
        //        GenerationFormTaxation(dbConn, taxFormID, empInfo.EmpID, UserID);
        //    }
        //    if (TaxFormType.Equals("B", StringComparison.CurrentCultureIgnoreCase))
        //        RearrangeSheetNo(taxFormID);

        //}

        public static void RearrangeSheetNo(DatabaseConnection dbConn, int TaxFormID)
        {
            DBFilter taxEmpFilter = new DBFilter();
            taxEmpFilter.add(new Match("TaxFormID",TaxFormID));
            taxEmpFilter.add(new Match("TaxEmpSheetNo","<=",900000));
            //taxEmpFilter.add("TaxEmpSurname",true);
            //taxEmpFilter.add("TaxEmpOtherName",true);
            ArrayList taxEmpList = ETaxEmp.db.select(dbConn, taxEmpFilter);
            taxEmpList.Sort(new TaxEmpCompareByName(true));
            int intSheetNumCount=0;
            foreach (ETaxEmp taxEmp in taxEmpList)
            {
                intSheetNumCount++;
                if (taxEmp.TaxEmpSheetNo != intSheetNumCount)
                {
                    taxEmp.TaxEmpSheetNo = intSheetNumCount;
                    ETaxEmp.db.update(dbConn, taxEmp);
                }
            }
        }

        // Start 0000020, KuangWei, 2014-07-22
        public static void RearrangeSheetNoForM(DatabaseConnection dbConn, int TaxFormID)
        {
            DBFilter taxEmpFilter = new DBFilter();
            taxEmpFilter.add(new Match("TaxFormID", TaxFormID));
            ArrayList taxEmpList = ETaxEmp.db.select(dbConn, taxEmpFilter);
            taxEmpList.Sort(new TaxEmpCompareByName(true));
            int intSheetNumCount = 900000;
            foreach (ETaxEmp taxEmp in taxEmpList)
            {
                intSheetNumCount++;
                if (taxEmp.TaxEmpSheetNo != intSheetNumCount)
                {
                    taxEmp.TaxEmpSheetNo = intSheetNumCount;
                    ETaxEmp.db.update(dbConn, taxEmp);
                }
            }
        }
        // End 0000020, KuangWei, 2014-07-22

        private static ArrayList TaxPaymentList(DatabaseConnection dbConn, string TaxFormType)
        {
            DBFilter TaxPaymentFilter= new DBFilter();
            TaxPaymentFilter.add(new Match("TaxFormType",TaxFormType));
            return ETaxPayment.db.select(dbConn, TaxPaymentFilter);
        }

        public static DataSet.Taxation_IR56B_DataSet GenerateTaxationDataSet(DatabaseConnection dbConn, int TaxFormID, ArrayList TaxEmpList)
        {
            DataSet.Taxation_IR56B_DataSet dataSet = new DataSet.Taxation_IR56B_DataSet();
            ETaxForm taxForm = new ETaxForm();
            taxForm.TaxFormID = TaxFormID;
            if (ETaxForm.db.select(dbConn, taxForm))
            {
                DataSet.Taxation_IR56B_DataSet.TaxationHeaderDataTable taxHeaderTable = dataSet.TaxationHeader;
                DataSet.Taxation_IR56B_DataSet.IR56BDetailDataTable taxDetailTable = dataSet.IR56BDetail;
                DataSet.Taxation_IR56B_DataSet.TaxationHeaderRow taxHeaderRow = taxHeaderTable.NewTaxationHeaderRow();

                ETaxCompany taxComp = new ETaxCompany();
                taxComp.TaxCompID = taxForm.TaxCompID;
                if (ETaxCompany.db.select(dbConn, taxComp))
                {
                    //  Use up-to-date Tax Company Information to override generated information.
                    taxHeaderRow.Section = taxComp.TaxCompSection;
                    taxHeaderRow.ERN = taxComp.TaxCompERN;
                    taxHeaderRow.EmployerName = taxComp.TaxCompEmployerName;
                    taxHeaderRow.EmployerAddress = taxComp.TaxCompEmployerAddress;
                    taxHeaderRow.Designation = taxComp.TaxCompDesignation;
                }
                else
                {

                    taxHeaderRow.Section = taxForm.TaxFormSection;
                    taxHeaderRow.ERN = taxForm.TaxFormERN;
                    taxHeaderRow.EmployerName = taxForm.TaxFormEmployerName;
                    taxHeaderRow.EmployerAddress = taxForm.TaxFormEmployerAddress;
                    taxHeaderRow.Designation = taxForm.TaxFormDesignation;
                }
                    taxHeaderRow.YearReturn = taxForm.TaxFormYear;
                    taxHeaderRow.BatchNo = taxForm.TaxFormBatchNo;
                    taxHeaderRow.SubmissionDate = taxForm.TaxFormSubmissionDate;
                taxHeaderRow.TaxFormID = taxForm.TaxFormID;
                taxHeaderRow.SheetNo = 0;
                taxHeaderRow.TotalIncome = 0;
                taxHeaderRow.TotalRecord = 0;

                taxHeaderTable.Rows.Add(taxHeaderRow);
                if (TaxEmpList == null)
                {
                    DBFilter taxEmpFilter = new DBFilter();
                    taxEmpFilter.add(new Match("TaxFormID", TaxFormID));
                    taxEmpFilter.add("TaxEmpSheetNo",true);
                    TaxEmpList = ETaxEmp.db.select(dbConn, taxEmpFilter);
                }
                foreach (ETaxEmp taxEmp in TaxEmpList)
                {
                    ETaxEmp.db.select(dbConn, taxEmp);
                    taxHeaderRow.TotalRecord++;
                    DataSet.Taxation_IR56B_DataSet.IR56BDetailRow taxDetailRow = taxDetailTable.NewIR56BDetailRow();
                    taxDetailRow.TaxEmpID = taxEmp.TaxEmpID;
                    taxDetailRow.TaxFormID = taxForm.TaxFormID;
                    taxDetailRow.SheetNo = taxEmp.TaxEmpSheetNo;
                    taxDetailRow.TaxFileNo = taxEmp.TaxEmpTaxFileNo;
                    if (!taxEmp.TaxEmpHKID.Equals("()"))
                        taxDetailRow.HKID = taxEmp.TaxEmpHKID;
                    else
                        taxDetailRow.HKID = string.Empty;
                    taxDetailRow.EmployeeSurname = taxEmp.TaxEmpSurname;
                    taxDetailRow.EmployeeOtherName = taxEmp.TaxEmpOtherName;
                    taxDetailRow.EmployeeChineseName = taxEmp.TaxEmpChineseName;
                    taxDetailRow.Sex = taxEmp.TaxEmpSex;
                    taxDetailRow.MaritalStatus = taxEmp.TaxEmpMartialStatus;
                    taxDetailRow.EmployeePassportNoIssueCountry = taxEmp.TaxEmpPassportNo + " " + taxEmp.TaxEmpIssuedCountry;
                    if (taxEmp.TaxEmpMartialStatus.Equals("2"))
                    {
                        taxDetailRow.SpouseName = taxEmp.TaxEmpSpouseName;
                        if (!string.IsNullOrEmpty(taxEmp.TaxEmpSpouseHKID))
                            if (!taxEmp.TaxEmpSpouseHKID.Equals("()"))
                                taxDetailRow.SpouseHKID = taxEmp.TaxEmpSpouseHKID;
                            else
                                taxDetailRow.SpouseHKID = string.Empty;
                        taxDetailRow.SpousePassportNoIssueCountry = taxEmp.TaxEmpSpousePassportNo + " " + taxEmp.TaxEmpSpouseIssuedCountry;
                    }
                    taxDetailRow.ResidentialAddress = taxEmp.TaxEmpResAddr;
                    taxDetailRow.ResidentialAddressAreaCode = taxEmp.TaxEmpResAddrAreaCode;
                    taxDetailRow.CorrespondenceAddress = taxEmp.TaxEmpCorAddr;
                    taxDetailRow.CapacityEmployed = taxEmp.TaxEmpCapacity;
                    taxDetailRow.PartTimeEmployer = taxEmp.TaxEmpPartTimeEmployer;
                    taxDetailRow.EmploymentStartDate = taxEmp.TaxEmpStartDate;
                    taxDetailRow.EmploymentEndDate = taxEmp.TaxEmpEndDate;
                    taxDetailRow.CessationReason = taxEmp.TaxEmpCessationReason;
                    taxDetailRow.TotalIncome = 0;

                    DBFilter taxEmpPaymentFilter = new DBFilter();
                    taxEmpPaymentFilter.add(new Match("TaxEmpID", taxEmp.TaxEmpID));
                    ArrayList taxEmpPaymentList = ETaxEmpPayment.db.select(dbConn, taxEmpPaymentFilter);
                    foreach (ETaxEmpPayment taxEmpPayment in taxEmpPaymentList)
                    {
                        ETaxPayment taxPayment = new ETaxPayment();
                        taxPayment.TaxPayID = taxEmpPayment.TaxPayID;
                        if (ETaxPayment.db.select(dbConn, taxPayment))
                        {
                            taxDetailRow["PeriodFr_" + taxPayment.TaxPayCode] = taxEmpPayment.TaxEmpPayPeriodFr;
                            taxDetailRow["PeriodTo_" + taxPayment.TaxPayCode] = taxEmpPayment.TaxEmpPayPeriodTo;
                            taxDetailRow["amount_" + taxPayment.TaxPayCode] = taxEmpPayment.TaxEmpPayAmount;
                            taxHeaderRow.TotalIncome += taxEmpPayment.TaxEmpPayAmount;
                            taxDetailRow.TotalIncome += taxEmpPayment.TaxEmpPayAmount;
                            // Start 0000020, KuangWei, 2014-08-18
                            if (taxPayment.TaxPayCode.StartsWith("k", StringComparison.CurrentCultureIgnoreCase)
                                || taxPayment.TaxPayCode == "Others (d)" || taxPayment.TaxPayCode == "Others (e)")                         
                            // End 0000020, KuangWei, 2014-08-18
                                taxDetailRow["Nature_" + taxPayment.TaxPayCode] = taxEmpPayment.TaxEmpPayNature;
                            
                        }

                    }
                    DBFilter taxEmpPoRFilter = new DBFilter();
                    taxEmpPoRFilter.add(new Match("TaxEmpID", taxEmp.TaxEmpID));
                    ArrayList taxEmpPoRList = ETaxEmpPlaceOfResidence.db.select(dbConn, taxEmpPoRFilter);
                    int iPoRCount = 0;
                    foreach (ETaxEmpPlaceOfResidence taxEmpPoR in taxEmpPoRList)
                    {
                        iPoRCount++;
                        if (iPoRCount <= 2)
                        {
                            taxDetailRow["PlaceOfResidenceAddress" + iPoRCount] = taxEmpPoR.TaxEmpPlaceAddress;
                            taxDetailRow["PlaceOfResidenceNature" + iPoRCount] = taxEmpPoR.TaxEmpPlaceNature;
                            taxDetailRow["PlaceOfResidenceFr" + iPoRCount] = taxEmpPoR.TaxEmpPlacePeriodFr;
                            taxDetailRow["PlaceOfResidenceTo" + iPoRCount] = taxEmpPoR.TaxEmpPlacePeriodTo;
                            taxDetailRow["PlaceOfResidenceRentByER" + iPoRCount] = taxEmpPoR.TaxEmpPlaceERRent;
                            taxDetailRow["PlaceOfResidenceRentByEE" + iPoRCount] = taxEmpPoR.TaxEmpPlaceEERent;
                            taxDetailRow["PlaceOfResidenceRefundedEE" + iPoRCount] = taxEmpPoR.TaxEmpPlaceEERentRefunded;
                            taxDetailRow["PlaceOfResidenceRentToERByEE" + iPoRCount] = taxEmpPoR.TaxEmpPlaceERRentByEE;
                        }
                    }
                    taxDetailRow.PlaceOfResidenceIndicator = taxEmpPoRList.Count > 0 ? 1 : 0; //taxEmp.TaxEmpPlaceOfResidenceIndicator;
                    taxDetailRow.OverseasIncomeIndicator = taxEmp.TaxEmpOvearseasIncomeIndicator;
                    // Start 0000020, KuangWei, 2014-07-22
                    taxDetailRow.SumWithheldIndicator = taxEmp.TaxEmpSumWithheldIndicator;
                    taxDetailRow.SumWithheldAmount = taxEmp.TaxEmpSumWithheldAmount;
                    // End 0000020, KuangWei, 2014-07-22
                    taxDetailRow.OverseasCompanyName = taxEmp.TaxEmpOverseasCompanyName;
                    taxDetailRow.OverseasCompanyAddress = taxEmp.TaxEmpOverseasCompanyAddress;
                    taxDetailRow.OverseasCompanyAmount = taxEmp.TaxEmpOverseasCompanyAmount;
                    taxDetailRow.NewEmployerNameddress = taxEmp.TaxEmpNewEmployerNameddress;
                    taxDetailRow.Remarks = taxEmp.TaxEmpRemark;
                    taxDetailRow.FutureCorAddr = taxEmp.TaxEmpFutureCorAddr;
                    taxDetailRow.LeaveHKDate = taxEmp.TaxEmpLeaveHKDate;
                    taxDetailRow.IsERBearTax = taxEmp.TaxEmpIsERBearTax;
                    taxDetailRow.IsMoneyHoldByOrdinance = taxEmp.TaxEmpIsMoneyHoldByOrdinance;
                    if (!string.IsNullOrEmpty(taxEmp.TaxEmpIsMoneyHoldByOrdinance))
                        if (taxEmp.TaxEmpIsMoneyHoldByOrdinance.Equals("Y"))
                            taxDetailRow.HoldAmount = taxEmp.TaxEmpHoldAmount;
                        else if (taxEmp.TaxEmpIsMoneyHoldByOrdinance.Equals("N"))
                            taxDetailRow.ReasonForNotHold = taxEmp.TaxEmpReasonForNotHold;
                    taxDetailRow.ReasonForDepartureReason = taxEmp.TaxEmpReasonForDepartureReason;
                    if (!string.IsNullOrEmpty(taxEmp.TaxEmpReasonForDepartureReason))
                        if (taxEmp.TaxEmpReasonForDepartureReason.Equals("Other"))
                            taxDetailRow.ReasonForDepartureOtherReason = taxEmp.TaxEmpReasonForDepartureOtherReason;
                    taxDetailRow.IsEEReturnHK = taxEmp.TaxEmpIsEEReturnHK;
                    if (!string.IsNullOrEmpty(taxEmp.TaxEmpIsEEReturnHK))
                        if (taxEmp.TaxEmpIsEEReturnHK.Equals("Y"))
                            if (!taxEmp.TaxEmpEEReturnHKDate.Ticks.Equals(0))
                                taxDetailRow.EEReturnHKDate = taxEmp.TaxEmpEEReturnHKDate;
                    taxDetailRow.IsShareOptionsGrant = taxEmp.TaxEmpIsShareOptionsGrant;
                    if (!string.IsNullOrEmpty(taxEmp.TaxEmpIsShareOptionsGrant))
                        if (taxEmp.TaxEmpIsShareOptionsGrant.Equals("Y"))
                        {
                            taxDetailRow.ShareOptionsGrantCount = taxEmp.TaxEmpShareOptionsGrantCount;
                            if (!taxEmp.TaxEmpShareOptionsGrantDate.Ticks.Equals(0))
                                taxDetailRow.ShareOptionsGrantDate = taxEmp.TaxEmpShareOptionsGrantDate;
                        }
                    taxDetailRow.PreviousEmployerNameddress = taxEmp.TaxEmpPreviousEmployerNameddress;
                    taxDetailTable.Rows.Add(taxDetailRow);
                }
            }
            return dataSet;
        }

        public static string GenerateTaxationFileData(DatabaseConnection dbConn, int TaxFormID)
        {

            ETaxForm m_taxForm = new ETaxForm();
            m_taxForm.TaxFormID = TaxFormID;
            ETaxForm.db.select(dbConn, m_taxForm);

            DataSet.Taxation_IR56B_DataSet dataSet = GenerateTaxationDataSet(dbConn, TaxFormID, null);
            string taxData = string.Empty;
            foreach (DataSet.Taxation_IR56B_DataSet.TaxationHeaderRow header in dataSet.TaxationHeader.Rows)
            {
                taxData += header.Section.PadRight(3).Substring(0, 3);
                taxData += header.ERN.PadRight(8).Substring(0, 8);
                taxData += header.YearReturn.ToString().PadRight(4).Substring(0, 4);
                taxData += header.SubmissionDate.ToString("yyyyMMdd");
                taxData += header.BatchNo.ToString("00000");
                taxData += "000000";
                taxData += string.Empty.PadRight(9);
                taxData += BytePadRightWithMaxLength(header.EmployerName, 70);
                taxData += BytePadRightWithMaxLength(header.Designation, 25);
                taxData += header.TotalRecord.ToString("00000");
                taxData += header.TotalIncome.ToString().PadLeft(11,'0');
                taxData += string.Empty.PadRight(1480);
                taxData += "\r\n";
                if (System.Text.Encoding.Default.GetBytes(taxData).Length != 1636)
                    throw new Exception("Invalid Header Length");
                foreach (DataSet.Taxation_IR56B_DataSet.IR56BDetailRow detail in dataSet.IR56BDetail.Rows)
                {
                    if (detail.TaxFormID == header.TaxFormID)
                    {
                        string taxDetail = string.Empty;
                        taxDetail += header.Section.PadRight(3).Substring(0, 3);
                        taxDetail += header.ERN.PadRight(8).Substring(0, 8);
                        taxDetail += header.YearReturn.ToString().PadRight(4).Substring(0, 4);
                        taxDetail += header.SubmissionDate.ToString("yyyyMMdd");
                        taxDetail += header.BatchNo.ToString("00000");
                        taxDetail += detail.SheetNo.ToString("000000");
                        taxDetail += BytePadLeftWithMaxLength(detail.HKID.Trim().Replace("(", "").Replace(")", ""), 9);
                        taxDetail += "O";
                        taxDetail += BytePadRightWithMaxLength(detail.EmployeeSurname, 20);
                        taxDetail += BytePadRightWithMaxLength(detail.EmployeeOtherName, 55);
                        taxDetail += BytePadRightWithMaxLength(detail.EmployeeChineseName, 50);
                        taxDetail += BytePadRightWithMaxLength(detail.Sex, 1);
                        taxDetail += BytePadRightWithMaxLength(detail.MaritalStatus, 1);
                        taxDetail += BytePadRightWithMaxLength(detail.EmployeePassportNoIssueCountry, 40);
                        if (detail.MaritalStatus.Equals("2"))
                        {
                            taxDetail += BytePadRightWithMaxLength(detail.SpouseName, 50);
                            taxDetail += BytePadLeftWithMaxLength(detail.SpouseHKID.Trim().Replace("(", "").Replace(")", ""), 9);
                            taxDetail += BytePadRightWithMaxLength(detail.SpousePassportNoIssueCountry, 40);
                        }
                        else
                        {
                            taxDetail += BytePadRightWithMaxLength(string.Empty, 50);
                            taxDetail += BytePadLeftWithMaxLength(string.Empty, 9);
                            taxDetail += BytePadRightWithMaxLength(string.Empty, 40);
                        }
                        taxDetail += BytePadRightWithMaxLength(detail.ResidentialAddress, 90);
                        if (detail.ResidentialAddressAreaCode.Equals("O", StringComparison.CurrentCultureIgnoreCase))
                            taxDetail += BytePadRightWithMaxLength("F", 1);
                        else
                            taxDetail += BytePadRightWithMaxLength(detail.ResidentialAddressAreaCode, 1);
                        taxDetail += BytePadRightWithMaxLength(detail.CorrespondenceAddress, 60);
                        taxDetail += BytePadRightWithMaxLength(detail.CapacityEmployed, 40);
                        taxDetail += BytePadRightWithMaxLength(detail.PartTimeEmployer, 30);
                        taxDetail += detail.EmploymentStartDate.ToString("yyyyMMdd");
                        taxDetail += detail.EmploymentEndDate.ToString("yyyyMMdd");
                        if (detail["PeriodFr_a"] != System.DBNull.Value && detail["PeriodTo_a"] !=  System.DBNull.Value && detail["Amount_a"] !=  System.DBNull.Value)
                        {
                            taxDetail += detail.PeriodFr_a.ToString("yyyyMMdd") + " - " + detail.PeriodTo_a.ToString("yyyyMMdd");
                            taxDetail += BytePadLeftWithMaxLength(detail.Amount_a.ToString("000000000"), 9);
                        }
                        else
                            taxDetail += string.Empty.PadLeft(19)+"000000000";
                        if (detail["PeriodFr_b"] != System.DBNull.Value && detail["PeriodTo_b"] != System.DBNull.Value && detail["Amount_b"] != System.DBNull.Value)
                        {
                            taxDetail += detail.PeriodFr_b.ToString("yyyyMMdd") + " - " + detail.PeriodTo_b.ToString("yyyyMMdd");
                            taxDetail += BytePadLeftWithMaxLength(detail.Amount_b.ToString("000000000"), 9);
                        }
                        else
                            taxDetail += string.Empty.PadLeft(19)+"000000000";
                        if (detail["PeriodFr_c"] !=  System.DBNull.Value && detail["PeriodTo_c"] !=  System.DBNull.Value && detail["Amount_c"] !=  System.DBNull.Value)
                        {
                            taxDetail += detail.PeriodFr_c.ToString("yyyyMMdd") + " - " + detail.PeriodTo_c.ToString("yyyyMMdd");
                            taxDetail += BytePadLeftWithMaxLength(detail.Amount_c.ToString("000000000"), 9);
                        }
                        else
                            taxDetail += string.Empty.PadLeft(19)+"000000000";
                        if (detail["PeriodFr_d"] !=  System.DBNull.Value && detail["PeriodTo_d"] !=  System.DBNull.Value && detail["Amount_d"] !=  System.DBNull.Value)
                        {
                            taxDetail += detail.PeriodFr_d.ToString("yyyyMMdd") + " - " + detail.PeriodTo_d.ToString("yyyyMMdd");
                            taxDetail += BytePadLeftWithMaxLength(detail.Amount_d.ToString("000000000"), 9);
                        }
                        else
                            taxDetail += string.Empty.PadLeft(19)+"000000000";
                        if (detail["PeriodFr_e"] !=  System.DBNull.Value && detail["PeriodTo_e"] !=  System.DBNull.Value && detail["Amount_e"] !=  System.DBNull.Value)
                        {
                            taxDetail += detail.PeriodFr_e.ToString("yyyyMMdd") + " - " + detail.PeriodTo_e.ToString("yyyyMMdd");
                            taxDetail += BytePadLeftWithMaxLength(detail.Amount_e.ToString("000000000"), 9);
                        }
                        else
                            taxDetail += string.Empty.PadLeft(19)+"000000000";
                        if (detail["PeriodFr_f"] !=  System.DBNull.Value && detail["PeriodTo_f"] !=  System.DBNull.Value && detail["Amount_f"] !=  System.DBNull.Value)
                        {
                            taxDetail += detail.PeriodFr_f.ToString("yyyyMMdd") + " - " + detail.PeriodTo_f.ToString("yyyyMMdd");
                            taxDetail += BytePadLeftWithMaxLength(detail.Amount_f.ToString("000000000"), 9);
                        }
                        else
                            taxDetail += string.Empty.PadLeft(19)+"000000000";
                        if (detail["PeriodFr_g"] !=  System.DBNull.Value && detail["PeriodTo_g"] !=  System.DBNull.Value && detail["Amount_g"] !=  System.DBNull.Value)
                        {
                            taxDetail += detail.PeriodFr_g.ToString("yyyyMMdd") + " - " + detail.PeriodTo_g.ToString("yyyyMMdd");
                            taxDetail += BytePadLeftWithMaxLength(detail.Amount_g.ToString("000000000"), 9);
                        }
                        else
                            taxDetail += string.Empty.PadLeft(19)+"000000000";
                        if (detail["PeriodFr_h"] !=  System.DBNull.Value && detail["PeriodTo_h"] !=  System.DBNull.Value && detail["Amount_h"] !=  System.DBNull.Value)
                        {
                            taxDetail += detail.PeriodFr_h.ToString("yyyyMMdd") + " - " + detail.PeriodTo_h.ToString("yyyyMMdd");
                            taxDetail += BytePadLeftWithMaxLength(detail.Amount_h.ToString("000000000"), 9);
                        }
                        else
                            taxDetail += string.Empty.PadLeft(19)+"000000000";
                        if (detail["PeriodFr_i"] !=  System.DBNull.Value && detail["PeriodTo_i"] !=  System.DBNull.Value && detail["Amount_i"] !=  System.DBNull.Value)
                        {
                            taxDetail += detail.PeriodFr_i.ToString("yyyyMMdd") + " - " + detail.PeriodTo_i.ToString("yyyyMMdd");
                            taxDetail += BytePadLeftWithMaxLength(detail.Amount_i.ToString("000000000"), 9);
                        }
                        else
                            taxDetail += string.Empty.PadLeft(19)+"000000000";
                        if (detail["PeriodFr_j"] !=  System.DBNull.Value && detail["PeriodTo_j"] !=  System.DBNull.Value && detail["Amount_j"] !=  System.DBNull.Value)
                        {
                            taxDetail += detail.PeriodFr_j.ToString("yyyyMMdd") + " - " + detail.PeriodTo_j.ToString("yyyyMMdd");
                            taxDetail += BytePadLeftWithMaxLength(detail.Amount_j.ToString("000000000"), 9);
                        }
                        else
                            taxDetail += string.Empty.PadLeft(19)+"000000000";
                        if (detail["PeriodFr_k1"] !=  System.DBNull.Value && detail["PeriodTo_k1"] !=  System.DBNull.Value && detail["Amount_k1"] !=  System.DBNull.Value)
                        {
                            taxDetail += BytePadLeftWithMaxLength(detail["Nature_k1"] !=  System.DBNull.Value ? detail.Nature_k1 : string.Empty, 35);
                            taxDetail += detail.PeriodFr_k1.ToString("yyyyMMdd") + " - " + detail.PeriodTo_k1.ToString("yyyyMMdd");
                            taxDetail += BytePadLeftWithMaxLength(detail.Amount_k1.ToString("000000000"), 9);
                        }
                        else
                        {
                            taxDetail += string.Empty.PadLeft(35);
                            taxDetail += string.Empty.PadLeft(19)+"000000000";
                        }
                        if (detail["PeriodFr_k2"] !=  System.DBNull.Value && detail["PeriodTo_k2"] !=  System.DBNull.Value && detail["Amount_k2"] !=  System.DBNull.Value)
                        {
                            taxDetail += BytePadLeftWithMaxLength(detail["Nature_k2"] !=  System.DBNull.Value ? detail.Nature_k2 : string.Empty, 35);
                            taxDetail += detail.PeriodFr_k2.ToString("yyyyMMdd") + " - " + detail.PeriodTo_k2.ToString("yyyyMMdd");
                            taxDetail += BytePadLeftWithMaxLength(detail.Amount_k2.ToString("000000000"), 9);
                        }
                        else
                        {
                            taxDetail += string.Empty.PadLeft(35);
                            taxDetail += string.Empty.PadLeft(19)+"000000000";
                        }
                        if (detail["PeriodFr_k3"] !=  System.DBNull.Value && detail["PeriodTo_k3"] !=  System.DBNull.Value && detail["Amount_k3"] !=  System.DBNull.Value)
                        {
                            taxDetail += BytePadLeftWithMaxLength(detail["Nature_k3"] !=  System.DBNull.Value ? detail.Nature_k3 : string.Empty, 35);
                            taxDetail += detail.PeriodFr_k3.ToString("yyyyMMdd") + " - " + detail.PeriodTo_k3.ToString("yyyyMMdd");
                            taxDetail += BytePadLeftWithMaxLength(detail.Amount_k3.ToString("000000000"), 9);
                        }
                        else
                        {
                            taxDetail += string.Empty.PadLeft(35);
                            taxDetail += string.Empty.PadLeft(19)+"000000000";
                        }
                        if (detail["PeriodFr_l"] !=  System.DBNull.Value && detail["PeriodTo_l"] !=  System.DBNull.Value && detail["Amount_l"] !=  System.DBNull.Value)
                        {
                            taxDetail += detail.PeriodFr_l.ToString("yyyyMMdd") + " - " + detail.PeriodTo_l.ToString("yyyyMMdd");
                            taxDetail += BytePadLeftWithMaxLength(detail.Amount_l.ToString("000000000"), 9);
                        }
                        else
                        {
                            taxDetail += string.Empty.PadLeft(19)+"000000000";
                        }
                        // Start 0000020, KuangWei, 2014-08-04
                        // Start 0000187, Ricky So, 2015-04-16
                        if (m_taxForm.TaxFormType == "M")
                        // End 0000187, Ricky So, 2015-04-16
                        {
                            if (detail["PeriodFr_Type 1"] != System.DBNull.Value && detail["PeriodTo_Type 1"] != System.DBNull.Value && detail["Amount_Type 1"] != System.DBNull.Value)
                            {
                                taxDetail += detail.PeriodFr_Type_1.ToString("yyyyMMdd") + " - " + detail.PeriodFr_Type_1.ToString("yyyyMMdd");
                                taxDetail += BytePadLeftWithMaxLength(detail.Amount_Type_1.ToString("000000000"), 9);
                            }
                            else
                            {
                                taxDetail += string.Empty.PadLeft(19) + "000000000";
                            }
                            if (detail["PeriodFr_Type 2"] != System.DBNull.Value && detail["PeriodTo_Type 2"] != System.DBNull.Value && detail["Amount_Type 2"] != System.DBNull.Value)
                            {
                                taxDetail += detail.PeriodFr_Type_2.ToString("yyyyMMdd") + " - " + detail.PeriodTo_Type_2.ToString("yyyyMMdd");
                                taxDetail += BytePadLeftWithMaxLength(detail.Amount_Type_2.ToString("000000000"), 9);
                            }
                            else
                            {
                                taxDetail += string.Empty.PadLeft(19) + "000000000";
                            }
                            if (detail["PeriodFr_Type 3"] != System.DBNull.Value && detail["PeriodTo_Type 3"] != System.DBNull.Value && detail["Amount_Type 3"] != System.DBNull.Value)
                            {
                                taxDetail += detail.PeriodFr_Type_3.ToString("yyyyMMdd") + " - " + detail.PeriodTo_Type_3.ToString("yyyyMMdd");
                                taxDetail += BytePadLeftWithMaxLength(detail.Amount_Type_3.ToString("000000000"), 9);
                            }
                            else
                            {
                                taxDetail += string.Empty.PadLeft(19) + "000000000";
                            }
                            if (detail["PeriodFr_Others (a)"] != System.DBNull.Value && detail["PeriodTo_Others (a)"] != System.DBNull.Value && detail["Amount_Others (a)"] != System.DBNull.Value)
                            {
                                taxDetail += detail._PeriodFr_Others__a_.ToString("yyyyMMdd") + " - " + detail._PeriodTo_Others__a_.ToString("yyyyMMdd");
                                taxDetail += BytePadLeftWithMaxLength(detail._Amount_Others__a_.ToString("000000000"), 9);
                            }
                            else
                            {
                                taxDetail += string.Empty.PadLeft(19) + "000000000";
                            }
                            if (detail["PeriodFr_Others (b)"] != System.DBNull.Value && detail["PeriodTo_Others (b)"] != System.DBNull.Value && detail["Amount_Others (b)"] != System.DBNull.Value)
                            {
                                taxDetail += detail._PeriodFr_Others__b_.ToString("yyyyMMdd") + " - " + detail._PeriodTo_Others__b_.ToString("yyyyMMdd");
                                taxDetail += BytePadLeftWithMaxLength(detail._Amount_Others__b_.ToString("000000000"), 9);
                            }
                            else
                            {
                                taxDetail += string.Empty.PadLeft(19) + "000000000";
                            }
                            if (detail["PeriodFr_Others (c)"] != System.DBNull.Value && detail["PeriodTo_Others (c)"] != System.DBNull.Value && detail["Amount_Others (c)"] != System.DBNull.Value)
                            {
                                taxDetail += detail._PeriodFr_Others__c_.ToString("yyyyMMdd") + " - " + detail._PeriodTo_Others__c_.ToString("yyyyMMdd");
                                taxDetail += BytePadLeftWithMaxLength(detail._Amount_Others__c_.ToString("000000000"), 9);
                            }
                            else
                            {
                                taxDetail += string.Empty.PadLeft(19) + "000000000";
                            }
                            if (detail["PeriodFr_Others (d)"] != System.DBNull.Value && detail["PeriodTo_Others (d)"] != System.DBNull.Value && detail["Amount_Others (d)"] != System.DBNull.Value)
                            {
                                taxDetail += BytePadLeftWithMaxLength(detail["Nature_Others (d)"] != System.DBNull.Value ? detail._Nature_Others__d_ : string.Empty, 35);
                                taxDetail += detail._PeriodFr_Others__d_.ToString("yyyyMMdd") + " - " + detail._PeriodTo_Others__d_.ToString("yyyyMMdd");
                                taxDetail += BytePadLeftWithMaxLength(detail._Amount_Others__d_.ToString("000000000"), 9);
                            }
                            else
                            {
                                taxDetail += string.Empty.PadLeft(19) + "000000000";
                            }
                            if (detail["PeriodFr_Others (e)"] != System.DBNull.Value && detail["PeriodTo_Others (e)"] != System.DBNull.Value && detail["Amount_Others (e)"] != System.DBNull.Value)
                            {
                                taxDetail += BytePadLeftWithMaxLength(detail["Nature_Others (e)"] != System.DBNull.Value ? detail._Nature_Others__e_ : string.Empty, 35);
                                taxDetail += detail._PeriodFr_Others__e_.ToString("yyyyMMdd") + " - " + detail._PeriodTo_Others__e_.ToString("yyyyMMdd");
                                taxDetail += BytePadLeftWithMaxLength(detail._Amount_Others__e_.ToString("000000000"), 9);
                            }
                            else
                            {
                                taxDetail += string.Empty.PadLeft(19) + "000000000";
                            }
                        }
                        // End 0000020, KuangWei, 2014-08-04
                        taxDetail += BytePadLeftWithMaxLength(detail.TotalIncome.ToString("000000000"), 9);
                        taxDetail += BytePadRightWithMaxLength(detail.PlaceOfResidenceIndicator.ToString(), 1);
                        taxDetail += BytePadRightWithMaxLength(detail.PlaceOfResidenceAddress1, 110);
                        taxDetail += BytePadRightWithMaxLength(detail.PlaceOfResidenceNature1, 19);
                        if (detail["PlaceOfResidenceFr1"] != System.DBNull.Value && detail["PlaceOfResidenceTo1"] != System.DBNull.Value)
                            taxDetail += BytePadRightWithMaxLength(detail.PlaceOfResidenceFr1.ToString("yyyy/MM/dd") + " - " + detail.PlaceOfResidenceTo1.ToString("yyyy/MM/dd"), 26);
                        else
                            taxDetail += string.Empty.PadLeft(26);
                        taxDetail += BytePadLeftWithMaxLength(detail["PlaceOfResidenceRentByER1"] != System.DBNull.Value ? detail.PlaceOfResidenceRentByER1.ToString().PadLeft(7,'0') : "0000000", 7);
                        taxDetail += BytePadLeftWithMaxLength(detail["PlaceOfResidenceRentByEE1"] != System.DBNull.Value ? detail.PlaceOfResidenceRentByEE1.ToString().PadLeft(7, '0') : "0000000", 7);
                        taxDetail += BytePadLeftWithMaxLength(detail["PlaceOfResidenceRefundedEE1"] != System.DBNull.Value ? detail.PlaceOfResidenceRefundedEE1.ToString().PadLeft(7, '0') : "0000000", 7);
                        taxDetail += BytePadLeftWithMaxLength(detail["PlaceOfResidenceRentToERByEE1"] != System.DBNull.Value ? detail.PlaceOfResidenceRentToERByEE1.ToString().PadLeft(7, '0') : "0000000", 7);
                        taxDetail += BytePadRightWithMaxLength(detail.PlaceOfResidenceAddress2, 110);
                        taxDetail += BytePadRightWithMaxLength(detail.PlaceOfResidenceNature2, 19);
                        if (detail["PlaceOfResidenceFr2"] != System.DBNull.Value && detail["PlaceOfResidenceTo2"] != System.DBNull.Value)
                            taxDetail += BytePadRightWithMaxLength(detail.PlaceOfResidenceFr2.ToString("yyyy/MM/dd") + " - " + detail.PlaceOfResidenceTo2.ToString("yyyy/MM/dd"), 26);
                        else
                            taxDetail += string.Empty.PadLeft(26);
                        taxDetail += BytePadLeftWithMaxLength(detail["PlaceOfResidenceRentByER2"] != System.DBNull.Value ? detail.PlaceOfResidenceRentByER2.ToString().PadLeft(7, '0') : "0000000", 7);
                        taxDetail += BytePadLeftWithMaxLength(detail["PlaceOfResidenceRentByEE2"] != System.DBNull.Value ? detail.PlaceOfResidenceRentByEE2.ToString().PadLeft(7, '0') : "0000000", 7);
                        taxDetail += BytePadLeftWithMaxLength(detail["PlaceOfResidenceRefundedEE2"] != System.DBNull.Value ? detail.PlaceOfResidenceRefundedEE2.ToString().PadLeft(7, '0') : "0000000", 7);
                        taxDetail += BytePadLeftWithMaxLength(detail["PlaceOfResidenceRentToERByEE2"] != System.DBNull.Value ? detail.PlaceOfResidenceRentToERByEE2.ToString().PadLeft(7, '0') : "0000000", 7);
                        taxDetail += BytePadRightWithMaxLength(detail.OverseasIncomeIndicator.ToString(), 1);
                        if (detail.OverseasIncomeIndicator == 1 && detail.OverseasCompanyAmount.Trim().Equals(string.Empty))
                            taxDetail += BytePadRightWithMaxLength("Unknown", 20);
                        else
                            taxDetail += BytePadRightWithMaxLength(detail.OverseasCompanyAmount, 20);
                        // Start 0000020, KuangWei, 2014-08-06
                        // Start 0000187, Ricky So, 2015-04-16
                        if (m_taxForm.TaxFormType == "M")
                        // End 0000187, Ricky So, 2015-04-16
                        {
                            if (detail.SumWithheldIndicator == 1 && detail.SumWithheldAmount.Trim().Equals(string.Empty))
                                taxDetail += BytePadRightWithMaxLength("Unknown", 20);
                            else
                                taxDetail += BytePadRightWithMaxLength(detail.SumWithheldAmount, 20);
                        }
                        // End 0000020, KuangWei, 2014-08-06
                        taxDetail += BytePadRightWithMaxLength(detail.OverseasCompanyName, 60);
                        taxDetail += BytePadRightWithMaxLength(detail.OverseasCompanyAddress, 60);
                        taxDetail += BytePadRightWithMaxLength(detail.TaxFileNo, 13);
                        taxDetail += BytePadRightWithMaxLength(detail.Remarks, 60);
                        taxDetail += "\r\n";
                        if (GetTaxationFileEncoding().GetBytes(taxDetail).Length != 1636)
                            throw new Exception("Invalid Record Length");
                        taxData += taxDetail;
                    }
                }
            }
            return taxData + char.ConvertFromUtf32(26);
        }

        private static XmlElement MyCreateElement(XmlDocument root, string elementName, string innerText)
        {
            XmlElement info = root.CreateElement(elementName);

            if (innerText != null && innerText != "")
            {
                info.InnerText = innerText;
            }
            return info;
        }

        private static string MaxStringLength(string s, int maxLength)
        {
            if (s.Length > maxLength)
                return s.Substring(0, maxLength);
            else
                return s;
        }

        public static XmlDocument GenerateTaxationXMLData(DatabaseConnection dbConn, int TaxFormID)
        {

            ETaxForm m_taxForm = new ETaxForm();
            m_taxForm.TaxFormID = TaxFormID;
            ETaxForm.db.select(dbConn, m_taxForm);

            DataSet.Taxation_IR56B_DataSet dataSet = GenerateTaxationDataSet(dbConn, TaxFormID, null);

            XmlDocument doc = new XmlDocument();
            XmlElement employee;
            XmlElement ir56b = null;

            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", null));

            foreach (DataSet.Taxation_IR56B_DataSet.TaxationHeaderRow header in dataSet.TaxationHeader.Rows)
            {
                ir56b = doc.CreateElement("IR56B");

                XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

                XmlAttribute att = doc.CreateAttribute("xsi", "noNamespaceSchemaLocation",
                "http://www.w3.org/2001/XMLSchema-instance");
                att.Value = "ir56b.xsd";
                ir56b.SetAttributeNode(att);
                doc.AppendChild(ir56b);

                //ir56b.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                //ir56b.SetAttribute("xsi:noNamespaceSchemaLocation", "ir56b.xsd");

                //doc.AppendChild(ir56b);

                ir56b.AppendChild(MyCreateElement(doc, "Section", header.Section.PadRight(3).Substring(0, 3)));
                ir56b.AppendChild(MyCreateElement(doc, "ERN", MaxStringLength(header.ERN.PadRight(8), 8)));
                ir56b.AppendChild(MyCreateElement(doc, "YrErReturn", header.YearReturn.ToString().PadRight(4).Substring(0, 4)));
                ir56b.AppendChild(MyCreateElement(doc, "SubDate", header.SubmissionDate.ToString("yyyyMMdd")));
                ir56b.AppendChild(MyCreateElement(doc, "ErName", MaxStringLength(header.EmployerName, 70)));
                ir56b.AppendChild(MyCreateElement(doc, "Designation", MaxStringLength(header.Designation, 25)));
                ir56b.AppendChild(MyCreateElement(doc, "NoRecordBatch", header.TotalRecord.ToString("00000")));
                ir56b.AppendChild(MyCreateElement(doc, "TotIncomeBatch", header.TotalIncome.ToString().PadLeft(11, '0')));

                foreach (DataSet.Taxation_IR56B_DataSet.IR56BDetailRow detail in dataSet.IR56BDetail.Rows)
                {
                    if (detail.TaxFormID == header.TaxFormID)
                    {
                        employee = doc.CreateElement("Employee");

                        // employees
                        employee.AppendChild(MyCreateElement(doc, "SheetNo", detail.SheetNo.ToString("000000")));
                        employee.AppendChild(MyCreateElement(doc, "HKID", MaxStringLength(detail.HKID.Trim().Replace("(", "").Replace(")", ""), 9)));
                        employee.AppendChild(MyCreateElement(doc, "TypeOfForm", "O"));
                        employee.AppendChild(MyCreateElement(doc, "Surname", MaxStringLength(detail.EmployeeSurname, 20)));
                        employee.AppendChild(MyCreateElement(doc, "GivenName", MaxStringLength(detail.EmployeeOtherName, 55)));
                        employee.AppendChild(MyCreateElement(doc, "NameInChinese", MaxStringLength(detail.EmployeeChineseName, 50)));
                        employee.AppendChild(MyCreateElement(doc, "Sex", MaxStringLength(detail.Sex, 1)));
                        employee.AppendChild(MyCreateElement(doc, "MaritalStatus", MaxStringLength(detail.MaritalStatus, 1)));

                        if (detail.HKID == "" || detail.HKID == null)
                            employee.AppendChild(MyCreateElement(doc, "PpNum", MaxStringLength(detail.EmployeePassportNoIssueCountry, 40)));
                        else
                            employee.AppendChild(MyCreateElement(doc, "PpNum", ""));

                        if (detail.MaritalStatus.Equals("2"))
                        {
                            employee.AppendChild(MyCreateElement(doc, "SpouseName", MaxStringLength(detail.SpouseName, 50)));
                            employee.AppendChild(MyCreateElement(doc, "SpouseHKID", MaxStringLength(detail.SpouseHKID.Trim().Replace("(", "").Replace(")", ""), 9)));
                            if (detail.SpouseHKID != null && detail.SpouseHKID != "")
                                employee.AppendChild(MyCreateElement(doc, "SpousePpNum", MaxStringLength(detail.SpousePassportNoIssueCountry, 40)));
                            else
                                employee.AppendChild(MyCreateElement(doc, "SpousePpNum", ""));
                        }
                        else
                        {
                            employee.AppendChild(MyCreateElement(doc, "SpouseName", ""));
                            employee.AppendChild(MyCreateElement(doc, "SpouseHKID", ""));
                            employee.AppendChild(MyCreateElement(doc, "SpousePpNum", ""));
                        }
                        employee.AppendChild(MyCreateElement(doc, "ResAddr", MaxStringLength(detail.ResidentialAddress, 90)));

                        if (detail.ResidentialAddressAreaCode.Equals("O", StringComparison.CurrentCultureIgnoreCase))
                            employee.AppendChild(MyCreateElement(doc, "AreaCodeResAddr", "F"));
                        else
                            employee.AppendChild(MyCreateElement(doc, "AreaCodeResAddr", MaxStringLength(detail.ResidentialAddressAreaCode, 1)));

                        employee.AppendChild(MyCreateElement(doc, "PosAddr", MaxStringLength(detail.CorrespondenceAddress, 60)));
                        employee.AppendChild(MyCreateElement(doc, "Capacity", MaxStringLength(detail.CapacityEmployed, 40)));
                        employee.AppendChild(MyCreateElement(doc, "PtPrinEmp", MaxStringLength(detail.PartTimeEmployer, 30)));
                        employee.AppendChild(MyCreateElement(doc, "StartDateOfEmp", detail.EmploymentStartDate.ToString("yyyyMMdd")));
                        employee.AppendChild(MyCreateElement(doc, "EndDateOfEmp", detail.EmploymentEndDate.ToString("yyyyMMdd")));

                        if (detail["PeriodFr_a"] != System.DBNull.Value && detail["PeriodTo_a"] != System.DBNull.Value && detail["Amount_a"] != System.DBNull.Value)
                        {
                            employee.AppendChild(MyCreateElement(doc, "PerOfSalary", detail.PeriodFr_a.ToString("yyyyMMdd") + " - " + detail.PeriodTo_a.ToString("yyyyMMdd")));
                            employee.AppendChild(MyCreateElement(doc, "AmtOfSalary", MaxStringLength(detail.Amount_a.ToString("0"), 9)));
                        }
                        else
                        {
                            employee.AppendChild(MyCreateElement(doc, "PerOfSalary", ""));
                            employee.AppendChild(MyCreateElement(doc, "AmtOfSalary", "0"));
                        }
                        
                        if (detail["PeriodFr_b"] != System.DBNull.Value && detail["PeriodTo_b"] != System.DBNull.Value && detail["Amount_b"] != System.DBNull.Value)
                        {
                            employee.AppendChild(MyCreateElement(doc, "PerOfLeavePay", detail.PeriodFr_b.ToString("yyyyMMdd") + " - " + detail.PeriodTo_b.ToString("yyyyMMdd")));
                            employee.AppendChild(MyCreateElement(doc, "AmtOfLeavePay", MaxStringLength(detail.Amount_b.ToString("0"), 9)));
                        }
                        else
                        {
                            employee.AppendChild(MyCreateElement(doc, "PerOfLeavePay", ""));
                            employee.AppendChild(MyCreateElement(doc, "AmtOfLeavePay", "0"));
                        }

                        if (detail["PeriodFr_c"] != System.DBNull.Value && detail["PeriodTo_c"] != System.DBNull.Value && detail["Amount_c"] != System.DBNull.Value)
                        {
                            employee.AppendChild(MyCreateElement(doc, "PerOfDirectorFee", detail.PeriodFr_c.ToString("yyyyMMdd") + " - " + detail.PeriodTo_c.ToString("yyyyMMdd")));
                            employee.AppendChild(MyCreateElement(doc, "AmtOfDirectorFee", MaxStringLength(detail.Amount_c.ToString("0"), 9)));
                        }
                        else
                        {
                            employee.AppendChild(MyCreateElement(doc, "PerOfDirectorFee", ""));
                            employee.AppendChild(MyCreateElement(doc, "AmtOfDirectorFee", ""));
                        }
                        if (detail["PeriodFr_d"] != System.DBNull.Value && detail["PeriodTo_d"] != System.DBNull.Value && detail["Amount_d"] != System.DBNull.Value)
                        {
                            employee.AppendChild(MyCreateElement(doc, "PerOfCommFee", detail.PeriodFr_d.ToString("yyyyMMdd") + " - " + detail.PeriodTo_d.ToString("yyyyMMdd")));
                            employee.AppendChild(MyCreateElement(doc, "AmtOfCommFee", MaxStringLength(detail.Amount_d.ToString("0"), 9))); 
                        }
                        else
                        {
                            employee.AppendChild(MyCreateElement(doc, "PerOfCommFee", ""));
                            employee.AppendChild(MyCreateElement(doc, "AmtOfCommFee", "0"));
                        }

                        if (detail["PeriodFr_e"] != System.DBNull.Value && detail["PeriodTo_e"] != System.DBNull.Value && detail["Amount_e"] != System.DBNull.Value)
                        {
                            employee.AppendChild(MyCreateElement(doc, "PerOfBonus", detail.PeriodFr_e.ToString("yyyyMMdd") + " - " + detail.PeriodTo_e.ToString("yyyyMMdd")));
                            employee.AppendChild(MyCreateElement(doc, "AmtOfBonus", MaxStringLength(detail.Amount_e.ToString("0"), 9)));
                        }
                        else
                        {
                            employee.AppendChild(MyCreateElement(doc, "PerOfBonus", ""));
                            employee.AppendChild(MyCreateElement(doc, "AmtOfBonus", "0"));
                        }                        
                        if (detail["PeriodFr_f"] != System.DBNull.Value && detail["PeriodTo_f"] != System.DBNull.Value && detail["Amount_f"] != System.DBNull.Value)
                        {
                            employee.AppendChild(MyCreateElement(doc, "PerOfBpEtc", detail.PeriodFr_f.ToString("yyyyMMdd") + " - " + detail.PeriodTo_f.ToString("yyyyMMdd")));
                            employee.AppendChild(MyCreateElement(doc, "AmtOfBpEtc", MaxStringLength(detail.Amount_f.ToString("0"), 9)));
                        }
                        else
                        {
                            employee.AppendChild(MyCreateElement(doc, "PerOfBpEtc", ""));
                            employee.AppendChild(MyCreateElement(doc, "AmtOfBpEtc", "0"));
                        }
                        if (detail["PeriodFr_g"] != System.DBNull.Value && detail["PeriodTo_g"] != System.DBNull.Value && detail["Amount_g"] != System.DBNull.Value)
                        {
                            employee.AppendChild(MyCreateElement(doc, "PerOfPayRetire", detail.PeriodFr_g.ToString("yyyyMMdd") + " - " + detail.PeriodTo_g.ToString("yyyyMMdd")));
                            employee.AppendChild(MyCreateElement(doc, "AmtOfPayRetire", MaxStringLength(detail.Amount_g.ToString("0"), 9)));
                        }
                        else
                        {
                            employee.AppendChild(MyCreateElement(doc, "PerOfPayRetire", ""));
                            employee.AppendChild(MyCreateElement(doc, "AmtOfPayRetire", "0"));
                        }
                        if (detail["PeriodFr_h"] != System.DBNull.Value && detail["PeriodTo_h"] != System.DBNull.Value && detail["Amount_h"] != System.DBNull.Value)
                        {
                            employee.AppendChild(MyCreateElement(doc, "PerOfSalTaxPaid", detail.PeriodFr_h.ToString("yyyyMMdd") + " - " + detail.PeriodTo_h.ToString("yyyyMMdd")));
                            employee.AppendChild(MyCreateElement(doc, "AmtOfSalTaxPaid", MaxStringLength(detail.Amount_h.ToString("0"), 9)));
                        }
                        else
                        {
                            employee.AppendChild(MyCreateElement(doc, "PerOfSalTaxPaid", ""));
                            employee.AppendChild(MyCreateElement(doc, "AmtOfSalTaxPaid", "0"));
                        }
                        if (detail["PeriodFr_i"] != System.DBNull.Value && detail["PeriodTo_i"] != System.DBNull.Value && detail["Amount_i"] != System.DBNull.Value)
                        {
                            employee.AppendChild(MyCreateElement(doc, "PerOfEduBen", detail.PeriodFr_i.ToString("yyyyMMdd") + " - " + detail.PeriodTo_i.ToString("yyyyMMdd")));
                            employee.AppendChild(MyCreateElement(doc, "AmtOfEduBen", MaxStringLength(detail.Amount_i.ToString("0"), 9)));
                        }
                        else
                        {
                            employee.AppendChild(MyCreateElement(doc, "PerOfEduBen", ""));
                            employee.AppendChild(MyCreateElement(doc, "AmtOfEduBen", "0"));
                        }

                        if (detail["PeriodFr_j"] != System.DBNull.Value && detail["PeriodTo_j"] != System.DBNull.Value && detail["Amount_j"] != System.DBNull.Value)
                        {
                            employee.AppendChild(MyCreateElement(doc, "PerOfGainShareOption", detail.PeriodFr_j.ToString("yyyyMMdd") + " - " + detail.PeriodTo_j.ToString("yyyyMMdd")));
                            employee.AppendChild(MyCreateElement(doc, "AmtOfGainShareOption", MaxStringLength(detail.Amount_j.ToString("0"), 9)));
                        }
                        else
                        {
                            employee.AppendChild(MyCreateElement(doc, "PerOfGainShareOption", ""));
                            employee.AppendChild(MyCreateElement(doc, "AmtOfGainShareOption", "0"));
                        }

                        // #40-#42
                        if (detail["PeriodFr_k1"] != System.DBNull.Value && detail["PeriodTo_k1"] != System.DBNull.Value && detail["Amount_k1"] != System.DBNull.Value)
                        {
                            employee.AppendChild(MyCreateElement(doc, "NatureOtherRAP1", MaxStringLength(detail["Nature_k1"] != System.DBNull.Value ? detail.Nature_k1 : string.Empty, 35)));
                            employee.AppendChild(MyCreateElement(doc, "PerOfOtherRAP1", detail.PeriodFr_k1.ToString("yyyyMMdd") + " - " + detail.PeriodTo_k1.ToString("yyyyMMdd")));
                            employee.AppendChild(MyCreateElement(doc, "AmtOfOtherRAP1", MaxStringLength(detail.Amount_k1.ToString("0"), 9)));
                        }
                        else
                        {
                            employee.AppendChild(MyCreateElement(doc, "NatureOtherRAP1", ""));
                            employee.AppendChild(MyCreateElement(doc, "PerOfOtherRAP1", ""));
                            employee.AppendChild(MyCreateElement(doc, "AmtOfOtherRAP1", "0"));
                        }


                        // #43 - 45
                        if (detail["PeriodFr_k2"] != System.DBNull.Value && detail["PeriodTo_k2"] != System.DBNull.Value && detail["Amount_k2"] != System.DBNull.Value)
                        {
                            employee.AppendChild(MyCreateElement(doc, "NatureOtherRAP2", MaxStringLength(detail["Nature_k2"] != System.DBNull.Value ? detail.Nature_k2 : string.Empty, 35)));
                            employee.AppendChild(MyCreateElement(doc, "PerOfOtherRAP2", detail.PeriodFr_k2.ToString("yyyyMMdd") + " - " + detail.PeriodTo_k2.ToString("yyyyMMdd")));
                            employee.AppendChild(MyCreateElement(doc, "AmtOfOtherRAP2", MaxStringLength(detail.Amount_k2.ToString("0"), 9)));
                        }
                        else
                        {
                            employee.AppendChild(MyCreateElement(doc, "NatureOtherRAP2", ""));
                            employee.AppendChild(MyCreateElement(doc, "PerOfOtherRAP2", ""));
                            employee.AppendChild(MyCreateElement(doc, "AmtOfOtherRAP2", "0"));
                        }

                        //#46 - 48
                        if (detail["PeriodFr_k3"] !=  System.DBNull.Value && detail["PeriodTo_k3"] !=  System.DBNull.Value && detail["Amount_k3"] !=  System.DBNull.Value)
                        {
                            employee.AppendChild(MyCreateElement(doc, "NatureOtherRAP3", MaxStringLength(detail["Nature_k3"] !=  System.DBNull.Value ? detail.Nature_k3 : string.Empty, 35)));
                            employee.AppendChild(MyCreateElement(doc, "PerOfOtherRAP3", detail.PeriodFr_k3.ToString("yyyyMMdd") + " - " + detail.PeriodTo_k3.ToString("yyyyMMdd")));
                            employee.AppendChild(MyCreateElement(doc, "AmtOfOtherRAP3", MaxStringLength(detail.Amount_k3.ToString("0"), 9)));
                        }
                        else
                        {
                            employee.AppendChild(MyCreateElement(doc, "NatureOtherRAP3", ""));
                            employee.AppendChild(MyCreateElement(doc, "PerOfOtherRAP3", ""));
                            employee.AppendChild(MyCreateElement(doc, "AmtOfOtherRAP3", "0"));
                        }

                        // #49 - #50
                        if (detail["PeriodFr_l"] != System.DBNull.Value && detail["PeriodTo_l"] != System.DBNull.Value && detail["Amount_l"] != System.DBNull.Value)
                        {
                            employee.AppendChild(MyCreateElement(doc, "PerOfPension", detail.PeriodFr_l.ToString("yyyyMMdd") + " - " + detail.PeriodTo_l.ToString("yyyyMMdd")));
                            employee.AppendChild(MyCreateElement(doc, "AmtOfPension", MaxStringLength(detail.Amount_l.ToString("0"), 9)));
                        }
                        else
                        {
                            employee.AppendChild(MyCreateElement(doc, "PerOfPension", ""));
                            employee.AppendChild(MyCreateElement(doc, "AmtOfPension", "0"));
                        }

                        // Start 0000020, KuangWei, 2014-08-04
                        // Start 0000187, Ricky So, 2015-04-16
                        if (m_taxForm.TaxFormType == "M")
                        // End 0000187, Ricky So, 2015-04-16
                        {
                            if (detail["PeriodFr_Type 1"] != System.DBNull.Value && detail["PeriodTo_Type 1"] != System.DBNull.Value && detail["Amount_Type 1"] != System.DBNull.Value)
                            {
                                employee.AppendChild(MyCreateElement(doc, "PerOfPension", detail.PeriodFr_Type_1.ToString("yyyyMMdd") + " - " + detail.PeriodTo_Type_1.ToString("yyyyMMdd")));
                                employee.AppendChild(MyCreateElement(doc, "AmtOfPension", MaxStringLength(detail.Amount_Type_1.ToString("0"), 9)));
                            }
                            else
                            {
                                employee.AppendChild(MyCreateElement(doc, "PerOfPension", ""));
                                employee.AppendChild(MyCreateElement(doc, "AmtOfPension", "0"));
                            }
                            if (detail["PeriodFr_Type 2"] != System.DBNull.Value && detail["PeriodTo_Type 2"] != System.DBNull.Value && detail["Amount_Type 2"] != System.DBNull.Value)
                            {
                                employee.AppendChild(MyCreateElement(doc, "PerOfPension", detail.PeriodFr_Type_2.ToString("yyyyMMdd") + " - " + detail.PeriodTo_Type_2.ToString("yyyyMMdd")));
                                employee.AppendChild(MyCreateElement(doc, "AmtOfPension", MaxStringLength(detail.Amount_Type_2.ToString("0"), 9)));
                            }
                            else
                            {
                                employee.AppendChild(MyCreateElement(doc, "PerOfPension", ""));
                                employee.AppendChild(MyCreateElement(doc, "AmtOfPension", "0"));
                            }
                            if (detail["PeriodFr_Type 3"] != System.DBNull.Value && detail["PeriodTo_Type 3"] != System.DBNull.Value && detail["Amount_Type 3"] != System.DBNull.Value)
                            {
                                employee.AppendChild(MyCreateElement(doc, "PerOfPension", detail.PeriodFr_Type_3.ToString("yyyyMMdd") + " - " + detail.PeriodTo_Type_3.ToString("yyyyMMdd")));
                                employee.AppendChild(MyCreateElement(doc, "AmtOfPension", MaxStringLength(detail.Amount_Type_3.ToString("0"), 9)));
                            }
                            else
                            {
                                employee.AppendChild(MyCreateElement(doc, "PerOfPension", ""));
                                employee.AppendChild(MyCreateElement(doc, "AmtOfPension", "0"));
                            }
                            if (detail["PeriodFr_Others (a)"] != System.DBNull.Value && detail["PeriodTo_Others (a)"] != System.DBNull.Value && detail["Amount_Others (a)"] != System.DBNull.Value)
                            {
                                employee.AppendChild(MyCreateElement(doc, "PerOfPension", detail._PeriodFr_Others__a_.ToString("yyyyMMdd") + " - " + detail._PeriodTo_Others__a_.ToString("yyyyMMdd")));
                                employee.AppendChild(MyCreateElement(doc, "AmtOfPension", MaxStringLength(detail._Amount_Others__a_.ToString("0"), 9)));
                            }
                            else
                            {
                                employee.AppendChild(MyCreateElement(doc, "PerOfPension", ""));
                                employee.AppendChild(MyCreateElement(doc, "AmtOfPension", "0"));
                            }
                            if (detail["PeriodFr_Others (b)"] != System.DBNull.Value && detail["PeriodTo_Others (b)"] != System.DBNull.Value && detail["Amount_Others (b)"] != System.DBNull.Value)
                            {
                                employee.AppendChild(MyCreateElement(doc, "PerOfPension", detail._PeriodFr_Others__b_.ToString("yyyyMMdd") + " - " + detail._PeriodTo_Others__b_.ToString("yyyyMMdd")));
                                employee.AppendChild(MyCreateElement(doc, "AmtOfPension", MaxStringLength(detail._Amount_Others__b_.ToString("0"), 9)));
                            }
                            else
                            {
                                employee.AppendChild(MyCreateElement(doc, "PerOfPension", ""));
                                employee.AppendChild(MyCreateElement(doc, "AmtOfPension", "0"));
                            }
                            if (detail["PeriodFr_Others (c)"] != System.DBNull.Value && detail["PeriodTo_Others (c)"] != System.DBNull.Value && detail["Amount_Others (c)"] != System.DBNull.Value)
                            {
                                employee.AppendChild(MyCreateElement(doc, "PerOfPension", detail._PeriodFr_Others__c_.ToString("yyyyMMdd") + " - " + detail._PeriodTo_Others__c_.ToString("yyyyMMdd")));
                                employee.AppendChild(MyCreateElement(doc, "AmtOfPension", MaxStringLength(detail._Amount_Others__c_.ToString("0"), 9)));
                            }
                            else
                            {
                                employee.AppendChild(MyCreateElement(doc, "PerOfPension", ""));
                                employee.AppendChild(MyCreateElement(doc, "AmtOfPension", "0"));
                            }
                            if (detail["PeriodFr_Others (d)"] != System.DBNull.Value && detail["PeriodTo_Others (d)"] != System.DBNull.Value && detail["Amount_Others (d)"] != System.DBNull.Value)
                            {
                                employee.AppendChild(MyCreateElement(doc, "NatureOtherD", MaxStringLength(detail["Nature_Others (d)"] != System.DBNull.Value ? detail._Nature_Others__d_ : string.Empty, 35)));
                                employee.AppendChild(MyCreateElement(doc, "PerOfPension", detail._PeriodFr_Others__d_.ToString("yyyyMMdd") + " - " + detail._PeriodTo_Others__d_.ToString("yyyyMMdd")));
                                employee.AppendChild(MyCreateElement(doc, "AmtOfPension", MaxStringLength(detail._Amount_Others__d_.ToString("0"), 9)));
                            }
                            else
                            {
                                employee.AppendChild(MyCreateElement(doc, "NatureOtherD", ""));
                                employee.AppendChild(MyCreateElement(doc, "PerOfPension", ""));
                                employee.AppendChild(MyCreateElement(doc, "AmtOfPension", "0"));
                            }
                            if (detail["PeriodFr_Others (e)"] != System.DBNull.Value && detail["PeriodTo_Others (e)"] != System.DBNull.Value && detail["Amount_Others (e)"] != System.DBNull.Value)
                            {
                                employee.AppendChild(MyCreateElement(doc, "NatureOtherE", MaxStringLength(detail["Nature_Others (e)"] != System.DBNull.Value ? detail._Nature_Others__e_ : string.Empty, 35)));
                                employee.AppendChild(MyCreateElement(doc, "PerOfPension", detail._PeriodFr_Others__e_.ToString("yyyyMMdd") + " - " + detail._PeriodTo_Others__e_.ToString("yyyyMMdd")));
                                employee.AppendChild(MyCreateElement(doc, "AmtOfPension", MaxStringLength(detail._Amount_Others__e_.ToString("0"), 9)));
                            }
                            else
                            {
                                employee.AppendChild(MyCreateElement(doc, "NatureOtherE", ""));
                                employee.AppendChild(MyCreateElement(doc, "PerOfPension", ""));
                                employee.AppendChild(MyCreateElement(doc, "AmtOfPension", "0"));
                            }
                        }
                        // End 0000020, KuangWei, 2014-08-04
                        
                        // #51 - #54
                        employee.AppendChild(MyCreateElement(doc, "TotalIncome", MaxStringLength(detail.TotalIncome.ToString("0"), 9)));
                        employee.AppendChild(MyCreateElement(doc, "PlaceOfResInd", MaxStringLength(detail.PlaceOfResidenceIndicator.ToString(), 1)));
                        employee.AppendChild(MyCreateElement(doc, "AddrOfPlace1", MaxStringLength(detail.PlaceOfResidenceAddress1, 110)));
                        employee.AppendChild(MyCreateElement(doc, "NatureOfPlace1", MaxStringLength(detail.PlaceOfResidenceNature1, 19)));
                        
                        // #55
                        if (detail["PlaceOfResidenceFr1"] != System.DBNull.Value && detail["PlaceOfResidenceTo1"] != System.DBNull.Value)
                            employee.AppendChild(MyCreateElement(doc, "PerOfPlace1", MaxStringLength(detail.PlaceOfResidenceFr1.ToString("yyyy/MM/dd") + " - " + detail.PlaceOfResidenceTo1.ToString("yyyy/MM/dd"), 26)));
                        else
                            employee.AppendChild(MyCreateElement(doc, "PerOfPlace1", ""));

                        // #56 - #61
                        employee.AppendChild(MyCreateElement(doc, "RentPaidEr1", MaxStringLength(detail["PlaceOfResidenceRentByER1"] != System.DBNull.Value ? detail.PlaceOfResidenceRentByER1.ToString() : "0", 7)));
                        employee.AppendChild(MyCreateElement(doc, "RentPaidEe1", MaxStringLength(detail["PlaceOfResidenceRentByEE1"] != System.DBNull.Value ? detail.PlaceOfResidenceRentByEE1.ToString() : "0", 7)));
                        employee.AppendChild(MyCreateElement(doc, "RentRefund1", MaxStringLength(detail["PlaceOfResidenceRefundedEE1"] != System.DBNull.Value ? detail.PlaceOfResidenceRefundedEE1.ToString() : "0", 7)));
                        employee.AppendChild(MyCreateElement(doc, "RentPaidErByEe1", MaxStringLength(detail["PlaceOfResidenceRentToERByEE1"] != System.DBNull.Value ? detail.PlaceOfResidenceRentToERByEE1.ToString() : "0", 7)));
                        employee.AppendChild(MyCreateElement(doc, "AddrOfPlace2", MaxStringLength(detail.PlaceOfResidenceAddress2, 110)));
                        employee.AppendChild(MyCreateElement(doc, "NatureOfPlace2", MaxStringLength(detail.PlaceOfResidenceNature2, 19)));
                        
                        // #62
                        if (detail["PlaceOfResidenceFr2"] != System.DBNull.Value && detail["PlaceOfResidenceTo2"] != System.DBNull.Value)
                            employee.AppendChild(MyCreateElement(doc, "PerOfPlace2", MaxStringLength(detail.PlaceOfResidenceFr2.ToString("yyyy/MM/dd") + " - " + detail.PlaceOfResidenceTo2.ToString("yyyy/MM/dd"), 26)));
                        else
                            employee.AppendChild(MyCreateElement(doc, "PerOfPlace2", ""));
                        
                        // #63 - #67
                        employee.AppendChild(MyCreateElement(doc, "RentPaidEr2", MaxStringLength(detail["PlaceOfResidenceRentByER2"] != System.DBNull.Value ? detail.PlaceOfResidenceRentByER2.ToString() : "0", 7)));
                        employee.AppendChild(MyCreateElement(doc, "RentPaidEe2", MaxStringLength(detail["PlaceOfResidenceRentByEE2"] != System.DBNull.Value ? detail.PlaceOfResidenceRentByEE2.ToString() : "0", 7)));
                        employee.AppendChild(MyCreateElement(doc, "RentRefund2", MaxStringLength(detail["PlaceOfResidenceRefundedEE2"] != System.DBNull.Value ? detail.PlaceOfResidenceRefundedEE2.ToString() : "0", 7)));
                        employee.AppendChild(MyCreateElement(doc, "RentPaidErByEe2", MaxStringLength(detail["PlaceOfResidenceRentToERByEE2"] != System.DBNull.Value ? detail.PlaceOfResidenceRentToERByEE2.ToString() : "0", 7)));
                        employee.AppendChild(MyCreateElement(doc, "OverseaIncInd", MaxStringLength(detail.OverseasIncomeIndicator.ToString(), 1)));
                        
                        // #68
                        if (detail.OverseasIncomeIndicator == 1 && detail.OverseasCompanyAmount.Trim().Equals(string.Empty))
                            employee.AppendChild(MyCreateElement(doc, "AmtPaidOverseaCo", "Unknown"));
                        else
                            employee.AppendChild(MyCreateElement(doc, "AmtPaidOverseaCo", MaxStringLength(detail.OverseasCompanyAmount, 20)));

                        // Start 0000020, KuangWei, 2014-08-06
                        // Start 0000187, Ricky So, 2015-04-16
                        if (m_taxForm.TaxFormType == "M")
                        // End 0000187, Ricky So, 2015-04-16
                        {
                            if (detail.SumWithheldIndicator == 1 && detail.SumWithheldAmount.Trim().Equals(string.Empty))
                                employee.AppendChild(MyCreateElement(doc, "AmtPaidSumWithheld", "Unknown"));
                            else
                                employee.AppendChild(MyCreateElement(doc, "AmtPaidSumWithheld", MaxStringLength(detail.SumWithheldAmount, 20)));
                        }
                        // End 0000020, KuangWei, 2014-08-06

                        // #69 - #71
                        employee.AppendChild(MyCreateElement(doc, "NameOfOverseaCo", MaxStringLength(detail.OverseasCompanyName, 60)));
                        employee.AppendChild(MyCreateElement(doc, "AddrOfOverseaCo", MaxStringLength(detail.OverseasCompanyAddress, 60)));
                        employee.AppendChild(MyCreateElement(doc, "Remarks", MaxStringLength(detail.Remarks, 60)));
                        ir56b.AppendChild(employee);
                    }
                }

            }

            return doc;

        }

        private static string BytePadLeftWithMaxLength(string originalString, int maxLength)
        {
            originalString = originalString.Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty).Trim();
            byte[] big5Bytes = System.Text.Encoding.Convert(System.Text.Encoding.Unicode, GetTaxationFileEncoding(), System.Text.Encoding.Unicode.GetBytes(originalString));
            char[] big5Chars = GetTaxationFileEncoding().GetChars(big5Bytes);
            originalString = new string(big5Chars);

            int byteCount = big5Bytes.Length;
            int wordCount = originalString.Length;

            if (byteCount <= maxLength)
                return originalString.PadLeft(maxLength - (byteCount - wordCount));
            else
            {
                return originalString.Substring(0, maxLength - (byteCount - wordCount));
            }
        }
        private static string BytePadRightWithMaxLength(string originalString, int maxLength)
        {
            originalString = originalString.Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty).Trim();
            byte[] big5Bytes = System.Text.Encoding.Convert(System.Text.Encoding.Unicode, GetTaxationFileEncoding(), System.Text.Encoding.Unicode.GetBytes(originalString));
            char[] big5Chars = GetTaxationFileEncoding().GetChars(big5Bytes);
            originalString = new string(big5Chars); 
            
            int byteCount = big5Bytes.Length;
            int wordCount = originalString.Length;

            if (byteCount <= maxLength)
                return originalString.PadRight(maxLength - (byteCount - wordCount));
            else
            {
                return originalString.Substring(0, maxLength - (byteCount - wordCount));
            }
            //return string.Empty;
        }
        private static System.Text.Encoding defaultTaxFileEncoding = System.Text.Encoding.GetEncoding(950);
        public static System.Text.Encoding GetTaxationFileEncoding()
        {
            return defaultTaxFileEncoding;
        }
    }
}