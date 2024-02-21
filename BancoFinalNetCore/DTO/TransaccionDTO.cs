namespace BancoFinalNetCore.DTO
{
    /// <summary>
    /// Clase que representa un objeto de transferencia de datos (DTO) para una transacción bancaria.
    /// </summary>
    public class TransaccionDTO
    {
        /// <summary>
        /// ID de la transacción.
        /// </summary>
        public long IdTransaccion { get; set; }

        /// <summary>
        /// ID de la cuenta remitente.
        /// </summary>
        public long CuentaRemitenteId { get; set; }

        /// <summary>
        /// ID de la cuenta destino.
        /// </summary>
        public long CuentaDestinoId { get; set; }

        /// <summary>
        /// Cantidad de la transacción.
        /// </summary>
        public decimal Cantidad { get; set; }

        /// <summary>
        /// IBAN de la cuenta destino.
        /// </summary>
        public string? IbanDestino { get; set; }

        /// <summary>
        /// IBAN de la cuenta remitente.
        /// </summary>
        public string? IbanRemitente { get; set; }

        /// <summary>
        /// Fecha de la transacción.
        /// </summary>
        public DateTime fechaTransaccion { get; set; }
    }
}