using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;

namespace HROne.SaaS.FileExchangeInterface
{
    public class HSBCFileReportProcess
    {
        protected DatabaseConnection dbConn;
        HROne.SaaS.FileExchangeInterface.HSBCFileExchangeProcess tmpProcess = new HROne.SaaS.FileExchangeInterface.HSBCFileExchangeProcess();

        protected string m_FilenameInFile;
        protected string lastLine = string.Empty;

        public string FilenameInFile
        {
            get { return m_FilenameInFile; }
        }

        public HSBCFileReportProcess(DatabaseConnection dbConn)
        {
            this.dbConn = dbConn;

        }

        public void Load(string filename)
        {
            tmpProcess.CreateReader(filename);
            lastLine = string.Empty;
            m_FilenameInFile = tmpProcess.FilenameInFile;
        }
        public void Close()
        {
            tmpProcess.Close();
        }
        public HSBCFileReport getNextFileReportObject()
        {
            if (string.IsNullOrEmpty(lastLine))
                lastLine = tmpProcess.ReadNextLine();
            if (!string.IsNullOrEmpty(lastLine))
            {
                lastLine = lastLine.Replace(Convert.ToChar(0x0).ToString(), " ");
                if (lastLine.StartsWith("S"))
                {
                    HSBCFileReport fileReport = new HSBCFileReport(lastLine.PadRight(19).Substring(1, 18).Trim());
                    while (!tmpProcess.EndOfFlie())
                    {
                        lastLine = tmpProcess.ReadNextLine();
                        if (!string.IsNullOrEmpty(lastLine))
                        {
                            lastLine = lastLine.Replace(Convert.ToChar(0x0).ToString(), " ");
                            if (lastLine.StartsWith("S"))
                                break;
                            fileReport.AddLine(lastLine);
                        }
                    }

                    return fileReport;
                }
            }
            return null;
        }
    }

    public class HSBCFileReport
    {
        protected internal string m_RemoteProfileID;
        List<List<string>> reportPages = new List<List<string>>();
        List<string> currentPage;

        public string RemoteProfileID
        {
            get { return m_RemoteProfileID; }
        }
        protected internal HSBCFileReport(string RemoteProfileID)
        {
            m_RemoteProfileID = RemoteProfileID;
        }
        public void AddLine(string line)
        {
            //ASA Character	    Action	                            ASCII Equivalent
            //1	                Advance to next page (form feed)	CR FF
            //2¡V9, A, B, C	    Advance to vertical tab stop	    CR VT (approximately)
            //blank	            Advance 1 line (single spacing)	    CR LF
            //0	                Advance 2 lines (double spacing)	CR LF LF
            //-	                Advance 3 lines (triple spacing)	CR LF LF LF
            //+	                Do not advance any lines before printing, overstrike the previous line	CR
            if (!string.IsNullOrEmpty(line))
            {
                char ControlChar = line[0];
                string content = line.Substring(1);

                if (ControlChar == '1')
                {
                    currentPage = new List<string>();
                    reportPages.Add(currentPage);
                }
                else if (ControlChar == '0')
                {
                    currentPage.Add(string.Empty);
                }
                else if (ControlChar == '-')
                {
                    currentPage.Add(string.Empty);
                    currentPage.Add(string.Empty);
                }

                currentPage.Add(content);
            }
        }
        public void Export(string Filename)
        {
            //System.IO.StreamWriter writer = new System.IO.StreamWriter(Filename);
            //foreach (string line in reportLines)
            //    writer.WriteLine(line);
            //writer.Close();
            ReportTemplate.GenericHSBCReportDataSet dataSet = new HROne.SaaS.ReportTemplate.GenericHSBCReportDataSet();
            int pageCount = 0;
            foreach (List<string> reportLines in reportPages)
            {
                pageCount++;
                int lineCount = 0;
                foreach (string line in reportLines)
                {
                    lineCount++;
                    ReportTemplate.GenericHSBCReportDataSet.ReportRow row = dataSet.Report.NewReportRow();
                    string content = string.Empty;
                    if (line.Length>10)
                        content = line.Substring(10);
                    if (content.Length > 10)
                        content = content.Substring(0, content.Length - 10);
                    row.Content = content;
                    row.LineNo = lineCount;
                    row.PageNo = pageCount;
                    dataSet.Report.Rows.Add(row);
                }
            }
            ReportTemplate.GenericHSBCReport rpt = new HROne.SaaS.ReportTemplate.GenericHSBCReport();
            rpt.SetDataSource(dataSet);
            rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Filename);

        }
    }
}
