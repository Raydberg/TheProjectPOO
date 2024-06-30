using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
namespace CapaDatos
{
    public class CD_Reporte
    {
        public List<Reporte> Ventas(string fechainicio,string fechafin,string idtransaccion )
        {
            List<Reporte> lista = new List<Reporte>();
            var conexion = new Conexion();
            try
            {
                using (SqlConnection oconexion = new SqlConnection(conexion.getConexion()))
                {
                  

                    SqlCommand cmd = new SqlCommand("sp_ReporteVentas", oconexion);
                    cmd.Parameters.AddWithValue("fechainicio",fechainicio);
                    cmd.Parameters.AddWithValue("fechafin", fechafin);
                    cmd.Parameters.AddWithValue("idtransaccion",idtransaccion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    // El SqlDataReader lo que hara es poder leer los datos que se obtengan de la consulta
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            //Lo que hace es leer los datos de la consulta y los va guardando en la lista
                            lista.Add(
                             new Reporte()
                             {
                                 //DATOS QUE SE OBTIENEN DE LA CONSULTA DE LA BASE DE DATOS DE LOS PROCEDIMIENTOS ALMACENADOS
                                 FechaVenta = dr["FechaVenta"].ToString(),
                                 Cliente = dr["Cliente"].ToString(),
                                 Producto = dr["Producto"].ToString(),
                                 Precio = Convert.ToDecimal( dr["Precio"],new CultureInfo("es-PE")),
                                 Cantidad = Convert.ToInt32( dr["Cantidad"].ToString()),
                                 Total = Convert.ToDecimal(dr["Total"], new CultureInfo("es-PE")),
                                 IdTransaccion = dr["Idtransaccion"].ToString()
                             });
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                string error = ex.Message;
                Console.WriteLine("Error al traer reporte de la base de datos");
                Console.WriteLine(error);
                lista = new List<Reporte>();
            }



            return lista;
        }

        public DashBoard VerDashBoard()
        {
            DashBoard objeto = new DashBoard();
            var conexion = new Conexion();
            try
            {
                using (SqlConnection oconexion = new SqlConnection(conexion.getConexion()))
                {                  

                    SqlCommand cmd = new SqlCommand("sp_ReportesDashboard", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
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
                Console.WriteLine("Error al traer reporte DashBoard de la base de datos");
                Console.WriteLine(error);
                objeto = new DashBoard();
            }
            return objeto;

        }
    }
}
