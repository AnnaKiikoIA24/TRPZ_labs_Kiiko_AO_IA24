using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServApp.Models
{
    internal static class Configuration
    {
        public static int Port { get; set; } = Convert.ToInt32(ConfigurationManager.AppSettings["port"]);
        public static string? ResourcePath { get; set; } = Convert.ToString(ConfigurationManager.AppSettings["resource_path"]);
        public static string? DBConnStr {  get; set; } = Convert.ToString(ConfigurationManager.AppSettings["db_conn_str"]);


    }
}
