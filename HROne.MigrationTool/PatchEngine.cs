using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
using HROne.Lib.Entities;

namespace HROne.MigrationTool
{
    class PatchEngine
    {
        private string m_SCRIPT_DB_VERSION = ""; // for hrexpress conversion
        private string m_PatchScriptPath = "./";

        DatabaseConnection dbConn;

        public PatchEngine(DatabaseConnection dbConn, string pathScriptPath, string script_db_version)
        {
            this.dbConn = dbConn;
            this.m_PatchScriptPath = pathScriptPath;
            this.m_SCRIPT_DB_VERSION = script_db_version;
        }

        public string SCRIPT_DB_VERSION
        {
            get { return m_SCRIPT_DB_VERSION; }
            set { m_SCRIPT_DB_VERSION = value; }
        }

        public string RunningDatabaseVersion()
        {
            return ESystemParameter.getParameter(dbConn, "DBVersion");
        }

        private bool IsUpdateRequired()
        {
            return string.Compare(RunningDatabaseVersion(), m_SCRIPT_DB_VERSION) < 0;
        }

        public bool UpdateDatabaseVersion(bool ForceBackupDatabase)
        {
            string strLastErrorMessage = string.Empty;

            if (IsUpdateRequired())
            {
                string dbVersion = RunningDatabaseVersion();
                bool passBackupDatabase = true;
                if (ForceBackupDatabase)
                    passBackupDatabase = AutoBackUpDatabase(dbVersion);
                if (passBackupDatabase)
                {
#region version_upgrade_script_section

                    if (dbVersion.StartsWith("0.00"))
                    {
                        if (dbVersion.Equals("0.00.0019"))
                        {
                            RunDataPatch("20090414_Beta0020.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("0.00.0020"))
                        {
                            RunDataPatch("20090416_Beta0021.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("0.00.0021"))
                        {
                            RunDataPatch("20090420_Beta0022.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("0.00.0022"))
                        {
                            RunDataPatch("20090430_Beta0023.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("0.00.0023"))
                        {
                            RunDataPatch("20090512 _1_00_0024.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                    }
                    if (dbVersion.StartsWith("1.00"))
                    {
                        if (dbVersion.Equals("1.00.0024"))
                        {
                            RunDataPatch("20090526_1_00_0025.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.00.0025"))
                        {
                            RunDataPatch("20090604_1_00_0029.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.00.0029"))
                        {
                            RunDataPatch("20090626_1_00_0036.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.00.0036"))
                        {
                            RunDataPatch("20090702_1_00_0040.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.00.0040"))
                        {
                            RunDataPatch("20090706_1_01_0042.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                    }
                    if (dbVersion.StartsWith("1.01"))
                    {

                        if (dbVersion.Equals("1.01.0042"))
                        {
                            RunDataPatch("20090707_1_01_0043.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.01.0043"))
                        {
                            RunDataPatch("20090709_1_01_0046_Skip_0044.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.01.0044"))
                        {
                            RunDataPatch("20090709_1_01_0046.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.01.0046"))
                        {
                            RunDataPatch("20090714_1_01_0049.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.01.0049"))
                        {
                            RunDataPatch("20090715_1_01_0050.sql");
                            RunDataPatch("20090715_1_01_0050_DataPatch.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.01.0050"))
                        {
                            RunDataPatch("20090722_1_01_0051.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.01.0051"))
                        {
                            RunDataPatch("20090724_1_01_0052.sql");
                            RunDataPatch("20090724_1_01_0052_DataPatch.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.01.0052"))
                        {
                            RunDataPatch("20090728_1_01_0054.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.01.0054"))
                        {
                            RunDataPatch("20090805_1_01_0056.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.01.0056"))
                        {
                            RunDataPatch("20090810_1_01_0058.sql");
                            RunDataPatch("20090810_1_01_0058_DataPatch.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.01.0058"))
                        {
                            RunDataPatch("20090811_1_01_0059.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.01.0059"))
                        {
                            RunDataPatch("20090819_1_01_0060.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.01.0060"))
                        {
                            RunDataPatch("20090820_1_01_0061.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.01.0061"))
                        {
                            RunDataPatch("20090821_1_01_0062.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.01.0062"))
                        {
                            RunDataPatch("20090822_1_01_0063.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.01.0063"))
                        {
                            RunDataPatch("20090827_1_01_0065.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.01.0065"))
                        {
                            RunDataPatch("20090831_1_01_0066.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.01.0066"))
                        {
                            RunDataPatch("20090903_1_01_0067.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.01.0067"))
                        {
                            RunDataPatch("20090910_1_02_0068.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                    }
                    if (dbVersion.StartsWith("1.02"))
                    {

                        if (dbVersion.Equals("1.02.0068"))
                        {
                            RunDataPatch("20090911_1_02_0069.sql");
                            RunDataPatch("20090911_1_02_0069_DataPatch.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.02.0069"))
                        {
                            RunDataPatch("20090914_1_02_0070.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.02.0070"))
                        {
                            RunDataPatch("20090916_1_02_0071.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.02.0071"))
                        {
                            RunDataPatch("20090918_1_02_0072.sql");
                            RunDataPatch("20090918_1_02_0072_DataPatch.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.02.0072"))
                        {
                            RunDataPatch("20090921_1_02_0073.sql");
                            RunDataPatch("20090921_1_02_0073_DataPatch.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.02.0073"))
                        {
                            RunDataPatch("20090924_1_02_0074.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.02.0074"))
                        {
                            RunDataPatch("20090929_1_02_0075.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.02.0075"))
                        {
                            RunDataPatch("20091005_1_02_0077.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.02.0077"))
                        {
                            RunDataPatch("20091008_1_02_0078.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.02.0078"))
                        {
                            RunDataPatch("20091013_1_02_0079.sql");
                            RunDataPatch("20091013_1_02_0079_DataPatch.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.02.0079"))
                        {
                            RunDataPatch("20091021_1_02_0080.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.02.0080"))
                        {
                            RunDataPatch("20091027_1_02_0081.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.02.0081"))
                        {
                            RunDataPatch("20091028_1_02_0082.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.02.0082"))
                        {
                            RunDataPatch("20091030_1_02_0083.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.02.0083"))
                        {
                            RunDataPatch("20091104_1_02_0084.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.02.0084"))
                        {
                            RunDataPatch("20091105_1_02_0085.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.02.0085"))
                        {
                            RunDataPatch("20091108_1_02_0086.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.02.0086"))
                        {
                            RunDataPatch("20091110_1_02_0087.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.02.0087"))
                        {
                            RunDataPatch("20091113_1_02_0088.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.02.0088"))
                        {
                            RunDataPatch("20091120_1_03_0089.sql");
                            RunDataPatch("20091120_1_03_0089_DataPatch.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                    }
                    if (dbVersion.StartsWith("1.03"))
                    {

                        if (dbVersion.Equals("1.03.0089"))
                        {
                            RunDataPatch("20091130_1_03_0091.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.03.0091"))
                        {
                            RunDataPatch("20091201_1_03_0092.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.03.0092"))
                        {
                            RunDataPatch("20091207_1_03_0093.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.03.0093"))
                        {
                            RunDataPatch("20091211_1_03_0094.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.03.0094"))
                        {
                            RunDataPatch("20091214_1_03_0095.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.03.0095"))
                        {
                            RunDataPatch("20100105_1_03_0099.sql");
                            RunDataPatch("20100105_1_03_0099_DataPatch.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.03.0099"))
                        {
                            RunDataPatch("20100108_1_03_0101.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.03.0101"))
                        {
                            RunDataPatch("20100120_1_03_0107.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.03.0107"))
                        {
                            RunDataPatch("20100129_1_03_0112.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.03.0112"))
                        {
                            RunDataPatch("20100204_1_03_0116.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.03.0116"))
                        {
                            RunDataPatch("20100211_1_03_0118.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.03.0118"))
                        {
                            RunDataPatch("20100304_1_03_0125.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.03.0125"))
                        {
                            RunDataPatch("20100305_1_03_0126.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.03.0126"))
                        {
                            RunDataPatch("20100306_1_03_0127.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.03.0127"))
                        {
                            RunDataPatch("20100316_1_04_0130.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                    }
                    if (dbVersion.StartsWith("1.04"))
                    {

                        if (dbVersion.Equals("1.04.0130"))
                        {
                            RunDataPatch("20100326_1_04_0132.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.04.0132"))
                        {
                            RunDataPatch("20100430_1_05_0143.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                    }
                    if (dbVersion.StartsWith("1.05"))
                    {
                        if (dbVersion.Equals("1.05.0143"))
                        {
                            RunDataPatch("20100531_1_05_0153.sql");
                            RunDataPatch("20100531_1_05_0153_DataPatch.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.05.0153"))
                        {
                            RunDataPatch("20100602_1_05_0154.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.05.0154"))
                        {
                            RunDataPatch("20100610_1_05_0155.sql");
                            RunDataPatch("20100610_1_05_0155_DataPatch.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.05.0155"))
                        {
                            RunDataPatch("20100624_1_05_0161.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.05.0161"))
                        {
                            RunDataPatch("20100630_1_05_0162.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.05.0162"))
                        {
                            RunDataPatch("20100716_1_05_0169.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.05.0169"))
                        {
                            RunDataPatch("20100722_1_06_0172.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                    }
                    if (dbVersion.StartsWith("1.06"))
                    {
                        if (dbVersion.Equals("1.06.0172"))
                        {
                            RunDataPatch("20100729_1_06_0174.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.06.0174"))
                        {
                            RunDataPatch("20100803_1_06_0175.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.06.0175"))
                        {
                            RunDataPatch("20100902_1_06_0186.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.06.0186"))
                        {
                            RunDataPatch("20100924_1_07_0190.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                    }
                    if (dbVersion.StartsWith("1.07"))
                    {
                        if (dbVersion.Equals("1.07.0190"))
                        {
                            RunDataPatch("20100927_1_07_0191.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.07.0191"))
                        {
                            RunDataPatch("20100929_1_07_0192.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.07.0192"))
                        {
                            RunDataPatch("20101013_1_07_0196.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.07.0196"))
                        {
                            RunDataPatch("20101015_1_07_0197.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.07.0197"))
                        {
                            RunDataPatch("20101019_1_07_0198.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.07.0198"))
                        {
                            RunDataPatch("20101022_1_07_0199.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.07.0199"))
                        {
                            RunDataPatch("20101028_1_07_0201.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.07.0201"))
                        {
                            RunDataPatch("20101029_1_07_0202.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.07.0202"))
                        {
                            RunDataPatch("20101103_1_07_0203.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.07.0203"))
                        {
                            RunDataPatch("20101124_1_08_0207.sql");
                            RunDataPatch("20101124_1_08_0207_DataPatch.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                    }
                    if (dbVersion.StartsWith("1.08"))
                    {
                        if (dbVersion.Equals("1.08.0207"))
                        {
                            RunDataPatch("20101209_1_08_0210.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.08.0210"))
                        {
                            RunDataPatch("20101214_1_08_0211.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.08.0211"))
                        {
                            RunDataPatch("20101217_1_08_0212.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.08.0212"))
                        {
                            RunDataPatch("20101217_1_08_0213.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.08.0213"))
                        {
                            RunDataPatch("20110110_1_08_0218.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.08.0218"))
                        {
                            RunDataPatch("20110114_1_08_0219.sql");
                            RunDataPatch("20110114_1_08_0219_DataPatch.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.08.0219"))
                        {
                            RunDataPatch("20110126_1_09_0220.sql");
                            dbVersion = RunningDatabaseVersion();
                        }

                    }
                    if (dbVersion.StartsWith("1.09"))
                    {
                        if (dbVersion.Equals("1.09.0220"))
                        {
                            RunDataPatch("20110218_1_09_0226.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.09.0226"))
                        {
                            RunDataPatch("20110221_1_09_0227.sql");
                            HROne.ProductVersion.Patch.Patch_0227.DBPatch(dbConn);
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.09.0227"))
                        {
                            RunDataPatch("20110228_1_10_0228.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                    }
                    if (dbVersion.StartsWith("1.10"))
                    {
                        if (dbVersion.Equals("1.10.0228"))
                        {
                            RunDataPatch("20110301_1_10_0229.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.10.0229"))
                        {
                            RunDataPatch("20110304_1_10_0230.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.10.0230"))
                        {
                            RunDataPatch("20110316_1_10_0231.sql");
                            RunDataPatch("20110316_1_10_0231_DataPatch.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.10.0231"))
                        {
                            RunDataPatch("20110401_1_10_0235.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.10.0235"))
                        {
                            RunDataPatch("20110408_1_10_0236.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.10.0236"))
                        {
                            RunDataPatch("20110506_1_11_0241.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                    }
                    if (dbVersion.StartsWith("1.11"))
                    {
                        if (dbVersion.Equals("1.11.0241"))
                        {
                            RunDataPatch("20110509_1_11_0242.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.11.0242"))
                        {
                            RunDataPatch("20110516_1_11_0244.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.11.0244"))
                        {
                            RunDataPatch("20110518_1_11_0245.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.11.0245"))
                        {
                            RunDataPatch("20110527_1_11_0246.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.11.0246"))
                        {
                            RunDataPatch("20110601_1_11_0247.sql");
                            HROne.ProductVersion.Patch.Patch_0247.DBPatch(dbConn);
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.11.0247"))
                        {
                            RunDataPatch("20110603_1_11_0248.sql");
                            RunDataPatch("20110603_1_11_0248_DataPatch.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.11.0248"))
                        {
                            RunDataPatch("20110610_1_11_0249.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.11.0249"))
                        {
                            RunDataPatch("20110617_1_11_0251.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.11.0251"))
                        {
                            RunDataPatch("20110625_1_11_0253.sql");
                            RunDataPatch("20110625_1_11_0253_DataPatch.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.11.0253"))
                        {
                            RunDataPatch("20110629_1_11_0256.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.11.0256"))
                        {
                            RunDataPatch("20110706_1_11_0257.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.11.0257"))
                        {
                            RunDataPatch("20110714_1_11_0259.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.11.0259"))
                        {
                            RunDataPatch("20110823_1_12_0271.sql");
                            RunDataPatch("20110823_1_12_0271.sql_DataPatch.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                    }
                    if (dbVersion.StartsWith("1.12"))
                    {
                        if (dbVersion.Equals("1.12.0271"))
                        {
                            RunDataPatch("20110826_1_12_0272.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.12.0272"))
                        {
                            RunDataPatch("20110829_1_12_0273.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.12.0273"))
                        {
                            RunDataPatch("20110909_1_12_0280.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.12.0280"))
                        {
                            RunDataPatch("20110915_1_12_0281.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.12.0281"))
                        {
                            RunDataPatch("20111007_1_12_0285.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.12.0285"))
                        {
                            RunDataPatch("20111024_1_12_0289.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.12.0289"))
                        {
                            RunDataPatch("20111101_1_12_0292.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.12.0292"))
                        {
                            RunDataPatch("20111104_1_12_0293.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.12.0293"))
                        {
                            RunDataPatch("20111108_1_12_0294.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.12.0294"))
                        {
                            RunDataPatch("20111130_1_12_0300.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.12.0300"))
                        {
                            RunDataPatch("20111212_1_12_0305.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.12.0305"))
                        {
                            RunDataPatch("20111221_1_12_0311.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.12.0311"))
                        {
                            RunDataPatch("20111222_1_12_0312.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.12.0312"))
                        {
                            RunDataPatch("20111223_1_12_0313.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.12.0313"))
                        {
                            RunDataPatch("20120112_1_13_0322.sql");
                            dbVersion = RunningDatabaseVersion();

                            ////  for testing encryption only
                            //ArrayList taxempList = ETaxEmp.db.select(dbConn, new DBFilter());
                            //foreach (ETaxEmp taxEmp in taxempList)
                            //    ETaxEmp.db.update(dbConn, taxEmp);

                            //ArrayList taxCompanyList = ETaxCompany.db.select(dbConn, new DBFilter());
                            //foreach (ETaxCompany taxComp in taxCompanyList)
                            //    ETaxCompany.db.update(dbConn, taxComp);

                            //ArrayList taxEmpPoRList = ETaxEmpPlaceOfResidence.db.select(dbConn, new DBFilter());
                            //foreach (ETaxEmpPlaceOfResidence taxEmpPoR in taxEmpPoRList)
                            //    ETaxEmpPlaceOfResidence.db.update(dbConn, taxEmpPoR);
                        }
                    }
                    if (dbVersion.StartsWith("1.13"))
                    {
                        if (dbVersion.Equals("1.13.0322"))
                        {
                            RunDataPatch("20120117_1_13_0324.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.13.0324"))
                        {
                            RunDataPatch("20120119_1_13_0325.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.13.0325"))
                        {
                            RunDataPatch("20120127_1_13_0326.sql");
                            RunDataPatch("20120127_1_13_0326_DataPatch.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.13.0326"))
                        {
                            RunDataPatch("20120131_1_13_0327.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.13.0327"))
                        {
                            RunDataPatch("20120202_1_13_0328.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.13.0328"))
                        {
                            RunDataPatch("20120207_1_13_0329.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.13.0329"))
                        {
                            RunDataPatch("20120216_1_13_0333.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.13.0333"))
                        {
                            RunDataPatch("20120221_1_13_0335.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.13.0335"))
                        {
                            RunDataPatch("20120222_1_13_0336.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.13.0336"))
                        {
                            RunDataPatch("20120302_1_13_0339.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.13.0339"))
                        {
                            RunDataPatch("20120405_1_13_0358.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.13.0358"))
                        {
                            RunDataPatch("20120412_1_13_0361.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.13.0361"))
                        {
                            RunDataPatch("20120419_1_13_0364.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.13.0364"))
                        {
                            RunDataPatch("20120425_1_14_368.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                    }
                    if (dbVersion.StartsWith("1.14"))
                    {
                        if (dbVersion.Equals("1.14.368"))
                        {
                            RunDataPatch("20120426_1_14_369.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.14.369"))
                        {
                            RunDataPatch("20120427_1_14_370.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.14.370"))
                        {
                            RunDataPatch("20120503_1_14_373.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.14.373"))
                        {
                            RunDataPatch("20120504_1_14_374.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.14.374"))
                        {
                            RunDataPatch("20120509_1_14_377.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.14.377"))
                        {
                            RunDataPatch("20120511_1_14_378.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.14.378"))
                        {
                            RunDataPatch("20120516_1_14_380.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.14.380"))
                        {
                            RunDataPatch("20120524_1_14_387.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("1.14.387"))
                        {
                            RunDataPatch("20120713_2_0_416.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                    }
                    if (dbVersion.StartsWith("2.0"))
                    {
                        if (dbVersion.Equals("2.0.416"))
                        {
                            RunDataPatch("20120724_2_0_418.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.0.418"))
                        {
                            RunDataPatch("20120731_2_0_419.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.0.419"))
                        {
                            RunDataPatch("20120802_2_0_420.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.0.420"))
                        {
                            RunDataPatch("20120806_2_0_421.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.0.421"))
                        {
                            RunDataPatch("20120824_2_0_430.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.0.430"))
                        {
                            RunDataPatch("20120828_2_0_433.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.0.433"))
                        {
                            RunDataPatch("20120905_2_0_436.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.0.436"))
                        {
                            RunDataPatch("20120918_2_0_446.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.0.446"))
                        {
                            RunDataPatch("20120921_2_0_448.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.0.448"))
                        {
                            RunDataPatch("20121009_2_0_457.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.0.457"))
                        {
                            RunDataPatch("20121017_2_0_461.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.0.461"))
                        {
                            RunDataPatch("20121019_2_0_463.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.0.463"))
                        {
                            RunDataPatch("20121024_2_0_465.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.0.465"))
                        {
                            RunDataPatch("20121031_2_0_470.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.0.470"))
                        {
                            RunDataPatch("20121031_2_0_479.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.0.479"))
                        {
                            RunDataPatch("20121121_2_0_480.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.0.480"))
                        {
                            RunDataPatch("20121122_2_0_481.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.0.481"))
                        {
                            RunDataPatch("20121124_2_0_483.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.0.483"))
                        {
                            RunDataPatch("20121128_2_1_485.sql");
                            dbVersion = RunningDatabaseVersion();

                        }
                    }
                    if (dbVersion.StartsWith("2.1"))
                    {
                        if (dbVersion.Equals("2.1.485"))
                        {
                            RunDataPatch("20121129_2_1_486.sql");
                            dbVersion = RunningDatabaseVersion();

                        }
                        if (dbVersion.Equals("2.1.486"))
                        {
                            RunDataPatch("20121130_2_1_487.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.1.487"))
                        {
                            RunDataPatch("20121203_2_1_488.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.1.488"))
                        {
                            HROne.ProductVersion.Patch.Patch_490.DBPatch(dbConn);
                            RunDataPatch("20121207_2_1_490.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.1.490"))
                        {
                            RunDataPatch("20121211_2_1_491.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.1.491"))
                        {
                            RunDataPatch("20121212_2_1_492.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.1.492"))
                        {
                            RunDataPatch("20121214_2_1_494.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.1.494"))
                        {
                            RunDataPatch("20130104_2_1_504.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.1.504"))
                        {
                            RunDataPatch("20130122_2_1_513.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.1.513"))
                        {
                            RunDataPatch("20130130_2_1_515.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.1.515"))
                        {
                            RunDataPatch("20130201_2_1_516.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.1.516"))
                        {
                            RunDataPatch("20130204_2_1_517.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.1.517"))
                        {
                            RunDataPatch("20130206_2_1_518.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.1.518"))
                        {
                            RunDataPatch("20130207_2_1_519.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.1.519"))
                        {
                            RunDataPatch("20130208_2_1_520.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.1.520"))
                        {
                            RunDataPatch("20130218_2_1_521.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.1.521"))
                        {
                            RunDataPatch("20130225_2_1_523.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.1.523"))
                        {
                            RunDataPatch("20130226_2_1_524.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.1.524"))
                        {
                            RunDataPatch("20130228_2_1_526.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.1.526"))
                        {
                            RunDataPatch("20130301_2_1_527.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.1.527"))
                        {
                            RunDataPatch("20130306_2_1_530.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.1.530"))
                        {
                            RunDataPatch("20130308_2_1_532.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.1.532"))
                        {
                            RunDataPatch("20130311_2_1_533.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.1.533"))
                        {
                            RunDataPatch("20130315_2_1_535.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.1.535"))
                        {
                            RunDataPatch("20130319_2_1_536.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.1.536"))
                        {
                            RunDataPatch("20130320_2_1_537.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.1.537"))
                        {
                            RunDataPatch("20130322_2_1_539.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.1.539"))
                        {
                            RunDataPatch("20130326_2_1_541.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.1.541"))
                        {
                            RunDataPatch("20130328_2_2_544.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                    }
                    if (dbVersion.StartsWith("2.2"))
                    {
                        if (dbVersion.Equals("2.2.544"))
                        {
                            RunDataPatch("20130402_2_2_545.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.2.545"))
                        {
                            RunDataPatch("20130418_2_2_551.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.2.551"))
                        {
                            RunDataPatch("20130426_2_2_555.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.2.555"))
                        {
                            RunDataPatch("20130523_2_2_561.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.2.561"))
                        {
                            RunDataPatch("20130527_2_2_562.sql");
                            RunDataPatch("20130527_2_2_562_DataPatch.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.2.562"))
                        {
                            RunDataPatch("20130529_2_2_563.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.2.563"))
                        {
                            RunDataPatch("20130627_2_2_565_3.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.2.565.3"))
                        {
                            RunDataPatch("20130607_2_3_566.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                    }
                    if (dbVersion.StartsWith("2.3"))
                    {
                        if (dbVersion.Equals("2.3.566"))
                        {
                            RunDataPatch("20130611_2_3_569.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.3.569"))
                        {
                            RunDataPatch("20130618_2_3_570.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.3.570"))
                        {
                            RunDataPatch("20130621_2_3_571.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.3.571"))
                        {
                            RunDataPatch("20130624_2_3_572.sql");
                            RunDataPatch("20130624_2_3_572_DataPatch.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.3.572"))
                        {
                            RunDataPatch("20130627_2_3_573.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.3.573"))
                        {
                            RunDataPatch("20130709_2_3_576.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.3.576"))
                        {
                            RunDataPatch("20130710_2_3_577.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.3.577"))
                        {
                            RunDataPatch("20130711_2_3_578.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.3.578"))
                        {
                            RunDataPatch("20130715_2_3_580.sql");
                            RunDataPatch("20130715_2_3_580_DataPatch.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.3.580"))
                        {
                            RunDataPatch("20130716_2_3_581.sql");
                            RunDataPatch("20130716_2_3_581_DataPatch.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.3.581"))
                        {
                            RunDataPatch("20130718_2_3_582.sql");
                            RunDataPatch("20130718_2_3_582_DataPatch.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.3.582"))
                        {
                            RunDataPatch("20130730_2_3_584.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                        if (dbVersion.Equals("2.3.584"))
                        {
                            RunDataPatch("20130826_2_3_588.sql");
                            dbVersion = RunningDatabaseVersion();
                        }
                    }
                }
#endregion version_upgrade_script_section

                //RunDataPatch("Import_Employee.sql");
                //if (!dbVersion.Equals(CURRENT_DB_VERSION))
                //if (!String.IsNullOrEmpty(LastErrorMessage))
                //{
                //    return false;
                //}
                RunDataPatch("Import_Employee.sql");
                RunDataPatch("PermissionUpdate.sql");
            }
            return true;

        }

        private void RunDataPatch(string Filename)
        {
            string strLastErrorMessage = string.Empty;
            try
            {
                //string FullPath = System.IO.Path.Combine(System.IO.Path.Combine(HROneWebPath, @"DBPatch"), Filename);
                string FullPath = System.IO.Directory.GetCurrentDirectory() + "\\DBPatch\\" + Filename; 
                     
                System.IO.FileInfo dbPatchFile = new System.IO.FileInfo(FullPath);
                System.IO.StreamReader reader = dbPatchFile.OpenText();
                string PatchString = reader.ReadToEnd();
                reader.Close();
                AppUtils.DBPatch(dbConn, PatchString, out strLastErrorMessage);
            }
            catch (Exception ex)
            {
                strLastErrorMessage = ex.Message;
            }
        }

        private bool AutoBackUpDatabase(string VersionNo)
        {
            string strLastErrorMessage;
            string filePath = string.Empty;
            if (dbConn.dbType == DatabaseConnection.DatabaseType.MSSQL)
            {
                System.Data.SqlClient.SqlConnection conn = (System.Data.SqlClient.SqlConnection)dbConn.Connection;
                filePath = conn.Database + "_" + VersionNo + "_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".bak";
            }
            return AppUtils.BackUpDatabase(dbConn, filePath, out strLastErrorMessage);
            //if (perspectivemind.common.DBUtil.type is perspectivemind.common.SQLType)
            //{
            //    System.Data.SqlClient.SqlConnection conn = (System.Data.SqlClient.SqlConnection)perspectivemind.common.DBUtil.getConnection();
            //    string backupSQL = "BACKUP DATABASE " + conn.Database + "\r\n" +
            //        "TO DISK='" + conn.Database + "_" + VersionNo + "_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".bak'";
            //    if (AppUtils.DBPatch(backupSQL, out strLastErrorMessage))
            //    {
            //        return true;
            //    }
            //}
            //return false;
        }

        //private static void CreateSchema(string ConnectionStringWithoutInitialCatalog, string DatabaseName, string GrantUserID)
        //{
        //    System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(ConnectionStringWithoutInitialCatalog);

        //    System.Data.SqlClient.SqlCommand command = conn.CreateCommand();
        //    command.CommandType = System.Data.CommandType.Text;
        //    command.CommandText = "Create Database " + DatabaseName + "\r\n";

        //    command.Connection.Open();
        //    command.ExecuteNonQuery();

        //    //if (MSSQLUserID.Text.Trim() != saUser.Trim())
        //    {
        //        command.CommandText = "USE " + DatabaseName + "\r\n"
        //        + "CREATE USER " + GrantUserID + " FOR LOGIN " + GrantUserID + "\r\n"
        //        + "EXEC sp_addrolemember N'db_owner', N'" + GrantUserID + "'";

        //        command.ExecuteNonQuery();
        //    }
        //    command.Connection.Close();
        //}

        //public static void CreateTableAndData(string HROneWebPath, string ConnectionString)
        //{
        //    System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(ConnectionString);

        //    System.Data.SqlClient.SqlCommand command = conn.CreateCommand();
        //    command.CommandType = System.Data.CommandType.Text;

        //    string FullPath = System.IO.Path.Combine(System.IO.Path.Combine(HROneWebPath, @"EmptyDatabase"), "HROneDBScheme.sql");
        //    System.IO.FileInfo dbPatchFile = new System.IO.FileInfo(FullPath);
        //    System.IO.StreamReader reader = dbPatchFile.OpenText();
        //    string PatchString = reader.ReadToEnd();
        //    reader.Close();
        //    command.CommandText = PatchString;

        //    command.Connection.Open();
        //    command.ExecuteNonQuery();

        //    FullPath = System.IO.Path.Combine(System.IO.Path.Combine(HROneWebPath, @"EmptyDatabase"), "SystemData.sql");
        //    dbPatchFile = new System.IO.FileInfo(FullPath);
        //    reader = dbPatchFile.OpenText();
        //    PatchString = reader.ReadToEnd();
        //    reader.Close();
        //    command.CommandText = PatchString;

        //    command.ExecuteNonQuery();

        //    FullPath = System.IO.Path.Combine(System.IO.Path.Combine(HROneWebPath, @"EmptyDatabase"), "SupplementaryData.sql");
        //    dbPatchFile = new System.IO.FileInfo(FullPath);
        //    reader = dbPatchFile.OpenText();
        //    PatchString = reader.ReadToEnd();
        //    reader.Close();
        //    command.CommandText = PatchString;

        //    command.ExecuteNonQuery();
        //    command.Connection.Close();
        //}

    }
}
