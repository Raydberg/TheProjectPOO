using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using Microsoft.Extensions.Configuration;
using ConfigurationBuilder = Microsoft.Extensions.Configuration.ConfigurationBuilder;
namespace CapaDatos
{
    public class Conexion
    {
        private string sql = string.Empty;
        public Conexion()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
            sql = builder.GetSection("ConnectionStrings:sql").Value;
        }
        public string getConexion()
        {
            return sql;
        }
        //public static string Cn = ConfigurationManager.ConnectionStrings["sql"].ToString();
    }
}
