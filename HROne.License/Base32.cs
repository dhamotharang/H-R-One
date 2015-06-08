using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace HROne.License
{
    public class Base32
    {
        private static char[] b32cd;

        /// <summary>
        /// initilises the characters in base 32
        /// </summary>
        protected static void initbase32()
        {
            b32cd = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'T', 'U', 'V', 'W', 'X', 'Y', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        }

        public static string ConvertBase64ToBase32(string source)
        {
            initbase32();
            byte[] byteArray = Convert.FromBase64String(source);
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
