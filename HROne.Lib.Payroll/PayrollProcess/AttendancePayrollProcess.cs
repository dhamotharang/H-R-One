using System;
using System.Collections;
using System.Text;
using HROne.Lib.Entities;
using HROne.DataAccess;
using HROne.Lib;

namespace HROne.Payroll
{
    public class AttendanceProcess
    {
        protected DatabaseConnection dbConn;

        public AttendanceProcess(DatabaseConnection dbConn)
        {
            this.dbConn = dbConn;
        }

        protected int GetRosterCostCenterID(int RosterCodeID)
        {
            ERosterCode rosterCode = new ERosterCode();
            rosterCode.RosterCodeID = RosterCodeID;
            if (ERosterCode.db.select(dbConn, rosterCode))
            {
                ECostCenter defaultCostCenter = new ECostCenter();
                defaultCostCenter.CostCenterID = rosterCode.CostCenterID;

                if (!ECostCenter.db.select(dbConn, defaultCostCenter))
                {
                    ERosterClientSite site = new ERosterClientSite();
                    site.RosterClientSiteID = rosterCode.RosterClientSiteID;
                    if (ERosterClientSite.db.select(dbConn, site))
                    {
                        defaultCostCenter.CostCenterID = site.GetDefaultCostCenterID(dbConn);
                    }
                    else
                    {
                        //  If site does not exists or deleted accidently
                        ERosterClient client = new ERosterClient();
                        client.RosterClientID = rosterCode.RosterClientID;
                        if (ERosterClient.db.select(dbConn, client))
                        {
                            defaultCostCenter.CostCenterID = client.CostCenterID;
                            if (!ECostCenter.db.select(dbConn, defaultCostCenter))
                            {
                                defaultCostCenter.CostCenterID = 0;
                            }
                        }
                    }
                }
                return defaultCostCenter.CostCenterID;
            }
            else
                return 0;
        }

        protected bool IsUseEmpCostCenter(int EmpID, DateTime AsOfDate, int CostCenterID)
        {
            if (CostCenterID > 0)
            {
                //  Check if the cost center match with the pre-defined cost center in employee detail
                AND andTerm = new AND();
                OR orEffectiveFrom = new OR();
                orEffectiveFrom.add(new Match("EmpCostCenterEffFr", "<=", AsOfDate));
                orEffectiveFrom.add(new NullTerm("EmpCostCenterEffFr"));
                andTerm.add(orEffectiveFrom);

                OR orEffectiveTo = new OR();
                orEffectiveTo.add(new Match("EmpCostCenterEffTo", ">=", AsOfDate));
                orEffectiveTo.add(new NullTerm("EmpCostCenterEffTo"));
                andTerm.add(orEffectiveTo);

                EEmpCostCenter currentEmpCostCenter = (EEmpCostCenter)AppUtils.GetLastObj(dbConn, EEmpCostCenter.db, "EmpCostCenterEffFr", EmpID, andTerm);
                if (currentEmpCostCenter != null)
                {
                    DBFilter empCostCenterDetailFilter = new DBFilter();
                    empCostCenterDetailFilter.add(new Match("EmpCostCenterID", currentEmpCostCenter.EmpCostCenterID));
                    empCostCenterDetailFilter.add(new Match("CostCenterID", CostCenterID));
                    empCostCenterDetailFilter.add(new Match("EmpCostCenterPercentage", 100));

                    if (EEmpCostCenterDetail.db.count(dbConn, empCostCenterDetailFilter) > 0)
                        return true;
                }
                return false;
            }
            else
                return true;
        }

