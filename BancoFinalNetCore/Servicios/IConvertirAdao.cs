using BancoFinalNetCore.DTO;
using DAL.Entidades;

namespace BancoFinalNetCore.Servicios
{
    public interface IConvertirAdao
    {
        public Usuario usuarioToDao(UsuarioDTO usuarioDTO);

        public List<Usuario> listUsuarioToDao(List<UsuarioDTO> listaUsuarioDTO);
    }
}
