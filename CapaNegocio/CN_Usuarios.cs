using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;
namespace CapaNegocio
{
    public class CN_Usuarios
    {
        //Con este metodo lo que haremos es llamar al metodo Listar de la capa de datos y poder acceder a la lista de usuarios
        private CD_Usuarios objCapaDato = new CD_Usuarios();


        public List<Usuario> Listar()
        {
            return objCapaDato.Listar();
        }
    }
}
