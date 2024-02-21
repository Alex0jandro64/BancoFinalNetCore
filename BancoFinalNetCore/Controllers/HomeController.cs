using BancoFinalNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BancoFinalNetCore.Controllers
{
    /// <summary>
    /// Controlador principal que gestiona las vistas principales del sitio.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Acci�n para mostrar la p�gina de inicio.
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Acci�n para mostrar la pol�tica de privacidad.
        /// </summary>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Acci�n para manejar errores.
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}