﻿using Microsoft.AspNetCore.Mvc;
using CapaEntidad;
using CapaNegocio;
using System.IO;
using System.Text.Json;

namespace CapaPresentacionTienda.Controllers
{
    public class TiendaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DetalleProducto(int idproducto = 0)
        {

            Producto oProducto = new Producto();
            bool conversion;


            oProducto = new CN_Producto().Listar().Where(p => p.IdProducto == idproducto).FirstOrDefault();


            if (oProducto != null)
            {
                oProducto.Base64 = CN_Recursos.ConvertirBase64(Path.Combine(oProducto.RutaImagen, oProducto.NombreImagen), out conversion);
                oProducto.Extension = Path.GetExtension(oProducto.NombreImagen);
            }

            return View(oProducto);
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

        [HttpPost]
        public JsonResult ListarProducto(int idcategoria, int idmarca, int nroPagina)
        {
            List<Producto> lista = new List<Producto>();

            bool conversion;

            lista = new CN_Producto().Listar().Select(p => new Producto()
            {
                IdProducto = p.IdProducto,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                oMarca = p.oMarca,
                oCategoria = p.oCategoria,
                Precio = p.Precio,
                Stock = p.Stock,
                RutaImagen = p.RutaImagen,
                Base64 = CN_Recursos.ConvertirBase64(Path.Combine(p.RutaImagen, p.NombreImagen), out conversion),
                Extension = Path.GetExtension(p.NombreImagen),
                Activo = p.Activo
            }).Where(p =>
                p.oCategoria.IdCategoria == (idcategoria == 0 ? p.oCategoria.IdCategoria : idcategoria) &&
                p.oMarca.IdMarca == (idmarca == 0 ? p.oMarca.IdMarca : idmarca) &&
                p.Stock > 0 && p.Activo == true
            ).ToList();


            var jsonresult = Json(new { data = lista });
            jsonresult;

            return jsonresult;


        }



    }
}
