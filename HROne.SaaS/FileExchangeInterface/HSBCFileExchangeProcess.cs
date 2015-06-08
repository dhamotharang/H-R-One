using System;
using System.Collections.Generic;
using System.Text;

namespace HROne.SaaS.FileExchangeInterface
{
    public enum HSBCEnvironmentIndicatorEnum
    {
        Production,
        Testing
    }

    public enum HSBCFileIDEnum
    {
        AISTN = 60,
        AMPFF = 50,
        AMCND = 23,
        NWMBR = 20,
        AMAVC = 21,
        AMMBR = 22,
        AMPCR = 24,
        //ASTM,
        //ARET,
    }
    public class HSBCFileExchangeProcess
    {
        //protected Queue<string> DetailContents = new Queue<string>();
        protected long lineCount = 0;
        protected HSBCFileIDEnum m_FileID;
        public HSBCFileIDEnum FileID
        {
            get { return m_FileID; }
            set
            {
                m_FileID = value;
                if (value == HSBCFileIDEnum.AISTN || value == HSBCFileIDEnum.AMPFF)
                    RecordLength = 80;
                else if (value == HSBCFileIDEnum.AMCND)
                    RecordLength = 1500;
                else if (value == HSBCFileIDEnum.NWMBR)
                    RecordLength = 846;
                else if (value == HSBCFileIDEnum.AMAVC)
                    RecordLength = 148;
                else if (value == HSBCFileIDEnum.AMMBR)
                    RecordLength = 711;
                else if (value == HSBCFileIDEnum.AMPCR)
                    RecordLength = 926;
                m_FilenameInFile = m_FileID.ToString();
            }
        }
        protected string m_FilenameInFile ;
        public string FilenameInFile
        {
            get { return m_FilenameInFile; }
        }
        public string SendingSystem = string.Empty;
        public string ReceivingSystem = string.Empty;
        protected DateTime CreatedDate;
        public HSBCEnvironmentIndicatorEnum Environment = HSBCEnvironmentIndicatorEnum.Production;
        public int RecordLength = 80;
        System.IO.StreamWriter writer;
        System.IO.StreamReader reader;
        protected string NextLine = null;

        public HSBCFileExchangeProcess()
        {
            CreatedDate = AppUtils.ServerDateTime();
        }

        public void CreateWriter(string Filename)
        {
            if (RecordLength < 68)
                throw new Exception("Incorrect record length setting");
            SendingSystem = "HREX";
            ReceivingSystem = "APS";

            writer = new System.IO.StreamWriter(Filename);
            AddLine(getHeader());
            lineCount = 0;

        }
        public void CreateReader(string Filename)
        {
            NextLine = null;

            reader = new System.IO.StreamReader(Filename);
            if (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (line.StartsWith("HEADER "))
                {
                    RetrieveFromHeader(line);
                }
                else
                    throw new Exception("Incorrect Header mark");
            }
            lineCount = 0;
            if (!reader.EndOfStream)
            {
                NextLine = reader.ReadLine();
                if (NextLine.StartsWith("TRAILER"))
                {
                    RetrieveFromTrailer(NextLine);
                    NextLine = null;
                }
            }


        }
        public string ReadNextLine()
        {
            string returnValue = NextLine;
            if (!reader.EndOfStream)
            {
                NextLine = reader.ReadLine();
                if (NextLine.StartsWith("TRAILER"))
                {
                    RetrieveFromTrailer(NextLine);
                    NextLine = null;
                }
            }
            else
                NextLine = null;
            return returnValue;
        }
        public bool EndOfFlie()
        {
            return reader.EndOfStream || NextLine==null;
        }
        protected void RetrieveFromHeader(string line)
        {
            string[] headers = new string[10];
            headers[0] = line.Substring(0, 7).Trim();
            headers[1] = line.Substring(7, 23).Trim();
            headers[2] = line.Substring(30, 8).Trim();
            headers[3] = line.Substring(38, 4).Trim();
            headers[4] = line.Substring(42, 4).Trim();
            headers[5] = line.Substring(46, 8).Trim();
            headers[6] = line.Substring(54, 6).Trim();
            headers[7] = line.Substring(60, 1).Trim();

            m_FilenameInFile = headers[2];
            SendingSystem = headers[3];
            ReceivingSystem = headers[4];
            if (!DateTime.TryParseExact(headers[5] + headers[6], "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out CreatedDate))
                CreatedDate = AppUtils.ServerDateTime();
        }

        protected void RetrieveFromTrailer(string line)
        {
            //  for debug only, not in use

            string[] trailers = new string[10];
            trailers[0] = line.Substring(0, 7).Trim();
            trailers[1] = line.Substring(7, 23).Trim();
            trailers[2] = line.Substring(30, 8).Trim();
            trailers[3] = line.Substring(38, 4).Trim();
            trailers[4] = line.Substring(42, 4).Trim();
            trailers[5] = line.Substring(46, 7).Trim();
            trailers[6] = line.Substring(53, 15).Trim();
            //trailers[7] = line.Substring(68, 1).Trim();

            //FileID = (HSBCFileIDEnum)Enum.Parse(typeof(HSBCFileIDEnum), headers[2]);
            //SendingSystem = headers[3];
            //ReceivingSystem = headers[4];
        }

        public void AddLine(string Content)
        {
            if (RecordLength != Content.Length)
                throw new Exception("Incorrect record length: " + Content);
            writer.WriteLine(Content);
            lineCount++;
        }

        public void Close()
        {
            if (writer != null)
            {
                AddLine(getTrailer());
                writer.Close();
            }
            if (reader != null)
                reader.Close();
            writer = null;
            reader = null;
        }

        protected string getHeader()
        {
            string[] headers = new string[10];
            headers[0] = "HEADER".PadRight(7);
            headers[1] = string.Empty.PadRight(23);
            headers[2] = FileID.ToString().PadRight(8);
            headers[3] = SendingSystem.PadRight(4);
            headers[4] = ReceivingSystem.PadRight(4);
            headers[5] = CreatedDate.ToString("yyyyMMdd");
            headers[6] = CreatedDate.ToString("HHmmss");
            if (Environment == HSBCEnvironmentIndicatorEnum.Testing)
                headers[7] = "T";
            else
                headers[7] = "P";
            headers[8] = " ";
            headers[9] = string.Empty.PadRight(RecordLength - 62);

            return string.Join(string.Empty, headers);
        }

        protected string getTrailer()
        {
            string[] trailers = new string[10];
            trailers[0] = "TRAILER".PadRight(7);
            trailers[1] = string.Empty.PadRight(23);
            trailers[2] = FileID.ToString().PadRight(8);
            trailers[3] = SendingSystem.PadRight(4);
            trailers[4] = ReceivingSystem.PadRight(4);
            trailers[5] = lineCount.ToString("0000000");
            trailers[6] = string.Empty.PadRight(15, '0');
            if (RecordLength > 68)
            {
                trailers[7] = "+";
                trailers[8] = string.Empty.PadRight(RecordLength - 69);
            }
            return string.Join(string.Empty, trailers);
        }
    }
}
