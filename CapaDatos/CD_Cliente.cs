using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Cliente
    {
        public List<Cliente> Listar()
        {
            List<Cliente> lista = new List<Cliente>();
            var conexion = new Conexion();
            try
            {
                using (SqlConnection oconexion = new SqlConnection(conexion.getConexion()))
                {
                    string query = "select IdCliente,Nombre,Apellidos,Correo,Clave,Reestablecer from CLIENTE";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    // El SqlDataReader lo que hara es poder leer los datos que se obtengan de la consulta
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            //Lo que hace es leer los datos de la consulta y los va guardando en la lista
                            lista.Add(
                             new Cliente()
                             {
                                 IdCliente = Convert.ToInt32(dr["IdCliente"]),
                                 Nombre = dr["Nombre"].ToString(),
                                 Apellidos = dr["Apellidos"].ToString(),
                                 Correo = dr["Correo"].ToString(),
                                 Clave = dr["Clave"].ToString(),
                                 Restablecer = Convert.ToBoolean(dr["Reestablecer"]),
                             }
                              );
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                string error = ex.Message;
                Console.WriteLine("Error al traer cliente de la base de datos");
                Console.WriteLine(error);
                lista = new List<Cliente>();
            }



            return lista;
        }
        public int Registrar(Cliente obj, out string Mensaje)
        {
            int idautogenerado = 0;
            var conexion = new Conexion();
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(conexion.getConexion()))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistarCliente", oconexion);
                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Clave", obj.Clave);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    cmd.ExecuteNonQuery();

                    idautogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idautogenerado = 0;
                Mensaje = ex.Message;
            }
            return idautogenerado;
        }


        public bool CambiarClave(int idcliente, string nuevaclave, out string Mensaje)
        {
            bool resultado = false;
            var conexion = new Conexion();
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(conexion.getConexion()))
                {
                    SqlCommand cmd = new SqlCommand("update  cliente set clave = @nuevaclave , restablecer = 0 where idcliente = @Id", oconexion);
                    cmd.Parameters.AddWithValue("@Id", idcliente);
                    cmd.Parameters.AddWithValue("@nuevaclave", nuevaclave);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }
        public bool ReestablecerClave(int idcliente, string clave, out string Mensaje)
        {
            bool resultado = false;
            var conexion = new Conexion();
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(conexion.getConexion()))
                {
                    SqlCommand cmd = new SqlCommand("update  cliente set clave = @clave , restablecer = 1 where idcliente = @Id", oconexion);
                    cmd.Parameters.AddWithValue("@Id", idcliente);
                    cmd.Parameters.AddWithValue("@clave", clave);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }
    }
}
