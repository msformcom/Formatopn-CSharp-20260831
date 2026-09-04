using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace LibrodocusDAL
{
    public class LibrodocusContext : DbContext
    {
        // Les options (Chaine de connection, provider...) seront fournies par l'application grâce au constructeur
        // C'est DI qui construira
        public LibrodocusContext(DbContextOptions<LibrodocusContext> options) : base(options) 
        {
            
        }

        public DbSet<GenreDAL> Genres { get; set; }
        public DbSet<LivreDAL> Livres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ici, on modifie la structure de la BDD pour l'adapter 

            modelBuilder.Entity<LivreDAL>(options =>
            {
                options.HasKey(c => c.Id);
                options.HasIndex(c => c.ISBN).IsUnique(true);
                options.Property(c => c.ISBN).IsRequired().HasMaxLength(20);

                options.HasOne(l=>l.Genre).WithMany(g=>g.Livres)
                            .HasForeignKey(c=>c.IdGenre).OnDelete(DeleteBehavior.Restrict);
                // ToTable méthode du provider SQLServer
                // Pas ici => dans l'application que je vais donner ces informations
                // options.ToTable("TBL_Livres");
            });

            modelBuilder.Entity<GenreDAL>(options =>
            {
                options.HasKey(c => c.Id);
                options.HasIndex(c => c.Libelle).IsUnique(true);
                options.Property(c => c.Libelle).IsRequired();
            
            });
        }

        // LivreDAL l=new LivreDAL();
       // GenreDAL g = new GenreDAL();
        // l.Genre=g;
        // g.Livres.Add(new LivreDAL())
        // db.Livres.Add(l);
        // db.SaveChanges();

    }
}
