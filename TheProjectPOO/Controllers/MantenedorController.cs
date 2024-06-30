using CapaEntidad;
using CapaNegocio;
using CapaNegocio;
using Microsoft.AspNetCore.Mvc;

namespace TheProjectPOO.Controllers
{
    public class MantenedorController : Controller
    {
        public IActionResult Categoria()
        {
            return View();
        }
        public IActionResult Marca()
        {
            return View();
        }
        public IActionResult Producto()
        {
            return View();
        }


        [HttpGet]
        public JsonResult ListarCategorias()
        {
            List<Categoria> oLista = new List<Categoria>();
            oLista = new CN_Categoria().Listar();
            return Json(new { data = oLista });

        }

        [HttpPost]
        public JsonResult GuardarCategoria([FromBody] Categoria objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.IdCategoria == 0)
            {
                resultado = new CN_Categoria().Registrar(objeto, out mensaje);
            }
            else
            {
                resultado = new CN_Categoria().Editar(objeto, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje });
        }

        [HttpDelete()]
        public JsonResult EliminarCategoria([FromQuery] int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;
            respuesta = new CN_Categoria().Eliminar(id, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje });
        }


        //para marcaaaa
        [HttpGet]
        public JsonResult ListarMarca()
        {
            List<Marca> oLista = new List<Marca>();
            oLista = new CN_Marca().Listar();
            return Json(new { data = oLista });

        }

        [HttpPost]
        public JsonResult GuardarMarca([FromBody] Marca objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.IdMarca == 0)
            {
                resultado = new CN_Marca().Registrar(objeto, out mensaje);
            }
            else
            {
                resultado = new CN_Marca().Editar(objeto, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje });
        }

        [HttpDelete()]
        public JsonResult EliminarMarca([FromQuery] int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;
            respuesta = new CN_Marca().Eliminar(id, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje });
        }



    }
}
