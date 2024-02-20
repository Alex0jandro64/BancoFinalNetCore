using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entidades
{
    [Table(name: "transacciones", Schema = "bf_operacional")]
    public class Transaccion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_transaccion", TypeName = "bigint")]
        public long IdTransaccion { get; set; }

        [ForeignKey("CuentaBancaria")]
        [Column("usuarioTransaccionRemitente", TypeName = "bigint")]
        public long UsuarioTransaccionRemitenteId { get; set; }
        public CuentaBancaria UsuarioTransaccionRemitente { get; set; }

        [ForeignKey("CuentaBancaria")]
        [Column("usuarioTransaccionDestinatario", TypeName = "bigint")]
        public long UsuarioTransaccionDestinatarioId { get; set; }
        public CuentaBancaria UsuarioTransaccionDestinatario { get; set; }

        [Column("cantidad_transaccion", TypeName = "decimal(18,2)")]
        public decimal CantidadTransaccion { get; set; }

        [Column("fecha_transaccion", TypeName = "timestamp")]
        public DateTime FechaTransaccion { get; set; }
    }
}
