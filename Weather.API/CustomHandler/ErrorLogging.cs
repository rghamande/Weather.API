using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace Weather.API.CustomHandler
{
    public class ErrorLogging
    {
        private string LogPath = ConfigurationManager.AppSettings["ExceptionLogPath"].ToString();
        public void InsertErrorLog(ApiError apiError)
        {
            try
            {
                LogPath = HttpContext.Current.Server.MapPath($"{LogPath}\\{DateTime.Now.ToFileTime()}.log");
                File.AppendAllText(LogPath, JsonConvert.SerializeObject(apiError));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}