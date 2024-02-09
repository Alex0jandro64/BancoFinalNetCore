using BancoFinalNetCore.DTO;
using BancoFinalNetCore.Servicios;
using BancoFinalNetCore.Util;
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
        public IActionResult Home()
        {
            UsuarioDTO u = _usuarioServicio.obtenerUsuarioPorEmail(User.Identity.Name);
            List<CuentaBancariaDTO> cuentasBancariasDTO = _cuentaServicio.obtenerCuentasPorUsuarioId(u.IdUsuario);
            ViewBag.CuentasBancarias = cuentasBancariasDTO;
            ViewBag.UsuarioDTO = u;
            EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método Home() de la clase LoginController");
            return View("~/Views/Home/home.cshtml");
        }
    }
}
