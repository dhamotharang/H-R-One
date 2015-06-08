using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using HROne.DataAccess;
using HROne.Lib.Entities;
using HROne.CommonLib;
using HROne.Lib;

/// <summary>
/// Summary description for AppUtils
/// </summary>
public abstract class AppUtils
{
    public static HROne.TaskService.TaskQueueService reportTaskQueueService = new HROne.TaskService.TaskQueueService(3);
    //  To prevent blocking from e-mail server, only 1 thread is assigned to e-mail service.
    public static HROne.TaskService.TaskQueueService emailTaskQueueService = new HROne.TaskService.TaskQueueService();

    //public static DateTime LastDayInMonth(DateTime m_dateValue)
    //{
    //    return m_dateValue.AddDays(-1 * m_dateValue.Day + 1).AddMonths(1).AddDays(-1);
    //}

    //public static DateTime FirstDayInMonth(DateTime m_dateValue)
    //{
    //    return m_dateValue.AddDays(-1 * m_dateValue.Day + 1);
    //}

    public static double Evaluate(string expression)
    {
        DataTable dt = new DataTable();
        expression = expression.Replace("%", "/100");
        try
        {
            return System.Convert.ToDouble(dt.Compute(expression, ""));
        }
        catch (Exception e)
        {
            return 0;
        }
    }

    public static System.Resources.ResourceManager getResourceManager()
    {
        return DBUtils.getResourceManager();
    }

    public static DBTerm GetPayemntCodeDBTermByPaymentType(DatabaseConnection dbConn, string pField, string pPaymentTypeCode)
    {
        DBFilter m_paymentTypeFilter = new DBFilter();
        m_paymentTypeFilter.add(new Match("PaymentTypeCode", pPaymentTypeCode));

        DBFilter m_paymentCodeFilter = new DBFilter();
        m_paymentCodeFilter.add(new IN("PaymentTypeID", "SELECT PaymentTypeID FROM PaymentType", m_paymentTypeFilter));

        DBFilter m_recurringPaymentFilter = new DBFilter();

        IN m_term = new IN(pField, "SELECT PaymentCodeID FROM PaymentCode", m_paymentCodeFilter);

        return m_term;
    }

    public static bool DBPatch(DatabaseConnection dbConn, string Content, out string ErrorMessage)
    {
        ErrorMessage = string.Empty;
        try
        {
            //System.Data.IDbConnection conn = perspectivemind.common.DBUtil.getConnection();
            //System.Data.IDbCommand command = conn.CreateCommand();
            //command.CommandType = System.Data.CommandType.Text;
            //command.CommandText = Content;
            //command.CommandTimeout = 300;
            //command.Connection.Open();
            //command.ExecuteNonQuery();
            //command.Connection.Close();
            System.Data.Common.DbCommand command = dbConn.CreateCommand();
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = Content;
            command.CommandTimeout = 600;
            dbConn.ExecuteNonQuery(command);
            return true;
        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
            return false;
        }
    }
    public static bool BackUpDatabase(DatabaseConnection dbConn, string FilePath, out string strLastErrorMessage)
    {
        strLastErrorMessage = string.Empty;
        //if (perspectivemind.common.DBUtil.type is perspectivemind.common.SQLType)
        if (dbConn.dbType == DatabaseConnection.DatabaseType.MSSQL)
        {
            System.Data.SqlClient.SqlConnection conn = (System.Data.SqlClient.SqlConnection)dbConn.Connection; //(System.Data.SqlClient.SqlConnection)perspectivemind.common.DBUtil.getConnection();
            string backupSQL = "BACKUP DATABASE " + conn.Database + "\r\n" +
                "TO DISK='" + FilePath + "'";
            if (DBPatch(dbConn, backupSQL, out strLastErrorMessage))
            {
                return true;
            }
        }
        return false;
    }

    //public static DataTable getSelectQueryResult(string SelectQuery)
    //{
    //    DataTable table = new DataTable();
    //        System.Data.IDbCommand command = DBUtil.getConnection().CreateCommand();
    //    command.CommandType=CommandType.Text;
    //    command.CommandText=SelectQuery;

    //    System.Data.Common.DbDataAdapter da = DBUtil.createAdapter(command);
    //    da.Fill(table);

    //    return table;
    //}
    public static bool repairHierarchyInformation(DatabaseConnection dbConn)
    {
        //DBPatch("Delete From HierarchyElement he "
        //    + "where Not Exists "
        //    + "( Select * from HierarchyLevel hl "
        //    + "	where hl.HLevelID= he.HLevelID ) ");
        string ErrorMessage = string.Empty;
        return DBPatch(dbConn, "Insert into EmpHierarchy (EmpID,EmpPosID,HElementID,HLevelID) "
            + "Select EmpID, EmpPosID,0,hl.HLevelID "
            + "from EmpPositionInfo ep, HierarchyLevel hl "
            + "where Not Exists "
            + "( Select * from EmpHierarchy eh "
            + "	where eh.EmpPosID= ep.EmpPosID "
            + "	and eh.HLevelID=hl.HLevelID) "
            + "order by ep.empposid, hl.hlevelid", out ErrorMessage);

    }

    public static DateTime ServerDateTime()
    {
        return DateTime.Now;
    }

    //public static DBTerm AddRankDBTerm(int UserID, string EmpID, bool isList)
    //{
    //    if (UserID > 0)
    //    {
    //        OR or = new OR();

    //        DBFilter f = new DBFilter();
    //        f.add(new Match("UserID", UserID));
    //        f.add(new MatchField("TempPI.EmpID", "TempEPI.EmpID"));
    //        or.add(new Exists("EmpPositionInfo TempPI LEFT JOIN UserRank TempUserRank ON TempPI.RankID=TempUserRank.RankID AND TempPI.EmpPosEffTo IS NULL ", f));

    //        f = new DBFilter();

    //        f.add(new MatchField("TempPI.EmpID", "TempEPI.EmpID"));
    //        f.add(new MatchField("TempPI.RankID", "TempRank.RankID"));
    //        f.add(new NullTerm("TempPI.EmpPosEffTo"));
    //        or.add(new Exists("EmpPositionInfo TempPI, RANK TempRank", f, true));

    //        f = new DBFilter();
    //        f.add(or);
    //        IN inTerms = new IN(EmpID, "Select TempEPI.EmpID from EmpPersonalInfo TempEPI", f);
    //        return inTerms;
    //    }
    //    else
    //    {
    //        IN inTerms = new IN(EmpID, "Select TempEPI.EmpID from EmpPersonalInfo TempEPI", new DBFilter());
    //        return inTerms;
    //    }
    //}

