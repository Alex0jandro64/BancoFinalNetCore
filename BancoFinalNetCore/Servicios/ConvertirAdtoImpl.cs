using BancoFinalNetCore.DTO;
using BancoFinalNetCore.Util;
using DAL.Entidades;
using System.Text;

namespace BancoFinalNetCore.Servicios
{
    public class ConvertirAdtoImpl : IConvertirAdto
    {
        public UsuarioDTO usuarioToDto(Usuario u)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método usuarioToDto() de la clase ConvertirAdtoImpl");

                UsuarioDTO dto = new UsuarioDTO();

                    dto.TlfUsuario = u.TlfUsuario;
                    dto.EmailUsuario = u.EmailUsuario;
                    dto.ClaveUsuario = u.ClaveUsuario;
                    dto.Token = u.Token;
                    dto.ExpiracionToken = u.ExpiracionToken;
                    dto.IdUsuario = u.IdUsuario;
                    dto.FchAltaUsuario = u.FchAltaUsuario;
                    dto.MailConfirmado = u.MailConfirmado;
                    dto.DniUsuario = u.DniUsuario;
                    dto.Rol = u.Rol;

                
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método usuarioToDto() de la clase ConvertirAdtoImpl");
                return dto;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR ConvertirAdtoImpl - usuarioToDto()] - Error al convertir usuario DAO a usuarioDTO (return null): {e}");
                return null;
            }
        }

        public List<UsuarioDTO> listaUsuarioToDto(List<Usuario> listaUsuario)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método listaUsuarioToDto() de la clase ConvertirAdtoImpl");

                List<UsuarioDTO> listaDto = new List<UsuarioDTO>();
                foreach (Usuario u in listaUsuario)
                {
                    listaDto.Add(usuarioToDto(u));
                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método listaUsuarioToDto() de la clase ConvertirAdtoImpl");
                return listaDto;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR ConvertirAdtoImpl - listaUsuarioToDto()] - Error al convertir lista de usuario DAO a lista de usuarioDTO (return null): {e}");
            }
            return null;
        }

    }
}
