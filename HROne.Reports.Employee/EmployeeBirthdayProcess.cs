using System;
using System.Collections;
using System.Text;
using HROne.Lib.Entities;
using HROne.Common;
using CrystalDecisions.CrystalReports.Engine;
using HROne.DataAccess;
using HROne.CommonLib;

namespace HROne.Reports.Employee
{
    public class EmployeeBirthdayProcess : GenericReportProcess
    {

        private DateTime BirthdayFrom, BirthdayTo;
        private bool ShowDisplayYearOfBirth;
        private ArrayList empList;
        public EmployeeBirthdayProcess(DatabaseConnection dbConn, ArrayList empList, DateTime BirthdayFrom, DateTime BirthdayTo, bool ShowDisplayYearOfBirth)
            : base(dbConn)
        {
            this.empList = empList;
            this.BirthdayFrom = BirthdayFrom;
            this.BirthdayTo = BirthdayTo;
            this.ShowDisplayYearOfBirth = ShowDisplayYearOfBirth;
        }

        public override ReportDocument GenerateReport()
        {
            DataSet.EmployeeDetail ds = new DataSet.EmployeeDetail();

            DBFilter empInfoFilter = new DBFilter();
            empInfoFilter.add(new Match("empDateOfJoin", "<=", BirthdayTo));

            DBFilter empTerminationFilter = new DBFilter();
            empTerminationFilter.add(new MatchField(EEmpPersonalInfo.db.dbclass.tableName + ".EmpID", "et.EmpID"));
            empTerminationFilter.add(new Match("et.EmpTermLastDate", "<", BirthdayFrom));
            empInfoFilter.add(new Exists(EEmpTermination.db.dbclass.tableName + " et", empTerminationFilter, true));



            foreach (EEmpPersonalInfo empInfo in empList)
            {
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                    if (!empInfo.EmpDateOfBirth.Ticks.Equals(0))
                    {

                        int yearFrom = Convert.ToInt32(Utility.YearDifference(empInfo.EmpDateOfBirth, BirthdayFrom));
                        while (empInfo.EmpDateOfBirth.AddYears(yearFrom) < BirthdayFrom)
                            yearFrom++;





                        if (empInfo.EmpDateOfBirth.AddYears(yearFrom) <= BirthdayTo)
                        {
                            DataSet.EmployeeDetail.employeedetailRow row = ds.employeedetail.NewemployeedetailRow();
                            row.EmpID = empInfo.EmpID;
                            row.EmpAlias = empInfo.EmpAlias;
                            row.EmpChiFullName = empInfo.EmpChiFullName;
                            row.EmpDateOfBirth = empInfo.EmpDateOfBirth;
                            row.EmpDateOfJoin = empInfo.EmpDateOfJoin;
                            row.EmpEngOtherName = empInfo.EmpEngOtherName;
                            row.EmpEngSurname = empInfo.EmpEngSurname;
                            row.EmpEngFullName = empInfo.EmpEngFullName;
                            row.EmpGender = empInfo.EmpGender;
                            row.EmpNo = empInfo.EmpNo;
                            row.EmpServiceDate = empInfo.EmpServiceDate;
                            row.EmpStatus = empInfo.EmpStatus;
                            ds.employeedetail.Rows.Add(row);
                        }

                    }
            }




            if (reportDocument == null)
            {
                reportDocument = new ReportTemplate.Report_Employee_BirthdayList();
            }
            else
            {

            }

            reportDocument.SetDataSource(ds);
            reportDocument.SetParameterValue("BirthdayFrom", BirthdayFrom.ToString("yyyy-MM-dd"));
            reportDocument.SetParameterValue("BirthdayTo", BirthdayTo.ToString("yyyy-MM-dd"));
            reportDocument.SetParameterValue("ShowDisplayOfBirth", ShowDisplayYearOfBirth);
            
            return reportDocument;

        }
    }

}
