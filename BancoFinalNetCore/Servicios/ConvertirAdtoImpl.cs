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
    }
}