using DAL.Entidades; // Importación del espacio de nombres que contiene la clase CuentaBancaria

namespace BancoFinalNetCore.DTO
{
    /// <summary>
    /// Clase que representa un objeto de transferencia de datos (DTO) para un usuario.
    /// </summary>
    public class UsuarioDTO
    {
        /// <summary>
        /// ID del usuario.
        /// </summary>
        public long IdUsuario { get; set; }

        /// <summary>
        /// DNI del usuario.
        /// </summary>
        public string DniUsuario { get; set; }

        /// <summary>
        /// Nombre del usuario.
        /// </summary>
        public string NombreUsuario { get; set; }

        /// <summary>
        /// Apellidos del usuario.
        /// </summary>
        public string ApellidosUsuario { get; set; }

        /// <summary>
        /// Teléfono del usuario.
        /// </summary>
        public string TlfUsuario { get; set; }

        /// <summary>
        /// Email del usuario.
        /// </summary>
        public string EmailUsuario { get; set; }

        /// <summary>
        /// Clave del usuario.
        /// </summary>
        public string ClaveUsuario { get; set; }

        /// <summary>
        /// Fecha de alta del usuario.
        /// </summary>
        public DateTime? FchAltaUsuario { get; set; }

        /// <summary>
        /// Fecha de baja del usuario.
        /// </summary>
        public DateTime? FchBajaUsuario { get; set; }

        /// <summary>
        /// Token del usuario.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Fecha de expiración del token del usuario.
        /// </summary>
        public DateTime? ExpiracionToken { get; set; }

        /// <summary>
        /// Rol del usuario.
        /// </summary>
        public string Rol { get; set; }

        /// <summary>
        /// Indica si el email del usuario ha sido confirmado.
        /// </summary>
        public bool? MailConfirmado { get; set; }

        /// <summary>
        /// Foto de perfil del usuario.
        /// </summary>
        public byte[]? FotoPerfil { get; set; }

        /// <summary>
        /// Lista de cuentas bancarias asociadas al usuario.
        /// </summary>
        public List<CuentaBancaria> CuentasBancarias { get; set; }

        /// <summary>
        /// Lista de citas asociadas al usuario.
        /// </summary>
        public List<CitaDTO> CitasUsuario { get; set; }
    }
}