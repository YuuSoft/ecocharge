﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Table("aparelho")]
    public class Aparelho : AbstractEntity
    {

        public Aparelho()
        {

        }

        [Required]
        [Column("nome")]
        public String Nome { get; set; }

        [Column("descricao")]
        public String Descricao { get; set; }

        [Required]
        [Column("voltagem")]
        public int Voltagem { get; set; }

        [Required]
        [Column("cor")]
        [MaxLength(7)]
        public string Cor { get; set; }

        [Column("ativo")]
        public bool Ativo { get; set; }

        [Required]
        [Column("usuario_id")]
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }

        [Required]
        [ForeignKey("EcoSense")]
        [Column("ecosense_id")]
        public int EcoSenseId { get; set; }
        public virtual EcoSense EcoSense { get; set; }

        [Required]
        [ForeignKey("Comodo")]
        [Column("comodo_id")]
        public int ComodoId { get; set; }
        public virtual Comodo Comodo { get; set; }

        [InverseProperty("Aparelho")]
        public virtual IList<Consumo> Consumos { get; set; }
    }
}
