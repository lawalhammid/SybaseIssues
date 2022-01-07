using AdoNetCore.AseClient;
using IBankRestWebService.DataAccess;
using IBankRestWebService.Helper;
using IBankRestWebService.Interfaces;
using IBankRestWebService.Parameters;
using IBankRestWebService.Utility;
using IBankRestWebService.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IBankRestWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebRestService : ControllerBase
    {
        private readonly IPostTransaction postTransaction;
        private readonly IBalanceEnquiry balanceEnquiry;
        private readonly IAuth auth;
        private readonly FormatterValidation _FormatterValidation;

        public WebRestService
            (
                    IPostTransaction postTransaction,
                    IBalanceEnquiry balanceEnquiry,
                    IAuth auth, FormatterValidation FormatterValidation
            )
        {
            this.postTransaction = postTransaction;
            this.balanceEnquiry = balanceEnquiry;
            this.auth = auth;

            _FormatterValidation =  FormatterValidation;
        }


        [HttpPost("StartOfDay")]
        public async Task<IActionResult> StartOfDay(ApplicationsDbConnection connection)
        {
            DataLayer dl = new DataLayer();
           
            Log.Warning($"------Request StartOfDay:{JsonConvert.SerializeObject(connection)}");
            
            try
            {
                var dt = dl.SqlDs("isp_tci_startday", "StartDay", null, connection.ConnectionString);

                Log.Warning($"------Response StartOfDay:{JsonConvert.SerializeObject(dt)}");


                var currentDate = _FormatterValidation.FormatToDateYearMonthDayWithSlash(dt.FirstOrDefault().GetValueOrDefault("dtCurDt"));
                Log.Warning($"------Response StartOfDay: {currentDate}");
                return Ok(currentDate);
            }
            catch (Exception ex)
            {
                Log.Warning($"StartOfDay error: {ex.StackTrace}");
                return BadRequest(ex.Message ?? ex.InnerException.Message);
            }
        }


        

        [HttpPost("TestConnectionOld")]
        public IActionResult TestConnectionOld(ConnectionDetail param)
        {
            var rtv = new ConnectionResponse();
            try
            {
                Log.Warning("-------TestConnection starts-----------");

                //string connstring = $"Data Source={param.IP}; port={param.Port};database={param.DataBaseName};uid={param.UserName};pwd={param.Password};charset=iso_1;Min Pool Size=200;Max Pool Size=5000; ConnectionIdleTimeout=60;";
                string connstring = $"Data Source={param.IP}; port={param.Port};database={param.DataBaseName};uid={param.UserName};pwd={param.Password};charset=iso_1;Min Pool Size=200;Max Pool Size=5000; ConnectionIdleTimeout=60;";

                string sqlConn = AppConfiguration.GetConnString("ConnectionStrings:DefaultConnection");

                Log.Warning($"Connection Core Banking: {connstring}");

                Log.Warning($"Connection Core SQL Server: {sqlConn}");
                
                //using (SqlConnection theCons = new SqlConnection(sqlConn))
                //{
                //    DataSet ds = new DataSet();
                //    DataTable dt = new DataTable();
                //    theCons.Open();
                //    Log.Warning($"SQL Connection Connected Successful");
                //    theCons.Close();
                //}

               
                //Try to add line 104 to your code before making calls to connect to your sybase. this would solve the issue
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                
                using  (AseConnection theCons = new AseConnection(connstring))
                {
                    theCons.Open();

                    rtv.ErrorCode = 0;
                    rtv.sErrorText = "Success";

                    Log.Warning($"Sybase Connection Core Connected Successful");

                    theCons.Close();
                    theCons.Dispose();
                    theCons.DisposeAsync();
                    
                    return Ok(rtv);
                }
            }
            catch (Exception ex)
            {
                var exM = ex.Message == null ? ex.InnerException.Message : ex.Message;
                //Log.Warning("-------PostTransaction-----------");
                Log.Warning($"Conn error**> {exM}");
                Log.Warning($"Conn error stacktrace**> {ex.StackTrace}");
                rtv.sErrorText = exM;
                return BadRequest(rtv);
            }
        }

        [HttpPost("TestConnection")]
        public async Task<IActionResult> Connection(ConnectionDetail param)
        {
            var rtv = new ConnectionResponse();
            try
            {
                //Below is the connectionString. You can replace the values accordingly to suit your database connection
                string connstring = $"Data Source={param.IP}; port={param.Port};database={param.DataBaseName};uid={param.UserName};pwd={param.Password};charset=iso_1;Min Pool Size=200;Max Pool Size=5000; ConnectionIdleTimeout=60;";
                
                //Try to add line 143 to your code before making calls to connect to your sybase. this would solve the issue

                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (AseConnection theCons = new AseConnection(connstring))
                {
                    await theCons.OpenAsync();

                    //fetch or add record to your sybase database

                    rtv.ErrorCode = 0;
                    rtv.sErrorText = "Connected Successfully!";

                    await theCons.CloseAsync();
                    await theCons.DisposeAsync();

                    return Ok(rtv);
                }
            }
            catch (Exception ex)
            {
                var exM = ex.Message == null ? ex.InnerException.Message : ex.Message;
                rtv.ErrorCode = -1;
                rtv.sErrorText = "Error while connecting to Sybase";

                rtv.sErrorText = exM;
                return BadRequest(rtv);
            }
        }

        [HttpPost("AuthenticateUser")]
        public IActionResult UserAuthentication(AuthRequest request) => Ok(auth.AccountAuth(request.AuthParam, request.Connection));


        [HttpPost("validateAccount")]
        public IActionResult AccountValidation(BalanceEnquiryRequest request) => Ok(balanceEnquiry.AcctValidation(request.EquiryParameter, request.Connection));


        [HttpPost("transaction")]
        public IActionResult CBPostTransaction(PostTransactionRequest request) => Ok(postTransaction.TCIPost3(request.PostParameters, request.ConnectionStrg));


        //============================================= PRIVATE METHODS ===========================================================

        private List<Dictionary<string, string>> ErrorReturnObject()
        {
            List<Dictionary<string, string>> keyValuePairs = new List<Dictionary<string, string>>();
            var dict = new Dictionary<string, string>();
            dict.Add("ErrorMessage", "Transaction was not posted. Kindly check your parameters and try again");
            keyValuePairs.Add(dict);


            return keyValuePairs;
        }


        private List<Dictionary<string, string>> ReturnObject(DataSet dt)
        {
            List<Dictionary<string, string>> keyValuePairs = new List<Dictionary<string, string>>();

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

        private object CheckDBNullValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return DBNull.Value;
            else
                return value;
        }

        private string NotNullInteger(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "0";
            else
                return value;
        }
    }
}

//var dict = new Dictionary<string, object> { { "Property", "foo" } };
//dynamic eo = dict.Aggregate(new ExpandoObject() as IDictionary<string, Object>,
//                            (a, p) => { a.Add(p.Key, p.Value); return a; });
//string value = eo.Property;

//var rows = dataTable.Select().ToList().Select(x=>x.ItemArray).ToList();

//List<BalanceEnquiry> studentList = new List<BalanceEnquiry>();
//studentList = (from DataRow dr in dt.Tables[0].Rows
//               select new BalanceEnquiry()
//               {
//                    AcctName = dr["AcctName"].ToString(),
//                   ErrorCode = dr["ErrorCode"].ToString(),
//                   AcctNo = dr["AcctNo"].ToString(),
//                   Branch = dr["Branch"].ToString()
//               }).ToList();


//return Ok
//    (
//        studentList
//    );