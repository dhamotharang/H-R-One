using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using HROne.DataAccess;
//using perspectivemind.validation;
using HROne.Lib.Entities;

namespace HROne.Import
{

    public class ImportEmpSkillProcess : ImportProcessInteface
    {

        [DBClass("UploadEmpSkill")]
        private class EUploadEmpSkill : ImportDBObject
        {
            public static DBManager db = new DBManager(typeof(EUploadEmpSkill));

            protected int m_UploadEmpSkillID;
            [DBField("UploadEmpSkillID", true, true), TextSearch, Export(false)]
            public int UploadEmpSkillID
            {
                get { return m_UploadEmpSkillID; }
                set { m_UploadEmpSkillID = value; modify("UploadEmpSkillID"); }
            }
            protected int m_UploadEmpID;
            [DBField("UploadEmpID"), TextSearch, Export(false)]
            public int UploadEmpID
            {
                get { return m_UploadEmpID; }
                set { m_UploadEmpID = value; modify("UploadEmpID"); }
            }

            protected int m_EmpSkillID;
            [DBField("EmpSkillID"), TextSearch, Export(false)]
            public int EmpSkillID
            {
                get { return m_EmpSkillID; }
                set { m_EmpSkillID = value; modify("EmpSkillID"); }
            }
            protected int m_EmpID;
            [DBField("EmpID"), TextSearch, Export(false)]
            public int EmpID
            {
                get { return m_EmpID; }
                set { m_EmpID = value; modify("EmpID"); }
            }
            protected int m_SkillID;
            [DBField("SkillID"), TextSearch, Export(false), Required]
            public int SkillID
            {
                get { return m_SkillID; }
                set { m_SkillID = value; modify("SkillID"); }
            }
            protected int m_SkillLevelID;
            [DBField("SkillLevelID"), TextSearch, Export(false), Required]
            public int SkillLevelID
            {
                get { return m_SkillLevelID; }
                set { m_SkillLevelID = value; modify("SkillLevelID"); }
            }
            //  For Synchronize Use only
            protected string m_SynID;
            [DBField("SynID"), TextSearch, Export(false)]
            public string SynID
            {
                get { return m_SynID; }
                set { m_SynID = value; modify("SynID"); }
            }


        }

        public const string TABLE_NAME = "skills";

        //  Field Const for Employee Personal Information
        private const string FIELD_EMP_NO = "Emp No";
        private const string FIELD_SKILL = "Skill";
        private const string FIELD_SKILL_LEVEL = "Skill Level";

        
        private DateTime UploadDateTime = AppUtils.ServerDateTime();
        private DBManager tempDB = EUploadEmpSkill.db;
        private DBManager uploadDB = EEmpSkill.db;

        public ImportErrorList errors = new ImportErrorList();
        

        public ImportEmpSkillProcess(DatabaseConnection dbConn, string SessionID): base(dbConn, SessionID)
        {
          
            
        }


        public DataTable UploadToTempDatabase(DataTable rawDataTable, int UserID, bool CreateCodeIfNotExists)
        {
            if (rawDataTable == null)
                return GetImportDataFromTempDatabase(null);

            int rowCount = 1;
            foreach (DataRow row in rawDataTable.Rows)
            {
                rowCount++;

                EUploadEmpSkill uploadEmpSkill = new EUploadEmpSkill();
                string EmpNo = row[FIELD_EMP_NO].ToString().Trim();
                uploadEmpSkill.EmpID = Parse.GetEmpID(dbConn, EmpNo, UserID);
                if (uploadEmpSkill.EmpID < 0)
                    errors.addError(ImportErrorMessage.ERROR_ACCESS_DENIED_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                uploadEmpSkill.SkillID = Parse.GetSkillID(dbConn, row[FIELD_SKILL].ToString(), CreateCodeIfNotExists, UserID);
                uploadEmpSkill.SkillLevelID = Parse.GetSkillLevelID(dbConn, row[FIELD_SKILL_LEVEL].ToString(), CreateCodeIfNotExists, UserID);

                if (uploadEmpSkill.SkillLevelID == 0)
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_SKILL_LEVEL + "=" + row[FIELD_SKILL_LEVEL].ToString(), EmpNo, rowCount.ToString() });

                uploadEmpSkill.SessionID = m_SessionID;
                uploadEmpSkill.TransactionDate = UploadDateTime;

                if (uploadEmpSkill.EmpID != 0)
                {
                    //if (rawDataTable.Columns.Contains(FIELD_INTERNAL_ID))
                    //{
                    //    try
                    //    {
                    //        if (!row.IsNull(FIELD_INTERNAL_ID))
                    //        {
                    //            int tmpID = FromHexDecWithCheckDigit((string)row[FIELD_INTERNAL_ID]);
                    //            EEmpSkill tmpObj = new EEmpSkill();
                    //            tmpObj.EmpSkillID = tmpID;
                    //            if (EEmpSkill.db.select(dbConn, tmpObj))
                    //                uploadEmpSkill.EmpSkillID = tmpID;
                    //            else
                    //            {
                    //                errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_INTERNAL_ID + "=" + row[FIELD_INTERNAL_ID].ToString(), EmpNo, rowCount.ToString() });
                    //                continue;
                    //            }
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_INTERNAL_ID + "=" + row[FIELD_INTERNAL_ID].ToString(), EmpNo, rowCount.ToString() });
                    //        continue;
                    //    }
                    //}

