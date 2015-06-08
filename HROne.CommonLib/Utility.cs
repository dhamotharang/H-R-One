using System;
using System.Collections;
using System.Text;
using System.Xml;


namespace HROne.CommonLib
{
    public abstract class Utility
    {
        public static DateTime FirstDateOfMonth(DateTime asOfDate)
        {
            return new DateTime(asOfDate.Year, asOfDate.Month, 1);
        }

        public static DateTime LastDateOfMonth(DateTime asOfDate)
        {
            return FirstDateOfMonth(asOfDate).AddMonths(1).AddDays(-1);
        }

        public static int GetNumberOfDayPerMonth(DateTime AsOfDate)
        {
            DateTime firstDateOfMonth = new DateTime(AsOfDate.Year, AsOfDate.Month, 1);
            return firstDateOfMonth.AddMonths(1).Subtract(firstDateOfMonth).Days;

        }

        public static double MonthDifference(DateTime FromDate, DateTime ToDate)
        {
            ToDate = ToDate.AddDays(1);
            int NoOfDay = 0;
            int NoOfMonth = 0;
            while (FromDate < ToDate.AddMonths(-NoOfMonth).AddDays(1))
                NoOfMonth++;
            NoOfMonth--;
            NoOfDay = ((TimeSpan)ToDate.AddMonths(-NoOfMonth).Subtract(FromDate)).Days;
            int numberOfDayPerYear = GetNumberOfDayPerMonth(FromDate);

            return (NoOfMonth + (double)NoOfDay / numberOfDayPerYear);
        }

        public static double YearDifference(DateTime FromDate, DateTime ToDate)
        {
            ToDate = ToDate.AddDays(1);
            DateTime tmpYearOfService = FromDate;
            DateTime AsOfDate = ToDate;
            int intYearDiff;
            intYearDiff = AsOfDate.Year - tmpYearOfService.Year;
            if (tmpYearOfService.AddYears(intYearDiff) > AsOfDate)
                intYearDiff--;
            int intDaysDiff = AsOfDate.Subtract(tmpYearOfService.AddYears(intYearDiff)).Days;
            int intTotalDaysPerYear = tmpYearOfService.AddYears(intYearDiff + 1).Subtract(tmpYearOfService.AddYears(intYearDiff)).Days;

            return (double)intYearDiff + (double)intDaysDiff / (double)intTotalDaysPerYear;

        }

        public static XmlDocument GetXmlDocumentByDataString(string xmlData)
        {
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            if (!string.IsNullOrEmpty(xmlData))
            {
                xmlDoc.LoadXml(xmlData);
            }
            return xmlDoc;
        }

        public static string GetXMLElementFromXmlString(string XmlSourceString, string rootNodeName, string xmlNodeName)
        {
            XmlDocument document = HROne.CommonLib.Utility.GetXmlDocumentByDataString(XmlSourceString);
            if (document == null)
                return string.Empty;
            XmlElement rootElement = document.DocumentElement;
            if (rootElement == null)
                return string.Empty;
            if (rootElement.Name.Equals(rootNodeName, StringComparison.CurrentCultureIgnoreCase))
            {
                System.Xml.XmlNodeList nodeList = document.GetElementsByTagName(xmlNodeName);
                if (nodeList.Count > 0)
                    return nodeList[0].InnerText;
            }
            return string.Empty;
        }

        public static string SetXmlElementFromXmlString(string XmlSourceString, string rootNodeName, string xmlNodeName, string InnerTextValue)
        {
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            if (string.IsNullOrEmpty(XmlSourceString))
            {
                System.Xml.XmlElement rootNode = xmlDoc.CreateElement(rootNodeName);
                xmlDoc.AppendChild(rootNode);
                XmlSourceString = xmlDoc.InnerXml;
            }
            else
                xmlDoc.LoadXml(XmlSourceString);


            System.Xml.XmlElement xmlElement = xmlDoc.CreateElement(xmlNodeName);
            xmlElement.InnerText = InnerTextValue;
            xmlDoc.DocumentElement.AppendChild(xmlElement);
            XmlSourceString = xmlDoc.InnerXml;

            return XmlSourceString;
        }

        public static object GetNewType(string className)
        {
            Type type = Type.GetType(className, true);
            object newInstance = Activator.CreateInstance(type);
            return newInstance;
        } 
    }

    public abstract class base32
    {
        private static char[] b32cd;

        /// <summary>
        /// initilises the characters in base 32
        /// </summary>
        protected static void initbase32()
        {
            b32cd = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'T', 'U', 'V', 'W', 'X', 'Y', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        }

