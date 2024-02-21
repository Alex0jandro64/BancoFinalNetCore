using BancoFinalNetCore.DTO; // Importación del espacio de nombres para los DTOs.
using DAL.Entidades; // Importación del espacio de nombres para las entidades del modelo de datos.
using System.Collections.Generic; // Importación del espacio de nombres para List<T>.

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


        /// <summary>
        /// Convierte una lista de objetos Transaccion a una lista de objetos TransaccionDTO.
        /// </summary>
        /// <param name="listaTransaccion">Lista de objetos Transaccion.</param>
        /// <returns>Lista de objetos TransaccionDTO.</returns>
        public List<TransaccionDTO> listaTransaccionToDto(List<Transaccion> listaTransaccion);

        /// <summary>
        /// Convierte una lista de objetos Cita a una lista de objetos CitaDTO.
        /// </summary>
        /// <param name="listaCitas">Lista de objetos Cita.</param>
        /// <returns>Lista de objetos CitaDTO.</returns>
        public List<CitaDTO> listaCitasToDto(List<Cita> listaCitas);

        /// <summary>
        /// Convierte una lista de objetos Oficina a una lista de objetos OficinaDTO.
        /// </summary>
        /// <param name="listaOficinas">Lista de objetos Oficina.</param>
        /// <returns>Lista de objetos OficinaDTO.</returns>
        public List<OficinaDTO> listaOficinaToDto(List<Oficina> listaOficinas);
    }
}