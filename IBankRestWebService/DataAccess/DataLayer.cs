using AdoNetCore.AseClient;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace IBankRestWebService.DataAccess
{
    public class RTGSReference
    {
        public string ReferenceNo { set; get; }
        public int ErrorCode { set; get; }
        public string ErrorMessage { set; get; }
    }
    public class ChequeNumberRange
    {
        public int LastChequeNumber { set; get; }
        public int ErrorCode { set; get; }
        public string ErrorMessage { set; get; }
    }
    public class ClassCodeMapping
    {
        public string StaffAccount { set; get; }
        public int? ClassCode { set; get; }
        public string IBType { set; get; }
    }
    public class CorporateIbankLoginDetail
    {
        public string LoginId { set; get; }

    }
    public class AuthenticateDetails
    {
        public string Reponse { set; get; }
    }


    public class responseValues
    {
        public int ErrorCode { set; get; }

    }
    public class InterfaceAcctResponseValues
    {
        public string GamSwitchTransientAcctNo { set; get; }
        public string GamSwitchTransientAcctType { set; get; }
        public string GamSwitchTransientAcctName { set; get; }

    }

    public class InwardAcctResponseValues
    {
        public string GamSwitchInterBankInterfaceAcctNo { set; get; }
        public string GamSwitchInterBankInterfaceAcctType { set; get; }
        public string GamSwitchInterBankInterfaceAcctName { set; get; }

    }

    public class BatchCounterResponseValues
    {
        public int counter { set; get; }


    }


    public class ServiceSetUpResponseValues
    {
        public string GamSwitchOutwardKey { set; get; }
        public string GamSwitchToken { set; get; }
        public string GamSwitchUrl { get; set; }
        public string GamSwitchKey { get; set; }
        public string MobileGamSwitchToken { get; set; }
        public string MobileGamSwitchOutwardKey { get; set; }
    }

    public class LocalCurrencyResponseValues
    {
        public string LocalCrncy { set; get; }


    }



    public class details
    {
        public string server { set; get; }
        public int port { set; get; }
        public string databasename { set; get; }
        public string userid { set; get; }
        public string password { set; get; }
        public string TokenUrl { set; get; }
    }
    public class DataFields
    {
        public AuthenticateDetails TokenResponseDetails(string data)
        {
            AuthenticateDetails po = new AuthenticateDetails();
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(data);
            short respCode = -1;
            string respMsg = string.Empty;
            foreach (XmlNode node in xdoc.GetElementsByTagName("AuthenticateUserResult"))
            {
                //SaveLog("XML NODE DATA" + node.InnerText);
                po.Reponse = node.InnerText;

            }
            return po;
        }
       
        
        //private Object cvLockObject = new Object();
        
        //string cvsLogFile = System.Configuration.ConfigurationManager.AppSettings["LogFile"];
        
        //string filePath = System.Configuration.ConfigurationManager.AppSettings["LogFilePath"];
        
        //string LogSize = System.Configuration.ConfigurationManager.AppSettings["LogSize"];
        //public void SaveLog(string psDetails)
        //{
        //    FileInfo f = new FileInfo(cvsLogFile);

        //    if (File.Exists(cvsLogFile))
        //    {
        //        long s1 = f.Length;
        //        if (s1 > Convert.ToInt32(LogSize))
        //        {
        //            string filename = Path.GetFileNameWithoutExtension(cvsLogFile) + string.Format("{0:yyyyMMdd}", DateTime.Now) + ".txt";
        //            //filename = GetNextFilename(filename);
        //            if (File.Exists(Path.Combine(filePath, filename)))
        //                File.Delete(Path.Combine(filePath, filename));

        //            File.Move(cvsLogFile, Path.Combine(filePath, filename));

        //            f.Delete();
        //        }
        //    }
        //    lock (cvLockObject)
        //    {
        //        string sError = DateTime.Now.ToString() + ": " + psDetails;
        //        StreamWriter sw = new StreamWriter(cvsLogFile, true, Encoding.ASCII);
        //        sw.WriteLine(sError);
        //        sw.Close();
        //    }
        //}
        public string GetNextFilename(string filename)
        {
            int i = 1;
            string dir = Path.GetDirectoryName(filename);
            string file = Path.GetFileNameWithoutExtension(filename) + "{0}";
            string extension = Path.GetExtension(filename);

            while (File.Exists(filename))
                filename = Path.Combine(dir, string.Format(file, "(" + i++ + ")") + extension);

            return filename;
        }

        public string SoapRequest(string path, string[] data)
        {

            details d = new details();
            d.TokenUrl = GetConnectionDetails().Rows[0]["TokenWebUrl"].ToString();
            //SaveLog("Token Url " + d.TokenUrl);
            string MyData = string.Empty;
            string rst = string.Empty;
            try
            {

                using (WebClient client = new WebClient())
                {
                    client.Proxy = null;
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/soap+xml; charset=utf-8");
                    client.Headers.Add("SOAPAction", "http://tempuri.org");
                    StreamReader reader = new StreamReader(path);


                    MyData = String.Format(reader.ReadToEnd(), data);
                    reader.Close();
                    //SaveLog("My Data Xml" + MyData);
                    //SaveLog("TToken Url Before passing it " + d.TokenUrl);
                    rst = client.UploadString(new Uri(d.TokenUrl), MyData);
                    //SaveLog("Xml after token is called  " + rst);
                }

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.ServerCertificateValidationCallback = MyCertHandler;
                return rst;
            }
            catch (Exception ex)
            {
                //SaveLog("Exception " + ex.Message);
                //ExceptionLog log = new ExceptionLog()
                //{
                //    PageName = controller,
                //    ExceptionMessage = ex.Message,
                //    ExceptionDate = DateTime.Now
                //};

                //new ExceptionUtil().SaveLog(log);
            }
            return rst;
        }
        private static bool MyCertHandler(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors error)
        {
            return true;
        }
        public string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        private string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        public details GetDetails()
        {
            details d = new details();
            var dt = GetConnectionDetails();
            Log.Warning($"------GetConnectionDetails success");
            //var a = Encrypt("Password1");
            d.databasename = dt.Rows[0]["DataBaseName"].ToString();
            d.port = Convert.ToInt16(dt.Rows[0]["DatabasePort"].ToString());
            d.server = dt.Rows[0]["server"].ToString();
            d.userid = dt.Rows[0]["UserName"].ToString();
            d.password = dt.Rows[0]["Password"].ToString();// Decrypt(dt.Rows[0]["Password"].ToString());
            Log.Warning($"------GetConnectionDetails returns");
            return d;
        }

        public IConfiguration Configuration { get; }

        private DataTable GetConnectionDetails()
        {
            Log.Warning($"------GetConnectionDetails starts");

            string connstring = AppConfiguration.GetConnString("ConnectionStrings:DefaultConnection");


            using (SqlConnection theCons = new SqlConnection(connstring))
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                theCons.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = theCons;
                    cmd.CommandText = "GetDataDetails";
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = cmd.ExecuteReader();
                    ds.Load(reader, LoadOption.OverwriteChanges, "Results");
                    dt = ds.Tables[0];
                    reader.Close();
                    theCons.Close();

                }
                catch (Exception ex)
                {

                }

                return dt;
            }
        }
        //List<AseParameter> parameterPasses,

        public DataTable GetConnectionDetails2(string sql)
        {
            string connstring = Configuration.GetConnectionString("DefaultConnection").ToString();
            using (SqlConnection theCons = new SqlConnection(connstring))
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                theCons.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = theCons;
                    cmd.CommandText = sql;
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.Text;

                    SqlDataReader reader = cmd.ExecuteReader();
                    ds.Load(reader, LoadOption.OverwriteChanges, "Results");
                    dt = ds.Tables[0];
                    reader.Close();
                    theCons.Close();

                }
                catch (Exception ex)
                {

                }

                return dt;
            }
        }

        public List<ClassCodeMapping> GetClassCodeMapping()
        {
            var f = new List<ClassCodeMapping>();
            foreach (DataRow d in GetConnectionDetails2("select staffaccount, classcode,Ibtype from ClassCodeMapping where status ='A'").Rows)
            {
                f.Add(new ClassCodeMapping()
                {
                    ClassCode = Convert.ToInt32(d[1].ToString()),
                    StaffAccount = d[0].ToString(),
                    IBType = d[2].ToString()
                });
            }
            return f;
        }
        public List<CorporateIbankLoginDetail> GetCorporateUserId(string acctno)
        {
            var f = new List<CorporateIbankLoginDetail>();
            foreach (DataRow d in GetConnectionDetails2("select loginid from CompanyUserProfile where accountno = '" + acctno + "'").Rows)
            {
                f.Add(new CorporateIbankLoginDetail()
                {
                    //LoginId = d[0].ToString() + "_" + System.Configuration.ConfigurationManager.AppSettings["IBLoc"]
                    LoginId = d[0].ToString() + "_" + Configuration.GetSection("IBLoc")

                });
            }
            return f;
        }
    }

    public class DataLayer
    {
        IConfiguration Configuration { get; }

        
        public DataSet SqlDs1(string SqlString, string TableName, List<AseParameter> parameterPasses, int i)
        {
            DataFields d = new DataFields();
            details de = d.GetDetails();
            //string connstring = Configuration.GetConnectionString("SybaseConnection").ToString();
            string connstring = AppConfiguration.GetConnString("ConnectionStrings:SybaseConnection");
            connstring = connstring.Replace("{{Data Source}}", "192.168.1.214"); //"192.168.1.214"); de.server);
            connstring = connstring.Replace("{{port}}", de.port.ToString());
            connstring = connstring.Replace("{{database}}", de.databasename);
            connstring = connstring.Replace("{{uid}}", de.userid);
            connstring = connstring.Replace("{{pwd}}", de.password);
            using (AseConnection theCons = new AseConnection(connstring))
            {
                DataSet ds = new DataSet();

                theCons.Open();
                try
                {

                    AseCommand cmd = new AseCommand();
                    cmd.Connection = theCons;

                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.Text;//i == 0 ? CommandType.StoredProcedure : CommandType.Text;
                                                       //if (parameterPasses != null)
                                                       //  cmd.Parameters.AddRange(parameterPasses.ToArray());
                    string qry = string.Empty;
                    List<string> sep = new List<string>();
                    sep.Add("AnsiString");// DateTime";
                    sep.Add("Date");
                    string fdf = string.Empty;
                    if (parameterPasses != null)
                    {
                        foreach (var t in parameterPasses.ToArray())
                        {
                            //sep=if t.DbType in () then "'" else ""
                            //sep = string.Empty;
                            fdf = string.Empty;
                            if (sep.Contains(t.DbType.ToString()))
                            {
                                fdf = "'" + t.Value.ToString() + "',";
                            }
                            else
                            {
                                t.Value = t.Value == null ? "NULL" : t.Value.ToString();
                                if (string.IsNullOrEmpty(t.Value.ToString()))
                                {
                                    fdf = "NULL,";
                                }
                                else
                                {
                                    fdf = t.Value.ToString() + ",";
                                }
                            }

                            qry += t.ToString() + "=" + fdf;

                        }

                        cmd.CommandText = SqlString + " " + qry.TrimEnd(',');
                        //d.SaveLog("Parameter is not null " + cmd.CommandText);
                    }
                    else
                    {
                        cmd.CommandText = SqlString;
                        //d.SaveLog("Parameter is null " + cmd.CommandText);
                    }

                    //d.SaveLog("Before Execution");

                    IDataReader reader = cmd.ExecuteReader();

                    ds.Load(reader, LoadOption.OverwriteChanges, TableName);
                    reader.Close();
                    theCons.Close();

                }
                catch (Exception ex)
                {
                    //d.SaveLog("Exception from Procedure call " + ex.Message == null ? ex.InnerException.Message : ex.Message);
                }

                return ds;
            }
        }

        public List<Dictionary<string, string>> SqlDs(string SqlString, string TableName, List<AseParameter> parameterPasses, string con)
        {
            DataFields d = new DataFields();

            details de = d.GetDetails();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (AseConnection theCons = new AseConnection(con))
            {
                DataSet dataSet = new DataSet();

                theCons.Open();
                try
                {
                    AseCommand cmd = new AseCommand();
                    cmd.Connection = theCons;

                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.Text;//i == 0 ? CommandType.StoredProcedure : CommandType.Text;
                                                       //if (parameterPasses != null)
                                                       //  cmd.Parameters.AddRange(parameterPasses.ToArray());
                    string queryString  = string.Empty;
                   
                    List<string> sep = new List<string>();
                    sep.Add("AnsiString");
                    sep.Add("Date");
                    
                    
                    if (parameterPasses != null)
                    {
                        foreach (var paramter in parameterPasses.ToArray())
                        {
                            string paramValue = string.Empty;

                            if (paramter != null)
                            {
                                if (sep.Contains(paramter.DbType.ToString()))
                                {
                                   if(paramter.Value != null)paramValue = "'" + paramter.Value.ToString() + "',";
                                    if (paramter.Value == null) paramValue = "NULL,";
                                }
                                else
                                {
                                    paramter.Value = paramter.Value == null ? "NULL" : paramter.Value.ToString();
                                    if (string.IsNullOrEmpty(paramter.Value.ToString()))
                                    {
                                        paramValue = "NULL,";
                                    }
                                    else
                                    {
                                        paramValue = paramter.Value.ToString() + ",";
                                    }
                                }
                            }

                            queryString += paramter.ToString() + "=" + paramValue;
                        }

                        cmd.CommandText = SqlString + " " + queryString.TrimEnd(',');
                       
                    }
                    else
                    {
                        cmd.CommandText = SqlString;          
                    }

                    //d.SaveLog("Before Execution");

                    IDataReader reader = cmd.ExecuteReader();

                    dataSet.Load(reader, LoadOption.OverwriteChanges, TableName);

                    Log.Warning($"Core banking executed successfully");

                    reader.Close();
                    theCons.Close();
                    theCons.Dispose();
                    theCons.DisposeAsync();
                }
                catch (Exception ex)
                {
                    string errMsg = string.IsNullOrWhiteSpace(ex.Message) ? ex.InnerException.Message : ex.Message;
                    Log.Error($"----------------------------Exception when trying to connect to core banking with error message: {errMsg}");
                    return ErrorReturnObject(errMsg);


                    
                    theCons.Close();
                    theCons.CloseAsync();
                    theCons.Dispose();
                    theCons.DisposeAsync();

                }

                return CoreBankingReturnResults(dataSet);
            }
        }

        private List<Dictionary<string, string>> CoreBankingReturnResults(DataSet dt)
        {
            List<Dictionary<string, string>> keyValuePairs = new List<Dictionary<string, string>>();

            if (dt.Tables.Count < 1)
            {
                return ErrorReturnObject();
            }


            DataTable dataTable = dt.Tables[0];

            foreach (DataRow r in dt.Tables[0].Rows)
            {
                var dict = new Dictionary<string, string>();
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    dict.Add(dataTable.Columns[i].ToString(), r.ItemArray.GetValue(i).ToString());
                }

                keyValuePairs.Add(dict);

            }
            return keyValuePairs;
        }



        public ChequeNumberRange GetChequeNumber(string accountNo, string AcctType)
        {
            ChequeNumberRange chequerange = new ChequeNumberRange();
            int lastchequenumber = -1;
            DataFields d = new DataFields();
            details de = d.GetDetails();
            string connstring = Configuration.GetConnectionString("SybaseConnection").ToString();
            connstring = connstring.Replace("{{Data Source}}", de.server);
            connstring = connstring.Replace("{{port}}", de.port.ToString());
            connstring = connstring.Replace("{{database}}", de.databasename);
            connstring = connstring.Replace("{{uid}}", de.userid);
            connstring = connstring.Replace("{{pwd}}", de.password);
            using (AseConnection theCons = new AseConnection(connstring))
            {
                DataSet ds = new DataSet();

                theCons.Open();
                try
                {

                    using (AseCommand cmd = new AseCommand())
                    {
                        cmd.Connection = theCons;
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = @"isp_get_MaxChqNo";
                        var account_param = cmd.Parameters.AddWithValue("@psAccountNo", accountNo);
                        var accounttype_param = cmd.Parameters.AddWithValue("@psAccountType", AcctType);
                        cmd.Parameters.Add(account_param);
                        cmd.Parameters.Add(accounttype_param);
                        AseParameter lastchqno = new AseParameter("@pnLastChqNo", AseDbType.Integer);
                        lastchqno.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(lastchqno);
                        AseParameter errorcode = new AseParameter("@rnErrorCode", AseDbType.Integer);
                        errorcode.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(errorcode);
                        AseParameter errormessage = new AseParameter("@rsErrorMsg", AseDbType.VarChar);
                        errormessage.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(errormessage);

                        using (AseDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                //lastchequenumber = Convert.ToInt32(reader[0]);
                            }

                            reader.Close();

                        }
                        chequerange.LastChequeNumber = Convert.ToInt32(cmd.Parameters["@pnLastChqNo"].Value);
                        chequerange.ErrorCode = Convert.ToInt32(cmd.Parameters["@rnErrorCode"].Value);
                        chequerange.ErrorMessage = cmd.Parameters["@rsErrorMsg"].Value.ToString();

                    }

                    theCons.Close();

                }
                catch (Exception ex)
                {
                    //d.SaveLog("Exception from Procedure call " + ex.Message == null ? ex.InnerException.Message : ex.Message);
                }

                return chequerange;
            }
        }
        public RTGSReference GetReferenceNumber(string Channel)
        {
            RTGSReference rtgsreference = new RTGSReference();
            int lastchequenumber = -1;
            DataFields d = new DataFields();
            //string connstr = Configuration.GetConnectionString("TechClearingConnection");
            string connstr = AppConfiguration.GetConnString("ConnectionStrings:TechClearingConnection");
            
            using (SqlConnection theCons = new SqlConnection(connstr))
            {
                DataSet ds = new DataSet();

                theCons.Open();
                try
                {

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = theCons;
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = @"isp_GetSwiftRef";
                        cmd.Parameters.Add(new SqlParameter() { ParameterName = "psChannel", Value = Channel, Size = 50 });

                        SqlParameter referenceno = new SqlParameter("@rsReferenceNo", SqlDbType.VarChar, 50);
                        referenceno.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(referenceno);
                        SqlParameter errorcode = new SqlParameter("@rnErrorCode", SqlDbType.Int);
                        errorcode.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(errorcode);
                        SqlParameter errormessage = new SqlParameter("@rsErrorMsg", SqlDbType.VarChar, 200);
                        errormessage.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(errormessage);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                //lastchequenumber = Convert.ToInt32(reader[0]);
                            }

                            reader.Close();

                        }
                        rtgsreference.ReferenceNo = cmd.Parameters["@rsReferenceNo"].Value.ToString();
                        rtgsreference.ErrorCode = Convert.ToInt32(cmd.Parameters["@rnErrorCode"].Value);
                        rtgsreference.ErrorMessage = cmd.Parameters["@rsErrorMsg"].Value.ToString();

                    }

                    theCons.Close();

                }
                catch (Exception ex)
                { 
                   // d.SaveLog("Exception from Procedure call " + ex.Message == null ? ex.InnerException.Message : ex.Message);
                }

                return rtgsreference;
            }
        }

        public DataSet TechClearingconDs(List<SqlParameter> parameter, string procname, int sqltype)
        {

            //oSmartObject = new SmartObject();
            string connstr = Configuration.GetConnectionString("TechClearingConnection").ToString();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataFields df = new DataFields();
            try
            {
                using (var con = new SqlConnection(connstr))
                {
                    con.Open();

                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = sqltype == 0 ? CommandType.StoredProcedure : CommandType.Text;
                        cmd.CommandText = procname;
                        if (parameter != null)
                            cmd.Parameters.AddRange(parameter.ToArray());
                        //for (int i = 0; i < cmd.Parameters.Count; i++)
                        //    oSmartObject.SaveLog(cmd.Parameters[i].ParameterName.ToString() + "-" + cmd.Parameters[i].Value.ToString());
                        SqlDataReader dr = cmd.ExecuteReader();

                        ds.Load(dr, LoadOption.OverwriteChanges, "Results");
                        dt = ds.Tables[0];
                        //oSmartObject.SaveLog("Reports get data " + dt.Rows.Count);

                        dr.Close();

                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {

                //df.SaveLog("Reports " + ex.Message == null ? ex.InnerException.Message : ex.Message);

            }

            return ds;
        }



        private List<Dictionary<string, string>> ErrorReturnObject(string err = "")
        {
            List<Dictionary<string, string>> keyValuePairs = new List<Dictionary<string, string>>();
            var dict = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(err))
            {
                dict.Add("ErrorMessage", "Transaction was not posted. Kindly check your parameters and try again");
            }
            else
            {
                dict.Add("ErrorMessage", err);
            }


            keyValuePairs.Add(dict);


            return keyValuePairs;
        }
    }


    public static class AppConfiguration
    {
        public static string GetConnString(string conn)
        {
            var configBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configBuilder.AddJsonFile(path, false);
            var root = configBuilder.Build();
            var appSetting = root.GetSection(conn);
            //sqlConnectonString = appSetting.Value;
            return appSetting.Value;
        }
        //public string sqlConnectonString { get; set; }
        //TechClearingConnection
        public static string GetTechConnString()
        {
            var configBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configBuilder.AddJsonFile(path, false);
            var root = configBuilder.Build();
            var appSetting = root.GetSection("ConnectionStrings:TechClearingConnection");
            //sqlConnectonString = appSetting.Value;
            return appSetting.Value;
        }

       
    }
}

//string connstring = Configuration.GetConnectionString("SybaseConnection").ToString();
//string connstring = AppConfiguration.GetConnString("ConnectionStrings:SybaseConnection");
//connstring = connstring.Replace("{{Data Source}}", "192.168.1.214"); //"192.168.1.214"); de.server);
//connstring = connstring.Replace("{{port}}", de.port.ToString());
//connstring = connstring.Replace("{{database}}", de.databasename);
//connstring = connstring.Replace("{{uid}}", de.userid);
//connstring = connstring.Replace("{{pwd}}", de.password);