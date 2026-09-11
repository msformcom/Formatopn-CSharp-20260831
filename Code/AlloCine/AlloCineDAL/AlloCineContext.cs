using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AlloCineDAL
{
    // Context qui hérite DbContext
    // représentaion de la BDD 
    public class AlloCineContext : DbContext
    {
        private readonly Action<ModelBuilder>? continueModelBuilding;


        // Constructeur qui demande les options de connexion à la BDD
        // et les passe au DbContext
        public AlloCineContext(DbContextOptions<AlloCineContext> options,
            ILogger<AlloCineContext> logger,
            IConfiguration config,
            // Cette méthode sera utilisée pour finir la config de la BDD dans OnModelCreating
            Action<ModelBuilder>? continueModelBuilding =null
            ) : base(options)
        {
            this.continueModelBuilding = continueModelBuilding;
            // Lecture dans la config
            var valeurInscriteDansLaConfig=config.GetSection("Nom utilisateur").Value;
            // Journalisation de la construction
            logger.LogInformation("Création d'un context Allocine");// => Dans un fichier disk
         

        }

        public DbSet<CinemaDAO> Cinemas { get; set; }
        public DbSet<SeanceDAO> Seances { get; set; }
        public DbSet<FilmDAO> Films { get; set; }
        public DbSet<CategorieDAO> Categories { get; set; }

        void AjouteEtooile()
        {
            //this.Database.ExecuteSql($"UPDATE Personnes SET Actif = 1 WHERE Age >= {age}");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Je peux spécifier des contraintes supplémentaire sur les données / structure
            // fluentapi
            // gestion des options de BDD pour FilmDAO
            modelBuilder.Entity<FilmDAO>(options =>
            {
                // Clé primaire
                options.HasKey(c=>c.Id);

                // Attention à la concurrence sur cette propriété
                options.Property(c => c.Title).IsConcurrencyToken();
                // Index sur le code
                options.HasIndex(c => c.Code).IsUnique(true);
                // Lonhueur de la chaine du code
                options.Property(c => c.Code).HasMaxLength(10).IsRequired(true);
                options.Property(c => c.Title).HasMaxLength(100).IsRequired(true);
                options.Property(c => c.IdCategorie).IsRequired(true);
                // 1 film a plusieurs seances avec pour chacune un seul film associé en utilisant la clé etrangere IdFilm
                options.HasMany(c => c.Seances).WithOne(c => c.Film).HasForeignKey(c => c.IdFilm)
                            // Avec supression en cascade
                            .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<CinemaDAO>(options =>
            {
                // Clé primaire
                options.HasKey(c => c.Id);
                // Index sur le code
                options.HasIndex(c => c.Code).IsUnique(true);
                // Lonhueur de la chaine du code
                options.Property(c => c.Code).HasMaxLength(10).IsRequired(true);
                options.Property(c => c.Name).HasMaxLength(100).IsRequired(true);

      

            });

            modelBuilder.Entity<SeanceDAO>(options =>
            {
                // Clé primaire
                // options.ToTable("TBL_Seances");
              
                options.HasKey(c => c.Id); //.HasColumnName("PK_Seance");
                // ou clé composite
                // options.HasKey(c => new { c.IdCinema, c.IdFilm });
                // Index sur le code
                options.HasIndex(c => c.Code).IsUnique(true);
                // Lonhueur de la chaine du code
                options.Property(c => c.Code).HasMaxLength(10).IsRequired(true);
                // CHAR(10)         DOM_______  10 octetd      Encodage ASCII => Table de code par pays 0-127 caractère alpha normaux  127 et > => special pays é => FR => 220  sur un PC danois 220 => ô
                // NCHAR(10)        N unicode => 1 car => 1 octet (alpha latin) , 2, 3, 4 octets suivants les alphabet
                // VARCHAR(10)      DOM$$      5 octet
                // NVARCHAR(10)
               
                options.Property(c => c.IdFilm).IsRequired(true);

                options.HasOne(c => c.Cinema).WithMany(c => c.Seances).HasForeignKey(c => c.IdCinema)
                                .IsRequired(true).OnDelete(DeleteBehavior.Cascade);
          

            });

            modelBuilder.Entity<CategorieDAO>(options =>
            {
                options.HasKey(c => c.Id); 
                options.HasIndex(c => c.Code).IsUnique(true);
                options.Property(c => c.Code).HasMaxLength(10).IsRequired(true);
                options.HasMany(c => c.Films).WithOne(c => c.Categorie).HasForeignKey(c => c.IdCategorie).OnDelete(DeleteBehavior.Restrict);

            });

            // Que demander à DI pour obtenir une instance de cette fonction
            // Action<ModelBuilder> => type (delegate) de la fonction dont j'ai besoin
            if (this.continueModelBuilding != null)
            {
                this.continueModelBuilding(modelBuilder);
            }
   
            //{
            //    modelBuilder....
            //}



        }
    }
}
