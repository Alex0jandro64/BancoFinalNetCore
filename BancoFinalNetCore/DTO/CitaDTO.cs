namespace BancoFinalNetCore.DTO
{
    /// <summary>
    /// Clase que representa un objeto de transferencia de datos (DTO) para una cita.
    /// </summary>
    public class CitaDTO
    {
        /// <summary>
        /// ID de la cita.
        /// </summary>
        public long IdCita { get; set; }

        /// <summary>
        /// ID del usuario asociado a la cita.
        /// </summary>
        public long UsuarioCitaId { get; set; }

        /// <summary>
        /// ID de la oficina asociada a la cita.
        /// </summary>
        public long OficinaCitaId { get; set; }

        /// <summary>
        /// Fecha de la cita.
        /// </summary>
        public DateTime FechaCita { get; set; }

        /// <summary>
        /// Motivo de la cita.
        /// </summary>
        public string? MotivoCita { get; set; }

        /// <summary>
        /// Dirección de la oficina asociada a la cita.
        /// </summary>
        public string? OficinaDireccion { get; set; }
    }
}
