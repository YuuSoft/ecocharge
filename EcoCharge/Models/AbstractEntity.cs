using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public abstract class AbstractEntity
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("data_criacao")]
        public DateTime DataCriacao { get; set; }

        [Column("data_atualizacao")]
        public DateTime DataAtualização { get; set; }

        public AbstractEntity()
        {
            this.Id = 0;
        }

        public virtual void PrepareSave()
        {
            if (this.Id == 0)
            {
                this.DataCriacao = DateTime.Now;
                this.DataAtualização = DateTime.Now;
            }
            else
            {
                this.DataAtualização = DateTime.Now;
            }

        }

    }
}
