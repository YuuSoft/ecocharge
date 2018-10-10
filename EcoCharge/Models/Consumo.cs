using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Table("consumo")]
    public class Consumo : AbstractEntity
    {
        [Column("serial")]
        [MaxLength(12)]
        public string Serial { get; set; }

        [Column("potencia_watts")]
        public decimal PotenciaWatts { get; set; }

        [Column("quilowatts")]
        public decimal Quilowatts { get; set; }

        [Column("ciclos")]
        public int Ciclos { get; set; }

        [ForeignKey("Aparelho")]
        [Column("aparelho_id")]
        public int AparelhoId { get; set; }
        public virtual Aparelho Aparelho { get; set; }
    }
}
