namespace BancoFinalNetCore.DTO
{
    public class TransaccionDTO
    {

        public long IdTransaccion { get; set; }

        public long CuentaRemitenteId { get; set; }
        public long CuentaDestinoId { get; set; }
        public decimal Cantidad { get; set; }
        public string? IbanDestino { get; set; }
        public DateTime fechaTransaccion { get; set; }

    }
}