                    if (rawDataTable.Columns.Contains(FIELD_SYNC_ID))
                    {
                        if (!row.IsNull(FIELD_SYNC_ID))
                        {
                            string strSynID = row[FIELD_SYNC_ID].ToString();
                            uploadEmpSkill.SynID = strSynID;
                            if (!string.IsNullOrEmpty(strSynID))
                            {
                                DBFilter synIDFilter = new DBFilter();
                                synIDFilter.add(new Match("SynID", strSynID));
                                ArrayList objSameSynIDList = EEmpSkill.db.select(dbConn, synIDFilter);
                                if (objSameSynIDList.Count > 0)
                                    uploadEmpSkill.EmpSkillID = ((EEmpSkill)objSameSynIDList[0]).EmpSkillID;
                            }
                        }

                    }

                    if (uploadEmpSkill.EmpSkillID == 0)
                    {
                        AND andTerms = new AND();
                        andTerms.add(new Match("SkillID", uploadEmpSkill.SkillID));

                        EEmpSkill lastEmpSkill = (EEmpSkill)AppUtils.GetLastObj(dbConn, uploadDB, "EmpSkillID", uploadEmpSkill.EmpID, andTerms);


                        if (lastEmpSkill != null)
                        {
                            if (uploadEmpSkill.SkillLevelID == lastEmpSkill.SkillLevelID)
                                continue;
                            else
                            {
                                uploadEmpSkill.EmpSkillID = lastEmpSkill.EmpSkillID;
                            }
                        }
                    }
                }

                if (uploadEmpSkill.EmpSkillID <= 0)
                    uploadEmpSkill.ImportActionStatus = ImportDBObject.ImportActionEnum.INSERT;
                else
                    uploadEmpSkill.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;

