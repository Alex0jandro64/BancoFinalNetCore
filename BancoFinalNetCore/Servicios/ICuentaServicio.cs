using BancoFinalNetCore.DTO;
using DAL.Entidades;

namespace BancoFinalNetCore.Servicios
{
    public interface ICuentaServicio
    {
        public CuentaBancaria GenerarCuentaBancaria(Usuario usuario);
        public List<CuentaBancariaDTO> obtenerCuentasPorUsuarioId(long id);
    }
}
