using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index(string correo,string clave)
        {
            Usuario oUsuario = new Usuario();

            oUsuario = new CN_Usuarios().Listar().Where(u => u.Correo == correo && u.Clave == CN_Recursos.CovertirSha256(clave)).FirstOrDefault();

            if (oUsuario == null)
            {
                ViewBag.Error = "Correo o Clave incorrecta";
            }
            else
            {
                ViewBag.Error = null;

                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}
