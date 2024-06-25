using CapaEntidad;
using System.Data.SqlClient;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Usuarios
    {
        public List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Cn))
                {
                    string query = "select IdUsuario,Nombres,Apellidos,Correo,Clave,Reestablecer,Activo from USUARIO";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    // El SqlDataReader lo que hara es poder leer los datos que se obtengan de la consulta
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while(dr.Read()) {
                            //Lo que hace es leer los datos de la consulta y los va guardando en la lista
                           lista.Add(
                            new Usuario()
                            {
                                IdUsuario=Convert.ToInt32(dr["IdUsuario"]),
                                Nombres=dr["Nombres"].ToString(),
                                Apellidos=dr["Apellidos"].ToString(),
                                Correo=dr["Correo"].ToString(),
                                Clave=dr["Clave"].ToString(),
                                Reestablecer=Convert.ToBoolean(dr["Reestablecer"]),
                                Activo=Convert.ToBoolean(dr["Activo"])
                            }
                             );
                        }
                    }
                }

            }
            catch
            {
                lista = new List<Usuario>();
            }



            return lista;
        }
    }
}
