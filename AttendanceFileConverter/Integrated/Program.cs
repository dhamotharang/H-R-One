using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


//AGMS/TGMS 6.0 access records's format

//Output Text File file structure

//String      : 0        1         2         3         4
//Position    : 12345678901234567890123456789012345678901234
//Indicator   : AAAAAAAABBBBBBBBBBCCCCDDEEFF GG HHIIIJJJKKLM
//Example     : 00017389A950906   1997010100:08:0300100101 B

//Position        Indicator       Content         Data Format
//01-08           A               Card No.        Chr. (0-9)      eg. 00012345
//09-18           B               Employee No.    Chr.            eg. ABC1234567
//19-22           C               Year            Chr. (0-9)      eg. 1997,2001
//23-24           D               Month           Chr. (0-9)      eg. 01-12
//25-26           E               Day of Month    Chr. (0-9)      eg. 01-31
//27-28           F               Hour            Chr. (0-9)      eg. 00-23
//30-31           G               Minute          Chr. (0-9)      eg. 00-59
//33-34           H               Second          Chr. (0-9)      eg. 00-59
//35-37           I               Link No.        Chr.            eg. Aa9,-
//38-40           J               Controller ID.  Chr. (0-9)      eg. 001-128
//41-42           K               Reader ID.      Chr. (0-9)      eg. 01-04
//43-43           L               Input Method    Chr.            refer to Notes
//44-44           M               Record Status   Chr.            refer to Notes

//Notes :
//1. Input Method
//    For Access Control Module
//    L = " " : Wipe Card Records
//    L = "M" : Direct Input
//    L = "B" : Manual Key In Card No. & Password from Reader
//    In/Out Status (For Time Clock Unit)
//    L = "1" to "7" can be defined by user.
//    Before wipe card, press *1,...*7, Time Clock Unit will 
//    display the specified message on display panel. Futher calculation
//    according to this situation shall be done by developer.

//2. Record Status	
//    M = "A" : Valid Entry Record
//    M = "B" : No Access Rights (Card Holder not found in controllers)
//    M = "C" : Invalid Password
//    M = "D" : Invalid Time Zone
//    M = "E" : Card Holder expired (check from Database File)
//    M = "F" : Card Holder Not Found (check from Database File)
//    M = "G" : Manual Open
//    M = "I" : Card holder deleted caused by invalid password
//    M = "X" : Computer open door

//    For Time Clock System, just select the M = "A" record

//3. Card No = "00000000" means Manual Open Door Record is equal to M = "G"



namespace FixLen2Delimited
{
    class Program
    {
        static public int[] FIELD_LENGTH_LIST = new int[] { 8, 10, 8, 8, 3, 3, 2, 1, 1 };
        static public int KEY_FIELD_NO = 9;
        static public string[] KEY_FILTER_LIST = new string[] { "A" };
        static public string DELIMITER = ",";

        static public int field_start(int fieldNo)
        {
            int m_fieldStart=0;

            for (int i = 0; i < fieldNo - 1; i++)
            {
                m_fieldStart += (int)FIELD_LENGTH_LIST.GetValue(i);
            }

            return m_fieldStart; 
        }

        static public int field_length(int fieldNo)
        {
            if (fieldNo <= FIELD_LENGTH_LIST.Length)
            {
                return FIELD_LENGTH_LIST[fieldNo-1];
            }
            return -1;
        }

        static void Main(string[] args)
        {
            if (args == null || args.Length != 2)
            {
                Console.WriteLine("Usage: Integrated.exe <source-data-file> <target-data-file-import>");
            }
            else
            {
                //string m_ini = @"setting.ini";
                string m_source = @args[0];
                string m_target = @args[1];
                //string m_ini = @"D:\Projects\HROne\source 1.0.13\Core\AttendanceFileConverter\Integrated\bin\Debug\setting.ini";
                //string m_source = @"D:\Projects\HROne\source 1.0.13\Core\AttendanceFileConverter\Integrated\bin\Debug\sample.txt";
                //string m_target = @"D:\Projects\HROne\source 1.0.13\Core\AttendanceFileConverter\Integrated\bin\Debug\output.csv";

                //if (!File.Exists(m_ini))
                //{
                //    IniParser parser = new IniParser(@m_ini);
                //    string s ;
                    
                //    s = parser.GetSetting("setting", "FIELD_LENGTH_LIST");
                //    this.FIELD_LENGTH_LIST = s.Split();
                    
                //    s = parser.GetSetting("setting", "KEY_FIELD_NO");
                //    int.TryParse(s, out KEY_FIELD_NO);

                //    s = parser.GetSetting("setting", "KEY_FILTER_LIST");
                //    this.KEY_FILTER_LIST = s.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);

                //    this.DELIMITER = parser.GetSetting("setting", "DELIMITER");
                //}

                if (!File.Exists(m_source))
                {
                    Console.WriteLine("Cannot locate source file " + m_source + ".");
                    return; 
                }
                string[] m_lines = System.IO.File.ReadAllLines(m_source);

                if (!File.Exists(m_target))
                {
                    using (StreamWriter fs = new StreamWriter(@m_target))
                    {
                        foreach (string m_line in m_lines)
                        {
                            string m_key = m_line.Substring(field_start(KEY_FIELD_NO), field_length(KEY_FIELD_NO));

                            if (Array.IndexOf(KEY_FILTER_LIST, m_key) > -1)
                            {
                                string m_outLine = "";

                                for (int i = 0; i< FIELD_LENGTH_LIST.Length; i++)
                                {
                                    m_outLine += m_line.Substring(field_start(i+1), field_length(i+1)).Trim() + ((i < FIELD_LENGTH_LIST.Length-1) ? DELIMITER : "");
                                }
                                fs.WriteLine(m_outLine);
                            }
                        }
                        fs.Close();
                    }
                }
                else
                {
                    Console.WriteLine("Target file (" + m_target + ") already exists.");
                }
            }
        }
    }
}
