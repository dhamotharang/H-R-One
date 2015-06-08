using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Globalization;
using HROne.Lib.Entities;
using HROne.DataAccess;

namespace HROne.Import
{
    [DBClass("UploadAttendancePreparationProcess")]
    public class EUploadAttendancePreparationProcess : ImportDBObject
    {
        public static DBManager db = new DBManagerWithRecordInfo(typeof(EUploadAttendancePreparationProcess));

        public static EUploadAttendancePreparationProcess GetObject(DatabaseConnection dbConn, int ID)
        {
            EUploadAttendancePreparationProcess m_object = new EUploadAttendancePreparationProcess();
            m_object.UploadAttendancePreparationProcessID = ID;
            if (EUploadAttendancePreparationProcess.db.select(dbConn, m_object))
                return m_object;
            return null;
        }

        protected int m_UploadAttendancePreparationProcessID;
        [DBField("UploadAttendancePreparationProcessID", true, true), TextSearch, Export(false)]
        public int UploadAttendancePreparationProcessID
        {
            get { return m_UploadAttendancePreparationProcessID; }
            set { m_UploadAttendancePreparationProcessID = value; modify("UploadAttendancePreparationProcessID"); }
        }

        protected int m_EmpAPPID;
        [DBField("EmpAPPID"), TextSearch, Export(false)]
        public int EmpAPPID
        {
            get { return m_EmpAPPID; }
            set { m_EmpAPPID = value; modify("EmpAPPID"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), Int, TextSearch, Export(false), Required]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }

        protected int m_AttendancePreparationProcessID;
        [DBField("AttendancePreparationProcessID"), Int, TextSearch, Export(false)]
        public int AttendancePreparationProcessID
        {
            get { return m_AttendancePreparationProcessID; }
            set { m_AttendancePreparationProcessID = value; modify("AttendancePreparationProcessID"); }
        }

        protected int m_EmpRPID;
        [DBField("EmpRPID"), Int, TextSearch, Export(false), Required]
        public int EmpRPID
        {
            get { return m_EmpRPID; }
            set { m_EmpRPID = value; modify("EmpRPID"); }
        }

        protected string m_Day1;
        [DBField("Day1"), TextSearch, Export(false)]
        public string Day1
        {
            get { return m_Day1; }
            set { m_Day1 = value; modify("Day1"); }
        }

        protected string m_Day2;
        [DBField("Day2"), TextSearch, Export(false)]
        public string Day2
        {
            get { return m_Day2; }
            set { m_Day2 = value; modify("Day2"); }
        }

        protected string m_Day3;
        [DBField("Day3"), TextSearch, Export(false)]
        public string Day3
        {
            get { return m_Day3; }
            set { m_Day3 = value; modify("Day3"); }
        }

        protected string m_Day4;
        [DBField("Day4"), TextSearch, Export(false)]
        public string Day4
        {
            get { return m_Day4; }
            set { m_Day4 = value; modify("Day4"); }
        }

        protected string m_Day5;
        [DBField("Day5"), TextSearch, Export(false)]
        public string Day5
        {
            get { return m_Day5; }
            set { m_Day5 = value; modify("Day5"); }
        }

        protected string m_Day6;
        [DBField("Day6"), TextSearch, Export(false)]
        public string Day6
        {
            get { return m_Day6; }
            set { m_Day6 = value; modify("Day6"); }
        }

        protected string m_Day7;
        [DBField("Day7"), TextSearch, Export(false)]
        public string Day7
        {
            get { return m_Day7; }
            set { m_Day7 = value; modify("Day7"); }
        }

        protected string m_Day8;
        [DBField("Day8"), TextSearch, Export(false)]
        public string Day8
        {
            get { return m_Day8; }
            set { m_Day8 = value; modify("Day8"); }
        }

        protected string m_Day9;
        [DBField("Day9"), TextSearch, Export(false)]
        public string Day9
        {
            get { return m_Day9; }
            set { m_Day9 = value; modify("Day9"); }
        }

        protected string m_Day10;
        [DBField("Day10"), TextSearch, Export(false)]
        public string Day10
        {
            get { return m_Day10; }
            set { m_Day10 = value; modify("Day10"); }
        }

        protected string m_Day11;
        [DBField("Day11"), TextSearch, Export(false)]
        public string Day11
        {
            get { return m_Day11; }
            set { m_Day11 = value; modify("Day11"); }
        }

        protected string m_Day12;
        [DBField("Day12"), TextSearch, Export(false)]
        public string Day12
        {
            get { return m_Day12; }
            set { m_Day12 = value; modify("Day12"); }
        }

        protected string m_Day13;
        [DBField("Day13"), TextSearch, Export(false)]
        public string Day13
        {
            get { return m_Day13; }
            set { m_Day13 = value; modify("Day13"); }
        }

        protected string m_Day14;
        [DBField("Day14"), TextSearch, Export(false)]
        public string Day14
        {
            get { return m_Day14; }
            set { m_Day14 = value; modify("Day14"); }
        }

        protected string m_Day15;
        [DBField("Day15"), TextSearch, Export(false)]
        public string Day15
        {
            get { return m_Day15; }
            set { m_Day15 = value; modify("Day15"); }
        }

        protected string m_Day16;
        [DBField("Day16"), TextSearch, Export(false)]
        public string Day16
        {
            get { return m_Day16; }
            set { m_Day16 = value; modify("Day16"); }
        }

        protected string m_Day17;
        [DBField("Day17"), TextSearch, Export(false)]
        public string Day17
        {
            get { return m_Day17; }
            set { m_Day17 = value; modify("Day17"); }
        }

        protected string m_Day18;
        [DBField("Day18"), TextSearch, Export(false)]
        public string Day18
        {
            get { return m_Day18; }
            set { m_Day18 = value; modify("Day18"); }
        }

        protected string m_Day19;
        [DBField("Day19"), TextSearch, Export(false)]
        public string Day19
        {
            get { return m_Day19; }
            set { m_Day19 = value; modify("Day19"); }
        }

        protected string m_Day20;
        [DBField("Day20"), TextSearch, Export(false)]
        public string Day20
        {
            get { return m_Day20; }
            set { m_Day20 = value; modify("Day20"); }
        }

        protected string m_Day21;
        [DBField("Day21"), TextSearch, Export(false)]
        public string Day21
        {
            get { return m_Day21; }
            set { m_Day21 = value; modify("Day21"); }
        }

        protected string m_Day22;
        [DBField("Day22"), TextSearch, Export(false)]
        public string Day22
        {
            get { return m_Day22; }
            set { m_Day22 = value; modify("Day22"); }
        }

        protected string m_Day23;
        [DBField("Day23"), TextSearch, Export(false)]
        public string Day23
        {
            get { return m_Day23; }
            set { m_Day23 = value; modify("Day23"); }
        }

        protected string m_Day24;
        [DBField("Day24"), TextSearch, Export(false)]
        public string Day24
        {
            get { return m_Day24; }
            set { m_Day24 = value; modify("Day24"); }
        }

        protected string m_Day25;
        [DBField("Day25"), TextSearch, Export(false)]
        public string Day25
        {
            get { return m_Day25; }
            set { m_Day25 = value; modify("Day25"); }
        }

        protected string m_Day26;
        [DBField("Day26"), TextSearch, Export(false)]
        public string Day26
        {
            get { return m_Day26; }
            set { m_Day26 = value; modify("Day26"); }
        }

        protected string m_Day27;
        [DBField("Day27"), TextSearch, Export(false)]
        public string Day27
        {
            get { return m_Day27; }
            set { m_Day27 = value; modify("Day27"); }
        }

        protected string m_Day28;
        [DBField("Day28"), TextSearch, Export(false)]
        public string Day28
        {
            get { return m_Day28; }
            set { m_Day28 = value; modify("Day28"); }
        }

        protected string m_Day29;
        [DBField("Day29"), TextSearch, Export(false)]
        public string Day29
        {
            get { return m_Day29; }
            set { m_Day29 = value; modify("Day29"); }
        }

        protected string m_Day30;
        [DBField("Day30"), TextSearch, Export(false)]
        public string Day30
        {
            get { return m_Day30; }
            set { m_Day30 = value; modify("Day30"); }
        }

        protected string m_Day31;
        [DBField("Day31"), TextSearch, Export(false)]
        public string Day31
        {
            get { return m_Day31; }
            set { m_Day31 = value; modify("Day31"); }
        }

        protected int m_TotalHours;
        [DBField("TotalHours"), Int, TextSearch, Export(false), Required]
        public int TotalHours
        {
            get { return m_TotalHours; }
            set { m_TotalHours = value; modify("TotalHours"); }
        }

        protected string m_Remarks;
        [DBField("Remarks"), TextSearch, Export(false)]
        public string Remarks
        {
            get { return m_Remarks; }
            set { m_Remarks = value; modify("Remarks"); }
        }

        protected double m_ReductionOthers;
        [DBField("ReductionOthers", format = "#,##0.00"), TextSearch, Export(false), Required]
        public double ReductionOthers
        {
            get { return m_ReductionOthers; }
            set { m_ReductionOthers = value; modify("ReductionOthers"); }
        }

        protected double m_ReductionUniformTimecard;
        [DBField("ReductionUniformTimecard", format = "#,##0.00"), TextSearch, Export(false), Required]
        public double ReductionUniformTimecard
        {
            get { return m_ReductionUniformTimecard; }
            set { m_ReductionUniformTimecard = value; modify("ReductionUniformTimecard"); }
        }

        protected double m_BackpayAllowance;
        [DBField("BackpayAllowance", format = "#,##0.00"), TextSearch, Export(false), Required]
        public double BackpayAllowance
        {
            get { return m_BackpayAllowance; }
            set { m_BackpayAllowance = value; modify("BackpayAllowance"); }
        }

        protected double m_BackpayOthers;
        [DBField("BackpayOthers", format = "#,##0.00"), TextSearch, Export(false), Required]
        public double BackpayOthers
        {
            get { return m_BackpayOthers; }
            set { m_BackpayOthers = value; modify("BackpayOthers"); }
        }

        protected double m_BackpayUniformTimecard;
        [DBField("BackpayUniformTimecard", format = "#,##0.00"), TextSearch, Export(false), Required]
        public double BackpayUniformTimecard
        {
            get { return m_BackpayUniformTimecard; }
            set { m_BackpayUniformTimecard = value; modify("BackpayUniformTimecard"); }
        }
    }

    /// <summary>
    /// Summary description for ImportCND
    /// </summary>
    public class ImportAttendancePreparationProcess : ImportProcessInteface
    {
        public const string TABLE_NAME = "AttendanceRecordDataEntry";
        public const string FIELD_TITLE = "Attendance Record Data Entry";
        public const string FIELD_MONTH = "月份";
        public const string FIELD_YEAR = "年份";
        public const string FIELD_EMP_NO = "員工號碼";
        public const string FIELD_CHINESE_FULL_NAME = "中文姓名";
        public const string FIELD_ENGLISH_FULL_NAME = "英文姓名";
        public const string FIELD_SHIFT_CODE = "編碼";
        public const string FIELD_SHIFT_TIME = "工作時間";
        public const string FIELD_PAY_FORMULA_CODE = "假期編號";
        public const string FIELD_TOTAL_HOURS = "本月總工時";
        public const string FIELD_EMPLOYEE_SIGNATURE = "員工簽署";
        public const string FIELD_REMARK = "備  註";
        public const string FIELD_BASIC_SALARY = "底薪";
        public const string FIELD_ALLOWANCE_AMOUNT = "津貼";
        public const string FIELD_TRAVEL_ALLOWANCE = "車津";
        public const string FIELD_REDUCTION_OTHERS = "其它";
        public const string FIELD_REDUCTION_UNIFORM_TIMECARD = "製服/工咭";
        public const string FIELD_BACKPAY_ALLOWANCE = "補津";
        public const string FIELD_BACKPAY_OTHERS = "其它2";
        public const string FIELD_BACKPAY_UNIFORM_TIMECARD = "製服/工咭2";

        public const string FIELD_REDUCTION_ITEM = "扣薪項";
        public const string FIELD_BACKPAY_ITEM = "補薪項";

        // For Calculated Attendance Record Report
        public const string FIELD_CURRENT_MONTH = "本月";
        public const string FIELD_WORK_YEAR = "勞+年";
        public const string FIELD_OTHER_WAGE = "其他全薪";
        public const string FIELD_VOLUNTARY_OVERTIME = "例假自願加班";
        public const string FIELD_CASUAL_LEAVE = "例假";
        public const string FIELD_SICK_MATERNITY = "病產傷假";
        public const string FIELD_NO_SICK_MATERNITY = "病產事無";
        public const string FIELD_NO_WORK = "沒有工作";
        public const string FIELD_LEAVE_DAYS = "總工作及假期日數";
        public const string FIELD_OVERTIME_DAYS = "額外工作日數";
        public const string FIELD_BACKPAY = "補薪";

        public const string FIELD_SICK_LEAVE = "病假";
        public const string FIELD_SICK_DAYS = "日數";
        public const string FIELD_SICK_BACKPAY = "補薪2";
        
        public const string FIELD_INJURY = "工傷";
        public const string FIELD_INJURY_DAYS = "日數2";
        public const string FIELD_INJURY_BACKPAY = "補薪3";

        public const string FIELD_UNPAID_LEAVE = "無薪假";
        public const string FIELD_UNPAID_DAYS = "日數3";
        public const string FIELD_UNPAID_REDUCTION = "扣薪";

        public const string FIELD_PAID_LEAVE = "有薪年假";
        public const string FIELD_PAID_DAYS = "日數4";        
        public const string FIELD_PAID_BACKPAY = "補薪4";

