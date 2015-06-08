//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Data;
//using System.IO;
//using Microsoft.Office.Interop.Word;
//using Word = Microsoft.Office.Interop.Word;


//namespace HROne.Lib
//{
//    public class MailMergeService
//    {
//        public MailMergeService()
//        {
//        }

//        public static bool MergeDataWithWordTemplate(string sourceTemplatePath, string outputDocPath, System.Data.DataTable sourceData, out string errorString)
//        {
//            #region Declares
//            errorString = "";
//            Object oMissing = System.Reflection.Missing.Value; //null value 
//            Object oZero = (object) 0;
//            Object oOne = (object)1;
//            Object oTrue = true;
//            Object oFalse = false;
//            Object oTemplatePath = sourceTemplatePath;
//            Object oOutputPath = outputDocPath;
//            Object oOutputPathTemp = outputDocPath.Substring(0, outputDocPath.IndexOf(".doc")) + "_temp.doc";
//            Object sectionStart = (Object)Microsoft.Office.Interop.Word.WdSectionStart.wdSectionNewPage;

//            Word.Application oWord = null;
//            Word.Document oWordDoc = null; //the document to load into word application 
//            Word.Document oFinalWordDoc = null; //the document to load into word application 

//            string csvFile="";

//            #endregion

//            try
//            {
//                csvFile = System.IO.Path.GetTempFileName() + ".csv";

//                DataTableToCSV(sourceData, csvFile, out errorString);

//                oWord = new Word.Application(); //starts an instance of word 
//                oWord.Visible = false; //don't show the UI 

//                //oWordDoc = oWord.Documents.Add(ref oTemplatePath, ref oMissing, ref oMissing, ref oMissing);
//                oWordDoc = oWord.Documents.Open(ref oTemplatePath, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, 
//                                                               ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
//                                                               ref oMissing, ref oMissing, ref oMissing);
//                oWordDoc.MailMerge.OpenDataSource(csvFile, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
//                                                           ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
//                                                           ref oMissing);

//                oWordDoc.MailMerge.Destination = Word.WdMailMergeDestination.wdSendToNewDocument;
//                //oWordDoc.MailMerge.SuppressBlankLines = true;
//                oWordDoc.MailMerge.DataSource.FirstRecord = (int)Word.WdMailMergeDefaultRecord.wdDefaultFirstRecord;
//                oWordDoc.MailMerge.DataSource.LastRecord = (int)Word.WdMailMergeDefaultRecord.wdDefaultLastRecord;
//                oWordDoc.MailMerge.Execute(ref oFalse);
//                oWordDoc.Close(ref oFalse, ref oMissing, ref oMissing);
                
//                if (oWord.Documents.Count > 0)
//                {
//                    oFinalWordDoc = oWord.Documents.get_Item(ref oOne);
//                    oFinalWordDoc.SaveAs(ref oOutputPath, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
//                                                                                ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

//                    oFinalWordDoc.Close(ref oFalse, ref oMissing, ref oMissing);
//                }
//                oWord.Quit(ref oFalse, ref oMissing, ref oMissing);

//            }
//            catch (System.Exception ex)
//            {
//                errorString = ex.Message;
//                return false;
//            }
//            return true;
//        }



//        protected static bool DataTableToCSV(System.Data.DataTable pSourceData, string pCsvPath, out string pErrorString)
//        {
//            pErrorString = "";

//            try{

//                System.IO.File.Delete(pCsvPath);

//                StringBuilder sb = new StringBuilder(); 

//                foreach(DataColumn m_col in pSourceData.Columns)
//                {
//                    sb.Append(m_col.ToString() + ",");
//                }
//                sb.Replace(",", System.Environment.NewLine, sb.Length - 1, 1);

//                foreach (DataRow m_row in pSourceData.Rows)
//                {
//                    foreach (DataColumn m_col in pSourceData.Columns) 
//                    {
//                        sb.Append("\"" + m_row[m_col.ToString()].ToString() + "\",");
//                    }                
//                    sb.Replace(",", System.Environment.NewLine, sb.Length - 1, 1);
//                }

//                System.IO.File.WriteAllText(pCsvPath, sb.ToString());
//            }
//            catch (System.Exception ex)
//            {
//                pErrorString = ex.Message;
//                return false;
//            }
//            return true;
//        }
//    }
//}
