using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TheProjectPOO.Models;
using CapaEntidad;
using CapaNegocio;
using System.Data;
using ClosedXML.Excel;
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
        #region USUARIO
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

        [HttpDelete()]
        //fromRoute: hace el parametro en la ruta para cuando mande paremetros como id
        public JsonResult EliminarUsuario([FromQuery] int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;
            respuesta = new CN_Usuarios().Eliminar(id,out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje });
        }
        #endregion


        [HttpGet]
        public JsonResult VistaDashBoard()
        {
            DashBoard objeto = new CN_Reporte().VerDashBoard();

            return Json(new { resultado = objeto });
        }


        [HttpGet]
        public JsonResult ListaReporte(string fechainicio,string fechafin , string idtransaccion)
        {
            List<Reporte> oLista = new List<Reporte>();
           oLista = new CN_Reporte().Ventas(fechainicio,fechafin,idtransaccion);
            return Json(new { data = oLista });
        }

        [HttpPost]

        public FileResult ExportarVenta(string fechainicio, string fechafin, string idtransaccion)
        {
            List<Reporte> oLista = new List<Reporte>();
            oLista = new CN_Reporte().Ventas(fechainicio, fechafin, idtransaccion);
            DataTable dt = new DataTable();

            dt.Locale= new System.Globalization.CultureInfo("en-PE");
            dt.Columns.Add("FechaVenta", typeof(string));
            dt.Columns.Add("Cliente", typeof(string));
            dt.Columns.Add("Producto", typeof(string));
            dt.Columns.Add("Precio", typeof(decimal));
            dt.Columns.Add("Cantidad", typeof(int));
            dt.Columns.Add("Total", typeof(decimal));
            dt.Columns.Add("IdTransaccion", typeof(string));

            foreach (Reporte rp in  oLista) {
                dt.Rows.Add( new object[]
                {
                    rp.FechaVenta,
                    rp.FechaVenta,
                    rp.Cliente,
                    rp.Producto,
                    rp.Precio,
                    rp.Cantidad,
                    rp.Total,
                    rp.IdTransaccion
                });
            }

            dt.TableName = "Datos";

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    //Especificando que el tipo de archivo es un excel y en el otro parametro el nombre del archivo
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteVentas"+ DateTime.Now.ToString()+ ".xlsx" );
                }
            }


        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

       
    }
}