    public static DBTerm AddRankDBTerm(int UserID, string ParentEmpIDFieldName, bool isList)
    {
        if (UserID > 0)
        {

#region old style filter, using NOT exists position with (NOT exists RANK or NOT exists company) on non-NULL terms
            //OR orUserFilter = new OR();
            //DBFilter userRankFilter = new DBFilter();
            //userRankFilter.add(new Match("UserRank.UserID",UserID));
            //userRankFilter.add(new MatchField("UserRank.RankID","Rank.RankID"));
            //DBFilter rankFilter= new DBFilter ();
            //rankFilter.add (new Exists("UserRank ",userRankFilter,true));
            //orUserFilter.add(new IN("RankID", "Select RANKID from RANK", rankFilter));

            //DBFilter userCompanyFilter = new DBFilter();
            //userCompanyFilter.add(new Match("UserCompany.UserID", UserID));
            //userCompanyFilter.add(new MatchField("UserCompany.CompanyID ", "Company.CompanyID"));
            //DBFilter companyFilter = new DBFilter();
            //companyFilter.add(new Exists("UserCompany ", userCompanyFilter, true));
            //orUserFilter.add(new IN("CompanyID", "Select CompanyID from Company", companyFilter));


            //DBFilter empPosFilter = new DBFilter();

            //empPosFilter.add(new MatchField("TempPI.EmpID", "TempEPI.EmpID"));
            //empPosFilter.add(new NullTerm("TempPI.EmpPosEffTo"));
            //empPosFilter.add(orUserFilter);

            //Exists notEmpPositionInfoExists = new Exists("EmpPositionInfo TempPI", empPosFilter, true);

            //DBFilter empIDFilter = new DBFilter();
            //empIDFilter.add(empPositionInfoExists);
            //IN inTerms = new IN(ParentEmpIDFieldName, "Select TempEPI.EmpID from " + EEmpPersonalInfo.db.dbclass.tableName + " TempEPI", empIDFilter);

#endregion

#region new style filter, using exists position with (Not RANK or NOT company)
            AND andUserTerm = new AND();
            DBFilter userRankFilter = new DBFilter();
            userRankFilter.add(new Match("tmpUserRank.UserID", UserID));
            userRankFilter.add(new MatchField("tmpUserRank.RankID", "tmpRank_1.RankID"));
            DBFilter rankFilter = new DBFilter();
            rankFilter.add(new Exists(EUserRank.db.dbclass.tableName + " tmpUserRank", userRankFilter));

            //  Use OR term to resolve rank not exists in Rank table
            OR orRankTerm = new OR();
            orRankTerm.add(new IN("TempPI.RankID", "Select RankID from " + ERank.db.dbclass.tableName + " tmpRank_1", rankFilter));
            orRankTerm.add(new IN("NOT TempPI.RankID", "Select RankID from " + ERank.db.dbclass.tableName + " tmpRank_2", null));
            andUserTerm.add(orRankTerm);

            DBFilter userCompanyFilter = new DBFilter();
            userCompanyFilter.add(new Match("tmpUserCompany.UserID", UserID));
            userCompanyFilter.add(new MatchField("tmpUserCompany.CompanyID ", "tmpCompany_1.CompanyID"));
            DBFilter companyFilter = new DBFilter();
            companyFilter.add(new Exists(EUserCompany.db.dbclass.tableName + " tmpUserCompany", userCompanyFilter));

            //  Use OR term to resolve rank not exists in Rank table
            OR orCompanyTerm = new OR();
            orCompanyTerm.add(new IN("TempPI.CompanyID", "Select CompanyID from " + ECompany.db.dbclass.tableName + " tmpCompany_1", companyFilter));
            orCompanyTerm.add(new IN("NOT TempPI.CompanyID", "Select CompanyID from " + ECompany.db.dbclass.tableName + " tmpCompany_2", null));
            andUserTerm.add(orCompanyTerm);

            DBFilter empPosLastTermFilter = new DBFilter();
            OR orEmpPosLastTermEffFrTerms = new OR();
            orEmpPosLastTermEffFrTerms.add(new Match("TempLastPosInfo.EmpPosEffFr", "<=", AppUtils.ServerDateTime()));
            orEmpPosLastTermEffFrTerms.add(new NullTerm("TempLastPosInfo.EmpPosEffFr"));
            empPosLastTermFilter.add(orEmpPosLastTermEffFrTerms);
            empPosLastTermFilter.addGroupBy("TempLastPosInfo.EmpID having TempPI.EmpPosEffFr>=MAX(TempLastPosInfo.EmpPosEffFr) and TempLastPosInfo.EmpID=TempPI.EmpID");

            DBFilter empCurrentAndFuturePosFilter = new DBFilter();
            OR orCurrentAndFuturePosEffToTerms = new OR();
            orCurrentAndFuturePosEffToTerms.add(new Match("TempCurrentAndFuturePosInfo.EmpPosEffTo", ">=", AppUtils.ServerDateTime()));
            orCurrentAndFuturePosEffToTerms.add(new NullTerm("TempCurrentAndFuturePosInfo.EmpPosEffTo"));
            empCurrentAndFuturePosFilter.add(orCurrentAndFuturePosEffToTerms);
            empCurrentAndFuturePosFilter.addGroupBy("TempCurrentAndFuturePosInfo.EmpID having TempPI.EmpPosEffFr>=MIN(TempCurrentAndFuturePosInfo.EmpPosEffFr) and TempCurrentAndFuturePosInfo.EmpID=TempPI.EmpID");

            OR orPosInfoTerm = new OR();
            orPosInfoTerm.add(new Exists(EEmpPositionInfo.db.dbclass.tableName + " TempLastPosInfo", empPosLastTermFilter));
            orPosInfoTerm.add(new Exists(EEmpPositionInfo.db.dbclass.tableName + " TempCurrentAndFuturePosInfo", empCurrentAndFuturePosFilter));


            DBFilter empPosFilter = new DBFilter();
            empPosFilter.add(orPosInfoTerm);
            empPosFilter.add(new MatchField("TempPI.EmpID", "TempEPI.EmpID"));
            empPosFilter.add(andUserTerm);

            Exists empPositionInfoExists = new Exists(EEmpPositionInfo.db.dbclass.tableName + " TempPI", empPosFilter);

            DBFilter empPosNotExistsFilter = new DBFilter();
            empPosNotExistsFilter.add(new MatchField("TempNotExistsPI.EmpID", "TempEPI.EmpID"));

            DBFilter empIDFilter = new DBFilter();
            OR orEmpIDTerms = new OR();
            orEmpIDTerms.add(empPositionInfoExists);
            orEmpIDTerms.add(new Exists(EEmpPositionInfo.db.dbclass.tableName + " TempNotExistsPI", empPosNotExistsFilter, true));
            empIDFilter.add(orEmpIDTerms);
            IN inTerms = new IN(ParentEmpIDFieldName, "Select TempEPI.EmpID from " + EEmpPersonalInfo.db.dbclass.tableName + " TempEPI", empIDFilter);
#endregion

            return inTerms;
        }
        else if (UserID == 0)
        {
            //  Systen user for import console
            IN inTerms = new IN(ParentEmpIDFieldName, "Select TempEPI.EmpID from EmpPersonalInfo TempEPI", new DBFilter());
            return inTerms;
        }
        else
        {
            IN inTerms = new IN(ParentEmpIDFieldName, "0", new DBFilter());
            return inTerms;
        }
    }

