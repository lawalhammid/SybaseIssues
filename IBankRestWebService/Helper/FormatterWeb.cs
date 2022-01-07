using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace IBankRestWebService.Helper
{
    public static class FormatterWeb
    {
        public static string ValidateDateReturnString(string Param)
        {
            DateTime decChck = DateTime.Now;
            try
            {

                DateTime val = Param == null ? decChck : DateTime.TryParse(Param, out decChck) ? decChck : decChck;
                return string.Format("{0:yyyyMMdd}", val);

            }
            catch (Exception ex)
            {
                var exM = ex == null ? ex.InnerException.Message : ex.Message;
                return null;
            }
        }
    }
}