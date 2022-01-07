using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IBankRestWebService.ViewModel
{
    public class ConnectionResponse
    {
        public int? ErrorCode { get; set; } = -99;
        public string sErrorText { get; set; }
    }
}