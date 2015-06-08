using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
namespace HROne.DataAccess
{
    public class DBField
    {
        //public DBColumnAttribute column;
        public string columnName
        {
            get { return dbFieldattribute.columnName; }
        }
        public bool datesearch;
        //public object dbType;
        public string excelFormat
        {
            get { return dbFieldattribute.excelFormat; }
        }
        //public DBExchangeAttribute exchange;
        //public ExportAttribute export;
        public string format
        {
            get { return dbFieldattribute.format; }
        }
        public bool isAuto
        {
            get { return dbFieldattribute.isAuto; }
        }
        public bool isKey
        {
            get { return dbFieldattribute.isKey; }
        }
        //public ListOutputAttribute listOutput;
        public string name
        {
            get { return property.Name; }
        }
        public PropertyInfo property
        {
            get { return propInfo; }
        }

        protected bool m_required = false;
        public bool required
        {
            get { return m_required; }
        }
        //public bool system;
        public bool textsearch
        {
            get { return dbFieldattribute.textsearch; }
        }

        private DBFieldTranscoder m_transcoder = null;
        public DBFieldTranscoder transcoder
        {
            get { return m_transcoder; }
        }

        private DBFieldAttribute dbFieldattribute;
        private PropertyInfo propInfo;
        private List<ValidationAttribute> validationAttributeList;
        public DBField(DBFieldAttribute dbFieldattribute, PropertyInfo propInfo)
        {
            this.dbFieldattribute = dbFieldattribute;
            this.propInfo = propInfo;

            if (propInfo.PropertyType == typeof(DateTime) && string.IsNullOrEmpty(dbFieldattribute.format))
            {
                //  Set default DateTime format to "yyyy-MM-dd"
                dbFieldattribute.format = "yyyy-MM-dd";
            }

            this.validationAttributeList = new List<ValidationAttribute>();

            bool hasDataTypeValidationAttribute = false;


            foreach (Attribute a in propInfo.GetCustomAttributes(false))
            {
                if (a is DBFieldTranscoder)
                    if (m_transcoder == null)
                        m_transcoder = (DBFieldTranscoder)a;
                    else
                        throw new Exception("Only 1 transcoder attribute is allowed");
                if (a is RequiredAttribute)
                    m_required = true;
                if (a is ValidationAttribute)
                {
                    validationAttributeList.Add((ValidationAttribute)a);
                    if (a is DoubleAttribute && propInfo.PropertyType == typeof(double))
                        hasDataTypeValidationAttribute = true;
                    if (a is IntAttribute && propInfo.PropertyType == typeof(int))
                        hasDataTypeValidationAttribute = true;
                }
            }
            if (!hasDataTypeValidationAttribute)
                if (propInfo.PropertyType == typeof(double))
                    validationAttributeList.Add(new DoubleAttribute());
                else if (propInfo.PropertyType == typeof(int))
                    validationAttributeList.Add(new IntAttribute());

        }
        public List<ValidationAttribute> validations
        {
            get { return validationAttributeList; }
        }
        public Attribute findAttribute(Type attributeType)
        {
            object[] attributeList = propInfo.GetCustomAttributes(attributeType, false);
            if (attributeList.GetLength(0) > 0)
                return (Attribute)attributeList[0];
            return null;
        }
        public object convert(object from)
        {
            if (Convert.IsDBNull(from))
                return null;

            object result = from;

            if (transcoder != null)
                result = transcoder.fromDB(result);

            if (result.GetType() == property.PropertyType)
                return result;
            if (property.PropertyType == typeof(double) && result is float)
                return Convert.ToDouble(result.ToString());
            if (property.PropertyType.IsSubclassOf(typeof(Enum)))
            {
                return Enum.ToObject(property.PropertyType, result);
            }
            return Convert.ChangeType(result, property.PropertyType);
        }
        //public ValidationAttribute findValidation(Type t);
        public object getValue(object obj)
        {
            return propInfo.GetValue(obj, null);
        }
        public object parseValue(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                if (property.PropertyType == typeof(string))
                    return string.Empty;
                else
                    return null;
            }
            if (property.PropertyType == typeof(bool))
            {
                return s.Equals("1") || s.Equals("-1") || s.Equals("true", StringComparison.CurrentCultureIgnoreCase) || s.Equals("yes", StringComparison.CurrentCultureIgnoreCase);
            }
            else if (property.PropertyType==typeof(DateTime)) 
			{
                s = s.Trim();
                if (s.Equals(string.Empty))
                    return null;
                else
                {
                    if (!string.IsNullOrEmpty(format))
                        return DateTime.Parse(s);
                    else
                        return DateTime.ParseExact(s, format, System.Globalization.CultureInfo.CurrentUICulture);
                }
			}
            return Convert.ChangeType(s, property.PropertyType);
        }
        public string populate(DBObject o)
        {
            object v = property.GetValue(o, null);
            return populateValue(v);
        }
        public string populateValue(object v)
        {
            if (v == null)
                return string.Empty;

            if (string.IsNullOrEmpty(format))
                return v.ToString();

            if (v is double)
            {
                double values = (double)v;
                if (values.Equals(double.NaN))
                    return string.Empty;
                return values.ToString(format);
            }
            else if (v is DateTime)
            {
                DateTime dateTimeValue = (DateTime)v;
                if (dateTimeValue.Ticks.Equals(0))
                    return string.Empty;
                return dateTimeValue.ToString(format);
            }
            else if (v is int)
            {
                int values = (int)v;
                if (!string.IsNullOrEmpty(format))
                    return values.ToString(format);
                return values.ToString(format);
            }
            else if (v is float)
            {
                float values = (float)v;
                if (v.Equals(float.NaN))
                    return string.Empty;
                return values.ToString(format);
            }
            else if (v is long)
            {
                long values = (long)v;
                if (v.Equals(float.NaN))
                    return string.Empty;
                return values.ToString(format);
            }
            return v.ToString();
        }
        public void setValue(object obj, object val)
        {
            propInfo.SetValue(obj, val, null);
        }
    }
}