    public static bool checkDuplicate(DatabaseConnection dbConn, DBManager db, DBObject c, PageErrors errors, string field)
    {
        return checkDuplicate(dbConn, db, c, errors, field, null);
    }
    public static bool checkDuplicate(DatabaseConnection dbConn, DBManager db, DBObject c, PageErrors errors, string field, DBTerm otherCheckingTerms)
    {
        DBFilter filter = new DBFilter();

        foreach (DBField f in db.keys)
            filter.add(new Match(f.columnName, "<>", f.getValue(c)));
        if (otherCheckingTerms != null)
            filter.add(otherCheckingTerms);
        //DBField cf = db.getField(field);
        //OR orEncryptedTest = new OR();
        //orEncryptedTest.add(new Match(cf.columnName, cf.getValue(c)));
        //DBFieldTranscoder transcoder = cf.transcoder;
        //if (transcoder != null)
        //    orEncryptedTest.add(new Match(cf.columnName, transcoder.toDB(cf.getValue(c))));
        //filter.add(orEncryptedTest);


        //if (db.count(dbConn, filter) > 0)
        //{
        //    errors.addError(field, field + " Duplicated");
        //    return false;
        //}

        ArrayList list = db.select(dbConn, filter);
        foreach (DBObject obj in list)
        {
            DBField f = db.getField(field);
            if (f.getValue(c) is string)
            {
                if (((string)f.getValue(c)).Equals((string)f.getValue(obj), StringComparison.CurrentCultureIgnoreCase))
                {
                    errors.addError(field, string.Format(HROne.Common.WebUtility.GetLocalizedString("ERROR_CODE_DUPLICATE"), new string[] { HROne.Common.WebUtility.GetLocalizedString(GetActualFieldName(field)), f.getValue(c).ToString() }));
                    return false;
                }
            }
            else if (f.getValue(c).Equals(f.getValue(obj)))
            {
                errors.addError(field, string.Format(HROne.Common.WebUtility.GetLocalizedString("ERROR_CODE_DUPLICATE"), new string[] { HROne.Common.WebUtility.GetLocalizedString(GetActualFieldName(field)), f.getValue(c).ToString() }));
                return false;
            }

        }
        return true;
    }

    public static bool checkDuplicate(DatabaseConnection dbConn, int EmpID, DBManager db, DBObject c, PageErrors errors, string field)
    {
        ////  Skip Case-insensitive checking since integer type is checked only
        //DBFilter filter = new DBFilter();
        //filter.add(new Match("EmpID", EmpID));
        //foreach (DBField f in db.keys)
        //{
        //    filter.add(new Match(f.columnName, "<>", f.getValue(c)));
        //}
        //DBField cf = db.getField(field);
        //OR orEncryptedTest = new OR();
        //orEncryptedTest.add(new Match(cf.columnName, cf.getValue(c)));
        //DBFieldTranscoder transcoder = cf.transcoder;
        //if (transcoder != null)
        //    orEncryptedTest.add(new Match(cf.columnName, transcoder.toDB(cf.getValue(c))));
        //filter.add(orEncryptedTest);
        //if (db.count(dbConn, filter) > 0)
        //{
        //    errors.addError(field, string.Format(HROne.Common.WebUtility.GetLocalizedString("ERROR_CODE_DUPLICATE"), new string[] { HROne.Common.WebUtility.GetLocalizedString(GetActualFieldName(field)), cf.getValue(c).ToString() }));
        //    return false;
        //}
        //return true;
        return checkDuplicate(dbConn, db, c, errors, field, new Match("EmpID", EmpID));
    }
    public static DBObject GetLastObj(DatabaseConnection dbConn, DBManager db, string FromField, int EmpID)
    {
        return GetLastObj(dbConn, db, FromField, EmpID, null);
    }
    public static DBObject GetLastObj(DatabaseConnection dbConn, DBManager db, string FromField, int EmpID, DBTerm t)
    {
        DBFilter filter = new DBFilter();
        filter.add(FromField,false);
        filter.add(new Match("EmpID", EmpID));
        if(t!=null)
            filter.add(t);
        ArrayList list = db.select(dbConn, filter);
        if (list.Count > 0)
            return (DBObject)list[0];
        else
            return null;
    }
    public static EEmpPositionInfo GetLastPositionInfo(DatabaseConnection dbConn, int CurID)
    {
        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpID", CurID));
//        filter.add(new NullTerm("EmpPosEffTo"));
        filter.add("EmpPosEffFr", false);
        ArrayList list = EEmpPositionInfo.db.select(dbConn, filter);
        if (list.Count == 0)
            return null;
        return (EEmpPositionInfo)list[0];
    }
    public static EEmpPositionInfo GetLastPositionInfo(DatabaseConnection dbConn, DateTime date, int CurID)
    {
        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpID", CurID));
        filter.add(new Match("EmpPosEffFr","<=", date));
        //OR orPosEffTerms = new OR();
        //orPosEffTerms.add(new Match("EmpPosEffTo", ">=", date));
        //orPosEffTerms.add(new NullTerm("EmpPosEffTo"));
        //filter.add(orPosEffTerms);
        //  20090107 added by Jimmy
        //  Get Last terms
        filter.add("EmpPosEffFr", false);
        ArrayList list = EEmpPositionInfo.db.select(dbConn, filter);
        if (list.Count == 0)
            return GetLastPositionInfo(dbConn, CurID);
        return (EEmpPositionInfo)list[0];
    }

    [Obsolete("Implemented EmpPersonalInfo.EmpProbaLastDate")]
    public static DateTime GetLastProbationDate(DatabaseConnection dbConn, int EmpID)
    {
        EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
        empInfo.EmpID = EmpID;
        if (EEmpPersonalInfo.db.select(dbConn, empInfo))
        {
            DateTime lastProbationDate = empInfo.EmpProbaLastDate;
            if (lastProbationDate.Ticks.Equals(0))
            {
                lastProbationDate = empInfo.EmpDateOfJoin;
                if (empInfo.EmpProbaPeriod > 0)
                    if (empInfo.EmpProbaUnit.Equals("M"))
                        lastProbationDate = lastProbationDate.AddMonths(empInfo.EmpProbaPeriod);
                    else if (empInfo.EmpProbaUnit.Equals("D"))
                        lastProbationDate = lastProbationDate.AddDays(empInfo.EmpProbaPeriod);

                lastProbationDate = lastProbationDate.AddDays(-1);
            }
            return lastProbationDate;
        }
        else
            return new DateTime();
    }

