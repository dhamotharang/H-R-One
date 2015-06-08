using System;
using System.Data;
using System.Configuration;

using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    /// <summary>
    /// Summary description for TaxCompmentMap
    /// </summary>
    [DBClass("TaxCompanyMap")]
    public class ETaxCompanyMap : BaseObject 
    {
        public static DBManager db = new DBManager(typeof(ETaxCompanyMap));

        protected int m_TaxCompMapID;
        [DBField("TaxCompMapID", true, true), TextSearch, Export(false)]
        public int TaxCompMapID
        {
            get { return m_TaxCompMapID; }
            set { m_TaxCompMapID = value; modify("TaxCompMapID"); }
        }

        protected int m_TaxCompID;
        [DBField("TaxCompID"), TextSearch, Export(false)]
        public int TaxCompID
        {
            get { return m_TaxCompID; }
            set { m_TaxCompID = value; modify("TaxCompID"); }
        }

        protected int m_CompanyID;
        [DBField("CompanyID"), TextSearch, Export(false)]
        public int CompanyID
        {
            get { return m_CompanyID; }
            set { m_CompanyID = value; modify("CompanyID"); }
        }
    }
}