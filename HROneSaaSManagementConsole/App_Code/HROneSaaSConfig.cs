using System;
using System.Data;
using System.Configuration;
using System.Xml;
using HROne.DataAccess;


public class HROneSaaSConfig
{
    //[ThreadStatic]
    //private static HROneConfig currentHROneConfig = null;

    private const string ConfigFilename = "HROneSaaS.config";
    public string HROneConfigFullPath = string.Empty;

    protected HROneSaaSConfig()
    {
        load(getFilename());
    }

    public HROneSaaSConfig(string filename)
    {
        load(filename);
    }

    public static HROneSaaSConfig GetCurrentConfig()
    {
        //if (currentHROneConfig == null)
        //    currentHROneConfig = new HROneConfig();
        //return currentHROneConfig;
        return new HROneSaaSConfig();
    }

    public void load(string filename)
    {


        System.Configuration.ConfigXmlDocument config = new System.Configuration.ConfigXmlDocument();
        try
        {
            config.Load(filename);
            if (config["Settings"] != null)
            {
                if (config["Settings"].Attributes["Version"].Value == "2.0")
                {
                    if (config["Settings"]["HROneConfigFullPath"] != null)
                    {
                        string HROnePath = config["Settings"]["HROneConfigFullPath"].InnerText;
                        if (System.IO.File.Exists(HROnePath))
                            HROneConfigFullPath = HROnePath;
                    }
                }
            }



        }
        catch
        {
        }

    }

    public void Save()
    {
        string errorMessage = null;
        if (!HROne.CommonLib.FileIOProcess.IsFolderAllowWritePermission(AppDomain.CurrentDomain.BaseDirectory, out errorMessage))
            throw new Exception(errorMessage);
        string filename = getFilename();


        System.Configuration.ConfigXmlDocument config = new System.Configuration.ConfigXmlDocument();
        XmlElement settings = config.CreateElement("Settings");
        XmlAttribute version = config.CreateAttribute("Version");
        version.Value = "2.0";
        settings.Attributes.Append(version);
        config.AppendChild(settings);
        //SetDatabaseConfigList(settings);

        settings.AppendChild(config.CreateElement("HROneConfigFullPath"));
        settings["HROneConfigFullPath"].InnerText = HROneConfigFullPath;


        config.Save(filename);
    }

    private string getFilename()
    {
        return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigFilename);

    }

}

