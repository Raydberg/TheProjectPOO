using Microsoft.AspNetCore.Mvc;
using CapaEntidad;
using CapaNegocio;
using DocumentFormat.OpenXml.Wordprocessing;

using CapaEntidad;
using CapaNegocio;

namespace TheProjectPOO.Controllers
{
    public class AccesoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CambiarClave()
        {
            return View();
        }

        public IActionResult Reestablecer()
        {
            return View();
        }



        [HttpPost]
        public IActionResult Index(string correo, string clave)
        {
            Usuario oUsuario = new Usuario();

            oUsuario = new CN_Usuarios().Listar().Where(u => u.Correo == correo && u.Clave == CN_Recursos.CovertirSha256(clave)).FirstOrDefault();

            if (oUsuario == null)
            {
                ViewBag.Error = "Correo o Clave incorrecta";
                return View();
            }
            else
            {
                if (oUsuario.Reestablecer)
                {
                    TempData["IdUsuario"] = oUsuario.IdUsuario;
                    return RedirectToAction("CambiarClave");
                }

                ViewBag.Error = null;

                return RedirectToAction("Index", "Home");
            }
            
        }

        [HttpPost]
        public IActionResult CambiarClave(string idusuario, string claveactual, string nuevaclave, string confirmarclave)
        {
            Usuario oUsuario = new Usuario();

            oUsuario = new CN_Usuarios().Listar().Where(u => u.IdUsuario == int.Parse(idusuario)).FirstOrDefault();

            if(oUsuario.Clave != CN_Recursos.CovertirSha256(claveactual))
            {
                TempData["IdUsuario"] = idusuario;
                ViewData["vclave"] = "";
                ViewBag.Error = "Contraseña actual no es correcta";
                return View();
            }
            else if(nuevaclave != confirmarclave)
            {
                TempData["IdUsuario"] = idusuario;
                ViewData["vclave"] = "claveactual";
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }

            ViewData["vclave"] = "";
            nuevaclave = CN_Recursos.CovertirSha256(nuevaclave);
            string mensaje = string.Empty;
            bool respuesta = new CN_Usuarios().CambiarClave(int.Parse(idusuario),nuevaclave,out mensaje);
            if (respuesta)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["IdUsuario"] = idusuario;

                ViewBag.Error = mensaje;
                return View();
            }

        }

        [HttpPost]
        public IActionResult Reestablecer(string correo)
        {
            Usuario ousuario = new Usuario();
            ousuario = new CN_Usuarios().Listar().Where(item => item.Correo == correo).FirstOrDefault();

            if (ousuario == null)
            {
                ViewBag.Error = "No se encontro un usuario relacionado a ese correo";
                return View();
            }

            string mensaje = string.Empty;
            bool respuesta = new CN_Usuarios().ReestablecerClave(ousuario.IdUsuario, correo, out mensaje);

            if (respuesta) { 
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }


        }

        public IActionResult CerrarSesion()
        {
            return RedirectToAction("Index", "Acceso");
        }

    }
}
