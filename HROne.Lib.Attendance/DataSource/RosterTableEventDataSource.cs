
using System;
using System.Data;
using System.Collections;
using HROne.Lib.Entities;
using HROne.DataAccess;

/// <summary>
/// Summary description for CustomerEventDataSource
/// </summary>
/// 

//public class SchedulerResource
//{
//    private object id;
//    private string caption;
//    private System.Drawing.Color color;
//    private System.Drawing.Image image;

//    public SchedulerResource()
//    {
//    }

//    public object Id
//    {
//        get { return this.id; }
//        set { this.id = value; }
//    }
//    public string Caption
//    {
//        get { return this.caption; }
//        set { this.caption = value; }
//    }

//    public System.Drawing.Color Color
//    {
//        get { return this.color; }
//        set { this.color = value; }
//    }

//    public System.Drawing.Image Image
//    {
//        get { return this.image; }
//        set { this.image = value; }
//    }
//}
//public class SchedulerResourceList : System.ComponentModel.BindingList<SchedulerResource>
//{
//    public void AddRange(SchedulerResourceList resources)
//    {
//        foreach (SchedulerResource customResource in resources)
//            this.Add(customResource);
//    }
//    public int GetResourceIndex(object resourceId)
//    {
//        for (int i = 0; i < Count; i++)
//            if (this[i].Id == resourceId)
//                return i;
//        return -1;
//    }

//}

//public class SchedulerResourceDataSource
//{
//    public SchedulerResourceList SelectSchedulerResource()
//    {
//        SchedulerResourceList resourceList = new SchedulerResourceList();

//        DBFilter rosterCodeFilter = new DBFilter();
//        rosterCodeFilter.add(new IN("RosterCodeID", "Select distinct RosterCodeID from " + ERosterTable.db.dbclass.tableName, new DBFilter()));

//        DBFilter rosterClientSiteFilter = new DBFilter();
//        rosterClientSiteFilter.add(new IN("RosterClientSiteID", "Select Distinct RosterClientSiteID from " + ERosterCode.db.dbclass.tableName, rosterCodeFilter));

//        ArrayList rosterClientSiteList = ERosterClientSite.db.select(dbConn, rosterClientSiteFilter);
//        foreach (ERosterClientSite rosterClientSite in rosterClientSiteList)
//        {
//            ERosterClient rosterClient = new ERosterClient();
//            rosterClient.RosterClientID = rosterClientSite.RosterClientID;
//            if (ERosterClient.db.select(dbConn, rosterClient))
//            {
//                SchedulerResource resource = new SchedulerResource();
//                resource.Id = rosterClientSite.RosterClientSiteID;
//                resource.Caption = rosterClient.RosterClientCode + " - " + rosterClientSite.RosterClientSiteCode;
//                resourceList.Add(resource);
//            }
//        }
//        return resourceList;
//    }

//    //public static void InsertSchedulerResource(SchedulerResource e)
//    //{
//    //}

//    //public static void UpdateSchedulerResource(SchedulerResource e)
//    //{
//    //}

//    //public static void DeleteSchedulerResource(SchedulerResource e)
//    //{
//    //}
//}
namespace HROne.Lib.Attendance.DataSource
{

    public class RosterTableEvent
    {
        string id;
        DateTime start;
        DateTime end;
        string subject;
        int status;
        string description;
        string toolTip;
        long label;
        string location;
        bool allday;
        int eventType;
        string recurrenceInfo;
        string reminderInfo;
        object ownerId;
        string recurrenceParentId;
        bool m_isCancel;
        //Telerik.Web.UI.RecurrenceState recurState;

        public const string ROSTER_TABLE_ID_PREFIX = "RosterTable";
        public const string LEAVEAPP_ID_PREFIX = "LeaveApp";
        public const string PUBLICHOLIDAY_PREFIX = "PublicHoliday";
        public const string STATUTORYHOLIDAY_PREFIX = "StatutoryHoliday";
        //public Telerik.Web.UI.RecurrenceState RecurrenceState
        //{
        //    get { return recurState; }
        //    set { recurState = value; }
        //}

        public RosterTableEvent()
        {
        }

