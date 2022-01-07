using IBankRestWebService.Parameters;
using IBankRestWebService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IBankRestWebService.Interfaces
{
    public interface IBalanceEnquiry
    {
        List<Dictionary<string, string>> AcctValidation(BalanceEnquiryParam balanceParams, ApplicationsDbConnection conn);       
    }
}
