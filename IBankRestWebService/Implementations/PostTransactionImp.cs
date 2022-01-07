using AdoNetCore.AseClient;
using IBankRestWebService.DataAccess;
using IBankRestWebService.Interfaces;
using IBankRestWebService.Parameters;
using IBankRestWebService.Utility;
using IBankRestWebService.ViewModel;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace IBankRestWebService.Implementations
{
    public class PostTransactionImp : IPostTransaction
    {
        public List<Dictionary<string, string>> TCIPost3(PostTransactionParam postTransactionParam, ApplicationsDbConnection conn)
        {
            Log.Warning("-------TCIPost3 starts-----------");

            string request = JsonConvert.SerializeObject(postTransactionParam);

            Log.Warning($"-------TCIPost3 Request: {request}");

            DataLayer dl = new DataLayer();

            List<AseParameter> sp = new List<AseParameter>()
            {
                new AseParameter() {ParameterName = "@psTransactionRef", AseDbType = AseDbType.VarChar, Value= System.Web.HttpUtility.HtmlDecode(postTransactionParam.TransReference)},
                new AseParameter() {ParameterName = "@psDrAcctNo", AseDbType = AseDbType.VarChar, Value= postTransactionParam.DrAcctNo},
                new AseParameter() {ParameterName = "@psDrAcctType", AseDbType = AseDbType.VarChar, Value=postTransactionParam.DrAcctType},
                new AseParameter() {ParameterName = "@pnDrAcctTC", AseDbType = AseDbType.Integer, Value= CheckIntDBNullValue(postTransactionParam.DrAcctCbsTC)},
                new AseParameter() {ParameterName = "@psDrAcctNarration", AseDbType = AseDbType.VarChar, Value= System.Web.HttpUtility.HtmlDecode(postTransactionParam.DrAcctNarration)},
                new AseParameter() {ParameterName = "@pnTranAmount", AseDbType = AseDbType.Decimal, Value= postTransactionParam.Amount},
                new AseParameter() {ParameterName = "@psCrAcctNo", AseDbType = AseDbType.VarChar, Value= postTransactionParam.CrAcctNo},
                new AseParameter() {ParameterName = "@psCrAcctType", AseDbType = AseDbType.VarChar, Value= postTransactionParam.CrAcctType},
                new AseParameter() {ParameterName = "@pnCrAcctTC", AseDbType = AseDbType.Integer, Value= CheckIntDBNullValue(postTransactionParam.CrAcctCbsTC)},
                new AseParameter() {ParameterName = "@psCrAcctNarration", AseDbType = AseDbType.VarChar, Value= System.Web.HttpUtility.HtmlDecode(postTransactionParam.CrAcctNarration)},
                new AseParameter() {ParameterName = "@psCurrencyISO", AseDbType = AseDbType.VarChar, Value= postTransactionParam.CcyCode},
                new AseParameter() {ParameterName = "@pdtPostDate", AseDbType = AseDbType.Date, Value= string.Format("{0:yyyyMMdd}", postTransactionParam.TransactionDate)},
                //new AseParameter() {ParameterName = "@pdtPostDate", AseDbType = AseDbType.Date, Value= postTransactionParam.TransactionDate.GetValueOrDefault().ToShortDateString()},
                new AseParameter() {ParameterName = "@pdtValueDate", AseDbType = AseDbType.Date, Value= string.Format("{0:yyyyMMdd}", postTransactionParam.ValueDate)},
                //new AseParameter() {ParameterName = "@pdtValueDate", AseDbType = AseDbType.Date, Value= postTransactionParam.ValueDate.GetValueOrDefault().ToShortDateString()},
                new AseParameter() {ParameterName = "@psUserName", AseDbType = AseDbType.VarChar, Value= postTransactionParam.CbsUserId},
                new AseParameter() {ParameterName = "@pnFloatDays", AseDbType = AseDbType.Integer, Value= DBNull.Value},
                new AseParameter() {ParameterName = "@psRoutingNo", AseDbType = AseDbType.Integer, Value= DBNull.Value},
                new AseParameter() {ParameterName = "@psSupervisorName", AseDbType = AseDbType.VarChar, Value= DBNull.Value},
                new AseParameter() {ParameterName = "@pnChannelId", AseDbType = AseDbType.Integer, Value= postTransactionParam.CbsChannelId},
                new AseParameter() {ParameterName = "@pnForcePostFlag", AseDbType = AseDbType.Integer, Value= postTransactionParam.ForceFloatFlag},
                new AseParameter() {ParameterName = "@pnReversal", AseDbType = AseDbType.Integer, Value= postTransactionParam.Reversal},
                new AseParameter() {ParameterName = "@pnTranBatchID", AseDbType = AseDbType.Integer, Value= DBNull.Value},
                new AseParameter() {ParameterName = "@pnParentTransRef", AseDbType = AseDbType.Integer, Value= NotNullInteger(postTransactionParam.ParentTransactionId)},
                new AseParameter() {ParameterName = "@pnDirection", AseDbType = AseDbType.Integer, Value= postTransactionParam.Direction},
                new AseParameter() {ParameterName = "@pnDrAcctChequeNo", AseDbType = AseDbType.Integer, Value= postTransactionParam.DrAcctChequeNo},
                new AseParameter() {ParameterName = "@pnDrAcctChargeCode", AseDbType = AseDbType.Integer, Value= postTransactionParam.DrAcctChgCode},
                new AseParameter() {ParameterName = "@pnDrAcctChargeAmt", AseDbType = AseDbType.Decimal, Value= postTransactionParam.DrAcctChgAmt},
                new AseParameter() {ParameterName = "@pnDrAcctTaxAmt", AseDbType = AseDbType.Decimal, Value= postTransactionParam.DrAcctTaxAmt},
                new AseParameter() {ParameterName = "@pnCrAcctChequeNo", AseDbType = AseDbType.Integer, Value= postTransactionParam.CrAcctChequeNo},
                new AseParameter() {ParameterName = "@pnCrAcctChargeCode", AseDbType = AseDbType.Integer, Value= postTransactionParam.CrAcctChgCode},
                new AseParameter() {ParameterName = "@pnCrAcctChargeAmt", AseDbType = AseDbType.Decimal, Value= postTransactionParam.CrAcctChgAmt},
                new AseParameter() {ParameterName = "@pnCrAcctTaxAmt", AseDbType = AseDbType.Decimal, Value= postTransactionParam.CrAcctTaxAmt},
                new AseParameter() {ParameterName = "@psTransTracer", AseDbType = AseDbType.VarChar, Value= postTransactionParam.TransTracer},
                new AseParameter() {ParameterName = "@psDrAcctChgDescr", AseDbType = AseDbType.VarChar, Value= postTransactionParam.DrAcctChgNarr},
                new AseParameter() {ParameterName = "@psCrAcctChgDescr", AseDbType = AseDbType.VarChar, Value= postTransactionParam.CrAcctChgNarr},
                new AseParameter() {ParameterName = "@pnDrAcctCashAmt", AseDbType = AseDbType.Decimal, Value= postTransactionParam.DrAcctCashAmt},
                new AseParameter() {ParameterName = "@pnCrAcctCashAmt", AseDbType = AseDbType.Decimal, Value= postTransactionParam.CrAcctCashAmt},
                new AseParameter() {ParameterName = "@pnEquivAmt", AseDbType = AseDbType.Decimal, Value= postTransactionParam.LcyEquivAmt},
                new AseParameter() {ParameterName = "@pnOrigExchRate", AseDbType = AseDbType.Decimal, Value= postTransactionParam.OrigExchRate},
                new AseParameter() {ParameterName = "@pnExchRate", AseDbType = AseDbType.Decimal, Value= postTransactionParam.ExchangeRate},
                new AseParameter() {ParameterName = "@psOriginChannel", AseDbType = AseDbType.VarChar, Value= postTransactionParam.OriginChannel},
                
                };


            string DB = AppSettingsConfig.DbToCall();

            string DBAndProcName = $"{DB}{"isp_tci_post3"}";


            var res =  dl.SqlDs(DBAndProcName, "PostTransaction", sp, conn.ConnectionString);
            Log.Warning($"-------TCIPost3 Response: {res}");
            return res;
            
        }

        public Task<int> TCIPost4(PostTransactionParam postTransactionParam)
        {
            throw new NotImplementedException();
        }

        private string NotNullInteger(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "0";
            else
                return value;
        }


        private object CheckIntDBNullValue(int? value)
        {
            if (value == null)
                return DBNull.Value;
            else
                return value;
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


    }
}




//using AdoNetCore.AseClient;
//using IBankRestWebService.DataAccess;
//using IBankRestWebService.Interfaces;
//using IBankRestWebService.Parameters;
//using IBankRestWebService.ViewModel;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;
//using Serilog;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Threading.Tasks;

//namespace IBankRestWebService.Implementations
//{
//    public class PostTransactionImp : IPostTransaction
//    { 

//        public List<Dictionary<string, string>> TCIPost3(PostTransactionParam postTransactionParam, ApplicationsDbConnection conn)
//        {
//            Log.Warning("---------------------- PostTransaction");
//            Log.Warning($"------Request-------- {JsonConvert.SerializeObject(postTransactionParam)}");
//            DataLayer dl = new DataLayer();

//            List<AseParameter> sp = new List<AseParameter>()
//            {
//                new AseParameter() {ParameterName = "@psTransactionRef", AseDbType = AseDbType.VarChar, Value= System.Web.HttpUtility.HtmlDecode(postTransactionParam.TransReference)},
//                new AseParameter() {ParameterName = "@psDrAcctNo", AseDbType = AseDbType.VarChar, Value= postTransactionParam.DrAcctNo},
//                new AseParameter() {ParameterName = "@psDrAcctType", AseDbType = AseDbType.VarChar, Value=postTransactionParam.DrAcctType},
//                new AseParameter() {ParameterName = "@pnDrAcctTC", AseDbType = AseDbType.Integer, Value= CheckIntDBNullValue(postTransactionParam.DrAcctCbsTC)},
//                new AseParameter() {ParameterName = "@psDrAcctNarration", AseDbType = AseDbType.VarChar, Value= System.Web.HttpUtility.HtmlDecode(postTransactionParam.DrAcctNarration)},
//                new AseParameter() {ParameterName = "@pnTranAmount", AseDbType = AseDbType.Decimal, Value= postTransactionParam.Amount},
//                new AseParameter() {ParameterName = "@psCrAcctNo", AseDbType = AseDbType.VarChar, Value= postTransactionParam.CrAcctNo},
//                new AseParameter() {ParameterName = "@psCrAcctType", AseDbType = AseDbType.VarChar, Value= postTransactionParam.CrAcctType},
//                new AseParameter() {ParameterName = "@pnCrAcctTC", AseDbType = AseDbType.Integer, Value= CheckIntDBNullValue(postTransactionParam.CrAcctCbsTC)},
//                new AseParameter() {ParameterName = "@psCrAcctNarration", AseDbType = AseDbType.VarChar, Value= System.Web.HttpUtility.HtmlDecode(postTransactionParam.CrAcctNarration)},
//                new AseParameter() {ParameterName = "@psCurrencyISO", AseDbType = AseDbType.VarChar, Value= postTransactionParam.CcyCode},
//                new AseParameter() {ParameterName = "@pdtPostDate", AseDbType = AseDbType.Date, Value= postTransactionParam.TransactionDate},
//                //new AseParameter() {ParameterName = "@pdtPostDate", AseDbType = AseDbType.Date, Value= postTransactionParam.TransactionDate.GetValueOrDefault().ToShortDateString()},
//                new AseParameter() {ParameterName = "@pdtValueDate", AseDbType = AseDbType.Date, Value= postTransactionParam.ValueDate},
//                //new AseParameter() {ParameterName = "@pdtValueDate", AseDbType = AseDbType.Date, Value= postTransactionParam.ValueDate.GetValueOrDefault().ToShortDateString()},
//                new AseParameter() {ParameterName = "@psUserName", AseDbType = AseDbType.VarChar, Value= postTransactionParam.CbsUserId},
//                new AseParameter() {ParameterName = "@pnFloatDays", AseDbType = AseDbType.Integer, Value= DBNull.Value},
//                new AseParameter() {ParameterName = "@psRoutingNo", AseDbType = AseDbType.Integer, Value= DBNull.Value},
//                new AseParameter() {ParameterName = "@psSupervisorName", AseDbType = AseDbType.VarChar, Value= DBNull.Value},
//                new AseParameter() {ParameterName = "@pnChannelId", AseDbType = AseDbType.Integer, Value= postTransactionParam.CbsChannelId},
//                new AseParameter() {ParameterName = "@pnForcePostFlag", AseDbType = AseDbType.Integer, Value= postTransactionParam.ForceFloatFlag},
//                new AseParameter() {ParameterName = "@pnReversal", AseDbType = AseDbType.Integer, Value= postTransactionParam.Reversal},
//                new AseParameter() {ParameterName = "@pnTranBatchID", AseDbType = AseDbType.Integer, Value= DBNull.Value},
//                new AseParameter() {ParameterName = "@pnParentTransRef", AseDbType = AseDbType.Integer, Value= NotNullInteger(postTransactionParam.ParentTransactionId)},
//                new AseParameter() {ParameterName = "@pnDirection", AseDbType = AseDbType.Integer, Value= postTransactionParam.Direction},
//                new AseParameter() {ParameterName = "@pnDrAcctChequeNo", AseDbType = AseDbType.Integer, Value= postTransactionParam.DrAcctChequeNo},
//                new AseParameter() {ParameterName = "@pnDrAcctChargeCode", AseDbType = AseDbType.Integer, Value= postTransactionParam.DrAcctChgCode},
//                new AseParameter() {ParameterName = "@pnDrAcctChargeAmt", AseDbType = AseDbType.Decimal, Value= postTransactionParam.DrAcctChgAmt},
//                new AseParameter() {ParameterName = "@pnDrAcctTaxAmt", AseDbType = AseDbType.Decimal, Value= postTransactionParam.DrAcctTaxAmt},
//                new AseParameter() {ParameterName = "@pnCrAcctChequeNo", AseDbType = AseDbType.Integer, Value= postTransactionParam.CrAcctChequeNo},
//                new AseParameter() {ParameterName = "@pnCrAcctChargeCode", AseDbType = AseDbType.Integer, Value= postTransactionParam.CrAcctChgCode},
//                new AseParameter() {ParameterName = "@pnCrAcctChargeAmt", AseDbType = AseDbType.Decimal, Value= postTransactionParam.CrAcctChgAmt},
//                new AseParameter() {ParameterName = "@pnCrAcctTaxAmt", AseDbType = AseDbType.Decimal, Value= postTransactionParam.CrAcctTaxAmt},
//                new AseParameter() {ParameterName = "@psTransTracer", AseDbType = AseDbType.VarChar, Value= postTransactionParam.TransTracer},
//                new AseParameter() {ParameterName = "@psDrAcctChgDescr", AseDbType = AseDbType.VarChar, Value= postTransactionParam.DrAcctChgNarr},
//                new AseParameter() {ParameterName = "@psCrAcctChgDescr", AseDbType = AseDbType.VarChar, Value= postTransactionParam.CrAcctChgNarr},
//                new AseParameter() {ParameterName = "@pnDrAcctCashAmt", AseDbType = AseDbType.Decimal, Value= postTransactionParam.DrAcctCashAmt},
//                new AseParameter() {ParameterName = "@pnCrAcctCashAmt", AseDbType = AseDbType.Decimal, Value= postTransactionParam.CrAcctCashAmt},
//                new AseParameter() {ParameterName = "@pnEquivAmt", AseDbType = AseDbType.Decimal, Value= postTransactionParam.LcyEquivAmt},
//                new AseParameter() {ParameterName = "@pnOrigExchRate", AseDbType = AseDbType.Decimal, Value= postTransactionParam.OrigExchRate},
//                new AseParameter() {ParameterName = "@pnExchRate", AseDbType = AseDbType.Decimal, Value= postTransactionParam.ExchangeRate},

//            };

//            var res = dl.SqlDs("isp_tci_post3", "PostTransaction", sp, conn.ConnectionString);

//            Log.Warning($"------Response-------- {JsonConvert.SerializeObject(res)}");

//            return res;
//        }

//        public Task<int> TCIPost4(PostTransactionParam postTransactionParam)
//        {
//            throw new NotImplementedException();
//        }

//        private string NotNullInteger(string value)
//        {
//            if (string.IsNullOrWhiteSpace(value))
//                return "0";
//            else
//                return value;
//        }


//        private object CheckIntDBNullValue(int? value)
//        {
//            if (value == null)
//                return DBNull.Value;
//            else
//                return value;
//        }

//        private List<Dictionary<string, string>> ReturnObject(DataSet dt)
//        {
//            List<Dictionary<string, string>> keyValuePairs = new List<Dictionary<string, string>>();

//            DataTable dataTable = dt.Tables[0];

//            foreach (DataRow r in dt.Tables[0].Rows)
//            {
//                var dict = new Dictionary<string, string>();
//                for (int i = 0; i < dataTable.Columns.Count; i++)
//                {
//                    dict.Add(dataTable.Columns[i].ToString(), r.ItemArray.GetValue(i).ToString());
//                }

//                keyValuePairs.Add(dict);

//            }
//            return keyValuePairs;
//        }


//    }
//}
