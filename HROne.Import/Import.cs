using System;
using System.Configuration;
using System.Collections;
using System.Globalization;
using HROne.Lib.Entities;
using HROne.DataAccess;
//using perspectivemind.validation;

namespace HROne.Import
{

    public static class ParseTemp
    {
        public static int GetUploadEmpID(DatabaseConnection dbConn, string EmpNo, string SessionID)
        {
            DBFilter dbfilter = new DBFilter();
            dbfilter.add(new Match("EmpNo", EmpNo));
            dbfilter.add(new Match("SessionID", SessionID));

            ArrayList list = EUploadEmpPersonalInfo.db.select(dbConn, dbfilter);
            if (list.Count > 0)
                return ((EUploadEmpPersonalInfo)list[0]).UploadEmpID;
            else
                return 0;
        }

        public static int GetEmpIDFromUploadEmpID(DatabaseConnection dbConn, int UploadEmpID)
        {
            EUploadEmpPersonalInfo uploadEmp = new EUploadEmpPersonalInfo();
            uploadEmp.UploadEmpID = UploadEmpID;
            EUploadEmpPersonalInfo.db.select(dbConn, uploadEmp);
            return uploadEmp.EmpID;

        }

        public static int GetEmpBankAccIDFromUploadEmpBankAccID(DatabaseConnection dbConn, int UploadEmpBankAccID)
        {
            EUploadEmpBankAccount uploadEmpBankAccount = new EUploadEmpBankAccount();
            uploadEmpBankAccount.UploadEmpBankAccountID = UploadEmpBankAccID;
            EUploadEmpBankAccount.db.select(dbConn, uploadEmpBankAccount);
            return uploadEmpBankAccount.EmpBankAccountID;
        }

        public static int GetUploadEmpAccID(DatabaseConnection dbConn, string BankCode, string BranchCode, string AccountNo, int UploadEmpID)
        {
            DBFilter dbfilter = new DBFilter();
            dbfilter.add(new Match("EmpBankCode", BankCode));
            dbfilter.add(new Match("EmpBranchCode", BranchCode));
            dbfilter.add(new Match("EmpAccountNo", AccountNo));
            dbfilter.add(new Match("UploadEmpID", UploadEmpID));
            ArrayList list = EUploadEmpBankAccount.db.select(dbConn, dbfilter);
            if (list.Count > 0)
                return ((EUploadEmpBankAccount)list[0]).UploadEmpBankAccountID;
            else
                return 0;

        }

        public static int GetUploadEmpAccID(DatabaseConnection dbConn, string BankNo, int EmpID)
        {
            string[] BankAccountArray = BankNo.Trim().Split(new char[] { '-' });
            string BankCode = string.Empty;
            string BranchCode = string.Empty;
            string AccountNo = string.Empty;

            switch (BankAccountArray.GetLength(0))
            {
                case 1:
                    if (BankAccountArray[0].Length >= 7)
                    {
                        BankCode = BankAccountArray[0].Substring(0, 3);
                        BranchCode = BankAccountArray[0].Substring(3, 3);
                        AccountNo = BankAccountArray[0].Substring(6);
                    }
                    else
                        return 0;
                    break;
                case 2:
                    if (BankAccountArray[0].Length <= 3)
                    {
                        BankCode = BankAccountArray[0];
                        if (BankAccountArray[1].Length >= 3)
                        {
                            BranchCode = BankAccountArray[1].Substring(0, 3);
                            AccountNo = BankAccountArray[1].Substring(3);
                        }
                        else
                            return 0;
                    }
                    break;
                case 3:
                    if (BankAccountArray[0].Length <= 3)
                    {
                        BankCode = BankAccountArray[0];
                        if (BankAccountArray[1].Length <= 3)
                        {
                            BranchCode = BankAccountArray[1];
                            AccountNo = BankAccountArray[2];
                        }
                        else
                            return 0;
                    }
                    break;
                default:
                    return 0;
            }
            return GetUploadEmpAccID(dbConn, BankCode, BranchCode, AccountNo, EmpID);
        }
    }


    //public class ImportException : Exception
    //{
    //    private string m_FieldName;
    //    private string m_FieldValue;

    //    public string FieldName
    //    {
    //        get { return m_FieldName; }
    //    }

    //    public string FieldValue
    //    {
    //        get { return m_FieldValue; }
    //    }

    //    public ImportException(string FieldName, string FieldValue, string ErrorMessage)
    //        : base(ErrorMessage)
    //    {
    //        m_FieldName = FieldName;
    //        m_FieldValue = FieldValue;
    //    }
    //}

    public static class Parse
    {
        [ThreadStatic()]
        private static DatabaseConnection cachedDbConn = null;
        [ThreadStatic()]
        private static ArrayList cachedPaymentCodeList = null;
        [ThreadStatic()]
        private static DateTime lastCacheTime= new DateTime();

        private const string CODE_DESC_DELIMITOR = "~";

        private static string[] GetCodeDescArray(string Name)
        {
            string[] codeDescArray = Name.Split(new string[] { CODE_DESC_DELIMITOR }, StringSplitOptions.None);
            if (codeDescArray.GetLength(0) >= 2)
                return new string[] { codeDescArray[0].Trim(), string.Join("~", codeDescArray, 1, codeDescArray.GetLength(0) - 1).Trim() };
            else if (codeDescArray.GetLength(0).Equals(1))
                return new string[] { codeDescArray[0].Trim(), codeDescArray[0].Trim() };
            else
                return new string[] { "", "" };

        }

        public static int GetEmpID(DatabaseConnection dbConn, string EmpNo, int UserID)
        {
            EmpNo = EmpNo.ToUpper();

            DBFilter dbfilter = new DBFilter();
            OR orEmpNo = new OR();
            orEmpNo.add(new Match("EmpNo", EmpNo));
            DBFieldTranscoder transcoder = EEmpPersonalInfo.db.getField("EmpNo").transcoder;
            if (transcoder != null)
                orEmpNo.add(new Match("EmpNo", transcoder.toDB(EmpNo)));

            dbfilter.add(AppUtils.AddRankDBTerm(UserID, "EmpID", true));
            dbfilter.add(orEmpNo);

            ArrayList list = EEmpPersonalInfo.db.select(dbConn, dbfilter);
            if (list.Count > 0)
                return ((EEmpPersonalInfo)list[0]).EmpID;
            else
            {
                DBFilter dbfilter2 = new DBFilter();

                dbfilter2.add(orEmpNo);

                ArrayList list2 = EEmpPersonalInfo.db.select(dbConn, dbfilter2);
                if (list2.Count > 0)
                    return -1;
                else
                    return 0;
            }
        }

        public static int GetPaymentCodeID(DatabaseConnection dbConn, string NameOrCode)
        {
            //try not to load too many payment code at the same time
            DateTime currentDateTime = AppUtils.ServerDateTime();
            if (lastCacheTime.Ticks == 0 || cachedPaymentCodeList == null || cachedDbConn != dbConn || ((TimeSpan)currentDateTime.Subtract(lastCacheTime)).Seconds > 2)
            {
                cachedDbConn = dbConn;
                cachedPaymentCodeList = EPaymentCode.db.select(dbConn, new DBFilter());
            }
            lastCacheTime = currentDateTime;

            ArrayList list = cachedPaymentCodeList;

            if (string.IsNullOrEmpty(NameOrCode))
                return 0;

            string[] codeDescArray = GetCodeDescArray(NameOrCode);
            //  Search code first
            foreach (EPaymentCode item in list)
            {
                if (item.PaymentCode.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase))
                    return item.PaymentCodeID;
            }
            foreach (EPaymentCode item in list)
            {
                if (item.PaymentCodeDesc.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase) && codeDescArray[0]==codeDescArray[1])
                    return item.PaymentCodeID;
            }
            return 0;
            //DBFilter dbfilter = new DBFilter();
            //OR orTerms = new OR();
            //string[] codeDescArray = GetCodeDescArray(NameOrCode);
            //orTerms.add(new Match("PaymentCode", codeDescArray[0]));
            //DBFieldTranscoder transcoder = EPaymentCode.db.getField("PaymentCode").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("PaymentCode", transcoder.toDB(codeDescArray[0])));

            //orTerms.add(new Match("PaymentCodeDesc", codeDescArray[1]));
            //transcoder = EPaymentCode.db.getField("PaymentCodeDesc").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("PaymentCodeDesc", transcoder.toDB(codeDescArray[1])));

