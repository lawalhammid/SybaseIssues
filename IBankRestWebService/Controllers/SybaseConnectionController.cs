using AdoNetCore.AseClient;
using IBankRestWebService.DataAccess;
using IBankRestWebService.Helper;
using IBankRestWebService.Interfaces;
using IBankRestWebService.Parameters;
using IBankRestWebService.Utility;
using IBankRestWebService.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IBankRestWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SybaseConnectionController : ControllerBase
    {
        public SybaseConnectionController( ) { }

        [HttpPost("TestConnection")]
        public async Task<IActionResult> Connection(ConnectionDetail param)
        {
            var rtv = new ConnectionResponse();
            try
            {
                //Below is the connectionString. You can replace the values accordingly to suit your database connection
                string connstring = $"Data Source={param.IP}; port={param.Port};database={param.DataBaseName};uid={param.UserName};pwd={param.Password};charset=iso_1;Min Pool Size=200;Max Pool Size=5000; ConnectionIdleTimeout=60;";

                //Try to add line 143 to your code before making calls to connect to your sybase. this would solve the issue

                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (AseConnection theCons = new AseConnection(connstring))
                {
                    await theCons.OpenAsync();

                    //fetch or add record to your sybase database

                    rtv.ErrorCode = 0;
                    rtv.sErrorText = "Connected Successfully!";

                    await theCons.CloseAsync();
                    await theCons.DisposeAsync();

                    return Ok(rtv);
                }
            }
            catch (Exception ex)
            {
                var exM = ex.Message == null ? ex.InnerException.Message : ex.Message;
                rtv.ErrorCode = -1;
                rtv.sErrorText = "Error while connecting to Sybase";

                rtv.sErrorText = exM;
                return BadRequest(rtv);
            }
        }



    }
}