        public static string ConvertToBase32(byte[] byteArray)
        {
            initbase32();

            byte remainder = 0;
            ArrayList list = new System.Collections.ArrayList();
            string resultString = string.Empty;
            int shift = 0;
            foreach (byte byteData in byteArray)
            {
                shift += 8;
                int newValue = byteData;
                while (shift >= 5)
                {
                    newValue = (int)(newValue + remainder * 256);
                    shift -= 5;
                    int divident = (int)(Math.Pow(2, shift));
                    byte newRemainder = (byte)(newValue % divident);
                    byte result = (byte)(newValue / divident);
                    resultString += b32cd[result];
                    newValue = newRemainder;
                    remainder = 0;
                }
                remainder = (byte)newValue;
                //if (result >= b32cd.Length)
                //    resultString += b32cd[result % b32cd.Length] + b32cd[result / b32cd.Length];
                //else
            }

            if (shift != 0)
                resultString += b32cd[remainder * (int)Math.Pow(2, 5 - shift)];

            return resultString;

        }

        public static string ConvertBase64ToBase32(string source)
        {
            byte[] byteArray = Convert.FromBase64String(source);
            return ConvertToBase32(byteArray);
        }

        public static string ConvertBase32ToBase64(string source)
        {
            initbase32();

            char[] charArray = source.ToCharArray();
            ArrayList byteArray = new ArrayList();
            int shift = 0;
            int result = 0;
            for (int i = 0; i < charArray.Length; i++)
            {
                shift += 5;
                // find the index in the array that the char resides
                int valueindex = Array.IndexOf(b32cd, charArray[i]);
                result = result * 32 + valueindex;
                if (shift >= 8)
                {
                    shift -= 8;
                    int divident = (int)Math.Pow(2, shift);
                    byte value = (byte)(result / divident);
                    result = result % divident;
                    byteArray.Add(value);
                }
            }
            if (shift != 0 && result != 0)
            {
                byteArray.Add((byte)(result * (int)Math.Pow(2, 5 - shift)));

            }
            return Convert.ToBase64String((byte[])byteArray.ToArray(typeof(byte)));
        }

        /// <summary>
        /// Encodes an int into a base 36 string
        /// if you were to alter the values in the b36cd then 
        /// this could be to any base value.
        /// </summary>
        /// <param name="value">(int) the input decimal value</param>
        /// <returns>(string) the output string base 36 converter</returns>
        public static string encodeb32(Int64 value, int numberOfDigit)
        {
            initbase32();   // set the char[] array
            string rv = ""; // starting value
            while (value != 0)
            {
                rv = b32cd[value % b32cd.Length] + rv;
                value /= b32cd.Length;
            }
            rv = rv.ToUpper();
            rv = rv.PadLeft(numberOfDigit, b32cd[0]);

            return rv;
        }


        /// <summary>
        /// This decodes the base 36 number into a decimal number
        /// though this could be used to decode any base number depending on the input
        /// on the char[] b36cd
        /// </summary>
        /// <param name="input"></param>
        /// <returns>(int) the decimal value of the base 36 number</returns>
        public static Int64 base32decode(string input)
        {
            initbase32();
            input = input.Trim();
            input = input.ToLower();
            Int64 rv = 0;
            // break string into characters
            char[] encchars = input.ToCharArray();
            // reverse the array
            Array.Reverse(encchars);
            // loop through the values
            for (int i = 0; i < encchars.Length; i++)
            {
                // find the index in the array that the char resides
                int valueindex = Array.IndexOf(b32cd, encchars[i]);
                // the actual value given by that is 
                // the index multiplied by the base number to the power of the index
                double temp = valueindex * Math.Pow(b32cd.Length, i);
                // add this value to the counter until there are no more values
                rv += Convert.ToInt64(temp);
            }
            // return the total result
            return rv;
        }

        public static string ReverseEncodeString(string base36string)
        {
            char[] encchars = base36string.ToLower().ToCharArray();
            for (int i = encchars.GetLowerBound(0); i <= encchars.GetUpperBound(0); i++)
                encchars[i] = b32cd[b32cd.GetUpperBound(0) - b32cd.GetLowerBound(0) - Array.IndexOf(b32cd, encchars[i]) - 1];
            return new string(encchars);
        }

        public static char CheckSum(string base36string)
        {
            char[] encchars = base36string.ToLower().ToCharArray();
            int checkSum = 0;
            for (int i = encchars.GetLowerBound(0); i <= encchars.GetUpperBound(0); i++)
            {
                checkSum += Array.IndexOf(b32cd, encchars[i]) - 1;
                checkSum %= encchars.Length;
            }
            return encchars[checkSum];
        }

    }

}
