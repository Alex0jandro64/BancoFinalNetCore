using BancoFinalNetCore.DTO;

namespace BancoFinalNetCore.Servicios
{
    public interface ITransaccionServicio
    {
        public TransaccionDTO registrarTransaccion(TransaccionDTO transaccionDto);
    }
}
