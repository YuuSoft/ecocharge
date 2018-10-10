using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Table("ecosense")]
    public class EcoSense : AbstractEntity
    {

        public EcoSense()
        {

        }

        [Column("serial_id")]
        [ForeignKey("SerialAparelho")]
        public int SerialId { get; set; }
        public virtual SerialAparelho SerialAparelho { get; set; }

        [Column("usuario_id")]
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }

        [Column("status_aparelho")]
        public bool StatusAparelho { get; set; }

        [InverseProperty("EcoSense")]
        public virtual IList<Agendamento> Agendamentos { get; set; }
    }
}
