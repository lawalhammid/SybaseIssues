using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace IBankRestWebService
{
    public class Program
    {
        public static void Main(string[] args)
        {

            //IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            //configurationBuilder.AddJsonFile("AppSettings.json");
            //IConfiguration configuration = configurationBuilder.Build();

            //string RequestsResponseLogPath = configuration["LogPath:RequestsResponseLogPath"];
            //string ErrorLogPath =  configuration["LogPath:ErrorLogPath"];

            //Log.Logger = new LoggerConfiguration()
            //    .WriteTo.Console()
            //    .WriteTo.File(RequestsResponseLogPath, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning, rollingInterval: RollingInterval.Day)
            //    .WriteTo.File(ErrorLogPath, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error, rollingInterval: RollingInterval.Day)
            //    .CreateLogger();

            try
            {
                CreateHostBuilder(args).Build().Run();
                //Log.Information($"----------------------------Application started...");
            }
            catch (Exception ex)
            {
               // Log.Fatal(ex, "----------------------------Exception in Application");
            }
            finally
            {
               // Log.Information("----------------------------Exiting Application");
               // Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
