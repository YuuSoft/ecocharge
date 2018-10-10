﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Table("serial_aparelho")]
    public class SerialAparelho : AbstractEntity
    {

        public SerialAparelho()
        {

        }

        [Column("serial")]
        [MaxLength(12)]
        public string Serial { get; set; }

        [InverseProperty("SerialAparelho")]
        public virtual IList<EcoSense> EcoSenses { get; set; }
    }
}