    public static double GetAge(object DateOfBirth)
    {
        return GetAge(DateOfBirth, ServerDateTime());
    }

    public static double GetAge(object DateOfBirth, DateTime AsOfDate)
    {
        if (DateOfBirth is DateTime)
            //  Use inclusive calculation to calculate difference
            return HROne.CommonLib.Utility.YearDifference((DateTime)DateOfBirth, AsOfDate.AddDays(-1));
        else
            return double.NaN;
    }

    public class WFDBDistinctList : WFDBList2
    {
        public WFDBDistinctList(DBManager db, string keyField, string nameField, string sortField)
            : base(db, new string[] { keyField, nameField }, sortField)
        {
            distinct = true;
        }
        public override string GetKey(IDataReader reader)
        {
            return reader[0].ToString(); 
        }
        public override string  GetText(IDataReader reader)
        {
            return reader[1].ToString();
        }
    }

    public class NewWFTextList : WFValueList
    {
        string[] keyList;
        string[] valueList;
        public NewWFTextList(string[] keyList)
            : this(keyList, keyList)
        {

        }
        public NewWFTextList(string[] keyList, string[] valueList)
        {
            this.keyList = keyList;
            this.valueList = valueList;
        }

        public List<WFSelectValue> getValues(DatabaseConnection DBAccess, DBFilter filter, System.Globalization.CultureInfo ci)
        {
            List<WFSelectValue> resultList = new List<WFSelectValue>();

            for (int i = 0; i < keyList.Length; i++)
            {
                string key = keyList[i];
                string value = string.Empty;
                if (i < valueList.Length)
                    value = valueList[i];

                value = HROne.Common.WebUtility.GetLocalizedString(value);
                WFSelectValue vl = new WFSelectValue(key, value);
                resultList.Add(vl);
            }
            return resultList;
        }
    }

    public class DistinctEncryptedDBCodeList : WFValueList
    {
        // modified from EncryptedDBCodeList
        DBManager db;
        string id;
        string[] fieldList;
        string sort;
        string separator;
        public DistinctEncryptedDBCodeList(DBManager db, string distinctField)
            : base()
        {
            this.db = db;
            this.id = distinctField;
//            this.id = id;
            this.fieldList = new string[] {distinctField};
            this.separator = "";
//            this.sort = sort;
        }
        //public override string GetText(IDataReader reader)
        //{
        //    return db.getField(code).transcoder.fromDB(reader.GetValue(reader.GetOrdinal(code))).ToString()
        //    + " - " + db.getField(desc).transcoder.fromDB(reader.GetValue(reader.GetOrdinal(desc))).ToString();
        //}
        public List<WFSelectValue> getValues(DatabaseConnection DBAccess, DBFilter filter, System.Globalization.CultureInfo ci)
        {
            List<WFSelectValue> resultList = new List<WFSelectValue>();
            if (filter == null)
                filter = new DBFilter();
            DataTable table = filter.loadData(DBAccess, "Select DISTINCT " + id + " , " + string.Join(", ", fieldList) + " " + " from " + db.dbclass.tableName);
            foreach (DataRow row in table.Rows)
            {
                DBFieldTranscoder idTranscoder = db.getField(id).transcoder;
                if (idTranscoder != null)
                    row[id] = idTranscoder.fromDB(row[id]);
                
                foreach (string field in fieldList)
                {
                    DBFieldTranscoder transcoder = db.getField(field).transcoder;
                    if (transcoder != null)
                        row[field] = transcoder.fromDB(row[field]);
                }
            }
            DataRow[] resultRows = table.Select("", sort);
            foreach (DataRow row in resultRows)
            {
                string key = row[id].ToString();
                string value = string.Empty;
                foreach (string field in fieldList)
                {
                    if (string.IsNullOrEmpty(value))
                        value = row[field].ToString();
                    else
                        value += separator + row[field].ToString();
                }
                WFSelectValue vl = new WFSelectValue(key, value);

                resultList.Add(vl);
            }
            return resultList;
        }
    }


    public class EncryptedDBCodeList : WFValueList
    {
        DBManager db;
        string id;
        string[] fieldList;
        string sort;
        string separator;
        public EncryptedDBCodeList(DBManager db, string id, string[] fieldList, string separator, string sort)
            : base()
        {
            this.db = db;
            this.id = id;
            this.fieldList = fieldList;
            this.separator = separator;
            this.sort = sort;
        }
        //public override string GetText(IDataReader reader)
        //{
        //    return db.getField(code).transcoder.fromDB(reader.GetValue(reader.GetOrdinal(code))).ToString()
        //    + " - " + db.getField(desc).transcoder.fromDB(reader.GetValue(reader.GetOrdinal(desc))).ToString();
        //}
        public List<WFSelectValue> getValues(DatabaseConnection DBAccess, DBFilter filter, System.Globalization.CultureInfo ci)
        {
            List<WFSelectValue> resultList = new List<WFSelectValue>();
            if (filter == null)
                filter = new DBFilter();
            DataTable table = filter.loadData(DBAccess, "Select " + id + ", " + string.Join(", ", fieldList) + " " + " from " + db.dbclass.tableName);
            foreach (DataRow row in table.Rows)
            {
                DBFieldTranscoder idTranscoder = db.getField(id).transcoder;
                if (idTranscoder != null)
                    row[id] = idTranscoder.fromDB(row[id]);
                foreach (string field in fieldList)
                {
                    DBFieldTranscoder transcoder = db.getField(field).transcoder;
                    if (transcoder != null)
                        row[field] = transcoder.fromDB(row[field]);
                }
            }
            DataRow[] resultRows = table.Select("", sort);
            foreach (DataRow row in resultRows)
            {
                string key = row[id].ToString();
                string value = string.Empty;
                foreach (string field in fieldList)
                {
                    if (string.IsNullOrEmpty(value))
                        value = row[field].ToString();
                    else
                        value += separator + row[field].ToString();
                }
                WFSelectValue vl = new WFSelectValue(key, value);

                resultList.Add(vl);
            }
            return resultList;
        }
    }
    public class BankAccountValueList : WFValueList
    {
        DBManager db;
        string IDField;
        string BankCodeField;
        string BranchCodeField;
        string AccountNoField;
        string RemarkField;
        string orderBy;

        public BankAccountValueList(DBManager db, string IDField, string BankCodeField, string BranchCodeField, string AccountNoField, string orderBy)
            : this(db, IDField, BankCodeField, BranchCodeField, AccountNoField, string.Empty, orderBy)
        { }

