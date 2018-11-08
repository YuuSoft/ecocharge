using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Table("agendamento")]
    public class Agendamento : AbstractEntity
    {

        public Agendamento() 
        {

        }

        [MaxLength(300)]
        [Column("detalhes")]
        public string Detalhes { get; set; }

        [Column("usuario_id")]
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }

        [Required]
        [Column("ecosense_id")]
        [ForeignKey("EcoSense")]
        public int EcoSenseId { get; set; }
        public virtual EcoSense EcoSense { get; set; }

        public override void PrepareSave() {
            base.PrepareSave();

            //Json tem uma array de dias 0 a 6 (7 dias da semana), e cada dia podendo ter um array infinito de horarios.
            if (this.Id == 0) {
                this.Detalhes = "{" +
                    " 'dia' : [" +
                        "{ " +
                            "'horario' : []" + // { "inicio": "08:00", "fim" : "12:00" }, { "inicio": "14:00", "fim" : "18:00" }
                        "}," +
                        "{ " +
                            "'horario' : []" +
                        "}," +
                        "{ " +
                            "'horario' : []" +
                        "}," +
                        "{ " +
                            "'horario' : []" +
                        "}," +
                        "{ " +
                            "'horario' : []" +
                        "}," +
                        "{ " +
                            "'horario' : []" +
                        "}," +
                        "{ " +
                            "'horario' : []" +
                        "}" +
                    "]" +
                "}";
            }
        }
    }
}