        protected int GetRosterCostCenterIDByLocation(string StartLocation, string EndLocation)
        {
            ERosterClientSite startSite = GetRosterClientSiteByLocation(StartLocation);
            ERosterClientSite endSite = GetRosterClientSiteByLocation(EndLocation);
            if (startSite != null && endSite != null)
            {
                int m_startSiteCostCenterID = startSite.GetDefaultCostCenterID(dbConn);
                int m_endSiteCostCenterID = startSite.GetDefaultCostCenterID(dbConn);
                if (m_startSiteCostCenterID.Equals(m_endSiteCostCenterID))
                    return m_startSiteCostCenterID;
                else
                    if (!m_startSiteCostCenterID.Equals(0))
                        return m_startSiteCostCenterID;
                    else
                        return m_endSiteCostCenterID;
            }
            else if (startSite != null)
            {
                return startSite.GetDefaultCostCenterID(dbConn);
            }
            else if (endSite != null)
            {
                return endSite.GetDefaultCostCenterID(dbConn);
            }
            else
                return 0;
        }
        protected ERosterClientSite GetRosterClientSiteByLocation(string Location)
        {
            if (string.IsNullOrEmpty(Location))
                return null;
            DBFilter rosterClientSiteFilter = new DBFilter();
            rosterClientSiteFilter.add(new Match("RosterClientSiteCode", Location));
            ArrayList rosterClientSiteList = ERosterClientSite.db.select(dbConn, rosterClientSiteFilter);
            if (rosterClientSiteList.Count > 0)
                return (ERosterClientSite)rosterClientSiteList[0];
            else
            {
                rosterClientSiteList = ERosterClientSite.db.select(dbConn, (DBFilter)null);
                foreach (ERosterClientSite site in rosterClientSiteList)
                {
                    if (site.RosterClientSiteCode.Equals(Location, StringComparison.CurrentCultureIgnoreCase))
                        return site;
                }
            }
            return null;
        }
        public ArrayList AttendancePaymentTrialRun(int EmpID, EPayrollPeriod payrollPeriod, DateTime PayrollGroupDateFrom, DateTime PayrollGroupDateTo)
        {

            //Hashtable recurringPaymentHashTable = new Hashtable();
            PaymentBreakDownCollection recurringPaymentBreakDownCollection = new PaymentBreakDownCollection();
            PaymentBreakDownCollection LateEarlyLeaveBreakDownCollection = new PaymentBreakDownCollection();
            //Hashtable lunchPaymentHashTable = new Hashtable();
            //Hashtable overtimePaymentHashTable = new Hashtable();
            //Hashtable lateDeductionHashTable = new Hashtable();
            //ArrayList attendancePaymentList = new ArrayList();

            EAttendancePlan lastAttendancePlan = null;

            int absentCount = 0;
            int totalBonusLateMins = 0;
            int totalBonusLateCount = 0;
            int totalBonusLunchLateMins = 0;
            int totalBonusLunchLateCount = 0;
            int totalBonusEarlyLeaveMins = 0;
            int totalBonusEarlyLeaveCount = 0;
            int totalBonusLunchEarlyLeaveMins = 0;
            int totalBonusLunchEarlyLeaveCount = 0;
            int totalBonusSLWithMedicalCertificate = 0;
            int totalBonusSLWithoutMedicalCertificate = 0;
            int totalBonusInjuryLeave = 0;
            int totalBonusNonFullPayLeave = 0;
            double totalWorkHour = 0;
            double totalWorkDay = 0;

            int totalLateMins = 0;
            //int totalEarlyLeaveMins = 0;  no longer use, merged with totalLateMins
            int totalOvertimeMins = 0;
            //int totalCompensateOTLateEarlyLeaveMin = 0;
            //bool hasLastDateAttendance = false;
            DateTime lastDateOfAttendanceRecord = new DateTime();
            //double overtimePaymentAmount = 0;
            //double lateDeductionAmount = 0;

            EEmpTermination empTermination = EEmpTermination.GetObjectByEmpID(dbConn, EmpID);


            DBFilter attendanceRecordFilter = new DBFilter();
            attendanceRecordFilter.add(new Match("EmpID", EmpID));
            if (payrollPeriod.PayPeriodAttnFr <= PayrollGroupDateFrom && payrollPeriod.PayPeriodAttnTo >= PayrollGroupDateTo)
            {
                attendanceRecordFilter.add(new Match("AttendanceRecordDate", ">=", PayrollGroupDateFrom));
                attendanceRecordFilter.add(new Match("AttendanceRecordDate", "<=", PayrollGroupDateTo));
            }
            else
            {
                attendanceRecordFilter.add(new Match("AttendanceRecordDate", ">=", payrollPeriod.PayPeriodAttnFr));
                attendanceRecordFilter.add(new Match("AttendanceRecordDate", "<=", payrollPeriod.PayPeriodAttnTo));
            }
            attendanceRecordFilter.add("AttendanceRecordDate", true);
            ArrayList attendanceRecordList = EAttendanceRecord.db.select(dbConn, attendanceRecordFilter);
            //SortedList sortedAttendanceRecordList = new SortedList();

            //foreach (EAttendanceRecord attendanceRecord in attendanceRecordList )
            //{
            //    ERosterCode rosterCode= new ero
            //sortedAttendanceRecordList
            //}

            EPayrollGroup payGroup;

            payGroup = new EPayrollGroup();
            payGroup.PayGroupID = payrollPeriod.PayGroupID;
            EPayrollGroup.db.select(dbConn, payGroup);

            int numOfPeriodPerYear;
            if (payGroup.PayGroupFreq.Equals("M"))
                numOfPeriodPerYear = 12;
            else
                numOfPeriodPerYear = 24;

            //EPaymentRecord lastLatePaymentRecord = null;
            //double lastLatePaymentRate = 0;
            //double lastLateMins = 0;

            //EPaymentRecord lastOTPaymentRecord = null;
            //double lastOTPaymentRate = 0;
            //double lastOTMins = 0;

            bool skipBonusCalculation = false;
            foreach (EAttendanceRecord attendanceRecord in attendanceRecordList)
            {
                bool skipBonusEntitleCount = false;
                if (attendanceRecord.AttendanceRecordOverrideBonusEntitled)

                    if (attendanceRecord.AttendanceRecordHasBonus)
                    {
                        skipBonusEntitleCount = true;
                    }
                    else
                    {
                        skipBonusCalculation = true;
                    }

                totalWorkHour += attendanceRecord.AttendanceRecordActualWorkingHour;
                totalWorkDay += attendanceRecord.AttendanceRecordActualWorkingDay;

                //if (attendanceRecord.AttendanceRecordDate.Equals(payrollPeriod.PayPeriodAttnTo))
                //    hasLastDateAttendance = true;
                lastDateOfAttendanceRecord = attendanceRecord.AttendanceRecordDate;

                if (!skipBonusEntitleCount)
                {
                    if (attendanceRecord.AttendanceRecordIsAbsent)
                        absentCount++;
                    if (attendanceRecord.AttendanceRecordActualLateMins > 0)
                    {
                        totalBonusLateMins += attendanceRecord.AttendanceRecordActualLateMins;
                        totalBonusLateCount++;
                    }
                    if (attendanceRecord.AttendanceRecordActualEarlyLeaveMins > 0)
                    {
                        totalBonusEarlyLeaveMins += attendanceRecord.AttendanceRecordActualEarlyLeaveMins;
                        totalBonusEarlyLeaveCount++;
                    }
                    if (attendanceRecord.AttendanceRecordActualLunchLateMins > 0)
                    {
                        totalBonusLunchLateMins += attendanceRecord.AttendanceRecordActualLunchLateMins;
                        totalBonusLunchLateCount++;
                    }
                    if (attendanceRecord.AttendanceRecordActualLunchEarlyLeaveMins > 0)
                    {
                        totalBonusLunchEarlyLeaveMins += attendanceRecord.AttendanceRecordActualLunchEarlyLeaveMins;
                        totalBonusLunchEarlyLeaveCount++;
                    }
                }
                //if (attendanceRecord.AttendanceRecordActualLateMins > 0)
                //    totalLateMins += attendanceRecord.AttendanceRecordActualLateMins;
                //if (attendanceRecord.AttendanceRecordActualEarlyLeaveMins > 0)
                //    totalEarlyLeaveMins += attendanceRecord.AttendanceRecordActualEarlyLeaveMins;

                double RosterCodeWorkHourPerDay = 0;
                ERosterCode rosterCode = new ERosterCode();
                rosterCode.RosterCodeID = attendanceRecord.RosterCodeID;
                if (ERosterCode.db.select(dbConn, rosterCode))
                {
                    rosterCode = HROne.Attendance.AttendanceProcess.GetActualRosterDateTime(attendanceRecord.AttendanceRecordDate, rosterCode, attendanceRecord.AttendanceRecordRosterCodeInTimeOverride, attendanceRecord.AttendanceRecordRosterCodeOutTimeOverride, attendanceRecord.AttendanceRecordRosterCodeLunchStartTimeOverride, attendanceRecord.AttendanceRecordRosterCodeLunchEndTimeOverride);

                    RosterCodeWorkHourPerDay = rosterCode.RosterCodeDailyWorkingHour;
                    if (payGroup.PayGroupLunchTimeHasWage && RosterCodeWorkHourPerDay > 0)
                        RosterCodeWorkHourPerDay += rosterCode.RosterCodeLunchDurationHour;
                }
                int RosterCostCenterID = GetRosterCostCenterID(rosterCode.RosterCodeID);
                int costCenterIDByLocation1 = GetRosterCostCenterIDByLocation(attendanceRecord.AttendanceRecordWorkStartLocation, attendanceRecord.AttendanceRecordLunchOutLocation);
                int costCenterIDByLocation2 = GetRosterCostCenterIDByLocation(attendanceRecord.AttendanceRecordLunchInLocation, attendanceRecord.AttendanceRecordWorkEndLocation);
                if (costCenterIDByLocation1 == costCenterIDByLocation2 && costCenterIDByLocation1 > 0)
                {
                    RosterCostCenterID = costCenterIDByLocation1;
                }
                else
                {
                    if (costCenterIDByLocation1 <= 0)
                        costCenterIDByLocation1 = RosterCostCenterID;
                    if (costCenterIDByLocation2 <= 0)
                        costCenterIDByLocation2 = RosterCostCenterID;
                }

                #region Get Leave Information
                //  Get Leave Information For Bonus Calculation
                if (!skipBonusEntitleCount)
                {



                    DBFilter leaveApplicationFilter = new DBFilter();
                    leaveApplicationFilter.add(new Match("EmpID", EmpID));
                    leaveApplicationFilter.add(new Match("LeaveAppDateTo", ">=", attendanceRecord.AttendanceRecordDate));
                    if (rosterCode.RosterCodeOutTime.Ticks.Equals(0))
                        leaveApplicationFilter.add(new Match("LeaveAppDateFrom", "<=", attendanceRecord.AttendanceRecordDate));
                    else
                        leaveApplicationFilter.add(new Match("LeaveAppDateFrom", "<=", rosterCode.RosterCodeOutTime));
                    ArrayList leaveApplicationList = ELeaveApplication.db.select(dbConn, leaveApplicationFilter);
                    foreach (ELeaveApplication leaveApplication in leaveApplicationList)
                    {
                        if (leaveApplication.LeaveAppUnit.Equals("D", StringComparison.CurrentCultureIgnoreCase))
                        {
                            //  overnight roster do not use next day of "Daily" unit type leave application
                            if (leaveApplication.LeaveAppDateFrom > attendanceRecord.AttendanceRecordDate || leaveApplication.LeaveAppDateTo < attendanceRecord.AttendanceRecordDate)
                                break;
                        }
                        ELeaveCode leaveCode = new ELeaveCode();
                        leaveCode.LeaveCodeID = leaveApplication.LeaveCodeID;
                        if (ELeaveCode.db.select(dbConn, leaveCode))
                        {
                            if (leaveCode.LeaveTypeID.Equals(ELeaveType.SLCAT1_LEAVE_TYPE(dbConn).LeaveTypeID) || leaveCode.LeaveTypeID.Equals(ELeaveType.SLCAT2_LEAVE_TYPE(dbConn).LeaveTypeID))
                                if (leaveApplication.LeaveAppHasMedicalCertificate)
                                    totalBonusSLWithMedicalCertificate++;
                                else
                                    totalBonusSLWithoutMedicalCertificate++;
                            else if (leaveCode.LeaveTypeID.Equals(ELeaveType.INJURY_LEAVE_TYPE(dbConn).LeaveTypeID))
                                totalBonusInjuryLeave++;
                            else if (leaveCode.LeaveCodePayRatio < 1)
                                totalBonusNonFullPayLeave++;

                        }
                    }
                }
                #endregion

                EEmpPositionInfo empPos = AppUtils.GetLastPositionInfo(dbConn, attendanceRecord.AttendanceRecordDate, EmpID);
                EAttendancePlan attendancePlan = null;
                EWorkHourPattern workPatten = null;
                if (empPos != null)
                {
                    attendancePlan = new EAttendancePlan();
                    attendancePlan.AttendancePlanID = empPos.AttendancePlanID;
                    if (!EAttendancePlan.db.select(dbConn, attendancePlan))
                        attendancePlan = null;

                    workPatten = new EWorkHourPattern();
                    workPatten.WorkHourPatternID = empPos.WorkHourPatternID;
                    if (EWorkHourPattern.db.select(dbConn, workPatten))
                        workPatten = null;
                }

                double dailyOverrideAmount = 0;
                bool hasDailyOverridePayment=false;
                if (double.TryParse(attendanceRecord.GetAttendanceRecordExtendData(EAttendanceRecord.FIELD_EXTENDDATA_OVERRIDE_DAILY_PAYMENT), out dailyOverrideAmount))
                    hasDailyOverridePayment = true;

                CreateEmployeeRecurringPaymentListAttendanceResult(EmpID, attendanceRecord, attendancePlan, workPatten, payGroup, payrollPeriod, rosterCode, RosterCostCenterID, costCenterIDByLocation1, costCenterIDByLocation2, hasDailyOverridePayment, dailyOverrideAmount, recurringPaymentBreakDownCollection);

                int empOverrideCostCenterID = IsUseEmpCostCenter(EmpID, attendanceRecord.AttendanceRecordDate, RosterCostCenterID) ? 0 : RosterCostCenterID;
                int empOverrideCostCenterIDLocation1 = IsUseEmpCostCenter(EmpID, attendanceRecord.AttendanceRecordDate, costCenterIDByLocation1) ? 0 : costCenterIDByLocation1;
                int empOverrideCostCenterIDLocation2 = IsUseEmpCostCenter(EmpID, attendanceRecord.AttendanceRecordDate, costCenterIDByLocation2) ? 0 : costCenterIDByLocation2;

                if (attendancePlan != null)
                {
                    double totalPeriodPayment = 0;
                    double totalDailyPayment = 0;
                    double totalHourlyPayment = 0;

                    if (hasDailyOverridePayment)
                        totalDailyPayment = dailyOverrideAmount;
                    else
                    {
                        //  Only use Basic Salary type to calculate the OT/Late/EarlyLeave 
                        //  will add more option under attendance plan if non-basic type is included in calculation                        
                        totalPeriodPayment = HROne.Payroll.PayrollProcess.GetTotalPeriodPayRecurringPayment(dbConn, EmpID, attendanceRecord.AttendanceRecordDate, true, HROne.Payroll.PayrollProcess.PaymentCodeProrataOptionEnum.PaymentCodeProrataProrataOnly, HROne.Payroll.PayrollProcess.PaymentCodeProrataOptionEnum.PaymentCodeProrataProrataOnly, HROne.Payroll.PayrollProcess.PaymentCodeProrataOptionEnum.PaymentCodeProrataProrataOnly);
                        totalDailyPayment = HROne.Payroll.PayrollProcess.GetTotalDailyPayRecurringPayment(dbConn, EmpID, attendanceRecord.AttendanceRecordDate, true, payrollPeriod);
                        totalHourlyPayment = HROne.Payroll.PayrollProcess.GetTotalHourlyPayRecurringPayment(dbConn, EmpID, attendanceRecord.AttendanceRecordDate);
                    }
                    string payMethod = string.Empty;
                    int empAccID = 0;
                    ArrayList maxRecurringPaymentList = EEmpRecurringPayment.db.select(dbConn, HROne.Payroll.PayrollProcess.GetRecurringPaymentDBFilter(EmpID, attendanceRecord.AttendanceRecordDate, "EmpRPAmount", false));


                    if (maxRecurringPaymentList.Count > 0)
                    {
                        EEmpRecurringPayment recurringPayment = (EEmpRecurringPayment)maxRecurringPaymentList[0];
                        payMethod = recurringPayment.EmpRPMethod;
                        if (payMethod.Equals("A"))
                            empAccID = recurringPayment.EmpAccID;
                    }
                    else
                        payMethod = "Q";

                    #region Late Deduction Calculation

                    if (attendancePlan.AttendancePlanLateFormula > 0 && attendancePlan.AttendancePlanLatePayCodeID > 0 && attendancePlan.AttendancePlanLateMinsUnit > 0)
                    {
                        EAttendanceFormula attendanceFormula = new EAttendanceFormula();
                        EPaymentCode paymentCode = new EPaymentCode();


                        attendanceFormula.AttendanceFormulaID = attendancePlan.AttendancePlanLateFormula;
                        paymentCode.PaymentCodeID = attendancePlan.AttendancePlanLatePayCodeID;

                        if (EAttendanceFormula.db.select(dbConn, attendanceFormula) && EPaymentCode.db.select(dbConn, paymentCode))
                        {
                            int lateEarlyMinsLocation1;
                            int lateEarlyMinsLocation2;
                            int actualMins;

                            AddLateEarlyLeavePaymentAttendanceResult(EmpID, attendanceRecord, attendancePlan, payGroup, payrollPeriod, rosterCode, attendanceFormula, paymentCode, empOverrideCostCenterID, empOverrideCostCenterIDLocation1, empOverrideCostCenterIDLocation2, totalPeriodPayment, totalDailyPayment, totalHourlyPayment, numOfPeriodPerYear, payMethod, empAccID, LateEarlyLeaveBreakDownCollection, RosterCodeWorkHourPerDay, out actualMins, out lateEarlyMinsLocation1, out lateEarlyMinsLocation2);


                            totalLateMins += actualMins;

                            //if (attendancePlan.AttendancePlanCompensateLateByOT)
                            //    totalCompensateOTLateEarlyLeaveMin -= actualMins;
                        }
                    }
                    #endregion

                    #region Overtime Calculation
                    if (attendancePlan.AttendancePlanOTFormula > 0 && attendancePlan.AttendancePlanOTPayCodeID > 0 && attendancePlan.AttendancePlanOTMinsUnit > 0 && !attendancePlan.AttendancePlanOTGainAsCompensationLeaveEntitle)
                    {
                        EAttendanceFormula attendanceFormula = new EAttendanceFormula();
                        EPaymentCode paymentCode = new EPaymentCode();


                        attendanceFormula.AttendanceFormulaID = attendancePlan.AttendancePlanOTFormula;
                        paymentCode.PaymentCodeID = attendancePlan.AttendancePlanOTPayCodeID;

                        if (EAttendanceFormula.db.select(dbConn, attendanceFormula) && EPaymentCode.db.select(dbConn, paymentCode))
                        {
                            int actualMins = 0;
                            AddOvertimePaymentAttendanceResult(EmpID, attendanceRecord, attendancePlan, payGroup, payrollPeriod, rosterCode, attendanceFormula, paymentCode, empOverrideCostCenterID, empOverrideCostCenterIDLocation1, empOverrideCostCenterIDLocation2, totalPeriodPayment, totalDailyPayment, totalHourlyPayment, numOfPeriodPerYear, payMethod, empAccID, recurringPaymentBreakDownCollection, RosterCodeWorkHourPerDay, out actualMins);

                            totalOvertimeMins += actualMins;
                            //if (attendancePlan.AttendancePlanCompensateLateByOT)
                            //    totalCompensateOTLateEarlyLeaveMin += actualMins;
                        }
                    }
                    #endregion

                    #region Additional Payment
                    CreateAdditionalPaymentList(EmpID, attendanceRecord, attendancePlan, rosterCode, empOverrideCostCenterID, payMethod, empAccID, recurringPaymentBreakDownCollection);
                    #endregion

                    lastAttendancePlan = attendancePlan;
                }

            }
            if (lastAttendancePlan != null)
            {
                if (totalLateMins > lastAttendancePlan.AttendancePlanLateMaxTotalToleranceMins)
                    recurringPaymentBreakDownCollection.AddRange(LateEarlyLeaveBreakDownCollection);

                #region OT Compensate to Late/Early Leave
                if (lastAttendancePlan.AttendancePlanCompensateLateByOT)
                {
                    int totalCompensateOTLateEarlyLeaveMin = totalOvertimeMins ;
                    if (totalLateMins > lastAttendancePlan.AttendancePlanLateMaxTotalToleranceMins)
                        totalCompensateOTLateEarlyLeaveMin -= totalLateMins;

                    //  Only use Basic Salary type to calculate the OT/Late/EarlyLeave 
                    //  will add more option under attendance plan if non-basic type is included in calculation
                    double totalPeriodPayment = HROne.Payroll.PayrollProcess.GetTotalPeriodPayRecurringPayment(dbConn, EmpID, PayrollGroupDateTo, true, HROne.Payroll.PayrollProcess.PaymentCodeProrataOptionEnum.PaymentCodeProrataProrataOnly, HROne.Payroll.PayrollProcess.PaymentCodeProrataOptionEnum.PaymentCodeProrataProrataOnly, HROne.Payroll.PayrollProcess.PaymentCodeProrataOptionEnum.PaymentCodeProrataProrataOnly);
                    double totalDailyPayment = HROne.Payroll.PayrollProcess.GetTotalDailyPayRecurringPayment(dbConn, EmpID, PayrollGroupDateTo, true, payrollPeriod);
                    double totalHourlyPayment = HROne.Payroll.PayrollProcess.GetTotalHourlyPayRecurringPayment(dbConn, EmpID, PayrollGroupDateTo);

                    string payMethod = string.Empty;
                    int empAccID = 0;
                    ArrayList maxRecurringPaymentList = EEmpRecurringPayment.db.select(dbConn, HROne.Payroll.PayrollProcess.GetRecurringPaymentDBFilter(EmpID, PayrollGroupDateTo, "EmpRPAmount", false));


                    if (maxRecurringPaymentList.Count > 0)
                    {
                        EEmpRecurringPayment recurringPayment = (EEmpRecurringPayment)maxRecurringPaymentList[0];
                        payMethod = recurringPayment.EmpRPMethod;
                        if (payMethod.Equals("A"))
                            empAccID = recurringPayment.EmpAccID;
                    }
                    else
                        payMethod = "Q";

                    if (totalCompensateOTLateEarlyLeaveMin > 0)
                    {
                        EAttendanceFormula attendanceFormula = new EAttendanceFormula();
                        EPaymentCode paymentCode = new EPaymentCode();


                        attendanceFormula.AttendanceFormulaID = lastAttendancePlan.AttendancePlanOTFormula;
                        paymentCode.PaymentCodeID = lastAttendancePlan.AttendancePlanOTPayCodeID;

                        if (EAttendanceFormula.db.select(dbConn, attendanceFormula) && EPaymentCode.db.select(dbConn, paymentCode))
                        {

                            double hourlyRate = 0;
                            string hourlyRateRemark = string.Empty;     //  dummy, for debug only

                            hourlyRate = AttendanceFormulaProcess.GetHourlyRate(dbConn, attendanceFormula.AttendanceFormulaID, payGroup.PayGroupDefaultProrataFormula, EmpID, totalPeriodPayment, totalDailyPayment, totalHourlyPayment, payrollPeriod.PayPeriodAttnFr, payrollPeriod.PayPeriodAttnTo, numOfPeriodPerYear, PayrollGroupDateTo, false, 0, lastAttendancePlan.AttendancePlanOTRateMultiplier, 0, out hourlyRateRemark);


                            if (hourlyRate > 0)
                            {
                                HROne.Payroll.PaymentBreakDownKey overtimePaymentBreakDownKey = new HROne.Payroll.PaymentBreakDownKey(paymentCode.PaymentCodeID, hourlyRate, payMethod, empAccID, false, 0, true);
                                recurringPaymentBreakDownCollection.AddUnit(overtimePaymentBreakDownKey, totalCompensateOTLateEarlyLeaveMin / 60.0, 0);

                            }
                        }
                    }
                    else if (totalCompensateOTLateEarlyLeaveMin < 0)
                    {
                        EAttendanceFormula attendanceFormula = new EAttendanceFormula();
                        EPaymentCode paymentCode = new EPaymentCode();


                        attendanceFormula.AttendanceFormulaID = lastAttendancePlan.AttendancePlanLateFormula;
                        paymentCode.PaymentCodeID = lastAttendancePlan.AttendancePlanLatePayCodeID;

                        if (EAttendanceFormula.db.select(dbConn, attendanceFormula) && EPaymentCode.db.select(dbConn, paymentCode))
                        {
                            double hourlyRate = 0;
                            string hourlyRateRemark = string.Empty;     //  dummy, for debug only

                            hourlyRate = AttendanceFormulaProcess.GetHourlyRate(dbConn, attendanceFormula.AttendanceFormulaID, payGroup.PayGroupDefaultProrataFormula, EmpID, totalPeriodPayment, totalDailyPayment, totalHourlyPayment, payrollPeriod.PayPeriodAttnFr, payrollPeriod.PayPeriodAttnTo, numOfPeriodPerYear, PayrollGroupDateTo, false, 0, 1, 0, out hourlyRateRemark);

                            if (hourlyRate > 0)
                            {
                                HROne.Payroll.PaymentBreakDownKey lateDeductionBreakDownKey = new HROne.Payroll.PaymentBreakDownKey(paymentCode.PaymentCodeID, hourlyRate, payMethod, empAccID, false, 0, true);
                                //  totalCompensateOTLateEarlyLeaveMin is -ve
                                recurringPaymentBreakDownCollection.AddUnit(lateDeductionBreakDownKey, totalCompensateOTLateEarlyLeaveMin / 60.0, 0);

                            }
                        }
                    }
                }
                #endregion

                if (!skipBonusCalculation)
                {
                    string payMethod = string.Empty;
                    int empAccID = 0;
                    string bonusUnit = lastAttendancePlan.AttendancePlanBonusAmountUnit;
                    double bonusAmount = lastAttendancePlan.AttendancePlanBonusAmount;

                    ArrayList maxRecurringPaymentList = EEmpRecurringPayment.db.select(dbConn, HROne.Payroll.PayrollProcess.GetRecurringPaymentDBFilter(EmpID, PayrollGroupDateTo, "EmpRPAmount", false));
                    if (maxRecurringPaymentList.Count > 0)
                    {
                        EEmpRecurringPayment recurringPayment = (EEmpRecurringPayment)maxRecurringPaymentList[0];
                        payMethod = recurringPayment.EmpRPMethod;
                        if (payMethod.Equals("A"))
                            empAccID = recurringPayment.EmpAccID;
                    }
                    else
                        payMethod = "Q";

                    //  override setting by recurring payment
                    if (lastAttendancePlan.AttendancePlanUseBonusAmountByRecurringPayment)
                    {
                        DBFilter empBonusRPFilter = new DBFilter();
                        empBonusRPFilter.add(new Match("EmpRPEffFr", "<=", PayrollGroupDateTo));
                        {
                            OR orEmpRpEffToTerm = new OR();
                            orEmpRpEffToTerm.add(new Match("EmpRPEffTo", ">=", PayrollGroupDateTo));
                            orEmpRpEffToTerm.add(new NullTerm("EmpRPEffTo"));
                            empBonusRPFilter.add(orEmpRpEffToTerm);
                        }
                        {
                            empBonusRPFilter.add(new Match("EmpRPIsNonPayrollItem", true));
                            empBonusRPFilter.add(new Match("EmpID", EmpID));
                            empBonusRPFilter.add(new Match("PayCodeID", lastAttendancePlan.AttendancePlanBonusPayCodeID));
                            empBonusRPFilter.add("EmpRPAmount", false);
                        }
                        ArrayList empBonusRPList = EEmpRecurringPayment.db.select(dbConn, empBonusRPFilter);
                        if (empBonusRPList.Count > 0)
                        {
                            EEmpRecurringPayment empBonusRP = (EEmpRecurringPayment)empBonusRPList[0];
                            payMethod = empBonusRP.EmpRPMethod;
                            bonusAmount = empBonusRP.EmpRPAmount;
                            bonusUnit = empBonusRP.EmpRPUnit;
                            if (payMethod.Equals("A"))
                                empAccID = empBonusRP.EmpAccID;

                        }
                    }



                    if (lastAttendancePlan.AttendancePlanBonusMaxTotalLateCount >= totalBonusLateCount + (lastAttendancePlan.AttendancePlanBonusMaxTotalLateCountIncludeLunch ? totalBonusLunchLateCount : 0)
                        && lastAttendancePlan.AttendancePlanBonusMaxTotalLateMins >= totalBonusLateMins + (lastAttendancePlan.AttendancePlanBonusMaxTotalLateMinsIncludeLunch ? totalBonusLunchLateMins : 0)
                        && lastAttendancePlan.AttendancePlanBonusMaxTotalEarlyLeaveCount >= totalBonusEarlyLeaveCount + (lastAttendancePlan.AttendancePlanBonusMaxTotalEarlyLeaveCountIncludeLunch ? totalBonusLunchEarlyLeaveCount : 0)
                        && lastAttendancePlan.AttendancePlanBonusMaxTotalEarlyLeaveMins >= totalBonusEarlyLeaveMins + (lastAttendancePlan.AttendancePlanBonusMaxTotalEarlyLeaveMinsIncludeLunch ? totalBonusLunchEarlyLeaveMins : 0)
                        && lastAttendancePlan.AttendancePlanBonusMaxTotalAbsentCount >= absentCount
                        && lastAttendancePlan.AttendancePlanBonusMaxTotalInjuryLeave >= totalBonusInjuryLeave
                        && lastAttendancePlan.AttendancePlanBonusMaxTotalNonFullPayCasualLeave >= totalBonusNonFullPayLeave
                        && lastAttendancePlan.AttendancePlanBonusMaxTotalSLWithMedicalCertificate >= totalBonusSLWithMedicalCertificate
                        && lastAttendancePlan.AttendancePlanBonusMaxTotalSLWithoutMedicalCertificate >= totalBonusSLWithoutMedicalCertificate
                        && (lastDateOfAttendanceRecord >= payrollPeriod.PayPeriodAttnTo || (lastDateOfAttendanceRecord >= (empTermination == null ? payrollPeriod.PayPeriodAttnTo : empTermination.EmpTermLastDate)) && lastAttendancePlan.AttendancePlanTerminatedHasBonus)
                        //&& hasLastDateAttendance
                        && (bonusAmount > 0 || lastAttendancePlan.AttendancePlanBonusOTAmount > 0)
                        )
                    {

                        double amount = GetActualAttendanceBonus(EmpID, payrollPeriod.PayPeriodAttnFr, payrollPeriod.PayPeriodAttnTo, empTermination != null ? empTermination.EmpTermLastDate : new DateTime(), lastAttendancePlan, bonusAmount, bonusUnit, totalWorkDay, totalWorkHour + (double)(totalOvertimeMins - totalLateMins) / 60.0);

                        //if (bonusUnit.Equals("P"))
                        //{
                        //    amount = bonusAmount;
                        //    double numOfDaysPerCycle = ((TimeSpan)payrollPeriod.PayPeriodAttnTo.Subtract(payrollPeriod.PayPeriodAttnFr)).TotalDays + 1;
                        //    double numOfDaysCount = numOfDaysPerCycle;
                        //    if (lastAttendancePlan.AttendancePlanProrataBonusforNewJoin)
                        //    {
                        //        EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                        //        empInfo.EmpID = EmpID;
                        //        if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                        //        {
                        //            if (empInfo.EmpDateOfJoin > payrollPeriod.PayPeriodAttnFr && empInfo.EmpDateOfJoin <= payrollPeriod.PayPeriodAttnTo)
                        //                numOfDaysCount -= ((TimeSpan)empInfo.EmpDateOfJoin.Subtract(payrollPeriod.PayPeriodAttnFr)).TotalDays;
                        //        }
                        //    }
                        //    if (lastAttendancePlan.AttendancePlanProrataBonusforTerminated)
                        //    {
                        //        if (empTermination != null)
                        //        {
                        //            if (empTermination.EmpTermLastDate >= payrollPeriod.PayPeriodAttnFr && empTermination.EmpTermLastDate < payrollPeriod.PayPeriodAttnTo)
                        //                numOfDaysCount -= ((TimeSpan)payrollPeriod.PayPeriodAttnTo.Subtract(empTermination.EmpTermLastDate)).TotalDays;
                        //        }
                        //    }
                        //    if (numOfDaysCount != numOfDaysPerCycle)
                        //        amount = (bonusAmount * numOfDaysCount) / numOfDaysPerCycle;

                        //    if (!lastAttendancePlan.AttendancePlanBonusOTAmount.Equals(0))
                        //        amount += (totalOvertimeMins * lastAttendancePlan.AttendancePlanBonusOTAmount) / 60.0;
                        //}
                        //else if (bonusUnit.Equals("D"))
                        //{
                        //    amount = bonusAmount * totalWorkDay;
                        //    if (!lastAttendancePlan.AttendancePlanBonusOTAmount.Equals(0))
                        //        amount += (totalOvertimeMins * lastAttendancePlan.AttendancePlanBonusOTAmount) / 60.0;
                        //}
                        //else if (bonusUnit.Equals("H"))
                        //    amount = bonusAmount * (totalWorkHour + (double)(totalOvertimeMins - totalLateMins) / 60.0);// - totalEarlyLeaveMins) / 60);

                        if (!lastAttendancePlan.AttendancePlanBonusOTAmount.Equals(0))
                            amount += (totalOvertimeMins * lastAttendancePlan.AttendancePlanBonusOTAmount) / 60.0;

                        EPaymentCode paymentCode = new EPaymentCode();
                        paymentCode.PaymentCodeID = lastAttendancePlan.AttendancePlanBonusPayCodeID;
                        if (amount > 0 && EPaymentCode.db.select(dbConn, paymentCode))
                        {
                            HROne.Payroll.PaymentBreakDownKey bonusBreakDownKey = new HROne.Payroll.PaymentBreakDownKey(paymentCode.PaymentCodeID, amount, payMethod, empAccID, false, 0, false);
                            recurringPaymentBreakDownCollection.AddUnit(bonusBreakDownKey, 1, 0);

                            //EPaymentRecord paymentRecord = new EPaymentRecord();
                            //paymentRecord.CurrencyID = ExchangeCurrency.DefaultCurrency();
                            //paymentRecord.PayRecMethod = payMethod;
                            //paymentRecord.EmpAccID = empAccID;
                            //paymentRecord.PaymentCodeID = paymentCode.PaymentCodeID;
                            //paymentRecord.PayRecCalAmount = amount;
                            //paymentRecord.PayRecActAmount = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(paymentRecord.PayRecCalAmount, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), ExchangeCurrency.DefaultCurrencyDecimalPlaces());
                            ////paymentRecord.PayRecRemark = hourlyRate + " * " + actualMins + " / 60";
                            //paymentRecord.PayRecType = PaymentRecordType.PAYRECORDTYPE_RECURRING;
                            //paymentRecord.PayRecStatus = PaymentRecordStatus.PAYRECORDSTATUS_ACTIVE;
                            //attendancePaymentList.Add(paymentRecord);
                        }
                    }
                    else if (lastAttendancePlan.AttendancePlanBonusPartialPaidMaxTotalLateCount >= totalBonusLateCount + (lastAttendancePlan.AttendancePlanBonusPartialPaidMaxTotalLateCountIncludeLunch ? totalBonusLunchLateCount : 0)
                        && lastAttendancePlan.AttendancePlanBonusPartialPaidMaxTotalLateMins >= totalBonusLateMins + (lastAttendancePlan.AttendancePlanBonusPartialPaidMaxTotalLateMinsIncludeLunch ? totalBonusLunchLateMins : 0)
                        && lastAttendancePlan.AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveCount >= totalBonusEarlyLeaveCount + (lastAttendancePlan.AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveCountIncludeLunch ? totalBonusLunchEarlyLeaveCount : 0)
                        && lastAttendancePlan.AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveMins >= totalBonusEarlyLeaveMins + (lastAttendancePlan.AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveMinsIncludeLunch ? totalBonusLunchEarlyLeaveMins : 0)
                        && lastAttendancePlan.AttendancePlanBonusPartialPaidMaxTotalAbsentCount >= absentCount
                        && lastAttendancePlan.AttendancePlanBonusPartialPaidMaxTotalInjuryLeave >= totalBonusInjuryLeave
                        && lastAttendancePlan.AttendancePlanBonusPartialPaidMaxTotalNonFullPayCasualLeave >= totalBonusNonFullPayLeave
                        && lastAttendancePlan.AttendancePlanBonusPartialPaidMaxTotalSLWithMedicalCertificate >= totalBonusSLWithMedicalCertificate
                        && lastAttendancePlan.AttendancePlanBonusPartialPaidMaxTotalSLWithoutMedicalCertificate >= totalBonusSLWithoutMedicalCertificate
                        && (lastDateOfAttendanceRecord >= payrollPeriod.PayPeriodAttnTo || (lastDateOfAttendanceRecord >= (empTermination == null ? payrollPeriod.PayPeriodAttnTo : empTermination.EmpTermLastDate)) && lastAttendancePlan.AttendancePlanTerminatedHasBonus)
                        //&& hasLastDateAttendance
                        && (bonusAmount > 0 || lastAttendancePlan.AttendancePlanBonusOTAmount > 0)
                        && lastAttendancePlan.AttendancePlanBonusPartialPaidPercent > 0
                        )
                    {

                        double amount = GetActualAttendanceBonus(EmpID, payrollPeriod.PayPeriodAttnFr, payrollPeriod.PayPeriodAttnTo, empTermination != null ? empTermination.EmpTermLastDate : new DateTime(), lastAttendancePlan, bonusAmount, bonusUnit, totalWorkDay, totalWorkHour + (double)(totalOvertimeMins - totalLateMins) / 60.0);

                        //if (bonusUnit.Equals("P"))
                        //{
                        //    amount = bonusAmount;
                        //    double numOfDaysPerCycle = ((TimeSpan)payrollPeriod.PayPeriodAttnTo.Subtract(payrollPeriod.PayPeriodAttnFr)).TotalDays + 1;
                        //    double numOfDaysCount = numOfDaysPerCycle;
                        //    if (lastAttendancePlan.AttendancePlanProrataBonusforNewJoin)
                        //    {
                        //        EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                        //        empInfo.EmpID = EmpID;
                        //        if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                        //        {
                        //            if (empInfo.EmpDateOfJoin > payrollPeriod.PayPeriodAttnFr && empInfo.EmpDateOfJoin <= payrollPeriod.PayPeriodAttnTo)
                        //                numOfDaysCount -= ((TimeSpan)empInfo.EmpDateOfJoin.Subtract(payrollPeriod.PayPeriodAttnFr)).TotalDays;
                        //        }
                        //    }
                        //    if (lastAttendancePlan.AttendancePlanProrataBonusforTerminated)
                        //    {
                        //        if (empTermination != null)
                        //        {
                        //            if (empTermination.EmpTermLastDate >= payrollPeriod.PayPeriodAttnFr && empTermination.EmpTermLastDate < payrollPeriod.PayPeriodAttnTo)
                        //                numOfDaysCount -= ((TimeSpan)payrollPeriod.PayPeriodAttnTo.Subtract(empTermination.EmpTermLastDate)).TotalDays;
                        //        }
                        //    }
                        //    if (numOfDaysCount != numOfDaysPerCycle)
                        //        amount = (bonusAmount * numOfDaysCount) / numOfDaysPerCycle;

                        //    if (!lastAttendancePlan.AttendancePlanBonusOTAmount.Equals(0))
                        //        amount += (totalOvertimeMins * lastAttendancePlan.AttendancePlanBonusOTAmount) / 60.0;
                        //}
                        //else if (bonusUnit.Equals("D"))
                        //{
                        //    amount = bonusAmount * totalWorkDay;
                        //    if (!lastAttendancePlan.AttendancePlanBonusOTAmount.Equals(0))
                        //        amount += (totalOvertimeMins * lastAttendancePlan.AttendancePlanBonusOTAmount) / 60.0;
                        //}
                        //else if (bonusUnit.Equals("H"))
                        //    amount = bonusAmount * (totalWorkHour + (double)(totalOvertimeMins - totalLateMins) / 60.0);// - totalEarlyLeaveMins) / 60);

                        if (!lastAttendancePlan.AttendancePlanBonusOTAmount.Equals(0))
                            amount += (totalOvertimeMins * lastAttendancePlan.AttendancePlanBonusOTAmount) / 60.0;

                        EPaymentCode paymentCode = new EPaymentCode();
                        paymentCode.PaymentCodeID = lastAttendancePlan.AttendancePlanBonusPayCodeID;
                        if (amount > 0 && EPaymentCode.db.select(dbConn, paymentCode))
                        {
                            HROne.Payroll.PaymentBreakDownKey bonusBreakDownKey = new HROne.Payroll.PaymentBreakDownKey(paymentCode.PaymentCodeID, amount, payMethod, empAccID, false, 0, false);
                            recurringPaymentBreakDownCollection.AddUnit(bonusBreakDownKey, lastAttendancePlan.AttendancePlanBonusPartialPaidPercent / 100, 0);

                            //EPaymentRecord paymentRecord = new EPaymentRecord();
                            //paymentRecord.CurrencyID = ExchangeCurrency.DefaultCurrency();
                            //paymentRecord.PayRecMethod = payMethod;
                            //paymentRecord.EmpAccID = empAccID;
                            //paymentRecord.PaymentCodeID = paymentCode.PaymentCodeID;
                            //paymentRecord.PayRecCalAmount = amount * lastAttendancePlan.AttendancePlanBonusPartialPaidPercent / 100;
                            //paymentRecord.PayRecActAmount = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(paymentRecord.PayRecCalAmount, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), ExchangeCurrency.DefaultCurrencyDecimalPlaces());
                            ////paymentRecord.PayRecRemark = hourlyRate + " * " + actualMins + " / 60";
                            //paymentRecord.PayRecType = PaymentRecordType.PAYRECORDTYPE_RECURRING;
                            //paymentRecord.PayRecStatus = PaymentRecordStatus.PAYRECORDSTATUS_ACTIVE;
                            //attendancePaymentList.Add(paymentRecord);
                        }
                    }


                }
            }
            return new ArrayList(recurringPaymentBreakDownCollection.GeneratePaymentRecordList());

        }

