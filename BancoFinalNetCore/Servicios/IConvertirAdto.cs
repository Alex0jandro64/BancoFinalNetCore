using BancoFinalNetCore.DTO;
using DAL.Entidades;

namespace BancoFinalNetCore.Servicios
{
    public interface IConvertirAdto
    {
        public UsuarioDTO usuarioToDto(Usuario u);

        public List<UsuarioDTO> listaUsuarioToDto(List<Usuario> listaUsuario);
    }
}
