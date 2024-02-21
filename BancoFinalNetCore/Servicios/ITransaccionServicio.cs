using BancoFinalNetCore.DTO;

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
    }
}