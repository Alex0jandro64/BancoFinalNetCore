using BancoFinalNetCore.DTO; // Importación del espacio de nombres para los DTOs.
using DAL.Entidades; // Importación del espacio de nombres para las entidades del modelo de datos.

namespace BancoFinalNetCore.Servicios
{
    /// <summary>
    /// Interfaz que define las operaciones del servicio de transacciones.
    /// </summary>
    public interface ITransaccionServicio
    {
        /// <summary>
        /// Registra una nueva transacción.
        /// </summary>
        /// <param name="transaccionDto">DTO de la transacción a registrar.</param>
        /// <returns>DTO de la transacción registrada.</returns>
        public TransaccionDTO registrarTransaccion(TransaccionDTO transaccionDto);

        /// <summary>
        /// Obtiene las transacciones asociadas a un usuario.
        /// </summary>
        /// <param name="userId">ID del usuario.</param>
        /// <returns>Lista de transacciones asociadas al usuario.</returns>
        public List<Transaccion> ObtenerTransaccionesDeUsuario(long userId);
    }
}