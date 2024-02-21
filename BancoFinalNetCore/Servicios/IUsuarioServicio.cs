using BancoFinalNetCore.DTO;
using DAL.Entidades;
using System.Collections.Generic;

namespace BancoFinalNetCore.Servicios
{
    /// <summary>
    /// Interfaz que define las operaciones del servicio de usuario.
    /// </summary>
    public interface IUsuarioServicio
    {
        /// <summary>
        /// Registra un nuevo usuario.
        /// </summary>
        /// <param name="userDTO">DTO del usuario a registrar.</param>
        /// <returns>DTO del usuario registrado.</returns>
        public UsuarioDTO registrarUsuario(UsuarioDTO userDTO);

        /// <summary>
        /// Confirma una cuenta de usuario a través de un token.
        /// </summary>
        /// <param name="token">Token de confirmación.</param>
        /// <returns>true si la cuenta se confirma correctamente, false en caso contrario.</returns>
        public bool confirmarCuenta(string token);

        /// <summary>
        /// Inicia el proceso de recuperación de la contraseña para un usuario.
        /// </summary>
        /// <param name="emailUsuario">Correo electrónico del usuario.</param>
        /// <returns>true si el proceso se inicia correctamente, false en caso contrario.</returns>
        public bool iniciarProcesoRecuperacion(string emailUsuario);

        /// <summary>
        /// Obtiene un usuario por su token de confirmación.
        /// </summary>
        /// <param name="token">Token de confirmación.</param>
        /// <returns>DTO del usuario encontrado.</returns>
        public UsuarioDTO obtenerUsuarioPorToken(string token);

        /// <summary>
        /// Modifica la contraseña de un usuario utilizando un token.
        /// </summary>
        /// <param name="usuario">DTO del usuario con la nueva contraseña.</param>
        /// <returns>true si la contraseña se modifica correctamente, false en caso contrario.</returns>
        public bool modificarContraseñaConToken(UsuarioDTO usuario);

        /// <summary>
        /// Verifica las credenciales de un usuario.
        /// </summary>
        /// <param name="emailUsuario">Correo electrónico del usuario.</param>
        /// <param name="claveUsuario">Contraseña del usuario.</param>
        /// <returns>true si las credenciales son válidas, false en caso contrario.</returns>
        bool verificarCredenciales(string emailUsuario, string claveUsuario);

        /// <summary>
        /// Obtiene un usuario por su correo electrónico.
        /// </summary>
        /// <param name="email">Correo electrónico del usuario.</param>
        /// <returns>DTO del usuario encontrado.</returns>
        public UsuarioDTO obtenerUsuarioPorEmail(string email);

        /// <summary>
        /// Obtiene todos los usuarios.
        /// </summary>
        /// <returns>Lista de todos los usuarios en formato DTO.</returns>
        public List<UsuarioDTO> obtenerTodosLosUsuarios();

        /// <summary>
        /// Busca un usuario por su ID.
        /// </summary>
        /// <param name="id">ID del usuario.</param>
        /// <returns>DTO del usuario encontrado.</returns>
        public UsuarioDTO buscarPorId(long id);

        /// <summary>
        /// Elimina un usuario por su ID.
        /// </summary>
        /// <param name="id">ID del usuario a eliminar.</param>
        public void eliminar(long id);

        /// <summary>
        /// Actualiza un usuario.
        /// </summary>
        /// <param name="usuarioModificado">DTO del usuario actualizado.</param>
        public void actualizarUsuario(UsuarioDTO usuarioModificado);

        /// <summary>
        /// Cuenta el número de usuarios con un rol específico.
        /// </summary>
        /// <param name="rol">Rol de los usuarios a contar.</param>
        /// <returns>Número de usuarios con el rol especificado.</returns>
        public int contarUsuariosPorRol(string rol);

        /// <summary>
        /// Busca usuarios por coincidencia en su correo electrónico.
        /// </summary>
        /// <param name="palabra">Palabra a buscar en el correo electrónico de los usuarios.</param>
        /// <returns>Lista de usuarios que coinciden con la palabra especificada.</returns>
        public List<UsuarioDTO> buscarPorCoincidenciaEnEmail(string palabra);

        /// <summary>
        /// Otorga un rol específico a un usuario.
        /// </summary>
        /// <param name="usuarioDto">DTO del usuario al que se le otorgará el rol.</param>
        public void darRolUsuario(UsuarioDTO usuarioDto);
    }
}