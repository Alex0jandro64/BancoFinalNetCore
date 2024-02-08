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
    [Table(name: "oficinas", Schema = "bf_operacional")]
    public class Oficina
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(name: "id_oficina", TypeName = "bigint")]
        public long IdOficina { get; set; }

        [InverseProperty("OficinaCita")]
        public List<Cita> CitasOficina { get; set; }

        [Required]
        [Column(name : "direccion_oficina", TypeName = "varchar(100)")]
        public string DireccionOficina { get; set; }
    }
}
