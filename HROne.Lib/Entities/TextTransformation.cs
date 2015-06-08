using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("TextTransformation")]
    public class ETextTransformation : BaseObject
    {
        public static DBManager db = new DBManager(typeof(ETextTransformation));
        protected int m_TextTransformationID;
        [DBField("TextTransformationID", true, true), TextSearch, Export(false)]
        public int TextTransformationID
        {
            get { return m_TextTransformationID; }
            set { m_TextTransformationID = value; modify("TextTransformationID"); }
        }
        protected string m_TextTransformationOriginalString;
        [DBField("TextTransformationOriginalString"), MaxLength(100, 50), TextSearch, Export(false)]
        public string TextTransformationOriginalString
        {
            get { return m_TextTransformationOriginalString; }
            set { m_TextTransformationOriginalString = value; modify("TextTransformationOriginalString"); }
        }
        protected string m_TextTransformationReplacedTo;
        [DBField("TextTransformationReplacedTo"), MaxLength(100, 50), TextSearch, Export(false)]
        public string TextTransformationReplacedTo
        {
            get { return m_TextTransformationReplacedTo; }
            set { m_TextTransformationReplacedTo = value; modify("TextTransformationReplacedTo"); }
        }

        public string Replace(string originalString)
        {
            return originalString.Replace(m_TextTransformationOriginalString, m_TextTransformationReplacedTo);

            //string regExSearchPattern = ".*" + m_TextTransformationOriginalString.Replace(".","\\.") + ".*";
            //string regExReplacePattern = m_TextTransformationReplacedTo;
            //System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(regExSearchPattern);
            //if (regEx.IsMatch(originalString))
            //{
            //    string result = regEx.Replace(originalString, m_TextTransformationReplacedTo);
            //    return result;
            //}
            //return originalString;
        }

    }
}
