using BancoFinalNetCore.DTO;
using DAL.Entidades;
using System.Collections.Generic;

namespace BancoFinalNetCore.Servicios
{
    /// <summary>
    /// Interfaz que define los métodos para convertir DTO a entidades DAO.
    /// </summary>
    public interface IConvertirAdao
    {
        /// <summary>
        /// Convierte un objeto UsuarioDTO a un objeto Usuario.
        /// </summary>
        /// <param name="usuarioDTO">Objeto UsuarioDTO a convertir.</param>
        /// <returns>Objeto Usuario.</returns>
        public Usuario usuarioToDao(UsuarioDTO usuarioDTO);

        /// <summary>
        /// Convierte una lista de objetos UsuarioDTO a una lista de objetos Usuario.
        /// </summary>
        /// <param name="listaUsuarioDTO">Lista de objetos UsuarioDTO a convertir.</param>
        /// <returns>Lista de objetos Usuario.</returns>
        public List<Usuario> listUsuarioToDao(List<UsuarioDTO> listaUsuarioDTO);
    }
}