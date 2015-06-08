using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;
using HROne.Lib.Entities;

namespace HROne.Payroll
{
    public class CostAllocationProcess
    {
        private const string ERROR_TOTAL_PERCENTAGE_NOT_100 = "ERROR_TOTAL_PERCENTAGE_NOT_100";
        private const string MESSAGE_COST_CENTER_NOT_FOUND = "MESSAGE_COST_CENTER_NOT_FOUND";

        private StringBuilder errorLogBuiler = new StringBuilder();

        protected DatabaseConnection dbConn;

        public CostAllocationProcess(DatabaseConnection dbConn)
        {
            this.dbConn = dbConn;    
        }

        public string ErrorLog
        {
            get { return errorLogBuiler.ToString(); }
        }

        public bool hasError
        {
            get { return errorLogBuiler.Length > 0; }
        }

        public void TrialRun(int EmpPayrollID, EUser user)
        {
            EEmpPayroll empPayroll = new EEmpPayroll();
            empPayroll.EmpPayrollID = EmpPayrollID;
            if (EEmpPayroll.db.select(dbConn, empPayroll))
            {
                EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                empInfo.EmpID = empPayroll.EmpID;
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                {
                    EPayrollPeriod payPeriod = new EPayrollPeriod();
                    payPeriod.PayPeriodID = empPayroll.PayPeriodID;
                    if (EPayrollPeriod.db.select(dbConn, payPeriod))
                    {
                        DateTime startDate = payPeriod.PayPeriodFr;
                        DateTime endDate = payPeriod.PayPeriodTo;

                        if (empInfo.EmpDateOfJoin > payPeriod.PayPeriodFr && empInfo.EmpDateOfJoin<payPeriod.PayPeriodTo)
                            startDate = empInfo.EmpDateOfJoin;

                        DBFilter empTerminationFilter = new DBFilter();
                        empTerminationFilter.add(new Match("EmpID", empPayroll.EmpID));
                        ArrayList empTerminationList = EEmpTermination.db.select(dbConn, empTerminationFilter);
                        foreach (EEmpTermination empTerm in empTerminationList)
                        {
                            if (empTerm.EmpTermLastDate > payPeriod.PayPeriodFr && empTerm.EmpTermLastDate < payPeriod.PayPeriodTo)
                                endDate = empTerm.EmpTermLastDate;
                        }

                        DBFilter empPosFilter = new DBFilter();
                        empPosFilter.add(new Match("EmpID", empPayroll.EmpID));
                        empPosFilter.add(new Match("EmpPosEffFr", "<=", payPeriod.PayPeriodTo));
                        OR orEmpPosEffToTerms = new OR();
                        orEmpPosEffToTerms.add(new Match("EmpPosEffTo", ">=", payPeriod.PayPeriodFr));
                        orEmpPosEffToTerms.add(new NullTerm("EmpPosEffTo"));
                        empPosFilter.add(orEmpPosEffToTerms);
                        empPosFilter.add(new Match("PayGroupID", payPeriod.PayGroupID));

                        ArrayList empPosList = EEmpPositionInfo.db.select(dbConn, empPosFilter);

                        DBFilter empCostCenterFilter = new DBFilter();
                        empCostCenterFilter.add(new Match("EmpID", empPayroll.EmpID));
                        empCostCenterFilter.add(new Match("EmpCostCenterEffFr", "<=", payPeriod.PayPeriodTo));
                        OR orEmpCostCenterEffToTerms = new OR();
                        orEmpCostCenterEffToTerms.add(new Match("EmpCostCenterEffTo", ">=", payPeriod.PayPeriodFr));
                        orEmpCostCenterEffToTerms.add(new NullTerm("EmpCostCenterEffTo"));
                        empCostCenterFilter.add(orEmpCostCenterEffToTerms);

                        ArrayList empCostCenterList = EEmpCostCenter.db.select(dbConn, empCostCenterFilter);



                        ArrayList costAllocationDetailList = new ArrayList();
                        ArrayList costAllocationDetailHierarchyLIst = new ArrayList();

                        DBFilter paymentRecordFilter = new DBFilter();
                        paymentRecordFilter.add(new Match("EmpPayrollID", EmpPayrollID));

                        ArrayList paymentRecordList = EPaymentRecord.db.select(dbConn, paymentRecordFilter);

                        if (empCostCenterList.Count == 0 && paymentRecordList.Count > 0)
                        {
                            errorLogBuiler.AppendLine(
                                string.Format(
                                HROne.Common.WebUtility.GetLocalizedStringByCode(MESSAGE_COST_CENTER_NOT_FOUND, "Cost Center for Employee ({0}) has not been set.")
                                , new string[] { empInfo.EmpNo })
                            );
                            return;
                        }
                        
                        foreach (EPaymentRecord paymentRecord in paymentRecordList)
                        {
                            try
                            {
                                costAllocationDetailList.AddRange(CreateCostAllocationRecordList(startDate, endDate, empPosList, empCostCenterList, paymentRecord.PaymentCodeID, paymentRecord.PayRecActAmount, paymentRecord.PayRecID, false, paymentRecord.CostCenterID));
                            }
                            catch (Exception e)
                            {
                                errorLogBuiler.AppendLine(
                                    string.Format(
                                    e.Message
                                    , new string[] { empInfo.EmpNo })
                                );
                            }
                            //ArrayList costAllocationDetailListByPaymentRecord = new ArrayList();
                            //int daysTotal = 0;
                            //foreach (EEmpPositionInfo empPos in empPosList)
                            //{
                            //    DateTime posStartDate = startDate;
                            //    DateTime posEndDate = endDate;

                            //    if (empPos.EmpPosEffFr > posStartDate)
                            //        posStartDate = empPos.EmpPosEffFr;

                            //    if (empPos.EmpPosEffTo < posEndDate && empPos.EmpPosEffTo > posStartDate)
                            //        posEndDate = empPos.EmpPosEffTo;

                            //    DBFilter empHierarchyElementFilter = new DBFilter();
                            //    empHierarchyElementFilter.add(new Match("EmpPosID", empPos.EmpPosID));
                            //    ArrayList empHiererchyElementList = EEmpHierarchy.db.select(dbConn, empHierarchyElementFilter);

                            //    double percentagePaymentTotal = 0;

                            //    foreach (EEmpCostCenter empCostCenter in empCostCenterList)
                            //    {
                            //        DateTime costStartDate = posStartDate;
                            //        DateTime costEndDate = posEndDate;

                            //        if (empCostCenter.EmpCostCenterEffFr > costStartDate)
                            //            costStartDate = empCostCenter.EmpCostCenterEffFr;

                            //        if (empCostCenter.EmpCostCenterEffTo < costEndDate && empCostCenter.EmpCostCenterEffTo > costStartDate)
                            //            costEndDate = empCostCenter.EmpCostCenterEffTo;

                            //        if (costStartDate > costEndDate)
                            //            continue;

                            //        int numOfDays = ((TimeSpan)costEndDate.Subtract(costStartDate)).Days;
                            //        daysTotal += numOfDays;

                            //        DBFilter empCostCenterDetailFilter = new DBFilter();
                            //        empCostCenterDetailFilter.add(new Match("EmpCostCenterID", empCostCenter.EmpCostCenterID));
                            //        ArrayList empCostCenterDetailList = EEmpCostCenterDetail.db.select(dbConn, empCostCenterDetailFilter);

                            //        double percentageCostCenterTotal = 0;
                            //        foreach (EEmpCostCenterDetail empCostCenterDetail in empCostCenterDetailList)
                            //        {

                            //            ECostAllocationDetail costAllocDetail = new ECostAllocationDetail();
                            //            costAllocDetail.CostCenterID = empCostCenterDetail.CostCenterID;
                            //            costAllocDetail.CostAllocationDetailRoundCalAmount = paymentRecord.PayRecActAmount * daysTotal * empCostCenterDetail.EmpCostCenterPercentage;
                            //            costAllocDetail.PaymentCodeID = paymentRecord.PaymentCodeID;
                            //            costAllocDetail.PayRecID = paymentRecord.PayRecID;
                            //            costAllocDetail.CompanyID = empPos.CompanyID;
                            //            costAllocationDetailListByPaymentRecord.Add(costAllocDetail);

                            //            foreach (EEmpHierarchy empHierarchy in empHiererchyElementList)
                            //            {
                            //                ECostAllocationDetailHElement costAllocHierarchy = new ECostAllocationDetailHElement();
                            //                costAllocHierarchy.HElementID = empHierarchy.HElementID;
                            //                costAllocHierarchy.HLevelID = empHierarchy.HLevelID;
                            //                costAllocHierarchy.RelatedCostAllocationDetailObject = costAllocDetail;
                            //                costAllocationDetailHierarchyLIst.Add(costAllocHierarchy);
                            //            }

                            //            percentageCostCenterTotal += empCostCenterDetail.EmpCostCenterPercentage;

                                        
                            //        }

                            //        if (Math.Round(Math.Abs(percentageCostCenterTotal - 100), 2, MidpointRounding.AwayFromZero) > 0)
                            //        {
                            //            errorLogBuiler.AppendLine(
                            //                string.Format(
                            //                HROne.Common.WebUtility.GetLocalizedStringByCode(MESSAGE_NOT_100_PERCENTAGE, "Total Percentage of Employee ({0}) is not 100%.")
                            //                , new string[] { empInfo.EmpNo })
                            //            );
                            //        }
                            //    }

                            //}

                            ////SortedList sortedCostAllocationDetailbyDecimalPlaceRoundDown = new SortedList();
                            ////SortedList sortedCostAllocationDetailbyDecimalPlaceRoundUp = new SortedList();

                            //double totalCostAllocPayment = 0;
                            //foreach (ECostAllocationDetail costAllocationDetail in costAllocationDetailListByPaymentRecord)
                            //{
                            //    costAllocationDetail.CostAllocationDetailRoundCalAmount = costAllocationDetail.CostAllocationDetailRoundCalAmount / (100 * daysTotal);
                            //    costAllocationDetail.CostAllocationDetailAmount = Math.Round(costAllocationDetail.CostAllocationDetailRoundCalAmount, 2, MidpointRounding.AwayFromZero);
                            //    totalCostAllocPayment += costAllocationDetail.CostAllocationDetailAmount;

                            //    //sortedCostAllocationDetailbyDecimalPlaceRoundDown.Add(costAllocationDetail.CostAllocationDetailAmount - originalCostAllocationAmount, costAllocationDetail.CostAllocationDetailAmount);
                            //    //sortedCostAllocationDetailbyDecimalPlaceRoundUp.Add(originalCostAllocationAmount - costAllocationDetail.CostAllocationDetailAmount, costAllocationDetail.CostAllocationDetailAmount);

                            //}
                            //double diff = Math.Round(paymentRecord.PayRecActAmount - totalCostAllocPayment, 2, MidpointRounding.AwayFromZero);
                            //if (Math.Abs(diff) >= 0.01 && costAllocationDetailListByPaymentRecord.Count>0)
                            //{
                            //    if (diff > 0)
                            //    {
                            //        while (Math.Abs(diff) >= 0.01)
                            //        {
                            //            ECostAllocationDetail minCostAllocationDetail = null;
                            //            double minDiff = 1;
                            //            foreach (ECostAllocationDetail costAllocationDetail in costAllocationDetailListByPaymentRecord)
                            //            {
                            //                double costAllocDiff= costAllocationDetail.CostAllocationDetailAmount -costAllocationDetail.CostAllocationDetailRoundCalAmount;
                            //                if (costAllocDiff < minDiff)
                            //                {
                            //                    minDiff = costAllocDiff;
                            //                    minCostAllocationDetail = costAllocationDetail;
                            //                }
                            //            }
                            //            if (minCostAllocationDetail != null)
                            //            {
                            //                minCostAllocationDetail.CostAllocationDetailAmount += 0.01;
                            //                diff -= 0.01;
                            //            }
                            //        }
                            //    }
                            //    else
                            //    {
                            //        while (Math.Abs(diff) >= 0.01)
                            //        {
                            //            ECostAllocationDetail maxCostAllocationDetail = null;
                            //            double maxDiff = 1;
                            //            foreach (ECostAllocationDetail costAllocationDetail in costAllocationDetailListByPaymentRecord)
                            //            {
                            //                double costAllocDiff = costAllocationDetail.CostAllocationDetailAmount - costAllocationDetail.CostAllocationDetailRoundCalAmount;
                            //                if (costAllocDiff > maxDiff)
                            //                {
                            //                    maxDiff = costAllocDiff;
                            //                    maxCostAllocationDetail = costAllocationDetail;
                            //                }
                            //            }
                            //            if (maxCostAllocationDetail != null)
                            //            {
                            //                maxCostAllocationDetail.CostAllocationDetailAmount -= 0.01;
                            //                diff += 0.01;
                            //            }
                            //        }
                            //    }
                            //}
                            //costAllocationDetailList.AddRange(costAllocationDetailListByPaymentRecord);

                        }

                        ArrayList mpfRecordList = EMPFRecord.db.select(dbConn, paymentRecordFilter);

                        foreach (EMPFRecord mpfRecord in mpfRecordList)
                        {
                            try
                            {
                                ArrayList paymentList = PayrollProcess.GetPaymentCodeByPaymentType(dbConn, EPaymentType.SystemPaymentType.MPFEmployeeMandatoryContributionPaymentType(dbConn).PaymentTypeCode);
                                if (paymentList.Count > 0)
                                    costAllocationDetailList.AddRange(CreateCostAllocationRecordList(startDate, endDate, empPosList, empCostCenterList, ((EPaymentCode)paymentList[0]).PaymentCodeID, mpfRecord.MPFRecActMCEE, mpfRecord.MPFRecordID, true));
                                paymentList = PayrollProcess.GetPaymentCodeByPaymentType(dbConn, EPaymentType.SystemPaymentType.MPFEmployerMandatoryContributionPaymentType(dbConn).PaymentTypeCode);
                                if (paymentList.Count > 0)
                                    costAllocationDetailList.AddRange(CreateCostAllocationRecordList(startDate, endDate, empPosList, empCostCenterList, ((EPaymentCode)paymentList[0]).PaymentCodeID, mpfRecord.MPFRecActMCER, mpfRecord.MPFRecordID, true));
                                paymentList = PayrollProcess.GetPaymentCodeByPaymentType(dbConn, EPaymentType.SystemPaymentType.MPFEmployeeVoluntaryContributionPaymentType(dbConn).PaymentTypeCode);
                                if (paymentList.Count > 0)
                                    costAllocationDetailList.AddRange(CreateCostAllocationRecordList(startDate, endDate, empPosList, empCostCenterList, ((EPaymentCode)paymentList[0]).PaymentCodeID, mpfRecord.MPFRecActVCEE, mpfRecord.MPFRecordID, true));
                                paymentList = PayrollProcess.GetPaymentCodeByPaymentType(dbConn, EPaymentType.SystemPaymentType.MPFEmployerVoluntaryContributionPaymentType(dbConn).PaymentTypeCode);
                                if (paymentList.Count > 0)
                                    costAllocationDetailList.AddRange(CreateCostAllocationRecordList(startDate, endDate, empPosList, empCostCenterList, ((EPaymentCode)paymentList[0]).PaymentCodeID, mpfRecord.MPFRecActVCER, mpfRecord.MPFRecordID, true));
                            }
                            catch (Exception e)
                            {
                                errorLogBuiler.AppendLine(
                                    string.Format(
                                    e.Message
                                    , new string[] { empInfo.EmpNo })
                                );
                            }
                        }

                        ArrayList orsoRecordList = EORSORecord.db.select(dbConn, paymentRecordFilter);

                        foreach (EORSORecord orsoRecord in orsoRecordList)
                        {
                            try
                            {
                                ArrayList paymentList = PayrollProcess.GetPaymentCodeByPaymentType(dbConn, EPaymentType.SystemPaymentType.PFundEmployeeContributionPaymentType(dbConn).PaymentTypeCode);
                                if (paymentList.Count > 0)
                                    costAllocationDetailList.AddRange(CreateCostAllocationRecordList(startDate, endDate, empPosList, empCostCenterList, ((EPaymentCode)paymentList[0]).PaymentCodeID, orsoRecord.ORSORecActEE, orsoRecord.ORSORecordID, true));
                                paymentList = PayrollProcess.GetPaymentCodeByPaymentType(dbConn, EPaymentType.SystemPaymentType.PFundEmployerContributionPaymentType(dbConn).PaymentTypeCode);
                                if (paymentList.Count > 0)
                                    costAllocationDetailList.AddRange(CreateCostAllocationRecordList(startDate, endDate, empPosList, empCostCenterList, ((EPaymentCode)paymentList[0]).PaymentCodeID, orsoRecord.ORSORecActER, orsoRecord.ORSORecordID, true));
                            }
                            catch (Exception e)
                            {
                                errorLogBuiler.AppendLine(
                                    string.Format(
                                    e.Message
                                    , new string[] { empInfo.EmpNo })
                                );
                            }
                        }

                        ECostAllocation costAllocation = new ECostAllocation();
                        costAllocation.EmpID = empInfo.EmpID;
                        costAllocation.CostAllocationStatus = "T";
                        costAllocation.CostAllocationTrialRunDate = AppUtils.ServerDateTime();
                        costAllocation.CostAllocationTrialRunBy = user.UserID;
                        costAllocation.EmpPayrollID = EmpPayrollID;
                        ECostAllocation.db.insert(dbConn, costAllocation);
                        foreach (ECostAllocationDetail costAllocationDetail in costAllocationDetailList)
                        {
                            costAllocationDetail.CostAllocationID = costAllocation.CostAllocationID;
                            ECostAllocationDetail.db.insert(dbConn, costAllocationDetail);
                            foreach (ECostAllocationDetailHElement costAllocationDetailHierarchy in costAllocationDetail.HierarchyElementList)
                            {
                                costAllocationDetailHierarchy.CostAllocationDetailID = costAllocationDetail.CostAllocationDetailID;
                                ECostAllocationDetailHElement.db.insert(dbConn, costAllocationDetailHierarchy);
                            }
                        }
                    }
                } 
            }
        }
        private ArrayList CreateCostAllocationRecordList(DateTime startDate, DateTime endDate, ArrayList empPosList, ArrayList empCostCenterList, int PaymentCodeID, double Amount, int PaymentRecordID, bool IsContribution)
        {
            return CreateCostAllocationRecordList(startDate, endDate, empPosList, empCostCenterList, PaymentCodeID, Amount, PaymentRecordID, IsContribution, 0);
        }
        private ArrayList CreateCostAllocationRecordList(DateTime startDate, DateTime endDate, ArrayList empPosList, ArrayList empCostCenterList, int PaymentCodeID, double Amount, int PaymentRecordID, bool IsContribution, int overrideCostCenterID)
        {
            if (!Amount.Equals(0))
            {
                ArrayList costAllocationDetailList = new ArrayList();
                int daysTotal = 0;

                foreach (EEmpPositionInfo empPos in empPosList)
                {
                    DateTime posStartDate = startDate;
                    DateTime posEndDate = endDate;

                    if (empPos.EmpPosEffFr > posStartDate)
                        posStartDate = empPos.EmpPosEffFr;

                    if (!empPos.EmpPosEffTo.Ticks.Equals(0))
                        if (empPos.EmpPosEffTo < posEndDate)
                            if (empPos.EmpPosEffTo >= posStartDate)
                                posEndDate = empPos.EmpPosEffTo;
                            else
                                posEndDate = new DateTime();

                    DBFilter empHierarchyElementFilter = new DBFilter();
                    empHierarchyElementFilter.add(new Match("EmpPosID", empPos.EmpPosID));
                    ArrayList empHiererchyElementList = EEmpHierarchy.db.select(dbConn, empHierarchyElementFilter);


                    ECostCenter overrideCostCenter = new ECostCenter();
                    overrideCostCenter.CostCenterID = overrideCostCenterID;
                    if (ECostCenter.db.select(dbConn, overrideCostCenter))
                    {
                        DateTime costStartDate = posStartDate;
                        DateTime costEndDate = posEndDate;
                        int numOfDays = ((TimeSpan)costEndDate.AddDays(1).Subtract(costStartDate)).Days;
                        daysTotal += numOfDays;

                        ECostAllocationDetail costAllocDetail = new ECostAllocationDetail();
                        costAllocDetail.HierarchyElementList = new ArrayList();
                        costAllocDetail.CostCenterID = overrideCostCenterID;
                        costAllocDetail.CostAllocationDetailRoundCalAmount = Amount * numOfDays * 100;
                        costAllocDetail.PaymentCodeID = PaymentCodeID;
                        costAllocDetail.PayRecID = PaymentRecordID;
                        costAllocDetail.CompanyID = empPos.CompanyID;
                        costAllocDetail.CostAllocationDetailIsContribution = IsContribution;
                        costAllocationDetailList.Add(costAllocDetail);

                        foreach (EEmpHierarchy empHierarchy in empHiererchyElementList)
                        {
                            ECostAllocationDetailHElement costAllocHierarchy = new ECostAllocationDetailHElement();
                            costAllocHierarchy.HElementID = empHierarchy.HElementID;
                            costAllocHierarchy.HLevelID = empHierarchy.HLevelID;
                            costAllocHierarchy.RelatedCostAllocationDetailObject = costAllocDetail;
                            costAllocDetail.HierarchyElementList.Add(costAllocHierarchy);
                        }
                    }
                    else
                    {
                        //double percentagePaymentTotal = 0;


                        foreach (EEmpCostCenter empCostCenter in empCostCenterList)
                        {
                            DateTime costStartDate = posStartDate;
                            DateTime costEndDate = posEndDate;

                            if (empCostCenter.EmpCostCenterEffFr > costStartDate)
                                costStartDate = empCostCenter.EmpCostCenterEffFr;

                            if (!empCostCenter.EmpCostCenterEffTo.Ticks.Equals(0))
                                if (empCostCenter.EmpCostCenterEffTo < costEndDate)
                                    if (empCostCenter.EmpCostCenterEffTo >= costStartDate)
                                        costEndDate = empCostCenter.EmpCostCenterEffTo;
                                    else
                                        costEndDate = new DateTime();

                            if (costStartDate > costEndDate)
                                continue;

                            int numOfDays = ((TimeSpan)costEndDate.AddDays(1).Subtract(costStartDate)).Days;
                            daysTotal += numOfDays;

                            DBFilter empCostCenterDetailFilter = new DBFilter();
                            empCostCenterDetailFilter.add(new Match("EmpCostCenterID", empCostCenter.EmpCostCenterID));
                            ArrayList empCostCenterDetailList = EEmpCostCenterDetail.db.select(dbConn, empCostCenterDetailFilter);

                            double percentageCostCenterTotal = 0;
                            foreach (EEmpCostCenterDetail empCostCenterDetail in empCostCenterDetailList)
                            {

                                ECostAllocationDetail costAllocDetail = new ECostAllocationDetail();
                                costAllocDetail.HierarchyElementList = new ArrayList();
                                costAllocDetail.CostCenterID = empCostCenterDetail.CostCenterID;
                                costAllocDetail.CostAllocationDetailRoundCalAmount = Amount * numOfDays * empCostCenterDetail.EmpCostCenterPercentage;
                                costAllocDetail.PaymentCodeID = PaymentCodeID;
                                costAllocDetail.PayRecID = PaymentRecordID;
                                costAllocDetail.CompanyID = empPos.CompanyID;
                                costAllocDetail.CostAllocationDetailIsContribution = IsContribution;
                                costAllocationDetailList.Add(costAllocDetail);

                                foreach (EEmpHierarchy empHierarchy in empHiererchyElementList)
                                {
                                    ECostAllocationDetailHElement costAllocHierarchy = new ECostAllocationDetailHElement();
                                    costAllocHierarchy.HElementID = empHierarchy.HElementID;
                                    costAllocHierarchy.HLevelID = empHierarchy.HLevelID;
                                    costAllocHierarchy.RelatedCostAllocationDetailObject = costAllocDetail;
                                    costAllocDetail.HierarchyElementList.Add(costAllocHierarchy);
                                }

                                percentageCostCenterTotal += empCostCenterDetail.EmpCostCenterPercentage;


                            }

                            if (Math.Round(Math.Abs(percentageCostCenterTotal - 100), 2, MidpointRounding.AwayFromZero) > 0)
                            {
                                throw new Exception(HROne.Common.WebUtility.GetLocalizedStringByCode(ERROR_TOTAL_PERCENTAGE_NOT_100, "Total percentage must be 100%.") + "(" + HROne.Common.WebUtility.GetLocalizedStringByCode("EmpNo", "Employee No") + ": {0})");
                            }
                        }
                    }
                }
                double totalCostAllocPayment = 0;

                foreach (ECostAllocationDetail costAllocationDetail in costAllocationDetailList)
                {
                    costAllocationDetail.CostAllocationDetailRoundCalAmount = costAllocationDetail.CostAllocationDetailRoundCalAmount / (100 * daysTotal);
                    costAllocationDetail.CostAllocationDetailAmount = Math.Round(costAllocationDetail.CostAllocationDetailRoundCalAmount, 2, MidpointRounding.AwayFromZero);
                    totalCostAllocPayment += costAllocationDetail.CostAllocationDetailAmount;
                }

                double diff = Math.Round(Amount - totalCostAllocPayment, 2, MidpointRounding.AwayFromZero);
                if (Math.Abs(diff) >= 0.01 && costAllocationDetailList.Count > 0)
                {
                    if (diff > 0)
                    {
                        while (diff >= 0.01)
                        {
                            ECostAllocationDetail minCostAllocationDetail = null;
                            double minDiff = 1;
                            foreach (ECostAllocationDetail costAllocationDetail in costAllocationDetailList)
                            {
                                double costAllocDiff = costAllocationDetail.CostAllocationDetailAmount - costAllocationDetail.CostAllocationDetailRoundCalAmount;
                                if (costAllocDiff < minDiff)
                                {
                                    minDiff = costAllocDiff;
                                    minCostAllocationDetail = costAllocationDetail;
                                }
                            }
                            if (minCostAllocationDetail != null)
                            {
                                minCostAllocationDetail.CostAllocationDetailAmount += 0.01;
                                diff -= 0.01;
                            }
                        }
                    }
                    else
                    {
                        while (diff <= -0.01)
                        {
                            ECostAllocationDetail maxCostAllocationDetail = null;
                            double maxDiff = -1;
                            foreach (ECostAllocationDetail costAllocationDetail in costAllocationDetailList)
                            {
                                double costAllocDiff = costAllocationDetail.CostAllocationDetailAmount - costAllocationDetail.CostAllocationDetailRoundCalAmount;
                                if (costAllocDiff > maxDiff)
                                {
                                    maxDiff = costAllocDiff;
                                    maxCostAllocationDetail = costAllocationDetail;
                                }
                            }
                            if (maxCostAllocationDetail != null)
                            {
                                maxCostAllocationDetail.CostAllocationDetailAmount -= 0.01;
                                diff += 0.01;
                            }
                        }
                    }
                }
                //costAllocationDetailList.AddRange(costAllocationDetailList);


                return costAllocationDetailList;
            }
            else
                return new ArrayList();
            //ArrayList mpfRecordList = EMPFRecord.db.select(dbConn, paymentRecordFilter);

            //foreach (EMPFRecord mpfRecord in mpfRecordList)
            //{
            //    ArrayList costAllocationDetailListByMPFRecordMCEE = new ArrayList();
            //    ArrayList costAllocationDetailListByMPFRecordMCER = new ArrayList();
            //    ArrayList costAllocationDetailListByMPFRecordVCEE = new ArrayList();
            //    ArrayList costAllocationDetailListByMPFRecordVCER = new ArrayList();
            //    int daysTotal = 0;
            //    foreach (EEmpPositionInfo empPos in empPosList)
            //    {
            //        DateTime posStartDate = startDate;
            //        DateTime posEndDate = endDate;

            //        if (empPos.EmpPosEffFr > posStartDate)
            //            posStartDate = empPos.EmpPosEffFr;

            //        if (empPos.EmpPosEffTo < posEndDate && empPos.EmpPosEffTo > posStartDate)
            //            posEndDate = empPos.EmpPosEffTo;

            //        DBFilter empHierarchyElementFilter = new DBFilter();
            //        empHierarchyElementFilter.add(new Match("EmpPosID", empPos.EmpPosID));
            //        ArrayList empHiererchyElementList = EEmpHierarchy.db.select(dbConn, empHierarchyElementFilter);

            //        double percentagePaymentTotal = 0;

            //        foreach (EEmpCostCenter empCostCenter in empCostCenterList)
            //        {
            //            DateTime costStartDate = posStartDate;
            //            DateTime costEndDate = posEndDate;

            //            if (empCostCenter.EmpCostCenterEffFr > costStartDate)
            //                costStartDate = empCostCenter.EmpCostCenterEffFr;

            //            if (empCostCenter.EmpCostCenterEffTo < costEndDate && empCostCenter.EmpCostCenterEffTo > costStartDate)
            //                costEndDate = empCostCenter.EmpCostCenterEffTo;

            //            if (costStartDate > costEndDate)
            //                continue;

            //            int numOfDays = ((TimeSpan)costEndDate.Subtract(costStartDate)).Days;
            //            daysTotal += numOfDays;

            //            DBFilter empCostCenterDetailFilter = new DBFilter();
            //            empCostCenterDetailFilter.add(new Match("EmpCostCenterID", empCostCenter.EmpCostCenterID));
            //            ArrayList empCostCenterDetailList = EEmpCostCenterDetail.db.select(dbConn, empCostCenterDetailFilter);

            //            double percentageCostCenterTotal = 0;
            //            foreach (EEmpCostCenterDetail empCostCenterDetail in empCostCenterDetailList)
            //            {

            //                ECostAllocationDetail costAllocMCEEDetail = new ECostAllocationDetail();
            //                ECostAllocationDetail costAllocMCERDetail = new ECostAllocationDetail();
            //                ECostAllocationDetail costAllocVCEEDetail = new ECostAllocationDetail();
            //                ECostAllocationDetail costAllocVCERDetail = new ECostAllocationDetail();
            //                costAllocMCEEDetail.CostCenterID = empCostCenterDetail.CostCenterID;
            //                costAllocMCEEDetail.CostAllocationDetailRoundCalAmount = mpfRecord.MPFRecActMCEE * daysTotal * empCostCenterDetail.EmpCostCenterPercentage;
            //                costAllocMCEEDetail.PaymentCodeID = ((EPaymentCode)PayrollProcess.GetPaymentCodeByPaymentType(dbConn, "MPFEE")[0]).PaymentCode;
            //                costAllocMCEEDetail.PayRecID = mpfRecord.MPFRecordID;
            //                costAllocMCEEDetail.CompanyID = empPos.CompanyID;
            //                costAllocationDetailListByMPFRecordMCEE.Add(costAllocMCEEDetail);

            //                foreach (EEmpHierarchy empHierarchy in empHiererchyElementList)
            //                {
            //                    ECostAllocationDetailHElement costAllocHierarchy = new ECostAllocationDetailHElement();
            //                    costAllocHierarchy.HElementID = empHierarchy.HElementID;
            //                    costAllocHierarchy.HLevelID = empHierarchy.HLevelID;
            //                    costAllocHierarchy.RelatedCostAllocationDetailObject = costAllocMCEEDetail;
            //                    costAllocationDetailHierarchyLIst.Add(costAllocHierarchy);
            //                }

            //                costAllocMCERDetail.CostCenterID = empCostCenterDetail.CostCenterID;
            //                costAllocMCERDetail.CostAllocationDetailRoundCalAmount = mpfRecord.MPFRecActMCER * daysTotal * empCostCenterDetail.EmpCostCenterPercentage;
            //                costAllocMCERDetail.PaymentCodeID = ((EPaymentCode)PayrollProcess.GetPaymentCodeByPaymentType(dbConn, "MPFER")[0]).PaymentCode;
            //                costAllocMCERDetail.PayRecID = mpfRecord.MPFRecordID;
            //                costAllocMCERDetail.CompanyID = empPos.CompanyID;
            //                costAllocationDetailListByMPFRecordMCER.Add(costAllocMCERDetail);

            //                foreach (EEmpHierarchy empHierarchy in empHiererchyElementList)
            //                {
            //                    ECostAllocationDetailHElement costAllocHierarchy = new ECostAllocationDetailHElement();
            //                    costAllocHierarchy.HElementID = empHierarchy.HElementID;
            //                    costAllocHierarchy.HLevelID = empHierarchy.HLevelID;
            //                    costAllocHierarchy.RelatedCostAllocationDetailObject = costAllocMCERDetail;
            //                    costAllocationDetailHierarchyLIst.Add(costAllocHierarchy);
            //                }

            //                costAllocVCEEDetail.CostCenterID = empCostCenterDetail.CostCenterID;
            //                costAllocVCEEDetail.CostAllocationDetailRoundCalAmount = mpfRecord.MPFRecActVCEE * daysTotal * empCostCenterDetail.EmpCostCenterPercentage;
            //                costAllocVCEEDetail.PaymentCodeID = ((EPaymentCode)PayrollProcess.GetPaymentCodeByPaymentType(dbConn, "TOPUPEE")[0]).PaymentCode;
            //                costAllocVCEEDetail.PayRecID = mpfRecord.MPFRecordID;
            //                costAllocVCEEDetail.CompanyID = empPos.CompanyID;
            //                costAllocationDetailListByMPFRecordVCEE.Add(costAllocVCEEDetail);

            //                foreach (EEmpHierarchy empHierarchy in empHiererchyElementList)
            //                {
            //                    ECostAllocationDetailHElement costAllocHierarchy = new ECostAllocationDetailHElement();
            //                    costAllocHierarchy.HElementID = empHierarchy.HElementID;
            //                    costAllocHierarchy.HLevelID = empHierarchy.HLevelID;
            //                    costAllocHierarchy.RelatedCostAllocationDetailObject = costAllocVCEEDetail;
            //                    costAllocationDetailHierarchyLIst.Add(costAllocHierarchy);
            //                }

            //                costAllocVCERDetail.CostCenterID = empCostCenterDetail.CostCenterID;
            //                costAllocVCERDetail.CostAllocationDetailRoundCalAmount = mpfRecord.MPFRecActVCER * daysTotal * empCostCenterDetail.EmpCostCenterPercentage;
            //                costAllocVCERDetail.PaymentCodeID = ((EPaymentCode)PayrollProcess.GetPaymentCodeByPaymentType(dbConn, "TOPUPER")[0]).PaymentCode;
            //                costAllocVCERDetail.PayRecID = mpfRecord.MPFRecordID;
            //                costAllocVCERDetail.CompanyID = empPos.CompanyID;
            //                costAllocationDetailListByMPFRecordVCER.Add(costAllocVCERDetail);

            //                foreach (EEmpHierarchy empHierarchy in empHiererchyElementList)
            //                {
            //                    ECostAllocationDetailHElement costAllocHierarchy = new ECostAllocationDetailHElement();
            //                    costAllocHierarchy.HElementID = empHierarchy.HElementID;
            //                    costAllocHierarchy.HLevelID = empHierarchy.HLevelID;
            //                    costAllocHierarchy.RelatedCostAllocationDetailObject = costAllocVCERDetail;
            //                    costAllocationDetailHierarchyLIst.Add(costAllocHierarchy);
            //                }

            //                percentageCostCenterTotal += empCostCenterDetail.EmpCostCenterPercentage;


            //            }

            //            if (Math.Round(Math.Abs(percentageCostCenterTotal - 100), 2, MidpointRounding.AwayFromZero) > 0)
            //            {
            //                errorLogBuiler.AppendLine(
            //                    string.Format(
            //                    HROne.Common.WebUtility.GetLocalizedStringByCode(MESSAGE_NOT_100_PERCENTAGE, "Total Percentage of Employee ({0}) is not 100%.")
            //                    , new string[] { empInfo.EmpNo })
            //                );
            //            }
            //        }

            //    }

            //    //SortedList sortedCostAllocationDetailbyDecimalPlaceRoundDown = new SortedList();
            //    //SortedList sortedCostAllocationDetailbyDecimalPlaceRoundUp = new SortedList();

            //    double totalCostAllocPaymentMCEE = 0;
            //    double totalCostAllocPaymentMCER = 0;
            //    double totalCostAllocPaymentVCEE = 0;
            //    double totalCostAllocPaymentVCER = 0;
            //    foreach (ECostAllocationDetail costAllocationDetail in costAllocationDetailListByMPFRecordMCEE)
            //    {
            //        costAllocationDetail.CostAllocationDetailRoundCalAmount = costAllocationDetail.CostAllocationDetailRoundCalAmount / (100 * daysTotal);
            //        costAllocationDetail.CostAllocationDetailAmount = Math.Round(costAllocationDetail.CostAllocationDetailRoundCalAmount, 2, MidpointRounding.AwayFromZero);
            //        totalCostAllocPaymentMCEE += costAllocationDetail.CostAllocationDetailAmount;
            //    }
            //    foreach (ECostAllocationDetail costAllocationDetail in costAllocationDetailListByMPFRecordMCER)
            //    {
            //        costAllocationDetail.CostAllocationDetailRoundCalAmount = costAllocationDetail.CostAllocationDetailRoundCalAmount / (100 * daysTotal);
            //        costAllocationDetail.CostAllocationDetailAmount = Math.Round(costAllocationDetail.CostAllocationDetailRoundCalAmount, 2, MidpointRounding.AwayFromZero);
            //        totalCostAllocPaymentMCER += costAllocationDetail.CostAllocationDetailAmount;
            //    }
            //    foreach (ECostAllocationDetail costAllocationDetail in costAllocationDetailListByMPFRecordVCEE)
            //    {
            //        costAllocationDetail.CostAllocationDetailRoundCalAmount = costAllocationDetail.CostAllocationDetailRoundCalAmount / (100 * daysTotal);
            //        costAllocationDetail.CostAllocationDetailAmount = Math.Round(costAllocationDetail.CostAllocationDetailRoundCalAmount, 2, MidpointRounding.AwayFromZero);
            //        totalCostAllocPaymentVCEE += costAllocationDetail.CostAllocationDetailAmount;
            //    }
            //    foreach (ECostAllocationDetail costAllocationDetail in costAllocationDetailListByMPFRecordVCER)
            //    {
            //        costAllocationDetail.CostAllocationDetailRoundCalAmount = costAllocationDetail.CostAllocationDetailRoundCalAmount / (100 * daysTotal);
            //        costAllocationDetail.CostAllocationDetailAmount = Math.Round(costAllocationDetail.CostAllocationDetailRoundCalAmount, 2, MidpointRounding.AwayFromZero);
            //        totalCostAllocPaymentVCER += costAllocationDetail.CostAllocationDetailAmount;
            //    }

            //    double diff = Math.Round(mpfRecord.PayRecActAmount - totalCostAllocPayment, 2, MidpointRounding.AwayFromZero);
            //    if (Math.Abs(diff) >= 0.01 && costAllocationDetailListByMPFRecord.Count > 0)
            //    {
            //        if (diff > 0)
            //        {
            //            while (Math.Abs(diff) >= 0.01)
            //            {
            //                ECostAllocationDetail minCostAllocationDetail = null;
            //                double minDiff = 1;
            //                foreach (ECostAllocationDetail costAllocationDetail in costAllocationDetailListByMPFRecord)
            //                {
            //                    double costAllocDiff = costAllocationDetail.CostAllocationDetailAmount - costAllocationDetail.CostAllocationDetailRoundCalAmount;
            //                    if (costAllocDiff < minDiff)
            //                    {
            //                        minDiff = costAllocDiff;
            //                        minCostAllocationDetail = costAllocationDetail;
            //                    }
            //                }
            //                if (minCostAllocationDetail != null)
            //                {
            //                    minCostAllocationDetail.CostAllocationDetailAmount += 0.01;
            //                    diff -= 0.01;
            //                }
            //            }
            //        }
            //        else
            //        {
            //            while (Math.Abs(diff) >= 0.01)
            //            {
            //                ECostAllocationDetail maxCostAllocationDetail = null;
            //                double maxDiff = 1;
            //                foreach (ECostAllocationDetail costAllocationDetail in costAllocationDetailListByMPFRecord)
            //                {
            //                    double costAllocDiff = costAllocationDetail.CostAllocationDetailAmount - costAllocationDetail.CostAllocationDetailRoundCalAmount;
            //                    if (costAllocDiff > maxDiff)
            //                    {
            //                        maxDiff = costAllocDiff;
            //                        maxCostAllocationDetail = costAllocationDetail;
            //                    }
            //                }
            //                if (maxCostAllocationDetail != null)
            //                {
            //                    maxCostAllocationDetail.CostAllocationDetailAmount -= 0.01;
            //                    diff += 0.01;
            //                }
            //            }
            //        }
            //    }
            //    costAllocationDetailList.AddRange(costAllocationDetailListByMPFRecord);
            //}

        }

        public void Confirm(int CostAllocationID, EUser user)
        {
            ECostAllocation costAllocation = new ECostAllocation();
            costAllocation.CostAllocationID = CostAllocationID;
            if (ECostAllocation.db.select(dbConn, costAllocation))
            {
                if (costAllocation.CostAllocationStatus.Equals("T"))
                {
                    costAllocation.CostAllocationStatus = "C";
                    costAllocation.CostAllocationConfirmBy = user.UserID;
                    costAllocation.CostAllocationConfirmDate = AppUtils.ServerDateTime();
                    ECostAllocation.db.update(dbConn, costAllocation);
                }
            }

        }

    }

}
