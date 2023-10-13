using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Pizzeria.Models
{
    public partial class ModelDBContext : DbContext
    {
        public ModelDBContext()
            : base("name=ModelDBContext")
        {
        }

        public virtual DbSet<DettagliOrdine> DettagliOrdine { get; set; }
        public virtual DbSet<Ordini> Ordini { get; set; }
        public virtual DbSet<Prodotti> Prodotti { get; set; }
        public virtual DbSet<Utenti> Utenti { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DettagliOrdine>()
                .Property(e => e.PrezzoTotale)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Ordini>()
                .HasMany(e => e.DettagliOrdine)
                .WithOptional(e => e.Ordini)
                .HasForeignKey(e => e.ID_Ordine);

            modelBuilder.Entity<Prodotti>()
                .Property(e => e.Prezzo)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Prodotti>()
                .HasMany(e => e.DettagliOrdine)
                .WithOptional(e => e.Prodotti)
                .HasForeignKey(e => e.ID_Prodotto);

            modelBuilder.Entity<Utenti>()
                .HasMany(e => e.Ordini)
                .WithOptional(e => e.Utenti)
                .HasForeignKey(e => e.ID_Utente);
        }
    }
}
