using BancoFinalNetCore.DTO;
using BancoFinalNetCore.Util;
using DAL.Entidades;

namespace BancoFinalNetCore.Servicios
{
    public class ConvertirAdaoImpl : IConvertirAdao
    {
        public Usuario usuarioToDao(UsuarioDTO usuarioDTO)
        {

            Usuario usuarioDao = new Usuario();

            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método usuarioToDao() de la clase ConvertirAdaoImpl");

                usuarioDao.IdUsuario = usuarioDTO.IdUsuario;
                usuarioDao.NombreUsuario = usuarioDTO.NombreUsuario;
                usuarioDao.ApellidosUsuario = usuarioDTO.ApellidosUsuario;
                usuarioDao.EmailUsuario = usuarioDTO.EmailUsuario;
                usuarioDao.ClaveUsuario = usuarioDTO.ClaveUsuario;
                usuarioDao.TlfUsuario = usuarioDTO.TlfUsuario;
                usuarioDao.DniUsuario = usuarioDTO.DniUsuario;
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método usuarioToDao() de la clase ConvertirAdaoImpl");

                return usuarioDao;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"\n[ERROR ConvertirAdaoImpl - UsuarioToDao()] - Al convertir usuarioDTO a usuarioDAO (return null): {e}");
                return null;
            }
        }

        public List<Usuario> listUsuarioToDao(List<UsuarioDTO> listaUsuarioDTO)
        {
            EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método listUsuarioToDao() de la clase ConvertirAdaoImpl");

            List<Usuario> listaUsuarioDao = new List<Usuario>();

            try
            {
                foreach (UsuarioDTO usuarioDTO in listaUsuarioDTO)
                {
                    listaUsuarioDao.Add(usuarioToDao(usuarioDTO));
                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método listUsuarioToDao() de la clase ConvertirAdaoImpl");

                return listaUsuarioDao;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"\n[ERROR ConvertirAdaoImpl - listUsuarioToDao()] - Al convertir lista de usuarioDTO a lista de usuarioDAO (return null): {e}");
            }
            return null;
        }
    }
}
