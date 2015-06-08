using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using HROne.DataAccess;
//using perspectivemind.validation;
using HROne.SaaS.Entities;

public partial class CompanyDB_Edit : HROneWebPage
{
    const string FUNCTION_CODE = "ADM002";
    public Binding binding;
    public DBManager db = ECompanyDatabase.db;
    public int CurID = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE))
            return;

        binding = new Binding(dbConn, db);
        binding.add(CompanyDBID);
        binding.add(CompanyDBClientCode);
        binding.add(CompanyDBClientContactPerson);
        binding.add(CompanyDBClientName);
        binding.add(CompanyDBClientAddress);
        binding.add(new DropDownVLBinder(db, DBServerID, EDatabaseServer.VLDBServerList));
        binding.add(CompanyDBSchemaName);
        binding.add(new CheckBoxBinder(db, CompanyDBIsActive));
        binding.add(CompanyDBMaxCompany);
        binding.add(CompanyDBMaxUser);
        binding.add(CompanyDBMaxEmployee);
        binding.add(CompanyDBInboxMaxQuotaMB);
        binding.add(new CheckBoxBinder(db, CompanyDBAutopayMPFFileHasHSBCHASE));
        binding.add(new CheckBoxBinder(db, CompanyDBAutopayMPFFileHasOthers));
        binding.add(new CheckBoxBinder(db, CompanyDBHasEChannel));
        binding.add(new CheckBoxBinder(db, CompanyDBHasIMGR));
        binding.add(new CheckBoxBinder(db, CompanyDBHasIStaff));
        binding.init(Request, Session);

        if (!IsPostBack)
        {
            if (!int.TryParse(DecryptedRequest["CompanyDBID"], out CurID))
                CurID = -1;
        }
        else
        {
            if (!int.TryParse(CompanyDBID.Value, out CurID))
                CurID = -1;
        }
        HSBCExchangeProfile_List1.CompanyDBID = CurID;
        if (!Page.IsPostBack)
        {
            chkAutoCreateID.Attributes.Add("onclick", CompanyDBClientCode.ClientID + ".disabled=" + chkAutoCreateID.ClientID + ".checked;");
            if (CurID > 0)
            {
                loadObject();
            }
            else
            {
                chkCreateDB.Checked = true;
                toolBar.DeleteButton_Visible = false;
            }
        }
    }

    protected void Page_PreRender()
    {
        if (CurID > 0)
            AdditionalInformation.Visible = true;
        else
            AdditionalInformation.Visible = false;

    }

    protected bool loadObject()
    {
        ECompanyDatabase obj = new ECompanyDatabase();
        obj.CompanyDBID = CurID;

        if (!db.select(dbConn, obj))
            if (CurID <= 0)
                return false;
            else
                Response.Redirect("~/AccessDeny.aspx");


        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);

        return true;
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        HROneSaaSConfig SaaSconfig = HROneSaaSConfig.GetCurrentConfig();
        string HROnePath = new System.IO.FileInfo(SaaSconfig.HROneConfigFullPath).Directory.FullName;

        ECompanyDatabase c = new ECompanyDatabase();
        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;

        db.parse(values, c);

        if (!chkAutoCreateID.Checked && string.IsNullOrEmpty(c.CompanyDBClientCode))
        {
            errors.addError("Client ID is required");
            return;
        }



        HROne.ProductKey key = new HROne.ProductKey();
        key.ProductType = HROne.ProductKey.ProductLicenseType.HROneSaaS;
        key.NumOfCompanies = Convert.ToUInt16(c.CompanyDBMaxCompany);
        key.NumOfUsers = Convert.ToUInt16(c.CompanyDBMaxUser);
        if (c.CompanyDBHasIMGR)
        {
            key.IsLeaveManagement = true;
            key.IsPayroll = true;
            key.IsTaxation = true;
        }
        if (c.CompanyDBHasIStaff)
        {
            key.IsESS = true;
        }

        if (string.IsNullOrEmpty(c.CompanyDBClientCode))
        {
            const int MAX_LENGTH = 8;
            string prefix = CreateClientCodePrefix(c.CompanyDBClientName);
            //if (c.CompanyDBClientBank.Equals("HSBC", StringComparison.CurrentCultureIgnoreCase))
            //    prefix = "H";
            //else if (c.CompanyDBClientBank.Equals("HangSeng", StringComparison.CurrentCultureIgnoreCase))
            //    prefix = "X";
            int idx = 0;
            if (prefix.Length >= MAX_LENGTH)
                prefix = prefix.Substring(0, MAX_LENGTH);
            else
            {
                idx++;
                string idxString = idx.ToString().Trim();
                prefix = prefix.PadRight(MAX_LENGTH - idxString.Length, '0') + idxString;
            }
            c.CompanyDBClientCode = prefix;
            while (!AppUtils.checkDuplicate(dbConn, ECompanyDatabase.db, c, new PageErrors(), "CompanyDBClientCode"))
            {
                idx++;
                string idxString = idx.ToString().Trim();
                c.CompanyDBClientCode = prefix.Substring(0, MAX_LENGTH - idxString.Length) + idxString;
            }
        }
        if (!AppUtils.checkDuplicate(dbConn, ECompanyDatabase.db, c, errors, "CompanyDBClientCode"))
            return;

        EDatabaseServer dbServer = new EDatabaseServer();
        dbServer.DBServerID = c.DBServerID;
        if (EDatabaseServer.db.select(dbConn, dbServer))
        {
            if (dbServer.DBServerDBType.Equals("MSSQL"))
            {
                System.Data.SqlClient.SqlConnectionStringBuilder saConnStringBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder();
                saConnStringBuilder.DataSource = dbServer.DBServerLocation;
                saConnStringBuilder.UserID = dbServer.DBServerSAUserID;
                saConnStringBuilder.Password = dbServer.DBServerSAPassword;

                DatabaseConfig dbConfig = new DatabaseConfig();
                dbConfig.DBType = WebUtils.DBTypeEmun.MSSQL;
                dbConfig.ConnectionString = saConnStringBuilder.ConnectionString;
                if (dbConfig.TestServerConnectionWithoutDatabase())
                {
                    string DBSchemaName = c.CompanyDBSchemaName.Trim();
                    if (DBSchemaName.Equals(string.Empty))
                        DBSchemaName = c.CompanyDBClientCode;
                    System.Data.SqlClient.SqlConnectionStringBuilder connStringBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder();
                    connStringBuilder.DataSource = dbServer.DBServerLocation;
                    connStringBuilder.InitialCatalog = DBSchemaName;
                    connStringBuilder.UserID = dbServer.DBServerUserID;
                    connStringBuilder.Password = dbServer.DBServerPassword;
                    dbConfig.ConnectionString = connStringBuilder.ConnectionString;
                    if (!dbConfig.TestConnection())
                        if (chkCreateDB.Checked)
                        {
                            try
                            {
                                HROne.ProductVersion.Database.CreateSchema(saConnStringBuilder.ConnectionString, DBSchemaName, dbServer.DBServerUserID);
                                //c.CompanyDBSchemaName = DBSchemaName;
                                saConnStringBuilder.InitialCatalog = DBSchemaName;
                                HROne.ProductVersion.Database.CreateTableAndData(HROnePath, saConnStringBuilder.ConnectionString);
                                // drop all the connection so that new "normal user" connection to database is accepted
                                System.Data.SqlClient.SqlConnection.ClearAllPools();
                            }
                            catch (Exception ex)
                            {
                                errors.addError(ex.Message);
                                return;
                            }
                        }
                        else
                        {
                            errors.addError("Fail to connect to database");
                            return;
                        }
                }
                else
                {
                    errors.addError("Fail to connect to server");
                    return;
                }
            }
        }
        if (CurID < 0)
        {
            //            Utils.MarkCreate(Session, c);

            db.insert(dbConn, c);
            CurID = c.CompanyDBID;
            HSBCExchangeProfile_List1.CompanyDBID = CurID;
            //            url = Utils.BuildURL(-1, CurID);
        }
        else
        {
            //            Utils.Mark(Session, c);
        }

        key.SerialNo = Convert.ToUInt16(c.CompanyDBID);
        c.CompanyDBProductKey = key.GetProductKey();
        db.update(dbConn, c);

        HROne.ProductVersion.Database databaseProcess = new HROne.ProductVersion.Database(new DatabaseConnection(c.getConnectionString(dbConn), DatabaseConnection.DatabaseType.MSSQL), HROnePath);
        databaseProcess.UpdateDatabaseVersion(true);
        errors.addError("Saved");
        loadObject();
        //Response.Redirect("~/CompanyDB_List.aspx");


    }
    protected void Delete_Click(object sender, EventArgs e)
    {

        ECompanyDatabase c = new ECompanyDatabase();
        c.CompanyDBID = CurID;
        db.select(dbConn, c);
        db.delete(dbConn, c);
        Response.Redirect("~/CompanyDB_List.aspx");
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/CompanyDB_List.aspx");
    }

    protected void Reset_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ECompanyDatabase obj = new ECompanyDatabase();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);

        if (db.select(dbConn, obj))
        {
            try
            {
                ResetLoginIDPasswordWithRandomKey(obj, false);
                errors.addError("The UserID/Password is reset to\r\nUserID: " + obj.CompanyDBResetDefaultUserLoginID + "\r\nPassword: " + obj.CompanyDBResetDefaultUserPassword);
            }
            catch (Exception ex)
            {
                errors.addError(ex.Message);
            }
        }
    }

    protected void ResetPassword_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ECompanyDatabase obj = new ECompanyDatabase();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);

        if (db.select(dbConn, obj))
        {
            try
            {
                ResetLoginIDPasswordWithRandomKey(obj, true);
                errors.addError("The UserID/Password is reset to\r\nUserID: " + obj.CompanyDBResetDefaultUserLoginID + "\r\nPassword: " + obj.CompanyDBResetDefaultUserPassword);
            }
            catch (Exception ex)
            {
                errors.addError(ex.Message);
            }
        }
    }

    protected void ResetLoginIDPasswordWithRandomKey(ECompanyDatabase obj, bool skipResetLoginID)
    {
        DatabaseConnection companyDBConn;

        try
        {
            companyDBConn = new DatabaseConnection(obj.getConnectionString(dbConn), DatabaseConnection.DatabaseType.MSSQL);
        }
        catch (Exception ex)
        {
            throw new Exception("Fail to connect to database");
        }


        HROne.Lib.Entities.EUser user = new HROne.Lib.Entities.EUser();
        user.UserID = 1;
        if (HROne.Lib.Entities.EUser.db.select(companyDBConn, user))
        {
            string randomPharse = RandomString(16);
            if (!skipResetLoginID)
                obj.CompanyDBResetDefaultUserLoginID = randomPharse.Substring(0, 8);
            else
                obj.CompanyDBResetDefaultUserLoginID = user.LoginID;
            obj.CompanyDBResetDefaultUserPassword = randomPharse.Substring(8);

            user.LoginID = obj.CompanyDBResetDefaultUserLoginID;
            user.UserPassword = HROne.CommonLib.Hash.PasswordHash(obj.CompanyDBResetDefaultUserPassword);
            user.UserAccountStatus = "A";
            user.UserChangePassword = true;
            user.FailCount = 0;
            HROne.Lib.Entities.EUser.db.update(companyDBConn, user);

            DBFilter SystemFunctionFilter = new DBFilter();
            SystemFunctionFilter.add(new IN("FunctionCode", "'SEC001', 'SEC002'", null));
            ArrayList systemFunctionList = HROne.Lib.Entities.ESystemFunction.db.select(companyDBConn, SystemFunctionFilter);
            foreach (HROne.Lib.Entities.ESystemFunction function in systemFunctionList)
            {
                DBFilter userGroupFunctionFilter = new DBFilter();
                userGroupFunctionFilter.add(new Match("FunctionID", function.FunctionID));
                userGroupFunctionFilter.add(new Match("FunctionAllowWrite", true));
                
                DBFilter userGroupAccessFilter = new DBFilter();
                userGroupAccessFilter.add(new Match("UserID", user.UserID));
                userGroupAccessFilter.add(new IN("UserGroupID", "SELECT ugf.UserGroupID FROM " + HROne.Lib.Entities.EUserGroupFunction.db.dbclass.tableName + " ugf", userGroupFunctionFilter));
                if (HROne.Lib.Entities.EUserGroupAccess.db.count(companyDBConn, userGroupAccessFilter)<=0)
                {
                    ArrayList UserGroupFunctionList = HROne.Lib.Entities.EUserGroupFunction.db.select(companyDBConn, userGroupFunctionFilter);
                    if (UserGroupFunctionList.Count > 0)
                    {
                        HROne.Lib.Entities.EUserGroupAccess access = new HROne.Lib.Entities.EUserGroupAccess();
                        access.UserID = user.UserID;
                        access.UserGroupID = ((HROne.Lib.Entities.EUserGroupFunction)UserGroupFunctionList[0]).UserGroupID;
                        HROne.Lib.Entities.EUserGroupAccess.db.insert(companyDBConn, access);
                    }
                }
            }

            ECompanyDatabase.db.update(dbConn, obj);
        }
        else
        {
            throw new Exception("Default User does not appear on company database");
        }
    }

    protected string RandomString(int length)
    {
        string resultString = string.Empty;
        Random random = new Random(DateTime.Now.Millisecond);

        while (resultString.Length < length)
        {
            int num = random.Next(62);
            if (num >= 0 & num <= 9)
                resultString += num.ToString();
            else if (num >= 10 & num <= 35)
                resultString += Convert.ToChar(num + 55);
            else if (num >= 36 & num <= 61)
                resultString += Convert.ToChar(num + 61);
        }
        return resultString;
    }
    protected string CreateClientCodePrefix(string CompanyName)
    {
        string[] words = CompanyName.Split(new char[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);
        string ClientCodePrefix=string.Empty;
        for (int idx = words.Length - 1; idx >= 0; idx--)
        {
            string word = words[idx].ToUpper();
            if (word[0] >= 65 && word[0] <= 90)
                ClientCodePrefix = word[0].ToString() + ClientCodePrefix;
        }
        return ClientCodePrefix;
    }
}
