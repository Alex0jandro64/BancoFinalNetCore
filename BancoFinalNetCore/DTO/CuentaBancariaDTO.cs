namespace BancoFinalNetCore.DTO
{
    public class CuentaBancariaDTO
    {
        public long IdCuenta { get; set; }

        public long IdUsuarioCuenta { get; set; }

        public decimal SaldoCuenta { get; set; }

        public string CodigoIban { get; set; }

        public List<TransaccionDTO> TransaccionesRemitentes { get; set; }

        public List<TransaccionDTO> TransaccionesDestinatarios { get; set; }
    }
}
