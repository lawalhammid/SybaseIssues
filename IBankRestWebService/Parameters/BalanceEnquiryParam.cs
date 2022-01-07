
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IBankRestWebService.Parameters
{
    public class BalanceEnquiryParam
    {
        public string AcctType { get; set; }
        public string AccountNo { get; set; }
        public string CrncyCode { get; set; }
        public string UserName { get; set; }
        public int ChequeNo { get; set; }
        public string TransactionRef { get; set; }
    }

    public class RsmPhone
    {
        public string Space { get; set; }
    }

    public class RsmEmail
    {
        public string Space { get; set; }
    }

    public class ErrorText
    {
        public string Space { get; set; }
    }
}
