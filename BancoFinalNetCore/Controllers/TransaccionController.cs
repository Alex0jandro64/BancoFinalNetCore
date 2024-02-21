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
    /// Controlador para manejar las transacciones bancarias.
    /// </summary>
    public class TransaccionController : Controller
    {
        private readonly IUsuarioServicio _usuarioServicio;
        private readonly ICuentaServicio _cuentaServicio;
        private readonly IConvertirAdao _convertirAdao;
        private readonly ITransaccionServicio _transaccionServicio;

        public TransaccionController(ITransaccionServicio transaccionServicio, IConvertirAdao convertirAdao, IUsuarioServicio usuarioServicio, ICuentaServicio cuentaServicio)
        {
            _usuarioServicio = usuarioServicio;
            _cuentaServicio = cuentaServicio;
            _convertirAdao = convertirAdao;
            _transaccionServicio = transaccionServicio;
        }

        /// <summary>
        /// Método para mostrar la vista de creación de nueva transacción.
        /// </summary>
        [Authorize]
        [HttpGet]
        [Route("/privada/nuevaTransaccion")]
        public IActionResult NuevaTransaccion()
        {
            EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método NuevaTransaccion() de la clase TransaccionController");

            try
            {
                UsuarioDTO u = _usuarioServicio.obtenerUsuarioPorEmail(User.Identity.Name);
                List<CuentaBancariaDTO> cuentasBancariasDTO = _cuentaServicio.obtenerCuentasPorUsuarioId(u.IdUsuario);

                ViewBag.UsuarioDTO = u;
                ViewBag.CuentasBancarias = cuentasBancariasDTO;

                return View("~/Views/Home/nuevaTransaccion.cshtml");
            }
            catch (Exception e)
            {
                ViewBag.Error = "Error al obtener datos para la transacción. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se produjo una excepción en el método NuevaTransaccion() de la clase TransaccionController: " + e.Message);
                return View("~/Views/Home/home.cshtml");
            }
        }

        /// <summary>
        /// Método para procesar la solicitud de creación de una nueva transacción.
        /// </summary>
        [Authorize]
        [HttpPost]
        [Route("/privada/crearNuevaTransaccion")]
        public IActionResult TransaccionPost(TransaccionDTO transaccionDTO)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método TransaccionPost() de la clase TransaccionController");
                UsuarioDTO usuarioDto = _usuarioServicio.obtenerUsuarioPorEmail(User.Identity.Name);

                _transaccionServicio.registrarTransaccion(transaccionDTO);

                if (transaccionDTO.IbanDestino == "CuentaNoEncontrada")
                {
                    TempData["CuentaNoEncontrada"] = "No existe esa cuenta";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método TransaccionPost() de la clase TransaccionController. " + TempData["CuentaNoEncontrada"]);
                    return RedirectToAction("NuevaTransaccion");
                }
                else if (transaccionDTO.IbanDestino == "MismaCuenta")
                {
                    TempData["MismaCuenta"] = "No puede hacer una transferencia a la misma cuenta";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método TransaccionPost() de la clase TransaccionController. " + TempData["MismaCuenta"]);
                    return RedirectToAction("NuevaTransaccion");
                }
                else if (transaccionDTO.IbanDestino == "NoSaldoSuficiente")
                {
                    TempData["NoSaldoSuficiente"] = "No hay saldo suficiente";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método TransaccionPost() de la clase TransaccionController. " + TempData["NoSaldoSuficiente"]);
                    return RedirectToAction("NuevaTransaccion");
                }
                else
                {
                    TempData["TransaccionRealizada"] = "Transacción realizada con éxito";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método TransaccionPost() de la clase TransaccionController. " + TempData["TransaccionRealizada"]);
                    return RedirectToAction("Home", "MenuPrincipal");
                }
            }
            catch (Exception e)
            {
                // Manejar la excepción aquí si es necesario
                // También puedes incluir mensajes de error en ViewBag si deseas mostrarlos en la vista
                ViewBag.Error = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se produjo una excepción en el método TransaccionPost() de la clase TransaccionController: " + e.Message + e.StackTrace);

                // Recuperar los datos necesarios para mantenerlos en ViewBag
                UsuarioDTO u = _usuarioServicio.obtenerUsuarioPorEmail(User.Identity.Name);
                List<CuentaBancariaDTO> cuentasBancariasDTO = _cuentaServicio.obtenerCuentasPorUsuarioId(u.IdUsuario);

                // Definir la cuenta seleccionada (puede ser la primera de la lista)
                ViewBag.CuentaSeleccionada = cuentasBancariasDTO.FirstOrDefault();

                // Asegurar que los datos necesarios estén en ViewBag antes de devolver la vista
                ViewBag.UsuarioDTO = u;
                ViewBag.CuentasBancarias = cuentasBancariasDTO;

                return View("~/Views/Home/home.cshtml");
            }
        }
    }
}