        protected virtual double GetActualAttendanceBonus(int EmpID, DateTime periodFrom, DateTime periodTo, DateTime lastEmploymentDate, EAttendancePlan lastAttendancePlan, double bonusAmount, string bonusUnit, double totalWorkDay, double totalWorkHourForBonus)
        {
            double amount = 0;
            if (bonusUnit.Equals("P"))
            {
                amount = bonusAmount;
                double numOfDaysPerCycle = ((TimeSpan)periodTo.Subtract(periodFrom)).TotalDays + 1;
                double numOfDaysCount = numOfDaysPerCycle;
                if (lastAttendancePlan.AttendancePlanProrataBonusforNewJoin)
                {
                    EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                    empInfo.EmpID = EmpID;
                    if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                    {
                        if (empInfo.EmpDateOfJoin > periodFrom && empInfo.EmpDateOfJoin <= periodTo)
                            numOfDaysCount -= ((TimeSpan)empInfo.EmpDateOfJoin.Subtract(periodFrom)).TotalDays;
                    }
                }
                if (lastAttendancePlan.AttendancePlanProrataBonusforTerminated)
                {
                    if (lastEmploymentDate.Ticks>0)
                    {
                        if (lastEmploymentDate >= periodFrom && lastEmploymentDate < periodTo)
                            numOfDaysCount -= ((TimeSpan)periodTo.Subtract(lastEmploymentDate)).TotalDays;
                    }
                }
                if (numOfDaysCount != numOfDaysPerCycle)
                    amount = (bonusAmount * numOfDaysCount) / numOfDaysPerCycle;

            }
            else if (bonusUnit.Equals("D"))
            {
                amount = bonusAmount * totalWorkDay;
            }
            else if (bonusUnit.Equals("H"))
                amount = bonusAmount * totalWorkHourForBonus;// - totalEarlyLeaveMins) / 60);

            return amount;
        }