        public const string DAY_1 = "1";
        public const string DAY_2 = "2";
        public const string DAY_3 = "3";
        public const string DAY_4 = "4";
        public const string DAY_5 = "5";
        public const string DAY_6 = "6";
        public const string DAY_7 = "7";
        public const string DAY_8 = "8";
        public const string DAY_9 = "9";
        public const string DAY_10 = "10";
        public const string DAY_11 = "11";
        public const string DAY_12 = "12";
        public const string DAY_13 = "13";
        public const string DAY_14 = "14";
        public const string DAY_15 = "15";
        public const string DAY_16 = "16";
        public const string DAY_17 = "17";
        public const string DAY_18 = "18";
        public const string DAY_19 = "19";
        public const string DAY_20 = "20";
        public const string DAY_21 = "21";
        public const string DAY_22 = "22";
        public const string DAY_23 = "23";
        public const string DAY_24 = "24";
        public const string DAY_25 = "25";
        public const string DAY_26 = "26";
        public const string DAY_27 = "27";
        public const string DAY_28 = "28";
        public const string DAY_29 = "29";
        public const string DAY_30 = "30";
        public const string DAY_31 = "31";
        
        //Day in month
        public const string AL = "AL";
        public const string ALH = "AL/";
        public const string S = "S";
        public const string SH = "S/";
        public const string FL = "FL";
        public const string FLH = "FL/";
        public const string T = "T";
        public const string TH = "T/";
        public const string R = "R";
        public const string RH = "R/";
        public const string SP = "SP";
        public const string SPH = "SP/";
        public const string IJ = "IJ";
        public const string IJH = "IJ/";
        public const string SL = "SL";
        public const string SLH = "SL/";
        public const string ML = "ML";
        public const string MLH= "ML/";
        public const string NA = "NA";
        public const string NAH = "NA/";
        public const string DNA = "DNA";
        public const string DNAH = "DNA/";
        public const string NP = "NP";
        public const string NPH = "NP/";
        public const string SD = "S-";
        public const string SDH = "S-/";

        //Holiday code
        public const string SEVEN_N = "1/7N";
        public const string SEVEN_Y = "1/7Y";
        public const string HOURLY_PAY = "時";
        public const string DAILY_PAY = "日"; 
        public const string TEMP_WORKERS = "418";
        public const string SEVEN_P = "1/7P";
        public const string SIX_DAYS = "六日";
        public const string DAILY_PAY_BONUS = "日/P"; 

        private int m_UserID;
        protected int m_ProcessID;

        protected DatabaseConnection m_dbConn;

        public string Remark;
        private DateTime UploadDateTime = AppUtils.ServerDateTime();
        private DBManager db = EUploadAttendancePreparationProcess.db;
        public static int intYear, intMonth, processID, EmpID, ShiftDutyCodeID, PayCalFormulaID, EmpRPID;

        public ImportErrorList errors = new ImportErrorList();

        protected int PID;
        public int gPID
        {
            get { return PID; }
            set { PID = value; }
        }

        public ImportAttendancePreparationProcess(DatabaseConnection dbConn, string SessionID, int UserID, int pProcessID)
        : base(dbConn, SessionID)
        {
            m_dbConn = dbConn;
            m_UserID = UserID;
            m_ProcessID = pProcessID;
        }

        public ImportAttendancePreparationProcess(DatabaseConnection dbConn, string SessionID, int UserID)
            : base(dbConn, SessionID)
        {
            m_dbConn = dbConn;
            m_UserID = UserID;
        }

