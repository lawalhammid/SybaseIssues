using AdoNetCore.AseClient;
using IBankRestWebService.DataAccess;
using IBankRestWebService.Interfaces;
using IBankRestWebService.Parameters;
using IBankRestWebService.ViewModel;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IBankRestWebService.Implementations
{
    public class AuthImp : IAuth
    {
        public List<Dictionary<string, string>> AccountAuth(AuthParam authParams, ApplicationsDbConnection conn)
        {
            if (authParams.loginType == "CBS")
            {
                return ValidateViaCoreBankingSystem(authParams, conn);
            }

            if (authParams.loginType == "2Fpin")
            {
                return ValidateViaTwoFactor(authParams, conn);
            }

            return null;

        }

        public List<Dictionary<string, string>> TCIPost4(AuthParam authParams, ApplicationsDbConnection conn)
        {
            throw new NotImplementedException();
        }

        private List<Dictionary<string, string>> ValidateViaTwoFactor(AuthParam authParams, ApplicationsDbConnection conn)
        {
            Log.Warning("---------------------- ValidateViaTwoFactor starts---");
            Log.Warning($"------ ValidateViaTwoFactor Request-------- {JsonConvert.SerializeObject(authParams)}");

            var res = new List<Dictionary<string, string>>();

            if (!string.IsNullOrWhiteSpace(authParams.StaffId) && authParams.Password == "1234567890")
            {
                var json = JsonConvert.SerializeObject(new { Status = true, Message = "Success"});
                var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                
                res.Add(dictionary);
            }
            else
            {
                var json = JsonConvert.SerializeObject(new { Status = false, Message = "Failed" });
                var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                res.Add(dictionary);
            }

            Log.Warning($"------ValidateViaTwoFactor Response-------- {JsonConvert.SerializeObject(res)}");
            
            return res;
        }

        private List<Dictionary<string, string>> ValidateViaCoreBankingSystem(AuthParam authParams, ApplicationsDbConnection conn)
        {
            Log.Warning("---------------------- ValidateViaCoreBankingSystem starts");
            DataLayer dl = new DataLayer();

            List<AseParameter> sp = new List<AseParameter>()
            {
                new AseParameter() {ParameterName = "@psUserName", AseDbType = AseDbType.VarChar, Value= authParams.LoginId},
                new AseParameter() {ParameterName = "@psStaffId", AseDbType = AseDbType.VarChar, Value= authParams.StaffId}
            };

             var res = dl.SqlDs("Isp_GetUserInfo", "Autheticate", sp, conn.ConnectionString);

            Log.Warning($"------ValidateViaCoreBankingSystem Response-------- {JsonConvert.SerializeObject(res)}");

            return res;
        }
    }

    public class UserCoreBannkingDetailsDTO
    {
        public int? nErrorCode { get; set; } = -1;
        public string sErrorText { get; set; }
        public long? EmployeeId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string UserStatus { get; set; }
        public string BranchNo { get; set; }
        public string Email { get; set; }
        public string LoginId { get; set; }
        public string StaffId { get; set; }

    }
}
