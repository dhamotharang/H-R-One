//using System;
//using System.Data;
//using System.Configuration;
//using System.Globalization;
//using HROne.DataAccess;


//namespace HROne.Translation
//{

//    //public class MessageCreater
//    //{
//    //    private CultureInfo ci;

//    //    public MessageCreater(CultureInfo ci)
//    //    {
//    //        this.ci = ci;
//    //    }

//    //    public MessageCreater()
//    //    {
//    //        this.ci = CultureInfo.CurrentCulture;
//    //    }

//    //    public CultureInfo CurrentCultureInfo
//    //    {
//    //        get { return ci; }
//    //        set { ci = value; }
//    //    }

//    //    public string GetStringByCode(string code, string defaultString)
//    //    {
//    //        string result = GetString(code);
//    //        if (code != result)
//    //            return result;
//    //        return GetString(defaultString);

//    //    }

//    //    public string GetString(string originalString)
//    //    {
//    //        System.Resources.ResourceManager rm = AppUtils.getResourceManager();

//    //        string newString = rm.GetString(originalString, ci);
//    //        if (string.IsNullOrEmpty(newString))
//    //            return originalString;
//    //        else
//    //            return newString;

//    //    }

//    //}

//    public abstract class MessageBuilder
//    {
//        public static string MessageWithParameter(string message, string[] parameters)
//        {
//            int parameterCount = parameters.GetLength(0);
//            for (int i = parameters.GetUpperBound(0); i >= parameters.GetLowerBound(0); i--)
//            {
//                string parameterName = "%" + parameterCount.ToString().Trim();
//                message = message.Replace(parameterName, parameters[i]);
//                parameterCount--;
//            }
//            return message;
//        }
//    }
//}