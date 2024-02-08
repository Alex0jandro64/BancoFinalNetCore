namespace BancoFinalNetCore.DTO
{
    public class CitaDTO
    {
        public long IdCita { get; set; }
        public long UsuarioCitaId { get; set; }
        public long OficinaCitaId { get; set; }
        public DateTime FechaCita { get; set; }
        public string? MotivoCita { get; set; }
    }
}
