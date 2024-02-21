using BancoFinalNetCore.DTO; // Importación del espacio de nombres para los DTOs.

namespace BancoFinalNetCore.Servicios
{
    /// <summary>
    /// Interfaz que define las operaciones del servicio de citas.
    /// </summary>
    public interface ICitaServicio
    {
        /// <summary>
        /// Registra una nueva cita.
        /// </summary>
        /// <param name="citaDTO">DTO de la cita a registrar.</param>
        /// <returns>DTO de la cita registrada.</returns>
        public CitaDTO registrarCita(CitaDTO citaDTO);
    }
}