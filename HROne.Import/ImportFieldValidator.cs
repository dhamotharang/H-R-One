using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace HROne.Import
{
    public class ImportFieldValidator
    {
        public enum EnumCheckOptions
        {
            Required = 1,
            Numeric = 2,
            Date = 4,
            DateTime = 8,
            Decimal = 16
        }


        protected string m_fieldName;
        protected EnumCheckOptions m_checkOptions;
        protected string m_errorMessage;

        #region "GetterSetter"
        public string FieldName
        {
            get { return m_fieldName; }
            set { m_fieldName = value; }
        }

        public EnumCheckOptions CheckOptions
        {
            get { return m_checkOptions; }
            set { m_checkOptions = value; }
        }
        public string ErrorMessage
        {
            get { return m_errorMessage; }
            set { m_errorMessage = value; }
        }
        #endregion "GetterSetter"

        public ImportFieldValidator(string fieldName, string error, EnumCheckOptions checkOptions)
        {
            m_fieldName = fieldName;
            m_checkOptions = checkOptions;
            m_errorMessage = error; 
        }

        public bool validate(DataRow inputRow, out string outError, out string outColName)
        {
            string m_rawData;
            int m_tmpInt;
            Decimal m_tmpDecimal;
            DateTime m_tmpDateTime;

            outError = m_errorMessage;
            outColName = m_fieldName;

            try {
                m_rawData = inputRow[m_fieldName].ToString();
            }
            catch (Exception)
            {
                outError = ImportErrorMessages.ERROR_INVALID_COLUMN;
                return false;
            }

            if ((m_checkOptions & EnumCheckOptions.Required) == EnumCheckOptions.Required)
            {
                return (m_rawData.Trim().Length > 0);
            }
            else if ((m_checkOptions & EnumCheckOptions.Numeric) == EnumCheckOptions.Numeric)
            {
                return (!int.TryParse(m_rawData.Trim(), out m_tmpInt));
            }
            else if ((m_checkOptions & EnumCheckOptions.Decimal) == EnumCheckOptions.Decimal)
            {
                return (!Decimal.TryParse(m_rawData.Trim(), out m_tmpDecimal));
            }
            else if ((m_checkOptions & EnumCheckOptions.Date) == EnumCheckOptions.Date)
            {
                return (!DateTime.TryParse(m_rawData.Trim(), out m_tmpDateTime));
            }
            else if ((m_checkOptions & EnumCheckOptions.DateTime) == EnumCheckOptions.DateTime)
            {
                return (!DateTime.TryParse(m_rawData.Trim(), out m_tmpDateTime));
            }

            return true; 

        }
    }


}
