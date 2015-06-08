using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
using HROne.Common;

namespace HROne.Lib.Entities
{
    [DBClass("ReportBuilder")]
    public class EReportBuilder : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EReportBuilder));

        protected int m_ReportBuilderID;
        [DBField("ReportBuilderID", true, true), TextSearch, Export(false)]
        public int ReportBuilderID
        {
            get { return m_ReportBuilderID; }
            set { m_ReportBuilderID = value; modify("ReportBuilderID"); }
        }

        // Start 0000167, Miranda, 2015-03-09
        protected String m_ReportModule;
        [DBField("ReportModule"), TextSearch, Export(false)]
        public String ReportModule
        {
            get { return m_ReportModule; }
            set { m_ReportModule = value; modify("ReportModule"); }
        }

        protected String m_SelectedFieldNames;
        [DBField("SelectedFieldNames"), TextSearch, Export(false)]
        public String SelectedFieldNames
        {
            get { return m_SelectedFieldNames; }
            set { m_SelectedFieldNames = value; modify("SelectedFieldNames"); }
        }
        // End 0000167, Miranda, 2015-03-09

        protected String m_DBFilterExpressions;
        [DBField("DBFilterExpressions"), TextSearch, Export(false)]
        public String DBFilterExpressions
        {
            get { return m_DBFilterExpressions; }
            set { m_DBFilterExpressions = value; modify("DBFilterExpressions"); }
        }

        protected String m_ReportName;
        [DBField("ReportName"), TextSearch, Export(false)]
        public String ReportName
        {
            get { return m_ReportName; }
            set { m_ReportName = value; modify("ReportName"); }
        }
    }
}