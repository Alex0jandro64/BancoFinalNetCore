using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entidades
{
    [Table(name: "cuentasBancarias", Schema = "bf_operacional")]
    public class CuentaBancaria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(name: "id_cuenta", TypeName = "bigint")]
        public long IdCuenta { get; set; }

        [ForeignKey("Usuario")]
        public long UsuarioCuentaId { get; set; }
        public Usuario UsuarioCuenta { get; set; }

        [Column(name: "saldo_cuenta", TypeName = "decimal(18,2)")]
        public decimal SaldoCuenta { get; set; }

        [Column(name: "codigo_iban_cuenta", TypeName = "varchar(100)")]
        public string CodigoIban { get; set; }

        [InverseProperty("UsuarioTransaccionRemitente")]
        public List<Transaccion> TrasaccionesRemitentes { get; set; }

        [InverseProperty("UsuarioTransaccionDestinatario")]
        public List<Transaccion> TrasaccionesDestinatarios { get; set; }
    }
}
