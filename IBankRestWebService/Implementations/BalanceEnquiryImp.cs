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
using System.Linq;
using System.Threading.Tasks;

namespace IBankRestWebService.Implementations
{
    public class BalanceEnquiryImp:IBalanceEnquiry
    {
        public List<Dictionary<string, string>> AcctValidation(BalanceEnquiryParam balanceParams, ApplicationsDbConnection conn)
        {
            Log.Warning("-------AcctValidation-----------");
            Log.Warning($"------ AcctValidation Request-------- {JsonConvert.SerializeObject(balanceParams)}");
            DataLayer dl = new DataLayer();

            List<AseParameter> sp = new List<AseParameter>()
            {
                new AseParameter() {ParameterName = "@psTransactionRef", AseDbType = AseDbType.VarChar, Value= balanceParams.TransactionRef},
                new AseParameter() {ParameterName = "@psAccountType", AseDbType = AseDbType.VarChar, Value= balanceParams.AcctType},
                new AseParameter() {ParameterName = "@psAccountNo", AseDbType = AseDbType.VarChar, Value= balanceParams.AccountNo},
                new AseParameter() {ParameterName = "@psCrncyCode", AseDbType = AseDbType.VarChar, Value= string.IsNullOrWhiteSpace(balanceParams.CrncyCode)?"0":balanceParams.CrncyCode},
                new AseParameter() {ParameterName = "@psUserName", AseDbType = AseDbType.VarChar, Value= string.IsNullOrWhiteSpace(balanceParams.UserName)?"system":balanceParams.UserName},
                new AseParameter() {ParameterName = "@pnChequeNo", AseDbType = AseDbType.Integer, Value= balanceParams.ChequeNo}
            };
            string val = string.Empty;
            foreach (var y in sp)
            {
                val = val + "Param=" + y.ParameterName + "- Value=" + y.Value;
            }
            string DB = AppSettingsConfig.DbToCall();

            string DBAndProcName = $"{DB}{"isp_tci_validate"}";

            var res = dl.SqlDs(DBAndProcName, "BalanceEnquiry", sp, conn.ConnectionString);

            Log.Warning($"------AcctValidation Response-------- {JsonConvert.SerializeObject(res)}");

            return res;
        }

        public List<Dictionary<string, string>> TCIPost4(BalanceEnquiryParam balanceParams, ApplicationsDbConnection conn)
        {
            throw new NotImplementedException();
        }
    }
}