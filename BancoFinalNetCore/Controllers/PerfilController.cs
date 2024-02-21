using BancoFinalNetCore.DTO;
using BancoFinalNetCore.Servicios;
using BancoFinalNetCore.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace BancoFinalNetCore.Controllers
{
    /// <summary>
    /// Controlador encargado de gestionar el perfil del usuario.
    /// </summary>
    public class PerfilController : Controller
    {
        private readonly IUsuarioServicio _usuarioServicio;
        private readonly ICuentaServicio _cuentaServicio;
        private readonly IConvertirAdao _convertirAdao;

        public PerfilController(IConvertirAdao convertirAdao, IUsuarioServicio usuarioServicio, ICuentaServicio cuentaServicio)
        {
            _usuarioServicio = usuarioServicio;
            _cuentaServicio = cuentaServicio;
            _convertirAdao = convertirAdao;
        }

        /// <summary>
        /// Método para mostrar el perfil del usuario.
        /// </summary>
        [Authorize]
        [HttpGet]
        [Route("/privada/miPerfil")]
        public IActionResult MiPerfil()
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método MiPerfil() de la clase PerfilController");
                UsuarioDTO u = _usuarioServicio.obtenerUsuarioPorEmail(User.Identity.Name);
                ViewBag.UsuarioDTO = u;

                return View("~/Views/Home/miPerfil.cshtml", u);
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al obtener el perfil del usuario. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se produjo una excepción en el método MiPerfil() de la clase PerfilController: " + e.Message);
                return View("~/Views/Home/miPerfil.cshtml");
            }
        }

        /// <summary>
        /// Método para procesar el formulario de edición del perfil del usuario.
        /// </summary>
        [HttpPost]
        [Route("/privada/procesar-editar")]
        public IActionResult ProcesarFormularioEdicion(long id, IFormFile foto)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método ProcesarFormularioEdicion() de la clase PerfilController");

                UsuarioDTO usuarioDTO = _usuarioServicio.buscarPorId(id);

                if (foto != null && foto.Length > 0)
                {
                    byte[] fotoBytes;
                    using (var memoryStream = new MemoryStream())
                    {
                        foto.CopyTo(memoryStream);
                        fotoBytes = memoryStream.ToArray();
                    }
                    usuarioDTO.FotoPerfil = fotoBytes;
                }
                else
                {
                    UsuarioDTO usuarioActualDTO = _usuarioServicio.buscarPorId(id);
                    byte[] fotoActual = usuarioActualDTO.FotoPerfil;
                    usuarioDTO.FotoPerfil = fotoActual;
                }

                _usuarioServicio.actualizarUsuario(usuarioDTO);

                ViewData["EdicionCorrecta"] = "El Usuario se ha editado correctamente";
                ViewBag.Usuarios = _usuarioServicio.obtenerTodosLosUsuarios();

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarFormularioEdicion() de la clase PerfilController. " + ViewData["EdicionCorrecta"]);
                UsuarioDTO u = _usuarioServicio.obtenerUsuarioPorEmail(User.Identity.Name);
                return View("~/Views/Home/miPerfil.cshtml", u);
            }
            catch (Exception e)
            {
                ViewData["Error"] = "Ocurrió un error al editar el usuario";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se produjo una excepción en el método ProcesarFormularioEdicion() de la clase PerfilController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/dashboard.cshtml");
            }
        }
    }
}