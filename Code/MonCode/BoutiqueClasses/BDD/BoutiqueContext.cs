using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Boutique.BDD
{
    // BoutiqueContext va faire l'accès à la BDD pour moi
    // Package Microsoft.EntityFrameworkCore
    // DbContext qui contient la mécanique d'accès à la BDD
    public class BoutiqueContext : DbContext
    {
        // Ce constructeur sera utilisé par DI
        // Afin de passer les options => Provider + Chaine de connexion
        // Ces options sont passées au constructeur de la classe mère (base) => dbContext
        public BoutiqueContext(DbContextOptions<BoutiqueContext> options) : base(options) 
        {
            
        }
        // Stockage des produits dans une table de la BDD
        public DbSet<ProduitBDD> Produits { get; set; }


        // Exécutée pour spécifier les attributs spécifique à la BDD
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Je suis dans du code qui s'exécute (par opposition aux attributs)
            // Options pour la classe ProduitBDD => Colonne associée
            modelBuilder.Entity<ProduitBDD>(options =>
            {
                options.HasKey(c => c.Id).IsClustered(false);
                options.Property(c => c.Nom).HasMaxLength(50).HasColumnName("Name");
                options.HasIndex(c => new {  c.IdCatalogue, c.Nom })
                                .IsClustered(true)
                                .IsUnique(true);
                options.ToTable("TBL_Produits");
            });
        }
    }
}
