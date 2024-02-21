namespace BancoFinalNetCore.DTO
{
    /// <summary>
    /// Clase que representa un objeto de transferencia de datos (DTO) para una oficina bancaria.
    /// </summary>
    public class OficinaDTO
    {
        /// <summary>
        /// ID de la oficina.
        /// </summary>
        public long IdOficina { get; set; }

        /// <summary>
        /// Dirección de la oficina.
        /// </summary>
        public string? DireccionOficina { get; set; }
    }
}