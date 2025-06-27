using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Steady_Management_App
{
    public class AppConfig
    {
        public static IConfiguration Configuration { get; }
        static AppConfig()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }
        public static string GetApiBaseUrl()
        {
            // Lee el valor desde la sección ApiSettings del JSON
            return Configuration["ApiSettings:BaseUrl"];
        }
    }
}
