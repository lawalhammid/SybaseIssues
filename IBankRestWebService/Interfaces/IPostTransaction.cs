using IBankRestWebService.Parameters;
using IBankRestWebService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IBankRestWebService.Interfaces
{
    public interface IPostTransaction
    {
        List<Dictionary<string, string>> TCIPost3(PostTransactionParam postTransactionParam, ApplicationsDbConnection conn);
        Task<int> TCIPost4(PostTransactionParam postTransactionParam);
    }
}
