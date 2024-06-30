using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;
namespace CapaDatos
{
    public class CD_Reporte
    {
        public DashBoard VerDashBoard()
        {
            DashBoard objeto = new DashBoard();
            var conexion = new Conexion();
            try
            {
                using (SqlConnection oconexion = new SqlConnection(conexion.getConexion()))
                {
                   

                    SqlCommand cmd = new SqlCommand("sp_ReportesDashboard", oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    // El SqlDataReader lo que hara es poder leer los datos que se obtengan de la consulta
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            objeto = new DashBoard()
                            {
                                TotalCliente = Convert.ToInt32(dr["TotalCliente"]),
                                TotalVenta = Convert.ToInt32(dr["TotalVenta"]),
                                TotalProducto = Convert.ToInt32(dr["TotalProducto"]),
                            };
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                string error = ex.Message;
                Console.WriteLine("Error al traer cliente de la base de datos");
                Console.WriteLine(error);
                objeto = new DashBoard();
            }
            return objeto;

        }
    }
}
