using System;
using System.Data;
using System.Configuration;
using System.Collections;
using HROne.Lib;
using HROne.Lib.Entities;
using HROne.DataAccess;
//using perspectivemind.validation;
namespace HROne.Payroll
{
    /// <summary>
    /// Class for reading override amount.
    /// </summary>
    [DBClass("MPFCalculationOverride")]
    public class EMPFCalculationOverride : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EMPFCalculationOverride));

        protected int m_MPFCalculationOverrideID;
        [DBField("MPFCalculationOverrideID", true, true), TextSearch, Export(false)]
        public int MPFCalculationOverrideID
        {
            get { return m_MPFCalculationOverrideID; }
            set { m_MPFCalculationOverrideID = value; }
        }

        public DateTime m_MPFCalculationOverridePeriodFr;
        [DBField("MPFCalculationOverridePeriodFr"), TextSearch, Export(false), Required]
        public DateTime MPFCalculationOverridePeriodFr
        {
            get { return m_MPFCalculationOverridePeriodFr; }
            set { m_MPFCalculationOverridePeriodFr = value; }
        }

        protected DateTime m_MPFCalculationOverridePeriodTo;
        [DBField("MPFCalculationOverridePeriodTo"), TextSearch, Export(false), Required]
        public DateTime MPFCalculationOverridePeriodTo
        {
            get { return m_MPFCalculationOverridePeriodTo; }
            set { m_MPFCalculationOverridePeriodTo = value; }
        }

        protected double m_MPFCalculationOverrideMCRI;
        [DBField("MPFCalculationOverrideMCRI", "0.00"), TextSearch, Export(false), Required]
        public double MPFCalculationOverrideMCRI
        {
            get { return m_MPFCalculationOverrideMCRI; }
            set { m_MPFCalculationOverrideMCRI = value; }
        }

        protected double m_MPFCalculationOverrideMCER;
        [DBField("MPFCalculationOverrideMCER", "0.00"), TextSearch, Export(false), Required]
        public double MPFCalculationOverrideMCER
        {
            get { return m_MPFCalculationOverrideMCER; }
            set { m_MPFCalculationOverrideMCER = value; }
        }

        protected double m_MPFCalculationOverrideMCEE;
        [DBField("MPFCalculationOverrideMCEE", "0.00"), TextSearch, Export(false), Required]
        public double MPFCalculationOverrideMCEE
        {
            get { return m_MPFCalculationOverrideMCEE; }
            set { m_MPFCalculationOverrideMCEE = value; }
        }

        protected double m_MPFCalculationOverrideVCRI;
        [DBField("MPFCalculationOverrideVCRI", "0.00"), TextSearch, Export(false), Required]
        public double MPFCalculationOverrideVCRI
        {
            get { return m_MPFCalculationOverrideVCRI; }
            set { m_MPFCalculationOverrideVCRI = value; }
        }

        protected double m_MPFCalculationOverrideVCER;
        [DBField("MPFCalculationOverrideVCER", "0.00"), TextSearch, Export(false), Required]
        public double MPFCalculationOverrideVCER
        {
            get { return m_MPFCalculationOverrideVCER; }
            set { m_MPFCalculationOverrideVCER = value; }
        }

        protected double m_MPFCalculationOverrideVCEE;
        [DBField("MPFCalculationOverrideVCEE", "0.00"), TextSearch, Export(false), Required]
        public double MPFCalculationOverrideVCEE
        {
            get { return m_MPFCalculationOverrideVCEE; }
            set { m_MPFCalculationOverrideVCEE = value; }
        }
    }

    public abstract class MPFProcess
    {
        private const int AGE_MINIMUM = 18;
        private const int AGE_MAXIMUM = 65;
        private const int MPF_MEMBER_EXEMPTION_DAYS = 59;
        public enum MPFJoinType
        {
            NewJoin = 1,
            Existing = 2,
            //            Terminated = 3

        }

        public enum MPFCalculationMethod
        {
            MPF_AND_AVC = 1,
            AVCOnly = 2,
        }

        public static ArrayList MPFTrialRun(DatabaseConnection dbConn, int EmpID, EPayrollPeriod payrollPeriod, ArrayList trialrunPaymentRecords)
        {
            EEmpPersonalInfo oEmp = new EEmpPersonalInfo();
            oEmp.EmpID = EmpID;
            if (!EEmpPersonalInfo.db.select(dbConn, oEmp))
                return null;

            if (oEmp.MasterEmpID > 0 && oEmp.EmpIsCombineMPF)
                return MPFTrialRun(dbConn, oEmp.MasterEmpID, payrollPeriod, trialrunPaymentRecords);

            ArrayList mpfRecords = new ArrayList();

            DBFilter dbFilter = new DBFilter();
            dbFilter.add(new Match("EmpID", EmpID));

            dbFilter.add(new Match("EmpMPFEffFr", "<=", payrollPeriod.PayPeriodTo));
            dbFilter.add("EmpMPFEffFr", true);
            ArrayList oEmpMPFs = EEmpMPFPlan.db.select(dbConn, dbFilter);


            DBFilter payPeriodFilter = new DBFilter();
            payPeriodFilter.add(new Match("PayPeriodFr", "<=", payrollPeriod.PayPeriodTo));
            payPeriodFilter.add(new Match("PayPeriodTo", ">=", payrollPeriod.PayPeriodFr));

            DBFilter empPayrollFilter = new DBFilter();

            empPayrollFilter.add(new IN("PayPeriodID", "Select PayPeriodID from " + EPayrollPeriod.db.dbclass.tableName + " ", payPeriodFilter));
            empPayrollFilter.add(oEmp.GetAllRoleEmpIDTerms(dbConn, "EmpID", EEmpPersonalInfo.RoleFilterOptionEnum.MPF));
            IN inEmpPayrollFilter2 = new IN("EmpPayrollID", "Select EmpPayrollID from EmpPayroll", empPayrollFilter);

            DBFilter paymentRecordFilter = new DBFilter();
            paymentRecordFilter.add(inEmpPayrollFilter2);

            ArrayList totalPaymentRecords = EPaymentRecord.db.select(dbConn, paymentRecordFilter);

            totalPaymentRecords.AddRange(trialrunPaymentRecords);

            if (oEmpMPFs.Count > 0)
            {

                EEmpMPFPlan oEmpMPF = (EEmpMPFPlan)oEmpMPFs[0];

                DateTime mpfDateOfJoin = oEmpMPF.EmpMPFEffFr;

                DateTime dt1AgeMin = oEmp.EmpDateOfBirth.AddYears(AGE_MINIMUM);

                if (mpfDateOfJoin <= dt1AgeMin)
                    mpfDateOfJoin = dt1AgeMin;

                DateTime mpfJoin60Date = mpfDateOfJoin.AddDays(MPF_MEMBER_EXEMPTION_DAYS);

                //  Change the date to last employment date so that the MPF record can be generated
                EEmpTermination empTermination = EEmpTermination.GetObjectByEmpID(dbConn, EmpID);
                if (empTermination != null)
                {
                    if (empTermination.EmpTermLastDate <= payrollPeriod.PayPeriodTo && empTermination.EmpTermLastDate >= payrollPeriod.PayPeriodFr)
                        if (mpfJoin60Date > empTermination.EmpTermLastDate)
                            mpfJoin60Date = empTermination.EmpTermLastDate;
                }
                DateTime contributionDate = new DateTime(payrollPeriod.PayPeriodTo.Year, payrollPeriod.PayPeriodTo.Month, 1).AddMonths(1);
                DateTime firstContributionDate = new DateTime(mpfJoin60Date.Year, mpfJoin60Date.Month, 1).AddMonths(1);
                EPayrollPeriod nextPayrollPeriod = HROne.Payroll.PayrollProcess.GenerateDummyPayrollPeriod(dbConn, payrollPeriod.PayGroupID, payrollPeriod.PayPeriodTo.AddDays(1));
                DateTime nextContributionDate = new DateTime(nextPayrollPeriod.PayPeriodTo.Year, nextPayrollPeriod.PayPeriodTo.Month, 1).AddMonths(1);


                if (contributionDate.Equals(firstContributionDate) && (!contributionDate.Equals(nextContributionDate) || payrollPeriod.PayPeriodTo >= mpfJoin60Date))
                {
                    //  New Join Contribution
                    //  Check if contribution month is same as first contribution month and the period should be last period during this month by comparing contribution date of next period
                    ArrayList newJoinMPFRecords = CreateNewJoinMPFRecords(dbConn, EmpID, mpfDateOfJoin, mpfJoin60Date, MPFCalculationMethod.MPF_AND_AVC);
                    if (newJoinMPFRecords != null)
                        mpfRecords.AddRange(newJoinMPFRecords);
                    mpfRecords.Add(CreateMPFRecord(dbConn, EmpID, payrollPeriod, totalPaymentRecords, mpfDateOfJoin, MPFJoinType.NewJoin, MPFCalculationMethod.MPF_AND_AVC));
                }
                else if (firstContributionDate < contributionDate)
                {
                    mpfRecords.Add(CreateMPFRecord(dbConn, EmpID, payrollPeriod, totalPaymentRecords, mpfDateOfJoin, MPFJoinType.Existing, MPFCalculationMethod.MPF_AND_AVC));
                }
                else// if (contributionDate < firstContributionDate)
                {

                    DBFilter AVCPlanFilter = new DBFilter();
                    AVCPlanFilter.add(new Match("EmpID", EmpID));
                    AVCPlanFilter.add(new Match("EmpAVCEffFr", "<=", payrollPeriod.PayPeriodTo));
                    AVCPlanFilter.add("EmpAVCEffFr", true);
                    ArrayList oEmpAVCs = EEmpAVCPlan.db.select(dbConn, AVCPlanFilter);
                    if (oEmpAVCs.Count > 0)
                    {
                        EEmpAVCPlan oEmpAVC = (EEmpAVCPlan)oEmpAVCs[0];
                        EAVCPlan avcPlan = new EAVCPlan();
                        avcPlan.AVCPlanID = oEmpAVC.AVCPlanID;
                        if (EAVCPlan.db.select(dbConn, avcPlan))
                        {
                            if (avcPlan.AVCPlanJoinDateStart)
                            {
                                mpfRecords.Add(CreateMPFRecord(dbConn, EmpID, payrollPeriod, totalPaymentRecords, mpfDateOfJoin, MPFJoinType.Existing, MPFCalculationMethod.AVCOnly));
                            }
                        }
                    }
                }
                //else
                //{


                //    DBFilter AVCPlanFilter = new DBFilter();
                //    AVCPlanFilter.add(new Match("EmpID", EmpID));
                //    AVCPlanFilter.add(new Match("EmpAVCEffFr", "<=", payrollPeriod.PayPeriodTo));
                //    AVCPlanFilter.add("EmpAVCEffFr", true);
                //    ArrayList oEmpAVCs = EEmpAVCPlan.db.select(dbConn, AVCPlanFilter);
                //    if (oEmpAVCs.Count > 0)
                //    {
                //        EEmpAVCPlan oEmpAVC = (EEmpAVCPlan)oEmpAVCs[0];
                //        EAVCPlan avcPlan = new EAVCPlan();
                //        avcPlan.AVCPlanID = oEmpAVC.AVCPlanID;
                //        if (EAVCPlan.db.select(dbConn, avcPlan))
                //        {
                //            {
                //                mpfRecords.Add(CreateMPFRecord(EmpID, payrollPeriod, totalPaymentRecords, oEmpAVC.EmpAVCEffFr, MPFJoinType.Existing, MPFCalculationMethod.AVCOnly));
                //            }
                //        }
                //    }
                //}

            }
            else
            {
                DBFilter AVCPlanFilter = new DBFilter();
                AVCPlanFilter.add(new Match("EmpID", EmpID));
                AVCPlanFilter.add(new Match("EmpAVCEffFr", "<=", payrollPeriod.PayPeriodTo));
                AVCPlanFilter.add("EmpAVCEffFr", true);
                ArrayList oEmpAVCs = EEmpAVCPlan.db.select(dbConn, AVCPlanFilter);
                if (oEmpAVCs.Count > 0)
                {
                    EEmpAVCPlan oEmpAVC = (EEmpAVCPlan)oEmpAVCs[0];
                    EAVCPlan avcPlan = new EAVCPlan();
                    avcPlan.AVCPlanID = oEmpAVC.AVCPlanID;
                    if (EAVCPlan.db.select(dbConn, avcPlan))
                        if (!avcPlan.AVCPlanJoinDateStart)
                        {

                            DateTime mpfDateOfJoin = oEmpAVC.EmpAVCEffFr;


                            DateTime mpfJoin60Date = mpfDateOfJoin.AddDays(MPF_MEMBER_EXEMPTION_DAYS);

                            DateTime contributionDate = new DateTime(payrollPeriod.PayPeriodTo.Year, payrollPeriod.PayPeriodTo.Month, 1).AddMonths(1);
                            DateTime firstContributionDate = new DateTime(mpfJoin60Date.Year, mpfJoin60Date.Month, 1).AddMonths(1);
                            EPayrollPeriod nextPayrollPeriod = HROne.Payroll.PayrollProcess.GenerateDummyPayrollPeriod(dbConn, payrollPeriod.PayGroupID, payrollPeriod.PayPeriodTo.AddDays(1));
                            DateTime nextContributionDate = new DateTime(nextPayrollPeriod.PayPeriodTo.Year, nextPayrollPeriod.PayPeriodTo.Month, 1).AddMonths(1);

                            if (contributionDate.Equals(firstContributionDate) && (!contributionDate.Equals(nextContributionDate) || payrollPeriod.PayPeriodTo >= mpfJoin60Date))
                            {
                                ArrayList newJoinMPFRecords = CreateNewJoinMPFRecords(dbConn, EmpID, mpfDateOfJoin, mpfJoin60Date, MPFCalculationMethod.AVCOnly);
                                if (newJoinMPFRecords != null)
                                    mpfRecords.AddRange(newJoinMPFRecords);
                                mpfRecords.Add(CreateMPFRecord(dbConn, EmpID, payrollPeriod, totalPaymentRecords, oEmpAVC.EmpAVCEffFr, MPFJoinType.NewJoin, MPFCalculationMethod.AVCOnly));
                            }
                            else if (firstContributionDate < contributionDate)
                                mpfRecords.Add(CreateMPFRecord(dbConn, EmpID, payrollPeriod, totalPaymentRecords, oEmpAVC.EmpAVCEffFr, MPFJoinType.Existing, MPFCalculationMethod.AVCOnly));
                            //else if (payrollPeriod.PayPeriodTo < mpfJoin60Date)
                            //{
                            //    // do nothing
                            //    //mpfRecords.Add(CreateMPFRecord(EmpID, payrollPeriod, totalPaymentRecords, oEmpAVC.EmpAVCEffFr, MPFJoinType.Existing, MPFCalculationMethod.AVCOnly));
                            //}
                            //else
                            //    mpfRecords.Add(CreateMPFRecord(EmpID, payrollPeriod, totalPaymentRecords, oEmpAVC.EmpAVCEffFr, MPFJoinType.Existing, MPFCalculationMethod.AVCOnly));
                        }
                        else
                            mpfRecords.Add(CreateMPFRecord(dbConn, EmpID, payrollPeriod, totalPaymentRecords, oEmpAVC.EmpAVCEffFr, MPFJoinType.Existing, MPFCalculationMethod.AVCOnly));

                }
            }

            return mpfRecords;
        }

        private static ArrayList CreateNewJoinMPFRecords(DatabaseConnection dbConn, int EmpID, DateTime MPFJoinDate, DateTime AsOfDate, MPFCalculationMethod mpfCalculationMethod)
        {

            ArrayList newJoinMPFRecords = new ArrayList();

            DBFilter empPayrollFilter = new DBFilter();
            empPayrollFilter.add(new Match("EmpID", EmpID));
            IN inEmpPayrollFilter = new IN("PayPeriodID", "SELECT DISTINCT PayPeriodID FROM EmpPayroll ", empPayrollFilter);

            DBFilter payPeriodFilter = new DBFilter();
            payPeriodFilter.add(inEmpPayrollFilter);
            payPeriodFilter.add(new Match("PayPeriodTo", "<", AsOfDate));
            payPeriodFilter.add(new Match("PayPeriodTo", ">=", MPFJoinDate));
            payPeriodFilter.add("PayPeriodFr", true);
            ArrayList payPeriods = EPayrollPeriod.db.select(dbConn, payPeriodFilter);

            foreach (EPayrollPeriod payrollPeriod in payPeriods)
            {

                DBFilter payPeriodFilter2 = new DBFilter();
                payPeriodFilter2.add(new Match("PayPeriodID", payrollPeriod.PayPeriodID));
                payPeriodFilter2.add(new Match("EmpID", EmpID));
                IN inEmpPayrollFilter2 = new IN("EmpPayrollID", "SELECT EmpPayrollID FROM EmpPayroll", payPeriodFilter2);

                DBFilter paymentRecordFilter = new DBFilter();
                paymentRecordFilter.add(inEmpPayrollFilter2);

                ArrayList paymentRecords = EPaymentRecord.db.select(dbConn, paymentRecordFilter);

                newJoinMPFRecords.Add(CreateMPFRecord(dbConn, EmpID, payrollPeriod, paymentRecords, MPFJoinDate, MPFJoinType.NewJoin, mpfCalculationMethod));

            }

            if (newJoinMPFRecords.Count == 0)
                return null;
            return newJoinMPFRecords;
        }

        private static double CalculateProrataFactor(DatabaseConnection dbConn, int EmpID, EPayrollPeriod payrollPeriod)
        {
            EEmpPersonalInfo oEmp = new EEmpPersonalInfo();
            oEmp.EmpID = EmpID;
            EEmpPersonalInfo.db.select(dbConn, oEmp);

            DateTime dtMPFPlanStartDate;

            DBFilter empFirstMPFFilter = new DBFilter();
            empFirstMPFFilter.add(new Match("EmpID", EmpID));
            empFirstMPFFilter.add("EmpMPFEffFr", true);
            ArrayList empMPFPlanList = EEmpMPFPlan.db.select(dbConn, empFirstMPFFilter);
            if (empMPFPlanList.Count > 0)
                dtMPFPlanStartDate = ((EEmpMPFPlan)empMPFPlanList[0]).EmpMPFEffFr;
            else
                dtMPFPlanStartDate = new DateTime();

            DateTime dt1AgeMin = oEmp.EmpDateOfBirth.AddYears(AGE_MINIMUM);
            DateTime dt1AgeMax = oEmp.EmpDateOfBirth.AddYears(AGE_MAXIMUM);

            EEmpTermination empTermination = EEmpTermination.GetObjectByEmpID(dbConn, EmpID);
            bool blnTerminated;
            if (empTermination != null)
            {
                if (empTermination.EmpTermLastDate <= payrollPeriod.PayPeriodTo && empTermination.EmpTermLastDate >= payrollPeriod.PayPeriodFr)
                    blnTerminated = true;
                else
                    blnTerminated = false;
            }
            else
                blnTerminated = false;

            DateTime dtActualJoinDate;
            if (dtMPFPlanStartDate <= dt1AgeMin)
                dtActualJoinDate = dt1AgeMin;
            else
                dtActualJoinDate = dtMPFPlanStartDate;

            double prorataFactor = 1;
            if (oEmp.EmpDateOfJoin < dtActualJoinDate)
            {
                if (dtActualJoinDate <= payrollPeriod.PayPeriodTo && dtActualJoinDate >= payrollPeriod.PayPeriodFr)
                {
                    if (blnTerminated)
                        prorataFactor = (double)(empTermination.EmpTermLastDate.Subtract(dtActualJoinDate).Days + 1) / (empTermination.EmpTermLastDate.Subtract(payrollPeriod.PayPeriodFr > oEmp.EmpDateOfJoin ? payrollPeriod.PayPeriodFr : oEmp.EmpDateOfJoin).Days + 1);
                    else
                        prorataFactor = (double)(payrollPeriod.PayPeriodTo.Subtract(dtActualJoinDate).Days + 1) / (payrollPeriod.PayPeriodTo.Subtract(payrollPeriod.PayPeriodFr > oEmp.EmpDateOfJoin ? payrollPeriod.PayPeriodFr : oEmp.EmpDateOfJoin).Days + 1);
                }
                else
                    if (dtActualJoinDate >= payrollPeriod.PayPeriodTo)
                        prorataFactor = 0;
            }

            if (dt1AgeMax <= payrollPeriod.PayPeriodTo && dt1AgeMax >= payrollPeriod.PayPeriodFr)
            {
                if (blnTerminated)
                {
                    if (empTermination.EmpTermLastDate < dt1AgeMax)
                        prorataFactor = 1;
                    else
                        prorataFactor = 1.0 - (double)(empTermination.EmpTermLastDate.Subtract(dt1AgeMax).Days + 1) / (empTermination.EmpTermLastDate.Subtract(payrollPeriod.PayPeriodFr > oEmp.EmpDateOfJoin ? payrollPeriod.PayPeriodFr : oEmp.EmpDateOfJoin).Days + 1);
                }
                else
                    prorataFactor = 1.0 - (double)(payrollPeriod.PayPeriodTo.Subtract(dt1AgeMax).Days + 1) / (payrollPeriod.PayPeriodTo.Subtract(payrollPeriod.PayPeriodFr > oEmp.EmpDateOfJoin ? payrollPeriod.PayPeriodFr : oEmp.EmpDateOfJoin).Days + 1);
            }
            if (dt1AgeMax <= payrollPeriod.PayPeriodFr)
                prorataFactor = 0;

            return prorataFactor;
        }

        private static EMPFRecord CreateMPFRecord(DatabaseConnection dbConn, int EmpID, EPayrollPeriod payrollPeriod, ArrayList paymentRecords, DateTime MPFJoinDate, MPFJoinType mpfJoinType, MPFCalculationMethod mpfCalMethod)
        {
            double RIProrateFactor = CalculateProrataFactor(dbConn, EmpID, payrollPeriod);

            // Get Actual MPF Join Date to resolve transfer company
            DateTime ActualMPFJoinDate = GetActualMPFJoinDate(dbConn, EmpID, MPFJoinDate);
            EMPFRecord mpfRecord = new EMPFRecord();
            switch (mpfJoinType)
            {
                case MPFJoinType.NewJoin:
                    mpfRecord.MPFRecType = "N";
                    break;
                case MPFJoinType.Existing:
                    mpfRecord.MPFRecType = "E";
                    break;
                //case MPFJoinType.Terminated:
                //    mpfRecord.MPFRecType = "T";
                //break;
            }
            mpfRecord.MPFRecCalMCRI = 0;
            mpfRecord.MPFRecCalVCRI = 0;
            mpfRecord.MPFPlanID = GetMPFPlainID(dbConn, EmpID, payrollPeriod.PayPeriodTo);
            mpfRecord.AVCPlanID = GetAVCPlainID(dbConn, EmpID, payrollPeriod.PayPeriodTo);

            EEmpPersonalInfo oEmp = new EEmpPersonalInfo();
            oEmp.EmpID = EmpID;
            EEmpPersonalInfo.db.select(dbConn, oEmp);

            //DateTime dt1Age18 = oEmp.EmpDateOfBirth.AddYears(18);
            //DateTime dt1Age65 = oEmp.EmpDateOfBirth.AddYears(65);

            EEmpTermination empTermination = EEmpTermination.GetObjectByEmpID(dbConn, EmpID);

            mpfRecord.MPFRecPeriodFr = payrollPeriod.PayPeriodFr;
            mpfRecord.MPFRecPeriodTo = payrollPeriod.PayPeriodTo;

            if (mpfRecord.MPFRecPeriodFr > oEmp.EmpDateOfJoin && mpfRecord.MPFRecPeriodTo < oEmp.EmpDateOfJoin)
                mpfRecord.MPFRecPeriodFr = oEmp.EmpDateOfJoin;
            //if (mpfRecord.MPFRecPeriodFr > dt1Age18 && mpfRecord.MPFRecPeriodTo < dt1Age18)
            //    mpfRecord.MPFRecPeriodFr = dt1Age18;
            if (empTermination != null)
                if (mpfRecord.MPFRecPeriodTo < empTermination.EmpTermLastDate && mpfRecord.MPFRecPeriodFr >= empTermination.EmpTermLastDate)
                    mpfRecord.MPFRecPeriodTo = empTermination.EmpTermLastDate;

            //if (mpfRecord.MPFRecPeriodTo < dt1Age65 && mpfRecord.MPFRecPeriodFr > dt1Age65)
            //    mpfRecord.MPFRecPeriodTo = dt1Age65;


            DBFilter AVCPlanPaymentCeilingFilter = new DBFilter();
            AVCPlanPaymentCeilingFilter.add(new Match("AVCPlanID", mpfRecord.AVCPlanID));
            ArrayList ceilingCreditList = EAVCPlanPaymentCeiling.db.select(dbConn, AVCPlanPaymentCeilingFilter);

            double dblMCRI = 0, dblVCRI = 0;
            double dblMCRIBeforeConsiderVC = 0;
            double dblNotDeductVCRI = 0;

            foreach (EPaymentRecord paymentRecord in paymentRecords)
            {
                EPaymentCode paymentCode = new EPaymentCode();
                paymentCode.PaymentCodeID = paymentRecord.PaymentCodeID;
                EPaymentCode.db.select(dbConn, paymentCode);
                if (paymentCode.PaymentCodeIsMPF)
                {
                    dblMCRI += paymentRecord.PayRecActAmount;
                    DBFilter paymentConsiderFilter = new DBFilter();
                    paymentConsiderFilter.add(new Match("AVCPlanID", mpfRecord.AVCPlanID));
                    paymentConsiderFilter.add(new Match("PaymentCodeID", paymentRecord.PaymentCodeID));
                    ArrayList paymentConsiderList = EAVCPlanPaymentConsider.db.select(dbConn, paymentConsiderFilter);
                    if (paymentConsiderList.Count > 0)
                    {
                        EAVCPlanPaymentConsider paymentConsider = (EAVCPlanPaymentConsider)paymentConsiderList[0];
                        if (!paymentConsider.AVCPlanPaymentConsiderAfterMPF)
                            dblMCRIBeforeConsiderVC += paymentRecord.PayRecActAmount;
                    }
                    else
                        dblMCRIBeforeConsiderVC += paymentRecord.PayRecActAmount;

                }
                if (paymentCode.PaymentCodeIsTopUp)
                {
                    double dblActualContributionAmount = paymentRecord.PayRecActAmount;
                    //bool paymentHasCeiling = false;
                    foreach (EAVCPlanPaymentCeiling ceilingCredit in ceilingCreditList)
                    {
                        if (ceilingCredit.PaymentCodeID.Equals(paymentRecord.PaymentCodeID))
                        {
                            //paymentHasCeiling = true;

                            if (ceilingCredit.AVCPlanPaymentCeilingAmount < paymentRecord.PayRecActAmount)
                            {
                                dblActualContributionAmount = ceilingCredit.AVCPlanPaymentCeilingAmount;
                                ceilingCredit.AVCPlanPaymentCeilingAmount = 0;
                            }
                            else
                            {
                                ceilingCredit.AVCPlanPaymentCeilingAmount -= paymentRecord.PayRecActAmount;
                            }
                            break;
                        }
                    }

                    dblVCRI += dblActualContributionAmount;

                }
                else
                {
                    if (paymentCode.PaymentCodeIsMPF && paymentCode.PaymentCodeNotRemoveContributionFromTopUp)
                        dblNotDeductVCRI += paymentRecord.PayRecActAmount;
                }

            }


            EAVCPlan avcPlan = new EAVCPlan();
            avcPlan.AVCPlanID = mpfRecord.AVCPlanID;
            if (EAVCPlan.db.select(dbConn, avcPlan))
            {
                if (avcPlan.AVCPlanNotRemoveContributionFromTopUp)
                {
                    EMPFParameter mpfParameter = getLatestMPFParameter(dbConn, payrollPeriod.PayPeriodFr);
                    if (dblMCRIBeforeConsiderVC < mpfParameter.MPFParamMaxMonthly)
                        dblVCRI += dblNotDeductVCRI;
                    else if (dblMCRIBeforeConsiderVC - dblNotDeductVCRI < mpfParameter.MPFParamMaxMonthly)
                    {
                        dblVCRI += dblNotDeductVCRI - (dblMCRIBeforeConsiderVC - mpfParameter.MPFParamMaxMonthly);

                    }
                }
            }
            else
                avcPlan = null;

            if (dblMCRI < 0)
                dblMCRI = 0;
            if (dblVCRI < 0)
                dblVCRI = 0;
            mpfRecord.MPFRecCalMCRI = dblMCRI;
            mpfRecord.MPFRecCalVCRI = dblVCRI;
            mpfRecord.MPFRecActMCRI = mpfRecord.MPFRecCalMCRI;
            mpfRecord.MPFRecActVCRI = mpfRecord.MPFRecCalVCRI;

            if (mpfRecord.MPFPlanID > 0)
            {

                DBFilter oldMPFRecordFilter = new DBFilter();

                DBFilter empIDFilter = new DBFilter();
                empIDFilter.add(oEmp.GetAllRoleEmpIDTerms(dbConn, "EmpID", EEmpPersonalInfo.RoleFilterOptionEnum.MPF));

                oldMPFRecordFilter.add(new IN("EmpPayrollID", "Select EmpPayrollID from EMPPayroll ", empIDFilter));
                oldMPFRecordFilter.add(new Match("MPFRecPeriodFr", "<=", mpfRecord.MPFRecPeriodTo));
                oldMPFRecordFilter.add(new Match("MPFRecPeriodTo", ">=", mpfRecord.MPFRecPeriodFr));
                ArrayList oldMPFRecords = EMPFRecord.db.select(dbConn, oldMPFRecordFilter);

                EMPFRecord oldTotalMPFRecord = new EMPFRecord();
                EMPFRecord newTotalMPFRecord = new EMPFRecord();

                foreach (EMPFRecord oldMPFRecord in oldMPFRecords)
                {
                    oldTotalMPFRecord.MPFRecActMCRI += oldMPFRecord.MPFRecActMCRI;
                    oldTotalMPFRecord.MPFRecActMCER += oldMPFRecord.MPFRecActMCER;
                    oldTotalMPFRecord.MPFRecActMCEE += oldMPFRecord.MPFRecActMCEE;

                    oldTotalMPFRecord.MPFRecActVCRI += oldMPFRecord.MPFRecActVCRI;
                    oldTotalMPFRecord.MPFRecActVCER += oldMPFRecord.MPFRecActVCER;
                    oldTotalMPFRecord.MPFRecActVCEE += oldMPFRecord.MPFRecActVCEE;
                }

                //newTotalMPFRecord.MPFRecCalMCRI = oldTotalMPFRecord.MPFRecActMCRI + mpfRecord.MPFRecCalMCRI;
                //newTotalMPFRecord.MPFRecCalVCRI = oldTotalMPFRecord.MPFRecActVCRI + mpfRecord.MPFRecCalVCRI;

                newTotalMPFRecord.MPFRecCalMCRI = mpfRecord.MPFRecCalMCRI;
                newTotalMPFRecord.MPFRecCalVCRI = mpfRecord.MPFRecCalVCRI;

                if (mpfCalMethod != MPFCalculationMethod.AVCOnly)
                {
                    newTotalMPFRecord.MPFRecCalMCER = CalculateMCERAmount(dbConn, newTotalMPFRecord.MPFRecCalMCRI * RIProrateFactor, payrollPeriod);
                    newTotalMPFRecord.MPFRecCalMCEE = CalculateMCEEAmount(dbConn, newTotalMPFRecord.MPFRecCalMCRI * RIProrateFactor, payrollPeriod, ActualMPFJoinDate);
                }

                //  Check if the employee is terminated before 60 days
                if (empTermination != null)
                    if (empTermination.EmpTermLastDate < MPFJoinDate.AddDays(MPF_MEMBER_EXEMPTION_DAYS))
                    {
                        newTotalMPFRecord.MPFRecCalMCER = 0;
                        newTotalMPFRecord.MPFRecCalMCEE = 0;
                    }

                newTotalMPFRecord.MPFRecCalVCER = CalculateVCERAmount(dbConn, oEmp, mpfRecord.AVCPlanID, newTotalMPFRecord.MPFRecCalMCRI * RIProrateFactor, newTotalMPFRecord.MPFRecCalMCER, newTotalMPFRecord.MPFRecCalVCRI, payrollPeriod);
                newTotalMPFRecord.MPFRecCalVCEE = CalculateVCEEAmount(dbConn, oEmp, mpfRecord.AVCPlanID, newTotalMPFRecord.MPFRecCalMCRI * RIProrateFactor, newTotalMPFRecord.MPFRecCalMCEE, newTotalMPFRecord.MPFRecCalVCRI, payrollPeriod, ActualMPFJoinDate);

                if (avcPlan != null)
                {
                    //  Check if the employee is terminated before 60 days
                    if (empTermination != null)
                        if (empTermination.EmpTermLastDate < MPFJoinDate.AddDays(MPF_MEMBER_EXEMPTION_DAYS) && !avcPlan.AVCPlanJoinDateStart)
                        {
                            newTotalMPFRecord.MPFRecCalVCER = 0;
                            newTotalMPFRecord.MPFRecCalVCEE = 0;
                        }
                }
                    EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                    empInfo.EmpID = EmpID;
                    if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                    {
                        DBFilter mpfOverrideRecordFilter = new DBFilter();
                        mpfOverrideRecordFilter.add(new Match("EmpNo", empInfo.EmpNo));
                        mpfOverrideRecordFilter.add(new Match("MPFCalculationOverridePeriodFr", ">=", payrollPeriod.PayPeriodFr));
                        mpfOverrideRecordFilter.add(new Match("MPFCalculationOverridePeriodTo", "<=", payrollPeriod.PayPeriodTo));
                        //  Try to override MPF calculation result for data migration
                        try
                        {
                            ArrayList mpfOverrideRecordList = EMPFCalculationOverride.db.select(dbConn, mpfOverrideRecordFilter);
                            if (mpfOverrideRecordList.Count > 0)
                            {
                                EMPFCalculationOverride mpfOverride = (EMPFCalculationOverride)mpfOverrideRecordList[0];
                                if (mpfOverride.MPFCalculationOverrideMCRI > 0)
                                    newTotalMPFRecord.MPFRecCalMCRI = mpfOverride.MPFCalculationOverrideMCRI;
                                newTotalMPFRecord.MPFRecCalMCEE = mpfOverride.MPFCalculationOverrideMCEE;
                                newTotalMPFRecord.MPFRecCalMCER = mpfOverride.MPFCalculationOverrideMCER;
                                if (mpfOverride.MPFCalculationOverrideVCRI > 0)
                                    newTotalMPFRecord.MPFRecCalVCRI = mpfOverride.MPFCalculationOverrideVCRI;
                                newTotalMPFRecord.MPFRecCalVCEE = mpfOverride.MPFCalculationOverrideVCEE;
                                newTotalMPFRecord.MPFRecCalVCER = mpfOverride.MPFCalculationOverrideVCER;
                            }
                        }
                        catch (Exception ex)
                        {
                            //  error occurs if override table does not exists.
                            System.Diagnostics.Debug.WriteLine(ex.Message);
                        }
                    }

                EMPFPlan mpfPlan = new EMPFPlan();
                mpfPlan.MPFPlanID = mpfRecord.MPFPlanID;
                if (EMPFPlan.db.select(dbConn, mpfPlan))
                {
                    if (string.IsNullOrEmpty(mpfPlan.MPFPlanEmployerRoundingRule))
                    {
                        mpfPlan.MPFPlanEmployerRoundingRule = Values.ROUNDING_RULE_ROUND_TO;
                        mpfPlan.MPFPlanEmployerDecimalPlace = 2;
                    }
                    if (mpfPlan.MPFPlanEmployerRoundingRule.Equals(Values.ROUNDING_RULE_ROUND_TO))
                        newTotalMPFRecord.MPFRecActMCER = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(newTotalMPFRecord.MPFRecCalMCER, mpfPlan.MPFPlanEmployerDecimalPlace, ExchangeCurrency.DefaultCurrencyDecimalPlaces());
                    else if (mpfPlan.MPFPlanEmployerRoundingRule.Equals(Values.ROUNDING_RULE_ROUND_UP))
                        newTotalMPFRecord.MPFRecActMCER = HROne.CommonLib.GenericRoundingFunctions.RoundingUp(newTotalMPFRecord.MPFRecCalMCER, mpfPlan.MPFPlanEmployerDecimalPlace, ExchangeCurrency.DefaultCurrencyDecimalPlaces());
                    else if (mpfPlan.MPFPlanEmployerRoundingRule.Equals(Values.ROUNDING_RULE_ROUND_DOWN))
                        newTotalMPFRecord.MPFRecActMCER = HROne.CommonLib.GenericRoundingFunctions.RoundingDown(newTotalMPFRecord.MPFRecCalMCER, mpfPlan.MPFPlanEmployerDecimalPlace, ExchangeCurrency.DefaultCurrencyDecimalPlaces());

                    if (string.IsNullOrEmpty(mpfPlan.MPFPlanEmployeeRoundingRule))
                    {
                        mpfPlan.MPFPlanEmployeeRoundingRule = Values.ROUNDING_RULE_ROUND_TO;
                        mpfPlan.MPFPlanEmployeeDecimalPlace = 2;
                    }
                    if (mpfPlan.MPFPlanEmployeeRoundingRule.Equals(Values.ROUNDING_RULE_ROUND_TO))
                        newTotalMPFRecord.MPFRecActMCEE = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(newTotalMPFRecord.MPFRecCalMCEE, mpfPlan.MPFPlanEmployeeDecimalPlace, ExchangeCurrency.DefaultCurrencyDecimalPlaces());
                    else if (mpfPlan.MPFPlanEmployeeRoundingRule.Equals(Values.ROUNDING_RULE_ROUND_UP))
                        newTotalMPFRecord.MPFRecActMCEE = HROne.CommonLib.GenericRoundingFunctions.RoundingUp(newTotalMPFRecord.MPFRecCalMCEE, mpfPlan.MPFPlanEmployeeDecimalPlace, ExchangeCurrency.DefaultCurrencyDecimalPlaces());
                    else if (mpfPlan.MPFPlanEmployeeRoundingRule.Equals(Values.ROUNDING_RULE_ROUND_DOWN))
                        newTotalMPFRecord.MPFRecActMCEE = HROne.CommonLib.GenericRoundingFunctions.RoundingDown(newTotalMPFRecord.MPFRecCalMCEE, mpfPlan.MPFPlanEmployeeDecimalPlace, ExchangeCurrency.DefaultCurrencyDecimalPlaces());
                }


                if (avcPlan != null)
                {

                    if (string.IsNullOrEmpty(avcPlan.AVCPlanEmployerRoundingRule))
                    {
                        avcPlan.AVCPlanEmployerRoundingRule = Values.ROUNDING_RULE_ROUND_TO;
                        avcPlan.AVCPlanEmployerDecimalPlace = 2;
                    }

                    if (avcPlan.AVCPlanEmployerRoundingRule.Equals(Values.ROUNDING_RULE_ROUND_TO))
                        newTotalMPFRecord.MPFRecActVCER = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(newTotalMPFRecord.MPFRecCalVCER, avcPlan.AVCPlanEmployerDecimalPlace, ExchangeCurrency.DefaultCurrencyDecimalPlaces());
                    else if (avcPlan.AVCPlanEmployerRoundingRule.Equals(Values.ROUNDING_RULE_ROUND_UP))
                        newTotalMPFRecord.MPFRecActVCER = HROne.CommonLib.GenericRoundingFunctions.RoundingUp(newTotalMPFRecord.MPFRecCalVCER, avcPlan.AVCPlanEmployerDecimalPlace, ExchangeCurrency.DefaultCurrencyDecimalPlaces());
                    else if (avcPlan.AVCPlanEmployerRoundingRule.Equals(Values.ROUNDING_RULE_ROUND_DOWN))
                        newTotalMPFRecord.MPFRecActVCER = HROne.CommonLib.GenericRoundingFunctions.RoundingDown(newTotalMPFRecord.MPFRecCalVCER, avcPlan.AVCPlanEmployerDecimalPlace, ExchangeCurrency.DefaultCurrencyDecimalPlaces());

                    if (string.IsNullOrEmpty(avcPlan.AVCPlanEmployeeRoundingRule))
                    {
                        avcPlan.AVCPlanEmployeeRoundingRule = Values.ROUNDING_RULE_ROUND_TO;
                        avcPlan.AVCPlanEmployeeDecimalPlace = 2;
                    }
                    if (avcPlan.AVCPlanEmployeeRoundingRule.Equals(Values.ROUNDING_RULE_ROUND_TO))
                        newTotalMPFRecord.MPFRecActVCEE = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(newTotalMPFRecord.MPFRecCalVCEE, avcPlan.AVCPlanEmployeeDecimalPlace, ExchangeCurrency.DefaultCurrencyDecimalPlaces());
                    else if (avcPlan.AVCPlanEmployeeRoundingRule.Equals(Values.ROUNDING_RULE_ROUND_UP))
                        newTotalMPFRecord.MPFRecActVCEE = HROne.CommonLib.GenericRoundingFunctions.RoundingUp(newTotalMPFRecord.MPFRecCalVCEE, avcPlan.AVCPlanEmployeeDecimalPlace, ExchangeCurrency.DefaultCurrencyDecimalPlaces());
                    else if (avcPlan.AVCPlanEmployeeRoundingRule.Equals(Values.ROUNDING_RULE_ROUND_DOWN))
                        newTotalMPFRecord.MPFRecActVCEE = HROne.CommonLib.GenericRoundingFunctions.RoundingDown(newTotalMPFRecord.MPFRecCalVCEE, avcPlan.AVCPlanEmployeeDecimalPlace, ExchangeCurrency.DefaultCurrencyDecimalPlaces());
                }
                mpfRecord.MPFRecCalMCER = newTotalMPFRecord.MPFRecCalMCER - oldTotalMPFRecord.MPFRecActMCER;
                mpfRecord.MPFRecCalMCEE = newTotalMPFRecord.MPFRecCalMCEE - oldTotalMPFRecord.MPFRecActMCEE;

                mpfRecord.MPFRecCalVCER = newTotalMPFRecord.MPFRecCalVCER - oldTotalMPFRecord.MPFRecActVCER;
                mpfRecord.MPFRecCalVCEE = newTotalMPFRecord.MPFRecCalVCEE - oldTotalMPFRecord.MPFRecActVCEE;

                mpfRecord.MPFRecCalMCRI = Math.Round(newTotalMPFRecord.MPFRecCalMCRI - oldTotalMPFRecord.MPFRecActMCRI, 2, MidpointRounding.AwayFromZero);
                mpfRecord.MPFRecCalVCRI = Math.Round(newTotalMPFRecord.MPFRecCalVCRI - oldTotalMPFRecord.MPFRecActVCRI, 2, MidpointRounding.AwayFromZero);

                mpfRecord.MPFRecActMCER = Math.Round(newTotalMPFRecord.MPFRecActMCER - oldTotalMPFRecord.MPFRecActMCER, 2, MidpointRounding.AwayFromZero);
                mpfRecord.MPFRecActMCEE = Math.Round(newTotalMPFRecord.MPFRecActMCEE - oldTotalMPFRecord.MPFRecActMCEE, 2, MidpointRounding.AwayFromZero);
                mpfRecord.MPFRecActVCER = Math.Round(newTotalMPFRecord.MPFRecActVCER - oldTotalMPFRecord.MPFRecActVCER, 2, MidpointRounding.AwayFromZero);
                mpfRecord.MPFRecActVCEE = Math.Round(newTotalMPFRecord.MPFRecActVCEE - oldTotalMPFRecord.MPFRecActVCEE, 2, MidpointRounding.AwayFromZero);
                mpfRecord.MPFRecActMCRI = Math.Round(mpfRecord.MPFRecCalMCRI, 2, MidpointRounding.AwayFromZero);
                mpfRecord.MPFRecActVCRI = Math.Round(mpfRecord.MPFRecCalVCRI, 2, MidpointRounding.AwayFromZero);
            }

            return mpfRecord;
        }

        /**
         *              Function to resolve transfer company case
        */
        private static DateTime GetActualMPFJoinDate(DatabaseConnection dbConn, int EmpID, DateTime MPFJoinDate)
        {

            EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
            empInfo.EmpID = EmpID;
            EEmpPersonalInfo.db.select(dbConn, empInfo);

            //  inter-group transfer special condition
            if (MPFJoinDate.Equals(empInfo.EmpDateOfJoin) && empInfo.EmpDateOfJoin > empInfo.EmpServiceDate)
            {
                //  Skip apply inter-group transfer rule for 1st contribution since service start date
                //  use (EmpDateOfJoin - EmpServiceDate) >=60? to determine whether it has first contribuion since last position/job
                //  known issue: 1. fail to trigger if transfer more than 1 time
                //               2. transfer with VISA staff
                if (((TimeSpan)empInfo.EmpDateOfJoin.Subtract(empInfo.EmpServiceDate)).TotalDays >= 60)
                {
                    DateTime dt1AgeMin = empInfo.EmpDateOfBirth.AddYears(AGE_MINIMUM);
                    return dt1AgeMin > empInfo.EmpServiceDate ? dt1AgeMin : empInfo.EmpServiceDate;
                }
            }
            return MPFJoinDate;
        }

        private static double CalculateMCEEAmount(DatabaseConnection dbConn, double TotalRI, EPayrollPeriod payrollPeriod, DateTime MPFJoinDate)
        {
            if (MPFJoinDate != null)
            {
                DateTime mpfJoin31thDate = MPFJoinDate.AddDays(30);
                if (payrollPeriod.PayPeriodTo < mpfJoin31thDate)
                {
                    TotalRI = 0;
                }
                if (payrollPeriod.PayPeriodFr < mpfJoin31thDate && mpfJoin31thDate <= payrollPeriod.PayPeriodTo)
                {
                    // According to MPF Ordinance effective at Feb 2003, no prorata will be calculated
                    TotalRI = 0;
                }
            }
            if (TotalRI < 0)
                return 0;
            EMPFParameter mpfParameter = getLatestMPFParameter(dbConn, payrollPeriod.PayPeriodFr);
            if (mpfParameter != null)
            {
                double dblMaxTotal;
                double dblMinTotal;
                EPayrollGroup payrollGroup = EPayrollGroup.GetPayrollGroup(dbConn, payrollPeriod.PayGroupID);

                if (payrollGroup.PayGroupFreq == "M")
                {
                    dblMaxTotal = mpfParameter.MPFParamMaxMonthly;
                    dblMinTotal = mpfParameter.MPFParamMinMonthly;
                }
                else
                {
                    int intNumberOfDate = payrollPeriod.PayPeriodTo.Subtract(payrollPeriod.PayPeriodFr).Days + 1;
                    dblMaxTotal = mpfParameter.MPFParamMaxDaily * intNumberOfDate;
                    dblMinTotal = mpfParameter.MPFParamMinDaily * intNumberOfDate;
                }

                TotalRI = TotalRI > dblMaxTotal ? dblMaxTotal : TotalRI;
                TotalRI = TotalRI < dblMinTotal ? 0 : TotalRI;

                return HROne.CommonLib.GenericRoundingFunctions.RoundingTo(TotalRI * mpfParameter.MPFParamEEPercent / 100, 4, 4);
            }
            else
                return 0;

        }

        private static double CalculateMCERAmount(DatabaseConnection dbConn, double TotalRI, EPayrollPeriod payrollPeriod)
        {
            if (TotalRI < 0)
                return 0;

            EMPFParameter mpfParameter = getLatestMPFParameter(dbConn, payrollPeriod.PayPeriodFr);
            if (mpfParameter != null)
            {
                double dblMaxTotal;
                EPayrollGroup payrollGroup = EPayrollGroup.GetPayrollGroup(dbConn, payrollPeriod.PayGroupID);

                if (payrollGroup.PayGroupFreq == "M")
                {
                    dblMaxTotal = mpfParameter.MPFParamMaxMonthly;
                }
                else
                {
                    int intNumberOfDate = payrollPeriod.PayPeriodTo.Subtract(payrollPeriod.PayPeriodFr).Days + 1;
                    dblMaxTotal = mpfParameter.MPFParamMaxDaily * intNumberOfDate;
                }

                TotalRI = TotalRI > dblMaxTotal ? dblMaxTotal : TotalRI;
                return HROne.CommonLib.GenericRoundingFunctions.RoundingTo(TotalRI * mpfParameter.MPFParamEEPercent / 100, 4, 4);
            }
            else
                return 0;

        }

        private static double CalculateVCEEAmount(DatabaseConnection dbConn, EEmpPersonalInfo empInfo, int AVCPlanID, double TotalMCRI, double TotalMC, double TotalVCRI, EPayrollPeriod payrollPeriod, DateTime MPFJoinDate)
        {
            double totalVC = 0;

            EAVCPlan avcPlan = new EAVCPlan();
            avcPlan.AVCPlanID = AVCPlanID;
            if (EAVCPlan.db.select(dbConn, avcPlan))
            {
                if (!avcPlan.AVCPlanContributeMaxAge)
                {
                    DateTime dt1AgeMax = empInfo.EmpDateOfBirth.AddYears(AGE_MAXIMUM);
                    if (dt1AgeMax <= payrollPeriod.PayPeriodTo && dt1AgeMax >= payrollPeriod.PayPeriodFr)
                    {
                        EEmpTermination empTermination = EEmpTermination.GetObjectByEmpID(dbConn, empInfo.EmpID);
                        bool blnTerminated;
                        if (empTermination != null)
                        {
                            if (empTermination.EmpTermLastDate <= payrollPeriod.PayPeriodTo && empTermination.EmpTermLastDate >= payrollPeriod.PayPeriodFr)
                                blnTerminated = true;
                            else
                                blnTerminated = false;
                        }
                        else
                            blnTerminated = false;
                        if (blnTerminated)
                        {
                            if (empTermination.EmpTermLastDate >= dt1AgeMax)
                                TotalVCRI = TotalVCRI * (1.0 - (double)(empTermination.EmpTermLastDate.Subtract(dt1AgeMax).Days + 1) / (empTermination.EmpTermLastDate.Subtract(payrollPeriod.PayPeriodFr > empInfo.EmpDateOfJoin ? payrollPeriod.PayPeriodFr : empInfo.EmpDateOfJoin).Days + 1));
                        }
                        else
                            TotalVCRI = TotalVCRI * (1.0 - (double)(payrollPeriod.PayPeriodTo.Subtract(dt1AgeMax).Days + 1) / (payrollPeriod.PayPeriodTo.Subtract(payrollPeriod.PayPeriodFr > empInfo.EmpDateOfJoin ? payrollPeriod.PayPeriodFr : empInfo.EmpDateOfJoin).Days + 1));
                    }
                    if (dt1AgeMax <= payrollPeriod.PayPeriodFr)
                        return 0;
                }

                //  Use Service Year to Compare
                double YearOfService = HROne.Payroll.PayrollProcess.GetYearOfServer(dbConn, empInfo.EmpID, payrollPeriod.PayPeriodFr);
                EAVCPlanDetail avcPlanDetailFrom = avcPlan.GetAVCPlanDetail(dbConn, YearOfService);
                if (avcPlanDetailFrom == null)
                    avcPlanDetailFrom = new EAVCPlanDetail();

                EMPFParameter mpfParameter = getLatestMPFParameter(dbConn, payrollPeriod.PayPeriodFr);
                double dblMaxTotal = 0;
                double dblMinTotal = 0;
                if (mpfParameter != null)
                {
                    EPayrollGroup payrollGroup = EPayrollGroup.GetPayrollGroup(dbConn, payrollPeriod.PayGroupID);

                    if (payrollGroup.PayGroupFreq == "M")
                    {
                        dblMaxTotal = mpfParameter.MPFParamMaxMonthly;
                        dblMinTotal = mpfParameter.MPFParamMinMonthly;
                    }
                    else
                    {
                        int intNumberOfDate = payrollPeriod.PayPeriodTo.Subtract(payrollPeriod.PayPeriodFr).Days + 1;
                        dblMaxTotal = mpfParameter.MPFParamMaxDaily * intNumberOfDate;
                        dblMinTotal = mpfParameter.MPFParamMinDaily * intNumberOfDate;
                    }


                }
                //  Check Exemption Period (For EE Voluntary Contribution Only)
                if (MPFJoinDate != null)
                {
                    DateTime mpfJoin31thDate = MPFJoinDate.AddDays(30);
                    if (payrollPeriod.PayPeriodTo < mpfJoin31thDate)
                    {
                        if (avcPlan.AVCPlanUseMPFExemption)
                            TotalVCRI = 0;
                    }
                    if (payrollPeriod.PayPeriodFr < mpfJoin31thDate && mpfJoin31thDate <= payrollPeriod.PayPeriodTo)
                    {
                        // According to MPF Ordinance effective at Feb 2003, no prorata will be calculated
                        if (avcPlan.AVCPlanUseMPFExemption)
                            TotalVCRI = 0;
                    }
                }
                if (TotalVCRI <= 0)
                    return 0;
                else
                {
                    double dblEEBelowRI = avcPlanDetailFrom.AVCPlanDetailEEBelowRI;
                    double dblEEAboveRI = avcPlanDetailFrom.AVCPlanDetailEEAboveRI;
                    double dblEEFix = avcPlanDetailFrom.AVCPlanDetailEEFix;

                    //  May be changed using AVCPlanID in future if has problem?
                    EEmpAVCPlan empAVCPlan = (EEmpAVCPlan)AppUtils.GetLastObj(dbConn, EEmpAVCPlan.db, "EmpAVCEffFr", empInfo.EmpID, new Match("EmpAVCEffFr", "<=", payrollPeriod.PayPeriodTo));
                    if (empAVCPlan.EmpAVCEEOverrideSetting)
                    {
                        dblEEBelowRI = empAVCPlan.EmpAVCEEBelowRI;
                        dblEEAboveRI = empAVCPlan.EmpAVCEEAboveRI;
                        dblEEFix = empAVCPlan.EmpAVCEEFix;
                    }

                    if (TotalVCRI < dblMinTotal)
                    {
                        //  Check Minimum Contribution Amount (For EE Voluntary Contribution Only)
                        if (!avcPlan.AVCPlanContributeMinRI)
                            return 0;
                        else
                            totalVC = TotalVCRI * dblEEBelowRI / 100;
                    }
                    else if (TotalVCRI < dblMaxTotal)
                        totalVC = TotalVCRI * dblEEBelowRI / 100;
                    else
                        totalVC = dblMaxTotal * dblEEBelowRI / 100 + (TotalVCRI - dblMaxTotal) * dblEEAboveRI / 100;
                    totalVC += dblEEFix;
                    if (avcPlan.AVCPlanEmployeeResidual)
                    {
                        // Start 0000106, Ricky So, 2014-10-22
                        totalVC = (totalVC > avcPlan.AVCPlanEmployeeResidualCap) ? avcPlan.AVCPlanEmployeeResidualCap : totalVC;
                        // Start 0000106, Ricky So, 2014-10-22

                        if (totalVC > TotalMC)
                            totalVC -= TotalMC;
                        else
                            totalVC = 0;
                    }
                    totalVC = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(totalVC, 4, 4);
                    return totalVC < avcPlan.AVCPlanMaxEmployeeVC ? totalVC : avcPlan.AVCPlanMaxEmployeeVC;
                }
            }
            else
                return 0;
        }

        private static double CalculateVCERAmount(DatabaseConnection dbConn, EEmpPersonalInfo empInfo, int AVCPlanID, double TotalMCRI, double TotalMC, double TotalVCRI, EPayrollPeriod payrollPeriod)
        {
            double totalVC = 0;

            EAVCPlan avcPlan = new EAVCPlan();
            avcPlan.AVCPlanID = AVCPlanID;
            if (EAVCPlan.db.select(dbConn, avcPlan))
            {
                if (!avcPlan.AVCPlanContributeMaxAge)
                {
                    DateTime dt1AgeMax = empInfo.EmpDateOfBirth.AddYears(AGE_MAXIMUM);
                    if (dt1AgeMax <= payrollPeriod.PayPeriodTo && dt1AgeMax >= payrollPeriod.PayPeriodFr)
                    {
                        EEmpTermination empTermination = EEmpTermination.GetObjectByEmpID(dbConn, empInfo.EmpID);
                        bool blnTerminated;
                        if (empTermination != null)
                        {
                            if (empTermination.EmpTermLastDate <= payrollPeriod.PayPeriodTo && empTermination.EmpTermLastDate >= payrollPeriod.PayPeriodFr)
                                blnTerminated = true;
                            else
                                blnTerminated = false;
                        }
                        else
                            blnTerminated = false;
                        if (blnTerminated)
                        {
                            if (empTermination.EmpTermLastDate >= dt1AgeMax)
                                TotalVCRI = TotalVCRI * (1.0 - (double)(empTermination.EmpTermLastDate.Subtract(dt1AgeMax).Days + 1) / (empTermination.EmpTermLastDate.Subtract(payrollPeriod.PayPeriodFr > empInfo.EmpDateOfJoin ? payrollPeriod.PayPeriodFr : empInfo.EmpDateOfJoin).Days + 1));
                        }
                        else
                            TotalVCRI = TotalVCRI * (1.0 - (double)(payrollPeriod.PayPeriodTo.Subtract(dt1AgeMax).Days + 1) / (payrollPeriod.PayPeriodTo.Subtract(payrollPeriod.PayPeriodFr > empInfo.EmpDateOfJoin ? payrollPeriod.PayPeriodFr : empInfo.EmpDateOfJoin).Days + 1));
                    }
                    if (dt1AgeMax <= payrollPeriod.PayPeriodFr)
                        return 0;
                }

                //  Use Service Year to Compare
                double YearOfService = HROne.Payroll.PayrollProcess.GetYearOfServer(dbConn, empInfo.EmpID, payrollPeriod.PayPeriodFr);
                EAVCPlanDetail avcPlanDetailFrom = avcPlan.GetAVCPlanDetail(dbConn, YearOfService);
                if (avcPlanDetailFrom == null)
                    avcPlanDetailFrom = new EAVCPlanDetail();
                EMPFParameter mpfParameter = getLatestMPFParameter(dbConn, payrollPeriod.PayPeriodFr);
                double dblMaxTotal = 0;
                double dblMinTotal = 0;
                if (mpfParameter != null)
                {
                    EPayrollGroup payrollGroup = EPayrollGroup.GetPayrollGroup(dbConn, payrollPeriod.PayGroupID);

                    if (payrollGroup.PayGroupFreq == "M")
                    {
                        dblMaxTotal = mpfParameter.MPFParamMaxMonthly;
                        dblMinTotal = mpfParameter.MPFParamMinMonthly;
                    }
                    else
                    {
                        int intNumberOfDate = payrollPeriod.PayPeriodTo.Subtract(payrollPeriod.PayPeriodFr).Days + 1;
                        dblMaxTotal = mpfParameter.MPFParamMaxDaily * intNumberOfDate;
                        dblMinTotal = mpfParameter.MPFParamMinDaily * intNumberOfDate;
                    }


                }
                if (TotalVCRI <= 0)
                    return 0;
                else
                {
                    double dblERBelowRI = avcPlanDetailFrom.AVCPlanDetailERBelowRI;
                    double dblERAboveRI = avcPlanDetailFrom.AVCPlanDetailERAboveRI;
                    double dblERFix = avcPlanDetailFrom.AVCPlanDetailERFix;

                    //  May be changed using AVCPlanID in future if has problem?
                    EEmpAVCPlan empAVCPlan = (EEmpAVCPlan)AppUtils.GetLastObj(dbConn, EEmpAVCPlan.db, "EmpAVCEffFr", empInfo.EmpID, new Match("EmpAVCEffFr", "<=", payrollPeriod.PayPeriodTo));
                    if (empAVCPlan.EmpAVCEROverrideSetting)
                    {
                        dblERBelowRI = empAVCPlan.EmpAVCERBelowRI;
                        dblERAboveRI = empAVCPlan.EmpAVCERAboveRI;
                        dblERFix = empAVCPlan.EmpAVCERFix;
                    }

                    if (TotalVCRI < dblMaxTotal)
                        totalVC = TotalVCRI * dblERBelowRI / 100;
                    else
                        totalVC = dblMaxTotal * dblERBelowRI / 100 + (TotalVCRI - dblMaxTotal) * dblERAboveRI / 100;
                    totalVC += dblERFix;
                    if (avcPlan.AVCPlanEmployerResidual)
                    {
                        // Start 0000106, Ricky So, 2014/10/22
                        totalVC = (totalVC > avcPlan.AVCPlanEmployerResidualCap) ? avcPlan.AVCPlanEmployerResidualCap : totalVC;
                        // End 0000106, Ricky So, 2014/10/22
                        if (totalVC > TotalMC)
                            totalVC -= TotalMC;
                        else
                            totalVC = 0;
                    }
                    totalVC = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(totalVC, 4, 4);
                    return totalVC < avcPlan.AVCPlanMaxEmployerVC ? totalVC : avcPlan.AVCPlanMaxEmployerVC;
                }
            }
            else
                return 0;
        }

        private static EMPFParameter getLatestMPFParameter(DatabaseConnection dbConn, DateTime effectiveDate)
        {
            DBFilter filter = new DBFilter();
            filter.add(new Match("MPFParamEffFr", "<=", effectiveDate));
            filter.add("MPFParamEffFr", false);

            ArrayList EMPFParameters = EMPFParameter.db.select(dbConn, filter);
            if (EMPFParameters.Count > 0)
                return ((EMPFParameter)EMPFParameters[0]);
            else
                return null;
        }

        public static ArrayList GenerateMPFEEPaymentRecords(DatabaseConnection dbConn, int EmpID, EPayrollPeriod PayPeriod, ArrayList mpfRecords, ArrayList paymentRecords)
        {
            DBFilter payPeriodFilter = new DBFilter();
            payPeriodFilter.add(new Match("PayPeriodFr", "<=", PayPeriod.PayPeriodTo));
            payPeriodFilter.add(new Match("PayPeriodTo", ">=", PayPeriod.PayPeriodFr));

            DBFilter empPayrollFilter = new DBFilter();
            empPayrollFilter.add(new Match("EmpID", EmpID));
            empPayrollFilter.add(new IN("PayPeriodID", "Select PayPeriodID from " + EPayrollPeriod.db.dbclass.tableName, payPeriodFilter));

            DBFilter allMPFRecordFilter = new DBFilter();
            allMPFRecordFilter.add(new IN("EmpPayrollID", "Select EmpPayrollID from " + EEmpPayroll.db.dbclass.tableName, empPayrollFilter));

            ArrayList allMPFRecordList = EMPFRecord.db.select(dbConn, allMPFRecordFilter);
            allMPFRecordList.AddRange(mpfRecords);

            ArrayList allPaymentRecordList = EPaymentRecord.db.select(dbConn, allMPFRecordFilter);

            //double totalStoredMPFEE = 0, totalStoredAVCEE = 0;

            EPaymentType mpfEEPaymentType = EPaymentType.SystemPaymentType.MPFEmployeeMandatoryContributionPaymentType(dbConn);
            if (mpfEEPaymentType == null)
                mpfEEPaymentType = new EPaymentType();
            EPaymentType avcEEPaymentType = EPaymentType.SystemPaymentType.MPFEmployeeVoluntaryContributionPaymentType(dbConn);
            if (avcEEPaymentType == null)
                avcEEPaymentType = new EPaymentType();

            System.Collections.Generic.Dictionary<int, double> totalStoredMPFEEByCostCenterID = new System.Collections.Generic.Dictionary<int, double>();
            System.Collections.Generic.Dictionary<int, double> totalStoredAVCEEByCostCenterID = new System.Collections.Generic.Dictionary<int, double>();

            foreach (EPaymentRecord paymentRecord in allPaymentRecordList)
            {
                if (paymentRecord != null)
                {
                    int costCenterID = paymentRecord.CostCenterID;
                    EPaymentCode paymentCode = new EPaymentCode();
                    paymentCode.PaymentCodeID = paymentRecord.PaymentCodeID;
                    if (EPaymentCode.db.select(dbConn, paymentCode))
                        if (paymentCode.PaymentTypeID.Equals(mpfEEPaymentType.PaymentTypeID))
                        {
                            if (!totalStoredMPFEEByCostCenterID.ContainsKey(costCenterID))
                                totalStoredMPFEEByCostCenterID.Add(costCenterID, 0);
                            totalStoredMPFEEByCostCenterID[costCenterID] += paymentRecord.PayRecActAmount;
                        }
                        else if (paymentCode.PaymentTypeID.Equals(avcEEPaymentType.PaymentTypeID))
                        {
                            if (!totalStoredAVCEEByCostCenterID.ContainsKey(costCenterID))
                                totalStoredAVCEEByCostCenterID.Add(costCenterID, 0);
                            totalStoredAVCEEByCostCenterID[costCenterID] += paymentRecord.PayRecActAmount;
                        }
                }
            }
            foreach (EMPFRecord mpfRecord in allMPFRecordList)
            {
                if (mpfRecord != null)
                {
                    int costCenterID = mpfRecord.CostCenterID;
                    if (!totalStoredMPFEEByCostCenterID.ContainsKey(costCenterID))
                        totalStoredMPFEEByCostCenterID.Add(costCenterID, 0);
                    totalStoredMPFEEByCostCenterID[costCenterID] += mpfRecord.MPFRecActMCEE;

                    if (!totalStoredAVCEEByCostCenterID.ContainsKey(costCenterID))
                        totalStoredAVCEEByCostCenterID.Add(costCenterID, 0);
                    totalStoredAVCEEByCostCenterID[costCenterID] += mpfRecord.MPFRecActVCEE;
 
                }
            }

            ArrayList mpfPaymentRecords = new ArrayList();

            ArrayList mpfPaymentCodes = PayrollProcess.GetPaymentCodeByPaymentType(dbConn, mpfEEPaymentType.PaymentTypeCode);
            if (mpfPaymentCodes.Count > 0)
            {
                EPaymentCode mpfPaymentCode = (EPaymentCode)mpfPaymentCodes[0];

                foreach (int costCenterID in totalStoredMPFEEByCostCenterID.Keys)
                {
                    double totalStoredMPFEE = totalStoredMPFEEByCostCenterID[costCenterID];
                    if (Math.Abs(totalStoredMPFEE) >= 0.01)
                        mpfPaymentRecords.AddRange(PayrollProcess.GetDeductablePaymentRecord(dbConn, EmpID, paymentRecords, -totalStoredMPFEE, mpfPaymentCode.PaymentCodeID));
                }
            }
            mpfPaymentCodes = PayrollProcess.GetPaymentCodeByPaymentType(dbConn, avcEEPaymentType.PaymentTypeCode);
            if (mpfPaymentCodes.Count > 0)
            {
                EPaymentCode mpfPaymentCode = (EPaymentCode)mpfPaymentCodes[0];

                foreach (int costCenterID in totalStoredAVCEEByCostCenterID.Keys)
                {
                    double totalStoredAVCEE = totalStoredAVCEEByCostCenterID[costCenterID];
                    if (Math.Abs(totalStoredAVCEE) >= 0.01)
                        mpfPaymentRecords.AddRange(PayrollProcess.GetDeductablePaymentRecord(dbConn, EmpID, paymentRecords, -totalStoredAVCEE, mpfPaymentCode.PaymentCodeID));
                }
            }
            foreach (EPaymentRecord paymentRecord in mpfPaymentRecords)
                paymentRecord.PayRecType = PaymentRecordType.PAYRECORDTYPE_PENSION;

            return mpfPaymentRecords;
        }

        //private static EEmpTermination EEmpTermination.GetObjectByEmpID(dbConn, DatabaseConnection dbConn, int EmpID)
        //{
        //    DBFilter filter = new DBFilter();
        //    filter.add(new Match("empid", EmpID));


        //    ArrayList empTerminations = EEmpTermination.db.select(dbConn, filter);
        //    if (empTerminations.Count > 0)
        //        return ((EEmpTermination)empTerminations[0]);
        //    else
        //        return null;
        //}

        private static int GetMPFPlainID(DatabaseConnection dbConn, int EmpID, DateTime ReferenceDate)
        {
            DBFilter empMpfPlanFilter = new DBFilter();
            empMpfPlanFilter.add(new Match("EMPID", EmpID));
            empMpfPlanFilter.add(new Match("EmpMPFEffFr", "<=", ReferenceDate));

            OR orMpfEffFilter = new OR();
            orMpfEffFilter.add(new Match("EmpMPFEffTo", ">=", ReferenceDate));
            orMpfEffFilter.add(new NullTerm("EmpMPFEffTo"));
            empMpfPlanFilter.add(orMpfEffFilter);
            empMpfPlanFilter.add("EmpMPFEffFr", false);

            ArrayList empMPFPlans = EEmpMPFPlan.db.select(dbConn, empMpfPlanFilter);

            if (empMPFPlans.Count > 0)
                return ((EEmpMPFPlan)empMPFPlans[0]).MPFPlanID;
            else
            {
                DBFilter empAVCPlanFilter = new DBFilter();
                empAVCPlanFilter.add(new Match("EMPID", EmpID));
                empAVCPlanFilter.add(new Match("EmpAVCEffFr", "<=", ReferenceDate));

                OR orAVCEffFilter = new OR();
                orAVCEffFilter.add(new Match("EmpAVCEffTo", ">=", ReferenceDate));
                orAVCEffFilter.add(new NullTerm("EmpAVCEffTo"));
                empAVCPlanFilter.add(orAVCEffFilter);
                empAVCPlanFilter.add("EmpAVCEffFr", false);

                ArrayList empAVCPlans = EEmpAVCPlan.db.select(dbConn, empAVCPlanFilter);

                if (empAVCPlans.Count > 0)
                    return ((EEmpAVCPlan)empAVCPlans[0]).DefaultMPFPlanID;
                else
                    return 0;
            }

        }

        private static int GetAVCPlainID(DatabaseConnection dbConn, int EmpID, DateTime ReferenceDate)
        {
            DBFilter empAVCPlanFilter = new DBFilter();
            empAVCPlanFilter.add(new Match("EMPID", EmpID));
            empAVCPlanFilter.add(new Match("EmpAVCEffFr", "<=", ReferenceDate));

            OR orAVCEffFilter = new OR();
            orAVCEffFilter.add(new Match("EmpAVCEffTo", ">=", ReferenceDate));
            orAVCEffFilter.add(new NullTerm("EmpAVCEffTo"));
            empAVCPlanFilter.add(orAVCEffFilter);
            empAVCPlanFilter.add("EmpAVCEffFr", false);

            ArrayList empAVCPlans = EEmpAVCPlan.db.select(dbConn, empAVCPlanFilter);

            if (empAVCPlans.Count >= 1)
                return ((EEmpAVCPlan)empAVCPlans[0]).AVCPlanID;
            else
                return 0;

        }
        public static void UndoMPF(DatabaseConnection dbConn, EEmpPayroll empPayroll)
        {


            DBFilter filter = new DBFilter();
            filter.add(new Match("EmpPayrollID", empPayroll.EmpPayrollID));
            ArrayList MPFRecords = EMPFRecord.db.select(dbConn, filter);
            foreach (EMPFRecord MPFRecord in MPFRecords)
            {
                EMPFRecord.db.delete(dbConn, MPFRecord);

            }

        }
        public static void Recalculate(DatabaseConnection dbConn, EEmpPayroll empPayroll)
        {
            EPayrollPeriod payPeriod = new EPayrollPeriod();
            payPeriod.PayPeriodID = empPayroll.PayPeriodID;
            if (EPayrollPeriod.db.select(dbConn, payPeriod))
            {
                DBFilter paymentFilter = new DBFilter();
                paymentFilter.add(new Match("EmpPayrollID", empPayroll.EmpPayrollID));
                ArrayList paymentRecords = EPaymentRecord.db.select(dbConn, paymentFilter);
                ArrayList mpfRecords = HROne.Payroll.MPFProcess.MPFTrialRun(dbConn, empPayroll.EmpID, payPeriod, new ArrayList());
                ArrayList mpfPaymentRecords = HROne.Payroll.MPFProcess.GenerateMPFEEPaymentRecords(dbConn, empPayroll.EmpID, payPeriod, mpfRecords, paymentRecords);



                if (mpfRecords != null)
                    foreach (EMPFRecord mpfRecord in mpfRecords)
                    {
                        DBFilter existingMPFRecordFilter = new DBFilter();
                        existingMPFRecordFilter.add(new Match("EmpPayrollID", empPayroll.EmpPayrollID));
                        existingMPFRecordFilter.add(new Match("MPFRecPeriodFr", mpfRecord.MPFRecPeriodFr));
                        existingMPFRecordFilter.add(new Match("MPFRecPeriodTo", mpfRecord.MPFRecPeriodTo));
                        existingMPFRecordFilter.add(new Match("MPFPlanID", mpfRecord.MPFPlanID));

                        if (!(mpfRecord.MPFRecActMCEE == 0 && mpfRecord.MPFRecActMCER == 0 && mpfRecord.MPFRecActMCRI == 0 && mpfRecord.MPFRecActVCEE == 0 && mpfRecord.MPFRecActVCER == 0 && mpfRecord.MPFRecActVCRI == 0) || EMPFRecord.db.count(dbConn, existingMPFRecordFilter) <= 0)
                        {
                            mpfRecord.EmpPayrollID = empPayroll.EmpPayrollID;
                            if (!empPayroll.EmpPayStatus.Equals("T"))
                                mpfRecord.MPFRecType = "A";
                            EMPFRecord.db.insert(dbConn, mpfRecord);
                        }
                    }
                if (mpfPaymentRecords != null)
                    foreach (EPaymentRecord paymentRecord in mpfPaymentRecords)
                    {
                        paymentRecord.EmpPayrollID = empPayroll.EmpPayrollID;
                        if (!empPayroll.EmpPayStatus.Equals("T"))
                            paymentRecord.PayRecType = "A";
                        EPaymentRecord.db.insert(dbConn, paymentRecord);
                    }
            }
        }
    }
}