        public DataTable UploadToTempDatabase(DataTable rawDataTable, int UserID, bool CreateCodeIfNotExists)
        {
            if (rawDataTable == null)
                return GetImportDataFromTempDatabase(null);

            int rowCount = 1;

            // retrieve Payment Code of 薪金, 津貼, 車津
            int PayCodeID;
            int m_basicSalaryPayCodeID = 0;
            int m_allowancePayCodeID = 0;
            int m_travelAllowancePayCodeID = 0;

            foreach (EPaymentCode m_payCodeInfo in EPaymentCode.db.select(dbConn, new DBFilter()))
            {
                if (m_payCodeInfo.PaymentCodeDesc == "底薪")
                {
                    m_basicSalaryPayCodeID = m_payCodeInfo.PaymentCodeID;
                }
                else if (m_payCodeInfo.PaymentCodeDesc == "津貼")
                {
                    m_allowancePayCodeID = m_payCodeInfo.PaymentCodeID;
                }
                else if (m_payCodeInfo.PaymentCodeDesc == "車津")
                {
                    m_travelAllowancePayCodeID = m_payCodeInfo.PaymentCodeID;
                }
            }


            try
            {
                foreach (DataRow row in rawDataTable.Rows)
                {
                    string EmpNo = row[FIELD_EMP_NO].ToString();
                    EmpID = HROne.Import.Parse.GetEmpID(dbConn, EmpNo, UserID);
                    if (EmpID < 0)
                        errors.addError(ImportErrorMessage.ERROR_ACCESS_DENIED_EMP_NO, new string[] { EmpNo, rowCount.ToString() });

                    EmpRPID = 0;

                    string ShiftDutyCode = row[FIELD_SHIFT_CODE].ToString();
                    ShiftDutyCodeID = GetShiftDutyCodeID(dbConn, ShiftDutyCode, CreateCodeIfNotExists, UserID);

                    string ShiftTime = row[FIELD_SHIFT_TIME].ToString();
                    string[] times = ShiftTime.Split('-');
                    DateTime now = AppUtils.ServerDateTime();
                    string fromString = now.Year + "-" + now.Month + "-" + now.Day + " " + times[0].ToString() + ":00";
                    string toString = now.Year + "-" + now.Month + "-" + now.Day + " " + times[1].ToString() + ":00";
                    DateTime ShiftFrom = DateTime.Parse(fromString);
                    DateTime ShiftTo = DateTime.Parse(toString);

                    string PayCalFormulaCode = row[FIELD_PAY_FORMULA_CODE].ToString();
                    PayCalFormulaID = GetPayCalFormulaID(dbConn, PayCalFormulaCode, CreateCodeIfNotExists, UserID);

                    string[] Days = new string[31];

                    for (int i = 0; i < Days.GetUpperBound(0); i++)
                    {
                        Days[i] = row[rawDataTable.Columns[DAY_1].Ordinal + i].ToString();
                    }

                    string TotalHoursString = row[FIELD_TOTAL_HOURS].ToString();
                    int TotalHours = 0;
                    Remark = row[FIELD_REMARK].ToString();
                    string ReductionOthersString = row[FIELD_REDUCTION_OTHERS].ToString();
                    double ReductionOthers = 0;
                    string ReductionUniformTimecardString = row[FIELD_REDUCTION_UNIFORM_TIMECARD].ToString();
                    double ReductionUniformTimecard = 0;
                    string BackpayAllowanceString = row[FIELD_BACKPAY_ALLOWANCE].ToString();
                    double BackpayAllowance = 0;
                    string BackpayOthersString = row[FIELD_BACKPAY_OTHERS].ToString();
                    double BackpayOthers = 0;
                    string BackpayUniformTimecardString = row[FIELD_BACKPAY_UNIFORM_TIMECARD].ToString();
                    double BackpayUniformTimecard = 0;

                    // validate input data
                    if (EmpID <= 0)
                    {
                        errors.addError(ImportErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                    }
                    if (ShiftDutyCodeID <= 0)
                    {
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_SHIFT_CODE + "=" + ShiftDutyCode, EmpNo, rowCount.ToString() });
                    }
                    if (PayCalFormulaID <= 0)
                    {
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_PAY_FORMULA_CODE + "=" + PayCalFormulaID, EmpNo, rowCount.ToString() });
                    }
                    if (!double.TryParse(ReductionOthersString, out ReductionOthers))
                    {
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_REDUCTION_OTHERS + "=" + ReductionOthersString, EmpNo, rowCount.ToString() });
                    }
                    if (!int.TryParse(TotalHoursString, out TotalHours))
                    {
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_TOTAL_HOURS + "=" + TotalHoursString, EmpNo, rowCount.ToString() });
                    }
                    if (!double.TryParse(ReductionOthersString, out ReductionOthers))
                    {
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_REDUCTION_OTHERS + "=" + ReductionOthersString, EmpNo, rowCount.ToString() });
                    }
                    if (!double.TryParse(ReductionUniformTimecardString, out ReductionUniformTimecard))
                    {
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_REDUCTION_UNIFORM_TIMECARD + "=" + ReductionUniformTimecardString, EmpNo, rowCount.ToString() });
                    }
                    if (!double.TryParse(BackpayAllowanceString, out BackpayAllowance))
                    {
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_BACKPAY_ALLOWANCE + "=" + BackpayAllowanceString, EmpNo, rowCount.ToString() });
                    }
                    if (!double.TryParse(BackpayOthersString, out BackpayOthers))
                    {
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_BACKPAY_OTHERS + "=" + BackpayOthersString, EmpNo, rowCount.ToString() });
                    }
                    if (!double.TryParse(BackpayUniformTimecardString, out BackpayUniformTimecard))
                    {
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_BACKPAY_UNIFORM_TIMECARD + "=" + BackpayUniformTimecardString, EmpNo, rowCount.ToString() });
                    }
                    // check days value inputted
                    EAttendancePreparationProcess m_process = EAttendancePreparationProcess.GetObject(dbConn, m_ProcessID);
                    if (m_process != null)
                    {
                        int m_fromDay = m_process.AttendancePreparationProcessPeriodFr.Day;
                        int m_toDay = m_process.AttendancePreparationProcessPeriodTo.Day;

                        for (int i = 1; i < 31; i++)
                        {
                            if (m_fromDay < m_toDay)    // e.g. 1-Jan to 15-Jan
                            {
                                if (!string.IsNullOrEmpty(Days[i - 1]) &&
                                    (i < m_fromDay || i > m_toDay || i > HROne.CommonLib.Utility.LastDateOfMonth(m_process.AttendancePreparationProcessPeriodTo).Day)
                                    )
                                {
                                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { "DAY" + i.ToString("0") + "=" + Days[i-1], EmpNo, rowCount.ToString() });
                                }
                            }
                            else   // e.g. 16-Jan to 2-Feb
                            {
                                if (!string.IsNullOrEmpty(Days[i - 1]) &&
                                    ((i < m_fromDay && i > m_toDay) || i > HROne.CommonLib.Utility.LastDateOfMonth(m_process.AttendancePreparationProcessPeriodFr).Day)
                                    )
                                {
                                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { "DAY" + i.ToString("0") + "=" + Days[i - 1], EmpNo, rowCount.ToString() });
                                }
                            }
                        }
                    }
                    // parse PayCodeID
                    double m_paymentAmount = 0;
                    string m_payCodeDesc = "";

                    if (double.TryParse(row[FIELD_BASIC_SALARY].ToString(), out m_paymentAmount) && m_paymentAmount > 0)
                    {
                        PayCodeID = m_basicSalaryPayCodeID;
                    }
                    else if (double.TryParse(row[FIELD_ALLOWANCE_AMOUNT].ToString(), out m_paymentAmount) && m_paymentAmount > 0)
                    {
                        PayCodeID = m_allowancePayCodeID;
                    }
                    else if (double.TryParse(row[FIELD_TRAVEL_ALLOWANCE].ToString(), out m_paymentAmount) && m_paymentAmount > 0)
                    {
                        PayCodeID = m_travelAllowancePayCodeID;
                    }else
                        PayCodeID = 0;

                    if (PayCodeID <= 0)
                    {
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { "Cannot Locate Payment Code", EmpNo, rowCount.ToString() });
                    }

                    // check Recurring Payment ID
                    DBFilter m_rpFilter = new DBFilter();

                    m_rpFilter.add(new Match("EmpID", EmpID));
                    m_rpFilter.add(new Match("PayCodeID", PayCodeID));
                    m_rpFilter.add(new Match("EmpRPEffFr", "<=", m_process.AttendancePreparationProcessPeriodTo));
                    
                    OR m_orEffTo = new OR();
                    m_orEffTo.add(new Match("EmpRPEffTo", ">=", m_process.AttendancePreparationProcessPeriodFr));
                    m_orEffTo.add(new NullTerm("EmpRPEffTo"));

                    m_rpFilter.add(m_orEffTo);

                    ArrayList m_rpList = EEmpRecurringPayment.db.select(dbConn, m_rpFilter);
                    
                    if (m_rpList.Count > 0)
                    {
                        EEmpRecurringPayment m_rp = (EEmpRecurringPayment)m_rpList[0];
                        EmpRPID = m_rp.EmpRPID;
                    }

                    if (errors.List.Count <= 0)
                    {
                        EUploadAttendancePreparationProcess APP = new EUploadAttendancePreparationProcess();
                        APP.EmpID = EmpID;
                        APP.AttendancePreparationProcessID = this.gPID;
                        APP.EmpRPID = EmpRPID;
                        APP.Day1 = Days[0];
                        APP.Day2 = Days[1];
                        APP.Day3 = Days[2];
                        APP.Day4 = Days[3];
                        APP.Day5 = Days[4];
                        APP.Day6 = Days[5];
                        APP.Day7 = Days[6];
                        APP.Day8 = Days[7];
                        APP.Day9 = Days[8];
                        APP.Day10 = Days[9];
                        APP.Day11 = Days[10];
                        APP.Day12 = Days[11];
                        APP.Day13 = Days[12];
                        APP.Day14 = Days[13];
                        APP.Day15 = Days[14];
                        APP.Day16 = Days[15];
                        APP.Day17 = Days[16];
                        APP.Day18 = Days[17];
                        APP.Day19 = Days[18];
                        APP.Day20 = Days[19];
                        APP.Day21 = Days[20];
                        APP.Day22 = Days[21];
                        APP.Day23 = Days[22];
                        APP.Day24 = Days[23];
                        APP.Day25 = Days[24];
                        APP.Day26 = Days[25];
                        APP.Day27 = Days[26];
                        APP.Day28 = Days[27];
                        APP.Day29 = Days[28];
                        APP.Day30 = Days[29];
                        APP.Day31 = Days[30];
                        APP.TotalHours = TotalHours;
                        APP.Remarks = Remark;
                        APP.ReductionOthers = ReductionOthers;
                        APP.ReductionUniformTimecard = ReductionUniformTimecard;
                        APP.BackpayAllowance = BackpayAllowance;
                        APP.BackpayOthers = BackpayOthers;
                        APP.BackpayUniformTimecard = BackpayUniformTimecard;

                        APP.SessionID = m_SessionID;
                        APP.TransactionDate = UploadDateTime;

                        // check record existence in batch
                        DBFilter m_dupFilter = new DBFilter();
                        m_dupFilter.add(new Match("AttendancePreparationProcessID", APP.AttendancePreparationProcessID));
                        m_dupFilter.add(new Match("EmpRPID", APP.EmpRPID));

                        ArrayList m_existRecordList = EEmpAttendancePreparationProcess.db.select(dbConn, m_dupFilter);
                        //ArrayList m_existRecordList = EAttendancePreparationProcess.db.select(dbConn, m_dupFilter);
                        if (m_existRecordList.Count <= 0)
                            APP.ImportActionStatus = ImportDBObject.ImportActionEnum.INSERT;
                        else
                        {
                            APP.EmpAPPID = ((EEmpAttendancePreparationProcess)m_existRecordList[0]).EmpAPPID;
                            APP.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;
                        }
                        db.insert(dbConn, APP);
                    }
                    rowCount++;
                }
            }
            catch (Exception e)
            {
                errors.addError(e.Message, null);
            }
            if (errors.List.Count > 0)
            {
                ClearTempTable();
                throw (new HRImportException(rawDataTable.TableName + "\r\n" + errors.Message()));
            }
            return GetImportDataFromTempDatabase(null);

        }

        public DataTable UploadToTempDatabase(string Filename, int UserID, string ZipPassword, int gPID)
        {
            this.gPID = gPID;
            processID = gPID;
            return UploadToTempDatabase(Filename, UserID, ZipPassword);
        }

        public override DataTable UploadToTempDatabase(string Filename, int UserID, string ZipPassword)
        {
            ClearTempTable();
            DataTable rawDataTable = this.parse(Filename, ZipPassword, string.Empty).Tables[0];
            return UploadToTempDatabase(rawDataTable, UserID, true);
        }

        public override DataTable GetImportDataFromTempDatabase(ListInfo info)
        {
            DBFilter sessionFilter = new DBFilter();
            sessionFilter.add(new Match("SessionID", m_SessionID));
            sessionFilter.add(new MatchField("c.EmpID", "e.EmpID"));

            return sessionFilter.loadData(dbConn, null, "e.*, c.* ", " from " + db.dbclass.tableName + " c, " + EEmpPersonalInfo.db.dbclass.tableName + " e");
        }

        public static void ClearTempTable(DatabaseConnection dbConn, string sessionID)
        {
            ImportAttendancePreparationProcess import = new ImportAttendancePreparationProcess(dbConn, sessionID, 0);
            import.ClearTempTable();
        }

        public override void ClearTempTable()
        {
            //  Clear Old Import Session
            DBFilter sessionFilter = new DBFilter();
            if (!string.IsNullOrEmpty(m_SessionID))
                sessionFilter.add(new Match("SessionID", m_SessionID));
            EUploadAttendancePreparationProcess.db.delete(dbConn, sessionFilter);
        }

        public bool ClearUploadedData(PageErrors pErrors, int processid)
        {
            DBFilter m_filter = new DBFilter();
            m_filter.add(new Match("AttendancePreparationProcessID", processid));
            if (EEmpAttendancePreparationProcess.db.delete(dbConn, m_filter))
            {
                EAttendancePreparationProcess m_process = EAttendancePreparationProcess.GetObject(dbConn, processid);
                if (m_process != null)
                {
                    m_process.AttendancePreparationProcessEmpCount = 0;
                    EAttendancePreparationProcess.db.update(dbConn, m_process);
                }
            }
            return true;
        }

        public bool ConfirmAttendanceProcess(PageErrors pErrors)
        {
            EAttendancePreparationProcess m_process = EAttendancePreparationProcess.GetObject(dbConn, m_ProcessID);

            if (m_process.AttendancePreparationProcessStatus == EAttendancePreparationProcess.STATUS_NORMAL)
            {
                m_process.AttendancePreparationProcessStatus = EAttendancePreparationProcess.STATUS_CONFIRMED;
                return (EAttendancePreparationProcess.db.update(dbConn, m_process));
            }
            return false;
        }

        public override void ImportToDatabase()
        {
            DataTable dataTable = GetImportDataFromTempDatabase(null);

            if (dataTable.Rows.Count > 0)
            {
                EAttendancePreparationImportBatch batchDetail = new EAttendancePreparationImportBatch();
                batchDetail.AttendancePreparationImportBatchDateTime = AppUtils.ServerDateTime();
                batchDetail.AttendancePreparationImportBatchRemark = Remark;
                batchDetail.AttendancePreparationImportBatchUploadedBy = m_UserID;
                EAttendancePreparationImportBatch.db.insert(dbConn, batchDetail);

                foreach (DataRow row in dataTable.Rows)
                {
                    EUploadAttendancePreparationProcess obj = new EUploadAttendancePreparationProcess();
                    EUploadAttendancePreparationProcess.db.toObject(row, obj);

                    EEmpAttendancePreparationProcess attendance = new EEmpAttendancePreparationProcess();
                    attendance.EmpID = obj.EmpID;
                    attendance.AttendancePreparationProcessID = obj.AttendancePreparationProcessID;
                    attendance.EmpRPID = obj.EmpRPID;
                    attendance.Day1 = obj.Day1;
                    attendance.Day2 = obj.Day2;
                    attendance.Day3 = obj.Day3;
                    attendance.Day4 = obj.Day4;
                    attendance.Day5 = obj.Day5;
                    attendance.Day6 = obj.Day6;
                    attendance.Day7 = obj.Day7;
                    attendance.Day8 = obj.Day8;
                    attendance.Day9 = obj.Day9;
                    attendance.Day10 = obj.Day10;
                    attendance.Day11 = obj.Day11;
                    attendance.Day12 = obj.Day12;
                    attendance.Day13 = obj.Day13;
                    attendance.Day14 = obj.Day14;
                    attendance.Day15 = obj.Day15;
                    attendance.Day16 = obj.Day16;
                    attendance.Day17 = obj.Day17;
                    attendance.Day18 = obj.Day18;
                    attendance.Day19 = obj.Day19;
                    attendance.Day20 = obj.Day20;
                    attendance.Day21 = obj.Day21;
                    attendance.Day22 = obj.Day22;
                    attendance.Day23 = obj.Day23;
                    attendance.Day24 = obj.Day24;
                    attendance.Day25 = obj.Day25;
                    attendance.Day26 = obj.Day26;
                    attendance.Day27 = obj.Day27;
                    attendance.Day28 = obj.Day28;
                    attendance.Day29 = obj.Day29;
                    attendance.Day30 = obj.Day30;
                    attendance.Day31 = obj.Day31;

                    attendance.TotalHours = obj.TotalHours;
                    attendance.Remarks = obj.Remarks;
                    attendance.ReductionOthers = obj.ReductionOthers;
                    attendance.ReductionUniformTimecard = obj.ReductionUniformTimecard;
                    attendance.BackpayAllowance = obj.BackpayAllowance;
                    attendance.BackpayOthers = obj.BackpayOthers;
                    attendance.BackpayUniformTimecard = obj.BackpayUniformTimecard;
                    attendance.APPImportBatchID = batchDetail.AttendancePreparationImportBatchID;

                    DBFilter filter = new DBFilter();

                    filter.add(new Match("EmpID", attendance.EmpID));
                    filter.add(new Match("EmpRPID", attendance.EmpRPID));
                    ArrayList list = EEmpAttendancePreparationProcess.db.select(dbConn, filter);
                    if (list.Count > 0)
                    {
                        EEmpAttendancePreparationProcess temp = (EEmpAttendancePreparationProcess)list[0];
                        attendance.EmpAPPID = temp.EmpAPPID;
                        EEmpAttendancePreparationProcess.db.update(dbConn, attendance);
                    }
                    else
                    {
                        EEmpAttendancePreparationProcess.db.insert(dbConn, attendance);
                    }
                }
            }
        }

        public static void ExportTemplate(DatabaseConnection dbConn, ArrayList empList, string exportFileName, int PID)
        {
            DataTable tmpDataTable = new DataTable(TABLE_NAME);
            tmpDataTable.Columns.Add(FIELD_EMP_NO, typeof(string));
            tmpDataTable.Columns.Add(FIELD_CHINESE_FULL_NAME, typeof(string));
            tmpDataTable.Columns.Add(FIELD_ENGLISH_FULL_NAME, typeof(string));
            tmpDataTable.Columns.Add(FIELD_SHIFT_CODE, typeof(string));
            tmpDataTable.Columns.Add(FIELD_SHIFT_TIME, typeof(string));
            tmpDataTable.Columns.Add(FIELD_PAY_FORMULA_CODE, typeof(string));
            for (int i = 1; i <= 31; i++) {
                tmpDataTable.Columns.Add(i + "", typeof(string));
            }
            tmpDataTable.Columns.Add(FIELD_TOTAL_HOURS, typeof(int));
            tmpDataTable.Columns.Add(FIELD_EMPLOYEE_SIGNATURE, typeof(string));
            tmpDataTable.Columns.Add(FIELD_REMARK, typeof(string));
            tmpDataTable.Columns.Add(FIELD_BASIC_SALARY, typeof(double));
            tmpDataTable.Columns.Add(FIELD_ALLOWANCE_AMOUNT, typeof(double));
            tmpDataTable.Columns.Add(FIELD_TRAVEL_ALLOWANCE, typeof(double));
            tmpDataTable.Columns.Add(FIELD_REDUCTION_OTHERS, typeof(double));
            tmpDataTable.Columns.Add(FIELD_REDUCTION_UNIFORM_TIMECARD, typeof(double));
            tmpDataTable.Columns.Add(FIELD_BACKPAY_ALLOWANCE, typeof(double));
            tmpDataTable.Columns.Add(FIELD_BACKPAY_OTHERS, typeof(double));
            tmpDataTable.Columns.Add(FIELD_BACKPAY_UNIFORM_TIMECARD, typeof(double));

            EAttendancePreparationProcess attendanceProcess = EAttendancePreparationProcess.GetObject(dbConn, PID);

            intMonth = attendanceProcess.AttendancePreparationProcessMonth.Month;
            intYear = attendanceProcess.AttendancePreparationProcessMonth.Year;

            foreach (EEmpPersonalInfo empInfo in empList)
            {
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                {
                    DBFilter filter = new DBFilter();
                    filter.add(new Match("EmpID", empInfo.EmpID));
                    filter.add(new Match("EmpRPEffFr", "<=", attendanceProcess.AttendancePreparationProcessPeriodTo));

                    OR m_orEffTo = new OR();
                    m_orEffTo.add(new Match("EmpRPEffTo", ">=", attendanceProcess.AttendancePreparationProcessPeriodFr));
                    m_orEffTo.add(new NullTerm("EmpRPEffTo"));

                    filter.add(m_orEffTo);

                    DBFilter m_winsonRPFilter = new DBFilter();
                    m_winsonRPFilter.add(new Match("EmpRPShiftDutyID", ">", 0));
                    m_winsonRPFilter.add(new Match("EmpRPPayCalFormulaID", ">", 0));

                    filter.add(new IN("EmpRPID", "SELECT EmpRPID FROM EmpRPWinson", m_winsonRPFilter));

                    foreach (EEmpRecurringPayment recPayment in EEmpRecurringPayment.db.select(dbConn, filter))
                    {
                        DataRow row = tmpDataTable.NewRow();

                        row[FIELD_EMP_NO] = empInfo.EmpNo;
                        row[FIELD_CHINESE_FULL_NAME] = empInfo.EmpChiFullName;
                        row[FIELD_ENGLISH_FULL_NAME] = empInfo.EmpEngFullName;

                        EEmpRPWinson winson = EEmpRPWinson.GetObjectByRPID(dbConn, recPayment.EmpRPID);

                        if (winson != null)
                        {
                            EShiftDutyCode shiftCode = new EShiftDutyCode();
                            shiftCode.ShiftDutyCodeID = winson.EmpRPShiftDutyID;
                            if (EShiftDutyCode.db.select(dbConn, shiftCode))
                            {
                                row[FIELD_SHIFT_CODE] = shiftCode.ShiftDutyCode;
                                row[FIELD_SHIFT_TIME] = shiftCode.ShiftDutyFromTime.ToString("HH:mm") + "-" + shiftCode.ShiftDutyToTime.ToString("HH:mm");
                            }

                            EPaymentCalculationFormula payCalFormula = new EPaymentCalculationFormula();
                            payCalFormula.PayCalFormulaID = winson.EmpRPPayCalFormulaID;
                            if (EPaymentCalculationFormula.db.select(dbConn, payCalFormula))
                            {
                                row[FIELD_PAY_FORMULA_CODE] = payCalFormula.PayCalFormulaCode;
                            }
                        }

                        row[FIELD_BASIC_SALARY] = 0;
                        row[FIELD_ALLOWANCE_AMOUNT] = 0;
                        row[FIELD_TRAVEL_ALLOWANCE] = 0;

                        EPaymentCode m_payCode = EPaymentCode.GetObject(dbConn, recPayment.PayCodeID);
                        if (m_payCode != null)
                        {
                            if (m_payCode.PaymentCodeDesc == "底薪")
                            {
                                row[FIELD_BASIC_SALARY] = recPayment.EmpRPAmount;
                            }else if (m_payCode.PaymentCodeDesc == "津貼")
                            {
                                row[FIELD_ALLOWANCE_AMOUNT] = recPayment.EmpRPAmount;
                            }
                            else if (m_payCode.PaymentCodeDesc == "車津")
                            {
                                row[FIELD_TRAVEL_ALLOWANCE] = recPayment.EmpRPAmount;
                            }
                        }

                        tmpDataTable.Rows.Add(row);
                    }
                }
            }
            GenerateExcelReport(tmpDataTable, exportFileName, PID);
        }

        public static void GenerateExcelReport(DataTable tmpDataTable, string exportFileName, int PID)
        {
            int columnCount = 0;
            int lastRowIndex = 0;

            // Set column style
            NPOI.HSSF.UserModel.HSSFWorkbook workbook = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.HSSF.UserModel.HSSFSheet worksheet = (NPOI.HSSF.UserModel.HSSFSheet)workbook.CreateSheet(TABLE_NAME);
            NPOI.HSSF.UserModel.HSSFCellStyle numericStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            numericStyle.DataFormat = NPOI.HSSF.UserModel.HSSFDataFormat.GetBuiltinFormat("#,##0.00");
            numericStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.RIGHT;
            NPOI.HSSF.UserModel.HSSFCellStyle style = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.RIGHT;
            NPOI.SS.UserModel.IFont font = workbook.CreateFont();
            font.IsItalic = true;
            style.SetFont(font);
            NPOI.HSSF.UserModel.HSSFCellStyle leftStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            leftStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.LEFT;
            NPOI.HSSF.UserModel.HSSFCellStyle centerStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            centerStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;

            // Set column width
            worksheet.SetColumnWidth(0, 10 * 256);
            worksheet.SetColumnWidth(1, 15 * 256);
            worksheet.SetColumnWidth(2, 15 * 256);
            worksheet.SetColumnWidth(3, 8 * 256);
            worksheet.SetColumnWidth(4, 15 * 256);
            worksheet.SetColumnWidth(5, 10 * 256);
            worksheet.SetColumnWidth(37, 15 * 256);
            worksheet.SetColumnWidth(38, 15 * 256);
            worksheet.SetColumnWidth(39, 15 * 256);
            worksheet.SetColumnWidth(40, 15 * 256);
            worksheet.SetColumnWidth(41, 15 * 256);
            worksheet.SetColumnWidth(42, 15 * 256);
            worksheet.SetColumnWidth(43, 15 * 256);
            worksheet.SetColumnWidth(44, 15 * 256);
            worksheet.SetColumnWidth(45, 15 * 256);
            worksheet.SetColumnWidth(46, 15 * 256);
            worksheet.SetColumnWidth(47, 15 * 256);
            for (int i = 6; i <= 36; i++)
            {
                worksheet.SetColumnWidth(i, 3 * 256);
            }

            // Set column title
            NPOI.HSSF.UserModel.HSSFRow headerRow = (NPOI.HSSF.UserModel.HSSFRow)worksheet.CreateRow(lastRowIndex);
            headerRow.CreateCell(0).SetCellValue(FIELD_TITLE);
            headerRow = (NPOI.HSSF.UserModel.HSSFRow)worksheet.CreateRow(lastRowIndex + 1);
            headerRow.CreateCell(0).SetCellValue(FIELD_MONTH);
            NPOI.HSSF.UserModel.HSSFCell headerCell = (NPOI.HSSF.UserModel.HSSFCell)headerRow.CreateCell(1);
            headerCell.SetCellValue(intMonth);
            headerCell.CellStyle = leftStyle;

            headerRow = (NPOI.HSSF.UserModel.HSSFRow)worksheet.CreateRow(lastRowIndex + 2);
            headerRow.CreateCell(0).SetCellValue(FIELD_YEAR);
            headerCell = (NPOI.HSSF.UserModel.HSSFCell)headerRow.CreateCell(1);
            headerCell.SetCellValue(intYear);
            headerCell.CellStyle = leftStyle;

            // Merge cell from 43-44
            NPOI.SS.Util.CellRangeAddress cellRangeAddress = new NPOI.SS.Util.CellRangeAddress(3, (short)3, 43, (short)44);
            worksheet.AddMergedRegion(cellRangeAddress);
            headerRow = (NPOI.HSSF.UserModel.HSSFRow)worksheet.CreateRow(3);
            headerCell = (NPOI.HSSF.UserModel.HSSFCell)headerRow.CreateCell(43);
            headerCell.SetCellValue(FIELD_REDUCTION_ITEM);
            headerCell.CellStyle = centerStyle;

            // Merge cell from 45-47
            cellRangeAddress = new NPOI.SS.Util.CellRangeAddress(3, (short)3, 45, (short)47);
            worksheet.AddMergedRegion(cellRangeAddress);
            headerCell = (NPOI.HSSF.UserModel.HSSFCell)headerRow.CreateCell(45);
            headerCell.SetCellValue(FIELD_BACKPAY_ITEM);
            headerCell.CellStyle = centerStyle;

            headerRow = (NPOI.HSSF.UserModel.HSSFRow)worksheet.CreateRow(lastRowIndex + 4);
            int count = 1;
            foreach (DataColumn headercolumn in tmpDataTable.Columns)
            {
                NPOI.HSSF.UserModel.HSSFCell cell = (NPOI.HSSF.UserModel.HSSFCell)headerRow.CreateCell(columnCount);
                cell.SetCellValue(headercolumn.ColumnName);
                if (columnCount >= 6 && columnCount <= 36 && count <= 31)
                {
                    cell.SetCellValue(count);
                    cell.CellStyle = style;
                    count++;
                }
                //if (columnCount == 46)
                //{
                //    cell.SetCellValue(FIELD_REDUCTION_OTHERS);
                //}
                //if (columnCount == 47)
                //{
                //    cell.SetCellValue(FIELD_REDUCTION_UNIFORM_TIMECARD);
                //}
                columnCount++;
            }

            // Set value for every row
            foreach (DataRow row in tmpDataTable.Rows)
            {
                NPOI.HSSF.UserModel.HSSFRow detailRow = (NPOI.HSSF.UserModel.HSSFRow)worksheet.CreateRow(lastRowIndex + 5);

                detailRow.CreateCell(0).SetCellValue(row[FIELD_EMP_NO].ToString());
                detailRow.CreateCell(1).SetCellValue(row[FIELD_CHINESE_FULL_NAME].ToString());
                detailRow.CreateCell(2).SetCellValue(row[FIELD_ENGLISH_FULL_NAME].ToString());
                detailRow.CreateCell(3).SetCellValue(row[FIELD_SHIFT_CODE].ToString());
                detailRow.CreateCell(4).SetCellValue(row[FIELD_SHIFT_TIME].ToString());
                detailRow.CreateCell(5).SetCellValue(row[FIELD_PAY_FORMULA_CODE].ToString());
                detailRow.CreateCell(37).SetCellValue(row[FIELD_TOTAL_HOURS].ToString());
                detailRow.CreateCell(38).SetCellValue(row[FIELD_EMPLOYEE_SIGNATURE].ToString());
                detailRow.CreateCell(39).SetCellValue(row[FIELD_REMARK].ToString());

                NPOI.HSSF.UserModel.HSSFCell cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(40);
                cell.SetCellValue((double)row[FIELD_BASIC_SALARY]);
                cell.CellStyle = numericStyle;

                cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(41);
                cell.SetCellValue((double)row[FIELD_ALLOWANCE_AMOUNT]);
                cell.CellStyle = numericStyle;

                cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(42);
                cell.SetCellValue((double)row[FIELD_TRAVEL_ALLOWANCE]);
                cell.CellStyle = numericStyle;

                cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(43);
                cell.SetCellValue(0);
                cell.CellStyle = numericStyle;

                cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(44);
                cell.SetCellValue(0);
                cell.CellStyle = numericStyle;

                cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(45);
                cell.SetCellValue(0);
                cell.CellStyle = numericStyle;

                cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(46);
                cell.SetCellValue(0);
                cell.CellStyle = numericStyle;

                cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(47);
                cell.SetCellValue(0);
                cell.CellStyle = numericStyle;

                lastRowIndex++;
            }

            System.IO.FileStream file = new System.IO.FileStream(exportFileName, System.IO.FileMode.Create);
            workbook.Write(file);
            file.Close();
        }

        // Export Calculated Attendance Record Report
        public static void ExportCalculatedTemplate(DatabaseConnection dbConn, ArrayList empList, string exportFileName, int PID)
        {
            DataTable tmpDataTable = new DataTable("CalculatedAttendanceRecord");
            tmpDataTable.Columns.Add(FIELD_EMP_NO, typeof(string));
            tmpDataTable.Columns.Add(FIELD_CHINESE_FULL_NAME, typeof(string));
            tmpDataTable.Columns.Add(FIELD_ENGLISH_FULL_NAME, typeof(string));
            tmpDataTable.Columns.Add(FIELD_SHIFT_CODE, typeof(string));
            tmpDataTable.Columns.Add(FIELD_SHIFT_TIME, typeof(string));
            tmpDataTable.Columns.Add(FIELD_PAY_FORMULA_CODE, typeof(string));
            for (int i = 1; i <= 31; i++)
            {
                tmpDataTable.Columns.Add(i + "", typeof(string));
            }
            tmpDataTable.Columns.Add(FIELD_TOTAL_HOURS, typeof(int));
            tmpDataTable.Columns.Add(FIELD_EMPLOYEE_SIGNATURE, typeof(string));
            tmpDataTable.Columns.Add(FIELD_REMARK, typeof(string));
            tmpDataTable.Columns.Add(FIELD_BASIC_SALARY, typeof(double));
            tmpDataTable.Columns.Add(FIELD_ALLOWANCE_AMOUNT, typeof(double));
            tmpDataTable.Columns.Add(FIELD_TRAVEL_ALLOWANCE, typeof(double));
            tmpDataTable.Columns.Add(FIELD_CURRENT_MONTH, typeof(int));
            tmpDataTable.Columns.Add(FIELD_WORK_YEAR, typeof(double));
            tmpDataTable.Columns.Add(FIELD_OTHER_WAGE, typeof(double));
            tmpDataTable.Columns.Add(FIELD_VOLUNTARY_OVERTIME, typeof(double));
            tmpDataTable.Columns.Add(FIELD_CASUAL_LEAVE, typeof(double));
            tmpDataTable.Columns.Add(FIELD_SICK_MATERNITY, typeof(double));
            tmpDataTable.Columns.Add(FIELD_NO_SICK_MATERNITY, typeof(double));
            tmpDataTable.Columns.Add(FIELD_NO_WORK, typeof(double));
            tmpDataTable.Columns.Add(FIELD_LEAVE_DAYS, typeof(double));
            tmpDataTable.Columns.Add(FIELD_OVERTIME_DAYS, typeof(double));
            tmpDataTable.Columns.Add(FIELD_BACKPAY, typeof(double));

            tmpDataTable.Columns.Add(FIELD_SICK_DAYS, typeof(double));
            tmpDataTable.Columns.Add(FIELD_SICK_BACKPAY, typeof(double));
            tmpDataTable.Columns.Add(FIELD_INJURY_DAYS, typeof(double));
            tmpDataTable.Columns.Add(FIELD_INJURY_BACKPAY, typeof(double));
            tmpDataTable.Columns.Add(FIELD_UNPAID_DAYS, typeof(double));
            tmpDataTable.Columns.Add(FIELD_UNPAID_REDUCTION, typeof(double));
            tmpDataTable.Columns.Add(FIELD_PAID_DAYS, typeof(double));
            tmpDataTable.Columns.Add(FIELD_PAID_BACKPAY, typeof(double));

            tmpDataTable.Columns.Add(FIELD_REDUCTION_OTHERS, typeof(double));
            tmpDataTable.Columns.Add(FIELD_REDUCTION_UNIFORM_TIMECARD, typeof(double));
            tmpDataTable.Columns.Add(FIELD_BACKPAY_ALLOWANCE, typeof(double));
            tmpDataTable.Columns.Add(FIELD_BACKPAY_OTHERS, typeof(double));
            tmpDataTable.Columns.Add(FIELD_BACKPAY_UNIFORM_TIMECARD, typeof(double));

            EAttendancePreparationProcess attendanceProcess = EAttendancePreparationProcess.GetObject(dbConn, PID);
            if (attendanceProcess != null)
            {
                intMonth = attendanceProcess.AttendancePreparationProcessMonth.Month;
                intYear = attendanceProcess.AttendancePreparationProcessMonth.Year;
            }

            DBFilter m_employeeFilter = new DBFilter();
            m_employeeFilter.add(new Match("AttendancePreparationProcessID", attendanceProcess.AttendancePreparationProcessID));
            m_employeeFilter.add("EmpID", true);

            foreach (EEmpAttendancePreparationProcess empAttendProcess in EEmpAttendancePreparationProcess.db.select(dbConn, m_employeeFilter))
            {
                EEmpPersonalInfo empInfo = EEmpPersonalInfo.GetObject(dbConn, empAttendProcess.EmpID);

                EEmpRPWinson winson = EEmpRPWinson.GetObjectByRPID(dbConn, empAttendProcess.EmpRPID);

                EShiftDutyCode shiftCode = EShiftDutyCode.GetObject(dbConn, winson.EmpRPShiftDutyID);

                EPaymentCalculationFormula payCalFormula = EPaymentCalculationFormula.GetObject(dbConn, winson.EmpRPPayCalFormulaID);

                EEmpRecurringPayment recPayment = EEmpRecurringPayment.GetObject(dbConn, empAttendProcess.EmpRPID);

                //if (empInfo != null && winson != null && shiftCode != null && payCalFormula != null)
                {

                    //DBFilter filter = new DBFilter();
                    //filter.add(new Match("EmpID", empInfo.EmpID));
                    //ArrayList recPayList = EEmpRecurringPayment.db.select(dbConn, filter);

                    //foreach (EEmpRecurringPayment recPayment in recPayList)
                    //{
                    DataRow row = tmpDataTable.NewRow();

                    row[FIELD_EMP_NO] = empInfo.EmpNo;
                    row[FIELD_CHINESE_FULL_NAME] = empInfo.EmpChiFullName;
                    row[FIELD_ENGLISH_FULL_NAME] = empInfo.EmpEngFullName;
                    row[FIELD_SHIFT_CODE] = shiftCode.ShiftDutyCode;
                    row[FIELD_SHIFT_TIME] = shiftCode.ShiftDutyFromTime.ToString("HH:mm") + "-" + shiftCode.ShiftDutyToTime.ToString("HH:mm");            
                    row[FIELD_PAY_FORMULA_CODE] = payCalFormula.PayCalFormulaCode;

                    row[DAY_1] = empAttendProcess.Day1;
                    row[DAY_2] = empAttendProcess.Day2;
                    row[DAY_3] = empAttendProcess.Day3;
                    row[DAY_4] = empAttendProcess.Day4;
                    row[DAY_5] = empAttendProcess.Day5;
                    row[DAY_6] = empAttendProcess.Day6;
                    row[DAY_7] = empAttendProcess.Day7;
                    row[DAY_8] = empAttendProcess.Day8;
                    row[DAY_9] = empAttendProcess.Day9;
                    row[DAY_10] = empAttendProcess.Day10;
                    row[DAY_11] = empAttendProcess.Day11;
                    row[DAY_12] = empAttendProcess.Day12;
                    row[DAY_13] = empAttendProcess.Day13;
                    row[DAY_14] = empAttendProcess.Day14;
                    row[DAY_15] = empAttendProcess.Day15;
                    row[DAY_16] = empAttendProcess.Day16;
                    row[DAY_17] = empAttendProcess.Day17;
                    row[DAY_18] = empAttendProcess.Day18;
                    row[DAY_19] = empAttendProcess.Day19;
                    row[DAY_20] = empAttendProcess.Day20;
                    row[DAY_21] = empAttendProcess.Day21;
                    row[DAY_22] = empAttendProcess.Day22;
                    row[DAY_23] = empAttendProcess.Day23;
                    row[DAY_24] = empAttendProcess.Day24;
                    row[DAY_25] = empAttendProcess.Day25;
                    row[DAY_26] = empAttendProcess.Day26;
                    row[DAY_27] = empAttendProcess.Day27;
                    row[DAY_28] = empAttendProcess.Day28;
                    row[DAY_29] = empAttendProcess.Day29;
                    row[DAY_30] = empAttendProcess.Day30;
                    row[DAY_31] = empAttendProcess.Day31;

                    row[FIELD_TOTAL_HOURS] = empAttendProcess.TotalHours;
                    row[FIELD_REMARK] = empAttendProcess.Remarks;
                    row[FIELD_REDUCTION_OTHERS] = empAttendProcess.ReductionOthers;
                    row[FIELD_REDUCTION_UNIFORM_TIMECARD] = empAttendProcess.ReductionUniformTimecard;
                    row[FIELD_BACKPAY_ALLOWANCE] = empAttendProcess.BackpayAllowance;
                    row[FIELD_BACKPAY_OTHERS] = empAttendProcess.BackpayOthers;
                    row[FIELD_BACKPAY_UNIFORM_TIMECARD] = empAttendProcess.BackpayUniformTimecard;

                    //AR 該月總日數
                    int daysInMonth = DateTime.DaysInMonth(intYear, intMonth);
                    //double daysInMonth = (attendanceProcess.AttendancePreparationProcessPeriodTo - attendanceProcess.AttendancePreparationProcessPeriodFr).TotalDays;
                    //row[FIELD_CURRENT_MONTH] = daysInMonth;

                    double workYearDays = 0;            //AS 勞+年 "AL", "S", "AL/", "S/"
                    double otherWageDays = 0;           //AT 其他全薪 "FL" or "FL/"
                    double voluntaryOverTimeDays = 0;   //AU 例假自願加班 "T"or "T/"
                    double casualLeaveDays = 0;         //AV 例假 "R" or "R/"
                    double sickMaternityDays = 0;       //AW 病產傷假 "SP" or "SP/" or "IJ" or "IJ/" or "ML" or "ML/"
                    double noSickMaternityDays = 0;     //AX 病產事無 "SL", "SL/", "ML", "ML/"
                    double noWorkDays = 0;              //AY 沒有工作 "SL", "SL/", "ML", "ML/"
                    double totalLeaveDays = 0;          //AZ 總工作及假期日數 AR - SUM(AS:AY)
                    double sickLeaveDays = 0;           //BC 病假日數 "SP" or "SP/"
                    double injureLeaveDays = 0;         //BE 工傷日數 "IJ" or "IJ/"
                    double noPaidLeaveDays = 0;         //BG 無薪假日數 "NP" or "NP/" or "S-" or "S-/" or "ML" or "ML/" or "SL" OR "SL/" or "NA" or "NA/"
                    double paidLeaveDays = 0;           //BI 有薪年假日數 "AL" or "AL/"

                    ArrayList daysList = new ArrayList();
                    for (int i = 1; i <= 31; i++)
                    {
                        daysList.Add(row[i.ToString("0")].ToString());
                    }
                    #region "days processing"
                    foreach (string d in daysList)
                    {
                        switch (d)
                        {
                            case AL:
                                workYearDays++;
                                paidLeaveDays++;
                                break;
                            case ALH:
                                workYearDays = workYearDays + 0.5;
                                paidLeaveDays = paidLeaveDays + 0.5;
                                break;
                            case S:
                                workYearDays++;
                                break;
                            case SH:
                                workYearDays = workYearDays + 0.5;
                                break;
                            case FL:
                                otherWageDays++;
                                break;
                            case FLH:
                                otherWageDays = otherWageDays + 0.5;
                                break;
                            case T:
                                voluntaryOverTimeDays++;
                                break;
                            case TH:
                                voluntaryOverTimeDays = voluntaryOverTimeDays + 0.5;
                                break;
                            case R:
                                casualLeaveDays++;
                                break;
                            case RH:
                                casualLeaveDays = casualLeaveDays + 0.5;
                                break;
                            case SP:
                                sickMaternityDays++;
                                sickLeaveDays++;
                                break;
                            case SPH:
                                sickMaternityDays = sickMaternityDays + 0.5;
                                sickLeaveDays = sickLeaveDays + 0.5;
                                break;
                            case IJ:
                                sickMaternityDays++;
                                injureLeaveDays++;
                                break;
                            case IJH:
                                sickMaternityDays = sickMaternityDays + 0.5;
                                injureLeaveDays = injureLeaveDays + 0.5;
                                break;
                            case ML:
                                sickMaternityDays++;
                                noSickMaternityDays++;
                                noPaidLeaveDays++;
                                break;
                            case MLH:
                                sickMaternityDays = sickMaternityDays + 0.5;
                                noSickMaternityDays = noSickMaternityDays + 0.5;
                                noPaidLeaveDays = noPaidLeaveDays + 0.5;
                                break;
                            case SL:
                                noSickMaternityDays++;
                                noPaidLeaveDays++;
                                break;
                            case SLH:
                                noSickMaternityDays = noSickMaternityDays + 0.5;
                                noPaidLeaveDays = noPaidLeaveDays + 0.5;
                                break;
                            case NA:
                                noWorkDays++;
                                noPaidLeaveDays++;
                                break;
                            case NAH:
                                noWorkDays = noWorkDays + 0.5;
                                noPaidLeaveDays = noPaidLeaveDays + 0.5;
                                break;
                            case DNA:
                                noWorkDays++;
                                break;
                            case DNAH:
                                noWorkDays = noWorkDays + 0.5;
                                break;
                            case NP:
                                noPaidLeaveDays++;
                                break;
                            case NPH:
                                noPaidLeaveDays = noPaidLeaveDays + 0.5;
                                break;
                            case SD:
                                noPaidLeaveDays++;
                                break;
                            case SDH:
                                noPaidLeaveDays = noPaidLeaveDays + 0.5;
                                break;
                        }
                    }
                    #endregion "days processing"

                    row[FIELD_WORK_YEAR] = workYearDays;
                    row[FIELD_OTHER_WAGE] = otherWageDays;
                    row[FIELD_VOLUNTARY_OVERTIME] = voluntaryOverTimeDays;
                    row[FIELD_CASUAL_LEAVE] = casualLeaveDays;
                    row[FIELD_SICK_MATERNITY] = sickMaternityDays;
                    row[FIELD_NO_SICK_MATERNITY] = noSickMaternityDays;
                    row[FIELD_NO_WORK] = noWorkDays;
                    row[FIELD_BASIC_SALARY] = 0;
                    row[FIELD_ALLOWANCE_AMOUNT] = 0;
                    row[FIELD_TRAVEL_ALLOWANCE] = 0;

                    double targetSalary = 0; //底薪
                    double bonusAmount = 0; //津貼
                    double vehicleAllowance = 0; //車津

                    EPaymentCode m_paymentCode = EPaymentCode.GetObject(dbConn, recPayment.PayCodeID);
                    if (m_paymentCode != null)
                    {
                        switch (m_paymentCode.PaymentCodeDesc)
                        {
                            case "底薪":
                                targetSalary = recPayment.EmpRPAmount;
                                row[FIELD_BASIC_SALARY] = recPayment.EmpRPAmount;
                                break;
                            case "津貼":
                                bonusAmount = recPayment.EmpRPAmount;
                                row[FIELD_ALLOWANCE_AMOUNT] = recPayment.EmpRPAmount;
                                break;
                            case "車津":
                                vehicleAllowance = recPayment.EmpRPAmount;
                                row[FIELD_TRAVEL_ALLOWANCE] = recPayment.EmpRPAmount;
                                break;
                            default:
                                break;
                        }
                    }




                    double totalSalary = targetSalary + bonusAmount + vehicleAllowance; //底薪+津貼+車津

                    string holidayCode = row[FIELD_PAY_FORMULA_CODE].ToString(); //假期編號
                    double overTimeDays = 0; //額外工作日數
                    double backPay = 0; //補薪
                    double sickBackpay = 0; //病假補薪
                    double injuryBackpay = 0; //工傷補薪
                    double unpaidReduction = 0; //無薪假扣薪
                    double paidBackpay = 0; //有薪年假補薪

                    double reductionOthers = empAttendProcess.ReductionOthers; // 扣薪項其它
                    double reductionUniformTimecard = empAttendProcess.ReductionUniformTimecard; // 扣薪項製服/工咭
                    double backpayAllowance = empAttendProcess.BackpayAllowance; // 補薪項補津
                    double backpayOthers = empAttendProcess.BackpayOthers; // 補薪項其它
                    double backpayUniformTimecard = empAttendProcess.BackpayUniformTimecard; // 補薪項製服/工咭

                    if (injureLeaveDays > 30) //IJ 的總數最多30日
                        injureLeaveDays = 30;

                    switch (holidayCode)
                    {
                        case SEVEN_N: // 1/7N
                            overTimeDays = voluntaryOverTimeDays; // T 的總數
                            backPay = totalSalary / 26 * overTimeDays; // 補薪 = (底薪+津貼+車津)/ 26 X 額外工作日數
                            sickBackpay = totalSalary / 30 * 4 / 5 * sickLeaveDays; // 病假補薪 = (底薪+津貼+車津) / 30 X 4 / 5 X ( SP 的總數)
                            injuryBackpay = totalSalary / 30 * 4 / 5 * injureLeaveDays; //工傷補薪 = (底薪+津貼+車津) / 30 X 4 / 5 X ( IJ 的總數最多30日)
                            unpaidReduction = totalSalary / 26 * (-1) * noPaidLeaveDays; // 無薪假 = (底薪+津貼+車津) / 26 X -1 X (Column BG  無薪假日數)
                            paidBackpay = totalSalary / 26 * paidLeaveDays; //有薪年假補薪 = (底薪+津貼+車津) / 26 X ( AL 總數)
                            break;
                        case SEVEN_Y: // 1/7Y
                            overTimeDays = voluntaryOverTimeDays; // T 的總數
                            backPay = totalSalary / daysInMonth * overTimeDays; // 補薪 = (底薪+津貼+車津)/(當月日數) X 額外工作日數
                            sickBackpay = totalSalary / 30 * 4 / 5 * sickLeaveDays; // 病假補薪 = (底薪+津貼+車津) / 30 X 4 / 5 X ( SP 的總數)
                            injuryBackpay = totalSalary / 30 * 4 / 5 * injureLeaveDays; //工傷補薪 = (底薪+津貼+車津) / 30 X 4 / 5 X ( IJ 的總數最多30日)
                            unpaidReduction = totalSalary / daysInMonth * (-1) * noPaidLeaveDays; // 無薪假 = (底薪+津貼+車津) / 當月日數 X -1 X (Column BG  無薪假日數)
                            paidBackpay = totalSalary / 30 * paidLeaveDays; //有薪年假補薪 = (底薪+津貼+車津) / 30 X ( AL 總數)
                            break;
                        case DAILY_PAY: //日薪
                            overTimeDays = daysInMonth - voluntaryOverTimeDays - casualLeaveDays - sickMaternityDays - noSickMaternityDays - noWorkDays; // 本月日數 – 例假自願加班 – 例假 –病產傷假 -病產事無 -沒有工作
                            backPay = totalSalary * overTimeDays; // 補薪 = (底薪+津貼+車津) X 額外工作日數
                            sickBackpay = totalSalary * 4 / 5 * sickLeaveDays; // 病假補薪 = 底薪X 4 / 5 X ( SP 的總數)
                            injuryBackpay = totalSalary * 4 / 5 * injureLeaveDays; //工傷補薪 = (底薪+津貼+車津) X 4 / 5 ( IJ 的總數最多30日)
                            paidBackpay = totalSalary * paidLeaveDays; //有薪年假補薪 = (底薪+津貼+車津) X ( AL 總數)
                            break;
                        case SEVEN_P: // 1/7P
                            overTimeDays = voluntaryOverTimeDays; // T 的總數
                            backPay = totalSalary / 26 * overTimeDays; // 補薪 = (底薪+津貼+車津)/ 26 X 額外工作日數
                            sickBackpay = totalSalary * 4 / 5 * sickLeaveDays; // 病假補薪 = 底薪X 4 / 5 X ( SP 的總數)
                            injuryBackpay = totalSalary / 30 * 4 / 5 * injureLeaveDays; //工傷補薪 = (底薪+津貼+車津) / 30 X 4 / 5 X ( IJ 的總數最多30日)
                            unpaidReduction = totalSalary / 26 * (-1) * noPaidLeaveDays; // 無薪假 = (底薪+津貼+車津) / 26 X -1 X (Column BG 無薪假日數)
                            paidBackpay = totalSalary / 26 * paidLeaveDays; //有薪年假補薪 = (底薪+津貼+車津) / 26 X ( AL 總數)
                            break;
                        case SIX_DAYS: // 六日
                            overTimeDays = daysInMonth - voluntaryOverTimeDays - casualLeaveDays - sickMaternityDays - noSickMaternityDays - noWorkDays; // 額外工作日數 = 本月日數 – 例假自願加班 – 例假 –病產傷假 -病產事無 -沒有工作
                            backPay = totalSalary / daysInMonth * overTimeDays; // 補薪 = (底薪+津貼+車津)/(當月日數) X 額外工作日數
                            sickBackpay = totalSalary * 4 / 5 * sickLeaveDays; // 病假補薪 = 底薪X 4 / 5 X ( SP 的總數)
                            injuryBackpay = totalSalary / 30 * 4 / 5 * injureLeaveDays; //工傷補薪 = (底薪+津貼+車津) / 30 X 4 / 5 X ( IJ 的總數最多30日)
                            unpaidReduction = totalSalary / daysInMonth * (-1) * noPaidLeaveDays; // 無薪假 = (底薪+津貼+車津) / 當月日數 X -1 X (Column BG  無薪假日數)
                            paidBackpay = totalSalary / 30 * paidLeaveDays; //有薪年假補薪 = (底薪+津貼+車津) / 30 X ( AL 總數)
                            break;
                        case DAILY_PAY_BONUS: // 日/P
                            overTimeDays = daysInMonth - voluntaryOverTimeDays - casualLeaveDays - sickMaternityDays - noSickMaternityDays - noWorkDays; // 額外工作日數 = 本月日數 – 例假自願加班 – 例假 –病產傷假 -病產事無 -沒有工作
                            backPay = totalSalary * overTimeDays; // 補薪 = (底薪+津貼+車津) X 額外工作日數
                            sickBackpay = totalSalary * 4 / 5 * sickLeaveDays; // 病假補薪 = 底薪X 4 / 5 X ( SP 的總數)
                            paidBackpay = totalSalary * paidLeaveDays; //有薪年假補薪 = (底薪+津貼+車津) X ( AL 總數)
                            break;
                    }

                    // AZ 總工作及假期日數
                    totalLeaveDays = daysInMonth - (workYearDays + otherWageDays + voluntaryOverTimeDays + casualLeaveDays + sickMaternityDays + noSickMaternityDays + noWorkDays);

                    //CNDAmount for Generate Claims And Deduction template
                    double CNDAmount = backPay + sickBackpay + injuryBackpay + unpaidReduction + paidBackpay + reductionOthers + reductionUniformTimecard + backpayAllowance + backpayOthers + backpayUniformTimecard;

                    row[FIELD_LEAVE_DAYS] = totalLeaveDays;
                    row[FIELD_OVERTIME_DAYS] = overTimeDays;
                    row[FIELD_BACKPAY] = backPay;
                    row[FIELD_SICK_DAYS] = sickLeaveDays;
                    row[FIELD_SICK_BACKPAY] = sickBackpay;
                    row[FIELD_INJURY_DAYS] = injureLeaveDays;
                    row[FIELD_INJURY_BACKPAY] = injuryBackpay;
                    row[FIELD_UNPAID_DAYS] = noPaidLeaveDays;
                    row[FIELD_UNPAID_REDUCTION] = unpaidReduction;
                    row[FIELD_PAID_DAYS] = paidLeaveDays;
                    row[FIELD_PAID_BACKPAY] = paidBackpay;

                    tmpDataTable.Rows.Add(row);


                    // Calculate CNDAmount and save to db
                    //EEmpAttendancePreparationProcess obj = new EEmpAttendancePreparationProcess();
                    //obj.EmpAPPID = empAttendProcess.EmpAPPID;
                    //obj.EmpID = empAttendProcess.EmpID;
                    //obj.AttendancePreparationProcessID = empAttendProcess.AttendancePreparationProcessID;
                    //obj.EmpRPID = empAttendProcess.EmpRPID;
                    //obj.Day1 = empAttendProcess.Day1;
                    //obj.Day2 = empAttendProcess.Day2;
                    //obj.Day3 = empAttendProcess.Day3;
                    //obj.Day4 = empAttendProcess.Day4;
                    //obj.Day5 = empAttendProcess.Day5;
                    //obj.Day6 = empAttendProcess.Day6;
                    //obj.Day7 = empAttendProcess.Day7;
                    //obj.Day8 = empAttendProcess.Day8;
                    //obj.Day9 = empAttendProcess.Day9;
                    //obj.Day10 = empAttendProcess.Day10;
                    //obj.Day11 = empAttendProcess.Day11;
                    //obj.Day12 = empAttendProcess.Day12;
                    //obj.Day13 = empAttendProcess.Day13;
                    //obj.Day14 = empAttendProcess.Day14;
                    //obj.Day15 = empAttendProcess.Day15;
                    //obj.Day16 = empAttendProcess.Day16;
                    //obj.Day17 = empAttendProcess.Day17;
                    //obj.Day18 = empAttendProcess.Day18;
                    //obj.Day19 = empAttendProcess.Day19;
                    //obj.Day20 = empAttendProcess.Day20;
                    //obj.Day21 = empAttendProcess.Day21;
                    //obj.Day22 = empAttendProcess.Day22;
                    //obj.Day23 = empAttendProcess.Day23;
                    //obj.Day24 = empAttendProcess.Day24;
                    //obj.Day25 = empAttendProcess.Day25;
                    //obj.Day26 = empAttendProcess.Day26;
                    //obj.Day27 = empAttendProcess.Day27;
                    //obj.Day28 = empAttendProcess.Day28;
                    //obj.Day29 = empAttendProcess.Day29;
                    //obj.Day30 = empAttendProcess.Day30;
                    //obj.Day31 = empAttendProcess.Day31;
                    //obj.TotalHours = empAttendProcess.TotalHours;
                    //obj.Remarks = empAttendProcess.Remarks;
                    //obj.ReductionOthers = empAttendProcess.ReductionOthers;
                    //obj.ReductionUniformTimecard = empAttendProcess.ReductionUniformTimecard;
                    //obj.BackpayAllowance = empAttendProcess.BackpayAllowance;
                    //obj.BackpayOthers = empAttendProcess.BackpayOthers;
                    //obj.BackpayUniformTimecard = empAttendProcess.BackpayUniformTimecard;
                    //obj.CNDAmount = CNDAmount;
                    //obj.APPImportBatchID = empAttendProcess.APPImportBatchID;
                    //EEmpAttendancePreparationProcess.db.update(dbConn, obj);
                }
            }
            GenerateCalculatedReport(tmpDataTable, exportFileName, PID);
        }

        public static void GenerateCalculatedReport(DataTable tmpDataTable, string exportFileName, int PID)
        {
            int columnCount = 0;
            int lastRowIndex = 0;

            // Set column style
            NPOI.HSSF.UserModel.HSSFWorkbook workbook = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.HSSF.UserModel.HSSFSheet worksheet = (NPOI.HSSF.UserModel.HSSFSheet)workbook.CreateSheet(TABLE_NAME);
            NPOI.HSSF.UserModel.HSSFCellStyle numericStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            numericStyle.DataFormat = NPOI.HSSF.UserModel.HSSFDataFormat.GetBuiltinFormat("#,##0.00");
            numericStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.RIGHT;

            NPOI.HSSF.UserModel.HSSFCellStyle style = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.RIGHT;
            NPOI.SS.UserModel.IFont font = workbook.CreateFont();
            font.IsItalic = true;
            style.SetFont(font);

            NPOI.HSSF.UserModel.HSSFCellStyle leftStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            leftStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.LEFT;
            NPOI.HSSF.UserModel.HSSFCellStyle centerStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            centerStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;

            NPOI.HSSF.UserModel.HSSFCellStyle grey25Style = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            grey25Style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
            grey25Style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.GREY_25_PERCENT.index;
            grey25Style.FillPattern = NPOI.SS.UserModel.FillPatternType.SOLID_FOREGROUND;

            NPOI.HSSF.UserModel.HSSFCellStyle grey40Style = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            grey40Style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
            grey40Style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.GREY_40_PERCENT.index;
            grey40Style.FillPattern = NPOI.SS.UserModel.FillPatternType.SOLID_FOREGROUND;

            NPOI.HSSF.UserModel.HSSFCellStyle boldStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            NPOI.SS.UserModel.IFont boldFont = workbook.CreateFont();
            boldFont.Boldweight = 700;              
            boldStyle.SetFont(boldFont);

            // Set column width
            worksheet.SetColumnWidth(0, 10 * 256);
            worksheet.SetColumnWidth(1, 15 * 256);
            worksheet.SetColumnWidth(2, 15 * 256);
            worksheet.SetColumnWidth(3, 8 * 256);
            worksheet.SetColumnWidth(4, 15 * 256);
            worksheet.SetColumnWidth(5, 10 * 256);
            worksheet.SetColumnWidth(37, 15 * 256);
            worksheet.SetColumnWidth(38, 15 * 256);
            worksheet.SetColumnWidth(39, 15 * 256);
            worksheet.SetColumnWidth(40, 15 * 256);
            worksheet.SetColumnWidth(41, 15 * 256);
            worksheet.SetColumnWidth(42, 15 * 256);
            worksheet.SetColumnWidth(43, 15 * 256);
            worksheet.SetColumnWidth(44, 15 * 256);
            worksheet.SetColumnWidth(45, 15 * 256);
            worksheet.SetColumnWidth(46, 15 * 256);
            worksheet.SetColumnWidth(47, 15 * 256);
            for (int i = 6; i <= 36; i++)
            {
                worksheet.SetColumnWidth(i, 4 * 256);
            }

            // Set column title
            NPOI.HSSF.UserModel.HSSFRow headerRow = (NPOI.HSSF.UserModel.HSSFRow)worksheet.CreateRow(lastRowIndex);
            NPOI.HSSF.UserModel.HSSFCell headerCell = (NPOI.HSSF.UserModel.HSSFCell)headerRow.CreateCell(0);

            headerCell.SetCellValue("Calculated Attendance Record Report");
            headerCell.CellStyle = boldStyle;
            headerRow = (NPOI.HSSF.UserModel.HSSFRow)worksheet.CreateRow(lastRowIndex + 1);
            headerRow.CreateCell(0).SetCellValue(FIELD_MONTH);

            headerCell = (NPOI.HSSF.UserModel.HSSFCell)headerRow.CreateCell(1);
            headerCell.SetCellValue(intMonth);
            headerCell.CellStyle = leftStyle;

            headerRow = (NPOI.HSSF.UserModel.HSSFRow)worksheet.CreateRow(lastRowIndex + 2);
            headerRow.CreateCell(0).SetCellValue(FIELD_YEAR);
            headerCell = (NPOI.HSSF.UserModel.HSSFCell)headerRow.CreateCell(1);
            headerCell.SetCellValue(intYear);
            headerCell.CellStyle = leftStyle;

            // Merge cell from 54-55
            NPOI.SS.Util.CellRangeAddress cellRangeAddress = new NPOI.SS.Util.CellRangeAddress(3, (short)3, 54, (short)55);
            worksheet.AddMergedRegion(cellRangeAddress);
            headerRow = (NPOI.HSSF.UserModel.HSSFRow)worksheet.CreateRow(3);
            headerCell = (NPOI.HSSF.UserModel.HSSFCell)headerRow.CreateCell(54);
            headerCell.SetCellValue(FIELD_SICK_LEAVE);
            headerCell.CellStyle = grey25Style;

            // Merge cell from 56-57
            cellRangeAddress = new NPOI.SS.Util.CellRangeAddress(3, (short)3, 56, (short)57);
            worksheet.AddMergedRegion(cellRangeAddress);
            headerCell = (NPOI.HSSF.UserModel.HSSFCell)headerRow.CreateCell(56);
            headerCell.SetCellValue(FIELD_INJURY);
            headerCell.CellStyle = grey40Style;

            // Merge cell from 58-59
            cellRangeAddress = new NPOI.SS.Util.CellRangeAddress(3, (short)3, 58, (short)59);
            worksheet.AddMergedRegion(cellRangeAddress);
            headerCell = (NPOI.HSSF.UserModel.HSSFCell)headerRow.CreateCell(58);
            headerCell.SetCellValue(FIELD_UNPAID_LEAVE);
            headerCell.CellStyle = grey25Style;

            // Merge cell from 60-61
            cellRangeAddress = new NPOI.SS.Util.CellRangeAddress(3, (short)3, 60, (short)61);
            worksheet.AddMergedRegion(cellRangeAddress);
            headerCell = (NPOI.HSSF.UserModel.HSSFCell)headerRow.CreateCell(60);
            headerCell.SetCellValue(FIELD_PAID_LEAVE);
            headerCell.CellStyle = grey40Style;

            // Merge cell from 62-63
            cellRangeAddress = new NPOI.SS.Util.CellRangeAddress(3, (short)3, 62, (short)63);
            worksheet.AddMergedRegion(cellRangeAddress);
            headerCell = (NPOI.HSSF.UserModel.HSSFCell)headerRow.CreateCell(62);
            headerCell.SetCellValue(FIELD_REDUCTION_ITEM);
            headerCell.CellStyle = grey25Style;

            // Merge cell from 64-66
            cellRangeAddress = new NPOI.SS.Util.CellRangeAddress(3, (short)3, 64, (short)66);
            worksheet.AddMergedRegion(cellRangeAddress);
            headerCell = (NPOI.HSSF.UserModel.HSSFCell)headerRow.CreateCell(64);
            headerCell.SetCellValue(FIELD_BACKPAY_ITEM);
            headerCell.CellStyle = grey40Style;

            headerRow = (NPOI.HSSF.UserModel.HSSFRow)worksheet.CreateRow(lastRowIndex + 4);
            int count = 1;
            foreach (DataColumn headercolumn in tmpDataTable.Columns)
            {
                NPOI.HSSF.UserModel.HSSFCell cell = (NPOI.HSSF.UserModel.HSSFCell)headerRow.CreateCell(columnCount);
                cell.SetCellValue(headercolumn.ColumnName);
                if (columnCount >= 6 && columnCount <= 36 && count <= 31)
                {
                    cell.SetCellValue(count);
                    cell.CellStyle = style;
                    count++;
                }
                if (columnCount == 53)
                {
                    cell.SetCellValue(FIELD_BACKPAY);
                }
                if (columnCount == 55)
                {
                    cell.SetCellValue(FIELD_BACKPAY);
                }
                if (columnCount == 56)
                {
                    cell.SetCellValue(FIELD_SICK_DAYS);
                }
                if (columnCount == 57)
                {
                    cell.SetCellValue(FIELD_BACKPAY);
                }
                if (columnCount == 58)
                {
                    cell.SetCellValue(FIELD_SICK_DAYS);
                }
                if (columnCount == 60)
                {
                    cell.SetCellValue(FIELD_SICK_DAYS);
                }
                if (columnCount == 61)
                {
                    cell.SetCellValue(FIELD_BACKPAY);
                }
                if (columnCount == 65)
                {
                    cell.SetCellValue(FIELD_REDUCTION_OTHERS);
                }
                if (columnCount == 66)
                {
                    cell.SetCellValue(FIELD_REDUCTION_UNIFORM_TIMECARD);
                }
                columnCount++;
            }

            // Set value for every row
            foreach (DataRow row in tmpDataTable.Rows)
            {
                NPOI.HSSF.UserModel.HSSFRow detailRow = (NPOI.HSSF.UserModel.HSSFRow)worksheet.CreateRow(lastRowIndex + 5);

                detailRow.CreateCell(0).SetCellValue(row[FIELD_EMP_NO].ToString());
                detailRow.CreateCell(1).SetCellValue(row[FIELD_CHINESE_FULL_NAME].ToString());
                detailRow.CreateCell(2).SetCellValue(row[FIELD_ENGLISH_FULL_NAME].ToString());
                detailRow.CreateCell(3).SetCellValue(row[FIELD_SHIFT_CODE].ToString());
                detailRow.CreateCell(4).SetCellValue(row[FIELD_SHIFT_TIME].ToString());
                detailRow.CreateCell(5).SetCellValue(row[FIELD_PAY_FORMULA_CODE].ToString());

                detailRow.CreateCell(6).SetCellValue(row[DAY_1].ToString());
                detailRow.CreateCell(7).SetCellValue(row[DAY_2].ToString());
                detailRow.CreateCell(8).SetCellValue(row[DAY_3].ToString());
                detailRow.CreateCell(9).SetCellValue(row[DAY_4].ToString());
                detailRow.CreateCell(10).SetCellValue(row[DAY_5].ToString());
                detailRow.CreateCell(11).SetCellValue(row[DAY_6].ToString());
                detailRow.CreateCell(12).SetCellValue(row[DAY_7].ToString());
                detailRow.CreateCell(13).SetCellValue(row[DAY_8].ToString());
                detailRow.CreateCell(14).SetCellValue(row[DAY_9].ToString());
                detailRow.CreateCell(15).SetCellValue(row[DAY_10].ToString());
                detailRow.CreateCell(16).SetCellValue(row[DAY_11].ToString());
                detailRow.CreateCell(17).SetCellValue(row[DAY_12].ToString());
                detailRow.CreateCell(18).SetCellValue(row[DAY_13].ToString());
                detailRow.CreateCell(19).SetCellValue(row[DAY_14].ToString());
                detailRow.CreateCell(20).SetCellValue(row[DAY_15].ToString());
                detailRow.CreateCell(21).SetCellValue(row[DAY_16].ToString());
                detailRow.CreateCell(22).SetCellValue(row[DAY_17].ToString());
                detailRow.CreateCell(23).SetCellValue(row[DAY_18].ToString());
                detailRow.CreateCell(24).SetCellValue(row[DAY_19].ToString());
                detailRow.CreateCell(25).SetCellValue(row[DAY_20].ToString());
                detailRow.CreateCell(26).SetCellValue(row[DAY_21].ToString());
                detailRow.CreateCell(27).SetCellValue(row[DAY_22].ToString());
                detailRow.CreateCell(28).SetCellValue(row[DAY_23].ToString());
                detailRow.CreateCell(29).SetCellValue(row[DAY_24].ToString());
                detailRow.CreateCell(30).SetCellValue(row[DAY_25].ToString());
                detailRow.CreateCell(31).SetCellValue(row[DAY_26].ToString());
                detailRow.CreateCell(32).SetCellValue(row[DAY_27].ToString());
                detailRow.CreateCell(33).SetCellValue(row[DAY_28].ToString());
                detailRow.CreateCell(34).SetCellValue(row[DAY_29].ToString());
                detailRow.CreateCell(35).SetCellValue(row[DAY_30].ToString());
                detailRow.CreateCell(36).SetCellValue(row[DAY_31].ToString());

                detailRow.CreateCell(37).SetCellValue(row[FIELD_TOTAL_HOURS].ToString());
                detailRow.CreateCell(38).SetCellValue(row[FIELD_EMPLOYEE_SIGNATURE].ToString());
                detailRow.CreateCell(39).SetCellValue(row[FIELD_REMARK].ToString());

                NPOI.HSSF.UserModel.HSSFCell cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(40);
                cell.SetCellValue((double)row[FIELD_BASIC_SALARY]);
                cell.CellStyle = numericStyle;

                cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(41);
                cell.SetCellValue((double)row[FIELD_ALLOWANCE_AMOUNT]);
                cell.CellStyle = numericStyle;

                cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(42);
                cell.SetCellValue((double)row[FIELD_TRAVEL_ALLOWANCE]);
                cell.CellStyle = numericStyle;

                detailRow.CreateCell(43).SetCellValue((int)row[FIELD_CURRENT_MONTH]);
                detailRow.CreateCell(44).SetCellValue((double)row[FIELD_WORK_YEAR]);
                detailRow.CreateCell(45).SetCellValue((double)row[FIELD_OTHER_WAGE]);
                detailRow.CreateCell(46).SetCellValue((double)row[FIELD_VOLUNTARY_OVERTIME]);
                detailRow.CreateCell(47).SetCellValue((double)row[FIELD_CASUAL_LEAVE]);
                detailRow.CreateCell(48).SetCellValue((double)row[FIELD_SICK_MATERNITY]);
                detailRow.CreateCell(49).SetCellValue((double)row[FIELD_NO_SICK_MATERNITY]);
                detailRow.CreateCell(50).SetCellValue((double)row[FIELD_NO_WORK]);
                detailRow.CreateCell(51).SetCellValue((double)row[FIELD_LEAVE_DAYS]);

                detailRow.CreateCell(52).SetCellValue((double)row[FIELD_OVERTIME_DAYS]);
                cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(53);
                cell.SetCellValue((double)row[FIELD_BACKPAY]);
                cell.CellStyle = numericStyle;

                detailRow.CreateCell(54).SetCellValue((double)row[FIELD_SICK_DAYS]);
                cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(55);
                cell.SetCellValue((double)row[FIELD_SICK_BACKPAY]);
                cell.CellStyle = numericStyle;

                detailRow.CreateCell(56).SetCellValue((double)row[FIELD_INJURY_DAYS]);
                cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(57);
                cell.SetCellValue((double)row[FIELD_INJURY_BACKPAY]);
                cell.CellStyle = numericStyle;

                detailRow.CreateCell(58).SetCellValue((double)row[FIELD_UNPAID_DAYS]);
                cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(59);
                cell.SetCellValue((double)row[FIELD_UNPAID_REDUCTION]);
                cell.CellStyle = numericStyle;

                detailRow.CreateCell(60).SetCellValue((double)row[FIELD_PAID_DAYS]);
                cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(61);
                cell.SetCellValue((double)row[FIELD_PAID_BACKPAY]);
                cell.CellStyle = numericStyle;
                
                cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(62);                
                cell.SetCellValue((double)row[FIELD_REDUCTION_OTHERS]);
                cell.CellStyle = numericStyle;
                
                cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(63);
                cell.SetCellValue((double)row[FIELD_REDUCTION_UNIFORM_TIMECARD]);
                cell.CellStyle = numericStyle;

                cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(64);
                cell.SetCellValue((double)row[FIELD_BACKPAY_ALLOWANCE]);
                cell.CellStyle = numericStyle;

                cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(65);
                cell.SetCellValue((double)row[FIELD_BACKPAY_OTHERS]);
                cell.CellStyle = numericStyle;

                cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(66);
                cell.SetCellValue((double)row[FIELD_BACKPAY_UNIFORM_TIMECARD]);
                cell.CellStyle = numericStyle;

                lastRowIndex++;
            }

            System.IO.FileStream file = new System.IO.FileStream(exportFileName, System.IO.FileMode.Create);
            workbook.Write(file);
            file.Close();
        }

        public DataSet GenerateCND(DatabaseConnection dbConn, ArrayList empList, string exportFileName, int PID)
        {

            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable("ClaimsAndDeduction$");
            dataSet.Tables.Add(dataTable);

            dataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_EMP_NO, typeof(string));
            dataTable.Columns.Add("English Name", typeof(string));
            dataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_EFFECTIVE_DATE, typeof(DateTime));
            dataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_PAYMENT_CODE, typeof(string));
            dataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_PAYMENT_METHOD, typeof(string));
            dataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_BANK_ACCOUNT_NO, typeof(string));
            dataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_AMOUNT, typeof(double));
            dataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_NUM_OF_DAY_ADJUST, typeof(double));
            dataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_REST_PAYMENT, typeof(string));
            dataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_REMARK, typeof(string));
            dataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_COST_CENTER, typeof(string));
    
            EAttendancePreparationProcess attendanceProcess = EAttendancePreparationProcess.GetObject(dbConn, PID);
            DateTime PaymentDate = attendanceProcess.AttendancePreparationProcessPayDate;
            //int NoOfDay = DateTime.DaysInMonth(attendanceProcess.AttendancePreparationProcessMonth.Year, attendanceProcess.AttendancePreparationProcessMonth.Month);
            double NoOfDay = (attendanceProcess.AttendancePreparationProcessPeriodTo - attendanceProcess.AttendancePreparationProcessPeriodFr).TotalDays;

            DBFilter m_empAttendancePreparationProcessFilter = new DBFilter();
            m_empAttendancePreparationProcessFilter.add(new Match("AttendancePreparationProcessID", PID));
            m_empAttendancePreparationProcessFilter.add("EmpID", true);

            foreach(EEmpAttendancePreparationProcess m_empAttend in EEmpAttendancePreparationProcess.db.select(dbConn, m_empAttendancePreparationProcessFilter))
            {
                EEmpPersonalInfo empInfo = EEmpPersonalInfo.GetObject(dbConn, m_empAttend.EmpID);
                if (empInfo != null)
                {
                    EEmpRecurringPayment m_empRP = EEmpRecurringPayment.GetObject(dbConn, m_empAttend.EmpRPID);

                    if (m_empRP != null)
                    {
                        EPaymentCode m_paymentCode = EPaymentCode.GetObject(dbConn, m_empRP.PayCodeID);

                        DataRow m_row = dataTable.NewRow();
                        m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_EMP_NO] = empInfo.EmpNo;
                        m_row["English Name"] = empInfo.EmpEngFullName;
                        m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_EFFECTIVE_DATE] = PaymentDate;
                        m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_PAYMENT_CODE] = m_paymentCode.PaymentCode;
                        switch (m_empRP.EmpRPMethod)
                        {
                            case "A":
                                m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_PAYMENT_METHOD] = "Autopay";
                                break;
                            case "Q":
                                m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_PAYMENT_METHOD] = "Cheque";
                                break;
                            case "C":
                                m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_PAYMENT_METHOD] = "Cash";
                                break;
                            default:
                                m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_PAYMENT_METHOD] = "Other";
                                break;
                        }
                        m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_AMOUNT] = Math.Round(m_empAttend.CNDAmount, 2);
                        m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_NUM_OF_DAY_ADJUST] = NoOfDay;
                        m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_REST_PAYMENT] = "No";
                        m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_REMARK] = ""; // empAttendancePreparationProcess.Remarks;

                        EEmpBankAccount m_bank = EEmpBankAccount.GetObject(dbConn, m_empRP.EmpAccID);
                        if (m_bank != null)
                            m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_BANK_ACCOUNT_NO] = m_bank.EmpAccountNo;

                        ECostCenter m_costCenter = ECostCenter.GetObject(dbConn, m_empRP.CostCenterID);
                        if (m_costCenter != null)
                            m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_COST_CENTER] = m_costCenter.CostCenterCode;

                        dataTable.Rows.Add(m_row);
                    }
                }
            }

            return dataSet;
        }

        public int GetShiftDutyCodeID(DatabaseConnection dbConn, string NameOrCode, bool CreateIfNotExist, int CreateUserID)
        {
            if (string.IsNullOrEmpty(NameOrCode))
                return 0;

            string[] codeDescArray = GetCodeDescArray(NameOrCode);
            ArrayList list = EShiftDutyCode.db.select(dbConn, new DBFilter());
            //  Search code first
            foreach (EShiftDutyCode item in list)
            {
                if (item.ShiftDutyCode.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase))
                    return item.ShiftDutyCodeID;
            }

            foreach (EShiftDutyCode item in list)
            {
                if (item.ShiftDutyCodeDesc.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase) && codeDescArray[0] == codeDescArray[1])
                    return item.ShiftDutyCodeID;
            }
            //  Create Code if necessary
            string newCode = codeDescArray[0].Replace(" ", "_").PadRight(20).Substring(0, 20).Trim().ToUpper();
            foreach (EShiftDutyCode item in list)
            {
                if (item.ShiftDutyCode.Trim().Equals(newCode, StringComparison.CurrentCultureIgnoreCase))
                    return item.ShiftDutyCodeID;
            }
            if (CreateIfNotExist)
            {
                AppUtils.StartChildFunction(dbConn, "SYS025", 0);
                EShiftDutyCode newObj = new EShiftDutyCode();
                newObj.ShiftDutyCode = newCode;
                newObj.ShiftDutyCodeDesc = codeDescArray[1];

                EShiftDutyCode.db.insert(dbConn, newObj);
                AppUtils.EndChildFunction(dbConn);
                return newObj.ShiftDutyCodeID;
            }
            return 0;
        }

        public int GetPayCalFormulaID(DatabaseConnection dbConn, string NameOrCode, bool CreateIfNotExist, int CreateUserID)
        {
            if (string.IsNullOrEmpty(NameOrCode))
                return 0;

            string[] codeDescArray = GetCodeDescArray(NameOrCode);
            ArrayList list = EPaymentCalculationFormula.db.select(dbConn, new DBFilter());
            //  Search code first
            foreach (EPaymentCalculationFormula item in list)
            {
                if (item.PayCalFormulaCode.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase))
                    return item.PayCalFormulaID;
            }

            foreach (EPaymentCalculationFormula item in list)
            {
                if (item.PayCalFormulaCodeDesc.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase) && codeDescArray[0] == codeDescArray[1])
                    return item.PayCalFormulaID;
            }
            //  Create Code if necessary
            string newCode = codeDescArray[0].Replace(" ", "_").PadRight(20).Substring(0, 20).Trim().ToUpper();
            foreach (EPaymentCalculationFormula item in list)
            {
                if (item.PayCalFormulaCode.Trim().Equals(newCode, StringComparison.CurrentCultureIgnoreCase))
                    return item.PayCalFormulaID;
            }
            if (CreateIfNotExist)
            {
                AppUtils.StartChildFunction(dbConn, "SYS026", 0);
                EPaymentCalculationFormula newObj = new EPaymentCalculationFormula();
                newObj.PayCalFormulaCode = newCode;
                newObj.PayCalFormulaCodeDesc = codeDescArray[1];

                EPaymentCalculationFormula.db.insert(dbConn, newObj);
                AppUtils.EndChildFunction(dbConn);
                return newObj.PayCalFormulaID;
            }
            return 0;
        }

        private string[] GetCodeDescArray(string Name)
        {
            string[] codeDescArray = Name.Split(new string[] { "~" }, StringSplitOptions.None);
            if (codeDescArray.GetLength(0) >= 2)
                return new string[] { codeDescArray[0].Trim(), string.Join("~", codeDescArray, 1, codeDescArray.GetLength(0) - 1).Trim() };
            else if (codeDescArray.GetLength(0).Equals(1))
                return new string[] { codeDescArray[0].Trim(), codeDescArray[0].Trim() };
            else
                return new string[] { "", "" };
        }

         public DataSet parse(string ExcelFilePath, string ZipPassword, string FirstColumnName)
         {
            DataSet ds = new DataSet();
            if (System.IO.Path.GetExtension(ExcelFilePath).Equals(".zip", StringComparison.CurrentCultureIgnoreCase))
            {
                string strTmpFolder = ExcelFilePath + ".dir";

                try
                {
                    zip.ExtractAll(ExcelFilePath, strTmpFolder, ZipPassword);
                    System.IO.DirectoryInfo rootDir = new System.IO.DirectoryInfo(strTmpFolder);
                    foreach (System.IO.FileInfo fileInfo in rootDir.GetFiles("*", System.IO.SearchOption.AllDirectories))
                        ds.Merge(parse(fileInfo.FullName, ZipPassword, FirstColumnName));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    System.IO.Directory.Delete(strTmpFolder, true);
                }
            }
            else if (System.IO.Path.GetExtension(ExcelFilePath).Equals(".csv", StringComparison.CurrentCultureIgnoreCase))
            {
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(ExcelFilePath);
                DataTable table = CSVReader.parse(fileInfo.OpenRead(), true, ",", "\"");
                table.TableName = System.IO.Path.GetFileNameWithoutExtension(fileInfo.FullName);
                ds.Tables.Add(table);
            }
            else
            {
                NPOI.HSSF.UserModel.HSSFWorkbook workBook = new NPOI.HSSF.UserModel.HSSFWorkbook(new System.IO.FileStream(ExcelFilePath, System.IO.FileMode.Open)); 

                for (int sheetIndex = 0; sheetIndex < workBook.NumberOfSheets; sheetIndex++)
                {
                    if (!workBook.IsSheetHidden(sheetIndex))
                    {
                        int intHeaderRow = 4;
                        NPOI.HSSF.UserModel.HSSFSheet workSheet = (NPOI.HSSF.UserModel.HSSFSheet)workBook.GetSheetAt(sheetIndex);
                        NPOI.HSSF.UserModel.HSSFRow headerRow = null;

                        if (!string.IsNullOrEmpty(FirstColumnName))
                        {
                            for (int tmpRowIdx = intHeaderRow; tmpRowIdx <= workSheet.LastRowNum; tmpRowIdx++)
                            {
                                headerRow = (NPOI.HSSF.UserModel.HSSFRow)workSheet.GetRow(tmpRowIdx);
                                if (headerRow == null)
                                    continue;
                                bool columnNameMatch = false;
                                for (int tmpColumnIndex = 0; tmpColumnIndex <= headerRow.LastCellNum; tmpColumnIndex++)
                                {
                                    if (headerRow.GetCell(tmpColumnIndex) != null)
                                    {
                                        string columnName = headerRow.GetCell(tmpColumnIndex).ToString().Trim();
                                        if (FirstColumnName.Equals(columnName))
                                        {
                                            intHeaderRow = tmpRowIdx;
                                            columnNameMatch = true;
                                            break;
                                        }
                                    }
                                }
                                if (columnNameMatch)
                                    break;
                            }
                        }
                        else
                        {
                            headerRow = (NPOI.HSSF.UserModel.HSSFRow)workSheet.GetRow(intHeaderRow);
                        }

                        if (headerRow == null)
                            continue;
                        string tableName = workSheet.SheetName.Trim();
                        DataTable table = new DataTable(tableName);
                        int intColumnIndex = 0;
                        while (intColumnIndex <= headerRow.LastCellNum)
                        {
                            if (headerRow.GetCell(intColumnIndex) != null)
                            {
                                string columnName = headerRow.GetCell(intColumnIndex).ToString().Trim();
                                if (string.IsNullOrEmpty(columnName))
                                    columnName = "Column_" + intColumnIndex;
                                if (table.Columns.Contains(columnName))
                                    columnName = "Column_" + intColumnIndex;
                                table.Columns.Add(columnName, typeof(string));

                                //  resign new value of column name to Excel for below part of import 
                                headerRow.GetCell(intColumnIndex).SetCellValue(columnName);
                            }
                            intColumnIndex++;
                        }
                        int rowCount = 1;

                        while (intHeaderRow + rowCount <= workSheet.LastRowNum)
                        {
                            int colCount = 0;

                            NPOI.HSSF.UserModel.HSSFRow row = (NPOI.HSSF.UserModel.HSSFRow)workSheet.GetRow(intHeaderRow + rowCount);
                            if (row == null)
                            {
                                rowCount++;
                                continue;
                            }

                            DataRow dataRow = table.NewRow();
       
                            while (colCount <= headerRow.LastCellNum)
                            {
                                if (headerRow.GetCell(colCount) != null)
                                {
                                    string columnName = headerRow.GetCell(colCount).ToString();
                                    if (table.Columns.Contains(columnName))
                                    {
                                        NPOI.HSSF.UserModel.HSSFCell cell = (NPOI.HSSF.UserModel.HSSFCell)row.GetCell(colCount);
                                        if (cell != null)
                                        {
                                            if (cell.CellType.Equals(NPOI.SS.UserModel.CellType.FORMULA))
                                            {
                                                NPOI.HSSF.UserModel.HSSFFormulaEvaluator e = new NPOI.HSSF.UserModel.HSSFFormulaEvaluator(workBook);
                                                cell = (NPOI.HSSF.UserModel.HSSFCell)e.EvaluateInCell(cell);
                                            }
                                            string fieldValue = cell.ToString();
                                            if (cell.CellType.Equals(NPOI.SS.UserModel.CellType.NUMERIC))
                                            {
                                                string format = string.Empty;
                                                try
                                                {
                                                    format = NPOI.HSSF.UserModel.HSSFDataFormat.GetBuiltinFormat(cell.CellStyle.DataFormat);
                                                }
                                                catch
                                                {
                                                    format = workBook.CreateDataFormat().GetFormat(cell.CellStyle.DataFormat);
                                                }

                                                //  [h]:mm:ss handle NOT support
                                                int midBlanketStartPos = format.IndexOf('[');
                                                while (midBlanketStartPos >= 0)
                                                {
                                                    int midBlanketEndPos = format.IndexOf(']', midBlanketStartPos);
                                                    format = format.Substring(0, midBlanketStartPos) + format.Substring(midBlanketStartPos + 1, midBlanketEndPos - midBlanketStartPos - 1) + format.Substring(midBlanketEndPos + 1);
                                                    midBlanketStartPos = format.IndexOf('[');
                                                }

                                                if (format.IndexOf("y", StringComparison.CurrentCultureIgnoreCase) >= 0 || format.IndexOf("d", StringComparison.CurrentCultureIgnoreCase) >= 0)
                                                    if (format.IndexOf("h", StringComparison.CurrentCultureIgnoreCase) >= 0)
                                                        fieldValue = cell.DateCellValue.ToString("yyyy-MM-dd HH:mm:ss");
                                                    else
                                                    {
                                                        DateTime date = cell.DateCellValue;
                                                        if (date.TimeOfDay.TotalSeconds > 0)
                                                            fieldValue = date.ToString("yyyy-MM-dd HH:mm:ss");
                                                        else
                                                            fieldValue = date.ToString("yyyy-MM-dd");
                                                    }
                                                else if (format.IndexOf("h", StringComparison.CurrentCultureIgnoreCase) >= 0)
                                                {
                                                    DateTime date = cell.DateCellValue;

                                                    //  default date of "Time Only" field is 1899-12-31
                                                    if (!date.Date.Ticks.Equals(new DateTime(1899, 12, 31).Ticks))
                                                        fieldValue = cell.DateCellValue.ToString("yyyy-MM-dd HH:mm:ss");
                                                    else
                                                        fieldValue = cell.DateCellValue.ToString("HH:mm:ss");
                                                }
                                                else
                                                    fieldValue = cell.NumericCellValue.ToString();
                                            }
                                            dataRow[columnName] = fieldValue;
                                        }
                                    }
                                }
                                colCount++;
                            }
                            table.Rows.Add(dataRow);
                            rowCount++;
                        }
                        ds.Tables.Add(table);
                    }
                }
            }
            foreach (DataTable tempTable in ds.Tables)
            {
                for (int rowIdx = tempTable.Rows.Count - 1; rowIdx >= 0; rowIdx--)
                {
                    DataRow row = tempTable.Rows[rowIdx];
                    bool isEmptyRow = true;
                    foreach (DataColumn tempColumn in tempTable.Columns)
                    {
                        if (!row.IsNull(tempColumn))
                            if (!string.IsNullOrEmpty(row[tempColumn].ToString().Trim()))
                            {
                                isEmptyRow = false;
                                break;
                            }
                    }
                    if (isEmptyRow)
                        tempTable.Rows.Remove(row);
                    else
                        break;
                }
            }
            foreach (DataTable tempTable in ds.Tables)
            {
                foreach (DataColumn tempColumn in tempTable.Columns)
                {
                    string tempColumnName = tempColumn.ColumnName;
                    tempColumnName = tempColumnName.Trim().Replace("*", "");
                    tempColumnName = tempColumnName.Trim().Replace("#", "");
                    tempColumn.ColumnName = tempColumnName;
                }
            }
            return ds;
        }
    }
}