using BancoFinalNetCore.DTO;
using BancoFinalNetCore.Util;
using DAL.Entidades;
using System;
using System.Collections.Generic;

namespace BancoFinalNetCore.Servicios
{
    /// <summary>
    /// Implementación de la interfaz IConvertirAdao para convertir DTOs a entidades DAO.
    /// </summary>
    public class ConvertirAdaoImpl : IConvertirAdao
    {
        /// <summary>
        /// Convierte un objeto de tipo UsuarioDTO a un objeto Usuario.
        /// </summary>
        /// <param name="usuarioDTO">Objeto de tipo UsuarioDTO.</param>
        /// <returns>Objeto Usuario.</returns>
        public Usuario usuarioToDao(UsuarioDTO usuarioDTO)
        {
            // Crear un nuevo objeto de tipo Usuario
            Usuario usuarioDao = new Usuario();

            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método usuarioToDao() de la clase ConvertirAdaoImpl");

                // Asignar los valores del UsuarioDTO al Usuario correspondiente
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
                // En caso de error, registrar el error y devolver null
                EscribirLog.escribirEnFicheroLog($"\n[ERROR ConvertirAdaoImpl - UsuarioToDao()] - Al convertir usuarioDTO a usuarioDAO (return null): {e}");
                return null;
            }
        }

        /// <summary>
        /// Convierte una lista de objetos UsuarioDTO a una lista de objetos Usuario.
        /// </summary>
        /// <param name="listaUsuarioDTO">Lista de objetos UsuarioDTO.</param>
        /// <returns>Lista de objetos Usuario.</returns>
        public List<Usuario> listUsuarioToDao(List<UsuarioDTO> listaUsuarioDTO)
        {
            EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método listUsuarioToDao() de la clase ConvertirAdaoImpl");

            // Crear una lista para almacenar los objetos Usuario convertidos
            List<Usuario> listaUsuarioDao = new List<Usuario>();

            try
            {
                // Iterar sobre cada UsuarioDTO en la lista y convertirlo a Usuario
                foreach (UsuarioDTO usuarioDTO in listaUsuarioDTO)
                {
                    listaUsuarioDao.Add(usuarioToDao(usuarioDTO));
                }

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método listUsuarioToDao() de la clase ConvertirAdaoImpl");

                return listaUsuarioDao;
            }
            catch (Exception e)
            {
                // En caso de error, registrar el error y devolver null
                EscribirLog.escribirEnFicheroLog($"\n[ERROR ConvertirAdaoImpl - listUsuarioToDao()] - Al convertir lista de usuarioDTO a lista de usuarioDAO (return null): {e}");
                return null;
            }
        }
    }
}