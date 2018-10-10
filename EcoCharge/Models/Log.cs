using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("log")]
    public class Log : AbstractEntity
    {
        [Column("acao")]
        [MaxLength(30)]
        public string Acao { get; set; }

        [Column("controlador")]
        [MaxLength(30)]
        public string Controlador { get; set; }

        [Column("email")]
        [MaxLength(50)]
        public string Email { get; set; }

        [Column("ip")]
        [MaxLength(20)]
        public string Ip { get; set; }

        //[Column("usuario_id")]
        //[ForeignKey("Usuario")]
        //public int UsuarioId { get; set; }
        //public virtual Usuario Usuario { get; set; }
    }
}