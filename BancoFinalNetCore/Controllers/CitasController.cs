using BancoFinalNetCore.DTO;
using BancoFinalNetCore.Servicios;
using BancoFinalNetCore.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace BancoFinalNetCore.Controllers
{
    public class CitasController : Controller
    {
        private readonly IUsuarioServicio _usuarioServicio;
        private readonly IOficinaServicio _oficinaServicio;
        private readonly ICitaServicio _citaServicio;

        public CitasController(ICitaServicio citaServicio, IOficinaServicio oficinaServicio, IUsuarioServicio usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
            _oficinaServicio = oficinaServicio;
            _citaServicio = citaServicio;
        }

        /// <summary>
        /// Método para mostrar las citas del usuario actual.
        /// </summary>
        [Authorize]
        [HttpGet]
        [Route("/privada/citas")]
        public IActionResult Citas()
        {
            try
            {
                // Escribir en el registro de log
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método Citas() de la clase CitasController");

                // Obtener el usuario actual
                UsuarioDTO u = _usuarioServicio.obtenerUsuarioPorEmail(User.Identity.Name);

                // Obtener las citas del usuario
                List<CitaDTO> citasDto = u.CitasUsuario;

                if (User.IsInRole("ROLE_ADMIN"))
                {
                    // Pasar los datos a la vista
                    ViewBag.Citas = _citaServicio.obtenerTodosLasCitas();
                }
                else
                {
                    // Pasar los datos a la vista
                    ViewBag.Citas = citasDto;
                }
                    ViewBag.UsuarioDTO = u;


                // Retornar la vista de citas
                return View("~/Views/Home/citas.cshtml");
            }
            catch (Exception e)
            {
                // Manejar la excepción aquí
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método Citas() de la clase CitasController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/error.cshtml");
            }
        }

        /// <summary>
        /// Método para mostrar la vista de programación de nueva cita.
        /// </summary>
        [Authorize]
        [HttpGet]
        [Route("/privada/nuevaCita")]
        public IActionResult NuevaCitaVista()
        {
            try
            {
                // Escribir en el registro de log
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método NuevaCitaVista() de la clase CitasController");

                // Obtener el usuario actual
                UsuarioDTO u = _usuarioServicio.obtenerUsuarioPorEmail(User.Identity.Name);

                // Obtener todas las oficinas
                List<OficinaDTO> oficinas = _oficinaServicio.obtenerTodasLasOficinas();

                // Crear un objeto CitaDTO vacío
                CitaDTO cita = new CitaDTO();

                // Pasar los datos a la vista
                ViewBag.Oficinas = oficinas;
                ViewBag.UsuarioDTO = u;

                // Retornar la vista de programación de nueva cita
                return View("~/Views/Home/nuevaCita.cshtml", cita);
            }
            catch (Exception e)
            {
                // Manejar la excepción aquí
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método NuevaCitaVista() de la clase CitasController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/error.cshtml");
            }
        }


        [Authorize]
        [HttpPost]
        [Route("/privada/crearNuevaCita")]
        /// <summary>
        /// Método para manejar la creación de una nueva cita.
        /// </summary>
        /// <param name="citaDto">DTO (Objeto de Transferencia de Datos) de la cita a crear.</param>
        /// <returns>Una acción de redirección según el resultado de la operación.</returns>
        public IActionResult CitaPost(CitaDTO citaDto)
        {
            try
            {
                // Escribir en el registro de log
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método CitaPost() de la clase MenuPrincipalController");

                // Obtener el usuario actual
                UsuarioDTO u = _usuarioServicio.obtenerUsuarioPorEmail(User.Identity.Name);

                // Asignar el ID del usuario a la cita
                citaDto.UsuarioCitaId = u.IdUsuario;

                // Registrar la nueva cita
                _citaServicio.registrarCita(citaDto);

                // Redireccionar dependiendo del motivo de la cita
                if (citaDto.MotivoCita == "FechaAnterior")
                {
                    // Mostrar mensaje de error si la fecha es anterior a la actual
                    TempData["FechaAnterior"] = "La fecha no puede ser anterior a la actual";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método CitaPost() de la clase MenuPrincipalController. " + ViewData["FechaAnterior"]);
                    return RedirectToAction("Citas");
                }
                else
                {
                    // Mostrar mensaje de éxito si la cita se registra correctamente
                    TempData["CitaRealizada"] = "Registro de la nueva Cita OK";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método CitaPost() de la clase MenuPrincipalController. " + ViewData["MensajeRegistroExitoso"]);
                    return RedirectToAction("Citas");
                }
            }
            catch (Exception e)
            {
                // Manejar la excepción aquí si es necesario
                // También puedes incluir mensajes de error en TempData si deseas mostrarlos en la vista
                TempData["errorCita"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método CitaPost() de la clase MenuPrincipalController: " + e.Message + e.StackTrace);
                return RedirectToAction("Citas");
            }
        }
    }
}