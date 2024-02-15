using BancoFinalNetCore.DTO;
using DAL.Entidades;

namespace BancoFinalNetCore.Servicios
{
    public interface ICuentaServicio
    {
        public CuentaBancaria GenerarCuentaBancaria(UsuarioDTO usuarioDto);
        public List<CuentaBancariaDTO> obtenerCuentasPorUsuarioId(long id);
    }
}
