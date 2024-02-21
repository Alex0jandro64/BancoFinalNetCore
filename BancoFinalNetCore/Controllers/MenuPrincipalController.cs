using BancoFinalNetCore.DTO;
using BancoFinalNetCore.Servicios;
using BancoFinalNetCore.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BancoFinalNetCore.Controllers
{
    /// <summary>
    /// Controlador encargado de gestionar el menú principal del usuario.
    /// </summary>
    public class MenuPrincipalController : Controller
    {
        private readonly IUsuarioServicio _usuarioServicio;
        private readonly ICuentaServicio _cuentaServicio;
        private readonly IConvertirAdao _convertirAdao;
        private readonly ITransaccionServicio _transaccionServicio;
        private readonly IConvertirAdto _convertirAdto;

        public MenuPrincipalController(IConvertirAdto convertirAdto, ITransaccionServicio transaccionServicio, IConvertirAdao convertirAdao, IUsuarioServicio usuarioServicio, ICuentaServicio cuentaServicio)
        {
            _usuarioServicio = usuarioServicio;
            _cuentaServicio = cuentaServicio;
            _convertirAdao = convertirAdao;
            _transaccionServicio = transaccionServicio;
            _convertirAdto = convertirAdto;
        }

        /// <summary>
        /// Método para mostrar la página de inicio del usuario.
        /// </summary>
        [Authorize]
        [HttpGet]
        [Route("/privada/home")]
        public IActionResult Home(string cuentaBancariaSelect)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método Home() de la clase MenuPrincipalController");

                UsuarioDTO u = _usuarioServicio.obtenerUsuarioPorEmail(User.Identity.Name);
                List<CuentaBancariaDTO> cuentasBancariasDTO = _cuentaServicio.obtenerCuentasPorUsuarioId(u.IdUsuario);

                if (cuentaBancariaSelect == null)
                {
                    ViewBag.CuentaSeleccionada = cuentasBancariasDTO[0];
                }
                else
                {
                    var cuentaSeleccionada = cuentasBancariasDTO.FirstOrDefault(c => c.CodigoIban == cuentaBancariaSelect);

                    if (cuentaSeleccionada != null)
                    {
                        ViewBag.CuentaSeleccionada = cuentaSeleccionada;
                    }
                    else
                    {
                        ViewBag.CuentaSeleccionada = cuentasBancariasDTO.FirstOrDefault();
                    }
                }

                List<TransaccionDTO> transacciones = _convertirAdto.listaTransaccionToDto(_transaccionServicio.ObtenerTransaccionesDeUsuario(u.IdUsuario));
                ViewBag.Transacciones = transacciones;
                ViewBag.UsuarioDTO = u;
                ViewBag.CuentasBancarias = cuentasBancariasDTO;

                return View("~/Views/Home/home.cshtml");
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al cargar la página de inicio. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se produjo una excepción en el método Home() de la clase MenuPrincipalController: " + e.Message);
                return View("~/Views/Home/home.cshtml");
            }
        }

        /// <summary>
        /// Método para procesar el formulario de creación de cuenta bancaria.
        /// </summary>
        [Authorize]
        [HttpPost]
        [Route("/auth/crear-cuenta-bancaria")]
        public IActionResult CuentaPost()
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método CuentaPost() de la clase MenuPrincipalController");

                UsuarioDTO usuarioDto = _usuarioServicio.obtenerUsuarioPorEmail(User.Identity.Name);
                _cuentaServicio.GenerarCuentaBancaria(usuarioDto);

                List<CuentaBancariaDTO> cuentasBancariasDTO = _cuentaServicio.obtenerCuentasPorUsuarioId(usuarioDto.IdUsuario);
                ViewBag.CuentaSeleccionada = cuentasBancariasDTO.FirstOrDefault();
                ViewBag.UsuarioDTO = usuarioDto;
                ViewBag.CuentasBancarias = cuentasBancariasDTO;

                return View("~/Views/Home/home.cshtml");
            }
            catch (Exception e)
            {
                ViewBag.Error = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se produjo una excepción en el método CuentaPost() de la clase MenuPrincipalController: " + e.Message + e.StackTrace);

                UsuarioDTO u = _usuarioServicio.obtenerUsuarioPorEmail(User.Identity.Name);
                List<CuentaBancariaDTO> cuentasBancariasDTO = _cuentaServicio.obtenerCuentasPorUsuarioId(u.IdUsuario);

                ViewBag.CuentaSeleccionada = cuentasBancariasDTO.FirstOrDefault();
                ViewBag.UsuarioDTO = u;
                ViewBag.CuentasBancarias = cuentasBancariasDTO;

                return View("~/Views/Home/home.cshtml");
            }
        }
    }
}