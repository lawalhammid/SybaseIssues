using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IBankRestWebService.ViewModel
{

    public class ConnectionDetail
    {
        public string DataBaseName { get; set; }
        public string Port { get; set; }
        public string IP { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
       
    }
}