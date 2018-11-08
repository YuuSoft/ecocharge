using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{

    [Table("usuario")]
    public class Usuario : AbstractEntity
    {

        public Usuario()
        {

        }

        [Column("nome")]
        [MaxLength(60)]
        public string Nome { get; set; }

        [Column("sobrenome")]
        [MaxLength(60)]
        public string Sobrenome { get; set; }

        [Required]
        [Column("email")]
        [MaxLength(50)]
        public string Email { get; set; }

        [Required]
        [Column("senha")]
        [MaxLength(24)]
        public string Senha { get; set; }
        
        [Column("google_id")]
        [MaxLength(21)]
        public string GoogleId { get; set; }

        [Column("facebook_id")]
        [MaxLength(20)]
        public string FacebookId { get; set; }

        [Column("tarifa")]
        public decimal Tarifa { get; set; }

        [Column("telefone")]
        [MaxLength(24)]
        public string Telefone { get; set; }

        [Column("genero")]
        [MaxLength(20)]
        public string Genero { get; set; }

        [Column("data_de_nascimento ")]
        public DateTime DataNascimento { get; set; }

        [Column("localizacao")]
        [MaxLength(60)]
        public string Localizacao { get; set; }

        [Column("foto")]
        public string Foto { get; set; }

        [Column("qtde_pessoas_na_casa")]
        public int QtdePessoasCasa { get; set; }

        [Column("qual_comodo_mais_gasta")]
        public string Qualcomodomaisgasta { get; set; }

        [Column("valor_da_conta_de_luz")]
        public decimal ValorContaLuz { get; set; }

        [InverseProperty("Usuario")]
        public virtual IList<Agendamento> Agendamentos { get; set; }

        [InverseProperty("Usuario")]
        public virtual IList<Comodo> Comodos { get; set; }

        [InverseProperty("Usuario")]
        public virtual IList<EcoSense> EcoSenses { get; set; }
        public int UsuarioId { get; internal set; }
    }
}
