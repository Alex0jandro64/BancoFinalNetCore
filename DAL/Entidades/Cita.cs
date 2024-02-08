using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DAL.Entidades
{
    [Table(name: "citas", Schema = "bf_operacional")]
    public class Cita
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(name: "id_cita", TypeName = "bigint")]
        public long IdCita { get; set; }

        [ForeignKey("Usuario")]
        [Column(name: "id_usuario", TypeName = "bigint")]
        public long UsuarioCitaId { get; set; }
        public Usuario UsuarioCita { get; set; }

        [ForeignKey("Oficina")]
        [Column(name: "id_oficina", TypeName = "bigint")]
        public long OficinaCitaId { get; set; }
        public Oficina OficinaCita { get; set; }

        [Column(name : "fecha_cita", TypeName = "timestamp")]
        public DateTime FechaCita { get; set; }

        [Column(name : "motivo_cita", TypeName = "varchar(255)")]
        public string MotivoCita { get; set; }
    }
}
