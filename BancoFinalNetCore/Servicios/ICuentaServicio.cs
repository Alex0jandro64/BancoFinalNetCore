using BancoFinalNetCore.DTO;
using DAL.Entidades;
using System.Collections.Generic;

namespace BancoFinalNetCore.Servicios
{
    /// <summary>
    /// Interfaz que define las operaciones del servicio de cuentas bancarias.
    /// </summary>
    public interface ICuentaServicio
    {
        /// <summary>
        /// Genera una nueva cuenta bancaria para un usuario dado.
        /// </summary>
        /// <param name="usuarioDto">DTO del usuario para el cual se generará la cuenta bancaria.</param>
        /// <returns>La cuenta bancaria generada.</returns>
        public CuentaBancaria GenerarCuentaBancaria(UsuarioDTO usuarioDto);

        /// <summary>
        /// Obtiene todas las cuentas bancarias asociadas a un usuario por su ID.
        /// </summary>
        /// <param name="id">ID del usuario.</param>
        /// <returns>Lista de cuentas bancarias en formato DTO.</returns>
        public List<CuentaBancariaDTO> obtenerCuentasPorUsuarioId(long id);
    }
}