        public DateTime StartTime { get { return start; } set { start = value; } }
        public DateTime EndTime { get { return end; } set { end = value; } }
        public string Subject { get { return subject; } set { subject = value; } }
        public int Status { get { return status; } set { status = value; } }
        public string Description { get { return description; } set { description = value; } }
        public string ToolTip { get { return toolTip; } set { toolTip = value; } }
        public long Label { get { return label; } set { label = value; } }
        public string Location { get { return location; } set { location = value; } }
        public bool AllDay { get { return allday; } set { allday = value; } }
        public int EventType { get { return eventType; } set { eventType = value; } }
        public string RecurrenceInfo { get { return recurrenceInfo; } set { recurrenceInfo = value; } }
        public string RecurrenceParentId { get { return recurrenceParentId; } set { recurrenceParentId = value; } }
        public string ReminderInfo { get { return reminderInfo; } set { reminderInfo = value; } }
        public object OwnerId { get { return ownerId; } set { ownerId = value; } }
        public string Id { get { return id; } set { id = value; } }
        public bool IsCancel { get { return m_isCancel; } set { m_isCancel = value; } }

        public void Update(DatabaseConnection dbConn)
        {
            if (id != null)
            {
                object obj = IDToDBObject(dbConn, id);
                if (obj is ERosterTable)
                {
                    ERosterTable rosterTable = (ERosterTable)obj;
                    ERosterCode rosterCode = new ERosterCode();
                    rosterCode.RosterCodeID = rosterTable.RosterCodeID;
                    if (ERosterCode.db.select(dbConn, rosterCode))
                    {
                        if (rosterCode.RosterCodeInTime.TimeOfDay.Equals(StartTime.TimeOfDay))
                            rosterTable.RosterTableOverrideInTime = new DateTime();
                        else
                            rosterTable.RosterTableOverrideInTime = StartTime;

                        if (rosterCode.RosterCodeOutTime.TimeOfDay.Equals(EndTime.TimeOfDay))
                            rosterTable.RosterTableOverrideOutTime = new DateTime();
                        else
                            rosterTable.RosterTableOverrideOutTime = EndTime;
                    }
                    ERosterTable.db.update(dbConn, rosterTable);
                }
            }

        }
        public static DBObject IDToDBObject(DatabaseConnection dbConn, object appointmentID)
        {
            if (appointmentID != null)
            {
                string item_ID = appointmentID.ToString();
                string[] id_Array = item_ID.ToString().Split(new char[] { '_' });

                if (id_Array.GetLength(0) == 2)
                {
                    if (id_Array[0].Equals(RosterTableEvent.ROSTER_TABLE_ID_PREFIX))
                    {
                        ERosterTable rosterTable = new ERosterTable();
                        rosterTable.RosterTableID = int.Parse(id_Array[1]);
                        if (ERosterTable.db.select(dbConn, rosterTable))
                            return rosterTable;

                    }
                }
            }
            return null;
        }
    }

    public class RosterTableEventList : System.ComponentModel.BindingList<RosterTableEvent>
    {
        public void AddRange(RosterTableEventList events)
        {
            foreach (RosterTableEvent customEvent in events)
                if (GetEventIndex(customEvent.Id) < 0)
                    this.Add(customEvent);
        }
        public int GetEventIndex(object eventId)
        {
            for (int i = 0; i < Count; i++)
                if (this[i].Id.ToString().Equals(eventId.ToString()))
                    return i;
            return -1;
        }

    }


    public class RosterTableEventDataSource
    {
        protected DateTime DateFrom = new DateTime();
        protected DateTime DateTo = new DateTime();
        protected ArrayList RosterClientSiteIDList = null;
        protected bool DisplayOvernightToNextDay;
        protected int UserID = 0;
        protected int ESSUserID = 0;
        protected bool ShowEmployeeRosterWithSameSite;
        protected bool ShowRosterCodeWithoutSiteMapping;
        RosterTableEventList eventList = null;
        public DatabaseConnection dbConn;
        //SchedulerResourceList resourceList = null;


        public void clear()
        {
            eventList = null;
            //resourceList = null;
        }

        public RosterTableEventList Select(DateTime DateFrom, DateTime DateTo, int UserID, ArrayList RosterClientSiteIDList, bool DisplayOvernightToNextDay, bool ShowEmployeeRosterWithSameSite, bool ShowRosterCodeWithoutSiteMapping)
        {
            this.DateFrom = DateFrom;
            this.DateTo = DateTo;
            this.RosterClientSiteIDList = (ArrayList)RosterClientSiteIDList;
            this.DisplayOvernightToNextDay = DisplayOvernightToNextDay;
            this.UserID = UserID;
            this.ShowEmployeeRosterWithSameSite = ShowEmployeeRosterWithSameSite;
            this.ShowRosterCodeWithoutSiteMapping = ShowRosterCodeWithoutSiteMapping;
            LoadRosterTableEventList();
            return eventList;
        }

