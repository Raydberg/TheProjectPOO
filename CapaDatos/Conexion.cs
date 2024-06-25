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

        public static string ObtenerCadenaConexion()
        {
            // Construye la configuración sin intentar instanciar IConfiguration directamente.
            IConfiguration configuracion = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Obtiene la cadena de conexión usando la configuración construida.
            return configuracion.GetConnectionString("sql");
        }
        //public static string Cn = ConfigurationManager.ConnectionStrings["sql"].ToString();

    }
}
