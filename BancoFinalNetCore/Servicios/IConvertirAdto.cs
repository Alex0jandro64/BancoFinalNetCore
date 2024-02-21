using BancoFinalNetCore.DTO;
using DAL.Entidades;
using System.Collections.Generic;

namespace BancoFinalNetCore.Servicios
{
    /// <summary>
    /// Interfaz que define los métodos para convertir entidades a DTO.
    /// </summary>
    public interface IConvertirAdto
    {
        /// <summary>
        /// Convierte un objeto de tipo Usuario a un objeto UsuarioDTO.
        /// </summary>
        /// <param name="u">Objeto de tipo Usuario.</param>
        /// <returns>Objeto UsuarioDTO.</returns>
        public UsuarioDTO usuarioToDto(Usuario u);

        /// <summary>
        /// Convierte una lista de objetos Usuario a una lista de objetos UsuarioDTO.
        /// </summary>
        /// <param name="listaUsuario">Lista de objetos Usuario.</param>
        /// <returns>Lista de objetos UsuarioDTO.</returns>
        public List<UsuarioDTO> listaUsuarioToDto(List<Usuario> listaUsuario);

        /// <summary>
        /// Convierte una lista de objetos CuentaBancaria a una lista de objetos CuentaBancariaDTO.
        /// </summary>
        /// <param name="listaCuenta">Lista de objetos CuentaBancaria.</param>
        /// <returns>Lista de objetos CuentaBancariaDTO.</returns>
        public List<CuentaBancariaDTO> listaCuentaToDto(List<CuentaBancaria> listaCuenta);
    }
}