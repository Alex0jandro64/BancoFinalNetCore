using BancoFinalNetCore.DTO;
using BancoFinalNetCore.Util;
using DAL.Entidades;
using System;
using System.Collections.Generic;

namespace BancoFinalNetCore.Servicios
{
    /// <summary>
    /// Implementación de la interfaz IConvertirAdto para convertir entidades DAO a DTO.
    /// </summary>
    public class ConvertirAdtoImpl : IConvertirAdto
    {
        /// <summary>
        /// Convierte un objeto de tipo Usuario a un objeto UsuarioDTO.
        /// </summary>
        /// <param name="u">Objeto de tipo Usuario.</param>
        /// <returns>Objeto UsuarioDTO.</returns>
        public UsuarioDTO usuarioToDto(Usuario u)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método usuarioToDto() de la clase ConvertirAdtoImpl");

                // Crear un nuevo objeto UsuarioDTO y asignar los valores correspondientes
                UsuarioDTO dto = new UsuarioDTO();
                dto.TlfUsuario = u.TlfUsuario;
                dto.EmailUsuario = u.EmailUsuario;
                dto.ClaveUsuario = u.ClaveUsuario;
                dto.NombreUsuario = u.NombreUsuario;
                dto.ApellidosUsuario = u.ApellidosUsuario;
                dto.Token = u.Token;
                dto.ExpiracionToken = u.ExpiracionToken;
                dto.IdUsuario = u.IdUsuario;
                dto.FchAltaUsuario = u.FchAltaUsuario;
                dto.MailConfirmado = u.MailConfirmado;
                dto.DniUsuario = u.DniUsuario;
                dto.Rol = u.Rol;
                dto.FotoPerfil = u.FotoPerfil;
                dto.CitasUsuario = listaCitasToDto( u.CitasUsuario);

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método usuarioToDto() de la clase ConvertirAdtoImpl");
                return dto;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR ConvertirAdtoImpl - usuarioToDto()] - Error al convertir usuario DAO a usuarioDTO (return null): {e}");
                return null;
            }
        }

        /// <summary>
        /// Convierte una lista de objetos Usuario a una lista de objetos UsuarioDTO.
        /// </summary>
        /// <param name="listaUsuario">Lista de objetos Usuario.</param>
        /// <returns>Lista de objetos UsuarioDTO.</returns>
        public List<UsuarioDTO> listaUsuarioToDto(List<Usuario> listaUsuario)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método listaUsuarioToDto() de la clase ConvertirAdtoImpl");

                List<UsuarioDTO> listaDto = new List<UsuarioDTO>();
                foreach (Usuario u in listaUsuario)
                {
                    listaDto.Add(usuarioToDto(u));
                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método listaUsuarioToDto() de la clase ConvertirAdtoImpl");
                return listaDto;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR ConvertirAdtoImpl - listaUsuarioToDto()] - Error al convertir lista de usuario DAO a lista de usuarioDTO (return null): {e}");
                return null;
            }
        }

        /// <summary>
        /// Convierte un objeto de tipo CuentaBancaria a un objeto CuentaBancariaDTO.
        /// </summary>
        /// <param name="u">Objeto de tipo CuentaBancaria.</param>
        /// <returns>Objeto CuentaBancariaDTO.</returns>
        public CuentaBancariaDTO cuentaToDto(CuentaBancaria u)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método cuentaToDto() de la clase ConvertirAdtoImpl");

                // Crear un nuevo objeto CuentaBancariaDTO y asignar los valores correspondientes
                CuentaBancariaDTO dto = new CuentaBancariaDTO();
                dto.SaldoCuenta = u.SaldoCuenta;
                dto.CodigoIban = u.CodigoIban;
                dto.IdCuenta = u.IdCuenta;
                dto.IdUsuarioCuenta = u.UsuarioCuentaId;

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método cuentaToDto() de la clase ConvertirAdtoImpl");
                return dto;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR ConvertirAdtoImpl - cuentaToDto()] - Error al convertir cuenta bancaria DAO a cuenta bancaria DTO (return null): {e}");
                return null;
            }
        }

        /// <summary>
        /// Convierte una lista de objetos CuentaBancaria a una lista de objetos CuentaBancariaDTO.
        /// </summary>
        /// <param name="listaCuenta">Lista de objetos CuentaBancaria.</param>
        /// <returns>Lista de objetos CuentaBancariaDTO.</returns>
        public List<CuentaBancariaDTO> listaCuentaToDto(List<CuentaBancaria> listaCuenta)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método listaCuentaToDto() de la clase ConvertirAdtoImpl");

                List<CuentaBancariaDTO> listaDto = new List<CuentaBancariaDTO>();
                foreach (CuentaBancaria u in listaCuenta)
                {
                    listaDto.Add(cuentaToDto(u));
                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método listaCuentaToDto() de la clase ConvertirAdtoImpl");
                return listaDto;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR ConvertirAdtoImpl - listaCuentaToDto()] - Error al convertir lista de cuenta bancaria DAO a lista de cuenta bancaria DTO (return null): {e}");
                return null;
            }
        }
        /// <summary>
        /// Convierte un objeto de tipo Transaccion a un objeto de tipo TransaccionDTO.
        /// </summary>
        /// <param name="u">Objeto Transaccion a convertir.</param>
        /// <returns>Objeto TransaccionDTO convertido.</returns>
        public TransaccionDTO transaccionToDto(Transaccion u)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método transaccionToDto() de la clase ConvertirAdtoImpl");

                // Crear un nuevo objeto TransaccionDTO y asignar los valores correspondientes
                TransaccionDTO dto = new TransaccionDTO();
                dto.fechaTransaccion = u.FechaTransaccion;
                dto.Cantidad = u.CantidadTransaccion;
                dto.IdTransaccion = u.IdTransaccion;
                dto.CuentaDestinoId = u.UsuarioTransaccionDestinatarioId;
                dto.CuentaRemitenteId = u.UsuarioTransaccionRemitenteId;
                dto.IbanDestino = u.UsuarioTransaccionDestinatario.CodigoIban;
                dto.IbanRemitente = u.UsuarioTransaccionRemitente.CodigoIban;
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método transaccionToDto() de la clase ConvertirAdtoImpl");
                return dto;
            }
            catch (Exception e)
            {
                // Manejo de excepción: Escribir en el registro de log y devolver null
                EscribirLog.escribirEnFicheroLog($"[ERROR ConvertirAdtoImpl - transaccionToDto()] - Error al convertir Transaccion a TransaccionDTO (return null): {e}");
                return null;
            }
        }

        /// <summary>
        /// Convierte una lista de objetos de tipo Transaccion a una lista de objetos de tipo TransaccionDTO.
        /// </summary>
        /// <param name="listaTransaccion">Lista de Transacciones a convertir.</param>
        /// <returns>Lista de TransaccionesDTO convertidas.</returns>
        public List<TransaccionDTO> listaTransaccionToDto(List<Transaccion> listaTransaccion)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método listaTransaccionToDto() de la clase ConvertirAdtoImpl");

                List<TransaccionDTO> listaDto = new List<TransaccionDTO>();
                foreach (Transaccion u in listaTransaccion)
                {
                    listaDto.Add(transaccionToDto(u));
                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método listaTransaccionToDto() de la clase ConvertirAdtoImpl");
                return listaDto;
            }
            catch (Exception e)
            {
                // Manejo de excepción: Escribir en el registro de log y devolver null
                EscribirLog.escribirEnFicheroLog($"[ERROR ConvertirAdtoImpl - listaTransaccionToDto()] - Error al convertir lista de Transacciones a lista de TransaccionesDTO (return null): {e}");
                return null;
            }
        }

        /// <summary>
        /// Convierte un objeto de tipo Cita a un objeto de tipo CitaDTO.
        /// </summary>
        /// <param name="u">Objeto Cita a convertir.</param>
        /// <returns>Objeto CitaDTO convertido.</returns>
        public CitaDTO citaToDto(Cita u)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método citaToDto() de la clase ConvertirAdtoImpl");

                // Crear un nuevo objeto CitaDTO y asignar los valores correspondientes
                CitaDTO dto = new CitaDTO();
                dto.IdCita = u.IdCita;
                dto.FechaCita = u.FechaCita;
                dto.MotivoCita = u.MotivoCita;
                dto.OficinaCitaId = u.OficinaCitaId;
                dto.OficinaDireccion = u.OficinaCita.DireccionOficina;
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método citaToDto() de la clase ConvertirAdtoImpl");
                return dto;
            }
            catch (Exception e)
            {
                // Manejo de excepción: Escribir en el registro de log y devolver null
                EscribirLog.escribirEnFicheroLog($"[ERROR ConvertirAdtoImpl - citaToDto()] - Error al convertir Cita a CitaDTO (return null): {e}");
                return null;
            }
        }

        /// <summary>
        /// Convierte una lista de objetos de tipo Cita a una lista de objetos de tipo CitaDTO.
        /// </summary>
        /// <param name="listaCitas">Lista de Citas a convertir.</param>
        /// <returns>Lista de CitasDTO convertidas.</returns>
        public List<CitaDTO> listaCitasToDto(List<Cita> listaCitas)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método listaCitasToDto() de la clase ConvertirAdtoImpl");

                List<CitaDTO> listaDto = new List<CitaDTO>();
                foreach (Cita u in listaCitas)
                {
                    listaDto.Add(citaToDto(u));
                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método listaCitasToDto() de la clase ConvertirAdtoImpl");
                return listaDto;
            }
            catch (Exception e)
            {
                // Manejo de excepción: Escribir en el registro de log y devolver null
                EscribirLog.escribirEnFicheroLog($"[ERROR ConvertirAdtoImpl - listaCitasToDto()] - Error al convertir lista de Citas a lista de CitasDTO (return null): {e}");
                return null;
            }
        }

        /// <summary>
        /// Convierte un objeto de tipo Oficina a un objeto de tipo OficinaDTO.
        /// </summary>
        /// <param name="u">Objeto Oficina a convertir.</param>
        /// <returns>Objeto OficinaDTO convertido.</returns>
        public OficinaDTO oficinaToDto(Oficina u)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método oficinaToDto() de la clase ConvertirAdtoImpl");

                // Crear un nuevo objeto OficinaDTO y asignar los valores correspondientes
                OficinaDTO dto = new OficinaDTO();
                dto.DireccionOficina = u.DireccionOficina;
                dto.IdOficina = u.IdOficina;
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método oficinaToDto() de la clase ConvertirAdtoImpl");
                return dto;
            }
            catch (Exception e)
            {
                // Manejo de excepción: Escribir en el registro de log y devolver null
                EscribirLog.escribirEnFicheroLog($"[ERROR ConvertirAdtoImpl - oficinaToDto()] - Error al convertir Oficina a OficinaDTO (return null): {e}");
                return null;
            }
        }

        /// <summary>
        /// Convierte una lista de objetos de tipo Oficina a una lista de objetos de tipo OficinaDTO.
        /// </summary>
        /// <param name="listaOficinas">Lista de Oficinas a convertir.</param>
        /// <returns>Lista de OficinasDTO convertidas.</returns>
        public List<OficinaDTO> listaOficinaToDto(List<Oficina> listaOficinas)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método listaOficinaToDto() de la clase ConvertirAdtoImpl");

                List<OficinaDTO> listaDto = new List<OficinaDTO>();
                foreach (Oficina u in listaOficinas)
                {
                    listaDto.Add(oficinaToDto(u));
                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método listaOficinaToDto() de la clase ConvertirAdtoImpl");
                return listaDto;
            }
            catch (Exception e)
            {
                // Manejo de excepción: Escribir en el registro de log y devolver null
                EscribirLog.escribirEnFicheroLog($"[ERROR ConvertirAdtoImpl - listaOficinaToDto()] - Error al convertir lista de Oficinas a lista de OficinasDTO (return null): {e}");
                return null;
            }
        }
    }
}