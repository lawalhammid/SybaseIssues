using IBankRestWebService.Parameters;
using IBankRestWebService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IBankRestWebService.Interfaces
{
    public interface IAuth
    {
        List<Dictionary<string, string>> AccountAuth(AuthParam authParams, ApplicationsDbConnection conn);
        
    }
}
