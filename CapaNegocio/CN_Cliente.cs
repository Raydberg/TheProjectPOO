﻿using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Cliente
    {
        private CD_Cliente objCapaDato = new CD_Cliente();


        public int Registrar(Cliente obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El Nombre del cliente no puede ser vacio";

            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "El Apellido del cliente no puede ser vacio";

            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El Correo del cliente no puede ser vacio";

            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                obj.Clave = CN_Recursos.CovertirSha256(obj.Clave);

                return objCapaDato.Registrar(obj, out Mensaje);

  
            }
            else
            {
                return 0;
            }

        }
        public List<Cliente> Listar()
        {
            return objCapaDato.Listar();
        }

        public bool CambiarClave(int idcliente, string nuevaclave, out string Mensaje)
        {
            return objCapaDato.CambiarClave(idcliente, nuevaclave, out Mensaje);
        }


        public bool ReestablecerClave(int idcliente, string correo, out string Mensaje)
        {
            Mensaje = string.Empty;
            string nuevaclave = CN_Recursos.GenerarClave();

            bool resultado = objCapaDato.ReestablecerClave(idcliente, CN_Recursos.CovertirSha256(nuevaclave), out Mensaje);
            try
            {
                if (resultado)
                {
                    string asunto = "Contraseña Reestablecida";
                    string mensaje_correo = "<h3>Su Cuenta fue reestablecida correctamente</h3></br><p>Su contraseña para acceder  ahora  es : !clave! </p>";
                    mensaje_correo = mensaje_correo.Replace("!clave!", nuevaclave);
                    bool respuesta = CN_Recursos.EnviarCorreo(correo, asunto, mensaje_correo);
                    if (respuesta)
                    {


                        return true;
                    }
                    else
                    {
                        Mensaje = "No se pudo enviar el correo";
                        return false;
                    }
                }
                else
                {
                    Mensaje = "No se pudo reestablecer la contraseña";
                    return false;
                }
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
