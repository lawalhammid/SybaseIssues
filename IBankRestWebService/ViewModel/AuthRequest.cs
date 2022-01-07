using IBankRestWebService.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IBankRestWebService.ViewModel
{
    public class AuthRequest
    {
        public AuthParam AuthParam { get; set; }
        public ApplicationsDbConnection Connection { get; set; }
    }
}
