using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
namespace HROne.DataAccess
{
    public class HTMLUtils
    {
        //public static string toHTML(string v)
        //{
        //    v = v.Trim();
        //    v = v.Replace("\"", "&quot;");
        //    v = v.Replace(@"<", @"&lt;");
        //    v = v.Replace(@">", @"&gt;");
        //    return v;
        //}
        public static string toHTMLText(string v)
        {
            v = v.Trim();
            v = HttpUtility.HtmlEncode(v);
            v = v.Replace("\n", @"<BR>");
            return v;
        }
        //public static string toExcelHTMLText(string v)
        //{
        //    v = v.Trim();
        //    v = v.Replace(" ", "&nbsp;");
        //    v = v.Replace("\"", "&quot;");
        //    v = v.Replace(@"<", @"&lt;");
        //    v = v.Replace(@">", @"&gt;");
        //    v = v.Replace("\n", @"<BR>");
        //    return v;
        //}
        //public static string toHTML(string v, int maxRow)
        //{

        //    v = v.Trim();
        //    if (v.Length == 0)
        //        return "";

        //    StringBuilder b = new StringBuilder();
        //    String[] str = v.Split(new char[] { '\n' });
        //    for (int i = 0; i != str.Length; i++)
        //    {
        //        while (true)
        //        {
        //            if (str[i].Length > maxRow)
        //            {
        //                b.Append(toHTMLText(str[i].Substring(0, maxRow)));
        //                str[i] = str[i].Substring(maxRow);
        //                b.Append("\n");
        //            }
        //            else
        //            {
        //                b.Append(toHTMLText(str[i]));
        //                if (i < str.Length - 1)
        //                    b.Append("\n");
        //                break;
        //            }
        //        }
        //    }
        //    return b.ToString();
        //}
    }
}