        // Start 0000179, KuangWei, 2015-03-20
        public RosterTableEventList Select(DateTime DateFrom, DateTime DateTo, int ESSUserID, bool DisplayOvernightToNextDay, bool LeaveChecking, bool RosterChecking)
        {
            return Select(DateFrom, DateTo, ESSUserID, DisplayOvernightToNextDay, 0, LeaveChecking, RosterChecking);
        }
        public RosterTableEventList Select(DateTime DateFrom, DateTime DateTo, int ESSUserID, bool DisplayOvernightToNextDay, int RosterTableGroupID, bool LeaveChecking, bool RosterChecking)
        {
            eventList = new RosterTableEventList();

            for (DateTime rosterTableDate = DateFrom.AddDays(-1); rosterTableDate <= DateTo; rosterTableDate = rosterTableDate.AddDays(1))
            {
                DBFilter rosterTableFilter = new DBFilter();
                rosterTableFilter.add(new Match("RosterTableDate", "=", rosterTableDate));

                DBFilter leaveAppFilter = new DBFilter();
                leaveAppFilter.add(new Match("LeaveAppDateFrom", "<=", rosterTableDate));
                leaveAppFilter.add(new Match("LeaveAppDateTo", ">=", rosterTableDate));

                //EEmpPositionInfo empPos = AppUtils.GetLastPositionInfo(dbConn, rosterTableDate, ESSUserID);
                OR orRosterTableTerms = new OR();
                orRosterTableTerms.add(new Match("EmpID", ESSUserID));

                OR orLeaveAppTerms = new OR();
                orLeaveAppTerms.add(new Match("EmpID", ESSUserID));

                DBFilter empRosterTableGroupListFilter = new DBFilter();
                {
                    empRosterTableGroupListFilter.add(new Match("EmpID", ESSUserID));
                    empRosterTableGroupListFilter.add(new Match("empRosterTableGroupEffFr", "<=", DateTo));
                    OR orEmpPosEffToTerms = new OR();
                    orEmpPosEffToTerms.add(new Match("empRosterTableGroupEffTo", ">=", DateFrom));
                    orEmpPosEffToTerms.add(new NullTerm("empRosterTableGroupEffTo"));
                    empRosterTableGroupListFilter.add(orEmpPosEffToTerms);
                }
                System.Collections.Generic.List<string> RosterTableGroupIDList = new System.Collections.Generic.List<string>();
                if (RosterTableGroupID == 0)
                {
                    ArrayList empRosterTableGroupList = EEmpRosterTableGroup.db.select(dbConn, empRosterTableGroupListFilter);
                    foreach (EEmpRosterTableGroup empRosterTableGroup in empRosterTableGroupList)
                    {
                        string idString = empRosterTableGroup.RosterTableGroupID.ToString();
                        if (!RosterTableGroupIDList.Contains(idString))
                            RosterTableGroupIDList.Add(idString);
                    }
                }
                else
                    RosterTableGroupIDList.Add(RosterTableGroupID.ToString());

                if (RosterTableGroupIDList.Count > 0)
                {
                    string RosterTableGroupListInString = string.Join(",", RosterTableGroupIDList.ToArray());

                    //ERosterTableGroup rosterTableGroup = new ERosterTableGroup();
                    //rosterTableGroup.RosterTableGroupID = empPos.RosterTableGroupID;
                    //if (ERosterTableGroup.db.select(dbConn, rosterTableGroup))
                    //{


                    DBFilter rosterClientSiteFilter = new DBFilter();
                    {
                        DBFilter rosterTableGroupFilter = new DBFilter();
                        rosterTableGroupFilter.add(new IN("rtg.RosterTableGroupID", RosterTableGroupListInString, null));

                        OR orRosterClientSiteIDTerms = new OR();
                        orRosterClientSiteIDTerms.add(new IN("rc.RosterClientSiteID", "SELECT DISTINCT RosterClientSiteID FROM " + ERosterTableGroup.db.dbclass.tableName + " rtg", rosterTableGroupFilter));

                        rosterClientSiteFilter.add(orRosterClientSiteIDTerms);
                    }
                    orRosterTableTerms.add(new IN("RosterCodeID", "Select rc.RosterCodeID From " + ERosterCode.db.dbclass.tableName + " rc", rosterClientSiteFilter));

                    DBFilter empRosterTableGroupFilter = new DBFilter();
                    {
                        DBFilter rosterTableGroupFilter = new DBFilter();
                        rosterTableGroupFilter.add(new IN("ertg.RosterTableGroupID", RosterTableGroupListInString, null));
                        empRosterTableGroupFilter.add(new Exists(ERosterTableGroup.db.dbclass.tableName + " rtg", rosterTableGroupFilter));

                        empRosterTableGroupFilter.add(new Match("ertg.empRosterTableGroupEffFr", "<=", rosterTableDate));
                        OR orEmpPosEffToTerms = new OR();
                        orEmpPosEffToTerms.add(new Match("ertg.empRosterTableGroupEffTo", ">=", rosterTableDate));
                        orEmpPosEffToTerms.add(new NullTerm("ertg.empRosterTableGroupEffTo"));
                        empRosterTableGroupFilter.add(orEmpPosEffToTerms);
                    }

                    orRosterTableTerms.add(new IN("EmpID", "Select distinct EmpID From  " + EEmpRosterTableGroup.db.dbclass.tableName + " ertg ", empRosterTableGroupFilter));

                    orLeaveAppTerms.add(new IN("EmpID", "Select distinct EmpID From  " + EEmpRosterTableGroup.db.dbclass.tableName + " ertg ", empRosterTableGroupFilter));
                    //}


                }

                if (LeaveChecking && RosterChecking)
                {
                    rosterTableFilter.add(orRosterTableTerms);
                    ArrayList rosterTableList = ERosterTable.db.select(dbConn, rosterTableFilter);
                    eventList.AddRange(GenerateEventList(rosterTableList));

                    leaveAppFilter.add(orLeaveAppTerms);
                    ArrayList leaveAppList = ELeaveApplication.db.select(dbConn, leaveAppFilter);
                    eventList.AddRange(GenerateEventList(leaveAppList));
                }
                else if (RosterChecking)
                {
                    rosterTableFilter.add(orRosterTableTerms);
                    ArrayList rosterTableList = ERosterTable.db.select(dbConn, rosterTableFilter);
                    eventList.AddRange(GenerateEventList(rosterTableList));
                }
                else if (LeaveChecking)
                {
                    leaveAppFilter.add(orLeaveAppTerms);
                    ArrayList leaveAppList = ELeaveApplication.db.select(dbConn, leaveAppFilter);
                    eventList.AddRange(GenerateEventList(leaveAppList));
                }
            }

            //  Put the holiday list at the end of the collection so that the item can be display at the top of the day.
            eventList.AddRange(GenerateEventList(HolidayDBObjectList(DateFrom, DateTo)));


            return eventList;
        }
        // End 0000179, KuangWei, 2015-03-20

