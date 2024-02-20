using BancoFinalNetCore.DTO;
using BancoFinalNetCore.Servicios;
using BancoFinalNetCore.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BancoFinalNetCore.Controllers
{
    public class TransaccionController : Controller
    {

        private readonly IUsuarioServicio _usuarioServicio;
        private readonly ICuentaServicio _cuentaServicio;
        private readonly IConvertirAdao _convertirAdao;
        private readonly ITransaccionServicio _transaccionServicio;

        public TransaccionController(ITransaccionServicio transaccionServicio ,IConvertirAdao convertirAdao, IUsuarioServicio usuarioServicio, ICuentaServicio cuentaServicio)
        {
            _usuarioServicio = usuarioServicio;
            _cuentaServicio = cuentaServicio;
            _convertirAdao = convertirAdao;
            _transaccionServicio = transaccionServicio;
        }

        [Authorize]
        [HttpGet]
        [Route("/privada/nuevaTransaccion")]
        public IActionResult NuevaTransaccion()
        { 
            EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método Home() de la clase LoginController");
            UsuarioDTO u = _usuarioServicio.obtenerUsuarioPorEmail(User.Identity.Name);
            List<CuentaBancariaDTO> cuentasBancariasDTO = _cuentaServicio.obtenerCuentasPorUsuarioId(u.IdUsuario);

            ViewBag.UsuarioDTO = u;
            ViewBag.CuentasBancarias = cuentasBancariasDTO;

            return View("~/Views/Home/nuevaTransaccion.cshtml");

        }

        [Authorize]
        [HttpPost]
        [Route("/privada/crearNuevaTransaccion")]
        public IActionResult TransaccionPost(TransaccionDTO transaccionDTO)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método CuentaPost() de la clase MenuPrincipalController");
                UsuarioDTO usuarioDto = _usuarioServicio.obtenerUsuarioPorEmail(User.Identity.Name);

                

                _transaccionServicio.registrarTransaccion(transaccionDTO);

                

                if (transaccionDTO.IbanDestino == "CuentaNoEncontrada")
                {
                    ViewData["CuentaNoEncontrada"] = "No existe esa cuenta";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método TransaccionPost() de la clase TransaccionController. " + ViewData["CuentaNoEncontrada"]);
                    return View("~/Views/Home/nuevaTransaccion.cshtml");

                }
                else if (transaccionDTO.IbanDestino == "MismaCuenta")
                {
                    ViewData["MismaCuenta"] = "No puede hacer una transferencia a la misma cuenta";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método TransaccionPost() de la clase TransaccionController. " + ViewData["MismaCuenta"]);
                    return View("~/Views/Home/nuevaTransaccion.cshtml");
                }
                else if (transaccionDTO.IbanDestino == "NoSaldoSuficiente")
                {
                    ViewData["NoSaldoSuficiente"] = "No hay saldo Suficiente";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método TransaccionPost() de la clase TransaccionController. " + ViewData["NoSaldoSuficiente"]);
                    return View("~/Views/Home/nuevaTransaccion.cshtml");
                }
                else
                {
                    ViewData["TransaccionRealizada"] = "Registro del nuevo usuario OK";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método TransaccionPost() de la clase TransaccionController. " + ViewData["MensajeRegistroExitoso"]);
                    List<CuentaBancariaDTO> cuentasBancariasDTO = _cuentaServicio.obtenerCuentasPorUsuarioId(usuarioDto.IdUsuario);
                    ViewBag.CuentaSeleccionada = cuentasBancariasDTO[0];
                    ViewBag.UsuarioDTO = usuarioDto;
                    ViewBag.CuentasBancarias = cuentasBancariasDTO;
                    return View("~/Views/Home/home.cshtml");

                }
            }
            catch (Exception e)
            {
                // Manejar la excepción aquí si es necesario
                // También puedes incluir mensajes de error en ViewBag si deseas mostrarlos en la vista
                ViewBag.Error = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método CuentaPost() de la clase MenuPrincipalController: " + e.Message + e.StackTrace);

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
