using CapaEntidad;
using CapaNegocio;
using Microsoft.AspNetCore.Mvc;

namespace CapaPresentacionTienda.Controllers
{
    public class AccesoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Reestablecer()
        {
            return View();
        }
        public IActionResult Registrar()
        {
            return View();
        }
        public IActionResult CambiarClave()
        {
            return View();
        }



        [HttpPost]
        public IActionResult Registrar(Cliente objeto)
        {
            int resultado;
            string mensaje = string.Empty;

            ViewData["Nombre"] = string.IsNullOrEmpty(objeto.Nombre) ? "" : objeto.Nombre;
            ViewData["Apellidos"] = string.IsNullOrEmpty(objeto.Apellidos) ? "" : objeto.Apellidos;
            ViewData["Correo"] = string.IsNullOrEmpty(objeto.Correo) ? "" : objeto.Correo;

            if (objeto.Clave != objeto.ConfirmarClave)
            {
                ViewBag.Error = "las contraseñas no coiciden";
                return View();
            }

            resultado = new CN_Cliente().Registrar(objeto, out mensaje);

            if (resultado > 0)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else {
                ViewBag.Error = mensaje;
                return View();
            }
        }

        [HttpPost]
        public IActionResult Index(string correo, string clave)
        {
            Cliente oCliente = new Cliente();

            oCliente = new CN_Cliente().Listar().Where(c => c.Correo == correo && c.Clave == CN_Recursos.CovertirSha256(clave)).FirstOrDefault();

            if (oCliente == null)
            {
                ViewBag.Error = "Correo o Clave incorrecta";
                return View();
            }
            else
            {
                if (oCliente.Restablecer)
                {
                    TempData["IdUsuario"] = oCliente.IdCliente;
                    return RedirectToAction("CambiarClave","Acceso");
                }
                else {
                    
                    ViewBag.Error = null;

                    return RedirectToAction("Index", "Tienda");

                }                
            }
        }

        [HttpPost]
        public IActionResult Reestablecer(string correo)
        {
            Cliente cliente = new Cliente();
            cliente = new CN_Cliente().Listar().Where(item => item.Correo == correo).FirstOrDefault();

            if (cliente == null)
            {
                ViewBag.Error = "No se encontro un Cliente  relacionado a ese correo";
                return View();
            }

            string mensaje = string.Empty;
            bool respuesta = new CN_Cliente().ReestablecerClave(cliente.IdCliente, correo, out mensaje);

            if (respuesta)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }


        }

        [HttpPost]
        public IActionResult CambiarClave(string idcliente, string claveactual, string nuevaclave, string confirmarclave)
        {
            Cliente oCliente = new Cliente();

            oCliente = new CN_Cliente().Listar().Where(u => u.IdCliente == int.Parse(idcliente)).FirstOrDefault();

            if (oCliente.Clave != CN_Recursos.CovertirSha256(claveactual))
            {
                TempData["IdCliente"] = idcliente;
                ViewData["vclave"] = "";
                ViewBag.Error = "Contraseña actual no es correcta";
                return View();
            }
            else if (nuevaclave != confirmarclave)
            {
                TempData["IdCliente"] = idcliente;
                ViewData["vclave"] = claveactual;
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }

            ViewData["vclave"] = "";
            nuevaclave = CN_Recursos.CovertirSha256(nuevaclave);
            string mensaje = string.Empty;
            bool respuesta = new CN_Cliente().CambiarClave(int.Parse(idcliente), nuevaclave, out mensaje);
            if (respuesta)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["IdCliente"] = idcliente;

                ViewBag.Error = mensaje;
                return View();
            }

        }

        public IActionResult CerrarSesion()
            
        {
            Session["Cliente"] = null;
            return RedirectToAction("Index", "Acceso");
        }

    }
}