        protected virtual void LoadRosterTableEventList()
        {
            eventList = new RosterTableEventList();
            //resourceList = new SchedulerResourceList();

            DBFilter rosterTableFilter = new DBFilter();
            rosterTableFilter.add(new Match("RosterTableDate", ">=", DateFrom.AddDays(-1).Date));
            rosterTableFilter.add(new Match("RosterTableDate", "<=", DateTo.AddMilliseconds(-1).Date));

            DBFilter rosterClientSiteFilter = new DBFilter();
            OR orRosterClientSiteIDTerms = new OR();
            orRosterClientSiteIDTerms.add(new Match("rc.RosterClientSiteID", -1));

            //OR orEmpHierarchyTerms = null;
            OR orEmpPosRosterTableGroupTerms = null;

            foreach (object obj in RosterClientSiteIDList)
            {
                ERosterClientSite site = null;
                if (obj is string)
                {
                    string rosterClientSiteString = (string)obj;
                    int rosterClientSiteID = 0;
                    site = new ERosterClientSite();
                    if (int.TryParse(rosterClientSiteString, out rosterClientSiteID))
                    {
                        site.RosterClientSiteID = rosterClientSiteID;
                    }
                    else
                    {
                    }
                }
                else if (obj is ERosterClientSite)
                {
                    site = (ERosterClientSite)obj;
                }

                if (site != null)
                {
                    orRosterClientSiteIDTerms.add(new Match("rc.RosterClientSiteID", site.RosterClientSiteID));

                    if (ShowEmployeeRosterWithSameSite)
                    {
                        if (ERosterClientSite.db.select(dbConn, site))
                        {
                            //ERosterClient rosterClient = new ERosterClient();
                            //rosterClient.RosterClientID = site.RosterClientID;
                            //if (ERosterClient.db.select(dbConn, rosterClient))
                            //{
                            //    DBFilter hElementFilter = new DBFilter();
                            //    hElementFilter.add(new Match("HLevelID", rosterClient.RosterClientMappingSiteCodeToHLevelID));
                            //    ArrayList hElementList = EHierarchyElement.db.select(dbConn, hElementFilter);
                            //    foreach (EHierarchyElement hElement in hElementList)
                            //    {
                            //        if (hElement.HElementCode.Trim().Equals(site.RosterClientSiteCode.ToString().Trim()))
                            //        {
                            //            DBFilter sub = new DBFilter();
                            //            sub.add(new MatchField("epi.EmpPosID", "eeh.EmpPosID"));
                            //            sub.add(new Match("eeh.HLevelID", hElement.HLevelID));
                            //            sub.add(new Match("eeh.HElementID", hElement.HElementID));
                            //            if (orEmpHierarchyTerms == null)
                            //                orEmpHierarchyTerms = new OR();
                            //            orEmpHierarchyTerms.add(new Exists(EEmpHierarchy.db.dbclass.tableName + " eeh", sub));
                            //        }


                            //    }

                            //}


                            DBFilter rosterTableGroupFilter = new DBFilter();
                            rosterTableGroupFilter.add(new MatchField("ertg.RosterTableGroupID", "rtg.RosterTableGroupID"));
                            rosterTableGroupFilter.add(new Match("RosterClientSiteID", site.RosterClientSiteID));

                            if (orEmpPosRosterTableGroupTerms == null)
                                orEmpPosRosterTableGroupTerms = new OR();
                            orEmpPosRosterTableGroupTerms.add(new Exists(ERosterTableGroup.db.dbclass.tableName + " rtg", rosterTableGroupFilter));
                        }
                    }
                }
            }
            OR orRosterTableTerms = new OR();
            if (ShowRosterCodeWithoutSiteMapping)
            {
                orRosterClientSiteIDTerms.add(new Match("rc.RosterClientSiteID", 0));
                DBFilter notExistsFilter = new DBFilter();
                notExistsFilter.add(new MatchField("RosterTable.RosterCodeID", "nic.RosterCodeID"));
                orRosterTableTerms.add(new Exists(ERosterCode.db.dbclass.tableName + " nic", notExistsFilter, true));
            }
            rosterClientSiteFilter.add(orRosterClientSiteIDTerms);
            orRosterTableTerms.add(new IN("RosterCodeID", "Select rc.RosterCodeID From " + ERosterCode.db.dbclass.tableName + " rc", rosterClientSiteFilter));

            //if (orEmpHierarchyTerms != null)
            //{
            //    DBFilter empRosterTableGroupFilter = new DBFilter();

            //    empRosterTableGroupFilter.add(orEmpHierarchyTerms);

            //    empRosterTableGroupFilter.add(new Match("ertg.empRosterTableGroupEffFr", "<=", DateTo));
            //    OR orEmpPosEffToTerms = new OR();
            //    orEmpPosEffToTerms.add(new Match("ertg.empRosterTableGroupEffTo", ">=", DateFrom));
            //    orEmpPosEffToTerms.add(new NullTerm("ertg.empRosterTableGroupEffTo"));
            //    empRosterTableGroupFilter.add(orEmpPosEffToTerms);


            //    orRosterTableTerms.add(new IN("EmpID", "Select distinct EmpID From  " + EEmpPositionInfo.db.dbclass.tableName + " epi ", empRosterTableGroupFilter));

            //}

            if (orEmpPosRosterTableGroupTerms != null)
            {
                DBFilter empRosterTableGroupFilter = new DBFilter();

                empRosterTableGroupFilter.add(orEmpPosRosterTableGroupTerms);

                empRosterTableGroupFilter.add(new Match("ertg.empRosterTableGroupEffFr", "<=", DateTo));
                OR orEmpPosEffToTerms = new OR();
                orEmpPosEffToTerms.add(new Match("ertg.empRosterTableGroupEffTo", ">=", DateFrom));
                orEmpPosEffToTerms.add(new NullTerm("ertg.empRosterTableGroupEffTo"));
                empRosterTableGroupFilter.add(orEmpPosEffToTerms);


                orRosterTableTerms.add(new IN("EmpID", "Select distinct EmpID From  " + EEmpRosterTableGroup.db.dbclass.tableName + " ertg ", empRosterTableGroupFilter));

            }

            rosterTableFilter.add(orRosterTableTerms);
            if (UserID > 0)
            {
                rosterTableFilter.add(AppUtils.AddRankDBTerm(UserID, "EmpID", true));
            }


            ArrayList rosterTableList = ERosterTable.db.select(dbConn, rosterTableFilter);
            eventList.AddRange(GenerateEventList(rosterTableList));



            DBFilter leaveAppFilter = new DBFilter();
            leaveAppFilter.add(new Match("LeaveAppDateTo", ">=", DateFrom.AddDays(-1)));
            leaveAppFilter.add(new Match("LeaveAppDateFrom", "<=", DateTo));
            if (!ShowRosterCodeWithoutSiteMapping && orEmpPosRosterTableGroupTerms != null)
            {
                DBFilter empRosterTableGroupFilter = new DBFilter();

                empRosterTableGroupFilter.add(orEmpPosRosterTableGroupTerms);

                empRosterTableGroupFilter.add(new Match("ertg.empRosterTableGroupEffFr", "<=", DateTo));
                OR orEmpPosEffToTerms = new OR();
                orEmpPosEffToTerms.add(new Match("ertg.empRosterTableGroupEffTo", ">=", DateFrom));
                orEmpPosEffToTerms.add(new NullTerm("ertg.empRosterTableGroupEffTo"));
                empRosterTableGroupFilter.add(orEmpPosEffToTerms);
                leaveAppFilter.add(new IN("EmpID", "Select distinct EmpID From  " + EEmpRosterTableGroup.db.dbclass.tableName + " ertg ", empRosterTableGroupFilter));
            }
            eventList.AddRange(GenerateEventList(ELeaveApplication.db.select(dbConn, leaveAppFilter)));

            //  Put the holiday list at the end of the collection so that the item can be display at the top of the day.
            eventList.AddRange(GenerateEventList(HolidayDBObjectList(DateFrom, DateTo)));

        }

