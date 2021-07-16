using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditCrawler.Utility
{
    abstract class Constants
    {
        public static string datasource = string.Empty; //your server
        public static string database = string.Empty; //your database name
        public static string username = string.Empty; //username of server to connect
        public static string password = string.Empty; //password
        public static string apikey = string.Empty; //VirusTotal API key

        internal static void getConfigurations()
        {
            var resourcePaths = ConfigurationManager.GetSection("ResourcePaths") as NameValueCollection;
            datasource = resourcePaths["DATASOURCE"];
            database = resourcePaths["DATABASE"];
            username = resourcePaths["USERNAME"];
            password = resourcePaths["PASSWORD"];
            apikey = resourcePaths["API_KEY"];
        }
    }
}
