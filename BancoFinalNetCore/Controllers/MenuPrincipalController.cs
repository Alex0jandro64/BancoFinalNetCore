﻿    using BancoFinalNetCore.DTO;
    using BancoFinalNetCore.Servicios;
    using BancoFinalNetCore.Util;
using DAL.Entidades;
using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

namespace BancoFinalNetCore.Controllers
{
    public class MenuPrincipalController : Controller
    {

        private readonly IUsuarioServicio _usuarioServicio;
        private readonly ICuentaServicio _cuentaServicio;
        private readonly IConvertirAdao _convertirAdao;

        public MenuPrincipalController(IConvertirAdao convertirAdao, IUsuarioServicio usuarioServicio, ICuentaServicio cuentaServicio)
        {
            _usuarioServicio = usuarioServicio;
            _cuentaServicio = cuentaServicio;
            _convertirAdao = convertirAdao;
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
                UsuarioDTO usuarioDto = _usuarioServicio.obtenerUsuarioPorEmail(User.Identity.Name);

                _cuentaServicio.GenerarCuentaBancaria(usuarioDto);

                // Recuperar los datos necesarios para mantenerlos en ViewBag
                List<CuentaBancariaDTO> cuentasBancariasDTO = _cuentaServicio.obtenerCuentasPorUsuarioId(usuarioDto.IdUsuario);

                // Definir la cuenta seleccionada (puede ser la primera de la lista)
                ViewBag.CuentaSeleccionada = cuentasBancariasDTO.FirstOrDefault();

                // Asegurar que los datos necesarios estén en ViewBag antes de devolver la vista
                ViewBag.UsuarioDTO = usuarioDto;
                ViewBag.CuentasBancarias = cuentasBancariasDTO;

                return View("~/Views/Home/home.cshtml");
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