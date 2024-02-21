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
        /// Acción para mostrar la página de inicio.
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Acción para mostrar la política de privacidad.
        /// </summary>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Acción para manejar errores.
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}