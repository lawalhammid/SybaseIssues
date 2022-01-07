using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IBankRestWebService.Parameters
{
    public class AuthParam
    {
        public string LoginId { get; set; }
        public string Password { get; set; }
        public string StaffId { get; set; }
        public string Code { get; set; }
        public string loginType { get; set; }
    }
}
