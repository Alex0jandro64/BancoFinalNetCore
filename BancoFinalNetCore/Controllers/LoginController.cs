using BancoFinalNetCore.Servicios;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BancoFinalNetCore.DTO;
using BancoFinalNetCore.Util;
using System;

namespace BancoFinalNetCore.Controllers
{
    /// <summary>
    /// Controlador encargado de gestionar el inicio de sesión de usuarios.
    /// </summary>
    public class LoginController : Controller
    {
        private readonly IUsuarioServicio _usuarioServicio;

        public LoginController(IUsuarioServicio usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }

        /// <summary>
        /// Método para mostrar la vista de inicio de sesión.
        /// </summary>
        [HttpGet]
        [Route("/auth/login")]
        public IActionResult Login()
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método Login() de la clase LoginController");

                UsuarioDTO usuarioDTO = new UsuarioDTO();
                return View("~/Views/Home/login.cshtml", usuarioDTO);
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método Login() de la clase LoginController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/login.cshtml");
            }
        }

        /// <summary>
        /// Método para procesar el inicio de sesión del usuario.
        /// </summary>
        [HttpPost]
        [Route("/auth/iniciar-sesion")]
        public IActionResult ProcesarInicioSesion(UsuarioDTO usuarioDTO)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método ProcesarInicioSesion() de la clase LoginController");

                bool credencialesValidas = _usuarioServicio.verificarCredenciales(usuarioDTO.EmailUsuario, usuarioDTO.ClaveUsuario);

                if (credencialesValidas)
                {
                    UsuarioDTO u = _usuarioServicio.obtenerUsuarioPorEmail(usuarioDTO.EmailUsuario);

                    // Al hacer login correctamente se crea una identidad de reclamaciones (claims identity) con información del usuario 
                    // y su rol, de esta manera se controla que solo los admin puedan acceder a la administracion de usuarios
                    // y se mantiene esa info del usuario autenticado durante toda la sesión en una cookie.
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuarioDTO.EmailUsuario),
                    };
                    if (!string.IsNullOrEmpty(u.Rol))
                    {
                        claims.Add(new Claim(ClaimTypes.Role, u.Rol));
                    }

                    var identidadDeReclamaciones = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Establece una cookie en el navegador con los datos del usuario antes mencionados y se mantiene en el contexto.
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identidadDeReclamaciones));

                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarInicioSesion() de la clase LoginController");
                    return RedirectToAction("Home", "MenuPrincipal");
                }
                else
                {
                    ViewData["MensajeErrorInicioSesion"] = "Credenciales inválidas o cuenta no confirmada. Inténtelo de nuevo.";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarInicioSesion() de la clase LoginController. " + ViewData["MensajeErrorInicioSesion"]);
                    return View("~/Views/Home/login.cshtml");
                }
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método ProcesarInicioSesion() de la clase LoginController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/login.cshtml");
            }
        }



        /// <summary>
        /// Método para confirmar la cuenta de usuario.
        /// </summary>
        [HttpGet]
        [Route("/auth/confirmar-cuenta")]
        public IActionResult ConfirmarCuenta([FromQuery] string token)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método ConfirmarCuenta() de la clase LoginController");

                bool confirmacionExitosa = _usuarioServicio.confirmarCuenta(token);

                if (confirmacionExitosa)
                {
                    ViewData["CuentaVerificada"] = "La dirección de correo ha sido confirmada correctamente";
                }
                else
                {
                    ViewData["yaEstabaVerificada"] = "El usuario ya estaba registrado y verificado";
                }

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ConfirmarCuenta() de la clase LoginController" +
                    (ViewData["CuentaVerificada"] != null ? ". " + ViewData["CuentaVerificada"] :
                    (ViewData["yaEstabaVerificada"] != null ? ". " + ViewData["yaEstabaVerificada"] : "")));

                return View("~/Views/Home/login.cshtml");
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método ConfirmarCuenta() de la clase LoginController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/login.cshtml");
            }
        }

        /// <summary>
        /// Método para cerrar sesión de usuario.
        /// </summary>
        [HttpPost]
        public IActionResult CerrarSesion()
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método CerrarSesion() de la clase LoginController");
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método CerrarSesion() de la clase LoginController: " + e.Message + e.StackTrace);
                return RedirectToAction("Index", "Home");
            }
        }
    }
}