                uploadEmpSkill.UploadEmpID = ParseTemp.GetUploadEmpID(dbConn, EmpNo, m_SessionID);
                if (uploadEmpSkill.UploadEmpID == 0)
                    if (uploadEmpSkill.EmpID == 0)
                        errors.addError(ImportErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                    else
                        uploadEmpSkill.UploadEmpID = ImportEmpPersonalInfoProcess.CreateDummyUploadEmployeeInfo(dbConn, uploadEmpSkill.EmpID, m_SessionID, UploadDateTime);

                Hashtable values = new Hashtable();
                tempDB.populate(uploadEmpSkill, values);
                PageErrors pageErrors = new PageErrors(EUploadEmpPersonalInfo.db);
                tempDB.validate(pageErrors, values);
                if (pageErrors.errors.Count == 0)
                {

                    tempDB.insert(dbConn, uploadEmpSkill);
                }
                else
                {
                    pageErrors.addError(rawDataTable.TableName);
                    throw new HRImportException(pageErrors.getPrompt() + "(line " +rowCount.ToString() + ")");

                }

            }
            if (errors.List.Count > 0)
            {
                ClearTempTable();
                throw (new HRImportException(rawDataTable.TableName + "\r\n"+ errors.Message()));
            }
            return GetImportDataFromTempDatabase(null);
        }

        public override DataTable UploadToTempDatabase(string Filename, int UserID, string ZipPassword)
        {
            ClearTempTable();
            DataTable rawDataTable = HROne.Import.ExcelImport.parse(Filename, ZipPassword).Tables[TABLE_NAME];
            return UploadToTempDatabase(rawDataTable, UserID, true);
        }
        public override DataTable GetImportDataFromTempDatabase(ListInfo info)
        {
            DBFilter sessionFilter = new DBFilter();
            sessionFilter.add(new Match("SessionID", m_SessionID));
            if (info != null && info.orderby != null && !info.orderby.Equals(""))
                sessionFilter.add(info.orderby, info.order);
            DataTable table = sessionFilter.loadData(dbConn, info, " c.* ", " from " + tempDB.dbclass.tableName + " c ");


            return table;

        }

        public static void ClearTempTable(DatabaseConnection dbConn, string sessionID)
        {
            ImportEmpSkillProcess import = new ImportEmpSkillProcess(dbConn, sessionID);
            import.ClearTempTable();
        }

        public override void ClearTempTable()
        {
            //  Clear Old Import Session
            DBFilter sessionFilter = new DBFilter();
            if (!string.IsNullOrEmpty(m_SessionID))
                sessionFilter.add(new Match("SessionID", m_SessionID));
            tempDB.delete(dbConn, sessionFilter);
        }

        public override void ImportToDatabase()
        {
            ImportToDatabase(0);
        }
        public void ImportToDatabase(int UploadEmpID)
        {
            DBFilter sessionFilter = new DBFilter();
            sessionFilter.add(new Match("SessionID", m_SessionID));
            if (UploadEmpID > 0)
                sessionFilter.add(new Match("UploadEmpID", UploadEmpID));
            ArrayList uploadEmpSkillList = tempDB.select(dbConn, sessionFilter);
            foreach (EUploadEmpSkill obj in uploadEmpSkillList)
            {
                EEmpSkill empSkill = new EEmpSkill();

                if (obj.ImportActionStatus != ImportDBObject.ImportActionEnum.INSERT)
                {
                    empSkill.EmpSkillID = obj.EmpSkillID;
                    uploadDB.select(dbConn, empSkill);
                }

                obj.ExportToObject(empSkill);

                if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.INSERT)
                {


                    empSkill.EmpID = ParseTemp.GetEmpIDFromUploadEmpID(dbConn, obj.UploadEmpID);
                    uploadDB.insert(dbConn, empSkill);
                }
                else if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.UPDATE)
                {
                    uploadDB.update(dbConn, empSkill);

                }
                tempDB.delete(dbConn, obj);
            }
        }

        public static DataTable Export(DatabaseConnection dbConn, ArrayList empList, bool IsIncludeCurrentPositionInfo, bool IsShowDescription)
        {
            return Export(dbConn, empList, IsIncludeCurrentPositionInfo, IsShowDescription, false, new DateTime());
        }
        public static DataTable Export(DatabaseConnection dbConn, ArrayList empList, bool IsIncludeCurrentPositionInfo, bool IsShowDescription, bool IsIncludeSyncID, DateTime ReferenceDateTime)
        {
            DataTable tmpDataTable = new DataTable(TABLE_NAME);
            //if (IsIncludeInternalID)
            //    tmpDataTable.Columns.Add(FIELD_INTERNAL_ID, typeof(string));
            tmpDataTable.Columns.Add(FIELD_EMP_NO, typeof(string));

            if (IsIncludeCurrentPositionInfo)
            {
                ImportEmpPersonalInfoProcess.AddEmployeeInfoHeader(tmpDataTable);
                ImportEmpPositionInfoProcess.AddEmployeePositionInfoHeader(dbConn, tmpDataTable);
            }

            tmpDataTable.Columns.Add(FIELD_SKILL);
            tmpDataTable.Columns.Add(FIELD_SKILL_LEVEL);
            if (IsIncludeSyncID)
                tmpDataTable.Columns.Add(FIELD_SYNC_ID, typeof(string));

            foreach (EEmpPersonalInfo empInfo in empList)
            {
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                {
                    DBFilter filter = new DBFilter();
                    filter.add(new Match("EmpID", empInfo.EmpID));
                    filter.add(getCreateModifiedRecordsAfterDBTerm(ReferenceDateTime));
                    ArrayList list = EEmpSkill.db.select(dbConn, filter);
                    foreach (EEmpSkill empSkill in list)
                    {
                        DataRow row = tmpDataTable.NewRow();
                        //if (IsIncludeInternalID)
                        //    row[FIELD_INTERNAL_ID] = ToHexDecWithCheckDigit(empSkill.EmpSkillID);
                        row[FIELD_EMP_NO] = empInfo.EmpNo;

                        if (IsIncludeCurrentPositionInfo)
                        {
                            ImportEmpPersonalInfoProcess.AddEmployeeInfo(dbConn, row, empInfo.EmpID);
                            ImportEmpPositionInfoProcess.AddEmployeePositionInfo(dbConn, row, empInfo.EmpID);
                        }

                        ESkill skill = new ESkill();
                        skill.SkillID = empSkill.SkillID;
                        if (ESkill.db.select(dbConn, skill))
                            row[FIELD_SKILL] = IsShowDescription ? skill.SkillDesc : skill.SkillCode;

                        ESkillLevel skillLevel = new ESkillLevel();
                        skillLevel.SkillLevelID = empSkill.SkillLevelID;
                        if (ESkillLevel.db.select(dbConn, skillLevel))
                            row[FIELD_SKILL_LEVEL] = IsShowDescription ? skillLevel.SkillLevelDesc : skillLevel.SkillLevelCode;
                        if (IsIncludeSyncID)
                            row[FIELD_SYNC_ID] = empSkill.SynID;

                        tmpDataTable.Rows.Add(row);
                    }
                }

            }
            if (IsIncludeCurrentPositionInfo)
                ImportEmpPositionInfoProcess.RetriveHierarchyLevelHeader(dbConn, tmpDataTable);
            return tmpDataTable;
        }
    }

}