        protected RosterTableEventList GenerateEventList(ICollection DBObjectList)
        {
            RosterTableEventList tmpEventList = new RosterTableEventList();

            if (DBObjectList != null)
            {
                foreach (DBObject dbObject in DBObjectList)
                {
                    if (dbObject is ERosterTable)
                    {
                        ERosterTable rosterTable = (ERosterTable)dbObject;
                        ERosterCode rosterCode = new ERosterCode();
                        rosterCode.RosterCodeID = rosterTable.RosterCodeID;
                        bool hasRecord = ERosterCode.db.select(dbConn, rosterCode);
                        if (hasRecord || rosterCode.RosterCodeID == 0)
                        {
                            RosterTableEvent eventDetail = new RosterTableEvent();
                            eventDetail.Id = RosterTableEvent.ROSTER_TABLE_ID_PREFIX + "_" + rosterTable.RosterTableID;

                            //  Override Start/End time only when the roster code exists
                            if (rosterTable.RosterTableOverrideInTime.Ticks.Equals(0) || !hasRecord)
                                eventDetail.StartTime = rosterTable.RosterTableDate.Add(rosterCode.RosterCodeInTime.TimeOfDay);
                            else
                                eventDetail.StartTime = rosterTable.RosterTableDate.Add(rosterTable.RosterTableOverrideInTime.TimeOfDay);

                            if (rosterTable.RosterTableOverrideOutTime.Ticks.Equals(0) || !hasRecord)
                                eventDetail.EndTime = rosterTable.RosterTableDate.Add(rosterCode.RosterCodeOutTime.TimeOfDay);
                            else
                                eventDetail.EndTime = rosterTable.RosterTableDate.Add(rosterTable.RosterTableOverrideOutTime.TimeOfDay);
                            if (eventDetail.StartTime >= eventDetail.EndTime)
                            {
                                eventDetail.EndTime = eventDetail.EndTime.AddDays(1);

                                if (!DisplayOvernightToNextDay)
                                    eventDetail.EndTime = eventDetail.EndTime.Date;
                            }

                            EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                            empInfo.EmpID = rosterTable.EmpID;
                            EEmpPersonalInfo.db.select(dbConn, empInfo);

                            eventDetail.Subject = (hasRecord ? rosterCode.RosterCode : "(unspecified)") + " - " + empInfo.EmpEngDisplayName;
                            eventDetail.ToolTip = eventDetail.Subject;
                            eventDetail.Label = rosterCode.RosterClientSiteID;

                            eventDetail.RecurrenceInfo = string.Empty;
                            eventDetail.OwnerId = rosterCode.RosterClientSiteID;

                            tmpEventList.Add(eventDetail);
                        }
                    }
                    else if (dbObject is ELeaveApplication)
                    {
                        ELeaveApplication leaveApplication = (ELeaveApplication)dbObject;
                        ELeaveCode leaveCode = new ELeaveCode();
                        leaveCode.LeaveCodeID = leaveApplication.LeaveCodeID;
                        bool hasRecord = ELeaveCode.db.select(dbConn, leaveCode);
                        if (hasRecord)
                        {
                            RosterTableEvent eventDetail = new RosterTableEvent();
                            eventDetail.Id = RosterTableEvent.LEAVEAPP_ID_PREFIX + "_" + leaveApplication.LeaveAppID;

                            if (leaveApplication.LeaveAppUnit.Equals(ELeaveApplication.LEAVEUNIT_DAYS))
                            {
                                eventDetail.StartTime = leaveApplication.LeaveAppDateFrom;
                                eventDetail.EndTime = leaveApplication.LeaveAppDateTo.AddDays(1);
                            }
                            else
                            {
                                eventDetail.StartTime = leaveApplication.LeaveAppDateFrom;
                                eventDetail.EndTime = leaveApplication.LeaveAppDateFrom;
                                if (!leaveApplication.LeaveAppTimeFrom.Ticks.Equals(0))
                                    eventDetail.StartTime = eventDetail.StartTime.Add(leaveApplication.LeaveAppTimeFrom.TimeOfDay);
                                if (!leaveApplication.LeaveAppTimeTo.Ticks.Equals(0))
                                    eventDetail.EndTime = eventDetail.EndTime.Add(leaveApplication.LeaveAppTimeTo.TimeOfDay);

                                if (leaveApplication.LeaveAppUnit.Equals(ELeaveApplication.LEAVEUNIT_AM))
                                {
                                    if (eventDetail.StartTime.Equals(leaveApplication.LeaveAppDateFrom))
                                        eventDetail.StartTime = eventDetail.StartTime.Add(new TimeSpan(0, 0, 0));
                                    if (eventDetail.EndTime.Equals(leaveApplication.LeaveAppDateFrom))
                                        eventDetail.EndTime = eventDetail.EndTime.Add(new TimeSpan(13, 0, 0));
                                }
                                if (leaveApplication.LeaveAppUnit.Equals(ELeaveApplication.LEAVEUNIT_PM))
                                {
                                    if (eventDetail.StartTime.Equals(leaveApplication.LeaveAppDateFrom))
                                        eventDetail.StartTime = eventDetail.StartTime.Add(new TimeSpan(13, 0, 0));
                                    if (eventDetail.EndTime.Equals(leaveApplication.LeaveAppDateFrom))
                                        eventDetail.EndTime = eventDetail.EndTime.AddDays(1);
                                }
                                else
                                {
                                    if (eventDetail.StartTime.Equals(leaveApplication.LeaveAppDateFrom))
                                        eventDetail.StartTime = eventDetail.StartTime.Add(new TimeSpan(0, 0, 0));
                                    if (eventDetail.EndTime.Equals(leaveApplication.LeaveAppDateFrom))
                                        eventDetail.EndTime = eventDetail.EndTime.AddDays(1);
                                }

                            }


                            EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                            empInfo.EmpID = leaveApplication.EmpID;
                            EEmpPersonalInfo.db.select(dbConn, empInfo);

                            //eventDetail.Subject = (hasRecord ? leaveCode.LeaveCode : "(unspecified)") + " - " + empInfo.EmpEngDisplayName;

                            if (leaveApplication.LeaveAppDays < 0 || leaveApplication.LeaveAppHours < 0)
                            {
                                eventDetail.IsCancel = true; 
                            }

                            string LeaveUnitDetail = string.Empty;
                            if (leaveApplication.LeaveAppUnit.Equals("D"))
                            {
                                LeaveUnitDetail = leaveApplication.LeaveAppDays + " " + "Day" + (leaveApplication.LeaveAppDays > 1 ? "s" : string.Empty);
                            }
                            else if (leaveApplication.LeaveAppUnit.Equals("H"))
                            {
                                if (leaveApplication.LeaveAppTimeFrom.Ticks.Equals(0) || leaveApplication.LeaveAppTimeTo.Ticks.Equals(0))
                                    LeaveUnitDetail = leaveApplication.LeaveAppHours + " " + "Hour" + (leaveApplication.LeaveAppHours > 1 ? "s" : string.Empty);
                                else
                                    LeaveUnitDetail = leaveApplication.LeaveAppTimeFrom.ToString("HH:mm") + "-" + leaveApplication.LeaveAppTimeTo.ToString("HH:mm");
                            }
                            else if (leaveApplication.LeaveAppUnit.Equals("A"))
                            {
                                if (leaveApplication.LeaveAppDays > 0)
                                    LeaveUnitDetail = "A.M.";
                                else
                                    LeaveUnitDetail = "-A.M.";
                            }
                            else if (leaveApplication.LeaveAppUnit.Equals("P"))
                            {
                                if (leaveApplication.LeaveAppDays > 0)
                                    LeaveUnitDetail = "P.M.";
                                else
                                    LeaveUnitDetail = "-P.M.";
                            }

                            eventDetail.Subject = (hasRecord ? leaveCode.LeaveCode : "(unspecified)")
                                + (string.IsNullOrEmpty(LeaveUnitDetail) ? string.Empty : ("(" + LeaveUnitDetail + ")"))
                                + " - " + empInfo.EmpEngDisplayName;
                            eventDetail.ToolTip = eventDetail.Subject;
                            //eventDetail.Label = rosterCode.RosterClientSiteID;

                            eventDetail.RecurrenceInfo = string.Empty;
                            //eventDetail.OwnerId = rosterCode.RosterClientSiteID;

                            tmpEventList.Add(eventDetail);
                        }
                    }
                    else if (dbObject is EPublicHoliday)
                    {
                        EPublicHoliday holiday = (EPublicHoliday)dbObject;
                        RosterTableEvent eventDetail = new RosterTableEvent();
                        eventDetail.Id = RosterTableEvent.PUBLICHOLIDAY_PREFIX + "_" + holiday.PublicHolidayID;

                        eventDetail.StartTime = holiday.PublicHolidayDate;
                        eventDetail.EndTime = holiday.PublicHolidayDate.AddDays(1);



                        eventDetail.Subject = holiday.PublicHolidayDesc;
                        eventDetail.ToolTip = eventDetail.Subject;
                        //eventDetail.Label = rosterCode.RosterClientSiteID;

                        eventDetail.RecurrenceInfo = string.Empty;
                        //eventDetail.OwnerId = rosterCode.RosterClientSiteID;

                        tmpEventList.Add(eventDetail);

                    }
                    else if (dbObject is EStatutoryHoliday)
                    {
                        EStatutoryHoliday holiday = (EStatutoryHoliday)dbObject;
                        RosterTableEvent eventDetail = new RosterTableEvent();
                        eventDetail.Id = RosterTableEvent.STATUTORYHOLIDAY_PREFIX + "_" + holiday.StatutoryHolidayID;

                        eventDetail.StartTime = holiday.StatutoryHolidayDate;
                        eventDetail.EndTime = holiday.StatutoryHolidayDate.AddDays(1);



                        eventDetail.Subject = holiday.StatutoryHolidayDesc;
                        eventDetail.ToolTip = eventDetail.Subject;
                        //eventDetail.Label = rosterCode.RosterClientSiteID;

                        eventDetail.RecurrenceInfo = string.Empty;
                        //eventDetail.OwnerId = rosterCode.RosterClientSiteID;

                        tmpEventList.Add(eventDetail);

                    }

                }

            }
            return tmpEventList;
        }

