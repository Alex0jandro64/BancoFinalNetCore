using DAL.Entidades;

namespace BancoFinalNetCore.DTO
{
    public class UsuarioDTO
    {
        
        public long IdUsuario { get; set; }
        public string DniUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string ApellidosUsuario { get; set; }
        public string TlfUsuario { get; set; }
        public string EmailUsuario { get; set; }
        public string ClaveUsuario { get; set; }
        public DateTime? FchAltaUsuario { get; set; }
        public DateTime? FchBajaUsuario { get; set; }
        public string Token { get; set; }
        public DateTime? ExpiracionToken { get; set; }
        public string Rol { get; set; }
        public bool? MailConfirmado { get; set; }
        public byte[]? FotoPerfil { get; set; }
        public List<CuentaBancaria> CuentasBancarias { get; set; }
        public List<Cita> CitasUsuario { get; set; }
    }
}
