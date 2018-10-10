using Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DAL.DB
{
    public class AccessDB : DbContext
    {
        public AccessDB() : base("name=AccessDB")
        {

        }

        public virtual DbSet<Agendamento> Agendamento { get; set; }

        public virtual DbSet<Aparelho> Aparelho { get; set; }

        public virtual DbSet<Comodo> Comodo { get; set; }

        public virtual DbSet<Consumo> Consumo { get; set; }

        public virtual DbSet<EcoSense> EcoSense { get; set; }

        public virtual DbSet<SerialAparelho> SerialAparelho { get; set; }

        public virtual DbSet<Usuario> Usuario { get; set; }

        public virtual DbSet<Log> Log { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);
        }
    }
}