        protected ICollection HolidayDBObjectList(DateTime PeriodFrom, DateTime PeriodTo)
        {
            System.Collections.Generic.Dictionary<DateTime, DBObject> holidayList = new System.Collections.Generic.Dictionary<DateTime, DBObject>();
            DBFilter publicHolidayDBFilter = new DBFilter();
            publicHolidayDBFilter.add(new Match("PublicHolidayDate", ">=", PeriodFrom));
            publicHolidayDBFilter.add(new Match("PublicHolidayDate", "<=", PeriodTo));
            ArrayList publicHolidayList = EPublicHoliday.db.select(dbConn, publicHolidayDBFilter);

            foreach (EPublicHoliday publicHoliday in publicHolidayList)
                holidayList.Add(publicHoliday.PublicHolidayDate, publicHoliday);

            DBFilter statutoryHolidayDBFilter = new DBFilter();
            statutoryHolidayDBFilter.add(new Match("StatutoryHolidayDate", ">=", PeriodFrom));
            statutoryHolidayDBFilter.add(new Match("StatutoryHolidayDate", "<=", PeriodTo));
            ArrayList statutoryHolidayList = EStatutoryHoliday.db.select(dbConn, statutoryHolidayDBFilter);
            foreach (EStatutoryHoliday statutoryHoliday in statutoryHolidayList)
            {
                if (!holidayList.ContainsKey(statutoryHoliday.StatutoryHolidayDate))
                    holidayList.Add(statutoryHoliday.StatutoryHolidayDate, statutoryHoliday);
            }

            return holidayList.Values;
        }

        public static void Insert(RosterTableEvent e)
        {
            if (e.Id != null)
            {
            }
        }

        public void Update(RosterTableEvent e)
        {
            if (e.Id != null)
            {
                e.Update(dbConn);
            }

        }

        public static void Delete(RosterTableEvent e)
        {
        }
    }

}