        //  Do override daily payment one of the recurring payments ONLY
        bool isDailyPaymentOverrided = false;

        protected virtual void CreateEmployeeRecurringPaymentListAttendanceResult(int EmpID, EAttendanceRecord attendanceRecord, EAttendancePlan attendancePlan, EWorkHourPattern workPatten, EPayrollGroup payGroup, EPayrollPeriod payrollPeriod, ERosterCode rosterCode, int RosterCostCenterID, int costCenterIDByLocation1, int costCenterIDByLocation2, bool hasDailyPaymentOverride, double dailyOverrideAmount, PaymentBreakDownCollection recurringPaymentBreakDownCollection)
        {
            #region Recurring Payment Calculation

            //  Skip calculation RP if the day is worked as overtime.
            if (rosterCode.RosterCodeType!=ERosterCode.ROSTERTYPE_CODE_RESTDAY && attendanceRecord.GetAttendanceRecordExtendData(EAttendanceRecord.FIELD_EXTENDDATA_WORK_AS_OVERTIME).Equals("Yes", StringComparison.CurrentCultureIgnoreCase))
            {
                return;
            }

            //reset override flag to false
            isDailyPaymentOverrided = false;

            DBFilter recurringPaymentFilter = HROne.Payroll.PayrollProcess.GetRecurringPaymentDBFilter(EmpID, attendanceRecord.AttendanceRecordDate, "EmpRPAmount", false);

            ArrayList recurringPaymentList = EEmpRecurringPayment.db.select(dbConn, recurringPaymentFilter);
            foreach (EEmpRecurringPayment empRP in recurringPaymentList)
            {
                int RPCostCenterID = empRP.CostCenterID;

                //  Check if Cost Center for Recurring Payment exists
                if (RPCostCenterID > 0)
                {
                    ECostCenter checkRPCostCenter = new ECostCenter();
                    checkRPCostCenter.CostCenterID = RPCostCenterID;
                    if (!ECostCenter.db.select(dbConn, checkRPCostCenter))
                        RPCostCenterID = 0;
                }

                int RPcostCenterIDLocation1 = RPCostCenterID;
                int RPcostCenterIDLocation2 = RPCostCenterID;
                if (RPCostCenterID > 0)
                {
                    //  Override to Cost Center for Roster if different
                    if (RPCostCenterID != RosterCostCenterID && RosterCostCenterID > 0)
                        RPCostCenterID = RosterCostCenterID;
                    if (RPcostCenterIDLocation1 != costCenterIDByLocation1 && costCenterIDByLocation1 > 0)
                        RPcostCenterIDLocation1 = costCenterIDByLocation1;
                    if (RPcostCenterIDLocation2 != costCenterIDByLocation2 && costCenterIDByLocation2 > 0)
                        RPcostCenterIDLocation2 = costCenterIDByLocation2;
                }
                else
                {
                    //  Override to Cost Center for Roster if not same as employee detail
                    if (!IsUseEmpCostCenter(EmpID, attendanceRecord.AttendanceRecordDate, RosterCostCenterID))
                        RPCostCenterID = RosterCostCenterID;
                    if (!IsUseEmpCostCenter(EmpID, attendanceRecord.AttendanceRecordDate, costCenterIDByLocation1))
                        RPcostCenterIDLocation1 = costCenterIDByLocation1;
                    if (!IsUseEmpCostCenter(EmpID, attendanceRecord.AttendanceRecordDate, costCenterIDByLocation2))
                        RPcostCenterIDLocation2 = costCenterIDByLocation2;
                }

                AddRecurringPaymentAttendanceResult(EmpID, empRP, attendanceRecord, attendancePlan, workPatten, payGroup, payrollPeriod, rosterCode, RPCostCenterID, RPcostCenterIDLocation1, RPcostCenterIDLocation2, hasDailyPaymentOverride, dailyOverrideAmount, recurringPaymentBreakDownCollection);

            }

            #endregion
        }