            //dbfilter.add(orTerms);
            //ArrayList list = EPaymentCode.db.select(dbConn, dbfilter);
            //if (list.Count > 0)
            //    return ((EPaymentCode)list[0]).PaymentCodeID;
            //else
            //    return 0;
        }
        public static int GetEmpAccID(DatabaseConnection dbConn, string BankNo, int EmpID)
        {
            string[] BankAccountArray = BankNo.Trim().Split(new char[] { '-' });
            string BankCode = string.Empty;
            string BranchCode = string.Empty;
            string AccountNo = string.Empty;

            switch (BankAccountArray.GetLength(0))
            {
                case 1:
                    if (BankAccountArray[0].Length >= 7)
                    {
                        BankCode = BankAccountArray[0].Substring(0, 3);
                        BranchCode = BankAccountArray[0].Substring(3, 3);
                        AccountNo = BankAccountArray[0].Substring(6);
                    }
                    else
                        return 0;
                    break;
                case 2:
                    if (BankAccountArray[0].Length <= 3)
                    {
                        BankCode = BankAccountArray[0];
                        if (BankAccountArray[1].Length >= 3)
                        {
                            BranchCode = BankAccountArray[1].Substring(0, 3);
                            AccountNo = BankAccountArray[1].Substring(3);
                        }
                        else
                            return 0;
                    }
                    break;
                case 3:
                    if (BankAccountArray[0].Length <= 3)
                    {
                        BankCode = BankAccountArray[0];
                        if (BankAccountArray[1].Length <= 3)
                        {
                            BranchCode = BankAccountArray[1];
                            AccountNo = BankAccountArray[2];
                        }
                        else
                            return 0;
                    }
                    break;
                default:
                    return 0;
            }
            return GetEmpAccID(dbConn, BankCode, BranchCode, AccountNo, EmpID);
        }

        public static int GetEmpAccID(DatabaseConnection dbConn, string BankCode, string BranchCode, string AccountNo, int EmpID)
        {
            DBFilter dbfilter = new DBFilter();

            OR orBankCodeTerm = new OR();
            orBankCodeTerm.add(new Match("EmpBankCode", BankCode));
            DBFieldTranscoder transcoder = EEmpBankAccount.db.getField("EmpBankCode").transcoder;
            if (transcoder != null)
                orBankCodeTerm.add(new Match("EmpBankCode", transcoder.toDB(BankCode)));
            dbfilter.add(orBankCodeTerm);

            OR orBranchCodeTerm = new OR();
            orBranchCodeTerm.add(new Match("EmpBranchCode", BranchCode));
            transcoder = EEmpBankAccount.db.getField("EmpBranchCode").transcoder;
            if (transcoder != null)
                orBranchCodeTerm.add(new Match("EmpBranchCode", transcoder.toDB(BranchCode)));
            dbfilter.add(orBranchCodeTerm);

            OR orAccountNoTerm = new OR();
            orAccountNoTerm.add(new Match("EmpAccountNo", AccountNo));
            transcoder = EEmpBankAccount.db.getField("EmpAccountNo").transcoder;
            if (transcoder != null)
                orAccountNoTerm.add(new Match("EmpAccountNo", transcoder.toDB(AccountNo)));
            dbfilter.add(orAccountNoTerm);

            dbfilter.add(new Match("EmpID", EmpID));
            ArrayList list = EEmpBankAccount.db.select(dbConn, dbfilter);
            if (list.Count > 0)
                return ((EEmpBankAccount)list[0]).EmpBankAccountID;
            else
            {
                EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                empInfo.EmpID = EmpID;
                if (EEmpPersonalInfo.db.select(dbConn, empInfo) && empInfo.MasterEmpID > 0)
                    return GetEmpAccID(dbConn, BankCode, BranchCode, AccountNo, empInfo.MasterEmpID);
            }
            return 0;

        }



        public static int GetCompanyID(DatabaseConnection dbConn, string NameOrCode, bool CreateIfNotExist, int CreaterUserID)
        {
            if (string.IsNullOrEmpty(NameOrCode))
                return 0;

            string[] codeDescArray = GetCodeDescArray(NameOrCode);
            ArrayList list = ECompany.db.select(dbConn, new DBFilter());
            //  Search code first
            foreach (ECompany item in list)
            {
                if (item.CompanyCode.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase))
                    return item.CompanyID;
            }
            foreach (ECompany item in list)
            {
                if (item.CompanyName.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase) && codeDescArray[0]==codeDescArray[1])
                    return item.CompanyID;
            }
            //  Create Code if necessary
            string newCode = codeDescArray[0].Replace(" ", "_").PadRight(20).Substring(0, 20).Trim().ToUpper();
            foreach (ECompany item in list)
            {
                if (item.CompanyCode.Trim().Equals(newCode, StringComparison.CurrentCultureIgnoreCase))
                    return item.CompanyID;
            }
            if (CreateIfNotExist)
            {
                AppUtils.StartChildFunction(dbConn, "SYS001", 0);
                ECompany company = new ECompany();
                company.CompanyCode = newCode;
                company.CompanyName = codeDescArray[1];

                ECompany.db.insert(dbConn, company);
                AppUtils.EndChildFunction(dbConn);
                return company.CompanyID;
            }

            return 0;

            //if (string.IsNullOrEmpty(NameOrCode))
            //    return 0;

            //DBManager db = ECompany.db;
            //DBFilter dbfilter = new DBFilter();

            //string[] codeDescArray = GetCodeDescArray(NameOrCode);

            //OR orTerms = new OR();

            //orTerms.add(new Match("CompanyCode", codeDescArray[0]));
            //DBFieldTranscoder transcoder = ECompany.db.getField("CompanyCode").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("CompanyCode", transcoder.toDB(codeDescArray[0])));

            //orTerms.add(new Match("CompanyName", codeDescArray[1]));
            //transcoder = ECompany.db.getField("CompanyName").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("CompanyName", transcoder.toDB(codeDescArray[1])));

            //dbfilter.add(orTerms);
            //ArrayList list = db.select(dbConn, dbfilter);
            //if (list.Count > 0)
            //    return ((ECompany)list[0]).CompanyID;

            //dbfilter = new DBFilter();
            //orTerms = new OR();
            //orTerms.add(new Match("CompanyCode", newCode));
            //transcoder = ECompany.db.getField("CompanyCode").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("CompanyCode", transcoder.toDB(newCode)));
            //dbfilter.add(orTerms);

            //list = db.select(dbConn, dbfilter);
            //if (list.Count > 0)
            //    return ((ECompany)list[0]).CompanyID;
            //else
            //    if (CreateIfNotExist)
            //    {
            //        ECompany company = new ECompany();
            //        company.CompanyCode = newCode;
            //        company.CompanyName = codeDescArray[1];

            //        db.insert(dbConn, company);
            //        return company.CompanyID;
            //    }
            //    else
            //        return 0;
        }

        public static int GetPositionID(DatabaseConnection dbConn, string NameOrCode, bool CreateIfNotExist, int CreaterUserID)
        {
            if (string.IsNullOrEmpty(NameOrCode))
                return 0;
            string[] codeDescArray = GetCodeDescArray(NameOrCode);
            ArrayList list = EPosition.db.select(dbConn, new DBFilter());
            //  Search code first
            foreach (EPosition item in list)
            {
                if (item.PositionCode.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase))
                    return item.PositionID;
            }
            foreach (EPosition item in list)
            {
                if (item.PositionDesc.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase) && codeDescArray[0]==codeDescArray[1])
                    return item.PositionID;
            }
            //  Create Code if necessary
            string newCode = codeDescArray[0].Replace(" ", "_").PadRight(20).Substring(0, 20).Trim().ToUpper();
            foreach (EPosition item in list)
            {
                if (item.PositionCode.Trim().Equals(newCode, StringComparison.CurrentCultureIgnoreCase))
                    return item.PositionID;
            }
            if (CreateIfNotExist)
            {
                AppUtils.StartChildFunction(dbConn, "SYS012", 0);
                EPosition position = new EPosition();
                position.PositionCode = newCode;
                position.PositionDesc = codeDescArray[1];

                EPosition.db.insert(dbConn, position);
                AppUtils.EndChildFunction(dbConn);
                return position.PositionID;
            }

            return 0;
            //DBManager db = EPosition.db;
            //DBFilter dbfilter = new DBFilter();
            //string[] codeDescArray = GetCodeDescArray(NameOrCode);

            //OR orTerms = new OR();
            //orTerms.add(new Match("PositionCode", codeDescArray[0]));
            //DBFieldTranscoder transcoder = EPosition.db.getField("PositionCode").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("PositionCode", transcoder.toDB(codeDescArray[0])));

            //orTerms.add(new Match("PositionDesc", codeDescArray[1]));
            //transcoder = EPosition.db.getField("PositionDesc").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("PositionDesc", transcoder.toDB(codeDescArray[1])));

            //dbfilter.add(orTerms);
            //ArrayList list = db.select(dbConn, dbfilter);
            //if (list.Count > 0)
            //    return ((EPosition)list[0]).PositionID;
            //dbfilter = new DBFilter();
            //orTerms = new OR();
            //orTerms.add(new Match("PositionCode", newCode));
            //transcoder = EPosition.db.getField("PositionCode").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("PositionCode", transcoder.toDB(newCode)));
            //dbfilter.add(orTerms);

            //list = db.select(dbConn, dbfilter);
            //if (list.Count > 0)
            //    return ((EPosition)list[0]).PositionID;
            //else
            //    if (CreateIfNotExist)
            //    {
            //        EPosition position = new EPosition();
            //        position.PositionCode = newCode;
            //        position.PositionDesc = codeDescArray[1]; 

            //        db.insert(dbConn, position);
            //        return position.PositionID;

            //    }
            //    else
            //        return 0;
        }

        public static int GetRankID(DatabaseConnection dbConn, string NameOrCode, bool CreateIfNotExist, int CreaterUserID)
        {
            if (string.IsNullOrEmpty(NameOrCode))
                return 0;
            string[] codeDescArray = GetCodeDescArray(NameOrCode);
            ArrayList list = ERank.db.select(dbConn, new DBFilter());
            //  Search code first
            foreach (ERank item in list)
            {
                if (item.RankCode.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase))
                    return item.RankID;
            }
            foreach (ERank item in list)
            {
                if (item.RankDesc.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase) && codeDescArray[0]==codeDescArray[1])
                    return item.RankID;
            }
            //  Create Code if necessary
            string newCode = codeDescArray[0].Replace(" ", "_").PadRight(20).Substring(0, 20).Trim().ToUpper();
            foreach (ERank item in list)
            {
                if (item.RankCode.Trim().Equals(newCode, StringComparison.CurrentCultureIgnoreCase))
                    return item.RankID;
            }
            if (CreateIfNotExist)
            {
                AppUtils.StartChildFunction(dbConn, "SYS016", 0);
                ERank rank = new ERank();
                rank.RankCode = newCode;
                rank.RankDesc = codeDescArray[1];

                ERank.db.insert(dbConn, rank);
                AppUtils.EndChildFunction(dbConn);


                ArrayList userList = EUser.db.select(dbConn, new DBFilter());
                foreach (EUser user in userList)
                {
                    if (user.UserAccountStatus.Equals("A"))
                    {
                        EUserRank userRank = new EUserRank();
                        userRank.UserID = user.UserID;
                        userRank.RankID = rank.RankID;
                        EUserRank.db.insert(dbConn, userRank);
                    }
                }
                return rank.RankID;
            }

            return 0;

            //DBManager db = ERank.db;
            //DBFilter dbfilter = new DBFilter();
            //string[] codeDescArray = GetCodeDescArray(NameOrCode);

            //OR orTerms = new OR();
            //orTerms.add(new Match("RankCode", codeDescArray[0]));
            //DBFieldTranscoder transcoder = ERank.db.getField("RankCode").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("RankCode", transcoder.toDB(codeDescArray[0])));

            //orTerms.add(new Match("RankDesc", codeDescArray[1]));
            //transcoder = ERank.db.getField("RankDesc").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("RankDesc", transcoder.toDB(codeDescArray[1])));

            //dbfilter.add(orTerms);
            //ArrayList list = db.select(dbConn, dbfilter);
            //if (list.Count > 0)
            //    return ((ERank)list[0]).RankID ;
            //dbfilter = new DBFilter();
            //orTerms = new OR();
            //orTerms.add(new Match("RankCode", newCode));
            //transcoder = ERank.db.getField("RankCode").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("RankCode", transcoder.toDB(newCode)));
            //dbfilter.add(orTerms); 

            //list = db.select(dbConn, dbfilter);
            //if (list.Count > 0)
            //    return ((ERank)list[0]).RankID ;
            //else
            //    if (CreateIfNotExist)
            //    {
            //        ERank rank = new ERank();
            //        rank.RankCode = newCode;
            //        rank.RankDesc = codeDescArray[1]; 

            //        db.insert(dbConn, rank);

            //        EUserRank.InsertRankForAllUsers(rank.RankID);
            //        return rank.RankID;
            //    }
            //    else
            //        return 0;
        }
        public static int GetEmploymentTypeID(DatabaseConnection dbConn, string NameOrCode, bool CreateIfNotExist, int CreaterUserID)
        {
            if (string.IsNullOrEmpty(NameOrCode))
                return 0;
            string[] codeDescArray = GetCodeDescArray(NameOrCode);
            ArrayList list = EEmploymentType.db.select(dbConn, new DBFilter());
            //  Search code first
            foreach (EEmploymentType item in list)
            {
                if (item.EmploymentTypeCode.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase))
                    return item.EmploymentTypeID;
            }
            foreach (EEmploymentType item in list)
            {
                if (item.EmploymentTypeDesc.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase) && codeDescArray[0]==codeDescArray[1])
                    return item.EmploymentTypeID;
            }
            //  Create Code if necessary
            string newCode = codeDescArray[0].Replace(" ", "_").PadRight(20).Substring(0, 20).Trim().ToUpper();
            foreach (EEmploymentType item in list)
            {
                if (item.EmploymentTypeCode.Trim().Equals(newCode, StringComparison.CurrentCultureIgnoreCase))
                    return item.EmploymentTypeID;
            }
            if (CreateIfNotExist)
            {
                AppUtils.StartChildFunction(dbConn, "SYS010", 0);

                EEmploymentType employmentType = new EEmploymentType();
                employmentType.EmploymentTypeCode = newCode;
                employmentType.EmploymentTypeDesc = codeDescArray[1];

                EEmploymentType.db.insert(dbConn, employmentType);
                AppUtils.EndChildFunction(dbConn);
                return employmentType.EmploymentTypeID;
            }
            return 0;
            //DBManager db = EEmploymentType.db;
            //DBFilter dbfilter = new DBFilter();
            //string[] codeDescArray = GetCodeDescArray(NameOrCode);

            //OR orTerms = new OR();

            //orTerms.add(new Match("EmploymentTypeCode", codeDescArray[0]));
            //DBFieldTranscoder transcoder = EEmploymentType.db.getField("EmploymentTypeCode").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("EmploymentTypeCode", transcoder.toDB(codeDescArray[0])));

            //orTerms.add(new Match("EmploymentTypeDesc", codeDescArray[1]));
            //transcoder = EEmploymentType.db.getField("EmploymentTypeDesc").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("EmploymentTypeDesc", transcoder.toDB(codeDescArray[1])));

            //dbfilter.add(orTerms);
            //ArrayList list = db.select(dbConn, dbfilter);
            //if (list.Count > 0)
            //    return ((EEmploymentType)list[0]).EmploymentTypeID;
            //dbfilter = new DBFilter();
            //orTerms = new OR();
            //orTerms.add(new Match("EmploymentTypeCode", newCode));
            //transcoder = EEmploymentType.db.getField("EmploymentTypeCode").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("EmploymentTypeCode", transcoder.toDB(newCode)));
            //dbfilter.add(orTerms); 
            //list = db.select(dbConn, dbfilter);
            //if (list.Count > 0)
            //    return ((EEmploymentType)list[0]).EmploymentTypeID;
            //else
            //    if (CreateIfNotExist)
            //    {
            //        EEmploymentType EmploymentType = new EEmploymentType();
            //        EmploymentType.EmploymentTypeCode = newCode;
            //        EmploymentType.EmploymentTypeDesc = codeDescArray[1];

            //        db.insert(dbConn, EmploymentType);
            //        return EmploymentType.EmploymentTypeID;
            //    }
            //    else
            //        return 0;
        }
        public static int GetStaffTypeID(DatabaseConnection dbConn, string NameOrCode, bool CreateIfNotExist, int CreaterUserID)
        {
            if (string.IsNullOrEmpty(NameOrCode))
                return 0;
            string[] codeDescArray = GetCodeDescArray(NameOrCode);
            ArrayList list = EStaffType.db.select(dbConn, new DBFilter());
            //  Search code first
            foreach (EStaffType item in list)
            {
                if (item.StaffTypeCode.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase))
                    return item.StaffTypeID;
            }
            foreach (EStaffType item in list)
            {
                if (item.StaffTypeDesc.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase) && codeDescArray[0]==codeDescArray[1])
                    return item.StaffTypeID;
            }
            //  Create Code if necessary
            string newCode = codeDescArray[0].Replace(" ", "_").PadRight(20).Substring(0, 20).Trim().ToUpper();
            foreach (EStaffType item in list)
            {
                if (item.StaffTypeCode.Trim().Equals(newCode, StringComparison.CurrentCultureIgnoreCase))
                    return item.StaffTypeID;
            }
            if (CreateIfNotExist)
            {
                AppUtils.StartChildFunction(dbConn, "SYS017", 0);
                EStaffType staffType = new EStaffType();
                staffType.StaffTypeCode = newCode;
                staffType.StaffTypeDesc = codeDescArray[1];

                EStaffType.db.insert(dbConn, staffType);
                AppUtils.EndChildFunction(dbConn);
                return staffType.StaffTypeID;
            }
            return 0;

            //DBManager db = EStaffType.db;
            //DBFilter dbfilter = new DBFilter();
            //string[] codeDescArray = GetCodeDescArray(NameOrCode);

            //OR orTerms = new OR();

            //orTerms.add(new Match("StaffTypeCode", codeDescArray[0]));
            //DBFieldTranscoder transcoder = EStaffType.db.getField("StaffTypeCode").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("StaffTypeCode", transcoder.toDB(codeDescArray[0])));

            //orTerms.add(new Match("StaffTypeDesc", codeDescArray[1]));
            //transcoder = EStaffType.db.getField("StaffTypeDesc").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("StaffTypeDesc", transcoder.toDB(codeDescArray[1])));

            //dbfilter.add(orTerms);
            //ArrayList list = db.select(dbConn, dbfilter);
            //if (list.Count > 0)
            //    return ((EStaffType)list[0]).StaffTypeID;
            //dbfilter = new DBFilter();
            //orTerms = new OR();
            //orTerms.add(new Match("StaffTypeCode", newCode));
            //transcoder = EStaffType.db.getField("StaffTypeCode").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("StaffTypeCode", transcoder.toDB(newCode)));
            //dbfilter.add(orTerms); 

            //list = db.select(dbConn, dbfilter);
            //if (list.Count > 0)
            //    return ((EStaffType)list[0]).StaffTypeID;
            //else
            //    if (CreateIfNotExist)
            //    {
            //        EStaffType staffType = new EStaffType();
            //        staffType.StaffTypeCode = newCode;
            //        staffType.StaffTypeDesc = codeDescArray[1];

            //        db.insert(dbConn, staffType);
            //        return staffType.StaffTypeID;
            //    }
            //    else
            //        return 0;
        }
        public static int GetPayrollGroupID(DatabaseConnection dbConn, string NameOrCode, bool CreateIfNotExist, int CreaterUserID)
        {
            if (string.IsNullOrEmpty(NameOrCode))
                return 0;

            string[] codeDescArray = GetCodeDescArray(NameOrCode);
            ArrayList list = EPayrollGroup.db.select(dbConn, new DBFilter());
            //  Search code first
            foreach (EPayrollGroup item in list)
            {
                if (item.PayGroupCode.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase))
                    return item.PayGroupID;
            }
            foreach (EPayrollGroup item in list)
            {
                if (item.PayGroupDesc.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase) && codeDescArray[0]==codeDescArray[1])
                    return item.PayGroupID;
            }
            //  Create Code if necessary
            string newCode = codeDescArray[0].Replace(" ", "_").PadRight(20).Substring(0, 20).Trim().ToUpper();
            foreach (EPayrollGroup item in list)
            {
                if (item.PayGroupCode.Trim().Equals(newCode, StringComparison.CurrentCultureIgnoreCase))
                    return item.PayGroupID;
            }
            if (CreateIfNotExist)
            {
                AppUtils.StartChildFunction(dbConn, "PAY001", 0);
                EPayrollGroup payrollGroup = new EPayrollGroup();
                payrollGroup.PayGroupCode = newCode;
                payrollGroup.PayGroupDesc = codeDescArray[1];

                EPayrollGroup.db.insert(dbConn, payrollGroup);
                AppUtils.EndChildFunction(dbConn);
                return payrollGroup.PayGroupID;
            }
            return 0;

        }
        public static int GetLeavePlanID(DatabaseConnection dbConn, string NameOrCode, bool CreateIfNotExist, int CreaterUserID)
        {
            if (string.IsNullOrEmpty(NameOrCode))
                return 0;
            string[] codeDescArray = GetCodeDescArray(NameOrCode);
            ArrayList list = ELeavePlan.db.select(dbConn, new DBFilter());
            //  Search code first
            foreach (ELeavePlan item in list)
            {
                if (item.LeavePlanCode.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase))
                    return item.LeavePlanID;
            }
            foreach (ELeavePlan item in list)
            {
                if (item.LeavePlanDesc.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase) && codeDescArray[0]==codeDescArray[1])
                    return item.LeavePlanID;
            }
            //  Create Code if necessary
            string newCode = codeDescArray[0].Replace(" ", "_").PadRight(20).Substring(0, 20).Trim().ToUpper();
            foreach (ELeavePlan item in list)
            {
                if (item.LeavePlanCode.Trim().Equals(newCode, StringComparison.CurrentCultureIgnoreCase))
                    return item.LeavePlanID;
            }
            if (CreateIfNotExist)
            {
                AppUtils.StartChildFunction(dbConn, "LEV003", 0);
                ELeavePlan leavePlan = new ELeavePlan();
                leavePlan.LeavePlanCode = newCode;
                leavePlan.LeavePlanDesc = codeDescArray[1];

                ELeavePlan.db.insert(dbConn, leavePlan);
                AppUtils.EndChildFunction(dbConn);
                return leavePlan.LeavePlanID;
            }
            return 0;

            //DBManager db = ELeavePlan.db;
            //DBFilter dbfilter = new DBFilter();
            //string[] codeDescArray = GetCodeDescArray(NameOrCode);

            //OR orTerms = new OR();

            //orTerms.add(new Match("LeavePlanCode", codeDescArray[0]));
            //DBFieldTranscoder transcoder = ELeavePlan.db.getField("LeavePlanCode").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("LeavePlanCode", transcoder.toDB(codeDescArray[0])));

            //orTerms.add(new Match("LeavePlanDesc", codeDescArray[1]));
            //transcoder = ELeavePlan.db.getField("LeavePlanDesc").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("LeavePlanDesc", transcoder.toDB(codeDescArray[1])));

            //dbfilter.add(orTerms);
            //ArrayList list = db.select(dbConn, dbfilter);
            //if (list.Count > 0)
            //    return ((ELeavePlan)list[0]).LeavePlanID;
            //dbfilter = new DBFilter();
            //orTerms = new OR();
            //orTerms.add(new Match("LeavePlanCode", newCode));
            //transcoder = ELeavePlan.db.getField("LeavePlanCode").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("LeavePlanCode", transcoder.toDB(newCode)));
            //dbfilter.add(orTerms);

            //list = db.select(dbConn, dbfilter);
            //if (list.Count > 0)
            //    return ((ELeavePlan)list[0]).LeavePlanID;
            //else
            //    if (CreateIfNotExist)
            //    {
            //        ELeavePlan leavePlan = new ELeavePlan();
            //        leavePlan.LeavePlanCode = newCode;
            //        leavePlan.LeavePlanDesc = codeDescArray[1];

            //        db.insert(dbConn, leavePlan);
            //        return leavePlan.LeavePlanID;
            //    }
            //    else
            //        return 0;
        }

        public static int GetYEBPlanID(DatabaseConnection dbConn, string NameOrCode, bool CreateIfNotExist, int CreaterUserID)
        {
            if (string.IsNullOrEmpty(NameOrCode))
                return 0;

            string[] codeDescArray = GetCodeDescArray(NameOrCode);
            ArrayList list = EYEBPlan.db.select(dbConn, new DBFilter());
            //  Search code first
            foreach (EYEBPlan item in list)
            {
                if (item.YEBPlanCode.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase))
                    return item.YEBPlanID;
            }
            foreach (EYEBPlan item in list)
            {
                if (item.YEBPlanDesc.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase) && codeDescArray[0]==codeDescArray[1])
                    return item.YEBPlanID;
            }
            //  Create Code if necessary
            string newCode = codeDescArray[0].Replace(" ", "_").PadRight(20).Substring(0, 20).Trim().ToUpper();
            foreach (EYEBPlan item in list)
            {
                if (item.YEBPlanCode.Trim().Equals(newCode, StringComparison.CurrentCultureIgnoreCase))
                    return item.YEBPlanID;
            }
            if (CreateIfNotExist)
            {
                AppUtils.StartChildFunction(dbConn, "SYS007", 0);
                EYEBPlan yebPlan = new EYEBPlan();
                yebPlan.YEBPlanCode = newCode;
                yebPlan.YEBPlanDesc = codeDescArray[1];

                EYEBPlan.db.insert(dbConn, yebPlan);
                AppUtils.EndChildFunction(dbConn);
                return yebPlan.YEBPlanID;
            }
            return 0;

        }

        //public static int GetAuthorizationGroupID(DatabaseConnection dbConn, string NameOrCode, bool CreateIfNotExist, int CreaterUserID)
        //{
        //    if (string.IsNullOrEmpty(NameOrCode))
        //        return 0;

        //    string[] codeDescArray = GetCodeDescArray(NameOrCode);
        //    ArrayList list = EAuthorizationGroup.db.select(dbConn, new DBFilter());
        //    //  Search code first
        //    foreach (EAuthorizationGroup item in list)
        //    {
        //        if (item.AuthorizationCode.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase))
        //            return item.AuthorizationGroupID;
        //    }
        //    foreach (EAuthorizationGroup item in list)
        //    {
        //        if (item.AuthorizationDesc.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase) && codeDescArray[0]==codeDescArray[1])
        //            return item.AuthorizationGroupID;
        //    }
        //    //  Create Code if necessary
        //    string newCode = codeDescArray[0].Replace(" ", "_").PadRight(20).Substring(0, 20).Trim().ToUpper();
        //    foreach (EAuthorizationGroup item in list)
        //    {
        //        if (item.AuthorizationCode.Trim().Equals(newCode, StringComparison.CurrentCultureIgnoreCase))
        //            return item.AuthorizationGroupID;
        //    }
        //    if (CreateIfNotExist)
        //    {
        //        AppUtils.StartChildFunction(dbConn, "SEC003", 0);
        //        EAuthorizationGroup AuthorizationGroup = new EAuthorizationGroup();
        //        AuthorizationGroup.AuthorizationCode = newCode;
        //        AuthorizationGroup.AuthorizationDesc = codeDescArray[1];

        //        EAuthorizationGroup.db.insert(dbConn, AuthorizationGroup);
        //        AppUtils.EndChildFunction(dbConn);
        //        return AuthorizationGroup.AuthorizationGroupID;
        //    }
        //    return 0;

        //}

        public static int GetAuthorizationWorkFlowID(DatabaseConnection dbConn, string NameOrCode, bool CreateIfNotExist, int CreaterUserID)
        {
            if (string.IsNullOrEmpty(NameOrCode))
                return 0;

            string[] codeDescArray = GetCodeDescArray(NameOrCode);
            ArrayList list = EAuthorizationWorkFlow.db.select(dbConn, new DBFilter());
            //  Search code first
            foreach (EAuthorizationWorkFlow item in list)
            {
                if (item.AuthorizationWorkFlowCode.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase))
                    return item.AuthorizationWorkFlowID;
            }
            foreach (EAuthorizationWorkFlow item in list)
            {
                if (item.AuthorizationWorkFlowDescription.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase) && codeDescArray[0] == codeDescArray[1])
                    return item.AuthorizationWorkFlowID;
            }
            //  Create Code if necessary
            string newCode = codeDescArray[0].Replace(" ", "_").PadRight(20).Substring(0, 20).Trim().ToUpper();
            foreach (EAuthorizationWorkFlow item in list)
            {
                if (item.AuthorizationWorkFlowCode.Trim().Equals(newCode, StringComparison.CurrentCultureIgnoreCase))
                    return item.AuthorizationWorkFlowID;
            }
            if (CreateIfNotExist)
            {
                AppUtils.StartChildFunction(dbConn, "SEC003", 0);
                EAuthorizationWorkFlow AuthorizationWorkFlow = new EAuthorizationWorkFlow();
                AuthorizationWorkFlow.AuthorizationWorkFlowCode = newCode;
                AuthorizationWorkFlow.AuthorizationWorkFlowDescription = codeDescArray[1];

                EAuthorizationWorkFlow.db.insert(dbConn, AuthorizationWorkFlow);
                AppUtils.EndChildFunction(dbConn);
                return AuthorizationWorkFlow.AuthorizationWorkFlowID;
            }
            return 0;

        }

        public static int GetNationalityID(DatabaseConnection dbConn, string NameOrCode, bool CreateIfNotExist, int CreateUserID)
        {
            if (string.IsNullOrEmpty(NameOrCode))
                return 0;

            string[] codeDescArray = GetCodeDescArray(NameOrCode);
            ArrayList list = ENationality.db.select(dbConn, new DBFilter());
            //  Search code first
            foreach (ENationality item in list)
            {
                if (item.NationalityCode.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase))
                    return item.NationalityID;
            }

            foreach (ENationality item in list)
            {
                if (item.NationalityDesc.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase) && codeDescArray[0] == codeDescArray[1])
                    return item.NationalityID;
            }
            //  Create Code if necessary
            string newCode = codeDescArray[0].Replace(" ", "_").PadRight(20).Substring(0, 20).Trim().ToUpper();
            foreach (ENationality item in list)
            {
                if (item.NationalityCode.Trim().Equals(newCode, StringComparison.CurrentCultureIgnoreCase))
                    return item.NationalityID;
            }
            if (CreateIfNotExist)
            {
                AppUtils.StartChildFunction(dbConn, "ATT003", 0);
                ENationality newObj = new ENationality();
                newObj.NationalityCode = newCode;
                newObj.NationalityDesc = codeDescArray[1];

                ENationality.db.insert(dbConn, newObj);
                AppUtils.EndChildFunction(dbConn);
                return newObj.NationalityID;
            }
            return 0;
        }

        public static int GetIssueCountryID(DatabaseConnection dbConn, string NameOrCode, bool CreateIfNotExist, int CreateUserID)
        {
            if (string.IsNullOrEmpty(NameOrCode))
                return 0;

            string[] codeDescArray = GetCodeDescArray(NameOrCode);
            ArrayList list = EIssueCountry.db.select(dbConn, new DBFilter());
            //  Search code first
            foreach (EIssueCountry item in list)
            {
                if (item.CountryCode.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase))
                    return item.CountryID;
            }

            foreach (EIssueCountry item in list)
            {
                if (item.CountryDesc.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase) && codeDescArray[0] == codeDescArray[1])
                    return item.CountryID;
            }
            //  Create Code if necessary
            string newCode = codeDescArray[0].Replace(" ", "_").PadRight(20).Substring(0, 20).Trim().ToUpper();
            foreach (EIssueCountry item in list)
            {
                if (item.CountryCode.Trim().Equals(newCode, StringComparison.CurrentCultureIgnoreCase))
                    return item.CountryID;
            }
            if (CreateIfNotExist)
            {
                AppUtils.StartChildFunction(dbConn, "ATT003", 0);
                EIssueCountry newObj = new EIssueCountry();
                newObj.CountryCode = newCode;
                newObj.CountryDesc = codeDescArray[1];

                EIssueCountry.db.insert(dbConn, newObj);
                AppUtils.EndChildFunction(dbConn);
                return newObj.CountryID;
            }
            return 0;
        }

        public static int GetPlaceOfBirthID(DatabaseConnection dbConn, string NameOrCode, bool CreateIfNotExist, int CreateUserID)
        {
            if (string.IsNullOrEmpty(NameOrCode))
                return 0;

            string[] codeDescArray = GetCodeDescArray(NameOrCode);
            ArrayList list = EPlaceOfBirth.db.select(dbConn, new DBFilter());
            //  Search code first
            foreach (EPlaceOfBirth item in list)
            {
                if (item.PlaceOfBirthCode.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase))
                    return item.PlaceOfBirthID;
            }

            foreach (EPlaceOfBirth item in list)
            {
                if (item.PlaceOfBirthDesc.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase) && codeDescArray[0] == codeDescArray[1])
                    return item.PlaceOfBirthID;
            }
            //  Create Code if necessary
            string newCode = codeDescArray[0].Replace(" ", "_").PadRight(20).Substring(0, 20).Trim().ToUpper();
            foreach (EPlaceOfBirth item in list)
            {
                if (item.PlaceOfBirthCode.Trim().Equals(newCode, StringComparison.CurrentCultureIgnoreCase))
                    return item.PlaceOfBirthID;
            }
            if (CreateIfNotExist)
            {
                AppUtils.StartChildFunction(dbConn, "ATT003", 0);
                EPlaceOfBirth newObj = new EPlaceOfBirth();
                newObj.PlaceOfBirthCode = newCode;
                newObj.PlaceOfBirthDesc = codeDescArray[1];

                EPlaceOfBirth.db.insert(dbConn, newObj);
                AppUtils.EndChildFunction(dbConn);
                return newObj.PlaceOfBirthID;
            }
            return 0;
        }
        
        public static int GetAttendancePlanID(DatabaseConnection dbConn, string NameOrCode, bool CreateIfNotExist, int CreaterUserID)
        {
            if (string.IsNullOrEmpty(NameOrCode))
                return 0;

            string[] codeDescArray = GetCodeDescArray(NameOrCode);
            ArrayList list = EAttendancePlan.db.select(dbConn, new DBFilter());
            //  Search code first
            foreach (EAttendancePlan item in list)
            {
                if (item.AttendancePlanCode.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase))
                    return item.AttendancePlanID;
            }
            foreach (EAttendancePlan item in list)
            {
                if (item.AttendancePlanDesc.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase) && codeDescArray[0]==codeDescArray[1])
                    return item.AttendancePlanID;
            }
            //  Create Code if necessary
            string newCode = codeDescArray[0].Replace(" ", "_").PadRight(20).Substring(0, 20).Trim().ToUpper();
            foreach (EAttendancePlan item in list)
            {
                if (item.AttendancePlanCode.Trim().Equals(newCode, StringComparison.CurrentCultureIgnoreCase))
                    return item.AttendancePlanID;
            }
            if (CreateIfNotExist)
            {
                AppUtils.StartChildFunction(dbConn, "ATT003", 0);
                EAttendancePlan AttendancePlan = new EAttendancePlan();
                AttendancePlan.AttendancePlanCode = newCode;
                AttendancePlan.AttendancePlanDesc = codeDescArray[1];

                EAttendancePlan.db.insert(dbConn, AttendancePlan);
                AppUtils.EndChildFunction(dbConn);
                return AttendancePlan.AttendancePlanID;
            }
            return 0;
        }

        public static int GetHierarchyLevelID(DatabaseConnection dbConn, int SeqNo)
        {
            DBFilter elementLevelFilter = new DBFilter();
            elementLevelFilter.add(new Match("HLevelSeqNo", SeqNo));
            ArrayList hierarchyLevelList = EHierarchyLevel.db.select(dbConn, elementLevelFilter);
            if (hierarchyLevelList.Count > 0)
            {
                EHierarchyLevel hierarchyLevel = (EHierarchyLevel)hierarchyLevelList[0];
                return hierarchyLevel.HLevelID;
            }
            return 0;

        }

        public static int GetHierarchyElementID(DatabaseConnection dbConn, string NameOrCode, int CompanyID, int SeqNo, bool CreateIfNotExist, int CreaterUserID)
        {
            if (string.IsNullOrEmpty(NameOrCode))
                return 0;

            int hLevelID = GetHierarchyLevelID(dbConn, SeqNo);
            if (hLevelID <= 0)
                return 0;

            string[] codeDescArray = GetCodeDescArray(NameOrCode);
            DBFilter dbfilter = new DBFilter();
            dbfilter.add(new Match("HLevelID", hLevelID));
            dbfilter.add(new Match("CompanyID", CompanyID));

            ArrayList list = EHierarchyElement.db.select(dbConn, dbfilter);
            //  Search code first
            foreach (EHierarchyElement item in list)
            {
                if (item.HElementCode.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase))
                    return item.HElementID;
            }
            foreach (EHierarchyElement item in list)
            {
                if (item.HElementDesc.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase) && codeDescArray[0]==codeDescArray[1])
                    return item.HElementID;
            }
            //  Create Code if necessary
            string newCode = codeDescArray[0].Replace(" ", "_").PadRight(20).Substring(0, 20).Trim().ToUpper();
            foreach (EHierarchyElement item in list)
            {
                if (item.HElementCode.Trim().Equals(newCode, StringComparison.CurrentCultureIgnoreCase))
                    return item.HElementID;
            }
            if (CreateIfNotExist)
            {
                AppUtils.StartChildFunction(dbConn, "SYS003", 0);
                EHierarchyElement hierarchyElement = new EHierarchyElement();
                hierarchyElement.HElementCode = newCode;
                hierarchyElement.HElementDesc = codeDescArray[1];
                hierarchyElement.CompanyID = CompanyID;
                hierarchyElement.HLevelID = hLevelID;

                EHierarchyElement.db.insert(dbConn, hierarchyElement);
                AppUtils.EndChildFunction(dbConn);
                return hierarchyElement.HElementID;
            }
            return 0;

            //DBManager db = EHierarchyElement.db;
            //DBFilter dbfilter = new DBFilter();

            //string[] codeDescArray = GetCodeDescArray(NameOrCode);

            //OR orTerms = new OR();

            //orTerms.add(new Match("HElementCode", codeDescArray[0]));
            //DBFieldTranscoder transcoder = EHierarchyElement.db.getField("HElementCode").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("HElementCode", transcoder.toDB(codeDescArray[0])));

            //orTerms.add(new Match("HElementDesc", codeDescArray[1]));
            //transcoder = EHierarchyElement.db.getField("HElementDesc").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("HElementDesc", transcoder.toDB(codeDescArray[1])));


            //dbfilter.add(orTerms);
            //dbfilter.add(new Match("HLevelID", hLevelID));
            //dbfilter.add(new Match("CompanyID", CompanyID));
            //ArrayList list = db.select(dbConn, dbfilter);
            //if (list.Count > 0)
            //    return ((EHierarchyElement)list[0]).HElementID;
            //dbfilter = new DBFilter();
            //orTerms = new OR();
            //orTerms.add(new Match("HElementCode", newCode));
            //transcoder = EHierarchyElement.db.getField("HElementCode").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("HElementCode", transcoder.toDB(newCode)));
            //dbfilter.add(orTerms);
            //dbfilter.add(new Match("HLevelID", hLevelID));
            //dbfilter.add(new Match("CompanyID", CompanyID));
            //list = db.select(dbConn, dbfilter);
            //if (list.Count > 0)
            //    return ((EHierarchyElement)list[0]).HElementID;
            //else
            //    if (CreateIfNotExist)
            //    {
            //        EHierarchyElement element = new EHierarchyElement();
            //        element.HElementCode = newCode;
            //        element.HElementDesc = codeDescArray[1];
            //        element.HLevelID = hLevelID;
            //        element.CompanyID = CompanyID;
            //        db.insert(dbConn, element);
            //        return element.HElementID;
            //    }
            //    else
            //        return 0;
        }

        public static int GetMPFPlanID(DatabaseConnection dbConn, string NameOrCode, bool CreateIfNotExist, int CreaterUserID)
        {
            if (string.IsNullOrEmpty(NameOrCode))
                return 0;

            string[] codeDescArray = GetCodeDescArray(NameOrCode);
            ArrayList list = EMPFPlan.db.select(dbConn, new DBFilter());
            //  Search code first
            foreach (EMPFPlan item in list)
            {
                if (item.MPFPlanCode.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase))
                    return item.MPFPlanID;
            }
            foreach (EMPFPlan item in list)
            {
                if (item.MPFPlanDesc.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase) && codeDescArray[0]==codeDescArray[1])
                    return item.MPFPlanID;
            }
            //  Create Code if necessary
            string newCode = codeDescArray[0].Replace(" ", "_").PadRight(20).Substring(0, 20).Trim().ToUpper();
            foreach (EMPFPlan item in list)
            {
                if (item.MPFPlanCode.Trim().Equals(newCode, StringComparison.CurrentCultureIgnoreCase))
                    return item.MPFPlanID;
            }
            if (CreateIfNotExist)
            {
                AppUtils.StartChildFunction(dbConn, "MPF001", 0);
                EMPFPlan mpfPlan = new EMPFPlan();
                mpfPlan.MPFPlanCode = newCode;
                mpfPlan.MPFPlanDesc = codeDescArray[1];

                EMPFPlan.db.insert(dbConn, mpfPlan);
                AppUtils.EndChildFunction(dbConn);
                return mpfPlan.MPFPlanID;
            }
            return 0;
            //DBManager db = EMPFPlan.db;
            //DBFilter dbfilter = new DBFilter();
            //string[] codeDescArray = GetCodeDescArray(NameOrCode);

            //OR orTerms = new OR();

            //orTerms.add(new Match("MPFPlanCode", codeDescArray[0]));
            //DBFieldTranscoder transcoder = EMPFPlan.db.getField("MPFPlanCode").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("MPFPlanCode", transcoder.toDB(codeDescArray[0])));

            //orTerms.add(new Match("MPFPlanDesc", codeDescArray[1]));
            //transcoder = EMPFPlan.db.getField("MPFPlanDesc").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("MPFPlanDesc", transcoder.toDB(codeDescArray[1]))); 

            //dbfilter.add(orTerms);
            //ArrayList list = db.select(dbConn, dbfilter);
            //if (list.Count > 0)
            //    return ((EMPFPlan)list[0]).MPFPlanID;
            //dbfilter = new DBFilter();
            //orTerms = new OR();
            //orTerms.add(new Match("MPFPlanCode", newCode));
            //transcoder = EMPFPlan.db.getField("MPFPlanCode").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("MPFPlanCode", transcoder.toDB(newCode)));
            //dbfilter.add(orTerms);

            //list = db.select(dbConn, dbfilter);
            //if (list.Count > 0)
            //    return ((EMPFPlan)list[0]).MPFPlanID;
            //else
            //    if (CreateIfNotExist)
            //    {
            //        EMPFPlan mpfPlan = new EMPFPlan();
            //        mpfPlan.MPFPlanCode = newCode;
            //        mpfPlan.MPFPlanDesc = codeDescArray[1];

            //        db.insert(dbConn, mpfPlan);
            //        return mpfPlan.MPFPlanID;
            //    }
            //    else
            //        return 0;
        }

        public static int GetLeaveCodeID(DatabaseConnection dbConn, string NameOrCode, bool CreateIfNotExist, int CreaterUserID)
        {
            if (string.IsNullOrEmpty(NameOrCode))
                return 0;

            string[] codeDescArray = GetCodeDescArray(NameOrCode);
            ArrayList list = ELeaveCode.db.select(dbConn, new DBFilter());
            //  Search code first
            foreach (ELeaveCode item in list)
            {
                if (item.LeaveCode.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase))
                    return item.LeaveCodeID;
            }
            foreach (ELeaveCode item in list)
            {
                if (item.LeaveCodeDesc.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase) && codeDescArray[0]==codeDescArray[1])
                    return item.LeaveCodeID;
            }
            //  Create Code if necessary
            string newCode = codeDescArray[0].Replace(" ", "_").PadRight(20).Substring(0, 20).Trim().ToUpper();
            foreach (ELeaveCode item in list)
            {
                if (item.LeaveCode.Trim().Equals(newCode, StringComparison.CurrentCultureIgnoreCase))
                    return item.LeaveCodeID;
            }
            if (CreateIfNotExist)
            {
                AppUtils.StartChildFunction(dbConn, "LEV002", 0);
                ELeaveCode leaveCode = new ELeaveCode();
                leaveCode.LeaveCode = newCode;
                leaveCode.LeaveCodeDesc = codeDescArray[1];

                ELeaveCode.db.insert(dbConn, leaveCode);
                AppUtils.EndChildFunction(dbConn);
                return leaveCode.LeaveCodeID;
            }
            return 0;
            //DBManager db = ELeaveCode.db;
            //DBFilter dbfilter = new DBFilter();
            //string[] codeDescArray = GetCodeDescArray(NameOrCode);

            //OR orTerms = new OR();

            //orTerms.add(new Match("LeaveCode", codeDescArray[0]));
            //DBFieldTranscoder transcoder = ELeaveCode.db.getField("LeaveCode").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("LeaveCode", transcoder.toDB(codeDescArray[0])));

            //orTerms.add(new Match("LeaveCodeDesc", codeDescArray[1]));
            //transcoder = ELeaveCode.db.getField("LeaveCodeDesc").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("LeaveCodeDesc", transcoder.toDB(codeDescArray[1])));

            //dbfilter.add(orTerms);
            //ArrayList list = db.select(dbConn, dbfilter);
            //if (list.Count > 0)
            //    return ((ELeaveCode)list[0]).LeaveCodeID;
            //dbfilter = new DBFilter();

            //orTerms = new OR();
            //orTerms.add(new Match("LeaveCode", newCode));
            //transcoder = ELeaveCode.db.getField("LeaveCode").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("LeaveCode", transcoder.toDB(newCode)));
            //dbfilter.add(orTerms);

            //list = db.select(dbConn, dbfilter);
            //if (list.Count > 0)
            //    return ((ELeaveCode)list[0]).LeaveCodeID;
            //else
            //    if (CreateIfNotExist)
            //    {
            //        ELeaveCode leaveCode = new ELeaveCode();
            //        leaveCode.LeaveCode = newCode;
            //        leaveCode.LeaveCodeDesc = codeDescArray[1];

            //        db.insert(dbConn, leaveCode);
            //        return leaveCode.LeaveCodeID;
            //    }
            //    else
            //        return 0;
        }
        public static int GetLeaveTypeID(DatabaseConnection dbConn, string NameOrCode, bool CreateIfNotExist, int CreaterUserID)
        {
            if (string.IsNullOrEmpty(NameOrCode))
                return 0;

            string[] codeDescArray = GetCodeDescArray(NameOrCode);
            ArrayList list = ELeaveType.db.select(dbConn, new DBFilter());
            //  Search code first
            foreach (ELeaveType item in list)
            {
                if (item.LeaveType.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase))
                    return item.LeaveTypeID;
            }
            foreach (ELeaveType item in list)
            {
                if (item.LeaveTypeDesc.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase) && codeDescArray[0]==codeDescArray[1])
                    return item.LeaveTypeID;
            }
            //  Create Code if necessary
            string newCode = codeDescArray[0].Replace(" ", "_").PadRight(20).Substring(0, 20).Trim().ToUpper();
            foreach (ELeaveType item in list)
            {
                if (item.LeaveType.Trim().Equals(newCode, StringComparison.CurrentCultureIgnoreCase))
                    return item.LeaveTypeID;
            }
            if (CreateIfNotExist)
            {
                AppUtils.StartChildFunction(dbConn, "LEV001", 0);
                ELeaveType leaveType = new ELeaveType();
                leaveType.LeaveType = newCode;
                leaveType.LeaveTypeDesc = codeDescArray[1];

                ELeaveType.db.insert(dbConn, leaveType);
                AppUtils.EndChildFunction(dbConn);
                return leaveType.LeaveTypeID;
            }
            return 0;
        }

        public static int GetAVCPlanID(DatabaseConnection dbConn, string NameOrCode, bool CreateIfNotExist, int CreaterUserID)
        {
            if (string.IsNullOrEmpty(NameOrCode))
                return 0;

            string[] codeDescArray = GetCodeDescArray(NameOrCode);
            ArrayList list = EAVCPlan.db.select(dbConn, new DBFilter());
            //  Search code first
            foreach (EAVCPlan item in list)
            {
                if (item.AVCPlanCode.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase))
                    return item.AVCPlanID;
            }
            foreach (EAVCPlan item in list)
            {
                if (item.AVCPlanDesc.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase) && codeDescArray[0]==codeDescArray[1])
                    return item.AVCPlanID;
            }
            //  Create Code if necessary
            string newCode = codeDescArray[0].Replace(" ", "_").PadRight(20).Substring(0, 20).Trim().ToUpper();
            foreach (EAVCPlan item in list)
            {
                if (item.AVCPlanCode.Trim().Equals(newCode, StringComparison.CurrentCultureIgnoreCase))
                    return item.AVCPlanID;
            }
            if (CreateIfNotExist)
            {
                AppUtils.StartChildFunction(dbConn, "MPF003", 0);
                EAVCPlan avcPlan = new EAVCPlan();
                avcPlan.AVCPlanCode = newCode;
                avcPlan.AVCPlanDesc = codeDescArray[1];

                EAVCPlan.db.insert(dbConn, avcPlan);
                AppUtils.EndChildFunction(dbConn);
                return avcPlan.AVCPlanID;
            }
            return 0;

            //DBManager db = EAVCPlan.db;
            //DBFilter dbfilter = new DBFilter();
            //string[] codeDescArray = GetCodeDescArray(NameOrCode);

            //OR orTerms = new OR();

            //orTerms.add(new Match("AVCPlanCode", codeDescArray[0]));
            //DBFieldTranscoder transcoder = EAVCPlan.db.getField("AVCPlanCode").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("AVCPlanCode", transcoder.toDB(codeDescArray[0])));

            //orTerms.add(new Match("AVCPlanDesc", codeDescArray[1]));
            //transcoder = EAVCPlan.db.getField("AVCPlanDesc").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("AVCPlanDesc", transcoder.toDB(codeDescArray[1])));

            //dbfilter.add(orTerms);
            //ArrayList list = db.select(dbConn, dbfilter);
            //if (list.Count > 0)
            //    return ((EAVCPlan)list[0]).AVCPlanID;
            //dbfilter = new DBFilter();

            //orTerms = new OR();
            //orTerms.add(new Match("AVCPlanCode", newCode));
            //transcoder = EAVCPlan.db.getField("AVCPlanCode").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("AVCPlanCode", transcoder.toDB(newCode)));
            //dbfilter.add(orTerms);

            //list = db.select(dbConn, dbfilter);
            //if (list.Count > 0)
            //    return ((EAVCPlan)list[0]).AVCPlanID;
            //else
            //    if (CreateIfNotExist)
            //    {
            //        EAVCPlan avcPlan = new EAVCPlan();
            //        avcPlan.AVCPlanCode = newCode;
            //        avcPlan.AVCPlanDesc = codeDescArray[1];

            //        db.insert(dbConn, avcPlan);
            //        return avcPlan.AVCPlanID;
            //    }
            //    else
            //        return 0;
        }
        public static int GetORSOPlanID(DatabaseConnection dbConn, string NameOrCode, bool CreateIfNotExist, int CreaterUserID)
        {
            if (string.IsNullOrEmpty(NameOrCode))
                return 0;

            string[] codeDescArray = GetCodeDescArray(NameOrCode);
            ArrayList list = EORSOPlan.db.select(dbConn, new DBFilter());
            //  Search code first
            foreach (EORSOPlan item in list)
            {
                if (item.ORSOPlanCode.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase))
                    return item.ORSOPlanID;
            }
            foreach (EORSOPlan item in list)
            {
                if (item.ORSOPlanDesc.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase) && codeDescArray[0]==codeDescArray[1])
                    return item.ORSOPlanID;
            }
            //  Create Code if necessary
            string newCode = codeDescArray[0].Replace(" ", "_").PadRight(20).Substring(0, 20).Trim().ToUpper();
            foreach (EORSOPlan item in list)
            {
                if (item.ORSOPlanCode.Trim().Equals(newCode, StringComparison.CurrentCultureIgnoreCase))
                    return item.ORSOPlanID;
            }
            if (CreateIfNotExist)
            {
                AppUtils.StartChildFunction(dbConn, "MPF004", 0);
                EORSOPlan ORSOPlan = new EORSOPlan();
                ORSOPlan.ORSOPlanCode = newCode;
                ORSOPlan.ORSOPlanDesc = codeDescArray[1];

                EORSOPlan.db.insert(dbConn, ORSOPlan);
                AppUtils.EndChildFunction(dbConn);
                return ORSOPlan.ORSOPlanID;
            }
            return 0;
        }
        public static int GetQualificationID(DatabaseConnection dbConn, string NameOrCode, bool CreateIfNotExist, int CreaterUserID)
        {
            if (string.IsNullOrEmpty(NameOrCode))
                return 0;

            string[] codeDescArray = GetCodeDescArray(NameOrCode);
            ArrayList list = EQualification.db.select(dbConn, new DBFilter());
            //  Search code first
            foreach (EQualification item in list)
            {
                if (item.QualificationCode.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase))
                    return item.QualificationID;
            }
            foreach (EQualification item in list)
            {
                if (item.QualificationDesc.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase) && codeDescArray[0]==codeDescArray[1])
                    return item.QualificationID;
            }
            //  Create Code if necessary
            string newCode = codeDescArray[0].Replace(" ", "_").PadRight(20).Substring(0, 20).Trim().ToUpper();
            foreach (EQualification item in list)
            {
                if (item.QualificationCode.Trim().Equals(newCode, StringComparison.CurrentCultureIgnoreCase))
                    return item.QualificationID;
            }
            if (CreateIfNotExist)
            {
                AppUtils.StartChildFunction(dbConn, "SYS013", 0);
                EQualification qualification = new EQualification();
                qualification.QualificationCode = newCode;
                qualification.QualificationDesc = codeDescArray[1];

                EQualification.db.insert(dbConn, qualification);
                AppUtils.EndChildFunction(dbConn);
                return qualification.QualificationID;
            }
            return 0;
            //DBManager db = EQualification.db;
            //DBFilter dbfilter = new DBFilter();
            //string[] codeDescArray = GetCodeDescArray(NameOrCode);

            //OR orTerms = new OR();

            //orTerms.add(new Match("QualificationCode", codeDescArray[0]));
            //DBFieldTranscoder transcoder = EQualification.db.getField("QualificationCode").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("QualificationCode", transcoder.toDB(codeDescArray[0])));

            //orTerms.add(new Match("QualificationDesc", codeDescArray[1]));
            //transcoder = EQualification.db.getField("QualificationDesc").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("QualificationDesc", transcoder.toDB(codeDescArray[1])));

            //dbfilter.add(orTerms);
            //ArrayList list = db.select(dbConn, dbfilter);
            //if (list.Count > 0)
            //    return ((EQualification)list[0]).QualificationID;
            //dbfilter = new DBFilter();

            //orTerms = new OR();
            //orTerms.add(new Match("QualificationCode", newCode));
            //transcoder = EQualification.db.getField("QualificationCode").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("QualificationCode", transcoder.toDB(newCode)));
            //dbfilter.add(orTerms);

            //list = db.select(dbConn, dbfilter);
            //if (list.Count > 0)
            //    return ((EQualification)list[0]).QualificationID;
            //else
            //    if (CreateIfNotExist)
            //    {
            //        EQualification qualification = new EQualification();
            //        qualification.QualificationCode = newCode;
            //        qualification.QualificationDesc = codeDescArray[1];

            //        db.insert(dbConn, qualification);
            //        return qualification.QualificationID;
            //    }
            //    else
            //        return 0;
        }

        public static int GetSkillID(DatabaseConnection dbConn, string NameOrCode, bool CreateIfNotExist, int CreaterUserID)
        {
            if (string.IsNullOrEmpty(NameOrCode))
                return 0;

            string[] codeDescArray = GetCodeDescArray(NameOrCode);
            ArrayList list = ESkill.db.select(dbConn, new DBFilter());
            //  Search code first
            foreach (ESkill item in list)
            {
                if (item.SkillCode.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase))
                    return item.SkillID;
            }
            foreach (ESkill item in list)
            {
                if (item.SkillDesc.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase) && codeDescArray[0]==codeDescArray[1])
                    return item.SkillID;
            }
            //  Create Code if necessary
            string newCode = codeDescArray[0].Replace(" ", "_").PadRight(20).Substring(0, 20).Trim().ToUpper();
            foreach (ESkill item in list)
            {
                if (item.SkillCode.Trim().Equals(newCode, StringComparison.CurrentCultureIgnoreCase))
                    return item.SkillID;
            }
            if (CreateIfNotExist)
            {
                AppUtils.StartChildFunction(dbConn, "SYS014", 0);
                ESkill skill = new ESkill();
                skill.SkillCode = newCode;
                skill.SkillDesc = codeDescArray[1];

                ESkill.db.insert(dbConn, skill);
                AppUtils.EndChildFunction(dbConn);
                return skill.SkillID;
            }
            return 0;

            //DBManager db = ESkill.db;
            //DBFilter dbfilter = new DBFilter();
            //string[] codeDescArray = GetCodeDescArray(NameOrCode);

            //OR orTerms = new OR();

            //orTerms.add(new Match("SkillCode", codeDescArray[0]));
            //DBFieldTranscoder transcoder = ESkill.db.getField("SkillCode").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("SkillCode", transcoder.toDB(codeDescArray[0])));

            //orTerms.add(new Match("SkillDesc", codeDescArray[1]));
            //transcoder = ESkill.db.getField("SkillDesc").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("SkillDesc", transcoder.toDB(codeDescArray[1])));

            //dbfilter.add(orTerms);
            //ArrayList list = db.select(dbConn, dbfilter);
            //if (list.Count > 0)
            //    return ((ESkill)list[0]).SkillID;
            //dbfilter = new DBFilter();

            //orTerms = new OR();
            //orTerms.add(new Match("SkillCode", newCode));
            //transcoder = ESkill.db.getField("SkillCode").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("SkillCode", transcoder.toDB(newCode)));
            //dbfilter.add(orTerms);

            //list = db.select(dbConn, dbfilter);
            //if (list.Count > 0)
            //    return ((ESkill)list[0]).SkillID;
            //else
            //    if (CreateIfNotExist)
            //    {
            //        ESkill skill = new ESkill();
            //        skill.SkillCode = newCode;
            //        skill.SkillDesc = codeDescArray[1];

            //        db.insert(dbConn, skill);
            //        return skill.SkillID;
            //    }
            //    else
            //        return 0;
        }

        public static int GetSkillLevelID(DatabaseConnection dbConn, string NameOrCode, bool CreateIfNotExist, int CreaterUserID)
        {
            if (string.IsNullOrEmpty(NameOrCode))
                return 0;

            string[] codeDescArray = GetCodeDescArray(NameOrCode);
            ArrayList list = ESkillLevel.db.select(dbConn, new DBFilter());
            //  Search code first
            foreach (ESkillLevel item in list)
            {
                if (item.SkillLevelCode.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase))
                    return item.SkillLevelID;
            }
            foreach (ESkillLevel item in list)
            {
                if (item.SkillLevelDesc.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase) && codeDescArray[0]==codeDescArray[1])
                    return item.SkillLevelID;
            }
            //  Create Code if necessary
            string newCode = codeDescArray[0].Replace(" ", "_").PadRight(20).Substring(0, 20).Trim().ToUpper();
            foreach (ESkillLevel item in list)
            {
                if (item.SkillLevelCode.Trim().Equals(newCode, StringComparison.CurrentCultureIgnoreCase))
                    return item.SkillLevelID;
            }
            if (CreateIfNotExist)
            {
                AppUtils.StartChildFunction(dbConn, "SYS015", 0);
                ESkillLevel skillLevel = new ESkillLevel();
                skillLevel.SkillLevelCode = newCode;
                skillLevel.SkillLevelDesc = codeDescArray[1];

                ESkillLevel.db.insert(dbConn, skillLevel);
                AppUtils.EndChildFunction(dbConn);
                return skillLevel.SkillLevelID;
            }
            return 0;
            //DBManager db = ESkillLevel.db;
            //DBFilter dbfilter = new DBFilter();
            //string[] codeDescArray = GetCodeDescArray(NameOrCode);

            //OR orTerms = new OR();

            //orTerms.add(new Match("SkillLevelCode", codeDescArray[0]));
            //DBFieldTranscoder transcoder = ESkillLevel.db.getField("SkillLevelCode").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("SkillLevelCode", transcoder.toDB(codeDescArray[0])));

            //orTerms.add(new Match("SkillLevelDesc", codeDescArray[1]));
            //transcoder = ESkillLevel.db.getField("SkillLevelDesc").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("SkillLevelDesc", transcoder.toDB(codeDescArray[1])));

            //dbfilter.add(orTerms);
            //ArrayList list = db.select(dbConn, dbfilter);
            //if (list.Count > 0)
            //    return ((ESkillLevel)list[0]).SkillLevelID;
            //dbfilter = new DBFilter();

            //orTerms = new OR();
            //orTerms.add(new Match("SkillLevelCode", newCode));
            //transcoder = ESkillLevel.db.getField("SkillLevelCode").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("SkillLevelCode", transcoder.toDB(newCode)));
            //dbfilter.add(orTerms);

            //list = db.select(dbConn, dbfilter);
            //if (list.Count > 0)
            //    return ((ESkillLevel)list[0]).SkillLevelID;
            //else
            //    if (CreateIfNotExist)
            //    {
            //        ESkillLevel SkillLevel = new ESkillLevel();
            //        SkillLevel.SkillLevelCode = newCode;
            //        SkillLevel.SkillLevelDesc = codeDescArray[1];

            //        db.insert(dbConn, SkillLevel);
            //        return SkillLevel.SkillLevelID;
            //    }
            //    else
            //        return 0;
        }

        public static int GetCessationReasonID(DatabaseConnection dbConn, string NameOrCode, bool CreateIfNotExist, int CreaterUserID)
        {
            if (string.IsNullOrEmpty(NameOrCode))
                return 0;

            string[] codeDescArray = GetCodeDescArray(NameOrCode);
            ArrayList list = ECessationReason.db.select(dbConn, new DBFilter());
            //  Search code first
            foreach (ECessationReason item in list)
            {
                if (item.CessationReasonCode.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase))
                    return item.CessationReasonID;
            }
            foreach (ECessationReason item in list)
            {
                if (item.CessationReasonDesc.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase) && codeDescArray[0]==codeDescArray[1])
                    return item.CessationReasonID;
            }
            //  Create Code if necessary
            string newCode = codeDescArray[0].Replace(" ", "_").PadRight(20).Substring(0, 20).Trim().ToUpper();
            foreach (ECessationReason item in list)
            {
                if (item.CessationReasonCode.Trim().Equals(newCode, StringComparison.CurrentCultureIgnoreCase))
                    return item.CessationReasonID;
            }
            if (CreateIfNotExist)
            {
                AppUtils.StartChildFunction(dbConn, "SYS011", 0);
                ECessationReason cessationReason = new ECessationReason();
                cessationReason.CessationReasonCode = newCode;
                cessationReason.CessationReasonDesc = codeDescArray[1];

                ECessationReason.db.insert(dbConn, cessationReason);
                AppUtils.EndChildFunction(dbConn);
                return cessationReason.CessationReasonID;
            }
            return 0;

            //DBManager db = ECessationReason.db;
            //DBFilter dbfilter = new DBFilter();
            //string[] codeDescArray = GetCodeDescArray(NameOrCode);

            //OR orTerms = new OR();
            //orTerms.add(new Match("CessationReasonCode", codeDescArray[0]));
            //DBFieldTranscoder transcoder = ECessationReason.db.getField("CessationReasonCode").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("CessationReasonCode", transcoder.toDB(codeDescArray[0])));

            //orTerms.add(new Match("CessationReasonDesc", codeDescArray[1]));
            //transcoder = ECessationReason.db.getField("CessationReasonDesc").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("CessationReasonDesc", transcoder.toDB(codeDescArray[1])));

            //dbfilter.add(orTerms);
            //ArrayList list = db.select(dbConn, dbfilter);
            //if (list.Count > 0)
            //    return ((ECessationReason)list[0]).CessationReasonID;
            //dbfilter = new DBFilter();

            //orTerms = new OR();
            //orTerms.add(new Match("CessationReasonCode", newCode));
            //transcoder = ECessationReason.db.getField("CessationReasonCode").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("CessationReasonCode", transcoder.toDB(newCode)));
            //dbfilter.add(orTerms);

            //list = db.select(dbConn, dbfilter);
            //if (list.Count > 0)
            //    return ((ECessationReason)list[0]).CessationReasonID;
            //else
            //    if (CreateIfNotExist)
            //    {
            //        ECessationReason CessationReason = new ECessationReason();
            //        CessationReason.CessationReasonCode = newCode;
            //        CessationReason.CessationReasonDesc = codeDescArray[1];

            //        db.insert(dbConn, CessationReason);
            //        return CessationReason.CessationReasonID;

            //    }
            //    else
            //        return 0;
        }

        public static int GetCostCenterID(DatabaseConnection dbConn, string NameOrCode, bool CreateIfNotExist, int CreaterUserID)
        {
            if (string.IsNullOrEmpty(NameOrCode))
                return 0;

            string[] codeDescArray = GetCodeDescArray(NameOrCode);
            ArrayList list = ECostCenter.db.select(dbConn, new DBFilter());
            //  Search code first
            foreach (ECostCenter item in list)
            {
                if (item.CostCenterCode.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase))
                    return item.CostCenterID;
            }
            foreach (ECostCenter item in list)
            {
                if (item.CostCenterDesc.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase) && codeDescArray[0]==codeDescArray[1])
                    return item.CostCenterID;
            }
            //  Create Code if necessary
            string newCode = codeDescArray[0].Replace(" ", "_").PadRight(20).Substring(0, 20).Trim().ToUpper();
            foreach (ECostCenter item in list)
            {
                if (item.CostCenterCode.Trim().Equals(newCode, StringComparison.CurrentCultureIgnoreCase))
                    return item.CostCenterID;
            }
            if (CreateIfNotExist)
            {
                AppUtils.StartChildFunction(dbConn, "CST000", 0);
                ECostCenter costCenter = new ECostCenter();
                costCenter.CostCenterCode = newCode;
                costCenter.CostCenterDesc = codeDescArray[1];

                ECostCenter.db.insert(dbConn, costCenter);
                AppUtils.EndChildFunction(dbConn);
                return costCenter.CostCenterID;
            }
            return 0;

            //DBManager db = ECostCenter.db;
            //DBFilter dbfilter = new DBFilter();
            //string[] codeDescArray = GetCodeDescArray(NameOrCode);

            //OR orTerms = new OR();

            //orTerms.add(new Match("CostCenterCode", codeDescArray[0]));
            //DBFieldTranscoder transcoder = ECostCenter.db.getField("CostCenterCode").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("CostCenterCode", transcoder.toDB(codeDescArray[0])));

            //orTerms.add(new Match("CostCenterDesc", codeDescArray[1]));
            //transcoder = ECostCenter.db.getField("CostCenterDesc").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("CostCenterDesc", transcoder.toDB(codeDescArray[1]))); 

            //dbfilter.add(orTerms);
            //ArrayList list = db.select(dbConn, dbfilter);
            //if (list.Count > 0)
            //    return ((ECostCenter)list[0]).CostCenterID;
            //dbfilter = new DBFilter();

            //orTerms = new OR();
            //orTerms.add(new Match("CostCenterCode", newCode));
            //transcoder = ECostCenter.db.getField("CostCenterCode").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("CostCenterCode", transcoder.toDB(newCode)));
            //dbfilter.add(orTerms);

            //list = db.select(dbConn, dbfilter);
            //if (list.Count > 0)
            //    return ((ECostCenter)list[0]).CostCenterID;
            //else
            //    if (CreateIfNotExist)
            //    {
            //        ECostCenter CostCenter = new ECostCenter();
            //        CostCenter.CostCenterCode = newCode;
            //        CostCenter.CostCenterDesc = codeDescArray[1];

            //        db.insert(dbConn, CostCenter);
            //        return CostCenter.CostCenterID;

            //    }
            //    else
            //        return 0;
        }

        public static int GetRosterCodeID(DatabaseConnection dbConn, string NameOrCode)
        {
            DateTime inTime, outTime;
            return GetRosterCodeID(dbConn, NameOrCode, out inTime, out outTime);
        }
        public static int GetRosterCodeID(DatabaseConnection dbConn, string NameOrCode, out DateTime OverrideInTime, out DateTime OverrideOutTime)
        {
            NameOrCode = NameOrCode.Trim();
            if (string.IsNullOrEmpty(NameOrCode))
            {
                OverrideInTime = new DateTime();
                OverrideOutTime = new DateTime();
                return 0;
            }

            string[] codeDescArray = GetCodeDescArray(NameOrCode);
            ArrayList list = ERosterCode.db.select(dbConn, new DBFilter());
            //  Search code first
            foreach (ERosterCode item in list)
            {
                if (item.RosterCode.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase))
                {
                    OverrideInTime = new DateTime();
                    OverrideOutTime = new DateTime();
                    return item.RosterCodeID;
                }
            }
            foreach (ERosterCode item in list)
            {
                if (item.RosterCodeDesc.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase) && codeDescArray[0]==codeDescArray[1])
                {
                    OverrideInTime = new DateTime();
                    OverrideOutTime = new DateTime();
                    return item.RosterCodeID;
                }
            }

            int openBlanketPos = NameOrCode.LastIndexOf("(");
            int closeBlanketPos = NameOrCode.LastIndexOf(")");
            if (openBlanketPos < closeBlanketPos && openBlanketPos > 0)
            {
                string timeString = NameOrCode.Substring(openBlanketPos + 1, closeBlanketPos - openBlanketPos - 1).Trim();
                string[] timePart = timeString.Split(new string[] { "-", "~" }, StringSplitOptions.None);
                if (timePart.Length.Equals(2))
                {
                    DateTime timeFrom;
                    DateTime timeTo;

                    if (DateTime.TryParseExact(timePart[0].Trim(), "HHmm", null, DateTimeStyles.None, out timeFrom) && DateTime.TryParseExact(timePart[1].Trim(), "HHmm", null, DateTimeStyles.None, out timeTo))
                    {
                        OverrideInTime = timeFrom;
                        OverrideOutTime = timeTo;
                        NameOrCode = NameOrCode.Substring(0, openBlanketPos);
                        return GetRosterCodeID(dbConn, NameOrCode);
                    }

                }
            }
            OverrideInTime = new DateTime();
            OverrideOutTime = new DateTime();
            return 0;

            //DBFilter dbfilter = new DBFilter();
            //OR orTerms = new OR();
            //string[] codeDescArray = GetCodeDescArray(NameOrCode);

            //orTerms.add(new Match("RosterCode", codeDescArray[0]));
            //DBFieldTranscoder transcoder = ERosterCode.db.getField("RosterCode").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("RosterCode", transcoder.toDB(codeDescArray[0])));

            //orTerms.add(new Match("RosterCodeDesc", codeDescArray[1]));
            //transcoder = ERosterCode.db.getField("RosterCodeDesc").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("RosterCodeDesc", transcoder.toDB(codeDescArray[1]))); 


            //dbfilter.add(orTerms);
            //ArrayList list = ERosterCode.db.select(dbConn, dbfilter);
            //if (list.Count > 0)
            //    return ((ERosterCode)list[0]).RosterCodeID;
            //else
            //    return 0;
        }

        public static int GetPermitID(DatabaseConnection dbConn, string NameOrCode, bool CreateIfNotExist, int CreaterUserID)
        {
            if (string.IsNullOrEmpty(NameOrCode))
                return 0;

            string[] codeDescArray = GetCodeDescArray(NameOrCode);
            ArrayList list = EPermitType.db.select(dbConn, new DBFilter());
            //  Search code first
            foreach (EPermitType item in list)
            {
                if (item.PermitTypeCode.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase))
                    return item.PermitTypeID;
            }
            foreach (EPermitType item in list)
            {
                if (item.PermitTypeDesc.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase) && codeDescArray[0]==codeDescArray[1])
                    return item.PermitTypeID;
            }
            //  Create Code if necessary
            string newCode = codeDescArray[0].Replace(" ", "_").PadRight(20).Substring(0, 20).Trim().ToUpper();
            foreach (EPermitType item in list)
            {
                if (item.PermitTypeCode.Trim().Equals(newCode, StringComparison.CurrentCultureIgnoreCase))
                    return item.PermitTypeID;
            }
            if (CreateIfNotExist)
            {
                AppUtils.StartChildFunction(dbConn, "SYS018", 0);

                EPermitType permitType = new EPermitType();
                permitType.PermitTypeCode = newCode;
                permitType.PermitTypeDesc = codeDescArray[1];

                EPermitType.db.insert(dbConn, permitType);
                AppUtils.EndChildFunction(dbConn);
                return permitType.PermitTypeID;
            }
            return 0;

            //DBManager db = EPermitType.db;
            //DBFilter dbfilter = new DBFilter();
            //string[] codeDescArray = GetCodeDescArray(NameOrCode);

            //OR orTerms = new OR();

            //orTerms.add(new Match("PermitTypeCode", codeDescArray[0]));
            //DBFieldTranscoder transcoder = EPaymentCode.db.getField("PermitTypeCode").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("PermitTypeCode", transcoder.toDB(codeDescArray[0])));

            //orTerms.add(new Match("PermitTypeDesc", codeDescArray[1]));
            //transcoder = EPaymentCode.db.getField("PermitTypeDesc").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("PermitTypeDesc", transcoder.toDB(codeDescArray[1]))); 

            //dbfilter.add(orTerms);
            //ArrayList list = db.select(dbConn, dbfilter);
            //if (list.Count > 0)
            //    return ((EPermitType)list[0]).PermitTypeID;
            //dbfilter = new DBFilter();

            //orTerms = new OR();
            //orTerms.add(new Match("PermitCode", newCode));
            //transcoder = EPermitType.db.getField("PermitCode").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("PermitCode", transcoder.toDB(newCode)));
            //dbfilter.add(orTerms);

            //list = db.select(dbConn, dbfilter);
            //if (list.Count > 0)
            //    return ((EPermitType)list[0]).PermitTypeID;
            //else
            //    if (CreateIfNotExist)
            //    {
            //        EPermitType Permit = new EPermitType();
            //        Permit.PermitTypeCode = newCode;
            //        Permit.PermitTypeDesc = codeDescArray[1];

            //        db.insert(dbConn, Permit);
            //        return Permit.PermitTypeID;
            //    }
            //    else
            //        return 0;
        }

        public static int GetWorkHourPatternID(DatabaseConnection dbConn, string NameOrCode)
        {
            if (string.IsNullOrEmpty(NameOrCode))
                return 0;

            string[] codeDescArray = GetCodeDescArray(NameOrCode);
            ArrayList list = EWorkHourPattern.db.select(dbConn, new DBFilter());
            //  Search code first
            foreach (EWorkHourPattern item in list)
            {
                if (item.WorkHourPatternCode.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase))
                    return item.WorkHourPatternID;
            }
            foreach (EWorkHourPattern item in list)
            {
                if (item.WorkHourPatternDesc.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase) && codeDescArray[0]==codeDescArray[1])
                    return item.WorkHourPatternID;
            }
            return 0;

            //DBFilter dbfilter = new DBFilter();
            //OR orTerms = new OR();
            //string[] codeDescArray = GetCodeDescArray(NameOrCode);

            //orTerms.add(new Match("RosterCode", codeDescArray[0]));
            //DBFieldTranscoder transcoder = ERosterCode.db.getField("RosterCode").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("RosterCode", transcoder.toDB(codeDescArray[0])));

            //orTerms.add(new Match("RosterCodeDesc", codeDescArray[1]));
            //transcoder = ERosterCode.db.getField("RosterCodeDesc").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("RosterCodeDesc", transcoder.toDB(codeDescArray[1]))); 


            //dbfilter.add(orTerms);
            //ArrayList list = ERosterCode.db.select(dbConn, dbfilter);
            //if (list.Count > 0)
            //    return ((ERosterCode)list[0]).RosterCodeID;
            //else
            //    return 0;
        }

        public static int GetRosterTableGroupID(DatabaseConnection dbConn, string NameOrCode)
        {
            if (string.IsNullOrEmpty(NameOrCode))
                return 0;

            string[] codeDescArray = GetCodeDescArray(NameOrCode);
            ArrayList list = ERosterTableGroup.db.select(dbConn, new DBFilter());
            //  Search code first
            foreach (ERosterTableGroup item in list)
            {
                if (item.RosterTableGroupCode.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase))
                    return item.RosterTableGroupID;
            }
            foreach (ERosterTableGroup item in list)
            {
                if (item.RosterTableGroupDesc.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase) && codeDescArray[0]==codeDescArray[1])
                    return item.RosterTableGroupID;
            }
            return 0;

            //DBFilter dbfilter = new DBFilter();
            //OR orTerms = new OR();
            //string[] codeDescArray = GetCodeDescArray(NameOrCode);

            //orTerms.add(new Match("RosterCode", codeDescArray[0]));
            //DBFieldTranscoder transcoder = ERosterCode.db.getField("RosterCode").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("RosterCode", transcoder.toDB(codeDescArray[0])));

            //orTerms.add(new Match("RosterCodeDesc", codeDescArray[1]));
            //transcoder = ERosterCode.db.getField("RosterCodeDesc").transcoder;
            //if (transcoder != null)
            //    orTerms.add(new Match("RosterCodeDesc", transcoder.toDB(codeDescArray[1]))); 


            //dbfilter.add(orTerms);
            //ArrayList list = ERosterCode.db.select(dbConn, dbfilter);
            //if (list.Count > 0)
            //    return ((ERosterCode)list[0]).RosterCodeID;
            //else
            //    return 0;
        }
        // Start 0000070, Miranda, 2014-09-08
        public static int GetBenefitPlanID(DatabaseConnection dbConn, string NameOrCode, bool CreateIfNotExist, int CreaterUserID)
        {
            if (string.IsNullOrEmpty(NameOrCode))
                return 0;
            string[] codeDescArray = GetCodeDescArray(NameOrCode);
            ArrayList list = EBenefitPlan.db.select(dbConn, new DBFilter());
            //  Search code first
            foreach (EBenefitPlan item in list)
            {
                if (item.BenefitPlanCode.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase))
                    return item.BenefitPlanID;
            }
            foreach (EBenefitPlan item in list)
            {
                if (item.BenefitPlanDesc.Trim().Equals(codeDescArray[0].Trim(), StringComparison.CurrentCultureIgnoreCase) && codeDescArray[0] == codeDescArray[1])
                    return item.BenefitPlanID;
            }
            //  Create Code if necessary
            string newCode = codeDescArray[0].Replace(" ", "_").PadRight(20).Substring(0, 20).Trim().ToUpper();
            foreach (EBenefitPlan item in list)
            {
                if (item.BenefitPlanCode.Trim().Equals(newCode, StringComparison.CurrentCultureIgnoreCase))
                    return item.BenefitPlanID;
            }
            if (CreateIfNotExist)
            {
                AppUtils.StartChildFunction(dbConn, "SYS022", 0);

                EBenefitPlan benefitPlan = new EBenefitPlan();
                benefitPlan.BenefitPlanCode = newCode;
                benefitPlan.BenefitPlanDesc = codeDescArray[1];

                EBenefitPlan.db.insert(dbConn, benefitPlan);
                AppUtils.EndChildFunction(dbConn);
                return benefitPlan.BenefitPlanID;
            }
            return 0;

        }
        // End 0000070, Miranda, 2014-09-08

        public static int GetEmpIDFromCardNo(string CardNo)
        {
            //DBFilter dbfilter = new DBFilter();
            //dbfilter.add(new Match("EmpNo", EmpNo));
            //dbfilter.add(AppUtils.AddRankFilter(UserID, "EmpID", true));

            //ArrayList list = EEmpPersonalInfo.db.select(dbConn, dbfilter);
            //if (list.Count > 0)
            //    return ((EEmpPersonalInfo)list[0]).EmpID;
            //else
            //{
            //    DBFilter dbfilter2 = new DBFilter();
            //    dbfilter2.add(new Match("EmpNo", EmpNo));

            //    ArrayList list2 = EEmpPersonalInfo.db.select(dbConn, dbfilter2);
            //    if (list2.Count > 0)
            //        return -1;
            //    else
            //        return 0;
            //}
            return 0;
        }

        //public static Decimal toDecimalObject(object decimalValue)
        //{
        //    Decimal outValue = null; 
        //    if (Decimal.TryParse(decimalValue, outValue))
        //    {
        //        return outValue;
        //    }
        //    throw new Exception("Invalid Decimal Value");

        //}

        public static decimal toDecimal(object decimalExpression)
        {
            decimal m_result;

            if (!decimal.TryParse(decimalExpression.ToString(), out m_result))
            {
                // to-do raise error
            }
            return m_result;
        }

        public static int toInteger(object integerExpression)
        {
            int m_result;



            if (!int.TryParse(integerExpression.ToString(), out m_result))
            {
                //TO-DO raise error
            }

            return m_result; 
        }

        public static DateTime toDateTimeObject(object dateTimeExpression)
        {
            if (dateTimeExpression is DateTime)
                return (DateTime)dateTimeExpression;
            else
            {
                string dateTimeString = dateTimeExpression.ToString();
                if (string.IsNullOrEmpty(dateTimeString.Trim()))
                    return new DateTime();
                DateTime tryParseDateTime = new DateTime();
                DateTime tmpDateTime = new DateTime();

                //long tryEffDateString;

                if (DateTime.TryParseExact(dateTimeString, new string[] { "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd HH:mm" }, null, DateTimeStyles.None, out tryParseDateTime))
                    tmpDateTime = tryParseDateTime;
                else if (DateTime.TryParseExact(dateTimeString, new string[] { "yyyyMMdd", "yyyy-MM-dd" }, null, DateTimeStyles.None, out tryParseDateTime))
                    tmpDateTime = tryParseDateTime;
                else if (DateTime.TryParseExact(dateTimeString, new string[] { "HH:mm:ss", "HH:mm" }, null, DateTimeStyles.None, out tryParseDateTime))
                    tmpDateTime = tryParseDateTime;
                else if (DateTime.TryParse(dateTimeString, out tryParseDateTime))
                    tmpDateTime = tryParseDateTime;
                //else if (dateTimeString.Trim().Length == 8 && long.TryParse(dateTimeString, out tryEffDateString))
                //{
                //    tmpDateTime = new DateTime(int.Parse(dateTimeString.Substring(0, 4)), int.Parse(dateTimeString.Substring(4, 2)), int.Parse(dateTimeString.Substring(6, 2)));
                //}
                if (tmpDateTime.Ticks.Equals(0))
                    throw new Exception("Invalid Date/Time format");
                else if (tmpDateTime.Year < 1800)
                    throw new Exception("Year too small");
                return tmpDateTime;
            }
        }

        public static string toPaymentMethodCode(string paymentMethod)
        {
            if (!string.IsNullOrEmpty(paymentMethod))
                paymentMethod = paymentMethod.Trim();
            if (paymentMethod.Equals("Autopay", StringComparison.CurrentCultureIgnoreCase))
                return "A";
            else if (paymentMethod.Equals("Cash", StringComparison.CurrentCultureIgnoreCase))
                return "C";
            else if (paymentMethod.Equals("Cheque", StringComparison.CurrentCultureIgnoreCase))
                return "Q";
            else
                return "O";
        }

        public static string toRecurringPaymentUnit(string RecurringPaymentUnit)
        {
            if (!string.IsNullOrEmpty(RecurringPaymentUnit))
                RecurringPaymentUnit = RecurringPaymentUnit.Trim();
            if (RecurringPaymentUnit.Equals("Hourly", StringComparison.CurrentCultureIgnoreCase))
                return "H";
            else if (RecurringPaymentUnit.Equals("Daily", StringComparison.CurrentCultureIgnoreCase))
                return "D";
            else if (RecurringPaymentUnit.Equals("Once per payroll cycle", StringComparison.CurrentCultureIgnoreCase))
                return "P";
            else
                return string.Empty;
        }

        public static string toQualificationLearningMethod(string QualificationLearningMethod)
        {
            if (!string.IsNullOrEmpty(QualificationLearningMethod))
                QualificationLearningMethod = QualificationLearningMethod.Trim();

            if (QualificationLearningMethod.Replace(" ", "").Replace("-", "").Equals(EEmpQualification.LEARNING_METHOD_DESC_ONCAMPUS.Replace(" ", "").Replace("-", ""), StringComparison.CurrentCultureIgnoreCase))
                return EEmpQualification.LEARNING_METHOD_CODE_ONCAMPUS;
            else if (QualificationLearningMethod.Replace(" ", "").Replace("-", "").Equals(EEmpQualification.LEARNING_METHOD_DESC_DISTANCE_LEARNING.Replace(" ", "").Replace("-", ""), StringComparison.CurrentCultureIgnoreCase))
                return EEmpQualification.LEARNING_METHOD_CODE_DISTANCE_LEARNING;
            else
                return string.Empty;
        }
        public static bool toBoolean(string BooleanString)
        {

            if (string.IsNullOrEmpty(BooleanString))
                return false;
            else if (BooleanString.Equals("Y",StringComparison.CurrentCultureIgnoreCase)
            || BooleanString.Equals("Yes",StringComparison.CurrentCultureIgnoreCase)
            || BooleanString.Equals("T",StringComparison.CurrentCultureIgnoreCase)
            || BooleanString.Equals("True",StringComparison.CurrentCultureIgnoreCase))
                return true;
            else
                return false;
        }

        private static string GenerateCodeByNameWithIndex(string Name, int Index)
        {
            Name = Name.Replace(" ", "_").Substring(0, 10).ToUpper();

            string indexString = "_" + Index;
            return Name.Substring(0, Name.Length - indexString.Length) + indexString;
        }
    }
}