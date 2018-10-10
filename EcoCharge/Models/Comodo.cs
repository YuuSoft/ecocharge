using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Table("comodo")]
    public class Comodo : AbstractEntity
    {

        public Comodo()
        {

        }

        [Required]
        [Column("nome")]
        [MaxLength(32)]
        public string Nome { get; set; }

        [Required]
        [Column("usuario_id")]
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
