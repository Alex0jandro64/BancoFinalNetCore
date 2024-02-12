using BancoFinalNetCore.DTO;
using BancoFinalNetCore.Servicios;
using BancoFinalNetCore.Util;
using Microsoft.AspNetCore.Mvc;

namespace BancoFinalNetCore.Controllers
{
    /// <summary>
    /// Controlador para manejar las peticiones HTTP POST y GET relacionadas con la recuperación de contraseña.
    /// </summary>
    public class RecuperarPasswordController : Controller
    {

        private readonly IUsuarioServicio _usuarioServicio;

        public RecuperarPasswordController(IUsuarioServicio usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }

        /// <summary>
        /// Método para mostrar la vista de recuperación de contraseña.
        /// </summary>
        /// <param name="token">Token de recuperación de contraseña</param>
        /// <returns>La vista de recuperación de contraseña o la vista de solicitud de recuperación de contraseña en caso de error</returns>
        [HttpGet]
        [Route("/auth/recuperar")]
        public IActionResult MostrarVistaRecuperar([FromQuery(Name = "token")] string token)
        {

            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método MostrarVistaRecuperar() de la clase RecuperarPasswordController");

                UsuarioDTO usuario = _usuarioServicio.obtenerUsuarioPorToken(token);

                if (usuario != null)
                {
                    ViewData["UsuarioDTO"] = usuario;
                }
                else
                {
                    ViewData["MensajeErrorTokenValidez"] = "El enlace de recuperación no es válido o el usuario no se ha encontrado";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método MostrarVistaRecuperar() de la clase RecuperarPasswordController. " + ViewData["MensajeErrorTokenValidez"]);
                    return View("~/Views/Home/solicitarRecuperacionPassword.cshtml");
                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método MostrarVistaRecuperar() de la clase RecuperarPasswordController");
                return View("~/Views/Home/recuperar.cshtml");
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método MostrarVistaRecuperar() de la clase RecuperarPasswordController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/solicitarRecuperacionPassword.cshtml");
            }
        }

        /// <summary>
        /// Método para procesar la recuperación de contraseña.
        /// </summary>
        /// <param name="usuarioDTO">DTO del usuario con los datos de recuperación de contraseña</param>
        /// <returns>La vista correspondiente según el resultado de la operación</returns>
        [HttpPost]
        [Route("/auth/recuperar")]
        public IActionResult ProcesarRecuperacionContraseña(UsuarioDTO usuarioDTO)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método ProcesarRecuperacionContraseña() de la clase RecuperarPasswordController");

                UsuarioDTO usuarioExistente = _usuarioServicio.obtenerUsuarioPorToken(usuarioDTO.Token);

                if (usuarioExistente == null)
                {
                    ViewData["MensajeErrorTokenValidez"] = "El enlace de recuperación no es válido";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarRecuperacionContraseña() de la clase RecuperarPasswordController. " + ViewData["MensajeErrorTokenValidez"]);
                    return View("~/Views/Home/solicitarRecuperacionPassword.cshtml");
                }

                if (usuarioExistente.ExpiracionToken.HasValue && usuarioExistente.ExpiracionToken.Value < DateTime.Now)
                {
                    ViewData["MensajeErrorTokenExpirado"] = "El enlace de recuperación ha expirado";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarRecuperacionContraseña() de la clase RecuperarPasswordController. " + ViewData["MensajeErrorTokenExpirado"]);
                    return View("~/Views/Home/solicitarRecuperacionPassword.cshtml");
                }

                bool modificadaPassword = _usuarioServicio.modificarContraseñaConToken(usuarioDTO);

                if (modificadaPassword)
                {
                    ViewData["ContraseñaModificadaExito"] = "Contraseña modificada OK";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarRecuperacionContraseña() de la clase RecuperarPasswordController. " + ViewData["ContraseñaModificadaExito"]);
                    return View("~/Views/Home/login.cshtml");
                }
                else
                {
                    ViewData["ContraseñaModificadaError"] = "Error al cambiar de contraseña";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarRecuperacionContraseña() de la clase RecuperarPasswordController. " + ViewData["ContraseñaModificadaError"]);
                    return View("~/Views/Home/solicitarRecuperacionPassword.cshtml");
                }
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método ProcesarRecuperacionContraseña() de la clase RecuperarPasswordController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/solicitarRecuperacionPassword.cshtml");
            }
        }

        /// <summary>
        /// Método HTTP GET para mostrar la vista de inicio de solicitud de recuperación de contraseña.
        /// </summary>
        /// <returns>La vista de solicitud de recuperación de contraseña</returns>
        [HttpGet]
        [Route("/auth/solicitar-recuperacion")]
        public IActionResult MostrarVistaIniciarRecuperacion()
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método MostrarVistaIniciarRecuperacion() de la clase SolicitarRecuperacionController");

                UsuarioDTO usuarioDTO = new UsuarioDTO();
                return View("~/Views/Home/solicitarRecuperacionPassword.cshtml", usuarioDTO);
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método MostrarVistaIniciarRecuperacion() de la clase SolicitarRecuperacionController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/solicitarRecuperacionPassword.cshtml");
            }
        }

        /// <summary>
        /// Método HTTP POST para procesar el inicio del proceso de recuperación de contraseña.
        /// </summary>
        /// <param name="usuarioDTO">DTO del usuario con el email para iniciar la recuperación</param>
        /// <returns>La vista correspondiente según el resultado del inicio de recuperación</returns>
        [HttpPost]
        [Route("/auth/iniciar-recuperacion")]
        public IActionResult ProcesarInicioRecuperacion([Bind("EmailUsuario")] UsuarioDTO usuarioDTO)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método ProcesarInicioRecuperacion() de la clase SolicitarRecuperacionController");

                bool envioConExito = _usuarioServicio.iniciarProcesoRecuperacion(usuarioDTO.EmailUsuario);

                if (envioConExito)
                {
                    ViewData["MensajeExitoMail"] = "Proceso de recuperación OK";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarInicioRecuperacion() de la clase SolicitarRecuperacionController. " + ViewData["MensajeExitoMail"]);
                    return View("~/Views/Home/login.cshtml");
                }
                else
                {
                    ViewData["MensajeErrorMail"] = "No se inició el proceso de recuperación, cuenta de correo electrónico no encontrada.";
                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarInicioRecuperacion() de la clase SolicitarRecuperacionController. " + ViewData["MensajeErrorMail"]);
                return View("~/Views/Home/solicitarRecuperacionPassword.cshtml");
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método ProcesarInicioRecuperacion() de la clase SolicitarRecuperacionController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/solicitarRecuperacionPassword.cshtml");
            }
        }
    }
}