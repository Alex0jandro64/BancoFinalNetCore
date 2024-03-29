﻿using BancoFinalNetCore.DTO;
using BancoFinalNetCore.Util;
using DAL.Entidades;
using Microsoft.EntityFrameworkCore;

namespace BancoFinalNetCore.Servicios
{
    public class CitaServicioImpl : ICitaServicio
    {
        private readonly MyDbContext _contexto;
        private readonly IConvertirAdto _convertirADto;

        /// <summary>
        /// Constructor de la clase CitaServicioImpl.
        /// </summary>
        /// <param name="contexto">El contexto de la base de datos.</param>
        public CitaServicioImpl(MyDbContext contexto, IConvertirAdto convertirAdto)
        {
            _contexto = contexto;
            _convertirADto = convertirAdto;
        }

        /// <summary>
        /// Registra una cita en la base de datos.
        /// </summary>
        /// <param name="citaDTO">Objeto CitaDTO a registrar.</param>
        /// <returns>El objeto CitaDTO registrado.</returns>
        public CitaDTO registrarCita(CitaDTO citaDTO)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método registrarCita() de la clase CitaServicioImpl");

                DateTime dateTime = DateTime.Now;
                // Comprobar si la cita está para una fecha pasada
                if (citaDTO.FechaCita < dateTime)
                {
                    citaDTO.MotivoCita = "FechaAnterior";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método registrarCita() de la clase CitaServicioImpl");
                    return citaDTO;
                }

                var oficina = _contexto.Oficinas.FirstOrDefault(u => u.IdOficina == citaDTO.OficinaCitaId);
                var usuario = _contexto.Usuarios.FirstOrDefault(u => u.IdUsuario == citaDTO.UsuarioCitaId);
                Cita citaDao = new Cita();

                citaDao.OficinaCita = oficina;
                citaDao.MotivoCita = citaDTO.MotivoCita;
                citaDao.FechaCita = citaDTO.FechaCita;
                citaDao.UsuarioCita = usuario;

                _contexto.Add(citaDao);
                _contexto.SaveChanges();

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método registrarCita() de la clase CitaServicioImpl");

                return citaDTO;
            }
            catch (DbUpdateException dbe)
            {
                EscribirLog.escribirEnFicheroLog("[ERROR CitaServicioImpl - registrarCita()] Error de persistencia al actualizar la base de datos: " + dbe.Message);
                return null;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog("[ERROR CitaServicioImpl - registrarCita()] Error al registrar una cita: " + e.Message);
                return null;
            }
        }

        public List<CitaDTO> obtenerTodosLasCitas()
        {
            try
            {
                // Se escribe un mensaje de registro al entrar al método.
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método obtenerTodosLosUsuarios() de la clase UsuarioServicioImpl");

                // Se obtienen todos los usuarios de la base de datos y se convierten a objetos UsuarioDTO.
                return _convertirADto.listaCitasToDto(_contexto.Citas.Include(u => u.OficinaCita).ToList());
            }
            catch (Exception e)
            {
                // Se atrapa cualquier excepción que pueda ocurrir durante la ejecución del método.
                // Se escribe un mensaje de registro indicando el error.
                EscribirLog.escribirEnFicheroLog("[Error UsuarioServicioImpl - obtenerTodosLosUsuarios()] Error al obtener todos los usuarios: " + e.Message);

                // Se retorna una lista vacía en caso de error.
                return new List<CitaDTO>();
            }
        }

    }
}