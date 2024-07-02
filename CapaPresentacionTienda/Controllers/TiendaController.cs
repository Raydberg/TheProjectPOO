using Microsoft.AspNetCore.Mvc;
using CapaEntidad;
using CapaNegocio;
namespace CapaPresentacionTienda.Controllers
{
    public class TiendaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListaCategorias()
        {
            List<Categoria> lista = new List<Categoria>();

            lista = new CN_Categoria().Listar();
            return Json(new {data = lista});
        }
        [HttpPost]
        public JsonResult ListaMarcaporCategorias(int idcategoria)
        {
            List<Marca> lista = new List<Marca>();
            lista = new CN_Marca().ListarMarcaporCategoria(idcategoria);
            return Json(new { data = lista });
        }





    }
}
