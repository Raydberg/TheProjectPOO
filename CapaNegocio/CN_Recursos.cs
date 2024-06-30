using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Mail;
namespace CapaNegocio
{
    public class CN_Recursos
    {

        public static string GenerarClave()
        {
            string clave = Guid.NewGuid().ToString("N").Substring(0, 6);
            return clave;
        }


        //ENCRIPTADOR DE TEXTO A SHA256
        public static string CovertirSha256(string texto)
        {
            StringBuilder Sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)

                    Sb.Append(b.ToString("x2"));
            }
            return Sb.ToString();
        }
        public static bool EnviarCorreo(string correo, string asunto, string mensaje)
        {
            bool resultado = false;
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(correo);
                mail.From = new MailAddress("darthvaderbig6005@gmail.com");//Poner correo de prueba
                mail.Subject = asunto;
                mail.Body = mensaje;
                mail.IsBodyHtml = true;

                var smtp = new SmtpClient()
                {
                    //VIDEO 13
                    Credentials = new NetworkCredential("darthvaderbig6005@gmail.com", "rvgr asdg zagi xvhb "),//Poner correo de prueba y contraseña generada por contraseña de aplicaciones
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true
                };
                smtp.Send(mail);
                resultado = true;

            }
            catch (Exception ex)
            {
                resultado = false;
            }
            return resultado;
        }
        public static string ConvertirBase64(string ruta, out bool conversion)
        {
            string textoBase64 = string.Empty;
            conversion = true;

            try
            {
                byte[] bytes = File.ReadAllBytes(ruta);
                textoBase64 = Convert.ToBase64String(bytes);
            }
            catch 
            {
                conversion = false;
            }
            return textoBase64;

        }
    }
}

