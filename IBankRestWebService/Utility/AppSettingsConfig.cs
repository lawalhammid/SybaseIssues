using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IBankRestWebService.Utility
{
 
        public static class AppSettingsConfig
        {
            public static string DbToCall()
            {
                IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
                configurationBuilder.AddJsonFile("AppSettings.json");
                IConfiguration configuration = configurationBuilder.Build();

                return configuration["DbToCall"];
            }
        }
}
