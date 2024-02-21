using BancoFinalNetCore.DTO; // Importación del espacio de nombres para los DTOs.

namespace BancoFinalNetCore.Servicios
{
    /// <summary>
    /// Interfaz que define las operaciones del servicio de oficinas.
    /// </summary>
    public interface IOficinaServicio
    {
        /// <summary>
        /// Obtiene todas las oficinas.
        /// </summary>
        /// <returns>Una lista de objetos OficinaDTO que representan todas las oficinas.</returns>
        public List<OficinaDTO> obtenerTodasLasOficinas();
    }
}