        public BankAccountValueList(DBManager db, string IDField, string BankCodeField, string BranchCodeField, string AccountNoField, string RemarkField, string orderBy)
        {
            this.db = db;
            this.IDField = IDField;
            this.BankCodeField = BankCodeField;
            this.BranchCodeField = BranchCodeField;
            this.AccountNoField = AccountNoField;
            this.RemarkField = RemarkField;
            this.orderBy = orderBy;
        }
        public List<WFSelectValue> getValues(DatabaseConnection DBAccess, DBFilter filter, System.Globalization.CultureInfo ci)
        {
            List<WFSelectValue> resultList = new List<WFSelectValue>();
            if (filter == null)
                filter = new DBFilter();
            DataTable table = filter.loadData(DBAccess, "Select " + IDField + ", "
                                                + BankCodeField + ", "
                                                + BranchCodeField + ", "
                                                + AccountNoField
                                                + (string.IsNullOrEmpty(RemarkField.Trim()) ? string.Empty : (", " + RemarkField))
                                                + " from " + db.dbclass.tableName);
            foreach (DataRow row in table.Rows)
            {
                DBFieldTranscoder transcoder = db.getField(IDField).transcoder;
                if (transcoder != null)
                    row[IDField] = transcoder.fromDB(row[IDField]);
                transcoder = db.getField(BankCodeField).transcoder;
                if (transcoder != null)
                    row[BankCodeField] = transcoder.fromDB(row[BankCodeField]);
                transcoder = db.getField(BranchCodeField).transcoder;
                if (transcoder != null)
                    row[BranchCodeField] = transcoder.fromDB(row[BranchCodeField]);
                transcoder = db.getField(AccountNoField).transcoder;
                if (transcoder != null)
                    row[AccountNoField] = transcoder.fromDB(row[AccountNoField]);
                if (!string.IsNullOrEmpty(RemarkField.Trim()))
                {
                    transcoder = db.getField(RemarkField).transcoder;
                    if (transcoder != null)
                        row[RemarkField] = transcoder.fromDB(row[RemarkField]);
                }
            }
            DataRow[] resultRows = table.Select("", orderBy);
            foreach (DataRow row in resultRows)
            {
                string key = row[IDField].ToString();
                string value = row[BankCodeField].ToString() + "-" + row[BranchCodeField].ToString() + "-" + row[AccountNoField].ToString();

                if (!string.IsNullOrEmpty(RemarkField.Trim()))
                    if (!row.IsNull(RemarkField))
                        if (row[RemarkField].ToString().Trim() != string.Empty)
                            value += " (" + row[RemarkField] + ")";

                WFSelectValue vl = new WFSelectValue(key, value);

                resultList.Add(vl);
            }
            return resultList;
        }

    }
    public static bool Sent_Email(DatabaseConnection dbConn, string ToEmail, string FromEmail, string ToName, string FromName, string subject, string body)
    {
        return Sent_Email(dbConn, ToEmail, FromEmail, ToName, FromName, subject, body, string.Empty, string.Empty, false);
    }

    public static bool Sent_Email(DatabaseConnection dbConn, string ToEmail, string FromEmail, string ToName, string FromName, string subject, string body, string attachmentFileName, string ActualAttachmentFileName, bool DeleteAttachmentAfterSent)
    {
        System.Threading.Thread thread = CreateEmailThreading(dbConn, ToEmail, FromEmail, ToName, FromName, subject, body, attachmentFileName, ActualAttachmentFileName, DeleteAttachmentAfterSent);
        return true;
    }
    public static System.Threading.Thread CreateEmailThreading(DatabaseConnection dbConn, string ToEmail, string FromEmail, string ToName, string FromName, string subject, string body, string attachmentFileName, string ActualAttachmentFileName, bool DeleteAttachmentAfterSent)
    {
        HROne.TaskService.EmailTaskFactory emailTask = new HROne.TaskService.EmailTaskFactory(dbConn, ToEmail, FromEmail, ToName, FromName, subject, body, attachmentFileName, ActualAttachmentFileName, DeleteAttachmentAfterSent);
        return emailTaskQueueService.AddTask(emailTask);
        //HROne.Lib.EmailQueueParameter parameter = new HROne.Lib.EmailQueueParameter();
        //parameter.dbConn = dbConn.createClone();
        //parameter.ToEmail = ToEmail;
        //parameter.FromEmail = FromEmail;
        //parameter.ToName = ToName;
        //parameter.FromName = FromName;
        //parameter.subject = subject;
        //parameter.body = body;
        //parameter.attachmentFileName = attachmentFileName;
        //parameter.ActualAttachmentFileName = ActualAttachmentFileName;
        //parameter.DeleteAttachmentAfterSent = DeleteAttachmentAfterSent;

        //return HROne.Lib.EmailQueueService.SendEmail(parameter);


        //System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(EmailProcess));
        //thread.IsBackground = false;

        //EmailProcessParameter parameter = new EmailProcessParameter();
        //parameter.dbConn = dbConn.createClone();
        //parameter.ToEmail = ToEmail;
        //parameter.FromEmail = FromEmail;
        //parameter.ToName = ToName;
        //parameter.FromName = FromName;
        //parameter.subject = subject;
        //parameter.body = body;
        //parameter.attachmentFileName = attachmentFileName;
        //parameter.ActualAttachmentFileName = ActualAttachmentFileName;
        //parameter.DeleteAttachmentAfterSent = DeleteAttachmentAfterSent;
        //thread.Start(parameter);

        //return thread;
    }

    //protected class EmailProcessParameter
    //{
    //    public DatabaseConnection dbConn;
    //    public string ToEmail;
    //    public string FromEmail;
    //    public string ToName;
    //    public string FromName;
    //    public string subject;
    //    public string body;
    //    public string attachmentFileName;
    //    public string ActualAttachmentFileName;
    //    public bool DeleteAttachmentAfterSent;
    //}

    //public static void EmailProcess(object obj)
    //{
    //    EmailProcessParameter parameter = (EmailProcessParameter)obj;
    //    //HROne.DataAccess.DatabaseConnection.SetDefaultDatabaseConnection(parameter.dbConn); // SetDefaultDatabaseConnection will be removed for stable release
    //    HROne.Lib.EmailService emailService = new HROne.Lib.EmailService(parameter.dbConn);
    //    emailService.SendAsync = true;
    //    emailService.SendEmail(parameter.ToEmail, parameter.FromEmail, parameter.ToName, parameter.FromName, parameter.subject, parameter.body, parameter.attachmentFileName, parameter.ActualAttachmentFileName, parameter.DeleteAttachmentAfterSent);
    //}

