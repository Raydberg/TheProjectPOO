using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TheProjectPOO.Models;
using CapaEntidad;
using CapaNegocio;
namespace TheProjectPOO.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Usuario()
        {
            return View();
        }
        [HttpGet]
        public JsonResult ListarUsuarios()
        {
            List<Usuario> oLista = new List<Usuario>();
            oLista = new CN_Usuarios().Listar();

            return Json(new { data = oLista });

        }
        [HttpPost]
        public JsonResult GuardarUsuario([FromBody] Usuario objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.IdUsuario == 0)
            {
                resultado = new CN_Usuarios().Registrar(objeto, out mensaje);
            }
            else
            {
                resultado = new CN_Usuarios().Editar(objeto, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje });
        }

        [HttpPost]
        public JsonResult EliminarUsuario([FromBody] int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;
            respuesta = new CN_Usuarios().Eliminar(id,out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje });
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
