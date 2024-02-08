using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entidades
{
    [Table(name: "usuarios", Schema = "bf_operacional_usu")]
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(name: "id_usuario", TypeName = "bigint")]
        public long IdUsuario { get; set; }

        [Required]
        [Column(name: "dni_usuario", TypeName = "varchar(9)")]
        public string DniUsuario { get; set; }

        [Required]
        [Column(name: "nombre_usuario", TypeName = "varchar(70)")]
        public string NombreUsuario { get; set; }

        [Column(name: "apellidos_usuario", TypeName = "varchar(100)")]
        public string ApellidosUsuario { get; set; }

        [Column(name: "tlf_usuario", TypeName = "varchar(9)")]
        public string TlfUsuario { get; set; }

        [Required]
        [Column(name: "email_usuario", TypeName = "varchar(100)")]
        public string EmailUsuario { get; set; }

        [Required]
        [Column(name: "clave_usuario", TypeName = "varchar(100)")]
        public string ClaveUsuario { get; set; }

        [Column(name: "fch_alta_usuario", TypeName = "timestamp")]
        public DateTime? FchAltaUsuario { get; set; }

        [Column(name: "fch_baja_usuario", TypeName = "timestamp")]
        public DateTime? FchBajaUsuario { get; set; }

        [Column(name: "tokenRecuperacion_usuario", TypeName = "varchar(100)")]
        public string? Token { get; set; }

        [Column(name: "expiracionToken_usuario", TypeName = "timestamp")]
        public DateTime? ExpiracionToken { get; set; }

        [Column(name: "rol_usuario", TypeName = "varchar(20)")]
        public string? Rol { get; set; }

        [Column(name: "mail_confirmado_usuario", TypeName = "boolean")]
        public bool MailConfirmado { get; set; } = false;

        // Agregar un campo para la imagen de perfil
        [Column(name: "foto_perfil_usuario", TypeName = "bytea")]
        public byte[]? FotoPerfil { get; set; }

        [InverseProperty("UsuarioCuenta")]
        public List<CuentaBancaria> CuentasBancarias { get; set; }

        [InverseProperty("UsuarioCita")]
        public List<Cita> CitasUsuario { get; set; }

        public Usuario(long idUsuario, string dniUsuario, string nombreUsuario, string apellidosUsuario, string tlfUsuario, string emailUsuario, string claveUsuario, DateTime? fchAltaUsuario, DateTime? fchBajaUsuario, string token, DateTime? expiracionToken, string rol, bool mailConfirmado, byte[] fotoPerfil, List<CuentaBancaria> cuentasBancarias, List<Cita> citasUsuario)
        {
            IdUsuario = idUsuario;
            DniUsuario = dniUsuario;
            NombreUsuario = nombreUsuario;
            ApellidosUsuario = apellidosUsuario;
            TlfUsuario = tlfUsuario;
            EmailUsuario = emailUsuario;
            ClaveUsuario = claveUsuario;
            FchAltaUsuario = fchAltaUsuario;
            FchBajaUsuario = fchBajaUsuario;
            Token = token;
            ExpiracionToken = expiracionToken;
            Rol = rol;
            MailConfirmado = mailConfirmado;
            FotoPerfil = fotoPerfil;
            CuentasBancarias = cuentasBancarias;
            CitasUsuario = citasUsuario;
        }

        public Usuario()
        {
        }


    }


}
