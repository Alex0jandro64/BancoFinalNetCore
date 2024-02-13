    using BancoFinalNetCore.DTO;
    using BancoFinalNetCore.Servicios;
    using BancoFinalNetCore.Util;
using DAL.Entidades;
using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    namespace BancoFinalNetCore.Controllers
    {
        public class MenuPrincipalController: Controller
        {

            private readonly IUsuarioServicio _usuarioServicio;
            private readonly ICuentaServicio _cuentaServicio;

            public MenuPrincipalController(IUsuarioServicio usuarioServicio, ICuentaServicio cuentaServicio)
            {
                _usuarioServicio = usuarioServicio;
                _cuentaServicio = cuentaServicio;
            }

            [Authorize]
            [HttpGet]
            [Route("/privada/home")] 
            public IActionResult Home(string cuentaBancariaSelect)
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método Home() de la clase LoginController");
                UsuarioDTO u = _usuarioServicio.obtenerUsuarioPorEmail(User.Identity.Name);
                List<CuentaBancariaDTO> cuentasBancariasDTO = _cuentaServicio.obtenerCuentasPorUsuarioId(u.IdUsuario);
                if (cuentaBancariaSelect == null)
                {
                    ViewBag.CuentaSeleccionada = cuentasBancariasDTO[0];

                }
            else
            {
                // Obtener la cuenta bancaria que coincida con el código IBAN seleccionado
                var cuentaSeleccionada = cuentasBancariasDTO.FirstOrDefault(c => c.CodigoIban == cuentaBancariaSelect);

                if (cuentaSeleccionada != null)
                {
                    ViewBag.CuentaSeleccionada = cuentaSeleccionada;
                }
                else
                {
                    // Si no se encuentra una cuenta con el código IBAN seleccionado, puedes manejarlo de la manera que desees.
                    // Por ejemplo, asignando la primera cuenta bancaria de la lista.
                    ViewBag.CuentaSeleccionada = cuentasBancariasDTO.FirstOrDefault();
                }
            }

            ViewBag.UsuarioDTO = u;
                ViewBag.CuentasBancarias = cuentasBancariasDTO;
                return View("~/Views/Home/home.cshtml");

            }

        [Authorize]
        [HttpPost]
        [Route("/auth/crear-cuenta-bancaria")]
        public IActionResult CuentaPost()
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método CuentaPost() de la clase MenuPrincipalController");
                _cuentaServicio.GenerarCuentaBancaria();

                    return View("~/Views/Home/login.cshtml");

            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método RegistrarPost() de la clase  MenuPrincipalController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/registro.cshtml");
            }
        }
    }
    }