        protected virtual void AddRecurringPaymentAttendanceResult(int EmpID, EEmpRecurringPayment empRP, EAttendanceRecord attendanceRecord, EAttendancePlan attendancePlan, EWorkHourPattern workPatten, EPayrollGroup payGroup, EPayrollPeriod payrollPeriod, ERosterCode rosterCode, int RPCostCenterID, int RPcostCenterIDLocation1, int RPcostCenterIDLocation2, bool hasDailyPaymentOverride, double dailyOverrideAmount, PaymentBreakDownCollection recurringPaymentBreakDownCollection)
        {
            double RPRate = 0;
            double RPUnit = 0;
            string RPRemark= string.Empty;
            bool RPIsRestDayPayment = false;
            double LunchRPRate = 0;
            double LunchRPUnit = 0;
            
            EPaymentCode overridePaymentCode = new EPaymentCode();
            overridePaymentCode.PaymentCodeID = empRP.PayCodeID;
            if (EPaymentCode.db.select(dbConn, overridePaymentCode))
            {
                //  Only Basic Salary with prorata option will be override
                if (hasDailyPaymentOverride && overridePaymentCode.PaymentTypeID.Equals(EPaymentType.SystemPaymentType.BasicSalaryPaymentType(dbConn).PaymentTypeID) && overridePaymentCode.PaymentCodeIsProrata)
                {
                    if (empRP.EmpRPUnit.Equals("P") && !empRP.EmpRPUnitPeriodAsDaily)
                    {
                        EEmpPositionInfo empOverrideDailyPaymentPos = AppUtils.GetLastPositionInfo(dbConn, attendanceRecord.AttendanceRecordDate, EmpID);

                        if (empOverrideDailyPaymentPos != null)
                        {
                            EPayrollGroup payrollGroup = new EPayrollGroup();
                            payrollGroup.PayGroupID = empOverrideDailyPaymentPos.PayGroupID;
                            if (EPayrollGroup.db.select(dbConn, payrollGroup))
                            {
                                string deductRPRemark;
                                double deductRPRate = 0;
                                double deductRPUnit = 0;

                                bool dummyDAW = false;
                                deductRPRate = -HROne.Payroll.PayrollFormula.DailyProrataCaluclation(dbConn, payrollGroup.PayGroupNewJoinFormula, payrollGroup.PayGroupDefaultProrataFormula, EmpID, empRP.EmpRPAmount, payrollPeriod.PayPeriodAttnFr, payrollPeriod.PayPeriodAttnTo, payrollGroup.NumOfPeriodPerYear(), attendanceRecord.AttendanceRecordDate, out deductRPRemark, out dummyDAW);
                                deductRPUnit = attendanceRecord.AttendanceRecordActualWorkingDay;

                                if (deductRPRate != 0 && deductRPUnit != 0)
                                {
                                    double dayAdjust = 0;
                                    EPaymentCode paymentCode = new EPaymentCode();
                                    paymentCode.PaymentCodeID = empRP.PayCodeID;
                                    if (EPaymentCode.db.select(dbConn, paymentCode))
                                        if (paymentCode.PaymentTypeID.Equals(EPaymentType.SystemPaymentType.BasicSalaryPaymentType(dbConn).PaymentTypeID))
                                            dayAdjust = -attendanceRecord.AttendanceRecordActualWorkingDay;


                                    HROne.Payroll.PaymentBreakDownKey key = new HROne.Payroll.PaymentBreakDownKey(empRP.PayCodeID, deductRPRate, deductRPRemark, 1, empRP.EmpRPMethod, empRP.EmpAccID, false, RPCostCenterID, true);
                                    recurringPaymentBreakDownCollection.AddUnit(key, deductRPUnit, dayAdjust);


                                }

                            }
                        }
                    }
                    if (!isDailyPaymentOverrided)
                    {
                        //  Treat current EmpRP as Daily payment
                        //  Remove EmpRPID to prevent any update function
                        empRP.EmpRPID = 0;
                        empRP.EmpRPUnit = "D";
                        empRP.EmpRPAmount = dailyOverrideAmount;



                        //double RPRate = dailyOverrideAmount;
                        //double RPUnit = 1;
                        //if (RPRate != 0 && RPUnit != 0)
                        //{
                        //    double dayAdjust = 0;

                        //    EPaymentCode paymentCode = new EPaymentCode();
                        //    paymentCode.PaymentCodeID = empRP.PayCodeID;
                        //    if (EPaymentCode.db.select(dbConn, paymentCode))
                        //        if (paymentCode.PaymentTypeID.Equals(EPaymentType.SystemPaymentType.BasicSalaryPaymentType(dbConn).PaymentTypeID))
                        //            dayAdjust = rosterCode.RosterCodeWorkingDayUnit;


                        //    HROne.Payroll.PaymentBreakDownKey key = new HROne.Payroll.PaymentBreakDownKey(empRP.PayCodeID, RPRate, empRP.EmpRPMethod, empRP.EmpAccID, false, RPCostCenterID, true);
                        //    recurringPaymentBreakDownCollection.AddUnit(key, RPUnit, dayAdjust);
                        //}
                        isDailyPaymentOverrided = true;
                    }
                    else
                    {
                        empRP.EmpRPID = 0;
                        empRP.EmpRPUnit = string.Empty;

                    }
                    //return;

                }
            }
            if (empRP.EmpRPUnit.Equals("P", StringComparison.CurrentCultureIgnoreCase) && !empRP.EmpRPUnitPeriodAsDaily)
            {
                //  Absent Calculation
                if (attendanceRecord.AttendanceRecordIsAbsent && attendancePlan.AttendancePlanAbsentProrataPayFormID > 0)
                {
                    string remark = string.Empty;
                    bool isDAW = false;
                    double dailyAmount = HROne.Payroll.PayrollFormula.DailyProrataCaluclation(dbConn, attendancePlan.AttendancePlanAbsentProrataPayFormID, payGroup.PayGroupDefaultProrataFormula, EmpID, empRP.EmpRPAmount, payrollPeriod.PayPeriodFr, payrollPeriod.PayPeriodTo, payGroup.NumOfPeriodPerYear(), attendanceRecord.AttendanceRecordDate, out remark, out isDAW);
                    if (isDAW)
                        dailyAmount = HROne.Payroll.PayrollFormula.DailyProrataCaluclation(dbConn, "SYS001", payGroup.PayGroupDefaultProrataFormula, EmpID, empRP.EmpRPAmount, payrollPeriod.PayPeriodFr, payrollPeriod.PayPeriodTo, payGroup.NumOfPeriodPerYear(), attendanceRecord.AttendanceRecordDate, out remark, out isDAW);

                    dailyAmount = -dailyAmount;
                    HROne.Payroll.PaymentBreakDownKey monthlyRPDeductionBreakDownKey = new HROne.Payroll.PaymentBreakDownKey(empRP.PayCodeID, dailyAmount, remark, 1, empRP.EmpRPMethod, empRP.EmpAccID, false, empRP.CostCenterID, true);
                    recurringPaymentBreakDownCollection.AddUnit(monthlyRPDeductionBreakDownKey, 1, 0);

                    EPaymentCode paymentCode = new EPaymentCode();
                    paymentCode.PaymentCodeID = empRP.PayCodeID;
                    if (EPaymentCode.db.select(dbConn, paymentCode))
                        if (paymentCode.PaymentTypeID.Equals(EPaymentType.SystemPaymentType.BasicSalaryPaymentType(dbConn).PaymentTypeID))
                        {
                            if (attendanceRecord.AttendanceRecordActualWorkingDay > rosterCode.RosterCodeWorkingDayUnit)
                            {
                                // usually if attendanceRecord.AttendanceRecordActualWorkingDay >1 override the working day
                                recurringPaymentBreakDownCollection.AddUnit(monthlyRPDeductionBreakDownKey, 0, -attendanceRecord.AttendanceRecordActualWorkingDay);
                            }
                            else
                                recurringPaymentBreakDownCollection.AddUnit(monthlyRPDeductionBreakDownKey, 0, -rosterCode.RosterCodeWorkingDayUnit);

                        }
                }
                else
                {
                    //  check if override cost center is required
                    if (RPcostCenterIDLocation1 != empRP.CostCenterID || RPcostCenterIDLocation2 != empRP.CostCenterID)
                    {
                        bool isDAW = false;
                        string remark = string.Empty;

                        double dailyAmount = HROne.Payroll.PayrollFormula.DailyProrataCaluclation(dbConn, attendancePlan.AttendancePlanAbsentProrataPayFormID, payGroup.PayGroupDefaultProrataFormula, EmpID, empRP.EmpRPAmount, payrollPeriod.PayPeriodFr, payrollPeriod.PayPeriodTo, payGroup.NumOfPeriodPerYear(), attendanceRecord.AttendanceRecordDate, out remark, out isDAW);
                        if (isDAW)
                            dailyAmount = HROne.Payroll.PayrollFormula.DailyProrataCaluclation(dbConn, "SYS001", payGroup.PayGroupDefaultProrataFormula, EmpID, empRP.EmpRPAmount, payrollPeriod.PayPeriodFr, payrollPeriod.PayPeriodTo, payGroup.NumOfPeriodPerYear(), attendanceRecord.AttendanceRecordDate, out remark, out isDAW);

                        double deductionDailyAmount = -dailyAmount;

                        HROne.Payroll.PaymentBreakDownKey monthlyRPDeductionBreakDownKey = new HROne.Payroll.PaymentBreakDownKey(empRP.PayCodeID, deductionDailyAmount, empRP.EmpRPMethod, empRP.EmpAccID, false, empRP.CostCenterID, false);

                        if (RPcostCenterIDLocation1 != empRP.CostCenterID)
                        {
                            //  Generate Deduction Payment for default cost center
                            HROne.Payroll.PaymentBreakDown firstPartBreakDownDeduction = recurringPaymentBreakDownCollection.AddUnit(monthlyRPDeductionBreakDownKey, attendanceRecord.AttendanceRecordActualWorkingDay / 2, 0);
                            //  Generate Addition Payment for actual cost center

                            HROne.Payroll.PaymentBreakDownKey firstPartRPAdditionBreakDownKey = new HROne.Payroll.PaymentBreakDownKey(empRP.PayCodeID, dailyAmount, empRP.EmpRPMethod, empRP.EmpAccID, false, RPcostCenterIDLocation1, false);
                            HROne.Payroll.PaymentBreakDown firstPartBreakDownAddition = recurringPaymentBreakDownCollection.AddUnit(firstPartRPAdditionBreakDownKey, attendanceRecord.AttendanceRecordActualWorkingDay / 2, 0);


                            EPaymentCode paymentCode = new EPaymentCode();
                            paymentCode.PaymentCodeID = empRP.PayCodeID;
                            if (EPaymentCode.db.select(dbConn, paymentCode))
                                if (paymentCode.PaymentTypeID.Equals(EPaymentType.SystemPaymentType.BasicSalaryPaymentType(dbConn).PaymentTypeID))
                                {
                                    if (attendanceRecord.AttendanceRecordActualWorkingDay > rosterCode.RosterCodeWorkingDayUnit)
                                    {
                                        // usually if attendanceRecord.AttendanceRecordActualWorkingDay >1 override the working day
                                        firstPartBreakDownDeduction.DayAdjusts -= attendanceRecord.AttendanceRecordActualWorkingDay / 2;
                                        firstPartBreakDownAddition.DayAdjusts += attendanceRecord.AttendanceRecordActualWorkingDay / 2;
                                    }
                                    else
                                    {
                                        firstPartBreakDownDeduction.DayAdjusts -= rosterCode.RosterCodeWorkingDayUnit / 2;
                                        firstPartBreakDownAddition.DayAdjusts += rosterCode.RosterCodeWorkingDayUnit / 2;
                                    }
                                }

                        }

                        if (RPcostCenterIDLocation2 != empRP.CostCenterID)
                        {
                            //  Generate Deduction Payment for default cost center
                            HROne.Payroll.PaymentBreakDown secondPartBreakDownDeduction = recurringPaymentBreakDownCollection.AddUnit(monthlyRPDeductionBreakDownKey, attendanceRecord.AttendanceRecordActualWorkingDay / 2, 0);
                            //  Generate Addition Payment for actual cost center

                            HROne.Payroll.PaymentBreakDownKey secondPartRPAdditionBreakDownKey = new HROne.Payroll.PaymentBreakDownKey(empRP.PayCodeID, dailyAmount, empRP.EmpRPMethod, empRP.EmpAccID, false, RPcostCenterIDLocation2, false);
                            HROne.Payroll.PaymentBreakDown secondPartBreakDownAddition = recurringPaymentBreakDownCollection.AddUnit(secondPartRPAdditionBreakDownKey, attendanceRecord.AttendanceRecordActualWorkingDay / 2, 0);


                            EPaymentCode paymentCode = new EPaymentCode();
                            paymentCode.PaymentCodeID = empRP.PayCodeID;
                            if (EPaymentCode.db.select(dbConn, paymentCode))
                                if (paymentCode.PaymentTypeID.Equals(EPaymentType.SystemPaymentType.BasicSalaryPaymentType(dbConn).PaymentTypeID))
                                {
                                    if (attendanceRecord.AttendanceRecordActualWorkingDay > rosterCode.RosterCodeWorkingDayUnit)
                                    {
                                        // usually if attendanceRecord.AttendanceRecordActualWorkingDay >1 override the working day
                                        secondPartBreakDownDeduction.DayAdjusts -= attendanceRecord.AttendanceRecordActualWorkingDay / 2;
                                        secondPartBreakDownAddition.DayAdjusts += attendanceRecord.AttendanceRecordActualWorkingDay / 2;
                                    }
                                    else
                                    {
                                        secondPartBreakDownDeduction.DayAdjusts -= rosterCode.RosterCodeWorkingDayUnit / 2;
                                        secondPartBreakDownAddition.DayAdjusts += rosterCode.RosterCodeWorkingDayUnit / 2;
                                    }
                                }

                        }
                    }
                }
            }
            else if (empRP.EmpRPUnit.Equals("D", StringComparison.CurrentCultureIgnoreCase) || (empRP.EmpRPUnit.Equals("P", StringComparison.CurrentCultureIgnoreCase) && empRP.EmpRPUnitPeriodAsDaily))
            {
                RPRate = empRP.EmpRPAmount;
                RPUnit = attendanceRecord.AttendanceRecordActualWorkingDay;
                if (!empRP.EmpRPUnit.Equals("D", StringComparison.CurrentCultureIgnoreCase))
                {
                    bool dummyISDAW;
                    RPRate = PayrollFormula.DailyProrataCaluclation(dbConn, empRP.EmpRPUnitPeriodAsDailyPayFormID, payGroup.PayGroupDefaultProrataFormula, EmpID, RPRate, payrollPeriod.PayPeriodFr, payrollPeriod.PayPeriodTo, payGroup.NumOfPeriodPerYear(), attendanceRecord.AttendanceRecordDate, 0, out RPRemark, out dummyISDAW);
                }
                if (!string.IsNullOrEmpty(rosterCode.RosterCodeType))
                    if (rosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_RESTDAY))
                    {
                        if (payGroup.PayGroupRestDayHasWage)
                        {
                            RPIsRestDayPayment = true;
                            RPUnit = 1;
                        }
                    }
                    else
                    {
                        if (attendanceRecord.AttendanceRecordWorkOnRestDay)
                            AddRecurringPaymentResult(empRP, attendanceRecord, rosterCode, RPRate, 1, RPRemark, true, 0, 0, RPCostCenterID, RPcostCenterIDLocation1, RPcostCenterIDLocation2, recurringPaymentBreakDownCollection);
                    }
                if (!RPIsRestDayPayment && attendanceRecord.AttendanceRecordActualLunchTimeMins > 0 && payGroup.PayGroupLunchTimeHasWage)
                {
                    if (rosterCode.RosterCodeID > 0)
                    {
                        LunchRPRate = RPRate * rosterCode.RosterCodeWorkingDayUnit / (rosterCode.RosterCodeDailyWorkingHour + Convert.ToDouble(rosterCode.GetDefaultLunchTimeMins()) / 60.0);
                    }
                    else
                    {
                        if (workPatten != null)
                        {
                            double totalWorkHourPattenDuration = workPatten.WorkHourPatternContractWorkHoursPerDay + workPatten.WorkHourPatternContractLunchTimeHoursPerDay;
                            if (totalWorkHourPattenDuration > 0)
                                LunchRPRate = RPRate / (totalWorkHourPattenDuration);

                        }
                    }
                    LunchRPUnit = Convert.ToDouble(attendanceRecord.AttendanceRecordActualLunchTimeMins) / 60.0;
                }
            }
            else if (empRP.EmpRPUnit.Equals("H"))
            {
                RPRate = empRP.EmpRPAmount;
                if (rosterCode.RosterCodeIsOverrideHourlyPayment)
                    RPRate = rosterCode.RosterCodeOverrideHoulyAmount;
                RPUnit = attendanceRecord.AttendanceRecordActualWorkingHour;
                RPRemark = RPRate.ToString("$#,##0.00##");  //require to same format as GetHourlyRate function
                if (attendanceRecord.AttendanceRecordActualLunchTimeMins > 0 && payGroup.PayGroupLunchTimeHasWage)
                {
                    LunchRPRate = RPRate;
                    LunchRPUnit = Convert.ToDouble(attendanceRecord.AttendanceRecordActualLunchTimeMins) / 60.0;
                }
            }

