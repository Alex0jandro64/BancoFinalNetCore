namespace BancoFinalNetCore.DTO
{
    /// <summary>
    /// Clase que representa un objeto de transferencia de datos (DTO) para una cuenta bancaria.
    /// </summary>
    public class CuentaBancariaDTO
    {
        /// <summary>
        /// ID de la cuenta bancaria.
        /// </summary>
        public long IdCuenta { get; set; }

        /// <summary>
        /// ID del usuario propietario de la cuenta.
        /// </summary>
        public long IdUsuarioCuenta { get; set; }

        /// <summary>
        /// Saldo actual de la cuenta.
        /// </summary>
        public decimal SaldoCuenta { get; set; }

        /// <summary>
        /// Código IBAN de la cuenta.
        /// </summary>
        public string CodigoIban { get; set; }

        /// <summary>
        /// Lista de transacciones en las que la cuenta es el remitente.
        /// </summary>
        public List<TransaccionDTO> TransaccionesRemitentes { get; set; }

        /// <summary>
        /// Lista de transacciones en las que la cuenta es el destinatario.
        /// </summary>
        public List<TransaccionDTO> TransaccionesDestinatarios { get; set; }
    }
}