    public static string DefaultDocumentUploadFolder=string.Empty;
    public static string GetDocumentUploadFolder(DatabaseConnection dbConn)
    {
        if (!string.IsNullOrEmpty(DefaultDocumentUploadFolder))
            return DefaultDocumentUploadFolder;
        string strDocumentPath = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_DOCUMENT_UPLOAD_FOLDER);
        if (string.IsNullOrEmpty(strDocumentPath))
        {
            strDocumentPath = HROne.CommonLib.FileIOProcess.DefaultUploadFolder();
        }
        return strDocumentPath;
    }

    public static string GetActualFieldName(string fieldname)
    {
        Hashtable table = new Hashtable(10);
        table.Add("EMPNO", "Emp No");
        table.Add("SKILLID", "Skill");
        table.Add("QUALIFICATIONID", "Qualification");
        object actualFieldName = table[fieldname.ToUpper()];
        if (actualFieldName!=null)
            return actualFieldName.ToString();
        else
            return fieldname;
    }

    public static bool TestSendEmail(string SMTPServer, int Port, string UserID, string Password, string SenderEmailAddress, string RecipientAddress, bool EnableSSL)
    {
        //SmtpClient smtpClient;
        //if (Port > 0)
        //    smtpClient = new SmtpClient(SMTPServer, Port);
        //else
        //    smtpClient = new SmtpClient(SMTPServer);
        //smtpClient.UseDefaultCredentials = false;
        ///* Email with Authentication */
        //smtpClient.Credentials = new System.Net.NetworkCredential(UserID,
        //                 Password);
        ////------------------------------------------------------
        ////Email function
        //MailMessage message = new MailMessage();
        //if ((SenderEmailAddress != null) && (RecipientAddress != null))
        //{
        //    message.From = new MailAddress(SenderEmailAddress);
        //    message.To.Add(RecipientAddress);
        //    message.Subject = "Test Email From HROne";
        //    message.Body = "This is a test";
        //    message.IsBodyHtml = false;

        //    try
        //    {
        //        smtpClient.Send(message);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //        //return false;
        //    }

        //}

        //return true;
        //------------------------------------------------------
        //return Sent_Email(SMTPServer, Port, UserID, Password, RecipientAddress, SenderEmailAddress, string.Empty, string.Empty, "Test Email From HROne", "This is a test", string.Empty);

        HROne.Lib.EmailService emailService = new HROne.Lib.EmailService(SMTPServer, Port, UserID, Password, EnableSSL);
        emailService.DefaultFromEmailAccount = SenderEmailAddress;
        emailService.SendAsync = false;
        emailService.SendEmail(RecipientAddress, SenderEmailAddress, string.Empty, string.Empty, "Test Email From HROne", "This is a test");

        return true;
    }

    public static DateTime getDateFromCode(string code, string key)
    {
        HROne.CommonLib.Crypto crypto = new HROne.CommonLib.Crypto(HROne.CommonLib.Crypto.SymmProvEnum.DES);
        try
        {
            code = base32.ConvertBase32ToBase64(code);

            string realTrialKey = crypto.Decrypting(code, key);
            string strYear = realTrialKey.Substring(0, 4);
            string strMonth = realTrialKey.Substring(4, 2);
            string strDay = realTrialKey.Substring(6, 2);

            return new DateTime(int.Parse(strYear), int.Parse(strMonth), int.Parse(strDay));

        }
        catch
        {
            return new DateTime();
        }
    }
    public static System.Drawing.Color ComputeTextColor(System.Drawing.Color bgColor)
    {
        double brt = (bgColor.R / 255 * 0.30) + (bgColor.G / 255 * 0.59) + (bgColor.B / 255 * 0.11);
        double m;
        double im;
        if (brt < 0.5)
        {
            m = Math.Floor(brt * 20) + 3;
            im = 255 * (m - 1);
        }
        else
        {
            m = Math.Floor((1.0 - brt) * 20) + 3;
            im = 0;
        }
        return System.Drawing.Color.FromArgb(Convert.ToInt32(Math.Floor((bgColor.R + im) / m)), Convert.ToInt32(Math.Floor((bgColor.G + im) / m)), Convert.ToInt32(Math.Floor((bgColor.B + im) / m)));
    }

    public static System.Drawing.FontFamily GetChineseFontFamily(DatabaseConnection dbConn)
    {
        System.Drawing.FontFamily chineseFontFamily = null;

        // Start 0000204, Ricky So, 2015-05-20
        string _defaultFont = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_REPORT_CHINESE_FONT);
        try
        {
            chineseFontFamily = new System.Drawing.FontFamily(_defaultFont);
        }
        catch 
        {
            string[] fontList = new string[]
            {
                "MingLiU_HKSCS", "細明體_HKSCS",
                "PMingLiU", "新細明體",
                "MingLiU", "細明體"            
            };

            foreach (string fontName in fontList)
            {
                try
                {
                    chineseFontFamily = new System.Drawing.FontFamily(fontName);
                }
                catch { continue; }
                break;
            }
        }
        
        return chineseFontFamily;

        //try
        //{
        //    chineseFontFamily = new System.Drawing.FontFamily(ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_REPORT_CHINESE_FONT));
        //}
        //catch { }
        //if (chineseFontFamily != null)
        //    return chineseFontFamily;

        //string[] fontList = new string[]{
        //    "MingLiU_HKSCS", "細明體_HKSCS",
        //    "PMingLiU", "新細明體",
        //    "MingLiU", "細明體",
        //    "Arial"
        //};

        //foreach (string fontName in fontList)
        //{
        //    try
        //    {
        //        chineseFontFamily = new System.Drawing.FontFamily(fontName);
        //    }
        //    catch { }
        //    if (chineseFontFamily != null)
        //        break;
        //}
        //return chineseFontFamily;

        // End 0000204, Ricky So, 2015-05-20
    }

    public static int ApplyRoundingRule(int OriginalValue, string RoundingRule, int Unit)
    {
        if (!string.IsNullOrEmpty(RoundingRule))
            if (RoundingRule.Equals(Values.ROUNDING_RULE_ROUND_UP))
                return IntegerRoundingFunctions.RoundingUp(OriginalValue, Unit);
            else if (RoundingRule.Equals(Values.ROUNDING_RULE_ROUND_TO))
                return IntegerRoundingFunctions.RoundingTo(OriginalValue, Unit);
            else if (RoundingRule.Equals(Values.ROUNDING_RULE_ROUND_DOWN))
                return IntegerRoundingFunctions.RoundingDown(OriginalValue, Unit);
            else
                return OriginalValue;
        else
            return OriginalValue;
    }

    public static EAuditTrail StartFunction(DatabaseConnection dbConn, EUser user, string FunctionCode, int EmpID, bool LogDetail)
    {
        if (user!=null)
            return StartFunction(dbConn, user.UserID, FunctionCode, EmpID, LogDetail);
        else
            return StartFunction(dbConn, 0, FunctionCode, EmpID, LogDetail);
    }
    public static EAuditTrail StartFunction(DatabaseConnection dbConn, int UserID, string FunctionCode, int EmpID, bool LogDetail)
    {
        return HROne.Common.AuditTrail.StartFunction(dbConn, UserID, FunctionCode, EmpID, LogDetail);
    }
    public static EAuditTrail StartChildFunction(DatabaseConnection dbConn, int EmpID)
    {
        return HROne.Common.AuditTrail.StartChildFunction(dbConn, EmpID);
    }
    public static EAuditTrail StartChildFunction(DatabaseConnection dbConn, string FunctionCode, int EmpID)
    {
        return HROne.Common.AuditTrail.StartChildFunction(dbConn, FunctionCode, EmpID);
    }
    public static void EndChildFunction(DatabaseConnection dbConn)
    {
        //EmailAuditTrail();
        HROne.Common.AuditTrail.EndChildFunction(dbConn);
    }
    //public static void EndFunction(dbConn)
    //{
    //    DatabaseConnection dbConn = DatabaseConnection.GetDatabaseConnection();
    //    EmailAuditTrail(dbConn);
    //    HROne.Common.AuditTrail.EndFunction(dbConn);
    //}
    public static void EndFunction(DatabaseConnection dbConn)
    {
        EmailAuditTrail(dbConn);
        HROne.Common.AuditTrail.EndFunction(dbConn);
    }

    //  Must be trigger before WebUtils.EndFunction(dbConn)
    public static void EmailAuditTrail(DatabaseConnection dbConn)
    {
        if (dbConn is DatabaseConnectionWithAudit)
        {
            EAuditTrail auditTrail = ((DatabaseConnectionWithAudit)dbConn).CurAuditTrail;
            if (auditTrail != null)
                if (auditTrail.AuditTrailID > 0)
                {
                    string emailAddressList = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_EMAIL_AUDIT_TRAIL_ADDRESS);
                    if (!string.IsNullOrEmpty(emailAddressList.Trim()))
                    {
                        ESystemFunction function = new ESystemFunction();
                        function.FunctionID = auditTrail.FunctionID;
                        if (ESystemFunction.db.select(dbConn, function))
                        {
                            string EmailContent = GenerateAuditTrailEmailContent(dbConn, auditTrail);
                            if (!string.IsNullOrEmpty(EmailContent))
                            {
                                AppUtils.Sent_Email(dbConn, emailAddressList, string.Empty, string.Empty, string.Empty, function.FunctionCode + "-" + function.Description + " has been updated", EmailContent, string.Empty, string.Empty, false);
                                //thread.Join();
                            }
                        }
                    }
                }
        }
    }
    public static string GenerateAuditTrailEmailContent(DatabaseConnection dbConn, EAuditTrail auditTrail)
    {
        System.Text.StringBuilder auditTrailTextDetailBuilder = new System.Text.StringBuilder();
        DBFilter childAuditTrailFilter = new DBFilter();
        childAuditTrailFilter.add(new Match("ParentAuditTrailID",auditTrail.AuditTrailID));
        ArrayList childAuditTrailList = EAuditTrail.db.select(dbConn, childAuditTrailFilter);
        foreach (EAuditTrail childAuditTrail in childAuditTrailList)
        {
            string childAuditTrailContent = GenerateAuditTrailEmailContent(dbConn, childAuditTrail);
            if (!string.IsNullOrEmpty(childAuditTrailContent))
                auditTrailTextDetailBuilder.AppendLine(childAuditTrailContent);
        }

        DBFilter systemEmailAlertFilter = new DBFilter();
        systemEmailAlertFilter.add(new Match("FunctionID", auditTrail.FunctionID));
        ArrayList systemAlertList = ESystemFunctionEmailAlert.db.select(dbConn, systemEmailAlertFilter);
        if (systemAlertList.Count > 0)
        {
            ESystemFunction function = new ESystemFunction();
            function.FunctionID = auditTrail.FunctionID;
            if (ESystemFunction.db.select(dbConn, function))
            {
                bool sendEmail = false;

                ////  for future adding different filter method for different Function Code
                if (function.FunctionCode.Equals("PER999"))
                {
                    //  testing for Import action
                    //  Insert - EmpPersonalInfo only
                    //  Update - Update of EmpPersonalInfo
                    //           Update of EmpPositionInfo
                    //           Update of EmpRecurringPayment
                    //           Insert of EmpBankAccount
                    //           Update of EmpMPFPlan
                    //           Update of EmpAVCPlan
                    //           Update of EmpORSOPlan
                    //           Update of EmpCostCenter
                    //  Delete - N/A

                    DBFilter auditTrailDetailFilter = new DBFilter();
                    auditTrailDetailFilter.add(new Match("AuditTrailID", auditTrail.AuditTrailID));
                    bool triggerEmailAlert = false;
                    OR orSearchAction = new OR();
                    foreach (ESystemFunctionEmailAlert systemAlert in systemAlertList)
                    {
                        if (systemAlert.SystemFunctionEmailAlertInsert)
                        {

                            AND andInsertAction = new AND();
                            andInsertAction.add(new Match("TableName", EEmpPersonalInfo.db.dbclass.tableName));
                            andInsertAction.add(new Match("ActionType", EAuditTrailDetail.ACTIONTYPE_INSERT));
                            orSearchAction.add(andInsertAction);
                            triggerEmailAlert = true;
                            break;
                        }
                    }
                    foreach (ESystemFunctionEmailAlert systemAlert in systemAlertList)
                    {
                        if (systemAlert.SystemFunctionEmailAlertUpdate)
                        {
                            OR orTableName = new OR();

                            orTableName.add(new Match("TableName", EEmpPositionInfo.db.dbclass.tableName));
                            orTableName.add(new Match("TableName", EEmpRecurringPayment.db.dbclass.tableName));
                            orTableName.add(new Match("TableName", EEmpBankAccount.db.dbclass.tableName));
                            orTableName.add(new Match("TableName", EEmpMPFPlan.db.dbclass.tableName));
                            orTableName.add(new Match("TableName", EEmpAVCPlan.db.dbclass.tableName));
                            orTableName.add(new Match("TableName", EEmpORSOPlan.db.dbclass.tableName));
                            orTableName.add(new Match("TableName", EEmpCostCenter.db.dbclass.tableName));

                            DBFilter notExistsData = new DBFilter();
                            notExistsData.add(new Match("detail2.TableName", EEmpPersonalInfo.db.dbclass.tableName));
                            notExistsData.add(new Match("detail2.ActionType", EAuditTrailDetail.ACTIONTYPE_INSERT));
                            notExistsData.add(new MatchField("detail2.AuditTrailID", EAuditTrailDetail.db.dbclass.tableName + ".AuditTrailID"));

                            AND andInsertAction = new AND();
                            andInsertAction.add(orTableName);
                            andInsertAction.add(new Exists(EAuditTrailDetail.db.dbclass.tableName + " detail2", notExistsData, true));
                            orSearchAction.add(andInsertAction);
                            triggerEmailAlert = true;
                            break;

                        }
                    }
                    if (triggerEmailAlert)
                    {
                        auditTrailDetailFilter.add(orSearchAction);
                        ArrayList auditTrailDetailList = EAuditTrailDetail.db.select(dbConn, auditTrailDetailFilter);
                        if (auditTrailDetailList.Count > 0)
                        {
                            auditTrailTextDetailBuilder.Append(auditTrail.GetLogText(dbConn, string.Empty, true, false, false));
                            foreach (EAuditTrailDetail detail in auditTrailDetailList)
                                auditTrailTextDetailBuilder.Append(detail.GetLogText(dbConn, true, false));
                        }
                    }

                }
                else
                {

                    DBFilter auditTrailDetailFilter = new DBFilter();
                    auditTrailDetailFilter.add(new Match("AuditTrailID", auditTrail.AuditTrailID));

                    if (function.FunctionCode.Equals("MPF003", StringComparison.CurrentCultureIgnoreCase))
                    {
                        //  do NOT consider AVCPlanPaymentCeiling and AVCPlanPaymentConsider, which should be determined by AVCPlan                            
                        OR orTableNameFilter = new OR();
                        orTableNameFilter.add(new Match("TableName", EAVCPlan.db.dbclass.tableName));
                        orTableNameFilter.add(new Match("TableName", EAVCPlanDetail.db.dbclass.tableName));
                        auditTrailDetailFilter.add(orTableNameFilter);
                    }
                    else if (function.FunctionCode.Equals("PER001", StringComparison.CurrentCultureIgnoreCase))
                    {
                        //  do NOT consider EmpExtraFieldValue, which should be determined by EmpPersonalInfo                            
                        auditTrailDetailFilter.add(new Match("TableName", EEmpPersonalInfo.db.dbclass.tableName));
                    }
                    else if (function.FunctionCode.Equals("PER007", StringComparison.CurrentCultureIgnoreCase))
                    {
                        //  do NOT consider EmpHierarchy, which should be determined by EmpPositionInfo                            
                        auditTrailDetailFilter.add(new Match("TableName", EEmpPositionInfo.db.dbclass.tableName));
                    }
                    else if (function.FunctionCode.Equals("PER012", StringComparison.CurrentCultureIgnoreCase))
                    {
                        //  do NOT consider EmpCostCenterDetail, which should be determined by EmpCostCenter                            
                        auditTrailDetailFilter.add(new Match("TableName", EEmpCostCenter.db.dbclass.tableName));
                    }
                    else if (function.FunctionCode.Equals("SEC001", StringComparison.CurrentCultureIgnoreCase))
                        auditTrailDetailFilter.add(new Match("TableName", EUser.db.dbclass.tableName));
                    else if (function.FunctionCode.Equals("SEC002", StringComparison.CurrentCultureIgnoreCase))
                        auditTrailDetailFilter.add(new Match("TableName", EUserGroup.db.dbclass.tableName));
                    else if (function.FunctionCode.Equals("SYS004", StringComparison.CurrentCultureIgnoreCase))
                    {
                        //  do NOT consider TaxPaymentMap, which should be determined by PaymentCode                            
                        auditTrailDetailFilter.add(new Match("TableName", EPaymentCode.db.dbclass.tableName));
                    }

                    OR orAuditTrailDetailActionTerm = new OR();
                    foreach (ESystemFunctionEmailAlert systemAlert in systemAlertList)
                    {
                        if (systemAlert.SystemFunctionEmailAlertInsert)
                            orAuditTrailDetailActionTerm.add(new Match("ActionType", EAuditTrailDetail.ACTIONTYPE_INSERT));
                        if (systemAlert.SystemFunctionEmailAlertUpdate)
                            orAuditTrailDetailActionTerm.add(new Match("ActionType", EAuditTrailDetail.ACTIONTYPE_UPDATE));
                        if (systemAlert.SystemFunctionEmailAlertDelete)
                        {
                            orAuditTrailDetailActionTerm.add(new Match("ActionType", EAuditTrailDetail.ACTIONTYPE_DELETE));
                            orAuditTrailDetailActionTerm.add(new Match("ActionType", EAuditTrailDetail.ACTIONTYPE_MARKDELETE));
                        }
                    }
                    auditTrailDetailFilter.add(orAuditTrailDetailActionTerm);
                    if (EAuditTrailDetail.db.count(dbConn, auditTrailDetailFilter) > 0)
                        sendEmail = true;
                }

                if (sendEmail)
                {
                    auditTrailTextDetailBuilder.Append(auditTrail.GetLogText(dbConn, string.Empty, false, true, false));
                }
            }
        }
        return auditTrailTextDetailBuilder.ToString();
    }

    public static string GetActualInboxSubject(DatabaseConnection dbConn, EInbox inbox)
    {
        if (inbox.EmpID > 0)
        {
            EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
            empInfo.EmpID = inbox.EmpID;
            EEmpPersonalInfo.db.select(dbConn, empInfo);
            string code = "INBOX_SUBJECT_" + inbox.InboxType;
            return string.Format(HROne.Common.WebUtility.GetLocalizedStringByCode(code, code), new string[] { empInfo.EmpNo, empInfo.EmpEngFullName, inbox.InboxRelatedReferenceDate.ToString("yyyy-MM-dd") });
        }
        else if (inbox.InboxType.StartsWith("FUNCTION_",StringComparison.CurrentCultureIgnoreCase))
        {
            return HROne.Common.WebUtility.GetLocalizedStringByCode(inbox.InboxType, inbox.InboxType);
        }
        else
            return inbox.InboxSubject;
    }


    public static DataTable runSelectSQL(string select, string from, DBFilter filter, DatabaseConnection dbConn)
    {

        if (filter == null)
        {
            filter = new DBFilter();
        }

        DataTable table = filter.loadData(dbConn, null, select, from);

        foreach (DataColumn col in table.Columns)
        {
            if (col.DataType.Equals(typeof(string)))
            {
                DBAESEncryptStringFieldAttribute.decode(table, col.ColumnName);
            }
        }

        return table;
    }

    public static int[] ObjectList2IDList(ArrayList objectList, string idFieldName)
    {
        // build and return list of ID from a list of object 
        ArrayList m_intArrayList = new ArrayList();

        foreach (object o in objectList)
        {
            Int32 m_intObject = new Int32();
            m_intObject = (Int32)Reflection.GetPropValue(o, idFieldName);
            m_intArrayList.Add(m_intObject);
        }

        return (int[])m_intArrayList.ToArray(System.Type.GetType("System.Int32"));
    }

    public static string Encode(DBField field, string value)
    {
        DBFieldTranscoder m_transcoder = field.transcoder;
        if (m_transcoder != null)
            return m_transcoder.toDB(value).ToString();
        return value;
    }

    public static string Decode(DBField field, string value)
    {
        DBFieldTranscoder m_transcoder = field.transcoder;
        if (m_transcoder != null)
            return m_transcoder.fromDB(value).ToString();
        return value;
    }

}