            AddRecurringPaymentResult(empRP, attendanceRecord, rosterCode, RPRate, RPUnit, RPRemark, RPIsRestDayPayment, LunchRPRate, LunchRPUnit, RPCostCenterID, RPcostCenterIDLocation1, RPcostCenterIDLocation2, recurringPaymentBreakDownCollection);
        }

        protected virtual void AddRecurringPaymentResult(EEmpRecurringPayment empRP, EAttendanceRecord attendanceRecord, ERosterCode rosterCode, double RPRate, double RPUnit, string RPRemark, bool RPIsRestDayPayment, double LunchRPRate, double LunchRPUnit, int RPCostCenterID, int RPcostCenterIDLocation1, int RPcostCenterIDLocation2, PaymentBreakDownCollection recurringPaymentBreakDownCollection)
        {
            //  Force add Recurring payment even though the unit = 0
            if (RPRate != 0)// && RPUnit != 0)
            {
                //  First Part 
                HROne.Payroll.PaymentBreakDownKey firstPartBreakDownKey = new HROne.Payroll.PaymentBreakDownKey(empRP.PayCodeID, RPRate, RPRemark, 1, empRP.EmpRPMethod, empRP.EmpAccID, RPIsRestDayPayment, RPcostCenterIDLocation1, true);
                HROne.Payroll.PaymentBreakDown firstPartBreakDown = recurringPaymentBreakDownCollection.AddUnit(firstPartBreakDownKey, RPUnit / 2, 0);

                //  Second Part 
                HROne.Payroll.PaymentBreakDownKey secondPartbreakDownKey = new HROne.Payroll.PaymentBreakDownKey(empRP.PayCodeID, RPRate, RPRemark, 1, empRP.EmpRPMethod, empRP.EmpAccID, RPIsRestDayPayment, RPcostCenterIDLocation2, true);
                HROne.Payroll.PaymentBreakDown secondPartBreakDown = recurringPaymentBreakDownCollection.AddUnit(secondPartbreakDownKey, RPUnit / 2, 0);

                if (RPUnit != 0)
                {
                    //  Adjust the days ONLY if RPUnit <> 0
                    EPaymentCode paymentCode = new EPaymentCode();
                    paymentCode.PaymentCodeID = empRP.PayCodeID;
                    if (EPaymentCode.db.select(dbConn, paymentCode))
                        if (paymentCode.PaymentTypeID.Equals(EPaymentType.SystemPaymentType.BasicSalaryPaymentType(dbConn).PaymentTypeID))
                        {
                            if (attendanceRecord.AttendanceRecordActualWorkingDay > rosterCode.RosterCodeWorkingDayUnit)
                            {
                                // usually if attendanceRecord.AttendanceRecordActualWorkingDay >1 override the working day
                                firstPartBreakDown.DayAdjusts += attendanceRecord.AttendanceRecordActualWorkingDay / 2;
                                secondPartBreakDown.DayAdjusts += attendanceRecord.AttendanceRecordActualWorkingDay / 2;
                            }
                            else
                            {
                                firstPartBreakDown.DayAdjusts += rosterCode.RosterCodeWorkingDayUnit / 2;
                                secondPartBreakDown.DayAdjusts += rosterCode.RosterCodeWorkingDayUnit / 2;
                            }
                        }

                }
            }
            if (LunchRPRate != 0 && LunchRPUnit != 0)
            {
                if (empRP.EmpRPUnit.Equals("D", StringComparison.CurrentCultureIgnoreCase) || (empRP.EmpRPUnit.Equals("P", StringComparison.CurrentCultureIgnoreCase) && empRP.EmpRPUnitPeriodAsDaily))
                {
                    HROne.Payroll.PaymentBreakDownKey lunchDeductionBreakDownKey = new HROne.Payroll.PaymentBreakDownKey(empRP.PayCodeID, -LunchRPRate, empRP.EmpRPMethod, empRP.EmpAccID, false, RPCostCenterID, true);
                    recurringPaymentBreakDownCollection.AddUnit(lunchDeductionBreakDownKey, LunchRPUnit, 0);
                }

                HROne.Payroll.PaymentBreakDownKey lunchAllowanceBreakDownKey = new HROne.Payroll.PaymentBreakDownKey(empRP.PayCodeID, LunchRPRate, empRP.EmpRPMethod, empRP.EmpAccID, true, RPCostCenterID, true);
                recurringPaymentBreakDownCollection.AddUnit(lunchAllowanceBreakDownKey, LunchRPUnit, 0);

            }
        }
        protected virtual void AddLateEarlyLeavePaymentAttendanceResult(int EmpID, EAttendanceRecord attendanceRecord, EAttendancePlan attendancePlan, EPayrollGroup payGroup, EPayrollPeriod payrollPeriod, ERosterCode rosterCode, EAttendanceFormula attendanceFormula, EPaymentCode paymentCode, int empOverrideCostCenterID, int empOverrideCostCenterIDLocation1, int empOverrideCostCenterIDLocation2, double totalPeriodPayment, double totalDailyPayment, double totalHourlyPayment, int numOfPeriodPerYear, string payMethod, int empAccID, PaymentBreakDownCollection recurringPaymentBreakDownCollection, double RosterCodeWorkHourPerDay, out int actualMins, out int lateEarlyMinsLocation1, out int lateEarlyMinsLocation2)
        {
            lateEarlyMinsLocation1 = attendanceRecord.AttendanceRecordActualLateMins;
            lateEarlyMinsLocation2 = 0;
            if (attendancePlan.AttendancePlanLateIncludeEarlyLeave)
                lateEarlyMinsLocation2 += attendanceRecord.AttendanceRecordActualEarlyLeaveMins;
            if (attendancePlan.AttendancePlanLateIncludeLunchLate)
                lateEarlyMinsLocation2 += attendanceRecord.AttendanceRecordActualLunchLateMins;
            if (attendancePlan.AttendancePlanLateIncludeLunchEarlyLeave)
                lateEarlyMinsLocation1 += attendanceRecord.AttendanceRecordActualLunchEarlyLeaveMins;

            actualMins = AppUtils.ApplyRoundingRule(lateEarlyMinsLocation1 + lateEarlyMinsLocation2, attendancePlan.AttendancePlanLateMinsRoundingRule, attendancePlan.AttendancePlanLateMinsUnit);

            if (!attendancePlan.AttendancePlanCompensateLateByOT)
            {

                double hourlyRate = 0;
                string hourlyRateRemark = string.Empty;     //  dummy, for debug only

                hourlyRate = AttendanceFormulaProcess.GetHourlyRate(dbConn, attendanceFormula.AttendanceFormulaID, payGroup.PayGroupDefaultProrataFormula, EmpID, totalPeriodPayment, totalDailyPayment, totalHourlyPayment, payrollPeriod.PayPeriodAttnFr, payrollPeriod.PayPeriodAttnTo, numOfPeriodPerYear, attendanceRecord.AttendanceRecordDate, rosterCode.RosterCodeIsOverrideHourlyPayment, rosterCode.RosterCodeOverrideHoulyAmount, 1, RosterCodeWorkHourPerDay, out hourlyRateRemark);
                if (hourlyRate > 0)
                {
                    if (empOverrideCostCenterIDLocation1 == empOverrideCostCenterIDLocation2)
                    {
                        if (actualMins > 0)
                        {
                            HROne.Payroll.PaymentBreakDownKey lateDeductionBreakDownKey = new HROne.Payroll.PaymentBreakDownKey(paymentCode.PaymentCodeID, hourlyRate, hourlyRateRemark, 1, payMethod, empAccID, false, empOverrideCostCenterID, true);
                            recurringPaymentBreakDownCollection.AddUnit(lateDeductionBreakDownKey, -actualMins / 60.0, 0);
                        }
                    }
                    else
                    {
                        int actualMinsLocation1 = AppUtils.ApplyRoundingRule(lateEarlyMinsLocation1, attendancePlan.AttendancePlanLateMinsRoundingRule, attendancePlan.AttendancePlanLateMinsUnit);
                        if (actualMinsLocation1 > 0)
                        {
                            HROne.Payroll.PaymentBreakDownKey lateDeductionBreakDownKey = new HROne.Payroll.PaymentBreakDownKey(paymentCode.PaymentCodeID, hourlyRate, hourlyRateRemark, 1, payMethod, empAccID, false, empOverrideCostCenterIDLocation1, true);
                            recurringPaymentBreakDownCollection.AddUnit(lateDeductionBreakDownKey, -actualMinsLocation1 / 60.0, 0);
                        }

                        int actualMinsLocation2 = AppUtils.ApplyRoundingRule(lateEarlyMinsLocation2, attendancePlan.AttendancePlanLateMinsRoundingRule, attendancePlan.AttendancePlanLateMinsUnit);
                        if (actualMinsLocation2 > 0)
                        {
                            HROne.Payroll.PaymentBreakDownKey lateDeductionBreakDownKey = new HROne.Payroll.PaymentBreakDownKey(paymentCode.PaymentCodeID, hourlyRate, hourlyRateRemark, 1, payMethod, empAccID, false, empOverrideCostCenterIDLocation2, true);
                            recurringPaymentBreakDownCollection.AddUnit(lateDeductionBreakDownKey, -actualMinsLocation2 / 60.0, 0);
                        }
                    }
                }
            }
        }
        protected virtual void AddOvertimePaymentAttendanceResult(int EmpID, EAttendanceRecord attendanceRecord, EAttendancePlan attendancePlan, EPayrollGroup payGroup, EPayrollPeriod payrollPeriod, ERosterCode rosterCode, EAttendanceFormula attendanceFormula, EPaymentCode paymentCode, int empOverrideCostCenterID, int empOverrideCostCenterIDLocation1, int empOverrideCostCenterIDLocation2, double totalPeriodPayment, double totalDailyPayment, double totalHourlyPayment, int numOfPeriodPerYear, string payMethod, int empAccID, PaymentBreakDownCollection recurringPaymentBreakDownCollection, double RosterCodeWorkHourPerDay, out int actualMins)
        {
            if (attendanceRecord.GetAttendanceRecordExtendData(EAttendanceRecord.FIELD_EXTENDDATA_WORK_AS_OVERTIME).Equals("Yes", StringComparison.CurrentCultureIgnoreCase))
            {
                double workAsOTDailyRate = 0;
                double workAsOTHourlyRate = 0;
                string workAsOTDailyRateRemark = string.Empty;
                string workAsOTHourlyRateRemark = string.Empty;
                if (attendanceFormula.AttendanceFormulaType.Equals(EAttendanceFormula.FORMULATYPE_CODE_FIX_RATE))
                    workAsOTHourlyRate = attendanceFormula.AttendanceFormulaFixedRate;
                else if (attendanceFormula.AttendanceFormulaType.Equals(EAttendanceFormula.FORMULATYPE_CODE_BY_FORMULA))
                {
                    bool dummyDAW = false;
                    workAsOTDailyRate = HROne.Payroll.PayrollFormula.DailyProrataCaluclation(dbConn, attendanceFormula.AttendanceFormulaPayFormID, payGroup.PayGroupDefaultProrataFormula, EmpID, totalPeriodPayment, payrollPeriod.PayPeriodAttnFr, payrollPeriod.PayPeriodAttnTo, numOfPeriodPerYear, attendanceRecord.AttendanceRecordDate, out workAsOTDailyRateRemark, out dummyDAW);
                    if (totalDailyPayment != 0)
                        if (workAsOTDailyRate.Equals(0))
                        {
                            workAsOTDailyRateRemark = totalDailyPayment.ToString("0.00##");
                            workAsOTDailyRate = totalDailyPayment;
                        }
                        else
                        {
                            workAsOTDailyRateRemark += " + " + totalDailyPayment.ToString("0.00##");
                            workAsOTDailyRate += totalDailyPayment;
                        }

                    if (totalHourlyPayment != 0)
                    {
                        if (rosterCode.RosterCodeIsOverrideHourlyPayment)
                            workAsOTHourlyRate = rosterCode.RosterCodeOverrideHoulyAmount;
                        else
                            workAsOTHourlyRate = totalHourlyPayment;
                        workAsOTHourlyRateRemark = workAsOTHourlyRate.ToString("0.00##");
                    }

                }
                if (workAsOTDailyRate > 0)
                {
                    HROne.Payroll.PaymentBreakDownKey key = new HROne.Payroll.PaymentBreakDownKey(paymentCode.PaymentCodeID, workAsOTDailyRate, workAsOTDailyRateRemark, 1, payMethod, empAccID, false, empOverrideCostCenterID, true);
                    recurringPaymentBreakDownCollection.AddUnit(key, attendanceRecord.AttendanceRecordActualWorkingDay, 0);

                }
                if (workAsOTHourlyRate > 0)
                {
                    HROne.Payroll.PaymentBreakDownKey key = new HROne.Payroll.PaymentBreakDownKey(paymentCode.PaymentCodeID, workAsOTHourlyRate, payMethod, empAccID, false, empOverrideCostCenterID, true);
                    recurringPaymentBreakDownCollection.AddUnit(key, attendanceRecord.AttendanceRecordActualWorkingHour, 0);

                }
            }
            int overTimeMinsLocatoin1 = 0;
            if (attendancePlan.AttendancePlanOTIncludeLunchOvertime)
                overTimeMinsLocatoin1 += attendanceRecord.AttendanceRecordActualLunchOvertimeMins;

            int overTimeMinsLocatoin2 = attendanceRecord.AttendanceRecordActualOvertimeMins;
            actualMins = AppUtils.ApplyRoundingRule(overTimeMinsLocatoin1 + overTimeMinsLocatoin2, attendancePlan.AttendancePlanOTMinsRoundingRule, attendancePlan.AttendancePlanOTMinsUnit);

            //totalOvertimeMins += actualMins;

            if (!attendancePlan.AttendancePlanCompensateLateByOT)
            {


                DBFilter rosterCodeDetailFilter = new DBFilter();
                //rosterCodeDetailFilter.add(new Match("RosterCodeDetailNoOfHour", "<=", (double)actualMins / 60));
                rosterCodeDetailFilter.add(new Match("RosterCodeID", rosterCode.RosterCodeID));
                rosterCodeDetailFilter.add("RosterCodeDetailNoOfHour", true);
                ArrayList rosterCodeDetailList = ERosterCodeDetail.db.select(dbConn, rosterCodeDetailFilter);

                //double actualMinsAfterRatio = 0;
                if (rosterCodeDetailList.Count == 0)
                {
                    ERosterCodeDetail rosterCodeDetail = new ERosterCodeDetail();
                    rosterCodeDetail.RosterCodeDetailNoOfHour = 9999;
                    rosterCodeDetail.RosterCodeDetailRate = 1;
                    rosterCodeDetailList.Add(rosterCodeDetail);
                }

                if (empOverrideCostCenterIDLocation1 == empOverrideCostCenterIDLocation2)
                {

                    double lastRankHour = 0;
                    foreach (ERosterCodeDetail rosterCodeDetail in rosterCodeDetailList)
                    {
                        if (actualMins > lastRankHour * 60)
                        {

                            double actualUnit = 0;
                            string hourlyRateRemark = string.Empty;     //  dummy, for debug only
                            double actualHourlyRate = AttendanceFormulaProcess.GetHourlyRate(dbConn, attendanceFormula.AttendanceFormulaID, payGroup.PayGroupDefaultProrataFormula, EmpID, totalPeriodPayment, totalDailyPayment, totalHourlyPayment, payrollPeriod.PayPeriodAttnFr, payrollPeriod.PayPeriodAttnTo, numOfPeriodPerYear, attendanceRecord.AttendanceRecordDate, rosterCode.RosterCodeIsOverrideHourlyPayment, rosterCode.RosterCodeOverrideHoulyAmount, rosterCodeDetail.RosterCodeDetailRate * attendancePlan.AttendancePlanOTRateMultiplier,RosterCodeWorkHourPerDay, out hourlyRateRemark);

                            if (rosterCodeDetail.RosterCodeDetailNoOfHour * 60 <= actualMins)
                                actualUnit = (rosterCodeDetail.RosterCodeDetailNoOfHour - lastRankHour);
                            else
                                actualUnit = (actualMins / 60.0 - lastRankHour);
                            if (actualHourlyRate > 0 && actualUnit > 0)
                            {
                                HROne.Payroll.PaymentBreakDownKey overtimePaymentBreakDownKey = new HROne.Payroll.PaymentBreakDownKey(paymentCode.PaymentCodeID, actualHourlyRate, hourlyRateRemark, 1, payMethod, empAccID, false, empOverrideCostCenterID, true);
                                recurringPaymentBreakDownCollection.AddUnit(overtimePaymentBreakDownKey, actualUnit, 0);
                            }
                            lastRankHour = rosterCodeDetail.RosterCodeDetailNoOfHour;
                        }
                        else
                            break;
                    }

                }
                else
                {
                    //  Generate normal Overtime
                    int actualMinsLocation2 = AppUtils.ApplyRoundingRule(overTimeMinsLocatoin2, attendancePlan.AttendancePlanOTMinsRoundingRule, attendancePlan.AttendancePlanOTMinsUnit);
                    double lastRankHour = 0;

                    foreach (ERosterCodeDetail rosterCodeDetail in rosterCodeDetailList)
                    {
                        if (actualMinsLocation2 > lastRankHour * 60)
                        {

                            double actualUnit = 0;
                            string hourlyRateRemark = string.Empty;     //  dummy, for debug only
                            double actualHourlyRate = AttendanceFormulaProcess.GetHourlyRate(dbConn, attendanceFormula.AttendanceFormulaID, payGroup.PayGroupDefaultProrataFormula, EmpID, totalPeriodPayment, totalDailyPayment, totalHourlyPayment, payrollPeriod.PayPeriodAttnFr, payrollPeriod.PayPeriodAttnTo, numOfPeriodPerYear, attendanceRecord.AttendanceRecordDate, rosterCode.RosterCodeIsOverrideHourlyPayment, rosterCode.RosterCodeOverrideHoulyAmount, rosterCodeDetail.RosterCodeDetailRate * attendancePlan.AttendancePlanOTRateMultiplier, RosterCodeWorkHourPerDay, out hourlyRateRemark);

                            if (rosterCodeDetail.RosterCodeDetailNoOfHour * 60 <= actualMinsLocation2)
                                actualUnit = (rosterCodeDetail.RosterCodeDetailNoOfHour - lastRankHour);
                            else
                                actualUnit = (actualMinsLocation2 / 60.0 - lastRankHour);
                            if (actualHourlyRate > 0 && actualUnit > 0)
                            {
                                HROne.Payroll.PaymentBreakDownKey overtimePaymentBreakDownKey = new HROne.Payroll.PaymentBreakDownKey(paymentCode.PaymentCodeID, actualHourlyRate, hourlyRateRemark, 1, payMethod, empAccID, false, empOverrideCostCenterIDLocation2, true);
                                recurringPaymentBreakDownCollection.AddUnit(overtimePaymentBreakDownKey, actualUnit, 0);
                            }
                            lastRankHour = rosterCodeDetail.RosterCodeDetailNoOfHour;

                        }
                        else
                            break;
                    }

                    //  Generate lunch Overtime
                    int actualMinsLocation1 = AppUtils.ApplyRoundingRule(overTimeMinsLocatoin1, attendancePlan.AttendancePlanOTMinsRoundingRule, attendancePlan.AttendancePlanOTMinsUnit);
                    lastRankHour = 0;

                    foreach (ERosterCodeDetail rosterCodeDetail in rosterCodeDetailList)
                    {
                        if (actualMinsLocation1 > lastRankHour * 60)
                        {
                            double actualUnit = 0;
                            string hourlyRateRemark = string.Empty;     //  dummy, for debug only
                            double actualHourlyRate = AttendanceFormulaProcess.GetHourlyRate(dbConn, attendanceFormula.AttendanceFormulaID, payGroup.PayGroupDefaultProrataFormula, EmpID, totalPeriodPayment, totalDailyPayment, totalHourlyPayment, payrollPeriod.PayPeriodAttnFr, payrollPeriod.PayPeriodAttnTo, numOfPeriodPerYear, attendanceRecord.AttendanceRecordDate, rosterCode.RosterCodeIsOverrideHourlyPayment, rosterCode.RosterCodeOverrideHoulyAmount, rosterCodeDetail.RosterCodeDetailRate * attendancePlan.AttendancePlanOTRateMultiplier, RosterCodeWorkHourPerDay, out hourlyRateRemark);

                            if (rosterCodeDetail.RosterCodeDetailNoOfHour * 60 <= actualMinsLocation1)
                                actualUnit = (rosterCodeDetail.RosterCodeDetailNoOfHour - lastRankHour);
                            else
                                actualUnit = (actualMinsLocation1 / 60.0 - lastRankHour);
                            if (actualHourlyRate > 0 && actualUnit > 0)
                            {
                                HROne.Payroll.PaymentBreakDownKey overtimePaymentBreakDownKey = new HROne.Payroll.PaymentBreakDownKey(paymentCode.PaymentCodeID, actualHourlyRate, hourlyRateRemark, 1, payMethod, empAccID, false, empOverrideCostCenterIDLocation1, true);
                                recurringPaymentBreakDownCollection.AddUnit(overtimePaymentBreakDownKey, actualUnit, 0);
                            }
                            lastRankHour = rosterCodeDetail.RosterCodeDetailNoOfHour;
                        }
                        else
                            break;
                    }

                }
            }

        }
        protected virtual void CreateAdditionalPaymentList(int EmpID, EAttendanceRecord attendanceRecord, EAttendancePlan attendancePlan, ERosterCode rosterCode, int empOverrideCostCenterID, string payMethod, int empAccID, PaymentBreakDownCollection recurringPaymentBreakDownCollection)
        {
            if (attendanceRecord.AttendanceRecordIsAbsent || attendanceRecord.AttendanceRecordActualWorkingDay <= 0)
                return;

            #region Additional Payment for Roster Code
            {
                DBFilter rosterCodeAdditionalPaymentFilter = new DBFilter();
                rosterCodeAdditionalPaymentFilter.add(new Match("RosterCodeID", attendanceRecord.RosterCodeID));
                ArrayList additionPaymentObjectList = ERosterCodeAdditionalPayment.db.select(dbConn, rosterCodeAdditionalPaymentFilter);
                foreach (ERosterCodeAdditionalPayment additionalPaymentObject in additionPaymentObjectList)
                {
                    HROne.Payroll.PaymentBreakDownKey additionalPaymentBreakDownKey = new HROne.Payroll.PaymentBreakDownKey(additionalPaymentObject.PaymentCodeID, additionalPaymentObject.RosterCodeAdditionalPaymentAmount, payMethod, empAccID, false, empOverrideCostCenterID, true);
                    recurringPaymentBreakDownCollection.AddUnit(additionalPaymentBreakDownKey, 1, 0);

                }
            }
            #endregion

            #region Additional Payment for Attendance Plan

            if (attendancePlan != null)
            {
                DBFilter attendancePlanAdditionalPaymentFilter = new DBFilter();
                attendancePlanAdditionalPaymentFilter.add(new Match("AttendancePlanID", attendancePlan.AttendancePlanID));
                ArrayList additionPaymentObjectList = EAttendancePlanAdditionalPayment.db.select(dbConn, attendancePlanAdditionalPaymentFilter);
                foreach (EAttendancePlanAdditionalPayment additionalPaymentObject in additionPaymentObjectList)
                {
                    bool isConditionFulfill = true;

                    //  Check Late
                    if (attendanceRecord.AttendanceRecordActualLateMins > additionalPaymentObject.AttendancePlanAdditionalPaymentMaxLateMins)
                        isConditionFulfill = false;

                    //  Check Early Leave
                    if (attendanceRecord.AttendanceRecordActualLunchEarlyLeaveMins > additionalPaymentObject.AttendancePlanAdditionalPaymentMaxEarlyLeaveMins)
                        isConditionFulfill = false;

                    //  Check Overtime
                    if (attendanceRecord.AttendanceRecordActualOvertimeMins < additionalPaymentObject.AttendancePlanAdditionalPaymentMinOvertimeMins)
                        isConditionFulfill = false;

                    if (!additionalPaymentObject.AttendancePlanAdditionalPaymentRosterAcrossTime.Ticks.Equals(0)
                        && !rosterCode.RosterCodeInTime.Ticks.Equals(0) && !rosterCode.RosterCodeOutTime.Ticks.Equals(0))
                    {
                        DateTime actualAcrossTime = attendanceRecord.AttendanceRecordDate.Add(additionalPaymentObject.AttendancePlanAdditionalPaymentRosterAcrossTime.TimeOfDay);
                        while (actualAcrossTime < rosterCode.RosterCodeInTime)
                            actualAcrossTime = actualAcrossTime.AddDays(1);
                        if (actualAcrossTime > rosterCode.RosterCodeOutTime)
                            isConditionFulfill = false;
                    }

                    if (isConditionFulfill)
                    {
                        HROne.Payroll.PaymentBreakDownKey additionalPaymentBreakDownKey = new HROne.Payroll.PaymentBreakDownKey(additionalPaymentObject.PaymentCodeID, additionalPaymentObject.AttendancePlanAdditionalPaymentAmount, payMethod, empAccID, false, empOverrideCostCenterID, true);
                        recurringPaymentBreakDownCollection.AddUnit(additionalPaymentBreakDownKey, 1, 0);
                    }
                }

            }

            #endregion

        }

    }
}