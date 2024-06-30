using CapaEntidad;
using CapaNegocio;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;

namespace TheProjectPOO.Controllers
{
    public class MantenedorController : Controller
    {
        private readonly IConfiguration _configuration;

        public MantenedorController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

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

        //METODO CRUD CATEGORIA
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


        //METODO CRUD MARCA
        #region MARCA

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
        #endregion

        //METODOS CRUD PARA PRODUCTOS

        #region PRODUCTO
        [HttpGet]
        public JsonResult ListarProducto()
        {
            List<Producto> oLista = new List<Producto>();
            oLista = new CN_Producto().Listar();
            return Json(new { data = oLista });

        }
        [HttpPost]
        public async Task<JsonResult> GuardarProducto([FromBody] string objeto, IFormFile archivoImagen)
        {
            object resultado;
            string mensaje = string.Empty;
            bool operacion_exitosa = true;
            bool guardar_imagen_exitoso = true;

            Producto oProducto = JsonConvert.DeserializeObject<Producto>(objeto);
            decimal precio;

            if (decimal.TryParse(oProducto.PrecioTexto, NumberStyles.AllowDecimalPoint, new CultureInfo("es-PE"), out precio))
            {
                oProducto.Precio = precio;
            }
            else
            {
                return Json(new { operacionExitosa = false, mensaje = "El formato del precio debe ser ##.##" });
            }

            if (oProducto.IdProducto == 0)
            {
                int idProductoGenerado = new CN_Producto().Registrar(oProducto, out mensaje);
                if (idProductoGenerado != 0)
                {
                    oProducto.IdProducto = idProductoGenerado;
                }
                else
                {
                    operacion_exitosa = false;
                }
            }
            else
            {
                operacion_exitosa = new CN_Producto().Editar(oProducto, out mensaje);
            }

            if (operacion_exitosa && archivoImagen != null)
            {
                string nombreCarpetaFotos = _configuration["Rutas:CarpetaFotos"];
                string rutaGuardar = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", nombreCarpetaFotos);
                string extension = Path.GetExtension(archivoImagen.FileName);
                string nombre_imagen = string.Concat(oProducto.IdProducto.ToString(), extension);

                // Verifica si la carpeta existe, si no, la crea
                if (!Directory.Exists(rutaGuardar))
                {
                    Directory.CreateDirectory(rutaGuardar);
                }

                var filePath = Path.Combine(rutaGuardar, nombre_imagen);
                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await archivoImagen.CopyToAsync(stream);
                    }
                }
                catch (Exception ex)
                {
                    // Considera manejar la excepción de manera adecuada
                    operacion_exitosa = false;
                    mensaje = "Error al guardar la imagen: " + ex.Message;
                }
                if (guardar_imagen_exitoso)
                {
                    oProducto.RutaImagen = rutaGuardar;
                    oProducto.NombreImagen = nombre_imagen;
                    bool rspta = new CN_Producto().GuardarDatosImagen(oProducto, out mensaje);

                }
                else
                {
                    mensaje = "Se guardo la imagen pero hubo error con la imagen";
                }
            }

            return Json(new { operacionExitosa = operacion_exitosa, idGenerado = oProducto.IdProducto, mensaje = mensaje });
        }


        #endregion


    }
}

